using System.Collections.ObjectModel;

namespace ImageCollection.Interfaces
{
    public interface ICollectionsManager
    {
        string RootDirectory { get; }
        ObservableCollection<ICollection> Collections { get; }
        ICollection DefaultCollection { get; }

        bool Rename(ICollection collection, string name);
        ICollection Create(string name);
        void Remove(ICollection collection);
        void ToCollection(ICollection from, ICollection to);
    }
}
