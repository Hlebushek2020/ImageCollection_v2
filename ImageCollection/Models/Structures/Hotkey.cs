using System.Windows.Input;

namespace ImageCollection.Models.Structures
{
    public struct Hotkey
    {
        public ModifierKeys Modifier { get; }
        public Key Key { get; }

        public Hotkey(ModifierKeys modifier, Key key)
        {
            Modifier = modifier;
            Key = key;
        }

        public Hotkey(Key key) : this(ModifierKeys.None, key) { }
    }
}