using Spry.Table;

namespace Spry.Insert
{
    internal class SpryInsertTable<TDto> : SpryTable<TDto>
    {
        private readonly SpryInsert<TDto> _spry;

        public SpryInsertTable(string tableName, string schema, SpryInsert<TDto> spry)
            : base(tableName, schema)
        {
            _spry = spry;
        }

        public override string Build()
        {
            return _spry.Build();
        }
    }
}
