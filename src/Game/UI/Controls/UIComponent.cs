#nullable disable warnings
// Translated from AoWComp.pas.
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Controls;

// Fields: Color, SelectedColor, Valid (Validate found everything it needs), Aligned, Sized,
// _cursor, OnDraw, OnPreDraw, OnMouseDown, OnMouseUp, OnMouseMove, OnKeyPress,
// FOnKeyDown, FOnChange, FMouseDown, FVisible, FHandled (a Check* method consumed the input),
// _winWidth, _winHeight, _winLeft, _winTop, _font, _text, Redraw, _borderWidth,
// FBorderHeight, FPriority, FDrawn (the component is drawn as it is now), FFirstDraw, FMouseIn,
// ToolTipCount, _radioIndex, _checked, _ownerItem, OnUpdate, _captureInput, OnDebugMessage,
// _ownerComponent, Children (sorted by _priority), _alignment, _masked, BlitList, DrawList
// (DirtyRectangle), BlitCount, DrawCount, ToolTipLeft, OldRectangle, Hidden, _doubleBuffered,
// SurfaceValid
public partial class UIComponent(Rtl.Component? owner) : Rtl.Component(owner)
{

    public int Color { get; private set; }
    public int SelectedColor { get; private set; }
    public bool Valid { get; set; }
    public bool Aligned { get; set; }
    public bool Sized { get; set; }

    [Published]
    public virtual int Cursor { get; set; }

    [Published]
    public event AoWOnDrawEvent<UIComponent>? Drawn;

    protected void RaiseDraw(UIComponent sender, LockedSurface surfDesc) => Drawn?.Invoke(sender, surfDesc);

    [Published]
    public event AoWOnDrawEvent<UIComponent>? Drawing;

    protected void RaisePreDraw(UIComponent sender, LockedSurface surfDesc) => Drawing?.Invoke(sender, surfDesc);

    [Published]
    public event MouseEvent<UIComponent>? MousePressed;

    protected void RaiseMouseDown(UIComponent sender, MouseButton button, ShiftStates shift, int x, int y) => MousePressed?.Invoke(sender, button, shift, x, y);

    [Published]
    public event MouseEvent<UIComponent>? MouseReleased;

    protected void RaiseMouseUp(UIComponent sender, MouseButton button, ShiftStates shift, int x, int y) => MouseReleased?.Invoke(sender, button, shift, x, y);

    [Published]
    public event MouseMoveEvent<UIComponent>? MouseMoved;

    protected void RaiseMouseMove(UIComponent sender, ShiftStates shift, int x, int y) => MouseMoved?.Invoke(sender, shift, x, y);

    [Published]
    public event KeyPressEvent<UIComponent>? CharacterTyped;

    protected void RaiseKeyPress(UIComponent sender, ref byte key) => CharacterTyped?.Invoke(sender, ref key);

    // Memo.CheckKeyPress (AoWMemo.pas) calls OnKeyPress after a test of OnKeyDown only. This call
    // keeps that: it has no null test of its own and throws when OnKeyPress has no handler.
    protected void RaiseKeyPressUnchecked(UIComponent sender, ref byte key) => CharacterTyped(sender, ref key);

    [Published]
    public event KeyEvent<UIComponent>? KeyPressed;

    protected void RaiseKeyDown(UIComponent sender, ref ushort key, ShiftStates shift) => KeyPressed?.Invoke(sender, ref key, shift);

    protected bool HasKeyDownHandlers => KeyPressed is not null;

    public event NotifyEvent<UIComponent>? Changed;

    internal void RaiseChange(UIComponent sender) => Changed?.Invoke(sender);

    public bool IsMouseDown { get; protected set; }

    private bool _visible = true;

    public bool Handled { get; set; }

    private int _winWidth;
    private int _winHeight;
    private int _winLeft;
    private int _winTop;

    [Published]
    public virtual ImageLibraryFont Font { get; set; }

    [Published]
    public virtual string Text { get; set; } = "";

    public bool Redraw { get; set; }

    [Published]
    public int BorderWidth { get; set; }

    [Published]
    public int BorderHeight { get; set; }

    private int _priority;

    public bool IsDrawn { get; set; }
    public bool FirstDraw { get; set; } = true;
    public bool MouseIn { get; protected set; }
    public int ToolTipCount { get; protected set; }

    [Published]
    public int RadioIndex { get; set; }

    [Published]
    public virtual bool Checked { get; set; }

    [Published]
    public int OwnerItem { get; set; }

    [Published]
    public event NotifyEvent<UIComponent>? Updated;

    protected void RaiseUpdate(UIComponent sender) => Updated?.Invoke(sender);

    // Nothing in the port raises OnDebugMessage. The bound handler is kept.
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event OnDebugMessage keeps this backing field until the port raises it.")]
    private AoWOnDebugEvent _onDebugMessage;

    [Published]
    public event AoWOnDebugEvent DebugMessageWritten { add => _onDebugMessage += value; remove => _onDebugMessage -= value; }

    private UIComponent? _ownerComponent;

    public DelphiList<UIComponent> Children { get; init; } = [];

    public List<DirtyRectangle> BlitList { get; } = [];
    public List<DirtyRectangle> DrawList { get; } = [];
    public int BlitCount { get; protected set; }
    public int DrawCount { get; protected set; }
    public int ToolTipLeft { get; protected set; }

    public SizedRectangle OldRectangle { get; protected set; }

    public bool IsHidden { get; set; }

    [Published]
    public virtual bool DoubleBuffered { get; set; }

    public bool SurfaceValid { get; set; }

    // AoWComp.pas:233
    public override void Dispose()
    {
        SetOwner(null);
        while (Children.Count > 0)
        {
            Children[0].SetOwner(null);
        }

        Children?.Dispose();
        Alignment?.Dispose();
        for (int i = 0, last = DrawList.Count - 1; i <= last; i++)
        {
        }

        for (int i = 0, last = BlitList.Count - 1; i <= last; i++)
        {
        }

        BlitList?.Clear();
        DrawList?.Clear();
        base.Dispose();
    }

    // AoWComp.pas:251
    public virtual void OnChangeText(object sender) => Sized = false;

    // AoWComp.pas:300
    public virtual void Validate()
    {
        Sized = false;
        Aligned = false;
        foreach (UIComponent? component in Children)
        {
            component.Valid = false;
        }
    }

    // AoWComp.pas:678
    public virtual void SetSize()
    {
        Sized = true;
        IsDrawn = false;
        BlitCount = 0;
        DrawCount = 0;
        foreach (UIComponent? component in Children)
        {
            component.Aligned = false;
        }
    }

    // AoWComp.pas:690
    public virtual void ReAlign()
    {
        if (_ownerComponent is not null && (Alignment.AlignWidth != AlignWidth.None || Alignment.AlignHeight != AlignHeight.None))
        {
            switch (Alignment.AlignWidth)
            {
                case AlignWidth.Left:
                    _winLeft = (_ownerComponent._winWidth * Alignment.LeftOffsetPercent / 100) + Alignment.LeftOffset;
                    if (Alignment.WidthPercent > 0)
                    {
                        _winWidth = _ownerComponent._winWidth * Alignment.WidthPercent / 100;
                    }

                    break;
                case AlignWidth.Right:
                    if (Alignment.WidthPercent > 0)
                    {
                        _winWidth = _ownerComponent._winWidth * Alignment.WidthPercent / 100;
                    }

                    _winLeft = _ownerComponent._winWidth - Alignment.RightOffset - _winWidth - (_ownerComponent._winWidth * Alignment.RightOffsetPercent / 100);
                    break;
                case AlignWidth.Both:
                    _winLeft = (_ownerComponent._winWidth * Alignment.LeftOffsetPercent / 100) + Alignment.LeftOffset;
                    _winWidth = _ownerComponent._winWidth - _winLeft - Alignment.RightOffset - (_ownerComponent._winWidth * Alignment.RightOffsetPercent / 100);
                    break;
                case AlignWidth.Center:
                    _winLeft = (_ownerComponent._winWidth / 2) - (_winWidth / 2);
                    break;
                case AlignWidth.None:
                default:
                    break;
            }

            switch (Alignment.AlignHeight)
            {
                case AlignHeight.Top:
                    _winTop = (_ownerComponent._winHeight * Alignment.TopOffsetPercent / 100) + Alignment.TopOffset;
                    if (Alignment.HeightPercent > 0)
                    {
                        _winHeight = _ownerComponent._winHeight * Alignment.HeightPercent / 100;
                    }

                    break;
                case AlignHeight.Bottom:
                    if (Alignment.HeightPercent > 0)
                    {
                        _winHeight = _ownerComponent._winHeight * Alignment.HeightPercent / 100;
                    }

                    _winTop = _ownerComponent._winHeight - Alignment.BottomOffset - _winHeight - (_ownerComponent._winHeight * Alignment.BottomOffsetPercent / 100);
                    break;
                case AlignHeight.Both:
                    _winTop = (_ownerComponent._winHeight * Alignment.TopOffsetPercent / 100) + Alignment.TopOffset;
                    _winHeight = _ownerComponent._winHeight - _winTop - Alignment.BottomOffset - (_ownerComponent._winHeight * Alignment.BottomOffsetPercent / 100);
                    break;
                case AlignHeight.Center:
                    _winTop = (_ownerComponent._winHeight / 2) - (_winHeight / 2);
                    break;
                case AlignHeight.None:
                default:
                    break;
            }
        }

        Aligned = true;
        Sized = false;
        foreach (UIComponent? component in Children)
        {
            component.Aligned = false;
        }
    }

    // AoWComp.pas:753
    // { Setup brings the component up to date: Validate, SetSize and ReAlign when their flag is clear,
    //   then Setup of every visible child; an exception in any step is reported in a message box }
    public virtual void Setup()
    {
        if (_visible)
        {
            try
            {
                if (!Valid)
                {
                    Validate();
                }
            }
            catch
            {
                _ = MessageBox(0, text: "Exception during Validate", caption: $"Error - {Name}", flags: 0);
            }

            if (Valid)
            {
                try
                {
                    if (!Sized)
                    {
                        SetSize();
                    }
                }
                catch
                {
                    _ = MessageBox(0, text: "Exception during SetSize", caption: $"Error - {Name}", flags: 0);
                }

                try
                {
                    if (!Aligned)
                    {
                        ReAlign();
                    }
                }
                catch
                {
                    _ = MessageBox(0, text: "Exception during ReAlign", caption: $"Error - {Name}", flags: 0);
                }

                try
                {
                    if (!Sized)
                    {
                        SetSize();
                    }
                }
                catch
                {
                    _ = MessageBox(0, text: "Exception during SetSize", caption: $"Error - {Name}", flags: 0);
                }

                try
                {
                    for (int i = 0, last = Children.Count - 1; i <= last; i++)
                    {
                        if (Children[i]._visible)
                        {
                            Children[i].Setup();
                        }
                    }
                }
                catch
                {
                    _ = MessageBox(0, text: "Exception during Setup", caption: $"Error - {Name}", flags: 0);
                }
            }
        }
    }

    // AoWComp.pas:796
    public virtual void Update()
    {
        try
        {
            if (Valid && _visible)
            {
                Updated?.Invoke(this);

                for (int i = Children.Count - 1; i >= 0; i--)
                {
                    Children[i].Update();
                }

                if (!Valid || !Aligned || !Sized)
                {
                    Setup();
                }
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Update", caption: $"Error - {Name}", flags: 0);
        }
    }

    // AoWComp.pas:202
    public virtual void ClearDrawList()
    {
        DrawCount = 0;
        foreach (UIComponent? component in Children)
        {
            if (component._visible)
            {
                component.ClearDrawList();
            }
        }
    }

    // AoWComp.pas:212
    public virtual void ClearBlitList()
    {
        BlitCount = 0;
        foreach (UIComponent? component in Children)
        {
            if (component._visible)
            {
                component.ClearBlitList();
            }
        }
    }

    // AoWComp.pas:576
    public virtual void InvalidateRectangle(int index, ref Rect rectangle, UIComponent? sender)
    {
    }

    // AoWComp.pas:314
    // { AddBltRect adds ARect to the rectangles to blit, clipped against the rectangles already in the
    //   list so that no two overlap: the parts of ARect that another rectangle covers are cut off (a
    //   cut that leaves two pieces adds one of them as a new rectangle), and an old rectangle that ARect
    //   covers on one side is trimmed or emptied. }
    public virtual void AddBlitRectangle(ref Rect rectangle, UIComponent? sender)
    {
        if (Valid && _visible)
        {
            int count = BlitCount;
            bool add = true;
            if (rectangle.Left < 0)
            {
                rectangle.Left = 0;
            }

            if (rectangle.Top < 0)
            {
                rectangle.Top = 0;
            }

            DirtyRectangle r;
            if (rectangle.Right < rectangle.Left || rectangle.Bottom < rectangle.Top)
            {
                add = false;
            }
            else
            {
                int i = 0;
                while (i < count && add)
                {
                    r = BlitList[i];
                    if (rectangle.Right >= r.Rectangle.Left && rectangle.Left <= r.Rectangle.Right && rectangle.Bottom >= r.Rectangle.Top && rectangle.Top <= r.Rectangle.Bottom)
                    {
                        DirtyRectangle n;
                        if (r.Rectangle.Left <= rectangle.Left)
                        {
                            if (rectangle.Right <= r.Rectangle.Right)
                            {
                                if (rectangle.Top >= r.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        add = false;
                                    }
                                    else
                                    {
                                        rectangle.Top = r.Rectangle.Bottom + 1;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        rectangle.Bottom = r.Rectangle.Top - 1;
                                    }
                                    else
                                    {
                                        if (BlitCount == BlitList.Count)
                                        {
                                            DirtyRectangle dirtyRectangle = new();
                                            BlitList.Add(dirtyRectangle);
                                        }

                                        n = BlitList[BlitCount];
                                        n.Rectangle = new Rect(rectangle.Left, rectangle.Top, rectangle.Right, r.Rectangle.Top - 1);
                                        BlitCount += 1;
                                        rectangle.Top = r.Rectangle.Bottom + 1;
                                    }
                                }
                            }
                            else
                            {
                                if (rectangle.Top >= r.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        rectangle.Left = r.Rectangle.Right + 1;
                                    }
                                    else
                                    {
                                        if (BlitCount == BlitList.Count)
                                        {
                                            DirtyRectangle dirtyRectangle = new();
                                            BlitList.Add(dirtyRectangle);
                                        }

                                        n = BlitList[BlitCount];
                                        n.Rectangle = new Rect(rectangle.Left, r.Rectangle.Bottom + 1, r.Rectangle.Right, rectangle.Bottom);
                                        BlitCount += 1;
                                        rectangle.Left = r.Rectangle.Right + 1;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        if (BlitCount == BlitList.Count)
                                        {
                                            DirtyRectangle dirtyRectangle = new();
                                            BlitList.Add(dirtyRectangle);
                                        }

                                        n = BlitList[BlitCount];
                                        n.Rectangle = new Rect(rectangle.Left, rectangle.Top, r.Rectangle.Right, r.Rectangle.Top - 1);
                                        BlitCount += 1;
                                        rectangle.Left = r.Rectangle.Right + 1;
                                    }
                                    else
                                    {
                                        Rect rectangle2 = r.Rectangle;
                                        rectangle2.Right = rectangle.Left - 1;
                                        r.Rectangle = rectangle2;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (rectangle.Right <= r.Rectangle.Right)
                            {
                                if (rectangle.Top >= r.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        rectangle.Right = r.Rectangle.Left - 1;
                                    }
                                    else
                                    {
                                        if (BlitCount == BlitList.Count)
                                        {
                                            DirtyRectangle dirtyRectangle = new();
                                            BlitList.Add(dirtyRectangle);
                                        }

                                        n = BlitList[BlitCount];
                                        n.Rectangle = new Rect(r.Rectangle.Left, r.Rectangle.Bottom + 1, rectangle.Right, rectangle.Bottom);
                                        BlitCount += 1;
                                        rectangle.Right = r.Rectangle.Left - 1;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        if (BlitCount == BlitList.Count)
                                        {
                                            DirtyRectangle dirtyRectangle = new();
                                            BlitList.Add(dirtyRectangle);
                                        }

                                        n = BlitList[BlitCount];
                                        n.Rectangle = new Rect(r.Rectangle.Left, rectangle.Top, rectangle.Right, r.Rectangle.Top - 1);
                                        BlitCount += 1;
                                        rectangle.Right = r.Rectangle.Left - 1;
                                    }
                                    else
                                    {
                                        Rect rectangle3 = r.Rectangle;
                                        rectangle3.Left = rectangle.Right + 1;
                                        r.Rectangle = rectangle3;
                                    }
                                }
                            }
                            else
                            {
                                if (rectangle.Top >= r.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        if (BlitCount == BlitList.Count)
                                        {
                                            DirtyRectangle dirtyRectangle = new();
                                            BlitList.Add(dirtyRectangle);
                                        }

                                        n = BlitList[BlitCount];
                                        n.Rectangle = new Rect(rectangle.Left, rectangle.Top, r.Rectangle.Left - 1, rectangle.Bottom);
                                        BlitCount += 1;
                                        rectangle.Left = r.Rectangle.Right + 1;
                                    }
                                    else
                                    {
                                        Rect rectangle4 = r.Rectangle;
                                        rectangle4.Bottom = rectangle.Top - 1;
                                        r.Rectangle = rectangle4;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        Rect rectangle5 = r.Rectangle;
                                        rectangle5.Top = rectangle.Bottom + 1;
                                        r.Rectangle = rectangle5;
                                    }
                                    else
                                    {
                                        r.Rectangle = new Rect(0, 0, 0, 0);
                                    }
                                }
                            }
                        }
                    }

                    i += 1;
                }
            }

            if (add)
            {
                if (BlitCount == BlitList.Count)
                {
                    DirtyRectangle dirtyRectangle = new();
                    BlitList.Add(dirtyRectangle);
                }

                r = BlitList[BlitCount];
                r.Rectangle = rectangle;
                BlitCount += 1;
            }
        }
    }

    // AoWComp.pas:445
    // { AddDrawRect: the same for the rectangles to redraw; the rectangle added last records Sender }
    public virtual void AddDrawRectangle(ref Rect rectangle, UIComponent? sender)
    {
        if (Valid && _visible)
        {
            int count = DrawCount;
            bool add = true;
            if (rectangle.Left < 0)
            {
                rectangle.Left = 0;
            }

            if (rectangle.Top < 0)
            {
                rectangle.Top = 0;
            }

            DirtyRectangle r;
            if (rectangle.Right < rectangle.Left || rectangle.Bottom < rectangle.Top)
            {
                add = false;
            }
            else
            {
                int i = 0;
                while (i < count && add)
                {
                    r = DrawList[i];
                    if (rectangle.Right >= r.Rectangle.Left && rectangle.Left <= r.Rectangle.Right && rectangle.Bottom >= r.Rectangle.Top && rectangle.Top <= r.Rectangle.Bottom)
                    {
                        DirtyRectangle n;
                        if (r.Rectangle.Left <= rectangle.Left)
                        {
                            if (rectangle.Right <= r.Rectangle.Right)
                            {
                                if (rectangle.Top >= r.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        add = false;
                                    }
                                    else
                                    {
                                        rectangle.Top = r.Rectangle.Bottom + 1;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        rectangle.Bottom = r.Rectangle.Top - 1;
                                    }
                                    else
                                    {
                                        if (DrawCount == DrawList.Count)
                                        {
                                            DirtyRectangle dirtyRectangle = new();
                                            DrawList.Add(dirtyRectangle);
                                        }

                                        n = DrawList[DrawCount];
                                        n.Rectangle = new Rect(rectangle.Left, rectangle.Top, rectangle.Right, r.Rectangle.Top - 1);
                                        DrawCount += 1;
                                        rectangle.Top = r.Rectangle.Bottom + 1;
                                    }
                                }
                            }
                            else
                            {
                                if (rectangle.Top >= r.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        rectangle.Left = r.Rectangle.Right + 1;
                                    }
                                    else
                                    {
                                        if (DrawCount == DrawList.Count)
                                        {
                                            DirtyRectangle dirtyRectangle = new();
                                            DrawList.Add(dirtyRectangle);
                                        }

                                        n = DrawList[DrawCount];
                                        n.Rectangle = new Rect(rectangle.Left, r.Rectangle.Bottom + 1, r.Rectangle.Right, rectangle.Bottom);
                                        DrawCount += 1;
                                        rectangle.Left = r.Rectangle.Right + 1;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        if (DrawCount == DrawList.Count)
                                        {
                                            DirtyRectangle dirtyRectangle = new();
                                            DrawList.Add(dirtyRectangle);
                                        }

                                        n = DrawList[DrawCount];
                                        n.Rectangle = new Rect(rectangle.Left, rectangle.Top, r.Rectangle.Right, r.Rectangle.Top - 1);
                                        DrawCount += 1;
                                        rectangle.Left = r.Rectangle.Right + 1;
                                    }
                                    else
                                    {
                                        Rect rectangle2 = r.Rectangle;
                                        rectangle2.Right = rectangle.Left - 1;
                                        r.Rectangle = rectangle2;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (rectangle.Right <= r.Rectangle.Right)
                            {
                                if (rectangle.Top >= r.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        rectangle.Right = r.Rectangle.Left - 1;
                                    }
                                    else
                                    {
                                        if (DrawCount == DrawList.Count)
                                        {
                                            DirtyRectangle dirtyRectangle = new();
                                            DrawList.Add(dirtyRectangle);
                                        }

                                        n = DrawList[DrawCount];
                                        n.Rectangle = new Rect(r.Rectangle.Left, r.Rectangle.Bottom + 1, rectangle.Right, rectangle.Bottom);
                                        DrawCount += 1;
                                        rectangle.Right = r.Rectangle.Left - 1;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        if (DrawCount == DrawList.Count)
                                        {
                                            DirtyRectangle dirtyRectangle = new();
                                            DrawList.Add(dirtyRectangle);
                                        }

                                        n = DrawList[DrawCount];
                                        n.Rectangle = new Rect(r.Rectangle.Left, rectangle.Top, rectangle.Right, r.Rectangle.Top - 1);
                                        DrawCount += 1;
                                        rectangle.Right = r.Rectangle.Left - 1;
                                    }
                                    else
                                    {
                                        Rect rectangle3 = r.Rectangle;
                                        rectangle3.Left = rectangle.Right + 1;
                                        r.Rectangle = rectangle3;
                                    }
                                }
                            }
                            else
                            {
                                if (rectangle.Top >= r.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        if (DrawCount == DrawList.Count)
                                        {
                                            DirtyRectangle dirtyRectangle = new();
                                            DrawList.Add(dirtyRectangle);
                                        }

                                        n = DrawList[DrawCount];
                                        n.Rectangle = new Rect(rectangle.Left, rectangle.Top, r.Rectangle.Left - 1, rectangle.Bottom);
                                        DrawCount += 1;
                                        rectangle.Left = r.Rectangle.Right + 1;
                                    }
                                    else
                                    {
                                        Rect rectangle4 = r.Rectangle;
                                        rectangle4.Bottom = rectangle.Top - 1;
                                        r.Rectangle = rectangle4;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= r.Rectangle.Bottom)
                                    {
                                        Rect rectangle5 = r.Rectangle;
                                        rectangle5.Top = rectangle.Bottom + 1;
                                        r.Rectangle = rectangle5;
                                    }
                                    else
                                    {
                                        r.Rectangle = new Rect(0, 0, 0, 0);
                                    }
                                }
                            }
                        }
                    }

                    i += 1;
                }
            }

            if (add)
            {
                if (DrawCount == DrawList.Count)
                {
                    DirtyRectangle dirtyRectangle = new();
                    DrawList.Add(dirtyRectangle);
                }

                r = DrawList[DrawCount];
                r.Rectangle = rectangle;
                r.Owner = sender;
                DrawCount += 1;
            }
        }
    }

    // AoWComp.pas:262
    public virtual void SetOwner(UIComponent? value)
    {
        int oldIndex;
        if (_ownerComponent is not null)
        {
            oldIndex = _ownerComponent.Children.IndexOf(this);
            _ownerComponent.Children.Delete(oldIndex);
        }
        else
        {
            oldIndex = -2;
        }

        if (value != _ownerComponent)
        {
            Valid = false;
        }

        _ownerComponent = value;
        int newIndex = -1;
        if (_ownerComponent is not null)
        {
            int i = 0;
            while (i < _ownerComponent.Children.Count)
            {
                if (_ownerComponent.Children[i]._priority > _priority)
                {
                    newIndex = i;
                    i = _ownerComponent.Children.Count;
                }
                else
                {
                    i += 1;
                }
            }

            if (newIndex == -1)
            {
                _ = _ownerComponent.Children.Add(this);
            }
            else
            {
                _ownerComponent.Children.Insert(newIndex, item: this);
            }
        }

        if (newIndex != oldIndex)
        {
            if (DoubleBuffered)
            {
                SurfaceValid = false;
            }
            else
            {
                IsDrawn = false;
            }
        }
    }

    // AoWComp.pas:580
    public virtual void SetWinLeft(int value)
    {
        _winLeft = value;
        IsDrawn = false;
    }

    // AoWComp.pas:586
    public virtual void SetWinTop(int value)
    {
        _winTop = value;
        IsDrawn = false;
    }

    // AoWComp.pas:592
    public virtual void SetWidth(int value)
    {
        _winWidth = value;
        Sized = false;
    }

    // AoWComp.pas:598
    public virtual void SetHeight(int value)
    {
        _winHeight = value;
        Sized = false;
    }

    // AoWComp.pas:604
    public virtual void SetPriority(int value)
    {
        _priority = value;
        SetOwner(_ownerComponent);
    }

    // AoWComp.pas:610
    public virtual void SetVisible(bool value)
    {
        if (value != _visible)
        {
            _visible = value;
            IsDrawn = false;
            if (_visible)
            {
                IsHidden = false;
                ClearBlitList();
                ClearDrawList();
                if (!Valid || !Aligned || !Sized)
                {
                    Setup();
                }
            }
            else
            {
                IsHidden = true;
            }
        }
    }

    // AoWComp.pas:629
    public virtual void SetColor(int value)
    {
        Color = value;
        IsDrawn = false;
    }

    // AoWComp.pas:635
    public virtual void SetSelectedColor(int value)
    {
        SelectedColor = value;
        IsDrawn = false;
    }

    // AoWComp.pas:641
    public virtual void DrawSurface(Surface? surface)
    {
    }

    // AoWComp.pas:645
    public virtual void DrawTo(LockedSurface lockedSurface)
    {
    }

    // AoWComp.pas:649
    public virtual void Draw(LockedSurface lockedSurface)
    {
    }

    // AoWComp.pas:653
    public virtual void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y) => Handled = false;

    // AoWComp.pas:658
    public virtual void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y) => Handled = false;

    // AoWComp.pas:663
    public virtual void CheckMouseMove(ShiftStates shift, int X, int Y) => Handled = false;

    // AoWComp.pas:668
    public virtual void CheckKeyPress(ref byte key) => Handled = false;

    // AoWComp.pas:673
    public virtual void CheckKeyDown(ref ushort key, ShiftStates shift) => Handled = false;

    // AoWComp.pas:256
    public void SetAlign(Alignment value)
    {
        Alignment.Assign(value);
        Aligned = false;
    }

    public int Priority { get => _priority; set => SetPriority(value); }

    internal void SetPriorityWithoutReorder(int value) => _priority = value;

    public int WinLeft { get => _winLeft; set => SetWinLeft(value); }

    public int AbsoluteLeft => OwnerComponent is Windows.BaseWindow owner ? WinLeft + owner.AbsoluteLeft : WinLeft;

    public int WindowRelativeLeft => OwnerComponent is Windows.Panel panel ? WinLeft + panel.WindowRelativeLeft : WinLeft;

    protected internal void SetWinLeftWithoutInvalidate(int value) => _winLeft = value;

    public int WinTop { get => _winTop; set => SetWinTop(value); }

    public int AbsoluteTop => OwnerComponent is Windows.BaseWindow owner ? WinTop + owner.AbsoluteTop : WinTop;

    public int WindowRelativeTop => OwnerComponent is Windows.Panel panel ? WinTop + panel.WindowRelativeTop : WinTop;

    protected internal void SetWinTopWithoutInvalidate(int value) => _winTop = value;

    public int WinWidth { get => _winWidth; set => SetWidth(value); }

    protected internal void SetWinWidthWithoutInvalidate(int value) => _winWidth = value;

    public int WinHeight { get => _winHeight; set => SetHeight(value); }

    protected internal void SetWinHeightWithoutInvalidate(int value) => _winHeight = value;

    public int TextColor { get => Color; set => SetColor(value); }
    public int TextSelectedColor { get => SelectedColor; set => SetSelectedColor(value); }
    public UIComponent? OwnerComponent { get => _ownerComponent; set => SetOwner(value); }
    public Alignment Alignment { get; } = new();
    public bool Masked { get; set; }
    public bool CaptureInput { get; set; }

    [Published]
    [DefaultValue(true)]
    public bool Visible { get => _visible; set => SetVisible(value); }

    protected internal void SetVisibleWithoutUpdate(bool value) => _visible = value;

    public delegate void AoWOnDebugEvent(string Dtext);
}

public enum UIComponentButtonType : byte
{
    Text = 0, Icon = 1
}
