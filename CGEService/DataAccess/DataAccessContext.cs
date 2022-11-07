using Microsoft.EntityFrameworkCore;
using Entities;

namespace DataAccess
{
    public class DataAccessContext : DbContext
    {
        public DataAccessContext(DbContextOptions<DataAccessContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserResult> UserResults { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<ProfessionalType> ProfessionalTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>(eb =>
            {
                eb.Property(b => b.RoleId).IsRequired();
                eb.Property(b => b.FirtstName).HasMaxLength(20).IsRequired();
                eb.Property(b => b.SecondName).HasMaxLength(20).IsRequired();
                eb.Property(b => b.Login).HasMaxLength(20).IsRequired();
                eb.Property(b => b.Email).HasMaxLength(60).IsRequired();
                eb.Property(b => b.Birthdate).IsRequired();
                eb.Property(b => b.IsMan).IsRequired();
                eb.Property(b => b.Password).HasMaxLength(30).IsRequired();
            });

            modelBuilder.Entity<User>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<User>()
                .HasOne(e => e.Role)
                .WithMany(e => e.Users)
                .HasForeignKey(k => k.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // User result
            modelBuilder.Entity<UserResult>(eb =>
            {
                eb.Property(b => b.UserId).IsRequired();
                eb.Property(b => b.Date).IsRequired();
                eb.Property(b => b.R).IsRequired();
                eb.Property(b => b.I).IsRequired();
                eb.Property(b => b.A).IsRequired();
                eb.Property(b => b.S).IsRequired();
                eb.Property(b => b.E).IsRequired();
                eb.Property(b => b.C).IsRequired();
            });

            modelBuilder.Entity<UserResult>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<UserResult>()
                .HasOne(e => e.User)
                .WithMany(e => e.UserResults)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Question
            modelBuilder.Entity<Question>(eb => { eb.Property(b => b.Number).IsRequired(); });

            modelBuilder.Entity<Question>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<Question>()
                 .HasOne(e => e.ProfessionFirst)
                 .WithMany(e => e.QuestionFirst)
                 .HasForeignKey(k => k.ProfessionIdFirst)
                 .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Question>()
                 .HasOne(e => e.ProfessionSecond)
                 .WithMany(e => e.QuestionSecond)
                 .HasForeignKey(k => k.ProfessionIdSecond)
                 .OnDelete(DeleteBehavior.NoAction);

            // Profession
            modelBuilder.Entity<Profession>(eb =>
            {
                eb.Property(b => b.Name).HasMaxLength(100).IsRequired();
                eb.Property(b => b.ProfType).IsRequired();
            });

            modelBuilder.Entity<Profession>()
                .HasKey(k => k.Id);

            // Profession Course
            modelBuilder.Entity<ProfessionCourse>()
                .HasKey(k => new { k.ProfessionId, k.CourseId });

            // Role
            modelBuilder.Entity<Role>(eb =>
            {
                eb.Property(b => b.Name).HasMaxLength(10).IsRequired();
            });

            modelBuilder.Entity<Role>()
                .HasKey(k => k.Id);

            // University
            modelBuilder.Entity<University>( eb =>
            {
                eb.Property(b => b.Name).IsRequired();
                eb.Property(b => b.Info).IsRequired();
                eb.Property(b => b.Contacts).IsRequired();
            });

            modelBuilder.Entity<University>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<University>()
                .HasOne(e => e.User)
                .WithOne(e => e.University)
                .HasForeignKey<University>(e => e.UserId);

            // Department
            modelBuilder.Entity<Department>(eb =>
            {
                eb.Property(b => b.Name).IsRequired();
                eb.Property(b => b.Contacts).IsRequired();
                eb.Property(b => b.Info).IsRequired();
                eb.Property(b => b.UniversityId).IsRequired();
            });

            modelBuilder.Entity<Department>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<Department>()
                .HasOne(e => e.University)
                .WithMany(e => e.Departments)
                .HasForeignKey(e => e.UniversityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Course
            modelBuilder.Entity<Course>(eb => 
            {
                eb.Property(b => b.Name).IsRequired();
                eb.Property(b => b.Code).IsRequired();
                eb.Property(b => b.DepartmentId).IsRequired();
                eb.Property(b => b.Exams).IsRequired();
            });

            modelBuilder.Entity<Course>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<Course>()
                .HasOne(e => e.Department)
                .WithMany(e => e.Courses)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProfessionalType
            modelBuilder.Entity<ProfessionalType>()
                .HasKey(k => k.Id);
        }
    }
}
