using ImageCollection.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace ImageCollection.Models
{
    internal class Collection : ICollection
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public ObservableCollection<ICollectionItem> Items { get; } = new ObservableCollection<ICollectionItem>();

        public Collection(string name, IEnumerable<FileInfo> fileInfos)
        {
            Id = Guid.NewGuid();
            Name = name;
            foreach (FileInfo fileInfo in fileInfos)
            {
                Items.Add(new CollectionItem(fileInfo));
            }
        }

        public Collection(string name, Guid id)
        {
            Id = id;
            Name = name;
        }
    }
}