using ImageCollection.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
            string collectionDirectory = GetCollectionDirectory();
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
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            string imagePath = Path.Combine(GetCollectionDirectory(), item.Name);
            bitmapImage.StreamSource = new MemoryStream(File.ReadAllBytes(imagePath));
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public bool CheckingNewFileName(ICollectionItem collectionItem, string newName)
        {
            string filePath = Path.Combine(GetCollectionDirectory(), $"{newName}.{Path.GetExtension(collectionItem.Name)}");
            return File.Exists(filePath);
        }

        public void RenameFile(ICollectionItem item, string newName)
        {
            string directoryPath = GetCollectionDirectory();
            string fromPath = Path.Combine(directoryPath, item.Name);
            string toPath = Path.Combine(directoryPath, $"{newName}.{Path.GetExtension(item.Name)}");
            File.Move(fromPath, toPath);
        }

        public void RenameFiles(IEnumerable<ICollectionItem> items, string pattern)
        {
            string directoryPath = GetCollectionDirectory();
            HashSet<string> checkName = new HashSet<string>();
            int counter = 0;
            foreach (ICollectionItem collectionItem in items)
            {
                string extension = Path.GetExtension(collectionItem.Name);
                string fromPath = Path.Combine(directoryPath, collectionItem.Name);
                bool skip = false;
                string newName;
                string toPath;
                do
                {
                    newName = $"{string.Format(pattern, counter)}.{extension}";
                    checkName.Add(newName);
                    toPath = Path.Combine(directoryPath, newName);
                    if (checkName.Contains(collectionItem.Name))
                    {
                        skip = true;
                    }
                    else
                    {
                        counter++;
                    }
                } while (File.Exists(toPath) && !skip);
                if (!skip)
                {
                    File.Move(fromPath, toPath);
                    ((CollectionItem)collectionItem).Name = newName;
                }
            }
        }

        public CancellationTokenSource InitializingPreviewImages()
        {
            if (Items.Count == 0)
            {
                return null;
            }
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            Task.Run(() =>
            {
                string previewDirectory = Path.Combine(GetCollectionDirectory(), "IC_PREVIEW");
                Directory.CreateDirectory(previewDirectory);
                foreach (ICollectionItem collectionItem in Items)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    if (collectionItem.Preview == null)
                    {
                        string previewPath = Path.Combine(previewDirectory, $"{Path.GetFileNameWithoutExtension(collectionItem.Name)}.jpg");
                        if (!File.Exists(previewPath))
                        {
                            string originalPath = Path.Combine(previewDirectory, collectionItem.Name);
                            byte[] originalBuffer = File.ReadAllBytes(originalPath);
                            Size resolutionSize = collectionItem.Resolution;
                            int previewWidth = (int)(resolutionSize.Width / resolutionSize.Height * 94.0);
                            BitmapImage convert = new BitmapImage();
                            convert.BeginInit();
                            convert.StreamSource = new MemoryStream(originalBuffer);
                            convert.DecodePixelHeight = 94;
                            convert.DecodePixelWidth = previewWidth;
                            convert.EndInit();
                            JpegBitmapEncoder previewEncoder = new JpegBitmapEncoder();
                            previewEncoder.Frames.Add(BitmapFrame.Create(convert));
                            using (FileStream previewStream = new FileStream(previewPath, FileMode.Create, FileAccess.Write))
                            {
                                previewEncoder.Save(previewStream);
                            }
                        }
                        BitmapImage preview = new BitmapImage();
                        preview.BeginInit();
                        preview.StreamSource = new MemoryStream(File.ReadAllBytes(previewPath));
                        preview.EndInit();
                        collectionItem.Preview = preview;
                    }
                }
            }, token);
            return cts;
        }

        public override bool Equals(object obj) => obj != null && Equals(obj as Collection);

        public bool Equals(Collection other) => other != null && Id.Equals(other.Id);

        public override int GetHashCode() => Id.GetHashCode();

        private string GetCollectionDirectory()
        {
            if (!Equals(_collectionsManager.DefaultCollection))
            {
                return Path.Combine(_collectionsManager.RootDirectory, Name);
            }
            return _collectionsManager.RootDirectory;
        }
    }
}