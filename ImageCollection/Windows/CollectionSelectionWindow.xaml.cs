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
        public CollectionSelectionWindow(ICollectionsManager collectionsManager)
        {
            InitializeComponent();
            DataContext = new CollectionSelectionViewModel(collectionsManager);
        }
    }
}
