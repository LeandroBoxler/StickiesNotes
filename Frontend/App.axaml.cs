using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using System;

namespace StickieNotes
{
    public partial class App : Application
    {
        private TrayIcon? _trayIcon;
        private bool _isExitRequested;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new MainWindow();
                desktop.MainWindow = mainWindow;
                mainWindow.Show();

                // Keep running in background when the user closes the window.
                mainWindow.Closing += (_, e) =>
                {
                    if (_isExitRequested)
                    {
                        return;
                    }

                    e.Cancel = true;
                    mainWindow.Hide();
                };

                _trayIcon = new TrayIcon
                {
                    ToolTipText = "Stickie Notes",
                    IsVisible = true
                };

                using (var iconStream = AssetLoader.Open(new Uri("avares://StickieNotes/Assets/icon.ico")))
                {
                    _trayIcon.Icon = new WindowIcon(iconStream);
                }

                _trayIcon.Clicked += (_, _) =>
                {
                    if (mainWindow.IsVisible)
                    {
                        mainWindow.Hide();
                    }
                    else
                    {
                        mainWindow.Show();
                        mainWindow.Activate();
                    }
                };

                var menu = new NativeMenu();

                var showWindowMenuItem = new NativeMenuItem("Show window");
                showWindowMenuItem.Click += (_, _) =>
                {
                    mainWindow.Show();
                    mainWindow.Activate();
                };
                menu.Items.Add(showWindowMenuItem);

                var exitMenuItem = new NativeMenuItem("Exit");
                exitMenuItem.Click += (_, _) =>
                {
                    _isExitRequested = true;
                    desktop.Shutdown();
                };
                menu.Items.Add(exitMenuItem);

                _trayIcon.Menu = menu;
                TrayIcon.SetIcons(this, new TrayIcons { _trayIcon });
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}