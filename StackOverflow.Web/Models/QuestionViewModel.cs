using StackOverflow.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackOverflow.Web.Models
{
    public class QuestionViewModel
    {
        public bool IsVerified { get; set; }
        public Question Question { get; set; } 
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
        public User User { get; set; }
        public bool AlreadyLiked { get; set; }
    }
}
