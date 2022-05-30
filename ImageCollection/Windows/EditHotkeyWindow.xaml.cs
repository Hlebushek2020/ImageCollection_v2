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

        public EditHotkeyWindow()
        {
            InitializeComponent();
            _viewModel = new EditHotkeyViewModel();
            DataContext = _viewModel;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            _viewModel.SelectedKey = e.Key;
            e.Handled = true;
        }
    }
}