using ImageCollection.Interfaces;
using ImageCollection.ViewModels;
using System.Windows;

namespace ImageCollection
{
    /// <summary>
    /// Interaction logic for CollectionHotkeysWindow.xaml
    /// </summary>
    public partial class CollectionHotkeysWindow : Window
    {
        public CollectionHotkeysWindow(ICollectionsManager collectionsManager)
        {
            InitializeComponent();
            DataContext = new CollectionHotkeysViewModel(collectionsManager);
        }
    }
}
