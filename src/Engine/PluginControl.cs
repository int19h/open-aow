// Translated from Engine.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine;

// FEngine, FUsedPlugIns. the names of the plug-ins an engine uses, initialized and finalized as
// the list changes
public partial class PluginControl : EngineObject
{
    private GameEngine _engine;
    private readonly StringList _usedPlugins;

    // Engine.pas:9279
    internal void SetUsedPlugins(StringCollection value)
    {
        _usedPlugins.Assign(value);
    }

    // Engine.pas:9284
    internal StringCollection GetUsedPlugins() => _usedPlugins;

    // Engine.pas:9289
    internal void OnChange(object sender)
    {
        InitializePlugins();
    }

    // Engine.pas:9294
    internal void OnChanging(object sender)
    {
        FinalizePlugins();
    }

    // Engine.pas:9299
    internal void InitializePlugins()
    {
        int[] iCell = new int[1];
        ref int I = ref iCell[0];
        Plugin P;
        I = 0;
        int last = _usedPlugins.Count - 1;
        for (; I <= last; I += 1)
        {
            P = FindPlugin(_usedPlugins[I]);
            if (P != null)
            {
                P.Initialize(_engine);
            }
            else
            {
                throw new PluginMissingException("PlugIn Missing (" + _usedPlugins[I] + ")");
            }
        }
    }

    // Engine.pas:9319
    internal void FinalizePlugins()
    {
        Plugin P;
        for (int I = 0, last = _usedPlugins.Count - 1; I <= last; I++)
        {
            P = FindPlugin(_usedPlugins[I]);
            if (P != null)
            {
                P.Finalize(_engine);
            }
        }
    }

    // Engine.pas:9252
    public PluginControl(ECustomNode owner) : base(owner)
    {
        _usedPlugins = new StringList();
        _usedPlugins.Sorted = true;
        _usedPlugins.OnChange = OnChange;
        _usedPlugins.OnChanging = OnChanging;
        _engine = null;
    }

    // Engine.pas:9262
    public override void Dispose()
    {
        _usedPlugins.Clear();
        _usedPlugins?.Dispose();
        base.Dispose();
    }

    // Engine.pas:9269
    public override GameEngine GetEngine() => _engine;

    // Engine.pas:9274
    public override Set<ControlStyleElement> GetControlStyle() => Set<ControlStyleElement>.Of(EngineServices.Stored, EngineServices.Shared);

    // Engine.pas:9331
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        if (!stream.StorageMode().Contains(StorageModeElement.Translate))
        {
            stream.Lines(_usedPlugins, 1);
        }
    }

    public GameEngine Engine { get => _engine; set => _engine = value; }
    public StringCollection UsedPlugins { get => GetUsedPlugins(); set => SetUsedPlugins(value); }

    private static Plugin FindPlugin(string name)
    {
        Plugin plugin = null;
        if (EngineHost.Plugins != null)
        {
            for (int i = 0; i != EngineHost.Plugins.Count; i++)
            {
                if (plugin != null)
                {
                    break;
                }
                if ((EngineHost.Plugins[i]).Name() == name)
                {
                    plugin = EngineHost.Plugins[i];
                }
            }
        }
        return plugin;
    }
}
