using ImageCollection.Interfaces;
using ImageCollection.Models.Structures;
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
        #region Field
        private readonly CollectionsManager _collectionsManager;

        private Hotkey _hotkey;
        private string _name;

        private Task _taskInitPreviewImages;
        private CancellationTokenSource _ctsInitPreviewImages;
        private bool _isPreviousCompleted;
        #endregion

        #region Properties
        public Guid Id { get; }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
        public Hotkey Hotkey
        {
            get { return _hotkey; }
            set
            {
                _collectionsManager.HotkeyManager.Register(_hotkey, value, this);
                _hotkey = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<ICollectionItem> Items { get; } = new ObservableCollection<ICollectionItem>();
        #endregion

        public Collection(CollectionsManager collectionsManager, string name)
        {
            _collectionsManager = collectionsManager;
            Id = Guid.NewGuid();
            Name = name;
        }

        public void RemoveFiles(IEnumerable<ICollectionItem> items)
        {
            StopInitPreviewImages(true);
            string collectionDirectory = GetCollectionDirectory();
            foreach (ICollectionItem collectionItem in items)
            {
                File.Delete(Path.Combine(collectionDirectory, collectionItem.Name));
                Items.Remove(collectionItem);
            }
            InitPreviewImages();
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
            StopInitPreviewImages(true);
            string directoryPath = GetCollectionDirectory();
            string fromPath = Path.Combine(directoryPath, item.Name);
            string toPath = Path.Combine(directoryPath, $"{newName}.{Path.GetExtension(item.Name)}");
            File.Move(fromPath, toPath);
            InitPreviewImages();
        }

        public void RenameFiles(IEnumerable<ICollectionItem> items, string pattern)
        {
            StopInitPreviewImages(true);
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
            InitPreviewImages();
        }

        public void InitPreviewImages()
        {
            if (Items.Count != 0 && (!_isPreviousCompleted || _taskInitPreviewImages == null || _taskInitPreviewImages.IsCompleted))
            {
                _ctsInitPreviewImages = new CancellationTokenSource();
                CancellationToken token = _ctsInitPreviewImages.Token;
                _taskInitPreviewImages = Task.Run(() =>
                {
                    string collectionDirectory = GetCollectionDirectory();
                    string previewDirectory = Path.Combine(collectionDirectory, Settings.PreviewDirectoryName);
                    Directory.CreateDirectory(previewDirectory);
                    foreach (ICollectionItem collectionItem in Items)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }
                        if (!collectionItem.IsPreview)
                        {
                            string previewPath = Path.Combine(previewDirectory, $"{Path.GetFileNameWithoutExtension(collectionItem.Name)}.jpg");
                            if (!File.Exists(previewPath))
                            {
                                string originalPath = Path.Combine(collectionDirectory, collectionItem.Name);
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
                            if (token.IsCancellationRequested)
                            {
                                return;
                            }
                            App.Current.Dispatcher.Invoke((Action<string>)((string _previewPath) =>
                            {
                                BitmapImage preview = new BitmapImage();
                                preview.BeginInit();
                                preview.StreamSource = new MemoryStream(File.ReadAllBytes(previewPath));
                                preview.EndInit();
                                collectionItem.Preview = preview;
                            }), previewPath);
                        }
                    }
                }, token);
            }
        }

        public void StopInitPreviewImages(bool isWait = false)
        {
            if (_taskInitPreviewImages != null)
            {
                _isPreviousCompleted = _taskInitPreviewImages.IsCompleted;
                if (!_taskInitPreviewImages.IsCompleted)
                {
                    _ctsInitPreviewImages.Cancel();
                    if (isWait)
                    {
                        _taskInitPreviewImages.Wait();
                    }
                }
            }
        }

        public override bool Equals(object obj) => obj != null && Equals(obj as Collection);

        public bool Equals(Collection other) => other != null && Id.Equals(other.Id);

        public override int GetHashCode() => Id.GetHashCode();

        public void SaveHotkey() => IcdFile.WriteHotkey(Path.Combine(GetCollectionDirectory(), Settings.IcdFileName), _hotkey);

        public string GetCollectionDirectory()
        {
            if (!Equals(_collectionsManager.DefaultCollection))
            {
                return Path.Combine(_collectionsManager.RootDirectory, Name);
            }
            return _collectionsManager.RootDirectory;
        }
    }
}