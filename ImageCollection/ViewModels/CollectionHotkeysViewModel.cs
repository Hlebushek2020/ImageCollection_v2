using ImageCollection.Interfaces;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;

namespace ImageCollection.ViewModels
{
    internal class CollectionHotkeysViewModel : IWindowTitle
    {
        #region Fields
        private readonly ICollectionsManager _collectionsManager;
        private ICollection _selectedCollection;
        #endregion

        #region Properties
        public string Title { get { return App.Name; } }
        public ObservableCollection<ICollection> Collections { get; }
        public ICollection SelectedCollection
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
        DelegateCommand Edit { get; }
        DelegateCommand Reset { get; }
        #endregion

        public CollectionHotkeysViewModel(ICollectionsManager collectionsManager)
        {
            _collectionsManager = collectionsManager;
            Collections = _collectionsManager.Collections;
            Reset = new DelegateCommand(() => _selectedCollection.Hotkey = null, () => _selectedCollection != null);
            Edit = new DelegateCommand(() =>
            {
                throw new NotImplementedException();
            }, () => _selectedCollection != null);
        }
    }
}