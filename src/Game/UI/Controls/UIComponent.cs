// Translated from AoWComp.pas.
using System.ComponentModel;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// Fields: Color, SelectedColor, Valid (Validate found everything it needs), Aligned, Sized,
// _cursor, _onDraw, _onPreDraw, _onMouseDown, _onMouseUp, _onMouseMove, _onKeyPress,
// FOnKeyDown, FOnChange, FMouseDown, FVisible, FHandled (a Check* method consumed the input),
// _winWidth, _winHeight, _winLeft, _winTop, _font, _text, Redraw, _borderWidth,
// FBorderHeight, FPriority, FDrawn (the component is drawn as it is now), FFirstDraw, FMouseIn,
// ToolTipCount, _radioIndex, _checked, _ownerItem, _onUpdate, _captureInput, _onDebugMessage,
// _ownerComponent, Children (sorted by _priority), _alignment, _masked, BlitList, DrawList
// (DirtyRectangle), BlitCount, DrawCount, ToolTipLeft, OldRectangle, Hidden, _doubleBuffered,
// SurfaceValid
public partial class UIComponent : Rtl.Component
{
    public int Color;
    public int SelectedColor;
    public bool Valid;
    public bool Aligned;
    public bool Sized;
    public int _cursor;
    public AoWOnDrawEvent<UIComponent> _onDraw;
    public AoWOnDrawEvent<UIComponent> _onPreDraw;
    public MouseEvent<UIComponent> _onMouseDown;
    public MouseEvent<UIComponent> _onMouseUp;
    public MouseMoveEvent<UIComponent> _onMouseMove;
    public KeyPressEvent<UIComponent> _onKeyPress;
    public KeyEvent<UIComponent> _onKeyDown;
    public NotifyEvent<UIComponent> _onChange;
    public bool MouseDown;
    public bool _visible;
    public bool Handled;
    public int _winWidth;
    public int _winHeight;
    public int _winLeft;
    public int _winTop;
    public ImageLibraryFont _font;
    public string _text = "";
    public bool Redraw;
    public int _borderWidth;
    public int _borderHeight;
    public int _priority;
    public bool Drawn;
    public bool FirstDraw;
    public bool MouseIn;
    public int ToolTipCount;
    public int _radioIndex;
    public bool _checked;
    public int _ownerItem;
    public NotifyEvent<UIComponent> _onUpdate;
    public bool _captureInput;
    public AoWOnDebugEvent _onDebugMessage;
    public UIComponent _ownerComponent;
    public DelphiList Children;
    public Alignment _alignment;
    public bool _masked;
    public List<DirtyRectangle> BlitList;
    public List<DirtyRectangle> DrawList;
    public int BlitCount;
    public int DrawCount;
    public int ToolTipLeft;
    public Rect OldRectangle;
    public bool Hidden;
    public bool _doubleBuffered;
    public bool SurfaceValid;

    // AoWComp.pas:222
    public UIComponent(Rtl.Component owner) : base(owner)
    {
        _visible = true;
        FirstDraw = true;
        _alignment = new Alignment();
        Children = new DelphiList();
        BlitList = new List<DirtyRectangle>();
        DrawList = new List<DirtyRectangle>();
    }

    // AoWComp.pas:233
    public override void Dispose()
    {
        SetOwner(null);
        while (Children.Count > 0)
        {
            ((UIComponent)Children[0]).SetOwner(null);
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
            ((UIComponent)Children[I]).Valid = false;
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
            ((UIComponent)Children[I]).Aligned = false;
        }
    }

    // AoWComp.pas:690
    public virtual void ReAlign()
    {
        if (_ownerComponent != null && (_alignment._alignWidth != AlignWidth.None || _alignment._alignHeight != AlignHeight.None))
        {
            switch (this._alignment._alignWidth)
            {
                case AlignWidth.Left:
                    _winLeft = _ownerComponent._winWidth * this._alignment._leftOffsetPercent / 100 + this._alignment._leftOffset;
                    if (_alignment._widthPercent > 0)
                    {
                        _winWidth = _ownerComponent._winWidth * _alignment._widthPercent / 100;
                    }

                    break;
                case AlignWidth.Right:
                    if (this._alignment._widthPercent > 0)
                    {
                        _winWidth = _ownerComponent._winWidth * this._alignment._widthPercent / 100;
                    }

                    _winLeft = _ownerComponent._winWidth - _alignment._rightOffset - _winWidth - _ownerComponent._winWidth * _alignment._rightOffsetPercent / 100;
                    break;
                case AlignWidth.Both:
                    _winLeft = _ownerComponent._winWidth * this._alignment._leftOffsetPercent / 100 + this._alignment._leftOffset;
                    _winWidth = _ownerComponent._winWidth - _winLeft - _alignment._rightOffset - _ownerComponent._winWidth * _alignment._rightOffsetPercent / 100;
                    break;
                case AlignWidth.Center:
                    _winLeft = _ownerComponent._winWidth / 2 - _winWidth / 2;
                    break;
            }

            switch (this._alignment._alignHeight)
            {
                case AlignHeight.Top:
                    _winTop = _ownerComponent._winHeight * this._alignment._topOffsetPercent / 100 + this._alignment._topOffset;
                    if (_alignment._heightPercent > 0)
                    {
                        _winHeight = _ownerComponent._winHeight * _alignment._heightPercent / 100;
                    }

                    break;
                case AlignHeight.Bottom:
                    if (this._alignment._heightPercent > 0)
                    {
                        _winHeight = _ownerComponent._winHeight * this._alignment._heightPercent / 100;
                    }

                    _winTop = _ownerComponent._winHeight - _alignment._bottomOffset - _winHeight - _ownerComponent._winHeight * _alignment._bottomOffsetPercent / 100;
                    break;
                case AlignHeight.Both:
                    _winTop = _ownerComponent._winHeight * this._alignment._topOffsetPercent / 100 + this._alignment._topOffset;
                    _winHeight = _ownerComponent._winHeight - _winTop - _alignment._bottomOffset - _ownerComponent._winHeight * _alignment._bottomOffsetPercent / 100;
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
            ((UIComponent)Children[I]).Aligned = false;
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
                        if (((UIComponent)Children[I])._visible)
                        {
                            ((UIComponent)Children[I]).Setup();
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
                if (_onUpdate != null)
                {
                    _onUpdate(this);
                }

                for (int I = Children.Count - 1; I >= 0; I--)
                {
                    ((UIComponent)Children[I]).Update();
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
            if (((UIComponent)Children[I])._visible)
            {
                ((UIComponent)Children[I]).ClearDrawList();
            }
        }
    }

    // AoWComp.pas:212
    public virtual void ClearBlitList()
    {
        BlitCount = 0;
        for (int I = 0, last = Children.Count - 1; I <= last; I++)
        {
            if (((UIComponent)Children[I])._visible)
            {
                ((UIComponent)Children[I]).ClearBlitList();
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
                if (((UIComponent)_ownerComponent.Children[I])._priority > _priority)
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
    public int WinLeft { get => _winLeft; set => SetWinLeft(value); }
    public int WinTop { get => _winTop; set => SetWinTop(value); }
    public int WinWidth { get => _winWidth; set => SetWidth(value); }
    public int WinHeight { get => _winHeight; set => SetHeight(value); }
    public int TextColor { get => Color; set => SetColor(value); }
    public int TextSelectedColor { get => SelectedColor; set => SetSelectedColor(value); }
    public UIComponent OwnerComponent { get => _ownerComponent; set => SetOwner(value); }
    public Alignment Alignment { get => _alignment; set => SetAlign(value); }
    public bool Masked { get => _masked; set => _masked = value; }
    public bool CaptureInput { get => _captureInput; set => _captureInput = value; }
    public NotifyEvent<UIComponent> OnChange { get => _onChange; set => _onChange = value; }

    [Published]
    public MouseEvent<UIComponent> OnMouseDown { get => _onMouseDown; set => _onMouseDown = value; }

    [Published]
    public MouseEvent<UIComponent> OnMouseUp { get => _onMouseUp; set => _onMouseUp = value; }

    [Published]
    public MouseMoveEvent<UIComponent> OnMouseMove { get => _onMouseMove; set => _onMouseMove = value; }

    [Published]
    public KeyPressEvent<UIComponent> OnKeyPress { get => _onKeyPress; set => _onKeyPress = value; }

    [Published]
    public KeyEvent<UIComponent> OnKeyDown { get => _onKeyDown; set => _onKeyDown = value; }

    [Published]
    public AoWOnDrawEvent<UIComponent> OnPreDraw { get => _onPreDraw; set => _onPreDraw = value; }

    [Published]
    public AoWOnDrawEvent<UIComponent> OnDraw { get => _onDraw; set => _onDraw = value; }

    [Published]
    public NotifyEvent<UIComponent> OnUpdate { get => _onUpdate; set => _onUpdate = value; }

    [Published]
    [DefaultValue(true)]
    public bool Visible { get => _visible; set => SetVisible(value); }

    [Published]
    public AoWOnDebugEvent OnDebugMessage { get => _onDebugMessage; set => _onDebugMessage = value; }

    public delegate void AoWOnDebugEvent(string Dtext);
}

public enum UIComponentButtonType : byte
{
    Text = 0, Icon = 1
}
