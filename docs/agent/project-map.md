# Project Map

## Solution Layout

- `AutoClicker.sln` is the active solution.
- `AutoClicker/` is the WPF desktop application.
- `AutoClicker.Tests/` is the xUnit test project.
- `README.md` describes app features, user workflow, .NET 8.0 requirement, and version notes.
- `.editorconfig`, `.gitattributes`, and `.gitignore` are the visible repository configuration files.

## Runtime Flow

1. `AutoClicker/Program.cs` creates the WPF `App` and exposes the host builder.
2. `AutoClicker/App.xaml.cs` configures services, loads settings, starts the host, initializes tray behavior, and saves settings on exit.
3. `AutoClicker/Views/Main/MainWindow.xaml.cs` registers the HWND hotkey hook, hides to tray when configured, and performs exit cleanup.
4. `AutoClicker/ViewModels/MainWindowViewModel.cs` owns the main click settings, commands, start delay, countdown, fixed-position validation, and click configuration assembly.
5. `AutoClicker/Models/Clicks` and `AutoClicker/Models/States` translate UI strings and options into click-loop state.
6. `AutoClicker/Services/MouseClicker/MouseClicker.cs` runs the cancellable click loop.
7. `AutoClicker/Services/MouseClicker/DefaultClickExecutor.cs` performs the actual cursor movement and mouse input.

## Extension Points

- Add new services through `AutoClicker/Services/ServicesRegistrator.cs` or `AutoClicker/ViewModels/ViewModelsRegistrator.cs`.
- Add user-visible main-window state in `MainWindowViewModel` and bind it from `Views/Main/MainWindow.xaml`.
- Add hotkey behavior through `Models/Hotkeys`, `HotKeyWindowViewModel`, and the HWND hook in `MainWindow`.
- Add parsing behavior in `Models/Parsing` with matching tests under `AutoClicker.Tests/Parsing`.
- Add click-loop timing behavior through `IClickerTiming`, `MouseClicker`, and `AutoClicker.Tests/MouseClicker`.

## Boundaries to Preserve

- Keep platform-specific User32 calls out of view models.
- Keep UI thread updates on the WPF dispatcher when state can be raised from background work.
- Keep settings updates centralized through `ISettingsService.Update`, `Load`, and `Save`.
- Keep click execution testable through `IClickExecutor` and `IClickerTiming`.
- Keep tray UI disposal explicit; `NotifyIcon`, context menu items, icons, and streams are disposable.
