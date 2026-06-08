# Known Issues

## Analyzer and Nullable Warnings

Observed: `dotnet test AutoClicker.sln` passed on 2026-06-08, but the app project emitted nullable and analyzer warnings. Examples include `ICommand` nullability mismatches in `Infrastructure/Commands/Base/Command.cs`, nullable warnings in `LambdaCommand.cs`, disposable ownership warnings in `MainWindowViewModel.cs` and `MouseClicker.cs`, and analyzer warnings in hotkey, state, and tray code.

Impact: The current test suite passes, but warning cleanup may become necessary if the repository later enables warnings as errors or adds stricter analyzer policy.

Next Step: Address warnings in focused groups, starting with command nullability and disposable ownership because they affect shared infrastructure.

## EditorConfig Contains Embedded File Snippets

Observed: `.editorconfig` includes text labelled as `Directory.Build.props`, `.gitattributes`, `.gitignore`, and an older README snippet after the analyzer settings. No standalone `Directory.Build.props` file is present in the repository root.

Impact: Repository policy may be misleading: the embedded XML and copied snippets are not separate project configuration files, and the README snippet says `.NET 7.0` while the active projects target `net8.0-windows`.

Next Step: Split intended configuration into real files or remove stale pasted content from `.editorconfig` after confirming the intended analyzer policy.

## Sandbox Needs NuGet Config Access for Dotnet Test

Observed: A sandboxed `dotnet test AutoClicker.sln` run failed because the .NET SDK could not read `C:\Users\Denis\AppData\Roaming\NuGet\NuGet.Config`. The same command passed after approval to run outside the sandbox.

Impact: Future agents may see an environment failure before a real test result.

Workaround: Retry `dotnet test AutoClicker.sln` with scoped approval when the failure is limited to reading the user NuGet configuration.
