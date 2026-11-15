// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// selection rectangle, events
public partial class GameMapMouse : HexSpriteMapMouse
{
    public GameMapMouse() : base()
    {
        ResetRectangleSelection();
        RectangleSelectEventsStore = new();
    }

    public Rect SelectionRectangle;

    internal EventStore<GameMapMouse> RectangleSelectEventsStore { get; }

    public event NotifyEvent<GameMapMouse> RectangleSelected
    {
        add => RectangleSelectEventsStore.Add(value);
        remove => RectangleSelectEventsStore.Remove(value);
    }

    // AoWHSMap.PAS:1057
    public override void Dispose()
    {
        base.Dispose();
        RectangleSelectEventsStore.Clear();
    }

    // AoWHSMap.PAS:1123
    public override void Down(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.Down(button, shift, X, Y);
        if (Capture == null && !HexSpriteMap.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            StartRectangleSelection(HexSpriteMap.ViewPixelX + X - 2, HexSpriteMap.ViewPixelY + Y - 3);
        }
    }

    // AoWHSMap.PAS:1130
    public override void Up(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.Up(button, shift, X, Y);
        if (Capture == null && SelectionRectangle.Left != -1)
        {
            if (X != -1 && Y != -1)
            {
                StopRectangleSelection(HexSpriteMap.ViewPixelX + X - 2, HexSpriteMap.ViewPixelY + Y - 3);
            }
            else
            {
                ResetRectangleSelection();
            }
        }
    }

    // AoWHSMap.PAS:1141
    public override void Move(Set<ShiftState> shift, int X, int Y)
    {
        base.Move(shift, X, Y);
        if (Capture == null)
        {
            UpdateRectangleSelection(HexSpriteMap.ViewPixelX + X - 2, HexSpriteMap.ViewPixelY + Y - 3);
        }
    }

    // AoWHSMap.PAS:1063
    public void ResetRectangleSelection()
    {
        SelectionRectangle.Left = -1;
        SelectionRectangle.Top = -1;
        SelectionRectangle.Right = -1;
        SelectionRectangle.Bottom = -1;
    }

    // AoWHSMap.PAS:1071
    public bool GetSelectingRectangle()
    {
        bool result;
        if (SelectionRectangle.Left == -1 || (SelectionRectangle.Left == SelectionRectangle.Right && SelectionRectangle.Top == SelectionRectangle.Bottom))
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // AoWHSMap.PAS:1079
    public void StartRectangleSelection(int pixelX, int pixelY)
    {
        SelectionRectangle.Left = pixelX;
        SelectionRectangle.Top = pixelY;
        SelectionRectangle.Right = pixelX;
        SelectionRectangle.Bottom = pixelY;
    }

    // AoWHSMap.PAS:1087
    public void UpdateRectangleSelection(int pixelX, int pixelY)
    {
        if (Button == MouseButton.Left)
        {
            if (pixelX > 0 && pixelY > 0)
            {
                if (pixelX < SelectionRectangle.Left - 800)
                {
                    pixelX = SelectionRectangle.Left - 800;
                }

                if (pixelX > SelectionRectangle.Left + 800)
                {
                    pixelX = SelectionRectangle.Left + 800;
                }

                if (pixelY < SelectionRectangle.Top - 600)
                {
                    pixelY = SelectionRectangle.Top - 600;
                }

                if (pixelY > SelectionRectangle.Top + 600)
                {
                    pixelY = SelectionRectangle.Top + 600;
                }

                SelectionRectangle.Right = pixelX;
                SelectionRectangle.Bottom = pixelY;
            }
        }
        else
        {
            ResetRectangleSelection();
        }
    }

    // AoWHSMap.PAS:1104
    public void StopRectangleSelection(int pixelX, int pixelY)
    {
        Rect r = default;
        SelectionRectangle.Right = pixelX;
        SelectionRectangle.Bottom = pixelY;
        if (SelectionRectangle.Left < SelectionRectangle.Right)
        {
            r.Left = SelectionRectangle.Left;
        }
        else
        {
            r.Left = SelectionRectangle.Right;
        }

        if (SelectionRectangle.Right > SelectionRectangle.Left)
        {
            r.Right = SelectionRectangle.Right;
        }
        else
        {
            r.Right = SelectionRectangle.Left;
        }

        if (SelectionRectangle.Top < SelectionRectangle.Bottom)
        {
            r.Top = SelectionRectangle.Top;
        }
        else
        {
            r.Top = SelectionRectangle.Bottom;
        }

        if (SelectionRectangle.Bottom > SelectionRectangle.Top)
        {
            r.Bottom = SelectionRectangle.Bottom;
        }
        else
        {
            r.Bottom = SelectionRectangle.Top;
        }

        SelectionRectangle = r;
        RectangleSelectEventsStore.Raise(this);
        ResetRectangleSelection();
    }

    // intf_P1_05.inc:68

}
