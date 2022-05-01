using ImageCollection.Interfaces;
using ImageCollection.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace ImageCollection.ViewModels
{
    internal class MainViewModel : BindableBase, IWindowTitle
    {
        #region Fields
        private ICollectionsManager _collectionsManager;
        #endregion

        #region Property
        public string Title { get => App.Name; }
        public ObservableCollection<ICollection> Collections { get; set; }
        public ICollection SelectedCollection { get; set; }
        #endregion

        #region Commands
        public DelegateCommand OpenFolder { get; }
        public DelegateCommand CreateCollection { get; }
        public DelegateCommand RenameCollection { get; }
        public DelegateCommand RemoveCollection { get; }
        public DelegateCommand RemoveSelectedFiles { get; }
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
                    RaisePropertyChanged("Collections");
                    SelectedCollection = _collectionsManager.DefaultCollection;
                    RaisePropertyChanged("SelectedCollection");
                }
            });
            CreateCollection = new DelegateCommand(() =>
            {
                CollectionEditWindow collectionEdit = new CollectionEditWindow(_collectionsManager);
                collectionEdit.ShowDialog();
            });
            RenameCollection = new DelegateCommand(() =>
            {
                CollectionEditWindow collectionEdit = new CollectionEditWindow(_collectionsManager, SelectedCollection);
                collectionEdit.ShowDialog();
            });
            RemoveCollection = new DelegateCommand(() =>
            {
                _collectionsManager.Remove(SelectedCollection);
            });
            RemoveSelectedFiles = new DelegateCommand(() =>
            {
                SelectedCollection.RemoveSelectedFiles();
            });
        }
    }
}