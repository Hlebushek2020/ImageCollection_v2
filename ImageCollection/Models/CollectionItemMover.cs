using ImageCollection.Interfaces;
using System.IO;

namespace ImageCollection.Models
{
    internal class CollectionItemMover : IImageCollectionItemMover
    {
        #region Fields
        private readonly IImageCollection _from;
        private readonly string _fromPath;
        private readonly IImageCollection _to;
        private readonly string _toPath;
        private readonly bool _isDelete;
        #endregion

        public CollectionItemMover(IImageCollection from, IImageCollection to, bool isDelete = false)
        {
            _from = from;
            _fromPath = _from.GetCollectionDirectory();
            _to = to;
            _toPath = _to.GetCollectionDirectory();
            _isDelete = isDelete;
        }

        public void Move(IImageCollectionItem collectionItem)
        {
            string fromPathFull = Path.Combine(_fromPath, collectionItem.Name);
            string toPathFull = Path.Combine(_toPath, collectionItem.Name);
            int counter = 0;
            string newName = null;
            while (File.Exists(toPathFull))
            {
                newName = $"{counter}-{collectionItem.Name}";
                toPathFull = Path.Combine(_toPath, newName);
            }
            File.Move(fromPathFull, toPathFull);
            if (!_isDelete)
            {
                _from.RemoveItem(collectionItem);
            }
            collectionItem.IsSelected = false;
            _to.AddItem(collectionItem);
            if (newName != null)
            {
                ((CollectionItem)collectionItem).Name = newName;
            }
        }
    }
}