namespace Spry
{
    using System;
    using System.Linq.Expressions;
    using System.Text;

    internal sealed class Select : Spry
    {
        private readonly StringBuilder _innerjoinBuilder = new StringBuilder();

        internal override string BuildQuery()
        {
            Builder.AppendLine(_innerjoinBuilder.ToString());
            return base.BuildQuery();
        }

        internal override void UseTable(string tableName, string schema = "dbo", string tableAlias = null)
        {
            RemoveTrailingComma(Builder);

            Builder.Append(Environment.NewLine + "FROM " + schema + "." + tableName);

            string aliasString = null;
            if (!string.IsNullOrWhiteSpace(tableAlias))
            {
                aliasString = " AS " + tableAlias;
            }

            Builder.AppendLine(aliasString ?? " ");
        }

        internal void AppendJoin(string text)
        {
            _innerjoinBuilder.Append(text);
        }
    }

    public sealed class SpryColumn<TDtoType>
    {
        private readonly Select _spry;

        internal SpryColumn(Select spry)
        {
            _spry = spry;
        }

        public SpryColumn<TDtoType> Column<TProperty>(Expression<Func<TDtoType, TProperty>> columnExpression, string alias = null)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            _spry.AppendColumn(columnName);
            return this;
        }

        public SpryColumn<TDtoType> Column(string columnName)
        {
            _spry.AppendColumn(columnName);
            return this;
        }

        public SelectTable<TDtoType> From(string tableName, string tableAlias = null)
        {
            return new SelectTable<TDtoType>(_spry, tableName, tableAlias);
        }
    }
}
