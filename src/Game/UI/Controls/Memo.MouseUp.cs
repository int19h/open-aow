#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public sealed partial class Memo
{
    // AoWMemo.pas:511. The editable-byte comparison tests exactly one after the bounds checks.
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int x, int y)
    {
        Handled = false;
        IsMouseDown = false;
        bool old = Redraw;
        Redraw = false;
        if (Editable && _items.Count == 0)
        {
            _ = _items.Add("");
            _ = Lines.Add("");
            ColorList.Add(new ColorChange());
        }
        if (x > Window.AbsoluteLeft + WinLeft - 1 && x < Window.AbsoluteLeft + WinLeft + WinWidth &&
            y > Window.AbsoluteTop + WinTop - 1 && y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            int left, top, bottom;
            if (!NoFrame)
            {
                left = Window.AbsoluteLeft + WinLeft + _imageLibraryFrame[_imageLibraryIndexFrame + 3].Width - 1;
                top = Window.AbsoluteTop + WinTop + BorderHeight + _imageLibraryFrame[_imageLibraryIndexFrame + 1].Height - 1;
                bottom = Window.AbsoluteTop + WinTop + WinHeight - _imageLibraryFrame[_imageLibraryIndexFrame + 6].Height;
            }
            else
            {
                left = Window.AbsoluteLeft + WinLeft - 1;
                top = Window.AbsoluteTop + WinTop + BorderHeight - 1;
                bottom = Window.AbsoluteTop + WinTop + WinHeight;
            }
            if (Editable && x > left && x < Window.AbsoluteLeft + WinLeft + WinWidth - _scrollWidth && y > top && y < bottom && button == MouseButton.Left)
            {
                if (Editable)
                {
                    Redraw = true;
                    _blink = true;
                    _blinkCount = 0;
                    Handled = true;
                    IsDrawn = false;
                    if (NoFrame && Window is not null)
                    {
                        Window.IsDrawn = false;
                    }
                }
                if (((Lines.Count - _topItem) * ItemHeight) + top < y)
                {
                    _currentLine = Lines.Count - 1;
                    _currentColumn = Lines[^1].Length;
                    _realLine = _items.Count - 1;
                    _realColumn = _items[^1].Length;
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
                    while (Lines.Count > _currentLine && ((_currentLine - _topItem + 1) * ItemHeight) + top < y)
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
                    if (Lines.Count == _currentLine)
                    {
                        _currentLine--;
                    }

                    while (Lines[_currentLine].Length > _currentColumn && Font.GetTextWidth(DelphiStrings.Copy(Lines[_currentLine], index: 1, count: _currentColumn + 1)) + left < x)
                    {
                        _currentColumn++;
                        _realColumn++;
                    }
                }
            }
            RaiseMouseUp(button, shift, x, y);
        }
        if (Redraw != old)
        {
            IsDrawn = false;
        }
    }
}
