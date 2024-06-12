using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
namespace Cat.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _cache;
        public HomeController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetCatStatusImage(string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out var uriResult))
            {
                var cacheKey = $"StatusCodeImage-{uriResult}";
                if (_cache.TryGetValue(cacheKey, out byte[] imageFromCache))
                {
                    return File(imageFromCache, "image/jpeg");
                }
                using (HttpClient client = new HttpClient())
                {
                    var request = WebRequest.Create(url) as HttpWebRequest;
                    var response = request.GetResponse() as HttpWebResponse;
                    HttpStatusCode statusCode = response.StatusCode;
                    Console.WriteLine("Status Code: {0}", (int)statusCode);
                    var catImageUrl = $"https://http.cat/{(int)statusCode}";
                    var catImageResponse = await client.GetAsync(catImageUrl);
                    var imageBytes = await catImageResponse.Content.ReadAsByteArrayAsync();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(60));
                    _cache.Set(cacheKey, imageBytes, cacheEntryOptions);
                    return File(imageBytes, "image/jpeg");
                }
            }
            return null;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}