namespace Spry
{
    using System;
    using System.Linq.Expressions;
    using System.Text;

    internal sealed class Insert : Spry
    {
        private readonly StringBuilder _columnBuilder = new StringBuilder();
        private readonly StringBuilder _valuesBuilder = new StringBuilder();
        private readonly StringBuilder _outputBuilder = new StringBuilder();

        internal Insert()
        {
            _columnBuilder.Append("( ");
            _valuesBuilder.Append("VALUES" + Environment.NewLine + "(");
        }

        internal override void UseTable(string tableName, string schema = "dbo", string tableAlias = null)
        {
            Builder.AppendLine(" INTO " + schema + "." + tableName);
        }

        internal override void AppendColumn(string columnName)
        {
            _columnBuilder.Append(columnName + ", ");
        }

        internal void AppendValues(string columnName)
        {
            _valuesBuilder.Append(columnName + ", ");
        }

        internal void AppendOutputColumn(string columnName)
        {
            _outputBuilder.Append(columnName);
        }

        internal override string BuildQuery()
        {
            RemoveTrailingCharacter(_columnBuilder);
            RemoveTrailingCharacter(_valuesBuilder);

            _columnBuilder.Append(" )");
            _valuesBuilder.Append(" )");

            Builder.AppendLine(_columnBuilder.ToString());
            Builder.AppendLine(_outputBuilder.ToString());
            Builder.AppendLine(_valuesBuilder.ToString());

            return base.BuildQuery();
        }

        private static void RemoveTrailingCharacter(StringBuilder builder)
        {
            builder.Length--;
            builder.Length--;
        }
    }

    public sealed class SpryInsertColumn<TDtoType>
    {
        private readonly Insert _spry;

        internal SpryInsertColumn(Insert spry)
        {
            _spry = spry;
        }

        public SpryInsertColumn<TDtoType> Column<TProperty>(Expression<Func<TDtoType, TProperty>> columnExpression)
        {
            return ColumnImpl(columnExpression);
        }

        public SpryInsertColumn<TDtoType> Column(Expression<Func<TDtoType, string>> columnExpression)
        {
            return ColumnImpl(columnExpression);
        }

        private SpryInsertColumn<TDtoType> ColumnImpl<TProperty>(Expression<Func<TDtoType, TProperty>> columnExpression)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            var value = SpryExpression.GetColumnValue(columnExpression);

            _spry.AppendColumn(columnName);
            _spry.AppendValues(value);

            return this;
        }

        public SpryInsertColumn<TDtoType> ColumnWithValue(string columnName, string value)
        {
            _spry.AppendColumn(columnName);
            _spry.AppendValues(value);
            return this;
        }

        public SpryInsertColumn<TDtoType> Output<TProperty>(Expression<Func<TDtoType, TProperty>> columnExpression)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            _spry.AppendOutputColumn("OUTPUT Inserted." + columnName);
            return this;
        }

        public SpryInsertColumn<TDtoType> Output(string columnName)
        {
            _spry.AppendOutputColumn("OUTPUT Inserted." + columnName);
            return this;
        }

        public SpryTable<TDtoType> Into(string tableName)
        {
            return new SpryTable<TDtoType>(_spry, tableName, null);
        }
    }
}