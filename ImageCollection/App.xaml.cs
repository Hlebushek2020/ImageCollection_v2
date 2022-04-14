using ImageCollection.Models;
using Sergey.UI.Extension.Themes;
using System;
using System.Reflection;
using System.Windows;

namespace ImageCollection
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string Name { get; } = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SwitchTheme(null);
            MainWindow = new MainWindow();
            MainWindow.Show();
        }

        public static void SwitchTheme(Theme? theme)
        {
            Settings settings = Settings.Current;
            if (!theme.HasValue || settings.Theme != theme.Value)
            {
                Uri uri = ThemeUri.Get(settings.Theme);
                if (theme.HasValue)
                {
                    settings.Theme = theme.Value;
                    uri = ThemeUri.Get(theme.Value);
                }
                ResourceDictionary resource = (ResourceDictionary)LoadComponent(uri);
                Current.Resources.MergedDictionaries.Clear();
                Current.Resources.MergedDictionaries.Add(resource);
            }
        }
    }
}