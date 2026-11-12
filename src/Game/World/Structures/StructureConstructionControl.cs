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
    public override bool CanConstruct(Troop troop, ref string reason) => ValidateLocation(troop, ref reason) && (base.CanConstruct(troop, ref reason) && StructureResource(troop) != null);

    // Structur.PAS:3201
    public override bool Construct(Troop troop)
    {
        bool result;
        ConstructStructureTokenEvent eventValue;
        string reason = "";
        if (CanConstruct(troop, ref reason) && !StrategicMap.Current.Players.GetPlayers(troop.Player).GetBusy())
        {
            eventValue = new ConstructStructureTokenEvent(null) { Holder = CreationHolder() };
            eventValue.UnitId = troop.UnitId;
            eventValue.Player = troop.Player;
            eventValue.ControlId = Id();
            StrategicMap.Current.TokenManager.GetTokenControl(troop.Player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:3162
    public virtual bool ValidateLocation(Troop troop, ref string reason)
    {
        StructureResource resource;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        MapLevel level;
        MapField mapField;
        HexNumberWalkData walk = default;
        bool result = true;
        resource = StructureResource(troop);
        troop.GetLocation(out X, out Y, out L);
        level = StrategicMap.Current.MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
        while (resource.TerrainList.Count > walk.HexNumber && result)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                if (resource.TerrainList.GetUsed(walk.HexNumber))
                {
                    mapField = level.GetField(walk.X, walk.Y);
                    result = mapField.GetPlayer() == -1 || mapField.GetPlayer() == troop.Player;
                    result = result && mapField.FindNoneTransparentHexSprite(typeof(Structure)) == null;
                    result = result && !Set<byte>.WithBounds(0, 255).With(0).With(2).Contains((byte)mapField.OverlayType);
                    result = result && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)mapField.TerrainType);
                }
            }
            else
            {
                result = false;
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        if (!result)
        {
            reason = AbilityRules.TranslateResourceString(GetEngine(), s_cannotBuildOnCurrentLocationText);
        }

        return result;
    }

    // Structur.PAS:3158
    public virtual StructureResource StructureResource(Troop troop) => null;

    private static readonly string s_cannotBuildOnCurrentLocationText = "Cannot build on current location";
}
