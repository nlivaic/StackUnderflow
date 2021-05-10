using Microsoft.EntityFrameworkCore;
using StackUnderflow.Core.Entities;
using StackUnderflow.Data.Models;

namespace StackUnderflow.Data
{
    public class StackUnderflowDbContext : DbContext
    {
        public StackUnderflowDbContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<QuestionTag> QuestionTags { get; set; }
        // Made internal so as not to allow any clients to have access outside the repository/data assembly.
        internal DbSet<LimitsKeyValuePair> LimitsKeyValuePairs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionTag>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });

            modelBuilder.Entity<QuestionTag>()
                .HasOne(qt => qt.Question)
                .WithMany(q => q.QuestionTags)
                .HasForeignKey(qt => qt.QuestionId);

            modelBuilder.Entity<QuestionTag>()
                .HasOne(qt => qt.Tag)
                .WithMany(t => t.QuestionTags)
                .HasForeignKey(qt => qt.TagId);

            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name);

            modelBuilder.Entity<Vote>()
                .Ignore(t => t.TargetId)
                .Ignore(t => t.Target);
        }
    }
}
