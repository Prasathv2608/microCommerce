using System.Collections.Generic;

namespace microCommerce.Dapper
{
    public class TableDefination
    {
        public TableDefination(string tableName, int entityCount, IEnumerable<string> columns)
        {
            TableName = tableName;
            EntityCount = entityCount;
            Columns = columns;
        }

        public TableDefination(string tableName, IEnumerable<string> columns)
        {
            TableName = tableName;
            Columns = columns;
        }

        public string TableName { get; }
        public string IdentityColumn { get; }
        public int EntityCount { get; }
        public IEnumerable<string> Columns { get; }
    }
}