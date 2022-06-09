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
        public string Title { get { return App.Name; } }
        public List<DisplayTheme> Themes { get; } = DisplayTheme.GetList();
        public DisplayTheme SelectedTheme { get; set; } = new DisplayTheme(Settings.Current.Theme);
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
                App.SwitchTheme(SelectedTheme.Value);
                Settings.Current.Save();
            });
        }
    }
}