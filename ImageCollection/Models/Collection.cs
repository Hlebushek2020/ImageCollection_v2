using ImageCollection.Interfaces;
using ImageCollection.Models.Structures;
using ImageCollection.Utils;
using ImageCollection.ViewModels;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageCollection.Models
{
    internal class Collection : BindableBase, IImageCollection, IEquatable<Collection>
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
        public ObservableCollection<IImageCollectionItem> Items { get; } = new ObservableCollection<IImageCollectionItem>();

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
        #endregion

        public Collection(CollectionsManager collectionsManager, string name)
        {
            _collectionsManager = collectionsManager;
            Id = Guid.NewGuid();
            Name = name;
        }

        public void RemoveFiles(IEnumerable<IImageCollectionItem> items)
        {
            StopInitPreviewImages(true);
            string collectionDirectory = GetCollectionDirectory();
            foreach (IImageCollectionItem collectionItem in items)
            {
                File.Delete(Path.Combine(collectionDirectory, collectionItem.Name));
                Items.Remove(collectionItem);
            }
            InitPreviewImages();
        }

        public void AddItem(IImageCollectionItem item) => Items.Add(item);

        public bool RemoveItem(IImageCollectionItem item) => Items.Remove(item);

        public BitmapImage GetImageOfCollectionItem(IImageCollectionItem item)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            string imagePath = Path.Combine(GetCollectionDirectory(), item.Name);
            bitmapImage.StreamSource = new MemoryStream(File.ReadAllBytes(imagePath));
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public bool CheckingNewFileName(IImageCollectionItem collectionItem, string newName)
        {
            string filePath = Path.Combine(GetCollectionDirectory(), $"{newName}.{Path.GetExtension(collectionItem.Name)}");
            return File.Exists(filePath);
        }

        public void RenameFile(IImageCollectionItem item, string newName)
        {
            StopInitPreviewImages(true);
            string directoryPath = GetCollectionDirectory();
            string fromPath = Path.Combine(directoryPath, item.Name);
            string toPath = Path.Combine(directoryPath, $"{newName}.{Path.GetExtension(item.Name)}");
            File.Move(fromPath, toPath);
            InitPreviewImages();
        }

        public void RenameFiles(IEnumerable<IImageCollectionItem> items, string pattern)
        {
            StopInitPreviewImages(true);
            ProgressViewModel progressView = new ProgressViewModel();
            progressView.DoWork += delegate (IWorkProgress progress)
            {
                progress.State = "Подготовка";
                string directoryPath = GetCollectionDirectory();
                HashSet<string> checkName = new HashSet<string>();
                int counter = 0;
                foreach (IImageCollectionItem collectionItem in items)
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
                    if (skip)
                    {
                        progress.State = "Пропущен: " + fromPath;
                    }
                    else
                    {
                        progress.State = fromPath + " -> " + toPath;
                        File.Move(fromPath, toPath);
                        ((CollectionItem)collectionItem).Name = newName;
                    }
                }
            };
            ProgressWindow progressWindow = new ProgressWindow(progressView);
            progressWindow.ShowDialog();
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
                    foreach (IImageCollectionItem collectionItem in Items)
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
                                IcUtils.CreateThumbnail(originalPath, collectionItem.Resolution, previewPath);
                            }
                            if (token.IsCancellationRequested)
                            {
                                return;
                            }
                            App.Current.Dispatcher.Invoke((Action<string>)((string _previewPath) =>
                            {
                                collectionItem.Preview = IcUtils.GetThumbnail(_previewPath);
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