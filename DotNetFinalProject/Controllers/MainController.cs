using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetFinalProject.Controllers
{
    [AllowAnonymous]
    public class MainController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}