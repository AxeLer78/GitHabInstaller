using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace GitHabInstaller.Services
{
    public class InstallerService
    {
        private readonly LoggerService _logger;

        public InstallerService(LoggerService logger)
        {
            _logger = logger;
        }

        public async Task InstallDependencies(string projectFolder, ProjectType projectType)
        {
            _logger.Log($"Installing dependencies for {projectType} project...", LogLevel.Info);

            switch (projectType)
            {
                case ProjectType.Node:
                    await InstallNodeDependencies(projectFolder);
                    break;
                case ProjectType.Python:
                    await InstallPythonDependencies(projectFolder);
                    break;
                case ProjectType.DotNet:
                    await InstallDotNetDependencies(projectFolder);
                    break;
                case ProjectType.PHP:
                    await InstallPHPDependencies(projectFolder);
                    break;
                case ProjectType.Java:
                    await InstallJavaDependencies(projectFolder);
                    break;
                case ProjectType.Go:
                    await InstallGoDependencies(projectFolder);
                    break;
                case ProjectType.Rust:
                    await InstallRustDependencies(projectFolder);
                    break;
                case ProjectType.Ruby:
                    await InstallRubyDependencies(projectFolder);
                    break;
                default:
                    _logger.Log("No automatic installation for this project type", LogLevel.Warning);
                    break;
            }
        }

        private async Task InstallNodeDependencies(string projectFolder)
        {
            _logger.Log("Running: npm install", LogLevel.Info);
            await RunProcessInFolder("npm", "install", projectFolder);
        }

        private async Task InstallPythonDependencies(string projectFolder)
        {
            if (File.Exists(Path.Combine(projectFolder, "requirements.txt")))
            {
                _logger.Log("Running: pip install -r requirements.txt", LogLevel.Info);
                await RunProcessInFolder("python", "-m pip install -r requirements.txt", projectFolder);
            }
            else if (File.Exists(Path.Combine(projectFolder, "pyproject.toml")))
            {
                _logger.Log("Running: pip install -e .", LogLevel.Info);
                await RunProcessInFolder("python", "-m pip install -e .", projectFolder);
            }
        }

        private async Task InstallDotNetDependencies(string projectFolder)
        {
            _logger.Log("Running: dotnet restore", LogLevel.Info);
            await RunProcessInFolder("dotnet", "restore", projectFolder);
        }

        private async Task InstallPHPDependencies(string projectFolder)
        {
            _logger.Log("Running: composer install", LogLevel.Info);
            await RunProcessInFolder("composer", "install", projectFolder);
        }

        private async Task InstallJavaDependencies(string projectFolder)
        {
            if (File.Exists(Path.Combine(projectFolder, "pom.xml")))
            {
                _logger.Log("Running: mvn clean install", LogLevel.Info);
                await RunProcessInFolder("mvn", "clean install", projectFolder);
            }
            else if (File.Exists(Path.Combine(projectFolder, "build.gradle")))
            {
                _logger.Log("Running: gradle build", LogLevel.Info);
                await RunProcessInFolder("gradle", "build", projectFolder);
            }
        }

        private async Task InstallGoDependencies(string projectFolder)
        {
            _logger.Log("Running: go mod download", LogLevel.Info);
            await RunProcessInFolder("go", "mod download", projectFolder);
        }

        private async Task InstallRustDependencies(string projectFolder)
        {
            _logger.Log("Running: cargo build", LogLevel.Info);
            await RunProcessInFolder("cargo", "build", projectFolder);
        }

        private async Task InstallRubyDependencies(string projectFolder)
        {
            _logger.Log("Running: bundle install", LogLevel.Info);
            await RunProcessInFolder("bundle", "install", projectFolder);
        }

        private async Task RunProcessInFolder(string fileName, string arguments, string workingDirectory)
        {
            await Task.Run(() =>
            {
                var psi = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi) ?? throw new InvalidOperationException($"Failed to start: {fileName}");

                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                        _logger.Log(line, LogLevel.Info);
                }

                while (!process.StandardError.EndOfStream)
                {
                    string line = process.StandardError.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                        _logger.Log(line, LogLevel.Warning);
                }

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException($"Process failed with exit code {process.ExitCode}: {fileName} {arguments}");
                }
            });
        }
    }
}