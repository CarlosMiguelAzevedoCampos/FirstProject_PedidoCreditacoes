using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Post(CreditacaoDto creditacaoDto)
        {
            return View();
        }
    }
}
