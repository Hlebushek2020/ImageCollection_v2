using ImageCollection.Interfaces;
using ImageCollection.ViewModels;
using System.Windows;

namespace ImageCollection
{
    /// <summary>
    /// Interaction logic for CollectionEditWindow.xaml
    /// </summary>
    public partial class CollectionEditWindow : Window
    {
        public CollectionEditWindow(ICollectionsManager collectionsManager, ICollection collection)
        {
            InitializeComponent();
            DataContext = new CollectionEditViewModel(collectionsManager, collection);
        }

        public CollectionEditWindow(ICollectionsManager collectionsManager) : this(collectionsManager, null) { }

    }
}