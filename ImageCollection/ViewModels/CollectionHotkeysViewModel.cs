using ImageCollection.Interfaces;
using ImageCollection.Models.Structures;
using Prism.Commands;
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
        public DelegateCommand Edit { get; }
        public DelegateCommand Reset { get; }
        #endregion

        public CollectionHotkeysViewModel(ICollectionsManager collectionsManager)
        {
            _collectionsManager = collectionsManager;
            Collections = _collectionsManager.Collections;
            Reset = new DelegateCommand(() => _selectedCollection.Hotkey = new Hotkey(), () => _selectedCollection != null);
            Edit = new DelegateCommand(() =>
            {
                EditHotkeyWindow editHotkey = new EditHotkeyWindow(_selectedCollection.Hotkey, _collectionsManager.HotkeyManager);
                editHotkey.ShowDialog();
                if (editHotkey.IsAvailableHotkey)
                {
                    _selectedCollection.Hotkey = editHotkey.GetHotkey();
                }
            }, () => _selectedCollection != null);
        }
    }
}