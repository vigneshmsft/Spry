using System.Linq.Expressions;
using System;
using System.Text;
using Spry.Where;

namespace Spry.Update
{
    public class UpdateValue<TDto>
    {
        private readonly SpryUpdate<TDto> _spry;
        private readonly SpryUpdateTable<TDto> _table;
        private readonly StringBuilder _updateBuilder;

        public UpdateValue(SpryUpdate<TDto> spry , SpryUpdateTable<TDto> table)
        {
            _spry = spry;
            _table = table;
            _updateBuilder = new StringBuilder();
        }

        public UpdateValue<TDto> Set<TProperty>(Expression<Func<TDto, TProperty>> valueExpression)
        {
            var columnName = SpryExpression.GetColumnName(valueExpression);
            var value = SpryExpression.GetColumnName(valueExpression);
            SetValueImpl(columnName, value);
            return this;
        }

        public UpdateValue<TDto> Set(string columnName, object value)
        {
            SetValueImpl(columnName, value);
            return this;
        }

        public Where<TDto, TProperty> Where<TProperty>(string columnName)
        {
            return _table.Where<TProperty>(columnName);
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
    }
}