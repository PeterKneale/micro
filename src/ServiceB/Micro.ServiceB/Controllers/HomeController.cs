using Microsoft.AspNetCore.Mvc;

namespace Micro.ServiceB.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        public ContentResult Index() => Content(content: typeof(Startup).Namespace);
    }
}
