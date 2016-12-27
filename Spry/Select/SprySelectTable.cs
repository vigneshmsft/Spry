using System;
using System.Collections.Generic;
using Spry.Table;

namespace Spry.Select
{
    public sealed class SprySelectTable<TDto> : SpryTable<TDto>
    {
        private readonly SprySelect<TDto> _spry;

        private readonly List<InnerJoin<TDto>> _innerJoin;

        public SprySelectTable(SprySelect<TDto> spry, string tableName, string tableAlias = null, string schema = "dbo")
            : base(tableName, schema, tableAlias)
        {
            _innerJoin = new List<InnerJoin<TDto>>();
            _spry = spry;
        }

        public InnerJoin<TDto> InnerJoin(string tableName, string tableAlias = null, string dbSchema = "dbo")
        {
            var innerJoin = new InnerJoin<TDto>(_spry, this, new SprySelectTable<TDto>(_spry, tableName, tableAlias, dbSchema));
            _innerJoin.Add(innerJoin);
            return innerJoin;
        }

        public new SprySelectTable<TDto> As(string tableAlias)
        {
            Alias = tableAlias;
            return this;
        }

        public override string Build()
        {
            return _spry.Build();
        }

        internal string BuildImpl()
        {
            string returnString = BuildImpl(this);

            returnString += Environment.NewLine;

            foreach (var innerJoin in _innerJoin)
            {
                returnString += innerJoin.BuildImpl();
            }

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
