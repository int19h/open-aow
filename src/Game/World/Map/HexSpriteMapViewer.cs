#nullable disable
// Translated from HSEngine.pas.
using Aow.Game.Draw;

using Aow.Rtl;
using static Aow.Rtl.LegacyApplication;

namespace Aow.Game.World.Map;

// shows a map on a surface. FStaticSurface (the static scene, kept between frames),
// _sceneWidth, _sceneHeight, _sceneX, _sceneY, _sceneL, _staticSurfaceValid, OnSceneChanged
// (published), FHSMap. SetHSMap points the map's OnCenterView, OnChangeView and OnViewLevel at
// CenterScene, ChangeScene and SetSceneL
public partial class HexSpriteMapViewer : Component
{
    private int _sceneWidth;
    private int _sceneHeight;
    private int _sceneX;
    private int _sceneY;
    private int _sceneL;

    [Published]
    public event NotifyEvent<object> OnSceneChanged;
    private GameMap _hexSpriteMap;

    // Port extension: the scene drawn under the original clamp before ShowCenteredMap centers it.
    internal Surface CenteredSurface { get; private set; }

    // HSEngine.pas:4670
    public HexSpriteMapViewer(Component owner) : base(owner)
    {
        StaticSurfaceValid = false;
        OnSceneChanged = null;
    }

    // HSEngine.pas:4678
    // { Leaves the map's OnChangeView set (original slip). }
    public override void Dispose()
    {
        if (_hexSpriteMap is not null)
        {
            _hexSpriteMap.ClearCenterViewHandler();
            _hexSpriteMap.ClearViewLevelHandler();
        }

        StaticSurface?.Dispose();
        CenteredSurface?.Dispose();
        base.Dispose();
    }

    // HSEngine.pas:4718
    public virtual void SetHexSpriteMap(GameMap value)
    {
        _hexSpriteMap = value;
        if (_hexSpriteMap is not null)
        {
            // { Install viewer callbacks on the assigned map. }
            _hexSpriteMap.SetCenterViewHandler(CenterScene);
            _hexSpriteMap.SetChangeViewHandler(ChangeScene);
            _hexSpriteMap.SetViewLevelHandler(SetSceneL);
        }
    }

    // HSEngine.pas:4794
    public virtual void VerifyStaticSurface(Surface surface)
    {
        if (StaticSurface is not null && StaticSurface.GetPixelFormatId() != surface.GetPixelFormatId())
        {
            StaticSurface?.Dispose();
            StaticSurface = null;
        }

        if (StaticSurface is null || StaticSurface.GetWidth() != _sceneWidth || StaticSurface.GetHeight() != _sceneHeight)
        {
            if (StaticSurface is null)
            {
                PixelFormat pixelFormatId = surface.GetPixelFormatId();
                StaticSurface = new DataSurface(_sceneWidth, _sceneHeight, pixelFormatId, null);
            }
            else
            {
                _ = StaticSurface.Resize(_sceneWidth, _sceneHeight);
            }

            _hexSpriteMap.StaticSceneInvalid = true;
            StaticSurfaceValid = false;
        }
    }

    // HSEngine.pas:4689
    public void ChangeScene(int pixelX, int pixelY, int lHitPoints)
    {
        if (_hexSpriteMap is not null)
        {
            SetSceneX(pixelX);
            SetSceneY(pixelY);
            SetSceneL(lHitPoints);
            if (_hexSpriteMap.SetViewPort(_sceneX, _sceneY, _sceneL, _sceneWidth, _sceneHeight) && OnSceneChanged is not null)
            {
                OnSceneChanged(this);
            }
        }
    }

    // HSEngine.pas:4702
    public void CenterScene(int hexX, int hexY, int hexLevel)
    {
        if (_hexSpriteMap is not null)
        {
            HexGeometry.HexToPixel(hexX, hexY, out int pixelX, out int pixelY);
            SetSceneX(pixelX + 16 - (int)(((uint)_sceneWidth) >> 1));
            SetSceneY(pixelY + 16 - (int)(((uint)_sceneHeight) >> 1));
            SetSceneL(hexLevel);
            if (_hexSpriteMap.SetViewPort(_sceneX, _sceneY, _sceneL, _sceneWidth, _sceneHeight) && OnSceneChanged is not null)
            {
                OnSceneChanged(this);
            }
        }
    }

    // HSEngine.pas:4730
    public void SetSceneL(int value)
    {
        if (_hexSpriteMap is not null)
        {
            if (value < 0)
            {
                value = 0;
            }

            if (value >= _hexSpriteMap.MapContainer.LevelCount)
            {
                value = _hexSpriteMap.MapContainer.LevelCount - 1;
            }

            _sceneL = value;
        }
    }

    // HSEngine.pas:4742
    public void SetSceneX(int value)
    {
        if (_hexSpriteMap is not null)
        {
            if (value < 16)
            {
                value = 16;
            }

            _sceneX = _hexSpriteMap.GetMaxPixelX() < _sceneWidth + value + 16 ? _hexSpriteMap.GetMaxPixelX() - _sceneWidth - 16 : value;
        }
    }

    // HSEngine.pas:4756
    public void SetSceneY(int value)
    {
        if (_hexSpriteMap is not null)
        {
            if (value < 16)
            {
                value = 16;
            }

            _sceneY = _hexSpriteMap.GetMaxPixelY() - 16 < _sceneHeight + value ? _hexSpriteMap.GetMaxPixelY() - _sceneHeight - 16 : value;
        }
    }

    // HSEngine.pas:4770
    public void SetSceneWidth(int value)
    {
        GameMap m = _hexSpriteMap;
        if (m is not null && value > 0)
        {
            _sceneWidth = value;
            SetSceneX(_sceneX);
        }
    }

    // HSEngine.pas:4782
    public void SetSceneHeight(int value)
    {
        GameMap m = _hexSpriteMap;
        if (m is not null && value > 0)
        {
            _sceneHeight = value;
            SetSceneY(_sceneY);
        }
    }

    // HSEngine.pas:4837
    public void ResetStaticSurface()
    {
        StaticSurface?.Dispose();
        StaticSurface = null;
    }

    // HSEngine.pas:4843
    public void ShowScene(int X, int Y, Surface surface)
    {
        Rect r = default;
        Surface target = surface;
        int targetX = X;
        int targetY = Y;
        try
        {
            object mp = _hexSpriteMap;
            if (mp is not null)
            {
                // The field holds the map. The object-typed copy belongs to #150.
                GameMap m = (GameMap)mp;
                if (m.SetViewPort(_sceneX, _sceneY, _sceneL, _sceneWidth, _sceneHeight) && OnSceneChanged is not null)
                {
                    OnSceneChanged(this);
                }

                // Port extension: a scene larger than the map draws as the original does into
                // FCenteredSurface, then ShowCenteredMap centers the map on Target and fills the rest black.
                if (GetSceneExceedsMap())
                {
                    surface = VerifyCenteredSurface(target);
                    X = 0;
                    Y = 0;
                }
                else if (CenteredSurface is not null)
                {
                    CenteredSurface?.Dispose();
                    CenteredSurface = null;
                }

                VerifyStaticSurface(surface);
                r.Left = X;
                r.Right = _sceneWidth + X;
                r.Top = Y;
                r.Bottom = _sceneHeight + Y;
                LockedSurface lockedSurface = default;
                if (m.StaticSceneInvalid)
                {
                    m.UpdateStaticScene();
                    m.UpdateDynamicScene();
                    if (surface.Lock(ref lockedSurface, r) >= 0)
                    {
                        m.DrawStaticScene(in lockedSurface);
                        m.DrawDynamicScene(ref lockedSurface);
                        _ = surface.Unlock(lockedSurface);
                    }

                    StaticSurfaceValid = false;
                }
                else
                {
                    if (StaticSurfaceValid)
                    {
                        _ = StaticSurface.BlitClipped(surface, X, Y);
                        m.UpdateDynamicScene();
                        if (surface.Lock(ref lockedSurface, r) >= 0)
                        {
                            m.DrawDynamicScene(ref lockedSurface);
                            _ = surface.Unlock(lockedSurface);
                        }
                    }
                    else
                    {
                        m.UpdateStaticScene();
                        if (StaticSurface.Lock(ref lockedSurface) >= 0)
                        {
                            m.DrawStaticScene(in lockedSurface);
                            _ = StaticSurface.Unlock();
                            StaticSurfaceValid = true;
                        }

                        if (StaticSurface.BlitClipped(surface, X, Y) >= 0)
                        {
                            m.UpdateDynamicScene();
                            if (surface.Lock(ref lockedSurface, r) >= 0)
                            {
                                m.DrawDynamicScene(ref lockedSurface);
                                _ = surface.Unlock(lockedSurface);
                            }
                        }
                    }
                }

                if (surface != target)
                {
                    ShowCenteredMap(target, targetX, targetY);
                }
            }
        }
        catch
        {
            ShowMessage("Exception during MapViewer.ShowScene");
        }
    }

    // Port extension: the original never showed a scene wider or taller than the map. SceneX and
    // SceneY keep the original clamp there, so the view port and the saved view stay the original's.
    public bool GetSceneExceedsMap() => _hexSpriteMap is not null && (_sceneWidth > _hexSpriteMap.GetMaxPixelX() - 32 || _sceneHeight > _hexSpriteMap.GetMaxPixelY() - 32);

    // Port extension: the part of the scene that shows the map under the original clamp. On an axis
    // where the scene fits inside the map it spans the scene.
    public Rect GetMapSceneRectangle()
    {
        Rect r = default;
        if (_hexSpriteMap is not null)
        {
            r.Left = Math.Max(16 - _sceneX, 0);
            r.Top = Math.Max(16 - _sceneY, 0);
            r.Right = Math.Min(_hexSpriteMap.GetMaxPixelX() - 16 - _sceneX, _sceneWidth);
            r.Bottom = Math.Min(_hexSpriteMap.GetMaxPixelY() - 16 - _sceneY, _sceneHeight);
        }

        return r;
    }

    // Port extension: where ShowScene shows the map, centered in the scene. It equals
    // GetMapSceneRect when the scene fits inside the map.
    public Rect GetMapDisplayRectangle()
    {
        Rect r = GetMapSceneRectangle();
        int width = r.Right - r.Left;
        int height = r.Bottom - r.Top;
        r.Left = (_sceneWidth - width) >> 1;
        r.Top = (_sceneHeight - height) >> 1;
        r.Right = r.Left + width;
        r.Bottom = r.Top + height;
        return r;
    }

    // Port extension: turns a point of the shown scene into the scene point of the original clamp,
    // which the map mouse takes. A point outside the centered map gives false.
    public bool SceneToMapMouse(ref int X, ref int Y)
    {
        if (!GetSceneExceedsMap())
        {
            return true;
        }

        Rect shown = GetMapDisplayRectangle();
        if (X < shown.Left || X >= shown.Right || Y < shown.Top || Y >= shown.Bottom)
        {
            return false;
        }

        Rect source = GetMapSceneRectangle();
        X += source.Left - shown.Left;
        Y += source.Top - shown.Top;
        return true;
    }

    // Port extension: the scene-sized surface that ShowScene draws on when the scene exceeds the map.
    public Surface VerifyCenteredSurface(Surface surface)
    {
        if (CenteredSurface is not null && CenteredSurface.GetPixelFormatId() != surface.GetPixelFormatId())
        {
            CenteredSurface?.Dispose();
            CenteredSurface = null;
        }

        if (CenteredSurface is null)
        {
            CenteredSurface = new DataSurface(_sceneWidth, _sceneHeight, surface.GetPixelFormatId(), null);
        }
        else if (CenteredSurface.GetWidth() != _sceneWidth || CenteredSurface.GetHeight() != _sceneHeight)
        {
            _ = CenteredSurface.Resize(_sceneWidth, _sceneHeight);
        }

        return CenteredSurface;
    }

    // Port extension: fills the scene outside the centered map black and copies the map part of
    // FCenteredSurface into the centered rectangle.
    public void ShowCenteredMap(Surface target, int X, int Y)
    {
        LockedSurface lockedSurface = default;
        Rect source = GetMapSceneRectangle();
        Rect shown = GetMapDisplayRectangle();
        if (target.Lock(ref lockedSurface, new Rect(X, Y, X + _sceneWidth, Y + _sceneHeight)) >= 0)
        {
            FillOutsideRectangle(shown, lockedSurface);
            _ = target.Unlock(lockedSurface);
        }

        _ = CenteredSurface.BlitClipped(target, source, X + shown.Left, Y + shown.Top);
    }

    // Port extension: fills every pixel of SD outside Inside black.
    public static void FillOutsideRectangle(Rect inside, in LockedSurface lockedSurface)
    {
        _ = PixelFormats.ColorFillSurfaceDescription(new Rect(0, 0, lockedSurface.Width, inside.Top), 0, lockedSurface);
        _ = PixelFormats.ColorFillSurfaceDescription(new Rect(0, inside.Bottom, lockedSurface.Width, lockedSurface.Height), 0, lockedSurface);
        _ = PixelFormats.ColorFillSurfaceDescription(new Rect(0, inside.Top, inside.Left, inside.Bottom), 0, lockedSurface);
        _ = PixelFormats.ColorFillSurfaceDescription(new Rect(inside.Right, inside.Top, lockedSurface.Width, inside.Bottom), 0, lockedSurface);
    }

    public GameMap HexSpriteMap { get => _hexSpriteMap; set => SetHexSpriteMap(value); }
    public Surface StaticSurface { get; private set; }
    public bool StaticSurfaceValid { get; set; }
    public int SceneX { get => _sceneX; set => SetSceneX(value); }
    public int SceneY { get => _sceneY; set => SetSceneY(value); }
    public int SceneL { get => _sceneL; set => SetSceneL(value); }
    public int SceneWidth { get => _sceneWidth; set => SetSceneWidth(value); }
    public int SceneHeight { get => _sceneHeight; set => SetSceneHeight(value); }
}
