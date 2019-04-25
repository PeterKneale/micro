using Microsoft.AspNetCore.Mvc;

namespace Micro.Services.Content.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        public ContentResult Index() => Content(content: $"{Program.AppName} v{Program.AppVersion} - {Program.EnvName}");
    }
}
