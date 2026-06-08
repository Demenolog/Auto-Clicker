# Command Cookbook

Run commands from the repository root: `C:\Users\Denis\source\repos\Auto-Clicker`.

## Restore

```powershell
dotnet restore AutoClicker.sln
```

This command is consistent with the solution layout and the README build instructions. The 2026-06-08 test run also restored both projects implicitly.

## Build

```powershell
dotnet build AutoClicker.sln
```

The README documents a Release build with:

```powershell
dotnet build -c Release
```

## Test

```powershell
dotnet test AutoClicker.sln
```

Verified on 2026-06-08 with elevated access to the user NuGet configuration. Result: 28 passed, 0 failed, 0 skipped.

## Notes

- The app project targets `net8.0-windows` and requires Windows targeting.
- In the Codex sandbox, `dotnet test AutoClicker.sln` first failed because the SDK could not read `C:\Users\Denis\AppData\Roaming\NuGet\NuGet.Config`; the same command passed after approval to run outside the sandbox.
- No repository-specific publish command is currently documented.
