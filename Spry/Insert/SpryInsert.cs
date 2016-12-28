using System;
using Spry.Table;

namespace Spry.Insert
{
    public class SpryInsert<TDto>
    {
        private InsertValue<TDto> _value;
        private SpryInsertTable<TDto> _spryTable;
        private string _table;
        private string _dbSchema;

        private SpryInsert()
        {

        }

        public static InsertValue<TDto> Insert(string tableName, string dbSchema)
        {
            var spry = new SpryInsert<TDto>
            {
                _table = tableName,
                _dbSchema = dbSchema,
            };

            spry._spryTable = new SpryInsertTable<TDto>(tableName, dbSchema, spry);
            return spry._value = new InsertValue<TDto>(spry, spry._spryTable);
        }

        public string Build()
        {
            return "INSERT INTO " + _dbSchema + "." + _table + Environment.NewLine + _value.BuildImpl();
        }
    }
}
