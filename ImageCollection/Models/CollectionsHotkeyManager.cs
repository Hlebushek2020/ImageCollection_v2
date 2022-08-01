using ImageCollection.Interfaces;
using ImageCollection.Models.Structures;
using System.Collections.Generic;
using System.Windows.Input;

namespace ImageCollection.Models
{
    public class CollectionsHotkeyManager
    {
        private readonly Dictionary<ModifierKeys, Dictionary<Key, IImageCollection>> _hotkeysForCollection =
            new Dictionary<ModifierKeys, Dictionary<Key, IImageCollection>>();

        public void Register(Hotkey oldHotkey, Hotkey newHotkey, IImageCollection collection)
        {
            Remove(oldHotkey);
            if (newHotkey.Key != Key.None && newHotkey.Modifier != ModifierKeys.None)
            {
                if (_hotkeysForCollection.ContainsKey(newHotkey.Modifier))
                    _hotkeysForCollection[newHotkey.Modifier].Add(newHotkey.Key, collection);
                else
                {
                    _hotkeysForCollection.Add(newHotkey.Modifier, new Dictionary<Key, IImageCollection> {
                        { newHotkey.Key, collection }
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

        public void Remove(Hotkey hotkey)
        {
            if (hotkey.Key != Key.None && hotkey.Modifier != ModifierKeys.None)
                _hotkeysForCollection[hotkey.Modifier]?.Remove(hotkey.Key);
        }

        public IImageCollection GetCollectionByHotkeys(ModifierKeys modifier, Key key)
        {
            if (_hotkeysForCollection.ContainsKey(modifier))
            {
                Dictionary<Key, IImageCollection> keysForModifier = _hotkeysForCollection[modifier];
                if (keysForModifier.ContainsKey(key))
                {
                    return keysForModifier[key];
                }
            }
            return null;
        }
    }
}