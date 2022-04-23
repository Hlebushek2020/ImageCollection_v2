using System.Collections.ObjectModel;

namespace ImageCollection.Interfaces
{
    public interface ICollection
    {
        string Name { get; }
        ObservableCollection<ICollectionItem> Items { get; }
    }
}
