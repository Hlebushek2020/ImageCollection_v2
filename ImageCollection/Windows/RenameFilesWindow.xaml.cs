using ImageCollection.Interfaces;
using ImageCollection.ViewModels;
using System.Windows;

namespace ImageCollection
{
    /// <summary>
    /// Interaction logic for RenameWindow.xaml
    /// </summary>
    public partial class RenameFilesWindow : Window
    {
        public RenameFilesWindow(ICollectionItem collectionItem, ICollection collection)
        {
            InitializeComponent();
            DataContext = new RenameFilesViewModel(collectionItem, collection);
        }

        public RenameFilesWindow() : this(null, null) { }
    }
}
