using ImageCollection.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using SUID = Sergey.UI.Extension.Dialogs;

namespace ImageCollection
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        private readonly ProgressViewModel _viewModel;
        private bool _isCompleted = false;

        public ProgressWindow(ProgressViewModel progressViewModel)
        {
            InitializeComponent();
            _viewModel = progressViewModel;
            DataContext = _viewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    _viewModel.Run();
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        SUID.MessageBox.Show(ex.Message, App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
                Dispatcher.Invoke(() =>
                {
                    _isCompleted = true;
                    Close();
                });
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !_isCompleted;
        }
    }
}