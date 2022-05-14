using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageCollection.Interfaces
{
    public interface ICollectionItem
    {
        string Name { get; }
        string Description { get; }
        long Size { get; }
        Size Resolution { get; }
        BitmapImage Preview { get; }
        bool IsSelected { get; set; }
    }
}