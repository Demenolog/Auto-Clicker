using AutoClicker.Services.Interfaces;
using AutoClicker.ViewModels;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using WpfApplication = System.Windows.Application;

namespace AutoClicker.Services.TrayIcon
{
    internal sealed class TrayIconService : ITrayIconService
    {
        private readonly MainWindowViewModel _viewModel;
        private NotifyIcon? _notifyIcon;
        private Icon? _trayIcon;
        private MemoryStream? _iconStream;
        private ToolStripMenuItem? _showHideItem;
        private ToolStripMenuItem? _startItem;
        private ToolStripMenuItem? _stopItem;
        private ToolStripMenuItem? _exitItem;
        private ToolStripSeparator? _controlSeparator;
        private ToolStripSeparator? _exitSeparator;
        private ContextMenuStrip? _contextMenu;
        private bool _isInitialized;

        public TrayIconService(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            _trayIcon = LoadTrayIcon(out _iconStream);

            _showHideItem = new ToolStripMenuItem("Show");
            _showHideItem.Click += (_, _) => ToggleMainWindow();

            _startItem = new ToolStripMenuItem("Start");
            _startItem.Click += (_, _) => ExecuteCommand(_viewModel.StartClicking);

            _stopItem = new ToolStripMenuItem("Stop");
            _stopItem.Click += (_, _) => ExecuteCommand(_viewModel.StopClicking);

            _exitItem = new ToolStripMenuItem("Exit");
            _exitItem.Click += (_, _) => ExitApplication();

            _controlSeparator = new ToolStripSeparator();
            _exitSeparator = new ToolStripSeparator();
            _contextMenu = new ContextMenuStrip();
            _contextMenu.Items.AddRange(
            [
                _showHideItem,
                _controlSeparator,
                _startItem,
                _stopItem,
                _exitSeparator,
                _exitItem
            ]);

            _notifyIcon = new NotifyIcon
            {
                Icon = _trayIcon,
                ContextMenuStrip = _contextMenu,
                Text = "AutoClicker",
                Visible = true
            };
            _notifyIcon.DoubleClick += (_, _) => ShowMainWindow();
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;

            UpdateStatus();
            _isInitialized = true;
        }

        public void UpdateStatus()
        {
            if (_notifyIcon is null)
            {
                return;
            }

            RunOnUi(() =>
            {
                var isRunning = _viewModel.IsRunning;
                var statusText = isRunning ? "Running" : "Stopped";
                _notifyIcon.Text = TrimTooltip(statusText);
                _startItem!.Enabled = _viewModel.StartClicking.CanExecute(null);
                _stopItem!.Enabled = _viewModel.StopClicking.CanExecute(null);
                _showHideItem!.Text = IsMainWindowVisible() ? "Hide" : "Show";
            });
        }

        public void ShowBalloon(string title, string message, ToolTipIcon icon, int timeoutMilliseconds)
        {
            if (_notifyIcon is null)
            {
                return;
            }

            RunOnUi(() => _notifyIcon.ShowBalloonTip(timeoutMilliseconds, title, message, icon));
        }

        public void Dispose()
        {
            if (_notifyIcon is null)
            {
                return;
            }

            _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            _contextMenu?.Dispose();
            _exitItem?.Dispose();
            _controlSeparator?.Dispose();
            _exitSeparator?.Dispose();
            _trayIcon?.Dispose();
            _iconStream?.Dispose();
            _notifyIcon = null;
            _trayIcon = null;
            _iconStream = null;
            _contextMenu = null;
            _exitItem = null;
            _controlSeparator = null;
            _exitSeparator = null;
        }

        private static string TrimTooltip(string text)
        {
            const int maxLength = 63;
            if (text.Length <= maxLength)
            {
                return text;
            }

            return text[..maxLength];
        }

        private void ExecuteCommand(System.Windows.Input.ICommand command)
        {
            if (command.CanExecute(null))
            {
                command.Execute(null);
            }
        }

        private static bool IsMainWindowVisible()
        {
            var window = WpfApplication.Current?.MainWindow;
            return window is not null && window.IsVisible;
        }

        private static void ShowMainWindow()
        {
            var window = WpfApplication.Current?.MainWindow;
            if (window is null)
            {
                return;
            }

            if (!window.IsVisible)
            {
                window.Show();
            }

            window.WindowState = WindowState.Normal;
            window.Activate();
        }

        private static void HideMainWindow()
        {
            var window = WpfApplication.Current?.MainWindow;
            if (window is null)
            {
                return;
            }

            window.Hide();
        }

        private void ToggleMainWindow()
        {
            if (IsMainWindowVisible())
            {
                HideMainWindow();
                UpdateShowHideItem();
                return;
            }

            ShowMainWindow();
            UpdateShowHideItem();
        }

        private static void ExitApplication()
        {
            RunOnUi(() =>
            {
                App.RequestExit();
                WpfApplication.Current?.Shutdown();
            });
        }

        private static Icon LoadTrayIcon(out MemoryStream? iconStream)
        {
            var streamInfo = WpfApplication.GetResourceStream(new Uri("pack://application:,,,/Resources/Icons/Main/Cursor.ico"));
            if (streamInfo?.Stream is null)
            {
                iconStream = null;
                return SystemIcons.Application;
            }

            iconStream = new MemoryStream();
            using (streamInfo.Stream)
            {
                streamInfo.Stream.CopyTo(iconStream);
            }
            iconStream.Position = 0;
            return new Icon(iconStream);
        }

        private static void RunOnUi(Action action)
        {
            var dispatcher = WpfApplication.Current?.Dispatcher;
            if (dispatcher is null)
            {
                action();
                return;
            }

            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Invoke(action);
            }
        }

        private void UpdateShowHideItem()
        {
            if (_showHideItem is null)
            {
                return;
            }

            RunOnUi(() => _showHideItem.Text = IsMainWindowVisible() ? "Hide" : "Show");
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowViewModel.IsRunning)
                || e.PropertyName == nameof(MainWindowViewModel.IsStarting))
            {
                UpdateStatus();
            }
        }
    }
}
