using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Seguridad.Controllers
{
    [Authorize(Roles ="Administrador")]
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
