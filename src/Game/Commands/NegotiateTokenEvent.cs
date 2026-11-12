// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

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

    public int Gold;
    public NegotiateOffer Offer;
    public byte X;
    public byte Y;
    public byte MapLevel;
    public ArmyId ArmyId;
    public sbyte Player;
    public byte OwnerPlayer;
    public ArmyHexSprite ArmyHexSprite;
    public NegotiateObject Object;
    public int MovePoints;
    public byte Selection;
    public Set<NegotiateModeElement> Mode;
    public NegotiateResult Result;
    public int EventLogId;
    public int Data1;
    public int Data2;

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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref Gold, 30);
        stream.rwData(Bytes.Of(ref Offer, 1), 1, 31);
        stream.rwByte(ref X, 32);
        stream.rwByte(ref Y, 33);
        stream.rwByte(ref MapLevel, 34);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Player), 35);
        stream.rwByte(ref OwnerPlayer, 36);
        stream.rwData(Bytes.Of(ref Object, 1), 1, 37);
        stream.rwInteger(ref MovePoints, 38);
        stream.rwInteger(ref Data1, 39);
        stream.rwInteger(ref Data2, 40);
        RwSet(ref Mode, 1, 41, stream.rwData, 0);
        stream.rwData(Bytes.Of(ref Result, 1), 1, 42);
        stream.rwCreateEObject(ref ArmyId, null, 43);
        stream.rwIntegerEx(ref EventLogId, -1, 44);
    }

    // Diplomacy.pas:115
    public NegotiateTokenEvent(ECustomNode owner) : base(owner)
    {
        ArmyId = null;
        Mode = Set<NegotiateModeElement>.Of(NegotiateModeElement.AttackOnReject, NegotiateModeElement.MoveOnAccept);
        EventLogId = -1;
        OwnerPlayer = 0;
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
            AoWHSMap.Current.MapContainer.GetField((sbyte)X, (sbyte)Y, (sbyte)MapLevel).SendMapFieldMessage(ref message);
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
            result = id.RetrieveArmy(ref ArmyHexSprite, ref Selection);
        }
        else
        {
            result = true;
        }

        if (result && X != 255)
        {
            result = (byte)((AoWMapField)AoWHSMap.Current.MapContainer.GetField((sbyte)X, (sbyte)Y, (sbyte)MapLevel)).GetPlayer() == OwnerPlayer;
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
        if (AoWHSMap.Current.Players.GetPlayers(Player).Gems >= Gold)
        {
            var players = AoWHSMap.Current.Players.GetPlayers(Player);
            AoWHSMap.Current.Players.GetPlayers(Player).SetGems(players.Gems - Gold);
            if (EventLogId != -1)
            {
                log = AoWHSMap.Current.Players.GetPlayers(Player).EventLogbook.FindId(EventLogId);
                if (log != null)
                {
                    AoWHSMap.Current.Players.GetPlayers(Player).EventLogbook.RemoveEvent(log);
                }
            }

            if (ArmyHexSprite != null && Mode.Contains(NegotiateModeElement.MoveOnAccept))
            {
                path = new MovePath();
                path.AddMapCoordinate((sbyte)X, (sbyte)Y, (sbyte)MapLevel);
                var hexY = ArmyHexSprite.HexY;
                path.AddMapCoordinate(ArmyHexSprite.HexX, hexY, ArmyHexSprite.HexLevel);
                path.List[0].Tag = (sbyte)MovePoints;
                move = ArmyHexSprite.CreateArmyMoveTokenEvent(ArmyHexSprite, Selection, path, (MoveAction)(byte)3, Set<ArmyMoveModeElement>.Of(ArmyMoveModeElement.SelectEndLocation));
                AoWHSMap.Current.TokenManager.TokenExecutor.InsertExecuteEvent(move);
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
            path.List[0].Tag = (sbyte)MovePoints;
            move = ArmyHexSprite.CreateArmyMoveTokenEvent(ArmyHexSprite, Selection, path, (MoveAction)(byte)1, Set<ArmyMoveModeElement>.Of(ArmyMoveModeElement.SelectEndLocation));
            AoWHSMap.Current.TokenManager.TokenExecutor.InsertExecuteEvent(move);
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
