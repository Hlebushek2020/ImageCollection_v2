using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace ImageCollection.Interfaces
{
    public interface ICollection
    {
        Guid Id { get; }
        string Name { get; }
        ObservableCollection<ICollectionItem> Items { get; }

        void AddItem(ICollectionItem item);
        bool RemoveItem(ICollectionItem item);
        void RemoveFiles(IEnumerable<ICollectionItem> items);

        BitmapImage GetImageOfCollectionItem(ICollectionItem item);
    }
}