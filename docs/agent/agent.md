# Project Agent Guide

Last refreshed: 2026-06-08

## Mission

Auto-Clicker is a Windows desktop app for automating mouse clicks. It lets users choose interval, mouse button, click burst, cursor position, repeat mode, start delay, stop-after timing, tray behavior, and global start/stop hotkeys.

## Current Repository Shape

- `AutoClicker.sln` contains the app project `AutoClicker/AutoClicker.csproj` and the xUnit project `AutoClicker.Tests/AutoClicker.Tests.csproj`.
- `AutoClicker` targets `net8.0-windows`, enables WPF and Windows Forms, and starts from `AutoClicker.Program`.
- `AutoClicker.Tests` targets `net8.0-windows`, references the app project, and uses xUnit with `Microsoft.NET.Test.Sdk`.
- `AutoClicker/ViewModels` owns user-facing state and commands for the main window and hotkey window.
- `AutoClicker/Services` owns settings, tray icon behavior, child-window handling, and click-loop services.
- `AutoClicker/Models` owns click configuration, parsing, hotkey definitions, and state translation.
- `AutoClicker/Infrastructure/UnsafeCode/User32.cs` contains Win32 interop for cursor, hotkey, and input APIs.

## Major Runtime Flow

`Program.Main` creates `App`, initializes WPF, and runs it. `App` builds a `Microsoft.Extensions.Hosting` host, loads settings from `%APPDATA%/AutoClicker/settings.json`, starts the host, and initializes the tray icon service.

The main window registers global hotkey hooks during `OnSourceInitialized`. `GlobalHotKey` maps configured start and stop bindings to `MainWindowViewModel` commands. The view model builds a `ClickConfig`, validates fixed coordinates against the virtual screen, manages start delay and countdown state, and calls `IMouseClicker.StartClicking`.

`MouseClicker` guards the click loop with a lock and cancellation token source. It snapshots click options, waits for start delay, links optional stop-after cancellation, and delegates actual mouse input to `IClickExecutor`. `DefaultClickExecutor` moves the cursor and sends mouse down/up input through User32 `SendInput`.

On app exit, settings are saved, the main window removes the hotkey hook, any active click loop is stopped, registered hotkeys are unregistered, child windows are closed, the tray icon is disposed, and the host is stopped.

## Important Boundaries

- Keep Win32 interop isolated behind `AutoClicker/Infrastructure/UnsafeCode` and click services.
- Preserve the MVVM shape: view models should coordinate state and commands, while services own side effects.
- Do not bypass `ISettingsService` when changing persisted user settings.
- Do not let multiple click loops run concurrently; `MouseClicker` is the guard for this.
- Treat hotkey registration as a recoverable operation: failed new bindings should not strand previous working bindings.
- Keep parser and click-loop behavior covered by focused tests before changing user-visible timing, repeat, or hotkey semantics.

## Verification Snapshot

2026-06-08:

- `dotnet test AutoClicker.sln` passed after running with access to the user NuGet configuration.
- Result: 28 passed, 0 failed, 0 skipped.
- The build emitted analyzer and nullable warnings in the app project; see `known-issues.md`.
- A sandboxed first attempt failed because the .NET SDK could not read `C:\Users\Denis\AppData\Roaming\NuGet\NuGet.Config`.
