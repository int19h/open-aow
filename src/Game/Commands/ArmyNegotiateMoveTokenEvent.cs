// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Commands;

public partial class ArmyNegotiateMoveTokenEvent : MoveArmyTokenEvent
{
    public ArmyNegotiateMoveTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 135430;

    // MoveT.PAS:2400
    public override bool Validate()
    {
        // { Matches: Assign inherited Validate to Result before the conjunction. }
        bool Result = base.Validate();
        return Result && ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(FTargetX, FTargetY, FTargetL)).GetMoveAction(FArmyHS.FArmy.FPlayer) == MoveAction.Negotiate;
    }

    // MoveT.PAS:2442
    public override bool Animating()
    {
        bool Result;
        if (FMoveIndex == 1)
        {
            if (FArmyHS.GetMoveStatus() != -1)
            {
                // { Process the negotiate or ordinary move animation. }
                // { Evaluate the map field directly. }
                if (GetMapF(FPath[0]).GetMoveAction(FArmyHS.FArmy.FPlayer) == MoveAction.Negotiate)
                {
                    Result = FArmyHS.GetMoveStatus() < 50;
                }
                else
                {
                    Result = FArmyHS.GetMoveStatus() < 100;
                }
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = base.Animating();
        }

        return Result;
    }

    // MoveT.PAS:2407
    public override bool CanMoveNext()
    {
        bool Result;
        if (FMoveIndex == 1)
        {
            // { Evaluate the map field directly. }
            Result = GetMapF(FPath[0]).GetMoveAction(FArmyHS.FArmy.FPlayer) == MoveAction.Negotiate || base.CanMoveNext();
        }
        else
        {
            // { Otherwise retain the successful negotiation test. }
            Result = base.CanMoveNext();
        }

        return Result;
    }

    // MoveT.PAS:2419
    public override MoveStatus ExecuteMove()
    {
        MoveStatus Result;
        AoWMapField Field;
        int Direction;
        if (FMoveIndex == 1)
        {
            Field = GetMapF(FPath[0]);
            if (Field.GetMoveAction(FArmyHS.FArmy.FPlayer) == MoveAction.Negotiate)
            {
                Direction = GetDirection(1);
                if (Direction == 0)
                {
                    Direction = GetHT(1);
                }

                if (Direction > 0 && Direction <= 6)
                {
                    FArmyHS.SetDirection((byte)Direction);
                }
                else
                {
                    FArmyHS.SetDirection(0);
                }

                var item = FPath[0];
                var fPathItem = FPath[0];
                var item2 = FPath[0];
                var item3 = FPath[0];
                NegotiateArmy(FArmyHS.FArmy.FPlayer, FArmyHS, FSelection, item.X, fPathItem.Y, item2.L, item3.Flag);
                Result = MoveStatus.Done;
            }
            else
            {
                Result = base.ExecuteMove();
            }
        }
        else
        {
            Result = base.ExecuteMove();
        }

        return Result;
    }

    private static void NegotiateArmy(sbyte Player, ArmyHexSprite ArmyHS, byte Selection, sbyte X, sbyte Y, sbyte L, int MovePoints)
    {
        string ErrorText = "";
        MapFieldMessage message = default(MapFieldMessage);
        if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType == PlayerType.Human)
        {
            if (Player == AoWHSMap.Current.FSeatedPlayer)
            {
                NegotiateRequestEventLog tNegotiateRequestEventLog = new NegotiateRequestEventLog(null);
                tNegotiateRequestEventLog.FArmyID = ArmyHS.FArmy.CreateID(Selection);
                tNegotiateRequestEventLog.FMovePoints = MovePoints;
                tNegotiateRequestEventLog.FX = (sbyte)(byte)X;
                tNegotiateRequestEventLog.FY = (sbyte)(byte)Y;
                tNegotiateRequestEventLog.FL = (sbyte)(byte)L;
                tNegotiateRequestEventLog.FPlayer = (sbyte)(byte)Player;
                message.MsgID = 4366;
                message.Param[0] = new MsgArg((object)tNegotiateRequestEventLog);
                message.Param[1] = (MsgArg)(0);
                message.Result = (MsgArg)(0);
                AoWHSMap.Current.MapContainer.GetField(X, Y, L).SendMapFieldMsg(ref message);
                if (message.Param[1].Int != 0)
                {
                    tNegotiateRequestEventLog.Execute(mode: true, ExecuteTrigger.Event, null);
                }
                tNegotiateRequestEventLog.Release();
            }
        }
        else if (AoWHSMap.Current.FPlayers.GetPlayers(Player).GetLocal())
        {
            NegotiateRequestEventLog tNegotiateRequestEventLog = new NegotiateRequestEventLog(null);
            tNegotiateRequestEventLog.FArmyID = ArmyHS.FArmy.CreateID(Selection);
            tNegotiateRequestEventLog.FMovePoints = MovePoints;
            tNegotiateRequestEventLog.FX = (sbyte)(byte)X;
            tNegotiateRequestEventLog.FY = (sbyte)(byte)Y;
            tNegotiateRequestEventLog.FL = (sbyte)(byte)L;
            tNegotiateRequestEventLog.FPlayer = (sbyte)(byte)Player;
            message.MsgID = 4366;
            message.Param[0] = new MsgArg((object)tNegotiateRequestEventLog);
            message.Param[1] = (MsgArg)(0);
            message.Result = (MsgArg)(0);
            AoWHSMap.Current.MapContainer.GetField(X, Y, L).SendMapFieldMsg(ref message);
            if (message.Param[1].Int != 0 && AoWHSMap.Current.FPlayers.GetPlayers(Player).FGems >= tNegotiateRequestEventLog.FGold)
            {
                tNegotiateRequestEventLog.Accept(ref ErrorText);
            }
            else
            {
                tNegotiateRequestEventLog.Reject(ref ErrorText);
            }
            tNegotiateRequestEventLog.Release();
        }
    }

    // intf_P5_07.inc:816

    // intf_P5_07.inc:816

}
