using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Entity;

namespace ProjectManager.Connection
{
    public class DatabaseContext : IdentityDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public virtual DbSet<Classs> Classs { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Intern> Intern { get; set; }

        public virtual DbSet<ProjectList> ProjectList { get; set; }
        public virtual DbSet<SchoolYear> SchoolYear { get; set; }
        public virtual DbSet<Specialized> Specialized { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Teacher> Teacher { get; set; }
        public virtual DbSet<TrainingSystem> TrainingSystem { get; set; }
        public virtual DbSet<UserManagement> UserManagement { get; set; }

    }
}
