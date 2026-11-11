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
        internal readonly List<(int Steps, Set<ShiftState> Shift, int X, int Y)> Rolls = [];

        public void MouseWheel(int steps, Set<ShiftState> shift, int x, int y) => Rolls.Add((steps, shift, x, y));
    }

    private static void Place(UIComponent component, UIComponent owner, int left, int top, int width, int height, int priority = 0)
    {
        component.FWinLeft = left;
        component.FWinTop = top;
        component.FWinWidth = width;
        component.FWinHeight = height;
        component.FPriority = priority;
        component.SetOwner(owner);
    }

    private static void Fill(ListBox list, int count, int shown)
    {
        list.F11C = shown;
        for (int i = 0; i < count; i++) list.FItems.Add("item" + i);
    }

    private static VerticalScrollBar Bar(UIComponent owner, int left, int top, int width, int height, int max, int visible)
    {
        var bar = new VerticalScrollBar(null);
        Place(bar, owner, left, top, width, height);
        bar.FMin = 0;
        bar.FMax = max;
        bar.FNumberVisible = visible;
        bar.FSmallChange = 1;
        return bar;
    }

    [Fact]
    public void MapperCountsWholeStepsFlipsDirectionAndMapsThePointer()
    {
        var mapper = new InputMapper();
        var sink = new Sink();
        var view = new DisplayViewport(100, 100, 2, 10, 20, 220, 240);
        mapper.SetModifiers(1);
        mapper.Wheel(2, 0, 20, 34, view, sink);
        mapper.Wheel(2, 1, 20, 34, view, sink);
        // The margin around the image is still inside the output, so it lands on the nearest edge.
        mapper.Wheel(1, 0, 0, 20, view, sink);
        mapper.Wheel(1, 0, -1, 20, view, sink);
        Assert.Equal([(2, true, 5, 7), (-2, true, 5, 7), (1, true, 0, 0)], sink.Rolls.Select(r => (r.Steps, r.Shift.Contains(ShiftState.Shift), r.X, r.Y)));
    }

    [Fact]
    public void MapperAccumulatesFractionsAndDropsARollOutsideTheOutput()
    {
        var mapper = new InputMapper();
        var sink = new Sink();
        var view = new DisplayViewport(100, 100, 1, 0, 0, 100, 100);
        mapper.Wheel(0.5f, 0, 10, 10, view, sink);
        mapper.Wheel(0.5f, 0, 10, 10, view, sink);
        mapper.Wheel(0.5f, 1, 10, 10, view, sink);
        mapper.Wheel(0.5f, 1, 10, 10, view, sink);
        mapper.Wheel(0.5f, 0, -1, 10, view, sink);
        mapper.Wheel(0.5f, 0, 10, 10, view, sink);
        // The leftover +0.5 is dropped when the roll reverses, so the next two halves make one step down.
        mapper.Wheel(-0.5f, 0, 10, 10, view, sink);
        mapper.Wheel(-0.5f, 0, 10, 10, view, sink);
        mapper.Reset();
        mapper.Wheel(0.5f, 0, 10, 10, view, sink);
        mapper.Wheel(0.5f, 0, 10, 10, view, sink);
        Assert.Equal([1, -1, -1, 1], sink.Rolls.Select(r => r.Steps));
    }

    [Fact]
    public void HeadlessHostUsesTheSameMapper()
    {
        var host = new HeadlessDisplayHost(Path.Combine(Path.GetTempPath(), "open-aow-wheel-unused"));
        var sink = new Sink();
        host.Wheel(0.5f, 0, 12, 34, sink);
        host.Wheel(0.5f, 0, 12, 34, sink);
        host.Wheel(3, 1, 12, 34, sink);
        host.Wheel(4, 0, -1, 34, sink);
        host.Wheel(1, 0, 640, 10, sink);
        Assert.Equal([(1, 12, 34), (-3, 12, 34)], sink.Rolls.Select(r => (r.Steps, r.X, r.Y)));
    }

    [Fact]
    public void DeepestChildWinsAndAnEmptyPanelReachesItsOneBar()
    {
        var manager = new WindowManager(null);
        var window = new Window(null);
        Place(window, manager, 100, 50, 400, 400, 1);
        var panel = new Panel(null);
        Place(panel, window, 20, 30, 220, 160);
        var list = new ListBox(null);
        Place(list, panel, 10, 15, 80, 40);
        Fill(list, 10, 3);
        var bar = Bar(panel, 180, 10, 16, 80, 9, 3);
        Set<ShiftState> shift = default;

        Assert.True(manager.CheckMouseWheel(-1, shift, 140, 100));
        Assert.Equal(1, list.F15C);
        Assert.Equal(0, bar.FPosition);

        Assert.True(manager.CheckMouseWheel(-1, shift, 125, 90));
        Assert.Equal(1, list.F15C);
        Assert.Equal(1, bar.FPosition);

        Assert.False(manager.CheckMouseWheel(0, shift, 140, 100));
        Assert.Equal(1, list.F15C);
    }

    [Fact]
    public void AChildThatDoesNotScrollLeavesTheWheelWithItsParent()
    {
        var manager = new WindowManager(null);
        var window = new Window(null);
        Place(window, manager, 0, 0, 400, 400, 1);
        var panel = new Panel(null);
        Place(panel, window, 0, 0, 300, 200);
        var list = new ListBox(null);
        Place(list, panel, 0, 0, 40, 40);
        Fill(list, 8, 3);
        var bar = Bar(panel, 200, 0, 16, 80, 9, 3);
        var label = new Label(null);
        Place(label, panel, 80, 80, 40, 20);
        Set<ShiftState> shift = default;

        Assert.True(manager.CheckMouseWheel(-2, shift, 90, 90));
        Assert.Equal(0, list.F15C);
        Assert.Equal(2, bar.FPosition);
    }

    [Fact]
    public void TwoVisibleBarsStayPutAndAHiddenBarIsNotCounted()
    {
        var manager = new WindowManager(null);
        var window = new Window(null);
        Place(window, manager, 0, 0, 400, 400, 1);
        var panel = new Panel(null);
        Place(panel, window, 0, 0, 300, 200);
        var first = Bar(panel, 200, 0, 16, 40, 9, 3);
        var second = Bar(panel, 200, 50, 16, 40, 9, 3);
        Set<ShiftState> shift = default;

        // The window contains the pointer, so the manager accepts the wheel. Two bars mean the panel scrolls neither.
        Assert.False(window.CheckMouseWheel(-1, shift, 20, 20));
        Assert.True(manager.CheckMouseWheel(-1, shift, 20, 20));
        Assert.Equal(0, first.FPosition);
        Assert.Equal(0, second.FPosition);

        Assert.True(manager.CheckMouseWheel(-1, shift, 205, 10));
        Assert.Equal(1, first.FPosition);
        Assert.Equal(0, second.FPosition);

        second.FVisible = false;
        Assert.True(manager.CheckMouseWheel(-1, shift, 20, 20));
        Assert.Equal(2, first.FPosition);
        Assert.Equal(0, second.FPosition);
    }

    [Fact]
    public void AModalWindowBlocksEveryWindowUnderTheOverlay()
    {
        var manager = new WindowManager(null);
        manager.FOverlayPriority = 10;
        var back = new Window(null);
        Place(back, manager, 0, 0, 400, 300, 1);
        var backList = new ListBox(null);
        Place(backList, back, 0, 0, 400, 40);
        Fill(backList, 10, 3);
        var modal = new Window(null);
        Place(modal, manager, 0, 0, 100, 100, 10);
        var modalList = new ListBox(null);
        Place(modalList, modal, 0, 0, 80, 40);
        Fill(modalList, 10, 3);
        Set<ShiftState> shift = default;

        Assert.True(manager.CheckMouseWheel(-1, shift, 20, 20));
        Assert.Equal(1, modalList.F15C);
        Assert.Equal(0, backList.F15C);

        Assert.False(manager.CheckMouseWheel(-1, shift, 200, 20));
        Assert.Equal(0, backList.F15C);

        modalList.SetOwner(null);
        Assert.True(manager.CheckMouseWheel(-1, shift, 20, 20));
        Assert.Equal(0, backList.F15C);
    }

    [Fact]
    public void ScrollBarAndLinkedListClampAtBothEndsAndFireOnChange()
    {
        var manager = new WindowManager(null);
        var window = new Window(null);
        Place(window, manager, 0, 0, 200, 120, 1);
        var list = new ListBox(null);
        Place(list, window, 0, 0, 80, 40);
        Fill(list, 10, 3);
        var bar = Bar(window, 90, 0, 16, 40, 9, 3);
        bar.FSmallChange = 2;
        list.FVScrollBar = bar;
        int changes = 0;
        bar.FOnChange = _ =>
        {
            changes++;
            list.ListOffset = bar.FPosition;
        };
        Set<ShiftState> shift = default;

        Assert.True(list.CheckMouseWheel(-4, shift, 10, 10));
        Assert.Equal(7, bar.FPosition);
        Assert.Equal(7, list.F15C);
        Assert.Equal(4, changes);

        Assert.True(list.CheckMouseWheel(-1, shift, 10, 10));
        Assert.Equal(7, bar.FPosition);
        Assert.Equal(5, changes);

        Assert.True(bar.CheckMouseWheel(4, shift, 95, 10));
        Assert.Equal(0, bar.FPosition);
        Assert.Equal(0, list.F15C);
        Assert.Equal(9, changes);

        Assert.True(bar.CheckMouseWheel(1, shift, 95, 10));
        Assert.Equal(0, bar.FPosition);
        Assert.Equal(10, changes);
    }

    [Fact]
    public void ListBoxWithoutABarScrollsItsOwnOffset()
    {
        var manager = new WindowManager(null);
        var window = new Window(null);
        Place(window, manager, 0, 0, 200, 120, 1);
        var panel = new Panel(null);
        Place(panel, window, 0, 0, 180, 100);
        var list = new ListBox(null);
        Place(list, panel, 10, 10, 80, 40);
        Fill(list, 10, 4);
        var bar = Bar(panel, 140, 10, 16, 40, 9, 3);
        int changes = 0;
        list.FOnChange = _ => changes++;
        Set<ShiftState> shift = default;

        Assert.True(manager.CheckMouseWheel(-1, shift, 30, 30));
        Assert.Equal(1, list.F15C);
        Assert.True(manager.CheckMouseWheel(-100, shift, 30, 30));
        Assert.Equal(6, list.F15C);
        Assert.True(manager.CheckMouseWheel(1, shift, 30, 30));
        Assert.Equal(5, list.F15C);
        Assert.True(manager.CheckMouseWheel(100, shift, 30, 30));
        Assert.Equal(0, list.F15C);
        Assert.Equal(0, bar.FPosition);
        Assert.Equal(0, changes);
    }

    [Fact]
    public void BookTurnsOnePagePerEventAndStopsAtTheEnds()
    {
        var manager = new WindowManager(null);
        var book = new BookWindow(null);
        Place(book, manager, 0, 0, 200, 160, 1);
        int page = 0;
        var back = new Button(null);
        var forward = new Button(null);
        back.FVisible = false;
        back.FOnClick = _ =>
        {
            page--;
            forward.FVisible = true;
            if (page == 0) back.FVisible = false;
        };
        forward.FOnClick = _ =>
        {
            page++;
            back.FVisible = true;
            if (page == 2) forward.FVisible = false;
        };
        book.PageBack = back;
        book.PageForward = forward;
        var list = new ListBox(null);
        Place(list, book, 10, 10, 60, 30);
        Fill(list, 8, 3);
        Set<ShiftState> shift = default;

        Assert.True(manager.CheckMouseWheel(-5, shift, 120, 80));
        Assert.Equal(1, page);
        Assert.True(back.FVisible);
        Assert.True(forward.FVisible);
        Assert.True(manager.CheckMouseWheel(-1, shift, 120, 80));
        Assert.Equal(2, page);
        Assert.False(forward.FVisible);
        Assert.False(book.CheckMouseWheel(-1, shift, 120, 80));
        Assert.True(manager.CheckMouseWheel(-1, shift, 120, 80));
        Assert.Equal(2, page);

        Assert.True(manager.CheckMouseWheel(5, shift, 120, 80));
        Assert.Equal(1, page);
        Assert.True(manager.CheckMouseWheel(1, shift, 120, 80));
        Assert.Equal(0, page);
        Assert.False(back.FVisible);
        Assert.False(book.CheckMouseWheel(1, shift, 120, 80));
        Assert.True(manager.CheckMouseWheel(1, shift, 120, 80));
        Assert.Equal(0, page);

        Assert.True(manager.CheckMouseWheel(-1, shift, 20, 20));
        Assert.Equal(0, page);
        Assert.Equal(1, list.F15C);

        book.FAnimating = true;
        Assert.True(manager.CheckMouseWheel(-1, shift, 20, 20));
        Assert.Equal(0, page);
        Assert.Equal(1, list.F15C);
    }

    [Fact]
    public void ShortListAndShortMemoLeaveTheWheelWithTheParent()
    {
        var manager = new WindowManager(null);
        var window = new Window(null);
        Place(window, manager, 0, 0, 200, 120, 1);
        var parent = Bar(window, 170, 10, 16, 80, 9, 3);
        var list = new ListBox(null);
        Place(list, window, 10, 10, 70, 40);
        Fill(list, 2, 4);
        var memo = new Memo(null);
        Place(memo, window, 90, 10, 70, 40);
        memo.FCaptureInput = false;
        var memoBar = Bar(window, 140, 10, 16, 40, 0, 1);
        memoBar.FVisible = false;
        memo.FVScrollBar = memoBar;
        Set<ShiftState> shift = default;

        Assert.False(list.CheckMouseWheel(-1, shift, 20, 20));
        Assert.True(window.CheckMouseWheel(-1, shift, 20, 20));
        Assert.Equal(0, list.F15C);
        Assert.Equal(1, parent.FPosition);

        Assert.False(memo.CheckMouseWheel(-1, shift, 100, 20));
        Assert.True(window.CheckMouseWheel(-1, shift, 100, 20));
        Assert.Equal(0, memoBar.FPosition);
        Assert.Equal(2, parent.FPosition);

        memoBar.FMax = 8;
        memoBar.FNumberVisible = 3;
        memoBar.FVisible = true;
        Assert.True(window.CheckMouseWheel(-1, shift, 100, 20));
        Assert.Equal(1, memoBar.FPosition);
        Assert.Equal(2, parent.FPosition);
    }

    [Fact]
    public void ArrowClickAndHeldArrowStepClampAndFireOnChange()
    {
        var window = new Window(null);
        var manager = new WindowManager(null);
        Place(window, manager, 0, 0, 80, 80);
        var bar = new VerticalScrollBar(null);
        Place(bar, window, 10, 10, 16, 40);
        bar.FMin = 0;
        bar.FMax = 5;
        bar.FNumberVisible = 2;
        bar.FSmallChange = 2;
        bar.F14C = bar.FMax - bar.FMin + 1;
        bar.FDelay = 1;
        var library = new InterfaceImageLibrary(null);
        var image = new Picture16(library);
        image.SetHeight(8);
        library.SetImage(0, image);
        bar.FILIDec = library;
        bar.FILIInc = library;
        int changes = 0;
        bar.FOnChange = _ => changes++;
        int x = window.FWinLeft + bar.FWinLeft + 1;
        int top = window.FWinTop + bar.FWinTop;

        bar.CheckMouseDown(MouseButton.Left, default, x, top + 1);
        Assert.Equal(0, bar.FPosition);
        Assert.Equal(1, changes);

        bar.CheckMouseDown(MouseButton.Left, default, x, top + bar.FWinHeight - 1);
        Assert.Equal(2, bar.FPosition);
        Assert.Equal(2, changes);
        Assert.Equal(4, bar.F151);

        bar.Update();
        Assert.Equal(4, bar.FPosition);
        Assert.Equal(3, changes);
        bar.Update();
        Assert.Equal(4, bar.FPosition);
        Assert.Equal(4, changes);
    }
}
