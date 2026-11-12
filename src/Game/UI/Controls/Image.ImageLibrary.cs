using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public partial class Image
{
    // FastBattleWindow.GlowAnimation is a plain TImageLibrary in the recovered form.
    public ImageLibrary _imageLibrary;

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
            if (!Valid || !_visible) return;
            _onPreDraw?.Invoke(this, surface);
            var rectangle = new Rect(_window._winLeft + _winLeft,
                _window._winTop + _winTop, _winWidth, _winHeight);
            _imageLibrary.DrawTiled(_currentImage, rectangle, ref surface);
            _onDraw?.Invoke(this, surface);
        }
        catch
        {
            RtlUnit.MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }
}
