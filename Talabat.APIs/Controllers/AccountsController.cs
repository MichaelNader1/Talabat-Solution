using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.ServiceInterfaces;

namespace Talabat.APIs.Controllers
{
    public class AccountsController: BaseApiController
    {
        public readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExist(model.Email).Result.Value)
            {
                return BadRequest(new ApiResponse(400, "Email already Exist."));
            }
            var user = new AppUser
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
             
                
                return BadRequest(new ApiResponse(400));
            }
            else
            {
                var returnedUser = new UserDto()
                {
                    DisplayName = model.DisplayName,
                    Email = model.Email,
                    Token = await _tokenService.CreateTokenAsync(user,_userManager)
                };
            return Ok(returnedUser);


            }
        }

        [HttpPost("Login")]

        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user= await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);
            if(!result.Succeeded)
                return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)

            });
        }

        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> CheckEmailExist(string Email)
        {
            return await _userManager.FindByEmailAsync(Email) is not null;
        }

        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var UserEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(UserEmail);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }
        [Authorize]
        [HttpGet("GetCurrentUserAddress")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<Adress,AddressDto>(user.Adress);
            return Ok(MappedAddress);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress( AddressDto model)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var address = _mapper.Map<AddressDto, Adress>(model);
            user.Adress = address;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));    
            return Ok(model);
        }



    }
}
