using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Drawing.Printing;
using System.Text;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CachedAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Ask CLR to create object from "ResponseCacheService" Explicity
            var respnseCacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cachedKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var response = await respnseCacheService.GetCachedResponseAsync(cachedKey);

            if (!string.IsNullOrEmpty(response))
            {
                var result = new ContentResult()
                {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
               context.Result = result;
                return;
            }

            //Response Is not Cached
            var executedActionContext = await next.Invoke(); //Will Exrcute the next Action filter or Endpoint

            if (executedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
            {
                await respnseCacheService.CacheResponseAsync(cachedKey, okObjectResult.Value,TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            // BaseUrl/api/products?pageIndex=1&pageSize=5&sort=name

            //Ask your self [ String VS StringBuilder]
            var keyBuilder = new StringBuilder();

            keyBuilder.Append(request.Path);  //BaseUrl/api/products

            //pageIndex = 1 &
            //pageSize = 5 &
            //sort = name
            foreach (var (key,value) in request.Query)
            {
                //BaseUrl/api/products|pageIndex - 1
                keyBuilder.Append($"| {key}-{value}");
                //BaseUrl/api/products|pageIndex - 1
                //BaseUrl/api/products|pageIndex - 1|pageSize - 5
                //BaseUrl/api/products|pageIndex - 1|pageSize - 5|sort - name
            }
            return keyBuilder.ToString();
        }
    }
}
