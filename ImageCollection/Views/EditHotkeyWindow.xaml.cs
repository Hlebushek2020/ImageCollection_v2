using ImageCollection.Models;
using ImageCollection.Models.Structures;
using ImageCollection.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace ImageCollection
{
    /// <summary>
    /// Interaction logic for EditHotkeyWindow.xaml
    /// </summary>
    public partial class EditHotkeyWindow : Window
    {
        private readonly EditHotkeyViewModel _viewModel;

        public bool IsAvailableHotkey => _viewModel.SelectedKey != Key.None;

        public EditHotkeyWindow(Hotkey hotkey, CollectionsHotkeyManager hotkeyManager)
        {
            InitializeComponent();
            _viewModel = new EditHotkeyViewModel(hotkey, hotkeyManager);
            DataContext = _viewModel;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.Z) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                if (!(_viewModel.SelectedModifier == ModifierKeys.Control &&
                    (e.Key == Key.O || e.Key == Key.A || e.Key == Key.H)))
                {
                    _viewModel.SelectedKey = e.Key;
                }
            }
            e.Handled = true;
        }

        public Hotkey GetHotkey() => new Hotkey(_viewModel.SelectedModifier, _viewModel.SelectedKey);
    }
}