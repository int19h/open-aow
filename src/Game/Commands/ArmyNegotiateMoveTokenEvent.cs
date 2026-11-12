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

    public new const int ClassId = 135430;

    // MoveT.PAS:2400
    public override bool Validate()
    {
        // { Matches: Assign inherited Validate to Result before the conjunction. }
        bool result = base.Validate();
        return result && StrategicMap.Current.MapContainer.GetField(TargetX, TargetY, TargetMapLevel).GetMoveAction(ArmyHexSprite.Army.Player) == MoveAction.Negotiate;
    }

    // MoveT.PAS:2442
    public override bool Animating()
    {
        bool result;
        if (MoveIndex == 1)
        {
            if (ArmyHexSprite.GetMoveStatus() != -1)
            {
                // { Process the negotiate or ordinary move animation. }
                // { Evaluate the map field directly. }
                if (GetMapField(Path[0]).GetMoveAction(ArmyHexSprite.Army.Player) == MoveAction.Negotiate)
                {
                    result = ArmyHexSprite.GetMoveStatus() < 50;
                }
                else
                {
                    result = ArmyHexSprite.GetMoveStatus() < 100;
                }
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = base.Animating();
        }

        return result;
    }

    // MoveT.PAS:2407
    public override bool CanMoveNext()
    {
        bool result;
        if (MoveIndex == 1)
        {
            // { Evaluate the map field directly. }
            result = GetMapField(Path[0]).GetMoveAction(ArmyHexSprite.Army.Player) == MoveAction.Negotiate || base.CanMoveNext();
        }
        else
        {
            // { Otherwise retain the successful negotiation test. }
            result = base.CanMoveNext();
        }

        return result;
    }

    // MoveT.PAS:2419
    public override MoveStatus ExecuteMove()
    {
        MoveStatus result;
        MapField field;
        int direction;
        if (MoveIndex == 1)
        {
            field = GetMapField(Path[0]);
            if (field.GetMoveAction(ArmyHexSprite.Army.Player) == MoveAction.Negotiate)
            {
                direction = GetDirection(1);
                if (direction == 0)
                {
                    direction = GetHexDirection(1);
                }

                if (direction > 0 && direction <= 6)
                {
                    ArmyHexSprite.SetDirection((byte)direction);
                }
                else
                {
                    ArmyHexSprite.SetDirection(0);
                }

                var item = Path[0];
                var fPathItem = Path[0];
                var item2 = Path[0];
                var item3 = Path[0];
                NegotiateArmy(ArmyHexSprite.Army.Player, ArmyHexSprite, Selection, item.X, fPathItem.Y, item2.MapLevel, item3.Tag);
                result = MoveStatus.Done;
            }
            else
            {
                result = base.ExecuteMove();
            }
        }
        else
        {
            result = base.ExecuteMove();
        }

        return result;
    }

    private static void NegotiateArmy(sbyte player, ArmyHexSprite armyHexSprite, byte selection, sbyte X, sbyte Y, sbyte L, int movePoints)
    {
        string errorText = "";
        MapFieldMessage message = default(MapFieldMessage);
        if (StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
        {
            if (player == StrategicMap.Current.SeatedPlayer)
            {
                NegotiateRequestEventLog negotiateRequestEventLog = new NegotiateRequestEventLog(null);
                negotiateRequestEventLog.ArmyId = armyHexSprite.Army.CreateId(selection);
                negotiateRequestEventLog.MovePoints = movePoints;
                negotiateRequestEventLog.X = (sbyte)(byte)X;
                negotiateRequestEventLog.Y = (sbyte)(byte)Y;
                negotiateRequestEventLog.MapLevel = (sbyte)(byte)L;
                negotiateRequestEventLog._player = (sbyte)(byte)player;
                message.MessageId = 4366;
                message.Param[0] = new MsgArg((object)negotiateRequestEventLog);
                message.Param[1] = (MsgArg)(0);
                message.Result = (MsgArg)(0);
                StrategicMap.Current.MapContainer.GetField(X, Y, L).SendMapFieldMessage(ref message);
                if (message.Param[1].Int != 0)
                {
                    negotiateRequestEventLog.Execute(mode: true, ExecuteTrigger.Event, null);
                }
                negotiateRequestEventLog.Release();
            }
        }
        else if (StrategicMap.Current.Players.GetPlayers(player).GetLocal())
        {
            NegotiateRequestEventLog negotiateRequestEventLog = new NegotiateRequestEventLog(null);
            negotiateRequestEventLog.ArmyId = armyHexSprite.Army.CreateId(selection);
            negotiateRequestEventLog.MovePoints = movePoints;
            negotiateRequestEventLog.X = (sbyte)(byte)X;
            negotiateRequestEventLog.Y = (sbyte)(byte)Y;
            negotiateRequestEventLog.MapLevel = (sbyte)(byte)L;
            negotiateRequestEventLog._player = (sbyte)(byte)player;
            message.MessageId = 4366;
            message.Param[0] = new MsgArg((object)negotiateRequestEventLog);
            message.Param[1] = (MsgArg)(0);
            message.Result = (MsgArg)(0);
            StrategicMap.Current.MapContainer.GetField(X, Y, L).SendMapFieldMessage(ref message);
            if (message.Param[1].Int != 0 && StrategicMap.Current.Players.GetPlayers(player).Gems >= negotiateRequestEventLog.Gold)
            {
                negotiateRequestEventLog.Accept(ref errorText);
            }
            else
            {
                negotiateRequestEventLog.Reject(ref errorText);
            }
            negotiateRequestEventLog.Release();
        }
    }

    // intf_P5_07.inc:816

    // intf_P5_07.inc:816

}
