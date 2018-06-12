using microCommerce.Domain;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace microCommerce.Dapper
{
    public partial class DataContext
    {
        #region Find

        /// <summary>
        /// Find by identifier
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual T Find<T>(int Id) where T : BaseEntity
        {
            Type entityType = typeof(T);
            string commandText = _provider.SelectFirstQuery<T>(new TableDefination(entityType.Name, GetColumns(entityType, true)));

            //execute first query
            return _connection.QueryFirstOrDefault<T>(new CommandDefinition(commandText, new { Id }, null, _executionTimeOut, CommandType.Text));
        }

        /// <summary>
        /// Find by identifier
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual async Task<T> FindAsync<T>(int Id) where T : BaseEntity
        {
            Type entityType = typeof(T);
            string commandText = _provider.SelectFirstQuery<T>(new TableDefination(entityType.Name, GetColumns(entityType, true)));

            //execute first query
            return await _connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(commandText, new { Id }, null, _executionTimeOut, CommandType.Text));
        }

        #endregion

        #region First Query

        /// <summary>
        /// Get the first item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual T First<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity
        {
            return _connection.QueryFirstOrDefault<T>(new CommandDefinition(commandText, parameters, transaction, _executionTimeOut, CommandType.Text));
        }

        /// <summary>
        /// Get the first item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<T> FirstAsync<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity
        {
            return await _connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(commandText, parameters, transaction, _executionTimeOut, CommandType.Text));
        }

        #endregion

        #region Execute

        /// <summary>
        /// Execute a query
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual int Execute(string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return _connection.Execute(new CommandDefinition(commandText, parameters, transaction, _executionTimeOut, CommandType.Text));
        }

        /// <summary>
        /// Execute a query
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<int> ExecuteAsync(string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return await _connection.ExecuteAsync(new CommandDefinition(commandText, parameters, transaction, _executionTimeOut, CommandType.Text));
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// Execute reader
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual IDataReader ExecuteReader(string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return _connection.ExecuteReader(new CommandDefinition(commandText, parameters, transaction, _executionTimeOut, CommandType.Text));
        }

        /// <summary>
        /// Execute reader
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<IDataReader> ExecuteReaderAsync(string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return await _connection.ExecuteReaderAsync(new CommandDefinition(commandText, parameters, transaction, _executionTimeOut, CommandType.Text));
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Execute scalar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual T ExecuteScalar<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity
        {
            return _connection.ExecuteScalar<T>(new CommandDefinition(commandText, parameters, transaction, _executionTimeOut, CommandType.Text));
        }

        /// <summary>
        /// Execute scalar
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual object ExecuteScalar(string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return _connection.ExecuteScalar(new CommandDefinition(commandText, parameters, transaction, _executionTimeOut, CommandType.Text));
        }

        /// <summary>
        /// Execute scalar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<T> ExecuteScalarAsync<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity
        {
            return await _connection.ExecuteScalarAsync<T>(new CommandDefinition(commandText, parameters, transaction, _executionTimeOut, CommandType.Text));
        }

        /// <summary>
        /// Execute scalar
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<object> ExecuteScalarAsync(string commandText, object parameters = null, IDbTransaction transaction = null)
        {
            return await _connection.ExecuteScalarAsync(new CommandDefinition(commandText, parameters, transaction, _executionTimeOut, CommandType.Text));
        }

        #endregion

        #region Query

        /// <summary>
        /// Gets the item collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> Query<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity
        {
            return _connection.Query<T>(new CommandDefinition(commandText, parameters, transaction, _executionTimeOut, CommandType.Text));
        }

        /// <summary>
        /// Gets the item collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> QueryAsync<T>(string commandText, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity
        {
            return await _connection.QueryAsync<T>(new CommandDefinition(commandText, parameters, transaction, _executionTimeOut, CommandType.Text));
        }

        #endregion

        #region First Query Procedure

        /// <summary>
        /// Gets the single item with stored procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual T FirstProcedure<T>(string procedureName, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity
        {
            return _connection.QueryFirstOrDefault<T>(new CommandDefinition(procedureName, parameters, transaction, _executionTimeOut, CommandType.StoredProcedure));
        }

        /// <summary>
        /// Gets the single item with stored procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<T> FirstProcedureAsync<T>(string procedureName, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity
        {
            return await _connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(procedureName, parameters, transaction, _executionTimeOut, CommandType.StoredProcedure));
        }

        #endregion

        #region Execute Procedure

        /// <summary>
        /// Execute with stored procedure
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual int ExecuteProcedure(string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return _connection.Execute(new CommandDefinition(procedureName, parameters, transaction, _executionTimeOut, CommandType.StoredProcedure));
        }

        /// <summary>
        /// Execute with stored procedure
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<int> ExecuteProcedureAsync(string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return await _connection.ExecuteAsync(new CommandDefinition(procedureName, parameters, transaction, _executionTimeOut, CommandType.StoredProcedure));
        }

        #endregion

        #region ExecuteReader Procedure

        /// <summary>
        /// Execute reader with stored procedure
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual IDataReader ExecuteReaderProcedure(string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return _connection.ExecuteReader(new CommandDefinition(procedureName, parameters, transaction, _executionTimeOut, CommandType.StoredProcedure));
        }

        /// <summary>
        /// Execute reader with stored procedure
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<IDataReader> ExecuteReaderProcedureAsync(string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return await _connection.ExecuteReaderAsync(new CommandDefinition(procedureName, parameters, transaction, _executionTimeOut, CommandType.StoredProcedure));
        }

        #endregion

        #region Execute Scalar Procedure

        /// <summary>
        /// Execute scalar with stored procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual T ExecuteScalarProcedure<T>(string procedureName, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity
        {
            return _connection.ExecuteScalar<T>(new CommandDefinition(procedureName, parameters, transaction, _executionTimeOut, CommandType.StoredProcedure));
        }

        /// <summary>
        /// Execute scalar with stored procedure
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual object ExecuteScalarProcedure(string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return _connection.ExecuteScalar(new CommandDefinition(procedureName, parameters, transaction, _executionTimeOut, CommandType.StoredProcedure));
        }

        /// <summary>
        /// Execute scalar with stored procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<T> ExecuteScalarProcedureAsync<T>(string procedureName, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity
        {
            return await _connection.ExecuteScalarAsync<T>(new CommandDefinition(procedureName, parameters, transaction, _executionTimeOut, CommandType.StoredProcedure));
        }

        /// <summary>
        /// Execute scalar with stored procedure
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<object> ExecuteScalarProcedureAsync(string procedureName, object parameters = null, IDbTransaction transaction = null)
        {
            return await _connection.ExecuteScalarAsync(new CommandDefinition(procedureName, parameters, transaction, _executionTimeOut, CommandType.StoredProcedure));
        }

        #endregion

        #region Query Procedure

        /// <summary>
        /// Gets the item collection with stored procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> QueryProcedure<T>(string procedureName, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity
        {
            return _connection.Query<T>(new CommandDefinition(procedureName, parameters, transaction, _executionTimeOut, CommandType.StoredProcedure));
        }

        /// <summary>
        /// Gets the item collection with stored procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> QueryProcedureAsync<T>(string procedureName, object parameters = null, IDbTransaction transaction = null) where T : BaseEntity
        {
            return await _connection.QueryAsync<T>(new CommandDefinition(procedureName, parameters, transaction, _executionTimeOut, CommandType.StoredProcedure));
        }

        #endregion
    }
}