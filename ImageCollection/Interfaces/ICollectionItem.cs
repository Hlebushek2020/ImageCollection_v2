using System.Windows.Media.Imaging;

namespace ImageCollection.Interfaces
{
    public interface ICollectionItem
    {
        string Name { get; }
        string Description { get; }
        BitmapImage Preview { get; }
        bool IsSelected { get; }
    }
}