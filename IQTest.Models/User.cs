using System;
using System.ComponentModel.DataAnnotations;

namespace IQTest.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Please Enter Your Name")]
        public string Name { get; set; }

        public int Score { get; set; }

        public DateTime Time { get; set; }

    }
}
