using System.Collections.ObjectModel;

namespace ImageCollection.Interfaces
{
    internal interface ICollection
    {
        string Name { get; }
        ObservableCollection<ICollectionItem> Items { get; }
    }
}
