using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageCollection.Utils
{
    internal static class IcUtils
    {
        public static void CreateThumbnail(string fileName, Size resolution, string thumbnailFileName)
        {
            byte[] originalBuffer = File.ReadAllBytes(fileName);
            int previewWidth = (int)(resolution.Width / resolution.Height * 94.0);
            BitmapImage convert = new BitmapImage();
            convert.BeginInit();
            convert.StreamSource = new MemoryStream(originalBuffer);
            convert.DecodePixelHeight = 94;
            convert.DecodePixelWidth = previewWidth;
            convert.EndInit();
            JpegBitmapEncoder previewEncoder = new JpegBitmapEncoder();
            previewEncoder.Frames.Add(BitmapFrame.Create(convert));
            using (FileStream previewStream = new FileStream(thumbnailFileName, FileMode.Create, FileAccess.Write))
            {
                previewEncoder.Save(previewStream);
            }
        }

        public static BitmapImage GetThumbnail(string fileName)
        {
            BitmapImage preview = new BitmapImage();
            preview.BeginInit();
            preview.StreamSource = new MemoryStream(File.ReadAllBytes(fileName));
            preview.EndInit();
            return preview;
        }
    }
}