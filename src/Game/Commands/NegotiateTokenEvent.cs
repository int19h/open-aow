// Translated from AoWE.pas.
using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

// Gold, Offer, X/Y/L, ArmyId, Player, OwnerPlayer, ArmyHexSprite, Object, MovePoints, Selection, Mode,
// Result, EventLogID, Data1/Data2
public partial class NegotiateTokenEvent : TokenEvent
{
    // Diplomacy.pas:133
    public new const int ClassId = 135431;

    public int Gold { get; set; }
    public NegotiateOffer Offer { get; set; }
    public byte X { get; set; }
    public byte Y { get; set; }
    public byte MapLevel { get; set; }
    public ArmyId ArmyId { get; set; }
    public sbyte Player { get; set; }

    private byte _ownerPlayer;
    public ArmyHexSprite ArmyHexSprite;

    public NegotiateObject Object { get; set; }
    public int MovePoints { get; set; }

    private byte _selection;

    public Set<NegotiateModeElement> Mode { get; set; }
    public NegotiateResult Result { get; set; }
    public int EventLogId { get; set; }
    public int Data1 { get; set; }
    public int Data2 { get; set; }

    // Diplomacy.pas:124
    public override void Dispose()
    {
        ArmyId id;
        id = ArmyId;
        if (id != null)
        {
            id?.Dispose();
        }

        base.Dispose();
    }

    // Diplomacy.pas:138
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Gold = persister.Int32("Gold", value: Gold);
        Offer = persister.Enum("Offer", value: Offer);
        X = persister.Byte("X", value: X);
        Y = persister.Byte("Y", value: Y);
        MapLevel = persister.Byte("MapLevel", value: MapLevel);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        _ownerPlayer = persister.Byte("OwnerPlayer", value: _ownerPlayer);
        Object = persister.Enum("Object", value: Object);
        MovePoints = persister.Int32("MovePoints", value: MovePoints);
        Data1 = persister.Int32("Data1", value: Data1);
        Data2 = persister.Int32("Data2", value: Data2);
        Mode = persister.Set("Mode", value: Mode);
        Result = persister.Enum("Result", value: Result);
        ArmyId = persister.Created("ArmyId", objectValue: ArmyId);
        EventLogId = persister.Int32("EventLogId", value: EventLogId, fallback: -1);
    }

    // Diplomacy.pas:115
    public NegotiateTokenEvent(CustomNode owner) : base(owner)
    {
        ArmyId = null;
        Mode = Set<NegotiateModeElement>.Of(NegotiateModeElement.AttackOnReject, NegotiateModeElement.MoveOnAccept);
        EventLogId = -1;
        _ownerPlayer = 0;
    }

    // Diplomacy.pas:247
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        MapFieldMessage message = default;
        if (Validate())
        {
            message.MessageId = 4367;
            message.Param[0] = this;
            message.Result = 0;
            StrategicMap.Current.MapContainer.GetField((sbyte)X, (sbyte)Y, (sbyte)MapLevel).SendMapFieldMessage(ref message);
        }

        return ExecuteResult.Done;
    }

    // Diplomacy.pas:158
    public virtual bool Validate()
    {
        bool result;
        ArmyId id;
        id = ArmyId;
        if (id != null)
        {
            result = id.RetrieveArmy(ref ArmyHexSprite, ref _selection);
        }
        else
        {
            result = true;
        }

        if (result && X != 255)
        {
            result = (byte)StrategicMap.Current.MapContainer.GetField((sbyte)X, (sbyte)Y, (sbyte)MapLevel).GetPlayer() == _ownerPlayer;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Diplomacy.pas:192
    public virtual void Accept()
    {
        EventLog log;
        MovePath path;
        MoveArmyTokenEvent move;
        if (StrategicMap.Current.Players.GetPlayers(Player).Gems >= Gold)
        {
            var players = StrategicMap.Current.Players.GetPlayers(Player);
            StrategicMap.Current.Players.GetPlayers(Player).SetGems(players.Gems - Gold);
            if (EventLogId != -1)
            {
                log = StrategicMap.Current.Players.GetPlayers(Player).EventLogbook.FindId(EventLogId);
                if (log != null)
                {
                    StrategicMap.Current.Players.GetPlayers(Player).EventLogbook.RemoveEvent(log);
                }
            }

            if (ArmyHexSprite != null && Mode.Contains(NegotiateModeElement.MoveOnAccept))
            {
                path = new MovePath();
                path.AddMapCoordinate((sbyte)X, (sbyte)Y, (sbyte)MapLevel);
                var hexY = ArmyHexSprite.HexY;
                path.AddMapCoordinate(ArmyHexSprite.HexX, hexY, ArmyHexSprite.HexLevel);
                path.List[0] = path.List[0] with { MoveCost = (sbyte)MovePoints };
                move = ArmyHexSprite.CreateArmyMoveTokenEvent(ArmyHexSprite, _selection, path, (MoveAction)(byte)3, Set<ArmyMoveModeElement>.Of(ArmyMoveModeElement.SelectEndLocation));
                StrategicMap.Current.TokenManager.TokenExecutor.InsertExecuteEvent(move);
                // { Original bug: no nil check after CreateArmyMoveTE can return nil. }
                move.Release();
                path?.Dispose();
            }
        }
    }

    // Diplomacy.pas:226
    public virtual void Reject()
    {
        MovePath path;
        MoveArmyTokenEvent move;
        if (Offer == NegotiateOffer.ForceOffer && ArmyHexSprite != null && Mode.Contains(NegotiateModeElement.AttackOnReject))
        {
            path = new MovePath();
            path.AddMapCoordinate((sbyte)X, (sbyte)Y, (sbyte)MapLevel);
            var hexY = ArmyHexSprite.HexY;
            path.AddMapCoordinate(ArmyHexSprite.HexX, hexY, ArmyHexSprite.HexLevel);
            path.List[0] = path.List[0] with { MoveCost = (sbyte)MovePoints };
            move = ArmyHexSprite.CreateArmyMoveTokenEvent(ArmyHexSprite, _selection, path, (MoveAction)(byte)1, Set<ArmyMoveModeElement>.Of(ArmyMoveModeElement.SelectEndLocation));
            StrategicMap.Current.TokenManager.TokenExecutor.InsertExecuteEvent(move);
            // { Original bug: no nil check after CreateArmyMoveTE can return nil. }
            move.Release();
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
