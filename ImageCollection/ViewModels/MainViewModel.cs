﻿using ImageCollection.Interfaces;
using ImageCollection.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows;
using SUID = Sergey.UI.Extension.Dialogs;

namespace ImageCollection.ViewModels
{
    internal class MainViewModel : BindableBase, IWindowTitle
    {
        #region Fields
        private ICollectionsManager _collectionsManager;

        private ICollection _selectedCollection;
        private ObservableCollection<ICollection> _collections;
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
                    SelectedCollection = _collectionsManager.DefaultCollection;
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
        }
    }
}