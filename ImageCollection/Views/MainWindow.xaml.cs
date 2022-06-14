using ImageCollection.Interfaces;
using ImageCollection.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ImageCollection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_viewModel.CollectionsManager != null)
            {
                ICollection toCollection = _viewModel.CollectionsManager.HotkeyManager.GetCollectionByHotkeys(e.KeyboardDevice.Modifiers, e.Key);
                if (toCollection != null && _viewModel.SelectedCollection != null)
                {
                    _viewModel.CollectionsManager.ToCollection(_viewModel.SelectedCollection, toCollection);
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _viewModel.SelectedCollection?.StopInitPreviewImages(true);
        }
    }
}