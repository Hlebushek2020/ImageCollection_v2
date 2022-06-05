using ImageCollection.Extensions;
using ImageCollection.Interfaces;
using Prism.Mvvm;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageCollection.Models
{
    internal class CollectionItem : BindableBase, ICollectionItem
    {
        #region Fields
        private string _name;
        private BitmapImage _preview;
        #endregion

        public Guid Id { get; }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
        public string Description { get; }
        public long Size { get; set; }
        public Size Resolution { get; set; }
        public bool IsPreview { get { return _preview != null; } }
        public BitmapImage Preview
        {
            get
            {
                if (_preview != null)
                    return _preview;
                return Settings.DefaultPreview;
            }
            set
            {
                _preview = value;
                RaisePropertyChanged();
            }
        }
        public bool IsSelected { get; set; }

        public CollectionItem(FileInfo fileInfo)
        {
            Id = Guid.NewGuid();
            _name = fileInfo.Name;
            Size = fileInfo.Length;
            Resolution = fileInfo.GetImageResolution();
            Description = $"{Resolution.Width}x{Resolution.Height}; {Math.Round(Size / 1024.0 / 1024.0, 2)} Мб";
        }
    }
}