#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.World.Movement;

namespace Aow.Game.AI.Groups;

// inherited move control, executer and state
public class DefaultMoveExecutorControl(CustomNode owner) : AIMoveExecutorControl(owner)
{

    // AIGroup.pas:4978
    public override void CalculateMoves()
    {
        AIMoveExecutorArmyList armies;
        AIMoveExecutorMove move;
        byte moveResult;
        switch (State)
        {
            case 0:
                armies = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready);
                if (armies.Count != 0)
                {
                    CreateMoveModes mode = Executor.Mode.Contains(AIMoveExecutorModes.Attack)
                        ? CreateMoveModes.Surround
                        : CreateMoveModes.None;

                    (move, moveResult) = CreateMove(armies.GetArmies(0), selection: 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, mode);
                    if (move is not null)
                    {
                        if (!Executor.Mode.Contains(AIMoveExecutorModes.Simultaneous) || (Executor.Mode.Contains(AIMoveExecutorModes.Attack) && moveResult == 2))
                        {
                            Executor.ExecuteMoves();
                        }
                    }
                    else
                    {
                        armies.GetArmies(0).SetStatus(255, status: AIMoveExecutorUnitStatus.Done);
                    }
                }
                else
                {
                    if (Executor.Moves.Count != 0)
                    {
                        Executor.ExecuteMoves();
                        State = Executor.Mode.Contains(AIMoveExecutorModes.Attack) ? 1 : 2;
                    }
                }

                if (Executor.Moves.Count == 0 && armies.Count == 0)
                {
                    if (Executor.Mode.Contains(AIMoveExecutorModes.Attack))
                    {
                        State = 1;
                    }
                    else
                    {
                        Done();
                    }
                }

                armies?.Dispose();
                break;
            case 1:
                int i = 0;
                bool cannotMove = true;
                armies = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready | AIMoveExecutorUnitStatuses.Done);
                while (cannotMove && i != armies.Count)
                {
                    (move, moveResult) = CreateMove(armies.GetArmies(i), selection: 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, mode: CreateMoveModes.DoNotQueue | CreateMoveModes.Surround);
                    cannotMove = moveResult != 0;
                    move?.Dispose();

                    i += 1;
                }

                if (cannotMove)
                {
                    i = 0;
                    move = null;
                    while (move is null && i != armies.Count)
                    {
                        (move, _) = CreateMove(armies.GetArmies(i), selection: 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, mode: CreateMoveModes.None);
                        if (move is not null)
                        {
                            Executor.ExecuteMoves();
                        }

                        i += 1;
                    }
                }

                armies?.Dispose();
                Done();
                break;
            case 2:
                Done();
                break;
            default:
                break;
        }
    }
}
