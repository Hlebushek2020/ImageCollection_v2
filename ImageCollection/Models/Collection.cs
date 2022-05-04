using ImageCollection.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using SUID = Sergey.UI.Extension.Dialogs;

namespace ImageCollection.Models
{
    internal class Collection : BindableBase, ICollection
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
            IReadOnlyList<ICollectionItem> selectedItems = Items.Where(item => item.IsSelected).ToList();
            string message = "Удалить выбранные файлы?";
            if (selectedItems.Count == 1)
            {
                message = $"Удалить {selectedItems[0].Name}?";
            }
            if (SUID.MessageBox.Show(message, App.Name, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                string collectionDirectory = Path.Combine(_collectionsManager.RootDirectory, Name);
                foreach (ICollectionItem collectionItem in selectedItems)
                {
                    File.Delete(Path.Combine(collectionDirectory, collectionItem.Name));
                    Items.Remove(collectionItem);
                }
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

        public override bool Equals(object obj) => (obj is Collection).Equals(Id);

        public override int GetHashCode() => Id.GetHashCode();

    }
}