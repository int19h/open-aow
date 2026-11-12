// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// inherited move control, executer and state
public partial class DefaultMoveExecutorControl : AIMoveExecutorControl
{
    public DefaultMoveExecutorControl(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:4978
    public override void CalculateMoves()
    {
        int I;
        AIMoveExecutorArmyList armies;
        AIMoveExecutorMove move;
        byte moveResult = default;
        Set<CreateMoveModeElement> mode;
        bool cannotMove;
        switch (State)
        {
            case 0:
                armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
                if (armies.Count != 0)
                {
                    if (Executor.Mode.Contains(AIMoveExecutorModeElement.Attack))
                    {
                        mode = Set<CreateMoveModeElement>.Of(CreateMoveModeElement.Surround);
                    }
                    else
                    {
                        mode = Set<CreateMoveModeElement>.Of();
                    }

                    move = CreateMove(armies.GetArmies(0), 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, mode, ref moveResult);
                    if (move != null)
                    {
                        if (!Executor.Mode.Contains(AIMoveExecutorModeElement.Simultaneous) || (Executor.Mode.Contains(AIMoveExecutorModeElement.Attack) && moveResult == 2))
                        {
                            Executor.ExecuteMoves();
                        }
                    }
                    else
                    {
                        armies.GetArmies(0).SetStatus(255, AIMoveExecutorUnitStatus.Done);
                    }
                }
                else
                {
                    if (Executor.Moves.Count != 0)
                    {
                        Executor.ExecuteMoves();
                        if (Executor.Mode.Contains(AIMoveExecutorModeElement.Attack))
                        {
                            State = 1;
                        }
                        else
                        {
                            State = 2;
                        }
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
                I = 0;
                cannotMove = true;
                armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
                while (cannotMove && I != armies.Count)
                {
                    move = CreateMove(armies.GetArmies(I), 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DoNotQueue, CreateMoveModeElement.Surround), ref moveResult);
                    cannotMove = moveResult != 0;
                    if (move != null)
                    {
                        move?.Dispose();
                    }

                    I += 1;
                }

                if (cannotMove)
                {
                    I = 0;
                    move = null;
                    while (move == null && I != armies.Count)
                    {
                        move = CreateMove(armies.GetArmies(I), 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, Set<CreateMoveModeElement>.Of(), ref moveResult);
                        if (move != null)
                        {
                            Executor.ExecuteMoves();
                        }

                        I += 1;
                    }
                }

                armies?.Dispose();
                Done();
                break;
            case 2:
                Done();
                break;
        }
    }
}
