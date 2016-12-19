using System;
using System.Linq.Expressions;
using System.Text;

namespace Spry.Select
{
    public sealed class SprySelectColumn<TDto>
    {
        private readonly SprySelect<TDto> _spry;
        private readonly StringBuilder _columnBuilder;
        private SprySelectTable<TDto> _from;

        internal SprySelectColumn(SprySelect<TDto> spry)
        {
            _columnBuilder = new StringBuilder();
            _spry = spry;
        }

        public SprySelectColumn<TDto> Column<TProperty>(Expression<Func<TDto, TProperty>> columnExpression)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            AppendColumn(columnName);
            return this;
        }

        public SprySelectColumn<TDto> Column<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string alias)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            AppendColumn(alias + "." + columnName);
            return this;
        }

        public SprySelectColumn<TDto> Column(string columnName)
        {
            AppendColumn(columnName);
            return this;
        }

        public SprySelectTable<TDto> From(string tableName)
        {
            return _from = new SprySelectTable<TDto>(_spry, tableName);
        }

        private void AppendColumn(string columnName)
        {
            _columnBuilder.Append(columnName + ", ");
        }

        internal string BuildImpl()
        {
            RemoveTrailingComma();
            return _columnBuilder + Environment.NewLine + "FROM" + Environment.NewLine + _from.BuildImpl();
        }

        private void RemoveTrailingComma()
        {
            _columnBuilder.Length -= 2;
        }
    }
}
