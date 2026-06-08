# Quality Gates

## Scope Control

- Keep changes focused on the app, service, model, or test layer directly involved in the task.
- Avoid broad rewrites of generated settings files, XAML resources, or Win32 interop unless the task requires it.
- Preserve existing repository terminology such as Auto-Clicker, click burst, repeat mode, start delay, stop-after, tray mode, and global hotkeys.

## Architectural Safety

- Keep WPF UI code, view models, services, models, and User32 interop separated.
- Register dependencies through the existing service registration extension methods.
- Keep long-running or blocking work off the UI thread.
- Preserve cancellation behavior around start delay, stop-after, and manual stop.

## Behavioral Safety

- Validate fixed cursor coordinates before starting a click loop.
- Preserve hotkey rollback behavior when new bindings cannot be registered.
- Preserve settings load/save compatibility with `%APPDATA%/AutoClicker/settings.json`.
- Avoid adding click-loop behavior that can spin the CPU at zero interval without yielding.

## Verification

- Run `dotnet test AutoClicker.sln` for changes to parsing, hotkeys, click-loop timing, services, or view-model behavior.
- For UI-only XAML changes, still build or test the solution when possible because WPF compile errors surface at build time.
- Record any warnings or environment failures separately from test pass/fail results.

## Completion Criteria

- The solution builds or tests successfully, or the blocking environment issue is reported with the exact command.
- User-visible behavior changes are reflected in README or agent docs when they affect common workflows.
- New warnings are understood before final reporting.
- Files under `docs/agent/` continue to pass the agent-docs verifier.
