using System;
using System.IO;

namespace GitHabInstaller.Services
{
    public static class ShortcutService
    {
        public static void CreateDesktopShortcut(string targetFolder, string repoName)
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string shortcutPath = Path.Combine(desktop, $"{SanitizeName(repoName)}.url");

            if (File.Exists(shortcutPath))
            {
                return;
            }

            string fileContent =
                "[InternetShortcut]\r\n" +
                $"URL=file:///{targetFolder.Replace('\\', '/')}\r\n" +
                "IconFile=\r\n" +
                "IconIndex=0\r\n";

            File.WriteAllText(shortcutPath, fileContent);
        }

        private static string SanitizeName(string value)
        {
            foreach (char invalid in Path.GetInvalidFileNameChars())
            {
                value = value.Replace(invalid, '_');
            }

            return value.Trim();
        }
    }
}
