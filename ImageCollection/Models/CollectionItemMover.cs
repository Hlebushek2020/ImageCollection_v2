using ImageCollection.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CollectionItemMover(CollectionsManager collectionsManager, ICollection from, ICollection to)
        {
            _from = from;
            _fromPath = collectionsManager.RootDirectory;
            if (from.Equals(collectionsManager.RootDirectory))
            {
                _fromPath = Path.Combine(collectionsManager.RootDirectory, from.Name);
            }
            _to = to;
            _toPath = collectionsManager.RootDirectory;
            if (to.Equals(collectionsManager.DefaultCollection))
            {
                _toPath = Path.Combine(collectionsManager.RootDirectory, to.Name);
            }
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