using System.IO.Compression;
using FastSlnPresentation.BLL.Models;

namespace FastSlnPresentation.BLL.Services.Static
{
    public static class ZipService
    {
        public static List<ContentFile> ExtractArchiveContents(Stream fileStream)
        {
            var contentFiles = new List<ContentFile>();

            using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Read))
            {
                foreach (var entry in archive.Entries)
                {
                    using (var stream = entry.Open())
                    using (var reader = new StreamReader(stream))
                    {
                        string content = reader.ReadToEnd();
                        contentFiles.Add(new ContentFile(entry.FullName, content));
                    }
                }
            }

            return contentFiles;
        }
    }
}
