using Microsoft.EntityFrameworkCore;

namespace JWT.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Person>().HasData
        //    (
        //        new Person {Id = 1, Login = "raz@gmail.com", Password = "123", Role = "admin" },
        //        new Person {Id = 2, Login = "dva@gmail.com", Password = "345", Role = "user" }
        //    );
        //}
    }
}