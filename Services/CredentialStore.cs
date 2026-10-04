using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GitHabInstaller.Services
{
    public static class CredentialStore
    {
        public static string StorePath(string key)
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GitHabInstaller");
            Directory.CreateDirectory(folder);
            return Path.Combine(folder, $"{key}.bin");
        }

        public static void Save(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            byte[] protectedBytes = ProtectedData.Protect(
                Encoding.UTF8.GetBytes(value),
                null,
                DataProtectionScope.CurrentUser);

            File.WriteAllBytes(StorePath(key), protectedBytes);
        }

        public static string? Load(string key)
        {
            string path = StorePath(key);
            if (!File.Exists(path))
            {
                return null;
            }

            try
            {
                byte[] protectedBytes = File.ReadAllBytes(path);
                byte[] bytes = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return null;
            }
        }

        public static void Delete(string key)
        {
            string path = StorePath(key);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
