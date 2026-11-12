// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Draw;

// FShowPriority, FOnShow (Show calls it). TAbstractAoWHSMap (AoWEPACK) creates two and sets
// OnShow and ShowPriority
public partial class CustomFixedVisibleHexSprite : FixedVisibleHexSprite
{
    public CustomFixedVisibleHexSprite(ECustomNode owner) : base(owner)
    { }

    private int _showPriority;
    public event OnShowHSEvent OnShow;

    // HXSPRITE.PAS:800
    public override int GetShowPriority() => _showPriority;

    // HXSPRITE.PAS:805
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (OnShow != null)
        {
            OnShow(pixelXSurface, pixelYSurface, lockedSurface);
        }
    }

    public new int ShowPriority { get => GetShowPriority(); set => _showPriority = value; }

    public delegate void OnShowHSEvent(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc);

}
