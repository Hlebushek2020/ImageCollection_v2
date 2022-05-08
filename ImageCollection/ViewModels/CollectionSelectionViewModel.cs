using ImageCollection.Interfaces;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows;

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

        public CollectionSelectionViewModel(ICollectionsManager collectionsManager)
        {
            Collections = collectionsManager.Collections;
            CanselCommand = new DelegateCommand<Window>((w) => w.Close());
            OkCommand = new DelegateCommand<Window>((w) =>
            {
                throw new NotImplementedException();
            });
        }
    }
}