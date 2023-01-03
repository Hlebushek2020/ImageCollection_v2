using ImageCollection.Interfaces;
using ImageCollection.Models;
using Prism.Commands;
using Sergey.UI.Extension.Themes;
using System.Collections.Generic;
using System.Windows;

namespace ImageCollection.ViewModels
{
    internal class SettingsViewModel : IWindowTitle
    {
        #region Propirties
        public string Title => App.Name;
        public List<DisplayTheme> Themes => DisplayTheme.GetList();
        public DisplayTheme SelectedTheme { get; set; } = new DisplayTheme(Settings.Current.Theme);
        public bool MoveItemsFromRemoveCollection { get; set; } = Settings.Current.MoveItemsFromRemoveCollection;
        public bool DeleteCollectionFolder { get; set; } = Settings.Current.DeleteCollectionFolder;
        public bool DeleteCollectionFolderIfEmpty { get; set; } = Settings.Current.DeleteCollectionFolderIfEmpty;
        public string SearchCommand { get; set; } = Settings.Current.SearchCommand;
        #endregion

        #region Commands
        public DelegateCommand<Window> CanselCommand { get; }
        public DelegateCommand<Window> OkCommand { get; }
        #endregion

        public SettingsViewModel()
        {
            CanselCommand = new DelegateCommand<Window>((w) => w.Close());
            OkCommand = new DelegateCommand<Window>((w) =>
            {
                Settings.Current.MoveItemsFromRemoveCollection = MoveItemsFromRemoveCollection;
                Settings.Current.DeleteCollectionFolder = DeleteCollectionFolder;
                Settings.Current.DeleteCollectionFolderIfEmpty = DeleteCollectionFolderIfEmpty;
                Settings.Current.SearchCommand = SearchCommand;
                App.SwitchTheme(SelectedTheme.Value);
                Settings.Current.Save();
                w.Close();
            });
        }
    }
}