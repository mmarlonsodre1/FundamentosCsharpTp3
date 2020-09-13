using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FundamentosCsharpTp3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FundamentosCsharpTp3.WebApplication.Controllers
{
    public class LoginController : Controller
    {
        private string BaseURL = "https://csharptpapi.azurewebsites.net/api/";

        public LoginController()
        { }

        private async Task<AuthenticationReturn> postLogin(User model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                var serializedModel = JsonConvert.SerializeObject(model);
                var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");
                HttpResponseMessage result = await client.PostAsync("users/authorize", content);

                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<AuthenticationReturn>(readTask);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error.");
                    return null;
                }
            }
        }

        private async Task<bool> postRegister(User model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                var serializedModel = JsonConvert.SerializeObject(model);
                var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");
                HttpResponseMessage result = await client.PostAsync("users", content);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error.");
                    return false;
                }
            }
        }

        public IActionResult Index(string? message)
        {
            ViewBag.Message = message;
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            if (ModelState.IsValid == false)
                return View();

            var login = await postLogin(model);
            if (login != null)
            {
                HttpContext.Session.SetString("token", login.Token);
                return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Index", "Login", new { message = "Erro ao logar" });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(User model)
        {
            if (ModelState.IsValid == false)
                return View();

            model.Id = Guid.NewGuid();
            var login = await postRegister(model);
            if (login) return RedirectToAction("Index", "Login", new { message = "Registrado com sucesso" });
            else return RedirectToAction("Index", "Login", new { message = "Erro ao Registrar" });
        }
    }
}
