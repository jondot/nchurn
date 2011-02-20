namespace NChurn.Core.Support
{
    public interface IAdapterDataSource
    {
        string GetDataWithQuery(string command);
        void SetContext(DataSourceContextKeys key, object value);
    }
}