using System;
using System.Collections.Generic;
using Spry.Table;

namespace Spry.Select
{
    public sealed class SprySelectTable<TDto> : SpryTable<TDto, SprySelectTable<TDto>>
    {
        private readonly SprySelect<TDto> _spry;

        private readonly List<Join<TDto>> _joins;

        public SprySelectTable(SprySelect<TDto> spry, string tableName, string tableAlias = null, string schema = "dbo")
            : base(tableName, schema, tableAlias)
        {
            _joins = new List<Join<TDto>>();
            _spry = spry;
        }

        public Join<TDto> InnerJoin(string tableName, string tableAlias = null, string dbSchema = "dbo")
        {
            var innerJoin = new InnerJoin<TDto>(_spry, this, new SprySelectTable<TDto>(_spry, tableName, tableAlias, dbSchema));
            _joins.Add(innerJoin);
            return innerJoin;
        }

        public Join<TDto> LeftOuterJoin(string tableName, string tableAlias = null, string dbSchema = "dbo")
        {
            var leftOuterJoin = new LeftOuterJoin<TDto>(_spry, this, new SprySelectTable<TDto>(_spry, tableName, tableAlias, dbSchema));
            _joins.Add(leftOuterJoin);
            return leftOuterJoin;
        }

        public Join<TDto> RightOuterJoin(string tableName, string tableAlias = null, string dbSchema = "dbo")
        {
            var rightOuterJoin = new RightOuterJoin<TDto>(_spry, this, new SprySelectTable<TDto>(_spry, tableName, tableAlias, dbSchema));
            _joins.Add(rightOuterJoin);
            return rightOuterJoin;
        }

        public Join<TDto> FullOuterJoin(string tableName, string tableAlias = null, string dbSchema = "dbo")
        {
            var fullOuterJoin = new FullOuterJoin<TDto>(_spry, this, new SprySelectTable<TDto>(_spry, tableName, tableAlias, dbSchema));
            _joins.Add(fullOuterJoin);
            return fullOuterJoin;
        }

        protected override SprySelectTable<TDto> TableImpl
        {
            get { return this; }
        }

        public override string Build()
        {
            return _spry.Build();
        }

        internal string BuildImpl()
        {
            string returnString = BuildImpl(this);

            returnString += Environment.NewLine;

            foreach (var joins in _joins)
            {
                returnString += joins.BuildImpl();
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
