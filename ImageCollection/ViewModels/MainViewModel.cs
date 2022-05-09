using ImageCollection.Interfaces;
using ImageCollection.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
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
        #endregion

        #region Property
        public string Title { get => App.Name; }

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
        public DelegateCommand ToCollection { get; }
        public DelegateCommand CreateCollection { get; }
        public DelegateCommand RenameCollection { get; }
        public DelegateCommand RemoveCollection { get; }
        #endregion

        public MainViewModel()
        {
            OpenFolder = new DelegateCommand(() =>
            {
                System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _collectionsManager = new CollectionsManager(folderBrowserDialog.SelectedPath);
                    Collections = _collectionsManager.Collections;
                    SelectedCollection = _collectionsManager.DefaultCollection;
                }
            });
            CreateCollection = new DelegateCommand(() =>
            {
                AddOrRenameCollectionWindow collectionEdit = new AddOrRenameCollectionWindow(_collectionsManager);
                collectionEdit.ShowDialog();
            });
            RenameCollection = new DelegateCommand(() =>
            {
                AddOrRenameCollectionWindow collectionEdit = new AddOrRenameCollectionWindow(_collectionsManager, SelectedCollection);
                collectionEdit.ShowDialog();
            });
            RemoveCollection = new DelegateCommand(() =>
            {
                if (_selectedCollection != null && _selectedCollection != _collectionsManager.DefaultCollection)
                {
                    if (SUID.MessageBox.Show($"Удалить коллекцию \"{_selectedCollection.Name}\"?", App.Name, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        _collectionsManager.Remove(SelectedCollection);
                    }
                }
            });
            RemoveSelectedFiles = new DelegateCommand(() =>
            {
                SelectedCollection.RemoveSelectedFiles();
            });
            ToCollection = new DelegateCommand(() =>
            {
                CollectionSelectionWindow collectionSelection = new CollectionSelectionWindow(_collectionsManager, _selectedCollection);
                bool? result = collectionSelection.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    _collectionsManager.ToCollection(_selectedCollection, collectionSelection.GetSelectedCollection());
                }
            });
        }
    }
}