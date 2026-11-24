#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

namespace Aow.Game.Commands;

public class ConstructStructureTokenEvent(CustomNode? owner) : TokenEvent(owner)
{
    public new const int ClassId = 135_962;

    public int UnitId { get; set; }
    public PlayerId? Player { get; set; } = PlayerId.Independent;
    public int ControlId { get; set; }

    // Structur.PAS:3098
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        ControlId = persister.Int32("ControlId", value: ControlId);
        UnitId = persister.Int32("UnitId", value: UnitId);
        Player = persister.OptionalPlayer(name: "Player", value: Player, width: IdWidth.UnsignedByte, fallback: 0);
    }

    // Structur.PAS:3106
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        string reason = "";
        _ = base.Execute(tokenEvent);
        Troop? troop = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        StructureConstructionControl control = GamePack.Current.AbilityControl.GetAbility<ConstructAbility>(125).ConstructionControls.FindId<StructureConstructionControl>(ControlId);
        if (control is not null && troop is not null && troop.Player == Player && control.CanConstruct(troop, ref reason))
        {
            StructureResource? resource = control.StructureResource(troop);
            if (resource is not null)
            {
                MapCoordinate coordinate4 = troop.GetLocation();
                int x = coordinate4.X;
                int y = coordinate4.Y;
                int l = coordinate4.MapLevel;
                MapLevel level = StrategicMap.Current.MapContainer.GetMapLevel(l);
                HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, radius: 0);
                while (walk.HexNumber < resource.TerrainList.Count)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && level.Width > walk.X && level.Height > walk.Y && resource.TerrainList.GetUsed(walk.HexNumber))
                    {
                        StrategicMap.Current.ClearTerrain(walk.X, walk.Y, l, radius: 0, terrainType: -1);
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }

                bool locked = troop.Owner is not IHeroHolder;
                ObservableNode? owner = locked ? troop.OwnerUnits.Holder : null;
                using ObservableNode.UpdateBatch batch = locked ? owner.BeginUpdate() : default;
                Structure structure = resource.PlaceResourceUser<Structure>(StrategicMap.Current, hexX: x, hexY: y, hexLevel: l);
                if (structure is not null)
                {
                    control.GetInfo(troop, out string text, out int gold, out int days);
                    _ = structure.ExecuteBuild(troop, days);
                    Player? players = StrategicMap.Current.Players.GetPlayers(troop.Player);
                    StrategicMap.Current.Players.GetPlayers(troop.Player).SetGems(players.Gems - gold);
                    troop.Killed();
                    return ExecuteResult.Done;
                }
            }
        }

        return ExecuteResult.DoneHalt;
    }
}
