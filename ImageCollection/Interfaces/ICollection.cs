using ImageCollection.Models.Structures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Media.Imaging;

namespace ImageCollection.Interfaces
{
    public interface ICollection
    {
        Guid Id { get; }
        string Name { get; }
        Hotkey Hotkey { get; set; }
        ObservableCollection<ICollectionItem> Items { get; }

        void AddItem(ICollectionItem item);
        bool RemoveItem(ICollectionItem item);
        void RemoveFiles(IEnumerable<ICollectionItem> items);
        void RenameFile(ICollectionItem item, string newName);
        void RenameFiles(IEnumerable<ICollectionItem> items, string pattern);

        BitmapImage GetImageOfCollectionItem(ICollectionItem item);
        CancellationTokenSource InitializingPreviewImages();
        bool CheckingNewFileName(ICollectionItem collectionItem, string newName);
    }
}