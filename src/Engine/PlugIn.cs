// Translated from Engine.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine;

// FRefCount, FEngines, FCounts. A plug-in registers itself in PlugIns on creation and counts
// the engines it is initialized for
public partial class PlugIn : IDisposable
{
    internal int FRefCount;
    internal DelphiList FEngines;
    internal IntegerList FCounts;

    // Engine.pas:9144
    public PlugIn()
    {
        FEngines = new DelphiList();
        FCounts = new IntegerList();
        RegisterPlugIn(this);
        FRefCount = 1;
    }

    // Engine.pas:9153
    public virtual void Dispose()
    {
        while (FEngines.Count != 0)
        {
            Finalize((GameEngine)FEngines[FEngines.Count - 1]);
        }

        FEngines?.Dispose();
        FCounts?.Dispose();
        UnregisterPlugIn(this);
    }

    // Engine.pas:9162
    public void AddRef()
    {
        FRefCount += 1;
    }

    // Engine.pas:9167
    public void Release()
    {
        FRefCount -= 1;
        if (FRefCount == 0)
        {
            Dispose();
        }
    }

    // Engine.pas:9173
    public virtual string Name() => "Abstract PlugIn";

    // Engine.pas:9178
    public virtual bool Initialize(GameEngine AEngine)
    {
        bool Result;
        int I;
        AddRef();
        I = FEngines.IndexOf(AEngine);
        if (I == -1)
        {
            FEngines.Add(AEngine);
            FCounts.Add(1);
            Result = true;
        }
        else
        {
            var item = FCounts[I];
            FCounts[I] = item + 1;
            Result = false;
        }

        return Result;
    }

    // Engine.pas:9197
    public virtual bool Finalize(GameEngine AEngine)
    {
        bool Result;
        int I;
        I = FEngines.IndexOf(AEngine);
        if (I != -1)
        {
            if (FCounts[I] == 0)
            {
                throw new ArgumentException("Invalid PlugIn Finalize Usage");
            }

            if (FCounts[I] == 1)
            {
                FEngines.Remove(AEngine);
                FCounts.Delete(I);
                Result = true;
            }
            else
            {
                var item = FCounts[I];
                FCounts[I] = item - 1;
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        Release();
        return Result;
    }

    public int RefCount { get => FRefCount; }

    private static void RegisterPlugIn(PlugIn PlugIn)
    {
        bool flag = false;
        int num = 0;
        while (!flag && num != EngineHost.PlugIns.Count)
        {
            flag = ((PlugIn)EngineHost.PlugIns[num]).Name() == PlugIn.Name();
            num++;
        }
        if (!flag)
        {
            EngineHost.PlugIns.Add((object)PlugIn);
            return;
        }
        throw new InvalidOperationException("PlugIn " + PlugIn.Name() + " already registered");
    }

    private static void UnregisterPlugIn(PlugIn PlugIn)
    {
        if (EngineHost.PlugIns.IndexOf((object)PlugIn) != -1)
        {
            EngineHost.PlugIns.Remove((object)PlugIn);
            return;
        }
        throw new KeyNotFoundException("PlugIn not found");
    }

    // Engine.pas:550

    // Engine.pas:550

}
