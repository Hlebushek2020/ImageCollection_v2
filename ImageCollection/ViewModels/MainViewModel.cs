using ImageCollection.Interfaces;
using ImageCollection.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using SUID = Sergey.UI.Extension.Dialogs;

namespace ImageCollection.ViewModels
{
    internal class MainViewModel : BindableBase, IWindowTitle
    {
        #region Fields
        private ICollectionsManager _collectionsManager;
        private ICollectionItem _selectedCollectionItem;
        private ICollection _selectedCollection;
        private ObservableCollection<ICollection> _collections;
        private BitmapImage _imageOfSelectedCollectionItem;
        private CancellationTokenSource _initializingPreviewImagesCts;
        #endregion

        #region Property
        public string Title { get => App.Name; }

        private ICollectionsManager CollectionsManager
        {
            get { return _collectionsManager; }
            set
            {
                _collectionsManager = value;
                CreateCollection.RaiseCanExecuteChanged();
                Collections = _collectionsManager.Collections;
                SelectedCollection = _collectionsManager.DefaultCollection;
            }
        }

        public ObservableCollection<ICollection> Collections
        {
            get { return _collections; }
            set
            {
                _collections = value;
                RaisePropertyChanged();
            }
        }

        public ICollection SelectedCollection
        {
            get { return _selectedCollection; }
            set
            {
                _selectedCollection = value;
                RaisePropertyChanged();
                RenameCollection.RaiseCanExecuteChanged();
                RemoveCollection.RaiseCanExecuteChanged();
                RenameCollectionFiles.RaiseCanExecuteChanged();
                if (_selectedCollection != null)
                {
                    if (_initializingPreviewImagesCts != null)
                    {
                        _initializingPreviewImagesCts.Cancel();
                    }
                    _initializingPreviewImagesCts = _selectedCollection.InitializingPreviewImages();
                }
            }
        }

        public ICollectionItem SelectedCollectionItem
        {
            get { return _selectedCollectionItem; }
            set
            {
                _selectedCollectionItem = value;
                if (_selectedCollectionItem != null)
                {
                    ImageOfSelectedCollectionItem = _selectedCollection.GetImageOfCollectionItem(_selectedCollectionItem);
                }
                RaisePropertyChanged();
                ToCollection.RaiseCanExecuteChanged();
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
        public DelegateCommand ToCollection { get; }
        public DelegateCommand CreateCollection { get; }
        public DelegateCommand RenameCollection { get; }
        public DelegateCommand RemoveCollection { get; }
        public DelegateCommand RenameCollectionFiles { get; }
        public DelegateCommand CollectionHotkeys { get; }
        public DelegateCommand Settings { get; }
        #endregion

        public MainViewModel()
        {
            OpenFolder = new DelegateCommand(() =>
            {
                System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
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
                AddOrRenameCollectionWindow collectionEdit = new AddOrRenameCollectionWindow(_collectionsManager, _selectedCollection);
                collectionEdit.ShowDialog();
            }, () => _selectedCollection != null);
            RemoveCollection = new DelegateCommand(() =>
            {
                if (_selectedCollection != null && _selectedCollection != _collectionsManager.DefaultCollection)
                {
                    if (SUID.MessageBox.Show($"Удалить коллекцию \"{_selectedCollection.Name}\"?", App.Name, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        _collectionsManager.Remove(_selectedCollection);
                    }
                }
            }, () => _selectedCollection != null);
            RemoveSelectedFiles = new DelegateCommand(() =>
            {
                IReadOnlyList<ICollectionItem> selectedItems = _selectedCollection.Items.Where(item => item.IsSelected).ToList();
                string message = "Удалить выбранные файлы?";
                if (selectedItems.Count == 1)
                {
                    message = $"Удалить {selectedItems[0].Name}?";
                }
                if (SUID.MessageBox.Show(message, App.Name, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _selectedCollection.RemoveFiles(selectedItems);
                }
            }, () => _selectedCollectionItem != null);
            ToCollection = new DelegateCommand(() =>
            {
                CollectionSelectionWindow collectionSelection = new CollectionSelectionWindow(_collectionsManager, _selectedCollection);
                bool? result = collectionSelection.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    _collectionsManager.ToCollection(_selectedCollection, collectionSelection.GetSelectedCollection());
                }
            }, () => _selectedCollectionItem != null);
            RenameSelectedFiles = new DelegateCommand(() =>
            {
                IReadOnlyList<ICollectionItem> selectedFiles = _selectedCollection.Items.Where(ci => ci.IsSelected).ToList();
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
                throw new NotImplementedException();
            });
            Settings = new DelegateCommand(() =>
            {
                throw new NotImplementedException();
            });
        }
    }
}