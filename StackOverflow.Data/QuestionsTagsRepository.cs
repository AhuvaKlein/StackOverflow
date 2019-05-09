using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackOverflow.Data
{
    public class QuestionsTagsRepository
    {
        private string _connectionString;

        public QuestionsTagsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Question> GetAllQuestions()
        {
            using (var context = new QuestionsTagsContext(_connectionString))
            {
                return context.Questions.Include(q => q.Likes).Include(q => q.QuestionsTags).OrderByDescending(q => q.DatePosted).ToList();
            }
        }

        public Question GetQuestion(int id)
        {
            using (var context = new QuestionsTagsContext(_connectionString))
            {
                return context.Questions.Include(t => t.QuestionsTags).Include(l => l.Likes).FirstOrDefault(q => q.Id == id);
            }
        }

        public IEnumerable<Tag> GetTagsForQuestion(IEnumerable<QuestionsTags> ids)
        {
            using (var context = new QuestionsTagsContext(_connectionString))
            {
                List<Tag> tags = new List<Tag>();
                foreach (QuestionsTags qt in ids)
                {
                    tags.Add(context.Tags.FirstOrDefault(t => t.Id == qt.TagId));
                }
                return tags;
            }
        }

        public int AddUser(User user)
        {
            using (var context = new QuestionsTagsContext(_connectionString))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                context.Users.Add(user);
                context.SaveChanges();
            }
            return user.Id;
        }

        public bool LoginVerify(User user)
        {
            using (var context = new QuestionsTagsContext(_connectionString))
            {
                User u = GetUserByEmail(user.Email);
                bool verify = false;
                if (u != null)
                {
                    verify = BCrypt.Net.BCrypt.Verify(user.Password, u.Password);
                }
                return verify;


            }
        }

        public User GetUserByEmail(string email)
        {
            using (var context = new QuestionsTagsContext(_connectionString))
            {
                return context.Users.FirstOrDefault(u => u.Email == email);
            }
        }

        private Tag GetTag(string name)
        {
            using (var ctx = new QuestionsTagsContext(_connectionString))
            {
                return ctx.Tags.FirstOrDefault(t => t.Name == name);
            }
        }

        private int AddTag(string name)
        {
            using (var ctx = new QuestionsTagsContext(_connectionString))
            {
                var tag = new Tag { Name = name };
                ctx.Tags.Add(tag);
                ctx.SaveChanges();
                return tag.Id;
            }
        }

        public void AddQuestion(Question question, IEnumerable<string> tags)
        {
            using (var context = new QuestionsTagsContext(_connectionString))
            {
                context.Questions.Add(question);
                foreach (string tag in tags)
                {
                    Tag t = GetTag(tag);
                    int tagId;
                    if (t == null)
                    {
                        tagId = AddTag(tag);
                    }
                    else
                    {
                        tagId = t.Id;
                    }
                    context.QuestionsTags.Add(new QuestionsTags
                    {
                        QuestionId = question.Id,
                        TagId = tagId
                    });
                }

                context.SaveChanges();
            }
        }

        public void AddAnswer(Answer answer)
        {
            using (var context = new QuestionsTagsContext(_connectionString))
            {
                context.Answers.Add(answer);
                context.SaveChanges();
            }
        }

        public IEnumerable<Answer> GetAnswersByQuestionId(int id)
        {
            using (var context = new QuestionsTagsContext(_connectionString))
            {
                return context.Answers.Include(a => a.User).Where(a => a.QuestionId == id).ToList();
            }
        }

        public void AddLike(Like like)
        {
            using (var context = new QuestionsTagsContext(_connectionString))
            {
                context.Likes.Add(new Like
                {
                    QuestionId = like.QuestionId,
                    UserId = like.UserId
                });
                context.SaveChanges();
            }
        }

        public bool AlreadyLiked(int userId, int questionId)
        {
            using (var context = new QuestionsTagsContext(_connectionString))
            {
                Question question = context.Questions.Include(l => l.Likes).FirstOrDefault(q => q.Id == questionId);
                return question.Likes.Any(u => u.UserId == userId);
            }
        }

        public int NumberOfLikes(int questionId)
        {
            using (var context = new QuestionsTagsContext(_connectionString))
            {
                IEnumerable<Like> l = context.Likes.Where(li => li.QuestionId == questionId);
                return l.Count();
            }
        }
    }
}
