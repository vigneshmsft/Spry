using System;

namespace Spry
{
    using System.Linq.Expressions;
    using System.Text;

    internal sealed class Update : Spry
    {
        private readonly StringBuilder _setBuilder = new StringBuilder();

        internal Update()
        {
            _setBuilder.AppendLine("SET");
        }

        internal override void UseTable(string tableName, string schema = "dbo", string tableAlias = null)
        {
            Builder.AppendLine(" " + schema + "." + tableName);
        }

        internal override void AppendColumn(string column)
        {
            _setBuilder.AppendLine(column);
        }

        internal override string BuildQuery()
        {
            RemoveTrailingComma(_setBuilder);

            Builder.AppendLine(_setBuilder.ToString());

            return base.BuildQuery();
        }
    }

    public sealed class SpryUpdateColumn<TDtoType>
    {
        private readonly Update _spry;

        internal SpryUpdateColumn(Update spry)
        {
            _spry = spry;
        }

        public SpryUpdateColumn<TDtoType> Set<TProperty>(Expression<Func<TDtoType, TProperty>> columnExpression)
        {
            return SetImpl(columnExpression);
        }

        public SpryUpdateColumn<TDtoType> Set(Expression<Func<TDtoType, string>> columnExpression)
        {
            return SetImpl(columnExpression);
        }

        private SpryUpdateColumn<TDtoType> SetImpl<TProperty>(Expression<Func<TDtoType, TProperty>> columnExpression)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            var value = SpryExpression.GetColumnValue(columnExpression);

            _spry.AppendColumn(columnName + " = " + value + ",");

            return this;
        }

        public SpryUpdateColumn<TDtoType> SetWithValue(string columnName, string value)
        {
            _spry.AppendColumn(columnName);
            return this;
        }

        public SpryTable<TDtoType> In(string tableName, string tableAlias = null)
        {
            return new SpryTable<TDtoType>(_spry, tableName, tableAlias);
        }
    }
}
