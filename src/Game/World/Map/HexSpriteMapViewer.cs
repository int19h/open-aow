// Translated from HSEngine.pas.
using Aow.Game.Draw;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

// shows a map on a surface. FStaticSurface (the static scene, kept between frames),
// _sceneWidth, _sceneHeight, _sceneX, _sceneY, _sceneL, _staticSurfaceValid, _onSceneChanged
// (published), FHSMap. SetHSMap points the map's OnCenterView, OnChangeView and OnViewLevel at
// CenterScene, ChangeScene and SetSceneL
public partial class HexSpriteMapViewer : Component
{
    private Surface _staticSurface;
    private int _sceneWidth;
    private int _sceneHeight;
    private int _sceneX;
    private int _sceneY;
    private int _sceneL;
    private bool _staticSurfaceValid;
    private NotifyEvent<object> _onSceneChanged;
    private GameMap _hexSpriteMap;

    // Port extension: the scene drawn under the original clamp before ShowCenteredMap centers it.
    internal Surface CenteredSurface { get; private set; }

    // HSEngine.pas:4670
    public HexSpriteMapViewer(Component owner) : base(owner)
    {
        _staticSurfaceValid = false;
        _onSceneChanged = null;
    }

    // HSEngine.pas:4678
    // { Leaves the map's OnChangeView set (original slip). }
    public override void Dispose()
    {
        if (_hexSpriteMap != null)
        {
            _hexSpriteMap.OnCenterView = null;
            _hexSpriteMap.OnViewLevel = null;
        }

        _staticSurface?.Dispose();
        CenteredSurface?.Dispose();
        base.Dispose();
    }

    // HSEngine.pas:4718
    public virtual void SetHexSpriteMap(GameMap value)
    {
        _hexSpriteMap = value;
        if (_hexSpriteMap != null)
        {
            // { Install viewer callbacks on the assigned map. }
            _hexSpriteMap.OnCenterView = CenterScene;
            _hexSpriteMap.OnChangeView = ChangeScene;
            _hexSpriteMap.OnViewLevel = SetSceneL;
        }
    }

    // HSEngine.pas:4794
    public virtual void VerifyStaticSurface(Surface surface)
    {
        bool isDDraw;
        Set<DirectDrawSurfaceCreationFlagsElement> flags;
        isDDraw = _staticSurface is DirectDrawSurface;
        if (_staticSurface != null && (_staticSurface.GetPixelFormatId() != surface.GetPixelFormatId() || (surface is DirectDrawSurface) ^ isDDraw))
        {
            _staticSurface?.Dispose();
            _staticSurface = null;
        }

        if (_staticSurface == null || _staticSurface.GetWidth() != _sceneWidth || _staticSurface.GetHeight() != _sceneHeight)
        {
            if (_staticSurface == null)
            {
                if (surface is DirectDrawSurface)
                {
                    if (((DirectDrawSurface)surface).Status.Contains(DirectDrawSurfaceStatusElement.VideoMemory))
                    {
                        flags = Set<DirectDrawSurfaceCreationFlagsElement>.Of(DirectDrawSurfaceCreationFlagsElement.VideoMemory, DirectDrawSurfaceCreationFlagsElement.SystemMemory);
                    }
                    else
                    {
                        flags = Set<DirectDrawSurfaceCreationFlagsElement>.Of(DirectDrawSurfaceCreationFlagsElement.SystemMemory);
                    }

                    _staticSurface = new DirectDrawSurface();
                    var pixelFormatId = surface.GetPixelFormatId();
                    ((DirectDrawSurface)_staticSurface).CreateSurface(_sceneWidth, _sceneHeight, pixelFormatId, flags);
                }
                else
                {
                    var pixelFormatId = surface.GetPixelFormatId();
                    _staticSurface = new DataSurface(_sceneWidth, _sceneHeight, pixelFormatId, null);
                }
            }
            else
            {
                _staticSurface.Resize(_sceneWidth, _sceneHeight);
            }

            isDDraw = _staticSurface is DirectDrawSurface;
            _hexSpriteMap.StaticSceneInvalid = true;
            _staticSurfaceValid = false;
        }

        if (isDDraw && ((DirectDrawSurface)_staticSurface).Lost())
        {
            ((DirectDrawSurface)_staticSurface).Restore();
            _staticSurfaceValid = false;
        }
    }

    // HSEngine.pas:4689
    public void ChangeScene(int pixelX, int pixelY, int lHitPoints)
    {
        if (_hexSpriteMap != null)
        {
            SetSceneX(pixelX);
            SetSceneY(pixelY);
            SetSceneL(lHitPoints);
            if (_hexSpriteMap.SetViewPort(_sceneX, _sceneY, _sceneL, _sceneWidth, _sceneHeight) && _onSceneChanged != null)
            {
                _onSceneChanged(this);
            }
        }
    }

    // HSEngine.pas:4702
    public void CenterScene(int hexX, int hexY, int hexLevel)
    {
        int pixelX = default;
        int pixelY = default;
        if (_hexSpriteMap != null)
        {
            HexGeometry.HexToPixel(hexX, hexY, ref pixelX, ref pixelY);
            SetSceneX(pixelX + 16 - (int)(((uint)_sceneWidth) >> 1));
            SetSceneY(pixelY + 16 - (int)(((uint)_sceneHeight) >> 1));
            SetSceneL(hexLevel);
            if (_hexSpriteMap.SetViewPort(_sceneX, _sceneY, _sceneL, _sceneWidth, _sceneHeight) && _onSceneChanged != null)
            {
                _onSceneChanged(this);
            }
        }
    }

    // HSEngine.pas:4730
    public void SetSceneL(int value)
    {
        if (_hexSpriteMap != null)
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
        if (_hexSpriteMap != null)
        {
            if (value < 16)
            {
                value = 16;
            }

            if (this._hexSpriteMap.GetMaxPixelX() < _sceneWidth + value + 16)
            {
                this._sceneX = this._hexSpriteMap.GetMaxPixelX() - _sceneWidth - 16;
            }
            else
            {
                this._sceneX = value;
            }
        }
    }

    // HSEngine.pas:4756
    public void SetSceneY(int value)
    {
        if (_hexSpriteMap != null)
        {
            if (value < 16)
            {
                value = 16;
            }

            if (this._hexSpriteMap.GetMaxPixelY() - 16 < _sceneHeight + value)
            {
                this._sceneY = this._hexSpriteMap.GetMaxPixelY() - _sceneHeight - 16;
            }
            else
            {
                this._sceneY = value;
            }
        }
    }

    // HSEngine.pas:4770
    public void SetSceneWidth(int value)
    {
        GameMap M;
        M = _hexSpriteMap;
        if (M != null && value > 0)
        {
            _sceneWidth = value;
            SetSceneX(_sceneX);
        }
    }

    // HSEngine.pas:4782
    public void SetSceneHeight(int value)
    {
        GameMap M;
        M = _hexSpriteMap;
        if (M != null && value > 0)
        {
            _sceneHeight = value;
            SetSceneY(_sceneY);
        }
    }

    // HSEngine.pas:4837
    public void ResetStaticSurface()
    {
        _staticSurface?.Dispose();
        _staticSurface = null;
    }

    // HSEngine.pas:4843
    public void ShowScene(int X, int Y, Surface surface)
    {
        GameMap M;
        object mp;
        LockedSurface lockedSurface = default;
        Rect R = default;
        Surface target = surface;
        int targetX = X;
        int targetY = Y;
        try
        {
            mp = _hexSpriteMap;
            if (mp != null)
            {
                M = (GameMap)mp;
                if (M.SetViewPort(_sceneX, _sceneY, _sceneL, _sceneWidth, _sceneHeight) && _onSceneChanged != null)
                {
                    _onSceneChanged(this);
                }

                // Port extension: a scene larger than the map draws as the original does into
                // FCenteredSurface, then ShowCenteredMap centers the map on Target and fills the rest black.
                if (GetSceneExceedsMap())
                {
                    surface = VerifyCenteredSurface(target);
                    X = 0;
                    Y = 0;
                }
                else if (CenteredSurface != null)
                {
                    CenteredSurface?.Dispose();
                    CenteredSurface = null;
                }

                VerifyStaticSurface(surface);
                R.Left = X;
                R.Right = _sceneWidth + X;
                R.Top = Y;
                R.Bottom = _sceneHeight + Y;
                if (M.StaticSceneInvalid)
                {
                    M.UpdateStaticScene();
                    M.UpdateDynamicScene();
                    if (surface.Lock(ref lockedSurface, R) >= 0)
                    {
                        M.DrawStaticScene(ref lockedSurface);
                        M.DrawDynamicScene(ref lockedSurface);
                        surface.Unlock(lockedSurface);
                    }

                    _staticSurfaceValid = false;
                }
                else
                {
                    if (_staticSurfaceValid)
                    {
                        _staticSurface.BlitClipped(surface, X, Y);
                        M.UpdateDynamicScene();
                        if (surface.Lock(ref lockedSurface, R) >= 0)
                        {
                            M.DrawDynamicScene(ref lockedSurface);
                            surface.Unlock(lockedSurface);
                        }
                    }
                    else
                    {
                        M.UpdateStaticScene();
                        if (_staticSurface.Lock(ref lockedSurface) >= 0)
                        {
                            M.DrawStaticScene(ref lockedSurface);
                            _staticSurface.Unlock();
                            _staticSurfaceValid = true;
                        }

                        if (_staticSurface.BlitClipped(surface, X, Y) >= 0)
                        {
                            M.UpdateDynamicScene();
                            if (surface.Lock(ref lockedSurface, R) >= 0)
                            {
                                M.DrawDynamicScene(ref lockedSurface);
                                surface.Unlock(lockedSurface);
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
    public bool GetSceneExceedsMap() => _hexSpriteMap != null && (_sceneWidth > _hexSpriteMap.GetMaxPixelX() - 32 || _sceneHeight > _hexSpriteMap.GetMaxPixelY() - 32);

    // Port extension: the part of the scene that shows the map under the original clamp. On an axis
    // where the scene fits inside the map it spans the scene.
    public Rect GetMapSceneRectangle()
    {
        Rect R = default;
        if (_hexSpriteMap != null)
        {
            R.Left = Math.Max(16 - _sceneX, 0);
            R.Top = Math.Max(16 - _sceneY, 0);
            R.Right = Math.Min(_hexSpriteMap.GetMaxPixelX() - 16 - _sceneX, _sceneWidth);
            R.Bottom = Math.Min(_hexSpriteMap.GetMaxPixelY() - 16 - _sceneY, _sceneHeight);
        }

        return R;
    }

    // Port extension: where ShowScene shows the map, centered in the scene. It equals
    // GetMapSceneRect when the scene fits inside the map.
    public Rect GetMapDisplayRectangle()
    {
        Rect R = GetMapSceneRectangle();
        int width = R.Right - R.Left;
        int height = R.Bottom - R.Top;
        R.Left = (_sceneWidth - width) >> 1;
        R.Top = (_sceneHeight - height) >> 1;
        R.Right = R.Left + width;
        R.Bottom = R.Top + height;
        return R;
    }

    // Port extension: turns a point of the shown scene into the scene point of the original clamp,
    // which the map mouse takes. A point outside the centered map gives false.
    public bool SceneToMapMouse(ref int X, ref int Y)
    {
        Rect shown;
        Rect source;
        if (!GetSceneExceedsMap())
        {
            return true;
        }

        shown = GetMapDisplayRectangle();
        if (X < shown.Left || X >= shown.Right || Y < shown.Top || Y >= shown.Bottom)
        {
            return false;
        }

        source = GetMapSceneRectangle();
        X += source.Left - shown.Left;
        Y += source.Top - shown.Top;
        return true;
    }

    // Port extension: the scene-sized surface that ShowScene draws on when the scene exceeds the map.
    public Surface VerifyCenteredSurface(Surface surface)
    {
        if (CenteredSurface != null && CenteredSurface.GetPixelFormatId() != surface.GetPixelFormatId())
        {
            CenteredSurface?.Dispose();
            CenteredSurface = null;
        }

        if (CenteredSurface == null)
        {
            CenteredSurface = new DataSurface(_sceneWidth, _sceneHeight, surface.GetPixelFormatId(), null);
        }
        else if (CenteredSurface.GetWidth() != _sceneWidth || CenteredSurface.GetHeight() != _sceneHeight)
        {
            CenteredSurface.Resize(_sceneWidth, _sceneHeight);
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
            target.Unlock(lockedSurface);
        }

        CenteredSurface.BlitClipped(target, source, X + shown.Left, Y + shown.Top);
    }

    // Port extension: fills every pixel of SD outside Inside black.
    public static void FillOutsideRectangle(Rect inside, in LockedSurface lockedSurface)
    {
        PixelFormats.ColorFillSurfaceDescription(new Rect(0, 0, lockedSurface.Width, inside.Top), 0, lockedSurface);
        PixelFormats.ColorFillSurfaceDescription(new Rect(0, inside.Bottom, lockedSurface.Width, lockedSurface.Height), 0, lockedSurface);
        PixelFormats.ColorFillSurfaceDescription(new Rect(0, inside.Top, inside.Left, inside.Bottom), 0, lockedSurface);
        PixelFormats.ColorFillSurfaceDescription(new Rect(inside.Right, inside.Top, lockedSurface.Width, inside.Bottom), 0, lockedSurface);
    }

    public GameMap HexSpriteMap { get => _hexSpriteMap; set => SetHexSpriteMap(value); }
    public Surface StaticSurface { get => _staticSurface; }
    public bool StaticSurfaceValid { get => _staticSurfaceValid; set => _staticSurfaceValid = value; }
    public int SceneX { get => _sceneX; set => SetSceneX(value); }
    public int SceneY { get => _sceneY; set => SetSceneY(value); }
    public int SceneL { get => _sceneL; set => SetSceneL(value); }
    public int SceneWidth { get => _sceneWidth; set => SetSceneWidth(value); }
    public int SceneHeight { get => _sceneHeight; set => SetSceneHeight(value); }

    [Published]
    public NotifyEvent<object> OnSceneChanged { get => _onSceneChanged; set => _onSceneChanged = value; }
}
