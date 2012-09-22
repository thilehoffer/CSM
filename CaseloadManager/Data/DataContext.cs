namespace CaseloadManager.Data
{

    internal static class DataContext
    {
        internal static CaseloadManager.Entities.Context GetContext()
        {
            return new Entities.Context();
        }
    }
}