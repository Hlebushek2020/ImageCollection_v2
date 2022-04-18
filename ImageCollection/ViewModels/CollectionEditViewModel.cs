using ImageCollection.Interfaces;
using Prism.Commands;
using System.Text.RegularExpressions;
using System.Windows;

namespace ImageCollection.ViewModels
{
    internal class CollectionEditViewModel
    {
        #region Property
        public string NewName { get; set; }
        #endregion

        #region Commands
        public DelegateCommand<Window> CanselCommand { get; } = new DelegateCommand<Window>((w) => w.Close());
        public DelegateCommand<Window> OkCommand { get; }
        #endregion

        public CollectionEditViewModel(ICollectionsManager collectionsManager, ICollection collection)
        {
            NewName = collection.Name;
            OkCommand = new DelegateCommand<Window>((w) =>
            {
                if (string.IsNullOrWhiteSpace(NewName))
                {
                    return;
                }
                if (new Regex("[<>:\"/|?*\\\\]").IsMatch(NewName))
                {
                    return;
                }
                if (!collectionsManager.Rename(collection, NewName))
                {
                    return;
                }
                w.Close();
            });
        }

        public CollectionEditViewModel(ICollectionsManager collectionsManager)
        {
            OkCommand = new DelegateCommand<Window>((w) =>
            {
                if (string.IsNullOrWhiteSpace(NewName))
                {
                    return;
                }
                if (new Regex("[<>:\"/|?*\\\\]").IsMatch(NewName))
                {
                    return;
                }
                if (collectionsManager.Create(NewName) == null)
                {
                    return;
                }
                w.Close();
            });
        }
    }
}