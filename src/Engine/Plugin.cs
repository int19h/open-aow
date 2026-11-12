// Translated from Engine.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine;

// _refCount, _engines, _counts. A plug-in registers itself in PlugIns on creation and counts
// the engines it is initialized for
public partial class Plugin : IDisposable
{
    private int _refCount;
    private List<GameEngine> _engines;
    private Int32List _counts;

    // Engine.pas:9144
    public Plugin()
    {
        _engines = new List<GameEngine>();
        _counts = new Int32List();
        RegisterPlugin(this);
        _refCount = 1;
    }

    // Engine.pas:9153
    public virtual void Dispose()
    {
        while (_engines.Count != 0)
        {
            Finalize(_engines[_engines.Count - 1]);
        }

        _engines?.Clear();
        _counts?.Dispose();
        UnregisterPlugin(this);
    }

    // Engine.pas:9162
    public void AddRef()
    {
        _refCount += 1;
    }

    // Engine.pas:9167
    public void Release()
    {
        _refCount -= 1;
        if (_refCount == 0)
        {
            Dispose();
        }
    }

    // Engine.pas:9173
    public virtual string Name() => "Abstract PlugIn";

    // Engine.pas:9178
    public virtual bool Initialize(GameEngine engine)
    {
        bool result;
        int I;
        AddRef();
        I = _engines.IndexOf(engine);
        if (I == -1)
        {
            _engines.Add(engine);
            _counts.Add(1);
            result = true;
        }
        else
        {
            var item = _counts[I];
            _counts[I] = item + 1;
            result = false;
        }

        return result;
    }

    // Engine.pas:9197
    public virtual bool Finalize(GameEngine engine)
    {
        bool result;
        int I;
        I = _engines.IndexOf(engine);
        if (I != -1)
        {
            if (_counts[I] == 0)
            {
                throw new ArgumentException("Invalid PlugIn Finalize Usage");
            }

            if (_counts[I] == 1)
            {
                _engines.Remove(engine);
                _counts.Delete(I);
                result = true;
            }
            else
            {
                var item = _counts[I];
                _counts[I] = item - 1;
                result = false;
            }
        }
        else
        {
            result = false;
        }

        Release();
        return result;
    }

    public int RefCount { get => _refCount; }

    private static void RegisterPlugin(Plugin plugin)
    {
        bool flag = false;
        int number = 0;
        while (!flag && number != EngineHost.Plugins.Count)
        {
            flag = (EngineHost.Plugins[number]).Name() == plugin.Name();
            number++;
        }
        if (!flag)
        {
            EngineHost.Plugins.Add(plugin);
            return;
        }
        throw new InvalidOperationException("PlugIn " + plugin.Name() + " already registered");
    }

    private static void UnregisterPlugin(Plugin plugin)
    {
        if (EngineHost.Plugins.IndexOf(plugin) != -1)
        {
            EngineHost.Plugins.Remove(plugin);
            return;
        }
        throw new KeyNotFoundException("PlugIn not found");
    }

    // Engine.pas:550

    // Engine.pas:550

}
