using Microsoft.EntityFrameworkCore;

namespace QueryArchive.Api.Models
{
    public class QueryArchiveDbContext : DbContext
    {
        public QueryArchiveDbContext(DbContextOptions<QueryArchiveDbContext> options) : base(options) { }

        public DbSet<Topic> Topics { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Topic>()
                .HasMany(t => t.Questions)
                .WithOne()
                .HasForeignKey(q => q.TopicID);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionID);
        }
    }
}
