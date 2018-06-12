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
        /// Update an item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="transaction"></param>
        public virtual int Update<T>(T item, IDbTransaction transaction = null) where T : BaseEntity
        {
            Type entityType = typeof(T);
            string commandText = _provider.UpdateQuery(new TableDefination(entityType.Name, GetColumns(entityType)));

            //execute
            return _connection.Execute(commandText, item, transaction);
        }

        /// <summary>
        /// Update an item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync<T>(T item, IDbTransaction transaction = null) where T : BaseEntity
        {
            Type entityType = typeof(T);
            string commandText = _provider.UpdateQuery(new TableDefination(entityType.Name, GetColumns(entityType)));

            //execute
            return await _connection.ExecuteAsync(commandText, item, transaction);
        }

        /// <summary>
        /// Update item collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="transaction"></param>
        public virtual int UpdateBulk<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity
        {
            Type entityType = typeof(T);
            string commandText = _provider.UpdateBulkQuery(new TableDefination(entityType.Name, items.Count(), GetColumns(entityType)));
            var parameters = GetParameters(items);

            //execute
            return _connection.Execute(commandText, parameters, transaction);
        }

        /// <summary>
        /// Update item collection
        /// </summary>
        /// <typeparam name=""></typeparam>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateBulkAsync<T>(IEnumerable<T> items, IDbTransaction transaction = null) where T : BaseEntity
        {
            Type entityType = typeof(T);
            string commandText = _provider.UpdateBulkQuery(new TableDefination(entityType.Name, items.Count(), GetColumns(entityType)));
            var parameters = GetParameters(items);

            //execute
            return await _connection.ExecuteAsync(commandText, parameters, transaction);
        }
    }
}