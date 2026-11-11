// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// inherited FChildren
public partial class MoveSettingsList : ENode
{
    public MoveSettingsList(ECustomNode owner) : base(owner)
    { }

    // Structur.PAS:4040
    // inherited FChildren
    public MoveSettings GetMoveSettings(int Index) => (MoveSettings)FChildren.List[Index];

    // Structur.PAS:4045
    public int GetNoneValidMove()
    {
        int MinPriority;
        int Priority;
        int Result = -1;
        MinPriority = 16777215;
        for (int I = GetCount() - 1; I >= 0; I--)
        {
            var moveSettings = (MoveSettings)FChildren.List[I];
            if (!moveSettings.FValid)
            {
                Priority = moveSettings.GetMovePrio();
                if (Priority <= MinPriority)
                {
                    Result = I;
                    MinPriority = Priority;
                }
            }
        }

        return Result;
    }

    // Structur.PAS:4064
    public int IndexOfMoveObject(object MoveObject)
    {
        int Result = GetCount();
        while (Result != 0)
        {
            Result -= 1;
            if (MoveObject == ((MoveSettings)FChildren.List[Result]).FMoveObject)
            {
                return Result;
            }
        }

        return -1;
    }

    // Structur.PAS:4075
    public void CalculateMovePositions(ValidateMovePositionProc Validate)
    {
        int Index;
        MoveSettings Settings;
        for (int I = GetCount() - 1; I >= 0; I--)
        {
            Settings = GetMoveSettings(I);
            Settings.FMovePos = Settings.FPath.MovePointsToPos(Settings.FMovePoints);
            Settings.FValid = Settings.FPath.Count == 0 || Settings.FMovePos == Settings.FPath.Count - 1;
        }

        Index = GetNoneValidMove();
        while (!(Index == -1))
        {
            if (Validate(this, Index))
            {
                GetMoveSettings(Index).FValid = true;
                for (int I = 0, last = GetCount() - 1; I <= last; I++)
                {
                    if (Index != I)
                    {
                        var moveLoc = GetMoveSettings(Index).GetMoveLoc();
                        var moveLoc2 = GetMoveSettings(I).GetMoveLoc();
                        if (Unsafe.BitCast<MapCoordinate, int>(moveLoc) == Unsafe.BitCast<MapCoordinate, int>(moveLoc2))
                        {
                            if (GetMoveSettings(Index).GetMovePrio() < GetMoveSettings(I).GetMovePrio())
                            {
                                GetMoveSettings(I).FValid = false;
                            }
                        }
                    }
                }
            }
            else
            {
                var moveSettings = GetMoveSettings(Index);
                moveSettings.FMovePos += 1;
                var moveSettings2 = GetMoveSettings(Index);
                var moveSettings3 = GetMoveSettings(Index);
                GetMoveSettings(Index).FValid = moveSettings2.FMovePos == moveSettings3.FPath.Count - 1;
            }

            Index = GetNoneValidMove();
        }
    }

    public delegate bool ValidateMovePositionProc(MoveSettingsList Moves, int Index);

    // Engine.pas:631

}
