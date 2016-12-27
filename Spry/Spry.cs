using Spry.Delete;
using Spry.Insert;
using Spry.Select;
using Spry.Update;

namespace Spry
{
    public static class Spry
    {
        public static SprySelectColumn<TDto> Select<TDto>()
        {
            return SprySelect<TDto>.Select();
        }

        public static SprySelectColumn<dynamic> Select()
        {
            return SprySelect<dynamic>.Select();
        }

        public static InsertValue<TDto> InsertInto<TDto>(string tableName, string dbSchema = "dbo")
        {
            return SpryInsert<TDto>.Insert(tableName, dbSchema);
        }

        public static InsertValue<dynamic> InsertInto(string tableName, string dbSchema = "dbo")
        {
            return SpryInsert<dynamic>.Insert(tableName, dbSchema);
        }

        public static UpdateValue<TDto> Update<TDto>(string tableName, string dbSchema = "dbo")
        {
            return SpryUpdate<TDto>.Update(tableName, dbSchema);
        }

        public static UpdateValue<dynamic> Update(string tableName, string dbSchema = "dbo")
        {
            return SpryUpdate<dynamic>.Update(tableName, dbSchema);
        }

        public static SpryDelete<dynamic> Delete()
        {
            return new SpryDelete<dynamic>();
        }

        public static SpryDelete<TDto> Delete<TDto>()
        {
            return new SpryDelete<TDto>();
        }
    }
}