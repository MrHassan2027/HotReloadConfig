# HotReloadConfig

> Drop-in config system that hot-reloads appsettings.json at runtime — no restart needed

## What it does
Watches `appsettings.json` (and environment overlays like `appsettings.Production.json`) for file changes using `FileSystemWatcher`. When a change is detected, re-parses the file and fires strongly-typed `OnChanged<T>` callbacks so your app picks up new values instantly.

## Quick Start
```bash
git clone https://github.com/yourusername/HotReloadConfig
cd HotReloadConfig
dotnet run --project HotReloadConfig.Demo
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
- Environment overlay: loads `appsettings.{ASPNETCORE_ENVIRONMENT}.json` on top
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
├── HotConfig.cs          # Core watcher + callback engine
├── ConfigLoader.cs       # JSON parse + overlay merge
├── Debouncer.cs          # Prevents double-fire on file save
└── HotReloadConfig.Demo/ # Console demo app
```
