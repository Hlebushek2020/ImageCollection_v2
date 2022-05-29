using ImageCollection.Interfaces;
using System.Collections.Generic;
using System.Windows.Input;

namespace ImageCollection.Models
{
    public class CollectionsHotkeyManager
    {
        private readonly Dictionary<ModifierKeys, Dictionary<Key, ICollection>> _hotkeysForCollection =
            new Dictionary<ModifierKeys, Dictionary<Key, ICollection>>();

        public void Register(Hotkey? oldHotkey, Hotkey? newHotkey, ICollection collection)
        {
            Remove(oldHotkey);
            if (newHotkey != null)
            {
                if (_hotkeysForCollection.ContainsKey(newHotkey.Value.Modifier))
                    _hotkeysForCollection[newHotkey.Value.Modifier].Add(newHotkey.Value.Key, collection);
                else
                {
                    _hotkeysForCollection.Add(newHotkey.Value.Modifier, new Dictionary<Key, ICollection> {
                        { newHotkey.Value.Key, collection }
                    });
                }
            }
        }

        public bool Contains(ModifierKeys modifier, Key key)
        {
            if (_hotkeysForCollection.ContainsKey(modifier))
                return _hotkeysForCollection[modifier].ContainsKey(key);
            return false;
        }

        public void Remove(Hotkey? hotkey)
        {
            if (hotkey != null)
                _hotkeysForCollection[hotkey.Value.Modifier]?.Remove(hotkey.Value.Key);
        }

        public ICollection GetCollectionByHotkeys(ModifierKeys modifier, Key key) => _hotkeysForCollection[modifier]?[key];
    }
}