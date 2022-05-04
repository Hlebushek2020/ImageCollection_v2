using ImageCollection.Extensions;
using ImageCollection.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace ImageCollection.Models
{
    internal class CollectionsManager : ICollectionsManager
    {
        private HashSet<string> _collectionNames = new HashSet<string>();

        public string RootDirectory { get; private set; }

        public ObservableCollection<ICollection> Collections { get; private set; } = new ObservableCollection<ICollection>();
        public ICollection DefaultCollection { get; private set; }

        public CollectionsManager(string folder)
        {
            RootDirectory = folder;
            DirectoryInfo directoryInfo = new DirectoryInfo(RootDirectory);
            DefaultCollection = new Collection(this, "not collection", directoryInfo.GetFiles().WhereIsImage());
            _collectionNames.Add("not collection");
            Collections.Add(DefaultCollection);
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            foreach (DirectoryInfo directory in directoryInfos)
            {
                Collections.Add(new Collection(this, directory.Name, directory.GetFiles().WhereIsImage()));
                _collectionNames.Add(directory.Name);
            }
        }

        public bool Rename(ICollection collection, string name)
        {
            if (!_collectionNames.Contains(name))
            {
                Directory.Move(Path.Combine(RootDirectory, collection.Name), Path.Combine(RootDirectory, name));
                ((Collection)collection).Name = name;
                return true;
            }
            return false;
        }

        public ICollection Create(string name)
        {
            if (!_collectionNames.Contains(name))
            {
                Directory.CreateDirectory(Path.Combine(RootDirectory, name));
                Collection collection = new Collection(this, name, Guid.NewGuid());
                Collections.Add(collection);
                _collectionNames.Add(name);
                return collection;
            }
            return null;
        }

        public void Remove(ICollection collection)
        {
            string currentDirectoty = Path.Combine(RootDirectory, collection.Name);
            foreach (ICollectionItem item in collection.Items)
            {
                string fromPath = Path.Combine(currentDirectoty, item.Name);
                string toPath = Path.Combine(RootDirectory, item.Name);
                int counter = 0;
                string newName = null;
                while (File.Exists(toPath))
                {
                    newName = $"{counter}-{item.Name}";
                    toPath = Path.Combine(RootDirectory, newName);
                }
                File.Move(fromPath, toPath);
                if (newName != null)
                {
                    ((CollectionItem)item).Name = newName;
                }
            }
        }
    }
}