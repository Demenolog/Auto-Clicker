# Task Checklist

## 1) Locate the Layer

- UI or binding issue: start in `AutoClicker/Views` and the matching view model.
- Click behavior issue: start in `Models/Clicks`, `Models/States`, and `Services/MouseClicker`.
- Hotkey issue: start in `Models/Hotkeys`, `HotKeyWindowViewModel`, and `MainWindow.xaml.cs`.
- Settings issue: start in `Services/Settings` and the view model update paths.

## 2) Keep the Boundary Clean

- Keep User32 interop behind infrastructure and service code.
- Keep view models free of direct Win32 calls.
- Keep settings persistence behind `ISettingsService`.
- Keep test seams through interfaces such as `IClickExecutor` and `IClickerTiming`.

## 3) Implement Surgically

- Change the smallest set of files that owns the behavior.
- Preserve existing public and internal model shapes unless the task requires a contract change.
- Add or adjust tests near the behavior being changed.
- Avoid touching generated settings designer files unless the settings schema changes.

## 4) Pick the Right Validation

- Parser change: run `dotnet test AutoClicker.sln`.
- Click-loop timing change: run `dotnet test AutoClicker.sln`.
- Hotkey registration change: run `dotnet test AutoClicker.sln` and inspect UI hook behavior if manual verification is possible.
- XAML or resource change: build or test the solution to catch WPF compile issues.

## 5) Report Precisely

- State the files changed.
- State the validation command and result.
- Call out warnings separately from failures.
- Mention any environment-specific approval needed to run the command.
