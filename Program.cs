using HotReloadConfig;

var configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
File.Copy("appsettings.json", configPath, overwrite: true);

using var config = new HotConfig(configPath);

config.OnChanged<AppSettings>(s =>
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Config reloaded: AppName={s.AppName}, LogLevel={s.LogLevel}, MaxConnections={s.MaxConnections}");
    Console.ResetColor();
});

Console.WriteLine("Watching config for changes. Edit appsettings.json to see live reload.");
Console.WriteLine($"Config file: {configPath}");
Console.WriteLine("Press ENTER to quit.\n");

// Show initial values
var initial = System.Text.Json.JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(configPath))!;
Console.WriteLine($"Initial: AppName={initial.AppName}, LogLevel={initial.LogLevel}, MaxConnections={initial.MaxConnections}");

Console.ReadLine();

class AppSettings
{
    public string AppName { get; set; } = "";
    public string LogLevel { get; set; } = "";
    public int MaxConnections { get; set; }
}
