using Spry.Table;

namespace Spry.Update
{
    public class SpryUpdateTable<TDto> : SpryTable<TDto>
    {
        private readonly SpryUpdate<TDto> _spry;

        internal SpryUpdateTable(SpryUpdate<TDto> spry,string tableName, string schema)
            : base(tableName, schema)
        {
            _spry = spry;
        }

        public override string Build()
        {
            return _spry.Build();
        }

        internal string BuildImpl()
        {
            string returnString = null;

            if (WhereCondition != null)
            {
                returnString += "WHERE " + WhereCondition.BuildImpl();

                foreach (var andCondition in AndConditions)
                {
                    returnString += " AND " + andCondition.BuildImpl();
                }

                foreach (var orCondition in OrConditions)
                {
                    returnString += " OR " + orCondition.BuildImpl();
                }

            }

            return returnString;
        }
    }
}
