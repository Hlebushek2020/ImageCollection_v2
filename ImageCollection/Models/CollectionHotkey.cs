using ImageCollection.Interfaces;
using System;
using System.Windows.Input;

namespace ImageCollection.Models
{
    public class CollectionHotkey : IEquatable<CollectionHotkey>
    {
        public ModifierKeys Modifier { get; }
        public Key Key { get; }
        public ICollection Collection { get; }

        public CollectionHotkey(ICollection collection, ModifierKeys modifier, Key key)
        {
            Modifier = modifier;
            Key = key;
            Collection = collection;
        }

        public override bool Equals(object obj) => obj != null && Equals(obj as CollectionHotkey);

        public bool Equals(CollectionHotkey other) => other != null && Collection.Id.Equals(other.Collection.Id);

        public override int GetHashCode() => Collection.GetHashCode();
    }
}