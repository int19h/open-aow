// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Draw;

// connected/visible flags, structure
public partial class StructureShadow : HexSprite
{
    public StructureShadow(ECustomNode owner) : base(owner)
    { }

    public Set<StructureShadowStatusElement> ShadowStatus { get; private set; }
    public Structure Structure { get; set; }

    // Structur.PAS:934
    public override void Dispose()
    {
        if (ShadowStatus.Contains(StructureShadowStatusElement.Visible))
        {
            StrategicMap.Current.StaticVisibleHexSprite.RemoveChild(this);
        }

        base.Dispose();
    }

    // Structur.PAS:940
    public override void SetVisible(bool value)
    {
        if (value)
        {
            ShadowStatus = ShadowStatus + Set<StructureShadowStatusElement>.Of(StructureShadowStatusElement.Visible);
        }
        else
        {
            ShadowStatus = ShadowStatus - Set<StructureShadowStatusElement>.Of(StructureShadowStatusElement.Visible);
        }
    }

    // Structur.PAS:946
    public override bool GetVisible() => ShadowStatus.Contains(StructureShadowStatusElement.Visible);

    // Structur.PAS:995
    public override int GetShowPriority() => (Structure.GetShowPriority() & 67108863) + 805306368;

    // Structur.PAS:951
    public override void Connect()
    {
        HexNumberWalkData walk = default;
        int count;
        MapLevel level;
        if (!ShadowStatus.Contains(StructureShadowStatusElement.Connected))
        {
            ShadowStatus = ShadowStatus + Set<StructureShadowStatusElement>.Of(StructureShadowStatusElement.Connected);
            count = Structure.GetTerrainCount();
            level = Structure.Owner.Owner;
            HexNumberWalkData.InitializeHexNumberWalk(out walk, Structure._hexX, Structure._hexY, 0);
            while (walk.HexNumber < count)
            {
                if ((Structure.GetTerrain(walk.HexNumber) >= -1 || Structure.GetOverlay(walk.HexNumber) >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    level.GetField(walk.X, walk.Y).AddHexSprite(this, walk.HexNumber);
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
    }

    // Structur.PAS:973
    public override void Disconnect()
    {
        HexNumberWalkData walk = default;
        int count;
        MapLevel level;
        if (ShadowStatus.Contains(StructureShadowStatusElement.Connected))
        {
            count = Structure.GetTerrainCount();
            ShadowStatus = ShadowStatus - Set<StructureShadowStatusElement>.Of(StructureShadowStatusElement.Connected);
            level = Structure.Owner.Owner;
            HexNumberWalkData.InitializeHexNumberWalk(out walk, Structure._hexX, Structure._hexY, 0);
            while (walk.HexNumber < count)
            {
                if ((Structure.GetTerrain(walk.HexNumber) >= -1 || Structure.GetOverlay(walk.HexNumber) >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    level.GetField(walk.X, walk.Y).RemoveHexSprite(this);
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
    }

    // Structur.PAS:1000
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        Structure.ShowShadow(pixelXSurface, pixelYSurface, lockedSurface);
    }

    // Engine.pas:524

}
