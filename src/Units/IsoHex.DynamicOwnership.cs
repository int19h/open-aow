using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World.Hexes
{

    public partial class IsometricHexagon
    {
        protected void UpdateTransitionCore(Type peer, bool water, Action<int, int> shore = null)
        {
            FOpenSides = 0;
            FSides &= 0xC0;
            var field = (MapField)Owner;
            var level = (MapLevel)field.Owner;
            HNWalkData walk = default;
            HNWalkData.InitHNWalk(ref walk, field.Xhx, field.Yhx, 1);
            while (walk.HN <= 6)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    var neighbour = level.GetField(walk.X, walk.Y);
                    var sprite = neighbour.FindOwnedHS(peer);
                    bool peerFound = sprite != null;
                    sprite ??= neighbour.FindOwnedHS(typeof(BorderHexagon));
                    if (sprite == null)
                    {
                        FOpenSides |= (byte)(1 << (walk.HN - 1));
                        if (water) shore(walk.HN, neighbour.TerrainType switch { 3 => 40, 5 => 80, _ => 0 });
                    }
                    else if ((!water || peerFound) && neighbour.TerrainType != GetTerrain(0))
                        FSides |= (byte)(1 << (walk.HN - 1));
                }
                HNWalkData.WalkToNextHN(ref walk);
            }
            if (water || FOpenSides != 0)
            {
                if (FDynamic == null) FDynamic = new DynamicIsometricHexagon(this);
            }
            else if (FDynamic != null)
            {
                FDynamic?.Dispose();
                FDynamic = null;
                FSides = (byte)(RtlUnit.Random(4) << 6);
                FResource.Engine.HSMap.InvalidateStaticScene(field.Xhx, field.Yhx, field.Lhx);
            }
        }
    }

    public partial class UpperIsometricHexagon
    {
        public override void UpdateTransitions() => UpdateTransitionCore(typeof(UpperIsometricHexagon), false);
    }

    public partial class LowerIsometricHexagon
    {
        public override void UpdateTransitions() => UpdateTransitionCore(typeof(LowerIsometricHexagon), false);
    }
}

namespace Aow.Game.Draw
{

    public partial class DynamicIsometricHexagon
    {
        private IsometricHexagon hexagon;

        private MapField Field => (MapField)hexagon.Owner;

        // IsoHex.pas:109. The TEObject body runs with a null owner (the hexagon owns the sprite directly,
        // not as a child), then the sprite is attached and the field of the hexagon shows it.
        public DynamicIsometricHexagon(IsometricHexagon owner) : base(null)
        {
            hexagon = owner;
            Field.AddHS(this, 0);
        }

        public override Engine.Engine GetEngine() => hexagon.GetEngine();
        public override byte GetLevel() => ((IsometricHexagonResource)hexagon.FResource).FDynamicLevel;
        public override sbyte GetXhx() => Field.Xhx;
        public override sbyte GetYhx() => Field.Yhx;
        public override sbyte GetLhx() => Field.Lhx;

        public override int GetShowPriority()
        {
            int x = Field.Xhx * 32 + 8;
            int y = (Field.Xhx & 1) * 16 + Field.Yhx * 32;
            return (GetLevel() << 26) + ((y + 32) << 13) + x + 8;
        }

        public override void Show(int x, int y, in SurfaceDesc surface) => hexagon.ShowDynamic(x, y, surface);

        public override void Dispose()
        {
            if (hexagon != null)
            {
                if (hexagon.GetVisible()) ((MapLevel)Field.Owner).Map.DynamicVisibleHS.RemoveChild(this);
                Field.RemoveHS(this);
                hexagon.FDynamic = null;
                hexagon = null;
            }
            FOwner = null;
            base.Dispose();
        }
    }
}
