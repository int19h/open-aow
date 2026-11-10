// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// selection rectangle, events
public partial class AbstractAoWHSMapMouse : HSMapMouse
{
    public AbstractAoWHSMapMouse() : base()
    {
        ResetRectangleSelection();
        FRectangleSelectEventsStore = new();
    }

    public Rect FSelectionRectangle;

    internal EventStore<AbstractAoWHSMapMouse> FRectangleSelectEventsStore;

    public event NotifyEvent<AbstractAoWHSMapMouse> FRectangleSelectEvents
    {
        add => FRectangleSelectEventsStore.Add(value);
        remove => FRectangleSelectEventsStore.Remove(value);
    }

    // AoWHSMap.PAS:1057
    public override void Dispose()
    {
        base.Dispose();
        FRectangleSelectEventsStore.Clear();
    }

    // AoWHSMap.PAS:1123
    public override void Down(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        base.Down(Button, Shift, X, Y);
        if (Capture == null && !HSMap.Status.Contains(HSMapStatusElement.Editting))
        {
            StartRectangleSelection(HSMap.ViewXhp + X - 2, HSMap.ViewYhp + Y - 3);
        }
    }

    // AoWHSMap.PAS:1130
    public override void Up(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        base.Up(Button, Shift, X, Y);
        if (Capture == null && FSelectionRectangle.Left != -1)
        {
            if (X != -1 && Y != -1)
            {
                StopRectangleSelection(HSMap.ViewXhp + X - 2, HSMap.ViewYhp + Y - 3);
            }
            else
            {
                ResetRectangleSelection();
            }
        }
    }

    // AoWHSMap.PAS:1141
    public override void Move(Set<ShiftState> Shift, int X, int Y)
    {
        base.Move(Shift, X, Y);
        if (Capture == null)
        {
            UpdateRectangleSelection(HSMap.ViewXhp + X - 2, HSMap.ViewYhp + Y - 3);
        }
    }

    // AoWHSMap.PAS:1063
    public void ResetRectangleSelection()
    {
        FSelectionRectangle.Left = -1;
        FSelectionRectangle.Top = -1;
        FSelectionRectangle.Right = -1;
        FSelectionRectangle.Bottom = -1;
    }

    // AoWHSMap.PAS:1071
    public bool GetSelectingRectangle()
    {
        bool Result;
        if (FSelectionRectangle.Left == -1 || (FSelectionRectangle.Left == FSelectionRectangle.Right && FSelectionRectangle.Top == FSelectionRectangle.Bottom))
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // AoWHSMap.PAS:1079
    public void StartRectangleSelection(int Xhp, int Yhp)
    {
        FSelectionRectangle.Left = Xhp;
        FSelectionRectangle.Top = Yhp;
        FSelectionRectangle.Right = Xhp;
        FSelectionRectangle.Bottom = Yhp;
    }

    // AoWHSMap.PAS:1087
    public void UpdateRectangleSelection(int Xhp, int Yhp)
    {
        if (Button == MouseButton.Left)
        {
            if (Xhp > 0 && Yhp > 0)
            {
                if (Xhp < FSelectionRectangle.Left - 800)
                {
                    Xhp = FSelectionRectangle.Left - 800;
                }

                if (Xhp > FSelectionRectangle.Left + 800)
                {
                    Xhp = FSelectionRectangle.Left + 800;
                }

                if (Yhp < FSelectionRectangle.Top - 600)
                {
                    Yhp = FSelectionRectangle.Top - 600;
                }

                if (Yhp > FSelectionRectangle.Top + 600)
                {
                    Yhp = FSelectionRectangle.Top + 600;
                }

                FSelectionRectangle.Right = Xhp;
                FSelectionRectangle.Bottom = Yhp;
            }
        }
        else
        {
            ResetRectangleSelection();
        }
    }

    // AoWHSMap.PAS:1104
    public void StopRectangleSelection(int Xhp, int Yhp)
    {
        Rect R = default;
        FSelectionRectangle.Right = Xhp;
        FSelectionRectangle.Bottom = Yhp;
        if (FSelectionRectangle.Left < FSelectionRectangle.Right)
        {
            R.Left = FSelectionRectangle.Left;
        }
        else
        {
            R.Left = FSelectionRectangle.Right;
        }

        if (FSelectionRectangle.Right > FSelectionRectangle.Left)
        {
            R.Right = FSelectionRectangle.Right;
        }
        else
        {
            R.Right = FSelectionRectangle.Left;
        }

        if (FSelectionRectangle.Top < FSelectionRectangle.Bottom)
        {
            R.Top = FSelectionRectangle.Top;
        }
        else
        {
            R.Top = FSelectionRectangle.Bottom;
        }

        if (FSelectionRectangle.Bottom > FSelectionRectangle.Top)
        {
            R.Bottom = FSelectionRectangle.Bottom;
        }
        else
        {
            R.Bottom = FSelectionRectangle.Top;
        }

        FSelectionRectangle = R;
        FRectangleSelectEventsStore.Raise(this);
        ResetRectangleSelection();
    }

    // intf_P1_05.inc:68

}
