using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public partial class Memo
{
    // AoWMemo.pas:511. The editable-byte comparison tests exactly one after the bounds checks.
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int x, int y)
    {
        Handled = false;
        MouseDown = false;
        bool old = Redraw;
        Redraw = false;
        if (_editable && _items.Count == 0)
        {
            _items.Add("");
            Lines.Add("");
            ColorList.Add(new ColorChange());
        }
        if (x > Window.WinLeft + WinLeft - 1 && x < Window.WinLeft + WinLeft + WinWidth &&
            y > Window.WinTop + WinTop - 1 && y < Window.WinTop + WinTop + WinHeight)
        {
            int left, top, bottom;
            if (!NoFrame)
            {
                left = Window.WinLeft + WinLeft + _imageLibraryFrame[_imageLibraryIndexFrame + 3].Width - 1;
                top = Window.WinTop + WinTop + _borderHeight + _imageLibraryFrame[_imageLibraryIndexFrame + 1].Height - 1;
                bottom = Window.WinTop + WinTop + WinHeight - _imageLibraryFrame[_imageLibraryIndexFrame + 6].Height;
            }
            else
            {
                left = Window.WinLeft + WinLeft - 1;
                top = Window.WinTop + WinTop + _borderHeight - 1;
                bottom = Window.WinTop + WinTop + WinHeight;
            }
            if (_editable && x > left && x < Window.WinLeft + WinLeft + WinWidth - _scrollWidth && y > top && y < bottom && button == MouseButton.Left)
            {
                if (_editable)
                {
                    Redraw = true;
                    _blink = true;
                    _blinkCount = 0;
                    Handled = true;
                    Drawn = false;
                    if (NoFrame && Window != null) Window.Drawn = false;
                }
                if ((Lines.Count - _topItem) * _itemHeight + top < y)
                {
                    _currentLine = Lines.Count - 1;
                    _currentColumn = Lines[Lines.Count - 1].Length;
                    _realLine = _items.Count - 1;
                    _realColumn = _items[_items.Count - 1].Length;
                }
                else
                {
                    int length = _items[0].Length;
                    _realColumn = _realLine = _currentColumn = _currentLine = 0;
                    while (_currentLine < _topItem)
                    {
                        _realColumn += Lines[_currentLine].Length;
                        if (_realColumn >= length)
                        {
                            _realColumn = 0;
                            _realLine++;
                            length = _items[_realLine].Length;
                        }
                        _currentLine++;
                    }
                    while (Lines.Count > _currentLine && (_currentLine - _topItem + 1) * _itemHeight + top < y)
                    {
                        _realColumn += Lines[_currentLine].Length;
                        if (_realColumn >= length)
                        {
                            _realColumn = 0;
                            _realLine++;
                            length = _items[_realLine].Length;
                        }
                        _currentLine++;
                    }
                    if (Lines.Count == _currentLine) _currentLine--;
                    while (Lines[_currentLine].Length > _currentColumn && _font.GetTextWidth(RtlUnit.Copy(Lines[_currentLine], 1, _currentColumn + 1)) + left < x)
                    {
                        _currentColumn++;
                        _realColumn++;
                    }
                }
            }
            RaiseMouseUp(this, button, shift, x, y);
        }
        if (Redraw != old) Drawn = false;
    }
}
