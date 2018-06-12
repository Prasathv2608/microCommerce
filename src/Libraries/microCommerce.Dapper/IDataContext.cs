using microCommerce.Domain;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace microCommerce.Dapper
{
    public partial interface IDataContext
    {
        T Find<T>(int Id) where T : BaseEntity;
        Task<T> FindAsync<T>(int Id) where T : BaseEntity;
        T First<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity;
        Task<T> FirstAsync<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity;
        int Execute(string commandText, object parameters = null, IDbTransaction transaction = null);
        Task<int> ExecuteAsync(string commandText, object parameters = null, IDbTransaction transaction = null);
        IDataReader ExecuteReader(string commandText, object parameters = null, IDbTransaction transaction = null);
        Task<IDataReader> ExecuteReaderAsync(string commandText, object parameters = null, IDbTransaction transaction = null);
        T ExecuteScalar<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity;
        object ExecuteScalar(string commandText, object parameters = null, IDbTransaction transaction = null);
        Task<T> ExecuteScalarAsync<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity;
        Task<object> ExecuteScalarAsync(string commandText, object parameters = null, IDbTransaction transaction = null);
        IEnumerable<T> Query<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity;
        Task<IEnumerable<T>> QueryAsync<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity;
        T FirstProcedure<T>(string procedureName, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity;
        Task<T> FirstProcedureAsync<T>(string procedureName, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity;
        int ExecuteProcedure(string procedureName, object parameters = null, IDbTransaction transaction = null);
        Task<int> ExecuteProcedureAsync(string procedureName, object parameters = null, IDbTransaction transaction = null);
        IDataReader ExecuteReaderProcedure(string procedureName, object parameters = null, IDbTransaction transaction = null);
        Task<IDataReader> ExecuteReaderProcedureAsync(string procedureName, object parameters = null, IDbTransaction transaction = null);
        T ExecuteScalarProcedure<T>(string procedureName, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity;
        object ExecuteScalarProcedure(string procedureName, object parameters = null, IDbTransaction transaction = null);
        Task<T> ExecuteScalarProcedureAsync<T>(string procedureName, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity;
        Task<object> ExecuteScalarProcedureAsync(string procedureName, object parameters = null, IDbTransaction transaction = null);
        IEnumerable<T> QueryProcedure<T>(string procedureName, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity;
        Task<IEnumerable<T>> QueryProcedureAsync<T>(string procedureName, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity;
        
        void Insert<T>(T item, IDbTransaction transaction = null) where T : BaseEntity;
        Task InsertAsync<T>(T item, IDbTransaction transaction = null) where T : BaseEntity;

        int InsertBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity;
        Task<int> InsertBulkAsync<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity;

        int Update<T>(T item, IDbTransaction transaction = null) where T : BaseEntity;
        Task<int> UpdateAsync<T>(T item, IDbTransaction transaction = null) where T : BaseEntity;

        int UpdateBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity;
        Task<int> UpdateBulkAsync<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity;

        int Delete<T>(T item, IDbTransaction transaction = null) where T : BaseEntity;
        Task<int> DeleteAsync<T>(T item, IDbTransaction transaction = null) where T : BaseEntity;

        int DeleteBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity;
        Task<int> DeleteBulkAsync<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity;

        IDbTransaction BeginTransaction();
        IDbConnection Connection { get; }
        int ExecutionTimeOut { get; }
    }
}