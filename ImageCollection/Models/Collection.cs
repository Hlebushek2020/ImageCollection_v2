using ImageCollection.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImageCollection.Models
{
    internal class Collection : BindableBase, ICollection, IEquatable<Collection>
    {
        private readonly CollectionsManager _collectionsManager;

        public Guid Id { get; }
        public string Name { get; set; }
        public ObservableCollection<ICollectionItem> Items { get; } = new ObservableCollection<ICollectionItem>();

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

        public void RemoveFiles(IEnumerable<ICollectionItem> items)
        {
            string collectionDirectory = _collectionsManager.RootDirectory;
            if (Equals(_collectionsManager.DefaultCollection))
            {
                collectionDirectory = Path.Combine(_collectionsManager.RootDirectory, Name);
            }
            foreach (ICollectionItem collectionItem in items)
            {
                File.Delete(Path.Combine(collectionDirectory, collectionItem.Name));
                Items.Remove(collectionItem);
            }
        }

        public void AddItem(ICollectionItem item) => Items.Add(item);

        public bool RemoveItem(ICollectionItem item) => Items.Remove(item);

        public BitmapImage GetImageOfCollectionItem(ICollectionItem item)
        {
            string imagePath = Path.Combine(_collectionsManager.RootDirectory, item.Name);
            if (Equals(_collectionsManager.DefaultCollection))
            {
                imagePath = Path.Combine(_collectionsManager.RootDirectory, Name, item.Name);
            }
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(File.ReadAllBytes(imagePath));
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public override bool Equals(object obj) => obj != null && Equals(obj as Collection);

        public bool Equals(Collection other) => other != null && Id.Equals(other.Id);

        public override int GetHashCode() => Id.GetHashCode();
    }
}