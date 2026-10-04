# GitHab Installer

**Universal Git Project Installer for Windows 11**

A modern, cross-platform GUI application that automates the process of cloning Git repositories and installing their dependencies.

## Features

✅ **Easy Repository Cloning**
- Input any GitHub/GitLab repository URL
- Support for branches and tags
- Shallow clone for faster downloads

✅ **Automatic Dependency Installation**
- **Node.js**: npm install
- **Python**: pip install / poetry
- **.NET**: dotnet restore
- **PHP**: composer install
- **Java**: Maven / Gradle
- **Go**: go mod download
- **Rust**: cargo build
- **Ruby**: bundle install

✅ **Authentication Support**
- GitHub Personal Access Tokens
- SSH key authentication
- Secure credential handling

✅ **Post-Installation Options**
- Auto-run projects
- Create desktop shortcuts
- Open installation folder

✅ **Professional UI**
- Modern Windows 11 design
- Real-time installation logging
- Status indicator
- Error handling and recovery

## Installation

### Requirements
- Windows 11 or later
- .NET 8 Runtime
- Git for Windows (https://git-scm.com/download/win)

### Download
Download the latest release from [Releases](https://github.com/AxeLer78/GitHabInstaller/releases)

### Build from Source

```bash
git clone https://github.com/AxeLer78/GitHabInstaller.git
cd GitHabInstaller
dotnet build -c Release
```

## Usage

1. **Enter Repository URL**
   ```
   https://github.com/FFmpeg/FFmpeg
   ```

2. **Select Installation Path**
   - Click "Browse" to choose a folder
   - Default: `%USERPROFILE%\GitProjects`

3. **Optional: Select Branch**
   - Leave empty for default branch
   - Example: `master`, `develop`, `release-4.4`

4. **Optional: Authentication**
   - GitHub Token (for private repos)
   - SSH Key path (for SSH authentication)

5. **Click "Install"**
   - Application will:
     - Clone the repository
     - Detect project type
     - Install dependencies automatically
     - Show real-time log output

## Architecture

### Project Structure
```
GitHabInstaller/
├── App.xaml                      # Application resources
├── MainWindow.xaml               # UI definition
├── MainWindow.xaml.cs            # UI logic
├── GitHabInstaller.csproj        # Project file
└── Services/
    ├── GitService.cs             # Git operations
    ├── LoggerService.cs          # Logging system
    ├── ProjectDetectorService.cs # Project type detection
    └── InstallerService.cs       # Dependency installation
```

### Key Classes

**GitService**
- Repository cloning
- Git validation
- URL authentication handling

**ProjectDetectorService**
- Automatic project type detection
- Support for 9 different project types

**InstallerService**
- Dependency installation for each project type
- Process execution and monitoring

**LoggerService**
- Real-time log display
- Status updates
- Color-coded output

## Configuration

No configuration files needed. All settings are available in the UI.

## Troubleshooting

### "Git not found in PATH"
- Install Git for Windows: https://git-scm.com/download/win
- Restart the application after installation

### "Clone failed"
- Check repository URL is correct
- For private repos, provide GitHub token or SSH key
- Check your internet connection

### "Dependency installation failed"
- Ensure required tools are installed:
  - Node.js for npm
  - Python 3 for pip
  - .NET SDK for dotnet
  - PHP for composer, etc.

## Development

### Contributing

We welcome contributions! Please follow these steps:

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/my-feature`
3. Make your changes
4. Write/update tests
5. Commit: `git commit -am 'Add feature'`
6. Push: `git push origin feature/my-feature`
7. Create a Pull Request

### Pull Request Process
- Describe what your PR does
- Link related issues
- Ensure code follows C# conventions
- Test on Windows 11
- Wait for code review

### Code Style
- Use C# naming conventions (PascalCase for public members)
- Add XML documentation for public methods
- Keep methods focused and small
- Use async/await for I/O operations

### Setting Up Development Environment

```bash
# Clone repository
git clone https://github.com/AxeLer78/GitHabInstaller.git
cd GitHabInstaller

# Open in Visual Studio 2022
start GitHabInstaller.sln

# Or build from command line
dotnet build

# Run tests (when added)
dotnet test

# Publish release
dotnet publish -c Release -r win-x64
```

## Roadmap

- [ ] Support for additional version control systems (GitLab, Bitbucket)
- [ ] Project templates for quick scaffolding
- [ ] Built-in project runner/launcher
- [ ] Installation history and management
- [ ] Cloud sync for settings
- [ ] Plugin system for custom installers
- [ ] Localization support
- [ ] Portable ZIP distribution

## License

MIT License - See [LICENSE](LICENSE) file for details

## Support

- 📧 Issues: [GitHub Issues](https://github.com/AxeLer78/GitHabInstaller/issues)
- 💬 Discussions: [GitHub Discussions](https://github.com/AxeLer78/GitHabInstaller/discussions)
- 📖 Wiki: [GitHub Wiki](https://github.com/AxeLer78/GitHabInstaller/wiki)

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for version history

---

**Made with ❤️ for developers**