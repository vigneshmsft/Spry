using System.Collections.Generic;
using System.Data;
using Dapper;

namespace Spry
{
    public class SqlExecutor : IExecutable
    {
        private readonly string _query;

        public SqlExecutor(string query)
        {
            _query = query;
        }

        public int Execute(IDbConnection connection, CommandType commandType = CommandType.Text, SpryParameters parameters = null)
        {
            return connection.Execute(_query, parameters, commandType: commandType);
        }

        public IEnumerable<TDto> Query<TDto>(IDbConnection connection, CommandType commandType = CommandType.Text, SpryParameters parameters = null)
        {
            return connection.Query<TDto>(_query, parameters, commandType: commandType);
        }

        public IEnumerable<dynamic> Query(IDbConnection connection, CommandType commandType = CommandType.Text, SpryParameters parameters = null)
        {
            return connection.Query(_query, parameters, commandType: commandType);
        }

        public TColType ExecuteScalar<TColType>(IDbConnection connection, CommandType commandType = CommandType.Text, SpryParameters parameters = null)
        {
            return connection.ExecuteScalar<TColType>(_query, parameters, commandType: commandType);
        }
    }
}