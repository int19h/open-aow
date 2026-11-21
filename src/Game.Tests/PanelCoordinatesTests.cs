#nullable disable warnings
using Aow.Game.Draw;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;

namespace Aow.Game.Tests;

public sealed class PanelCoordinatesTests
{
    private static void Place(UIComponent component, UIComponent? owner, int left, int top, int width, int height, int priority = 0)
    {
        component.SetWinLeftWithoutInvalidate(left);
        component.SetWinTopWithoutInvalidate(top);
        component.SetWinWidthWithoutInvalidate(width);
        component.SetWinHeightWithoutInvalidate(height);
        component.SetPriorityWithoutReorder(priority);
        component.SetOwner(owner);
        component.Valid = true;
        component.Sized = true;
        component.Aligned = true;
    }

    [Fact]
    public void PanelCoordinatesStayFixedAcrossInputWheelAndDrawWhileChildrenSeeAbsoluteOrigin()
    {
        WindowManager manager = new(null);
        Place(manager, owner: null, left: 0, top: 0, width: 640, height: 480);

        Window window = new(null) { DoubleBuffered = false };
        Place(window, owner: manager, 100, 50, 400, 350, priority: 1);
        window.Validate();

        Panel panel = new(null);
        Place(panel, owner: window, 20, 30, 220, 180);
        panel.Validate();

        Panel nested = new(null);
        Place(nested, owner: panel, 15, 25, 100, 80);
        nested.Validate();

        List<(int WinLeft, int WinTop)> observedPanelCoords = [];
        List<(int WinLeft, int WinTop)> observedNestedCoords = [];
        (int X, int Y)? mouseDownLocal = null;
        (int X, int Y)? mouseMoveLocal = null;
        (int X, int Y)? mouseUpScreen = null;
        (int AbsLeft, int AbsTop, Rect Clip)? drawObserved = null;
        (int AbsLeft, int AbsTop)? nestedDrawObserved = null;

        UserDrawBox box = new(null);
        Place(box, owner: panel, 7, 9, 40, 30, priority: 1);
        box.MouseDown += (sender, _, _, x, y) =>
        {
            observedPanelCoords.Add((panel.WinLeft, panel.WinTop));
            mouseDownLocal = (x, y);
        };
        box.MouseMove += (sender, _, x, y) =>
        {
            observedPanelCoords.Add((panel.WinLeft, panel.WinTop));
            mouseMoveLocal = (x, y);
        };
        box.MouseUp += (sender, _, _, x, y) =>
        {
            observedPanelCoords.Add((panel.WinLeft, panel.WinTop));
            mouseUpScreen = (x, y);
        };
        box.OnDraw += (sender, lockedSurface) =>
        {
            observedPanelCoords.Add((panel.WinLeft, panel.WinTop));
            drawObserved = (sender.Window.AbsoluteLeft, sender.Window.AbsoluteTop, lockedSurface.ClipRectangle);
        };

        UserDrawBox nestedBox = new(null);
        Place(nestedBox, owner: nested, 3, 4, 20, 20);
        nestedBox.OnDraw += (sender, _) =>
        {
            observedNestedCoords.Add((nested.WinLeft, nested.WinTop));
            nestedDrawObserved = (sender.Window.AbsoluteLeft, sender.Window.AbsoluteTop);
        };

        ListBox list = new(null) { VisibleRows = 2 };
        Place(list, owner: panel, 60, 10, 80, 40);
        for (int i = 0; i < 6; i++)
        {
            _ = list.Items.Add($"item{i}");
        }

        ShiftStates shift = default;
        manager.CheckMouseDown(MouseButton.Left, shift, 132, 94);
        Assert.Equal((20, 30), (panel.WinLeft, panel.WinTop));
        Assert.Equal((5, 5), mouseDownLocal);

        manager.CheckMouseMove(shift, 134, 97);
        Assert.Equal((20, 30), (panel.WinLeft, panel.WinTop));
        Assert.Equal((7, 8), mouseMoveLocal);

        manager.CheckMouseUp(MouseButton.Left, shift, 132, 94);
        Assert.Equal((20, 30), (panel.WinLeft, panel.WinTop));
        Assert.Equal((132, 94), mouseUpScreen);

        Assert.True(manager.CheckMouseWheel(-1, shift, 190, 100));
        Assert.Equal((20, 30), (panel.WinLeft, panel.WinTop));
        Assert.Equal(1, list.FirstShownItem);
        list.Valid = false;

        using DataSurface surface = new(640, 480, pixelFormatId: PixelFormat.Rgb555, palette: null);
        LockedSurface lockedSurface = default;
        Assert.Equal(GraphicsResult.Ok, surface.Lock(ref lockedSurface));
        try
        {
            lockedSurface.ClipRectangle = new Rect(0, 0, 640, 480);
            panel.Draw(lockedSurface);
        }
        finally
        {
            _ = surface.Unlock();
        }

        Assert.Equal((20, 30), (panel.WinLeft, panel.WinTop));
        Assert.Equal((15, 25), (nested.WinLeft, nested.WinTop));
        (int absLeft, int absTop, Rect clip) = Assert.NotNull(drawObserved);
        Assert.Equal((120, 80), (absLeft, absTop));
        Assert.Equal(new Rect(120, 80, 340, 260), clip);
        Assert.Equal((135, 105), nestedDrawObserved);
        Assert.All(observedPanelCoords, coords => Assert.Equal((20, 30), coords));
        Assert.All(observedNestedCoords, coords => Assert.Equal((15, 25), coords));
    }
}
