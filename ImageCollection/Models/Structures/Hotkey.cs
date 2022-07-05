using System.Windows.Input;

namespace ImageCollection.Models.Structures
{
    public struct Hotkey
    {
        private static readonly ModifierKeysConverter _modifierKeysConverter = new ModifierKeysConverter();
        private static readonly KeyConverter _keyConverter = new KeyConverter();

        #region Properties
        public ModifierKeys Modifier { get; }
        public Key Key { get; }
        public string DisplayModifier => _modifierKeysConverter.ConvertToString(Modifier);
        public string DisplayKey => _keyConverter.ConvertToString(Key);
        #endregion

        public Hotkey(ModifierKeys modifier, Key key)
        {
            Modifier = modifier;
            Key = key;
        }

        public Hotkey(Key key) : this(ModifierKeys.None, key) { }
    }
}