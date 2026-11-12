// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// inherited FChildren
public partial class MoveSettingsList : ENode, IChildList<MoveSettings>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a MoveSettings, so the cast never fails.
    public MoveSettings this[int index] => (MoveSettings)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is MoveSettings;

    int IChildSink<MoveSettings>.AddChild(MoveSettings child) => AddChild(child);

    public MoveSettingsList(ECustomNode owner) : base(owner)
    { }

    // Structur.PAS:4040
    // inherited FChildren
    public MoveSettings GetMoveSettings(int index) => this[index];

    // Structur.PAS:4045
    public int GetNoneValidMove()
    {
        int minPriority;
        int priority;
        int result = -1;
        minPriority = 16777215;
        for (int I = GetCount() - 1; I >= 0; I--)
        {
            var moveSettings = this[I];
            if (!moveSettings.Valid)
            {
                priority = moveSettings.GetMovePrio();
                if (priority <= minPriority)
                {
                    result = I;
                    minPriority = priority;
                }
            }
        }

        return result;
    }

    // Structur.PAS:4064
    public int IndexOfMoveObject(object moveObject)
    {
        int result = GetCount();
        while (result != 0)
        {
            result -= 1;
            if (moveObject == this[result].MoveObject)
            {
                return result;
            }
        }

        return -1;
    }

    // Structur.PAS:4075
    public void CalculateMovePositions(ValidateMovePositionProc validate)
    {
        int index;
        MoveSettings settings;
        for (int I = GetCount() - 1; I >= 0; I--)
        {
            settings = GetMoveSettings(I);
            settings.MovePosition = settings.Path.MovePointsToPosition(settings.MovePoints);
            settings.Valid = settings.Path.Count == 0 || settings.MovePosition == settings.Path.Count - 1;
        }

        index = GetNoneValidMove();
        while (!(index == -1))
        {
            if (validate(this, index))
            {
                GetMoveSettings(index).Valid = true;
                for (int I = 0, last = GetCount() - 1; I <= last; I++)
                {
                    if (index != I)
                    {
                        var moveLocation = GetMoveSettings(index).GetMoveLocation();
                        var moveLocation2 = GetMoveSettings(I).GetMoveLocation();
                        if (Unsafe.BitCast<MapCoordinate, int>(moveLocation) == Unsafe.BitCast<MapCoordinate, int>(moveLocation2))
                        {
                            if (GetMoveSettings(index).GetMovePrio() < GetMoveSettings(I).GetMovePrio())
                            {
                                GetMoveSettings(I).Valid = false;
                            }
                        }
                    }
                }
            }
            else
            {
                var moveSettings = GetMoveSettings(index);
                moveSettings.MovePosition += 1;
                var moveSettings2 = GetMoveSettings(index);
                var moveSettings3 = GetMoveSettings(index);
                GetMoveSettings(index).Valid = moveSettings2.MovePosition == moveSettings3.Path.Count - 1;
            }

            index = GetNoneValidMove();
        }
    }

    public delegate bool ValidateMovePositionProc(MoveSettingsList Moves, int Index);

    // Engine.pas:631

}
