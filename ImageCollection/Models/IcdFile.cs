using ImageCollection.Interfaces;
using ImageCollection.Models.Structures;
using System;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace ImageCollection.Models
{
    internal class IcdFile
    {
        private const int CurrentFileVerson = 20220710;

        #region Property
        public int FileVersion { get; set; }
        public Guid Id { get; set; }
        public Hotkey Hotkey { get; set; }
        #endregion

        public static IcdFile Read(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            IcdFile icdFile = new IcdFile();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8))
                {
                    icdFile.FileVersion = br.ReadInt32();
                    icdFile.Id = new Guid(br.ReadBytes(16));
                    icdFile.Hotkey = new Hotkey((ModifierKeys)br.ReadInt32(), (Key)br.ReadInt32());
                }
            }

            return icdFile;
        }

        public static void WriteHotkey(string path, IImageCollection collection)
        {
            FileInfo icdInfo = new FileInfo(path);
            using (BinaryWriter bw = new BinaryWriter(icdInfo.OpenWrite(), Encoding.UTF8))
            {
                if (icdInfo.Exists)
                {
                    bw.BaseStream.Position = 16 + sizeof(int);
                }
                else
                {
                    bw.Write(CurrentFileVerson);
                    bw.Write(collection.Id.ToByteArray());
                }
                bw.Write((int)collection.Hotkey.Modifier);
                bw.Write((int)collection.Hotkey.Key);
            }
        }
    }
}