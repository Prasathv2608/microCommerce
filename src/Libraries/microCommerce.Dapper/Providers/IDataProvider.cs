using microCommerce.Domain;
using System.Data;

namespace microCommerce.Dapper.Providers
{
    public interface IDataProvider
    {
        IDbConnection CreateConnection(string connectionString);
        string SelectFirstQuery<T>(TableDefination table) where T : BaseEntity;
        string InsertQuery(TableDefination table);
        string InsertBulkQuery(TableDefination table);
        string UpdateQuery(TableDefination table);
        string UpdateBulkQuery(TableDefination table);
        string DeleteQuery(string tableName);
        string DeleteBulkQuery(string tableName);
        string ExistingQuery(string tableName);
        string CountQuery(string tableName);
    }
}