using System.Windows.Input;

namespace ImageCollection.Models.Structures
{
    public struct Hotkey
    {
        // TODO: add display property
        public ModifierKeys Modifier { get; }
        // TODO: add display property
        public Key Key { get; }

        public Hotkey(ModifierKeys modifier, Key key)
        {
            Modifier = modifier;
            Key = key;
        }

        public Hotkey(Key key) : this(ModifierKeys.None, key) { }
    }
}