using Moq;
using Practice.Foundation.Infrastructure.Interfaces;
using Practice.Foundation.Infrastructure.Responses;
using Practice.Services.APIManagers.Records;
using System.Reflection;
using xUnitTest.Tests.Interfaces;

namespace xUnitTest.Tests.ServicesMoq
{
    public class MoqPostgresManager : IPostgresManager
    {
        public ResponseStatus CreateTable(string tableName, Dictionary<string, string> columns, string dbName = null)
        {
            throw new NotImplementedException();
        }

        public bool IsDatabaseExist(string dbName)
        {
            throw new NotImplementedException();
        }

        public bool IsTableExists(string tableName)
        {
            throw new NotImplementedException();
        }
        private IDbDataReaderWrapper CreateMockDataReader()
        {
            // Create a list of dictionaries representing rows in the database table
            var rows = new List<Customer>
        {
            new Customer
            {
                first_name = "Nishu",
                last_name = "Tanwar",
                email = "nishuchauhan1@gmail.com",
                company = "Infoicon Technologies Pvt. Ltd.",
                street = "Bank wali gali",
                city = "New Delhi",
                state = "Delhi",
                zip = 110036,
                phone = "8861099544",
                birth_date = DateTime.UtcNow,
                gender = 'M',
                date_entered = DateTime.UtcNow,
                id = 1
            }
            // Add more rows as needed for your test cases
        };

            // Create a mock NpgsqlDataReader using the list of rows
            var mockDataReader = new Mock<IDbDataReaderWrapper>();
            mockDataReader.SetupSequence(m => m.Read())
                .Returns(true)  // Return true for the first row
                .Returns(true)  // Return true for the second row
                .Returns(false); // Return false to indicate the end of the data

            mockDataReader.Setup(m => m.FieldCount).Returns(typeof(Customer).GetProperties().Length);

            // Setup the mockDataReader to return values from the rows
            foreach (var row in rows)
            {
                foreach (var property in typeof(Customer).GetProperties())
                {
                    var value = property.GetValue(row);
                    mockDataReader.Setup(m => m.GetOrdinal(property.Name)).Returns(Array.IndexOf(typeof(Customer).GetProperties(), property));
                    mockDataReader.Setup(m => m.GetValue(Array.IndexOf(typeof(Customer).GetProperties(), property))).Returns(value);
                }
                
            }

            return mockDataReader.Object;
        }

        public List<T> ReadDataFromTable<T>(string tablename) where T : class, new()
        {
            List<T> result = new List<T>();

            var reader = CreateMockDataReader();
            while (reader.Read())
            {
                T instance = new T();
                PropertyInfo[] properties = instance.GetType().GetProperties();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = properties[i].Name;
                    PropertyInfo property = typeof(T).GetProperty(columnName);
                    if (property != null && reader.GetValue(i) != DBNull.Value)
                    {
                        object value = reader.GetValue(i);
                        property.SetValue(instance, Convert.ChangeType(value, property.PropertyType));
                    }
                }

                result.Add(instance);
            }

            return result;
        }

        public ResponseStatus UpsertRecord<T>(T record) where T : new()
        {
            ResponseStatus response = new ResponseStatus();
            string sql = $"INSERT INTO customer ({GetColumnNames(record)}) VALUES ({GetColumnValues(record)})";
            if (sql == "INSERT INTO customer (first_name, last_name, email, company, street, city, state, zip, phone, birth_date, gender, date_entered, id) VALUES (@first_name, @last_name, @email, @company, @street, @city, @state, @zip, @phone, @birth_date, @gender, @date_entered, @id)")
            {
                response.IsSuccess = true;
                response.Message = $"{nameof(Customer)} Data added successfully";
            }
            response.IsSuccess = true;
            response.Message = "Failed top add data successfully";
            return response;
        }
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

        public ResponseStatus DeleteIdByRow(string tableName, int id)
        {
            ResponseStatus response = new ResponseStatus();
            if (!string.IsNullOrEmpty(tableName))
            {
                response.IsSuccess = true;
                response.Message = $"Deleted 1 row(s).";
                return response;
            }
            response.IsSuccess = false;
            response.Message = $"Failed to delete for {id}";
            return response;
        }
    }
}
