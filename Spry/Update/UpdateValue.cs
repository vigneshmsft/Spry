using System.Linq.Expressions;
using System;
using System.Text;
using Spry.Table;
using Spry.Where;

namespace Spry.Update
{
    public class UpdateValue<TDto> : IConditionItem<TDto, SpryUpdateTable<TDto>>
    {
        private readonly SpryUpdate<TDto> _spry;
        private readonly SpryUpdateTable<TDto> _table;
        private readonly StringBuilder _updateBuilder;

        public UpdateValue(SpryUpdate<TDto> spry, SpryUpdateTable<TDto> table)
        {
            _spry = spry;
            _table = table;
            _updateBuilder = new StringBuilder();
        }

        public UpdateValue<TDto> Set<TProperty>(Expression<Func<TDto, TProperty>> valueExpression)
        {
            var columnName = SpryExpression.GetColumnName(valueExpression);
            var value = SpryExpression.GetColumnValue(valueExpression);
            SetValueImpl(columnName, value);
            return this;
        }

        public UpdateValue<TDto> Set(string columnName, object value)
        {
            SetValueImpl(columnName, value);
            return this;
        }

        private void SetValueImpl(string columnName, object value)
        {
            _table.AddParameter(columnName, value);
            _updateBuilder.AppendLine(columnName + " = @" + columnName + ", ");
        }

        public string Build()
        {
            return _spry.Build();
        }

        internal string BuildImpl()
        {
            RemoveTrailingCommas();
            return _updateBuilder + Environment.NewLine;
        }

        private void RemoveTrailingCommas()
        {
            _updateBuilder.Length -= 4;
        }

        public Where<TDto, TProperty, SpryUpdateTable<TDto>> Where<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string colPrefix = null)
        {
            return _table.Where(columnExpression, colPrefix);
        }

        public Where<TDto, TProperty, SpryUpdateTable<TDto>> AndWhere<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string colPrefix = null)
        {
            return _table.AndWhere(columnExpression, colPrefix);
        }

        public Where<TDto, TProperty, SpryUpdateTable<TDto>> OrWhere<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string colPrefix = null)
        {
            return _table.OrWhere(columnExpression, colPrefix);
        }

        public Where<TDto, TProperty, SpryUpdateTable<TDto>> Where<TProperty>(string columnName)
        {
            return _table.Where<TProperty>(columnName);
        }

        public Where<TDto, TProperty, SpryUpdateTable<TDto>> OrWhere<TProperty>(string columnName)
        {
            return _table.OrWhere<TProperty>(columnName);
        }

        public Where<TDto, TProperty, SpryUpdateTable<TDto>> AndWhere<TProperty>(string columnName)
        {
            return _table.AndWhere<TProperty>(columnName);
        }
    }
}