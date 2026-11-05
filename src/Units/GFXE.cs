// Translated from GFXE.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aow.Units.GFXE;

public enum TDDrawSurfaceStatusElement : byte
{
    ssVideoMemory = 0,
    ssLocked = 1,
    ssPrimary = 2,
    ssCreated = 3,
    ssComplex = 4
}

public enum TDDSCreationFlagsElement : byte
{
    cfVideoMemory = 0,
    cfSystemMemory = 1
}

[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct TDDColorKey
{
    [FieldOffset(0)]
    public int dwColorSpaceLowValue;
    [FieldOffset(4)]
    public int dwColorSpaceHighValue;
}

[StructLayout(LayoutKind.Explicit, Size = 32)]
public struct TDDPixelFormat
{
    [FieldOffset(0)]
    public int dwSize;
    [FieldOffset(4)]
    public int dwFlags;
    [FieldOffset(8)]
    public int dwFourCC;
    [FieldOffset(12)]
    public int dwRGBBitCount;
    [FieldOffset(16)]
    public int dwRBitMask;
    [FieldOffset(20)]
    public int dwGBitMask;
    [FieldOffset(24)]
    public int dwBBitMask;
    [FieldOffset(28)]
    public int dwRGBAlphaBitMask;
}

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct TDDSCaps
{
    [FieldOffset(0)]
    public int dwCaps;
}

public struct TDDSurfaceDesc
{
    public int dwSize;
    public int dwFlags;
    public int dwHeight;
    public int dwWidth;
    public int lPitch;
    public int dwBackBufferCount;
    public int dwMipMapCount;
    public int dwAlphaBitDepth;
    public int dwReserved;
    public object lpSurface;
    public TDDColorKey ddckCKDestOverlay;
    public TDDColorKey ddckCKDestBlt;
    public TDDColorKey ddckCKSrcOverlay;
    public TDDColorKey ddckCKSrcBlt;
    public TDDPixelFormat ddpfPixelFormat;
    public TDDSCaps ddsCaps;
}

[StructLayout(LayoutKind.Explicit, Size = 100)]
public struct TDDBltFX
{
    [FieldOffset(0)]
    public int dwSize;
    [FieldOffset(4)]
    public int dwDDFX;
    [FieldOffset(8)]
    public int dwROP;
    [FieldOffset(12)]
    public int dwDDROP;
    [FieldOffset(16)]
    public int dwRotationAngle;
    [FieldOffset(20)]
    public int dwZBufferOpCode;
    [FieldOffset(24)]
    public int dwZBufferLow;
    [FieldOffset(28)]
    public int dwZBufferHigh;
    [FieldOffset(32)]
    public int dwZBufferBaseDest;
    [FieldOffset(36)]
    public int dwZDestConstBitDepth;
    [FieldOffset(40)]
    public int dwZDestConst;
    [FieldOffset(44)]
    public int dwZSrcConstBitDepth;
    [FieldOffset(48)]
    public int dwZSrcConst;
    [FieldOffset(52)]
    public int dwAlphaEdgeBlendBitDepth;
    [FieldOffset(56)]
    public int dwAlphaEdgeBlend;
    [FieldOffset(60)]
    public int dwReserved;
    [FieldOffset(64)]
    public int dwAlphaDestConstBitDepth;
    [FieldOffset(68)]
    public int dwAlphaDestConst;
    [FieldOffset(72)]
    public int dwAlphaSrcConstBitDepth;
    [FieldOffset(76)]
    public int dwAlphaSrcConst;
    [FieldOffset(80)]
    public int dwFillColor;
    [FieldOffset(84)]
    public TDDColorKey ddckDestColorkey;
    [FieldOffset(92)]
    public TDDColorKey ddckSrcColorkey;
}

public delegate int TDDEnumModesCallback(Ptr<TDDSurfaceDesc> lpDDSurfaceDesc, object lpContext);
public partial class IDirectDrawSurface : IUnknown
{
    // GFXE.pas:212
    public int AddAttachedSurface(IDirectDrawSurface lpDDSAttachedSurface)
    {
        /* TODO: Implement IDirectDrawSurface.AddAttachedSurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:213
    public int AddOverlayDirtyRect(in TRect lpRect)
    {
        /* TODO: Implement IDirectDrawSurface.AddOverlayDirtyRect. */
        throw new NotImplementedException();
    }

    // GFXE.pas:214
    public int Blt(Ptr<TRect> lpDestRect, IDirectDrawSurface lpDDSrcSurface, Ptr<TRect> lpSrcRect, int dwFlags, Ptr<TDDBltFX> lpDDBltFx)
    {
        /* TODO: Implement IDirectDrawSurface.Blt. */
        throw new NotImplementedException();
    }

    // GFXE.pas:215
    public int BltBatch(object lpDDBltBatch, int dwCount, int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface.BltBatch. */
        throw new NotImplementedException();
    }

    // GFXE.pas:216
    public int BltFast(int dwX, int dwY, IDirectDrawSurface lpDDSrcSurface, Ptr<TRect> lpSrcRect, int dwTrans)
    {
        /* TODO: Implement IDirectDrawSurface.BltFast. */
        throw new NotImplementedException();
    }

    // GFXE.pas:217
    public int DeleteAttachedSurface(int dwFlags, IDirectDrawSurface lpDDSAttachedSurface)
    {
        /* TODO: Implement IDirectDrawSurface.DeleteAttachedSurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:218
    public int EnumAttachedSurfaces(object lpContext, object lpEnumSurfacesCallback)
    {
        /* TODO: Implement IDirectDrawSurface.EnumAttachedSurfaces. */
        throw new NotImplementedException();
    }

    // GFXE.pas:219
    public int EnumOverlayZOrders(int dwFlags, object lpContext, object lpfnCallback)
    {
        /* TODO: Implement IDirectDrawSurface.EnumOverlayZOrders. */
        throw new NotImplementedException();
    }

    // GFXE.pas:220
    public int Flip(IDirectDrawSurface lpDDSurfaceTargetOverride, int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface.Flip. */
        throw new NotImplementedException();
    }

    // GFXE.pas:221
    public int GetAttachedSurface(ref TDDSCaps lpDDSCaps, ref IDirectDrawSurface lplpDDAttachedSurface)
    {
        /* TODO: Implement IDirectDrawSurface.GetAttachedSurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:222
    public int GetBltStatus(int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface.GetBltStatus. */
        throw new NotImplementedException();
    }

    // GFXE.pas:223
    public int GetCaps(ref TDDSCaps lpDDSCaps)
    {
        /* TODO: Implement IDirectDrawSurface.GetCaps. */
        throw new NotImplementedException();
    }

    // GFXE.pas:224
    public int GetClipper(IDirectDrawClipper lplpDDClipper)
    {
        /* TODO: Implement IDirectDrawSurface.GetClipper. */
        throw new NotImplementedException();
    }

    // GFXE.pas:225
    public int GetColorKey(int dwFlags, ref TDDColorKey lpDDColorKey)
    {
        /* TODO: Implement IDirectDrawSurface.GetColorKey. */
        throw new NotImplementedException();
    }

    // GFXE.pas:226
    public int GetDC(ref int lphDC)
    {
        /* TODO: Implement IDirectDrawSurface.GetDC. */
        throw new NotImplementedException();
    }

    // GFXE.pas:227
    public int GetFlipStatus(int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface.GetFlipStatus. */
        throw new NotImplementedException();
    }

    // GFXE.pas:228
    public int GetOverlayPosition(ref int lplX, ref int lplY)
    {
        /* TODO: Implement IDirectDrawSurface.GetOverlayPosition. */
        throw new NotImplementedException();
    }

    // GFXE.pas:229
    public int GetPalette(IDirectDrawPalette lplpDDPalette)
    {
        /* TODO: Implement IDirectDrawSurface.GetPalette. */
        throw new NotImplementedException();
    }

    // GFXE.pas:230
    public int GetPixelFormat(ref TDDPixelFormat lpDDPixelFormat)
    {
        /* TODO: Implement IDirectDrawSurface.GetPixelFormat. */
        throw new NotImplementedException();
    }

    // GFXE.pas:231
    public int GetSurfaceDesc(ref TDDSurfaceDesc lpDDSurfaceDesc)
    {
        /* TODO: Implement IDirectDrawSurface.GetSurfaceDesc. */
        throw new NotImplementedException();
    }

    // GFXE.pas:232
    public int Initialize(IDirectDraw lpDD, ref TDDSurfaceDesc lpDDSurfaceDesc)
    {
        /* TODO: Implement IDirectDrawSurface.Initialize. */
        throw new NotImplementedException();
    }

    // GFXE.pas:233
    public int IsLost()
    {
        /* TODO: Implement IDirectDrawSurface.IsLost. */
        throw new NotImplementedException();
    }

    // GFXE.pas:234
    public int Lock(Ptr<TRect> lpDestRect, ref TDDSurfaceDesc lpDDSurfaceDesc, int dwFlags, int hEvent)
    {
        /* TODO: Implement IDirectDrawSurface.Lock. */
        throw new NotImplementedException();
    }

    // GFXE.pas:235
    public int ReleaseDC(int hDC)
    {
        /* TODO: Implement IDirectDrawSurface.ReleaseDC. */
        throw new NotImplementedException();
    }

    // GFXE.pas:236
    public int Restore()
    {
        /* TODO: Implement IDirectDrawSurface.Restore. */
        throw new NotImplementedException();
    }

    // GFXE.pas:237
    public int SetClipper(IDirectDrawClipper lpDDClipper)
    {
        /* TODO: Implement IDirectDrawSurface.SetClipper. */
        throw new NotImplementedException();
    }

    // GFXE.pas:238
    public int SetColorKey(int dwFlags, Ptr<TDDColorKey> lpDDColorKey)
    {
        /* TODO: Implement IDirectDrawSurface.SetColorKey. */
        throw new NotImplementedException();
    }

    // GFXE.pas:239
    public int SetOverlayPosition(int lX, int lY)
    {
        /* TODO: Implement IDirectDrawSurface.SetOverlayPosition. */
        throw new NotImplementedException();
    }

    // GFXE.pas:240
    public int SetPalette(IDirectDrawPalette lpDDPalette)
    {
        /* TODO: Implement IDirectDrawSurface.SetPalette. */
        throw new NotImplementedException();
    }

    // GFXE.pas:241
    public int Unlock(object lpSurfaceData)
    {
        /* TODO: Implement IDirectDrawSurface.Unlock. */
        throw new NotImplementedException();
    }

    // GFXE.pas:242
    public int UpdateOverlay(Ptr<TRect> lpSrcRect, IDirectDrawSurface lpDDDestSurface, Ptr<TRect> lpDestRect, int dwFlags, object lpDDOverlayFx)
    {
        /* TODO: Implement IDirectDrawSurface.UpdateOverlay. */
        throw new NotImplementedException();
    }

    // GFXE.pas:243
    public int UpdateOverlayDisplay(int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface.UpdateOverlayDisplay. */
        throw new NotImplementedException();
    }

    // GFXE.pas:244
    public int UpdateOverlayZOrder(int dwFlags, IDirectDrawSurface lpDDSReference)
    {
        /* TODO: Implement IDirectDrawSurface.UpdateOverlayZOrder. */
        throw new NotImplementedException();
    }
}

public partial class IDirectDrawPalette : IUnknown
{
    // GFXE.pas:196
    public int GetCaps(ref int lpdwCaps)
    {
        /* TODO: Implement IDirectDrawPalette.GetCaps. */
        throw new NotImplementedException();
    }

    // GFXE.pas:197
    public int GetEntries(int dwFlags, int dwBase, int dwNumEntries, object lpEntries)
    {
        /* TODO: Implement IDirectDrawPalette.GetEntries. */
        throw new NotImplementedException();
    }

    // GFXE.pas:198
    public int Initialize(IDirectDraw lpDD, int dwFlags, object lpDDColorTable)
    {
        /* TODO: Implement IDirectDrawPalette.Initialize. */
        throw new NotImplementedException();
    }

    // GFXE.pas:199
    public int SetEntries(int dwFlags, int dwStartingEntry, int dwCount, object lpEntries)
    {
        /* TODO: Implement IDirectDrawPalette.SetEntries. */
        throw new NotImplementedException();
    }
}

public partial class IDirectDrawClipper : IUnknown
{
    // GFXE.pas:203
    public int GetClipList(Ptr<TRect> lpRect, object lpClipList, ref int lpdwSize)
    {
        /* TODO: Implement IDirectDrawClipper.GetClipList. */
        throw new NotImplementedException();
    }

    // GFXE.pas:204
    public int GetHWnd(ref int lphWnd)
    {
        /* TODO: Implement IDirectDrawClipper.GetHWnd. */
        throw new NotImplementedException();
    }

    // GFXE.pas:205
    public int Initialize(IDirectDraw lpDD, int dwFlags)
    {
        /* TODO: Implement IDirectDrawClipper.Initialize. */
        throw new NotImplementedException();
    }

    // GFXE.pas:206
    public int IsClipListChanged(ref bool lpbChanged)
    {
        /* TODO: Implement IDirectDrawClipper.IsClipListChanged. */
        throw new NotImplementedException();
    }

    // GFXE.pas:207
    public int SetClipList(object lpClipList, int dwFlags)
    {
        /* TODO: Implement IDirectDrawClipper.SetClipList. */
        throw new NotImplementedException();
    }

    // GFXE.pas:208
    public int SetHWnd(int dwFlags, int hWnd)
    {
        /* TODO: Implement IDirectDrawClipper.SetHWnd. */
        throw new NotImplementedException();
    }
}

public partial class IDirectDraw : IUnknown
{
    // GFXE.pas:149
    public int Compact()
    {
        /* TODO: Implement IDirectDraw.Compact. */
        throw new NotImplementedException();
    }

    // GFXE.pas:150
    public int CreateClipper(int dwFlags, IDirectDrawClipper lplpDDClipper, IUnknown pUnkOuter)
    {
        /* TODO: Implement IDirectDraw.CreateClipper. */
        throw new NotImplementedException();
    }

    // GFXE.pas:151
    public int CreatePalette(int dwFlags, object lpColorTable, IDirectDrawPalette lplpDDPalette, IUnknown pUnkOuter)
    {
        /* TODO: Implement IDirectDraw.CreatePalette. */
        throw new NotImplementedException();
    }

    // GFXE.pas:152
    public int CreateSurface(in TDDSurfaceDesc lpDDSurfaceDesc, ref IDirectDrawSurface lplpDDSurface, IUnknown pUnkOuter)
    {
        /* TODO: Implement IDirectDraw.CreateSurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:153
    public int DuplicateSurface(IDirectDrawSurface lpDDSurface, IDirectDrawSurface lplpDupDDSurface)
    {
        /* TODO: Implement IDirectDraw.DuplicateSurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:154
    public int EnumDisplayModes(int dwFlags, Ptr<TDDSurfaceDesc> lpDDSurfaceDesc, object lpContext, TDDEnumModesCallback lpEnumModesCallback)
    {
        /* TODO: Implement IDirectDraw.EnumDisplayModes. */
        throw new NotImplementedException();
    }

    // GFXE.pas:155
    public int EnumSurfaces(int dwFlags, Ptr<TDDSurfaceDesc> lpDDSD, object lpContext, object lpEnumCallback)
    {
        /* TODO: Implement IDirectDraw.EnumSurfaces. */
        throw new NotImplementedException();
    }

    // GFXE.pas:156
    public int FlipToGDISurface()
    {
        /* TODO: Implement IDirectDraw.FlipToGDISurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:157
    public int GetCaps(object lpDDDriverCaps, object lpDDHELCaps)
    {
        /* TODO: Implement IDirectDraw.GetCaps. */
        throw new NotImplementedException();
    }

    // GFXE.pas:158
    public int GetDisplayMode(ref TDDSurfaceDesc lpDDSurfaceDesc)
    {
        /* TODO: Implement IDirectDraw.GetDisplayMode. */
        throw new NotImplementedException();
    }

    // GFXE.pas:159
    public int GetFourCCCodes(ref int lpNumCodes, Ptr<int> lpCodes)
    {
        /* TODO: Implement IDirectDraw.GetFourCCCodes. */
        throw new NotImplementedException();
    }

    // GFXE.pas:160
    public int GetGDISurface(IDirectDrawSurface lplpGDIDDSSurface)
    {
        /* TODO: Implement IDirectDraw.GetGDISurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:161
    public int GetMonitorFrequency(ref int lpdwFrequency)
    {
        /* TODO: Implement IDirectDraw.GetMonitorFrequency. */
        throw new NotImplementedException();
    }

    // GFXE.pas:162
    public int GetScanLine(ref int lpdwScanLine)
    {
        /* TODO: Implement IDirectDraw.GetScanLine. */
        throw new NotImplementedException();
    }

    // GFXE.pas:163
    public int GetVerticalBlankStatus(ref bool lpbIsInVB)
    {
        /* TODO: Implement IDirectDraw.GetVerticalBlankStatus. */
        throw new NotImplementedException();
    }

    // GFXE.pas:164
    public int Initialize(Ptr<TGUID> lpGUID)
    {
        /* TODO: Implement IDirectDraw.Initialize. */
        throw new NotImplementedException();
    }

    // GFXE.pas:165
    public int RestoreDisplayMode()
    {
        /* TODO: Implement IDirectDraw.RestoreDisplayMode. */
        throw new NotImplementedException();
    }

    // GFXE.pas:166
    public int SetCooperativeLevel(int hWnd, int dwFlags)
    {
        /* TODO: Implement IDirectDraw.SetCooperativeLevel. */
        throw new NotImplementedException();
    }

    // GFXE.pas:167
    public int SetDisplayMode(int dwWidth, int dwHeight, int dwBpp)
    {
        /* TODO: Implement IDirectDraw.SetDisplayMode. */
        throw new NotImplementedException();
    }

    // GFXE.pas:168
    public int WaitForVerticalBlank(int dwFlags, int hEvent)
    {
        /* TODO: Implement IDirectDraw.WaitForVerticalBlank. */
        throw new NotImplementedException();
    }
}

public partial class IDirectDraw2 : IUnknown
{
    // GFXE.pas:172
    public int Compact()
    {
        /* TODO: Implement IDirectDraw2.Compact. */
        throw new NotImplementedException();
    }

    // GFXE.pas:173
    public int CreateClipper(int dwFlags, IDirectDrawClipper lplpDDClipper, IUnknown pUnkOuter)
    {
        /* TODO: Implement IDirectDraw2.CreateClipper. */
        throw new NotImplementedException();
    }

    // GFXE.pas:174
    public int CreatePalette(int dwFlags, object lpColorTable, IDirectDrawPalette lplpDDPalette, IUnknown pUnkOuter)
    {
        /* TODO: Implement IDirectDraw2.CreatePalette. */
        throw new NotImplementedException();
    }

    // GFXE.pas:175
    public int CreateSurface(in TDDSurfaceDesc lpDDSurfaceDesc, ref IDirectDrawSurface lplpDDSurface, IUnknown pUnkOuter)
    {
        /* TODO: Implement IDirectDraw2.CreateSurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:176
    public int DuplicateSurface(IDirectDrawSurface lpDDSurface, IDirectDrawSurface lplpDupDDSurface)
    {
        /* TODO: Implement IDirectDraw2.DuplicateSurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:177
    public int EnumDisplayModes(int dwFlags, Ptr<TDDSurfaceDesc> lpDDSurfaceDesc, object lpContext, TDDEnumModesCallback lpEnumModesCallback)
    {
        /* TODO: Implement IDirectDraw2.EnumDisplayModes. */
        throw new NotImplementedException();
    }

    // GFXE.pas:178
    public int EnumSurfaces(int dwFlags, Ptr<TDDSurfaceDesc> lpDDSD, object lpContext, object lpEnumCallback)
    {
        /* TODO: Implement IDirectDraw2.EnumSurfaces. */
        throw new NotImplementedException();
    }

    // GFXE.pas:179
    public int FlipToGDISurface()
    {
        /* TODO: Implement IDirectDraw2.FlipToGDISurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:180
    public int GetCaps(object lpDDDriverCaps, object lpDDHELCaps)
    {
        /* TODO: Implement IDirectDraw2.GetCaps. */
        throw new NotImplementedException();
    }

    // GFXE.pas:181
    public int GetDisplayMode(ref TDDSurfaceDesc lpDDSurfaceDesc)
    {
        /* TODO: Implement IDirectDraw2.GetDisplayMode. */
        throw new NotImplementedException();
    }

    // GFXE.pas:182
    public int GetFourCCCodes(ref int lpNumCodes, Ptr<int> lpCodes)
    {
        /* TODO: Implement IDirectDraw2.GetFourCCCodes. */
        throw new NotImplementedException();
    }

    // GFXE.pas:183
    public int GetGDISurface(IDirectDrawSurface lplpGDIDDSSurface)
    {
        /* TODO: Implement IDirectDraw2.GetGDISurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:184
    public int GetMonitorFrequency(ref int lpdwFrequency)
    {
        /* TODO: Implement IDirectDraw2.GetMonitorFrequency. */
        throw new NotImplementedException();
    }

    // GFXE.pas:185
    public int GetScanLine(ref int lpdwScanLine)
    {
        /* TODO: Implement IDirectDraw2.GetScanLine. */
        throw new NotImplementedException();
    }

    // GFXE.pas:186
    public int GetVerticalBlankStatus(ref bool lpbIsInVB)
    {
        /* TODO: Implement IDirectDraw2.GetVerticalBlankStatus. */
        throw new NotImplementedException();
    }

    // GFXE.pas:187
    public int Initialize(Ptr<TGUID> lpGUID)
    {
        /* TODO: Implement IDirectDraw2.Initialize. */
        throw new NotImplementedException();
    }

    // GFXE.pas:188
    public int RestoreDisplayMode()
    {
        /* TODO: Implement IDirectDraw2.RestoreDisplayMode. */
        throw new NotImplementedException();
    }

    // GFXE.pas:189
    public int SetCooperativeLevel(int hWnd, int dwFlags)
    {
        /* TODO: Implement IDirectDraw2.SetCooperativeLevel. */
        throw new NotImplementedException();
    }

    // GFXE.pas:190
    public int SetDisplayMode(int dwWidth, int dwHeight, int dwBpp, int dwRefreshRate, int dwFlags)
    {
        /* TODO: Implement IDirectDraw2.SetDisplayMode. */
        throw new NotImplementedException();
    }

    // GFXE.pas:191
    public int WaitForVerticalBlank(int dwFlags, int hEvent)
    {
        /* TODO: Implement IDirectDraw2.WaitForVerticalBlank. */
        throw new NotImplementedException();
    }

    // GFXE.pas:192
    public int GetAvailableVidMem(Ptr<TDDSCaps> lpDDSCaps, ref int lpdwTotal, ref int lpdwFree)
    {
        /* TODO: Implement IDirectDraw2.GetAvailableVidMem. */
        throw new NotImplementedException();
    }
}

public partial class IDirectDrawSurface2 : IUnknown
{
    // GFXE.pas:248
    public int AddAttachedSurface(IDirectDrawSurface2 lpDDSAttachedSurface)
    {
        /* TODO: Implement IDirectDrawSurface2.AddAttachedSurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:249
    public int AddOverlayDirtyRect(in TRect lpRect)
    {
        /* TODO: Implement IDirectDrawSurface2.AddOverlayDirtyRect. */
        throw new NotImplementedException();
    }

    // GFXE.pas:250
    public int Blt(Ptr<TRect> lpDestRect, IDirectDrawSurface2 lpDDSrcSurface, Ptr<TRect> lpSrcRect, int dwFlags, Ptr<TDDBltFX> lpDDBltFx)
    {
        /* TODO: Implement IDirectDrawSurface2.Blt. */
        throw new NotImplementedException();
    }

    // GFXE.pas:251
    public int BltBatch(object lpDDBltBatch, int dwCount, int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface2.BltBatch. */
        throw new NotImplementedException();
    }

    // GFXE.pas:252
    public int BltFast(int dwX, int dwY, IDirectDrawSurface2 lpDDSrcSurface, Ptr<TRect> lpSrcRect, int dwTrans)
    {
        /* TODO: Implement IDirectDrawSurface2.BltFast. */
        throw new NotImplementedException();
    }

    // GFXE.pas:253
    public int DeleteAttachedSurface(int dwFlags, IDirectDrawSurface2 lpDDSAttachedSurface)
    {
        /* TODO: Implement IDirectDrawSurface2.DeleteAttachedSurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:254
    public int EnumAttachedSurfaces(object lpContext, object lpEnumSurfacesCallback)
    {
        /* TODO: Implement IDirectDrawSurface2.EnumAttachedSurfaces. */
        throw new NotImplementedException();
    }

    // GFXE.pas:255
    public int EnumOverlayZOrders(int dwFlags, object lpContext, object lpfnCallback)
    {
        /* TODO: Implement IDirectDrawSurface2.EnumOverlayZOrders. */
        throw new NotImplementedException();
    }

    // GFXE.pas:256
    public int Flip(IDirectDrawSurface2 lpDDSurfaceTargetOverride, int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface2.Flip. */
        throw new NotImplementedException();
    }

    // GFXE.pas:257
    public int GetAttachedSurface(ref TDDSCaps lpDDSCaps, ref IDirectDrawSurface2 lplpDDAttachedSurface)
    {
        /* TODO: Implement IDirectDrawSurface2.GetAttachedSurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:258
    public int GetBltStatus(int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface2.GetBltStatus. */
        throw new NotImplementedException();
    }

    // GFXE.pas:259
    public int GetCaps(ref TDDSCaps lpDDSCaps)
    {
        /* TODO: Implement IDirectDrawSurface2.GetCaps. */
        throw new NotImplementedException();
    }

    // GFXE.pas:260
    public int GetClipper(IDirectDrawClipper lplpDDClipper)
    {
        /* TODO: Implement IDirectDrawSurface2.GetClipper. */
        throw new NotImplementedException();
    }

    // GFXE.pas:261
    public int GetColorKey(int dwFlags, ref TDDColorKey lpDDColorKey)
    {
        /* TODO: Implement IDirectDrawSurface2.GetColorKey. */
        throw new NotImplementedException();
    }

    // GFXE.pas:262
    public int GetDC(ref int lphDC)
    {
        /* TODO: Implement IDirectDrawSurface2.GetDC. */
        throw new NotImplementedException();
    }

    // GFXE.pas:263
    public int GetFlipStatus(int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface2.GetFlipStatus. */
        throw new NotImplementedException();
    }

    // GFXE.pas:264
    public int GetOverlayPosition(ref int lplX, ref int lplY)
    {
        /* TODO: Implement IDirectDrawSurface2.GetOverlayPosition. */
        throw new NotImplementedException();
    }

    // GFXE.pas:265
    public int GetPalette(IDirectDrawPalette lplpDDPalette)
    {
        /* TODO: Implement IDirectDrawSurface2.GetPalette. */
        throw new NotImplementedException();
    }

    // GFXE.pas:266
    public int GetPixelFormat(ref TDDPixelFormat lpDDPixelFormat)
    {
        /* TODO: Implement IDirectDrawSurface2.GetPixelFormat. */
        throw new NotImplementedException();
    }

    // GFXE.pas:267
    public int GetSurfaceDesc(ref TDDSurfaceDesc lpDDSurfaceDesc)
    {
        /* TODO: Implement IDirectDrawSurface2.GetSurfaceDesc. */
        throw new NotImplementedException();
    }

    // GFXE.pas:268
    public int Initialize(IDirectDraw lpDD, ref TDDSurfaceDesc lpDDSurfaceDesc)
    {
        /* TODO: Implement IDirectDrawSurface2.Initialize. */
        throw new NotImplementedException();
    }

    // GFXE.pas:269
    public int IsLost()
    {
        /* TODO: Implement IDirectDrawSurface2.IsLost. */
        throw new NotImplementedException();
    }

    // GFXE.pas:270
    public int Lock(Ptr<TRect> lpDestRect, ref TDDSurfaceDesc lpDDSurfaceDesc, int dwFlags, int hEvent)
    {
        /* TODO: Implement IDirectDrawSurface2.Lock. */
        throw new NotImplementedException();
    }

    // GFXE.pas:271
    public int ReleaseDC(int hDC)
    {
        /* TODO: Implement IDirectDrawSurface2.ReleaseDC. */
        throw new NotImplementedException();
    }

    // GFXE.pas:272
    public int Restore()
    {
        /* TODO: Implement IDirectDrawSurface2.Restore. */
        throw new NotImplementedException();
    }

    // GFXE.pas:273
    public int SetClipper(IDirectDrawClipper lpDDClipper)
    {
        /* TODO: Implement IDirectDrawSurface2.SetClipper. */
        throw new NotImplementedException();
    }

    // GFXE.pas:274
    public int SetColorKey(int dwFlags, Ptr<TDDColorKey> lpDDColorKey)
    {
        /* TODO: Implement IDirectDrawSurface2.SetColorKey. */
        throw new NotImplementedException();
    }

    // GFXE.pas:275
    public int SetOverlayPosition(int lX, int lY)
    {
        /* TODO: Implement IDirectDrawSurface2.SetOverlayPosition. */
        throw new NotImplementedException();
    }

    // GFXE.pas:276
    public int SetPalette(IDirectDrawPalette lpDDPalette)
    {
        /* TODO: Implement IDirectDrawSurface2.SetPalette. */
        throw new NotImplementedException();
    }

    // GFXE.pas:277
    public int Unlock(object lpSurfaceData)
    {
        /* TODO: Implement IDirectDrawSurface2.Unlock. */
        throw new NotImplementedException();
    }

    // GFXE.pas:278
    public int UpdateOverlay(Ptr<TRect> lpSrcRect, IDirectDrawSurface2 lpDDDestSurface, Ptr<TRect> lpDestRect, int dwFlags, object lpDDOverlayFx)
    {
        /* TODO: Implement IDirectDrawSurface2.UpdateOverlay. */
        throw new NotImplementedException();
    }

    // GFXE.pas:279
    public int UpdateOverlayDisplay(int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface2.UpdateOverlayDisplay. */
        throw new NotImplementedException();
    }

    // GFXE.pas:280
    public int UpdateOverlayZOrder(int dwFlags, IDirectDrawSurface2 lpDDSReference)
    {
        /* TODO: Implement IDirectDrawSurface2.UpdateOverlayZOrder. */
        throw new NotImplementedException();
    }

    // GFXE.pas:281
    public int GetDDInterface(IUnknown lplpDD)
    {
        /* TODO: Implement IDirectDrawSurface2.GetDDInterface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:282
    public int PageLock(int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface2.PageLock. */
        throw new NotImplementedException();
    }

    // GFXE.pas:283
    public int PageUnlock(int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface2.PageUnlock. */
        throw new NotImplementedException();
    }
}

public partial class IDirectDrawSurface3 : IUnknown
{
    // GFXE.pas:287
    public int AddAttachedSurface(IDirectDrawSurface3 lpDDSAttachedSurface)
    {
        /* TODO: Implement IDirectDrawSurface3.AddAttachedSurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:288
    public int AddOverlayDirtyRect(in TRect lpRect)
    {
        /* TODO: Implement IDirectDrawSurface3.AddOverlayDirtyRect. */
        throw new NotImplementedException();
    }

    // GFXE.pas:289
    public int Blt(Ptr<TRect> lpDestRect, IDirectDrawSurface3 lpDDSrcSurface, Ptr<TRect> lpSrcRect, int dwFlags, Ptr<TDDBltFX> lpDDBltFx)
    {
        /* TODO: Implement IDirectDrawSurface3.Blt. */
        throw new NotImplementedException();
    }

    // GFXE.pas:290
    public int BltBatch(object lpDDBltBatch, int dwCount, int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface3.BltBatch. */
        throw new NotImplementedException();
    }

    // GFXE.pas:291
    public int BltFast(int dwX, int dwY, IDirectDrawSurface3 lpDDSrcSurface, Ptr<TRect> lpSrcRect, int dwTrans)
    {
        /* TODO: Implement IDirectDrawSurface3.BltFast. */
        throw new NotImplementedException();
    }

    // GFXE.pas:292
    public int DeleteAttachedSurface(int dwFlags, IDirectDrawSurface3 lpDDSAttachedSurface)
    {
        /* TODO: Implement IDirectDrawSurface3.DeleteAttachedSurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:293
    public int EnumAttachedSurfaces(object lpContext, object lpEnumSurfacesCallback)
    {
        /* TODO: Implement IDirectDrawSurface3.EnumAttachedSurfaces. */
        throw new NotImplementedException();
    }

    // GFXE.pas:294
    public int EnumOverlayZOrders(int dwFlags, object lpContext, object lpfnCallback)
    {
        /* TODO: Implement IDirectDrawSurface3.EnumOverlayZOrders. */
        throw new NotImplementedException();
    }

    // GFXE.pas:295
    public int Flip(IDirectDrawSurface3 lpDDSurfaceTargetOverride, int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface3.Flip. */
        throw new NotImplementedException();
    }

    // GFXE.pas:296
    public int GetAttachedSurface(ref TDDSCaps lpDDSCaps, ref IDirectDrawSurface3 lplpDDAttachedSurface)
    {
        /* TODO: Implement IDirectDrawSurface3.GetAttachedSurface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:297
    public int GetBltStatus(int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface3.GetBltStatus. */
        throw new NotImplementedException();
    }

    // GFXE.pas:298
    public int GetCaps(ref TDDSCaps lpDDSCaps)
    {
        /* TODO: Implement IDirectDrawSurface3.GetCaps. */
        throw new NotImplementedException();
    }

    // GFXE.pas:299
    public int GetClipper(IDirectDrawClipper lplpDDClipper)
    {
        /* TODO: Implement IDirectDrawSurface3.GetClipper. */
        throw new NotImplementedException();
    }

    // GFXE.pas:300
    public int GetColorKey(int dwFlags, ref TDDColorKey lpDDColorKey)
    {
        /* TODO: Implement IDirectDrawSurface3.GetColorKey. */
        throw new NotImplementedException();
    }

    // GFXE.pas:301
    public int GetDC(ref int lphDC)
    {
        /* TODO: Implement IDirectDrawSurface3.GetDC. */
        throw new NotImplementedException();
    }

    // GFXE.pas:302
    public int GetFlipStatus(int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface3.GetFlipStatus. */
        throw new NotImplementedException();
    }

    // GFXE.pas:303
    public int GetOverlayPosition(ref int lplX, ref int lplY)
    {
        /* TODO: Implement IDirectDrawSurface3.GetOverlayPosition. */
        throw new NotImplementedException();
    }

    // GFXE.pas:304
    public int GetPalette(IDirectDrawPalette lplpDDPalette)
    {
        /* TODO: Implement IDirectDrawSurface3.GetPalette. */
        throw new NotImplementedException();
    }

    // GFXE.pas:305
    public int GetPixelFormat(ref TDDPixelFormat lpDDPixelFormat)
    {
        /* TODO: Implement IDirectDrawSurface3.GetPixelFormat. */
        throw new NotImplementedException();
    }

    // GFXE.pas:306
    public int GetSurfaceDesc(ref TDDSurfaceDesc lpDDSurfaceDesc)
    {
        /* TODO: Implement IDirectDrawSurface3.GetSurfaceDesc. */
        throw new NotImplementedException();
    }

    // GFXE.pas:307
    public int Initialize(IDirectDraw lpDD, ref TDDSurfaceDesc lpDDSurfaceDesc)
    {
        /* TODO: Implement IDirectDrawSurface3.Initialize. */
        throw new NotImplementedException();
    }

    // GFXE.pas:308
    public int IsLost()
    {
        /* TODO: Implement IDirectDrawSurface3.IsLost. */
        throw new NotImplementedException();
    }

    // GFXE.pas:309
    public int Lock(Ptr<TRect> lpDestRect, ref TDDSurfaceDesc lpDDSurfaceDesc, int dwFlags, int hEvent)
    {
        /* TODO: Implement IDirectDrawSurface3.Lock. */
        throw new NotImplementedException();
    }

    // GFXE.pas:310
    public int ReleaseDC(int hDC)
    {
        /* TODO: Implement IDirectDrawSurface3.ReleaseDC. */
        throw new NotImplementedException();
    }

    // GFXE.pas:311
    public int Restore()
    {
        /* TODO: Implement IDirectDrawSurface3.Restore. */
        throw new NotImplementedException();
    }

    // GFXE.pas:312
    public int SetClipper(IDirectDrawClipper lpDDClipper)
    {
        /* TODO: Implement IDirectDrawSurface3.SetClipper. */
        throw new NotImplementedException();
    }

    // GFXE.pas:313
    public int SetColorKey(int dwFlags, Ptr<TDDColorKey> lpDDColorKey)
    {
        /* TODO: Implement IDirectDrawSurface3.SetColorKey. */
        throw new NotImplementedException();
    }

    // GFXE.pas:314
    public int SetOverlayPosition(int lX, int lY)
    {
        /* TODO: Implement IDirectDrawSurface3.SetOverlayPosition. */
        throw new NotImplementedException();
    }

    // GFXE.pas:315
    public int SetPalette(IDirectDrawPalette lpDDPalette)
    {
        /* TODO: Implement IDirectDrawSurface3.SetPalette. */
        throw new NotImplementedException();
    }

    // GFXE.pas:316
    public int Unlock(object lpSurfaceData)
    {
        /* TODO: Implement IDirectDrawSurface3.Unlock. */
        throw new NotImplementedException();
    }

    // GFXE.pas:317
    public int UpdateOverlay(Ptr<TRect> lpSrcRect, IDirectDrawSurface3 lpDDDestSurface, Ptr<TRect> lpDestRect, int dwFlags, object lpDDOverlayFx)
    {
        /* TODO: Implement IDirectDrawSurface3.UpdateOverlay. */
        throw new NotImplementedException();
    }

    // GFXE.pas:318
    public int UpdateOverlayDisplay(int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface3.UpdateOverlayDisplay. */
        throw new NotImplementedException();
    }

    // GFXE.pas:319
    public int UpdateOverlayZOrder(int dwFlags, IDirectDrawSurface3 lpDDSReference)
    {
        /* TODO: Implement IDirectDrawSurface3.UpdateOverlayZOrder. */
        throw new NotImplementedException();
    }

    // GFXE.pas:320
    public int GetDDInterface(IUnknown lplpDD)
    {
        /* TODO: Implement IDirectDrawSurface3.GetDDInterface. */
        throw new NotImplementedException();
    }

    // GFXE.pas:321
    public int PageLock(int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface3.PageLock. */
        throw new NotImplementedException();
    }

    // GFXE.pas:322
    public int PageUnlock(int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface3.PageUnlock. */
        throw new NotImplementedException();
    }

    // GFXE.pas:323
    public int SetSurfaceDesc(in TDDSurfaceDesc lpDDSD, int dwFlags)
    {
        /* TODO: Implement IDirectDrawSurface3.SetSurfaceDesc. */
        throw new NotImplementedException();
    }
}

public partial class IDirectDrawColorControl : IUnknown
{
    // GFXE.pas:327
    public int GetColorControls(object lpColorControl)
    {
        /* TODO: Implement IDirectDrawColorControl.GetColorControls. */
        throw new NotImplementedException();
    }

    // GFXE.pas:328
    public int SetColorControls(object lpColorControl)
    {
        /* TODO: Implement IDirectDrawColorControl.SetColorControls. */
        throw new NotImplementedException();
    }
}

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct TPaletteEntry
{
    [FieldOffset(0)]
    public byte R;
    [FieldOffset(1)]
    public byte G;
    [FieldOffset(2)]
    public byte B;
    [FieldOffset(3)]
    public byte Reserved;
    [FieldOffset(0)]
    public ushort W;
    [FieldOffset(0)]
    public int Value;
}

public partial class TGFXEPalette : TObject
{
    public int PixelFormatID;
    public TPaletteEntry[] Entries = new TPaletteEntry[256];
    // GFXE.pas:11050
    public void LoadFromStream(TStream Stream)
    {
        Stream.Read(Bytes.Of(ref PixelFormatID, 4));
        Stream.Read(MemoryMarshal.AsBytes((Span<TPaletteEntry>)Entries).Slice(0, 1024));
    }

    // GFXE.pas:11056
    public void SaveToStream(TStream Stream)
    {
        Stream.Write(Bytes.Of(ref PixelFormatID, 4));
        Stream.Write(MemoryMarshal.AsBytes((ReadOnlySpan<TPaletteEntry>)Entries).Slice(0, 1024));
    }

    // GFXE.pas:11068
    public Ptr<TPaletteEntry> GetEntry(int Index) => Ptr<TPaletteEntry>.To(Entries, Index);

    // GFXE.pas:11073
    public int Remap(int NewPixelFormatID)
    {
        int Result = GFXE.GFXE_OK;
        if (NewPixelFormatID != PixelFormatID)
        {
            switch (PixelFormatID)
            {
                case GFXE.pfBGR24:
                    switch (NewPixelFormatID)
                    {
                        case GFXE.pfRGB565:
                        {
                            for (int I = 0; I <= 255; I++)
                            {
                                ref var paletteEntry = ref Entries[I];
                                paletteEntry.W = (ushort)(((paletteEntry.R & 248) << 8) + ((paletteEntry.G & 252) << 3) + (int)(((uint)paletteEntry.B) >> 3));
                            }
                        }

                            break;
                        case GFXE.pfRGB555:
                        {
                            for (int I = 0; I <= 255; I++)
                            {
                                ref var paletteEntry = ref Entries[I];
                                paletteEntry.W = (ushort)(((paletteEntry.R & 248) << 7) + ((paletteEntry.G & 248) << 2) + (int)(((uint)paletteEntry.B) >> 3));
                            }
                        }

                            break;
                        case GFXE.pfXRGB32:
                            Result = GFXE.GFXE_OK;
                            break;
                        default:
                            Result = GFXE.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case GFXE.pfRGB24:
                    switch (NewPixelFormatID)
                    {
                        case GFXE.pfRGB565:
                        {
                            for (int I = 0; I <= 255; I++)
                            {
                                ref var paletteEntry = ref Entries[I];
                                paletteEntry.W = (ushort)(((paletteEntry.B & 248) << 8) + ((paletteEntry.G & 252) << 3) + (int)(((uint)paletteEntry.R) >> 3));
                            }
                        }

                            break;
                        case GFXE.pfRGB555:
                        {
                            for (int I = 0; I <= 255; I++)
                            {
                                ref var paletteEntry = ref Entries[I];
                                paletteEntry.W = (ushort)(((paletteEntry.B & 248) << 7) + ((paletteEntry.G & 248) << 2) + (int)(((uint)paletteEntry.R) >> 3));
                            }
                        }

                            break;
                        case GFXE.pfXRGB32:
                            Result = GFXE.GFXE_OK;
                            break;
                        default:
                            Result = GFXE.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                default:
                    Result = GFXE.GFXE_NOTSUPPORTED;
                    break;
            }

            if (Succeeded(Result))
            {
                PixelFormatID = NewPixelFormatID;
            }
        }
        else
        {
            Result = GFXE.GFXE_OK;
        }

        return Result;
    }

    // GFXE.pas:11140
    public bool Compare(TGFXEPalette Palette)
    {
        bool Result;
        int I;
        if (Palette.PixelFormatID == PixelFormatID)
        {
            switch (PixelFormatID & 31)
            {
                case 8:
                    Result = true;
                    I = 255;
                    while (I >= 0 && Result)
                    {
                        Result = Palette.Entries[I].W == Entries[I].W;
                        I -= 1;
                    }

                    break;
                case 16:
                    Result = true;
                    I = 255;
                    while (I >= 0 && Result)
                    {
                        Result = Palette.Entries[I].W == Entries[I].W;
                        I -= 1;
                    }

                    break;
                case 24:
                    Result = true;
                    I = 255;
                    while (I >= 0 && Result)
                    {
                        Result = Palette.Entries[I].W == Entries[I].W && Palette.Entries[I].B == Entries[I].B;
                        I -= 1;
                    }

                    break;
                case 32:
                    Result = CompareMem(MemoryMarshal.AsBytes((ReadOnlySpan<TPaletteEntry>)Entries).Slice(0, 1024), MemoryMarshal.AsBytes((ReadOnlySpan<TPaletteEntry>)Palette.Entries).Slice(0, 1024), 1024);
                    break;
                default:
                    Result = false;
                    break;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }
}

// the abstract surface. GetWidth and GetHeight are abstract
public partial class TSurface : TObject
{
    // GFXE.pas:11964
    public virtual int GetColor(int X, int Y)
    {
        int Result = default;
        TSurfaceDesc SD = default;
        Ptr<byte> P;
        int V;
        ushort W;
        byte Index;
        if (Succeeded(Lock(ref SD)))
        {
            try
            {
                Assert(X >= 0 && X < SD.Width && Y >= 0 && Y < SD.Height, "Coordinates out of bounds!", "GFXE.pas", 11974);
                // { 8 bit pixels need the palette; 1555 and the TColor ordered 24 bit layout are not handled }
                switch (SD.PixelFormatID)
                {
                    case GFXE.pfRGB555:
                        W = MemoryMarshal.Read<ushort>((BytePointer(SD.Data) + (X + X) + Y * SD.Pitch).AsSpan(2));
                        Result = (int)(((uint)(W & 31744)) >> 7) + ((W & 992) << 6) + ((W & 31) << 19);
                        break;
                    case GFXE.pfRGB565:
                        W = MemoryMarshal.Read<ushort>((BytePointer(SD.Data) + (X + X) + Y * SD.Pitch).AsSpan(2));
                        Result = (int)(((uint)(W & 63488)) >> 8) + ((W & 2016) << 5) + ((W & 31) << 19);
                        break;
                    case GFXE.pfPAL8:
                        Assert(SD.Palette != null, "Palette not found, cannot retrieve color (GetColor)", "GFXE.pas", 11986);
                        Index = MemoryMarshal.Read<byte>((BytePointer(SD.Data) + X + Y * SD.Pitch).AsSpan(1));
                        P = Ptr<TPaletteEntry>.To(SD.Palette.Entries, Index).Reinterpret<byte>();
                        Result = P[0] + (P[1] << 8) + (P[2] << 16);
                        break;
                    case GFXE.pfRGB24:
                        P = Ptr<byte>.Unbox((BytePointer(SD.Data) + (X + X + X) + Y * SD.Pitch).Box());
                        Result = P[2] + (P[1] << 8) + (P[0] << 16);
                        break;
                    case GFXE.pfRGB32:
                        V = MemoryMarshal.Read<int>((BytePointer(SD.Data) + (X << 2) + Y * SD.Pitch).AsSpan(4));
                        Result = (int)(((uint)(V & 16711680)) >> 16) + (V & 65280) + ((V & 255) << 16);
                        break;
                    default:
                        Assert(false, "Pixel format not supported (GetColor)", "GFXE.pas", 11999);
                        Result = 0;
                        break;
                }
            }
            finally
            {
                Unlock();
            }
        }
        else
        {
            Assert(false, "Cannot Lock Surface", "GFXE.pas", 12004);
            Result = -1;
        }

        return Result;
    }

    // GFXE.pas:11924
    // { The original unit was longer: the line numbers compiled into the Assert calls (11932 for the next Assert below) prove that 737 more lines preceded this point. Their content is unknown; the blank lines below keep the Assert line numbers identical. }
    public virtual int GetPixelInfo(int X, int Y)
    {
        int Result = default;
        TSurfaceDesc SD = default;
        object P;
        if (Succeeded(Lock(ref SD)))
        {
            try
            {
                Assert(X >= 0 && X < SD.Width && Y >= 0 && Y < SD.Height, "Coordinates out of bounds!", "GFXE.pas", 11932);
                switch (SD.BitCount)
                {
                    case 8:
                        Result = MemoryMarshal.Read<byte>((BytePointer(SD.Data) + X + Y * SD.Pitch).AsSpan(1));
                        break;
                    case 16:
                        Result = MemoryMarshal.Read<ushort>((BytePointer(SD.Data) + (X + X) + Y * SD.Pitch).AsSpan(2));
                        break;
                    case 24:
                        if (X + 1 < SD.Width)
                        {
                            Result = MemoryMarshal.Read<int>((BytePointer(SD.Data) + (X + X + X) + Y * SD.Pitch).AsSpan(4)) & 16777215;
                        }
                        else
                        {
                            P = (BytePointer(SD.Data) + (X + X + X) + Y * SD.Pitch).Box();
                            Result = MemoryMarshal.Read<ushort>(BytePointer(P).AsSpan(2)) + (MemoryMarshal.Read<byte>((BytePointer(P) + 2).AsSpan(1)) << 16);
                        }

                        break;
                    case 32:
                        Result = MemoryMarshal.Read<int>((BytePointer(SD.Data) + (X << 2) + Y * SD.Pitch).AsSpan(4));
                        break;
                    default:
                        Assert(false, "Pixel format not supported (GetColor)", "GFXE.pas", 11946);
                        Result = 0;
                        break;
                }
            }
            finally
            {
                Unlock();
            }
        }
        else
        {
            Assert(false, "Cannot Lock Surface", "GFXE.pas", 11951);
            Result = -1;
        }

        return Result;
    }

    // GFXE.pas:398
    public virtual int GetWidth()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // GFXE.pas:399
    public virtual int GetHeight()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // GFXE.pas:11954
    public virtual int GetPixelFormatID()
    {
        int Result;
        TSurfaceDesc SD = default;
        if (Succeeded(Lock(ref SD)))
        {
            Result = SD.PixelFormatID;
            Unlock();
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // GFXE.pas:12007
    public virtual int Blt(TSurface Dest, Ptr<TRect> SrcRect, int X, int Y)
    {
        int Result;
        TSurfaceDesc DSD = default;
        TSurfaceDesc SSD = default;
        TRect[] R__cell = new TRect[1];
        ref TRect R = ref R__cell[0];
        object S;
        object D;
        if (Dest.PixelFormatID == PixelFormatID)
        {
            if (Succeeded(Dest.Lock(ref DSD)))
            {
                if (Succeeded(Lock(ref SSD)))
                {
                    if (SrcRect.IsNull)
                    {
                        R.Left = 0;
                        R.Right = SSD.Width;
                        R.Top = 0;
                        R.Bottom = SSD.Height;
                        SrcRect = Ptr<TRect>.To(R__cell, 0);
                    }

                    S = (BytePointer(SSD.Data) + SrcRect.Value.Left * SSD.BitCount / 8 + SrcRect.Value.Top * SSD.Pitch).Box();
                    D = (BytePointer(DSD.Data) + X * DSD.BitCount / 8 + Y * DSD.Pitch).Box();
                    GFXE.BltRect(S, D, SSD.Pitch, DSD.Pitch, (SrcRect.Value.Right - SrcRect.Value.Left) * DSD.BitCount / 8, SrcRect.Value.Bottom - SrcRect.Value.Top);
                    Unlock();
                    Result = GFXE.GFXE_OK;
                }
                else
                {
                    Result = GFXE.GFXE_CANNOTLOCK;
                }

                Dest.Unlock();
            }
            else
            {
                Result = GFXE.GFXE_CANNOTLOCK;
            }
        }
        else
        {
            Result = GFXE.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:12045
    public virtual int BltClipped(TSurface Dest, Ptr<TRect> SrcRect, int X, int Y)
    {
        int Result;
        int W;
        int H;
        TRect[] R__cell = new TRect[1];
        ref TRect R = ref R__cell[0];
        if (SrcRect.IsNull)
        {
            R.Left = 0;
            var width = GetWidth();
            R.Right = width;
            R.Top = 0;
            var height = GetHeight();
            R.Bottom = height;
        }
        else
        {
            R = SrcRect.Value;
            var width = GetWidth();
            if (R.Right > width)
            {
                var width2 = GetWidth();
                R.Right = width2;
            }

            if (R.Left < 0)
            {
                X = X - R.Left;
                R.Left = 0;
            }
        }

        if (X < 0)
        {
            R.Left = R.Left - X;
            X = 0;
        }

        W = R.Right - R.Left;
        if (Dest.GetWidth() < X + W)
        {
            W = W - (X + W - Dest.GetWidth());
        }

        var height2 = GetHeight();
        if (R.Bottom > height2)
        {
            var height = GetHeight();
            R.Bottom = height;
        }

        if (R.Top < 0)
        {
            Y = Y - R.Top;
            R.Top = 0;
        }

        if (Y < 0)
        {
            R.Top = R.Top - Y;
            Y = 0;
        }

        H = R.Bottom - R.Top;
        if (Dest.GetHeight() < H + Y)
        {
            H = H - (H + Y - Dest.GetHeight());
        }

        if (W > 0 && H > 0)
        {
            R.Right = W + R.Left;
            R.Bottom = R.Top + H;
            Result = Blt(Dest, Ptr<TRect>.To(R__cell, 0), X, Y);
        }
        else
        {
            Result = GFXE.GFXE_OK;
        }

        return Result;
    }

    // GFXE.pas:12130
    public virtual int BltColorKey(TSurface Dest, Ptr<TRect> SrcRect, int X, int Y, int ColorKey)
    {
        int Result;
        TSurfaceDesc DSD = default;
        TSurfaceDesc SSD = default;
        TRect[] R__cell = new TRect[1];
        ref TRect R = ref R__cell[0];
        object S;
        object D;
        if (Dest.PixelFormatID == PixelFormatID && (PixelFormatID & 255) == 16)
        {
            if (Succeeded(Dest.Lock(ref DSD)))
            {
                if (Succeeded(Lock(ref SSD)))
                {
                    if (SrcRect.IsNull)
                    {
                        R.Left = 0;
                        R.Right = SSD.Width;
                        R.Top = 0;
                        R.Bottom = SSD.Height;
                        SrcRect = Ptr<TRect>.To(R__cell, 0);
                    }

                    S = (BytePointer(SSD.Data) + SrcRect.Value.Left * SSD.BitCount / 8 + SrcRect.Value.Top * SSD.Pitch).Box();
                    D = (BytePointer(DSD.Data) + X * DSD.BitCount / 8 + Y * DSD.Pitch).Box();
                    GFXE.BltColorKey16(S, D, SSD.Pitch, DSD.Pitch, SrcRect.Value.Right - SrcRect.Value.Left, SrcRect.Value.Bottom - SrcRect.Value.Top, ColorKey);
                    Result = GFXE.GFXE_OK;
                    Unlock();
                }
                else
                {
                    Result = GFXE.GFXE_CANNOTLOCK;
                }

                Dest.Unlock();
            }
            else
            {
                Result = GFXE.GFXE_CANNOTLOCK;
            }
        }
        else
        {
            Result = GFXE.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:12168
    public virtual int BltColorKeyClipped(TSurface Dest, Ptr<TRect> SrcRect, int X, int Y, int ColorKey)
    {
        int Result;
        int W;
        int H;
        TRect[] R__cell = new TRect[1];
        ref TRect R = ref R__cell[0];
        if (SrcRect.IsNull)
        {
            R.Left = 0;
            var width = GetWidth();
            R.Right = width;
            R.Top = 0;
            var height = GetHeight();
            R.Bottom = height;
        }
        else
        {
            R = SrcRect.Value;
            var width = GetWidth();
            if (R.Right > width)
            {
                var width2 = GetWidth();
                R.Right = width2;
            }

            if (R.Left < 0)
            {
                X = X - R.Left;
                R.Left = 0;
            }
        }

        if (X < 0)
        {
            R.Left = R.Left - X;
            X = 0;
        }

        W = R.Right - R.Left;
        if (Dest.GetWidth() < X + W)
        {
            W = W - (X + W - Dest.GetWidth());
        }

        var height2 = GetHeight();
        if (R.Bottom > height2)
        {
            var height = GetHeight();
            R.Bottom = height;
        }

        if (R.Top < 0)
        {
            Y = Y - R.Top;
            R.Top = 0;
        }

        if (Y < 0)
        {
            R.Top = R.Top - Y;
            Y = 0;
        }

        H = R.Bottom - R.Top;
        if (Dest.GetHeight() < H + Y)
        {
            H = H - (H + Y - Dest.GetHeight());
        }

        if (W > 0 && H > 0)
        {
            R.Right = W + R.Left;
            R.Bottom = R.Top + H;
            Result = BltColorKey(Dest, Ptr<TRect>.To(R__cell, 0), X, Y, ColorKey);
        }
        else
        {
            Result = GFXE.GFXE_OK;
        }

        return Result;
    }

    // GFXE.pas:12102
    public virtual int BltStretch(TSurface Dest, Ptr<TRect> SrcRect, Ptr<TRect> DestRect)
    {
        int Result;
        TRect[] R__cell = new TRect[1];
        ref TRect R = ref R__cell[0];
        TRect[] DR__cell = new TRect[1];
        ref TRect DR = ref DR__cell[0];
        if (SrcRect.IsNull)
        {
            R.Left = 0;
            var width = GetWidth();
            R.Right = width;
            R.Top = 0;
            var height = GetHeight();
            R.Bottom = height;
            SrcRect = Ptr<TRect>.To(R__cell, 0);
        }

        if (DestRect.IsNull)
        {
            DR.Left = 0;
            var width = Dest.GetWidth();
            DR.Right = width;
            DR.Top = 0;
            var height = Dest.GetHeight();
            DR.Bottom = height;
            DestRect = Ptr<TRect>.To(DR__cell, 0);
        }

        if (SrcRect.Value.Right - SrcRect.Value.Left == DestRect.Value.Right - DestRect.Value.Left && SrcRect.Value.Bottom - SrcRect.Value.Top == DestRect.Value.Bottom - DestRect.Value.Top)
        {
            Result = Blt(Dest, SrcRect, DestRect.Value.Left, DestRect.Value.Top);
        }
        else
        {
            Result = GFXE.GFXE_NOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:12225
    public virtual int Lock(ref TSurfaceDesc SD) => GFXE.GFXE_NOTSUPPORTED;

    // GFXE.pas:12248
    public virtual int Unlock() => GFXE.GFXE_NOTSUPPORTED;

    // GFXE.pas:12230
    public virtual int LockEx(ref TSurfaceDesc SD, in TRect R)
    {
        int Result;
        bool condition = R.Left >= 0 && R.Top >= 0;
        if (condition)
        {
            var height = GetHeight();
            condition = R.Bottom <= height;
        }

        bool test = condition;
        if (test)
        {
            var width = GetWidth();
            test = R.Right <= width;
        }

        if (test)
        {
            Result = Lock(ref SD);
            if (Succeeded(Result))
            {
                SD.Data = (BytePointer(SD.Data) + (int)(((uint)(R.Left * SD.BitCount)) >> 3) + R.Top * SD.Pitch).Box();
                SD.Width = R.Right - R.Left;
                SD.Height = R.Bottom - R.Top;
                SD.ClipRect.Right = SD.Width;
                SD.ClipRect.Bottom = SD.Height;
            }
        }
        else
        {
            Result = GFXE.GFXE_ERROR;
        }

        return Result;
    }

    // GFXE.pas:12253
    public virtual int UnlockEx(in TSurfaceDesc SD) => Unlock();

    // GFXE.pas:12220
    public virtual int UpdatePalette() => GFXE.GFXE_NOTSUPPORTED;

    // GFXE.pas:12258
    public virtual int Resize(int AWidth, int AHeight) => GFXE.GFXE_NOTSUPPORTED;

    public int Width { get => GetWidth(); }
    public int Height { get => GetHeight(); }
    public int PixelFormatID { get => GetPixelFormatID(); }
}

// a surface in memory. FWidth, FHeight, FMem, FData (FMem aligned to 8), FBitCount (Byte),
// FPitch, FPixelFormatID, FX, FY (origin of the captured rectangle), FPalette
public partial class TDataSurface : TSurface
{

    internal int FWidth;
    internal int FHeight;
    internal Ptr<byte> FMem;
    internal object FData;
    internal byte FBitCount;
    internal int FPitch;
    internal int FPixelFormatID;
    internal int FX;
    internal int FY;
    internal TGFXEPalette FPalette;
    // GFXE.pas:12387
    public TDataSurface(int AWidth, int AHeight, int APixelFormatID, TGFXEPalette APalette)
    {
        FBitCount = (byte)(APixelFormatID & 255);
        FPixelFormatID = APixelFormatID;
        SetPalette(APalette);
        Resize(AWidth, AHeight);
    }

    // GFXE.pas:12396
    public TDataSurface(TSurface Surface, Ptr<TRect> R)
    {
        CaptureSurface(Surface, R);
    }

    // GFXE.pas:12402
    public override void Destroy()
    {
        FPalette.Free();
        FPalette = null;
        FMem = FMem.Realloc(0);
        base.Destroy();
    }

    // GFXE.pas:12410
    public override int GetPixelFormatID() => FPixelFormatID;

    // GFXE.pas:12415
    public override int GetWidth() => FWidth;

    // GFXE.pas:12420
    public override int GetHeight() => FHeight;

    // GFXE.pas:12441
    public override int Unlock() => GFXE.GFXE_OK;

    // GFXE.pas:12446
    public int CaptureSurface(TSurface Surface, Ptr<TRect> R)
    {
        TSurfaceDesc SD = default;
        TRect[] RR__cell = new TRect[1];
        ref TRect RR = ref RR__cell[0];
        int W;
        int H;
        int Result = Surface.Lock(ref SD);
        if (Succeeded(Result))
        {
            try
            {
                FBitCount = (byte)SD.BitCount;
                FPixelFormatID = SD.PixelFormatID;
                if (R.IsNull)
                {
                    RR.Left = 0;
                    RR.Right = SD.Width;
                    RR.Top = 0;
                    RR.Bottom = SD.Height;
                    R = Ptr<TRect>.To(RR__cell, 0);
                }

                W = R.Value.Right - R.Value.Left;
                H = R.Value.Bottom - R.Value.Top;
                if (W > 0 && H > 0)
                {
                    Resize(W, H);
                    SetPalette(SD.Palette);
                    FX = R.Value.Left;
                    FY = R.Value.Top;
                    GFXE.BltRect((BytePointer(SD.Data) + FY * SD.Pitch + (int)(((uint)(FX * FBitCount)) >> 3)).Box(), FData, SD.Pitch, FPitch, (int)(((uint)(FWidth * FBitCount)) >> 3), FHeight);
                }
            }
            catch
            {
                FX = 30;
            }

            Surface.Unlock();
            Result = GFXE.GFXE_OK;
        }

        return Result;
    }

    // GFXE.pas:12485
    public void SetPalette(TGFXEPalette Palette)
    {
        if (Palette != null)
        {
            if (FPalette == null)
            {
                FPalette = new TGFXEPalette();
            }

            FPalette.Assign(Palette);
        }
        else
        {
            FPalette.Free();
            FPalette = null;
        }
    }

    // GFXE.pas:12499
    public override int Resize(int AWidth, int AHeight)
    {
        int Result;
        int Size;
        if (AWidth != FWidth || AHeight != FHeight)
        {
            FWidth = AWidth;
            FHeight = AHeight;
            FPitch = ((int)(((uint)(FBitCount + 7)) >> 3) * FWidth + 7) & ~7;
            Size = FPitch * FHeight + 7;
            FMem = FMem.Realloc(Size);
            FData = AlignPointer(FMem + 7, ~7).Box();
            Result = GFXE.GFXE_OK;
        }
        else
        {
            Result = GFXE.GFXE_OK;
        }

        return Result;
    }

    public object Data { get => FData; }
    public int Pitch { get => FPitch; }
    public TGFXEPalette Palette { get => FPalette; set => SetPalette(value); }

}

// a DIB section of any format (8, 16 or 24 bit). FHandle, FWidth, FHeight, FMem, FBits, FPitch,
// FPixelFormatID, FInfo (BITMAPINFOHEADER and 256 colors), FPalette
public partial class TDIBSurface : TSurface
{
    internal int FHandle;
    internal int FWidth;
    internal int FHeight;
    internal Ptr<byte> FMem;
    internal object FBits;
    internal int FPitch;
    internal int FPixelFormatID;
    internal TBitmapInfoHeader FInfo;
    internal FixedArray_Aow_Rtl_TRGBQuad_256 FColors;
    internal TGFXEPalette FPalette;
    // GFXE.pas:6397
    public Ptr<TBitmapInfo> GetBITMAPINFO()
    {
        Ptr<TBitmapInfo> Result = default;
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        return Result;
    }

    // GFXE.pas:6402
    public Ptr<TBitmapInfoHeader> GetBITMAPINFOHEADER()
    {
        Ptr<TBitmapInfoHeader> Result = default;
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        return Result;
    }

    // GFXE.pas:6407
    public override int GetWidth() => FWidth;

    // GFXE.pas:6412
    public override int GetHeight() => FHeight;

    // GFXE.pas:6767
    public void SaveToBMP24(string FileName)
    {
        TSurfaceDesc SD = default;
        Lock(ref SD);
        GFXE.SaveSurfaceDescToBMP24(SD, SD.ClipRect, FileName);
        Unlock();
    }

    public object Bits { get => FBits; }
    public int Pitch { get => FPitch; }
    public TGFXEPalette Palette { get => FPalette; }
}

// an 8 bit bitmap in memory (bottom-up: the pitch is negative). FWidth, FHeight, FMem, FBits,
// FPitch, FInfo (header and 256 colors), FPalette
public partial class TDIB08Surface : TSurface
{
    internal int FWidth;
    internal int FHeight;
    internal Ptr<byte> FMem;
    internal object FBits;
    internal int FPitch;
    internal TBitmapInfoHeader FInfo;
    internal FixedArray_Aow_Rtl_TRGBQuad_256 FColors;
    internal TGFXEPalette FPalette;
    // GFXE.pas:6805
    public override int GetPixelFormatID() => GFXE.pfPAL8;

    // GFXE.pas:6810
    public override int GetWidth() => FWidth;

    // GFXE.pas:6815
    public override int GetHeight() => FHeight;

    public TGFXEPalette Palette { get => FPalette; set => SetPalette(value); }

}

// a 16 bit DIB section (555 or 565). FHandle, FWidth, FHeight, FBits, FPitch, FPixelFormatID,
// FInfo, FMasks
public partial class TDIB16Surface : TSurface
{
    internal int FHandle;
    internal int FWidth;
    internal int FHeight;
    internal object FBits;
    internal int FPitch;
    internal int FPixelFormatID;
    internal TBitmapInfoHeader FInfo;
    internal FixedArray_int_3 FMasks;
    // GFXE.pas:6991
    public override int GetWidth() => FWidth;

    // GFXE.pas:6996
    public override int GetHeight() => FHeight;

    // GFXE.pas:7001
    public override int GetPixelFormatID() => FPixelFormatID;

    public new int PixelFormatID { get => FPixelFormatID; set => SetPixelFormatID(value); }

}

// a 24 bit DIB section. FHandle, FWidth, FHeight, FBits, FPitch, FInfo
public partial class TDIB24Surface : TSurface
{
    internal int FHandle;
    internal int FWidth;
    internal int FHeight;
    internal object FBits;
    internal int FPitch;
    internal TBitmapInfoHeader FInfo;
    // GFXE.pas:7238
    public override int GetWidth() => FWidth;

    // GFXE.pas:7243
    public override int GetHeight() => FHeight;

    // GFXE.pas:7248
    public Ptr<TBitmapInfoHeader> GetBMPInfo()
    {
        Ptr<TBitmapInfoHeader> Result = default;
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        return Result;
    }

}

// a 32 bit bitmap in memory (bottom-up). FWidth, FHeight, FMem, FBits, FPitch, FInfo
public partial class TDIB32Surface : TSurface
{
    internal int FWidth;
    internal int FHeight;
    internal Ptr<byte> FMem;
    internal object FBits;
    internal int FPitch;
    internal TBitmapInfoHeader FInfo;
    // GFXE.pas:7446
    public override int GetWidth() => FWidth;

    // GFXE.pas:7451
    public override int GetHeight() => FHeight;

}

// a window drawn through GDI. FHandle. Two more virtuals: GetHandle, SetHandle
public partial class TGDISurface : TSurface
{

    internal int FHandle;
    // GFXE.pas:12293
    public TGDISurface(int AHandle)
    {
        FHandle = AHandle;
    }

    // GFXE.pas:12299
    public virtual int GetHandle() => FHandle;

    // GFXE.pas:12304
    public virtual void SetHandle(int Value)
    {
        FHandle = Value;
    }

    // GFXE.pas:12309
    public override int GetWidth()
    {
        /* TODO: Implement Windows.GetClientRect in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:12319
    public override int GetHeight()
    {
        /* TODO: Implement Windows.GetClientRect in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:12329
    public override int Blt(TSurface Dest, Ptr<TRect> SrcRect, int X, int Y) => GFXE.GFXE_NOTSUPPORTED;

    // GFXE.pas:12334
    public override int BltDC(int DC, Ptr<TRect> SrcRect, Ptr<TRect> DestRect)
    {
        /* TODO: Implement Windows.GetDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:12360
    public override int Resize(int AWidth, int AHeight)
    {
        /* TODO: Implement Windows.SetWindowPos in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:12368
    public override int Cls(int Color)
    {
        /* TODO: Implement Windows.GetDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    public int Handle { get => GetHandle(); set => SetHandle(value); }

}

// a DirectDraw surface. FOnSurfaceLost, FPixelFormatID, FWidth, FHeight, FSurface,
// FCreationFlags, FStatus, FLockCount, FLockExCount, FSurfaceDesc (the description of the lock)
public partial class TDDrawSurface : TSurface
{
    protected internal TNotifyEvent FOnSurfaceLost;
    protected internal int FPixelFormatID;
    protected internal int FWidth;
    protected internal int FHeight;
    protected internal IDirectDrawSurface2 FSurface;
    protected internal TSet<TDDSCreationFlagsElement> FCreationFlags;
    protected internal TSet<TDDrawSurfaceStatusElement> FStatus;
    protected internal int FLockCount;
    protected internal int FLockExCount;
    protected internal TSurfaceDesc FSurfaceDesc;
    // GFXE.pas:9279
    public override int GetPixelFormatID() => FPixelFormatID;

    // GFXE.pas:9284
    public override int GetWidth() => FWidth;

    // GFXE.pas:9289
    public override int GetHeight() => FHeight;

    public IDirectDrawSurface2 Surface { get => FSurface; }
    public TSet<TDDrawSurfaceStatusElement> Status { get => FStatus; }
    public TSet<TDDSCreationFlagsElement> CreationFlags { get => FCreationFlags; }
    public TNotifyEvent OnSurfaceLost { get => FOnSurfaceLost; set => FOnSurfaceLost = value; }
}

// Rect
public partial class TRectangle : TObject
{
    public TRect Rect;
    // GFXE.pas:2432
    public TRectangle(int ALeft, int ATop, int ARight, int ABottom)
    {
        Rect.Left = ALeft;
        Rect.Top = ATop;
        Rect.Right = ARight;
        Rect.Bottom = ABottom;
    }

    // GFXE.pas:2440
    public TRectangle(in TRect R)
    {
        Rect = R;
    }
}

// a TList of TRectangle, owning them
public partial class TRectangleList : TList
{
    // GFXE.pas:2458
    internal TRectangle GetRectangle(int Index) => (TRectangle)List[Index];

    // GFXE.pas:2447
    public override void Destroy()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            ((TRectangle)this[I]).Destroy();
            Delete(I);
        }
    }

    public sealed class RectanglesAccessor(TRectangleList target)
    {
        public TRectangle this[int Index] { get => target.GetRectangle(Index); }
    }

    public RectanglesAccessor Rectangles => new RectanglesAccessor(this);
}

public delegate void TOnDrawRectEvent(TObject Sender, in TRect R);
// a panel that paints through OnDraw. FEraseBkgndEnabled, FOnDraw
public partial class TDrawPanel : TCustomPanel
{
    internal bool FEraseBkgndEnabled;
    internal TOnDrawRectEvent FOnDraw;
    // GFXE.pas:8730
    protected internal override void WndProc(ref TMessage Message)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:8741
    protected internal override void Paint()
    {
        /* TODO: Implement Controls.ClientRect in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:8721

    public TDrawPanel(TComponent owner) : base(owner)
    {
        /* TODO: Implement ExtCtrls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    [Published]
    public TOnDrawRectEvent OnDraw { get => FOnDraw; set => FOnDraw = value; }

    [Published]
    public bool EraseBkgndEnabled { get => FEraseBkgndEnabled; set => FEraseBkgndEnabled = value; }
}

// an image file format, all class functions
public partial class TImageFile : TObject, IImageFileClass
{
    // GFXE.pas:693
    public static string Name => throw EAbstractError.Create("Abstract Error");

    // GFXE.pas:694
    public static string Extension => throw EAbstractError.Create("Abstract Error");

    // GFXE.pas:695
    public static string Description => throw EAbstractError.Create("Abstract Error");

    // GFXE.pas:696
    public static int GetPixelFormatID(string FileName)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // GFXE.pas:697
    public static TSurface CreateSurface(string FileName)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // GFXE.pas:698
    public static int LoadToSurfaceDesc(string FileName, ref TSurfaceDesc SD, int X, int Y)
    {
        throw EAbstractError.Create("Abstract Error");
    }


}

// a TOpenDialog whose filter lists the registered image file classes
public partial class TOpenImageFileDialog : TOpenDialog
{
    // GFXE.pas:972
    public override void Loaded()
    {
        /* TODO: Implement Dialogs.Filter in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:966

    public TOpenImageFileDialog(TComponent owner) : base(owner)
    {
        /* TODO: Implement Dialogs.Create in the platform or editor service. */
        throw new NotImplementedException();
    }
}

// PixelFormatID, Width, Height
public partial class TDisplayModeItem : TObject
{
    public int PixelFormatID;
    public int Width;
    public int Height;
}

// FList
public partial class TDisplayModeList : TObject
{
    internal TList FList;
    // GFXE.pas:12629
    internal TDisplayModeItem GetItems(int Index) => (TDisplayModeItem)FList.List[Index];

    // GFXE.pas:12634
    internal int GetCount() => FList.Count;

    // GFXE.pas:12611
    public TDisplayModeList()
    {
        FList = new TList();
    }

    // GFXE.pas:12617
    public override void Destroy()
    {
        Clear();
        FList.Free();
        base.Destroy();
    }

    // GFXE.pas:12624
    public void Clear()
    {
        while (FList.Count != 0)
        {
            Delete(FList.Count - 1);
        }
    }

    // GFXE.pas:12639
    public void Add(int AWidth, int AHeight, int APixelFormatID)
    {
        TDisplayModeItem Item;
        Item = new TDisplayModeItem();
        Item.Width = AWidth;
        Item.Height = AHeight;
        Item.PixelFormatID = APixelFormatID;
        FList.Add(Item);
    }

    // GFXE.pas:12650
    public void Delete(int Index)
    {
        ((TDisplayModeItem)FList[Index]).Free();
        FList.Delete(Index);
    }

    public sealed class ItemsAccessor(TDisplayModeList target)
    {
        public TDisplayModeItem this[int Index] { get => target.GetItems(Index); }
    }

    public ItemsAccessor Items => new ItemsAccessor(this);
    public int Count { get => GetCount(); }

}

// FBits, FTable
public partial class TBlendTable : TObject
{
    internal int FBits;
    internal object FTable;
    // GFXE.pas:741
    public virtual TBlendTable Copy()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // GFXE.pas:742
    public virtual void Assign(TBlendTable Source)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // GFXE.pas:9601
    public void Initialize(int ABits, object ATable)
    {
        FTable = ATable;
        FBits = ABits;
    }

    public int Bits { get => FBits; }
    public object Table { get => FTable; }
}

// Table (32 x 32 bytes)
public partial class TBlendTable5Bit : TBlendTable
{
    public new FixedArray_byte_32[] Table = new FixedArray_byte_32[32];
    // GFXE.pas:9609
    public TBlendTable5Bit()
    {
        Initialize(5, Ptr<FixedArray_byte_32>.To(Table, 0).Reinterpret<byte>().Box());
    }

    // GFXE.pas:9621
    public override TBlendTable Copy()
    {
        TBlendTable Result = new TBlendTable5Bit();
        Result.Assign(this);
        return Result;
    }

}

// FList
public partial class TBlendTableList : TObject
{
    internal TList FList;
    // GFXE.pas:9642
    internal int GetCount() => FList.Count;

    // GFXE.pas:9656
    internal TBlendTable GetTables(int Index) => (TBlendTable)FList.List[Index];

    // GFXE.pas:9629
    public TBlendTableList()
    {
        FList = new TList();
    }

    // GFXE.pas:9635
    public override void Destroy()
    {
        Clear();
        FList.Free();
        base.Destroy();
    }

    // GFXE.pas:9647
    public void Clear()
    {
        while (FList.Count != 0)
        {
            ((TBlendTable)FList.List[FList.Count - 1]).Free();
            FList.Delete(FList.Count - 1);
        }
    }

    // GFXE.pas:9661
    public void Add(TBlendTable Table)
    {
        FList.Add(Table);
    }

    // GFXE.pas:9666
    public void Delete(int Index)
    {
        ((TBlendTable)FList.List[Index]).Free();
        FList.Delete(Index);
    }

    public int Count { get => GetCount(); }

    public sealed class TablesAccessor(TBlendTableList target)
    {
        public TBlendTable this[int Index] { get => target.GetTables(Index); }
    }

    public TablesAccessor Tables => new TablesAccessor(this);

}

public delegate void TGenerateBlendTableProc(TBlendTable Table, float Value);
// FTables, Lookup (101 tables, indexed by percentage)
public partial class TLookupBlendTableCollection100 : TObject
{
    internal TBlendTableList FTables;
    // { ImageLib's blitters read Lookup directly; adding lines here moves the Asserts below }
    public FixedArray_Aow_Units_GFXE_TBlendTable_101 Lookup;
    // GFXE.pas:9687
    public TBlendTable GetLookupTables(int Index) => Lookup[Index];

    // GFXE.pas:9674
    public TLookupBlendTableCollection100()
    {
        FTables = new TBlendTableList();
    }

    // GFXE.pas:9680
    public override void Destroy()
    {
        Clear();
        FTables.Free();
        base.Destroy();
    }

    // GFXE.pas:9692
    public void Clear()
    {
        FTables.Clear();
        Lookup = default;
    }

    // GFXE.pas:9698
    public bool GenerateLookupTables(int Bits, TGenerateBlendTableProc Proc, int Count)
    {
        bool Result;
        TBlendTable T;
        float V;
        int J;
        Clear();
        if (Bits == 5 && Count > 0)
        {
            for (int I = 0; I <= Count - 1; I++)
            {
                switch (Bits)
                {
                    case 5:
                        T = new TBlendTable5Bit();
                        break;
                    default:
                        T = null;
                        break;
                }

                if (Count != 1)
                {
                    V = (float)((double)I / (Count - 1));
                }
                else
                {
                    V = (float)0.5;
                }

                Proc(T, V);
                FTables.Add(T);
            }

            for (int I = 0; I <= 100; I++)
            {
                J = I * Count / 100;
                if (J == Count)
                {
                    J = Count - 1;
                }

                var blendTable = FTables.Tables[J];
                Lookup[I] = blendTable;
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    public TBlendTableList Tables { get => FTables; }

    public sealed class LookupTablesAccessor(TLookupBlendTableCollection100 target)
    {
        public TBlendTable this[int Index] { get => target.GetLookupTables(Index); }
    }

    public LookupTablesAccessor LookupTables => new LookupTablesAccessor(this);

}

public delegate void TBltRectProc(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height);
public delegate void TProgressEvent(TObject Sender, int Percent, int Param);
// { The implementation list of image file classes (TList of TImageFileClass). }
public partial class TImageFileClassList : TList
{
    // GFXE.pas:941
    internal ImageFileClass GetClasses(int Index)
    {
        object P;
        P = this[Index];
        return (ImageFileClass)P;
    }

    // GFXE.pas:949
    public ImageFileClass FindClass(string Ext)
    {
        ImageFileClass Result;
        int I;
        I = 0;
        if (!IsEmpty(Ext) && Ext[0] == 46)
        {
            Ext = Copy(Ext, 2, Ext.Length - 1);
        }

        while (I != Count && CompareText(((ImageFileClass)List[I]).Extension, Ext) != 0)
        {
            I += 1;
        }

        if (I != Count)
        {
            Result = (ImageFileClass)List[I];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    public sealed class ClassesAccessor(TImageFileClassList target)
    {
        public ImageFileClass this[int Index] { get => target.GetClasses(Index); }
    }

    public ClassesAccessor Classes => new ClassesAccessor(this);
}

// { The blit timing test of GetOptimalGFXESettings }
public struct TBltTest
{
    public Ptr<byte> Src;
    public Ptr<byte> Dst;
    public int SrcPitch;
    public int DstPitch;
    public int Width;
    public int Height;
}

[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct TBltTestResult
{
    [FieldOffset(0)]
    public int Index;
    [FieldOffset(4)]
    public int Bandwidth;
}

// the Windows bitmap file format
public partial class TBMPImageFile : TImageFile, IImageFileClass
{
    // GFXE.pas:9043
    public static new string Extension => "BMP";

    // GFXE.pas:9048
    public static new string Description => "Bitmap files (*.bmp)";

    // GFXE.pas:9053
    public static new int GetPixelFormatID(string FileName) => GFXE.GetBMPPixelFormat(FileName);

    // GFXE.pas:9058
    public static new TSurface CreateSurface(string FileName) => GFXE.CreateSurfaceFromBMP(FileName);

    // GFXE.pas:9063
    public static new int LoadToSurfaceDesc(string FileName, ref TSurfaceDesc SD, int X, int Y) => GFXE.LoadBMP(FileName, ref SD, X, Y);

}

public delegate int Procedure_0(Ptr<TGUID> lpGUID, IDirectDraw lplpDD, IUnknown pUnkOuter);
public delegate int Procedure_1(object lpCallback, object lpContext);
public delegate int Procedure_2(int dwFlags, IDirectDrawClipper lplpDDClipper, IUnknown pUnkOuter);
public static partial class GFXE
{
    // The GFXE unit of Gfxepack.dpl (the graphics engine of Age of Wonders: surfaces, blitters,
    // blend tables, DirectDraw access). The original was D:\GEngine\GFXE\GFXMAIN\GFXE.pas with
    // inc\DIBSURF.PAS and inc\DDSurf.PAS included (the Assert line numbers tell). the
    // DirectDraw interfaces and GUIDs are declared in this unit itself (their type info and
    // typed constants sit in Gfxepack.dpl), without GUIDs on the interface types. The assembler
    // blitters are transcribed from the disassembly: `asm` blocks compile verbatim. the MMX
    // instructions are DB-encoded (Delphi 3 has no MMX mnemonics)
    // { HResult codes returned by the surfaces (GetErrorMsg) }
    public const byte GFXE_OK = 0;
    public const int GFXE_ERROR = -2147483647;
    public const int GFXE_NOTSUPPORTED = -2147483646;
    public const int GFXE_PIXELFORMATNOTSUPPORTED = -2147483645;
    public const int GFXE_BITMAPFORMATNOTSUPPORTED = -2147483644;
    public const int GFXE_CANNOTLOCK = -2147483643;
    // { Pixel format IDs (MASKtoPFID): bits 0-7 the bit count, bits 8-15 the channel order, the
//     high nibbles the bits per channel (A, B, G, R) }
    public const byte pfPAL8 = 8;
    public const int pfRGB555 = 1431343888;
    public const int pfARGB1555 = 1431409424;
    public const int pfRGB565 = 1448121104;
    public const int pfRGB24 = -2004839656;
    public const int pfBGR24 = -2004870376;
    public const int pfRGB32 = -2004315360;
    public const int pfXRGB32 = -2004839648;
    // { DirectDraw constants used here }
    public const byte DD_OK = 0;
    public const int DDERR_UNSUPPORTED = -2005532242;
    public const int DDERR_SURFACELOST = -2005532222;
    public const int DDERR_WASSTILLDRAWING = -2005532132;
    public const byte DDENUMRET_OK = 1;
    public const byte DDSD_CAPS = 1;
    public const byte DDSD_HEIGHT = 2;
    public const byte DDSD_WIDTH = 4;
    public const byte DDSD_PITCH = 8;
    public const short DDSD_PIXELFORMAT = 4096;
    public const byte DDSCAPS_OFFSCREENPLAIN = 64;
    public const short DDSCAPS_PRIMARYSURFACE = 512;
    public const short DDSCAPS_SYSTEMMEMORY = 2048;
    public const short DDSCAPS_VIDEOMEMORY = 16384;
    public const byte DDPF_RGB = 64;
    public const byte DDLOCK_WAIT = 1;
    public const short DDBLT_ASYNC = 512;
    public const short DDBLT_COLORFILL = 1024;
    public const int DDBLT_KEYSRC = 32768;
    public const int DDBLT_WAIT = 16777216;
    public const byte DDBLTFAST_NOCOLORKEY = 0;
    public const byte DDBLTFAST_SRCCOLORKEY = 1;
    public const byte DDBLTFAST_WAIT = 16;
    public const byte DDCKEY_SRCBLT = 8;
    public const byte DDGBS_CANBLT = 1;
    public const byte DDGFS_ISFLIPDONE = 2;
    public const byte DDSCL_NORMAL = 8;
    public static TGUID CLSID_DirectDraw = new TGUID
    {
        D1 = -675868960,
        D2 = 17216,
        D3 = 4559,
        D4 = [176, 99, 0, 32, 175, 194, 205, 53]
    };
    public static TGUID CLSID_DirectDrawClipper = new TGUID
    {
        D1 = 1496848288,
        D2 = 32179,
        D3 = 4559,
        D4 = [162, 222, 0, 170, 0, 185, 51, 86]
    };
    public static TGUID IID_IDirectDraw = new TGUID
    {
        D1 = 1813306240,
        D2 = 42803,
        D3 = 4558,
        D4 = [165, 33, 0, 32, 175, 11, 229, 96]
    };
    public static TGUID IID_IDirectDraw2 = new TGUID
    {
        D1 = -1280904224,
        D2 = 11075,
        D3 = 4559,
        D4 = [162, 222, 0, 170, 0, 185, 51, 86]
    };
    public static TGUID IID_IDirectDrawSurface = new TGUID
    {
        D1 = 1813306241,
        D2 = 42803,
        D3 = 4558,
        D4 = [165, 33, 0, 32, 175, 11, 229, 96]
    };
    public static TGUID IID_IDirectDrawSurface2 = new TGUID
    {
        D1 = 1468029061,
        D2 = 28396,
        D3 = 4559,
        D4 = [148, 65, 168, 35, 3, 193, 14, 39]
    };
    public static TGUID IID_IDirectDrawSurface3 = new TGUID
    {
        D1 = -637252096,
        D2 = 27058,
        D3 = 4560,
        D4 = [161, 213, 0, 170, 0, 184, 223, 187]
    };
    public static TGUID IID_IDirectDrawPalette = new TGUID
    {
        D1 = 1813306244,
        D2 = 42803,
        D3 = 4558,
        D4 = [165, 33, 0, 32, 175, 11, 229, 96]
    };
    public static TGUID IID_IDirectDrawClipper = new TGUID
    {
        D1 = 1813306245,
        D2 = 42803,
        D3 = 4558,
        D4 = [165, 33, 0, 32, 175, 11, 229, 96]
    };
    public static TGUID IID_IDirectDrawColorControl = new TGUID
    {
        D1 = 1268715232,
        D2 = 3454,
        D3 = 4560,
        D4 = [155, 6, 0, 160, 201, 3, 163, 184]
    };
    public static Procedure_0 DirectDrawCreate;
    public static Procedure_1 DirectDrawEnumerate;
    public static Procedure_2 DirectDrawCreateClipper;
    public static TLookupBlendTableCollection100 AlphaBlendTable;
    public static TLookupBlendTableCollection100 IntensityBlendTable;
    public static TLookupBlendTableCollection100 BrightenBlendTable;
    public static TLookupBlendTableCollection100 ShadowBlendTable;
    public static TLookupBlendTableCollection100 LinearAlphaBlendTable;
    // GFXE.pas:978
    public static string GetImageFilesFilter()
    {
        string Result = "";
        for (int I = 0, last = ImageFileClassList.Count - 1; I <= last; I++)
        {
            if (!IsEmpty(Result))
            {
                Result = Result + "|";
            }

            Result = Result + ImageFileClassList.Classes[I].Description + "|*." + ImageFileClassList.Classes[I].Extension;
        }

        return Result;
    }

    // GFXE.pas:991
    public static bool ValidImageFile(string FileName)
    {
        var extractFileExt = ExtractFileExt(FileName);
        return ImageFileClassList.FindClass(extractFileExt) != null;
    }

    // GFXE.pas:996
    public static int LoadImageFile(string FileName, ref TSurfaceDesc SD, int X, int Y)
    {
        int Result;
        ImageFileClass C;
        var extractFileExt = ExtractFileExt(FileName);
        C = ImageFileClassList.FindClass(extractFileExt);
        if (C != null)
        {
            Result = C.LoadToSurfaceDesc(FileName, ref SD, X, Y);
        }
        else
        {
            Result = GFXE_NOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:1007
    public static TSurface CreateSurfaceFromImageFile(string FileName)
    {
        TSurface Result;
        ImageFileClass C;
        var extractFileExt = ExtractFileExt(FileName);
        C = ImageFileClassList.FindClass(extractFileExt);
        if (C != null)
        {
            Result = C.CreateSurface(FileName);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // GFXE.pas:1018
    public static int GetImageFilePixelFormat(string FileName)
    {
        int Result;
        ImageFileClass C;
        var extractFileExt = ExtractFileExt(FileName);
        C = ImageFileClassList.FindClass(extractFileExt);
        if (C != null)
        {
            Result = C.GetPixelFormatID(FileName);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // GFXE.pas:1029
    public static void RegisterImageFileClasses(ReadOnlySpan<ImageFileClass> AClasses)
    {
        for (int I = 0, last = AClasses.Length - 1; I <= last; I++)
        {
            var extension = AClasses[I].Extension;
            if (ImageFileClassList.FindClass(extension) == null)
            {
                ImageFileClassList.Add(AClasses[I]);
            }
            else
            {
                throw Exception.Create("ImageFile class with " + AClasses[I].Extension + " extension is already registered");
            }
        }
    }

    // GFXE.pas:1656
    public static void GetOptimalGFXESettings(bool ShowDialog)
    {
        /* TODO: Implement Grids.Cells in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:1736
    public static void RegisterBltProc(TBltRectProc Proc, string Name)
    {
        if (BltProcCount != MaxBltProcs)
        {
            BltProcCount += 1;
            BltProcs[BltProcCount - 1] = Proc;
            BltProcNames[BltProcCount - 1] = Name;
        }
        else
        {
            throw Exception.Create("Maximum number of BltProc reached");
        }
    }

    // GFXE.pas:1748
    public static int BltSDRectClipped(in TSurfaceDesc Src, TRect SrcRect, in TSurfaceDesc Dest, int X, int Y)
    {
        int Result;
        int W;
        int H;
        object S;
        object D;
        if (SrcRect.Right > Src.ClipRect.Right)
        {
            SrcRect.Right = Src.ClipRect.Right;
        }

        if (SrcRect.Left < Src.ClipRect.Left)
        {
            X = X + (Src.ClipRect.Left - SrcRect.Left);
            SrcRect.Left = Src.ClipRect.Left;
        }

        if (X < Dest.ClipRect.Left)
        {
            SrcRect.Left = SrcRect.Left + (Dest.ClipRect.Left - X);
            X = Dest.ClipRect.Left;
        }

        W = SrcRect.Right - SrcRect.Left;
        if (X + W > Dest.ClipRect.Right)
        {
            W = W - (X + W - Dest.ClipRect.Right);
        }

        if (SrcRect.Bottom > Src.ClipRect.Bottom)
        {
            SrcRect.Bottom = Src.ClipRect.Bottom;
        }

        if (SrcRect.Top < Src.ClipRect.Top)
        {
            Y = Y + (Src.ClipRect.Top - SrcRect.Top);
            SrcRect.Top = Src.ClipRect.Top;
        }

        if (Dest.ClipRect.Top > Y)
        {
            SrcRect.Top = SrcRect.Top + (Dest.ClipRect.Top - Y);
            Y = Dest.ClipRect.Top;
        }

        H = SrcRect.Bottom - SrcRect.Top;
        if (Y + H > Dest.ClipRect.Bottom)
        {
            H = H - (Y + H - Dest.ClipRect.Bottom);
        }

        if (Src.PixelFormatID == Dest.PixelFormatID)
        {
            if (W > 0 && H > 0)
            {
                W = (int)(((uint)(Src.BitCount * W)) >> 3);
                S = (BytePointer(Src.Data) + Src.Pitch * SrcRect.Top + (int)(((uint)(Src.BitCount * SrcRect.Left)) >> 3)).Box();
                D = (BytePointer(Dest.Data) + Dest.Pitch * Y + (int)(((uint)(Dest.BitCount * X)) >> 3)).Box();
                BltRect(S, D, Src.Pitch, Dest.Pitch, W, H);
            }

            Result = GFXE_OK;
        }
        else
        {
            Result = GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:2141
    public static int BltSprite16(in TSurfaceDesc Src, TRect SrcRect, in TSurfaceDesc Dest, int X, int Y, int ColorKey)
    {
        int Result;
        int Key;
        object S;
        object D;
        if (Src.BitCount == 16 && Dest.PixelFormatID == Src.PixelFormatID)
        {
            Key = Color2PixelInfo(ColorKey, Src.PixelFormatID, null);
            S = (BytePointer(Src.Data) + Src.Pitch * SrcRect.Top + SrcRect.Left * 2).Box();
            D = (BytePointer(Dest.Data) + Dest.Pitch * Y + X * 2).Box();
            BltColorKey16(S, D, Src.Pitch, Dest.Pitch, SrcRect.Right - SrcRect.Left, SrcRect.Bottom - SrcRect.Top, Key);
            Result = GFXE_OK;
        }
        else
        {
            Result = GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:2158
    public static int BltSpriteClipped16(in TSurfaceDesc Src, TRect SrcRect, in TSurfaceDesc Dest, int X, int Y, int ColorKey)
    {
        int Result;
        int W;
        int H;
        object S;
        object D;
        int Key;
        if (Src.BitCount == 16 && Dest.BitCount == 16)
        {
            if (SrcRect.Right > Src.ClipRect.Right)
            {
                SrcRect.Right = Src.ClipRect.Right;
            }

            if (SrcRect.Left < Src.ClipRect.Left)
            {
                X = X + (Src.ClipRect.Left - SrcRect.Left);
                SrcRect.Left = Src.ClipRect.Left;
            }

            if (X < Dest.ClipRect.Left)
            {
                SrcRect.Left = SrcRect.Left + (Dest.ClipRect.Left - X);
                X = Dest.ClipRect.Left;
            }

            W = SrcRect.Right - SrcRect.Left;
            if (W + X > Dest.ClipRect.Right)
            {
                W = W - (W + X - Dest.ClipRect.Right);
            }

            if (SrcRect.Bottom > Src.ClipRect.Bottom)
            {
                SrcRect.Bottom = Src.ClipRect.Bottom;
            }

            if (SrcRect.Top < Src.ClipRect.Top)
            {
                Y = Y + (Src.ClipRect.Top - SrcRect.Top);
                SrcRect.Top = Src.ClipRect.Top;
            }

            if (Y < Dest.ClipRect.Top)
            {
                SrcRect.Top = SrcRect.Top + (Dest.ClipRect.Top - Y);
                Y = Dest.ClipRect.Top;
            }

            H = SrcRect.Bottom - SrcRect.Top;
            if (Y + H > Dest.ClipRect.Bottom)
            {
                H = H - (Y + H - Dest.ClipRect.Bottom);
            }

            if (W > 0 && H > 0)
            {
                Key = Color2PixelInfo(ColorKey, Src.PixelFormatID, null);
                S = (BytePointer(Src.Data) + Src.Pitch * SrcRect.Top + SrcRect.Left * 2).Box();
                D = (BytePointer(Dest.Data) + Dest.Pitch * Y).Box();
                X += X;
                D = (BytePointer(D) + X).Box();
                BltColorKey16(S, D, Src.Pitch, Dest.Pitch, W, H, Key);
            }

            Result = GFXE_OK;
        }
        else
        {
            Result = GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:2326
    public static int GetPixelInfo(in TSurfaceDesc SD, int X, int Y)
    {
        int Result;
        switch (SD.BitCount)
        {
            case 8:
                Result = MemoryMarshal.Read<byte>((BytePointer(SD.Data) + X + SD.Pitch * Y).AsSpan(1));
                break;
            case 16:
                Result = MemoryMarshal.Read<ushort>((BytePointer(SD.Data) + X + X + SD.Pitch * Y).AsSpan(2));
                break;
            case 24:
                Result = MemoryMarshal.Read<int>((BytePointer(SD.Data) + X * 3 + SD.Pitch * Y).AsSpan(4)) & 16777215;
                break;
            case 32:
                Result = MemoryMarshal.Read<int>((BytePointer(SD.Data) + X * 4 + SD.Pitch * Y).AsSpan(4));
                break;
            default:
                throw Exception.Create("PixelFormat is not supported");
        }

        return Result;
    }

    // GFXE.pas:2340
    // { The pixel format ID: bits 0-7 the bit count, bits 8-15 the position (0-3) of each channel
//   (A, B, G, R) counted from the low end, bits 16-31 the bits per channel (A, B, G, R). }
    public static int MASKtoPFID(int BitCount, int RMask, int GMask, int BMask, int AMask)
    {
        FixedArray_byte_4 Order = default;
        bool Swapped;
        FixedArray_int_4 Masks = default;
        int Bits;
        int T;
        int Result = BitCount;
        Masks[0] = AMask;
        Order[3] = 3;
        Masks[1] = BMask;
        Order[0] = 0;
        Masks[2] = GMask;
        Order[1] = 1;
        Masks[3] = RMask;
        Order[2] = 2;
        do
        {
            Swapped = false;
            for (int I = 0; I <= 2; I++)
            {
                if (((Masks[Order[I]] < 0 || Masks[Order[I + 1]] < Masks[Order[I]]) && Masks[Order[I + 1]] > 0) || (Masks[Order[I]] == 0 && Masks[Order[I + 1]] != 0))
                {
                    T = Order[I];
                    Order[I] = Order[I + 1];
                    Order[I + 1] = (byte)T;
                    Swapped = true;
                }
            }
        }
        while (!!Swapped);
        for (int I = 0; I <= 3; I++)
        {
            Result = Result + (I << (Order[I] * 2 + 8));
        }

        for (int I = 0; I <= 3; I++)
        {
            Bits = 0;
            while (Masks[I] != 0)
            {
                if ((Masks[I] & 1) != 0)
                {
                    Bits += 1;
                }

                Masks[I] = (int)(((uint)Masks[I]) >> 1);
            }

            Result = Result + (Bits << (I * 4 + 16));
        }

        return Result;
    }

    // GFXE.pas:2382
    public static int PFIDtoMASK(int PixelFormatID, ref int RMask, ref int GMask, ref int BMask, ref int AMask)
    {
        int BitCount;
        FixedArray_byte_4 Order;
        int Pos;
        FixedArray_int_4 Masks;
        int J;
        int Bits;
        int M;
        Order = default;
        Masks = default;
        BitCount = PixelFormatID & 255;
        Pos = 0;
        Order[((int)(((uint)PixelFormatID) >> 8)) & 3] = 0;
        Order[((int)(((uint)PixelFormatID) >> 10)) & 3] = 1;
        Order[((int)(((uint)PixelFormatID) >> 12)) & 3] = 2;
        Order[((int)(((uint)PixelFormatID) >> 14)) & 3] = 3;
        for (int I = 0; I <= 3; I++)
        {
            Bits = ((int)(((uint)PixelFormatID) >> (Order[I] * 4 + 16))) & 15;
            J = Bits;
            M = 0;
            while (J != 0)
            {
                M = M * 2 + 1;
                J -= 1;
            }

            M = M << Pos;
            Pos = Pos + Bits;
            Masks[Order[I]] = M;
        }

        AMask = Masks[0];
        BMask = Masks[1];
        GMask = Masks[2];
        RMask = Masks[3];
        return BitCount;
    }

    // GFXE.pas:2419
    public static void CreateSDfromSD(in TSurfaceDesc Src, in TRect R, ref TSurfaceDesc Dest)
    {
        Dest = Src;
        IntersectRect(ref Dest.ClipRect, Src.ClipRect, R);
        Dest.Data = (BytePointer(Src.Data) + R.Top * Src.Pitch + (int)(((uint)Src.BitCount) >> 3) * R.Left).Box();
        Dest.ClipRect.Left -= R.Left;
        Dest.ClipRect.Right -= R.Left;
        Dest.ClipRect.Top -= R.Top;
        Dest.ClipRect.Bottom -= R.Top;
    }

    // GFXE.pas:2465
    // { The rectangles of a surface: the maximal rectangles of TransparentColor bordered by other
//   colors, found from their top left corners. }
    public static TRectangleList GetSurfaceRectangles(TSurfaceDesc SD, int TransparentColor, TProgressEvent OnProgress)
    {
        TRectangleList Result;
        TRectangleList List;
        TRectangle R;
        ushort X;
        ushort Y;
        int GetPixel(int X, int Y)
        {
            int Result;
            object P;
            switch (SD.BitCount)
            {
                case 8:
                    Result = MemoryMarshal.Read<byte>((BytePointer(SD.Data) + X + SD.Pitch * Y).AsSpan(1));
                    break;
                case 16:
                    Result = MemoryMarshal.Read<ushort>((BytePointer(SD.Data) + X + X + SD.Pitch * Y).AsSpan(2));
                    break;
                case 24:
                    if (X + 1 < SD.Width)
                    {
                        Result = MemoryMarshal.Read<int>((BytePointer(SD.Data) + (X + X + X) + SD.Pitch * Y).AsSpan(4)) & 16777215;
                    }
                    else
                    {
                        P = (BytePointer(SD.Data) + (X + X + X) + SD.Pitch * Y).Box();
                        Result = MemoryMarshal.Read<ushort>(BytePointer(P).AsSpan(2)) + (MemoryMarshal.Read<byte>((BytePointer(P) + 2).AsSpan(1)) << 16);
                    }

                    break;
                case 32:
                    Result = MemoryMarshal.Read<int>((BytePointer(SD.Data) + (X << 2) + SD.Pitch * Y).AsSpan(4));
                    break;
                default:
                    throw Exception.Create("PixelFormat is not supported");
            }

            return Result;
        }

        TRectangle FindRectangle(int X, int Y)
        {
            int W;
            int H;
            int I;
            bool Ok;
            TRectangle R;
            R = null;
            W = 1;
            H = 1;
            while (GetPixel(X + W, Y) == TransparentColor && GetPixel(X + W, Y + 1) != TransparentColor && X + W < SD.Width - 1)
            {
                W += 1;
            }

            if (GetPixel(X + W, Y) == TransparentColor && GetPixel(X + W, Y + 1) == TransparentColor && X + W < SD.Width - 1 && W > 1)
            {
                while (GetPixel(X, Y + H) == TransparentColor && GetPixel(X + W, Y + H) == TransparentColor && GetPixel(X + 1, Y + H) != TransparentColor && GetPixel(X + W - 1, Y + H) != TransparentColor && Y + H < SD.Height)
                {
                    H += 1;
                }

                if (GetPixel(X, Y + H) == TransparentColor && GetPixel(X + 1, Y + H) == TransparentColor && GetPixel(X + W, Y + H) == TransparentColor && GetPixel(X + W - 1, Y + H) == TransparentColor && Y + H < SD.Height && H > 1)
                {
                    Ok = true;
                    I = 1;
                    while (Ok && I != W)
                    {
                        Ok = GetPixel(X + I, Y + H) == TransparentColor;
                        I += 1;
                    }

                    if (Ok)
                    {
                        R = new TRectangle(X + 1, Y + 1, X + W, Y + H);
                    }
                }
            }

            return R;
        }

        List = new TRectangleList();
        for (int yOrdinal = 0, last = SD.Height - 1; yOrdinal <= last; yOrdinal++)
        {
            Y = (ushort)yOrdinal;
            for (int xOrdinal = 0, lastXOrdinal = SD.Width - 1; xOrdinal <= lastXOrdinal; xOrdinal++)
            {
                X = (ushort)xOrdinal;
                if (GetPixel(X, Y) == TransparentColor)
                {
                    R = FindRectangle(X, Y);
                    if (R != null)
                    {
                        List.Add(R);
                    }
                }
            }

            if (OnProgress != null && (Y % 20 == 0 || Y == SD.Height - 1))
            {
                OnProgress(null, Round((double)(Y * 100) / (SD.Height - 1)), 0);
            }
        }

        if (List.Count == 0)
        {
            List.Free();
            Result = null;
        }
        else
        {
            Result = List;
        }

        return Result;
    }

    // GFXE.pas:2547
    // { Shrinks R to the part of the surface that is not TransparentColor (MinimizeTransparentRect)
//   or that is TransparentColor (MinimizeInvTransparentRect). }
    public static void MinimizeTransparentRect(in TSurfaceDesc SD, in TRect R, int TransparentColor, ref TRect Result)
    {
        TSurfaceDesc SD__value = SD;
        bool Found;
        int X;
        int Y;
        TRect Rect;
        int GetPixel(int X, int Y)
        {
            int Result;
            switch (SD__value.BitCount)
            {
                case 8:
                    Result = MemoryMarshal.Read<byte>((BytePointer(SD__value.Data) + X + SD__value.Pitch * Y).AsSpan(1));
                    break;
                case 16:
                    Result = MemoryMarshal.Read<ushort>((BytePointer(SD__value.Data) + X + X + SD__value.Pitch * Y).AsSpan(2));
                    break;
                case 24:
                    Result = MemoryMarshal.Read<int>((BytePointer(SD__value.Data) + X * 3 + SD__value.Pitch * Y).AsSpan(4)) & 16777215;
                    break;
                case 32:
                    Result = MemoryMarshal.Read<int>((BytePointer(SD__value.Data) + X * 4 + SD__value.Pitch * Y).AsSpan(4));
                    break;
                default:
                    throw Exception.Create("PixelFormat is not supported");
            }

            return Result;
        }

        Rect = R;
        Found = false;
        while (!Found && Rect.Top < Rect.Bottom)
        {
            X = Rect.Left;
            while (!Found && X != Rect.Right)
            {
                Found = GetPixel(X, Rect.Top) != TransparentColor;
                X += 1;
            }

            if (!Found)
            {
                Rect.Top += 1;
            }
        }

        Found = false;
        while (!Found && Rect.Bottom > Rect.Top)
        {
            X = Rect.Left;
            while (!Found && X != Rect.Right)
            {
                Found = GetPixel(X, Rect.Bottom - 1) != TransparentColor;
                X += 1;
            }

            if (!Found)
            {
                Rect.Bottom -= 1;
            }
        }

        Found = false;
        while (!Found && Rect.Left < Rect.Right)
        {
            Y = Rect.Top;
            while (!Found && Y != Rect.Bottom)
            {
                Found = GetPixel(Rect.Left, Y) != TransparentColor;
                Y += 1;
            }

            if (!Found)
            {
                Rect.Left += 1;
            }
        }

        Found = false;
        while (!Found && Rect.Right > Rect.Left)
        {
            Y = Rect.Top;
            while (!Found && Y != Rect.Bottom)
            {
                Found = GetPixel(Rect.Right - 1, Y) != TransparentColor;
                Y += 1;
            }

            if (!Found)
            {
                Rect.Right -= 1;
            }
        }

        Result = Rect;
    }

    // GFXE.pas:2614
    public static void MinimizeInvTransparentRect(in TSurfaceDesc SD, in TRect R, int TransparentColor, ref TRect Result)
    {
        TSurfaceDesc SD__value = SD;
        bool Found;
        int X;
        int Y;
        TRect Rect;
        int GetPixel(int X, int Y)
        {
            int Result;
            switch (SD__value.BitCount)
            {
                case 8:
                    Result = MemoryMarshal.Read<byte>((BytePointer(SD__value.Data) + X + SD__value.Pitch * Y).AsSpan(1));
                    break;
                case 16:
                    Result = MemoryMarshal.Read<ushort>((BytePointer(SD__value.Data) + X + X + SD__value.Pitch * Y).AsSpan(2));
                    break;
                case 24:
                    Result = MemoryMarshal.Read<int>((BytePointer(SD__value.Data) + X * 3 + SD__value.Pitch * Y).AsSpan(4)) & 16777215;
                    break;
                case 32:
                    Result = MemoryMarshal.Read<int>((BytePointer(SD__value.Data) + X * 4 + SD__value.Pitch * Y).AsSpan(4));
                    break;
                default:
                    throw Exception.Create("PixelFormat is not supported");
            }

            return Result;
        }

        Rect = R;
        Found = false;
        while (!Found && Rect.Top < Rect.Bottom)
        {
            X = Rect.Left;
            while (!Found && X != Rect.Right)
            {
                Found = GetPixel(X, Rect.Top) == TransparentColor;
                X += 1;
            }

            if (!Found)
            {
                Rect.Top += 1;
            }
        }

        Found = false;
        while (!Found && Rect.Bottom > Rect.Top)
        {
            X = Rect.Left;
            while (!Found && X != Rect.Right)
            {
                Found = GetPixel(X, Rect.Bottom - 1) == TransparentColor;
                X += 1;
            }

            if (!Found)
            {
                Rect.Bottom -= 1;
            }
        }

        Found = false;
        while (!Found && Rect.Left < Rect.Right)
        {
            Y = Rect.Top;
            while (!Found && Y != Rect.Bottom)
            {
                Found = GetPixel(Rect.Left, Y) == TransparentColor;
                Y += 1;
            }

            if (!Found)
            {
                Rect.Left += 1;
            }
        }

        Found = false;
        while (!Found && Rect.Right > Rect.Left)
        {
            Y = Rect.Top;
            while (!Found && Y != Rect.Bottom)
            {
                Found = GetPixel(Rect.Right - 1, Y) == TransparentColor;
                Y += 1;
            }

            if (!Found)
            {
                Rect.Right -= 1;
            }
        }

        Result = Rect;
    }

    // GFXE.pas:2958
    // { Skip is not passed on: DrawLine16 always draws every pixel (original). }
    public static void DrawLine(int X1, int Y1, int X2, int Y2, int Skip, int Color, in TSurfaceDesc SD)
    {
        int Pixel;
        switch (SD.PixelFormatID)
        {
            case pfRGB555:
            case pfARGB1555:
            case pfRGB565:
                Pixel = Color2PixelInfo(Color, SD.PixelFormatID, null);
                DrawLine16(X1, Y1, X2, Y2, 0, (ushort)Pixel, SD);
                break;
        }
    }

    // GFXE.pas:8325
    public static void RemapSurface24to565(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height)
    {
        Ptr<byte> S;
        Ptr<ushort> D;
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            S = Ptr<byte>.Unbox((BytePointer(Src) + SrcPitch * Y).Box());
            D = Ptr<ushort>.Unbox((BytePointer(Dst) + DstPitch * Y).Box());
            for (int X = 0; X <= Width - 1; X++)
            {
                D.Value = (ushort)(((S[2] & 248) << 8) + ((S[1] & 252) << 3) + (int)(((uint)S[0]) >> 3));
                S = Ptr<byte>.Unbox((S.Reinterpret<byte>() + 3).Box());
                D = Ptr<ushort>.Unbox((D.Reinterpret<byte>() + 2).Box());
            }
        }
    }

    // GFXE.pas:8345
    public static void RemapSurface24to555(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height)
    {
        Ptr<byte> S;
        Ptr<ushort> D;
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            S = Ptr<byte>.Unbox((BytePointer(Src) + SrcPitch * Y).Box());
            D = Ptr<ushort>.Unbox((BytePointer(Dst) + DstPitch * Y).Box());
            for (int X = 0; X <= Width - 1; X++)
            {
                D.Value = (ushort)(((S[2] & 248) << 7) + ((S[1] & 248) << 2) + (int)(((uint)S[0]) >> 3));
                S = Ptr<byte>.Unbox((S.Reinterpret<byte>() + 3).Box());
                D = Ptr<ushort>.Unbox((D.Reinterpret<byte>() + 2).Box());
            }
        }
    }

    // GFXE.pas:8365
    public static void RemapSurface555to24(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height)
    {
        Ptr<byte> D;
        Ptr<ushort> S;
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            S = Ptr<ushort>.Unbox((BytePointer(Src) + SrcPitch * Y).Box());
            D = Ptr<byte>.Unbox((BytePointer(Dst) + DstPitch * Y).Box());
            for (int X = 0; X <= Width - 1; X++)
            {
                D[2] = (byte)(int)(((uint)(S.Value & 31744)) >> 7);
                D[1] = (byte)(int)(((uint)(S.Value & 992)) >> 2);
                D[0] = (byte)((S.Value & 31) << 3);
                D = Ptr<byte>.Unbox((D.Reinterpret<byte>() + 3).Box());
                S = Ptr<ushort>.Unbox((S.Reinterpret<byte>() + 2).Box());
            }
        }
    }

    // GFXE.pas:8386
    public static void RemapSurface565to24(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height)
    {
        Ptr<byte> D;
        Ptr<ushort> S;
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            S = Ptr<ushort>.Unbox((BytePointer(Src) + SrcPitch * Y).Box());
            D = Ptr<byte>.Unbox((BytePointer(Dst) + DstPitch * Y).Box());
            for (int X = 0; X <= Width - 1; X++)
            {
                D[2] = (byte)(int)(((uint)(S.Value & 63488)) >> 8);
                D[1] = (byte)(int)(((uint)(S.Value & 2016)) >> 3);
                D[0] = (byte)((S.Value & 31) << 3);
                D = Ptr<byte>.Unbox((D.Reinterpret<byte>() + 3).Box());
                S = Ptr<ushort>.Unbox((S.Reinterpret<byte>() + 2).Box());
            }
        }
    }

    // GFXE.pas:8408
    // { Both 32 bit converters produce 555 pixels from bytes 1-3 of the source (original). }
    public static void RemapSurface32to565(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height)
    {
        Ptr<byte> S;
        Ptr<ushort> D;
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            S = Ptr<byte>.Unbox((BytePointer(Src) + SrcPitch * Y).Box());
            D = Ptr<ushort>.Unbox((BytePointer(Dst) + DstPitch * Y).Box());
            for (int X = 0; X <= Width - 1; X++)
            {
                D.Value = (ushort)(((S[1] & 248) << 7) + ((S[2] & 248) << 2) + (int)(((uint)S[3]) >> 3));
                S = Ptr<byte>.Unbox((S.Reinterpret<byte>() + 4).Box());
                D = Ptr<ushort>.Unbox((D.Reinterpret<byte>() + 2).Box());
            }
        }
    }

    // GFXE.pas:8428
    public static void RemapSurface32to555(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height)
    {
        Ptr<byte> S;
        Ptr<ushort> D;
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            S = Ptr<byte>.Unbox((BytePointer(Src) + SrcPitch * Y).Box());
            D = Ptr<ushort>.Unbox((BytePointer(Dst) + DstPitch * Y).Box());
            for (int X = 0; X <= Width - 1; X++)
            {
                D.Value = (ushort)(((S[1] & 248) << 7) + ((S[2] & 248) << 2) + (int)(((uint)S[3]) >> 3));
                S = Ptr<byte>.Unbox((S.Reinterpret<byte>() + 4).Box());
                D = Ptr<ushort>.Unbox((D.Reinterpret<byte>() + 2).Box());
            }
        }
    }

    // GFXE.pas:8490
    public static void RemapSurface08to24(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height, TGFXEPalette Palette)
    {
        Ptr<byte> S;
        Ptr<byte> D;
        Ptr<TPaletteEntry> E;
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            S = Ptr<byte>.Unbox((BytePointer(Src) + SrcPitch * Y).Box());
            D = Ptr<byte>.Unbox((BytePointer(Dst) + DstPitch * Y).Box());
            for (int X = 0; X <= Width - 1; X++)
            {
                E = Ptr<TPaletteEntry>.To(Palette.Entries, S.Value);
                D[2] = E.Value.R;
                D[1] = E.Value.G;
                D[0] = E.Value.B;
                S = Ptr<byte>.Unbox((S.Reinterpret<byte>() + 1).Box());
                D = Ptr<byte>.Unbox((D.Reinterpret<byte>() + 3).Box());
            }
        }
    }

    // GFXE.pas:8513
    public static int RemapSurfacePart(in TSurfaceDesc Src, int SrcX, int SrcY, int Width, int Height, in TSurfaceDesc Dest, int DestX, int DestY)
    {
        object S;
        object D;
        int Result = GFXE_OK;
        if (SrcX + Width > Src.Width)
        {
            Width = Src.Width - SrcX;
        }

        if (SrcY + Height > Src.Height)
        {
            Height = Src.Height - SrcY;
        }

        if (SrcX < 0)
        {
            DestX = DestX - SrcX;
            Width = Width + SrcX;
            SrcX = 0;
        }

        if (SrcY < 0)
        {
            DestY = DestY - SrcY;
            Height = Height + SrcY;
            SrcY = 0;
        }

        if (DestX + Width > Dest.Width)
        {
            Width = Dest.Width - DestX;
        }

        if (DestY + Height > Dest.Height)
        {
            Height = Dest.Height - DestY;
        }

        if (DestX < 0)
        {
            SrcX = SrcX - DestX;
            Width = Width + DestX;
            DestX = 0;
        }

        if (DestY < 0)
        {
            SrcY = SrcY - DestY;
            Height = Height + DestY;
            DestY = 0;
        }

        if (Width > 0 && Height > 0)
        {
            S = (BytePointer(Src.Data) + Src.Pitch * SrcY + (int)(((uint)(Src.BitCount + 7)) >> 3) * SrcX).Box();
            D = (BytePointer(Dest.Data) + Dest.Pitch * DestY + (int)(((uint)(Dest.BitCount + 7)) >> 3) * DestX).Box();
            switch (Src.PixelFormatID)
            {
                case pfPAL8:
                    switch (Dest.PixelFormatID)
                    {
                        case pfRGB565:
                            RemapSurface08to565(S, D, Src.Pitch, Dest.Pitch, Width, Height, Src.Palette);
                            break;
                        case pfRGB555:
                            RemapSurface08to555(S, D, Src.Pitch, Dest.Pitch, Width, Height, Src.Palette);
                            break;
                        case pfRGB24:
                            RemapSurface08to24(S, D, Src.Pitch, Dest.Pitch, Width, Height, Src.Palette);
                            break;
                        case pfPAL8:
                            BltRect(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                            break;
                        default:
                            Result = GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case pfRGB555:
                case pfARGB1555:
                    switch (Dest.PixelFormatID)
                    {
                        case pfRGB24:
                            RemapSurface555to24(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                            break;
                        case pfRGB32:
                            RemapSurface555to32(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                            break;
                        case pfRGB565:
                            RemapSurface555to565(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                            break;
                        case pfRGB555:
                            BltRect(S, D, Src.Pitch, Dest.Pitch, Width * 2, Height);
                            break;
                        default:
                            Result = GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case pfRGB565:
                    switch (Dest.PixelFormatID)
                    {
                        case pfRGB24:
                            RemapSurface565to24(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                            break;
                        case pfRGB32:
                            RemapSurface565to32(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                            break;
                        case pfRGB555:
                            RemapSurface565to555(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                            break;
                        case pfRGB565:
                            BltRect(S, D, Src.Pitch, Dest.Pitch, Width * 2, Height);
                            break;
                        default:
                            Result = GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case pfRGB24:
                    switch (Dest.PixelFormatID)
                    {
                        case pfRGB565:
                            RemapSurface24to565(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                            break;
                        case pfRGB555:
                            RemapSurface24to555(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                            break;
                        case pfRGB24:
                            BltRect(S, D, Src.Pitch, Dest.Pitch, Width * 3, Height);
                            break;
                        default:
                            Result = GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case pfRGB32:
                    switch (Dest.PixelFormatID)
                    {
                        case pfRGB565:
                            RemapSurface32to565(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                            break;
                        case pfRGB555:
                            RemapSurface32to555(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                            break;
                        case pfRGB32:
                            BltRect(S, D, Src.Pitch, Dest.Pitch, Width * 4, Height);
                            break;
                        default:
                            Result = GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                default:
                    Result = GFXE_NOTSUPPORTED;
                    break;
            }
        }

        return Result;
    }

    // GFXE.pas:8600
    public static int RemapSurface(TSurface Src, TSurface Dest)
    {
        TSurfaceDesc SSD = default;
        TSurfaceDesc DSD = default;
        Src.Lock(ref SSD);
        Dest.Lock(ref DSD);
        int Result = RemapSurfacePart(SSD, 0, 0, SSD.Width, SSD.Height, DSD, 0, 0);
        Src.Unlock();
        Dest.Unlock();
        return Result;
    }

    // GFXE.pas:8637
    public static int PixelInfo2Color(int PixelInfo, int PixelFormatID, TGFXEPalette Palette) => RemapPixelInfo(PixelInfo, PixelFormatID, Palette, pfBGR24, null);

    // GFXE.pas:8642
    public static int RemapPixelInfo(int PixelInfo, int SrcPixelFormatID, TGFXEPalette SrcPalette, int DestPixelFormatID, TGFXEPalette DestPalette)
    {
        int Result;
        byte R;
        byte G;
        byte B;
        byte A;
        switch (SrcPixelFormatID)
        {
            case pfPAL8:
                if (SrcPalette != null)
                {
                    R = SrcPalette.Entries[PixelInfo].R;
                    G = SrcPalette.Entries[PixelInfo].G;
                    B = SrcPalette.Entries[PixelInfo].B;
                    A = 0;
                }
                else
                {
                    throw Exception.Create("Source Palette needed (GFXE)");
                }

                break;
            case pfRGB565:
                R = (byte)(((int)(((uint)PixelInfo) >> 8)) & 248);
                G = (byte)(((int)(((uint)PixelInfo) >> 3)) & 252);
                B = (byte)((PixelInfo << 3) & 248);
                A = 0;
                break;
            case pfRGB555:
                R = (byte)(((int)(((uint)PixelInfo) >> 7)) & 248);
                G = (byte)(((int)(((uint)PixelInfo) >> 2)) & 248);
                B = (byte)((PixelInfo << 3) & 248);
                A = 0;
                break;
            case pfARGB1555:
                R = (byte)(((int)(((uint)PixelInfo) >> 7)) & 248);
                G = (byte)(((int)(((uint)PixelInfo) >> 2)) & 248);
                B = (byte)((PixelInfo << 3) & 248);
                A = (byte)(((int)(((uint)PixelInfo) >> 8)) & 1);
                break;
            case pfRGB24:
                R = (byte)(((int)(((uint)PixelInfo) >> 16)) & 255);
                G = (byte)(((int)(((uint)PixelInfo) >> 8)) & 255);
                B = (byte)(PixelInfo & 255);
                A = 0;
                break;
            case pfBGR24:
                R = (byte)(PixelInfo & 255);
                G = (byte)(((int)(((uint)PixelInfo) >> 8)) & 255);
                B = (byte)(((int)(((uint)PixelInfo) >> 16)) & 255);
                A = 0;
                break;
            case pfRGB32:
                A = (byte)(((int)(((uint)PixelInfo) >> 24)) & 255);
                R = (byte)(((int)(((uint)PixelInfo) >> 16)) & 255);
                G = (byte)(((int)(((uint)PixelInfo) >> 8)) & 255);
                B = (byte)(PixelInfo & 255);
                break;
            default:
                throw Exception.Create("Source PixelFormat not supported");
        }

        switch (DestPixelFormatID)
        {
            case pfPAL8:
                if (DestPalette != null)
                {
                    Result = DestPalette.GetNearestColor(R + (G << 8) + (B << 16));
                }
                else
                {
                    throw Exception.Create("Destination Palette needed");
                }

                break;
            case pfRGB565:
                Result = ((R & 248) << 8) + ((G & 252) << 3) + (int)(((uint)(B & 248)) >> 3);
                break;
            case pfRGB555:
                Result = ((R & 248) << 7) + ((G & 248) << 2) + (int)(((uint)(B & 248)) >> 3);
                break;
            case pfARGB1555:
                Result = ((A & 128) << 8) + ((R & 248) << 7) + ((G & 248) << 2) + (int)(((uint)(B & 248)) >> 3);
                break;
            case pfRGB24:
                Result = (R << 16) + (G << 8) + B;
                break;
            case pfBGR24:
                Result = R + (G << 8) + (B << 16);
                break;
            case pfRGB32:
                Result = (A << 24) + (R << 16) + (G << 8) + B;
                break;
            default:
                throw Exception.Create("Destination PixelFormat not supported");
        }

        return Result;
    }

    // GFXE.pas:8923
    public static int SaveSurfaceDescToBMP24(in TSurfaceDesc SD, TRect R, string FileName)
    {
        /* TODO: Implement the untyped file stream buffer. */
        throw new NotImplementedException();
    }

    // GFXE.pas:9258
    public static int DDSDtoSurfaceDesc(in TDDSurfaceDesc DDSD, ref TSurfaceDesc SD)
    {
        int Result = DD_OK;
        SD = default;
        SD.Width = DDSD.dwWidth;
        SD.Height = DDSD.dwHeight;
        SD.Pitch = DDSD.lPitch;
        SD.BitCount = DDSD.ddpfPixelFormat.dwRGBBitCount;
        SD.ClipRect.Left = 0;
        SD.ClipRect.Right = DDSD.dwWidth;
        SD.ClipRect.Top = 0;
        SD.ClipRect.Bottom = DDSD.dwHeight;
        SD.Data = DDSD.lpSurface;
        var masKtoPFID = MASKtoPFID(DDSD.ddpfPixelFormat.dwRGBBitCount, DDSD.ddpfPixelFormat.dwRBitMask, DDSD.ddpfPixelFormat.dwGBitMask, DDSD.ddpfPixelFormat.dwBBitMask, DDSD.ddpfPixelFormat.dwRGBAlphaBitMask);
        SD.PixelFormatID = masKtoPFID;
        SD.RMask = DDSD.ddpfPixelFormat.dwRBitMask;
        SD.GMask = DDSD.ddpfPixelFormat.dwGBitMask;
        SD.BMask = DDSD.ddpfPixelFormat.dwBBitMask;
        SD.AMask = DDSD.ddpfPixelFormat.dwRGBAlphaBitMask;
        return Result;
    }

    // GFXE.pas:9517
    // { The 5 bit blend tables: Table[I, J] is the blend of source intensity I over destination
//   intensity J (0..31). }
    public static void GenerateAlphaBlendTable(TBlendTable Table, float Alpha)
    {
        if (Table is TBlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    ((TBlendTable5Bit)Table).Table[I][J] = (byte)Round((double)((double)I * Alpha) + (double)((double)1 - Alpha) * J);
                }
            }
        }
        else
        {
            throw Exception.Create("BlendTable format not supported");
        }
    }

    // GFXE.pas:9549
    public static void GenerateIntensityBlendTable(TBlendTable Table, float Intensity)
    {
        int V;
        float F;
        if (Table is TBlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    if (I == 0)
                    {
                        F = 0;
                    }
                    else
                    {
                        F = (float)((double)((double)((double)I * Intensity) * 1.75) / (I + J));
                    }

                    V = Round((double)I * F) + J;
                    if (V > 31)
                    {
                        V = 31;
                    }

                    ((TBlendTable5Bit)Table).Table[I][J] = (byte)V;
                }
            }
        }
    }

    // GFXE.pas:9568
    public static void GenerateShadowBlendTable(TBlendTable Table, float Shadow)
    {
        int V;
        float F;
        if (Table is TBlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    F = (float)((double)((double)(31 - I) * ((double)1 / 31)) * Shadow);
                    V = Round((double)((double)1 - F) * J);
                    if (V < 0)
                    {
                        V = 0;
                    }

                    if (V > J)
                    {
                        V = J;
                    }

                    ((TBlendTable5Bit)Table).Table[I][J] = (byte)V;
                }
            }
        }
    }

    // GFXE.pas:9585
    public static void GenerateBrightenBlendTable(TBlendTable Table, float Brighten)
    {
        int V;
        if (Table is TBlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    V = Round((double)((double)I * Brighten) + (double)((double)1 - Brighten) * J);
                    if (V < J)
                    {
                        V = J;
                    }

                    ((TBlendTable5Bit)Table).Table[I][J] = (byte)V;
                }
            }
        }
    }

    // GFXE.pas:12517
    public static string GetErrorMsg(int ErrorCode)
    {
        string Result;
        switch (ErrorCode)
        {
            case GFXE_OK:
                Result = "No ERROR occurred!";
                break;
            case GFXE_ERROR:
                Result = "Undefine ERROR occurred";
                break;
            case GFXE_NOTSUPPORTED:
                Result = "Function is not supported";
                break;
            case GFXE_PIXELFORMATNOTSUPPORTED:
                Result = "PixelFormat is not supported";
                break;
            case GFXE_BITMAPFORMATNOTSUPPORTED:
                Result = "Bitmap format is not supported";
                break;
            default:
                Result = "Undocumented Error (Error Code:" + IntToStr(ErrorCode) + ")";
                break;
        }

        return Result;
    }

    // GFXE.pas:12530
    public static void SaveGFXESettingsToStream(TStream Stream)
    {
    }

    // GFXE.pas:12534
    public static void LoadGFXESettingsFromStream(TStream Stream)
    {
    }

    // GFXE.pas:12538
    public static void WriteGFXESettingsToRegistry(string Key)
    {
        try
        {
        }
        finally
        {
        }
    }

    // GFXE.pas:12545
    public static void ReadGFXESettingsFromRegistry(string Key)
    {
        try
        {
        }
        finally
        {
        }
    }

    // GFXE.pas:12563
    public static int GetPrimarySurfacePFID()
    {
        int Result;
        TDDSurfaceDesc DDSD;
        IDirectDrawSurface S = default;
        if (DD() != null)
        {
            DDSD = default;
            DDSD.dwSize = 108;
            DDSD.dwFlags = DDSD_CAPS;
            DDSD.ddsCaps.dwCaps = DDSCAPS_PRIMARYSURFACE;
            DD().SetCooperativeLevel(0, DDSCL_NORMAL);
            if (Succeeded(DD().CreateSurface(DDSD, ref S, null)))
            {
                Result = DDStoPFID(S);
                S = null;
            }
            else
            {
                Result = 0;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // GFXE.pas:12587
    public static bool DisplayModeSupported(int Width, int Height, int PixelFormatID)
    {
        bool Result;
        if (DD() != null)
        {
            EnumDDSD = default;
            EnumDDSD.dwSize = 108;
            EnumDDSD.dwFlags = DDSD_HEIGHT | DDSD_WIDTH | DDSD_PIXELFORMAT;
            EnumDDSD.dwWidth = Width;
            EnumDDSD.dwHeight = Height;
            EnumDDSD.ddpfPixelFormat.dwSize = 32;
            EnumDDSD.ddpfPixelFormat.dwFlags = DDPF_RGB;
            var pfiDtoMASK = PFIDtoMASK(PixelFormatID, ref Unsafe.As<int, int>(ref EnumDDSD.ddpfPixelFormat.dwRBitMask), ref Unsafe.As<int, int>(ref EnumDDSD.ddpfPixelFormat.dwGBitMask), ref Unsafe.As<int, int>(ref EnumDDSD.ddpfPixelFormat.dwBBitMask), ref Unsafe.As<int, int>(ref EnumDDSD.ddpfPixelFormat.dwRGBAlphaBitMask));
            EnumDDSD.ddpfPixelFormat.dwRGBBitCount = pfiDtoMASK;
            ModeCount = 0;
            /* TODO: Implement cell or array backing for this address. */
            throw new NotImplementedException();
            Result = ModeCount != 0;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // GFXE.pas:12697
    public static void ForceOptimal16BitPixelFormatID(int PixelFormatID)
    {
        if (PixelFormatID == pfRGB565 || PixelFormatID == pfRGB555)
        {
            ForcedPixelFormatID = PixelFormatID;
        }
        else
        {
            ForcedPixelFormatID = 0;
        }
    }

    // GFXE.pas:12705
    public static int GetOptimalPixelFormatID(int BitCount)
    {
        /* TODO: Implement Forms.Width in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:12734
    public static int DDStoPFID(IDirectDrawSurface Surface)
    {
        int Result;
        TDDSurfaceDesc DDSD;
        if (Surface != null)
        {
            DDSD = default;
            DDSD.dwSize = 108;
            Result = Surface.Lock(default, ref DDSD, DDLOCK_WAIT, 0);
            if (Result >= 0)
            {
                Result = MASKtoPFID(DDSD.ddpfPixelFormat.dwRGBBitCount, DDSD.ddpfPixelFormat.dwRBitMask, DDSD.ddpfPixelFormat.dwGBitMask, DDSD.ddpfPixelFormat.dwBBitMask, DDSD.ddpfPixelFormat.dwRGBAlphaBitMask);
                Surface.Unlock(null);
            }
            else
            {
                Result = 0;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // GFXE.pas:12756
    public static bool DirectDrawActivated() => FDirectDrawActivated;

    // GFXE.pas:12787
    public static IDirectDraw2 DD()
    {
        IDirectDraw2 Result;
        if (!FDirectDrawActivated)
        {
            if (ActivateDirectDraw())
            {
                Result = FDirectDraw;
            }
            else
            {
                Result = null;
            }
        }
        else
        {
            Result = FDirectDraw;
        }

        return Result;
    }

    // GFXE.pas:12874
    public static void Register()
    {
        RegisterComponents("GFX Engine", [typeof(TDrawPanel), typeof(TOpenImageFileDialog)]);
    }

    public const byte MaxBltProcs = 20;
    public static int ForcedPixelFormatID;
    public static TImageFileClassList ImageFileClassList;
    public static FixedArray_Aow_Units_GFXE_TBltRectProc_21 BltProcs;
    public static FixedArray_string_21 BltProcNames = FixedArray_string_21.From([]);
    // { the blit routine indices chosen by GetOptimalGFXESettings: large and small blits, for
//     qword, dword, word and misaligned source/destination pairs }
    public static FixedArray_byte_8 OptimalBltProc;
    public static int BltProcCount;
    public static int ModeCount;
    public static TDDSurfaceDesc EnumDDSD;
    public static int ListPixelFormatID;
    public static bool FDirectDrawActivated;
    public static IDirectDraw2 FDirectDraw;
    public static bool PerformanceCounterAvailable;
    public static long PerformanceFrequency;
    // GFXE.pas:1508
    // // sic: the small misaligned entry (+7) is never used
    // { The four timing tests of GetOptimalGFXESettings: the source and destination of T are aligned
//   to 8 bytes and the destination offset by 0, 4, 2 or 1 bytes; every registered blit routine
//   is run Count times and the fastest one returned with its bandwidth. }
    public static TBltTestResult TestQWordAligned(TBltTest T, int Count)
    {
        TBltTestResult Res = default;
        int Start = default;
        int Stop = default;
        int BestTime;
        int Best;
        Best = -1;
        BestTime = 16777215;
        T.Src = AlignPointer(T.Src + 7, ~7);
        T.Dst = AlignPointer(T.Dst + 7, ~7);
        T.Width = T.Width - 8;
        if (T.Width > 0)
        {
            BltProcs[0](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
            for (int I = 0, last = BltProcCount - 1; I <= last; I++)
            {
                Start = GetElapsedMilliSeconds();
                for (int J = 0; J <= Count - 1; J++)
                {
                    BltProcs[I](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
                }

                Stop = GetElapsedMilliSeconds();
                if (Stop - Start < BestTime)
                {
                    BestTime = Stop - Start;
                    Best = I;
                }
            }

            Res.Index = Best;
            try
            {
                Res.Bandwidth = Round((double)(T.Width * T.Height) / ((double)((double)1000 / Count) * BestTime));
            }
            catch
            {
                Res.Bandwidth = 0;
            }
        }

        return Res;
    }

    // GFXE.pas:1545
    public static TBltTestResult TestDWordAligned(TBltTest T, int Count)
    {
        TBltTestResult Res = default;
        int Start = default;
        int Stop = default;
        int BestTime;
        int Best;
        Best = -1;
        BestTime = 16777215;
        T.Src = AlignPointer(T.Src + 7, ~7);
        T.Dst = AlignPointer(T.Dst + 7, ~7) + 4;
        T.Width = T.Width - 16;
        if (T.Width > 0)
        {
            BltProcs[0](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
            for (int I = 0, last = BltProcCount - 1; I <= last; I++)
            {
                Start = GetElapsedMilliSeconds();
                for (int J = 0; J <= Count - 1; J++)
                {
                    BltProcs[I](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
                }

                Stop = GetElapsedMilliSeconds();
                if (Stop - Start < BestTime)
                {
                    BestTime = Stop - Start;
                    Best = I;
                }
            }

            Res.Index = Best;
            try
            {
                Res.Bandwidth = Round((double)(T.Width * T.Height) / ((double)((double)1000 / Count) * BestTime));
            }
            catch
            {
                Res.Bandwidth = 0;
            }
        }

        return Res;
    }

    // GFXE.pas:1582
    public static TBltTestResult TestWordAligned(TBltTest T, int Count)
    {
        TBltTestResult Res = default;
        int Start = default;
        int Stop = default;
        int BestTime;
        int Best;
        Best = -1;
        BestTime = 16777215;
        T.Src = AlignPointer(T.Src + 7, ~7);
        T.Dst = AlignPointer(T.Dst + 7, ~7) + 2;
        T.Width = T.Width - 16;
        if (T.Width > 0)
        {
            BltProcs[0](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
            for (int I = 0, last = BltProcCount - 1; I <= last; I++)
            {
                Start = GetElapsedMilliSeconds();
                for (int J = 0; J <= Count - 1; J++)
                {
                    BltProcs[I](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
                }

                Stop = GetElapsedMilliSeconds();
                if (Stop - Start < BestTime)
                {
                    BestTime = Stop - Start;
                    Best = I;
                }
            }

            Res.Index = Best;
            try
            {
                Res.Bandwidth = Round((double)(T.Width * T.Height) / ((double)((double)1000 / Count) * BestTime));
            }
            catch
            {
                Res.Bandwidth = 0;
            }
        }

        return Res;
    }

    // GFXE.pas:1619
    public static TBltTestResult TestMissAligned(TBltTest T, int Count)
    {
        TBltTestResult Res = default;
        int Start = default;
        int Stop = default;
        int BestTime;
        int Best;
        Best = -1;
        BestTime = 16777215;
        T.Src = AlignPointer(T.Src + 7, ~7);
        T.Dst = AlignPointer(T.Dst + 7, ~7) + 1;
        T.Width = T.Width - 16;
        if (T.Width > 0)
        {
            BltProcs[0](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
            for (int I = 0, last = BltProcCount - 1; I <= last; I++)
            {
                Start = GetElapsedMilliSeconds();
                for (int J = 0; J <= Count - 1; J++)
                {
                    BltProcs[I](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
                }

                Stop = GetElapsedMilliSeconds();
                if (Stop - Start < BestTime)
                {
                    BestTime = Stop - Start;
                    Best = I;
                }
            }

            Res.Index = Best;
            try
            {
                Res.Bandwidth = Round((double)(T.Width * T.Height) / ((double)((double)1000 / Count) * BestTime));
            }
            catch
            {
                Res.Bandwidth = 0;
            }
        }

        return Res;
    }

    // GFXE.pas:8752
    // { The BMP file format: TBMPImageFile and its helpers. }
    public static int LoadBMP(string FileName, ref TSurfaceDesc SD, int X, int Y)
    {
        /* TODO: Implement the untyped file stream buffer. */
        throw new NotImplementedException();
    }

    // GFXE.pas:8968
    public static TSurface CreateSurfaceFromBMP(string FileName)
    {
        /* TODO: Implement the untyped file stream buffer. */
        throw new NotImplementedException();
    }

    // GFXE.pas:9009
    public static int GetBMPPixelFormat(string FileName)
    {
        /* TODO: Implement the untyped file stream buffer. */
        throw new NotImplementedException();
    }

    // GFXE.pas:9529
    public static void GenerateLinearAlphaBlendTable(TBlendTable Table, float Alpha)
    {
        float F;
        if (Table is TBlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                if ((double)Alpha > 0)
                {
                    F = (float)((double)I / ((double)31 * Alpha));
                }
                else
                {
                    F = 1;
                }

                if ((double)F > 1)
                {
                    F = 1;
                }

                for (int J = 0; J <= 31; J++)
                {
                    ((TBlendTable5Bit)Table).Table[I][J] = (byte)Round((double)((double)I * F) + (double)((double)1 - F) * J);
                }
            }
        }
        else
        {
            throw Exception.Create("BlendTable format not supported");
        }
    }

    // GFXE.pas:11039
    public static void InitBlendTables(int Bits, int Count)
    {
        AlphaBlendTable.GenerateLookupTables(Bits, GenerateAlphaBlendTable, Count);
        IntensityBlendTable.GenerateLookupTables(Bits, GenerateIntensityBlendTable, Count);
        BrightenBlendTable.GenerateLookupTables(Bits, GenerateBrightenBlendTable, Count);
        ShadowBlendTable.GenerateLookupTables(Bits, GenerateShadowBlendTable, Count);
        LinearAlphaBlendTable.GenerateLookupTables(Bits, GenerateLinearAlphaBlendTable, Count);
    }

    // GFXE.pas:12554
    // { DirectDraw: display modes and the DirectDraw object }
    public static int DisplayModeSupportedCallback(Ptr<TDDSurfaceDesc> DDSD, object Context)
    {
        if (EnumDDSD.ddpfPixelFormat.dwRBitMask == DDSD.Value.ddpfPixelFormat.dwRBitMask && EnumDDSD.ddpfPixelFormat.dwGBitMask == DDSD.Value.ddpfPixelFormat.dwGBitMask && EnumDDSD.ddpfPixelFormat.dwBBitMask == DDSD.Value.ddpfPixelFormat.dwBBitMask)
        {
            ModeCount += 1;
        }

        return DDENUMRET_OK;
    }

    // GFXE.pas:12657
    // { ListPixelFormatID is never set (original): every mode is listed. }
    public static int ListDisplayModesCallback(Ptr<TDDSurfaceDesc> DDSD, object Context)
    {
        int PFID;
        PFID = MASKtoPFID(DDSD.Value.ddpfPixelFormat.dwRGBBitCount, DDSD.Value.ddpfPixelFormat.dwRBitMask, DDSD.Value.ddpfPixelFormat.dwGBitMask, DDSD.Value.ddpfPixelFormat.dwBBitMask, DDSD.Value.ddpfPixelFormat.dwRGBAlphaBitMask);
        if (ListPixelFormatID == 0 || PFID == ListPixelFormatID)
        {
            ((TDisplayModeList)Context).Add(DDSD.Value.dwWidth, DDSD.Value.dwHeight, PFID);
        }

        return DDENUMRET_OK;
    }

    // GFXE.pas:12761
    public static bool ActivateDirectDraw()
    {
        /* TODO: Implement ActiveX.CoInitialize in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:12778
    public static void DeactivateDirectDraw()
    {
        if (FDirectDrawActivated)
        {
            FDirectDraw = null;
            CoUninitialize();
        }
    }

    public static void Initialize()
    {
        PerformanceCounterAvailable = QueryPerformanceFrequency(ref PerformanceFrequency);
        AlphaBlendTable = new TLookupBlendTableCollection100();
        IntensityBlendTable = new TLookupBlendTableCollection100();
        BrightenBlendTable = new TLookupBlendTableCollection100();
        ShadowBlendTable = new TLookupBlendTableCollection100();
        LinearAlphaBlendTable = new TLookupBlendTableCollection100();
        InitBlendTables(5, 31);
        FDirectDrawActivated = false;
        BltProcCount = 0;
        ForcedPixelFormatID = 0;
        ImageFileClassList = new TImageFileClassList();
        RegisterImageFileClasses([ImageFileClass.Of<TBMPImageFile>()]);
        RegisterBltProc(RegisterBltRect, "Register Blt");
        if (MMXsupport())
        {
            RegisterBltProc(MMXBltRect, "MMX Blt");
        }

        RegisterBltProc(RepMovsdBltRect, "MOVSD Blt");
    }

    public static void Finalize()
    {
        AlphaBlendTable.Free();
        IntensityBlendTable.Free();
        BrightenBlendTable.Free();
        ShadowBlendTable.Free();
        LinearAlphaBlendTable.Free();
        DeactivateDirectDraw();
        ImageFileClassList.Free();
    }
}
