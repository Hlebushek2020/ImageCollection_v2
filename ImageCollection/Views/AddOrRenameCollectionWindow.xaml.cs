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
        public AddOrRenameCollectionWindow(ICollectionsManager collectionsManager, ICollection collection)
        {
            InitializeComponent();
            DataContext = new AddOrRenameCollectionViewModel(collectionsManager, collection);
        }

        public AddOrRenameCollectionWindow(ICollectionsManager collectionsManager) : this(collectionsManager, null) { }

    }
}