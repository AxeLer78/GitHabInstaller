using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using GitHabInstaller.Services;

namespace GitHabInstaller
{
    public partial class MainWindow : Window
    {
        private readonly LoggerService _logger;
        private readonly GitService _gitService;
        private readonly ProjectDetectorService _projectDetector;
        private readonly InstallerService _installer;

        public MainWindow()
        {
            InitializeComponent();
            _logger = new LoggerService(LogBox, StatusText);
            _gitService = new GitService(_logger);
            _projectDetector = new ProjectDetectorService();
            _installer = new InstallerService(_logger);

            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            string defaultPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "GitProjects"
            );
            TargetPathBox.Text = defaultPath;
            _logger.Log("GitHab Installer initialized", LogLevel.Info);
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "Select Installation Folder",
                SelectedPath = TargetPathBox.Text
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TargetPathBox.Text = dialog.SelectedPath;
            }
        }

        private async void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InstallButton.IsEnabled = false;
                await PerformInstallation();
            }
            catch (Exception ex)
            {
                _logger.Log($"Fatal error: {ex.Message}", LogLevel.Error);
                MessageBox.Show($"Installation failed:\n\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                InstallButton.IsEnabled = true;
            }
        }

        private async Task PerformInstallation()
        {
            string repoUrl = RepoUrlBox.Text.Trim();
            string targetDir = TargetPathBox.Text.Trim();
            string branch = BranchBox.Text.Trim();
            string token = TokenBox.Password;
            string sshKey = SshKeyBox.Text.Trim();

            // Validation
            if (string.IsNullOrWhiteSpace(repoUrl))
            {
                MessageBox.Show("Please enter a repository URL", "Validation Error");
                return;
            }

            if (string.IsNullOrWhiteSpace(targetDir))
            {
                MessageBox.Show("Please select an installation path", "Validation Error");
                return;
            }

            _logger.Log("Starting installation...", LogLevel.Info);
            _logger.Log($"Repository: {repoUrl}", LogLevel.Info);
            _logger.Log($"Target: {targetDir}", LogLevel.Info);

            // Check Git
            if (!await _gitService.CheckGitInstalled())
            {
                _logger.Log("Git is not installed or not in PATH", LogLevel.Error);
                MessageBox.Show("Git is not installed. Please install Git for Windows first.", "Error");
                return;
            }

            // Create directory
            try
            {
                Directory.CreateDirectory(targetDir);
                _logger.Log($"Target directory created/verified", LogLevel.Success);
            }
            catch (Exception ex)
            {
                _logger.Log($"Failed to create directory: {ex.Message}", LogLevel.Error);
                throw;
            }

            // Clone repository
            try
            {
                await _gitService.CloneRepository(repoUrl, targetDir, branch, token, sshKey);
                _logger.Log("Repository cloned successfully", LogLevel.Success);
            }
            catch (Exception ex)
            {
                _logger.Log($"Clone failed: {ex.Message}", LogLevel.Error);
                throw;
            }

            // Find project folder
            string projectFolder = _projectDetector.FindProjectFolder(targetDir);
            if (string.IsNullOrEmpty(projectFolder))
            {
                _logger.Log("Project folder not found after clone", LogLevel.Warning);
                projectFolder = targetDir;
            }
            else
            {
                _logger.Log($"Project folder detected: {projectFolder}", LogLevel.Info);
            }

            // Detect project type
            var projectType = _projectDetector.DetectProjectType(projectFolder);
            _logger.Log($"Project type: {projectType}", LogLevel.Info);

            // Install dependencies
            if (projectType != ProjectType.Unknown)
            {
                try
                {
                    await _installer.InstallDependencies(projectFolder, projectType);
                    _logger.Log("Dependencies installed successfully", LogLevel.Success);
                }
                catch (Exception ex)
                {
                    _logger.Log($"Dependency installation warning: {ex.Message}", LogLevel.Warning);
                }
            }

            // Post-installation actions
            if (OpenFolderCheckBox.IsChecked == true)
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = projectFolder,
                    UseShellExecute = true
                };
                Process.Start(psi);
                _logger.Log("Installation folder opened", LogLevel.Info);
            }

            _logger.Log("Installation completed successfully", LogLevel.Success);
            MessageBox.Show("Installation completed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearLogButton_Click(object sender, RoutedEventArgs e)
        {
            LogBox.Clear();
            _logger.Log("Log cleared", LogLevel.Info);
        }
    }
}