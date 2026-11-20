// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// selection rectangle, events
public class GameMapMouse : HexSpriteMapMouse
{
    public GameMapMouse() : base()
    {
        ResetRectangleSelection();
        RectangleSelectEventsStore = new();
    }

    public Rect SelectionRectangle { get; private set; }

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
        if (Capture is null && !HexSpriteMap.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            StartRectangleSelection(HexSpriteMap.ViewPixelX + X - 2, HexSpriteMap.ViewPixelY + Y - 3);
        }
    }

    // AoWHSMap.PAS:1130
    public override void Up(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.Up(button, shift, X, Y);
        if (Capture is null && SelectionRectangle.Left != -1)
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
        if (Capture is null)
        {
            UpdateRectangleSelection(HexSpriteMap.ViewPixelX + X - 2, HexSpriteMap.ViewPixelY + Y - 3);
        }
    }

    // AoWHSMap.PAS:1063
    public void ResetRectangleSelection()
    {
        Rect selectionRectangle = SelectionRectangle;
        selectionRectangle.Left = -1;
        SelectionRectangle = selectionRectangle;
        Rect selectionRectangle2 = SelectionRectangle;
        selectionRectangle2.Top = -1;
        SelectionRectangle = selectionRectangle2;
        Rect selectionRectangle3 = SelectionRectangle;
        selectionRectangle3.Right = -1;
        SelectionRectangle = selectionRectangle3;
        Rect selectionRectangle4 = SelectionRectangle;
        selectionRectangle4.Bottom = -1;
        SelectionRectangle = selectionRectangle4;
    }

    // AoWHSMap.PAS:1071
    public bool SelectingRectangle => SelectionRectangle.Left != -1 && (SelectionRectangle.Left != SelectionRectangle.Right || SelectionRectangle.Top != SelectionRectangle.Bottom);

    // AoWHSMap.PAS:1079
    public void StartRectangleSelection(int pixelX, int pixelY)
    {
        Rect selectionRectangle = SelectionRectangle;
        selectionRectangle.Left = pixelX;
        SelectionRectangle = selectionRectangle;
        Rect selectionRectangle2 = SelectionRectangle;
        selectionRectangle2.Top = pixelY;
        SelectionRectangle = selectionRectangle2;
        Rect selectionRectangle3 = SelectionRectangle;
        selectionRectangle3.Right = pixelX;
        SelectionRectangle = selectionRectangle3;
        Rect selectionRectangle4 = SelectionRectangle;
        selectionRectangle4.Bottom = pixelY;
        SelectionRectangle = selectionRectangle4;
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

                Rect selectionRectangle = SelectionRectangle;
                selectionRectangle.Right = pixelX;
                SelectionRectangle = selectionRectangle;
                Rect selectionRectangle2 = SelectionRectangle;
                selectionRectangle2.Bottom = pixelY;
                SelectionRectangle = selectionRectangle2;
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
        Rect selectionRectangle = SelectionRectangle;
        selectionRectangle.Right = pixelX;
        SelectionRectangle = selectionRectangle;
        Rect selectionRectangle2 = SelectionRectangle;
        selectionRectangle2.Bottom = pixelY;
        SelectionRectangle = selectionRectangle2;
        r.Left = SelectionRectangle.Left < SelectionRectangle.Right ? SelectionRectangle.Left : SelectionRectangle.Right;

        r.Right = SelectionRectangle.Right > SelectionRectangle.Left ? SelectionRectangle.Right : SelectionRectangle.Left;

        r.Top = SelectionRectangle.Top < SelectionRectangle.Bottom ? SelectionRectangle.Top : SelectionRectangle.Bottom;

        r.Bottom = SelectionRectangle.Bottom > SelectionRectangle.Top ? SelectionRectangle.Bottom : SelectionRectangle.Top;

        SelectionRectangle = r;
        RectangleSelectEventsStore.Raise(this);
        ResetRectangleSelection();
    }
}
