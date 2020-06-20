using System;
using System.Diagnostics;
using System.Linq;
using FundamentosCsharpTp3.Models;
using Microsoft.AspNetCore.Mvc;
using FundamentosCsharpTp3.WebApplication.Models;
using FundamentosCsharpTp3.WebApplication.Repository;

namespace FundamentosCsharpTp3.WebApplication.Controllers
{
    public class FullBirthdaysController : Controller
    {
        private PersonRepository PersonRepository { get; set; }
        
        public FullBirthdaysController(PersonRepository personRepository)
        {
            PersonRepository = personRepository;
        }

        public IActionResult Index(string? message)
        {
            ViewBag.Message = message;
            var resultSql = PersonRepository.GetAllBirthdays();
            return View(resultSql.OrderBy(person => person.NextBirthday));
        }

        public IActionResult New()
        {
            return View();
        }
        
        public IActionResult Edit([FromQuery] Guid id)
        {
            return View(PersonRepository.GetBirthdayById(id));
        }
        
        public IActionResult Delete([FromQuery] Guid id)
        {
            return View(PersonRepository.GetBirthdayById(id));
        }
        
        
        [HttpPost]
        public IActionResult Save(Person model)
        {
            if (ModelState.IsValid == false)
                return View();
            PersonRepository.Save(model);
        
            return RedirectToAction("Index", "FullBirthdays", new { message = "Aniversariante cadastrado com sucesso" });
        }
        
        [HttpPost]
        public IActionResult SaveEdit(Guid id, Person model)
        {
            if (ModelState.IsValid == false)
                return View();
        
            PersonRepository.Update(model);
        
            return RedirectToAction("Index", "FullBirthdays", new { message = "Aniversariante editado com sucesso" });
        }
        
        [HttpPost]
        public IActionResult DeleteBirthday(Guid id)
        {
            if (ModelState.IsValid == false)
                return View();

            PersonRepository.Delete(id);
            return RedirectToAction("Index", "FullBirthdays", new { message = "Aniversariante excluído com sucesso" });
        }
        
        [HttpPost]
        public IActionResult Search(String name)
        {
            var resultSql = PersonRepository.GetBirthdaysByName(name);
            return View(resultSql.OrderBy(person => person.NextBirthday));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}