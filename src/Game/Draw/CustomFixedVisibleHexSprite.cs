// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Draw;

// FShowPriority, FOnShow (Show calls it). TAbstractAoWHSMap (AoWEPACK) creates two and sets
// OnShow and ShowPriority
public partial class CustomFixedVisibleHexSprite : FixedVisibleHexSprite
{
    public CustomFixedVisibleHexSprite(ECustomNode owner) : base(owner)
    { }

    internal int _showPriority;
    internal OnShowHSEvent _onShow;

    // HXSPRITE.PAS:800
    public override int GetShowPriority() => _showPriority;

    // HXSPRITE.PAS:805
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (_onShow != null)
        {
            _onShow(pixelXSurface, pixelYSurface, lockedSurface);
        }
    }

    public new int ShowPriority { get => GetShowPriority(); set => _showPriority = value; }
    public OnShowHSEvent OnShow { get => _onShow; set => _onShow = value; }

    public delegate void OnShowHSEvent(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc);

}
