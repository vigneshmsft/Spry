using System;
using System.Linq.Expressions;
using Spry.Where;

namespace Spry.Table
{
    public interface IConditionItem<TDto, TTable> where TTable : SpryTable<TDto, TTable>
    {
        Where<TDto, TProperty, TTable> Where<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string colPrefix = null);
        Where<TDto, TProperty, TTable> AndWhere<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string colPrefix = null);
        Where<TDto, TProperty, TTable> OrWhere<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string colPrefix = null);

        Where<TDto, TProperty, TTable> Where<TProperty>(string columnName);
        Where<TDto, TProperty, TTable> OrWhere<TProperty>(string columnName);
        Where<TDto, TProperty, TTable> AndWhere<TProperty>(string columnName);
    }
}
