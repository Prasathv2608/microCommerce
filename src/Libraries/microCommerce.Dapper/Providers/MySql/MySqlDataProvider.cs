using microCommerce.Domain;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace microCommerce.Dapper.Providers.MySQL
{
    public class MySqlDataProvider : IDataProvider
    {
        #region Constant

        private const string INSERT_QUERY = "INSERT INTO `{0}` ({1}) VALUES(@{2}) SELECT LAST_INSERT_ID()";
        private const string INSERT_BULK_QUERY = "INSERT INTO `{0}` ({1}) VALUES ({2})\r\n";
        private const string UPDATE_QUERY = "UPDATE `{0}` SET {1} WHERE `Id` = @Id";
        private const string UPDATE_BULK_QUERY = "UPDATE `{0}` SET {1} WHERE `Id` = @Id\r\n";
        private const string DELETE_QUERY = "DELETE FROM `{0}` WHERE `Id` = @Id";
        private const string DELETE_BULK_QUERY = "DELETE FROM `{0}` WHERE `Id` IN(@Ids)";
        private const string SELECT_FIRST_QUERY = "SELECT\r\n{1} FROM `{0}` WHERE `Id` = @Id LIMIT 1";
        private const string EXISTING_QUERY = "SELECT CASE WHEN EXISTS (SELECT Id FROM `{0}` WHERE `Id` = @Id) THEN 1 ELSE 0 END";
        private const string COUNT_QUERY = "SELECT COUNT(`Id`) FROM `{0}}`";

        #endregion

        #region Methods

        public virtual IDbConnection CreateConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            return new MySqlConnection(connectionString);
        }

        public virtual string SelectFirstQuery<T>(TableDefination table) where T : BaseEntity
        {
            string formattedColumns = string.Join(",\r\n", table.Columns.Select(p => string.Format("`{0}`", p)));

            return string.Format(SELECT_FIRST_QUERY,
                table.TableName,
                formattedColumns);
        }

        public virtual string InsertQuery(TableDefination table)
        {
            IEnumerable<string> formattedColumns = table.Columns.Select(p => string.Format("`{0}`", p));
            return string.Format(INSERT_QUERY,
                                 table.TableName,
                                 string.Join(", ", formattedColumns),
                                 string.Join(", @", table.Columns));
        }

        public virtual string InsertBulkQuery(TableDefination table)
        {
            IList<string> values = new List<string>();
            StringBuilder builder = new StringBuilder();
            string formattedColumns = string.Join(", ", table.Columns.Select(p => string.Format("`{0}`", p)));
            for (int i = 0; i < table.EntityCount; i++)
            {
                if (i != 0 && i % 100 == 0)
                    builder.Append("GO\r\n");

                string formattedValueColumns = string.Join(", ", table.Columns.Select(p => string.Format("@{0}{1}", p, i + 1)));
                builder.AppendFormat(INSERT_BULK_QUERY,
                                 table.TableName,
                                 formattedColumns,
                                 formattedValueColumns);
            }

            return builder.ToString();
        }

        public virtual string UpdateQuery(TableDefination table)
        {
            string formattedColumns = string.Join(", ", table.Columns.Select(p => string.Format("`{0}` = @{0}", p)));

            return string.Format(UPDATE_QUERY,
                                 table.TableName,
                                 formattedColumns);
        }

        public virtual string UpdateBulkQuery(TableDefination table)
        {
            IList<string> values = new List<string>();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < table.EntityCount; i++)
            {
                if (i != 0 && i % 100 == 0)
                    builder.Append("GO\r\n");

                string formattedColumns = string.Join(", ", table.Columns.Select(p => string.Format("`{0}` = @{0}{1}", p, i + 1)));
                builder.AppendFormat(UPDATE_BULK_QUERY,
                                 table.TableName,
                                 formattedColumns);
            }

            return builder.ToString();
        }

        public virtual string DeleteQuery(string tableName)
        {
            return string.Format(DELETE_QUERY,
                                 tableName);
        }

        public virtual string DeleteBulkQuery(string tableName)
        {
            return string.Format(DELETE_BULK_QUERY,
                                 tableName);
        }

        public virtual string ExistingQuery(string tableName)
        {
            string query = string.Format(EXISTING_QUERY,
                            tableName);

            return query;
        }

        public virtual string CountQuery(string tableName)
        {
            string query = string.Format(COUNT_QUERY,
                            tableName);

            return query;
        }

        #endregion
    }
}