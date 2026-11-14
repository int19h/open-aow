// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

namespace Aow.Game.Commands;

public partial class ConstructStructureTokenEvent : TokenEvent
{
    public ConstructStructureTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135962;

    public int UnitId;
    public sbyte Player;
    public int ControlId;

    // Structur.PAS:3098
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref ControlId, 20);
        stream.Int32(ref UnitId, 21);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Player), 22);
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
        EChangeNotifyNode owner;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
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
                troop.GetLocation(out X, out Y, out L);
                level = StrategicMap.Current.MapContainer.GetMapLevel(L);
                HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
                while (walk.HexNumber < resource.TerrainList.Count)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && level.Width > walk.X && level.Height > walk.Y && resource.TerrainList.GetUsed(walk.HexNumber))
                    {
                        StrategicMap.Current.ClearTerrain(walk.X, walk.Y, L, 0, -1);
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }

                bool locked = troop.Owner is not HeroList;
                owner = locked ? troop.OwnerUnits.Holder : null;
                if (locked)
                {
                    owner.BeginUpdate();
                }

                structure = (Structure)resource.PlaceResourceUser(StrategicMap.Current, X, Y, L);
                if (structure != null)
                {
                    control.GetInfo(troop, out text, out gold, out days);
                    structure.ExecuteBuild(troop, days);
                    var players = StrategicMap.Current.Players.GetPlayers(troop.Player);
                    StrategicMap.Current.Players.GetPlayers(troop.Player).SetGems(players.Gems - gold);
                    troop.Killed();
                    result = ExecuteResult.Done;
                }

                if (locked)
                {
                    owner.EndUpdate();
                }
            }
        }

        return result;
    }

    // NetworkE.pas:230

}
