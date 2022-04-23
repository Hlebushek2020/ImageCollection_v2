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

        private string _rootDirectory;

        public ObservableCollection<ICollection> Collections { get; private set; } = new ObservableCollection<ICollection>();
        public ICollection DefaultCollection { get; private set; }

        public CollectionsManager(string folder)
        {
            _rootDirectory = folder;
            DirectoryInfo directoryInfo = new DirectoryInfo(_rootDirectory);
            DefaultCollection = new Collection("not collection", directoryInfo.GetFiles().WhereIsImage());
            _collectionNames.Add("not collection");
            Collections.Add(DefaultCollection);
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            foreach (DirectoryInfo directory in directoryInfos)
            {
                Collections.Add(new Collection(directory.Name, directory.GetFiles().WhereIsImage()));
                _collectionNames.Add(directory.Name);
            }
        }

        public bool Rename(ICollection collection, string name)
        {
            if (!_collectionNames.Contains(name))
            {
                Directory.Move(Path.Combine(_rootDirectory, collection.Name), Path.Combine(_rootDirectory, name));
                ((Collection)collection).Name = name;
                return true;
            }
            return false;
        }

        public ICollection Create(string name)
        {
            if (!_collectionNames.Contains(name))
            {
                Directory.CreateDirectory(Path.Combine(_rootDirectory, name));
                Collection collection = new Collection(name, Guid.NewGuid());
                Collections.Add(collection);
                _collectionNames.Add(name);
                return collection;
            }
            return null;
        }

        public void Remove(ICollection collection)
        {
            throw new NotImplementedException();
        }
    }
}