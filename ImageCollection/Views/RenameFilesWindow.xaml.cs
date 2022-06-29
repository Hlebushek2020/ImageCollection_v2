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
        private readonly RenameFilesViewModel _viewModel;

        public RenameFilesWindow(IImageCollectionItem collectionItem, IImageCollection collection)
        {
            InitializeComponent();
            _viewModel = new RenameFilesViewModel(collectionItem, collection);
            DataContext = _viewModel;
        }

        public RenameFilesWindow() : this(null, null) { }

        public string GetNewNameOrPattern() => _viewModel.NewNameOrPattern;
    }
}
