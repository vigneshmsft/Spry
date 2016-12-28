using Spry.Where;
using System;
using System.Linq.Expressions;

namespace Spry.Table
{
    public abstract partial class SpryTable<TDto, TTable> where TTable : SpryTable<TDto, TTable>
    {
        public Where<TDto, TProperty, TTable> Where<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string colPrefix = null)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            if (!string.IsNullOrWhiteSpace(colPrefix))
            {
                columnName = colPrefix + "." + columnName;
            }
            var where = new Where<TDto, TProperty, TTable>(TableImpl, Parameters, columnName);
            WhereCondition = where;
            return where;
        }

        public Where<TDto, TProperty, TTable> Where<TProperty>(string columnName)
        {
            var where = new Where<TDto, TProperty, TTable>(TableImpl, Parameters, columnName);
            WhereCondition = where;
            return where;
        }

        public Where<TDto, TProperty, TTable> AndWhere<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string colPrefix = null)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            if (!string.IsNullOrWhiteSpace(colPrefix)) columnName = colPrefix + "." + columnName;
            var andWhere = new Where<TDto, TProperty, TTable>(TableImpl, Parameters, columnName);
            AndConditions.Add(andWhere);
            return andWhere;
        }

        public Where<TDto, TProperty, TTable> AndWhere<TProperty>(string columnName)
        {
            var andWhere = new Where<TDto, TProperty, TTable>(TableImpl, Parameters, columnName);
            AndConditions.Add(andWhere);
            return andWhere;
        }

        public Where<TDto, TProperty, TTable> OrWhere<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string colPrefix = null)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            if (!string.IsNullOrWhiteSpace(colPrefix)) columnName = colPrefix + "." + columnName;
            var orWhere = new Where<TDto, TProperty, TTable>(TableImpl, Parameters, columnName);
            OrConditions.Add(orWhere);
            return orWhere;
        }

        public Where<TDto, TProperty, TTable> OrWhere<TProperty>(string columnName)
        {
            var orWhere = new Where<TDto, TProperty, TTable>(TableImpl, Parameters, columnName);
            OrConditions.Add(orWhere);
            return orWhere;
        }
    }
}
