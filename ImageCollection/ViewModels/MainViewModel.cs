using ImageCollection.Interfaces;
using Prism.Mvvm;

namespace ImageCollection.ViewModels
{
    internal class MainViewModel : BindableBase, IWindowTitle
    {
        public string Title { get => App.Name; }
    }
}
