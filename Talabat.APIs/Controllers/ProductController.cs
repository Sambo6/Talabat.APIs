using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_Spec;

namespace Talabat.APIs.Controllers
{

    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;

        public ProductController(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }
        //BaseUrl/api/products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var spec = new ProductWithBrandAndCategorySpecifications();
            var products = await _productRepo.GetAllWithSpecAsync(spec);
            return Ok(products);
        }

        //BaseUrl/api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepo.GetAsync(id);

            if (product == null)
                return NotFound(new { Message = "Not Found", StatusCode = 404 });

            return Ok(product);
        }
    }
}
