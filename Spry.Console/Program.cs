namespace Spry.Console
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            //var myquery = Spry.Select<MyDto>()
            //    .Column(_ => "EvidenceStatusId")
            //    .Column(_ => _.Name)
            //    .From("MyTable", "MT")
            //    .In("Review")
            //    .InnerJoin("MylookupTable")
            //    .On("a", "b")
            //    .GetQuery();

            var myquery = Spry.Select().Column("EvidenceStatusId").From("ExpenditureEvidence").Where("ExpenditureId = @expenditureId");

            int? nullable = 0;
            var myDto = new MyDto() { Id = 1,  IsDeleted = true };

            var myquery2 = Spry.Insert()
                                .Column(_ => nullable)
                                .Column(_ => myDto.Name)
                                .Column(_ => myDto.Today)
                                .Into("MyTable")
                               .GetQuery();

            var myquery3 = Spry.Update<MyDto>()
                               .Set(_ => myDto.IsDeleted)
                               .In("MyTable").In("Review").Where("WHERE 1 = 1").GetQuery();

            DateTime now = DateTime.Now;

            DateTime ModifiedOn = now;
            DateTime CreatedOn = now;
            int ModifiedBy = 0;
            int expenditureId = 1;
            EvidenceStatus status = EvidenceStatus.Awaiting;
            int evidenceStatusId = (int)status;
            var q4 = Spry.Insert()
                .Column(_ => expenditureId)
                .Column(_ => evidenceStatusId)
                .Column(_ => CreatedOn)
                .Column(_ => ModifiedOn)
                .Column(_ => CreatedOn)
                .Into("ExpenditureEvidence").GetQuery();

            Console.WriteLine(myquery);
            Console.WriteLine();

            Console.WriteLine(myquery2);
            Console.WriteLine();

            Console.WriteLine(myquery3);
            Console.WriteLine();

            Console.WriteLine(q4);
            Console.WriteLine();

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