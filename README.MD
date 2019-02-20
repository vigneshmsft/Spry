Spry - a simple CRUD query builder extension for Dapper
=======================================================

[![Build status](https://dev.azure.com/vigneshmsft/Spry/_apis/build/status/Spry-CI)](https://dev.azure.com/vigneshmsft/Spry/_build/latest?definitionId=3)

What is Spry
------------
[Spry](https://www.nuget.org/packages/Spry) is a simple .NET library which allows you to write fluent CRUD queries 
and execute the queries using [Dapper](https://github.com/StackExchange/dapper-dot-net).

Why Spry ?
----------
In one of my projects we decided not to write stored procedures
and keep the queries within code, similar to the popular approach being followed by StackOverflow.
The upside of this approach is that one tends to keep only specific CRUD logic in queries and 
helps to ensure business logics do not get inside your stored procedures.
It is also easier to debug this way when one sees the actual query being executed on the server.
We also decided not to use any ORM like Entity Framework or NHibernate and only use Dapper.
You start to see code like this.

```csharp
public bool InsertCustomer(CustomerDto newCustomer){

const string insertCustomerQuery = @"INSERT INTO dbo.Customer
                                     ([CustomerId],[Name],[PhoneNumber])
                                    VALUES 
                                    (@customerId, @name, @phoneNumber)";

var count = _connection.Execute(insertCustomerQuery, new { @customerId = newCustomer.CustomerId, 
                                                           @name= newCustomer.Name, 
                                                           @phoneNumber = newCustomer.PhoneNumber});

return count == 1;
}
```

This is a relatively straightforward code but beings to break once you start making changes to the `Customer` table.
Believe me changes to the tables are very commom, especially when a project is in its nascent stages.
The moment you decide to rename few columns  or add new columns you need to remember to do a find and replace all occurances of
those strings in your solution, which is not a very efficient way of doing things.
This is precisely why most people decide to use an ORM and let if handle all these hassles for you.
But what if you don't want to use a ORM and at the same time not have this problem ?

What can Spry do ?
-------------------
Simple Insert
```csharp
                var customerInserted = Spry.InsertInto<Customer>(CUSTOMER_TABLE)
                                           .Value(_ => customer.CustomerId)
                                           .Value(_ => customer.Name)
                                           .Value(_ => customer.DateOfBirth)
                                           .ExecuteScalar<int>(connection) > 0;
```
Insert and output inserted identity
```chsarp
                customer.CustomerId = Spry.InsertInto<Customer>(CUSTOMER_TABLE)
                                     .Value(_ => customer.Name)
                                     .Value(_ => customer.DateOfBirth)
                                     .OutputInserted(_ => customer.CustomerId) // or .OutputIdentity()
                                     .ExecuteScalar<int>(connection);
```                                     
Simple Select
```csharp
                return Spry.Select<Customer>()
                           .Column(_ => _.CustomerId)
                           .Column(_ => _.Name)
                           .Column(_ => _.DateOfBirth)
                           .From(CUSTOMER_TABLE).InSchema("dbo")
                           .Where(_ => _.CustomerId).EqualTo(customerId)
                           .Query<Customer>(connection).SingleOrDefault();
```
Select from multiple tables.
```csharp
                        Spry.Select<Customer>()
                            .Column(_ => _.CustomerId, "C")
                            .Column(_ => _.Name)
                            .Column(_ => _.DateOfBirth)
                            .Column(_ => _.Address.City)
                            .Column(_ => _.Address.Country)
                            .Column(_ => _.Address.PostCode)
                            .Column(_ => _.Address.CustomerAddressId)
                            .Column(_ => _.Address.LineOne)
                            .From(CUSTOMER_TABLE).As("C").InSchema("dbo")
                            .InnerJoin(CUSTOMER_ADDRESS_TABLE, "CA").On("CA.CustomerId", "C.CustomerId")
                            .Where(_ => _.CustomerId, "C").EqualTo(customerId)
                            .Query<dynamic>(connection).SingleOrDefault();
```
Update
```csharp
                var rowsUpdated = Spry.Update<Customer>(CUSTOMER_TABLE)
                                      .Set(_ => name)
                                      .Set(_ => dateOfBirth)
                                      .Where(_ => customerId).EqualTo(customerId)
                                      .Execute(connection);

                return rowsUpdated > 0;
```
Delete
```csharp
                var rowsDeleted = Spry.Delete<Customer>()
                                      .From(CUSTOMER_TABLE)
                                      .Where(_ => customerId).EqualTo(customerId)
                                      .Execute(connection);

                return rowsDeleted == 1;
```
All the above types have a `.Build()` method that lets you view the actual generated query.

Installation
------------
Download and try Spry for yourself and let [me](https://twitter.com/N_Vignesh) know how it works or does not work for you. :)
```shell
PM > Install-Package Spry
```
