using Microsoft.EntityFrameworkCore;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Data
{
    public class StackUnderflowDbContext : DbContext
    {
        public StackUnderflowDbContext(DbContextOptions<StackUnderflowDbContext> options)
            : base(options)
        { }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}