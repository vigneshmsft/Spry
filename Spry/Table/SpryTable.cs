using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Spry.Where;

namespace Spry.Table
{
    public abstract class SpryTable<TDto> : IExecutable, IConditionItem<TDto>
    {
        protected string TableName;
        protected string Schema;
        protected string Alias;

        internal readonly SpryParameters Parameters = new SpryParameters();
        protected Buildable WhereCondition;
        protected readonly List<Buildable> AndConditions = new List<Buildable>();
        protected readonly List<Buildable> OrConditions = new List<Buildable>();
        protected string ExtraQuery = null;

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

        public SpryTable<TDto> As(string tableAlias)
        {
            Alias = tableAlias;
            return this;
        }

        public Where<TDto, TProperty> Where<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string colPrefix = null)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            if (!string.IsNullOrWhiteSpace(colPrefix))
            {
                columnName = colPrefix + "." + columnName;
            }
            var where = new Where<TDto, TProperty>(this, Parameters, columnName);
            WhereCondition = where;
            return where;
        }

        public Where<TDto, TProperty> Where<TProperty>(string columnName)
        {
            var where = new Where<TDto, TProperty>(this, Parameters, columnName);
            WhereCondition = where;
            return where;
        }

        public Where<TDto, TProperty> AndWhere<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string colPrefix = null)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            if (!string.IsNullOrWhiteSpace(colPrefix)) columnName = colPrefix + "." + columnName;
            var andWhere = new Where<TDto, TProperty>(this, Parameters, columnName);
            AndConditions.Add(andWhere);
            return andWhere;
        }

        public Where<TDto, TProperty> AndWhere<TProperty>(string columnName)
        {
            var andWhere = new Where<TDto, TProperty>(this, Parameters, columnName);
            AndConditions.Add(andWhere);
            return andWhere;
        }

        public Where<TDto, TProperty> OrWhere<TProperty>(Expression<Func<TDto, TProperty>> columnExpression, string colPrefix = null)
        {
            var columnName = SpryExpression.GetColumnName(columnExpression);
            if (!string.IsNullOrWhiteSpace(colPrefix)) columnName = colPrefix + "." + columnName;
            var orWhere = new Where<TDto, TProperty>(this, Parameters, columnName);
            OrConditions.Add(orWhere);
            return orWhere;
        }

        public Where<TDto, TProperty> OrWhere<TProperty>(string columnName)
        {
            var orWhere = new Where<TDto, TProperty>(this, Parameters, columnName);
            OrConditions.Add(orWhere);
            return orWhere;
        }

        public SpryTable<TDto> AppendQuery(string extraQuery)
        {
            ExtraQuery = extraQuery;
            return this;
        }

        public int Execute(IDbConnection connection, SpryParameters parameters = null)
        {
            Parameters.Add(parameters);
            return new SqlExecutor(Build()).Execute(connection, Parameters);
        }

        public IEnumerable<TDbDto> Query<TDbDto>(IDbConnection connection, SpryParameters parameters = null)
        {
            Parameters.Add(parameters);
            return new SqlExecutor(Build()).Query<TDbDto>(connection, Parameters);
        }

        public IEnumerable<dynamic> Query(IDbConnection connection, SpryParameters parameters = null)
        {
            Parameters.Add(parameters);
            return new SqlExecutor(Build()).Query(connection, Parameters);
        }

        public TColType ExecuteScalar<TColType>(IDbConnection connection, SpryParameters parameters = null)
        {
            Parameters.Add(parameters);
            return new SqlExecutor(Build()).ExecuteScalar<TColType>(connection, Parameters);
        }

        internal void AddParameter(string name, object value)
        {
            Parameters.Add(name, value);
        }
    }
}
