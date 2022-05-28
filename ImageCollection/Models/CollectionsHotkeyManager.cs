using ImageCollection.Interfaces;
using System.Collections.Generic;
using System.Windows.Input;

namespace ImageCollection.Models
{
    public class CollectionsHotkeyManager
    {
        private readonly Dictionary<ModifierKeys, Dictionary<Key, ICollection>> _hotkeysForCollection =
            new Dictionary<ModifierKeys, Dictionary<Key, ICollection>>();

        public void Register(ICollection collection, Hotkey hotkey)
        {
            if (_hotkeysForCollection.ContainsKey(hotkey.Modifier))
                _hotkeysForCollection[hotkey.Modifier].Add(hotkey.Key, collection);
            else
                _hotkeysForCollection.Add(hotkey.Modifier, new Dictionary<Key, ICollection> { { hotkey.Key, collection } });
        }

        public bool Contains(ModifierKeys modifier, Key key)
        {
            if (_hotkeysForCollection.ContainsKey(modifier))
                return _hotkeysForCollection[modifier].ContainsKey(key);
            return false;
        }

        public void Remove(Hotkey hotkey) => _hotkeysForCollection[hotkey.Modifier]?.Remove(hotkey.Key);

        public ICollection GetCollectionByHotkeys(ModifierKeys modifier, Key key) => _hotkeysForCollection[modifier]?[key];
    }
}