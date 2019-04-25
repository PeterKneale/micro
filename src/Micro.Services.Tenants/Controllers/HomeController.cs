using Microsoft.AspNetCore.Mvc;

namespace Micro.Services.Tenants.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        public ContentResult Index() => Content(content: typeof(Startup).Namespace);
    }
}
