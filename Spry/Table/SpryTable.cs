using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Spry.Where;

namespace Spry.Table
{
    public abstract class SpryTable<TDto> : IExecutable
    {
        protected string TableName;
        protected string Schema;
        protected string Alias;

        protected SpryTable(string tableName, string schema)
            : this(tableName, schema, null)
        {
        }

        protected SpryTable(string tableName, string schema, string alias)
        {
            TableName = tableName;
            Schema = schema;
            Alias = alias;
        }

        public virtual string Build()
        {
            return BuildImpl(this);
        }

        private readonly SpryParameters _parameters = new SpryParameters();
        protected Buildable WhereCondition;
        protected readonly List<Buildable> AndConditions = new List<Buildable>();
        protected readonly List<Buildable> OrConditions = new List<Buildable>();

        internal static string BuildImpl(SpryTable<TDto> table)
        {
            string returnString = null;

            if (string.IsNullOrWhiteSpace(table.Alias))
            {
                returnString = table.Schema + "." + table.TableName;
            }
            else
            {
                returnString = string.Format("{0}.{1} AS {2}", table.Schema, table.TableName, table.Alias);
            }
            return returnString;
        }

        public SpryTable<TDto> InSchema(string dbSchema)
        {
            Schema = dbSchema;
            return this;
        }

        public Where<TDto, TProperty> Where<TProperty>(Expression<Func<TDto, TProperty>> columnExpression)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            var where = new Where<TDto, TProperty>(this, _parameters, columnName);
            WhereCondition = where;
            return where;
        }

        public Where<TDto, TProperty> Where<TProperty>(string columnName)
        {
            var where = new Where<TDto, TProperty>(this, _parameters, columnName);
            WhereCondition = where;
            return where;
        }

        public Where<TDto, TProperty> AndWhere<TProperty>(Expression<Func<TDto, TProperty>> columnExpression)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            var andWhere = new Where<TDto, TProperty>(this, _parameters, columnName);
            AndConditions.Add(andWhere);
            return andWhere;
        }

        public Where<TDto, TProperty> AndWhere<TProperty>(string columnName)
        {
            var andWhere = new Where<TDto, TProperty>(this, _parameters, columnName);
            AndConditions.Add(andWhere);
            return andWhere;
        }

        public Where<TDto, TProperty> OrWhere<TProperty>(Expression<Func<TDto, TProperty>> columnExpression)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            var orWhere = new Where<TDto, TProperty>(this, _parameters, columnName);
            OrConditions.Add(orWhere);
            return orWhere;
        }

        public Where<TDto, TProperty> OrWhere<TProperty>(string columnName)
        {
            var orWhere = new Where<TDto, TProperty>(this, _parameters, columnName);
            OrConditions.Add(orWhere);
            return orWhere;
        }

        public int Execute(IDbConnection connection, object parameters = null)
        {
            return new SqlExecutor(Build()).Execute(connection, _parameters);
        }

        public IEnumerable<TDbDto> Query<TDbDto>(IDbConnection connection, object parameters = null)
        {
            return new SqlExecutor(Build()).Query<TDbDto>(connection, _parameters);
        }

        public IEnumerable<dynamic> Query(IDbConnection connection, object parameters = null)
        {
            return new SqlExecutor(Build()).Query(connection, _parameters);
        }

        public TColType ExecuteScalar<TColType>(IDbConnection connection, object parameters = null)
        {
            return new SqlExecutor(Build()).ExecuteScalar<TColType>(connection, _parameters);
        }

        internal void AddParameter(string name, object value)
        {
            _parameters.Add(name, value);
        }
    }
}
