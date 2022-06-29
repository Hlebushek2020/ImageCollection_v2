using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageCollection.Interfaces
{
    public interface IImageCollectionItem
    {
        string Name { get; }
        string Description { get; }
        long Size { get; }
        Size Resolution { get; }
        bool IsPreview { get; }
        BitmapImage Preview { get; set; }
        bool IsSelected { get; set; }
    }
}