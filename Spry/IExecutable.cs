namespace Spry
{
    using System.Collections.Generic;
    using System.Data;

    public interface IExecutable
    {
        int Execute(IDbConnection connection, SpryParameters parameters = null);

        IEnumerable<TDbDto> Query<TDbDto>(IDbConnection connection, SpryParameters parameters = null);

        IEnumerable<dynamic> Query(IDbConnection connection, SpryParameters parameters = null);

        TColType ExecuteScalar<TColType>(IDbConnection connection, SpryParameters parameters = null);
    }
}