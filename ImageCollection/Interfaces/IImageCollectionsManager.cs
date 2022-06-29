using ImageCollection.Models;
using System.Collections.ObjectModel;

namespace ImageCollection.Interfaces
{
    public interface IImageCollectionsManager
    {
        string RootDirectory { get; }
        CollectionsHotkeyManager HotkeyManager { get; }
        ObservableCollection<IImageCollection> Collections { get; }
        IImageCollection DefaultCollection { get; }

        bool Rename(IImageCollection collection, string name);
        IImageCollection Create(string name);
        void Remove(IImageCollection collection);
        void ToCollection(IImageCollection from, IImageCollection to);
    }
}
