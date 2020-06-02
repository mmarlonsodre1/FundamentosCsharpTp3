using System;

namespace FundamentosCsharpTp3.Models
{
    public class Person
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public DateTime Birthday { get; set; }

        public Person(string name, string surName, DateTime birthday)
        {
            Name = name;
            SurName = surName;
            Birthday = birthday;
        }
    }
}