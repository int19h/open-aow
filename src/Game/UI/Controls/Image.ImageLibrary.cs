using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public sealed partial class Image
{
    // FastBattleWindow.GlowAnimation is a plain TImageLibrary in the recovered form.
    private ImageLibrary _imageLibrary = null!;

    [Published]
    public ImageLibrary ImageLibrary { get => _imageLibrary; set => SetImageLibrary(value); }

    public void SetImageLibrary(ImageLibrary value)
    {
        if (value == _imageLibrary)
        {
            return;
        }

        Valid = false;
        _imageLibrary = value;
    }

    public override void Validate()
    {
        if (_imageLibrary is not null)
        {
            _imageLibrary = _animate
                ? _imageLibrary.CheckImageLibrary(_animationStart, count: _animationEnd + 1 - _animationStart, sender: this)
                : _imageLibrary.CheckImageLibrary(_currentImage, count: 1, sender: this);
        }

        if (_imageLibrary is not null)
        {
            Valid = true;
        }

        base.Validate();
    }

    public override void Draw(LockedSurface surface)
    {
        try
        {
            if (!Valid || !Visible)
            {
                return;
            }

            RaisePreDraw(this, surfDesc: surface);
            SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft,
                Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
            _imageLibrary.DrawTiled(_currentImage, area: rectangle, lockedSurface: ref surface);
            RaiseDraw(this, surfDesc: surface);
        }
        catch
        {
            _ = RtlHost.MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }
}
