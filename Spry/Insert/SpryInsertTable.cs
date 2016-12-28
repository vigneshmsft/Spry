using Spry.Table;

namespace Spry.Insert
{
    internal class SpryInsertTable<TDto> : SpryTable<TDto, SpryInsertTable<TDto>>
    {
        private readonly SpryInsert<TDto> _spry;

        public SpryInsertTable(string tableName, string schema, SpryInsert<TDto> spry)
            : base(tableName, schema)
        {
            _spry = spry;
        }

        protected override SpryInsertTable<TDto> TableImpl
        {
            get { return this; }
        }

        public override string Build()
        {
            var returnString = _spry.Build();

            if (string.IsNullOrWhiteSpace(ExtraQuery))
            {
                returnString += ExtraQuery;
            }

            return returnString;
        }
    }
}