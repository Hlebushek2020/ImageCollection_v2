using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace ImageCollection.Interfaces
{
    public interface ICollection
    {
        Guid Id { get; }
        string Name { get; }
        ObservableCollection<ICollectionItem> Items { get; }

        void RemoveSelectedFiles();
        bool AddItem(ICollectionItem item);
        bool RemoveItem(ICollection item);

        BitmapImage GetImageOfCollectionItem(ICollectionItem item);
    }
}