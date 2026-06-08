using System.Text.Json;

namespace HotReloadConfig;

public class HotConfig : IDisposable
{
    private readonly string _path;
    private readonly List<(Type type, Action<object> callback)> _handlers = new();
    private readonly FileSystemWatcher _watcher;
    private readonly Debouncer _debouncer = new(TimeSpan.FromMilliseconds(300));
    private JsonDocument? _current;

    public HotConfig(string path)
    {
        _path = Path.GetFullPath(path);
        Reload();
        _watcher = new FileSystemWatcher(Path.GetDirectoryName(_path)!, Path.GetFileName(_path))
        {
            NotifyFilter = NotifyFilters.LastWrite,
            EnableRaisingEvents = true,
        };
        _watcher.Changed += (_, _) => _debouncer.Invoke(OnFileChanged);
    }

    public void OnChanged<T>(Action<T> callback) where T : new()
    {
        _handlers.Add((typeof(T), obj => callback((T)obj)));
    }

    private void OnFileChanged()
    {
        Reload();
        FireCallbacks();
    }

    private void Reload()
    {
        try
        {
            var json = File.ReadAllText(_path);
            _current = JsonDocument.Parse(json);
        }
        catch { }
    }

    private void FireCallbacks()
    {
        if (_current == null) return;
        foreach (var (type, cb) in _handlers)
        {
            try
            {
                var obj = JsonSerializer.Deserialize(_current.RootElement.GetRawText(), type);
                if (obj != null) cb(obj);
            }
            catch { }
        }
    }

    public void Dispose()
    {
        _watcher.Dispose();
        _current?.Dispose();
    }
}
