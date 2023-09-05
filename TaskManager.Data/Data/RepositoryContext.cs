using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Domain;

namespace TaskManager.Data.Data
{
    public class RepositoryContext : IdentityDbContext<User, Role, string> 
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Role>(b =>
            {
                b.ToTable("AspNetRoles");
            });
                // modelBuilder.ApplyConfiguration(new TeacherData());
                //modelBuilder.ApplyConfiguration(new StudentData());
            }

        public DbSet<Core.Domain.Task> Tasks { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<TaskFiles> TaskFile { get; set; }
        //public DbSet<Role> Roles { get; set; }
        //public DbSet<Student> Students { get; set; }
    }
}
