using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageCollection.Extensions
{
    internal static class Extensions
    {
        public static IEnumerable<FileInfo> WhereIsImage(this FileInfo[] fileInfos)
        {
            return fileInfos.Where(x => x.Extension.Equals(".bmp") || x.Extension.Equals(".jpg") || x.Extension.Equals(".jpeg") || x.Extension.Equals(".png"));
        }
    }
}