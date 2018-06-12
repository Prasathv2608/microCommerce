using microCommerce.Dapper.Providers;
using microCommerce.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace microCommerce.Dapper
{
    public partial class DataContext : IDataContext, IDisposable
    {
        #region Fields

        private readonly IDataProvider _provider;
        private readonly IDbConnection _connection;
        private const int _defaultExecutionTimeOut = 20;
        private int _executionTimeOut;

        #endregion

        #region Ctor

        public DataContext(IDataProvider provider,
            IDbConnection connection)
        {
            _provider = provider;
            _connection = connection;
        }

        #endregion

        #region Utilities

        private static readonly ConcurrentDictionary<Type, IList<string>> _cachedColumns = new ConcurrentDictionary<Type, IList<string>>();

        /// <summary>
        /// Get entity columns with thread safe
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="includeIdentity"></param>
        /// <returns></returns>
        protected virtual IEnumerable<string> GetColumns(Type entityType, bool includeIdentity = false)
        {
            if (!_cachedColumns.TryGetValue(entityType, out IList<string> columns))
            {
                columns = new List<string>();
                var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetGetMethod(false) != null);
                foreach (var prop in properties)
                {
                    var ignoreAttribute = prop.GetCustomAttribute(typeof(ColumnIgnoreAttribute), true);
                    if (ignoreAttribute != null)
                        continue;

                    //exlude identity column
                    var identityAttribute = prop.GetCustomAttribute(typeof(IdentityAttribute), true);
                    if (!includeIdentity && identityAttribute != null)
                        continue;

                    columns.Add(prop.Name);
                }

                string identityColumn = GetIdentityColumnName(properties);
                if (includeIdentity && columns.Contains(identityColumn))
                {
                    //identity column move to first item in list
                    columns.Remove(identityColumn);
                    columns.Insert(0, identityColumn);
                }

                _cachedColumns[entityType] = columns;
            }

            return columns;
        }

        /// <summary>
        /// Get parameter name and value from item collections to dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        protected virtual IDictionary<string, object> GetParameters<T>(IEnumerable<T> items)
        {
            var parameters = new Dictionary<string, object>();

            var entityArray = items.ToArray();
            var entityType = entityArray[0].GetType();
            for (int i = 0; i < entityArray.Length; i++)
            {
                var properties = entityArray[i].GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).AsEnumerable();
                properties = properties.Where(x => x.Name != GetIdentityColumnName(properties));

                foreach (var property in properties)
                    parameters.Add(property.Name + (i + 1), entityType.GetProperty(property.Name).GetValue(entityArray[i], null));
            }

            return parameters;
        }

        /// <summary>
        /// Get identity column of table
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected virtual string GetIdentityColumnName(IEnumerable<PropertyInfo> properties)
        {
            foreach (var prop in properties)
            {
                var attribute = prop.GetCustomAttribute(typeof(ColumnIgnoreAttribute), true);
                if (attribute != null)
                    return prop.Name;
            }

            throw new MissingMemberException("Class does not have identity column.");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Begin transcation scope
        /// </summary>
        /// <returns></returns>
        public virtual IDbTransaction BeginTransaction()
        {
            return _connection.BeginTransaction();
        }

        /// <summary>
        /// Dispose the current connection
        /// </summary>
        public virtual void Dispose()
        {
            if (_connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current connection
        /// </summary>
        public virtual IDbConnection Connection
        {
            get
            {
                return _connection;
            }
        }

        public int ExecutionTimeOut
        {
            get
            {
                if (_executionTimeOut > 0)
                    return _executionTimeOut;

                return _defaultExecutionTimeOut;
            }
            set
            {
                _executionTimeOut = value;
            }
        }

        #endregion
    }
}