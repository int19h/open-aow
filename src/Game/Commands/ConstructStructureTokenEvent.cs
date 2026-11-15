// Translated from AoWE.pas.
using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

namespace Aow.Game.Commands;

public partial class ConstructStructureTokenEvent : TokenEvent
{
    public ConstructStructureTokenEvent(CustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135962;

    public int UnitId { get; set; }
    public sbyte Player { get; set; }
    public int ControlId { get; set; }

    // Structur.PAS:3098
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        ControlId = persister.Int32("ControlId", value: ControlId);
        UnitId = persister.Int32("UnitId", value: UnitId);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
    }

    // Structur.PAS:3106
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        Troop troop;
        StructureConstructionControl control;
        StructureResource resource;
        Structure structure;
        string reason = "";
        string text = "";
        int gold = default;
        int days = default;
        ObservableNode owner;
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        MapLevel level;
        HexNumberWalkData walk = default;
        ExecuteResult result = ExecuteResult.DoneHalt;
        base.Execute(tokenEvent);
        troop = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        control = (StructureConstructionControl)((ConstructAbility)GamePack.Current.AbilityControl.GetAbility(125)).ConstructionControls.FindId(ControlId);
        if (control != null && troop != null && troop.Player == Player && control.CanConstruct(troop, ref reason))
        {
            resource = control.StructureResource(troop);
            if (resource != null)
            {
                var coordinate4 = troop.GetLocation();
                x = coordinate4.X;
                y = coordinate4.Y;
                l = coordinate4.MapLevel;
                level = StrategicMap.Current.MapContainer.GetMapLevel(l);
                HexNumberWalkData.InitializeHexNumberWalk(out walk, x, y, 0);
                while (walk.HexNumber < resource.TerrainList.Count)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && level.Width > walk.X && level.Height > walk.Y && resource.TerrainList.GetUsed(walk.HexNumber))
                    {
                        StrategicMap.Current.ClearTerrain(walk.X, walk.Y, l, 0, -1);
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }

                bool locked = troop.Owner is not IHeroHolder;
                owner = locked ? troop.OwnerUnits.Holder : null;
                using var batch = locked ? owner.BeginUpdate() : default;
                structure = (Structure)resource.PlaceResourceUser(StrategicMap.Current, x, y, l);
                if (structure != null)
                {
                    control.GetInfo(troop, out text, out gold, out days);
                    structure.ExecuteBuild(troop, days);
                    var players = StrategicMap.Current.Players.GetPlayers(troop.Player);
                    StrategicMap.Current.Players.GetPlayers(troop.Player).SetGems(players.Gems - gold);
                    troop.Killed();
                    result = ExecuteResult.Done;
                }
            }
        }

        return result;
    }

    // NetworkE.pas:230

}
