using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.DBContext
{
    public class AppDBContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) 
        {
            var user = new User()
            {
                ID = Guid.NewGuid(),
                Email = "comonUser@gmail.ru",
                passHash = "secretUserPassword",
                Name = "Григорцев Григорий Григорьевич",
                IsAuthor = false,
                IsCouch = false,
            };

            Users.Add(user);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "MockDB");
        }

        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<BilingAccount>? BilingAccounts { get; set; }
        public virtual DbSet<Gym>? Gyms { get; set; }
        public virtual DbSet<Organization>? Organizations { get; set; }
        public virtual DbSet<SportEvent>? SportEvents { get; set; }
        public virtual DbSet<Training>? Trainings { get; set; }
        public virtual DbSet<UsersTrainings>? UsersTrainings { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<>
        }
    }
}
