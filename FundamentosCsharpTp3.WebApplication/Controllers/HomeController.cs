using FundamentosCsharpTp3.Data;
using Microsoft.AspNetCore.Mvc;

namespace FundamentosCsharpTp3.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View(PersonApp.showAllBirthdays());
        }
    }
}