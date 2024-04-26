using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.DBContext
{
    public class DBContext : Microsoft.EntityFrameworkCore.DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "MockDB");
        }

        public DbSet<User> Users { get; set; }
    }
}
