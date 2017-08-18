namespace Spry
{
    using System.Collections.Generic;
    using System.Data;

    public interface IExecutable
    {
        int Execute(IDbConnection connection, CommandType commandType = CommandType.Text, SpryParameters parameters = null);

        IEnumerable<TDbDto> Query<TDbDto>(IDbConnection connection, CommandType commandType = CommandType.Text, SpryParameters parameters = null);

        IEnumerable<dynamic> Query(IDbConnection connection, CommandType commandType = CommandType.Text, SpryParameters parameters = null);

        TColType ExecuteScalar<TColType>(IDbConnection connection, CommandType commandType = CommandType.Text, SpryParameters parameters = null);
    }
}