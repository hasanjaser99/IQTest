using System;
using System.Collections.Generic;
using System.Text;

namespace IQTest.Models
{
    public class Question
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public string Choice1 { get; set; }

        public string Choice2 { get; set; }

        public string Choice3 { get; set; }

        public string Choice4 { get; set; }

        public string Answer { get; set; }

        public int Score { get; set; }

    }
}
