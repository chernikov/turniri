using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace turniri.IntegrationTest.Tools
{
    public class Textarium : Filerarium
    {
        public static string SourceFolder = "D:\\test\\sandbox\\txt\\";

        private static Random rand = new Random((int)DateTime.Now.Ticks);

        public static string GetRandomText(int paragraph = 5)
        {
            var dir = new DirectoryInfo(SourceFolder);

            if (dir.Exists)
            {
                var files = dir.EnumerateFiles("*.txt").AsQueryable();

                var file = files.OrderBy(p => Guid.NewGuid()).FirstOrDefault();

                StringBuilder sb = new StringBuilder();
                string[] allLines = File.ReadAllLines(file.FullName);
                var randomLineNumber = rand.Next(0, allLines.Length - paragraph);

                for (int i = randomLineNumber; i <= randomLineNumber + paragraph; i++)
                {
                    sb.AppendLine(allLines[i]);
                };
                return sb.ToString();
            }
            return string.Empty;
        }


        public static string GetRandomHtmlText(int paragraph = 5)
        {
            var dir = new DirectoryInfo(SourceFolder);

            if (dir.Exists)
            {
                var files = dir.EnumerateFiles("*.txt").AsQueryable();

                var file = files.OrderBy(p => Guid.NewGuid()).FirstOrDefault();

                StringBuilder sb = new StringBuilder();
                string[] allLines = File.ReadAllLines(file.FullName);
                var randomLineNumber = rand.Next(0, allLines.Length - paragraph);

                for (int i = randomLineNumber; i <= randomLineNumber + paragraph; i++)
                {
                    var line = allLines[i];
                    sb.AppendLine("<p>" + allLines[i] + "</p>");
                };
                return sb.ToString();
            }
            return string.Empty;
        }
    }
}
