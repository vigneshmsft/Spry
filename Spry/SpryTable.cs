namespace Spry
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;

    public class SpryTable<TDtoType> : IExecutable
    {
        private readonly Spry _spry;
        private readonly string _tableName;
        private readonly string _tableAlias;
        private string _tableSchema;

        internal SpryTable(Spry spry, string tableName, string tableAlias = null)
        {
            _spry = spry;
            _tableName = tableName;
            _tableAlias = tableAlias;
        }

        public SpryTable<TDtoType> In(string tableSchema)
        {
            _tableSchema = tableSchema;
            return this;
        }

        public SpryTable<TDtoType> Where(string whereClause)
        {
            _spry.AppendCondition(whereClause);
            return this;
        }

        public virtual string GetQuery()
        {
            _spry.UseTable(_tableName, _tableSchema ?? "dbo", _tableAlias);
            return _spry.BuildQuery();
        }

        public int Execute(IDbConnection connection, object parameters = null)
        {
            return new SqlExecutor(GetQuery()).Execute(connection, parameters);
        }

        public IEnumerable<TDto> Query<TDto>(IDbConnection connection, object parameters = null)
        {
            return new SqlExecutor(GetQuery()).Query<TDto>(connection, parameters);
        }

        public IEnumerable<dynamic> Query(IDbConnection connection, object parameters = null)
        {
            return new SqlExecutor(GetQuery()).Query(connection, parameters);
        }

        public TColType ExecuteScalar<TColType>(IDbConnection connection, object parameters = null)
        {
            return new SqlExecutor(GetQuery()).ExecuteScalar<TColType>(connection, parameters);
        }
    }

    public sealed class SelectTable<TDtoType> : IExecutable
    {
        private readonly Select _spry;
        private readonly string _tableName;
        private readonly string _tableAlias;
        private string _tableSchema;

        internal SelectTable(Select spry, string tableName, string tableAlias = null)
        {
            _tableName = tableName;
            _tableAlias = tableAlias;
            _spry = spry;
        }

        public SelectTable<TDtoType> In(string tableSchema)
        {
            _tableSchema = tableSchema;
            return this;
        }

        public SelectTable<TDtoType> InnerJoin(string tableName, string tableSchema = null)
        {
            _spry.AppendJoin("INNER JOIN " + (tableSchema ?? "dbo") + "." + tableName);
            return this;
        }

        public SelectTable<TDtoType> On(string col1, string col2)
        {
            _spry.AppendJoin(" ON " + col1 + " = " + col2);
            _spry.AppendLine("");
            return this;
        }

        public string GetQuery()
        {
            _spry.UseTable(_tableName, _tableSchema ?? "dbo", _tableAlias);
            return _spry.BuildQuery();
        }

        [Obsolete("* Still a Work In Progress. Do not Use this Overload.!!")]
        public SelectTable<TDtoType> Where<TProperty>(Expression<Func<TDtoType, TProperty>> columnExpression)
        {
            new Where<TDtoType>(_spry).ColumnEquals(columnExpression);
            return this;
        }

        public SelectTable<TDtoType> Where(string whereClause)
        {
            _spry.AppendCondition(whereClause);
            return this;
        }

        public int Execute(IDbConnection connection, object parameters = null)
        {
            return new SqlExecutor(GetQuery()).Execute(connection, parameters);
        }

        public IEnumerable<TDto> Query<TDto>(IDbConnection connection, object parameters = null)
        {
            return new SqlExecutor(GetQuery()).Query<TDto>(connection, parameters);
        }

        public IEnumerable<dynamic> Query(IDbConnection connection, object parameters = null)
        {
            return new SqlExecutor(GetQuery()).Query(connection, parameters);
        }

        public TColType ExecuteScalar<TColType>(IDbConnection connection, object parameters = null)
        {
            return new SqlExecutor(GetQuery()).ExecuteScalar<TColType>(connection, parameters);
        }
    }
}
