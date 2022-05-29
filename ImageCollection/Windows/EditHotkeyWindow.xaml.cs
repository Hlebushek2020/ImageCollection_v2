using ImageCollection.ViewModels;
using System.Windows;

namespace ImageCollection
{
    /// <summary>
    /// Interaction logic for EditHotkeyWindow.xaml
    /// </summary>
    public partial class EditHotkeyWindow : Window
    {
        public EditHotkeyWindow()
        {
            InitializeComponent();
            DataContext = new EditHotkeyViewModel();
        }
    }
}
