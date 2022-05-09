using ImageCollection.Interfaces;
using Prism.Commands;
using System.Collections.Generic;
using System.Windows;
using SUID = Sergey.UI.Extension.Dialogs;

namespace ImageCollection.ViewModels
{
    internal class CollectionSelectionViewModel : IWindowTitle
    {
        #region Property
        public string Title { get => App.Name; }
        public IReadOnlyCollection<ICollection> Collections { get; }
        public ICollection SelectedCollection { get; set; }
        #endregion

        #region Commands
        public DelegateCommand<Window> CanselCommand { get; }
        public DelegateCommand<Window> OkCommand { get; }
        #endregion

        public CollectionSelectionViewModel(ICollectionsManager collectionsManager, ICollection currentCollection)
        {
            Collections = collectionsManager.Collections;
            CanselCommand = new DelegateCommand<Window>((w) => w.Close());
            OkCommand = new DelegateCommand<Window>((w) =>
            {
                if (SelectedCollection == null)
                {
                    SUID.MessageBox.Show("Выберите коллекцию!", App.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (SelectedCollection.Equals(currentCollection))
                {
                    SUID.MessageBox.Show("Невозможно добавть элементы в ту же коллекцию, выберите другую коллекцию!", App.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    w.DialogResult = true;
                    w.Close();
                }
            });
        }
    }
}