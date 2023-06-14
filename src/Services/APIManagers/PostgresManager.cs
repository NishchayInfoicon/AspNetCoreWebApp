using Npgsql;
using Practice.Foundation.Infrastructure.Responses;
using Practice.Services.APIManagers.Records;

namespace Practice.Services.APIManagers
{
    public class PostgresManager
    {
        private NpgsqlConnection connection;
        public NpgsqlCommand postgresClient;

        #region Schema Operations

        public ResponseStatus CreateTable(string tableName, Dictionary<string, string> columns)
        {
            ResponseStatus response = new ResponseStatus();
            CreateClient();
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