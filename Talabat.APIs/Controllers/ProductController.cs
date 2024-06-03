using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dto;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Product_Spec;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductsController(
            IProductService productService,
            IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }


        [Cached(800)]
        [HttpGet]
        //[Authorize()]

        #region GetProducts EndPoint
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var products = await _productService.GetProductsAsync(specParams);

            var count = await _productService.GetCountAsync(specParams);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, count, data));
        } 
        #endregion


        [Cached(800)]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]

        #region GetProduct EndPoint
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductAsync(id);

            if (product is null)
                return NotFound(new ApiResponse(404)); // 404

            return Ok(_mapper.Map<Product, ProductToReturnDto>(product)); // 200
        } 
        #endregion


        [Cached(800)]
        [HttpGet("brands")]

        #region GetBrands EndPoint
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _productService.GetBrandsAsync();

            return Ok(brands);
        } 
        #endregion


        [Cached(800)]
        [HttpGet("categories")]

        #region GetCategories EndPoint
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _productService.GetCategoriesAsync();

            return Ok(categories);
        } 
        #endregion

    }
}
