using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using Spry.Table;

namespace Spry.Insert
{
    public class InsertValue<TDto> : IExecutable
    {
        private readonly SpryInsert<TDto> _spry;
        private readonly SpryTable<TDto> _table;
        private readonly SpryParameters _parameters;
        private readonly StringBuilder _insertColumnBuilder;
        private readonly StringBuilder _insertValueBuilder;
        private string _outputCol = null;
        private bool _outputIdentity = false;

        internal InsertValue(SpryInsert<TDto> spry, SpryTable<TDto> table)
        {
            _spry = spry;
            _table = table;
            _insertColumnBuilder = new StringBuilder();
            _insertValueBuilder = new StringBuilder();
            _parameters = new SpryParameters();
        }

        public InsertValue<TDto> Value<TProperty>(Expression<Func<TDto, TProperty>> valueExpression)
        {
            var columnName = SpryExpression.GetColumnName(valueExpression);
            var value = SpryExpression.GetColumnName(valueExpression);
            InsertValueImpl(columnName, value);
            return this;
        }

        public InsertValue<TDto> Value(string columnName, object value)
        {
            InsertValueImpl(columnName, value);
            return this;
        }

        public InsertValue<TDto> OutputInserted(string outputColumnName)
        {
            _outputCol = outputColumnName;
            return this;
        }

        public InsertValue<TDto> OutputIdentity()
        {
            _outputIdentity = true;
            return this;
        }

        private void InsertValueImpl(string columnName, object value)
        {
            _parameters.Add(columnName, value);
            _insertColumnBuilder.Append(columnName + ", ");
            _insertValueBuilder.Append("@" + columnName + ", ");
        }

        public string Build()
        {
            return _spry.Build();
        }



        internal string BuildImpl()
        {
            RemoveTrailingCommas();
            string returnValue;

            if (!string.IsNullOrWhiteSpace(_outputCol))
            {
                returnValue = string.Format(@"({0}){1}OUTPUT Inserted.{3}{1}VALUES{1}({2})", _insertColumnBuilder, Environment.NewLine, _insertValueBuilder, _outputCol);
            }

            returnValue = string.Format(@"({0}){1}VALUES{1}({2})", _insertColumnBuilder, Environment.NewLine, _insertValueBuilder);

            if (_outputIdentity)
            {
                returnValue += @"; SELECT SCOPE_IDENTITY() AS [Inserted];";
            }

            return returnValue;
        }

        private void RemoveTrailingCommas()
        {
            _insertColumnBuilder.Length -= 2;
            _insertValueBuilder.Length -= 2;
        }

        public int Execute(IDbConnection connection, object parameters = null)
        {
            return _table.Execute(connection, parameters);
        }

        public IEnumerable<TDbDto> Query<TDbDto>(IDbConnection connection, object parameters = null)
        {
            return _table.Query<TDbDto>(connection, parameters);
        }

        public IEnumerable<dynamic> Query(IDbConnection connection, object parameters = null)
        {
            return _table.Query(connection, parameters);
        }

        public TColType ExecuteScalar<TColType>(IDbConnection connection, object parameters = null)
        {
            return _table.ExecuteScalar<TColType>(connection, parameters);
        }
    }
}
