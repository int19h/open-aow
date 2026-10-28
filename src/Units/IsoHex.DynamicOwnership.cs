using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;

namespace Aow.Units.IsoHex;

public partial class TIsometricHexagon
{
    protected void UpdateTransitionCore(Type peer, bool water, Action<int, int> shore = null)
    {
        FOpenSides = 0;
        FSides &= 0xC0;
        var field = (TMapField)Owner;
        var level = (TMapLevel)field.Owner;
        THNWalkData walk = default;
        HSEngine.HSEngine.InitHNWalk(ref walk, field.Xhx, field.Yhx, 1);
        while (walk.HN <= 6)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                var neighbour = level.GetField(walk.X, walk.Y);
                var sprite = neighbour.FindOwnedHS(peer);
                bool peerFound = sprite != null;
                sprite ??= neighbour.FindOwnedHS(typeof(BorderHx.TBorderHexagon));
                if (sprite == null)
                {
                    FOpenSides |= unchecked((byte)(1 << (walk.HN - 1)));
                    if (water) shore(walk.HN, neighbour.TerrainType switch { 3 => 40, 5 => 80, _ => 0 });
                }
                else if ((!water || peerFound) && neighbour.TerrainType != GetTerrain(0))
                    FSides |= unchecked((byte)(1 << (walk.HN - 1)));
            }
            HSEngine.HSEngine.WalkToNextHN(ref walk);
        }
        if (water || FOpenSides != 0)
        {
            if (FDynamic == null)
            {
                var dynamic = (TDynamicIsometricHexagon)ClassType.DynamicIsometricHexagonClass().NewInstance();
                dynamic.Attach(this);
                FDynamic = dynamic;
            }
        }
        else if (FDynamic != null)
        {
            FDynamic.Free();
            FDynamic = null;
            FSides = unchecked((byte)(Rtl.Rtl.Random(4) << 6));
            FResource.Engine.HSMap.InvalidateStaticScene(field.Xhx, field.Yhx, field.Lhx);
        }
    }
}

public partial class TUpperIsometricHexagon
{
    public override void UpdateTransitions() => UpdateTransitionCore(typeof(TUpperIsometricHexagon), false);
}

public partial class TLowerIsometricHexagon
{
    public override void UpdateTransitions() => UpdateTransitionCore(typeof(TLowerIsometricHexagon), false);
}

public partial class TDynamicIsometricHexagon
{
    private TIsometricHexagon hexagon;
    private TMapField Field => (TMapField)hexagon.Owner;
    internal void Attach(TIsometricHexagon owner)
    {
        base.Create__init(null);
        hexagon = owner;
        Field.AddHS(this, 0);
    }

    public override TEngine GetEngine() => hexagon.GetEngine();
    public override byte GetLevel() => ((TIsometricHexagonResource)hexagon.FResource).FDynamicLevel;
    public override sbyte GetXhx() => Field.Xhx;
    public override sbyte GetYhx() => Field.Yhx;
    public override sbyte GetLhx() => Field.Lhx;
    public override int GetShowPriority()
    {
        int x = Field.Xhx * 32 + 8;
        int y = (Field.Xhx & 1) * 16 + Field.Yhx * 32;
        return (GetLevel() << 26) + ((y + 32) << 13) + x + 8;
    }
    public override void Show(int x, int y, in TSurfaceDesc surface) => hexagon.ShowDynamic(x, y, surface);
    public override void Destroy()
    {
        if (hexagon != null)
        {
            if (hexagon.GetVisible()) ((TMapLevel)Field.Owner).Map.DynamicVisibleHS.RemoveChild(this);
            Field.RemoveHS(this);
            hexagon.FDynamic = null;
            hexagon = null;
        }
        FOwner = null;
        base.Destroy();
    }
}
