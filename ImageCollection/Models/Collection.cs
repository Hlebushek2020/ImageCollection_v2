using ImageCollection.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace ImageCollection.Models
{
    internal class Collection : ICollection
    {
        public string Name { get; set; }
        public ObservableCollection<ICollectionItem> Items { get; private set; }

        public Collection(string name, IEnumerable<FileInfo> fileInfos)
        {
            Name = name;
        }

        public Collection(string name, Guid guid)
        {
            Name = name;
        }
    }
}
