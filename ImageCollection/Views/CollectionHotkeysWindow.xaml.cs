using ImageCollection.Interfaces;
using ImageCollection.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace ImageCollection
{
    /// <summary>
    /// Interaction logic for CollectionHotkeysWindow.xaml
    /// </summary>
    public partial class CollectionHotkeysWindow : Window
    {
        private readonly CollectionHotkeysViewModel _viewModel;

        public CollectionHotkeysWindow(ICollectionsManager collectionsManager)
        {
            InitializeComponent();
            _viewModel = new CollectionHotkeysViewModel(collectionsManager);
            DataContext = _viewModel;
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) => _viewModel.Edit.Execute();
    }
}