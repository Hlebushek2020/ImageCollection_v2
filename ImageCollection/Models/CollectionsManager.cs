using ImageCollection.Extensions;
using ImageCollection.Interfaces;
using ImageCollection.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace ImageCollection.Models
{
    internal class CollectionsManager : ICollectionsManager
    {
        private readonly HashSet<string> _collectionNames = new HashSet<string>();

        public string RootDirectory { get; private set; }
        public CollectionsHotkeyManager HotkeyManager { get; } = new CollectionsHotkeyManager();
        public ObservableCollection<ICollection> Collections { get; private set; } = new ObservableCollection<ICollection>();
        public ICollection DefaultCollection { get; private set; }

        public CollectionsManager(string folder)
        {
            RootDirectory = folder;
            ProgressViewModel progressViewModel = new ProgressViewModel();
            progressViewModel.DoWork += InitCollectionManager;
            ProgressWindow progressWindow = new ProgressWindow(progressViewModel);
            progressWindow.ShowDialog();
        }

        private void InitCollectionManager(IWorkProgress progress)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(RootDirectory);
            progress.State = $"Создание коллекции: {Settings.DefaultCollectionName}";
            DefaultCollection = new Collection(this, Settings.DefaultCollectionName);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles().WhereIsImage())
            {
                progress.State = $"Добавление: {fileInfo.FullName}";
                DefaultCollection.AddItem(new CollectionItem(fileInfo));
            }
            progress.State = "Чтение и обработка метаданных";
            IcdFile icdFile = IcdFile.Read(Path.Combine(RootDirectory, Settings.IcdFileName));
            if (icdFile != null)
            {
                DefaultCollection.Hotkey = icdFile.Hotkey;
            }
            progress.State = $"Добавление коллекции \"{Settings.DefaultCollectionName}\" в список";
            Collections.Add(DefaultCollection);
            _collectionNames.Add(Settings.DefaultCollectionName.ToLower());
            Collections.Add(DefaultCollection);
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            foreach (DirectoryInfo directory in directoryInfos)
            {
                progress.State = $"Создание коллекции: {directory.Name}";
                Collection collection = new Collection(this, directory.Name);
                foreach (FileInfo fileInfo in directory.GetFiles().WhereIsImage())
                {
                    progress.State = $"Добавление: {fileInfo.FullName}";
                    collection.AddItem(new CollectionItem(fileInfo));
                }
                progress.State = "Чтение и обработка метаданных";
                icdFile = IcdFile.Read(Path.Combine(RootDirectory, directory.Name, Settings.IcdFileName));
                if (icdFile != null)
                {
                    collection.Hotkey = icdFile.Hotkey;
                }
                progress.State = $"Добавление коллекции \"{directory.Name}\" в список";
                Collections.Add(collection);
                _collectionNames.Add(directory.Name.ToLower());
            }
        }

        public bool Rename(ICollection collection, string name)
        {
            if (!_collectionNames.Contains(name))
            {
                collection.StopInitPreviewImages(true);
                Directory.Move(Path.Combine(RootDirectory, collection.Name), Path.Combine(RootDirectory, name));
                _collectionNames.Remove(collection.Name);
                _collectionNames.Add(name.ToLower());
                ((Collection)collection).Name = name;
                collection.InitPreviewImages();
                return true;
            }
            return false;
        }

        public ICollection Create(string name)
        {
            if (!_collectionNames.Contains(name))
            {
                Directory.CreateDirectory(Path.Combine(RootDirectory, name));
                Collection collection = new Collection(this, name);
                Collections.Add(collection);
                _collectionNames.Add(name.ToLower());
                return collection;
            }
            return null;
        }

        public void Remove(ICollection collection)
        {
            collection.StopInitPreviewImages(true);
            if (Settings.Current.MoveItemsFromRemoveCollection)
            {
                CollectionItemMover itemMover = new CollectionItemMover(this, collection, DefaultCollection);
                foreach (ICollectionItem item in collection.Items)
                {
                    itemMover.Move(item);
                }
            }
            if (Settings.Current.DeleteCollectionFolder)
            {
                if (Settings.Current.DeleteCollectionFolderIfEmpty)
                {
                    string collectionDirectory = collection.GetCollectionDirectory();
                    DirectoryInfo previewDirectory = new DirectoryInfo(Path.Combine(collectionDirectory, Settings.PreviewDirectoryName));
                    if (previewDirectory.Exists)
                    {
                        previewDirectory.Delete(true);
                    }
                    FileInfo dataIcd = new FileInfo(Path.Combine(collectionDirectory, Settings.IcdFileName));
                    if (dataIcd.Exists)
                    {
                        dataIcd.Delete();
                    }
                    DirectoryInfo collectionDirectoryInfo = new DirectoryInfo(collectionDirectory);
                    FileInfo[] directoryInfo = collectionDirectoryInfo.GetFiles("*", SearchOption.AllDirectories);
                    if (directoryInfo.Length == 0)
                    {
                        collectionDirectoryInfo.Delete(true);
                    }
                }
                else
                {
                    Directory.Delete(collection.GetCollectionDirectory(), true);
                }
            }
            Collections.Remove(collection);
            _collectionNames.Remove(collection.Name.ToLower());
        }

        public void ToCollection(ICollection from, ICollection to)
        {
            from.StopInitPreviewImages(true);
            IEnumerable<ICollectionItem> items = from.Items.Where(ci => ci.IsSelected);
            CollectionItemMover itemMover = new CollectionItemMover(this, from, to);
            foreach (ICollectionItem item in items)
            {
                itemMover.Move(item);
            }
            from.InitPreviewImages();
        }
    }
}