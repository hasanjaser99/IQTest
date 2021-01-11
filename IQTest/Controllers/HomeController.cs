using IQTest.DataAccess.Data;
using IQTest.Models;
using IQTest.Models.ViewModels;
using IQTest.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IQTest.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test()
        {

            var QuestionVM = new QuestionVM();

            var question = _db.Questions.FirstOrDefault(q => q.Number == 1);

            QuestionVM.Question = question;

            var StartTime = DateTime.UtcNow;

            var defaultDate = new DateTime();

            if (HttpContext.Session.GetObject<DateTime>("StartTime") != defaultDate)
            {
                StartTime = HttpContext.Session.GetObject<DateTime>("StartTime");
            }
            else
            {
                HttpContext.Session.SetObject("StartTime", StartTime);
            }

            ViewBag.StartTime = StartTime;

            return View(QuestionVM);
        }



     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region API_CALLS
        [HttpPost]
        public IActionResult NextQuestion(int number)
        {

            var questionsStore = HttpContext.Session.GetObject<QuestionsVM>("Questions");

            // Get Next question

            //initialize view model
            var nextQuestionVM = new QuestionVM();

            // check if next question exists in session
            var nxtQuestionVM = questionsStore.Questions.FirstOrDefault(q => q.Question.Number == number);

            if (nxtQuestionVM != null)
            {
                nextQuestionVM = nxtQuestionVM;
            }
            else
            {
                var nextQuestion = _db.Questions.FirstOrDefault(q => q.Number == number);

                nextQuestionVM.Question = nextQuestion;

            }

            return PartialView("~/Views/Shared/Partials/_QuestionForm.cshtml"
                , nextQuestionVM);

        }


        [HttpPost]
        public IActionResult PreviousQuestion(int number)
        {
            var questions = HttpContext.Session.GetObject<QuestionsVM>("Questions").Questions;

            var prevQuestionVM = questions.FirstOrDefault(q => q.Question.Number == number);

            return PartialView("~/Views/Shared/Partials/_QuestionForm.cshtml"
                , prevQuestionVM);

        }


        [HttpPost]
        public IActionResult SaveAnswer(int number, string selectedChoice)
        {
            // Get current question
            var currentQuestion = _db.Questions.FirstOrDefault(q => q.Number == number);

            var currentQuestionVM = new QuestionVM()
            {
                Question = currentQuestion,
                SelectedChoice = selectedChoice
            };

            // store current question in session
            var questionsStore = HttpContext.Session.GetObject<QuestionsVM>("Questions");

            //check if store is null to initialize it
            if (questionsStore == null)
            {
                var questionsVM = new QuestionsVM()
                {
                    Questions = Enumerable.Empty<QuestionVM>()
                };

                questionsVM.Questions = questionsVM.Questions.Append(currentQuestionVM);

                questionsStore = questionsVM;

                HttpContext.Session.SetObject("Questions", questionsStore);
            }
            else
            {
                var questionVM = questionsStore.Questions
                    .FirstOrDefault(qs => qs.Question.Id == currentQuestionVM.Question.Id);

                //check if current question exists in session (if exists delete it)
                if (questionVM != null)
                {
                    questionsStore.Questions = questionsStore.Questions
                        .Where(qVM => qVM.Question.Id != currentQuestionVM.Question.Id);
                }

                questionsStore.Questions = questionsStore.Questions.Append(currentQuestionVM);

                HttpContext.Session.SetObject("Questions", questionsStore);
            }

            return Json(new { message = "Saved Successfully" });

        }

        [HttpPost]
        public IActionResult FinishTest()
        {

            var questionsStore = HttpContext.Session.GetObject<QuestionsVM>("Questions");


            // Clear Sessions
            HttpContext.Session.Clear();


            var finialScore = 0;

            // calcualte result of the test
            foreach (var qVM in questionsStore.Questions)
            {
                if(qVM.SelectedChoice == qVM.Question.Answer)
                {
                    finialScore += qVM.Question.Score;
                }
            }

            return PartialView("~/Views/Shared/Partials/_FinishTest.cshtml"
                , finialScore);
        }

        #endregion
    }
}
