// Translated from Engine.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine;

// FEngine, FUsedPlugIns. the names of the plug-ins an engine uses, initialized and finalized as
// the list changes
public partial class PlugInControl : EObject
{
    internal GameEngine FEngine;
    internal StringList FUsedPlugIns;

    // Engine.pas:9279
    internal void SetUsedPlugIns(StringCollection Value)
    {
        FUsedPlugIns.Assign(Value);
    }

    // Engine.pas:9284
    internal StringCollection GetUsedPlugIns() => FUsedPlugIns;

    // Engine.pas:9289
    internal void OnChange(object Sender)
    {
        InitializePlugIns();
    }

    // Engine.pas:9294
    internal void OnChanging(object Sender)
    {
        FinalizePlugIns();
    }

    // Engine.pas:9299
    internal void InitializePlugIns()
    {
        int[] I__cell = new int[1];
        ref int I = ref I__cell[0];
        PlugIn P;
        I = 0;
        int last = FUsedPlugIns.Count - 1;
        for (; I <= last; I += 1)
        {
            P = FindPlugIn(FUsedPlugIns[I]);
            if (P != null)
            {
                P.Initialize(FEngine);
            }
            else
            {
                throw new PlugInMissingException("PlugIn Missing (" + FUsedPlugIns[I] + ")");
            }
        }
    }

    // Engine.pas:9319
    internal void FinalizePlugIns()
    {
        PlugIn P;
        for (int I = 0, last = FUsedPlugIns.Count - 1; I <= last; I++)
        {
            P = FindPlugIn(FUsedPlugIns[I]);
            if (P != null)
            {
                P.Finalize(FEngine);
            }
        }
    }

    // Engine.pas:9252
    public PlugInControl(ECustomNode owner) : base(owner)
    {
        FUsedPlugIns = new StringList();
        FUsedPlugIns.Sorted = true;
        FUsedPlugIns.OnChange = OnChange;
        FUsedPlugIns.OnChanging = OnChanging;
        FEngine = null;
    }

    // Engine.pas:9262
    public override void Dispose()
    {
        FUsedPlugIns.Clear();
        FUsedPlugIns?.Dispose();
        base.Dispose();
    }

    // Engine.pas:9269
    public override GameEngine GetEngine() => FEngine;

    // Engine.pas:9274
    public override Set<EControlStyleElement> GetControlStyle() => Set<EControlStyleElement>.Of(EngineServices.csStored, EngineServices.csShared);

    // Engine.pas:9331
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(StorageModeElement.Translate))
        {
            Stream.rwStrings(FUsedPlugIns, 1);
        }
    }

    public GameEngine Engine { get => FEngine; set => FEngine = value; }
    public StringCollection UsedPlugIns { get => GetUsedPlugIns(); set => SetUsedPlugIns(value); }

    private static PlugIn FindPlugIn(string Name)
    {
        PlugIn tPlugIn = null;
        if (EngineHost.PlugIns != null)
        {
            for (int i = 0; i != EngineHost.PlugIns.Count; i++)
            {
                if (tPlugIn != null)
                {
                    break;
                }
                if ((EngineHost.PlugIns[i]).Name() == Name)
                {
                    tPlugIn = EngineHost.PlugIns[i];
                }
            }
        }
        return tPlugIn;
    }
}
