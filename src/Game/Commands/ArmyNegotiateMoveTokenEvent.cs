#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Commands;

public class ArmyNegotiateMoveTokenEvent(CustomNode? owner) : MoveArmyTokenEvent(owner)
{
    public new const int ClassId = 135_430;

    // MoveT.PAS:2400
    public override bool Validate()
    {
        // { Matches: Assign inherited Validate to Result before the conjunction. }
        bool result = base.Validate();
        return result && StrategicMap.Current.MapContainer.GetField(Target.X, Target.Y, Target.MapLevel).GetMoveAction(ArmyHexSprite.Army.Player) == MoveAction.Negotiate;
    }

    // MoveT.PAS:2442
    public override bool Animating()
    {
        if (MoveIndex == 1)
        {
            if (ArmyHexSprite.GetMoveStatus() != -1)
            {
                // { Process the negotiate or ordinary move animation. }
                // { Evaluate the map field directly. }
                return GetMapField(Path[0].Location).GetMoveAction(ArmyHexSprite.Army.Player) == MoveAction.Negotiate
                    ? ArmyHexSprite.GetMoveStatus() < 50
                    : ArmyHexSprite.GetMoveStatus() < 100;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return base.Animating();
        }
    }

    // MoveT.PAS:2407
    public override bool CanMoveNext()
    {
        if (MoveIndex == 1)
        {
            // { Evaluate the map field directly. }
            return GetMapField(Path[0].Location).GetMoveAction(ArmyHexSprite.Army.Player) == MoveAction.Negotiate || base.CanMoveNext();
        }
        else
        {
            // { Otherwise retain the successful negotiation test. }
            return base.CanMoveNext();
        }
    }

    // MoveT.PAS:2419
    public override MoveStatus ExecuteMove()
    {
        if (MoveIndex == 1)
        {
            MapField field = GetMapField(Path[0].Location);
            if (field.GetMoveAction(ArmyHexSprite.Army.Player) == MoveAction.Negotiate)
            {
                int direction = GetDirection(1);
                if (direction == 0)
                {
                    direction = GetHexDirection(1);
                }

                if (direction is > 0 and <= 6)
                {
                    ArmyHexSprite.SetDirection((byte)direction);
                }
                else
                {
                    ArmyHexSprite.SetDirection(0);
                }

                PathStep item = Path[0];
                PathStep fPathItem = Path[0];
                PathStep item2 = Path[0];
                PathStep item3 = Path[0];
                NegotiateArmy(ArmyHexSprite.Army.Player, ArmyHexSprite, Selection, item.Location.X, fPathItem.Location.Y, L: item2.Location.MapLevel, movePoints: item3.MoveCost);
                return MoveStatus.Done;
            }
            else
            {
                return base.ExecuteMove();
            }
        }
        else
        {
            return base.ExecuteMove();
        }
    }

    private static void NegotiateArmy(sbyte player, ArmyHexSprite armyHexSprite, byte selection, sbyte X, sbyte Y, sbyte L, int movePoints)
    {
        MapFieldMessage message = default;
        if (StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
        {
            if (player == StrategicMap.Current.SeatedPlayer)
            {
                NegotiateRequestEventLog negotiateRequestEventLog = new(null)
                {
                    Holder = armyHexSprite,
                    ArmyId = armyHexSprite.Army.CreateId(selection),
                    MovePoints = movePoints,
                    Hex = new MapCoordinate(unchecked((sbyte)(byte)X), unchecked((sbyte)(byte)Y), unchecked((sbyte)(byte)L)),
                    Player = (sbyte)(byte)player
                };
                message.MessageId = 4366;
                MapFieldMessageParameters param = message.Param;
                param[0] = new MsgArg(negotiateRequestEventLog);
                message.Param = param;
                MapFieldMessageParameters param2 = message.Param;
                param2[1] = (MsgArg)0;
                message.Param = param2;
                message.Result = (MsgArg)0;
                StrategicMap.Current.MapContainer.GetField(X, Y, L).SendMapFieldMessage(ref message);
                if (message.Param[1].Int != 0)
                {
                    _ = negotiateRequestEventLog.Execute(mode: true, trigger: ExecuteTrigger.Event, done: null);
                }
                _ = negotiateRequestEventLog.Release();
            }
        }
        else if (StrategicMap.Current.Players.GetPlayers(player).GetLocal())
        {
            NegotiateRequestEventLog negotiateRequestEventLog = new(null)
            {
                Holder = armyHexSprite,
                ArmyId = armyHexSprite.Army.CreateId(selection),
                MovePoints = movePoints,
                Hex = new MapCoordinate(unchecked((sbyte)(byte)X), unchecked((sbyte)(byte)Y), unchecked((sbyte)(byte)L)),
                Player = (sbyte)(byte)player
            };
            message.MessageId = 4366;
            MapFieldMessageParameters param3 = message.Param;
            param3[0] = new MsgArg(negotiateRequestEventLog);
            message.Param = param3;
            MapFieldMessageParameters param4 = message.Param;
            param4[1] = (MsgArg)0;
            message.Param = param4;
            message.Result = (MsgArg)0;
            StrategicMap.Current.MapContainer.GetField(X, Y, L).SendMapFieldMessage(ref message);
            string errorText = "";
            _ = message.Param[1].Int != 0 && StrategicMap.Current.Players.GetPlayers(player).Gems >= negotiateRequestEventLog.Gold
                ? negotiateRequestEventLog.Accept(ref errorText)
                : negotiateRequestEventLog.Reject(ref errorText);
            _ = negotiateRequestEventLog.Release();
        }
    }
}
