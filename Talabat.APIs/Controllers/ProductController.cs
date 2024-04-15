using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs.Controllers
{

    public class ProductController : BaseApiController
    {
        public ProductController(IGenericRepository<Product> productRepo)
        {
            
        }
    }
}
