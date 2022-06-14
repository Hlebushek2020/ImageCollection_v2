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
        private readonly RenameFilesViewModel viewModel;

        public RenameFilesWindow(ICollectionItem collectionItem, ICollection collection)
        {
            InitializeComponent();
            viewModel = new RenameFilesViewModel(collectionItem, collection);
            DataContext = viewModel;
        }

        public RenameFilesWindow() : this(null, null) { }

        public string GetNewNameOrPattern() => viewModel.NewNameOrPattern;
    }
}
