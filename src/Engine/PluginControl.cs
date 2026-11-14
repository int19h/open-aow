// Translated from Engine.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine;

// FEngine, FUsedPlugIns. the names of the plug-ins an engine uses, checked as the list changes
public partial class PluginControl : EngineObject
{
    // The name of the abilities plug-in (AbilityP.dpl), the one plug-in the game provides.
    private const string AbilitiesPlugin = "Abilities";

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

    // Engine.pas:9299. The original looked each name up in the registry of loaded plug-ins and
    // initialized the plug-in for the engine. The game loads one plug-in, the abilities package,
    // whose initialization only counted the engines using it, so a name is checked against it.
    internal void InitializePlugins()
    {
        for (int I = 0, last = _usedPlugins.Count - 1; I <= last; I++)
        {
            if (_usedPlugins[I] != AbilitiesPlugin)
            {
                throw new PluginMissingException("PlugIn Missing (" + _usedPlugins[I] + ")");
            }
        }
    }

    // Engine.pas:9252
    public PluginControl(ECustomNode owner) : base(owner)
    {
        _usedPlugins = new StringList();
        _usedPlugins.Sorted = true;
        _usedPlugins.OnChange = OnChange;
        _engine = null;
    }

    // Engine.pas:9262
    public override void Dispose()
    {
        _usedPlugins.Clear();
        _usedPlugins?.Dispose();
        base.Dispose();
    }

    // Engine.pas:9269. Before the engine is set the control reaches it as any node does.
    public override GameEngine GetEngine() => _engine ?? base.GetEngine();

    // Engine.pas:9274
    public override Set<ControlStyleElement> GetControlStyle() => Set<ControlStyleElement>.Of(EngineServices.Stored, EngineServices.Shared);

    // Engine.pas:9331
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        // The original ReadWrite leaves out the used plug-ins under translate (the translate
        // stream), so a translated name does not reach the plug-in check of the list's change.
        if (!PersistSupport.Translating(persister))
        {
            PersistSupport.Lines(persister, name: "UsedPlugins", lines: _usedPlugins);
        }
    }

    public GameEngine Engine { get => _engine; set => _engine = value; }
    public StringCollection UsedPlugins { get => GetUsedPlugins(); set => SetUsedPlugins(value); }

}
