// Translated from HSEngine.pas.
using Aow.Game.Draw;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

// shows a map on a surface. FStaticSurface (the static scene, kept between frames),
// FSceneWidth, FSceneHeight, FSceneX, FSceneY, FSceneL, FStaticSurfaceValid, FOnSceneChanged
// (published), FHSMap. SetHSMap points the map's OnCenterView, OnChangeView and OnViewLevel at
// CenterScene, ChangeScene and SetSceneL
public partial class HSMapViewer : Component
{
    internal Surface FStaticSurface;
    internal int FSceneWidth;
    internal int FSceneHeight;
    internal int FSceneX;
    internal int FSceneY;
    internal int FSceneL;
    internal bool FStaticSurfaceValid;
    internal NotifyEvent<object> FOnSceneChanged;
    internal HSMap FHSMap;
    // Port extension: the scene drawn under the original clamp before ShowCenteredMap centers it.
    internal Surface FCenteredSurface;

    // HSEngine.pas:4670
    public HSMapViewer(Component owner) : base(owner)
    {
        FStaticSurfaceValid = false;
        FOnSceneChanged = null;
    }

    // HSEngine.pas:4678
    // { Leaves the map's OnChangeView set (original slip). }
    public override void Dispose()
    {
        if (FHSMap != null)
        {
            FHSMap.FOnCenterView = null;
            FHSMap.FOnViewLevel = null;
        }

        FStaticSurface?.Dispose();
        FCenteredSurface?.Dispose();
        base.Dispose();
    }

    // HSEngine.pas:4718
    public virtual void SetHSMap(HSMap Value)
    {
        FHSMap = Value;
        if (FHSMap != null)
        {
            // { Install viewer callbacks on the assigned map. }
            FHSMap.FOnCenterView = CenterScene;
            FHSMap.FOnChangeView = ChangeScene;
            FHSMap.FOnViewLevel = SetSceneL;
        }
    }

    // HSEngine.pas:4794
    public virtual void VerifyStaticSurface(Surface Surface)
    {
        bool IsDDraw;
        Set<DDSCreationFlagsElement> Flags;
        IsDDraw = FStaticSurface is DDrawSurface;
        if (FStaticSurface != null && (FStaticSurface.GetPixelFormatID() != Surface.GetPixelFormatID() || (Surface is DDrawSurface) ^ IsDDraw))
        {
            FStaticSurface?.Dispose();
            FStaticSurface = null;
        }

        if (FStaticSurface == null || FStaticSurface.GetWidth() != FSceneWidth || FStaticSurface.GetHeight() != FSceneHeight)
        {
            if (FStaticSurface == null)
            {
                if (Surface is DDrawSurface)
                {
                    if (((DDrawSurface)Surface).Status.Contains(DDrawSurfaceStatusElement.VideoMemory))
                    {
                        Flags = Set<DDSCreationFlagsElement>.Of(DDSCreationFlagsElement.VideoMemory, DDSCreationFlagsElement.SystemMemory);
                    }
                    else
                    {
                        Flags = Set<DDSCreationFlagsElement>.Of(DDSCreationFlagsElement.SystemMemory);
                    }

                    FStaticSurface = new DDrawSurface();
                    var pixelFormatID = Surface.GetPixelFormatID();
                    ((DDrawSurface)FStaticSurface).CreateSurface(FSceneWidth, FSceneHeight, pixelFormatID, Flags);
                }
                else
                {
                    var pixelFormatID = Surface.GetPixelFormatID();
                    FStaticSurface = new DataSurface(FSceneWidth, FSceneHeight, pixelFormatID, null);
                }
            }
            else
            {
                FStaticSurface.Resize(FSceneWidth, FSceneHeight);
            }

            IsDDraw = FStaticSurface is DDrawSurface;
            FHSMap.FStaticSceneInvalid = true;
            FStaticSurfaceValid = false;
        }

        if (IsDDraw && ((DDrawSurface)FStaticSurface).Lost())
        {
            ((DDrawSurface)FStaticSurface).Restore();
            FStaticSurfaceValid = false;
        }
    }

    // HSEngine.pas:4689
    public void ChangeScene(int Xhp, int Yhp, int Lhp)
    {
        if (FHSMap != null)
        {
            SetSceneX(Xhp);
            SetSceneY(Yhp);
            SetSceneL(Lhp);
            if (FHSMap.SetViewPort(FSceneX, FSceneY, FSceneL, FSceneWidth, FSceneHeight) && FOnSceneChanged != null)
            {
                FOnSceneChanged(this);
            }
        }
    }

    // HSEngine.pas:4702
    public void CenterScene(int Xhx, int Yhx, int Lhx)
    {
        int Xhp = default;
        int Yhp = default;
        if (FHSMap != null)
        {
            HexGeometry.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
            SetSceneX(Xhp + 16 - (int)(((uint)FSceneWidth) >> 1));
            SetSceneY(Yhp + 16 - (int)(((uint)FSceneHeight) >> 1));
            SetSceneL(Lhx);
            if (FHSMap.SetViewPort(FSceneX, FSceneY, FSceneL, FSceneWidth, FSceneHeight) && FOnSceneChanged != null)
            {
                FOnSceneChanged(this);
            }
        }
    }

    // HSEngine.pas:4730
    public void SetSceneL(int Value)
    {
        if (FHSMap != null)
        {
            if (Value < 0)
            {
                Value = 0;
            }

            if (Value >= FHSMap.FMapContainer.FLevelCount)
            {
                Value = FHSMap.FMapContainer.FLevelCount - 1;
            }

            FSceneL = Value;
        }
    }

    // HSEngine.pas:4742
    public void SetSceneX(int Value)
    {
        if (FHSMap != null)
        {
            if (Value < 16)
            {
                Value = 16;
            }

            if (this.FHSMap.GetMaxXhp() < FSceneWidth + Value + 16)
            {
                this.FSceneX = this.FHSMap.GetMaxXhp() - FSceneWidth - 16;
            }
            else
            {
                this.FSceneX = Value;
            }
        }
    }

    // HSEngine.pas:4756
    public void SetSceneY(int Value)
    {
        if (FHSMap != null)
        {
            if (Value < 16)
            {
                Value = 16;
            }

            if (this.FHSMap.GetMaxYhp() - 16 < FSceneHeight + Value)
            {
                this.FSceneY = this.FHSMap.GetMaxYhp() - FSceneHeight - 16;
            }
            else
            {
                this.FSceneY = Value;
            }
        }
    }

    // HSEngine.pas:4770
    public void SetSceneWidth(int Value)
    {
        HSMap M;
        M = FHSMap;
        if (M != null && Value > 0)
        {
            FSceneWidth = Value;
            SetSceneX(FSceneX);
        }
    }

    // HSEngine.pas:4782
    public void SetSceneHeight(int Value)
    {
        HSMap M;
        M = FHSMap;
        if (M != null && Value > 0)
        {
            FSceneHeight = Value;
            SetSceneY(FSceneY);
        }
    }

    // HSEngine.pas:4837
    public void ResetStaticSurface()
    {
        FStaticSurface?.Dispose();
        FStaticSurface = null;
    }

    // HSEngine.pas:4843
    public void ShowScene(int X, int Y, Surface Surface)
    {
        HSMap M;
        object MP;
        SurfaceDesc SD = default;
        Rect R = default;
        Surface Target = Surface;
        int TargetX = X;
        int TargetY = Y;
        try
        {
            MP = FHSMap;
            if (MP != null)
            {
                M = (HSMap)MP;
                if (M.SetViewPort(FSceneX, FSceneY, FSceneL, FSceneWidth, FSceneHeight) && FOnSceneChanged != null)
                {
                    FOnSceneChanged(this);
                }

                // Port extension: a scene larger than the map draws as the original does into
                // FCenteredSurface, then ShowCenteredMap centers the map on Target and fills the rest black.
                if (GetSceneExceedsMap())
                {
                    Surface = VerifyCenteredSurface(Target);
                    X = 0;
                    Y = 0;
                }
                else if (FCenteredSurface != null)
                {
                    FCenteredSurface?.Dispose();
                    FCenteredSurface = null;
                }

                VerifyStaticSurface(Surface);
                R.Left = X;
                R.Right = FSceneWidth + X;
                R.Top = Y;
                R.Bottom = FSceneHeight + Y;
                if (M.FStaticSceneInvalid)
                {
                    M.UpdateStaticScene();
                    M.UpdateDynamicScene();
                    if (Surface.LockEx(ref SD, R) >= 0)
                    {
                        M.DrawStaticScene(ref SD);
                        M.DrawDynamicScene(ref SD);
                        Surface.UnlockEx(SD);
                    }

                    FStaticSurfaceValid = false;
                }
                else
                {
                    if (FStaticSurfaceValid)
                    {
                        FStaticSurface.BltClipped(Surface, X, Y);
                        M.UpdateDynamicScene();
                        if (Surface.LockEx(ref SD, R) >= 0)
                        {
                            M.DrawDynamicScene(ref SD);
                            Surface.UnlockEx(SD);
                        }
                    }
                    else
                    {
                        M.UpdateStaticScene();
                        if (FStaticSurface.Lock(ref SD) >= 0)
                        {
                            M.DrawStaticScene(ref SD);
                            FStaticSurface.Unlock();
                            FStaticSurfaceValid = true;
                        }

                        if (FStaticSurface.BltClipped(Surface, X, Y) >= 0)
                        {
                            M.UpdateDynamicScene();
                            if (Surface.LockEx(ref SD, R) >= 0)
                            {
                                M.DrawDynamicScene(ref SD);
                                Surface.UnlockEx(SD);
                            }
                        }
                    }
                }

                if (Surface != Target)
                {
                    ShowCenteredMap(Target, TargetX, TargetY);
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
    public bool GetSceneExceedsMap() => FHSMap != null && (FSceneWidth > FHSMap.GetMaxXhp() - 32 || FSceneHeight > FHSMap.GetMaxYhp() - 32);

    // Port extension: the part of the scene that shows the map under the original clamp. On an axis
    // where the scene fits inside the map it spans the scene.
    public Rect GetMapSceneRect()
    {
        Rect R = default;
        if (FHSMap != null)
        {
            R.Left = Math.Max(16 - FSceneX, 0);
            R.Top = Math.Max(16 - FSceneY, 0);
            R.Right = Math.Min(FHSMap.GetMaxXhp() - 16 - FSceneX, FSceneWidth);
            R.Bottom = Math.Min(FHSMap.GetMaxYhp() - 16 - FSceneY, FSceneHeight);
        }

        return R;
    }

    // Port extension: where ShowScene shows the map, centered in the scene. It equals
    // GetMapSceneRect when the scene fits inside the map.
    public Rect GetMapDisplayRect()
    {
        Rect R = GetMapSceneRect();
        int Width = R.Right - R.Left;
        int Height = R.Bottom - R.Top;
        R.Left = (FSceneWidth - Width) >> 1;
        R.Top = (FSceneHeight - Height) >> 1;
        R.Right = R.Left + Width;
        R.Bottom = R.Top + Height;
        return R;
    }

    // Port extension: turns a point of the shown scene into the scene point of the original clamp,
    // which the map mouse takes. A point outside the centered map gives false.
    public bool SceneToMapMouse(ref int X, ref int Y)
    {
        Rect Shown;
        Rect Source;
        if (!GetSceneExceedsMap())
        {
            return true;
        }

        Shown = GetMapDisplayRect();
        if (X < Shown.Left || X >= Shown.Right || Y < Shown.Top || Y >= Shown.Bottom)
        {
            return false;
        }

        Source = GetMapSceneRect();
        X += Source.Left - Shown.Left;
        Y += Source.Top - Shown.Top;
        return true;
    }

    // Port extension: the scene-sized surface that ShowScene draws on when the scene exceeds the map.
    public Surface VerifyCenteredSurface(Surface Surface)
    {
        if (FCenteredSurface != null && FCenteredSurface.GetPixelFormatID() != Surface.GetPixelFormatID())
        {
            FCenteredSurface?.Dispose();
            FCenteredSurface = null;
        }

        if (FCenteredSurface == null)
        {
            FCenteredSurface = new DataSurface(FSceneWidth, FSceneHeight, Surface.GetPixelFormatID(), null);
        }
        else if (FCenteredSurface.GetWidth() != FSceneWidth || FCenteredSurface.GetHeight() != FSceneHeight)
        {
            FCenteredSurface.Resize(FSceneWidth, FSceneHeight);
        }

        return FCenteredSurface;
    }

    // Port extension: fills the scene outside the centered map black and copies the map part of
    // FCenteredSurface into the centered rectangle.
    public void ShowCenteredMap(Surface Target, int X, int Y)
    {
        SurfaceDesc SD = default;
        Rect Source = GetMapSceneRect();
        Rect Shown = GetMapDisplayRect();
        if (Target.LockEx(ref SD, new Rect(X, Y, X + FSceneWidth, Y + FSceneHeight)) >= 0)
        {
            FillOutsideRect(Shown, SD);
            Target.UnlockEx(SD);
        }

        FCenteredSurface.BltClipped(Target, Source, X + Shown.Left, Y + Shown.Top);
    }

    // Port extension: fills every pixel of SD outside Inside black.
    public static void FillOutsideRect(Rect Inside, in SurfaceDesc SD)
    {
        PixelFormats.ColorFillSD(new Rect(0, 0, SD.Width, Inside.Top), 0, SD);
        PixelFormats.ColorFillSD(new Rect(0, Inside.Bottom, SD.Width, SD.Height), 0, SD);
        PixelFormats.ColorFillSD(new Rect(0, Inside.Top, Inside.Left, Inside.Bottom), 0, SD);
        PixelFormats.ColorFillSD(new Rect(Inside.Right, Inside.Top, SD.Width, Inside.Bottom), 0, SD);
    }

    public HSMap HSMap { get => FHSMap; set => SetHSMap(value); }
    public Surface StaticSurface { get => FStaticSurface; }
    public bool StaticSurfaceValid { get => FStaticSurfaceValid; set => FStaticSurfaceValid = value; }
    public int SceneX { get => FSceneX; set => SetSceneX(value); }
    public int SceneY { get => FSceneY; set => SetSceneY(value); }
    public int SceneL { get => FSceneL; set => SetSceneL(value); }
    public int SceneWidth { get => FSceneWidth; set => SetSceneWidth(value); }
    public int SceneHeight { get => FSceneHeight; set => SetSceneHeight(value); }

    [Published]
    public NotifyEvent<object> OnSceneChanged { get => FOnSceneChanged; set => FOnSceneChanged = value; }
}
