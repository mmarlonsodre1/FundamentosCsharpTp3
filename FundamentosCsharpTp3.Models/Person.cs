using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace FundamentosCsharpTp3.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Campo obrigatório")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Email { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime Birthday { get; set; }
        public TimeSpan NextBirthdayInDays
        {
            get
            {
                var timeNow = DateTime.Now;

                var birthdayYear = DateTime.Now;
                try
                {
                    birthdayYear = DateTime.ParseExact($"{Birthday.Day}/{Birthday.Month}/{DateTime.Now.Year + 1}", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    if (Birthday.Day < 10 && Birthday.Month < 10)
                    {
                        birthdayYear = DateTime.ParseExact($"0{Birthday.Day}/0{Birthday.Month}/{DateTime.Now.Year + 1}", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else if (Birthday.Day < 10 && Birthday.Month > 9)
                    {
                        birthdayYear = DateTime.ParseExact($"0{Birthday.Day}/{Birthday.Month}/{DateTime.Now.Year + 1}", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        birthdayYear = DateTime.ParseExact($"{Birthday.Day}/0{Birthday.Month}/{DateTime.Now.Year + 1}", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }

                }

                var timeRemain = birthdayYear.Subtract(timeNow);
                    
                if (timeRemain.Days < 0 )
                {
                    try
                    {
                        birthdayYear = DateTime.ParseExact($"{Birthday.Day}/{Birthday.Month}/{DateTime.Now.Year + 1}", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        if (Birthday.Day < 10 && Birthday.Month < 10)
                        {
                            birthdayYear = DateTime.ParseExact($"0{Birthday.Day}/0{Birthday.Month}/{DateTime.Now.Year + 1}", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        } else if (Birthday.Day < 10 && Birthday.Month > 9)
                        {
                            birthdayYear = DateTime.ParseExact($"0{Birthday.Day}/{Birthday.Month}/{DateTime.Now.Year + 1}", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        } else
                        {
                            birthdayYear = DateTime.ParseExact($"{Birthday.Day}/0{Birthday.Month}/{DateTime.Now.Year + 1}", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        
                    }
                    timeRemain = birthdayYear.Subtract(timeNow);
                }
                return timeRemain;
            }
            set => NextBirthdayInDays = value;
        }
        public DateTime NextBirthday
        {
            get => NextBirthdayInDays.Milliseconds < 0 ? DateTime.Now : DateTime.Now.AddDays(NextBirthdayInDays.Days + 1);
            set => NextBirthday = value;
        }

        public Person()
        { }

        public Person(string name, string surName, DateTime birthday)
        {
            Name = name;
            SurName = surName;
            Birthday = birthday;
        }

        public Person(Guid id, string name, string surName, DateTime birthday)
        {
            Id = id;
            Name = name;
            SurName = surName;
            Birthday = birthday;
        }

        public Person(Guid id, string name, string surName, string email, DateTime birthday)
        {
            Id = id;
            Name = name;
            SurName = surName;
            Email = email;
            Birthday = birthday;
        }
    }
}