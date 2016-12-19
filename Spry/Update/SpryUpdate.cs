using System;

namespace Spry.Update
{
    public class SpryUpdate<TDto>
    {
        private string _table;
        private string _dbSchema;
        private UpdateValue<TDto> _value;
        private SpryUpdateTable<TDto> _spryTable;

        internal SpryUpdate()
        {
        }

        public static UpdateValue<TDto> Update(string tableName, string dbSchema)
        {
            var spry = new SpryUpdate<TDto>
            {
                _table = tableName,
                _dbSchema = dbSchema,
            };

            spry._spryTable = new SpryUpdateTable<TDto>(spry, tableName, dbSchema);
            return spry._value = new UpdateValue<TDto>(spry, spry._spryTable);
        }

        public string Build()
        {
            return "UPDATE " + _dbSchema + "." + _table + Environment.NewLine + "SET" + Environment.NewLine
                + _value.BuildImpl()
                + _spryTable.BuildImpl();
        }
    }


}