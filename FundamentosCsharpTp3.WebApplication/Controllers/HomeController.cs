using System;
using System.Diagnostics;
using System.Linq;
using FundamentosCsharpTp3.Models;
using Microsoft.AspNetCore.Mvc;
using FundamentosCsharpTp3.WebApplication.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;
using FundamentosCsharpTp3.Api.NovaPasta;
using System.Net.Http.Headers;

namespace FundamentosCsharpTp3.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private string BaseURL = "https://csharptpapi.azurewebsites.net/api/";
        public string token = null;
        
        public HomeController()
        {}

        private async Task<IEnumerable<Person>> getFriends()
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage result = await client.GetAsync("friends");

                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Person>>(readTask);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error.");
                    return Enumerable.Empty<Person>();
                }
            }
        }

        private async Task<Person> getFriendForId(Guid id)
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage result = await client.GetAsync($"friends/{id}");

                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Person>(readTask);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error.");
                    return null;
                }
            }
        }

        private async Task<String> postFriend(Person model)
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var serializedModel = JsonConvert.SerializeObject(new Friend(model.Id, model.Name, model.SurName, model.Email, model.Birthday));
                var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");
                HttpResponseMessage result = await client.PostAsync("friends", content);

                if (result.IsSuccessStatusCode)
                {
                    return null;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error.");
                    return null;
                }
            }
        }

        private async Task<String> putFriend(Person model)
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var serializedModel = JsonConvert.SerializeObject(new Friend(model.Id, model.Name, model.SurName, model.Email, model.Birthday));
                var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");
                HttpResponseMessage result = await client.PutAsync("friends", content);

                if (result.IsSuccessStatusCode)
                {
                    return null;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error.");
                    return null;
                }
            }
        }

        private async Task<String> deleteFriends(Guid id)
        {
            token = HttpContext.Session.GetString("token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage result = await client.DeleteAsync($"friends/{id}");

                if (result.IsSuccessStatusCode)
                {
                    return null;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error.");
                    return null;
                }
            }
        }


        public IActionResult Index(string? message)
        {
            var msg = message;
            return RedirectToAction("IndexBirthday", "Home", new { message = msg });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction("Index", "Login", new { message = "Usuário deslogado" });
        }

        public async Task<IActionResult> IndexBirthday(string? message)
        {
            ViewBag.Message = message;
            IEnumerable<Person> resultSql = await getFriends();
            foreach (Person person in resultSql)
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString(person.Id.ToString())))
                {
                    HttpContext.Session.SetString(person.Id.ToString(), "false");
                }
            }
            return View("Index", resultSql.OrderBy(person => person.NextBirthday));
        }

        public async Task<IActionResult> IndexEmail(string? message)
        {
            ViewBag.Message = message;
            IEnumerable<Person> resultSql = await getFriends();
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
        
        public async Task<IActionResult> Edit([FromQuery] Guid id)
        {
            var friend = await getFriendForId(id);
            return View(friend);
        }
        
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var friend = await getFriendForId(id);
            return View(friend);
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Save(Person model)
        {
            if (ModelState.IsValid == false)
                return View();
            
            model.Id = Guid.NewGuid();
            await postFriend(model);
        
            return RedirectToAction("Index", "Home", new { message = "cadastrado com sucesso" });
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveEdit(Guid id, Person model)
        {
            if (ModelState.IsValid == false)
                return View();

            await putFriend(model);
        
            return RedirectToAction("Index", "Home", new { message = "editado com sucesso" });
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteBirthday(Guid id)
        {
            if (ModelState.IsValid == false)
                return View();

            deleteFriends(id);
            return RedirectToAction("Index", "Home", new { message = "excluído com sucesso" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [HttpPost]
        public async Task<IActionResult> HandleSelection(SelectionOptions options)
        {
            IEnumerable<Person> persons = await getFriends();

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