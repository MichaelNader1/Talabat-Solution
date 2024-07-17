using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Order;
using Talabat.Core.ServiceInterfaces;
using Talabat.Repository;

namespace Talabat.APIs.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IOrderInterface _orderInterface;
        private readonly UnitOfWork _unitOfWork;

        public OrderController(IMapper mapper, IOrderInterface IOrderInreface, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _orderInterface = IOrderInreface;
            _unitOfWork = unitOfWork;
        }
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto model)
        {
            var BuyerEmail= User.FindFirstValue(ClaimTypes.Email);
            var Address = _mapper.Map<AddressDto, Address>(model.ShippingAddress);
            var Order = await _orderInterface.CreateOrderAsync(BuyerEmail, model.BasketId, model.DeliveryMethodId, Address);
            if(Order == null) return BadRequest(new ApiResponse(404, "Order has Failed!"));
            var result=_mapper.Map<Core.Order.Order, OrderToReturnDto>(Order);
            return Ok(result );
        }

        [ProducesResponseType(typeof(IReadOnlyList < OrderToReturnDto >), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrderForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _orderInterface.GetOrderForSpecificUserAsync(BuyerEmail);
            if (Order == null) return BadRequest(new ApiResponse(404, "No Orders!"));
            return Ok(_mapper.Map< IReadOnlyList<OrderToReturnDto>>(Order));
        }

        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _orderInterface.GetOrderByIdForSpecificUserAsync(BuyerEmail,id);
            if (Order == null) return BadRequest(new ApiResponse(404, "No Orders With This Id!"));
            return Ok(_mapper.Map<OrderToReturnDto>(Order));
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var DeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return Ok(DeliveryMethods);
        }



    }

}
