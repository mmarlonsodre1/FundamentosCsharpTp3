using System.Linq;
using FundamentosCsharpTp3.Data;
using FundamentosCsharpTp3.Models;
using FundamentosCsharpTp3.WebApplication.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FundamentosCsharpTp3.WebApplication.Controllers
{
    public class SecondScreenController : Controller
    {
        private PersonRepository PersonRepository { get; set; }
        
        public SecondScreenController(PersonRepository personRepository)
        {
            PersonRepository = personRepository;
        }
        
        public IActionResult Index()
        {
            var resultSql = PersonRepository.GetAllBirthdays();
            return View(resultSql.OrderBy(person => person.NextBirthday));
        }
    }
}