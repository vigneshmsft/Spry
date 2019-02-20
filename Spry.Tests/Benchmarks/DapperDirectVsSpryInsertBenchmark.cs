using System;
using Dapper;
using NUnit.Framework;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Spry.Tests.Dto;


namespace Spry.Tests.Benchmarks
{
    [TestFixture]
    public class DapperDirectVsSpryBenchmarkRunner
    {
        [Test]
        public void RunCustomerRepositoryBenchmarks()
        {
            var benchmark = BenchmarkRunner.Run<DapperDirectVsSpryInsertBenchmark>();
        }
    }

    public class DapperDirectVsSpryInsertBenchmark
    {
        private readonly TestConnectionFactory _connectionFactory = new TestConnectionFactory();
        private readonly CustomerRepository _customerRepository;

        public DapperDirectVsSpryInsertBenchmark()
        {
            _customerRepository = new CustomerRepository(_connectionFactory);
        }

        [Benchmark]
        public void Spry_Insert_CheckOutputIdentity()
        {
            var customer = new Customer
            {
                DateOfBirth = DateTime.Today,
                Name = "John Doe"
            };

            customer.CustomerId = _customerRepository.Create(customer.Name, customer.DateOfBirth);

            Assert.IsTrue(customer.CustomerId > 0);
        }

        [Benchmark]
        public void Dapper_Insert_CheckOutputIdentity()
        {
            var customer = new Customer
            {
                DateOfBirth = DateTime.Today,
                Name = "John Doe"
            };

            using (var connection = _connectionFactory.CreateConnection())
            {
                customer.CustomerId = connection.ExecuteScalar<int>(@"INSERT INTO dbo.Customer
                                                                        (Name, DateofBirth) 
                                                                 OUTPUT Inserted.CustomerId
                                                                 VALUES (@Name, @DateOfBirth);", customer);
            }

            Assert.IsTrue(customer.CustomerId > 0);
        }
    }
}
