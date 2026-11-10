// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Ai.Groups;

// inherited move control, executer and state
public partial class DefaultMEC : AIMoveExecuterControl
{
    public DefaultMEC(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:4978
    public override void CalculateMoves()
    {
        int I;
        AIMoveExecuterArmyList Armies;
        AIMoveExecuterMove Move;
        byte MoveResult = default;
        Set<CreateMoveModeElement> Mode;
        bool CannotMove;
        switch (FState)
        {
            case 0:
                Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready));
                if (Armies.Count != 0)
                {
                    if (FExecuter.FMode.Contains(AIMoveExecuterModeElement.Attack))
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
                        if (!FExecuter.FMode.Contains(AIMoveExecuterModeElement.Simultaneous) || (FExecuter.FMode.Contains(AIMoveExecuterModeElement.Attack) && MoveResult == 2))
                        {
                            FExecuter.ExecuteMoves();
                        }
                    }
                    else
                    {
                        Armies.GetArmies(0).SetStatus(255, AIMoveExecuterUnitStatus.Done);
                    }
                }
                else
                {
                    if (FExecuter.FMoves.Count != 0)
                    {
                        FExecuter.ExecuteMoves();
                        if (FExecuter.FMode.Contains(AIMoveExecuterModeElement.Attack))
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
                    if (FExecuter.FMode.Contains(AIMoveExecuterModeElement.Attack))
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
                Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready, AIMoveExecuterUnitStatus.Done));
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
