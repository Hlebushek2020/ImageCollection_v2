using ImageCollection.Interfaces;
using System.IO;

namespace ImageCollection.Models
{
    internal class CollectionItemMover : ICollectionItemMover
    {
        #region Fields
        private ICollection _from;
        private string _fromPath;
        private ICollection _to;
        private string _toPath;
        #endregion

        public CollectionItemMover(ICollection from, ICollection to)
        {
            _from = from;
            _fromPath = _from.GetCollectionDirectory();
            _to = to;
            _toPath = _to.GetCollectionDirectory();
        }

        public void Move(ICollectionItem collectionItem)
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
            _from.RemoveItem(collectionItem);
            _to.AddItem(collectionItem);
            if (newName != null)
            {
                ((CollectionItem)collectionItem).Name = newName;
            }
        }
    }
}