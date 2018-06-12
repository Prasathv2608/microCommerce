using microCommerce.Common;
using microCommerce.Dapper.Providers.MySQL;
using microCommerce.Dapper.Providers.SqlServer;

namespace microCommerce.Dapper.Providers
{
    public class ProviderFactory
    {
        public static IDataProvider GetProvider(string providerName)
        {
            switch (providerName)
            {
                case "MySql.Data.SqlClient":
                    return new MySqlDataProvider();
                case "System.Data.SqlClient":
                    return new SqlServerDataProvider();
                default:
                    throw new CustomException("Database provider does not supported!");
            }
        }
    }
}