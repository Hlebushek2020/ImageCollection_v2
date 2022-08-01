using ImageCollection.Models.Structures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace ImageCollection.Interfaces
{
    public interface IImageCollection
    {
        Guid Id { get; }
        string Name { get; }
        Hotkey Hotkey { get; set; }
        ObservableCollection<IImageCollectionItem> Items { get; }

        void AddItem(IImageCollectionItem item);
        bool RemoveItem(IImageCollectionItem item);
        void RemoveFiles(IEnumerable<IImageCollectionItem> items);
        void RenameFile(IImageCollectionItem item, string newName);
        void RenameFiles(IEnumerable<IImageCollectionItem> items, string pattern);

        BitmapImage GetImageOfCollectionItem(IImageCollectionItem item);
        bool CheckingNewFileName(IImageCollectionItem collectionItem, string newName);
        void InitPreviewImages();
        void StopInitPreviewImages(bool isWait = false);
        string GetCollectionDirectory();
    }
}