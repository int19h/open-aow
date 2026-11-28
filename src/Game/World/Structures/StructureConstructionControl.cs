#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World.Structures;

// { Interface declaration run owned by P1. }
public class StructureConstructionControl(CustomNode owner) : ConstructionControl(owner)
{

    // Structur.PAS:3195
    public override bool CanConstruct(Troop troop, ref string reason) => ValidateLocation(troop, ref reason) && base.CanConstruct(troop, ref reason) && StructureResource(troop) is not null;

    // Structur.PAS:3201
    public override bool Construct(Troop troop)
    {
        string reason = "";
        if (CanConstruct(troop, ref reason) && !GetEngine().StrategicMap.Players.GetPlayers(troop.Player).GetBusy())
        {
            ConstructStructureTokenEvent eventValue = new(null)
            {
                Holder = CreationHolder(),
                UnitId = troop.UnitId,
                Player = troop.Player,
                ControlId = Id()
            };
            GetEngine().StrategicMap.TokenManager.GetTokenControl(troop.Player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Structur.PAS:3162
    public virtual bool ValidateLocation(Troop troop, ref string reason)
    {
        bool result = true;
        StructureResource? resource = StructureResource(troop);
        MapCoordinate coordinate39 = troop.GetLocation();
        int x = coordinate39.X;
        int y = coordinate39.Y;
        int l = coordinate39.MapLevel;
        MapLevel level = GetEngine().StrategicMap.MapContainer.GetMapLevel(l);
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, radius: 0);
        while (resource.TerrainList.Count > walk.HexNumber && result)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                if (resource.TerrainList.GetUsed(walk.HexNumber))
                {
                    MapField mapField = level.GetField(walk.X, walk.Y);
                    result = mapField.GetPlayer() is null || mapField.GetPlayer() == troop.Player;
                    result = result && mapField.FindNoneTransparentHexSprite(typeof(Structure)) is null;
                    result = result && mapField.OverlayType is not (OverlayType.Mountain or OverlayType.Hill);
                    result = result && mapField.Terrain is (>= TerrainType.Grass and <= TerrainType.Wasteland) or TerrainType.Dirt;
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
            reason = AbilityRules.TranslateResourceString(GetEngine(), S: s_cannotBuildOnCurrentLocationText);
        }

        return result;
    }

    // Structur.PAS:3158
    public virtual StructureResource? StructureResource(Troop troop) => null;

    private static readonly string s_cannotBuildOnCurrentLocationText = "Cannot build on current location";
}
