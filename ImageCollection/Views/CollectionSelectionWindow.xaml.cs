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
        private readonly CollectionSelectionViewModel viewModel;

        public CollectionSelectionWindow(ICollectionsManager collectionsManager, ICollection currentCollection)
        {
            InitializeComponent();
            viewModel = new CollectionSelectionViewModel(collectionsManager, currentCollection);
            DataContext = viewModel;
        }

        public ICollection GetSelectedCollection() => viewModel.SelectedCollection;
    }
}