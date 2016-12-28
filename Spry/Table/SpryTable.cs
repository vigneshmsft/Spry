using System.Collections.Generic;
using System.Data;

namespace Spry.Table
{
    public abstract partial class SpryTable<TDto, TTable> : IExecutable, IConditionItem<TDto, TTable> where TTable : SpryTable<TDto, TTable>
    {
        protected string TableName;
        protected string Schema;
        protected string Alias;

        internal readonly SpryParameters Parameters = new SpryParameters();
        protected Buildable WhereCondition;
        protected readonly List<Buildable> AndConditions = new List<Buildable>();
        protected readonly List<Buildable> OrConditions = new List<Buildable>();
        protected string ExtraQuery = null;

        protected abstract TTable TableImpl { get; }

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

        internal static string BuildImpl(SpryTable<TDto, TTable> table)
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

        public TTable InSchema(string dbSchema)
        {
            Schema = dbSchema;
            return TableImpl;
        }

        public TTable As(string tableAlias)
        {
            Alias = tableAlias;
            return TableImpl;
        }

        public SpryTable<TDto, TTable> AppendQuery(string extraQuery)
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