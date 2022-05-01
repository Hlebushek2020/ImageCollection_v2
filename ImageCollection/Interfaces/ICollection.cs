using System.Collections.ObjectModel;

namespace ImageCollection.Interfaces
{
    public interface ICollection
    {
        string Name { get; }
        ObservableCollection<ICollectionItem> Items { get; }

        void RemoveSelectedFiles();
        bool AddItem(ICollectionItem item);
        bool RemoveItem(ICollection item);
    }
}
