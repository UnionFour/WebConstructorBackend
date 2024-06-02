using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.DBContext
{
    public class AppDBContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "MockDB");

            var user = new User()
            {
                ID = Guid.Parse("01147d68-f0cc-4c9f-ada1-66e923fc382e"),
                Email = "User@gmail.ru",
                passHash = "UserPassword",
                Name = "Григорцев Григорий Григорьевич",
                IsAuthor = false,
                IsCouch = false
            };

            var couch = new User()
            {
                ID = Guid.Parse("6f26ff94-cecf-4144-a495-c2e189e3d03f"),
                Email = "Couch@gmail.ru",
                passHash = "CouchPassword",
                Name = "Стрельцов Аркадий Михайлович",
                IsAuthor = false,
                IsCouch = true
            };

            var organizator = new User()
            {
                ID = Guid.Parse("88dd1000-3204-497c-8280-99cea55a34f5"),
                Email = "Organizator@gmail.ru",
                passHash = "OrganizatorePassword",
                Name = "Главных Денис Борисовчи",
                IsAuthor = true,
                IsCouch = false
            };

            Users.Add(user);
            Users.Add(couch);
            Users.Add(organizator);

            SaveChanges();
        }

        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<BilingAccount>? BilingAccounts { get; set; }
        public virtual DbSet<Gym>? Gyms { get; set; }
        public virtual DbSet<Organization>? Organizations { get; set; }
        public virtual DbSet<SportEvent>? SportEvents { get; set; }
        public virtual DbSet<Training>? Trainings { get; set; }
        public virtual DbSet<UsersTrainings>? UsersTrainings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // user <-> training
            modelBuilder.Entity<UsersTrainings>()
                .HasOne(ut => ut.Training)
                .WithMany(t => t.UsersTrainings)
                .HasForeignKey(ut => ut.TrainingID);
            modelBuilder.Entity<UsersTrainings>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UsersTrainings)
                .HasForeignKey(ut => ut.UserID);
            modelBuilder.Entity<UsersTrainings>()
                .HasKey(ut => new {ut.UserID, ut.TrainingID});
        }
    }
}
