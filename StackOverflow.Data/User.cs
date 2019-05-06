using System.Collections.Generic;

namespace StackOverflow.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Like> Likes { get; set; }
        public List<Answer> Answers { get; set; }
    }



}
