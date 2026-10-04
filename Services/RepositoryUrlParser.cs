using System;

namespace GitHabInstaller.Services
{
    public enum RepositoryHost
    {
        GitHub,
        GitLab,
        Bitbucket,
        Unknown
    }

    public sealed class RepositoryInfo
    {
        public RepositoryHost Host { get; init; } = RepositoryHost.Unknown;
        public string RepoName { get; init; } = string.Empty;
        public string CloneUrl { get; init; } = string.Empty;
        public string SanitizedUrl { get; init; } = string.Empty;
    }

    public static class RepositoryUrlParser
    {
        public static bool TryParse(string value, out RepositoryInfo repo, out string error)
        {
            repo = new RepositoryInfo();
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(value))
            {
                error = "Repository URL is empty.";
                return false;
            }

            string input = value.Trim();

            if (input.Contains("github.com", StringComparison.OrdinalIgnoreCase))
            {
                repo = ParseGitHub(input);
                return true;
            }

            if (input.Contains("gitlab.com", StringComparison.OrdinalIgnoreCase))
            {
                repo = ParseGitLab(input);
                return true;
            }

            if (input.Contains("bitbucket.org", StringComparison.OrdinalIgnoreCase))
            {
                repo = ParseBitbucket(input);
                return true;
            }

            if (input.StartsWith("git@", StringComparison.OrdinalIgnoreCase))
            {
                RepositoryInfo? sshRepo = ParseSsh(input);
                if (sshRepo is null)
                {
                    error = "Unsupported SSH repository URL.";
                    return false;
                }

                repo = sshRepo;
                return true;
            }

            error = "Unsupported repository URL. Use a GitHub, GitLab, Bitbucket, or SSH URL.";
            return false;
        }

        public static string SanitizeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            try
            {
                Uri uri = new Uri(url, UriKind.Absolute);
                if (!string.IsNullOrWhiteSpace(uri.UserInfo))
                {
                    return uri.GetLeftPart(UriPartial.Authority) + uri.PathAndQuery;
                }

                return uri.ToString();
            }
            catch
            {
                return url;
            }
        }

        private static RepositoryInfo ParseGitHub(string input)
        {
            string formatted = input.Trim();
            if (!formatted.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                formatted = "https://" + formatted;
            }

            Uri uri = new Uri(formatted);
            string path = uri.AbsolutePath.Trim('/').Replace(".git", string.Empty);
            return new RepositoryInfo
            {
                Host = RepositoryHost.GitHub,
                RepoName = path,
                CloneUrl = uri.ToString(),
                SanitizedUrl = SanitizeUrl(input)
            };
        }

        private static RepositoryInfo ParseGitLab(string input)
        {
            string formatted = input.Trim();
            if (!formatted.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                formatted = "https://" + formatted;
            }

            Uri uri = new Uri(formatted);
            string path = uri.AbsolutePath.Trim('/').Replace(".git", string.Empty);
            return new RepositoryInfo
            {
                Host = RepositoryHost.GitLab,
                RepoName = path,
                CloneUrl = uri.ToString(),
                SanitizedUrl = SanitizeUrl(input)
            };
        }

        private static RepositoryInfo ParseBitbucket(string input)
        {
            string formatted = input.Trim();
            if (!formatted.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                formatted = "https://" + formatted;
            }

            Uri uri = new Uri(formatted);
            string path = uri.AbsolutePath.Trim('/').Replace(".git", string.Empty);
            return new RepositoryInfo
            {
                Host = RepositoryHost.Bitbucket,
                RepoName = path,
                CloneUrl = uri.ToString(),
                SanitizedUrl = SanitizeUrl(input)
            };
        }

        private static RepositoryInfo? ParseSsh(string input)
        {
            // git@github.com:user/repo.git
            int separatorIndex = input.IndexOf(':');
            if (separatorIndex <= 0)
            {
                return null;
            }

            string hostPart = input.Substring(4, separatorIndex - 4);
            string repoPart = input.Substring(separatorIndex + 1).Trim().TrimEnd('/').Replace(".git", string.Empty);

            RepositoryHost host = hostPart.Contains("gitlab", StringComparison.OrdinalIgnoreCase)
                ? RepositoryHost.GitLab
                : hostPart.Contains("bitbucket", StringComparison.OrdinalIgnoreCase)
                    ? RepositoryHost.Bitbucket
                    : RepositoryHost.GitHub;

            return new RepositoryInfo
            {
                Host = host,
                RepoName = repoPart,
                CloneUrl = input,
                SanitizedUrl = input
            };
        }
    }
}
