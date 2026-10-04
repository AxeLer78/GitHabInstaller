# Contributing to GitHab Installer

Thank you for your interest in contributing! This document provides guidelines and instructions.

## Code of Conduct

Be respectful, inclusive, and professional in all interactions.

## How to Contribute

### Reporting Bugs

1. Check [existing issues](https://github.com/AxeLer78/GitHabInstaller/issues)
2. Create a new issue with:
   - Clear title
   - Detailed description
   - Steps to reproduce
   - Expected vs actual behavior
   - Screenshots if applicable
   - Environment info (Windows version, .NET version, etc.)

### Suggesting Features

1. Check [discussions](https://github.com/AxeLer78/GitHabInstaller/discussions)
2. Create a discussion or issue with:
   - Feature description
   - Use case / motivation
   - Proposed solution (optional)
   - Alternative approaches (optional)

### Submitting Code

#### Setup

```bash
# 1. Fork the repository
# 2. Clone your fork
git clone https://github.com/YOUR_USERNAME/GitHabInstaller.git
cd GitHabInstaller

# 3. Add upstream remote
git remote add upstream https://github.com/AxeLer78/GitHabInstaller.git

# 4. Create feature branch
git checkout -b feature/your-feature-name
```

#### Development

```bash
# Install .NET 8 SDK
# Open in Visual Studio 2022 or VS Code

# Build
dotnet build

# Run
dotnet run

# Build release
dotnet build -c Release
```

#### Code Standards

- Follow C# naming conventions
- Use PascalCase for public members
- Use camelCase for private members and local variables
- Add XML documentation comments for public methods
- Keep methods small and focused (< 20 lines ideally)
- Use async/await for I/O operations
- Handle exceptions appropriately
- Add logging for important operations

#### Commit Guidelines

```bash
# Use clear, descriptive commit messages
git commit -m "Add feature: description"
git commit -m "Fix: description of the bug"
git commit -m "Docs: update README"
git commit -m "Refactor: improve code structure"

# Keep commits atomic (one logical change per commit)
# Don't mix refactoring with feature work
```

#### Pull Request Process

1. **Update your branch**
   ```bash
   git fetch upstream
   git rebase upstream/main
   ```

2. **Push to your fork**
   ```bash
   git push origin feature/your-feature-name
   ```

3. **Create Pull Request**
   - Go to https://github.com/AxeLer78/GitHabInstaller/pulls
   - Click "New Pull Request"
   - Select your branch
   - Fill out the PR template:
     - Description of changes
     - Related issues (closes #123)
     - Testing performed
     - Screenshots if UI changes
     - Checklist items

4. **Address Review Comments**
   - Make requested changes
   - Commit with descriptive messages
   - Don't force-push unless asked
   - Respond to comments

5. **Merge**
   - Wait for approval
   - Ensure CI passes
   - Squash commits if needed
   - Merge via GitHub UI

### Areas for Contribution

#### Easy (Good for first-time contributors)
- Documentation improvements
- Typo fixes
- UI/UX polish
- Test coverage
- Error message improvements

#### Medium
- New project type support
- Additional authentication methods
- UI enhancements
- Performance improvements
- Bug fixes

#### Advanced
- Plugin system implementation
- Cloud sync features
- Advanced logging/telemetry
- Architecture refactoring
- Cross-platform support

## Project Structure

```
GitHabInstaller/
├── Services/              # Core business logic
│   ├── GitService.cs
│   ├── LoggerService.cs
│   ├── ProjectDetectorService.cs
│   └── InstallerService.cs
├── MainWindow.xaml        # UI definition
├── MainWindow.xaml.cs     # UI logic
├── App.xaml               # Application resources
└── App.xaml.cs           # Application code-behind
```

## Testing

### Manual Testing

Before submitting PR, test:
- Clone public repositories
- Clone private repositories (with token)
- Test with each supported project type
- Test error scenarios
- Test authentication methods

### Automated Testing

```bash
# Run tests
dotnet test
```

## Documentation

- Update README.md for user-facing changes
- Update inline code comments
- Add XML documentation for new public APIs
- Update CHANGELOG.md
- Create wiki pages for complex features

## Questions?

- Check existing [issues](https://github.com/AxeLer78/GitHabInstaller/issues)
- Read [discussions](https://github.com/AxeLer78/GitHabInstaller/discussions)
- Ask in comments on relevant issues/PRs

## Recognition

Contributors will be:
- Listed in README.md
- Credited in release notes
- Thanked in CHANGELOG.md

Thank you for contributing! 🎉
