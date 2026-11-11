// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// { Interface declaration run owned by P1. }
public partial class StructureConstructionControl : ConstructionControl
{

    public StructureConstructionControl(ECustomNode owner) : base(owner)
    { }

    // Structur.PAS:3195
    public override bool CanConstruct(Troop AUnit, ref string Reason) => ValidateLocation(AUnit, ref Reason) && (base.CanConstruct(AUnit, ref Reason) && StructureResource(AUnit) != null);

    // Structur.PAS:3201
    public override bool Construct(Troop AUnit)
    {
        bool Result;
        ConstructStructureTokenEvent Event;
        string Reason = "";
        if (CanConstruct(AUnit, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(AUnit.FPlayer).GetBusy())
        {
            Event = new ConstructStructureTokenEvent(null);
            Event.FUnitID = AUnit.FUnitID;
            Event.FPlayer = AUnit.FPlayer;
            Event.FControlID = ID();
            AoWHSMap.Current.FTokenManager.GetTokenControl(AUnit.FPlayer).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:3162
    public virtual bool ValidateLocation(Troop AUnit, ref string Reason)
    {
        StructureResource Resource;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        MapLevel Level;
        AoWMapField MapField;
        HexNumberWalkData Walk = default;
        bool Result = true;
        Resource = StructureResource(AUnit);
        AUnit.GetLocation(ref X, ref Y, ref L);
        Level = AoWHSMap.Current.MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 0);
        while (Resource.TerrainList.Count > Walk.HN && Result)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                if (Resource.TerrainList.GetUsed(Walk.HN))
                {
                    MapField = (AoWMapField)Level.GetField(Walk.X, Walk.Y);
                    Result = MapField.GetPlayer() == -1 || MapField.GetPlayer() == AUnit.FPlayer;
                    Result = Result && MapField.FindNoneTransparentHS(typeof(Structure)) == null;
                    Result = Result && !Set<byte>.WithBounds(0, 255).With(0).With(2).Contains((byte)MapField.OverlayType);
                    Result = Result && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)MapField.TerrainType);
                }
            }
            else
            {
                Result = false;
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        if (!Result)
        {
            Reason = AbilityRules.TranslateRStr(CannotBuildOnCurrentLocationRStr);
        }

        return Result;
    }

    // Structur.PAS:3158
    public virtual StructureResource StructureResource(Troop AUnit) => null;

    private static readonly string CannotBuildOnCurrentLocationRStr = "Cannot build on current location";
}
