using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helper;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Authorize(Policy = "Bearer")]
        [HttpGet("Products")]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetAllProducts([FromQuery]ProductSpecParams productSpecParams)
        {
            var spec = new ProductSpecification(productSpecParams);

            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            if (products is null)
                return NotFound(new ApiErrorResponse(404));

            var data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products);

            var CountSpec = new ProductWithFiltarationForCountSpecification(productSpecParams);
            var Count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);

            return Ok(new Pagination<ProductToReturnDto>(productSpecParams.PageIndex, productSpecParams.PageSize, Count, data));
        }



        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("Product/{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);

            if (product is null)
                return NotFound(new ApiErrorResponse(404));

            var mappedProduct = _mapper.Map<Product, ProductToReturnDto>(product);

            return Ok(mappedProduct);
        }



        [HttpGet("Brands")]   //api/Products/Brands
        public async Task<ActionResult<ProductBrand>> GetAllBrands()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }


        [HttpGet("Types")]   //api/Products/Types
        public async Task<ActionResult<ProductBrand>> GetAllTypes()
        {
            var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }

    }
}
