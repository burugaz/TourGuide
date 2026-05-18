using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System;
using System.Text;

namespace TourGuide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Process? _webProcess;

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if (_webProcess != null && !_webProcess.HasExited)
            {
                AppendLog("Web server уже запущен.");
                return;
            }

            // Try to locate solution root so `dotnet run --project Web` uses the correct path
            string solutionRoot = FindSolutionRoot();

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project \"{System.IO.Path.Combine(solutionRoot, "Web")}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
                CreateNoWindow = true,
                WorkingDirectory = solutionRoot
            };

            _webProcess = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
            _webProcess.OutputDataReceived += (s, ev) => { if (ev.Data != null) AppendLog(ev.Data); };
            _webProcess.ErrorDataReceived += (s, ev) => { if (ev.Data != null) AppendLog(ev.Data); };
            _webProcess.Exited += (s, ev) => AppendLog($"Web server exited with code {_webProcess?.ExitCode}");

            try
            {
                _webProcess.Start();
                _webProcess.BeginOutputReadLine();
                _webProcess.BeginErrorReadLine();
                AppendLog("Web server started (dotnet run --project Web)");
            }
            catch (Exception ex)
            {
                AppendLog("Не удалось запустить web сервер: " + ex.Message + "\nWorkingDirectory=" + startInfo.WorkingDirectory);
            }
        }

        private string FindSolutionRoot()
        {
            try
            {
                var dir = new DirectoryInfo(AppContext.BaseDirectory);
                while (dir != null)
                {
                    if (File.Exists(System.IO.Path.Combine(dir.FullName, "TourGuide.csproj")) ||
                        File.Exists(System.IO.Path.Combine(dir.FullName, "Web", "TourGuide.Web.csproj")))
                    {
                        return dir.FullName;
                    }

                    dir = dir.Parent;
                }
            }
            catch { }

            // fallback to current directory
            return Directory.GetCurrentDirectory();
        }

        private void OpenBrowserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo { FileName = "http://localhost:5000", UseShellExecute = true });
            }
            catch (Exception ex)
            {
                AppendLog("Не удалось открыть браузер: " + ex.Message);
            }
        }

        private void AppendLog(string line)
        {
            Dispatcher.Invoke(() =>
            {
                LogTextBox.AppendText(line + Environment.NewLine);
                LogTextBox.ScrollToEnd();
            });
        }
    }
}