using ImageCollection.Interfaces;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImageCollection.Models
{
    internal class CollectionItem : ICollectionItem
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; }
        public BitmapImage Preview { get; }
        public bool IsSelected { get; set; }

        public CollectionItem(FileInfo fileInfo)
        {
            Id = Guid.NewGuid();
            Name = fileInfo.Name;
            Description = fileInfo.Length + "";
        }
    }
}