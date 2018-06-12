using Dapper;
using System.Threading.Tasks;

namespace microCommerce.Dapper
{
    public static class Extensions
    {
        /// <summary>
        /// Check the value existing on database
        /// </summary>
        /// <param name="context"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool Exist(this IDataContext context, string commandText, object parameters = null)
        {
            return context.Connection.ExecuteScalar<bool>(commandText, parameters);
        }

        /// <summary>
        /// Check the value existing on database
        /// </summary>
        /// <param name="context"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<bool> ExistAsync(this IDataContext context, string commandText, object parameters = null)
        {
            return await context.Connection.ExecuteScalarAsync<bool>(commandText, parameters);
        }

        /// <summary>
        /// Gets the row count
        /// </summary>
        /// <param name="context"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int Count(this IDataContext context, string commandText, object parameters = null)
        {
            return context.Connection.ExecuteScalar<int>(commandText, parameters);
        }

        /// <summary>
        /// Gets the row count
        /// </summary>
        /// <param name="context"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<int> CountAsync(this IDataContext context, string commandText, object parameters = null)
        {
            return await context.Connection.ExecuteScalarAsync<int>(commandText, parameters);
        }
    }
}