using ImageCollection.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ImageCollection.ViewModels
{
    internal class EditHotkeyViewModel : BindableBase, IWindowTitle
    {
        #region Fields
        private Key _selectedKey;
        #endregion

        #region Property
        public string Title { get { return App.Name; } }
        public string DisplaySelectedKey
        {
            get
            {
                if (_selectedKey == Key.None)
                    return string.Empty;
                return _selectedKey.ToString();
            }
        }
        public Key SelectedKey
        {
            get { return _selectedKey; }
            set
            {
                _selectedKey = value;
                RaisePropertyChanged(nameof(DisplaySelectedKey));
            }
        }
        public ObservableCollection<ModifierKeys> Modifiers { get; set; }
        public ModifierKeys SelectedModifier { get; set; }
        #endregion

        #region Commands
        public DelegateCommand<Window> CanselCommand { get; }
        public DelegateCommand<Window> OkCommand { get; }
        #endregion

        public EditHotkeyViewModel()
        {
            Modifiers = new ObservableCollection<ModifierKeys> { ModifierKeys.None, ModifierKeys.Alt, ModifierKeys.Control, ModifierKeys.Shift };
            CanselCommand = new DelegateCommand<Window>((w) =>
            {
                _selectedKey = Key.None;
                SelectedModifier = ModifierKeys.None;
                w.Close();
            });
            OkCommand = new DelegateCommand<Window>((w) =>
            {
                throw new NotImplementedException();
            });
        }
    }
}