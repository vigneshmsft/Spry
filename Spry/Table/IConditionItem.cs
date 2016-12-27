using System;
using System.Linq.Expressions;
using Spry.Where;

namespace Spry.Table
{
    public interface IConditionItem<TDto>
    {
        Where<TDto, TProperty> Where<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string colPrefix = null);
        Where<TDto, TProperty> AndWhere<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string colPrefix = null);
        Where<TDto, TProperty> OrWhere<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string colPrefix = null);

        Where<TDto, TProperty> Where<TProperty>(string columnName);
        Where<TDto, TProperty> OrWhere<TProperty>(string columnName);
        Where<TDto, TProperty> AndWhere<TProperty>(string columnName);
    }
}
