namespace Spry
{
    using System;
    using System.Linq.Expressions;

    public class Where<TDtoType>
    {
        private readonly Select _spry;

        internal Where(Select spry)
        {
            _spry = spry;
        }

        public Where<TDtoType> ColumnEquals<TProperty>(Expression<Func<TDtoType, TProperty>> columnExpression)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            _spry.AppendCondition(columnName + " = " );
            return this;
        }
    }
}
