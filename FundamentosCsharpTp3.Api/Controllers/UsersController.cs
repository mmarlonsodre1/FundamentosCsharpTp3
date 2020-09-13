using System;
using FundamentosCsharpTp3.Api.Services;
using FundamentosCsharpTp3.Models;
using FundamentosCsharpTp3.WebApplication.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FundamentosCsharpTp3.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserRepository UserRepository { get; set; }

        public IAuthService AuthService { get; set; }

        public UsersController(UserRepository userRepository, IAuthService authService)
        {
            UserRepository = userRepository;
            AuthService = authService;
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User model)
        {
            model.Id = Guid.NewGuid();
            UserRepository.Save(model);
            return Ok();
        }

        [HttpPost]
        [Route("authorize")]
        public IActionResult Authorize([FromBody] User model)
        {
            AuthenticationReturn auth = AuthService.Authenticate(model);
            if (!auth.Status)
                return BadRequest(auth);

            return Ok(auth);
        }
    }
}
