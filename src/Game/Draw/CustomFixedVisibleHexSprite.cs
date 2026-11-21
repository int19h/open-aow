#nullable disable
// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Draw;

// FShowPriority, FOnShow (Show calls it). TAbstractAoWHSMap (AoWEPACK) creates two and sets
// OnShow and ShowPriority
public sealed class CustomFixedVisibleHexSprite(CustomNode owner) : FixedVisibleHexSprite(owner)
{
    private int _showPriority;
    private OnShowHSEvent _show;

    internal void SetShowHandler(OnShowHSEvent handler) => _show = handler;

    // HXSPRITE.PAS:800
    public override int GetShowPriority() => _showPriority;

    // HXSPRITE.PAS:805
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface) => _show?.Invoke(pixelXSurface, pixelYSurface, SurfaceDesc: lockedSurface);

    public new int ShowPriority { get => GetShowPriority(); set => _showPriority = value; }

    public delegate void OnShowHSEvent(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc);

}
