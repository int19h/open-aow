#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

namespace Aow.Game.Draw;

// connected/visible flags, structure
public sealed class StructureShadow(CustomNode? owner) : HexSprite(owner)
{
    public StructureShadowStatuses ShadowStatus { get; private set; }
    public Structure Structure { get; init; }

    // Structur.PAS:934
    public override void Dispose()
    {
        if (ShadowStatus.Contains(StructureShadowStatuses.Visible))
        {
            _ = StrategicMap.Current.StaticVisibleHexSprite.RemoveChild(this);
        }

        base.Dispose();
    }

    // Structur.PAS:946
    public override bool Visible
    {
        get => ShadowStatus.Contains(StructureShadowStatuses.Visible);
        // Structur.PAS:940
        set => ShadowStatus = value
            ? ShadowStatus | StructureShadowStatuses.Visible
            : ShadowStatus & ~StructureShadowStatuses.Visible;
    }

    // Structur.PAS:995
    public override int GetShowPriority() => (Structure.GetShowPriority() & 0b0011_1111_1111_1111_1111_1111_1111) + 805_306_368;

    // Structur.PAS:951
    public override void Connect()
    {
        if (!ShadowStatus.Contains(StructureShadowStatuses.Connected))
        {
            ShadowStatus |= StructureShadowStatuses.Connected;
            int count = Structure.TerrainCount;
            MapLevel level = Structure.Owner.Owner;
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, Structure.StoredHexX, Structure.StoredHexY, radius: 0);
            while (walk.HexNumber < count)
            {
                if ((Structure.GetTerrain(walk.HexNumber) >= -1 || Structure.GetOverlay(walk.HexNumber) >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    _ = level.GetField(walk.X, walk.Y).AddHexSprite(this, walk.HexNumber);
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
    }

    // Structur.PAS:973
    public override void Disconnect()
    {
        if (ShadowStatus.Contains(StructureShadowStatuses.Connected))
        {
            int count = Structure.TerrainCount;
            ShadowStatus &= ~StructureShadowStatuses.Connected;
            MapLevel level = Structure.Owner.Owner;
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, Structure.StoredHexX, Structure.StoredHexY, radius: 0);
            while (walk.HexNumber < count)
            {
                if ((Structure.GetTerrain(walk.HexNumber) >= -1 || Structure.GetOverlay(walk.HexNumber) >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    _ = level.GetField(walk.X, walk.Y).RemoveHexSprite(this);
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
    }

    // Structur.PAS:1000
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface) => Structure.ShowShadow(pixelXSurface, pixelYSurface, lockedSurface);
}
