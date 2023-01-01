using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace ImageCollection.Models
{
    public class Session
    {
        private const string FileName = "session.json";

        public string BaseDirectory { get; set; }
        public string Collection { get; set; }
        public string Item { get; set; }

        public void Save()
        {
            Directory.CreateDirectory(Settings.ProgramResourceFolder);
            string pathSessionFile = Path.Combine(Settings.ProgramResourceFolder, FileName);
            using (StreamWriter streamWriter = new StreamWriter(pathSessionFile, false, Encoding.UTF8))
            {
                streamWriter.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
            }
        }

        #region Static
        public static Session Load()
        {
            string pathSessionFile = Path.Combine(Settings.ProgramResourceFolder, FileName);
            return JsonConvert.DeserializeObject<Session>(File.ReadAllText(pathSessionFile, Encoding.UTF8));
        }

        public static bool AvailabilityOfSaving() =>
            File.Exists(Path.Combine(Settings.ProgramResourceFolder, FileName));
        #endregion
    }
}