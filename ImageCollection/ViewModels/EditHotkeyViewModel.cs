using Prism.Commands;
using System.Windows;

namespace ImageCollection.ViewModels
{
    internal class EditHotkeyViewModel
    {
        #region Property
        public string Title { get => App.Name; }
        public string NewName { get; set; }
        #endregion

        #region Commands
        public DelegateCommand<Window> CanselCommand { get; }
        public DelegateCommand<Window> OkCommand { get; }
        #endregion
    }
}