using System.Windows.Input;

namespace ImageCollection.Models.Structures
{
    public struct Hotkey
    {
        #region Properties
        public ModifierKeys Modifier { get; }
        public Key Key { get; }

        public string DisplayModifier
        {
            get
            {
                ModifierKeysConverter modifierKeysConverter = new ModifierKeysConverter();
                return modifierKeysConverter.ConvertToString(Modifier);
            }
        }

        public string DisplayKey
        {
            get
            {
                KeyConverter keyConverter = new KeyConverter();
                return keyConverter.ConvertToString(Key);
            }
        }
        #endregion

        public Hotkey(ModifierKeys modifier, Key key)
        {
            Modifier = modifier;
            Key = key;
        }

        public Hotkey(Key key) : this(ModifierKeys.None, key) { }
    }
}