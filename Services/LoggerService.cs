using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace GitHabInstaller.Services
{
    public enum LogLevel
    {
        Info,
        Success,
        Warning,
        Error
    }

    public class LoggerService
    {
        private readonly TextBox _logBox;
        private readonly TextBlock _statusText;

        public LoggerService(TextBox logBox, TextBlock statusText)
        {
            _logBox = logBox;
            _statusText = statusText;
        }

        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            string prefix = level switch
            {
                LogLevel.Success => "[✓]",
                LogLevel.Warning => "[!]",
                LogLevel.Error => "[✗]",
                _ => "[i]"
            };

            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            string logEntry = $"{timestamp} {prefix} {message}";

            _logBox.AppendText(logEntry + Environment.NewLine);
            _logBox.ScrollToEnd();

            _statusText.Text = message;

            if (level == LogLevel.Error)
            {
                _statusText.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (level == LogLevel.Success)
            {
                _statusText.Foreground = new SolidColorBrush(Colors.Green);
            }
            else if (level == LogLevel.Warning)
            {
                _statusText.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                _statusText.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
    }
}