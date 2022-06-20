using ImageCollection.Interfaces;
using ImageCollection.Models;
using ImageCollection.Models.Structures;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using SUID = Sergey.UI.Extension.Dialogs;

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
                // TODO: use KeyConverter
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

        public EditHotkeyViewModel(Hotkey hotkey, CollectionsHotkeyManager hotkeyManager)
        {
            Modifiers = new ObservableCollection<ModifierKeys> { ModifierKeys.None, ModifierKeys.Alt, ModifierKeys.Control, ModifierKeys.Shift };
            SelectedKey = hotkey.Key;
            SelectedModifier = hotkey.Modifier;
            CanselCommand = new DelegateCommand<Window>((w) =>
            {
                _selectedKey = Key.None;
                SelectedModifier = ModifierKeys.None;
                w.Close();
            });
            OkCommand = new DelegateCommand<Window>((w) =>
            {
                if (_selectedKey == Key.None)
                {
                    SUID.MessageBox.Show("Введите клавишу!", App.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (_selectedKey == hotkey.Key && SelectedModifier == hotkey.Modifier)
                {
                    SUID.MessageBox.Show("Текущая коллекция уже использует такую комбинацию клавиш!", App.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (hotkeyManager.Contains(SelectedModifier, _selectedKey))
                {
                    SUID.MessageBox.Show("Такая комбинация клавиш используется другой коллекцией!", App.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    w.Close();
                }
            });
        }
    }
}