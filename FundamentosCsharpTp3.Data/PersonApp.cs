using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FundamentosCsharpTp3.Models;

namespace FundamentosCsharpTp3.Data
{
    public class PersonApp
    {
        static ArrayList birthdays = new ArrayList();
        
        public static void AddBirthday(string name, string surName, DateTime birthday)
        {
            var personBirthday = new Person(name, surName, birthday);
            birthdays.Add(personBirthday);
        }

        public static IEnumerable<Person> ShowByName(string name)
        {
            var personsBirthday =
                from Person person in birthdays
                where name == person.Name || name == person.SurName || name == $"{person.Name} {person.SurName}"
                select person;

            return personsBirthday;
        }
    }
}