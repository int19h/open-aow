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
        AIMoveExecutorArmyList Armies;
        AIMoveExecutorMove Move;
        byte MoveResult = default;
        Set<CreateMoveModeElement> Mode;
        bool CannotMove;
        switch (FState)
        {
            case 0:
                Armies = FExecuter.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
                if (Armies.Count != 0)
                {
                    if (FExecuter.FMode.Contains(AIMoveExecutorModeElement.Attack))
                    {
                        Mode = Set<CreateMoveModeElement>.Of(CreateMoveModeElement.Surround);
                    }
                    else
                    {
                        Mode = Set<CreateMoveModeElement>.Of();
                    }

                    Move = CreateMove(Armies.GetArmies(0), 255, FExecuter.FDestination.X, FExecuter.FDestination.Y, FExecuter.FDestination.L, Mode, ref MoveResult);
                    if (Move != null)
                    {
                        if (!FExecuter.FMode.Contains(AIMoveExecutorModeElement.Simultaneous) || (FExecuter.FMode.Contains(AIMoveExecutorModeElement.Attack) && MoveResult == 2))
                        {
                            FExecuter.ExecuteMoves();
                        }
                    }
                    else
                    {
                        Armies.GetArmies(0).SetStatus(255, AIMoveExecutorUnitStatus.Done);
                    }
                }
                else
                {
                    if (FExecuter.FMoves.Count != 0)
                    {
                        FExecuter.ExecuteMoves();
                        if (FExecuter.FMode.Contains(AIMoveExecutorModeElement.Attack))
                        {
                            FState = 1;
                        }
                        else
                        {
                            FState = 2;
                        }
                    }
                }

                if (FExecuter.FMoves.Count == 0 && Armies.Count == 0)
                {
                    if (FExecuter.FMode.Contains(AIMoveExecutorModeElement.Attack))
                    {
                        FState = 1;
                    }
                    else
                    {
                        Done();
                    }
                }

                Armies?.Dispose();
                break;
            case 1:
                I = 0;
                CannotMove = true;
                Armies = FExecuter.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
                while (CannotMove && I != Armies.Count)
                {
                    Move = CreateMove(Armies.GetArmies(I), 255, FExecuter.FDestination.X, FExecuter.FDestination.Y, FExecuter.FDestination.L, Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DontQueue, CreateMoveModeElement.Surround), ref MoveResult);
                    CannotMove = MoveResult != 0;
                    if (Move != null)
                    {
                        Move?.Dispose();
                    }

                    I += 1;
                }

                if (CannotMove)
                {
                    I = 0;
                    Move = null;
                    while (Move == null && I != Armies.Count)
                    {
                        Move = CreateMove(Armies.GetArmies(I), 255, FExecuter.FDestination.X, FExecuter.FDestination.Y, FExecuter.FDestination.L, Set<CreateMoveModeElement>.Of(), ref MoveResult);
                        if (Move != null)
                        {
                            FExecuter.ExecuteMoves();
                        }

                        I += 1;
                    }
                }

                Armies?.Dispose();
                Done();
                break;
            case 2:
                Done();
                break;
        }
    }
}
