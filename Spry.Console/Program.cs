namespace Spry.Console
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            var myDto = new MyDto { Id = 1, IsDeleted = true };

            Spry.Select<MyDto>().Column(_ => myDto.Id).Column(_ => myDto.IsDeleted).From("tt")
                .InnerJoin("table2", "audit").On("c1", "d1")
                .InnerJoin("table3", "audit").On("c2", "d2")
                .InnerJoin("table4", "audit").On("c4", "d2")
                .InSchema("review")
                .Where(_ => myDto.Id).EqualTo(1)
                .AndWhere(_ => myDto.Id).InBetween(1, 10)
                .AndWhere(_ => myDto.Id).GreaterThan(5)
                .Build();

            Spry.InsertInto("tableOne", "review")
                .Value("One", 1)
                .Value(_ => myDto.Id)
                .OutputIdentity()
                .Execute(null);

            Spry.Update("tableOne")
                .Set(_ => myDto.Id)
                .Where<int>("id").EqualTo(1)
                .Execute(null);

            Console.ReadLine();
        }
    }

    public class MyDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Today { get; set; }

        public bool IsDeleted { get; set; }
    }

    public enum EvidenceStatus : uint
    {
        NotApplicable = 0,

        Requested = 1,

        Awaiting = 2,

        Received = 3
    }
}