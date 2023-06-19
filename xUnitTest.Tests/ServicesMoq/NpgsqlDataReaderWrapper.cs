using Npgsql;
using System.Data;
using xUnitTest.Tests.Interfaces;

namespace xUnitTest.Tests.ServicesMoq
{
    public class NpgsqlDataReaderWrapper : IDbDataReaderWrapper
    {
        private readonly NpgsqlDataReader reader;
        private readonly Dictionary<int, string> columnNames;

        public NpgsqlDataReaderWrapper(NpgsqlDataReader reader)
        {
            this.reader = reader;
            columnNames = GetColumnNames(reader);
        }

        public bool Read()
        {
            return reader.Read();
        }

        public int FieldCount => reader.FieldCount;

        public string GetName(int i)
        {
            if (columnNames.TryGetValue(i, out string columnName))
            {
                return columnName;
            }

            return null;
        }

        public object GetValue(int i)
        {
            return reader.GetValue(i);
        }

        public int GetOrdinal(string name)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (GetName(i) == name)
                {
                    return i;
                }
            }

            throw new IndexOutOfRangeException($"Column '{name}' not found in the reader.");
        }

        public void Dispose()
        {
            reader.Dispose();
        }

        private Dictionary<int, string> GetColumnNames(NpgsqlDataReader reader)
        {
            var columnNames = new Dictionary<int, string>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string columnName = reader.GetName(i);
                columnNames.Add(i, columnName);
            }

            return columnNames;
        }
    }
}
