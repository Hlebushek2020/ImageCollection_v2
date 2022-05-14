using MetadataExtractor.Formats.Bmp;
using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.Formats.Png;
using MetadataExtractor.Formats.WebP;
using MetadataExtractor.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace ImageCollection.Extensions
{
    internal static class Extensions
    {
        public static IEnumerable<FileInfo> WhereIsImage(this FileInfo[] fileInfos)
        {
            return fileInfos.Where(x => x.Extension.Equals(".bmp") || x.Extension.Equals(".jpg") || x.Extension.Equals(".jpeg") || x.Extension.Equals(".png"));
        }

        public static Size GetImageResolution(this FileInfo fileInfo)
        {
            Stream stream = fileInfo.OpenRead();
            FileType fileType = FileTypeDetector.DetectFileType(stream);
            IReadOnlyList<MetadataExtractor.Directory> collection;
            double height;
            double width;
            switch (fileType)
            {
                case FileType.Bmp:
                    collection = BmpMetadataReader.ReadMetadata(stream);
                    BmpHeaderDirectory bmpHeaderDirectory = collection.OfType<BmpHeaderDirectory>().FirstOrDefault();
                    height = Convert.ToDouble(bmpHeaderDirectory.GetDescription(BmpHeaderDirectory.TagImageHeight));
                    width = Convert.ToDouble(bmpHeaderDirectory.GetDescription(BmpHeaderDirectory.TagImageWidth));
                    return new Size(width, height);
                case FileType.Jpeg:
                    collection = JpegMetadataReader.ReadMetadata(stream);
                    JpegDirectory jpegDirectory = collection.OfType<JpegDirectory>().FirstOrDefault();
                    string heightMeta = jpegDirectory.GetDescription(JpegDirectory.TagImageHeight);
                    height = Convert.ToDouble(heightMeta.Substring(0, heightMeta.IndexOf(' ')));
                    string widthMeta = jpegDirectory.GetDescription(JpegDirectory.TagImageWidth);
                    width = Convert.ToDouble(widthMeta.Substring(0, widthMeta.IndexOf(' ')));
                    return new Size(width, height);
                case FileType.Png:
                    collection = PngMetadataReader.ReadMetadata(stream);
                    PngDirectory pngDirectory = collection.OfType<PngDirectory>().FirstOrDefault();
                    height = Convert.ToDouble(pngDirectory.GetDescription(PngDirectory.TagImageHeight));
                    width = Convert.ToDouble(pngDirectory.GetDescription(PngDirectory.TagImageWidth));
                    return new Size(width, height);
                case FileType.WebP:
                    collection = WebPMetadataReader.ReadMetadata(stream);
                    WebPDirectory webPDirectory = collection.OfType<WebPDirectory>().FirstOrDefault();
                    height = Convert.ToDouble(webPDirectory.GetDescription(WebPDirectory.TagImageHeight));
                    width = Convert.ToDouble(webPDirectory.GetDescription(WebPDirectory.TagImageWidth));
                    return new Size(width, height);
                default:
                    throw new Exception();
            }
        }
    }
}