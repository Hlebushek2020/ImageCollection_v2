using System.Collections.ObjectModel;

namespace ImageCollection.Interfaces
{
    internal interface ICollectionsManager
    {
        ObservableCollection<ICollection> Collections { get; }
        ICollection DefaultCollection { get; }
    }
}
