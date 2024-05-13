using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;

namespace Talabat.APIs.Controllers
{
    public class BasketsController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketsController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            return basket == null ? new CustomerBasket(id) : basket;
        }


        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateCustomerBasket (CustomerBasketDto basket)
        {
            var basketMapped = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket); 

            var createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(basketMapped);

            if (createdOrUpdatedBasket is null)
                return BadRequest(new ApiErrorResponse(400));

            return Ok(createdOrUpdatedBasket);
        }


        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            var basket = await _basketRepository.DeleteBasketAsync(id);

            return basket;
        }

    }
}
