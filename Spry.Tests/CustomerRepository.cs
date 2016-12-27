using System;
using System.Linq;
using Spry.Tests.Dto;

namespace Spry.Tests
{
    public class CustomerRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        private const string CUSTOMER_TABLE = "Customer";
        private const string CUSTOMER_ADDRESS_TABLE = "CustomerAddress";

        public CustomerRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int Create(string name, DateTime dateOfBirth)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var customer = new Customer
                {
                    DateOfBirth = dateOfBirth,
                    Name = name
                };

                customer.CustomerId = Spry.InsertInto<Customer>(CUSTOMER_TABLE)
                                     .Value(_ => customer.Name)
                                     .Value(_ => customer.DateOfBirth)
                                     .OutputIdentity()
                                     .ExecuteScalar<int>(connection);

                return customer.CustomerId;
            }
        }

        public int CreateAddress(CustomerAddress address)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                address.CustomerAddressId = Spry.InsertInto<CustomerAddress>(CUSTOMER_ADDRESS_TABLE)
                                                .Value(_ => address.CustomerId)
                                                .Value(_ => address.LineOne)
                                                .Value(_ => address.City)
                                                .Value(_ => address.Country)
                                                .Value(_ => address.PostCode)
                                                .ExecuteScalar<int>(connection);

                return address.CustomerAddressId;
            }
        }

        public bool Update(int customerId, string name, DateTime dateOfBirth)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var rowsUpdated = Spry.Update<Customer>(CUSTOMER_TABLE)
                                      .Set(_ => name)
                                      .Set(_ => dateOfBirth)
                                      .Where(_ => customerId).EqualTo(customerId)
                                      .Execute(connection);

                return rowsUpdated > 0;
            }
        }

        public bool Delete(int customerId)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var rowsDeleted = Spry.Delete<Customer>()
                    .From(CUSTOMER_TABLE)
                    .Where(_ => customerId).EqualTo(customerId)
                    .Execute(connection);

                return rowsDeleted == 1;
            }
        }

        public Customer ReadComplete(int customerId)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var customer = Spry.Select<Customer>()
                           .Column(_ => _.CustomerId, "C")
                           .Column(_ => _.Name)
                           .Column(_ => _.DateOfBirth)
                           .Column(_ => _.Address.City)
                           .Column(_ => _.Address.Country)
                           .Column(_ => _.Address.PostCode)
                           .Column(_ => _.Address.CustomerAddressId)
                           .Column(_ => _.Address.LineOne)
                           .From(CUSTOMER_TABLE).As("C")
                           .InnerJoin(CUSTOMER_ADDRESS_TABLE, "CA").On("CA.CustomerId", "C.CustomerId")
                           .Where(_ => _.CustomerId, "C").EqualTo(customerId)
                           .Query<dynamic>(connection).SingleOrDefault();

                return ToCustomer(customer);
            }
        }

        public Customer Read(int customerId)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                return Spry.Select<Customer>()
                           .Column(_ => _.CustomerId, "C")
                           .Column(_ => _.Name, "C")
                           .Column(_ => _.DateOfBirth, "C")
                           .From(CUSTOMER_TABLE).As("C").InSchema("dbo")
                           .Where(_ => _.CustomerId, "C").EqualTo(customerId)
                           .Query<Customer>(connection).SingleOrDefault();
            }
        }

        private Customer ToCustomer(dynamic customerRow)
        {
            if (customerRow == null)
                return null;

            var customer = new Customer
            {
                CustomerId = customerRow.CustomerId,
                Name = customerRow.Name,
                DateOfBirth = customerRow.DateOfBirth
            };

            if (customerRow.CustomerAddressId > 0)
            {
                customer.Address = new CustomerAddress
                {
                    City = customerRow.City,
                    PostCode = customerRow.PostCode,
                    Country = customerRow.Country,
                    LineOne = customerRow.LineOne,
                    CustomerAddressId = customerRow.CustomerAddressId,
                    CustomerId = customerRow.CustomerId
                };
            }

            return customer;
        }
    }
}
