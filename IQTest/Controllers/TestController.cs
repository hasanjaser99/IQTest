using IQTest.DataAccess.Data;
using IQTest.Models;
using IQTest.Models.ViewModels;
using IQTest.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IQTest.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TestController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
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

        public IActionResult Score(Guid id)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == id);

            return View(user);
        }

        [HttpPost]
        public IActionResult Score(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User oldUser = _db.Users.FirstOrDefault(u => u.Id == user.Id);
                    oldUser.Name = user.Name;
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                return View(user);
            }
            return View(user);

        }

        #region API_CALLS
        [HttpPost]
        public IActionResult GetQuestion(int number)
        {

            var questionsStore = HttpContext.Session.GetObject<QuestionsVM>("Questions");

            // Get Next question

            //initialize view model
            var nextQuestionVM = new QuestionVM();

            //check if there is no questions in session
            if(questionsStore == null)
            {
                nextQuestionVM.Question = _db.Questions.FirstOrDefault(q => q.Number == number);
            }else
            {
                // check if next question exists in session
                var nxtQuestionVM = questionsStore.Questions.FirstOrDefault(q => q.Question.Number == number);

                if (nxtQuestionVM != null)
                {
                    nextQuestionVM = nxtQuestionVM;
                }
                else
                {
                    nextQuestionVM.Question = _db.Questions.FirstOrDefault(q => q.Number == number);
                }
            }


            return PartialView("~/Views/Shared/Partials/_QuestionForm.cshtml"
                , nextQuestionVM);

        }


        [HttpPost]
        public IActionResult PreviousQuestion(int number)
        {
            var questionsStore = HttpContext.Session.GetObject<QuestionsVM>("Questions");

            var prevQuestionVM = new QuestionVM();

            if (questionsStore != null)
            {
                prevQuestionVM = questionsStore.Questions.FirstOrDefault(q => q.Question.Number == number);

                if(prevQuestionVM == null)
                {
                    prevQuestionVM = new QuestionVM() {
                        Question = _db.Questions.FirstOrDefault(q => q.Number == number)
                    };
                }

            }
            else
            {
                prevQuestionVM.Question = _db.Questions.FirstOrDefault(q => q.Number == number);
            }

            return PartialView("~/Views/Shared/Partials/_QuestionForm.cshtml"
                , prevQuestionVM);

        }



        [HttpPost]
        public IActionResult SaveAnswer(int number, string selectedChoice)
        {

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

            var finialScore = 0;

            // calcualte result of the test
            foreach (var qVM in questionsStore.Questions)
            {
                if (qVM.SelectedChoice == qVM.Question.Answer)
                {
                    finialScore += qVM.Question.Score;
                }
            }


            var startTime = HttpContext.Session.GetObject<DateTime>("StartTime");

            TimeSpan takenTime = DateTime.UtcNow - startTime;


            User user = new User()
            {
                Id = new Guid(),
                Score = finialScore,
                Time = takenTime,
                Name = "anonymous"
            };


            _db.Users.Add(user);
            _db.SaveChanges();

            // Clear Sessions
            HttpContext.Session.Clear();

            return Json(new { redirectToUrl = Url.Action(nameof(Score), "Test", new { id = user.Id }) });

        }



        #endregion

    }
}
