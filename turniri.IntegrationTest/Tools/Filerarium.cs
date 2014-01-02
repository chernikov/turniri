using turniri.Tools;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net;

namespace turniri.IntegrationTest.Tools
{
    public class Filerarium
    {
        public static string FilesFolder = "d:\\Projects\\RepositoryMercurial\\turniri\\source\\turniri\\turniri";

        public const string SourceFolder = "D:\\test\\sandbox\\files\\";

        public static string GetRandomSourceFile(string sourceFolder, string extension = "*.*")
        {
            var dir = new DirectoryInfo(sourceFolder);

            if (dir.Exists)
            {
                var files = dir.EnumerateFiles(extension).AsQueryable();
                var file = files.OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                if (file != null)
                {
                    return file.FullName;
                }

            }
            return string.Empty;
        }

        public static string MakeAbsFolder(string folder)
        {
            var file = Path.Combine(FilesFolder, folder.Replace("/", "\\").Substring(1));
            return file;
        }

        public static string CopyRandomFile(string folder, out string name, string sourceFolder = SourceFolder)
        {
            var source = GetRandomSourceFile(sourceFolder);
            var extension = Path.GetExtension(source);

            var url = string.Format("{0}{1}{2}", folder, StringExtension.GenerateNewFile(), extension);
            var absFile = MakeAbsFolder(url);
            using (var fileSource = new FileStream(source, FileMode.Open))
            {
                using (var fs = new FileStream(absFile, FileMode.CreateNew))
                {
                    fileSource.CopyTo(fs);
                    fs.Flush();
                }
            }
            name = Path.GetFileName(source);
            return url;
        }

        public static string SaveImageFromUrl(string folder, string url)
        {
            var webClient = new WebClient();

            var bytes = webClient.DownloadData(url);
            var ms = new MemoryStream(bytes);
            var filePlace = string.Format("{0}{1}{2}", folder, StringExtension.GenerateNewFile(), ".jpg");
            var absFile = MakeAbsFolder(filePlace);
            using (var fs = new FileStream(absFile, FileMode.CreateNew))
            {
                ms.CopyTo(fs);
                fs.Flush();
            }
            return filePlace;
        }

        public static string CopyRandomFile(string folder)
        {
            string forNull = string.Empty;
            return CopyRandomFile(folder, out forNull);
        }
    }
}
