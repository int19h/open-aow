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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref ControlId, 20);
        stream.rwInteger(ref UnitId, 21);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Player), 22);
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
                troop.GetLocation(ref X, ref Y, ref L);
                level = StrategicMap.Current.MapContainer.GetMapLevel(L);
                HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
                while (walk.HexNumber < resource.TerrainList.Count)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && level.Width > walk.X && level.Height > walk.Y && resource.TerrainList.GetUsed(walk.HexNumber))
                    {
                        StrategicMap.Current.ClearTerrain(walk.X, walk.Y, L, 0, -1);
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }

                bool locked = troop.Owner is not HeroList;
                owner = locked ? troop.UnitListOwner : null;
                if (locked)
                {
                    owner.BeginUpdate();
                }

                structure = (Structure)resource.PlaceResourceUser(StrategicMap.Current, X, Y, L);
                if (structure != null)
                {
                    control.GetInfo(troop, ref text, ref gold, ref days);
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
