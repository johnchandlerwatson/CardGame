using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vue.Models;
using Vue.Domain;

namespace Vue.Controllers
{
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        // GET api/values
        [HttpGet]
        public Profile Get()
        {
            return new Profile 
            { 
                Demographics = new Demographics 
                {
                    FirstName = "John",
                    MiddleName = "Chandler",
                    LastName = "Watson",
                    Age = 26,
                    Gender = Domain.Gender.Male
                },
                Experience = new List<Experience>
                {
                    new Experience 
                    { 
                        Duration = "Two years",
                        Company = "Usable Life",
                        Title = "Developer II"
                    }
                }
            };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
