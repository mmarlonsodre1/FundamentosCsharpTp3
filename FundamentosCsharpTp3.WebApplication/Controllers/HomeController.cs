using System;
using System.Diagnostics;
using System.Linq;
using FundamentosCsharpTp3.Models;
using Microsoft.AspNetCore.Mvc;
using FundamentosCsharpTp3.WebApplication.Models;
using FundamentosCsharpTp3.WebApplication.Repository;
using System.Collections;
using Microsoft.AspNetCore.Http;

namespace FundamentosCsharpTp3.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private PersonRepository PersonRepository { get; set; }
        
        public HomeController(PersonRepository personRepository)
        {
            PersonRepository = personRepository;
        }

        public IActionResult Index(string? message)
        {
            var msg = message;
            return RedirectToAction("IndexBirthday", "Home", new { message = msg });
        }

        public IActionResult IndexBirthday(string? message)
        {
            ViewBag.Message = message;
            var resultSql = PersonRepository.GetAllBirthdays();
            foreach (Person person in resultSql)
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString(person.Id.ToString())))
                {
                    HttpContext.Session.SetString(person.Id.ToString(), "false");
                }
            }
            return View("Index", resultSql.OrderBy(person => person.NextBirthday));
        }

        public IActionResult IndexEmail(string? message)
        {
            ViewBag.Message = message;
            var resultSql = PersonRepository.GetAllBirthdays();
            foreach (Person person in resultSql)
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString(person.Id.ToString())))
                {
                    HttpContext.Session.SetString(person.Id.ToString(), "false");
                }
            }
            return View("indexEmail", resultSql.OrderBy(person => person.NextBirthday));
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
            
            model.Id = Guid.NewGuid();
            PersonRepository.Save(model);
        
            return RedirectToAction("Index", "Home", new { message = "cadastrado com sucesso" });
        }
        
        [HttpPost]
        public IActionResult SaveEdit(Guid id, Person model)
        {
            if (ModelState.IsValid == false)
                return View();
        
            PersonRepository.Update(model);
        
            return RedirectToAction("Index", "Home", new { message = "editado com sucesso" });
        }
        
        [HttpPost]
        public IActionResult DeleteBirthday(Guid id)
        {
            if (ModelState.IsValid == false)
                return View();

            PersonRepository.Delete(id);
            return RedirectToAction("Index", "Home", new { message = "excluído com sucesso" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [HttpPost]
        public IActionResult HandleSelection(SelectionOptions options)
        {
            var persons = PersonRepository.GetAllBirthdays();

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(options.Id)))
            {
                HttpContext.Session.SetString(options.Id, options.NewValue);
            }

            return View(options.ViewName, persons);
        }

        public class SelectionOptions
        {
            public string Id { get; set; }
            public string NewValue { get; set; }
            public string ViewName { get; set; }

            public SelectionOptions()
            {
            }

            public SelectionOptions(string _Id, string _NewValue, string _ViewName)
            {
                Id = _Id;
                NewValue = _NewValue;
                ViewName = _ViewName;
            }
        }
    }
}