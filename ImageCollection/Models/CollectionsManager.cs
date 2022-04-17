using ImageCollection.Extensions;
using ImageCollection.Interfaces;
using System.Collections.ObjectModel;
using System.IO;

namespace ImageCollection.Models
{
    internal class CollectionsManager : ICollectionsManager
    {
        private string _rootDirectory;

        public ObservableCollection<ICollection> Collections { get; private set; } = new ObservableCollection<ICollection>();
        public ICollection DefaultCollection { get; private set; }

        public CollectionsManager(string folder)
        {
            _rootDirectory = folder;
            DirectoryInfo directoryInfo = new DirectoryInfo(_rootDirectory);
            DefaultCollection = new Collection("not collection", directoryInfo.GetFiles().WhereIsImage());
            Collections.Add(DefaultCollection);
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            foreach (DirectoryInfo directory in directoryInfos)
            {
                Collections.Add(new Collection(directory.Name, directory.GetFiles().WhereIsImage()));
            }
        }
    }
}