using Newtonsoft.Json;
using Sergey.UI.Extension.Themes;
using System;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace ImageCollection.Models
{
    internal class Settings
    {
        #region Constants
        public const string IcdFileName = "data.icd";
        public const string DefaultCollectionName = "Root";
        public const string PreviewDirectoryName = "IC_PREVIEW";

        public static string ProgramResourceFolder { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SergeyGovorunov", "ImageCollection");
        public static BitmapImage DefaultPreview { get; } = new BitmapImage(new Uri("/Resources/defaultImage.png", UriKind.Relative));
        #endregion

        #region Instance
        private static Settings _current;

        public static Settings Current
        {
            get
            {
                if (_current == null)
                {
                    string settingsFile = Path.Combine(ProgramResourceFolder, "settings.json");
                    if (File.Exists(settingsFile))
                    {
                        _current = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsFile, Encoding.UTF8));
                    }
                    else
                    {
                        _current = new Settings();
                    }
                }
                return _current;
            }
        }
        #endregion

        #region Properties
        public Theme Theme { get; set; } = Theme.Light;
        public bool MoveItemsFromRemoveCollection { get; set; } = true;
        public bool DeleteCollectionFolder { get; set; } = true;
        public bool DeleteCollectionFolderIfEmpty { get; set; } = true;
        public string SearchCommand { get; set; } = string.Empty;
        #endregion

        public void Save()
        {
            Directory.CreateDirectory(ProgramResourceFolder);
            using (StreamWriter streamWriter = new StreamWriter(Path.Combine(ProgramResourceFolder, "settings.json"), false, Encoding.UTF8))
            {
                streamWriter.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
            }
        }
    }
}