using System;
using Spry.Table;

namespace Spry.Delete
{
    public class SpryDeleteTable<TDto> : SpryTable<TDto>
    {
        private readonly SpryDelete<TDto> _spry;

        internal SpryDeleteTable(SpryDelete<TDto> spry, string tableName, string schema = "dbo")
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
            string returnString = BuildImpl(this);

            returnString += Environment.NewLine;

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

            if (!string.IsNullOrWhiteSpace(ExtraQuery))
            {
                returnString += ExtraQuery;
            }

            return returnString;
        }
    }
}
