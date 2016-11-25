namespace Spry
{
    using System;
    using System.Text;

    public abstract class Spry
    {
        internal Spry() { }

        protected readonly StringBuilder Builder = new StringBuilder();

        protected readonly StringBuilder WhereBuilder = new StringBuilder();

        public static SpryColumn<TDtoType> Select<TDtoType>()
        {
            var spry = new Select();
            spry.Builder.AppendLine("SELECT");
            return new SpryColumn<TDtoType>(spry);
        }

        public static SpryColumn<dynamic> Select()
        {
            var spry = new Select();
            spry.Builder.AppendLine("SELECT");
            return new SpryColumn<dynamic>(spry);
        }

        public static SpryInsertColumn<TDtoType> Insert<TDtoType>()
        {
            var spry = new Insert();
            spry.Builder.Append("INSERT");
            return new SpryInsertColumn<TDtoType>(spry);
        }

        public static SpryInsertColumn<dynamic> Insert()
        {
            var spry = new Insert();
            spry.Builder.Append("INSERT");
            return new SpryInsertColumn<dynamic>(spry);
        }

        public static SpryUpdateColumn<TDtoType> Update<TDtoType>()
        {
            var spry = new Update();
            spry.Builder.Append("UPDATE");
            return new SpryUpdateColumn<TDtoType>(spry);
        }

        public static SpryUpdateColumn<dynamic> Update()
        {
            var spry = new Update();
            spry.Builder.Append("UPDATE");
            return new SpryUpdateColumn<dynamic>(spry);
        }

        internal virtual void AppendColumn(string columnName)
        {
            Builder.AppendLine(columnName + ",");
        }

        internal virtual string BuildQuery()
        {
            var query = Builder.ToString();

            return query + WhereBuilder;
        }

        internal abstract void UseTable(string tableName, string schema = "dbo", string tableAlias = null);

        internal virtual void Append(string text)
        {
            Builder.Append(text);
        }

        internal virtual void AppendLine(string text)
        {
            Builder.AppendLine(text);
        }

        internal void AppendCondition(string text)
        {
            WhereBuilder.Append(text);
        }

        protected void RemoveTrailingComma(StringBuilder builder)
        {
            builder.Length -= Environment.NewLine.Length + 1;
        }
    }
}