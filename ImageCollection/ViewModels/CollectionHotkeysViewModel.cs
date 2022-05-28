using ImageCollection.Interfaces;

namespace ImageCollection.ViewModels
{
    internal class CollectionHotkeysViewModel : IWindowTitle
    {
        #region Fields
        private readonly ICollectionsManager _collectionsManager;
        #endregion

        public string Title { get => App.Name; }

        public CollectionHotkeysViewModel(ICollectionsManager collectionsManager)
        {
            _collectionsManager = collectionsManager;
        }
    }
}