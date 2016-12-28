using Spry.Table;

namespace Spry.Delete
{
    public class SpryDelete<TDto>
    {
        internal SpryDelete() { }

        private SpryDeleteTable<TDto> _table;

        public SpryDeleteTable<TDto> From(string tableName, string schema = "dbo")
        {
            _table = new SpryDeleteTable<TDto>(this, tableName, schema);
            return _table;
        }

        public string Build()
        {
            return "DELETE FROM " + _table.BuildImpl();
        }
    }
}
