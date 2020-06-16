using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FundamentosCsharpTp3.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Campo obrigatório")]
        public string SurName { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime Birthday { get; set; }
        public TimeSpan NextBirthdayInDays
        {
            get
            {
                var timeNow = DateTime.Now;
                var birthdayYear =
                    DateTime.Parse($"{Birthday.Day}/{Birthday.Month}/{DateTime.Now.Year}");
                var timeRemain = birthdayYear.Subtract(timeNow);
                    
                if (timeRemain.Days < 0 )
                {
                    birthdayYear = DateTime.Parse($"{Birthday.Day}/{Birthday.Month}/{DateTime.Now.Year + 1}");
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
    }
}