# HotReloadConfig

> Drop-in config system that hot-reloads appsettings.json at runtime — no restart needed

## What it does
Watches `appsettings.json` for file changes using `FileSystemWatcher`. When a change is detected, re-parses the file and fires strongly-typed `OnChanged<T>` callbacks so your app picks up new values instantly. Debounced to prevent double-fires on save.

## Quick Start
```bash
git clone https://github.com/MrHassan2027/HotReloadConfig
cd HotReloadConfig
dotnet run
# Edit appsettings.json while it's running — changes appear in the console live
```

```csharp
var config = new HotConfig("appsettings.json");
config.OnChanged<AppSettings>(settings =>
{
    Console.WriteLine($"Rate limit changed to {settings.RateLimit}");
});

// Runs forever — edit appsettings.json and watch the console update live
```

## Features
- File-watcher with debounce (prevents double-fires on save)
- Strongly-typed `OnChanged<T>` callbacks via `System.Text.Json`
- Thread-safe callback invocation
- Works in console apps, ASP.NET, and Worker Services

## Tech Stack
| Tool | Why |
|------|-----|
| C# / .NET 8 | `FileSystemWatcher` + `System.Text.Json` |
| Generic types | Strongly-typed config sections |

## Architecture
```
HotReloadConfig/
├── HotConfig.cs      # Core watcher + callback engine
├── Debouncer.cs      # Prevents double-fire on file save
├── Program.cs        # Demo: watches appsettings.json, prints changes
└── appsettings.json  # Sample config file
```
