#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.World.Movement;
using Aow.Rtl;

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
                armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
                if (armies.Count != 0)
                {
                    Set<CreateMoveModeElement> mode = Executor.Mode.Contains(AIMoveExecutorModeElement.Attack)
                        ? Set<CreateMoveModeElement>.Of(CreateMoveModeElement.Surround)
                        : Set<CreateMoveModeElement>.Of();

                    (move, moveResult) = CreateMove(armies.GetArmies(0), selection: 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, mode);
                    if (move is not null)
                    {
                        if (!Executor.Mode.Contains(AIMoveExecutorModeElement.Simultaneous) || (Executor.Mode.Contains(AIMoveExecutorModeElement.Attack) && moveResult == 2))
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
                        State = Executor.Mode.Contains(AIMoveExecutorModeElement.Attack) ? 1 : 2;
                    }
                }

                if (Executor.Moves.Count == 0 && armies.Count == 0)
                {
                    if (Executor.Mode.Contains(AIMoveExecutorModeElement.Attack))
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
                armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
                while (cannotMove && i != armies.Count)
                {
                    (move, moveResult) = CreateMove(armies.GetArmies(i), selection: 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, mode: Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DoNotQueue, CreateMoveModeElement.Surround));
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
                        (move, _) = CreateMove(armies.GetArmies(i), selection: 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, mode: Set<CreateMoveModeElement>.Of());
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
