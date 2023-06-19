using Practice.Foundation.Infrastructure.Responses;

namespace Practice.Foundation.Infrastructure.Interfaces
{
    public interface IPostgresManager
    {
        public bool IsDatabaseExist(string dbName);
        public bool IsTableExists(string tableName);

        public ResponseStatus CreateTable(string tableName, Dictionary<string, string> columns, string dbName = null);
        public ResponseStatus UpsertRecord<T>(T record) where T : new();
        public List<T> ReadDataFromTable<T>(string tablename) where T : class, new();

    }
}
