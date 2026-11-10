// Translated from HSEngine.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;

namespace Aow.Game.Draw;

// a sprite placed at a pixel position. FVisible, FXhp, FYhp, FLhx (HSLib.TLibraryHS reads FXhp
// and FYhp). Persisted: 4 FXhp, 5 FYhp, 6 FLhx (rwByte)
public partial class HPBasedHS : HexagonSprite
{
    public new const int ClassID = 65874;

    public HPBasedHS(ECustomNode owner) : base(owner)
    {
        FXhp = -2147483648;
        FYhp = -2147483648;
        FLhx = -1;
    }

    protected internal bool FVisible;
    protected internal int FXhp;
    protected internal int FYhp;
    protected internal sbyte FLhx;

    // HXSPRITE.PAS:622
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FXhp, 4);
        Stream.rwInteger(ref FYhp, 5);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FLhx), 6);
    }

    // HXSPRITE.PAS:599

    // HXSPRITE.PAS:612
    public override void SetVisible(bool Value)
    {
        FVisible = Value;
    }

    // HXSPRITE.PAS:617
    public override bool GetVisible() => FVisible;

    // HXSPRITE.PAS:630
    public override bool Place(MapContainer Map, int Xhp, int Yhp, int Lhx)
    {
        FXhp = Xhp;
        FYhp = Yhp;
        FLhx = (sbyte)Lhx;
        return true;
    }

}
