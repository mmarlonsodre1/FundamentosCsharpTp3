using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FundamentosCsharpTp3.Models;

namespace FundamentosCsharpTp3.Data
{
    public class PersonApp
    {
        private static ArrayList birthdays = new ArrayList();

        public static void GetAllBirthdaysInFile()
        {
            string nomeDoArquivo = GetFileName();

            FileStream arquivo;
            if (!File.Exists(nomeDoArquivo))
            {
                arquivo = File.Create(nomeDoArquivo);
                arquivo.Close();
            }

            string resultado = File.ReadAllText(nomeDoArquivo);

            string[] birthdaysInFile = resultado.Split(';');


            for (int i = 0; i < birthdaysInFile.Length - 1; i++)
            {
                string[] personBirthdays = birthdaysInFile[i].Split(',');
                
                Guid id = Guid.Parse(personBirthdays[0]);
                string name = personBirthdays[1];
                string surname = personBirthdays[2];
                DateTime date = Convert.ToDateTime(personBirthdays[3]);

                Person funcionario = new Person(id, name, surname, date);

                birthdays.Add(funcionario);
            }
        }

        public static void SaveListInFile()
        {
            string fileName = GetFileName();
            File.WriteAllText(fileName, "");
            foreach (Person birthday in birthdays)
            {
                var format = $"{birthday.Id},{birthday.Name},{birthday.SurName},{birthday.Birthday.ToString()};";
                File.AppendAllText(fileName, format);
            }
        }
        
        private static string GetFileName()
        {
            var pastaDesktop = Environment.SpecialFolder.Desktop;

            string localDaPastaDesktop = Environment.GetFolderPath(pastaDesktop);
            string nomeDoArquivo = @"\dadosDosFuncionariosAtMarlon.txt";

            return localDaPastaDesktop + nomeDoArquivo;
        }
        
        public static void AddBirthday(Person person)
        {
            person.Id = Guid.NewGuid();
            birthdays.Add(person);
            SaveListInFile();
        }

        public static void RemoveBirthday(Guid id)
        {
            birthdays.Remove(ShowById(id));
            SaveListInFile();
        }

        public static Person ShowById(Guid id)
        {
            var personsBirthday =
                from Person person in birthdays
                where id == person.Id
                select person;
            return personsBirthday.FirstOrDefault();
        }

        public static IEnumerable<Person> ShowByName(string name)
        {
            var lowerName = name.ToLower();
            var personsBirthday =
                from Person person in birthdays
                where lowerName == person.Name.ToLower() || lowerName == person.SurName.ToLower() 
                                || lowerName == $"{person.Name.ToLower()} {person.SurName.ToLower()}"
                select person;

            return personsBirthday.OrderBy(person => person.NextBirthday);
        }

        public static IEnumerable<Person> showAllBirthdays()
        {
            var personsBirthday =
                from Person person in birthdays
                select person;
            return personsBirthday.OrderBy(person => person.NextBirthday);
        }
    }
}