using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace GitHabInstaller.Services
{
    public class GitService
    {
        private readonly LoggerService _logger;

        public GitService(LoggerService logger)
        {
            _logger = logger;
        }

        public async Task<bool> CheckGitInstalled()
        {
            try
            {
                _logger.Log("Checking for Git installation...", LogLevel.Info);
                var result = await RunCommand("git", "--version");
                _logger.Log($"Found: {result}", LogLevel.Success);
                return true;
            }
            catch
            {
                _logger.Log("Git not found in PATH", LogLevel.Error);
                return false;
            }
        }

        public async Task CloneRepository(string repoUrl, string targetDir, string branch, string token, string sshKey)
        {
            _logger.Log("Starting repository clone...", LogLevel.Info);

            // Prepare URL with authentication
            string authenticatedUrl = PrepareUrl(repoUrl, token);

            // Prepare git clone command
            string branchArg = string.IsNullOrWhiteSpace(branch) ? "" : $"--branch {branch}";
            string depthArg = "--depth 1"; // Shallow clone for faster download
            string args = $"clone {depthArg} {branchArg} \"{authenticatedUrl}\" \"{targetDir}\"";

            if (!string.IsNullOrWhiteSpace(sshKey))
            {
                // Use SSH key
                _logger.Log("Using SSH authentication", LogLevel.Info);
                await RunCommandWithSSH("git", args, sshKey);
            }
            else
            {
                await RunCommand("git", args);
            }
        }

        private string PrepareUrl(string repoUrl, string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return repoUrl;

            // Convert HTTPS URL to include token
            if (repoUrl.StartsWith("https://"))
            {
                repoUrl = repoUrl.Replace("https://", $"https://x-access-token:{token}@");
                _logger.Log("Using GitHub token for authentication", LogLevel.Info);
            }

            return repoUrl;
        }

        private async Task RunCommandWithSSH(string fileName, string arguments, string sshKey)
        {
            if (!File.Exists(sshKey))
            {
                throw new FileNotFoundException($"SSH key not found: {sshKey}");
            }

            _logger.Log($"Using SSH key: {sshKey}", LogLevel.Info);
            
            // TODO: Implement SSH key handling
            await RunCommand(fileName, arguments);
        }

        private async Task<string> RunCommand(string fileName, string arguments)
        {
            return await RunProcessAsync(fileName, arguments);
        }

        private async Task<string> RunProcessAsync(string fileName, string arguments)
        {
            return await Task.Run(() =>
            {
                var psi = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi) ?? throw new InvalidOperationException($"Failed to start process: {fileName}");
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException($"Command failed with code {process.ExitCode}: {error}");
                }

                return output;
            });
        }
    }
}