#nullable disable warnings
using Aow.Game.Draw.Images;
using Aow.Game.Host;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Platform;

namespace Aow.Game.Tests;

/// <summary>
/// The mouse wheel is a port addition. These cases drive the mapper, the headless host and the
/// window routing without a display.
/// </summary>
public sealed class MouseWheelTests
{
    private sealed class Sink : IDisplayEvents
    {
        internal List<(int Steps, ShiftStates Shift, int X, int Y)> Rolls { get; } = [];

        public void MouseWheel(int steps, ShiftStates shift, int x, int y) => Rolls.Add((steps, shift, x, y));
    }

    private static void Place(UIComponent component, UIComponent owner, int left, int top, int width, int height, int priority = 0)
    {
        component.SetWinLeftWithoutInvalidate(left);
        component.SetWinTopWithoutInvalidate(top);
        component.SetWinWidthWithoutInvalidate(width);
        component.SetWinHeightWithoutInvalidate(height);
        component.SetPriorityWithoutReorder(priority);
        component.SetOwner(owner);
    }

    private static void Fill(ListBox list, int count, int shown)
    {
        list.VisibleRows = shown;
        for (int i = 0; i < count; i++)
        {
            _ = list.Items.Add($"item{i}");
        }
    }

    private static VerticalScrollBar Bar(UIComponent owner, int left, int top, int width, int height, int max, int visible)
    {
        VerticalScrollBar bar = new(null);
        Place(bar, owner, left, top, width, height);
        bar.SetMinUnclamped(0);
        bar.SetMaxUnclamped(max);
        bar.SetNumberVisibleUnclamped(visible);
        bar.SetSmallChangeUnclamped(1);
        return bar;
    }

    [Fact]
    public void MapperCountsWholeStepsFlipsDirectionAndMapsThePointer()
    {
        InputMapper mapper = new();
        Sink sink = new();
        DisplayViewport view = new(100, 100, Scale: 2, 10, 20, 220, 240);
        mapper.SetModifiers(1);
        mapper.Wheel(2, direction: 0, 20, 34, viewport: view, sink);
        mapper.Wheel(2, direction: 1, 20, 34, viewport: view, sink);
        // The margin around the image is still inside the output, so it lands on the nearest edge.
        mapper.Wheel(1, direction: 0, 0, 20, viewport: view, sink);
        mapper.Wheel(1, direction: 0, -1, 20, viewport: view, sink);
        Assert.Equal([(2, true, 5, 7), (-2, true, 5, 7), (1, true, 0, 0)], sink.Rolls.Select(r => (r.Steps, r.Shift.Contains(ShiftStates.Shift), r.X, r.Y)));
    }

    [Fact]
    public void MapperAccumulatesFractionsAndDropsARollOutsideTheOutput()
    {
        InputMapper mapper = new();
        Sink sink = new();
        DisplayViewport view = new(100, 100, Scale: 1, 0, 0, 100, 100);
        mapper.Wheel(0.5f, direction: 0, 10, 10, viewport: view, sink);
        mapper.Wheel(0.5f, direction: 0, 10, 10, viewport: view, sink);
        mapper.Wheel(0.5f, direction: 1, 10, 10, viewport: view, sink);
        mapper.Wheel(0.5f, direction: 1, 10, 10, viewport: view, sink);
        mapper.Wheel(0.5f, direction: 0, -1, 10, viewport: view, sink);
        mapper.Wheel(0.5f, direction: 0, 10, 10, viewport: view, sink);
        // The leftover +0.5 is dropped when the roll reverses, so the next two halves make one step down.
        mapper.Wheel(-0.5f, direction: 0, 10, 10, viewport: view, sink);
        mapper.Wheel(-0.5f, direction: 0, 10, 10, viewport: view, sink);
        mapper.Reset();
        mapper.Wheel(0.5f, direction: 0, 10, 10, viewport: view, sink);
        mapper.Wheel(0.5f, direction: 0, 10, 10, viewport: view, sink);
        Assert.Equal([1, -1, -1, 1], sink.Rolls.Select(r => r.Steps));
    }

    [Fact]
    public void HeadlessHostUsesTheSameMapper()
    {
        HeadlessDisplayHost host = new(Path.Combine(Path.GetTempPath(), "open-aow-wheel-unused"));
        Sink sink = new();
        host.Wheel(0.5f, direction: 0, 12, 34, sink);
        host.Wheel(0.5f, direction: 0, 12, 34, sink);
        host.Wheel(3, direction: 1, 12, 34, sink);
        host.Wheel(4, direction: 0, -1, 34, sink);
        host.Wheel(1, direction: 0, 640, 10, sink);
        Assert.Equal([(1, 12, 34), (-3, 12, 34)], sink.Rolls.Select(r => (r.Steps, r.X, r.Y)));
    }

    [Fact]
    public void DeepestChildWinsAndAnEmptyPanelReachesItsOneBar()
    {
        WindowManager manager = new(null);
        Window window = new(null);
        Place(window, owner: manager, 100, 50, 400, 400, priority: 1);
        Panel panel = new(null);
        Place(panel, owner: window, 20, 30, 220, 160);
        ListBox list = new(null);
        Place(list, owner: panel, 10, 15, 80, 40);
        Fill(list, count: 10, shown: 3);
        VerticalScrollBar bar = Bar(panel, 180, 10, 16, 80, max: 9, visible: 3);
        ShiftStates shift = default;

        Assert.True(manager.CheckMouseWheel(-1, shift, 140, 100));
        Assert.Equal(1, list.FirstShownItem);
        Assert.Equal(0, bar.Position);

        Assert.True(manager.CheckMouseWheel(-1, shift, 125, 90));
        Assert.Equal(1, list.FirstShownItem);
        Assert.Equal(1, bar.Position);

        Assert.False(manager.CheckMouseWheel(0, shift, 140, 100));
        Assert.Equal(1, list.FirstShownItem);
    }

    [Fact]
    public void AChildThatDoesNotScrollLeavesTheWheelWithItsParent()
    {
        WindowManager manager = new(null);
        Window window = new(null);
        Place(window, owner: manager, 0, 0, 400, 400, priority: 1);
        Panel panel = new(null);
        Place(panel, owner: window, 0, 0, 300, 200);
        ListBox list = new(null);
        Place(list, owner: panel, 0, 0, 40, 40);
        Fill(list, count: 8, shown: 3);
        VerticalScrollBar bar = Bar(panel, 200, 0, 16, 80, max: 9, visible: 3);
        Label label = new(null);
        Place(label, owner: panel, 80, 80, 40, 20);

        Assert.True(manager.CheckMouseWheel(shift: default, steps: -2, x: 90, y: 90));
        Assert.Equal(0, list.FirstShownItem);
        Assert.Equal(2, bar.Position);
    }

    [Fact]
    public void TwoVisibleBarsStayPutAndAHiddenBarIsNotCounted()
    {
        WindowManager manager = new(null);
        Window window = new(null);
        Place(window, owner: manager, 0, 0, 400, 400, priority: 1);
        Panel panel = new(null);
        Place(panel, owner: window, 0, 0, 300, 200);
        VerticalScrollBar first = Bar(panel, 200, 0, 16, 40, max: 9, visible: 3);
        VerticalScrollBar second = Bar(panel, 200, 50, 16, 40, max: 9, visible: 3);
        ShiftStates shift = default;

        // The window contains the pointer, so the manager accepts the wheel. Two bars mean the panel scrolls neither.
        Assert.False(window.CheckMouseWheel(-1, shift, 20, 20));
        Assert.True(manager.CheckMouseWheel(-1, shift, 20, 20));
        Assert.Equal(0, first.Position);
        Assert.Equal(0, second.Position);

        Assert.True(manager.CheckMouseWheel(-1, shift, 205, 10));
        Assert.Equal(1, first.Position);
        Assert.Equal(0, second.Position);

        second.SetVisibleWithoutUpdate(false);
        Assert.True(manager.CheckMouseWheel(-1, shift, 20, 20));
        Assert.Equal(2, first.Position);
        Assert.Equal(0, second.Position);
    }

    [Fact]
    public void AModalWindowBlocksEveryWindowUnderTheOverlay()
    {
        WindowManager manager = new(null);
        manager.SetOverlayPriorityImmediately(10);
        Window back = new(null);
        Place(back, owner: manager, 0, 0, 400, 300, priority: 1);
        ListBox backList = new(null);
        Place(backList, owner: back, 0, 0, 400, 40);
        Fill(backList, count: 10, shown: 3);
        Window modal = new(null);
        Place(modal, owner: manager, 0, 0, 100, 100, priority: 10);
        ListBox modalList = new(null);
        Place(modalList, owner: modal, 0, 0, 80, 40);
        Fill(modalList, count: 10, shown: 3);
        ShiftStates shift = default;

        Assert.True(manager.CheckMouseWheel(-1, shift, 20, 20));
        Assert.Equal(1, modalList.FirstShownItem);
        Assert.Equal(0, backList.FirstShownItem);

        Assert.False(manager.CheckMouseWheel(-1, shift, 200, 20));
        Assert.Equal(0, backList.FirstShownItem);

        modalList.SetOwner(null);
        Assert.True(manager.CheckMouseWheel(-1, shift, 20, 20));
        Assert.Equal(0, backList.FirstShownItem);
    }

    [Fact]
    public void ScrollBarAndLinkedListClampAtBothEndsAndFireOnChange()
    {
        WindowManager manager = new(null);
        Window window = new(null);
        Place(window, owner: manager, 0, 0, 200, 120, priority: 1);
        ListBox list = new(null);
        Place(list, owner: window, 0, 0, 80, 40);
        Fill(list, count: 10, shown: 3);
        VerticalScrollBar bar = Bar(window, 90, 0, 16, 40, max: 9, visible: 3);
        bar.SetSmallChangeUnclamped(2);
        list.SetVerticalScrollBarWithoutLayout(bar);
        int changes = 0;
        bar.Change += _ =>
        {
            changes++;
            list.ListOffset = bar.Position;
        };
        ShiftStates shift = default;

        Assert.True(list.CheckMouseWheel(-4, shift, 10, 10));
        Assert.Equal(7, bar.Position);
        Assert.Equal(7, list.FirstShownItem);
        Assert.Equal(4, changes);

        Assert.True(list.CheckMouseWheel(-1, shift, 10, 10));
        Assert.Equal(7, bar.Position);
        Assert.Equal(5, changes);

        Assert.True(bar.CheckMouseWheel(4, shift, 95, 10));
        Assert.Equal(0, bar.Position);
        Assert.Equal(0, list.FirstShownItem);
        Assert.Equal(9, changes);

        Assert.True(bar.CheckMouseWheel(1, shift, 95, 10));
        Assert.Equal(0, bar.Position);
        Assert.Equal(10, changes);
    }

    [Fact]
    public void ListBoxWithoutABarScrollsItsOwnOffset()
    {
        WindowManager manager = new(null);
        Window window = new(null);
        Place(window, owner: manager, 0, 0, 200, 120, priority: 1);
        Panel panel = new(null);
        Place(panel, owner: window, 0, 0, 180, 100);
        ListBox list = new(null);
        Place(list, owner: panel, 10, 10, 80, 40);
        Fill(list, count: 10, shown: 4);
        VerticalScrollBar bar = Bar(panel, 140, 10, 16, 40, max: 9, visible: 3);
        int changes = 0;
        list.Change += _ => changes++;
        ShiftStates shift = default;

        Assert.True(manager.CheckMouseWheel(-1, shift, 30, 30));
        Assert.Equal(1, list.FirstShownItem);
        Assert.True(manager.CheckMouseWheel(-100, shift, 30, 30));
        Assert.Equal(6, list.FirstShownItem);
        Assert.True(manager.CheckMouseWheel(1, shift, 30, 30));
        Assert.Equal(5, list.FirstShownItem);
        Assert.True(manager.CheckMouseWheel(100, shift, 30, 30));
        Assert.Equal(0, list.FirstShownItem);
        Assert.Equal(0, bar.Position);
        Assert.Equal(0, changes);
    }

    [Fact]
    public void BookTurnsOnePagePerEventAndStopsAtTheEnds()
    {
        WindowManager manager = new(null);
        BookWindow book = new(null);
        Place(book, owner: manager, 0, 0, 200, 160, priority: 1);
        int page = 0;
        Button back = new(null);
        Button forward = new(null);
        back.SetVisibleWithoutUpdate(false);
        back.Click += _ =>
        {
            page--;
            forward.SetVisibleWithoutUpdate(true);
            if (page == 0)
            {
                back.SetVisibleWithoutUpdate(false);
            }
        };
        forward.Click += _ =>
        {
            page++;
            back.SetVisibleWithoutUpdate(true);
            if (page == 2)
            {
                forward.SetVisibleWithoutUpdate(false);
            }
        };
        book.PageBack = back;
        book.PageForward = forward;
        ListBox list = new(null);
        Place(list, owner: book, 10, 10, 60, 30);
        Fill(list, count: 8, shown: 3);
        ShiftStates shift = default;

        Assert.True(manager.CheckMouseWheel(-5, shift, 120, 80));
        Assert.Equal(1, page);
        Assert.True(back.Visible);
        Assert.True(forward.Visible);
        Assert.True(manager.CheckMouseWheel(-1, shift, 120, 80));
        Assert.Equal(2, page);
        Assert.False(forward.Visible);
        Assert.False(book.CheckMouseWheel(-1, shift, 120, 80));
        Assert.True(manager.CheckMouseWheel(-1, shift, 120, 80));
        Assert.Equal(2, page);

        Assert.True(manager.CheckMouseWheel(5, shift, 120, 80));
        Assert.Equal(1, page);
        Assert.True(manager.CheckMouseWheel(1, shift, 120, 80));
        Assert.Equal(0, page);
        Assert.False(back.Visible);
        Assert.False(book.CheckMouseWheel(1, shift, 120, 80));
        Assert.True(manager.CheckMouseWheel(1, shift, 120, 80));
        Assert.Equal(0, page);

        Assert.True(manager.CheckMouseWheel(-1, shift, 20, 20));
        Assert.Equal(0, page);
        Assert.Equal(1, list.FirstShownItem);

        book.Animating = true;
        Assert.True(manager.CheckMouseWheel(-1, shift, 20, 20));
        Assert.Equal(0, page);
        Assert.Equal(1, list.FirstShownItem);
    }

    [Fact]
    public void ShortListAndShortMemoLeaveTheWheelWithTheParent()
    {
        WindowManager manager = new(null);
        Window window = new(null);
        Place(window, owner: manager, 0, 0, 200, 120, priority: 1);
        VerticalScrollBar parent = Bar(window, 170, 10, 16, 80, max: 9, visible: 3);
        ListBox list = new(null);
        Place(list, owner: window, 10, 10, 70, 40);
        Fill(list, count: 2, shown: 4);
        Memo memo = new(null);
        Place(memo, owner: window, 90, 10, 70, 40);
        memo.CaptureInput = false;
        VerticalScrollBar memoBar = Bar(window, 140, 10, 16, 40, max: 0, visible: 1);
        memoBar.SetVisibleWithoutUpdate(false);
        memo.SetVerticalScrollBarWithoutLayout(memoBar);
        ShiftStates shift = default;

        Assert.False(list.CheckMouseWheel(-1, shift, 20, 20));
        Assert.True(window.CheckMouseWheel(-1, shift, 20, 20));
        Assert.Equal(0, list.FirstShownItem);
        Assert.Equal(1, parent.Position);

        Assert.False(memo.CheckMouseWheel(-1, shift, 100, 20));
        Assert.True(window.CheckMouseWheel(-1, shift, 100, 20));
        Assert.Equal(0, memoBar.Position);
        Assert.Equal(2, parent.Position);

        memoBar.SetMaxUnclamped(8);
        memoBar.SetNumberVisibleUnclamped(3);
        memoBar.SetVisibleWithoutUpdate(true);
        Assert.True(window.CheckMouseWheel(-1, shift, 100, 20));
        Assert.Equal(1, memoBar.Position);
        Assert.Equal(2, parent.Position);
    }

    [Fact]
    public void ArrowClickAndHeldArrowStepClampAndFireOnChange()
    {
        Window window = new(null);
        WindowManager manager = new(null);
        Place(window, owner: manager, 0, 0, 80, 80);
        VerticalScrollBar bar = new(null);
        Place(bar, owner: window, 10, 10, 16, 40);
        bar.SetMinUnclamped(0);
        bar.SetMaxUnclamped(5);
        bar.SetNumberVisibleUnclamped(2);
        bar.SetSmallChangeUnclamped(2);
        bar.PositionCount = bar.Max - bar.Min + 1;
        bar.Delay = 1;
        InterfaceImageLibrary library = new(null);
        Picture16 image = new(library);
        image.SetHeight(8);
        library.SetImage(0, image);
        bar.SetImageLibraryIDecrementWithoutInvalidate(library);
        bar.SetImageLibraryImageIncrementWithoutInvalidate(library);
        int changes = 0;
        bar.Change += _ => changes++;
        int x = window.WinLeft + bar.WinLeft + 1;
        int top = window.WinTop + bar.WinTop;

        bar.CheckMouseDown(MouseButton.Left, shift: default, x, top + 1);
        Assert.Equal(0, bar.Position);
        Assert.Equal(1, changes);

        bar.CheckMouseDown(MouseButton.Left, shift: default, x, top + bar.WinHeight - 1);
        Assert.Equal(2, bar.Position);
        Assert.Equal(2, changes);
        Assert.Equal(4, bar.HeldPart);

        bar.Update();
        Assert.Equal(4, bar.Position);
        Assert.Equal(3, changes);
        bar.Update();
        Assert.Equal(4, bar.Position);
        Assert.Equal(4, changes);
    }
}
