using Npgsql;
using Practice.Foundation.Infrastructure.Responses;
using Practice.Services.APIManagers.Records;
using System.Xml.Linq;

namespace Practice.Services.APIManagers
{
    public class PostgresManager
    {
        public static string DBName = "Practice";
        private NpgsqlConnection connection;
        public NpgsqlCommand postgresClient;

        #region Validation based operations

        public void CreateDatabase(string dbName)
        {
            if (!IsDatabaseExist(dbName))
            {
                CreateClient();
                postgresClient.CommandText = $"CREATE DATABASE IF NOT EXISTS {dbName}";
                postgresClient.ExecuteNonQuery();
            }
        }

        public bool IsDatabaseExist(string dbName)
        {
            bool dbExists = false;
            CreateClient();
            postgresClient.CommandText = $"Select 1 FROM pg_database Where datname='{dbName}'";
            dbExists = postgresClient.ExecuteScalar() != null;
            return dbExists;
        }

        public bool IsTableExists(string tableName)
        {
            bool tableExists = false;
            CreateClient();
            postgresClient.CommandText = $"SELECT EXISTS (SELECT FROM pg_tables WHERE schemaname = 'public' AND tablename  = '{tableName}');";
            var dbresponse = postgresClient.ExecuteScalar();
            if (dbresponse != null && dbresponse.GetType().Name.ToLower() == "boolean" && dbresponse.ToString().ToLower() == "true")
            {
                tableExists = true;
            }
            return tableExists;
        }

        #endregion

        #region Database and table operations



        #endregion

        #region Schema Operations

        public ResponseStatus CreateTable(string tableName, Dictionary<string, string> columns, string dbName = null)
        {
            ResponseStatus response = new ResponseStatus();
            CreateClient();
            if (string.IsNullOrEmpty(dbName))
            {
                dbName = DBName;
            }
            CreateDatabase(dbName);
            if (!IsTableExists(tableName))
            {
                var createTableSql = $"CREATE TABLE {tableName} (";

                // Add each column with its respective data type to the SQL statement
                foreach (var column in columns)
                {
                    createTableSql += $"{column.Key} {column.Value.ToString().ToUpper()}, ";
                }

                createTableSql = createTableSql.TrimEnd(',', ' '); // Remove trailing comma and space

                createTableSql += ")";
                postgresClient.CommandText = createTableSql;
                postgresClient.ExecuteNonQuery();
                response.IsSuccess = true;
                response.Message = $"Table {tableName} successfully created";
            }
            return response;
        }

        public ResponseStatus UpsertRecord<T>(T record) where T : new()
        {
            CreateClient();
            ResponseStatus response = new ResponseStatus();
            string sql = $"INSERT INTO customer ({GetColumnNames(record)}) VALUES ({GetColumnValues(record)})";
            foreach (var property in record.GetType().GetProperties())
            {
                var propertyName = property.CustomAttributes.ToList().Select(x => x.ConstructorArguments[0]).ToList().FirstOrDefault();
                if (propertyName != null && !string.IsNullOrEmpty(propertyName.Value.ToString()))
                {
                    postgresClient.Parameters.AddWithValue($"@{propertyName.Value.ToString()}", property.GetValue(record));
                }
            }
            postgresClient.CommandText = sql;
            postgresClient.ExecuteNonQuery();
            postgresClient.CommandText = string.Empty;
            postgresClient.Parameters.Clear();
            response.IsSuccess = true;
            response.Message = $"{nameof(Customer)} Data added successfully";

            return response;
        }

        #endregion

        #region Query Modifiers

        private string GetColumnNames<T>(T record) where T : new()
        {
            var columnNames = new List<string>();
            foreach (var property in record.GetType().GetProperties())
            {
                var propertyName = property.CustomAttributes.ToList().Select(x => x.ConstructorArguments[0]).ToList().FirstOrDefault();
                if (propertyName != null && !string.IsNullOrEmpty(propertyName.Value.ToString()))
                {
                    columnNames.Add(propertyName.Value.ToString());
                }
            }

            return string.Join(", ", columnNames);
        }

        private string GetColumnValues<T>(T record) where T : new()
        {
            var columnValues = new List<string>();

            foreach (var property in record.GetType().GetProperties())
            {
                var propertyName = property.CustomAttributes.ToList().Select(x => x.ConstructorArguments[0]).ToList().FirstOrDefault();
                if (propertyName != null && !string.IsNullOrEmpty(propertyName.Value.ToString()))
                {
                    columnValues.Add($"@{propertyName.Value.ToString()}");
                }
            }

            return string.Join(", ", columnValues);
        }

        #endregion

        #region Helpers

        public NpgsqlCommand CreateClient()
        {
            if (connection == null || connection.State != System.Data.ConnectionState.Open)
            {
                OpenConnnection();
                postgresClient = new NpgsqlCommand();
                postgresClient.Connection = connection;

            }
            else if (postgresClient == null)
            {
                postgresClient = new NpgsqlCommand();
                postgresClient.Connection = connection;
            }
            return postgresClient;
        }

        public void OpenConnnection()
        {
            connection = new NpgsqlConnection("User ID =postgres;Password=Lostman@123;Server=localhost;Port=5432;Database=Practice; Integrated Security=true;Pooling=true;");
            connection.Open();
        }

        public void CloseConnection()
        {
            connection.Close();
        }

        #endregion
    }
}