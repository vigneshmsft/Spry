namespace Spry
{
    using System.Collections.Generic;
    using System.Data;

    public interface IExecutable
    {
        int Execute(IDbConnection connection, object parameters = null);

        IEnumerable<TDbDto> Query<TDbDto>(IDbConnection connection, object parameters = null);

        IEnumerable<dynamic> Query(IDbConnection connection, object parameters = null);

        TColType ExecuteScalar<TColType>(IDbConnection connection, object parameters = null);
    }
}