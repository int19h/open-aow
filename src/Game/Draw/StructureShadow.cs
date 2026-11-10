// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Draw;

// connected/visible flags, structure
public partial class StructureShadow : HexagonSprite
{
    public StructureShadow(ECustomNode owner) : base(owner)
    { }

    public Set<StructureShadowStatusElement> FShadowStatus;
    public Structure FStructure;

    // Structur.PAS:934
    public override void Dispose()
    {
        if (FShadowStatus.Contains(StructureShadowStatusElement.Visible))
        {
            AoWHSMap.Current.StaticVisibleHS.RemoveChild(this);
        }

        base.Dispose();
    }

    // Structur.PAS:940
    public override void SetVisible(bool Value)
    {
        if (Value)
        {
            FShadowStatus = FShadowStatus + Set<StructureShadowStatusElement>.Of(StructureShadowStatusElement.Visible);
        }
        else
        {
            FShadowStatus = FShadowStatus - Set<StructureShadowStatusElement>.Of(StructureShadowStatusElement.Visible);
        }
    }

    // Structur.PAS:946
    public override bool GetVisible() => FShadowStatus.Contains(StructureShadowStatusElement.Visible);

    // Structur.PAS:995
    public override int GetShowPriority() => (FStructure.GetShowPriority() & 67108863) + 805306368;

    // Structur.PAS:951
    public override void Connect()
    {
        HNWalkData Walk = default;
        int Count;
        MapLevel Level;
        if (!FShadowStatus.Contains(StructureShadowStatusElement.Connected))
        {
            FShadowStatus = FShadowStatus + Set<StructureShadowStatusElement>.Of(StructureShadowStatusElement.Connected);
            Count = FStructure.GetTerrainCount();
            Level = (MapLevel)FStructure.Owner.Owner;
            HNWalkData.InitHNWalk(ref Walk, FStructure.FXhx, FStructure.FYhx, 0);
            while (Walk.HN < Count)
            {
                if ((FStructure.GetTerrain(Walk.HN) >= -1 || FStructure.GetOverlay(Walk.HN) >= -1) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    Level.GetField(Walk.X, Walk.Y).AddHS(this, Walk.HN);
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }
        }
    }

    // Structur.PAS:973
    public override void Disconnect()
    {
        HNWalkData Walk = default;
        int Count;
        MapLevel Level;
        if (FShadowStatus.Contains(StructureShadowStatusElement.Connected))
        {
            Count = FStructure.GetTerrainCount();
            FShadowStatus = FShadowStatus - Set<StructureShadowStatusElement>.Of(StructureShadowStatusElement.Connected);
            Level = (MapLevel)FStructure.Owner.Owner;
            HNWalkData.InitHNWalk(ref Walk, FStructure.FXhx, FStructure.FYhx, 0);
            while (Walk.HN < Count)
            {
                if ((FStructure.GetTerrain(Walk.HN) >= -1 || FStructure.GetOverlay(Walk.HN) >= -1) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    Level.GetField(Walk.X, Walk.Y).RemoveHS(this);
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }
        }
    }

    // Structur.PAS:1000
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        FStructure.ShowShadow(XhpSurf, YhpSurf, SurfaceDesc);
    }

    // Engine.pas:524

}
