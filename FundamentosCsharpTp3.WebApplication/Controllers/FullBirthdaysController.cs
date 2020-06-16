using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FundamentosCsharpTp3.Data;
using FundamentosCsharpTp3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FundamentosCsharpTp3.WebApplication.Models;

namespace FundamentosCsharpTp3.WebApplication.Controllers
{
    public class FullBirthdaysController : Controller
    {
        private readonly ILogger<FullBirthdaysController> _logger;

        public FullBirthdaysController(ILogger<FullBirthdaysController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string? message)
        {
            ViewBag.Message = message;
            return View(PersonApp.showAllBirthdays());
        }

        public IActionResult New()
        {
            return View();
        }
        
        public IActionResult Edit([FromQuery] Guid id)
        {
            return View(PersonApp.ShowById(id));
        }
        
        public IActionResult Delete([FromQuery] Guid id)
        {
            return View(PersonApp.ShowById(id));
        }
        
        
        [HttpPost]
        public IActionResult Save(Person model)
        {
            if (ModelState.IsValid == false)
                return View();
            PersonApp.AddBirthday(model);
        
            return RedirectToAction("Index", "FullBirthdays", new { message = "Aniversariante cadastrado com sucesso" });
        }
        
        [HttpPost]
        public IActionResult SaveEdit(Guid id, Person model)
        {
            if (ModelState.IsValid == false)
                return View();
        
            var birthdayEdit = PersonApp.ShowById(id);
        
            birthdayEdit.Name = model.Name;
            birthdayEdit.SurName = model.SurName;
            birthdayEdit.Birthday = model.Birthday;
            
            PersonApp.RemoveBirthday(birthdayEdit.Id);
            PersonApp.AddBirthday(birthdayEdit);
        
            return RedirectToAction("Index", "FullBirthdays", new { message = "Aniversariante editado com sucesso" });
        }
        
        [HttpPost]
        public IActionResult DeleteBirthday(Guid id)
        {
            if (ModelState.IsValid == false)
                return View();

            PersonApp.RemoveBirthday(id);
            return RedirectToAction("Index", "FullBirthdays", new { message = "Aniversariante excluído com sucesso" });
        }
        
        [HttpPost]
        public IActionResult Search(String name)
        {
            return View(PersonApp.ShowByName(name));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}