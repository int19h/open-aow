// Translated from AoWComp.pas.
using System.ComponentModel;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

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
public partial class UIComponent : Rtl.Component
{
    public int Color { get; private set; }
    public int SelectedColor { get; private set; }
    public bool Valid { get; set; }
    public bool Aligned { get; set; }
    public bool Sized { get; set; }

    public int _cursor;
    [Published]
    public event AoWOnDrawEvent<UIComponent> OnDraw;

    protected void RaiseDraw(UIComponent sender, LockedSurface surfDesc)
    {
        if (OnDraw != null)
        {
            OnDraw(sender, surfDesc);
        }
    }

    [Published]
    public event AoWOnDrawEvent<UIComponent> OnPreDraw;

    protected void RaisePreDraw(UIComponent sender, LockedSurface surfDesc)
    {
        if (OnPreDraw != null)
        {
            OnPreDraw(sender, surfDesc);
        }
    }

    [Published]
    public event MouseEvent<UIComponent> OnMouseDown;

    protected void RaiseMouseDown(UIComponent sender, MouseButton button, Set<ShiftState> shift, int x, int y)
    {
        if (OnMouseDown != null)
        {
            OnMouseDown(sender, button, shift, x, y);
        }
    }

    [Published]
    public event MouseEvent<UIComponent> OnMouseUp;

    protected void RaiseMouseUp(UIComponent sender, MouseButton button, Set<ShiftState> shift, int x, int y)
    {
        if (OnMouseUp != null)
        {
            OnMouseUp(sender, button, shift, x, y);
        }
    }

    [Published]
    public event MouseMoveEvent<UIComponent> OnMouseMove;

    protected void RaiseMouseMove(UIComponent sender, Set<ShiftState> shift, int x, int y)
    {
        if (OnMouseMove != null)
        {
            OnMouseMove(sender, shift, x, y);
        }
    }

    [Published]
    public event KeyPressEvent<UIComponent> OnKeyPress;

    protected void RaiseKeyPress(UIComponent sender, ref byte key)
    {
        if (OnKeyPress != null)
        {
            OnKeyPress(sender, ref key);
        }
    }

    // Memo.CheckKeyPress (AoWMemo.pas) calls OnKeyPress after a test of OnKeyDown only. This call
    // keeps that: it has no null test of its own and throws when OnKeyPress has no handler.
    protected void RaiseKeyPressUnchecked(UIComponent sender, ref byte key) => OnKeyPress(sender, ref key);

    [Published]
    public event KeyEvent<UIComponent> OnKeyDown;

    protected void RaiseKeyDown(UIComponent sender, ref ushort key, Set<ShiftState> shift)
    {
        if (OnKeyDown != null)
        {
            OnKeyDown(sender, ref key, shift);
        }
    }

    protected bool HasKeyDownHandlers => OnKeyDown != null;

    public event NotifyEvent<UIComponent> OnChange;

    internal void RaiseChange(UIComponent sender)
    {
        if (OnChange != null)
        {
            OnChange(sender);
        }
    }

    public bool MouseDown { get; protected set; }

    private bool _visible;

    public bool Handled { get; set; }

    private int _winWidth;
    private int _winHeight;
    private int _winLeft;
    private int _winTop;
    public ImageLibraryFont _font;
    public string _text = "";

    public bool Redraw { get; set; }

    public int _borderWidth;
    public int _borderHeight;
    private int _priority;

    public bool Drawn { get; set; }
    public bool FirstDraw { get; set; }
    public bool MouseIn { get; protected set; }
    public int ToolTipCount { get; protected set; }

    public int _radioIndex;
    public bool _checked;
    public int _ownerItem;
    [Published]
    public event NotifyEvent<UIComponent> OnUpdate;

    protected void RaiseUpdate(UIComponent sender)
    {
        if (OnUpdate != null)
        {
            OnUpdate(sender);
        }
    }

    private bool _captureInput;
    // Nothing in the port raises OnDebugMessage. The bound handler is kept.
    private AoWOnDebugEvent _onDebugMessage;

    [Published]
    public event AoWOnDebugEvent OnDebugMessage { add => _onDebugMessage += value; remove => _onDebugMessage -= value; }

    private UIComponent _ownerComponent;

    public DelphiList<UIComponent> Children { get; init; }

    private readonly Alignment _alignment;
    private bool _masked;

    public List<DirtyRectangle> BlitList { get; }
    public List<DirtyRectangle> DrawList { get; }
    public int BlitCount { get; protected set; }
    public int DrawCount { get; protected set; }
    public int ToolTipLeft { get; protected set; }

    public Rect OldRectangle;

    public bool Hidden { get; set; }

    public bool _doubleBuffered;

    public bool SurfaceValid { get; set; }

    // AoWComp.pas:222
    public UIComponent(Rtl.Component owner) : base(owner)
    {
        _visible = true;
        FirstDraw = true;
        _alignment = new Alignment();
        Children = new DelphiList<UIComponent>();
        BlitList = new List<DirtyRectangle>();
        DrawList = new List<DirtyRectangle>();
    }

    // AoWComp.pas:233
    public override void Dispose()
    {
        SetOwner(null);
        while (Children.Count > 0)
        {
            Children[0].SetOwner(null);
        }

        Children?.Dispose();
        _alignment?.Dispose();
        for (int I = 0, last = DrawList.Count - 1; I <= last; I++)
        {
        }

        for (int I = 0, last = BlitList.Count - 1; I <= last; I++)
        {
        }

        BlitList?.Clear();
        DrawList?.Clear();
        base.Dispose();
    }

    // AoWComp.pas:251
    public virtual void OnChangeText(object sender)
    {
        Sized = false;
    }

    // AoWComp.pas:300
    public virtual void Validate()
    {
        Sized = false;
        Aligned = false;
        for (int I = 0, last = Children.Count - 1; I <= last; I++)
        {
            Children[I].Valid = false;
        }
    }

    // AoWComp.pas:678
    public virtual void SetSize()
    {
        Sized = true;
        Drawn = false;
        BlitCount = 0;
        DrawCount = 0;
        for (int I = 0, last = Children.Count - 1; I <= last; I++)
        {
            Children[I].Aligned = false;
        }
    }

    // AoWComp.pas:690
    public virtual void ReAlign()
    {
        if (_ownerComponent != null && (_alignment.AlignWidth != AlignWidth.None || _alignment.AlignHeight != AlignHeight.None))
        {
            switch (this._alignment.AlignWidth)
            {
                case AlignWidth.Left:
                    _winLeft = _ownerComponent._winWidth * this._alignment.LeftOffsetPercent / 100 + this._alignment.LeftOffset;
                    if (_alignment.WidthPercent > 0)
                    {
                        _winWidth = _ownerComponent._winWidth * _alignment.WidthPercent / 100;
                    }

                    break;
                case AlignWidth.Right:
                    if (this._alignment.WidthPercent > 0)
                    {
                        _winWidth = _ownerComponent._winWidth * this._alignment.WidthPercent / 100;
                    }

                    _winLeft = _ownerComponent._winWidth - _alignment.RightOffset - _winWidth - _ownerComponent._winWidth * _alignment.RightOffsetPercent / 100;
                    break;
                case AlignWidth.Both:
                    _winLeft = _ownerComponent._winWidth * this._alignment.LeftOffsetPercent / 100 + this._alignment.LeftOffset;
                    _winWidth = _ownerComponent._winWidth - _winLeft - _alignment.RightOffset - _ownerComponent._winWidth * _alignment.RightOffsetPercent / 100;
                    break;
                case AlignWidth.Center:
                    _winLeft = _ownerComponent._winWidth / 2 - _winWidth / 2;
                    break;
            }

            switch (this._alignment.AlignHeight)
            {
                case AlignHeight.Top:
                    _winTop = _ownerComponent._winHeight * this._alignment.TopOffsetPercent / 100 + this._alignment.TopOffset;
                    if (_alignment.HeightPercent > 0)
                    {
                        _winHeight = _ownerComponent._winHeight * _alignment.HeightPercent / 100;
                    }

                    break;
                case AlignHeight.Bottom:
                    if (this._alignment.HeightPercent > 0)
                    {
                        _winHeight = _ownerComponent._winHeight * this._alignment.HeightPercent / 100;
                    }

                    _winTop = _ownerComponent._winHeight - _alignment.BottomOffset - _winHeight - _ownerComponent._winHeight * _alignment.BottomOffsetPercent / 100;
                    break;
                case AlignHeight.Both:
                    _winTop = _ownerComponent._winHeight * this._alignment.TopOffsetPercent / 100 + this._alignment.TopOffset;
                    _winHeight = _ownerComponent._winHeight - _winTop - _alignment.BottomOffset - _ownerComponent._winHeight * _alignment.BottomOffsetPercent / 100;
                    break;
                case AlignHeight.Center:
                    _winTop = _ownerComponent._winHeight / 2 - _winHeight / 2;
                    break;
            }
        }

        Aligned = true;
        Sized = false;
        for (int I = 0, last = Children.Count - 1; I <= last; I++)
        {
            Children[I].Aligned = false;
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
                MessageBox(0, "Exception during Validate", "Error - " + Name, 0);
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
                    MessageBox(0, "Exception during SetSize", "Error - " + Name, 0);
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
                    MessageBox(0, "Exception during ReAlign", "Error - " + Name, 0);
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
                    MessageBox(0, "Exception during SetSize", "Error - " + Name, 0);
                }

                try
                {
                    for (int I = 0, last = Children.Count - 1; I <= last; I++)
                    {
                        if (Children[I]._visible)
                        {
                            Children[I].Setup();
                        }
                    }
                }
                catch
                {
                    MessageBox(0, "Exception during Setup", "Error - " + Name, 0);
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
                if (OnUpdate != null)
                {
                    OnUpdate(this);
                }

                for (int I = Children.Count - 1; I >= 0; I--)
                {
                    Children[I].Update();
                }

                if (!Valid || !Aligned || !Sized)
                {
                    Setup();
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during Update", "Error - " + Name, 0);
        }
    }

    // AoWComp.pas:202
    public virtual void ClearDrawList()
    {
        DrawCount = 0;
        for (int I = 0, last = Children.Count - 1; I <= last; I++)
        {
            if (Children[I]._visible)
            {
                Children[I].ClearDrawList();
            }
        }
    }

    // AoWComp.pas:212
    public virtual void ClearBlitList()
    {
        BlitCount = 0;
        for (int I = 0, last = Children.Count - 1; I <= last; I++)
        {
            if (Children[I]._visible)
            {
                Children[I].ClearBlitList();
            }
        }
    }

    // AoWComp.pas:576
    public virtual void InvalidateRectangle(int index, ref Rect rectangle, UIComponent sender)
    {
    }

    // AoWComp.pas:314
    // { AddBltRect adds ARect to the rectangles to blit, clipped against the rectangles already in the
    //   list so that no two overlap: the parts of ARect that another rectangle covers are cut off (a
    //   cut that leaves two pieces adds one of them as a new rectangle), and an old rectangle that ARect
    //   covers on one side is trimmed or emptied. }
    public virtual void AddBlitRectangle(ref Rect rectangle, UIComponent sender)
    {
        DirtyRectangle N;
        int I;
        int count;
        bool add;
        DirtyRectangle R;
        if (Valid && _visible)
        {
            I = 0;
            count = BlitCount;
            add = true;
            if (rectangle.Left < 0)
            {
                rectangle.Left = 0;
            }

            if (rectangle.Top < 0)
            {
                rectangle.Top = 0;
            }

            if (rectangle.Right < rectangle.Left || rectangle.Bottom < rectangle.Top)
            {
                add = false;
            }
            else
            {
                while (I < count && add)
                {
                    R = BlitList[I];
                    if (rectangle.Right >= R.Rectangle.Left && rectangle.Left <= R.Rectangle.Right && rectangle.Bottom >= R.Rectangle.Top && rectangle.Top <= R.Rectangle.Bottom)
                    {
                        if (R.Rectangle.Left <= rectangle.Left)
                        {
                            if (rectangle.Right <= R.Rectangle.Right)
                            {
                                if (rectangle.Top >= R.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        add = false;
                                    }
                                    else
                                    {
                                        rectangle.Top = R.Rectangle.Bottom + 1;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        rectangle.Bottom = R.Rectangle.Top - 1;
                                    }
                                    else
                                    {
                                        if (BlitCount == BlitList.Count)
                                        {
                                            var dirtyRectangle = new DirtyRectangle();
                                            BlitList.Add(dirtyRectangle);
                                        }

                                        N = BlitList[BlitCount];
                                        N.Rectangle = new Rect(rectangle.Left, rectangle.Top, rectangle.Right, R.Rectangle.Top - 1);
                                        BlitCount += 1;
                                        rectangle.Top = R.Rectangle.Bottom + 1;
                                    }
                                }
                            }
                            else
                            {
                                if (rectangle.Top >= R.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        rectangle.Left = R.Rectangle.Right + 1;
                                    }
                                    else
                                    {
                                        if (BlitCount == BlitList.Count)
                                        {
                                            var dirtyRectangle = new DirtyRectangle();
                                            BlitList.Add(dirtyRectangle);
                                        }

                                        N = BlitList[BlitCount];
                                        N.Rectangle = new Rect(rectangle.Left, R.Rectangle.Bottom + 1, R.Rectangle.Right, rectangle.Bottom);
                                        BlitCount += 1;
                                        rectangle.Left = R.Rectangle.Right + 1;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        if (BlitCount == BlitList.Count)
                                        {
                                            var dirtyRectangle = new DirtyRectangle();
                                            BlitList.Add(dirtyRectangle);
                                        }

                                        N = BlitList[BlitCount];
                                        N.Rectangle = new Rect(rectangle.Left, rectangle.Top, R.Rectangle.Right, R.Rectangle.Top - 1);
                                        BlitCount += 1;
                                        rectangle.Left = R.Rectangle.Right + 1;
                                    }
                                    else
                                    {
                                        R.Rectangle.Right = rectangle.Left - 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (rectangle.Right <= R.Rectangle.Right)
                            {
                                if (rectangle.Top >= R.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        rectangle.Right = R.Rectangle.Left - 1;
                                    }
                                    else
                                    {
                                        if (BlitCount == BlitList.Count)
                                        {
                                            var dirtyRectangle = new DirtyRectangle();
                                            BlitList.Add(dirtyRectangle);
                                        }

                                        N = BlitList[BlitCount];
                                        N.Rectangle = new Rect(R.Rectangle.Left, R.Rectangle.Bottom + 1, rectangle.Right, rectangle.Bottom);
                                        BlitCount += 1;
                                        rectangle.Right = R.Rectangle.Left - 1;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        if (BlitCount == BlitList.Count)
                                        {
                                            var dirtyRectangle = new DirtyRectangle();
                                            BlitList.Add(dirtyRectangle);
                                        }

                                        N = BlitList[BlitCount];
                                        N.Rectangle = new Rect(R.Rectangle.Left, rectangle.Top, rectangle.Right, R.Rectangle.Top - 1);
                                        BlitCount += 1;
                                        rectangle.Right = R.Rectangle.Left - 1;
                                    }
                                    else
                                    {
                                        R.Rectangle.Left = rectangle.Right + 1;
                                    }
                                }
                            }
                            else
                            {
                                if (rectangle.Top >= R.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        if (BlitCount == BlitList.Count)
                                        {
                                            var dirtyRectangle = new DirtyRectangle();
                                            BlitList.Add(dirtyRectangle);
                                        }

                                        N = BlitList[BlitCount];
                                        N.Rectangle = new Rect(rectangle.Left, rectangle.Top, R.Rectangle.Left - 1, rectangle.Bottom);
                                        BlitCount += 1;
                                        rectangle.Left = R.Rectangle.Right + 1;
                                    }
                                    else
                                    {
                                        R.Rectangle.Bottom = rectangle.Top - 1;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        R.Rectangle.Top = rectangle.Bottom + 1;
                                    }
                                    else
                                    {
                                        R.Rectangle = new Rect(0, 0, 0, 0);
                                    }
                                }
                            }
                        }
                    }

                    I += 1;
                }
            }

            if (add)
            {
                if (BlitCount == BlitList.Count)
                {
                    var dirtyRectangle = new DirtyRectangle();
                    BlitList.Add(dirtyRectangle);
                }

                R = BlitList[BlitCount];
                R.Rectangle = rectangle;
                BlitCount += 1;
            }
        }
    }

    // AoWComp.pas:445
    // { AddDrawRect: the same for the rectangles to redraw; the rectangle added last records Sender }
    public virtual void AddDrawRectangle(ref Rect rectangle, UIComponent sender)
    {
        DirtyRectangle N;
        int I;
        int count;
        bool add;
        DirtyRectangle R;
        if (Valid && _visible)
        {
            I = 0;
            count = DrawCount;
            add = true;
            if (rectangle.Left < 0)
            {
                rectangle.Left = 0;
            }

            if (rectangle.Top < 0)
            {
                rectangle.Top = 0;
            }

            if (rectangle.Right < rectangle.Left || rectangle.Bottom < rectangle.Top)
            {
                add = false;
            }
            else
            {
                while (I < count && add)
                {
                    R = DrawList[I];
                    if (rectangle.Right >= R.Rectangle.Left && rectangle.Left <= R.Rectangle.Right && rectangle.Bottom >= R.Rectangle.Top && rectangle.Top <= R.Rectangle.Bottom)
                    {
                        if (R.Rectangle.Left <= rectangle.Left)
                        {
                            if (rectangle.Right <= R.Rectangle.Right)
                            {
                                if (rectangle.Top >= R.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        add = false;
                                    }
                                    else
                                    {
                                        rectangle.Top = R.Rectangle.Bottom + 1;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        rectangle.Bottom = R.Rectangle.Top - 1;
                                    }
                                    else
                                    {
                                        if (DrawCount == DrawList.Count)
                                        {
                                            var dirtyRectangle = new DirtyRectangle();
                                            DrawList.Add(dirtyRectangle);
                                        }

                                        N = DrawList[DrawCount];
                                        N.Rectangle = new Rect(rectangle.Left, rectangle.Top, rectangle.Right, R.Rectangle.Top - 1);
                                        DrawCount += 1;
                                        rectangle.Top = R.Rectangle.Bottom + 1;
                                    }
                                }
                            }
                            else
                            {
                                if (rectangle.Top >= R.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        rectangle.Left = R.Rectangle.Right + 1;
                                    }
                                    else
                                    {
                                        if (DrawCount == DrawList.Count)
                                        {
                                            var dirtyRectangle = new DirtyRectangle();
                                            DrawList.Add(dirtyRectangle);
                                        }

                                        N = DrawList[DrawCount];
                                        N.Rectangle = new Rect(rectangle.Left, R.Rectangle.Bottom + 1, R.Rectangle.Right, rectangle.Bottom);
                                        DrawCount += 1;
                                        rectangle.Left = R.Rectangle.Right + 1;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        if (DrawCount == DrawList.Count)
                                        {
                                            var dirtyRectangle = new DirtyRectangle();
                                            DrawList.Add(dirtyRectangle);
                                        }

                                        N = DrawList[DrawCount];
                                        N.Rectangle = new Rect(rectangle.Left, rectangle.Top, R.Rectangle.Right, R.Rectangle.Top - 1);
                                        DrawCount += 1;
                                        rectangle.Left = R.Rectangle.Right + 1;
                                    }
                                    else
                                    {
                                        R.Rectangle.Right = rectangle.Left - 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (rectangle.Right <= R.Rectangle.Right)
                            {
                                if (rectangle.Top >= R.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        rectangle.Right = R.Rectangle.Left - 1;
                                    }
                                    else
                                    {
                                        if (DrawCount == DrawList.Count)
                                        {
                                            var dirtyRectangle = new DirtyRectangle();
                                            DrawList.Add(dirtyRectangle);
                                        }

                                        N = DrawList[DrawCount];
                                        N.Rectangle = new Rect(R.Rectangle.Left, R.Rectangle.Bottom + 1, rectangle.Right, rectangle.Bottom);
                                        DrawCount += 1;
                                        rectangle.Right = R.Rectangle.Left - 1;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        if (DrawCount == DrawList.Count)
                                        {
                                            var dirtyRectangle = new DirtyRectangle();
                                            DrawList.Add(dirtyRectangle);
                                        }

                                        N = DrawList[DrawCount];
                                        N.Rectangle = new Rect(R.Rectangle.Left, rectangle.Top, rectangle.Right, R.Rectangle.Top - 1);
                                        DrawCount += 1;
                                        rectangle.Right = R.Rectangle.Left - 1;
                                    }
                                    else
                                    {
                                        R.Rectangle.Left = rectangle.Right + 1;
                                    }
                                }
                            }
                            else
                            {
                                if (rectangle.Top >= R.Rectangle.Top)
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        if (DrawCount == DrawList.Count)
                                        {
                                            var dirtyRectangle = new DirtyRectangle();
                                            DrawList.Add(dirtyRectangle);
                                        }

                                        N = DrawList[DrawCount];
                                        N.Rectangle = new Rect(rectangle.Left, rectangle.Top, R.Rectangle.Left - 1, rectangle.Bottom);
                                        DrawCount += 1;
                                        rectangle.Left = R.Rectangle.Right + 1;
                                    }
                                    else
                                    {
                                        R.Rectangle.Bottom = rectangle.Top - 1;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Bottom <= R.Rectangle.Bottom)
                                    {
                                        R.Rectangle.Top = rectangle.Bottom + 1;
                                    }
                                    else
                                    {
                                        R.Rectangle = new Rect(0, 0, 0, 0);
                                    }
                                }
                            }
                        }
                    }

                    I += 1;
                }
            }

            if (add)
            {
                if (DrawCount == DrawList.Count)
                {
                    var dirtyRectangle = new DirtyRectangle();
                    DrawList.Add(dirtyRectangle);
                }

                R = DrawList[DrawCount];
                R.Rectangle = rectangle;
                R.Owner = sender;
                DrawCount += 1;
            }
        }
    }

    // AoWComp.pas:262
    public virtual void SetOwner(UIComponent value)
    {
        int I;
        int newIndex;
        int oldIndex;
        if (_ownerComponent != null)
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
        newIndex = -1;
        if (_ownerComponent != null)
        {
            I = 0;
            while (I < _ownerComponent.Children.Count)
            {
                if (_ownerComponent.Children[I]._priority > _priority)
                {
                    newIndex = I;
                    I = _ownerComponent.Children.Count;
                }
                else
                {
                    I += 1;
                }
            }

            if (newIndex == -1)
            {
                _ownerComponent.Children.Add(this);
            }
            else
            {
                _ownerComponent.Children.Insert(newIndex, this);
            }
        }

        if (newIndex != oldIndex)
        {
            if (_doubleBuffered)
            {
                SurfaceValid = false;
            }
            else
            {
                Drawn = false;
            }
        }
    }

    // AoWComp.pas:580
    public virtual void SetWinLeft(int value)
    {
        _winLeft = value;
        Drawn = false;
    }

    // AoWComp.pas:586
    public virtual void SetWinTop(int value)
    {
        _winTop = value;
        Drawn = false;
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
            Drawn = false;
            if (_visible)
            {
                Hidden = false;
                ClearBlitList();
                ClearDrawList();
                if (!Valid || !Aligned || !Sized)
                {
                    Setup();
                }
            }
            else
            {
                Hidden = true;
            }
        }
    }

    // AoWComp.pas:629
    public virtual void SetColor(int value)
    {
        Color = value;
        Drawn = false;
    }

    // AoWComp.pas:635
    public virtual void SetSelectedColor(int value)
    {
        SelectedColor = value;
        Drawn = false;
    }

    // AoWComp.pas:641
    public virtual void DrawSurface(Surface surface)
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
    public virtual void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
    }

    // AoWComp.pas:658
    public virtual void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
    }

    // AoWComp.pas:663
    public virtual void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
    }

    // AoWComp.pas:668
    public virtual void CheckKeyPress(ref byte key)
    {
        Handled = false;
    }

    // AoWComp.pas:673
    public virtual void CheckKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        Handled = false;
    }

    // AoWComp.pas:256
    public void SetAlign(Alignment value)
    {
        _alignment.Assign(value);
        Aligned = false;
    }

    public int Priority { get => _priority; set => SetPriority(value); }

    internal void SetPriorityWithoutReorder(int value) => _priority = value;

    public int WinLeft { get => _winLeft; set => SetWinLeft(value); }

    protected internal void SetWinLeftWithoutInvalidate(int value) => _winLeft = value;

    public int WinTop { get => _winTop; set => SetWinTop(value); }

    protected internal void SetWinTopWithoutInvalidate(int value) => _winTop = value;

    public int WinWidth { get => _winWidth; set => SetWidth(value); }

    protected internal void SetWinWidthWithoutInvalidate(int value) => _winWidth = value;

    public int WinHeight { get => _winHeight; set => SetHeight(value); }

    protected internal void SetWinHeightWithoutInvalidate(int value) => _winHeight = value;

    public int TextColor { get => Color; set => SetColor(value); }
    public int TextSelectedColor { get => SelectedColor; set => SetSelectedColor(value); }
    public UIComponent OwnerComponent { get => _ownerComponent; set => SetOwner(value); }
    public Alignment Alignment { get => _alignment; set => SetAlign(value); }
    public bool Masked { get => _masked; set => _masked = value; }
    public bool CaptureInput { get => _captureInput; set => _captureInput = value; }

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
