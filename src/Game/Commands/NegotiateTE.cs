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

// Gold, Offer, X/Y/L, ArmyID, Player, OwnerPlayer, ArmyHS, Object, MovePoints, Selection, Mode,
// Result, EventLogID, Data1/Data2
public partial class NegotiateTE : TokenEvent
{
    // Diplomacy.pas:133
    public new const int ClassID = 135431;

    public int FGold;
    public NegotiateOffer FOffer;
    public byte FX;
    public byte FY;
    public byte FL;
    public ArmyID FArmyID;
    public sbyte FPlayer;
    public byte FOwnerPlayer;
    public ArmyHS FArmyHS;
    public NegotiateObject FObject;
    public int FMovePoints;
    public byte FSelection;
    public Set<NegotiateModeElement> FMode;
    public NegotiateResult FResult;
    public int FEventLogID;
    public int FData1;
    public int FData2;

    // Diplomacy.pas:124
    public override void Dispose()
    {
        ArmyID ID;
        ID = FArmyID;
        if (ID != null)
        {
            ID?.Dispose();
        }

        base.Dispose();
    }

    // Diplomacy.pas:138
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FGold, 30);
        Stream.rwData(Bytes.Of(ref FOffer, 1), 1, 31);
        Stream.rwByte(ref FX, 32);
        Stream.rwByte(ref FY, 33);
        Stream.rwByte(ref FL, 34);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 35);
        Stream.rwByte(ref FOwnerPlayer, 36);
        Stream.rwData(Bytes.Of(ref FObject, 1), 1, 37);
        Stream.rwInteger(ref FMovePoints, 38);
        Stream.rwInteger(ref FData1, 39);
        Stream.rwInteger(ref FData2, 40);
        RwSet(ref FMode, 1, 41, Stream.rwData, 0);
        Stream.rwData(Bytes.Of(ref FResult, 1), 1, 42);
        Stream.rwCreateEObject(ref FArmyID, null, 43);
        Stream.rwIntegerEx(ref FEventLogID, -1, 44);
    }

    // Diplomacy.pas:115
    public NegotiateTE(ECustomNode owner) : base(owner)
    {
        FArmyID = null;
        FMode = Set<NegotiateModeElement>.Of(NegotiateModeElement.AttackOnReject, NegotiateModeElement.MoveOnAccept);
        FEventLogID = -1;
        FOwnerPlayer = 0;
    }

    // Diplomacy.pas:247
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        MapFieldMsg Msg = default;
        if (Validate())
        {
            Msg.MsgID = 4367;
            Msg.Param[0] = this;
            Msg.Result = 0;
            AoWHSMap.Current.MapContainer.GetField((sbyte)FX, (sbyte)FY, (sbyte)FL).SendMapFieldMsg(ref Msg);
        }

        return ExecuteResult.Done;
    }

    // Diplomacy.pas:158
    public virtual bool Validate()
    {
        bool Result;
        ArmyID ID;
        ID = FArmyID;
        if (ID != null)
        {
            Result = ID.RetrieveArmy(ref FArmyHS, ref FSelection);
        }
        else
        {
            Result = true;
        }

        if (Result && FX != 255)
        {
            Result = (byte)((AoWMapField)AoWHSMap.Current.MapContainer.GetField((sbyte)FX, (sbyte)FY, (sbyte)FL)).GetPlayer() == FOwnerPlayer;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Diplomacy.pas:192
    public virtual void Accept()
    {
        EventLog Log;
        Movepath Path;
        MoveArmyTE Move;
        if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FGems >= FGold)
        {
            var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).SetGems(players.FGems - FGold);
            if (FEventLogID != -1)
            {
                Log = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FEventLogbook.FindID(FEventLogID);
                if (Log != null)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FEventLogbook.RemoveEvent(Log);
                }
            }

            if (FArmyHS != null && FMode.Contains(NegotiateModeElement.MoveOnAccept))
            {
                Path = new Movepath();
                Path.AddXYL((sbyte)FX, (sbyte)FY, (sbyte)FL);
                var yhx = FArmyHS.Yhx;
                Path.AddXYL(FArmyHS.Xhx, yhx, FArmyHS.Lhx);
                Path.List[0].Flag = (sbyte)FMovePoints;
                Move = ArmyHS.CreateArmyMoveTE(FArmyHS, FSelection, Path, (MoveAction)(byte)3, Set<ArmyMoveModeElement>.Of(ArmyMoveModeElement.SelectEndLocation));
                AoWHSMap.Current.FTokenManager.TokenExecuter.InsertExecuteEvent(Move);
                // { Original bug: no nil check after CreateArmyMoveTE can return nil. }
                Move.Release();
                Path?.Dispose();
            }
        }
    }

    // Diplomacy.pas:226
    public virtual void Reject()
    {
        Movepath Path;
        MoveArmyTE Move;
        if (FOffer == NegotiateOffer.ForceOffer && FArmyHS != null && FMode.Contains(NegotiateModeElement.AttackOnReject))
        {
            Path = new Movepath();
            Path.AddXYL((sbyte)FX, (sbyte)FY, (sbyte)FL);
            var yhx = FArmyHS.Yhx;
            Path.AddXYL(FArmyHS.Xhx, yhx, FArmyHS.Lhx);
            Path.List[0].Flag = (sbyte)FMovePoints;
            Move = ArmyHS.CreateArmyMoveTE(FArmyHS, FSelection, Path, (MoveAction)(byte)1, Set<ArmyMoveModeElement>.Of(ArmyMoveModeElement.SelectEndLocation));
            AoWHSMap.Current.FTokenManager.TokenExecuter.InsertExecuteEvent(Move);
            // { Original bug: no nil check after CreateArmyMoveTE can return nil. }
            Move.Release();
            Path?.Dispose();
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
