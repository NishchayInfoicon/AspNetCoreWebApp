namespace xUnitTest.Tests.Interfaces
{
    public interface IDbDataReaderWrapper : IDisposable
    {
        bool Read();
        int FieldCount { get; }
        string GetName(int i);
        object GetValue(int i);
        int GetOrdinal(string name);
    }

}
