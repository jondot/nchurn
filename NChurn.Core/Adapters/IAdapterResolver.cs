namespace NChurn.Core.Adapters
{
    public interface IAdapterResolver
    {
        IVersioningAdapter CreateAdapter();
    }
}