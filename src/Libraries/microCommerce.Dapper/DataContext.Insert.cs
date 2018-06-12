using microCommerce.Domain;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace microCommerce.Dapper
{
    public partial class DataContext
    {
        /// <summary>
        /// Insert an item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="transaction"></param>
        public virtual void Insert<T>(T item, IDbTransaction transaction = null) where T : BaseEntity
        {
            Type entityType = typeof(T);
            string commandText = _provider.InsertQuery(new TableDefination(entityType.Name, GetColumns(entityType)));

            //execute
            item.Id = _connection.ExecuteScalar<int>(commandText, item, transaction);
        }

        /// <summary>
        /// Insert an item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task InsertAsync<T>(T item, IDbTransaction transaction = null) where T : BaseEntity
        {
            Type entityType = typeof(T);
            string commandText = _provider.InsertQuery(new TableDefination(entityType.Name, GetColumns(entityType)));

            //execute
            item.Id = await _connection.ExecuteScalarAsync<int>(commandText, item, transaction);
        }

        /// <summary>
        /// Insert item collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="transaction"></param>
        public virtual int InsertBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity
        {
            Type entityType = typeof(T);
            string commandText = _provider.InsertBulkQuery(new TableDefination(entityType.Name, items.Count(), GetColumns(entityType)));
            var parameters = GetParameters(items);

            //execute
            return _connection.Execute(commandText, parameters, transaction);
        }

        /// <summary>
        /// Insert item collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<int> InsertBulkAsync<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity
        {
            Type entityType = typeof(T);
            string commandText = _provider.InsertBulkQuery(new TableDefination(entityType.Name, items.Count(), GetColumns(entityType)));
            var parameters = GetParameters(items);

            //execute
            return await _connection.ExecuteAsync(commandText, parameters, transaction);
        }
    }
}