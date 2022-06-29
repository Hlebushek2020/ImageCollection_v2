using ImageCollection.Interfaces;
using ImageCollection.ViewModels;
using System.Windows;

namespace ImageCollection
{
    /// <summary>
    /// Interaction logic for CollectionSelectionWindow.xaml
    /// </summary>
    public partial class CollectionSelectionWindow : Window
    {
        private readonly CollectionSelectionViewModel _viewModel;

        public CollectionSelectionWindow(IImageCollectionsManager collectionsManager, IImageCollection currentCollection)
        {
            InitializeComponent();
            _viewModel = new CollectionSelectionViewModel(collectionsManager, currentCollection);
            DataContext = _viewModel;
        }

        public IImageCollection GetSelectedCollection() => _viewModel.SelectedCollection;
    }
}