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
            var lowerName = name.ToLower();
            var personsBirthday =
                from Person person in birthdays
                where lowerName == person.Name.ToLower() || lowerName == person.SurName.ToLower() 
                                || lowerName == $"{person.Name.ToLower()} {person.SurName.ToLower()}"
                select person;

            return personsBirthday;
        }
    }
}