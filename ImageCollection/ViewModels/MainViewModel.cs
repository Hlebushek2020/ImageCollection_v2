using ImageCollection.Interfaces;
using ImageCollection.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using SUID = Sergey.UI.Extension.Dialogs;

namespace ImageCollection.ViewModels
{
    internal class MainViewModel : BindableBase, IWindowTitle
    {
        #region Fields
        private IImageCollectionsManager _collectionsManager;
        private IImageCollectionItem _selectedCollectionItem;
        private IImageCollection _selectedCollection;
        private ObservableCollection<IImageCollection> _collections;
        private BitmapImage _imageOfSelectedCollectionItem;
        #endregion

        #region Property
        public string Title => App.Name;

        public IImageCollectionsManager CollectionsManager
        {
            get { return _collectionsManager; }
            private set
            {
                _collectionsManager = value;
                CreateCollection.RaiseCanExecuteChanged();
                CollectionHotkeys.RaiseCanExecuteChanged();
                Collections = _collectionsManager.Collections;
                SelectedCollection = _collectionsManager.DefaultCollection;
            }
        }

        public ObservableCollection<IImageCollection> Collections
        {
            get { return _collections; }
            set
            {
                _collections = value;
                RaisePropertyChanged();
            }
        }

        public IImageCollection SelectedCollection
        {
            get { return _selectedCollection; }
            set
            {
                if (_selectedCollection != null)
                {
                    _selectedCollection.StopInitPreviewImages();
                }
                _selectedCollection = value;
                RaisePropertyChanged();
                RenameCollection.RaiseCanExecuteChanged();
                RemoveCollection.RaiseCanExecuteChanged();
                RenameCollectionFiles.RaiseCanExecuteChanged();
                if (_selectedCollection != null)
                {
                    _selectedCollection.InitPreviewImages();
                }
            }
        }

        public IImageCollectionItem SelectedCollectionItem
        {
            get { return _selectedCollectionItem; }
            set
            {
                _selectedCollectionItem = value;
                if (_selectedCollectionItem != null)
                {
                    ImageOfSelectedCollectionItem =
                        _selectedCollection.GetImageOfCollectionItem(_selectedCollectionItem);
                }
                RaisePropertyChanged();
                ToCollection.RaiseCanExecuteChanged();
                SearchInInternet.RaiseCanExecuteChanged();
                RemoveSelectedFiles.RaiseCanExecuteChanged();
                RenameSelectedFiles.RaiseCanExecuteChanged();
            }
        }

        public BitmapImage ImageOfSelectedCollectionItem
        {
            get { return _imageOfSelectedCollectionItem; }
            private set
            {
                _imageOfSelectedCollectionItem = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public DelegateCommand OpenFolder { get; }
        public DelegateCommand RemoveSelectedFiles { get; }
        public DelegateCommand RenameSelectedFiles { get; }
        public DelegateCommand SearchInInternet { get; }
        public DelegateCommand ToCollection { get; }
        public DelegateCommand CreateCollection { get; }
        public DelegateCommand RenameCollection { get; }
        public DelegateCommand RemoveCollection { get; }
        public DelegateCommand RenameCollectionFiles { get; }
        public DelegateCommand CollectionHotkeys { get; }
        public DelegateCommand Settings { get; }
        public DelegateCommand ResetSorting { get; }
        public DelegateCommand SortByName { get; }
        public DelegateCommand SortBySize { get; }
        public DelegateCommand SortByResolution { get; }
        #endregion

        public MainViewModel()
        {
            OpenFolder = new DelegateCommand(() =>
            {
                System.Windows.Forms.FolderBrowserDialog folderBrowserDialog =
                    new System.Windows.Forms.FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _selectedCollection?.StopInitPreviewImages();
                    ImageOfSelectedCollectionItem = null;
                    CollectionsManager = new CollectionsManager(folderBrowserDialog.SelectedPath);
                }
            });
            CreateCollection = new DelegateCommand(() =>
            {
                AddOrRenameCollectionWindow collectionEdit = new AddOrRenameCollectionWindow(_collectionsManager);
                collectionEdit.ShowDialog();
            }, () => _collectionsManager != null);
            RenameCollection = new DelegateCommand(() =>
            {
                AddOrRenameCollectionWindow collectionEdit =
                    new AddOrRenameCollectionWindow(_collectionsManager, _selectedCollection);
                collectionEdit.ShowDialog();
            }, () => _selectedCollection != null && _selectedCollection != _collectionsManager.DefaultCollection);
            RemoveCollection = new DelegateCommand(() =>
            {
                if (SUID.MessageBox.Show($"Удалить коллекцию \"{_selectedCollection.Name}\"?", App.Name,
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _collectionsManager.Remove(_selectedCollection);
                    SelectedCollection = CollectionsManager.DefaultCollection;
                }
            }, () => _selectedCollection != null && _selectedCollection != _collectionsManager.DefaultCollection);
            RemoveSelectedFiles = new DelegateCommand(() =>
            {
                IReadOnlyList<IImageCollectionItem> selectedItems =
                    _selectedCollection.Items.Where(item => item.IsSelected).ToList();
                string message = "Удалить выбранные файлы?";
                if (selectedItems.Count == 1)
                {
                    message = $"Удалить {selectedItems[0].Name}?";
                }
                if (SUID.MessageBox.Show(message, App.Name, MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                    MessageBoxResult.Yes)
                {
                    _selectedCollection.RemoveFiles(selectedItems);
                }
            }, () => _selectedCollectionItem != null);
            ToCollection = new DelegateCommand(() =>
            {
                CollectionSelectionWindow collectionSelection =
                    new CollectionSelectionWindow(_collectionsManager, _selectedCollection);
                bool? result = collectionSelection.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    ICollectionView collectionView = CollectionViewSource.GetDefaultView(_selectedCollection.Items);
                    int currentIndex = collectionView.CurrentPosition;
                    _collectionsManager.ToCollection(_selectedCollection, collectionSelection.GetSelectedCollection());
                    collectionView.MoveCurrentToPosition(Math.Min(currentIndex, _selectedCollection.Items.Count - 1));
                }
            }, () => _selectedCollectionItem != null);
            SearchInInternet = new DelegateCommand(() =>
                {
                    try
                    {
                        string currentFile = Path.Combine(_selectedCollection.GetCollectionDirectory(),
                            _selectedCollectionItem.Name);
                        string searchCommand = string.Format(Models.Settings.Current.SearchCommand, currentFile);
                        int startIndex = searchCommand[0] == '"' ? 1 : 0;
                        int endIndex = searchCommand.IndexOf(".exe");
                        int argStartIndex = searchCommand[endIndex + 4] == '"' ? endIndex + 5 : endIndex + 4;
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = searchCommand.Substring(startIndex, endIndex + 3).Trim(),
                            Arguments = searchCommand.Substring(argStartIndex).Trim()
                        });
                    }
                    catch
                    {
                        SUID.MessageBox.Show("Невозможно выполнить команду поиска, проверьте ее правильность!",
                            App.Name, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                },
                () => _selectedCollectionItem != null &&
                      !string.IsNullOrWhiteSpace(Models.Settings.Current.SearchCommand));
            RenameSelectedFiles = new DelegateCommand(() =>
            {
                IReadOnlyList<IImageCollectionItem> selectedFiles =
                    _selectedCollection.Items.Where(ci => ci.IsSelected).ToList();
                RenameFilesWindow renameFiles = new RenameFilesWindow();
                if (selectedFiles.Count == 1)
                {
                    renameFiles = new RenameFilesWindow(_selectedCollectionItem, _selectedCollection);
                }
                bool? result = renameFiles.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    string newNameOrPattern = renameFiles.GetNewNameOrPattern();
                    if (selectedFiles.Count > 1)
                    {
                        _selectedCollection.RenameFile(_selectedCollectionItem, newNameOrPattern);
                    }
                    else
                    {
                        _selectedCollection.RenameFiles(selectedFiles, newNameOrPattern);
                    }
                }
            }, () => _selectedCollectionItem != null);
            RenameCollectionFiles = new DelegateCommand(() =>
            {
                RenameFilesWindow renameFiles = new RenameFilesWindow();
                bool? result = renameFiles.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    _selectedCollection.RenameFiles(_selectedCollection.Items, renameFiles.GetNewNameOrPattern());
                }
            }, () => _selectedCollection != null);
            CollectionHotkeys = new DelegateCommand(() =>
            {
                CollectionHotkeysWindow collectionHotkeys = new CollectionHotkeysWindow(_collectionsManager);
                collectionHotkeys.ShowDialog();
            }, () => _collectionsManager != null);
            Settings = new DelegateCommand(() =>
            {
                SettingsWindow settings = new SettingsWindow();
                settings.ShowDialog();
            });
            ResetSorting = new DelegateCommand(() =>
            {
                ICollectionView collectionView = CollectionViewSource.GetDefaultView(_selectedCollection.Items);
                collectionView.SortDescriptions.Clear();
            });
            SortByName = new DelegateCommand(() =>
            {
                ICollectionView collectionView = CollectionViewSource.GetDefaultView(_selectedCollection.Items);
                collectionView.SortDescriptions.Clear();
                collectionView.SortDescriptions.Add(new SortDescription(nameof(IImageCollectionItem.Name),
                    ListSortDirection.Descending));
            });
            SortBySize = new DelegateCommand(() =>
            {
                ICollectionView collectionView = CollectionViewSource.GetDefaultView(_selectedCollection.Items);
                collectionView.SortDescriptions.Clear();
                collectionView.SortDescriptions.Add(new SortDescription(nameof(IImageCollectionItem.Size),
                    ListSortDirection.Descending));
            });
            SortByResolution = new DelegateCommand(() =>
            {
                ICollectionView collectionView = CollectionViewSource.GetDefaultView(_selectedCollection.Items);
                collectionView.SortDescriptions.Clear();
                collectionView.SortDescriptions.Add(new SortDescription(nameof(IImageCollectionItem.Resolution),
                    ListSortDirection.Descending));
            });
        }

        public void LoadSession(Session session)
        {
            CollectionsManager = new CollectionsManager(session.BaseDirectory);
            bool isContinue = session.Collection.Equals(_collectionsManager.DefaultCollection.Name,
                StringComparison.OrdinalIgnoreCase);
            if (session.Collection != null && !isContinue)
            {
                IImageCollection collection = _collectionsManager.Collections.FirstOrDefault(col =>
                    col.Name.Equals(session.Collection, StringComparison.OrdinalIgnoreCase));
                if (collection != null)
                {
                    SelectedCollection = collection;
                    isContinue = true;
                }
            }
            if (session.Item != null && isContinue)
            {
                IImageCollectionItem collectionItem = _selectedCollection.Items.FirstOrDefault(item =>
                    item.Name.Equals(session.Item, StringComparison.OrdinalIgnoreCase));
                if (collectionItem != null)
                {
                    SelectedCollectionItem = collectionItem;
                }
            }
        }
    }
}