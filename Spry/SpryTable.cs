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

        //internal SpryTable(Spry spry, string tableName, string tableAlias = null)
        //{
        //    _spry = spry;
        //    _tableName = tableName;
        //    _tableAlias = tableAlias;
        //}

        //public SpryTable<TDtoType> In(string tableSchema)
        //{
        //    _tableSchema = tableSchema;
        //    return this;
        //}

        //public SpryTable<TDtoType> Where(string whereClause)
        //{
        //    _spry.AppendCondition(whereClause);
        //    return this;
        //}

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
}
