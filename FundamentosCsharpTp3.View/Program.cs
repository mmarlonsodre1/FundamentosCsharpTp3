using System;
using System.Collections.Generic;
using System.Linq;
using FundamentosCsharpTp3.Data;
using FundamentosCsharpTp3.Models;

namespace FundamentosCsharpTp3
{
    class Program
    {
        static void Main(string[] args)
        {
            PersonApp.GetAllBirthdaysInFile();
            
            Write("Gerenciador de Aniversários");
            Write("");
            showDayBirthdays();
            Menu();
        }

        private static void Menu()
        {
            Write("Selecione uma das opções abaixo");
            Write("1 - Pesquisar aniversariante");
            Write("2 - Adicionar novo aniversariante");
            Write("3 - Editar aniversariante");
            Write("4 - Deletar aniversariante");
            Write("5 - Sair");
            
            char operation = Console.ReadLine().ToCharArray()[0];

            switch (operation)
            {
                case '1': ShowBirthdays(); break;
                case '2': AddBirthday(); break;
                case '3': EditBirthday(); break;
                case '4': RemoveBirthday(); break;
                case '5': break;
                default: Write("Opção inexistente"); BackMenu(); break;
            }
        }

        private static void Write(string x)
        {
          Console.WriteLine(x);  
        }

        private static string Read()
        {
            return Console.ReadLine()?.Trim();
        }

        private static void ClearScreen()
        {
            Console.Clear();
        }

        private static void showDayBirthdays()
        {
            var persons = PersonApp.showAllBirthdays();
            List<Person> dayBirthdays = new List<Person>();
            
            foreach (var person in persons)
            {
                if (person.NextBirthday.Date == DateTime.Now.Date)
                {
                   dayBirthdays.Add(person);
                }
            }

            if (dayBirthdays.Count > 0)
            {
                Write("Aniversariantes do dia:");
                foreach (var person in dayBirthdays)
                {
                    if (person.NextBirthday.Date == DateTime.Now.Date)
                    {
                        Write($"{person.Name} {person.SurName}");
                    }
                }
                Write("");
                Write("");
            }
        }
        private static void ShowBirthdays()
        {
            ClearScreen();
            Write("Entre com o nome da pessoa");
            var persons = PersonApp.ShowByName(Read()).ToList();
            
            ClearScreen();
            if (persons.Count > 0)
            {
                Write("Aniversariantes:");
                WriteBirthdays(persons);
                BackMenu();
            }
            else
            {
                Write("Nenhum aniversariante encontrado");
                BackMenu();
            }
        }

        private static void WriteBirthdays(IEnumerable<Person> persons)
        {
            foreach (var person in persons)
            {
                WriteBirthday(person);
            }
        }

        private static void WriteBirthday(Person person)
        {
            Write($"ID: {person.Id}");
            Write($"Nome Completo: {person.Name} {person.SurName}");
            Write($"Data de Aniversário: {person.Birthday}");
            Write($"Faltam {person.NextBirthdayInDays.Days} dias para esse aniversário\n");
        }

        private static void AddBirthday()
        {
            ClearScreen();
            
            Write("Entre com o primeiro nome da pessoa");
            string name = Read();
            
            Write("Entre com o sobrenome da pessoa");
            string surName = Read();
            
            Write("Entre com a data de aniversário da pessoa em formato dd/mm/yyyy");
            DateTime birthday;
            
            try
            {
                birthday = DateTime.Parse(Read());
                Person personBirthday = new Person(name, surName, birthday);
                PersonApp.AddBirthday(personBirthday);
                
                Write("Aniversariante cadastrado no sistema");
                BackMenu();
            }
            catch (Exception e)
            {
                Console.WriteLine("Formato de data errada \n \n");
                BackMenu();
            }
        }

        private static bool ShowAllBirthdays()
        {
            var persons = PersonApp.showAllBirthdays();
            if (persons.Count() > 0)
            {
                Write("Aniversariantes Cadastrados:");
                WriteBirthdays(persons);
                return true;
            }
            else
            {
                Write("Não existe aniversariantes cadastrados");
                return false;
            }
        }
        
        private static void EditBirthday()
        {
            ClearScreen();
            
            if (ShowAllBirthdays())
            {
                Write("Digite o ID do aniversariante que deseja editar:");
                string id = Read();

                try
                {
                    var person = PersonApp.ShowById(Guid.Parse(id));
                    if (person != null)
                    {
                        WriteBirthday(person);
                        Write("");
                        
                        Write("Entre com o primeiro nome da pessoa");
                        string name = Read();
            
                        Write("Entre com o sobrenome da pessoa");
                        string surName = Read();
            
                        Write("Entre com a data de aniversário da pessoa em formato dd/mm/yyyy");
                        DateTime birthday;
            
                        try
                        {
                            birthday = DateTime.Parse(Read());
                            Person personBirthday = new Person(name, surName, birthday);
                            PersonApp.RemoveBirthday(Guid.Parse(id));
                            PersonApp.AddBirthday(personBirthday);
                
                            Write("Aniversariante editado");
                            BackMenu();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Formato de data errada \n \n");
                            BackMenu();
                        }
                    }
                    else
                    {
                        Write("Aniversariante não encontrado");
                        BackMenu();
                    }
                }
                catch (Exception e)
                {
                    Write("ID inválido");
                    BackMenu();
                }
            }
            else
            {
                BackMenu();
            }
        }
        
        private static void RemoveBirthday()
        {
            ClearScreen();
            
            if (ShowAllBirthdays())
            {
                Write("Digite o ID do aniversariante que deseja excluir:");
                string id = Read();

                try
                {
                    var person = PersonApp.ShowById(Guid.Parse(id));
                    if (person != null)
                    {
                        PersonApp.RemoveBirthday(Guid.Parse(id));
                        Write("Aniversariante deletado");
                        BackMenu();
                    } 
                    else
                    {
                        Write("Aniversariante não encontrado");
                        BackMenu();
                    }
                }
                catch (Exception e)
                {
                    Write("ID inválido");
                    BackMenu();
                }
            }
            else
            {
                BackMenu();
            }
        }

        private static void BackMenu()
        {
            Write("Pressione qualquer tecla para continuar");
            Console.ReadKey();
            
            ClearScreen();
            Menu();
        }
    }
}