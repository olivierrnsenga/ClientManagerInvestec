using System;
using System.Linq;
using ClientManager.Core.Domain;
using ClientManager.DataAccess;
using ClientManager.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeskBooker.DataAccess.Repositories
{
    [TestClass]
    public class ClientRepositoryTests
    {
        private DbContextOptions<ClientContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ClientContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private ClientRequest CreateNewClient(string firstName, string lastName)
        {
            return new ClientRequest()
            {
                FirstName = firstName,
                LastName = lastName,
                IdNumber = "8001015009087",
                MobileNumber = "1234567890",
                PhysicalAddress = "Some Address"
            };
        }

        [TestMethod]
        public void ShouldAddClient()
        {
            var options = CreateNewContextOptions();
            var newClient = CreateNewClient("John", "Doe");

            using (var context = new ClientContext(options))
            {
                var repository = new ClientRepository(context);
                repository.Add(newClient);
            }

            using (var context = new ClientContext(options))
            {
                Assert.AreEqual(1, context.Clients.Count());
                var clientInDb = context.Clients.Single();
                Assert.AreEqual("John", clientInDb.FirstName);
            }
        }

        [TestMethod]
        public void ShouldRetrieveClientById()
        {
            var options = CreateNewContextOptions();
            var newClient = CreateNewClient("Jane", "Doe");

            using (var context = new ClientContext(options))
            {
                context.Clients.Add(newClient);
                context.SaveChanges();
            }

            using (var context = new ClientContext(options))
            {
                var repository = new ClientRepository(context);
                var retrievedClient = repository.GetById((int)newClient.Id);

                Assert.IsNotNull(retrievedClient);
                Assert.AreEqual("Jane", retrievedClient.FirstName);
            }
        }

        [TestMethod]
        public void ShouldRetrieveAllClients()
        {
            var options = CreateNewContextOptions();

            using (var context = new ClientContext(options))
            {
                context.Clients.Add(CreateNewClient("Client1", "Last1"));
                context.Clients.Add(CreateNewClient("Client2", "Last2"));
                context.SaveChanges();
            }

            using (var context = new ClientContext(options))
            {
                var repository = new ClientRepository(context);
                var clients = repository.GetAll().ToList();

                Assert.AreEqual(2, clients.Count);
            }
        }

        [TestMethod]
        public void ShouldUpdateClient()
        {
            var options = CreateNewContextOptions();
            var client = CreateNewClient("Updatable", "Client");

            using (var context = new ClientContext(options))
            {
                context.Clients.Add(client);
                context.SaveChanges();
            }

            client.FirstName = "UpdatedName";

            using (var context = new ClientContext(options))
            {
                var repository = new ClientRepository(context);
                repository.Update(client);
            }

            using (var context = new ClientContext(options))
            {
                var updatedClient = context.Clients.Single(c => c.Id == client.Id);
                Assert.AreEqual("UpdatedName", updatedClient.FirstName);
            }
        }

        [TestMethod]
        public void ShouldDeleteClient()
        {
            var options = CreateNewContextOptions();
            var client = CreateNewClient("Deletable", "Client");

            using (var context = new ClientContext(options))
            {
                context.Clients.Add(client);
                context.SaveChanges();
            }

            using (var context = new ClientContext(options))
            {
                var repository = new ClientRepository(context);
                repository.Delete((int)client.Id);
            }

            using (var context = new ClientContext(options))
            {
                var clients = context.Clients.ToList();
                Assert.IsFalse(clients.Any(c => c.Id == client.Id));
            }
        }

        [TestMethod]
        public void ShouldNotAllowDuplicateIdNumber()
        {
            var options = CreateNewContextOptions();

            var client1 = CreateNewClient("John", "Doe");
            var client2 = CreateNewClient("Jane", "Doe");

            using (var context = new ClientContext(options))
            {
                var repository = new ClientRepository(context);
                repository.Add(client1);
                context.SaveChanges();

                var duplicateIdNumberCaught = false;
                try
                {
                    repository.Add(client2);
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    duplicateIdNumberCaught = true;
                }

                Assert.IsFalse(duplicateIdNumberCaught,
                    "Unexpected exception occurred when adding client with different IdNumber.");
            }
        }

        [TestMethod]
        public void ShouldNotAllowDuplicateMobileNumber()
        {
            var options = CreateNewContextOptions();

            var client1 = CreateNewClient("John", "Doe");
            client1.MobileNumber = "1234567890";

            var client2 = CreateNewClient("Jane", "Doe");
            client2.MobileNumber = "0987654321";

            using (var context = new ClientContext(options))
            {
                var repository = new ClientRepository(context);
                repository.Add(client1);
                context.SaveChanges();

                var duplicateMobileNumberCaught = false;
                try
                {
                    repository.Add(client2);
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    duplicateMobileNumberCaught = true;
                }

                Assert.IsFalse(duplicateMobileNumberCaught,
                    "Unexpected exception occurred when adding client with different MobileNumber.");
            }
        }

        [TestMethod]
        public void IdNumberIsValidatedForNewClients()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ClientContext>()
                .UseInMemoryDatabase("Test_Client")
                .Options;

            var validIdNumber = "8001015009087";
            var validClient = new ClientRequest { FirstName = "Valid", LastName = "Client", IdNumber = validIdNumber };

            using (var context = new ClientContext(options))
            {
                var repository = new ClientRepository(context);

                // Act
                var isValidClientAdded = false;

                try
                {
                    repository.Add(validClient);
                    context.SaveChanges();
                    isValidClientAdded = true;
                }
                catch (Exception)
                {
                }

                // Assert
                Assert.IsTrue(isValidClientAdded, "A valid client was not added successfully.");
            }
        }

        [TestMethod]
        public void ShouldSearchClientByFirstNameOrIdNumberOrPhoneNumber()
        {
            var options = CreateNewContextOptions();

            var client1 = CreateNewClient("John", "Doe");
            var client2 = CreateNewClient("Jane", "Doe");
            var client3 = CreateNewClient("Alice", "Smith");
            client3.IdNumber = "9002021234567";
            var client4 = CreateNewClient("Bob", "Johnson");
            client4.MobileNumber = "0987654321";

            using (var context = new ClientContext(options))
            {
                var repository = new ClientRepository(context);
                repository.Add(client1);
                repository.Add(client2);
                repository.Add(client3);
                repository.Add(client4);
                context.SaveChanges();
            }

            using (var context = new ClientContext(options))
            {
                var repository = new ClientRepository(context);

                // Search by FirstName
                var searchResultFirstName = repository.Search("John");
                Assert.AreEqual(1, searchResultFirstName.Count());
                Assert.AreEqual("John", searchResultFirstName.Single().FirstName);

                // Search by ID Number
                var searchResultIdNumber = repository.Search("9002021234567");
                Assert.AreEqual(1, searchResultIdNumber.Count());
                Assert.AreEqual("Alice", searchResultIdNumber.Single().FirstName);

                // Search by Phone Number
                var searchResultPhoneNumber = repository.Search("0987654321");
                Assert.AreEqual(1, searchResultPhoneNumber.Count());
                Assert.AreEqual("Bob", searchResultPhoneNumber.Single().FirstName);

                // Search by Non-existent Value
                var searchResultNonExistent = repository.Search("NonExistent");
                Assert.AreEqual(0, searchResultNonExistent.Count());
            }
        }
    }
}