using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public partial class Image
{
    // FastBattleWindow.GlowAnimation is a plain TImageLibrary in the recovered form.
    private ImageLibrary _imageLibrary;

    [Published]
    public ImageLibrary ImageLibrary { get => _imageLibrary; set => SetImageLibrary(value); }

    public void SetImageLibrary(ImageLibrary value)
    {
        if (value == _imageLibrary) return;
        Valid = false;
        _imageLibrary = value;
    }

    public override void Validate()
    {
        if (_imageLibrary != null)
            _imageLibrary = _animate
                ? _imageLibrary.CheckImageLibrary(_animationStart, _animationEnd + 1 - _animationStart, this)
                : _imageLibrary.CheckImageLibrary(_currentImage, 1, this);
        if (_imageLibrary != null) Valid = true;
        base.Validate();
    }

    public override void Draw(LockedSurface surface)
    {
        try
        {
            if (!Valid || !Visible) return;
            RaisePreDraw(this, surface);
            var rectangle = new Rect(Window.WinLeft + WinLeft,
                Window.WinTop + WinTop, WinWidth, WinHeight);
            _imageLibrary.DrawTiled(_currentImage, rectangle, ref surface);
            RaiseDraw(this, surface);
        }
        catch
        {
            RtlUnit.MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }
}
