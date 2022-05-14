using ImageCollection.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ImageCollection.Models
{
    public class CollectionsHotkeyManager
    {
        public ObservableCollection<CollectionHotkey> CollectionsHotkey { get; } = new ObservableCollection<CollectionHotkey>();

        public CollectionHotkey Add(ICollection collection, ModifierKeys modifier, Key key)
        {
            CollectionHotkey collectionHotkey = new CollectionHotkey(collection, modifier, key);
            CollectionsHotkey.Add(collectionHotkey);
            return collectionHotkey;
        }

        public void Remove(CollectionHotkey collectionHotkey) => CollectionsHotkey.Remove(collectionHotkey);

        public void Remove(ICollection collection) => Remove(CollectionsHotkey.First(x => x.Collection.Equals(collection)));

        public ICollection GetCollectionByHotkeys(ModifierKeys modifier, Key key) => CollectionsHotkey.First(x => x.Key.Equals(key) && x.Modifier.Equals(modifier))?.Collection;
    }
}