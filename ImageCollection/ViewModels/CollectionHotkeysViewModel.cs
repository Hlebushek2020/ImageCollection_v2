﻿using ImageCollection.Interfaces;
using ImageCollection.Models;
using ImageCollection.Models.Structures;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ImageCollection.ViewModels
{
    internal class CollectionHotkeysViewModel : IWindowTitle
    {
        #region Fields
        private readonly IImageCollectionsManager _collectionsManager;
        private IImageCollection _selectedCollection;
        #endregion

        #region Properties
        public string Title => App.Name;
        public ObservableCollection<IImageCollection> Collections { get; }

        public IImageCollection SelectedCollection
        {
            get => _selectedCollection;
            set
            {
                _selectedCollection = value;
                Edit.RaiseCanExecuteChanged();
                Reset.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Commands
        public DelegateCommand Edit { get; }
        public DelegateCommand Reset { get; }
        #endregion

        public CollectionHotkeysViewModel(IImageCollectionsManager collectionsManager)
        {
            _collectionsManager = collectionsManager;
            Collections = _collectionsManager.Collections;
            Reset = new DelegateCommand(() =>
            {
                if (_selectedCollection.Hotkey.Key != Key.None)
                {
                    _selectedCollection.Hotkey = new Hotkey();
                    if (_selectedCollection is Collection collection)
                    {
                        collection.SaveHotkey();
                    }
                }
            }, () => _selectedCollection != null);
            Edit = new DelegateCommand(() =>
            {
                EditHotkeyWindow editHotkey = new EditHotkeyWindow(_selectedCollection.Hotkey, _collectionsManager.HotkeyManager);
                editHotkey.ShowDialog();
                if (editHotkey.IsAvailableHotkey)
                {
                    _selectedCollection.Hotkey = editHotkey.GetHotkey();
                    if (_selectedCollection is Collection collection)
                    {
                        collection.SaveHotkey();
                    }
                }
            }, () => _selectedCollection != null);
        }
    }
}