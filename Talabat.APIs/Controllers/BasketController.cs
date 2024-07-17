using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.RepositoryInterfaces;

namespace Talabat.APIs.Controllers
{
    public class BasketController:BaseApiController
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;
        public BasketController(IBasketRepository BasketRepo, IMapper Mapper)
        {
            _basketRepo = BasketRepo;
            _mapper = Mapper;

        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var Basket = await _basketRepo.GetBasketAsync(id);
            return Ok(Basket??new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto model)
        {
            var mappedBasket = _mapper.Map<CustomerBasket>(model);
            var CreateOrUpdateBasket = await _basketRepo.UpdateBasketAsync(mappedBasket);
            if (CreateOrUpdateBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(CreateOrUpdateBasket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepo.DeleteBasketAsync(id);    
        }

    }
}
