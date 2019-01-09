using System;
using System.Data.SqlClient;
using NUnit.Framework;
using Spry.Tests.Dto;

namespace Spry.Tests
{
    [TestFixture]
    public class RepositoryTest : BaseTest
    {
        private readonly TestConnectionFactory _connectionFactory = new TestConnectionFactory();
        private CustomerRepository _customerRepository;

        [SetUp]
        public void TestMethodInitialize()
        {
            _customerRepository = new CustomerRepository(_connectionFactory);
        }

        [Test]
        public void Insert_CheckOutputIdentity()
        {
            var customer = new Customer
            {
                DateOfBirth = DateTime.Today,
                Name = "John Doe"
            };

            customer.CustomerId = _customerRepository.Create(customer.Name, customer.DateOfBirth);

            Assert.IsTrue(customer.CustomerId > 0);
        }

        [Test]
        public void InnerJoin_CheckInnerJoin()
        {
            var customer = new Customer
            {
                CustomerId = _customerRepository.Create("John Doe", DateTime.Today)
            };

            customer.Address = new CustomerAddress
            {
                CustomerId = customer.CustomerId,
                City = "MyCity",
                Country = "Isle of Man",
                LineOne = "Street of Man",
                PostCode = "PostCode"
                
            };

            customer.Address.CustomerAddressId = _customerRepository.CreateAddress(customer.Address);

            var savedCustomer = _customerRepository.ReadComplete(customer.CustomerId);

            Assert.IsNotNull(savedCustomer.Address);

            Assert.AreEqual(customer.Address.City, savedCustomer.Address.City);
            Assert.AreEqual(customer.Address.PostCode, savedCustomer.Address.PostCode);
            Assert.AreEqual(customer.Address.Country, savedCustomer.Address.Country);
            Assert.AreEqual(customer.Address.LineOne, savedCustomer.Address.LineOne);
        }

        [Test]
        public void Insert_CheckOutputInserted()
        {
            var customer = new Customer
            {
                DateOfBirth = DateTime.Today,
                Name = "John Doe"
            };

            using (var connection = CreateConnection())
            {
                customer.CustomerId = Spry.InsertInto<Customer>(CUSTOMER_TABLE)
                                     .Value(_ => customer.Name)
                                     .Value(_ => customer.DateOfBirth)
                                     .OutputInserted(_ => customer.CustomerId)
                                     .ExecuteScalar<int>(connection);
            }

            Assert.IsTrue(customer.CustomerId > 0);
        }

        [Test]
        public void Update_CheckUpdated()
        {
            var customer = new Customer
            {
                CustomerId = _customerRepository.Create("John", DateTime.Today),
                DateOfBirth = DateTime.Today.AddDays(-10),
                Name = "Mr John Doe"
            };

            var updated = _customerRepository.Update(customer.CustomerId, customer.Name, customer.DateOfBirth);

            Assert.IsTrue(updated);

            var updatedCustomer = _customerRepository.Read(customer.CustomerId);

            Assert.AreEqual(customer.DateOfBirth, updatedCustomer.DateOfBirth);
            Assert.AreEqual(customer.Name, updatedCustomer.Name);
        }

        [Test]
        public void UpdateColumInWhereClause_CheckUpdated()
        {
            var customer = new Customer
            {
                CustomerId = _customerRepository.Create("John", DateTime.Today),
            };

            customer = _customerRepository.Read(customer.CustomerId);

            const string newName = "John Doe";
            var updated = _customerRepository.UpdateByName("John", newName);

            Assert.IsTrue(updated);

            var updatedCustomer = _customerRepository.Read(customer.CustomerId);

            Assert.AreEqual(customer.DateOfBirth, updatedCustomer.DateOfBirth);
            Assert.AreEqual(newName, updatedCustomer.Name);
        }

        [Test]
        public void DeleteCustomer_CheckDeleted()
        {
            var customer = new Customer
            {
                DateOfBirth = DateTime.Today,
                Name = "John Doe",
                CustomerId = _customerRepository.Create("John Doe", DateTime.Today)
            };

            var deleted = _customerRepository.Delete(customer.CustomerId);

            Assert.IsTrue(deleted);
        }

        [Test]
        public void CleanUp()
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                Spry.Delete().From("CustomerAddress").Execute(connection);
                Spry.Delete().From("Customer").Execute(connection);
            }
        }

        [Test]
        public void SqlInjection_InWhereColumnCheckThrowsException()
        {
            var customer = new Customer
            {
                CustomerId = _customerRepository.Create("John", DateTime.Today),
            };

            customer = _customerRepository.Read(customer.CustomerId);

            Assert.Throws(typeof(SqlException), () => _customerRepository.UpdateSqlInjection(customer.CustomerId, "Customer"));
        }

        [Test]
        public void SqlInjection_InParameterValueCheckThrowsException()
        {
            var customer = new Customer
            {
                CustomerId = _customerRepository.Create("John", DateTime.Today),
            };

            customer = _customerRepository.Read(customer.CustomerId);

            Assert.Throws(typeof(SqlException), () => _customerRepository.Update(customer.CustomerId, ";DELETE FROM dbo.Customer; /*", DateTime.Today));
        }
    }
}