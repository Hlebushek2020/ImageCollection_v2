using ImageCollection.Interfaces;
using Prism.Commands;
using System.IO;
using System.Windows;
using SUID = Sergey.UI.Extension.Dialogs;

namespace ImageCollection.ViewModels
{
    internal class RenameFilesViewModel : IWindowTitle
    {
        #region Property
        public string Title => App.Name;
        public string NewNameOrPattern { get; set; }
        #endregion

        #region Commands
        public DelegateCommand<Window> CanselCommand { get; }
        public DelegateCommand<Window> OkCommand { get; }
        #endregion

        public RenameFilesViewModel(IImageCollectionItem collectionItem, IImageCollection collection)
        {
            if (collectionItem != null)
            {
                NewNameOrPattern = Path.GetFileNameWithoutExtension(collectionItem.Name);
            }
            CanselCommand = new DelegateCommand<Window>((w) => w.Close());
            OkCommand = new DelegateCommand<Window>((w) =>
            {
                if (string.IsNullOrWhiteSpace(NewNameOrPattern))
                {
                    SUID.MessageBox.Show(collectionItem != null ? "Имя файла не может быть пустым!" : "Паттерн имени файла не может быть пустым!", App.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (collectionItem != null && !collection.CheckingNewFileName(collectionItem, NewNameOrPattern))
                {
                    SUID.MessageBox.Show("Файл с таким именем уже существует!", App.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (collectionItem != null && NewNameOrPattern.Equals(Path.GetFileNameWithoutExtension(collectionItem.Name)))
                {
                    SUID.MessageBox.Show("Новое имя файла совпадает с текущим, введите другое имя!", App.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (collectionItem == null && !NewNameOrPattern.Contains("{0}"))
                {
                    SUID.MessageBox.Show("Паттерн не содержит конструкции ({0}) для подстановки номера!", App.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
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