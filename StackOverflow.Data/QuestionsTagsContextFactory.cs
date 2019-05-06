using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace StackOverflow.Data
{
    public class QuestionsTagsContextFactory : IDesignTimeDbContextFactory<QuestionsTagsContext>
    {
        public QuestionsTagsContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}StackOverflow.Web"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new QuestionsTagsContext(config.GetConnectionString("ConStr"));
        }
    }

}
