using System;
using System.Linq;
using FundamentosCsharpTp3.Data;

namespace FundamentosCsharpTp3
{
    class Program
    {
        static void Main(string[] args)
        {
            Write("Gerenciador de Aniversários");
            Menu();
        }

        private static void Menu()
        {
            Write("Selecione uma das opções abaixo");
            Write("1 - Pesquisar pessoas");
            Write("2 - Adicionar nova pessoa");
            Write("3 - Sair");
            
            char operation = Console.ReadLine().ToCharArray()[0];

            switch (operation)
            {
                case '1': ShowBirthdays(); break;
                case '2': AddBirthday(); break;
                case '3': break;
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

        private static void ShowBirthdays()
        {
            ClearScreen();
            Write("Entre com o nome da pessoa");
            var persons = PersonApp.ShowByName(Read()).ToList();
            
            ClearScreen();
            if (persons.Count > 0)
            {
                Write("Dados da pessoa");
                foreach (var person in persons)
                {
                    var timeNow = DateTime.Now;
                    var birthdayYear =
                        DateTime.Parse($"{person.Birthday.Day}/{person.Birthday.Month}/{DateTime.Now.Year}");
                    var timeRemain = birthdayYear.Subtract(timeNow);
                    
                    if (timeRemain.Days < 1 )
                    {
                        birthdayYear = DateTime.Parse($"{person.Birthday.Day}/{person.Birthday.Month}/{DateTime.Now.Year + 1}");
                        timeRemain = birthdayYear.Subtract(timeNow);
                    }
                    
                    Write($"Nome Completo: {person.Name} {person.SurName}");
                    Write($"Data de Aniversário: {person.Birthday}");
                    Write($"Faltam {timeRemain.Days} dias para esse aniversário\n");
                }
                BackMenu();
            }
            else
            {
                Write("Nenhum aniversariante encontrado");
                BackMenu();
            }
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
                PersonApp.AddBirthday(name, surName, birthday);
                
                Write("Aniversariante cadastrado no sistema");
                BackMenu();
            }
            catch (Exception e)
            {
                Console.WriteLine("Formato de data errada \n \n");
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