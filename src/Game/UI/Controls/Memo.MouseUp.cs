using System.Runtime.CompilerServices;
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public partial class Memo
{
    // AoWMemo.pas:511. The editable-byte comparison tests exactly one after the bounds checks.
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int x, int y)
    {
        Handled = false;
        MouseDown = false;
        UnusedMouseUpValue = 0;
        bool old = Redraw;
        Redraw = false;
        if (_editable && _items.Count == 0)
        {
            _items.Add("");
            Lines.Add("");
            ColorList.Add(new ColorChange());
        }
        if (x > _window._winLeft + _winLeft - 1 && x < _window._winLeft + _winLeft + _winWidth &&
            y > _window._winTop + _winTop - 1 && y < _window._winTop + _winTop + _winHeight)
        {
            int left, top, bottom;
            if (!NoFrame)
            {
                left = _window._winLeft + _winLeft + _imageLibraryFrame[_imageLibraryIndexFrame + 3].Width - 1;
                top = _window._winTop + _winTop + _borderHeight + _imageLibraryFrame[_imageLibraryIndexFrame + 1].Height - 1;
                bottom = _window._winTop + _winTop + _winHeight - _imageLibraryFrame[_imageLibraryIndexFrame + 6].Height;
            }
            else
            {
                left = _window._winLeft + _winLeft - 1;
                top = _window._winTop + _winTop + _borderHeight - 1;
                bottom = _window._winTop + _winTop + _winHeight;
            }
            if (_editable && x > left && x < _window._winLeft + _winLeft + _winWidth - ScrollWidth && y > top && y < bottom && button == MouseButton.Left)
            {
                if (Unsafe.As<bool, byte>(ref _editable) == 1)
                {
                    Redraw = true;
                    Blink = true;
                    BlinkCount = 0;
                    Handled = true;
                    Drawn = false;
                    if (NoFrame && _window != null) _window.Drawn = false;
                }
                if ((Lines.Count - TopItem) * _itemHeight + top < y)
                {
                    CurrentLine = Lines.Count - 1;
                    CurrentColumn = Lines[Lines.Count - 1].Length;
                    RealLine = _items.Count - 1;
                    RealColumn = _items[_items.Count - 1].Length;
                }
                else
                {
                    int length = _items[0].Length;
                    RealColumn = RealLine = CurrentColumn = CurrentLine = 0;
                    while (CurrentLine < TopItem)
                    {
                        RealColumn += Lines[CurrentLine].Length;
                        if (RealColumn >= length)
                        {
                            RealColumn = 0;
                            RealLine++;
                            length = _items[RealLine].Length;
                        }
                        CurrentLine++;
                    }
                    while (Lines.Count > CurrentLine && (CurrentLine - TopItem + 1) * _itemHeight + top < y)
                    {
                        RealColumn += Lines[CurrentLine].Length;
                        if (RealColumn >= length)
                        {
                            RealColumn = 0;
                            RealLine++;
                            length = _items[RealLine].Length;
                        }
                        CurrentLine++;
                    }
                    if (Lines.Count == CurrentLine) CurrentLine--;
                    while (Lines[CurrentLine].Length > CurrentColumn && _font.GetTextWidth(RtlUnit.Copy(Lines[CurrentLine], 1, CurrentColumn + 1)) + left < x)
                    {
                        CurrentColumn++;
                        RealColumn++;
                    }
                }
            }
            _onMouseUp?.Invoke(this, button, shift, x, y);
        }
        if (Redraw != old) Drawn = false;
    }
}
