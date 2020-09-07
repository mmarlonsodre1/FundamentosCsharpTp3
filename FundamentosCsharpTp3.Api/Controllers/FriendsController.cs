using System;
using System.Collections.Generic;
using System.Linq;
using FundamentosCsharpTp3.Api.NovaPasta;
using FundamentosCsharpTp3.Models;
using FundamentosCsharpTp3.WebApplication.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FundamentosCsharpTp3.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FriendsController : ControllerBase
    {
        private PersonRepository PersonRepository { get; set; }

        public FriendsController(PersonRepository personRepository)
        {
            PersonRepository = personRepository;
        }

        [HttpGet]
        public List<Friend> Get()
        {
            var resultSql = PersonRepository.GetAllBirthdays(); 
            List<Friend> response = new List<Friend>();
            foreach (Person person in resultSql)
            {
                var friend = new Friend(person.Id, person.Name, person.SurName, person.Email, person.Birthday);
                response.Add(friend);
            }
            return response;
        }

        [HttpGet("{id}")]
        public Friend Get(Guid id)
        {
            var resultSql = PersonRepository.GetBirthdayById(id);
            var response = new Friend(resultSql.Id, resultSql.Name, resultSql.SurName, resultSql.Email, resultSql.Birthday);
            return response;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Person model)
        {
            model.Id = Guid.NewGuid();
            PersonRepository.Save(model);
            return Ok(model);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Person model)
        {
            PersonRepository.Update(model);
            return Ok(model);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            PersonRepository.Delete(id);
            return Ok();
        }
    }
}
