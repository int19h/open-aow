#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Commands;

// Gold, Offer, X/Y/L, ArmyId, Player, OwnerPlayer, ArmyHexSprite, Object, MovePoints, Selection, Mode,
// Result, EventLogID
public class NegotiateTokenEvent(CustomNode? owner) : TokenEvent(owner)
{

    // Diplomacy.pas:133
    public new const int ClassId = 135_431;

    public int Gold { get; set; }
    public NegotiateOffer Offer { get; set; }
    public MapCoordinate Location { get; set; }
    public ArmyId? ArmyId { get; set; }
    public PlayerId? Player { get; set; } = PlayerId.Independent;

    private PlayerId? _ownerPlayer = PlayerId.Independent;

    public ArmyHexSprite? ArmyHexSprite { get; private set; }

    public NegotiateObject Object { get; set; }
    public int MovePoints { get; set; }

    private int _selection;

    public NegotiateModes Mode { get; set; } = NegotiateModes.AttackOnReject | NegotiateModes.MoveOnAccept;
    public NegotiateResult Result { get; set; }
    public int EventLogId { get; set; } = -1;

    // Diplomacy.pas:124
    public override void Dispose()
    {
        ArmyId? id = ArmyId;
        id?.Dispose();

        base.Dispose();
    }

    // Diplomacy.pas:138
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Gold = persister.Int32("Gold", value: Gold);
        Offer = persister.Enum("Offer", value: Offer);
        Location = persister.Coordinate(name: "Location", value: Location, width: CoordinateWidth.Byte);
        Player = persister.OptionalPlayer(name: "Player", value: Player, width: IdWidth.UnsignedByte, fallback: 0);
        _ownerPlayer = persister.OptionalPlayer("OwnerPlayer", value: _ownerPlayer, width: IdWidth.Byte, fallback: 0);
        Object = persister.Enum("Object", value: Object);
        MovePoints = persister.Int32("MovePoints", value: MovePoints);
        // Delphi TNegotiateTE.FData1 and FData2 (ids 39 and 40) are always 0 (omitted) and never read.
        persister.Skip("Data1");
        persister.Skip("Data2");
        Mode = persister.Set("Mode", value: Mode);
        Result = persister.Enum("Result", value: Result);
        ArmyId = persister.Created("ArmyId", objectValue: ArmyId);
        EventLogId = persister.Int32("EventLogId", value: EventLogId, fallback: -1);
    }

    // Diplomacy.pas:247
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        MapFieldMessage message = default;
        if (Validate())
        {
            message.MessageId = 4367;
            MapFieldMessageParameters param = message.Param;
            param[0] = this;
            message.Param = param;
            message.Result = 0;
            StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).SendMapFieldMessage(ref message);
        }

        return ExecuteResult.Done;
    }

    // Diplomacy.pas:158
    public override bool Validate()
    {
        ArmyId? id = ArmyId;
        bool result;
        if (id is not null)
        {
            ArmyHexSprite? armyHexSprite = ArmyHexSprite;
            result = id.RetrieveArmy(ref armyHexSprite, ref _selection);
            ArmyHexSprite = armyHexSprite;
        }
        else
        {
            result = true;
        }

        return result && Location.X != -1 && (byte)PlayerIdExtensions.ToOriginal(StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).GetPlayer()) == (byte)PlayerIdExtensions.ToOriginal(_ownerPlayer);
    }

    // Diplomacy.pas:192
    public virtual void Accept()
    {
        if (StrategicMap.Current.Players.GetPlayers(Player).Gems >= Gold)
        {
            Player? players = StrategicMap.Current.Players.GetPlayers(Player);
            StrategicMap.Current.Players.GetPlayers(Player).SetGems(players.Gems - Gold);
            if (EventLogId != -1)
            {
                EventLog? log = StrategicMap.Current.Players.GetPlayers(Player).EventLogbook.FindId(EventLogId);
                if (log is not null)
                {
                    StrategicMap.Current.Players.GetPlayers(Player).EventLogbook.RemoveEvent(log);
                }
            }

            if (ArmyHexSprite is not null && Mode.Contains(NegotiateModes.MoveOnAccept))
            {
                MovePath path = [];
                _ = path.AddMapCoordinate(Location.X, Location.Y, Location.MapLevel);
                _ = path.AddMapCoordinate(y: ArmyHexSprite.HexY, x: ArmyHexSprite.HexX, mapLevel: ArmyHexSprite.HexLevel);
                path.List[0] = path.List[0] with { MoveCost = (sbyte)MovePoints };
                MoveArmyTokenEvent? move = ArmyHexSprite.CreateArmyMoveTokenEvent(ArmyHexSprite, _selection, path, action: MoveAction.Combine, mode: ArmyMoveModes.SelectEndLocation);
                _ = StrategicMap.Current.TokenManager.TokenExecutor.InsertExecuteEvent(move);
                // { Original bug: no nil check after CreateArmyMoveTE can return nil. }
                _ = move.Release();
                path?.Dispose();
            }
        }
    }

    // Diplomacy.pas:226
    public virtual void Reject()
    {
        if (Offer == NegotiateOffer.ForceOffer && ArmyHexSprite is not null && Mode.Contains(NegotiateModes.AttackOnReject))
        {
            MovePath path = [];
            _ = path.AddMapCoordinate(Location.X, Location.Y, Location.MapLevel);
            _ = path.AddMapCoordinate(y: ArmyHexSprite.HexY, x: ArmyHexSprite.HexX, mapLevel: ArmyHexSprite.HexLevel);
            path.List[0] = path.List[0] with { MoveCost = (sbyte)MovePoints };
            MoveArmyTokenEvent? move = ArmyHexSprite.CreateArmyMoveTokenEvent(ArmyHexSprite, _selection, path, action: MoveAction.Attack, mode: ArmyMoveModes.SelectEndLocation);
            _ = StrategicMap.Current.TokenManager.TokenExecutor.InsertExecuteEvent(move);
            // { Original bug: no nil check after CreateArmyMoveTE can return nil. }
            _ = move.Release();
            path?.Dispose();
        }
    }

    public enum NegotiateOffer : byte
    {
        OfferBribe = 0, ForceOffer = 1
    }

    public enum NegotiateResult : byte
    {
        Unknown = 0, Accept = 1, Reject = 2
    }
}
