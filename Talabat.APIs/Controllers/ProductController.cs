using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Talabat.APIs.Dto;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_Spec;

namespace Talabat.APIs.Controllers
{

    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductController(IGenericRepository<Product> productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }
        //BaseUrl/api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductWithBrandAndCategorySpecifications();
            var products = await _productRepo.GetAllWithSpecAsync(spec);
            return Ok(_mapper.Map<IEnumerable<Product>,IEnumerable<ProductToReturnDto>>(products));
        }

        //BaseUrl/api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);
            var product = await _productRepo.GetWithSpecAsync(spec);

            if (product == null)
                return NotFound(new { Message = "Not Found", StatusCode = 404 });

            return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
        }
    }
}
