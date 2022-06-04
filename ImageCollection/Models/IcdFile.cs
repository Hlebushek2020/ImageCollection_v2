using ImageCollection.Models.Structures;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace ImageCollection.Models
{
    internal class IcdFile
    {
        public Hotkey Hotkey { get; set; }

        private IcdFile() { }

        public static IcdFile Read(string path)
        {
            if (!File.Exists(path))
                return null;
            IcdFile icdFile = new IcdFile();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8))
                {
                    icdFile.Hotkey = new Hotkey((ModifierKeys)br.ReadInt32(), (Key)br.ReadInt32());
                }
            }
            return icdFile;
        }

        public static void WriteHotkey(string path, Hotkey hotkey)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (BinaryWriter bw = new BinaryWriter(fs, Encoding.UTF8))
                {
                    bw.Write((int)hotkey.Modifier);
                    bw.Write((int)hotkey.Key);
                }
            }
        }
    }
}