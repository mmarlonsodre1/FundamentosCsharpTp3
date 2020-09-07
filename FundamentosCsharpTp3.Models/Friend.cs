using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundamentosCsharpTp3.Api.NovaPasta
{
    public class Friend
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public string Email { get; set; }

        public DateTime Birthday { get; set; }

        public Friend(Guid id, string name, string surName, string email, DateTime birthday)
        {
            Id = id;
            Name = name;
            SurName = surName;
            Email = email;
            Birthday = birthday;
        }
    }
}
