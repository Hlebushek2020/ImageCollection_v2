using ImageCollection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageCollection.Models
{
    internal class CollectionItem : ICollectionItem
    {
        public string Name => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public BitmapImage Preview => throw new NotImplementedException();

        public bool IsSelected => throw new NotImplementedException();
    }
}
