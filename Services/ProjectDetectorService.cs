using System;
using System.IO;
using System.Linq;

namespace GitHabInstaller.Services
{
    public enum ProjectType
    {
        Unknown,
        Node,
        Python,
        DotNet,
        PHP,
        Java,
        Go,
        Rust,
        Ruby
    }

    public class ProjectDetectorService
    {
        public string FindProjectFolder(string root)
        {
            if (string.IsNullOrWhiteSpace(root) || !Directory.Exists(root))
                return root;

            // Check immediate subdirectories
            var directories = Directory.GetDirectories(root);
            foreach (var dir in directories)
            {
                if (IsProjectRoot(dir))
                    return dir;
            }

            // If no subdirectory is a project, return root
            return root;
        }

        private bool IsProjectRoot(string path)
        {
            try
            {
                return File.Exists(Path.Combine(path, "package.json")) ||
                       File.Exists(Path.Combine(path, "requirements.txt")) ||
                       File.Exists(Path.Combine(path, "pyproject.toml")) ||
                       File.Exists(Path.Combine(path, "composer.json")) ||
                       File.Exists(Path.Combine(path, "pom.xml")) ||
                       File.Exists(Path.Combine(path, "build.gradle")) ||
                       File.Exists(Path.Combine(path, "Cargo.toml")) ||
                       File.Exists(Path.Combine(path, "Gemfile")) ||
                       Directory.GetFiles(path, "*.csproj").Length > 0 ||
                       Directory.GetFiles(path, "*.sln").Length > 0 ||
                       Directory.GetFiles(path, "go.mod").Length > 0;
            }
            catch
            {
                return false;
            }
        }

        public ProjectType DetectProjectType(string projectFolder)
        {
            if (!Directory.Exists(projectFolder))
                return ProjectType.Unknown;

            try
            {
                if (File.Exists(Path.Combine(projectFolder, "package.json")))
                    return ProjectType.Node;

                if (File.Exists(Path.Combine(projectFolder, "requirements.txt")) ||
                    File.Exists(Path.Combine(projectFolder, "pyproject.toml")))
                    return ProjectType.Python;

                if (Directory.GetFiles(projectFolder, "*.csproj").Length > 0 ||
                    Directory.GetFiles(projectFolder, "*.sln").Length > 0)
                    return ProjectType.DotNet;

                if (File.Exists(Path.Combine(projectFolder, "composer.json")))
                    return ProjectType.PHP;

                if (File.Exists(Path.Combine(projectFolder, "pom.xml")) ||
                    File.Exists(Path.Combine(projectFolder, "build.gradle")))
                    return ProjectType.Java;

                if (File.Exists(Path.Combine(projectFolder, "go.mod")))
                    return ProjectType.Go;

                if (File.Exists(Path.Combine(projectFolder, "Cargo.toml")))
                    return ProjectType.Rust;

                if (File.Exists(Path.Combine(projectFolder, "Gemfile")))
                    return ProjectType.Ruby;

                return ProjectType.Unknown;
            }
            catch
            {
                return ProjectType.Unknown;
            }
        }
    }
}