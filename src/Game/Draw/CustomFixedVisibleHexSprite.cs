// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Draw;

// FShowPriority, FOnShow (Show calls it). TAbstractAoWHSMap (AoWEPACK) creates two and sets
// OnShow and ShowPriority
public partial class CustomFixedVisibleHexSprite : FixedVisibleHexSprite
{
    public CustomFixedVisibleHexSprite(ECustomNode owner) : base(owner)
    { }

    internal int FShowPriority;
    internal OnShowHSEvent FOnShow;

    // HXSPRITE.PAS:800
    public override int GetShowPriority() => FShowPriority;

    // HXSPRITE.PAS:805
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        if (FOnShow != null)
        {
            FOnShow(XhpSurf, YhpSurf, SurfaceDesc);
        }
    }

    public new int ShowPriority { get => GetShowPriority(); set => FShowPriority = value; }
    public OnShowHSEvent OnShow { get => FOnShow; set => FOnShow = value; }

    public delegate void OnShowHSEvent(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc);

}
