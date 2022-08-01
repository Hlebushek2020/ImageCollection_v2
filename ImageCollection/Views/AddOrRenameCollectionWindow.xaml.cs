using ImageCollection.Interfaces;
using ImageCollection.ViewModels;
using System.Windows;

namespace ImageCollection
{
    /// <summary>
    /// Interaction logic for CollectionEditWindow.xaml
    /// </summary>
    public partial class AddOrRenameCollectionWindow : Window
    {
        public AddOrRenameCollectionWindow(IImageCollectionsManager collectionsManager, IImageCollection collection)
        {
            InitializeComponent();
            DataContext = new AddOrRenameCollectionViewModel(collectionsManager, collection);
        }

        public AddOrRenameCollectionWindow(IImageCollectionsManager collectionsManager) : this(collectionsManager, null) { }

    }
}