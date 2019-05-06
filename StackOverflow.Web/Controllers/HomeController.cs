using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StackOverflow.Data;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;
        private QuestionsTagsRepository _repo;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _repo = new QuestionsTagsRepository(_connectionString);
        }

        public IActionResult Index()
        {
            List<IndexViewModel> vm = new List<IndexViewModel>();
            IEnumerable<Question> q = _repo.GetAllQuestions();
            foreach(Question qu in q)
            {
                vm.Add(new IndexViewModel
                {
                    Question = qu,
                    Tags = _repo.GetTagsForQuestion(qu.QuestionsTags)
                });
            }
            return View(vm);
        }

        public IActionResult Question(int id)
        {
            QuestionViewModel vm = new QuestionViewModel();
            vm.IsVerified = User.Identity.IsAuthenticated;
            vm.Question = _repo.GetQuestion(id);
            vm.Tags = _repo.GetTagsForQuestion(vm.Question.QuestionsTags);
            vm.Answers = _repo.GetAnswersByQuestionId(id);         
            if (User.Identity.IsAuthenticated)
            {
                vm.User = _repo.GetUserByEmail(User.Identity.Name);
                vm.AlreadyLiked = _repo.AlreadyLiked(vm.User.Id, id);
            }
            return View(vm);
        }

        [Authorize]
        public IActionResult AskAQuestion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AskAQuestion(Question question, IEnumerable<string> tags)
        {
            question.DatePosted = DateTime.Now;
            _repo.AddQuestion(question, tags);
            return Redirect("/");
        }

        [HttpPost]
        public IActionResult Answer(Answer answer)
        {
            _repo.AddAnswer(answer);
            return Redirect($"/home/question?id={answer.QuestionId}");
        }

        public IActionResult AddLike(Like like)
        {
            _repo.AddLike(like);
            return Redirect($"/home/question?id={like.QuestionId}");
        }

        public int NumberOfLikes(int questionId)
        {
            return _repo.NumberOfLikes(questionId);
        }

        
    }
}
