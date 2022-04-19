using ImageCollection.Interfaces;
using Prism.Commands;
using System.Text.RegularExpressions;
using System.Windows;
using SUID = Sergey.UI.Extension.Dialogs;

namespace ImageCollection.ViewModels
{
    internal class CollectionEditViewModel : IWindowTitle
    {
        #region Property
        public string Title { get => App.Name; }
        public string NewName { get; set; }
        #endregion

        #region Commands
        public DelegateCommand<Window> CanselCommand { get; }
        public DelegateCommand<Window> OkCommand { get; }
        #endregion

        public CollectionEditViewModel(ICollectionsManager collectionsManager, ICollection collection)
        {
            if (collection != null)
            {
                NewName = collection.Name;
            }
            CanselCommand = new DelegateCommand<Window>((w) => w.Close());
            OkCommand = new DelegateCommand<Window>((w) =>
            {
                if (string.IsNullOrWhiteSpace(NewName))
                {
                    SUID.MessageBox.Show("Название коллекции не может быть пустым!", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (new Regex("[<>:\"/|?*\\\\]").IsMatch(NewName))
                {
                    SUID.MessageBox.Show("В названии коллекции есть запрещенные символы (< > : \" / | ? * \\)!", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if ((collection != null && !collectionsManager.Rename(collection, NewName)) || (collection == null && collectionsManager.Create(NewName) == null))
                {
                    SUID.MessageBox.Show("Коллекция с таким названием уже существует!", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    w.Close();
                }
            });
        }

        public CollectionEditViewModel(ICollectionsManager collectionsManager) : this(collectionsManager, null) { }
    }
}