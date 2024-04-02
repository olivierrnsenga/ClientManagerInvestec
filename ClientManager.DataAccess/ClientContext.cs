using ClientManager.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace ClientManager.DataAccess
{
    public class ClientContext : DbContext
    {
        public ClientContext(DbContextOptions<ClientContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    MobileNumber = "0760471757",
                    IdNumber = "8608126377180",
                    PhysicalAddress = "123 Main St"
                }

            );
        }
    }
}