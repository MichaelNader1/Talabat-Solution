using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helper;
using Talabat.APIs.Specifications.ProductsSpecs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.RepositoryInterfaces;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenaricRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenaricRepository<Product> ProductRepo,IMapper mapper ) {
            _productRepo = ProductRepo;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] ProductsSpecParams productsSpec)
        {
            var spec = new ProductsWithBrandsAndCategorySpec(productsSpec);
            var Products = await _productRepo.GetAllWithSpecAsync(spec);
            var data= _mapper.Map<IEnumerable<Product>,IEnumerable<ProductToReturnDto>>(Products);
            var countSpec = new ProductWithFilterationForCountSpecification(productsSpec);
            int count= await _productRepo.GetCountAsync(countSpec);

            return Ok(new Pagination<ProductToReturnDto>(productsSpec.PageSize, productsSpec.PageIndex, count, data));
        }
        [Authorize]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductById(int id)
        {
            var spec = new ProductsWithBrandsAndCategorySpec(id);
            var Product = await _productRepo.GetWithSpecAsync(spec);
            if (Product == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<Product,ProductToReturnDto>(Product);
            return Ok(result);
        }


    }
} 
