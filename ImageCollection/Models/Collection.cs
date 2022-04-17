using ImageCollection.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace ImageCollection.Models
{
    internal class Collection : ICollection
    {
        public string Name { get; private set; }
        public ObservableCollection<ICollectionItem> Items { get; private set; }

        public Collection(string name, IEnumerable<FileInfo> fileInfos)
        {
            Name = name;
        }
    }
}
