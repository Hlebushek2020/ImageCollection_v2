using ImageCollection.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace ImageCollection.Models
{
    internal class Collection : ICollection
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public ObservableCollection<ICollectionItem> Items { get; } = new ObservableCollection<ICollectionItem>();

        private CollectionsManager _collectionsManager;

        public Collection(CollectionsManager collectionsManager, string name, IEnumerable<FileInfo> fileInfos)
        {
            Id = Guid.NewGuid();
            _collectionsManager = collectionsManager;
            Name = name;
            foreach (FileInfo fileInfo in fileInfos)
            {
                Items.Add(new CollectionItem(fileInfo));
            }
        }

        public Collection(CollectionsManager collectionsManager, string name, Guid id)
        {
            _collectionsManager = collectionsManager;
            Id = id;
            Name = name;
        }

        public void RemoveSelectedFiles()
        {
            IEnumerable<ICollectionItem> selectedItems = Items.Where(item => item.IsSelected);
            string collectionDirectory = Path.Combine(_collectionsManager.RootDirectory, Name);
            foreach (ICollectionItem collectionItem in selectedItems)
            {
                File.Delete(Path.Combine(collectionDirectory, collectionItem.Name));
                Items.Remove(collectionItem);
            }
        }

        public bool AddItem(ICollectionItem item)
        {
            throw new NotImplementedException();
        }

        public bool RemoveItem(ICollection item)
        {
            throw new NotImplementedException();
        }
    }
}