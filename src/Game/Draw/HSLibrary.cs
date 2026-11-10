// Translated from HSLib.pas.
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Draw;

// FList (the TLibraryHS it created), FHSEngine, FEngine
public partial class HSLibrary : ImageLibrary
{
    internal DelphiList FList;
    internal GameEngine FHSEngine;
    internal HexagonSpriteEngine FEngine;

    // HSLib.pas:292
    internal void SetEngine(HexagonSpriteEngine Value)
    {
        if (!ComponentState.Contains(Rtl.ComponentState.Designing) && Value != FEngine)
        {
            FEngine = Value;
            FHSEngine = FEngine.HSEngine;
            for (int I = FList.Count - 1; I >= 0; I--)
            {
                ((HexagonSprite)FList[I]).Remove();
            }
        }
    }

    // HSLib.pas:279
    public HSLibrary(Component owner) : base(owner)
    {
        FList = new DelphiList();
    }

    // HSLib.pas:285
    public override void Dispose()
    {
        ClearList();
        FList?.Dispose();
        base.Dispose();
    }

    // HSLib.pas:304
    public void ClearList()
    {
        for (int I = FList.Count - 1; I >= 0; I--)
        {
            ((FList[I] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
        }

        FList.Clear();
    }

    // HSLib.pas:312
    public void RemoveHS(LibraryHS HS)
    {
        FList.Remove(HS);
    }

    // HSLib.pas:317
    public LibraryHS CreateHS(int Index)
    {
        LibraryHS HS;
        HS = new LibraryHS(null);
        HS.SetImage(this[Index]);
        HS.FLibrary = this;
        FList.Add(HS);
        return HS;
    }

    [Published]
    public HexagonSpriteEngine Engine { get => FEngine; set => SetEngine(value); }
}
