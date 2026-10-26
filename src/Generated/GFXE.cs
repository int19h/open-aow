// Generated from GFXE.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aow.Units.GFXE
{
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
        public Aow.Units.GFXE.TDDColorKey ddckCKDestOverlay;
        public Aow.Units.GFXE.TDDColorKey ddckCKDestBlt;
        public Aow.Units.GFXE.TDDColorKey ddckCKSrcOverlay;
        public Aow.Units.GFXE.TDDColorKey ddckCKSrcBlt;
        public Aow.Units.GFXE.TDDPixelFormat ddpfPixelFormat;
        public Aow.Units.GFXE.TDDSCaps ddsCaps;
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
        public Aow.Units.GFXE.TDDColorKey ddckDestColorkey;
        [FieldOffset(92)]
        public Aow.Units.GFXE.TDDColorKey ddckSrcColorkey;
    }

    public delegate int TDDEnumModesCallback(Ptr<Aow.Units.GFXE.TDDSurfaceDesc> lpDDSurfaceDesc, object lpContext);
    public partial class IDirectDrawSurface : Aow.Rtl.IUnknown
    {
        // GFXE.pas:212
        public int AddAttachedSurface(Aow.Units.GFXE.IDirectDrawSurface lpDDSAttachedSurface)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:213
        public int AddOverlayDirtyRect(in Aow.Rtl.TRect lpRect)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:214
        public int Blt(Ptr<Aow.Rtl.TRect> lpDestRect, Aow.Units.GFXE.IDirectDrawSurface lpDDSrcSurface, Ptr<Aow.Rtl.TRect> lpSrcRect, int dwFlags, Ptr<Aow.Units.GFXE.TDDBltFX> lpDDBltFx)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:215
        public int BltBatch(object lpDDBltBatch, int dwCount, int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:216
        public int BltFast(int dwX, int dwY, Aow.Units.GFXE.IDirectDrawSurface lpDDSrcSurface, Ptr<Aow.Rtl.TRect> lpSrcRect, int dwTrans)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:217
        public int DeleteAttachedSurface(int dwFlags, Aow.Units.GFXE.IDirectDrawSurface lpDDSAttachedSurface)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:218
        public int EnumAttachedSurfaces(object lpContext, object lpEnumSurfacesCallback)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:219
        public int EnumOverlayZOrders(int dwFlags, object lpContext, object lpfnCallback)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:220
        public int Flip(Aow.Units.GFXE.IDirectDrawSurface lpDDSurfaceTargetOverride, int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:221
        public int GetAttachedSurface(ref Aow.Units.GFXE.TDDSCaps lpDDSCaps, ref Aow.Units.GFXE.IDirectDrawSurface lplpDDAttachedSurface)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:222
        public int GetBltStatus(int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:223
        public int GetCaps(ref Aow.Units.GFXE.TDDSCaps lpDDSCaps)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:224
        public int GetClipper(Aow.Units.GFXE.IDirectDrawClipper lplpDDClipper)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:225
        public int GetColorKey(int dwFlags, ref Aow.Units.GFXE.TDDColorKey lpDDColorKey)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:226
        public int GetDC(ref int lphDC)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:227
        public int GetFlipStatus(int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:228
        public int GetOverlayPosition(ref int lplX, ref int lplY)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:229
        public int GetPalette(Aow.Units.GFXE.IDirectDrawPalette lplpDDPalette)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:230
        public int GetPixelFormat(ref Aow.Units.GFXE.TDDPixelFormat lpDDPixelFormat)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:231
        public int GetSurfaceDesc(ref Aow.Units.GFXE.TDDSurfaceDesc lpDDSurfaceDesc)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:232
        public int Initialize(Aow.Units.GFXE.IDirectDraw lpDD, ref Aow.Units.GFXE.TDDSurfaceDesc lpDDSurfaceDesc)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:233
        public int IsLost()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:234
        public int Lock(Ptr<Aow.Rtl.TRect> lpDestRect, ref Aow.Units.GFXE.TDDSurfaceDesc lpDDSurfaceDesc, int dwFlags, int hEvent)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:235
        public int ReleaseDC(int hDC)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:236
        public int Restore()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:237
        public int SetClipper(Aow.Units.GFXE.IDirectDrawClipper lpDDClipper)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:238
        public int SetColorKey(int dwFlags, Ptr<Aow.Units.GFXE.TDDColorKey> lpDDColorKey)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:239
        public int SetOverlayPosition(int lX, int lY)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:240
        public int SetPalette(Aow.Units.GFXE.IDirectDrawPalette lpDDPalette)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:241
        public int Unlock(object lpSurfaceData)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:242
        public int UpdateOverlay(Ptr<Aow.Rtl.TRect> lpSrcRect, Aow.Units.GFXE.IDirectDrawSurface lpDDDestSurface, Ptr<Aow.Rtl.TRect> lpDestRect, int dwFlags, object lpDDOverlayFx)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:243
        public int UpdateOverlayDisplay(int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:244
        public int UpdateOverlayZOrder(int dwFlags, Aow.Units.GFXE.IDirectDrawSurface lpDDSReference)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }
    }

    public partial class IDirectDrawPalette : Aow.Rtl.IUnknown
    {
        // GFXE.pas:196
        public int GetCaps(ref int lpdwCaps)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:197
        public int GetEntries(int dwFlags, int dwBase, int dwNumEntries, object lpEntries)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:198
        public int Initialize(Aow.Units.GFXE.IDirectDraw lpDD, int dwFlags, object lpDDColorTable)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:199
        public int SetEntries(int dwFlags, int dwStartingEntry, int dwCount, object lpEntries)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }
    }

    public partial class IDirectDrawClipper : Aow.Rtl.IUnknown
    {
        // GFXE.pas:203
        public int GetClipList(Ptr<Aow.Rtl.TRect> lpRect, object lpClipList, ref int lpdwSize)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:204
        public int GetHWnd(ref int lphWnd)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:205
        public int Initialize(Aow.Units.GFXE.IDirectDraw lpDD, int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:206
        public int IsClipListChanged(ref bool lpbChanged)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:207
        public int SetClipList(object lpClipList, int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:208
        public int SetHWnd(int dwFlags, int hWnd)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }
    }

    public partial class IDirectDraw : Aow.Rtl.IUnknown
    {
        // GFXE.pas:149
        public int Compact()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:150
        public int CreateClipper(int dwFlags, Aow.Units.GFXE.IDirectDrawClipper lplpDDClipper, Aow.Rtl.IUnknown pUnkOuter)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:151
        public int CreatePalette(int dwFlags, object lpColorTable, Aow.Units.GFXE.IDirectDrawPalette lplpDDPalette, Aow.Rtl.IUnknown pUnkOuter)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:152
        public int CreateSurface(in Aow.Units.GFXE.TDDSurfaceDesc lpDDSurfaceDesc, ref Aow.Units.GFXE.IDirectDrawSurface lplpDDSurface, Aow.Rtl.IUnknown pUnkOuter)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:153
        public int DuplicateSurface(Aow.Units.GFXE.IDirectDrawSurface lpDDSurface, Aow.Units.GFXE.IDirectDrawSurface lplpDupDDSurface)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:154
        public int EnumDisplayModes(int dwFlags, Ptr<Aow.Units.GFXE.TDDSurfaceDesc> lpDDSurfaceDesc, object lpContext, Aow.Units.GFXE.TDDEnumModesCallback lpEnumModesCallback)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:155
        public int EnumSurfaces(int dwFlags, Ptr<Aow.Units.GFXE.TDDSurfaceDesc> lpDDSD, object lpContext, object lpEnumCallback)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:156
        public int FlipToGDISurface()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:157
        public int GetCaps(object lpDDDriverCaps, object lpDDHELCaps)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:158
        public int GetDisplayMode(ref Aow.Units.GFXE.TDDSurfaceDesc lpDDSurfaceDesc)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:159
        public int GetFourCCCodes(ref int lpNumCodes, Ptr<int> lpCodes)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:160
        public int GetGDISurface(Aow.Units.GFXE.IDirectDrawSurface lplpGDIDDSSurface)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:161
        public int GetMonitorFrequency(ref int lpdwFrequency)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:162
        public int GetScanLine(ref int lpdwScanLine)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:163
        public int GetVerticalBlankStatus(ref bool lpbIsInVB)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:164
        public int Initialize(Ptr<Aow.Rtl.TGUID> lpGUID)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:165
        public int RestoreDisplayMode()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:166
        public int SetCooperativeLevel(int hWnd, int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:167
        public int SetDisplayMode(int dwWidth, int dwHeight, int dwBpp)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:168
        public int WaitForVerticalBlank(int dwFlags, int hEvent)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }
    }

    public partial class IDirectDraw2 : Aow.Rtl.IUnknown
    {
        // GFXE.pas:172
        public int Compact()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:173
        public int CreateClipper(int dwFlags, Aow.Units.GFXE.IDirectDrawClipper lplpDDClipper, Aow.Rtl.IUnknown pUnkOuter)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:174
        public int CreatePalette(int dwFlags, object lpColorTable, Aow.Units.GFXE.IDirectDrawPalette lplpDDPalette, Aow.Rtl.IUnknown pUnkOuter)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:175
        public int CreateSurface(in Aow.Units.GFXE.TDDSurfaceDesc lpDDSurfaceDesc, ref Aow.Units.GFXE.IDirectDrawSurface lplpDDSurface, Aow.Rtl.IUnknown pUnkOuter)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:176
        public int DuplicateSurface(Aow.Units.GFXE.IDirectDrawSurface lpDDSurface, Aow.Units.GFXE.IDirectDrawSurface lplpDupDDSurface)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:177
        public int EnumDisplayModes(int dwFlags, Ptr<Aow.Units.GFXE.TDDSurfaceDesc> lpDDSurfaceDesc, object lpContext, Aow.Units.GFXE.TDDEnumModesCallback lpEnumModesCallback)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:178
        public int EnumSurfaces(int dwFlags, Ptr<Aow.Units.GFXE.TDDSurfaceDesc> lpDDSD, object lpContext, object lpEnumCallback)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:179
        public int FlipToGDISurface()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:180
        public int GetCaps(object lpDDDriverCaps, object lpDDHELCaps)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:181
        public int GetDisplayMode(ref Aow.Units.GFXE.TDDSurfaceDesc lpDDSurfaceDesc)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:182
        public int GetFourCCCodes(ref int lpNumCodes, Ptr<int> lpCodes)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:183
        public int GetGDISurface(Aow.Units.GFXE.IDirectDrawSurface lplpGDIDDSSurface)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:184
        public int GetMonitorFrequency(ref int lpdwFrequency)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:185
        public int GetScanLine(ref int lpdwScanLine)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:186
        public int GetVerticalBlankStatus(ref bool lpbIsInVB)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:187
        public int Initialize(Ptr<Aow.Rtl.TGUID> lpGUID)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:188
        public int RestoreDisplayMode()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:189
        public int SetCooperativeLevel(int hWnd, int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:190
        public int SetDisplayMode(int dwWidth, int dwHeight, int dwBpp, int dwRefreshRate, int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:191
        public int WaitForVerticalBlank(int dwFlags, int hEvent)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:192
        public int GetAvailableVidMem(Ptr<Aow.Units.GFXE.TDDSCaps> lpDDSCaps, ref int lpdwTotal, ref int lpdwFree)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }
    }

    public partial class IDirectDrawSurface2 : Aow.Rtl.IUnknown
    {
        // GFXE.pas:248
        public int AddAttachedSurface(Aow.Units.GFXE.IDirectDrawSurface2 lpDDSAttachedSurface)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:249
        public int AddOverlayDirtyRect(in Aow.Rtl.TRect lpRect)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:250
        public int Blt(Ptr<Aow.Rtl.TRect> lpDestRect, Aow.Units.GFXE.IDirectDrawSurface2 lpDDSrcSurface, Ptr<Aow.Rtl.TRect> lpSrcRect, int dwFlags, Ptr<Aow.Units.GFXE.TDDBltFX> lpDDBltFx)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:251
        public int BltBatch(object lpDDBltBatch, int dwCount, int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:252
        public int BltFast(int dwX, int dwY, Aow.Units.GFXE.IDirectDrawSurface2 lpDDSrcSurface, Ptr<Aow.Rtl.TRect> lpSrcRect, int dwTrans)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:253
        public int DeleteAttachedSurface(int dwFlags, Aow.Units.GFXE.IDirectDrawSurface2 lpDDSAttachedSurface)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:254
        public int EnumAttachedSurfaces(object lpContext, object lpEnumSurfacesCallback)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:255
        public int EnumOverlayZOrders(int dwFlags, object lpContext, object lpfnCallback)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:256
        public int Flip(Aow.Units.GFXE.IDirectDrawSurface2 lpDDSurfaceTargetOverride, int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:257
        public int GetAttachedSurface(ref Aow.Units.GFXE.TDDSCaps lpDDSCaps, ref Aow.Units.GFXE.IDirectDrawSurface2 lplpDDAttachedSurface)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:258
        public int GetBltStatus(int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:259
        public int GetCaps(ref Aow.Units.GFXE.TDDSCaps lpDDSCaps)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:260
        public int GetClipper(Aow.Units.GFXE.IDirectDrawClipper lplpDDClipper)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:261
        public int GetColorKey(int dwFlags, ref Aow.Units.GFXE.TDDColorKey lpDDColorKey)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:262
        public int GetDC(ref int lphDC)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:263
        public int GetFlipStatus(int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:264
        public int GetOverlayPosition(ref int lplX, ref int lplY)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:265
        public int GetPalette(Aow.Units.GFXE.IDirectDrawPalette lplpDDPalette)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:266
        public int GetPixelFormat(ref Aow.Units.GFXE.TDDPixelFormat lpDDPixelFormat)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:267
        public int GetSurfaceDesc(ref Aow.Units.GFXE.TDDSurfaceDesc lpDDSurfaceDesc)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:268
        public int Initialize(Aow.Units.GFXE.IDirectDraw lpDD, ref Aow.Units.GFXE.TDDSurfaceDesc lpDDSurfaceDesc)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:269
        public int IsLost()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:270
        public int Lock(Ptr<Aow.Rtl.TRect> lpDestRect, ref Aow.Units.GFXE.TDDSurfaceDesc lpDDSurfaceDesc, int dwFlags, int hEvent)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:271
        public int ReleaseDC(int hDC)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:272
        public int Restore()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:273
        public int SetClipper(Aow.Units.GFXE.IDirectDrawClipper lpDDClipper)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:274
        public int SetColorKey(int dwFlags, Ptr<Aow.Units.GFXE.TDDColorKey> lpDDColorKey)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:275
        public int SetOverlayPosition(int lX, int lY)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:276
        public int SetPalette(Aow.Units.GFXE.IDirectDrawPalette lpDDPalette)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:277
        public int Unlock(object lpSurfaceData)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:278
        public int UpdateOverlay(Ptr<Aow.Rtl.TRect> lpSrcRect, Aow.Units.GFXE.IDirectDrawSurface2 lpDDDestSurface, Ptr<Aow.Rtl.TRect> lpDestRect, int dwFlags, object lpDDOverlayFx)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:279
        public int UpdateOverlayDisplay(int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:280
        public int UpdateOverlayZOrder(int dwFlags, Aow.Units.GFXE.IDirectDrawSurface2 lpDDSReference)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:281
        public int GetDDInterface(Aow.Rtl.IUnknown lplpDD)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:282
        public int PageLock(int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:283
        public int PageUnlock(int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }
    }

    public partial class IDirectDrawSurface3 : Aow.Rtl.IUnknown
    {
        // GFXE.pas:287
        public int AddAttachedSurface(Aow.Units.GFXE.IDirectDrawSurface3 lpDDSAttachedSurface)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:288
        public int AddOverlayDirtyRect(in Aow.Rtl.TRect lpRect)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:289
        public int Blt(Ptr<Aow.Rtl.TRect> lpDestRect, Aow.Units.GFXE.IDirectDrawSurface3 lpDDSrcSurface, Ptr<Aow.Rtl.TRect> lpSrcRect, int dwFlags, Ptr<Aow.Units.GFXE.TDDBltFX> lpDDBltFx)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:290
        public int BltBatch(object lpDDBltBatch, int dwCount, int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:291
        public int BltFast(int dwX, int dwY, Aow.Units.GFXE.IDirectDrawSurface3 lpDDSrcSurface, Ptr<Aow.Rtl.TRect> lpSrcRect, int dwTrans)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:292
        public int DeleteAttachedSurface(int dwFlags, Aow.Units.GFXE.IDirectDrawSurface3 lpDDSAttachedSurface)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:293
        public int EnumAttachedSurfaces(object lpContext, object lpEnumSurfacesCallback)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:294
        public int EnumOverlayZOrders(int dwFlags, object lpContext, object lpfnCallback)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:295
        public int Flip(Aow.Units.GFXE.IDirectDrawSurface3 lpDDSurfaceTargetOverride, int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:296
        public int GetAttachedSurface(ref Aow.Units.GFXE.TDDSCaps lpDDSCaps, ref Aow.Units.GFXE.IDirectDrawSurface3 lplpDDAttachedSurface)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:297
        public int GetBltStatus(int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:298
        public int GetCaps(ref Aow.Units.GFXE.TDDSCaps lpDDSCaps)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:299
        public int GetClipper(Aow.Units.GFXE.IDirectDrawClipper lplpDDClipper)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:300
        public int GetColorKey(int dwFlags, ref Aow.Units.GFXE.TDDColorKey lpDDColorKey)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:301
        public int GetDC(ref int lphDC)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:302
        public int GetFlipStatus(int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:303
        public int GetOverlayPosition(ref int lplX, ref int lplY)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:304
        public int GetPalette(Aow.Units.GFXE.IDirectDrawPalette lplpDDPalette)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:305
        public int GetPixelFormat(ref Aow.Units.GFXE.TDDPixelFormat lpDDPixelFormat)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:306
        public int GetSurfaceDesc(ref Aow.Units.GFXE.TDDSurfaceDesc lpDDSurfaceDesc)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:307
        public int Initialize(Aow.Units.GFXE.IDirectDraw lpDD, ref Aow.Units.GFXE.TDDSurfaceDesc lpDDSurfaceDesc)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:308
        public int IsLost()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:309
        public int Lock(Ptr<Aow.Rtl.TRect> lpDestRect, ref Aow.Units.GFXE.TDDSurfaceDesc lpDDSurfaceDesc, int dwFlags, int hEvent)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:310
        public int ReleaseDC(int hDC)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:311
        public int Restore()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:312
        public int SetClipper(Aow.Units.GFXE.IDirectDrawClipper lpDDClipper)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:313
        public int SetColorKey(int dwFlags, Ptr<Aow.Units.GFXE.TDDColorKey> lpDDColorKey)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:314
        public int SetOverlayPosition(int lX, int lY)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:315
        public int SetPalette(Aow.Units.GFXE.IDirectDrawPalette lpDDPalette)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:316
        public int Unlock(object lpSurfaceData)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:317
        public int UpdateOverlay(Ptr<Aow.Rtl.TRect> lpSrcRect, Aow.Units.GFXE.IDirectDrawSurface3 lpDDDestSurface, Ptr<Aow.Rtl.TRect> lpDestRect, int dwFlags, object lpDDOverlayFx)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:318
        public int UpdateOverlayDisplay(int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:319
        public int UpdateOverlayZOrder(int dwFlags, Aow.Units.GFXE.IDirectDrawSurface3 lpDDSReference)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:320
        public int GetDDInterface(Aow.Rtl.IUnknown lplpDD)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:321
        public int PageLock(int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:322
        public int PageUnlock(int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:323
        public int SetSurfaceDesc(in Aow.Units.GFXE.TDDSurfaceDesc lpDDSD, int dwFlags)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }
    }

    public partial class IDirectDrawColorControl : Aow.Rtl.IUnknown
    {
        // GFXE.pas:327
        public int GetColorControls(object lpColorControl)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:328
        public int SetColorControls(object lpColorControl)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
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

    public partial class TGFXEPalette : Aow.Rtl.TObject
    {
        public int PixelFormatID;
        public Aow.Units.GFXE.TPaletteEntry[] Entries = new Aow.Units.GFXE.TPaletteEntry[256];
        // GFXE.pas:11050
        // { TGFXEPalette }
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            Stream.Read(Aow.Rtl.Bytes.Of(ref PixelFormatID, 4));
            Stream.Read(MemoryMarshal.AsBytes((Span<Aow.Units.GFXE.TPaletteEntry>)Entries).Slice(0, 1024));
        }

        // GFXE.pas:11056
        public void SaveToStream(Aow.Rtl.TStream Stream)
        {
            Stream.Write(Aow.Rtl.Bytes.Of(ref PixelFormatID, 4));
            Stream.Write(MemoryMarshal.AsBytes((ReadOnlySpan<Aow.Units.GFXE.TPaletteEntry>)Entries).Slice(0, 1024));
        }

        // GFXE.pas:11062
        public void Assign(Aow.Units.GFXE.TGFXEPalette Source)
        {
            Entries = Source.Entries;
            PixelFormatID = Source.PixelFormatID;
        }

        // GFXE.pas:11068
        public Ptr<Aow.Units.GFXE.TPaletteEntry> GetEntry(int Index)
        {
            Ptr<Aow.Units.GFXE.TPaletteEntry> Result = default;
            Result = Ptr<Aow.Units.GFXE.TPaletteEntry>.To(Entries, Index);
            return Result;
        }

        // GFXE.pas:11073
        public int Remap(int NewPixelFormatID)
        {
            int Result = default;
            int I = default;
            Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            if (NewPixelFormatID != PixelFormatID)
            {
                switch (PixelFormatID)
                {
                    case Aow.Units.GFXE.GFXE.pfBGR24:
                        switch (NewPixelFormatID)
                        {
                            case Aow.Units.GFXE.GFXE.pfRGB565:
                            {
                                I = 0;
                                byte end__1 = 255;
                                for (; I <= end__1; I += 1)
                                {
                                    {
                                        ref var w__2 = ref Entries[I];
                                        w__2.W = unchecked((ushort)((((w__2.R & 248) << 8) + ((w__2.G & 252) << 3)) + ((int)(((uint)w__2.B) >> 3))));
                                    }
                                }
                            }

                                break;
                            case Aow.Units.GFXE.GFXE.pfRGB555:
                            {
                                I = 0;
                                byte end__3 = 255;
                                for (; I <= end__3; I += 1)
                                {
                                    {
                                        ref var w__4 = ref Entries[I];
                                        w__4.W = unchecked((ushort)((((w__4.R & 248) << 7) + ((w__4.G & 248) << 2)) + ((int)(((uint)w__4.B) >> 3))));
                                    }
                                }
                            }

                                break;
                            case Aow.Units.GFXE.GFXE.pfXRGB32:
                                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
                                break;
                            default:
                                Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                                break;
                        }

                        break;
                    case Aow.Units.GFXE.GFXE.pfRGB24:
                        switch (NewPixelFormatID)
                        {
                            case Aow.Units.GFXE.GFXE.pfRGB565:
                            {
                                I = 0;
                                byte end__5 = 255;
                                for (; I <= end__5; I += 1)
                                {
                                    {
                                        ref var w__6 = ref Entries[I];
                                        w__6.W = unchecked((ushort)((((w__6.B & 248) << 8) + ((w__6.G & 252) << 3)) + ((int)(((uint)w__6.R) >> 3))));
                                    }
                                }
                            }

                                break;
                            case Aow.Units.GFXE.GFXE.pfRGB555:
                            {
                                I = 0;
                                byte end__7 = 255;
                                for (; I <= end__7; I += 1)
                                {
                                    {
                                        ref var w__8 = ref Entries[I];
                                        w__8.W = unchecked((ushort)((((w__8.B & 248) << 7) + ((w__8.G & 248) << 2)) + ((int)(((uint)w__8.R) >> 3))));
                                    }
                                }
                            }

                                break;
                            case Aow.Units.GFXE.GFXE.pfXRGB32:
                                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
                                break;
                            default:
                                Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                                break;
                        }

                        break;
                    default:
                        Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                        break;
                }

                if (Aow.Rtl.Rtl.Succeeded(Result))
                {
                    PixelFormatID = NewPixelFormatID;
                }
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }

            return Result;
        }

        // GFXE.pas:11114
        public int GetNearestColor(int Color)
        {
            /* TODO(T-shim) Platform or editor boundary Graphics.ColorToRGB */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:11140
        public bool Compare(Aow.Units.GFXE.TGFXEPalette Palette)
        {
            bool Result = default;
            int I = default;
            if (Palette.PixelFormatID == PixelFormatID)
            {
                switch (PixelFormatID & 31)
                {
                    case 8:
                        Result = true;
                        I = 255;
                        while ((I >= 0) && Result)
                        {
                            Result = Palette.Entries[I].W == Entries[I].W;
                            I -= 1;
                        }

                        break;
                    case 16:
                        Result = true;
                        I = 255;
                        while ((I >= 0) && Result)
                        {
                            Result = Palette.Entries[I].W == Entries[I].W;
                            I -= 1;
                        }

                        break;
                    case 24:
                        Result = true;
                        I = 255;
                        while ((I >= 0) && Result)
                        {
                            Result = (Palette.Entries[I].W == Entries[I].W) && (Palette.Entries[I].B == Entries[I].B);
                            I -= 1;
                        }

                        break;
                    case 32:
                        var call__9 = Aow.Rtl.Rtl.CompareMem(MemoryMarshal.AsBytes((ReadOnlySpan<Aow.Units.GFXE.TPaletteEntry>)Entries).Slice(0, 1024), MemoryMarshal.AsBytes((ReadOnlySpan<Aow.Units.GFXE.TPaletteEntry>)Palette.Entries).Slice(0, 1024), 1024);
                        Result = call__9;
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

        // System.pas:111
        public static Aow.Units.GFXE.TGFXEPalette Create()
        {
            return new Aow.Units.GFXE.TGFXEPalette();
        }
    }

    public struct TSurfaceDesc
    {
        public int Width;
        public int Height;
        public int BitCount;
        public int Pitch;
        public Aow.Rtl.TRect ClipRect;
        public object Data;
        public Aow.Units.GFXE.TGFXEPalette Palette;
        public int PixelFormatID;
        public int RMask;
        public int GMask;
        public int BMask;
        public int AMask;
    }

    // { GFXE.TSurface (instance size 4): the abstract surface; GetWidth and GetHeight are abstract.
//     VMT: GetColor, GetPixelInfo, GetWidth, GetHeight, GetPixelFormatID, Blt, BltClipped,
//     BltColorKey, BltColorKeyClipped, BltDC, BltStretch, Lock, Unlock, LockEx, UnlockEx,
//     UpdatePalette, Resize, Cls }
    public partial class TSurface : Aow.Rtl.TObject
    {
        // GFXE.pas:11964
        public virtual int GetColor(int X, int Y)
        {
            int Result = default;
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            Ptr<byte> P = default;
            int V = default;
            ushort W = default;
            byte Index = default;
            var call__10 = Lock(ref SD);
            var call__11 = Aow.Rtl.Rtl.Succeeded(call__10);
            if (call__11)
            {
                try
                {
                    Aow.Rtl.Rtl.Assert((((X >= 0) && (X < SD.Width)) && (Y >= 0)) && (Y < SD.Height), (AnsiString)"Coordinates out of bounds!", "D:\\git\\aow\\src\\delphi3\\Gfxepack\\GFXE.pas", 11974);
                    // { 8 bit pixels need the palette; 1555 and the TColor ordered 24 bit layout are not handled }
                    switch (SD.PixelFormatID)
                    {
                        case Aow.Units.GFXE.GFXE.pfRGB555:
                            W = unchecked((ushort)MemoryMarshal.Read<ushort>(((Aow.Rtl.Rtl.BytePointer(SD.Data) + (X + X)) + (Y * SD.Pitch)).AsSpan(2)));
                            Result = (((int)(((uint)(W & 31744)) >> 7)) + ((W & 992) << 6)) + ((W & 31) << 19);
                            break;
                        case Aow.Units.GFXE.GFXE.pfRGB565:
                            W = unchecked((ushort)MemoryMarshal.Read<ushort>(((Aow.Rtl.Rtl.BytePointer(SD.Data) + (X + X)) + (Y * SD.Pitch)).AsSpan(2)));
                            Result = (((int)(((uint)(W & 63488)) >> 8)) + ((W & 2016) << 5)) + ((W & 31) << 19);
                            break;
                        case Aow.Units.GFXE.GFXE.pfPAL8:
                            Aow.Rtl.Rtl.Assert(SD.Palette != null, (AnsiString)"Palette not found, cannot retrieve color (GetColor)", "D:\\git\\aow\\src\\delphi3\\Gfxepack\\GFXE.pas", 11986);
                            Index = unchecked((byte)MemoryMarshal.Read<byte>(((Aow.Rtl.Rtl.BytePointer(SD.Data) + X) + (Y * SD.Pitch)).AsSpan(1)));
                            P = Ptr<Aow.Units.GFXE.TPaletteEntry>.To(SD.Palette.Entries, Index).Reinterpret<byte>();
                            Result = (P[0] + (P[1] << 8)) + (P[2] << 16);
                            break;
                        case Aow.Units.GFXE.GFXE.pfRGB24:
                            P = Ptr<byte>.Unbox(((Aow.Rtl.Rtl.BytePointer(SD.Data) + ((X + X) + X)) + (Y * SD.Pitch)).Box());
                            Result = (P[2] + (P[1] << 8)) + (P[0] << 16);
                            break;
                        case Aow.Units.GFXE.GFXE.pfRGB32:
                            V = MemoryMarshal.Read<int>(((Aow.Rtl.Rtl.BytePointer(SD.Data) + (X << 2)) + (Y * SD.Pitch)).AsSpan(4));
                            Result = (((int)(((uint)(V & 16711680)) >> 16)) + (V & 65280)) + ((V & 255) << 16);
                            break;
                        default:
                            Aow.Rtl.Rtl.Assert(false, (AnsiString)"Pixel format not supported (GetColor)", "D:\\git\\aow\\src\\delphi3\\Gfxepack\\GFXE.pas", 11999);
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
                Aow.Rtl.Rtl.Assert(false, (AnsiString)"Cannot Lock Surface", "D:\\git\\aow\\src\\delphi3\\Gfxepack\\GFXE.pas", 12004);
                Result = -1;
            }

            return Result;
        }

        // GFXE.pas:11924
        // { The original unit was longer: the line numbers compiled into the Assert calls (11932 for the next Assert below) prove that 737 more lines preceded this point. Their content is unknown; the blank lines below keep the Assert line numbers identical. }
        // { TSurface }
        public virtual int GetPixelInfo(int X, int Y)
        {
            int Result = default;
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            object P = default;
            var call__12 = Lock(ref SD);
            var call__13 = Aow.Rtl.Rtl.Succeeded(call__12);
            if (call__13)
            {
                try
                {
                    Aow.Rtl.Rtl.Assert((((X >= 0) && (X < SD.Width)) && (Y >= 0)) && (Y < SD.Height), (AnsiString)"Coordinates out of bounds!", "D:\\git\\aow\\src\\delphi3\\Gfxepack\\GFXE.pas", 11932);
                    switch (SD.BitCount)
                    {
                        case 8:
                            Result = MemoryMarshal.Read<byte>(((Aow.Rtl.Rtl.BytePointer(SD.Data) + X) + (Y * SD.Pitch)).AsSpan(1));
                            break;
                        case 16:
                            Result = MemoryMarshal.Read<ushort>(((Aow.Rtl.Rtl.BytePointer(SD.Data) + (X + X)) + (Y * SD.Pitch)).AsSpan(2));
                            break;
                        case 24:
                            if ((X + 1) < SD.Width)
                            {
                                Result = MemoryMarshal.Read<int>(((Aow.Rtl.Rtl.BytePointer(SD.Data) + ((X + X) + X)) + (Y * SD.Pitch)).AsSpan(4)) & 16777215;
                            }
                            else
                            {
                                P = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + ((X + X) + X)) + (Y * SD.Pitch)).Box();
                                Result = MemoryMarshal.Read<ushort>(Aow.Rtl.Rtl.BytePointer(P).AsSpan(2)) + (MemoryMarshal.Read<byte>((Aow.Rtl.Rtl.BytePointer(P) + 2).AsSpan(1)) << 16);
                            }

                            break;
                        case 32:
                            Result = MemoryMarshal.Read<int>(((Aow.Rtl.Rtl.BytePointer(SD.Data) + (X << 2)) + (Y * SD.Pitch)).AsSpan(4));
                            break;
                        default:
                            Aow.Rtl.Rtl.Assert(false, (AnsiString)"Pixel format not supported (GetColor)", "D:\\git\\aow\\src\\delphi3\\Gfxepack\\GFXE.pas", 11946);
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
                Aow.Rtl.Rtl.Assert(false, (AnsiString)"Cannot Lock Surface", "D:\\git\\aow\\src\\delphi3\\Gfxepack\\GFXE.pas", 11951);
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
            int Result = default;
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            var call__14 = Lock(ref SD);
            var call__15 = Aow.Rtl.Rtl.Succeeded(call__14);
            if (call__15)
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
        public virtual int Blt(Aow.Units.GFXE.TSurface Dest, Ptr<Aow.Rtl.TRect> SrcRect, int X, int Y)
        {
            int Result = default;
            Aow.Units.GFXE.TSurfaceDesc DSD = default;
            Aow.Units.GFXE.TSurfaceDesc SSD = default;
            Aow.Rtl.TRect[] R__cell = new Aow.Rtl.TRect[1];
            ref Aow.Rtl.TRect R = ref R__cell[0];
            object S = default;
            object D = default;
            var call__16 = Dest.PixelFormatID;
            var call__17 = PixelFormatID;
            if (call__16 == call__17)
            {
                var call__18 = Dest.Lock(ref DSD);
                var call__19 = Aow.Rtl.Rtl.Succeeded(call__18);
                if (call__19)
                {
                    var call__20 = Lock(ref SSD);
                    var call__21 = Aow.Rtl.Rtl.Succeeded(call__20);
                    if (call__21)
                    {
                        if (SrcRect.IsNull)
                        {
                            R.Left = 0;
                            R.Right = SSD.Width;
                            R.Top = 0;
                            R.Bottom = SSD.Height;
                            SrcRect = Ptr<Aow.Rtl.TRect>.To(R__cell, 0);
                        }

                        S = ((Aow.Rtl.Rtl.BytePointer(SSD.Data) + ((SrcRect.Value.Left * SSD.BitCount) / 8)) + (SrcRect.Value.Top * SSD.Pitch)).Box();
                        D = ((Aow.Rtl.Rtl.BytePointer(DSD.Data) + ((X * DSD.BitCount) / 8)) + (Y * DSD.Pitch)).Box();
                        Aow.Units.GFXE.GFXE.BltRect(S, D, SSD.Pitch, DSD.Pitch, ((SrcRect.Value.Right - SrcRect.Value.Left) * DSD.BitCount) / 8, SrcRect.Value.Bottom - SrcRect.Value.Top);
                        Unlock();
                        Result = Aow.Units.GFXE.GFXE.GFXE_OK;
                    }
                    else
                    {
                        Result = Aow.Units.GFXE.GFXE.GFXE_CANNOTLOCK;
                    }

                    Dest.Unlock();
                }
                else
                {
                    Result = Aow.Units.GFXE.GFXE.GFXE_CANNOTLOCK;
                }
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_PIXELFORMATNOTSUPPORTED;
            }

            return Result;
        }

        // GFXE.pas:12045
        public virtual int BltClipped(Aow.Units.GFXE.TSurface Dest, Ptr<Aow.Rtl.TRect> SrcRect, int X, int Y)
        {
            int Result = default;
            int W = default;
            int H = default;
            Aow.Rtl.TRect[] R__cell = new Aow.Rtl.TRect[1];
            ref Aow.Rtl.TRect R = ref R__cell[0];
            if (SrcRect.IsNull)
            {
                R.Left = 0;
                var call__22 = GetWidth();
                R.Right = call__22;
                R.Top = 0;
                var call__23 = GetHeight();
                R.Bottom = call__23;
            }
            else
            {
                R = SrcRect.Value;
                var call__24 = GetWidth();
                if (R.Right > call__24)
                {
                    var call__25 = GetWidth();
                    R.Right = call__25;
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
            if (Dest.GetWidth() < (X + W))
            {
                W = W - ((X + W) - Dest.GetWidth());
            }

            var call__26 = GetHeight();
            if (R.Bottom > call__26)
            {
                var call__27 = GetHeight();
                R.Bottom = call__27;
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
            if (Dest.GetHeight() < (H + Y))
            {
                H = H - ((H + Y) - Dest.GetHeight());
            }

            if ((W > 0) && (H > 0))
            {
                R.Right = W + R.Left;
                R.Bottom = R.Top + H;
                Result = Blt(Dest, Ptr<Aow.Rtl.TRect>.To(R__cell, 0), X, Y);
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }

            return Result;
        }

        // GFXE.pas:12130
        public virtual int BltColorKey(Aow.Units.GFXE.TSurface Dest, Ptr<Aow.Rtl.TRect> SrcRect, int X, int Y, int ColorKey)
        {
            int Result = default;
            Aow.Units.GFXE.TSurfaceDesc DSD = default;
            Aow.Units.GFXE.TSurfaceDesc SSD = default;
            Aow.Rtl.TRect[] R__cell = new Aow.Rtl.TRect[1];
            ref Aow.Rtl.TRect R = ref R__cell[0];
            object S = default;
            object D = default;
            var call__28 = Dest.PixelFormatID;
            var call__29 = PixelFormatID;
            bool bool__30 = (call__28 == call__29);
            if (bool__30)
            {
                var call__31 = PixelFormatID;
                bool__30 = ((call__31 & 255) == 16);
            }

            if (bool__30)
            {
                var call__32 = Dest.Lock(ref DSD);
                var call__33 = Aow.Rtl.Rtl.Succeeded(call__32);
                if (call__33)
                {
                    var call__34 = Lock(ref SSD);
                    var call__35 = Aow.Rtl.Rtl.Succeeded(call__34);
                    if (call__35)
                    {
                        if (SrcRect.IsNull)
                        {
                            R.Left = 0;
                            R.Right = SSD.Width;
                            R.Top = 0;
                            R.Bottom = SSD.Height;
                            SrcRect = Ptr<Aow.Rtl.TRect>.To(R__cell, 0);
                        }

                        S = ((Aow.Rtl.Rtl.BytePointer(SSD.Data) + ((SrcRect.Value.Left * SSD.BitCount) / 8)) + (SrcRect.Value.Top * SSD.Pitch)).Box();
                        D = ((Aow.Rtl.Rtl.BytePointer(DSD.Data) + ((X * DSD.BitCount) / 8)) + (Y * DSD.Pitch)).Box();
                        Aow.Units.GFXE.GFXE.BltColorKey16(S, D, SSD.Pitch, DSD.Pitch, SrcRect.Value.Right - SrcRect.Value.Left, SrcRect.Value.Bottom - SrcRect.Value.Top, ColorKey);
                        Result = Aow.Units.GFXE.GFXE.GFXE_OK;
                        Unlock();
                    }
                    else
                    {
                        Result = Aow.Units.GFXE.GFXE.GFXE_CANNOTLOCK;
                    }

                    Dest.Unlock();
                }
                else
                {
                    Result = Aow.Units.GFXE.GFXE.GFXE_CANNOTLOCK;
                }
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_PIXELFORMATNOTSUPPORTED;
            }

            return Result;
        }

        // GFXE.pas:12168
        public virtual int BltColorKeyClipped(Aow.Units.GFXE.TSurface Dest, Ptr<Aow.Rtl.TRect> SrcRect, int X, int Y, int ColorKey)
        {
            int Result = default;
            int W = default;
            int H = default;
            Aow.Rtl.TRect[] R__cell = new Aow.Rtl.TRect[1];
            ref Aow.Rtl.TRect R = ref R__cell[0];
            if (SrcRect.IsNull)
            {
                R.Left = 0;
                var call__36 = GetWidth();
                R.Right = call__36;
                R.Top = 0;
                var call__37 = GetHeight();
                R.Bottom = call__37;
            }
            else
            {
                R = SrcRect.Value;
                var call__38 = GetWidth();
                if (R.Right > call__38)
                {
                    var call__39 = GetWidth();
                    R.Right = call__39;
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
            if (Dest.GetWidth() < (X + W))
            {
                W = W - ((X + W) - Dest.GetWidth());
            }

            var call__40 = GetHeight();
            if (R.Bottom > call__40)
            {
                var call__41 = GetHeight();
                R.Bottom = call__41;
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
            if (Dest.GetHeight() < (H + Y))
            {
                H = H - ((H + Y) - Dest.GetHeight());
            }

            if ((W > 0) && (H > 0))
            {
                R.Right = W + R.Left;
                R.Bottom = R.Top + H;
                Result = BltColorKey(Dest, Ptr<Aow.Rtl.TRect>.To(R__cell, 0), X, Y, ColorKey);
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }

            return Result;
        }

        // GFXE.pas:12097
        public virtual int BltDC(int DC, Ptr<Aow.Rtl.TRect> SrcRect, Ptr<Aow.Rtl.TRect> DestRect)
        {
            int Result = default;
            Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
            return Result;
        }

        // GFXE.pas:12102
        public virtual int BltStretch(Aow.Units.GFXE.TSurface Dest, Ptr<Aow.Rtl.TRect> SrcRect, Ptr<Aow.Rtl.TRect> DestRect)
        {
            int Result = default;
            Aow.Rtl.TRect[] R__cell = new Aow.Rtl.TRect[1];
            ref Aow.Rtl.TRect R = ref R__cell[0];
            Aow.Rtl.TRect[] DR__cell = new Aow.Rtl.TRect[1];
            ref Aow.Rtl.TRect DR = ref DR__cell[0];
            if (SrcRect.IsNull)
            {
                R.Left = 0;
                var call__42 = GetWidth();
                R.Right = call__42;
                R.Top = 0;
                var call__43 = GetHeight();
                R.Bottom = call__43;
                SrcRect = Ptr<Aow.Rtl.TRect>.To(R__cell, 0);
            }

            if (DestRect.IsNull)
            {
                DR.Left = 0;
                var call__44 = Dest.GetWidth();
                DR.Right = call__44;
                DR.Top = 0;
                var call__45 = Dest.GetHeight();
                DR.Bottom = call__45;
                DestRect = Ptr<Aow.Rtl.TRect>.To(DR__cell, 0);
            }

            if (((SrcRect.Value.Right - SrcRect.Value.Left) == (DestRect.Value.Right - DestRect.Value.Left)) && ((SrcRect.Value.Bottom - SrcRect.Value.Top) == (DestRect.Value.Bottom - DestRect.Value.Top)))
            {
                var call__46 = Blt(Dest, SrcRect, DestRect.Value.Left, DestRect.Value.Top);
                Result = call__46;
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
            }

            return Result;
        }

        // GFXE.pas:12225
        public virtual int Lock(ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Result = default;
            Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
            return Result;
        }

        // GFXE.pas:12248
        public virtual int Unlock()
        {
            int Result = default;
            Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
            return Result;
        }

        // GFXE.pas:12230
        public virtual int LockEx(ref Aow.Units.GFXE.TSurfaceDesc SD, in Aow.Rtl.TRect R)
        {
            int Result = default;
            bool bool__47 = (R.Left >= 0) && (R.Top >= 0);
            if (bool__47)
            {
                var call__48 = GetHeight();
                bool__47 = (R.Bottom <= call__48);
            }

            bool bool__49 = bool__47;
            if (bool__49)
            {
                var call__50 = GetWidth();
                bool__49 = (R.Right <= call__50);
            }

            if (bool__49)
            {
                Result = Lock(ref SD);
                if (Aow.Rtl.Rtl.Succeeded(Result))
                {
                    SD.Data = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + ((int)(((uint)(R.Left * SD.BitCount)) >> 3))) + (R.Top * SD.Pitch)).Box();
                    SD.Width = R.Right - R.Left;
                    SD.Height = R.Bottom - R.Top;
                    SD.ClipRect.Right = SD.Width;
                    SD.ClipRect.Bottom = SD.Height;
                }
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_ERROR;
            }

            return Result;
        }

        // GFXE.pas:12253
        public virtual int UnlockEx(in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Result = default;
            Result = Unlock();
            return Result;
        }

        // GFXE.pas:12220
        public virtual int UpdatePalette()
        {
            int Result = default;
            Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
            return Result;
        }

        // GFXE.pas:12258
        public virtual int Resize(int AWidth, int AHeight)
        {
            int Result = default;
            Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
            return Result;
        }

        // GFXE.pas:12263
        public virtual int Cls(int Color)
        {
            int Result = default;
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            int Pixel = default;
            int I = default;
            var call__51 = Lock(ref SD);
            var call__52 = Aow.Rtl.Rtl.Succeeded(call__51);
            if (call__52)
            {
                var call__53 = Aow.Units.GFXE.GFXE.Color2PixelInfo(Color, SD.PixelFormatID, null);
                Pixel = call__53;
                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
                switch (SD.BitCount)
                {
                    case 8:
                    {
                        I = 0;
                        int end__54 = SD.Height - 1;
                        for (; I <= end__54; I += 1)
                        {
                            Aow.Rtl.Rtl.FillBytes((Aow.Rtl.Rtl.BytePointer(SD.Data) + (SD.Pitch * I)).AsSpan(SD.Width), Pixel);
                        }
                    }

                        break;
                    case 16:
                    {
                        I = 0;
                        int end__55 = SD.Height - 1;
                        for (; I <= end__55; I += 1)
                        {
                            Aow.Units.GFXE.GFXE.FillWord((Aow.Rtl.Rtl.BytePointer(SD.Data) + (SD.Pitch * I)).AsSpan(SD.Width * 2), SD.Width, Pixel);
                        }
                    }

                        break;
                    case 24:
                    {
                        I = 0;
                        int end__56 = SD.Height - 1;
                        for (; I <= end__56; I += 1)
                        {
                            Aow.Units.GFXE.GFXE.FillTrippleByte((Aow.Rtl.Rtl.BytePointer(SD.Data) + (SD.Pitch * I)).AsSpan(SD.Width * 3), SD.Width, Pixel);
                        }
                    }

                        break;
                    case 32:
                    {
                        I = 0;
                        int end__57 = SD.Height - 1;
                        for (; I <= end__57; I += 1)
                        {
                            Aow.Units.GFXE.GFXE.FillInt((Aow.Rtl.Rtl.BytePointer(SD.Data) + (SD.Pitch * I)).AsSpan(SD.Width * 4), SD.Width, Pixel);
                        }
                    }

                        break;
                    default:
                        Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                        break;
                }

                Unlock();
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
            }

            return Result;
        }

        public int Width { get => GetWidth(); }
        public int Height { get => GetHeight(); }
        public int PixelFormatID { get => GetPixelFormatID(); }
    }

    // { GFXE.TDataSurface (instance size 44): a surface in memory. +4 FWidth, +8 FHeight, +$C FMem,
//     +$10 FData (FMem aligned to 8), +$14 FBitCount (Byte), +$18 FPitch, +$1C FPixelFormatID,
//     +$20 FX, +$24 FY (origin of the captured rectangle), +$28 FPalette. }
    public partial class TDataSurface : Aow.Units.GFXE.TSurface
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
        internal Aow.Units.GFXE.TGFXEPalette FPalette;
        // GFXE.pas:12387
        // { TDataSurface }
        public void Create__init_TDataSurface(int AWidth, int AHeight, int APixelFormatID, Aow.Units.GFXE.TGFXEPalette APalette)
        {
            ;
            FBitCount = unchecked((byte)(APixelFormatID & 255));
            FPixelFormatID = APixelFormatID;
            SetPalette(APalette);
            Resize(AWidth, AHeight);
        }

        // GFXE.pas:12396
        public void CreateFromSurface__init_TDataSurface(Aow.Units.GFXE.TSurface Surface, Ptr<Aow.Rtl.TRect> R)
        {
            ;
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
        public override int GetPixelFormatID()
        {
            int Result = default;
            Result = FPixelFormatID;
            return Result;
        }

        // GFXE.pas:12415
        public override int GetWidth()
        {
            int Result = default;
            Result = FWidth;
            return Result;
        }

        // GFXE.pas:12420
        public override int GetHeight()
        {
            int Result = default;
            Result = FHeight;
            return Result;
        }

        // GFXE.pas:12425
        public override int Lock(ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Result = default;
            SD = default;
            SD.Width = FWidth;
            SD.Height = FHeight;
            SD.BitCount = FBitCount;
            SD.PixelFormatID = FPixelFormatID;
            SD.ClipRect.Left = 0;
            SD.ClipRect.Right = FWidth;
            SD.ClipRect.Top = 0;
            SD.ClipRect.Bottom = FHeight;
            SD.Pitch = FPitch;
            SD.Data = FData;
            Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            return Result;
        }

        // GFXE.pas:12441
        public override int Unlock()
        {
            int Result = default;
            Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            return Result;
        }

        // GFXE.pas:12446
        public int CaptureSurface(Aow.Units.GFXE.TSurface Surface, Ptr<Aow.Rtl.TRect> R)
        {
            int Result = default;
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            Aow.Rtl.TRect[] RR__cell = new Aow.Rtl.TRect[1];
            ref Aow.Rtl.TRect RR = ref RR__cell[0];
            int W = default;
            int H = default;
            Result = Surface.Lock(ref SD);
            if (Aow.Rtl.Rtl.Succeeded(Result))
            {
                try
                {
                    FBitCount = unchecked((byte)SD.BitCount);
                    FPixelFormatID = SD.PixelFormatID;
                    if (R.IsNull)
                    {
                        RR.Left = 0;
                        RR.Right = SD.Width;
                        RR.Top = 0;
                        RR.Bottom = SD.Height;
                        R = Ptr<Aow.Rtl.TRect>.To(RR__cell, 0);
                    }

                    W = R.Value.Right - R.Value.Left;
                    H = R.Value.Bottom - R.Value.Top;
                    if ((W > 0) && (H > 0))
                    {
                        Resize(W, H);
                        SetPalette(SD.Palette);
                        FX = R.Value.Left;
                        FY = R.Value.Top;
                        Aow.Units.GFXE.GFXE.BltRect(((Aow.Rtl.Rtl.BytePointer(SD.Data) + (FY * SD.Pitch)) + ((int)(((uint)(FX * FBitCount)) >> 3))).Box(), FData, SD.Pitch, FPitch, (int)(((uint)(FWidth * FBitCount)) >> 3), FHeight);
                    }
                }
                catch (System.Exception)
                {
                    FX = 30;
                }

                Surface.Unlock();
                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }

            return Result;
        }

        // GFXE.pas:12485
        public void SetPalette(Aow.Units.GFXE.TGFXEPalette Palette)
        {
            if (Palette != null)
            {
                if (FPalette == null)
                {
                    var call__58 = Aow.Units.GFXE.TGFXEPalette.Create();
                    FPalette = call__58;
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
            int Result = default;
            int Size = default;
            if ((AWidth != FWidth) || (AHeight != FHeight))
            {
                FWidth = AWidth;
                FHeight = AHeight;
                FPitch = ((((int)(((uint)(FBitCount + 7)) >> 3)) * FWidth) + 7) & ~7;
                Size = (FPitch * FHeight) + 7;
                FMem = FMem.Realloc(Size);
                FData = Aow.Rtl.Rtl.AlignPointer(FMem + 7, ~7).Box();
                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }

            return Result;
        }

        public object Data { get => FData; }
        public int Pitch { get => FPitch; }
        public Aow.Units.GFXE.TGFXEPalette Palette { get => FPalette; set => SetPalette(value); }

        // GFXE.pas:435
        public static Aow.Units.GFXE.TDataSurface Create(int AWidth, int AHeight, int APixelFormatID, Aow.Units.GFXE.TGFXEPalette APalette)
        {
            Aow.Units.GFXE.TDataSurface o = new Aow.Units.GFXE.TDataSurface();
            try
            {
                o.Create__init_TDataSurface(AWidth, AHeight, APixelFormatID, APalette);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        // GFXE.pas:436
        public static Aow.Units.GFXE.TDataSurface CreateFromSurface(Aow.Units.GFXE.TSurface Surface, Ptr<Aow.Rtl.TRect> R)
        {
            Aow.Units.GFXE.TDataSurface o = new Aow.Units.GFXE.TDataSurface();
            try
            {
                o.CreateFromSurface__init_TDataSurface(Surface, R);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { GFXE.TDIBSurface (instance size 1100): a DIB section of any format (8, 16 or 24 bit). +4
//     FHandle, +8 FWidth, +$C FHeight, +$10 FMem, +$14 FBits, +$18 FPitch, +$1C FPixelFormatID,
//     +$20 FInfo (BITMAPINFOHEADER and 256 colors), +$448 FPalette. }
    public partial class TDIBSurface : Aow.Units.GFXE.TSurface
    {
        internal int FHandle;
        internal int FWidth;
        internal int FHeight;
        internal Ptr<byte> FMem;
        internal object FBits;
        internal int FPitch;
        internal int FPixelFormatID;
        internal Aow.Rtl.TBitmapInfoHeader FInfo;
        internal Aow.Units.FixedArray_Aow_Rtl_TRGBQuad_256 FColors;
        internal Aow.Units.GFXE.TGFXEPalette FPalette;
        // GFXE.pas:6391
        // { The original unit was longer: the line numbers compiled into the Assert calls (7162 for the next Assert below) prove that 3417 more lines preceded this point. Their content is unknown; the blank lines below keep the Assert line numbers identical. }
        // { TDIBSurface }
        public override void Destroy()
        {
            Clear();
            base.Destroy();
        }

        // GFXE.pas:6397
        public Ptr<Aow.Rtl.TBitmapInfo> GetBITMAPINFO()
        {
            Ptr<Aow.Rtl.TBitmapInfo> Result = default;
            /* TODO(T-4.3) Address needs a cell or array backing */
            throw new System.NotImplementedException();
            return Result;
        }

        // GFXE.pas:6402
        public Ptr<Aow.Rtl.TBitmapInfoHeader> GetBITMAPINFOHEADER()
        {
            Ptr<Aow.Rtl.TBitmapInfoHeader> Result = default;
            /* TODO(T-4.3) Address needs a cell or array backing */
            throw new System.NotImplementedException();
            return Result;
        }

        // GFXE.pas:6407
        public override int GetWidth()
        {
            int Result = default;
            Result = FWidth;
            return Result;
        }

        // GFXE.pas:6412
        public override int GetHeight()
        {
            int Result = default;
            Result = FHeight;
            return Result;
        }

        // GFXE.pas:6417
        public void Clear()
        {
            /* TODO(T-shim) Platform or editor boundary Windows.DeleteObject */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:6435
        public override int UpdatePalette()
        {
            int Result = default;
            int I = default;
            if ((FPalette != null) && (FPixelFormatID == Aow.Units.GFXE.GFXE.pfPAL8))
            {
                {
                    I = 0;
                    byte end__59 = 255;
                    for (; I <= end__59; I += 1)
                    {
                        {
                            ref var w__60 = ref FPalette.Entries[I];
                            FColors[I].rgbRed = unchecked((byte)w__60.R);
                            FColors[I].rgbGreen = unchecked((byte)w__60.G);
                            FColors[I].rgbBlue = unchecked((byte)w__60.B);
                        }
                    }
                }

                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
            }

            return Result;
        }

        // GFXE.pas:6454
        public int Setup(int AWidth, int AHeight, int APixelFormatID, Aow.Units.GFXE.TGFXEPalette APalette)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetDC */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:6580
        public override int Resize(int AWidth, int AHeight)
        {
            int Result = default;
            if ((AWidth != FWidth) || (AHeight != FHeight))
            {
                var call__61 = Setup(AWidth, AHeight, FPixelFormatID, FPalette);
                Result = call__61;
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }

            return Result;
        }

        // GFXE.pas:6588
        public override int Lock(ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Result = default;
            SD = default;
            switch (FPixelFormatID)
            {
                case Aow.Units.GFXE.GFXE.pfPAL8:
                    SD.Width = FWidth;
                    SD.Height = FHeight;
                    SD.BitCount = 8;
                    SD.PixelFormatID = Aow.Units.GFXE.GFXE.pfPAL8;
                    SD.ClipRect.Left = 0;
                    SD.ClipRect.Right = FWidth;
                    SD.ClipRect.Top = 0;
                    SD.ClipRect.Bottom = FHeight;
                    SD.Pitch = FPitch;
                    SD.Palette = FPalette;
                    SD.Data = ((Aow.Rtl.Rtl.BytePointer(FBits) + System.Math.Abs(FHeight * FPitch)) + FPitch).Box();
                    Result = Aow.Units.GFXE.GFXE.GFXE_OK;
                    break;
                case Aow.Units.GFXE.GFXE.pfRGB555:
                case Aow.Units.GFXE.GFXE.pfRGB565:
                    SD.Width = FWidth;
                    SD.Height = FHeight;
                    SD.PixelFormatID = FPixelFormatID;
                    SD.BitCount = 16;
                    SD.ClipRect.Left = 0;
                    SD.ClipRect.Right = FWidth;
                    SD.ClipRect.Top = 0;
                    SD.ClipRect.Bottom = FHeight;
                    SD.Pitch = FPitch;
                    if (FPixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565)
                    {
                        SD.RMask = 63488;
                        SD.GMask = 2016;
                        SD.BMask = 31;
                    }
                    else
                    {
                        if (FPixelFormatID == Aow.Units.GFXE.GFXE.pfRGB555)
                        {
                            SD.RMask = 31744;
                            SD.GMask = 992;
                            SD.BMask = 31;
                        }
                    }

                    SD.Data = ((Aow.Rtl.Rtl.BytePointer(FBits) + System.Math.Abs(FHeight * FPitch)) + FPitch).Box();
                    Result = Aow.Units.GFXE.GFXE.GFXE_OK;
                    break;
                case Aow.Units.GFXE.GFXE.pfRGB24:
                    SD.Width = FWidth;
                    SD.Height = FHeight;
                    SD.BitCount = 24;
                    SD.PixelFormatID = Aow.Units.GFXE.GFXE.pfRGB24;
                    SD.ClipRect.Left = 0;
                    SD.ClipRect.Right = FWidth;
                    SD.ClipRect.Top = 0;
                    SD.ClipRect.Bottom = FHeight;
                    SD.Pitch = FPitch;
                    SD.RMask = 16711680;
                    SD.GMask = 65280;
                    SD.BMask = 255;
                    SD.Data = ((Aow.Rtl.Rtl.BytePointer(FBits) + System.Math.Abs(FHeight * FPitch)) + FPitch).Box();
                    Result = Aow.Units.GFXE.GFXE.GFXE_OK;
                    break;
                default:
                    Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                    break;
            }

            return Result;
        }

        // GFXE.pas:6655
        public override int Unlock()
        {
            int Result = default;
            Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            return Result;
        }

        // GFXE.pas:6660
        public override int Cls(int Color)
        {
            /* TODO(T-shim) Platform or editor boundary Graphics.ColorToRGB */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:6708
        public override int Blt(Aow.Units.GFXE.TSurface Dest, Ptr<Aow.Rtl.TRect> SrcRect, int X, int Y)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetDC */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:6767
        public void SaveToBMP24(Aow.Rtl.AnsiString FileName)
        {
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            Lock(ref SD);
            Aow.Units.GFXE.GFXE.SaveSurfaceDescToBMP24(SD, SD.ClipRect, FileName);
            Unlock();
        }

        public int Handle { get => FHandle; }
        public object Bits { get => FBits; }
        public int Pitch { get => FPitch; }
        public Aow.Units.GFXE.TGFXEPalette Palette { get => FPalette; }
    }

    // { GFXE.TDIB08Surface (instance size 1092): an 8 bit bitmap in memory (bottom-up: the pitch is
//     negative). +4 FWidth, +8 FHeight, +$C FMem, +$10 FBits, +$14 FPitch, +$18 FInfo (header and
//     256 colors), +$440 FPalette. }
    public partial class TDIB08Surface : Aow.Units.GFXE.TSurface
    {
        internal int FWidth;
        internal int FHeight;
        internal Ptr<byte> FMem;
        internal object FBits;
        internal int FPitch;
        internal Aow.Rtl.TBitmapInfoHeader FInfo;
        internal Aow.Units.FixedArray_Aow_Rtl_TRGBQuad_256 FColors;
        internal Aow.Units.GFXE.TGFXEPalette FPalette;
        // GFXE.pas:6778
        // { TDIB08Surface }
        public void Create__init_TDIB08Surface(int AWidth, int AHeight, Aow.Units.GFXE.TGFXEPalette APalette)
        {
            ;
            var call__62 = Aow.Units.GFXE.TGFXEPalette.Create();
            FPalette = call__62;
            FPalette.PixelFormatID = Aow.Units.GFXE.GFXE.pfRGB24;
            {
                ref var w__63 = ref FInfo;
                w__63.biSize = 40;
                w__63.biPlanes = 1;
                w__63.biBitCount = 8;
                w__63.biCompression = 0;
                w__63.biSizeImage = 0;
                w__63.biClrUsed = 256;
                w__63.biClrImportant = 0;
            }

            Resize(AWidth, AHeight);
            SetPalette(APalette);
        }

        // GFXE.pas:6797
        public override void Destroy()
        {
            FMem = FMem.Realloc(0);
            FPalette.Free();
            FPalette = null;
            base.Destroy();
        }

        // GFXE.pas:6805
        public override int GetPixelFormatID()
        {
            int Result = default;
            Result = Aow.Units.GFXE.GFXE.pfPAL8;
            return Result;
        }

        // GFXE.pas:6810
        public override int GetWidth()
        {
            int Result = default;
            Result = FWidth;
            return Result;
        }

        // GFXE.pas:6815
        public override int GetHeight()
        {
            int Result = default;
            Result = FHeight;
            return Result;
        }

        // GFXE.pas:6821
        // { Passes the address of the palette field, not the palette (original). }
        public override int UpdatePalette()
        {
            int Result = default;
            /* TODO(T-4.3) Address needs a cell or array backing */
            throw new System.NotImplementedException();
            Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            return Result;
        }

        // GFXE.pas:6827
        public override int Blt(Aow.Units.GFXE.TSurface Dest, Ptr<Aow.Rtl.TRect> SrcRect, int X, int Y)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetDC */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:6865
        public override int BltDC(int DC, Ptr<Aow.Rtl.TRect> SrcRect, Ptr<Aow.Rtl.TRect> DestRect)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetClientRect */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:6893
        public override int Lock(ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Result = default;
            SD = default;
            SD.Width = FWidth;
            SD.Height = FHeight;
            SD.BitCount = 8;
            SD.PixelFormatID = Aow.Units.GFXE.GFXE.pfPAL8;
            SD.ClipRect.Left = 0;
            SD.ClipRect.Right = FWidth;
            SD.ClipRect.Top = 0;
            SD.ClipRect.Bottom = FHeight;
            SD.Pitch = FPitch;
            SD.Palette = FPalette;
            SD.Data = ((Aow.Rtl.Rtl.BytePointer(FBits) + System.Math.Abs(FHeight * FPitch)) + FPitch).Box();
            Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            return Result;
        }

        // GFXE.pas:6910
        public override int Unlock()
        {
            int Result = default;
            Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            return Result;
        }

        // GFXE.pas:6915
        public override int Resize(int AWidth, int AHeight)
        {
            int Result = default;
            if ((AWidth != FWidth) || (AHeight != FHeight))
            {
                FWidth = AWidth;
                FHeight = AHeight;
                FPitch = -((FWidth + 7) & ~7);
                {
                    ref var w__64 = ref FInfo;
                    w__64.biWidth = System.Math.Abs(FPitch);
                    w__64.biHeight = FHeight;
                }

                FMem = FMem.Realloc(System.Math.Abs(FPitch * FHeight) + 7);
                FBits = Aow.Rtl.Rtl.AlignPointer(FMem + 7, ~7).Box();
                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }

            return Result;
        }

        // GFXE.pas:6935
        public override int Cls(int Color)
        {
            int Result = default;
            int Index = default;
            var call__65 = FPalette.GetNearestColor(Color);
            Index = call__65;
            Aow.Rtl.Rtl.FillBytes(Aow.Rtl.Rtl.BytePointer(FBits).AsSpan(System.Math.Abs(FPitch * FHeight)), Index);
            Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            return Result;
        }

        // GFXE.pas:6944
        public void SetPalette(Aow.Units.GFXE.TGFXEPalette Palette)
        {
            int I = default;
            if (Palette != null)
            {
                {
                    I = 0;
                    byte end__66 = 255;
                    for (; I <= end__66; I += 1)
                    {
                        {
                            ref var w__67 = ref Palette.Entries[I];
                            FColors[I].rgbRed = unchecked((byte)w__67.R);
                            FColors[I].rgbGreen = unchecked((byte)w__67.G);
                            FColors[I].rgbBlue = unchecked((byte)w__67.B);
                        }
                    }
                }

                /* TODO(T-4.3) Address needs a cell or array backing */
                throw new System.NotImplementedException();
            }
        }

        public Aow.Units.GFXE.TGFXEPalette Palette { get => FPalette; set => SetPalette(value); }

        // GFXE.pas:501
        public static Aow.Units.GFXE.TDIB08Surface Create(int AWidth, int AHeight, Aow.Units.GFXE.TGFXEPalette APalette)
        {
            Aow.Units.GFXE.TDIB08Surface o = new Aow.Units.GFXE.TDIB08Surface();
            try
            {
                o.Create__init_TDIB08Surface(AWidth, AHeight, APalette);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { GFXE.TDIB16Surface (instance size 80): a 16 bit DIB section (555 or 565). +4 FHandle, +8
//     FWidth, +$C FHeight, +$10 FBits, +$14 FPitch, +$18 FPixelFormatID, +$1C FInfo, +$44 FMasks. }
    public partial class TDIB16Surface : Aow.Units.GFXE.TSurface
    {
        internal int FHandle;
        internal int FWidth;
        internal int FHeight;
        internal object FBits;
        internal int FPitch;
        internal int FPixelFormatID;
        internal Aow.Rtl.TBitmapInfoHeader FInfo;
        internal Aow.Units.FixedArray_int_3 FMasks;
        // GFXE.pas:7006
        internal void SetPixelFormatID(int Value)
        {
            if ((Value == Aow.Units.GFXE.GFXE.pfRGB565) || (Value == Aow.Units.GFXE.GFXE.pfRGB555))
            {
                FPixelFormatID = Value;
                switch (FPixelFormatID)
                {
                    case Aow.Units.GFXE.GFXE.pfRGB565:
                        FMasks[0] = 63488;
                        FMasks[1] = 2016;
                        FMasks[2] = 31;
                        break;
                    case Aow.Units.GFXE.GFXE.pfRGB555:
                        FMasks[0] = 31744;
                        FMasks[1] = 992;
                        FMasks[2] = 31;
                        break;
                }
            }
            else
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"PixelFormatID not supported");
            }
        }

        // GFXE.pas:6963
        // { TDIB16Surface }
        public void Create__init_TDIB16Surface(int AWidth, int AHeight, int APixelFormatID)
        {
            ;
            FWidth = 0;
            FHeight = 0;
            FPixelFormatID = APixelFormatID;
            {
                ref var w__68 = ref FInfo;
                w__68.biSize = 40;
                w__68.biPlanes = 1;
                w__68.biBitCount = 16;
                w__68.biCompression = 3;
                w__68.biSizeImage = 0;
                w__68.biClrUsed = 0;
                w__68.biClrImportant = 0;
                w__68.biXPelsPerMeter = 0;
                w__68.biYPelsPerMeter = 0;
            }

            SetPixelFormatID(FPixelFormatID);
            Resize(AWidth, AHeight);
        }

        // GFXE.pas:6985
        public override void Destroy()
        {
            Resize(0, 0);
            base.Destroy();
        }

        // GFXE.pas:6991
        public override int GetWidth()
        {
            int Result = default;
            Result = FWidth;
            return Result;
        }

        // GFXE.pas:6996
        public override int GetHeight()
        {
            int Result = default;
            Result = FHeight;
            return Result;
        }

        // GFXE.pas:7001
        public override int GetPixelFormatID()
        {
            int Result = default;
            Result = FPixelFormatID;
            return Result;
        }

        // GFXE.pas:7030
        public override int Blt(Aow.Units.GFXE.TSurface Dest, Ptr<Aow.Rtl.TRect> SrcRect, int X, int Y)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetDC */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:7068
        public override int BltDC(int DC, Ptr<Aow.Rtl.TRect> SrcRect, Ptr<Aow.Rtl.TRect> DestRect)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetClientRect */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:7097
        public override int Lock(ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Result = default;
            SD = default;
            SD.Width = FWidth;
            SD.Height = FHeight;
            SD.PixelFormatID = FPixelFormatID;
            SD.BitCount = 16;
            SD.ClipRect.Left = 0;
            SD.ClipRect.Right = FWidth;
            SD.ClipRect.Top = 0;
            SD.ClipRect.Bottom = FHeight;
            SD.Pitch = FPitch;
            if (FPixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565)
            {
                SD.RMask = 63488;
                SD.GMask = 2016;
                SD.BMask = 31;
            }

            if (FPixelFormatID == Aow.Units.GFXE.GFXE.pfRGB555)
            {
                SD.RMask = 31744;
                SD.GMask = 992;
                SD.BMask = 31;
            }

            SD.Data = ((Aow.Rtl.Rtl.BytePointer(FBits) + System.Math.Abs(FHeight * FPitch)) + FPitch).Box();
            Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            return Result;
        }

        // GFXE.pas:7125
        public override int Unlock()
        {
            int Result = default;
            Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            return Result;
        }

        // GFXE.pas:7130
        public override int Cls(int Color)
        {
            /* TODO(T-shim) Platform or editor boundary Graphics.ColorToRGB */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:7150
        public override int Resize(int AWidth, int AHeight)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.DeleteObject */
            throw new System.NotImplementedException();
        }

        public int Handle { get => FHandle; }
        public new int PixelFormatID { get => FPixelFormatID; set => SetPixelFormatID(value); }

        // GFXE.pas:531
        public static Aow.Units.GFXE.TDIB16Surface Create(int AWidth, int AHeight, int APixelFormatID)
        {
            Aow.Units.GFXE.TDIB16Surface o = new Aow.Units.GFXE.TDIB16Surface();
            try
            {
                o.Create__init_TDIB16Surface(AWidth, AHeight, APixelFormatID);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { GFXE.TDIB24Surface (instance size 64): a 24 bit DIB section. +4 FHandle, +8 FWidth, +$C
//     FHeight, +$10 FBits, +$14 FPitch, +$18 FInfo. }
    public partial class TDIB24Surface : Aow.Units.GFXE.TSurface
    {
        internal int FHandle;
        internal int FWidth;
        internal int FHeight;
        internal object FBits;
        internal int FPitch;
        internal Aow.Rtl.TBitmapInfoHeader FInfo;
        // GFXE.pas:7216
        // { TDIB24Surface }
        public void Create__init_TDIB24Surface(int AWidth, int AHeight)
        {
            ;
            {
                ref var w__69 = ref FInfo;
                w__69.biSize = 40;
                w__69.biPlanes = 1;
                w__69.biBitCount = 24;
                w__69.biCompression = 0;
                w__69.biSizeImage = 0;
                w__69.biClrUsed = 0;
                w__69.biClrImportant = 0;
            }

            Resize(AWidth, AHeight);
        }

        // GFXE.pas:7232
        public override void Destroy()
        {
            Resize(0, 0);
            base.Destroy();
        }

        // GFXE.pas:7238
        public override int GetWidth()
        {
            int Result = default;
            Result = FWidth;
            return Result;
        }

        // GFXE.pas:7243
        public override int GetHeight()
        {
            int Result = default;
            Result = FHeight;
            return Result;
        }

        // GFXE.pas:7248
        public Ptr<Aow.Rtl.TBitmapInfoHeader> GetBMPInfo()
        {
            Ptr<Aow.Rtl.TBitmapInfoHeader> Result = default;
            /* TODO(T-4.3) Address needs a cell or array backing */
            throw new System.NotImplementedException();
            return Result;
        }

        // GFXE.pas:7253
        public override int Blt(Aow.Units.GFXE.TSurface Dest, Ptr<Aow.Rtl.TRect> SrcRect, int X, int Y)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetDC */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:7291
        public override int BltDC(int DC, Ptr<Aow.Rtl.TRect> SrcRect, Ptr<Aow.Rtl.TRect> DestRect)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetClientRect */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:7319
        public override int Cls(int Color)
        {
            /* TODO(T-shim) Platform or editor boundary Graphics.ColorToRGB */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:7338
        public override int Lock(ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Result = default;
            SD = default;
            SD.Width = FWidth;
            SD.Height = FHeight;
            SD.BitCount = 24;
            SD.PixelFormatID = Aow.Units.GFXE.GFXE.pfRGB24;
            SD.ClipRect.Left = 0;
            SD.ClipRect.Right = FWidth;
            SD.ClipRect.Top = 0;
            SD.ClipRect.Bottom = FHeight;
            SD.Pitch = FPitch;
            SD.RMask = 16711680;
            SD.GMask = 65280;
            SD.BMask = 255;
            SD.Data = ((Aow.Rtl.Rtl.BytePointer(FBits) + System.Math.Abs(FHeight * FPitch)) + FPitch).Box();
            Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            return Result;
        }

        // GFXE.pas:7357
        public override int Unlock()
        {
            int Result = default;
            Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            return Result;
        }

        // GFXE.pas:7362
        public override int Resize(int AWidth, int AHeight)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.DeleteObject */
            throw new System.NotImplementedException();
        }

        public int Handle { get => FHandle; }

        // GFXE.pas:557
        public static Aow.Units.GFXE.TDIB24Surface Create(int AWidth, int AHeight)
        {
            Aow.Units.GFXE.TDIB24Surface o = new Aow.Units.GFXE.TDIB24Surface();
            try
            {
                o.Create__init_TDIB24Surface(AWidth, AHeight);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { GFXE.TDIB32Surface (instance size 64): a 32 bit bitmap in memory (bottom-up). +4 FWidth, +8
//     FHeight, +$C FMem, +$10 FBits, +$14 FPitch, +$18 FInfo. }
    public partial class TDIB32Surface : Aow.Units.GFXE.TSurface
    {
        internal int FWidth;
        internal int FHeight;
        internal Ptr<byte> FMem;
        internal object FBits;
        internal int FPitch;
        internal Aow.Rtl.TBitmapInfoHeader FInfo;
        // GFXE.pas:7424
        // { TDIB32Surface }
        public void Create__init_TDIB32Surface(int AWidth, int AHeight)
        {
            ;
            {
                ref var w__70 = ref FInfo;
                w__70.biSize = 40;
                w__70.biPlanes = 1;
                w__70.biBitCount = 32;
                w__70.biCompression = 0;
                w__70.biSizeImage = 0;
                w__70.biClrUsed = 0;
                w__70.biClrImportant = 0;
            }

            Resize(AWidth, AHeight);
        }

        // GFXE.pas:7440
        public override void Destroy()
        {
            FMem = FMem.Realloc(0);
            base.Destroy();
        }

        // GFXE.pas:7446
        public override int GetWidth()
        {
            int Result = default;
            Result = FWidth;
            return Result;
        }

        // GFXE.pas:7451
        public override int GetHeight()
        {
            int Result = default;
            Result = FHeight;
            return Result;
        }

        // GFXE.pas:7456
        public override int Blt(Aow.Units.GFXE.TSurface Dest, Ptr<Aow.Rtl.TRect> SrcRect, int X, int Y)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetDC */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:7494
        public override int BltDC(int DC, Ptr<Aow.Rtl.TRect> SrcRect, Ptr<Aow.Rtl.TRect> DestRect)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetClientRect */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:7522
        public override int Lock(ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Result = default;
            SD = default;
            SD.Width = FWidth;
            SD.Height = FHeight;
            SD.BitCount = 32;
            SD.PixelFormatID = Aow.Units.GFXE.GFXE.pfRGB32;
            SD.ClipRect.Left = 0;
            SD.ClipRect.Right = FWidth;
            SD.ClipRect.Top = 0;
            SD.ClipRect.Bottom = FHeight;
            SD.Pitch = FPitch;
            SD.RMask = 16711680;
            SD.GMask = 65280;
            SD.BMask = 255;
            SD.AMask = -16777216;
            SD.Data = ((Aow.Rtl.Rtl.BytePointer(FBits) + System.Math.Abs(FHeight * FPitch)) + FPitch).Box();
            Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            return Result;
        }

        // GFXE.pas:7542
        public override int Unlock()
        {
            int Result = default;
            Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            return Result;
        }

        // GFXE.pas:7547
        public override int Cls(int Color)
        {
            /* TODO(T-shim) Platform or editor boundary Graphics.ColorToRGB */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:7557
        public override int Resize(int AWidth, int AHeight)
        {
            int Result = default;
            if ((AWidth != FWidth) || (AHeight != FHeight))
            {
                FWidth = AWidth;
                FHeight = AHeight;
                FPitch = -(((FWidth * 4) + 7) & ~7);
                {
                    ref var w__71 = ref FInfo;
                    w__71.biWidth = (int)(((uint)System.Math.Abs(FPitch)) >> 2);
                    w__71.biHeight = FHeight;
                }

                FMem = FMem.Realloc(System.Math.Abs(FPitch * FHeight) + 7);
                FBits = Aow.Rtl.Rtl.AlignPointer(FMem + 7, ~7).Box();
                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }

            return Result;
        }

        // GFXE.pas:582
        public static Aow.Units.GFXE.TDIB32Surface Create(int AWidth, int AHeight)
        {
            Aow.Units.GFXE.TDIB32Surface o = new Aow.Units.GFXE.TDIB32Surface();
            try
            {
                o.Create__init_TDIB32Surface(AWidth, AHeight);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { GFXE.TGDISurface (instance size 8): a window drawn through GDI. +4 FHandle. Two more
//     virtuals: GetHandle, SetHandle. }
    public partial class TGDISurface : Aow.Units.GFXE.TSurface
    {
        internal int FHandle;
        // GFXE.pas:12293
        // { TGDISurface }
        public void Create__init_TGDISurface(int AHandle)
        {
            ;
            FHandle = AHandle;
        }

        // GFXE.pas:12299
        public virtual int GetHandle()
        {
            int Result = default;
            Result = FHandle;
            return Result;
        }

        // GFXE.pas:12304
        public virtual void SetHandle(int Value)
        {
            FHandle = Value;
        }

        // GFXE.pas:12309
        public override int GetWidth()
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetClientRect */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:12319
        public override int GetHeight()
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetClientRect */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:12329
        public override int Blt(Aow.Units.GFXE.TSurface Dest, Ptr<Aow.Rtl.TRect> SrcRect, int X, int Y)
        {
            int Result = default;
            Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
            return Result;
        }

        // GFXE.pas:12334
        public override int BltDC(int DC, Ptr<Aow.Rtl.TRect> SrcRect, Ptr<Aow.Rtl.TRect> DestRect)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetDC */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:12360
        public override int Resize(int AWidth, int AHeight)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.SetWindowPos */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:12368
        public override int Cls(int Color)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetDC */
            throw new System.NotImplementedException();
        }

        public int Handle { get => GetHandle(); set => SetHandle(value); }

        // GFXE.pas:600
        public static Aow.Units.GFXE.TGDISurface Create(int AHandle)
        {
            Aow.Units.GFXE.TGDISurface o = new Aow.Units.GFXE.TGDISurface();
            try
            {
                o.Create__init_TGDISurface(AHandle);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { GFXE.TDDrawSurface (instance size 100): a DirectDraw surface. +4 FOnSurfaceLost, +$C
//     FPixelFormatID, +$10 FWidth, +$14 FHeight, +$18 FSurface, +$1C FCreationFlags, +$1D FStatus,
//     +$20 FLockCount, +$24 FLockExCount, +$28 FSurfaceDesc (the description of the lock). }
    public partial class TDDrawSurface : Aow.Units.GFXE.TSurface
    {
        protected internal Aow.Rtl.TNotifyEvent FOnSurfaceLost;
        protected internal int FPixelFormatID;
        protected internal int FWidth;
        protected internal int FHeight;
        protected internal Aow.Units.GFXE.IDirectDrawSurface2 FSurface;
        protected internal TSet<Aow.Units.GFXE.TDDSCreationFlagsElement> FCreationFlags;
        protected internal TSet<Aow.Units.GFXE.TDDrawSurfaceStatusElement> FStatus;
        protected internal int FLockCount;
        protected internal int FLockExCount;
        protected internal Aow.Units.GFXE.TSurfaceDesc FSurfaceDesc;
        // GFXE.pas:9108
        // { The original unit was longer: the line numbers compiled into the Assert calls (9342 for the next Assert below) prove that 37 more lines preceded this point. Their content is unknown; the blank lines below keep the Assert line numbers identical. }
        // { TDDrawSurface (inc\DDSurf.PAS in the original) }
        public int CreateSurface(int AWidth, int AHeight, int APixelFormatID, TSet<Aow.Units.GFXE.TDDSCreationFlagsElement> AFlags)
        {
            int Result = default;
            Aow.Units.GFXE.TDDSurfaceDesc DDSD = default;
            Aow.Units.GFXE.IDirectDrawSurface S = default;
            Result = Aow.Units.GFXE.GFXE.DD_OK;
            if (FSurface != null)
            {
                FSurface = null;
            }

            FWidth = AWidth;
            FHeight = AHeight;
            FPixelFormatID = APixelFormatID;
            if (!AFlags.Contains(Aow.Units.GFXE.TDDSCreationFlagsElement.cfVideoMemory))
            {
                AFlags = AFlags + TSet<Aow.Units.GFXE.TDDSCreationFlagsElement>.Of(Aow.Units.GFXE.TDDSCreationFlagsElement.cfSystemMemory);
            }

            FCreationFlags = AFlags;
            DDSD = default;
            DDSD.dwSize = 108;
            DDSD.dwWidth = AWidth;
            DDSD.dwHeight = AHeight;
            DDSD.dwFlags = ((Aow.Units.GFXE.GFXE.DDSD_CAPS | Aow.Units.GFXE.GFXE.DDSD_HEIGHT) | Aow.Units.GFXE.GFXE.DDSD_WIDTH) | Aow.Units.GFXE.GFXE.DDSD_PIXELFORMAT;
            DDSD.ddpfPixelFormat.dwSize = 32;
            DDSD.ddpfPixelFormat.dwFlags = Aow.Units.GFXE.GFXE.DDPF_RGB;
            var call__72 = Aow.Units.GFXE.GFXE.PFIDtoMASK(APixelFormatID, ref Unsafe.As<int, int>(ref DDSD.ddpfPixelFormat.dwRBitMask), ref Unsafe.As<int, int>(ref DDSD.ddpfPixelFormat.dwGBitMask), ref Unsafe.As<int, int>(ref DDSD.ddpfPixelFormat.dwBBitMask), ref Unsafe.As<int, int>(ref DDSD.ddpfPixelFormat.dwRGBAlphaBitMask));
            DDSD.ddpfPixelFormat.dwRGBBitCount = call__72;
            if (AFlags.Contains(Aow.Units.GFXE.TDDSCreationFlagsElement.cfVideoMemory))
            {
                DDSD.ddsCaps.dwCaps = Aow.Units.GFXE.GFXE.DDSCAPS_VIDEOMEMORY | Aow.Units.GFXE.GFXE.DDSCAPS_OFFSCREENPLAIN;
                var call__73 = Aow.Units.GFXE.GFXE.DD();
                var call__74 = call__73.CreateSurface(DDSD, ref S, null);
                Result = call__74;
                if (Aow.Rtl.Rtl.Succeeded(Result))
                {
                    FStatus = FStatus + TSet<Aow.Units.GFXE.TDDrawSurfaceStatusElement>.Of(Aow.Units.GFXE.TDDrawSurfaceStatusElement.ssVideoMemory);
                }
            }

            if ((S == null) && AFlags.Contains(Aow.Units.GFXE.TDDSCreationFlagsElement.cfSystemMemory))
            {
                DDSD.ddsCaps.dwCaps = Aow.Units.GFXE.GFXE.DDSCAPS_SYSTEMMEMORY | Aow.Units.GFXE.GFXE.DDSCAPS_OFFSCREENPLAIN;
                var call__75 = Aow.Units.GFXE.GFXE.DD();
                var call__76 = call__75.CreateSurface(DDSD, ref S, null);
                Result = call__76;
            }

            if (Aow.Rtl.Rtl.Succeeded(Result))
            {
                /* TODO(T-5.8) No physical byte image for IDirectDrawSurface2 */
                throw new System.NotImplementedException();
            }

            S = null;
            if (Aow.Rtl.Rtl.Succeeded(Result))
            {
                FStatus = FStatus + TSet<Aow.Units.GFXE.TDDrawSurfaceStatusElement>.Of(Aow.Units.GFXE.TDDrawSurfaceStatusElement.ssCreated);
            }

            return Result;
        }

        // GFXE.pas:9147
        public override void Destroy()
        {
            if (FSurface != null)
            {
                while (true)
                {
                    var call__78 = FSurface.GetFlipStatus(Aow.Units.GFXE.GFXE.DDGFS_ISFLIPDONE);
                    if (!(call__78 == Aow.Units.GFXE.GFXE.DDERR_WASSTILLDRAWING))
                        break;
                    {
                        ;
                    }
                }

                while (true)
                {
                    var call__79 = FSurface.GetBltStatus(Aow.Units.GFXE.GFXE.DDGBS_CANBLT);
                    if (!(call__79 == Aow.Units.GFXE.GFXE.DDERR_WASSTILLDRAWING))
                        break;
                    {
                        ;
                    }
                }

                FSurface = null;
            }

            base.Destroy();
        }

        // GFXE.pas:9158
        public bool Lost()
        {
            bool Result = default;
            bool bool__80 = (FSurface != null);
            if (bool__80)
            {
                var call__81 = FSurface.IsLost();
                bool__80 = (call__81 == Aow.Units.GFXE.GFXE.DDERR_SURFACELOST);
            }

            Result = bool__80;
            return Result;
        }

        // GFXE.pas:9163
        public override int Blt(Aow.Units.GFXE.TSurface Dest, Ptr<Aow.Rtl.TRect> SrcRect, int X, int Y)
        {
            int Result = default;
            Aow.Rtl.TRect[] R__cell = new Aow.Rtl.TRect[1];
            ref Aow.Rtl.TRect R = ref R__cell[0];
            if (FStatus.Contains(Aow.Units.GFXE.TDDrawSurfaceStatusElement.ssLocked))
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"SURFACE IS LOCKED");
            }

            if (Dest is Aow.Units.GFXE.TDDrawSurface)
            {
                if (((Aow.Units.GFXE.TDDrawSurface)Dest).FStatus.Contains(Aow.Units.GFXE.TDDrawSurfaceStatusElement.ssLocked))
                {
                    throw Aow.Rtl.Exception.Create((AnsiString)"DESTSURFACE IS LOCKED");
                }

                if (SrcRect.IsNull)
                {
                    R.Left = 0;
                    R.Right = FWidth;
                    R.Top = 0;
                    R.Bottom = FHeight;
                    SrcRect = Ptr<Aow.Rtl.TRect>.To(R__cell, 0);
                }

                if (FStatus.Contains(Aow.Units.GFXE.TDDrawSurfaceStatusElement.ssVideoMemory) && ((Aow.Units.GFXE.TDDrawSurface)Dest).FStatus.Contains(Aow.Units.GFXE.TDDrawSurfaceStatusElement.ssVideoMemory))
                {
                    var call__82 = ((Aow.Units.GFXE.TDDrawSurface)Dest).FSurface.BltFast(X, Y, FSurface, SrcRect, Aow.Units.GFXE.GFXE.DDBLTFAST_WAIT);
                    Result = call__82;
                }
                else
                {
                    Result = base.Blt(Dest, SrcRect, X, Y);
                }
            }
            else
            {
                Result = base.Blt(Dest, SrcRect, X, Y);
            }

            if (Result == Aow.Units.GFXE.GFXE.DDERR_SURFACELOST)
            {
                var call__83 = FSurface.IsLost();
                if (call__83 == Aow.Units.GFXE.GFXE.DDERR_SURFACELOST)
                {
                    if (FOnSurfaceLost != null)
                    {
                        FOnSurfaceLost(this);
                        var call__84 = FSurface.IsLost();
                        if (call__84 != Aow.Units.GFXE.GFXE.DDERR_SURFACELOST)
                        {
                            Result = Blt(Dest, SrcRect, X, Y);
                        }
                    }
                }
                else
                {
                    if (Dest is Aow.Units.GFXE.TDDrawSurface)
                    {
                        var call__85 = ((Aow.Units.GFXE.TDDrawSurface)Dest).FSurface.IsLost();
                        if ((call__85 == Aow.Units.GFXE.GFXE.DDERR_SURFACELOST) && (((Aow.Units.GFXE.TDDrawSurface)Dest).FOnSurfaceLost != null))
                        {
                            ((Aow.Units.GFXE.TDDrawSurface)Dest).FOnSurfaceLost(this);
                            var call__86 = ((Aow.Units.GFXE.TDDrawSurface)Dest).FSurface.IsLost();
                            if (call__86 != Aow.Units.GFXE.GFXE.DDERR_SURFACELOST)
                            {
                                Result = Blt(Dest, SrcRect, X, Y);
                            }
                        }
                    }
                }
            }

            return Result;
        }

        // GFXE.pas:9205
        public override int BltColorKey(Aow.Units.GFXE.TSurface Dest, Ptr<Aow.Rtl.TRect> SrcRect, int X, int Y, int ColorKey)
        {
            int Result = default;
            Aow.Units.GFXE.TDDColorKey[] Key__cell = new Aow.Units.GFXE.TDDColorKey[1];
            ref Aow.Units.GFXE.TDDColorKey Key = ref Key__cell[0];
            Aow.Rtl.TRect[] R__cell = new Aow.Rtl.TRect[1];
            ref Aow.Rtl.TRect R = ref R__cell[0];
            Aow.Rtl.TRect[] DR__cell = new Aow.Rtl.TRect[1];
            ref Aow.Rtl.TRect DR = ref DR__cell[0];
            if (Dest is Aow.Units.GFXE.TDDrawSurface)
            {
                if (SrcRect.IsNull)
                {
                    R.Left = 0;
                    R.Right = FWidth;
                    R.Top = 0;
                    R.Bottom = FHeight;
                    SrcRect = Ptr<Aow.Rtl.TRect>.To(R__cell, 0);
                }

                Key.dwColorSpaceLowValue = ColorKey;
                Key.dwColorSpaceHighValue = ColorKey;
                var call__87 = FSurface.SetColorKey(Aow.Units.GFXE.GFXE.DDCKEY_SRCBLT, Ptr<Aow.Units.GFXE.TDDColorKey>.To(Key__cell, 0));
                Result = call__87;
                if (Aow.Rtl.Rtl.Succeeded(Result))
                {
                    if (FStatus.Contains(Aow.Units.GFXE.TDDrawSurfaceStatusElement.ssVideoMemory) && ((Aow.Units.GFXE.TDDrawSurface)Dest).FStatus.Contains(Aow.Units.GFXE.TDDrawSurfaceStatusElement.ssVideoMemory))
                    {
                        var call__88 = ((Aow.Units.GFXE.TDDrawSurface)Dest).FSurface.BltFast(X, Y, FSurface, SrcRect, Aow.Units.GFXE.GFXE.DDBLTFAST_SRCCOLORKEY | Aow.Units.GFXE.GFXE.DDBLTFAST_WAIT);
                        Result = call__88;
                    }
                    else
                    {
                        DR.Left = X;
                        DR.Top = Y;
                        DR.Right = (SrcRect.Value.Right - SrcRect.Value.Left) + X;
                        DR.Bottom = (SrcRect.Value.Bottom - SrcRect.Value.Top) + Y;
                        var call__89 = ((Aow.Units.GFXE.TDDrawSurface)Dest).FSurface.Blt(Ptr<Aow.Rtl.TRect>.To(DR__cell, 0), FSurface, SrcRect, Aow.Units.GFXE.GFXE.DDBLT_WAIT | Aow.Units.GFXE.GFXE.DDBLT_KEYSRC, default);
                        Result = call__89;
                    }
                }

                if (Aow.Rtl.Rtl.Failed(Result))
                {
                    Result = base.BltColorKey(Dest, SrcRect, X, Y, ColorKey);
                }
            }
            else
            {
                Result = base.BltColorKey(Dest, SrcRect, X, Y, ColorKey);
            }

            if (Result == Aow.Units.GFXE.GFXE.DDERR_SURFACELOST)
            {
                var call__90 = FSurface.IsLost();
                if (call__90 == Aow.Units.GFXE.GFXE.DDERR_SURFACELOST)
                {
                    if (FOnSurfaceLost != null)
                    {
                        FOnSurfaceLost(this);
                        var call__91 = FSurface.IsLost();
                        if (call__91 != Aow.Units.GFXE.GFXE.DDERR_SURFACELOST)
                        {
                            Result = BltColorKey(Dest, SrcRect, X, Y, ColorKey);
                        }
                    }
                }
                else
                {
                    if (Dest is Aow.Units.GFXE.TDDrawSurface)
                    {
                        var call__92 = ((Aow.Units.GFXE.TDDrawSurface)Dest).FSurface.IsLost();
                        if ((call__92 == Aow.Units.GFXE.GFXE.DDERR_SURFACELOST) && (((Aow.Units.GFXE.TDDrawSurface)Dest).FOnSurfaceLost != null))
                        {
                            ((Aow.Units.GFXE.TDDrawSurface)Dest).FOnSurfaceLost(this);
                            var call__93 = ((Aow.Units.GFXE.TDDrawSurface)Dest).FSurface.IsLost();
                            if (call__93 != Aow.Units.GFXE.GFXE.DDERR_SURFACELOST)
                            {
                                Result = BltColorKey(Dest, SrcRect, X, Y, ColorKey);
                            }
                        }
                    }
                }
            }

            return Result;
        }

        // GFXE.pas:9279
        public override int GetPixelFormatID()
        {
            int Result = default;
            Result = FPixelFormatID;
            return Result;
        }

        // GFXE.pas:9284
        public override int GetWidth()
        {
            int Result = default;
            Result = FWidth;
            return Result;
        }

        // GFXE.pas:9289
        public override int GetHeight()
        {
            int Result = default;
            Result = FHeight;
            return Result;
        }

        // GFXE.pas:9294
        public override int Lock(ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Result = default;
            Aow.Units.GFXE.TDDSurfaceDesc DDSD = default;
            if (FLockCount == 0)
            {
                DDSD = default;
                DDSD.dwSize = 108;
                do
                {
                    var call__94 = FSurface.Lock(default, ref DDSD, Aow.Units.GFXE.GFXE.DDLOCK_WAIT, 0);
                    Result = call__94;
                }
                while (!(Result != Aow.Units.GFXE.GFXE.DDERR_WASSTILLDRAWING));
                if (Aow.Rtl.Rtl.Succeeded(Result))
                {
                    FLockCount += 1;
                    FStatus = FStatus + TSet<Aow.Units.GFXE.TDDrawSurfaceStatusElement>.Of(Aow.Units.GFXE.TDDrawSurfaceStatusElement.ssLocked);
                    SD = default;
                    SD.Width = DDSD.dwWidth;
                    SD.Height = DDSD.dwHeight;
                    SD.Pitch = DDSD.lPitch;
                    SD.ClipRect.Left = 0;
                    SD.ClipRect.Right = DDSD.dwWidth;
                    SD.ClipRect.Top = 0;
                    SD.ClipRect.Bottom = DDSD.dwHeight;
                    SD.Data = DDSD.lpSurface;
                    SD.PixelFormatID = FPixelFormatID;
                    SD.BitCount = DDSD.ddpfPixelFormat.dwRGBBitCount;
                    SD.RMask = DDSD.ddpfPixelFormat.dwRBitMask;
                    SD.GMask = DDSD.ddpfPixelFormat.dwGBitMask;
                    SD.BMask = DDSD.ddpfPixelFormat.dwBBitMask;
                    SD.AMask = DDSD.ddpfPixelFormat.dwRGBAlphaBitMask;
                }
                else
                {
                    if ((Result == Aow.Units.GFXE.GFXE.DDERR_SURFACELOST) && (FOnSurfaceLost != null))
                    {
                        FOnSurfaceLost(this);
                        var call__95 = FSurface.IsLost();
                        if (call__95 != Aow.Units.GFXE.GFXE.DDERR_SURFACELOST)
                        {
                            Result = Lock(ref SD);
                        }
                    }
                }

                if (Aow.Rtl.Rtl.Succeeded(Result))
                {
                    FSurfaceDesc = SD;
                }
            }
            else
            {
                FLockCount += 1;
                SD = FSurfaceDesc;
                Result = Aow.Units.GFXE.GFXE.DD_OK;
            }

            return Result;
        }

        // GFXE.pas:9340
        public override int Unlock()
        {
            int Result = default;
            Aow.Rtl.Rtl.Assert(FLockCount != 0, (AnsiString)"Invalid Unlock", "D:\\git\\aow\\src\\delphi3\\Gfxepack\\GFXE.pas", 9342);
            if (FLockCount == 1)
            {
                var call__96 = FSurface.Unlock(null);
                Result = call__96;
                if (Aow.Rtl.Rtl.Succeeded(Result))
                {
                    FLockCount -= 1;
                    if (FLockExCount == 0)
                    {
                        FStatus = FStatus - TSet<Aow.Units.GFXE.TDDrawSurfaceStatusElement>.Of(Aow.Units.GFXE.TDDrawSurfaceStatusElement.ssLocked);
                    }
                }
            }
            else
            {
                FLockCount -= 1;
                Result = Aow.Units.GFXE.GFXE.DD_OK;
            }

            return Result;
        }

        // GFXE.pas:9359
        public override int LockEx(ref Aow.Units.GFXE.TSurfaceDesc SD, in Aow.Rtl.TRect R)
        {
            int Result = default;
            Aow.Units.GFXE.TDDSurfaceDesc DDSD = default;
            DDSD = default;
            DDSD.dwSize = 108;
            do
            {
                /* TODO(T-4.3) Address needs a cell or array backing */
                throw new System.NotImplementedException();
            }
            while (!(Result != Aow.Units.GFXE.GFXE.DDERR_WASSTILLDRAWING));
            if (Aow.Rtl.Rtl.Succeeded(Result))
            {
                FLockExCount += 1;
                FStatus = TSet<Aow.Units.GFXE.TDDrawSurfaceStatusElement>.Of(Aow.Units.GFXE.TDDrawSurfaceStatusElement.ssLocked) + FStatus;
                SD = default;
                SD.Width = R.Right - R.Left;
                SD.Height = R.Bottom - R.Top;
                SD.Pitch = DDSD.lPitch;
                SD.ClipRect.Left = 0;
                SD.ClipRect.Right = SD.Width;
                SD.ClipRect.Top = 0;
                SD.ClipRect.Bottom = SD.Height;
                SD.Data = DDSD.lpSurface;
                SD.PixelFormatID = FPixelFormatID;
                SD.BitCount = DDSD.ddpfPixelFormat.dwRGBBitCount;
                SD.RMask = DDSD.ddpfPixelFormat.dwRBitMask;
                SD.GMask = DDSD.ddpfPixelFormat.dwGBitMask;
                SD.BMask = DDSD.ddpfPixelFormat.dwBBitMask;
                SD.AMask = DDSD.ddpfPixelFormat.dwRGBAlphaBitMask;
            }
            else
            {
                if ((Result == Aow.Units.GFXE.GFXE.DDERR_SURFACELOST) && (FOnSurfaceLost != null))
                {
                    FOnSurfaceLost(this);
                    var call__98 = FSurface.IsLost();
                    if (call__98 != Aow.Units.GFXE.GFXE.DDERR_SURFACELOST)
                    {
                        Result = LockEx(ref SD, R);
                    }
                }
            }

            return Result;
        }

        // GFXE.pas:9395
        public override int UnlockEx(in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Result = default;
            var call__99 = FSurface.Unlock(SD.Data);
            Result = call__99;
            if (Aow.Rtl.Rtl.Succeeded(Result))
            {
                FLockExCount -= 1;
                if ((FLockExCount == 0) && (FLockExCount == 0))
                {
                    // // sic
                    FStatus = FStatus - TSet<Aow.Units.GFXE.TDDrawSurfaceStatusElement>.Of(Aow.Units.GFXE.TDDrawSurfaceStatusElement.ssLocked);
                }
            }

            return Result;
        }

        // GFXE.pas:9405
        public override int Resize(int AWidth, int AHeight)
        {
            int Result = default;
            bool bool__100 = (AWidth != FWidth) || (AHeight != FHeight);
            if (!bool__100)
            {
                bool bool__101 = (FSurface != null);
                if (bool__101)
                {
                    var call__102 = FSurface.IsLost();
                    bool__101 = (call__102 == Aow.Units.GFXE.GFXE.DDERR_SURFACELOST);
                }

                bool__100 = bool__101;
            }

            if (bool__100)
            {
                var call__103 = CreateSurface(AWidth, AHeight, FPixelFormatID, FCreationFlags);
                Result = call__103;
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.DD_OK;
            }

            return Result;
        }

        // GFXE.pas:9414
        public int Restore()
        {
            int Result = default;
            bool bool__104 = (FSurface != null);
            if (bool__104)
            {
                var call__105 = FSurface.IsLost();
                bool__104 = (call__105 == Aow.Units.GFXE.GFXE.DDERR_SURFACELOST);
            }

            if (bool__104)
            {
                var call__106 = FSurface.Restore();
                Result = call__106;
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.DD_OK;
            }

            return Result;
        }

        // GFXE.pas:9422
        public int AsyncCls(int Color)
        {
            int Result = default;
            Aow.Rtl.TRect[] R__cell = new Aow.Rtl.TRect[1];
            ref Aow.Rtl.TRect R = ref R__cell[0];
            Aow.Units.GFXE.TDDBltFX[] Fx__cell = new Aow.Units.GFXE.TDDBltFX[1];
            ref Aow.Units.GFXE.TDDBltFX Fx = ref Fx__cell[0];
            int Count = default;
            int Value = default;
            int I = default;
            R.Top = 0;
            R.Bottom = FHeight;
            R.Left = 0;
            R.Right = FWidth;
            Fx.dwSize = 100;
            var call__107 = Aow.Units.GFXE.GFXE.Color2PixelInfo(Color, FPixelFormatID, null);
            Fx.dwFillColor = call__107;
            while (true)
            {
                var call__108 = FSurface.GetBltStatus(Aow.Units.GFXE.GFXE.DDGBS_CANBLT);
                if (!(call__108 == Aow.Units.GFXE.GFXE.DDERR_WASSTILLDRAWING))
                    break;
                {
                    ;
                }
            }

            var call__109 = FSurface.Blt(Ptr<Aow.Rtl.TRect>.To(R__cell, 0), null, default, Aow.Units.GFXE.GFXE.DDBLT_COLORFILL | Aow.Units.GFXE.GFXE.DDBLT_ASYNC, Ptr<Aow.Units.GFXE.TDDBltFX>.To(Fx__cell, 0));
            Result = call__109;
            if (Result == Aow.Units.GFXE.GFXE.DDERR_UNSUPPORTED)
            {
                if (FLockCount != 0)
                {
                    {
                        ref var w__110 = ref FSurfaceDesc;
                        Count = (int)(((uint)(w__110.Width * w__110.BitCount)) >> 3);
                        switch (w__110.BitCount)
                        {
                            case 8:
                                Value = (((Fx.dwFillColor << 24) + (Fx.dwFillColor << 16)) + (Fx.dwFillColor << 8)) + Fx.dwFillColor;
                                break;
                            case 16:
                                Value = (Fx.dwFillColor << 16) + Fx.dwFillColor;
                                break;
                            case 32:
                                Value = Fx.dwFillColor;
                                break;
                            default:
                                Value = 0;
                                break;
                        }

                        {
                            I = 0;
                            int end__111 = w__110.Height - 1;
                            for (; I <= end__111; I += 1)
                            {
                                Aow.Units.GFXE.GFXE.FillInt((Aow.Rtl.Rtl.BytePointer(w__110.Data) + (w__110.Pitch * I)).AsSpan(Count * 4), Count, Value);
                            }
                        }
                    }
                }

                Result = Aow.Units.GFXE.GFXE.DD_OK;
            }

            if ((Result == Aow.Units.GFXE.GFXE.DDERR_SURFACELOST) && (FOnSurfaceLost != null))
            {
                FOnSurfaceLost(this);
                var call__112 = FSurface.IsLost();
                if (call__112 != Aow.Units.GFXE.GFXE.DDERR_SURFACELOST)
                {
                    Result = Cls(Color);
                }
            }

            return Result;
        }

        // GFXE.pas:9461
        public override int Cls(int Color)
        {
            int Result = default;
            int Pixel = default;
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            int I = default;
            // { Match trials (4): named temporaries, with scopes, Pointer casts, zero-shift scheduling, identity assignments; still differs. }
            var call__113 = Aow.Units.GFXE.GFXE.Color2PixelInfo(Color, this.FPixelFormatID, null);
            Pixel = call__113;
            if (FLockCount != 0)
            {
                {
                    ref var w__114 = ref FSurfaceDesc;
                    Result = Aow.Units.GFXE.GFXE.DD_OK;
                    switch (w__114.BitCount)
                    {
                        case 8:
                        {
                            I = 0;
                            int end__115 = w__114.Height - 1;
                            for (; I <= end__115; I += 1)
                            {
                                Aow.Rtl.Rtl.FillBytes((Aow.Rtl.Rtl.BytePointer(w__114.Data) + (w__114.Pitch * I)).AsSpan(w__114.Width), Pixel);
                            }
                        }

                            break;
                        case 16:
                        {
                            I = 0;
                            int end__116 = w__114.Height - 1;
                            for (; I <= end__116; I += 1)
                            {
                                Aow.Units.GFXE.GFXE.FillWord((Aow.Rtl.Rtl.BytePointer(w__114.Data) + (w__114.Pitch * I)).AsSpan(w__114.Width * 2), w__114.Width, Pixel);
                            }
                        }

                            break;
                        case 24:
                        {
                            I = 0;
                            int end__117 = w__114.Height - 1;
                            for (; I <= end__117; I += 1)
                            {
                                Aow.Units.GFXE.GFXE.FillTrippleByte((Aow.Rtl.Rtl.BytePointer(w__114.Data) + (w__114.Pitch * I)).AsSpan(w__114.Width * 3), w__114.Width, Pixel);
                            }
                        }

                            break;
                        case 32:
                        {
                            I = 0;
                            int end__118 = w__114.Height - 1;
                            for (; I <= end__118; I += 1)
                            {
                                Aow.Units.GFXE.GFXE.FillInt((Aow.Rtl.Rtl.BytePointer(w__114.Data) + (w__114.Pitch * I)).AsSpan(w__114.Width * 4), w__114.Width, Pixel);
                            }
                        }

                            break;
                        default:
                            Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                            break;
                    }
                }
            }
            else
            {
                Result = Lock(ref SD);
                if (Aow.Rtl.Rtl.Succeeded(Result))
                {
                    switch (SD.BitCount)
                    {
                        case 8:
                        {
                            I = 0;
                            int end__119 = SD.Height - 1;
                            for (; I <= end__119; I += 1)
                            {
                                Aow.Rtl.Rtl.FillBytes((Aow.Rtl.Rtl.BytePointer(SD.Data) + (SD.Pitch * I)).AsSpan(SD.Width), Pixel);
                            }
                        }

                            break;
                        case 16:
                        {
                            I = 0;
                            int end__120 = SD.Height - 1;
                            for (; I <= end__120; I += 1)
                            {
                                Aow.Units.GFXE.GFXE.FillWord((Aow.Rtl.Rtl.BytePointer(SD.Data) + (SD.Pitch * I)).AsSpan(SD.Width * 2), SD.Width, Pixel);
                            }
                        }

                            break;
                        case 24:
                        {
                            I = 0;
                            int end__121 = SD.Height - 1;
                            for (; I <= end__121; I += 1)
                            {
                                Aow.Units.GFXE.GFXE.FillTrippleByte((Aow.Rtl.Rtl.BytePointer(SD.Data) + (SD.Pitch * I)).AsSpan(SD.Width * 3), SD.Width, Pixel);
                            }
                        }

                            break;
                        case 32:
                        {
                            I = 0;
                            int end__122 = SD.Height - 1;
                            for (; I <= end__122; I += 1)
                            {
                                Aow.Units.GFXE.GFXE.FillInt((Aow.Rtl.Rtl.BytePointer(SD.Data) + (SD.Pitch * I)).AsSpan(SD.Width * 4), SD.Width, Pixel);
                            }
                        }

                            break;
                        default:
                            Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                            break;
                    }

                    Unlock();
                }
            }

            if ((Result == Aow.Units.GFXE.GFXE.DDERR_SURFACELOST) && (FOnSurfaceLost != null))
            {
                FOnSurfaceLost(this);
                var call__123 = FSurface.IsLost();
                if (call__123 != Aow.Units.GFXE.GFXE.DDERR_SURFACELOST)
                {
                    Result = Cls(Color);
                }
            }

            return Result;
        }

        public Aow.Units.GFXE.IDirectDrawSurface2 Surface { get => FSurface; }
        public TSet<Aow.Units.GFXE.TDDrawSurfaceStatusElement> Status { get => FStatus; }
        public TSet<Aow.Units.GFXE.TDDSCreationFlagsElement> CreationFlags { get => FCreationFlags; }
        public Aow.Rtl.TNotifyEvent OnSurfaceLost { get => FOnSurfaceLost; set => FOnSurfaceLost = value; }

        // System.pas:111
        public static Aow.Units.GFXE.TDDrawSurface Create()
        {
            return new Aow.Units.GFXE.TDDrawSurface();
        }
    }

    // { GFXE.TRectangle (instance size 20): +4 Rect. }
    public partial class TRectangle : Aow.Rtl.TObject
    {
        public Aow.Rtl.TRect Rect;
        // GFXE.pas:2432
        // { TRectangle }
        public void Create__init_TRectangle(int ALeft, int ATop, int ARight, int ABottom)
        {
            Rect.Left = ALeft;
            Rect.Top = ATop;
            Rect.Right = ARight;
            Rect.Bottom = ABottom;
        }

        // GFXE.pas:2440
        public void CreateRect__init_TRectangle(in Aow.Rtl.TRect R)
        {
            Rect = R;
        }

        // GFXE.pas:657
        public static Aow.Units.GFXE.TRectangle Create(int ALeft, int ATop, int ARight, int ABottom)
        {
            Aow.Units.GFXE.TRectangle o = new Aow.Units.GFXE.TRectangle();
            try
            {
                o.Create__init_TRectangle(ALeft, ATop, ARight, ABottom);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        // GFXE.pas:658
        public static Aow.Units.GFXE.TRectangle CreateRect(in Aow.Rtl.TRect R)
        {
            Aow.Units.GFXE.TRectangle o = new Aow.Units.GFXE.TRectangle();
            try
            {
                o.CreateRect__init_TRectangle(R);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { GFXE.TRectangleList (instance size 16): a TList of TRectangle, owning them. }
    public partial class TRectangleList : Aow.Rtl.TList
    {
        // GFXE.pas:2458
        internal Aow.Units.GFXE.TRectangle GetRectangle(int Index)
        {
            Aow.Units.GFXE.TRectangle Result = default;
            Result = (Aow.Units.GFXE.TRectangle)List[Index];
            return Result;
        }

        // GFXE.pas:2447
        // { TRectangleList }
        public override void Destroy()
        {
            int I = default;
            {
                I = Count - 1;
                byte end__124 = 0;
                for (; I >= end__124; I -= 1)
                {
                    var call__125 = this[I];
                    ((Aow.Units.GFXE.TRectangle)call__125).Destroy();
                    Delete(I);
                }
            }
        }

        public sealed class RectanglesAccessor(Aow.Units.GFXE.TRectangleList target)
        {
            public Aow.Units.GFXE.TRectangle this[int Index] { get => target.GetRectangle(Index); }
        }

        public RectanglesAccessor Rectangles => new RectanglesAccessor(this);

        // System.pas:111
        public static Aow.Units.GFXE.TRectangleList Create()
        {
            return new Aow.Units.GFXE.TRectangleList();
        }
    }

    public delegate void TOnDrawRectEvent(Aow.Rtl.TObject Sender, in Aow.Rtl.TRect R);
    // { GFXE.TDrawPanel (instance size 324): a panel that paints through OnDraw. +$138
//     FEraseBkgndEnabled, +$13C FOnDraw. }
    public partial class TDrawPanel : Aow.Rtl.TCustomPanel
    {
        internal bool FEraseBkgndEnabled;
        internal Aow.Units.GFXE.TOnDrawRectEvent FOnDraw;
        // GFXE.pas:8730
        protected internal override void WndProc(ref Aow.Rtl.TMessage Message)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:8741
        protected internal override void Paint()
        {
            /* TODO(T-shim) Platform or editor boundary Controls.ClientRect */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:8721
        // { TDrawPanel }
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            /* TODO(T-shim) Platform or editor boundary ExtCtrls.Create */
            throw new System.NotImplementedException();
        }

        [Published]
        public Aow.Units.GFXE.TOnDrawRectEvent OnDraw { get => FOnDraw; set => FOnDraw = value; }

        [Published]
        public bool EraseBkgndEnabled { get => FEraseBkgndEnabled; set => FEraseBkgndEnabled = value; }

        // GFXE.pas:682
        public static Aow.Units.GFXE.TDrawPanel Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.GFXE.TDrawPanel o = new Aow.Units.GFXE.TDrawPanel();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { GFXE.TImageFile (instance size 4): an image file format, all class functions. }
    public partial class TImageFile : Aow.Rtl.TObject
    {
        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            // GFXE.pas:693
            public virtual Aow.Rtl.AnsiString GetName()
            {
                throw EAbstractError.Create("Abstract Error");
            }

            // GFXE.pas:694
            public virtual Aow.Rtl.AnsiString GetExtension()
            {
                throw EAbstractError.Create("Abstract Error");
            }

            // GFXE.pas:695
            public virtual Aow.Rtl.AnsiString GetDescription()
            {
                throw EAbstractError.Create("Abstract Error");
            }

            // GFXE.pas:696
            public virtual int GetPixelFormatID(Aow.Rtl.AnsiString FileName)
            {
                throw EAbstractError.Create("Abstract Error");
            }

            // GFXE.pas:697
            public virtual Aow.Units.GFXE.TSurface CreateSurface(Aow.Rtl.AnsiString FileName)
            {
                throw EAbstractError.Create("Abstract Error");
            }

            // GFXE.pas:698
            public virtual int LoadToSurfaceDesc(Aow.Rtl.AnsiString FileName, ref Aow.Units.GFXE.TSurfaceDesc SD, int X, int Y)
            {
                throw EAbstractError.Create("Abstract Error");
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GFXE.TImageFile => (Meta)MetaRegistry.Of(typeof(T));
        }

        public new Meta ClassType => (Meta)base.ClassType;
    }

    // { GFXE.TOpenImageFileDialog (instance size 144): a TOpenDialog whose filter lists the
//     registered image file classes. }
    public partial class TOpenImageFileDialog : Aow.Rtl.TOpenDialog
    {
        // GFXE.pas:972
        public override void Loaded()
        {
            /* TODO(T-shim) Platform or editor boundary Dialogs.Filter */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:966
        // { TOpenImageFileDialog }
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            /* TODO(T-shim) Platform or editor boundary Dialogs.Create */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:708
        public static Aow.Units.GFXE.TOpenImageFileDialog Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.GFXE.TOpenImageFileDialog o = new Aow.Units.GFXE.TOpenImageFileDialog();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { GFXE.TDisplayModeItem (instance size 16): +4 PixelFormatID, +8 Width, +$C Height. }
    public partial class TDisplayModeItem : Aow.Rtl.TObject
    {
        public int PixelFormatID;
        public int Width;
        public int Height;
        // System.pas:111
        public static Aow.Units.GFXE.TDisplayModeItem Create()
        {
            return new Aow.Units.GFXE.TDisplayModeItem();
        }
    }

    // { GFXE.TDisplayModeList (instance size 8): +4 FList. }
    public partial class TDisplayModeList : Aow.Rtl.TObject
    {
        internal Aow.Rtl.TList FList;
        // GFXE.pas:12629
        internal Aow.Units.GFXE.TDisplayModeItem GetItems(int Index)
        {
            Aow.Units.GFXE.TDisplayModeItem Result = default;
            Result = (Aow.Units.GFXE.TDisplayModeItem)FList.List[Index];
            return Result;
        }

        // GFXE.pas:12634
        internal int GetCount()
        {
            int Result = default;
            Result = FList.Count;
            return Result;
        }

        // GFXE.pas:12611
        // { TDisplayModeList }
        public void Create__init_TDisplayModeList()
        {
            ;
            var call__126 = new Aow.Rtl.TList();
            FList = call__126;
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
            Aow.Units.GFXE.TDisplayModeItem Item = default;
            Item = Aow.Units.GFXE.TDisplayModeItem.Create();
            Item.Width = AWidth;
            Item.Height = AHeight;
            Item.PixelFormatID = APixelFormatID;
            FList.Add(Item);
        }

        // GFXE.pas:12650
        public void Delete(int Index)
        {
            var call__127 = FList[Index];
            ((Aow.Units.GFXE.TDisplayModeItem)call__127).Free();
            FList.Delete(Index);
        }

        public sealed class ItemsAccessor(Aow.Units.GFXE.TDisplayModeList target)
        {
            public Aow.Units.GFXE.TDisplayModeItem this[int Index] { get => target.GetItems(Index); }
        }

        public ItemsAccessor Items => new ItemsAccessor(this);
        public int Count { get => GetCount(); }

        // GFXE.pas:726
        public static Aow.Units.GFXE.TDisplayModeList Create()
        {
            Aow.Units.GFXE.TDisplayModeList o = new Aow.Units.GFXE.TDisplayModeList();
            try
            {
                o.Create__init_TDisplayModeList();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { GFXE.TBlendTable (instance size 12): +4 FBits, +8 FTable. VMT: Copy, Assign. }
    public partial class TBlendTable : Aow.Rtl.TObject
    {
        internal int FBits;
        internal object FTable;
        // GFXE.pas:741
        public virtual Aow.Units.GFXE.TBlendTable Copy()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // GFXE.pas:742
        public virtual void Assign(Aow.Units.GFXE.TBlendTable Source)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // GFXE.pas:9601
        // { TBlendTable }
        public void Initialize(int ABits, object ATable)
        {
            FTable = ATable;
            FBits = ABits;
        }

        public int Bits { get => FBits; }
        public object Table { get => FTable; }
    }

    // { GFXE.TBlendTable5Bit (instance size 1036): +$C Table (32 x 32 bytes). }
    public partial class TBlendTable5Bit : Aow.Units.GFXE.TBlendTable
    {
        public new Aow.Units.FixedArray_byte_32[] Table = new Aow.Units.FixedArray_byte_32[32];
        // GFXE.pas:9609
        // { TBlendTable5Bit }
        public void Create__init_TBlendTable5Bit()
        {
            ;
            Initialize(5, Ptr<Aow.Units.FixedArray_byte_32>.To(Table, 0).Reinterpret<byte>().Box());
        }

        // GFXE.pas:9615
        public override void Assign(Aow.Units.GFXE.TBlendTable Source)
        {
            if (Source is Aow.Units.GFXE.TBlendTable5Bit)
            {
                /* TODO(T-5.8) Inline array requires a span for this memory operation */
                throw new System.NotImplementedException();
            }
        }

        // GFXE.pas:9621
        public override Aow.Units.GFXE.TBlendTable Copy()
        {
            Aow.Units.GFXE.TBlendTable Result = default;
            Result = Aow.Units.GFXE.TBlendTable5Bit.Create();
            Result.Assign(this);
            return Result;
        }

        // GFXE.pas:752
        public static Aow.Units.GFXE.TBlendTable5Bit Create()
        {
            Aow.Units.GFXE.TBlendTable5Bit o = new Aow.Units.GFXE.TBlendTable5Bit();
            try
            {
                o.Create__init_TBlendTable5Bit();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { GFXE.TBlendTableList (instance size 8): +4 FList. }
    public partial class TBlendTableList : Aow.Rtl.TObject
    {
        internal Aow.Rtl.TList FList;
        // GFXE.pas:9642
        internal int GetCount()
        {
            int Result = default;
            Result = FList.Count;
            return Result;
        }

        // GFXE.pas:9656
        internal Aow.Units.GFXE.TBlendTable GetTables(int Index)
        {
            Aow.Units.GFXE.TBlendTable Result = default;
            Result = (Aow.Units.GFXE.TBlendTable)FList.List[Index];
            return Result;
        }

        // GFXE.pas:9629
        // { TBlendTableList }
        public void Create__init_TBlendTableList()
        {
            ;
            var call__128 = new Aow.Rtl.TList();
            FList = call__128;
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
                ((Aow.Units.GFXE.TBlendTable)FList.List[FList.Count - 1]).Free();
                FList.Delete(FList.Count - 1);
            }
        }

        // GFXE.pas:9661
        public void Add(Aow.Units.GFXE.TBlendTable Table)
        {
            FList.Add(Table);
        }

        // GFXE.pas:9666
        public void Delete(int Index)
        {
            ((Aow.Units.GFXE.TBlendTable)FList.List[Index]).Free();
            FList.Delete(Index);
        }

        public int Count { get => GetCount(); }

        public sealed class TablesAccessor(Aow.Units.GFXE.TBlendTableList target)
        {
            public Aow.Units.GFXE.TBlendTable this[int Index] { get => target.GetTables(Index); }
        }

        public TablesAccessor Tables => new TablesAccessor(this);

        // GFXE.pas:764
        public static Aow.Units.GFXE.TBlendTableList Create()
        {
            Aow.Units.GFXE.TBlendTableList o = new Aow.Units.GFXE.TBlendTableList();
            try
            {
                o.Create__init_TBlendTableList();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public delegate void TGenerateBlendTableProc(Aow.Units.GFXE.TBlendTable Table, float Value);
    // { GFXE.TLookupBlendTableCollection100 (instance size 412): +4 FTables, +8 Lookup (101
//     tables, indexed by percentage). }
    public partial class TLookupBlendTableCollection100 : Aow.Rtl.TObject
    {
        internal Aow.Units.GFXE.TBlendTableList FTables;
        // { ImageLib's blitters read Lookup directly; adding lines here moves the Asserts below }
        public Aow.Units.FixedArray_Aow_Units_GFXE_TBlendTable_101 Lookup;
        // GFXE.pas:9687
        public Aow.Units.GFXE.TBlendTable GetLookupTables(int Index)
        {
            Aow.Units.GFXE.TBlendTable Result = default;
            Result = Lookup[Index];
            return Result;
        }

        // GFXE.pas:9674
        // { TLookupBlendTableCollection100 }
        public void Create__init_TLookupBlendTableCollection100()
        {
            ;
            var call__129 = Aow.Units.GFXE.TBlendTableList.Create();
            FTables = call__129;
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
        public bool GenerateLookupTables(int Bits, Aow.Units.GFXE.TGenerateBlendTableProc Proc, int Count)
        {
            bool Result = default;
            Aow.Units.GFXE.TBlendTable T = default;
            float V = default;
            int I = default;
            int J = default;
            Clear();
            if ((Bits == 5) && (Count > 0))
            {
                {
                    I = 0;
                    int end__130 = Count - 1;
                    for (; I <= end__130; I += 1)
                    {
                        switch (Bits)
                        {
                            case 5:
                                T = Aow.Units.GFXE.TBlendTable5Bit.Create();
                                break;
                            default:
                                T = null;
                                break;
                        }

                        if (Count != 1)
                        {
                            V = (float)(((double)I) / (Count - 1));
                        }
                        else
                        {
                            V = (float)0.5;
                        }

                        Proc(T, V);
                        FTables.Add(T);
                    }
                }

                {
                    I = 0;
                    byte end__131 = 100;
                    for (; I <= end__131; I += 1)
                    {
                        J = (I * Count) / 100;
                        if (J == Count)
                        {
                            J = Count - 1;
                        }

                        var call__132 = FTables.Tables[J];
                        Lookup[I] = call__132;
                    }
                }

                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        public Aow.Units.GFXE.TBlendTableList Tables { get => FTables; }

        public sealed class LookupTablesAccessor(Aow.Units.GFXE.TLookupBlendTableCollection100 target)
        {
            public Aow.Units.GFXE.TBlendTable this[int Index] { get => target.GetLookupTables(Index); }
        }

        public LookupTablesAccessor LookupTables => new LookupTablesAccessor(this);

        // GFXE.pas:783
        public static Aow.Units.GFXE.TLookupBlendTableCollection100 Create()
        {
            Aow.Units.GFXE.TLookupBlendTableCollection100 o = new Aow.Units.GFXE.TLookupBlendTableCollection100();
            try
            {
                o.Create__init_TLookupBlendTableCollection100();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public delegate void TBltRectProc(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height);
    public delegate void TProgressEvent(Aow.Rtl.TObject Sender, int Percent, int Param);
    // { The implementation list of image file classes (TList of TImageFileClass). }
    public partial class TImageFileClassList : Aow.Rtl.TList
    {
        // GFXE.pas:941
        // { TImageFileClassList }
        internal Aow.Units.GFXE.TImageFile.Meta GetClasses(int Index)
        {
            Aow.Units.GFXE.TImageFile.Meta Result = default;
            object P = default;
            var call__231 = this[Index];
            P = call__231;
            Result = (Aow.Units.GFXE.TImageFile.Meta)P;
            return Result;
        }

        // GFXE.pas:949
        public Aow.Units.GFXE.TImageFile.Meta FindClass(Aow.Rtl.AnsiString Ext)
        {
            Aow.Units.GFXE.TImageFile.Meta Result = default;
            int I = default;
            I = 0;
            if ((Ext != ((AnsiString)"")) && (Ext[1] == 46))
            {
                Ext = Aow.Rtl.Rtl.Copy(Ext, 2, Ext.Length - 1);
            }

            while (true)
            {
                bool bool__232 = (I != Count);
                if (bool__232)
                {
                    var call__233 = ((Aow.Units.GFXE.TImageFile.Meta)List[I]).GetExtension();
                    bool__232 = (Aow.Rtl.Rtl.CompareText(call__233, Ext) != 0);
                }

                if (!bool__232)
                    break;
                {
                    I += 1;
                }
            }

            if (I != Count)
            {
                Result = (Aow.Units.GFXE.TImageFile.Meta)List[I];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        public sealed class ClassesAccessor(Aow.Units.GFXE.TImageFileClassList target)
        {
            public Aow.Units.GFXE.TImageFile.Meta this[int Index] { get => target.GetClasses(Index); }
        }

        public ClassesAccessor Classes => new ClassesAccessor(this);

        // System.pas:111
        public static Aow.Units.GFXE.TImageFileClassList Create()
        {
            return new Aow.Units.GFXE.TImageFileClassList();
        }
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

    // { GFXE.TBMPImageFile (instance size 4): the Windows bitmap file format. }
    public partial class TBMPImageFile : Aow.Units.GFXE.TImageFile
    {
        public new partial class Meta : Aow.Units.GFXE.TImageFile.Meta
        {
            // GFXE.pas:9043
            public override Aow.Rtl.AnsiString GetExtension()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"BMP";
                return Result;
            }

            // GFXE.pas:9048
            public override Aow.Rtl.AnsiString GetDescription()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Bitmap files (*.bmp)";
                return Result;
            }

            // GFXE.pas:9053
            public override int GetPixelFormatID(Aow.Rtl.AnsiString FileName)
            {
                int Result = default;
                Result = Aow.Units.GFXE.GFXE.GetBMPPixelFormat(FileName);
                return Result;
            }

            // GFXE.pas:9058
            public override Aow.Units.GFXE.TSurface CreateSurface(Aow.Rtl.AnsiString FileName)
            {
                Aow.Units.GFXE.TSurface Result = default;
                Result = Aow.Units.GFXE.GFXE.CreateSurfaceFromBMP(FileName);
                return Result;
            }

            // GFXE.pas:9063
            public override int LoadToSurfaceDesc(Aow.Rtl.AnsiString FileName, ref Aow.Units.GFXE.TSurfaceDesc SD, int X, int Y)
            {
                int Result = default;
                Result = Aow.Units.GFXE.GFXE.LoadBMP(FileName, ref SD, X, Y);
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GFXE.TBMPImageFile => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public delegate int Procedure_0(Ptr<Aow.Rtl.TGUID> lpGUID, Aow.Units.GFXE.IDirectDraw lplpDD, Aow.Rtl.IUnknown pUnkOuter);
    public delegate int Procedure_1(object lpCallback, object lpContext);
    public delegate int Procedure_2(int dwFlags, Aow.Units.GFXE.IDirectDrawClipper lplpDDClipper, Aow.Rtl.IUnknown pUnkOuter);
    public static partial class GFXE
    {
        // { The GFXE unit of Gfxepack.dpl (the graphics engine of Age of Wonders: surfaces, blitters, blend
//   tables, DirectDraw access) written for the original compiler (Delphi 3.02), class by class:
//   tools/match.py compares the code DCC32 generates for every method with the code in the game's
//   module. Declarations follow the original instance layouts and VMT orders (data/classes.json).
//   The original was D:\GEngine\GFXE\GFXMAIN\GFXE.pas with inc\DIBSURF.PAS and inc\DDSurf.PAS
//   included (the Assert line numbers tell); the DirectDraw interfaces and GUIDs are declared in
//   this unit itself (their type info and typed constants sit in Gfxepack.dpl), without GUIDs on
//   the interface types. The assembler blitters are transcribed from the disassembly: `asm` blocks
//   compile verbatim; the MMX instructions are DB-encoded (Delphi 3 has no MMX mnemonics). }
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
        public static Aow.Rtl.TGUID CLSID_DirectDraw = new Aow.Rtl.TGUID
        {
            D1 = -675868960,
            D2 = 17216,
            D3 = 4559,
            D4 = [176, 99, 0, 32, 175, 194, 205, 53]
        };
        public static Aow.Rtl.TGUID CLSID_DirectDrawClipper = new Aow.Rtl.TGUID
        {
            D1 = 1496848288,
            D2 = 32179,
            D3 = 4559,
            D4 = [162, 222, 0, 170, 0, 185, 51, 86]
        };
        public static Aow.Rtl.TGUID IID_IDirectDraw = new Aow.Rtl.TGUID
        {
            D1 = 1813306240,
            D2 = 42803,
            D3 = 4558,
            D4 = [165, 33, 0, 32, 175, 11, 229, 96]
        };
        public static Aow.Rtl.TGUID IID_IDirectDraw2 = new Aow.Rtl.TGUID
        {
            D1 = -1280904224,
            D2 = 11075,
            D3 = 4559,
            D4 = [162, 222, 0, 170, 0, 185, 51, 86]
        };
        public static Aow.Rtl.TGUID IID_IDirectDrawSurface = new Aow.Rtl.TGUID
        {
            D1 = 1813306241,
            D2 = 42803,
            D3 = 4558,
            D4 = [165, 33, 0, 32, 175, 11, 229, 96]
        };
        public static Aow.Rtl.TGUID IID_IDirectDrawSurface2 = new Aow.Rtl.TGUID
        {
            D1 = 1468029061,
            D2 = 28396,
            D3 = 4559,
            D4 = [148, 65, 168, 35, 3, 193, 14, 39]
        };
        public static Aow.Rtl.TGUID IID_IDirectDrawSurface3 = new Aow.Rtl.TGUID
        {
            D1 = -637252096,
            D2 = 27058,
            D3 = 4560,
            D4 = [161, 213, 0, 170, 0, 184, 223, 187]
        };
        public static Aow.Rtl.TGUID IID_IDirectDrawPalette = new Aow.Rtl.TGUID
        {
            D1 = 1813306244,
            D2 = 42803,
            D3 = 4558,
            D4 = [165, 33, 0, 32, 175, 11, 229, 96]
        };
        public static Aow.Rtl.TGUID IID_IDirectDrawClipper = new Aow.Rtl.TGUID
        {
            D1 = 1813306245,
            D2 = 42803,
            D3 = 4558,
            D4 = [165, 33, 0, 32, 175, 11, 229, 96]
        };
        public static Aow.Rtl.TGUID IID_IDirectDrawColorControl = new Aow.Rtl.TGUID
        {
            D1 = 1268715232,
            D2 = 3454,
            D3 = 4560,
            D4 = [155, 6, 0, 160, 201, 3, 163, 184]
        };
        public static Aow.Units.GFXE.Procedure_0 DirectDrawCreate;
        public static Aow.Units.GFXE.Procedure_1 DirectDrawEnumerate;
        public static Aow.Units.GFXE.Procedure_2 DirectDrawCreateClipper;
        public static Aow.Units.GFXE.TLookupBlendTableCollection100 AlphaBlendTable;
        public static Aow.Units.GFXE.TLookupBlendTableCollection100 IntensityBlendTable;
        public static Aow.Units.GFXE.TLookupBlendTableCollection100 BrightenBlendTable;
        public static Aow.Units.GFXE.TLookupBlendTableCollection100 ShadowBlendTable;
        public static Aow.Units.GFXE.TLookupBlendTableCollection100 LinearAlphaBlendTable;
        // GFXE.pas:978
        public static Aow.Rtl.AnsiString GetImageFilesFilter()
        {
            Aow.Rtl.AnsiString Result = default;
            int I = default;
            Result = (AnsiString)"";
            {
                I = 0;
                int end__133 = Aow.Units.GFXE.GFXE.ImageFileClassList.Count - 1;
                for (; I <= end__133; I += 1)
                {
                    if (Result != ((AnsiString)""))
                    {
                        Result = Result + AnsiString.FromBytes(124);
                    }

                    var call__134 = Aow.Units.GFXE.GFXE.ImageFileClassList.Classes[I];
                    var call__135 = call__134.GetDescription();
                    var call__136 = Aow.Units.GFXE.GFXE.ImageFileClassList.Classes[I];
                    var call__137 = call__136.GetExtension();
                    Result = ((Result + call__135) + ((AnsiString)"|*.")) + call__137;
                }
            }

            return Result;
        }

        // GFXE.pas:991
        public static bool ValidImageFile(Aow.Rtl.AnsiString FileName)
        {
            bool Result = default;
            var call__138 = Aow.Rtl.Rtl.ExtractFileExt(FileName);
            var call__139 = Aow.Units.GFXE.GFXE.ImageFileClassList.FindClass(call__138);
            Result = call__139 != null;
            return Result;
        }

        // GFXE.pas:996
        public static int LoadImageFile(Aow.Rtl.AnsiString FileName, ref Aow.Units.GFXE.TSurfaceDesc SD, int X, int Y)
        {
            int Result = default;
            Aow.Units.GFXE.TImageFile.Meta C = default;
            var call__140 = Aow.Rtl.Rtl.ExtractFileExt(FileName);
            var call__141 = Aow.Units.GFXE.GFXE.ImageFileClassList.FindClass(call__140);
            C = call__141;
            if (C != null)
            {
                Result = C.LoadToSurfaceDesc(FileName, ref SD, X, Y);
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
            }

            return Result;
        }

        // GFXE.pas:1007
        public static Aow.Units.GFXE.TSurface CreateSurfaceFromImageFile(Aow.Rtl.AnsiString FileName)
        {
            Aow.Units.GFXE.TSurface Result = default;
            Aow.Units.GFXE.TImageFile.Meta C = default;
            var call__142 = Aow.Rtl.Rtl.ExtractFileExt(FileName);
            var call__143 = Aow.Units.GFXE.GFXE.ImageFileClassList.FindClass(call__142);
            C = call__143;
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
        public static int GetImageFilePixelFormat(Aow.Rtl.AnsiString FileName)
        {
            int Result = default;
            Aow.Units.GFXE.TImageFile.Meta C = default;
            var call__144 = Aow.Rtl.Rtl.ExtractFileExt(FileName);
            var call__145 = Aow.Units.GFXE.GFXE.ImageFileClassList.FindClass(call__144);
            C = call__145;
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
        public static void RegisterImageFileClasses(ReadOnlySpan<Aow.Units.GFXE.TImageFile.Meta> AClasses)
        {
            int I = default;
            {
                I = 0;
                int end__146 = AClasses.Length - 1;
                for (; I <= end__146; I += 1)
                {
                    var call__147 = AClasses[I].GetExtension();
                    var call__148 = Aow.Units.GFXE.GFXE.ImageFileClassList.FindClass(call__147);
                    if (call__148 == null)
                    {
                        Aow.Units.GFXE.GFXE.ImageFileClassList.Add(AClasses[I]);
                    }
                    else
                    {
                        var call__149 = AClasses[I].GetExtension();
                        var call__150 = Aow.Rtl.Exception.Create((((AnsiString)"ImageFile class with ") + call__149) + ((AnsiString)" extension is already registered"));
                        throw call__150;
                    }
                }
            }
        }

        // GFXE.pas:1656
        public static void GetOptimalGFXESettings(bool ShowDialog)
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Cells */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:1736
        public static void RegisterBltProc(Aow.Units.GFXE.TBltRectProc Proc, Aow.Rtl.AnsiString Name)
        {
            if (Aow.Units.GFXE.GFXE.BltProcCount != Aow.Units.GFXE.GFXE.MaxBltProcs)
            {
                Aow.Units.GFXE.GFXE.BltProcCount += 1;
                Aow.Units.GFXE.GFXE.BltProcs[Aow.Units.GFXE.GFXE.BltProcCount - 1] = Proc;
                Aow.Units.GFXE.GFXE.BltProcNames[Aow.Units.GFXE.GFXE.BltProcCount - 1] = Name;
            }
            else
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"Maximum number of BltProc reached");
            }
        }

        // GFXE.pas:1748
        public static int BltSDRectClipped(in Aow.Units.GFXE.TSurfaceDesc Src, Aow.Rtl.TRect SrcRect, in Aow.Units.GFXE.TSurfaceDesc Dest, int X, int Y)
        {
            int Result = default;
            int W = default;
            int H = default;
            object S = default;
            object D = default;
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
            if ((X + W) > Dest.ClipRect.Right)
            {
                W = W - ((X + W) - Dest.ClipRect.Right);
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
            if ((Y + H) > Dest.ClipRect.Bottom)
            {
                H = H - ((Y + H) - Dest.ClipRect.Bottom);
            }

            if (Src.PixelFormatID == Dest.PixelFormatID)
            {
                if ((W > 0) && (H > 0))
                {
                    W = (int)(((uint)(Src.BitCount * W)) >> 3);
                    S = ((Aow.Rtl.Rtl.BytePointer(Src.Data) + (Src.Pitch * SrcRect.Top)) + ((int)(((uint)(Src.BitCount * SrcRect.Left)) >> 3))).Box();
                    D = ((Aow.Rtl.Rtl.BytePointer(Dest.Data) + (Dest.Pitch * Y)) + ((int)(((uint)(Dest.BitCount * X)) >> 3))).Box();
                    Aow.Units.GFXE.GFXE.BltRect(S, D, Src.Pitch, Dest.Pitch, W, H);
                }

                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_PIXELFORMATNOTSUPPORTED;
            }

            return Result;
        }

        // GFXE.pas:2141
        public static int BltSprite16(in Aow.Units.GFXE.TSurfaceDesc Src, Aow.Rtl.TRect SrcRect, in Aow.Units.GFXE.TSurfaceDesc Dest, int X, int Y, int ColorKey)
        {
            int Result = default;
            int Key = default;
            object S = default;
            object D = default;
            if ((Src.BitCount == 16) && (Dest.PixelFormatID == Src.PixelFormatID))
            {
                var call__151 = Aow.Units.GFXE.GFXE.Color2PixelInfo(ColorKey, Src.PixelFormatID, null);
                Key = call__151;
                S = ((Aow.Rtl.Rtl.BytePointer(Src.Data) + (Src.Pitch * SrcRect.Top)) + (SrcRect.Left * 2)).Box();
                D = ((Aow.Rtl.Rtl.BytePointer(Dest.Data) + (Dest.Pitch * Y)) + (X * 2)).Box();
                Aow.Units.GFXE.GFXE.BltColorKey16(S, D, Src.Pitch, Dest.Pitch, SrcRect.Right - SrcRect.Left, SrcRect.Bottom - SrcRect.Top, Key);
                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_PIXELFORMATNOTSUPPORTED;
            }

            return Result;
        }

        // GFXE.pas:2158
        public static int BltSpriteClipped16(in Aow.Units.GFXE.TSurfaceDesc Src, Aow.Rtl.TRect SrcRect, in Aow.Units.GFXE.TSurfaceDesc Dest, int X, int Y, int ColorKey)
        {
            int Result = default;
            int W = default;
            int H = default;
            object S = default;
            object D = default;
            int Key = default;
            if ((Src.BitCount == 16) && (Dest.BitCount == 16))
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
                if ((W + X) > Dest.ClipRect.Right)
                {
                    W = W - ((W + X) - Dest.ClipRect.Right);
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
                if ((Y + H) > Dest.ClipRect.Bottom)
                {
                    H = H - ((Y + H) - Dest.ClipRect.Bottom);
                }

                if ((W > 0) && (H > 0))
                {
                    var call__152 = Aow.Units.GFXE.GFXE.Color2PixelInfo(ColorKey, Src.PixelFormatID, null);
                    Key = call__152;
                    S = ((Aow.Rtl.Rtl.BytePointer(Src.Data) + (Src.Pitch * SrcRect.Top)) + (SrcRect.Left * 2)).Box();
                    D = (Aow.Rtl.Rtl.BytePointer(Dest.Data) + (Dest.Pitch * Y)).Box();
                    X += X;
                    D = (Aow.Rtl.Rtl.BytePointer(D) + X).Box();
                    Aow.Units.GFXE.GFXE.BltColorKey16(S, D, Src.Pitch, Dest.Pitch, W, H, Key);
                }

                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_PIXELFORMATNOTSUPPORTED;
            }

            return Result;
        }

        // GFXE.pas:2247
        public static int ColorFillSD(Aow.Rtl.TRect R, int Color, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Result = default;
            int W = default;
            int Y = default;
            Ptr<byte> P = default;
            int Pixel = default;
            if (R.Left < SD.ClipRect.Left)
            {
                R.Left = SD.ClipRect.Left;
            }

            if (R.Right > SD.ClipRect.Right)
            {
                R.Right = SD.ClipRect.Right;
            }

            if (R.Top < SD.ClipRect.Top)
            {
                R.Top = SD.ClipRect.Top;
            }

            if (R.Bottom > SD.ClipRect.Bottom)
            {
                R.Bottom = SD.ClipRect.Bottom;
            }

            W = R.Right - R.Left;
            switch (SD.PixelFormatID)
            {
                case Aow.Units.GFXE.GFXE.pfRGB555:
                case Aow.Units.GFXE.GFXE.pfARGB1555:
                case Aow.Units.GFXE.GFXE.pfRGB565:
                    P = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + (SD.Pitch * R.Top)) + (R.Left * 2)).Reinterpret<byte>();
                    var call__153 = Aow.Units.GFXE.GFXE.Color2PixelInfo(Color, SD.PixelFormatID, null);
                    Pixel = call__153;
                {
                    Y = R.Top;
                    int end__154 = R.Bottom - 1;
                    for (; Y <= end__154; Y += 1)
                    {
                        Aow.Units.GFXE.GFXE.FillWord(P.Array.AsSpan(P.Index, W * 2), W, Pixel);
                        P += SD.Pitch;
                    }
                }

                    Result = Aow.Units.GFXE.GFXE.GFXE_OK;
                    break;
                default:
                    Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                    break;
            }

            return Result;
        }

        // GFXE.pas:2326
        public static int GetPixelInfo(in Aow.Units.GFXE.TSurfaceDesc SD, int X, int Y)
        {
            int Result = default;
            switch (SD.BitCount)
            {
                case 8:
                    Result = MemoryMarshal.Read<byte>(((Aow.Rtl.Rtl.BytePointer(SD.Data) + X) + (SD.Pitch * Y)).AsSpan(1));
                    break;
                case 16:
                    Result = MemoryMarshal.Read<ushort>((((Aow.Rtl.Rtl.BytePointer(SD.Data) + X) + X) + (SD.Pitch * Y)).AsSpan(2));
                    break;
                case 24:
                    Result = MemoryMarshal.Read<int>(((Aow.Rtl.Rtl.BytePointer(SD.Data) + (X * 3)) + (SD.Pitch * Y)).AsSpan(4)) & 16777215;
                    break;
                case 32:
                    Result = MemoryMarshal.Read<int>(((Aow.Rtl.Rtl.BytePointer(SD.Data) + (X * 4)) + (SD.Pitch * Y)).AsSpan(4));
                    break;
                default:
                    throw Aow.Rtl.Exception.Create((AnsiString)"PixelFormat is not supported");
                    break;
            }

            return Result;
        }

        // GFXE.pas:2340
        // { The pixel format ID: bits 0-7 the bit count, bits 8-15 the position (0-3) of each channel
//   (A, B, G, R) counted from the low end, bits 16-31 the bits per channel (A, B, G, R). }
        public static int MASKtoPFID(int BitCount, int RMask, int GMask, int BMask, int AMask)
        {
            int Result = default;
            Aow.Units.FixedArray_byte_4 Order = default;
            bool Swapped = default;
            Aow.Units.FixedArray_int_4 Masks = default;
            int I = default;
            int Bits = default;
            int T = default;
            Result = BitCount;
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
                {
                    I = 0;
                    byte end__155 = 2;
                    for (; I <= end__155; I += 1)
                    {
                        if ((((Masks[Order[I]] < 0) || (Masks[Order[I + 1]] < Masks[Order[I]])) && (Masks[Order[I + 1]] > 0)) || ((Masks[Order[I]] == 0) && (Masks[Order[I + 1]] != 0)))
                        {
                            T = Order[I];
                            Order[I] = unchecked((byte)Order[I + 1]);
                            Order[I + 1] = unchecked((byte)T);
                            Swapped = true;
                        }
                    }
                }
            }
            while (!!Swapped);
            {
                I = 0;
                byte end__156 = 3;
                for (; I <= end__156; I += 1)
                {
                    Result = Result + (I << ((Order[I] * 2) + 8));
                }
            }

            {
                I = 0;
                byte end__157 = 3;
                for (; I <= end__157; I += 1)
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

                    Result = Result + (Bits << ((I * 4) + 16));
                }
            }

            return Result;
        }

        // GFXE.pas:2382
        public static int PFIDtoMASK(int PixelFormatID, ref int RMask, ref int GMask, ref int BMask, ref int AMask)
        {
            int Result = default;
            int BitCount = default;
            Aow.Units.FixedArray_byte_4 Order = default;
            int Pos = default;
            Aow.Units.FixedArray_int_4 Masks = default;
            int I = default;
            int J = default;
            int Bits = default;
            int M = default;
            Order = default;
            Masks = default;
            BitCount = PixelFormatID & 255;
            Pos = 0;
            Order[((int)(((uint)PixelFormatID) >> 8)) & 3] = 0;
            Order[((int)(((uint)PixelFormatID) >> 10)) & 3] = 1;
            Order[((int)(((uint)PixelFormatID) >> 12)) & 3] = 2;
            Order[((int)(((uint)PixelFormatID) >> 14)) & 3] = 3;
            {
                I = 0;
                byte end__158 = 3;
                for (; I <= end__158; I += 1)
                {
                    Bits = ((int)(((uint)PixelFormatID) >> ((Order[I] * 4) + 16))) & 15;
                    J = Bits;
                    M = 0;
                    while (J != 0)
                    {
                        M = (M * 2) + 1;
                        J -= 1;
                    }

                    M = M << Pos;
                    Pos = Pos + Bits;
                    Masks[Order[I]] = M;
                }
            }

            AMask = Masks[0];
            BMask = Masks[1];
            GMask = Masks[2];
            RMask = Masks[3];
            Result = BitCount;
            return Result;
        }

        // GFXE.pas:2419
        public static void CreateSDfromSD(in Aow.Units.GFXE.TSurfaceDesc Src, in Aow.Rtl.TRect R, ref Aow.Units.GFXE.TSurfaceDesc Dest)
        {
            Dest = Src;
            Aow.Rtl.Rtl.IntersectRect(ref Dest.ClipRect, Src.ClipRect, R);
            Dest.Data = ((Aow.Rtl.Rtl.BytePointer(Src.Data) + (R.Top * Src.Pitch)) + (((int)(((uint)Src.BitCount) >> 3)) * R.Left)).Box();
            Dest.ClipRect.Left -= R.Left;
            Dest.ClipRect.Right -= R.Left;
            Dest.ClipRect.Top -= R.Top;
            Dest.ClipRect.Bottom -= R.Top;
        }

        // GFXE.pas:2465
        // { The rectangles of a surface: the maximal rectangles of TransparentColor bordered by other
//   colors, found from their top left corners. }
        public static Aow.Units.GFXE.TRectangleList GetSurfaceRectangles(Aow.Units.GFXE.TSurfaceDesc SD, int TransparentColor, Aow.Units.GFXE.TProgressEvent OnProgress)
        {
            Aow.Units.GFXE.TRectangleList Result = default;
            Aow.Units.GFXE.TRectangleList List = default;
            Aow.Units.GFXE.TRectangle R = default;
            ushort X = default;
            ushort Y = default;
            int GetPixel(int X, int Y)
            {
                int Result = default;
                object P = default;
                switch (SD.BitCount)
                {
                    case 8:
                        Result = MemoryMarshal.Read<byte>(((Aow.Rtl.Rtl.BytePointer(SD.Data) + X) + (SD.Pitch * Y)).AsSpan(1));
                        break;
                    case 16:
                        Result = MemoryMarshal.Read<ushort>((((Aow.Rtl.Rtl.BytePointer(SD.Data) + X) + X) + (SD.Pitch * Y)).AsSpan(2));
                        break;
                    case 24:
                        if ((X + 1) < SD.Width)
                        {
                            Result = MemoryMarshal.Read<int>(((Aow.Rtl.Rtl.BytePointer(SD.Data) + ((X + X) + X)) + (SD.Pitch * Y)).AsSpan(4)) & 16777215;
                        }
                        else
                        {
                            P = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + ((X + X) + X)) + (SD.Pitch * Y)).Box();
                            Result = MemoryMarshal.Read<ushort>(Aow.Rtl.Rtl.BytePointer(P).AsSpan(2)) + (MemoryMarshal.Read<byte>((Aow.Rtl.Rtl.BytePointer(P) + 2).AsSpan(1)) << 16);
                        }

                        break;
                    case 32:
                        Result = MemoryMarshal.Read<int>(((Aow.Rtl.Rtl.BytePointer(SD.Data) + (X << 2)) + (SD.Pitch * Y)).AsSpan(4));
                        break;
                    default:
                        throw Aow.Rtl.Exception.Create((AnsiString)"PixelFormat is not supported");
                        break;
                }

                return Result;
            }

            Aow.Units.GFXE.TRectangle FindRectangle(int X, int Y)
            {
                Aow.Units.GFXE.TRectangle Result = default;
                int W = default;
                int H = default;
                int I = default;
                bool Ok = default;
                Aow.Units.GFXE.TRectangle R = default;
                R = null;
                W = 1;
                H = 1;
                while (true)
                {
                    var call__159 = GetPixel(X + W, Y);
                    bool bool__160 = (call__159 == TransparentColor);
                    if (bool__160)
                    {
                        var call__161 = GetPixel(X + W, Y + 1);
                        bool__160 = (call__161 != TransparentColor);
                    }

                    if (!(bool__160 && ((X + W) < (SD.Width - 1))))
                        break;
                    {
                        W += 1;
                    }
                }

                var call__162 = GetPixel(X + W, Y);
                bool bool__163 = (call__162 == TransparentColor);
                if (bool__163)
                {
                    var call__164 = GetPixel(X + W, Y + 1);
                    bool__163 = (call__164 == TransparentColor);
                }

                if ((bool__163 && ((X + W) < (SD.Width - 1))) && (W > 1))
                {
                    while (true)
                    {
                        var call__165 = GetPixel(X, Y + H);
                        bool bool__166 = (call__165 == TransparentColor);
                        if (bool__166)
                        {
                            var call__167 = GetPixel(X + W, Y + H);
                            bool__166 = (call__167 == TransparentColor);
                        }

                        bool bool__168 = bool__166;
                        if (bool__168)
                        {
                            var call__169 = GetPixel(X + 1, Y + H);
                            bool__168 = (call__169 != TransparentColor);
                        }

                        bool bool__170 = bool__168;
                        if (bool__170)
                        {
                            var call__171 = GetPixel((X + W) - 1, Y + H);
                            bool__170 = (call__171 != TransparentColor);
                        }

                        if (!(bool__170 && ((Y + H) < SD.Height)))
                            break;
                        {
                            H += 1;
                        }
                    }

                    var call__172 = GetPixel(X, Y + H);
                    bool bool__173 = (call__172 == TransparentColor);
                    if (bool__173)
                    {
                        var call__174 = GetPixel(X + 1, Y + H);
                        bool__173 = (call__174 == TransparentColor);
                    }

                    bool bool__175 = bool__173;
                    if (bool__175)
                    {
                        var call__176 = GetPixel(X + W, Y + H);
                        bool__175 = (call__176 == TransparentColor);
                    }

                    bool bool__177 = bool__175;
                    if (bool__177)
                    {
                        var call__178 = GetPixel((X + W) - 1, Y + H);
                        bool__177 = (call__178 == TransparentColor);
                    }

                    if ((bool__177 && ((Y + H) < SD.Height)) && (H > 1))
                    {
                        Ok = true;
                        I = 1;
                        while (Ok && (I != W))
                        {
                            Ok = GetPixel(X + I, Y + H) == TransparentColor;
                            I += 1;
                        }

                        if (Ok)
                        {
                            R = Aow.Units.GFXE.TRectangle.Create(X + 1, Y + 1, X + W, Y + H);
                        }
                    }
                }

                Result = R;
                return Result;
            }

            List = Aow.Units.GFXE.TRectangleList.Create();
            {
                int first__184 = (int)0;
                int end__179 = (int)(SD.Height - 1);
                for (int ordinal__183 = first__184; ordinal__183 <= end__179; ordinal__183++)
                {
                    Y = (ushort)ordinal__183;
                    {
                        int first__182 = (int)0;
                        int end__180 = (int)(SD.Width - 1);
                        for (int ordinal__181 = first__182; ordinal__181 <= end__180; ordinal__181++)
                        {
                            X = (ushort)ordinal__181;
                            if (GetPixel(X, Y) == TransparentColor)
                            {
                                R = FindRectangle(X, Y);
                                if (R != null)
                                {
                                    List.Add(R);
                                }
                            }
                        }
                    }

                    if ((OnProgress != null) && (((Y % 20) == 0) || (Y == (SD.Height - 1))))
                    {
                        OnProgress(null, Aow.Rtl.Rtl.Round(((double)(Y * 100)) / (SD.Height - 1)), 0);
                    }
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
        public static void MinimizeTransparentRect(in Aow.Units.GFXE.TSurfaceDesc SD, in Aow.Rtl.TRect R, int TransparentColor, ref Aow.Rtl.TRect Result)
        {
            Aow.Units.GFXE.TSurfaceDesc SD__value = SD;
            bool Found = default;
            int X = default;
            int Y = default;
            Aow.Rtl.TRect Rect = default;
            int GetPixel(int X, int Y)
            {
                int Result = default;
                switch (SD__value.BitCount)
                {
                    case 8:
                        Result = MemoryMarshal.Read<byte>(((Aow.Rtl.Rtl.BytePointer(SD__value.Data) + X) + (SD__value.Pitch * Y)).AsSpan(1));
                        break;
                    case 16:
                        Result = MemoryMarshal.Read<ushort>((((Aow.Rtl.Rtl.BytePointer(SD__value.Data) + X) + X) + (SD__value.Pitch * Y)).AsSpan(2));
                        break;
                    case 24:
                        Result = MemoryMarshal.Read<int>(((Aow.Rtl.Rtl.BytePointer(SD__value.Data) + (X * 3)) + (SD__value.Pitch * Y)).AsSpan(4)) & 16777215;
                        break;
                    case 32:
                        Result = MemoryMarshal.Read<int>(((Aow.Rtl.Rtl.BytePointer(SD__value.Data) + (X * 4)) + (SD__value.Pitch * Y)).AsSpan(4));
                        break;
                    default:
                        throw Aow.Rtl.Exception.Create((AnsiString)"PixelFormat is not supported");
                        break;
                }

                return Result;
            }

            Rect = R;
            Found = false;
            while (!Found && (Rect.Top < Rect.Bottom))
            {
                X = Rect.Left;
                while (!Found && (X != Rect.Right))
                {
                    var call__185 = GetPixel(X, Rect.Top);
                    Found = call__185 != TransparentColor;
                    X += 1;
                }

                if (!Found)
                {
                    Rect.Top += 1;
                }
            }

            Found = false;
            while (!Found && (Rect.Bottom > Rect.Top))
            {
                X = Rect.Left;
                while (!Found && (X != Rect.Right))
                {
                    var call__186 = GetPixel(X, Rect.Bottom - 1);
                    Found = call__186 != TransparentColor;
                    X += 1;
                }

                if (!Found)
                {
                    Rect.Bottom -= 1;
                }
            }

            Found = false;
            while (!Found && (Rect.Left < Rect.Right))
            {
                Y = Rect.Top;
                while (!Found && (Y != Rect.Bottom))
                {
                    var call__187 = GetPixel(Rect.Left, Y);
                    Found = call__187 != TransparentColor;
                    Y += 1;
                }

                if (!Found)
                {
                    Rect.Left += 1;
                }
            }

            Found = false;
            while (!Found && (Rect.Right > Rect.Left))
            {
                Y = Rect.Top;
                while (!Found && (Y != Rect.Bottom))
                {
                    var call__188 = GetPixel(Rect.Right - 1, Y);
                    Found = call__188 != TransparentColor;
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
        public static void MinimizeInvTransparentRect(in Aow.Units.GFXE.TSurfaceDesc SD, in Aow.Rtl.TRect R, int TransparentColor, ref Aow.Rtl.TRect Result)
        {
            Aow.Units.GFXE.TSurfaceDesc SD__value = SD;
            bool Found = default;
            int X = default;
            int Y = default;
            Aow.Rtl.TRect Rect = default;
            int GetPixel(int X, int Y)
            {
                int Result = default;
                switch (SD__value.BitCount)
                {
                    case 8:
                        Result = MemoryMarshal.Read<byte>(((Aow.Rtl.Rtl.BytePointer(SD__value.Data) + X) + (SD__value.Pitch * Y)).AsSpan(1));
                        break;
                    case 16:
                        Result = MemoryMarshal.Read<ushort>((((Aow.Rtl.Rtl.BytePointer(SD__value.Data) + X) + X) + (SD__value.Pitch * Y)).AsSpan(2));
                        break;
                    case 24:
                        Result = MemoryMarshal.Read<int>(((Aow.Rtl.Rtl.BytePointer(SD__value.Data) + (X * 3)) + (SD__value.Pitch * Y)).AsSpan(4)) & 16777215;
                        break;
                    case 32:
                        Result = MemoryMarshal.Read<int>(((Aow.Rtl.Rtl.BytePointer(SD__value.Data) + (X * 4)) + (SD__value.Pitch * Y)).AsSpan(4));
                        break;
                    default:
                        throw Aow.Rtl.Exception.Create((AnsiString)"PixelFormat is not supported");
                        break;
                }

                return Result;
            }

            Rect = R;
            Found = false;
            while (!Found && (Rect.Top < Rect.Bottom))
            {
                X = Rect.Left;
                while (!Found && (X != Rect.Right))
                {
                    var call__189 = GetPixel(X, Rect.Top);
                    Found = call__189 == TransparentColor;
                    X += 1;
                }

                if (!Found)
                {
                    Rect.Top += 1;
                }
            }

            Found = false;
            while (!Found && (Rect.Bottom > Rect.Top))
            {
                X = Rect.Left;
                while (!Found && (X != Rect.Right))
                {
                    var call__190 = GetPixel(X, Rect.Bottom - 1);
                    Found = call__190 == TransparentColor;
                    X += 1;
                }

                if (!Found)
                {
                    Rect.Bottom -= 1;
                }
            }

            Found = false;
            while (!Found && (Rect.Left < Rect.Right))
            {
                Y = Rect.Top;
                while (!Found && (Y != Rect.Bottom))
                {
                    var call__191 = GetPixel(Rect.Left, Y);
                    Found = call__191 == TransparentColor;
                    Y += 1;
                }

                if (!Found)
                {
                    Rect.Left += 1;
                }
            }

            Found = false;
            while (!Found && (Rect.Right > Rect.Left))
            {
                Y = Rect.Top;
                while (!Found && (Y != Rect.Bottom))
                {
                    var call__192 = GetPixel(Rect.Right - 1, Y);
                    Found = call__192 == TransparentColor;
                    Y += 1;
                }

                if (!Found)
                {
                    Rect.Right -= 1;
                }
            }

            Result = Rect;
        }

        // GFXE.pas:2683
        // { Bresenham line on a 16 bit surface, every Skip-th pixel left out; the clip test is only made
//   when an end point lies outside the clip rectangle. }
        public static void DrawLine16(int X1, int Y1, int X2, int Y2, int Skip, ushort Color, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int X = default;
            int Y = default;
            int DX = default;
            int DY = default;
            int D2 = default;
            int DX2 = default;
            int DY2 = default;
            int E = default;
            Ptr<ushort> P = default;
            int Count = default;
            Count = 0;
            if (X2 < X1)
            {
                X = X2;
                X2 = X1;
                X1 = X;
                Y = Y2;
                Y2 = Y1;
                Y1 = Y;
            }
            else
            {
                X = X1;
                Y = Y1;
            }

            /* TODO(T-4.3) Address needs a cell or array backing */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:2958
        // { Skip is not passed on: DrawLine16 always draws every pixel (original). }
        public static void DrawLine(int X1, int Y1, int X2, int Y2, int Skip, int Color, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Pixel = default;
            switch (SD.PixelFormatID)
            {
                case Aow.Units.GFXE.GFXE.pfRGB555:
                case Aow.Units.GFXE.GFXE.pfARGB1555:
                case Aow.Units.GFXE.GFXE.pfRGB565:
                    var call__193 = Aow.Units.GFXE.GFXE.Color2PixelInfo(Color, SD.PixelFormatID, null);
                    Pixel = call__193;
                    Aow.Units.GFXE.GFXE.DrawLine16(X1, Y1, X2, Y2, 0, unchecked((ushort)Pixel), SD);
                    break;
            }
        }

        // GFXE.pas:8325
        public static void RemapSurface24to565(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height)
        {
            int Y = default;
            Ptr<byte> S = default;
            Ptr<ushort> D = default;
            int X = default;
            {
                Y = 0;
                int end__194 = Height - 1;
                for (; Y <= end__194; Y += 1)
                {
                    S = Ptr<byte>.Unbox((Aow.Rtl.Rtl.BytePointer(Src) + (SrcPitch * Y)).Box());
                    D = Ptr<ushort>.Unbox((Aow.Rtl.Rtl.BytePointer(Dst) + (DstPitch * Y)).Box());
                    {
                        X = 0;
                        int end__195 = Width - 1;
                        for (; X <= end__195; X += 1)
                        {
                            D.Value = unchecked((ushort)((((S[2] & 248) << 8) + ((S[1] & 252) << 3)) + ((int)(((uint)S[0]) >> 3))));
                            S = Ptr<byte>.Unbox((S.Reinterpret<byte>() + 3).Box());
                            D = Ptr<ushort>.Unbox((D.Reinterpret<byte>() + 2).Box());
                        }
                    }
                }
            }
        }

        // GFXE.pas:8345
        public static void RemapSurface24to555(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height)
        {
            int Y = default;
            Ptr<byte> S = default;
            Ptr<ushort> D = default;
            int X = default;
            {
                Y = 0;
                int end__196 = Height - 1;
                for (; Y <= end__196; Y += 1)
                {
                    S = Ptr<byte>.Unbox((Aow.Rtl.Rtl.BytePointer(Src) + (SrcPitch * Y)).Box());
                    D = Ptr<ushort>.Unbox((Aow.Rtl.Rtl.BytePointer(Dst) + (DstPitch * Y)).Box());
                    {
                        X = 0;
                        int end__197 = Width - 1;
                        for (; X <= end__197; X += 1)
                        {
                            D.Value = unchecked((ushort)((((S[2] & 248) << 7) + ((S[1] & 248) << 2)) + ((int)(((uint)S[0]) >> 3))));
                            S = Ptr<byte>.Unbox((S.Reinterpret<byte>() + 3).Box());
                            D = Ptr<ushort>.Unbox((D.Reinterpret<byte>() + 2).Box());
                        }
                    }
                }
            }
        }

        // GFXE.pas:8365
        public static void RemapSurface555to24(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height)
        {
            int X = default;
            int Y = default;
            Ptr<byte> D = default;
            Ptr<ushort> S = default;
            {
                Y = 0;
                int end__198 = Height - 1;
                for (; Y <= end__198; Y += 1)
                {
                    S = Ptr<ushort>.Unbox((Aow.Rtl.Rtl.BytePointer(Src) + (SrcPitch * Y)).Box());
                    D = Ptr<byte>.Unbox((Aow.Rtl.Rtl.BytePointer(Dst) + (DstPitch * Y)).Box());
                    {
                        X = 0;
                        int end__199 = Width - 1;
                        for (; X <= end__199; X += 1)
                        {
                            D[2] = unchecked((byte)((int)(((uint)(S.Value & 31744)) >> 7)));
                            D[1] = unchecked((byte)((int)(((uint)(S.Value & 992)) >> 2)));
                            D[0] = unchecked((byte)((S.Value & 31) << 3));
                            D = Ptr<byte>.Unbox((D.Reinterpret<byte>() + 3).Box());
                            S = Ptr<ushort>.Unbox((S.Reinterpret<byte>() + 2).Box());
                        }
                    }
                }
            }
        }

        // GFXE.pas:8386
        public static void RemapSurface565to24(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height)
        {
            int X = default;
            int Y = default;
            Ptr<byte> D = default;
            Ptr<ushort> S = default;
            {
                Y = 0;
                int end__200 = Height - 1;
                for (; Y <= end__200; Y += 1)
                {
                    S = Ptr<ushort>.Unbox((Aow.Rtl.Rtl.BytePointer(Src) + (SrcPitch * Y)).Box());
                    D = Ptr<byte>.Unbox((Aow.Rtl.Rtl.BytePointer(Dst) + (DstPitch * Y)).Box());
                    {
                        X = 0;
                        int end__201 = Width - 1;
                        for (; X <= end__201; X += 1)
                        {
                            D[2] = unchecked((byte)((int)(((uint)(S.Value & 63488)) >> 8)));
                            D[1] = unchecked((byte)((int)(((uint)(S.Value & 2016)) >> 3)));
                            D[0] = unchecked((byte)((S.Value & 31) << 3));
                            D = Ptr<byte>.Unbox((D.Reinterpret<byte>() + 3).Box());
                            S = Ptr<ushort>.Unbox((S.Reinterpret<byte>() + 2).Box());
                        }
                    }
                }
            }
        }

        // GFXE.pas:8408
        // { Both 32 bit converters produce 555 pixels from bytes 1-3 of the source (original). }
        public static void RemapSurface32to565(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height)
        {
            int Y = default;
            Ptr<byte> S = default;
            Ptr<ushort> D = default;
            int X = default;
            {
                Y = 0;
                int end__202 = Height - 1;
                for (; Y <= end__202; Y += 1)
                {
                    S = Ptr<byte>.Unbox((Aow.Rtl.Rtl.BytePointer(Src) + (SrcPitch * Y)).Box());
                    D = Ptr<ushort>.Unbox((Aow.Rtl.Rtl.BytePointer(Dst) + (DstPitch * Y)).Box());
                    {
                        X = 0;
                        int end__203 = Width - 1;
                        for (; X <= end__203; X += 1)
                        {
                            D.Value = unchecked((ushort)((((S[1] & 248) << 7) + ((S[2] & 248) << 2)) + ((int)(((uint)S[3]) >> 3))));
                            S = Ptr<byte>.Unbox((S.Reinterpret<byte>() + 4).Box());
                            D = Ptr<ushort>.Unbox((D.Reinterpret<byte>() + 2).Box());
                        }
                    }
                }
            }
        }

        // GFXE.pas:8428
        public static void RemapSurface32to555(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height)
        {
            int Y = default;
            Ptr<byte> S = default;
            Ptr<ushort> D = default;
            int X = default;
            {
                Y = 0;
                int end__204 = Height - 1;
                for (; Y <= end__204; Y += 1)
                {
                    S = Ptr<byte>.Unbox((Aow.Rtl.Rtl.BytePointer(Src) + (SrcPitch * Y)).Box());
                    D = Ptr<ushort>.Unbox((Aow.Rtl.Rtl.BytePointer(Dst) + (DstPitch * Y)).Box());
                    {
                        X = 0;
                        int end__205 = Width - 1;
                        for (; X <= end__205; X += 1)
                        {
                            D.Value = unchecked((ushort)((((S[1] & 248) << 7) + ((S[2] & 248) << 2)) + ((int)(((uint)S[3]) >> 3))));
                            S = Ptr<byte>.Unbox((S.Reinterpret<byte>() + 4).Box());
                            D = Ptr<ushort>.Unbox((D.Reinterpret<byte>() + 2).Box());
                        }
                    }
                }
            }
        }

        // GFXE.pas:8448
        public static void RemapSurface08to565(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height, Aow.Units.GFXE.TGFXEPalette Palette)
        {
            int X = default;
            int Y = default;
            Ptr<byte> S = default;
            Ptr<ushort> D = default;
            Ptr<Aow.Units.GFXE.TPaletteEntry> E = default;
            {
                Y = 0;
                int end__206 = Height - 1;
                for (; Y <= end__206; Y += 1)
                {
                    S = Ptr<byte>.Unbox((Aow.Rtl.Rtl.BytePointer(Src) + (SrcPitch * Y)).Box());
                    D = Ptr<ushort>.Unbox((Aow.Rtl.Rtl.BytePointer(Dst) + (DstPitch * Y)).Box());
                    {
                        X = 0;
                        int end__207 = Width - 1;
                        for (; X <= end__207; X += 1)
                        {
                            E = Ptr<Aow.Units.GFXE.TPaletteEntry>.To(Palette.Entries, S.Value);
                            D.Value = unchecked((ushort)((((E.Value.R & 248) << 8) + ((E.Value.G & 252) << 3)) + ((int)(((uint)E.Value.B) >> 3))));
                            S = Ptr<byte>.Unbox((S.Reinterpret<byte>() + 1).Box());
                            D = Ptr<ushort>.Unbox((D.Reinterpret<byte>() + 2).Box());
                        }
                    }
                }
            }
        }

        // GFXE.pas:8469
        public static void RemapSurface08to555(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height, Aow.Units.GFXE.TGFXEPalette Palette)
        {
            int X = default;
            int Y = default;
            Ptr<byte> S = default;
            Ptr<ushort> D = default;
            Ptr<Aow.Units.GFXE.TPaletteEntry> E = default;
            {
                Y = 0;
                int end__208 = Height - 1;
                for (; Y <= end__208; Y += 1)
                {
                    S = Ptr<byte>.Unbox((Aow.Rtl.Rtl.BytePointer(Src) + (SrcPitch * Y)).Box());
                    D = Ptr<ushort>.Unbox((Aow.Rtl.Rtl.BytePointer(Dst) + (DstPitch * Y)).Box());
                    {
                        X = 0;
                        int end__209 = Width - 1;
                        for (; X <= end__209; X += 1)
                        {
                            E = Ptr<Aow.Units.GFXE.TPaletteEntry>.To(Palette.Entries, S.Value);
                            D.Value = unchecked((ushort)((((E.Value.R & 248) << 7) + ((E.Value.G & 248) << 2)) + ((int)(((uint)E.Value.B) >> 3))));
                            S = Ptr<byte>.Unbox((S.Reinterpret<byte>() + 1).Box());
                            D = Ptr<ushort>.Unbox((D.Reinterpret<byte>() + 2).Box());
                        }
                    }
                }
            }
        }

        // GFXE.pas:8490
        public static void RemapSurface08to24(object Src, object Dst, int SrcPitch, int DstPitch, int Width, int Height, Aow.Units.GFXE.TGFXEPalette Palette)
        {
            int X = default;
            int Y = default;
            Ptr<byte> S = default;
            Ptr<byte> D = default;
            Ptr<Aow.Units.GFXE.TPaletteEntry> E = default;
            {
                Y = 0;
                int end__210 = Height - 1;
                for (; Y <= end__210; Y += 1)
                {
                    S = Ptr<byte>.Unbox((Aow.Rtl.Rtl.BytePointer(Src) + (SrcPitch * Y)).Box());
                    D = Ptr<byte>.Unbox((Aow.Rtl.Rtl.BytePointer(Dst) + (DstPitch * Y)).Box());
                    {
                        X = 0;
                        int end__211 = Width - 1;
                        for (; X <= end__211; X += 1)
                        {
                            E = Ptr<Aow.Units.GFXE.TPaletteEntry>.To(Palette.Entries, S.Value);
                            D[2] = unchecked((byte)E.Value.R);
                            D[1] = unchecked((byte)E.Value.G);
                            D[0] = unchecked((byte)E.Value.B);
                            S = Ptr<byte>.Unbox((S.Reinterpret<byte>() + 1).Box());
                            D = Ptr<byte>.Unbox((D.Reinterpret<byte>() + 3).Box());
                        }
                    }
                }
            }
        }

        // GFXE.pas:8513
        public static int RemapSurfacePart(in Aow.Units.GFXE.TSurfaceDesc Src, int SrcX, int SrcY, int Width, int Height, in Aow.Units.GFXE.TSurfaceDesc Dest, int DestX, int DestY)
        {
            int Result = default;
            object S = default;
            object D = default;
            Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            if ((SrcX + Width) > Src.Width)
            {
                Width = Src.Width - SrcX;
            }

            if ((SrcY + Height) > Src.Height)
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

            if ((DestX + Width) > Dest.Width)
            {
                Width = Dest.Width - DestX;
            }

            if ((DestY + Height) > Dest.Height)
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

            if ((Width > 0) && (Height > 0))
            {
                S = ((Aow.Rtl.Rtl.BytePointer(Src.Data) + (Src.Pitch * SrcY)) + (((int)(((uint)(Src.BitCount + 7)) >> 3)) * SrcX)).Box();
                D = ((Aow.Rtl.Rtl.BytePointer(Dest.Data) + (Dest.Pitch * DestY)) + (((int)(((uint)(Dest.BitCount + 7)) >> 3)) * DestX)).Box();
                switch (Src.PixelFormatID)
                {
                    case Aow.Units.GFXE.GFXE.pfPAL8:
                        switch (Dest.PixelFormatID)
                        {
                            case Aow.Units.GFXE.GFXE.pfRGB565:
                                Aow.Units.GFXE.GFXE.RemapSurface08to565(S, D, Src.Pitch, Dest.Pitch, Width, Height, Src.Palette);
                                break;
                            case Aow.Units.GFXE.GFXE.pfRGB555:
                                Aow.Units.GFXE.GFXE.RemapSurface08to555(S, D, Src.Pitch, Dest.Pitch, Width, Height, Src.Palette);
                                break;
                            case Aow.Units.GFXE.GFXE.pfRGB24:
                                Aow.Units.GFXE.GFXE.RemapSurface08to24(S, D, Src.Pitch, Dest.Pitch, Width, Height, Src.Palette);
                                break;
                            case Aow.Units.GFXE.GFXE.pfPAL8:
                                Aow.Units.GFXE.GFXE.BltRect(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                                break;
                            default:
                                Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                                break;
                        }

                        break;
                    case Aow.Units.GFXE.GFXE.pfRGB555:
                    case Aow.Units.GFXE.GFXE.pfARGB1555:
                        switch (Dest.PixelFormatID)
                        {
                            case Aow.Units.GFXE.GFXE.pfRGB24:
                                Aow.Units.GFXE.GFXE.RemapSurface555to24(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                                break;
                            case Aow.Units.GFXE.GFXE.pfRGB32:
                                Aow.Units.GFXE.GFXE.RemapSurface555to32(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                                break;
                            case Aow.Units.GFXE.GFXE.pfRGB565:
                                Aow.Units.GFXE.GFXE.RemapSurface555to565(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                                break;
                            case Aow.Units.GFXE.GFXE.pfRGB555:
                                Aow.Units.GFXE.GFXE.BltRect(S, D, Src.Pitch, Dest.Pitch, Width * 2, Height);
                                break;
                            default:
                                Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                                break;
                        }

                        break;
                    case Aow.Units.GFXE.GFXE.pfRGB565:
                        switch (Dest.PixelFormatID)
                        {
                            case Aow.Units.GFXE.GFXE.pfRGB24:
                                Aow.Units.GFXE.GFXE.RemapSurface565to24(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                                break;
                            case Aow.Units.GFXE.GFXE.pfRGB32:
                                Aow.Units.GFXE.GFXE.RemapSurface565to32(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                                break;
                            case Aow.Units.GFXE.GFXE.pfRGB555:
                                Aow.Units.GFXE.GFXE.RemapSurface565to555(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                                break;
                            case Aow.Units.GFXE.GFXE.pfRGB565:
                                Aow.Units.GFXE.GFXE.BltRect(S, D, Src.Pitch, Dest.Pitch, Width * 2, Height);
                                break;
                            default:
                                Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                                break;
                        }

                        break;
                    case Aow.Units.GFXE.GFXE.pfRGB24:
                        switch (Dest.PixelFormatID)
                        {
                            case Aow.Units.GFXE.GFXE.pfRGB565:
                                Aow.Units.GFXE.GFXE.RemapSurface24to565(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                                break;
                            case Aow.Units.GFXE.GFXE.pfRGB555:
                                Aow.Units.GFXE.GFXE.RemapSurface24to555(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                                break;
                            case Aow.Units.GFXE.GFXE.pfRGB24:
                                Aow.Units.GFXE.GFXE.BltRect(S, D, Src.Pitch, Dest.Pitch, Width * 3, Height);
                                break;
                            default:
                                Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                                break;
                        }

                        break;
                    case Aow.Units.GFXE.GFXE.pfRGB32:
                        switch (Dest.PixelFormatID)
                        {
                            case Aow.Units.GFXE.GFXE.pfRGB565:
                                Aow.Units.GFXE.GFXE.RemapSurface32to565(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                                break;
                            case Aow.Units.GFXE.GFXE.pfRGB555:
                                Aow.Units.GFXE.GFXE.RemapSurface32to555(S, D, Src.Pitch, Dest.Pitch, Width, Height);
                                break;
                            case Aow.Units.GFXE.GFXE.pfRGB32:
                                Aow.Units.GFXE.GFXE.BltRect(S, D, Src.Pitch, Dest.Pitch, Width * 4, Height);
                                break;
                            default:
                                Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                                break;
                        }

                        break;
                    default:
                        Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                        break;
                }
            }

            return Result;
        }

        // GFXE.pas:8600
        public static int RemapSurface(Aow.Units.GFXE.TSurface Src, Aow.Units.GFXE.TSurface Dest)
        {
            int Result = default;
            Aow.Units.GFXE.TSurfaceDesc SSD = default;
            Aow.Units.GFXE.TSurfaceDesc DSD = default;
            Src.Lock(ref SSD);
            Dest.Lock(ref DSD);
            var call__212 = Aow.Units.GFXE.GFXE.RemapSurfacePart(SSD, 0, 0, SSD.Width, SSD.Height, DSD, 0, 0);
            Result = call__212;
            Src.Unlock();
            Dest.Unlock();
            return Result;
        }

        // GFXE.pas:8611
        public static int Color2PixelInfo(int Color, int PixelFormatID, Aow.Units.GFXE.TGFXEPalette Palette)
        {
            /* TODO(T-shim) Platform or editor boundary Graphics.ColorToRGB */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:8637
        public static int PixelInfo2Color(int PixelInfo, int PixelFormatID, Aow.Units.GFXE.TGFXEPalette Palette)
        {
            int Result = default;
            Result = Aow.Units.GFXE.GFXE.RemapPixelInfo(PixelInfo, PixelFormatID, Palette, Aow.Units.GFXE.GFXE.pfBGR24, null);
            return Result;
        }

        // GFXE.pas:8642
        public static int RemapPixelInfo(int PixelInfo, int SrcPixelFormatID, Aow.Units.GFXE.TGFXEPalette SrcPalette, int DestPixelFormatID, Aow.Units.GFXE.TGFXEPalette DestPalette)
        {
            int Result = default;
            byte R = default;
            byte G = default;
            byte B = default;
            byte A = default;
            switch (SrcPixelFormatID)
            {
                case Aow.Units.GFXE.GFXE.pfPAL8:
                    if (SrcPalette != null)
                    {
                        R = unchecked((byte)SrcPalette.Entries[PixelInfo].R);
                        G = unchecked((byte)SrcPalette.Entries[PixelInfo].G);
                        B = unchecked((byte)SrcPalette.Entries[PixelInfo].B);
                        A = 0;
                    }
                    else
                    {
                        throw Aow.Rtl.Exception.Create((AnsiString)"Source Palette needed (GFXE)");
                    }

                    break;
                case Aow.Units.GFXE.GFXE.pfRGB565:
                    R = unchecked((byte)(((int)(((uint)PixelInfo) >> 8)) & 248));
                    G = unchecked((byte)(((int)(((uint)PixelInfo) >> 3)) & 252));
                    B = unchecked((byte)((PixelInfo << 3) & 248));
                    A = 0;
                    break;
                case Aow.Units.GFXE.GFXE.pfRGB555:
                    R = unchecked((byte)(((int)(((uint)PixelInfo) >> 7)) & 248));
                    G = unchecked((byte)(((int)(((uint)PixelInfo) >> 2)) & 248));
                    B = unchecked((byte)((PixelInfo << 3) & 248));
                    A = 0;
                    break;
                case Aow.Units.GFXE.GFXE.pfARGB1555:
                    R = unchecked((byte)(((int)(((uint)PixelInfo) >> 7)) & 248));
                    G = unchecked((byte)(((int)(((uint)PixelInfo) >> 2)) & 248));
                    B = unchecked((byte)((PixelInfo << 3) & 248));
                    A = unchecked((byte)(((int)(((uint)PixelInfo) >> 8)) & 1));
                    break;
                case Aow.Units.GFXE.GFXE.pfRGB24:
                    R = unchecked((byte)(((int)(((uint)PixelInfo) >> 16)) & 255));
                    G = unchecked((byte)(((int)(((uint)PixelInfo) >> 8)) & 255));
                    B = unchecked((byte)(PixelInfo & 255));
                    A = 0;
                    break;
                case Aow.Units.GFXE.GFXE.pfBGR24:
                    R = unchecked((byte)(PixelInfo & 255));
                    G = unchecked((byte)(((int)(((uint)PixelInfo) >> 8)) & 255));
                    B = unchecked((byte)(((int)(((uint)PixelInfo) >> 16)) & 255));
                    A = 0;
                    break;
                case Aow.Units.GFXE.GFXE.pfRGB32:
                    A = unchecked((byte)(((int)(((uint)PixelInfo) >> 24)) & 255));
                    R = unchecked((byte)(((int)(((uint)PixelInfo) >> 16)) & 255));
                    G = unchecked((byte)(((int)(((uint)PixelInfo) >> 8)) & 255));
                    B = unchecked((byte)(PixelInfo & 255));
                    break;
                default:
                    throw Aow.Rtl.Exception.Create((AnsiString)"Source PixelFormat not supported");
                    break;
            }

            switch (DestPixelFormatID)
            {
                case Aow.Units.GFXE.GFXE.pfPAL8:
                    if (DestPalette != null)
                    {
                        Result = DestPalette.GetNearestColor((R + (G << 8)) + (B << 16));
                    }
                    else
                    {
                        throw Aow.Rtl.Exception.Create((AnsiString)"Destination Palette needed");
                    }

                    break;
                case Aow.Units.GFXE.GFXE.pfRGB565:
                    Result = (((R & 248) << 8) + ((G & 252) << 3)) + ((int)(((uint)(B & 248)) >> 3));
                    break;
                case Aow.Units.GFXE.GFXE.pfRGB555:
                    Result = (((R & 248) << 7) + ((G & 248) << 2)) + ((int)(((uint)(B & 248)) >> 3));
                    break;
                case Aow.Units.GFXE.GFXE.pfARGB1555:
                    Result = ((((A & 128) << 8) + ((R & 248) << 7)) + ((G & 248) << 2)) + ((int)(((uint)(B & 248)) >> 3));
                    break;
                case Aow.Units.GFXE.GFXE.pfRGB24:
                    Result = ((R << 16) + (G << 8)) + B;
                    break;
                case Aow.Units.GFXE.GFXE.pfBGR24:
                    Result = (R + (G << 8)) + (B << 16);
                    break;
                case Aow.Units.GFXE.GFXE.pfRGB32:
                    Result = (((A << 24) + (R << 16)) + (G << 8)) + B;
                    break;
                default:
                    throw Aow.Rtl.Exception.Create((AnsiString)"Destination PixelFormat not supported");
                    break;
            }

            return Result;
        }

        // GFXE.pas:8923
        public static int SaveSurfaceDescToBMP24(in Aow.Units.GFXE.TSurfaceDesc SD, Aow.Rtl.TRect R, Aow.Rtl.AnsiString FileName)
        {
            /* TODO(T-5.8) Untyped file buffers require a stream override */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:9258
        public static int DDSDtoSurfaceDesc(in Aow.Units.GFXE.TDDSurfaceDesc DDSD, ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Result = default;
            Result = Aow.Units.GFXE.GFXE.DD_OK;
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
            var call__213 = Aow.Units.GFXE.GFXE.MASKtoPFID(DDSD.ddpfPixelFormat.dwRGBBitCount, DDSD.ddpfPixelFormat.dwRBitMask, DDSD.ddpfPixelFormat.dwGBitMask, DDSD.ddpfPixelFormat.dwBBitMask, DDSD.ddpfPixelFormat.dwRGBAlphaBitMask);
            SD.PixelFormatID = call__213;
            SD.RMask = DDSD.ddpfPixelFormat.dwRBitMask;
            SD.GMask = DDSD.ddpfPixelFormat.dwGBitMask;
            SD.BMask = DDSD.ddpfPixelFormat.dwBBitMask;
            SD.AMask = DDSD.ddpfPixelFormat.dwRGBAlphaBitMask;
            return Result;
        }

        // GFXE.pas:9517
        // { The 5 bit blend tables: Table[I, J] is the blend of source intensity I over destination
//   intensity J (0..31). }
        public static void GenerateAlphaBlendTable(Aow.Units.GFXE.TBlendTable Table, float Alpha)
        {
            int I = default;
            int J = default;
            if (Table is Aow.Units.GFXE.TBlendTable5Bit)
            {
                {
                    I = 0;
                    byte end__214 = 31;
                    for (; I <= end__214; I += 1)
                    {
                        {
                            J = 0;
                            byte end__215 = 31;
                            for (; J <= end__215; J += 1)
                            {
                                ((Aow.Units.GFXE.TBlendTable5Bit)Table).Table[I][J] = unchecked((byte)Aow.Rtl.Rtl.Round((I * Alpha) + ((1 - Alpha) * J)));
                            }
                        }
                    }
                }
            }
            else
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"BlendTable format not supported");
            }
        }

        // GFXE.pas:9549
        public static void GenerateIntensityBlendTable(Aow.Units.GFXE.TBlendTable Table, float Intensity)
        {
            int I = default;
            int J = default;
            int V = default;
            float F = default;
            if (Table is Aow.Units.GFXE.TBlendTable5Bit)
            {
                {
                    I = 0;
                    byte end__216 = 31;
                    for (; I <= end__216; I += 1)
                    {
                        {
                            J = 0;
                            byte end__217 = 31;
                            for (; J <= end__217; J += 1)
                            {
                                if (I == 0)
                                {
                                    F = 0;
                                }
                                else
                                {
                                    F = (float)(((double)((I * Intensity) * 1.75)) / (I + J));
                                }

                                V = Aow.Rtl.Rtl.Round(I * F) + J;
                                if (V > 31)
                                {
                                    V = 31;
                                }

                                ((Aow.Units.GFXE.TBlendTable5Bit)Table).Table[I][J] = unchecked((byte)V);
                            }
                        }
                    }
                }
            }
        }

        // GFXE.pas:9568
        public static void GenerateShadowBlendTable(Aow.Units.GFXE.TBlendTable Table, float Shadow)
        {
            int I = default;
            int J = default;
            int V = default;
            float F = default;
            if (Table is Aow.Units.GFXE.TBlendTable5Bit)
            {
                {
                    I = 0;
                    byte end__218 = 31;
                    for (; I <= end__218; I += 1)
                    {
                        {
                            J = 0;
                            byte end__219 = 31;
                            for (; J <= end__219; J += 1)
                            {
                                F = (float)(((31 - I) * (((double)1) / 31)) * Shadow);
                                V = Aow.Rtl.Rtl.Round((1 - F) * J);
                                if (V < 0)
                                {
                                    V = 0;
                                }

                                if (V > J)
                                {
                                    V = J;
                                }

                                ((Aow.Units.GFXE.TBlendTable5Bit)Table).Table[I][J] = unchecked((byte)V);
                            }
                        }
                    }
                }
            }
        }

        // GFXE.pas:9585
        public static void GenerateBrightenBlendTable(Aow.Units.GFXE.TBlendTable Table, float Brighten)
        {
            int I = default;
            int J = default;
            int V = default;
            if (Table is Aow.Units.GFXE.TBlendTable5Bit)
            {
                {
                    I = 0;
                    byte end__220 = 31;
                    for (; I <= end__220; I += 1)
                    {
                        {
                            J = 0;
                            byte end__221 = 31;
                            for (; J <= end__221; J += 1)
                            {
                                V = Aow.Rtl.Rtl.Round((I * Brighten) + ((1 - Brighten) * J));
                                if (V < J)
                                {
                                    V = J;
                                }

                                ((Aow.Units.GFXE.TBlendTable5Bit)Table).Table[I][J] = unchecked((byte)V);
                            }
                        }
                    }
                }
            }
        }

        // GFXE.pas:12517
        public static Aow.Rtl.AnsiString GetErrorMsg(int ErrorCode)
        {
            Aow.Rtl.AnsiString Result = default;
            switch (ErrorCode)
            {
                case Aow.Units.GFXE.GFXE.GFXE_OK:
                    Result = (AnsiString)"No ERROR occurred!";
                    break;
                case Aow.Units.GFXE.GFXE.GFXE_ERROR:
                    Result = (AnsiString)"Undefine ERROR occurred";
                    break;
                case Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED:
                    Result = (AnsiString)"Function is not supported";
                    break;
                case Aow.Units.GFXE.GFXE.GFXE_PIXELFORMATNOTSUPPORTED:
                    Result = (AnsiString)"PixelFormat is not supported";
                    break;
                case Aow.Units.GFXE.GFXE.GFXE_BITMAPFORMATNOTSUPPORTED:
                    Result = (AnsiString)"Bitmap format is not supported";
                    break;
                default:
                    Result = (((AnsiString)"Undocumented Error (Error Code:") + Aow.Rtl.Rtl.IntToStr(ErrorCode)) + AnsiString.FromBytes(41);
                    break;
            }

            return Result;
        }

        // GFXE.pas:12530
        public static void SaveGFXESettingsToStream(Aow.Rtl.TStream Stream)
        {
        }

        // GFXE.pas:12534
        public static void LoadGFXESettingsFromStream(Aow.Rtl.TStream Stream)
        {
        }

        // GFXE.pas:12538
        public static void WriteGFXESettingsToRegistry(Aow.Rtl.AnsiString Key)
        {
            try
            {
            }
            finally
            {
            }
        }

        // GFXE.pas:12545
        public static void ReadGFXESettingsFromRegistry(Aow.Rtl.AnsiString Key)
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
            int Result = default;
            Aow.Units.GFXE.TDDSurfaceDesc DDSD = default;
            Aow.Units.GFXE.IDirectDrawSurface S = default;
            if (Aow.Units.GFXE.GFXE.DD() != null)
            {
                DDSD = default;
                DDSD.dwSize = 108;
                DDSD.dwFlags = Aow.Units.GFXE.GFXE.DDSD_CAPS;
                DDSD.ddsCaps.dwCaps = Aow.Units.GFXE.GFXE.DDSCAPS_PRIMARYSURFACE;
                var call__222 = Aow.Units.GFXE.GFXE.DD();
                call__222.SetCooperativeLevel(0, Aow.Units.GFXE.GFXE.DDSCL_NORMAL);
                var call__223 = Aow.Units.GFXE.GFXE.DD();
                var call__224 = call__223.CreateSurface(DDSD, ref S, null);
                var call__225 = Aow.Rtl.Rtl.Succeeded(call__224);
                if (call__225)
                {
                    Result = Aow.Units.GFXE.GFXE.DDStoPFID(S);
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
            bool Result = default;
            if (Aow.Units.GFXE.GFXE.DD() != null)
            {
                Aow.Units.GFXE.GFXE.EnumDDSD = default;
                Aow.Units.GFXE.GFXE.EnumDDSD.dwSize = 108;
                Aow.Units.GFXE.GFXE.EnumDDSD.dwFlags = (Aow.Units.GFXE.GFXE.DDSD_HEIGHT | Aow.Units.GFXE.GFXE.DDSD_WIDTH) | Aow.Units.GFXE.GFXE.DDSD_PIXELFORMAT;
                Aow.Units.GFXE.GFXE.EnumDDSD.dwWidth = Width;
                Aow.Units.GFXE.GFXE.EnumDDSD.dwHeight = Height;
                Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwSize = 32;
                Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwFlags = Aow.Units.GFXE.GFXE.DDPF_RGB;
                var call__226 = Aow.Units.GFXE.GFXE.PFIDtoMASK(PixelFormatID, ref Unsafe.As<int, int>(ref Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwRBitMask), ref Unsafe.As<int, int>(ref Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwGBitMask), ref Unsafe.As<int, int>(ref Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwBBitMask), ref Unsafe.As<int, int>(ref Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwRGBAlphaBitMask));
                Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwRGBBitCount = call__226;
                Aow.Units.GFXE.GFXE.ModeCount = 0;
                /* TODO(T-4.3) Address needs a cell or array backing */
                throw new System.NotImplementedException();
                Result = Aow.Units.GFXE.GFXE.ModeCount != 0;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GFXE.pas:12668
        public static void ListDisplayModes(Aow.Units.GFXE.TDisplayModeList List, int Width, int Height, int PixelFormatID)
        {
            if (Aow.Units.GFXE.GFXE.DD() != null)
            {
                Aow.Units.GFXE.GFXE.EnumDDSD = default;
                Aow.Units.GFXE.GFXE.EnumDDSD.dwSize = 108;
                if (Width != 0)
                {
                    Aow.Units.GFXE.GFXE.EnumDDSD.dwFlags = Aow.Units.GFXE.GFXE.EnumDDSD.dwFlags | Aow.Units.GFXE.GFXE.DDSD_WIDTH;
                    Aow.Units.GFXE.GFXE.EnumDDSD.dwWidth = Width;
                }

                if (Height != 0)
                {
                    Aow.Units.GFXE.GFXE.EnumDDSD.dwFlags = Aow.Units.GFXE.GFXE.EnumDDSD.dwFlags | Aow.Units.GFXE.GFXE.DDSD_HEIGHT;
                    Aow.Units.GFXE.GFXE.EnumDDSD.dwHeight = Height;
                }

                if (PixelFormatID != 0)
                {
                    Aow.Units.GFXE.GFXE.EnumDDSD.dwFlags = Aow.Units.GFXE.GFXE.EnumDDSD.dwFlags | Aow.Units.GFXE.GFXE.DDSD_PIXELFORMAT;
                    Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwSize = 32;
                    Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwFlags = Aow.Units.GFXE.GFXE.DDPF_RGB;
                    var call__228 = Aow.Units.GFXE.GFXE.PFIDtoMASK(PixelFormatID, ref Unsafe.As<int, int>(ref Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwRBitMask), ref Unsafe.As<int, int>(ref Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwGBitMask), ref Unsafe.As<int, int>(ref Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwBBitMask), ref Unsafe.As<int, int>(ref Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwRGBAlphaBitMask));
                    Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwRGBBitCount = call__228;
                }

                /* TODO(T-4.3) Address needs a cell or array backing */
                throw new System.NotImplementedException();
            }
        }

        // GFXE.pas:12697
        public static void ForceOptimal16BitPixelFormatID(int PixelFormatID)
        {
            if ((PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB555))
            {
                Aow.Units.GFXE.GFXE.ForcedPixelFormatID = PixelFormatID;
            }
            else
            {
                Aow.Units.GFXE.GFXE.ForcedPixelFormatID = 0;
            }
        }

        // GFXE.pas:12705
        public static int GetOptimalPixelFormatID(int BitCount)
        {
            /* TODO(T-shim) Platform or editor boundary Forms.Width */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:12734
        public static int DDStoPFID(Aow.Units.GFXE.IDirectDrawSurface Surface)
        {
            int Result = default;
            Aow.Units.GFXE.TDDSurfaceDesc DDSD = default;
            if (Surface != null)
            {
                DDSD = default;
                DDSD.dwSize = 108;
                Result = Surface.Lock(default, ref DDSD, Aow.Units.GFXE.GFXE.DDLOCK_WAIT, 0);
                if (Result >= 0)
                {
                    var call__230 = Aow.Units.GFXE.GFXE.MASKtoPFID(DDSD.ddpfPixelFormat.dwRGBBitCount, DDSD.ddpfPixelFormat.dwRBitMask, DDSD.ddpfPixelFormat.dwGBitMask, DDSD.ddpfPixelFormat.dwBBitMask, DDSD.ddpfPixelFormat.dwRGBAlphaBitMask);
                    Result = call__230;
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
        public static bool DirectDrawActivated()
        {
            bool Result = default;
            Result = Aow.Units.GFXE.GFXE.FDirectDrawActivated;
            return Result;
        }

        // GFXE.pas:12787
        public static Aow.Units.GFXE.IDirectDraw2 DD()
        {
            Aow.Units.GFXE.IDirectDraw2 Result = default;
            if (!Aow.Units.GFXE.GFXE.FDirectDrawActivated)
            {
                if (Aow.Units.GFXE.GFXE.ActivateDirectDraw())
                {
                    Result = Aow.Units.GFXE.GFXE.FDirectDraw;
                }
                else
                {
                    Result = null;
                }
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.FDirectDraw;
            }

            return Result;
        }

        // GFXE.pas:12874
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"GFX Engine", [typeof(Aow.Units.GFXE.TDrawPanel), typeof(Aow.Units.GFXE.TOpenImageFileDialog)]);
        }

        public const byte MaxBltProcs = 20;
        public static int ForcedPixelFormatID;
        public static Aow.Units.GFXE.TImageFileClassList ImageFileClassList;
        public static Aow.Units.FixedArray_Aow_Units_GFXE_TBltRectProc_21 BltProcs;
        public static Aow.Units.FixedArray_Aow_Rtl_AnsiString_21 BltProcNames;
        // { the blit routine indices chosen by GetOptimalGFXESettings: large and small blits, for
//     qword, dword, word and misaligned source/destination pairs }
        public static Aow.Units.FixedArray_byte_8 OptimalBltProc;
        public static int BltProcCount;
        public static int ModeCount;
        public static Aow.Units.GFXE.TDDSurfaceDesc EnumDDSD;
        public static int ListPixelFormatID;
        public static bool FDirectDrawActivated;
        public static Aow.Units.GFXE.IDirectDraw2 FDirectDraw;
        public static bool PerformanceCounterAvailable;
        public static long PerformanceFrequency;
        // GFXE.pas:1508
        // // sic: the small misaligned entry (+7) is never used
        // { The four timing tests of GetOptimalGFXESettings: the source and destination of T are aligned
//   to 8 bytes and the destination offset by 0, 4, 2 or 1 bytes; every registered blit routine
//   is run Count times and the fastest one returned with its bandwidth. }
        public static Aow.Units.GFXE.TBltTestResult TestQWordAligned(Aow.Units.GFXE.TBltTest T, int Count)
        {
            Aow.Units.GFXE.TBltTestResult Result = default;
            int I = default;
            Aow.Units.GFXE.TBltTestResult Res = default;
            int Start = default;
            int Stop = default;
            int BestTime = default;
            int Best = default;
            int J = default;
            Best = -1;
            BestTime = 16777215;
            T.Src = Aow.Rtl.Rtl.AlignPointer(T.Src + 7, ~7);
            T.Dst = Aow.Rtl.Rtl.AlignPointer(T.Dst + 7, ~7);
            T.Width = T.Width - 8;
            if (T.Width > 0)
            {
                Aow.Units.GFXE.GFXE.BltProcs[0](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
                {
                    I = 0;
                    int end__234 = Aow.Units.GFXE.GFXE.BltProcCount - 1;
                    for (; I <= end__234; I += 1)
                    {
                        Start = Aow.Units.GFXE.GFXE.GetElapsedMilliSeconds();
                        {
                            J = 0;
                            int end__235 = Count - 1;
                            for (; J <= end__235; J += 1)
                            {
                                Aow.Units.GFXE.GFXE.BltProcs[I](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
                            }
                        }

                        Stop = Aow.Units.GFXE.GFXE.GetElapsedMilliSeconds();
                        if ((Stop - Start) < BestTime)
                        {
                            BestTime = Stop - Start;
                            Best = I;
                        }
                    }
                }

                Res.Index = Best;
                try
                {
                    Res.Bandwidth = Aow.Rtl.Rtl.Round(((double)(T.Width * T.Height)) / ((((double)1000) / Count) * BestTime));
                }
                catch (System.Exception)
                {
                    Res.Bandwidth = 0;
                }
            }

            Result = Res;
            return Result;
        }

        // GFXE.pas:1545
        public static Aow.Units.GFXE.TBltTestResult TestDWordAligned(Aow.Units.GFXE.TBltTest T, int Count)
        {
            Aow.Units.GFXE.TBltTestResult Result = default;
            int I = default;
            Aow.Units.GFXE.TBltTestResult Res = default;
            int Start = default;
            int Stop = default;
            int BestTime = default;
            int Best = default;
            int J = default;
            Best = -1;
            BestTime = 16777215;
            T.Src = Aow.Rtl.Rtl.AlignPointer(T.Src + 7, ~7);
            T.Dst = Aow.Rtl.Rtl.AlignPointer(T.Dst + 7, ~7) + 4;
            T.Width = T.Width - 16;
            if (T.Width > 0)
            {
                Aow.Units.GFXE.GFXE.BltProcs[0](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
                {
                    I = 0;
                    int end__236 = Aow.Units.GFXE.GFXE.BltProcCount - 1;
                    for (; I <= end__236; I += 1)
                    {
                        Start = Aow.Units.GFXE.GFXE.GetElapsedMilliSeconds();
                        {
                            J = 0;
                            int end__237 = Count - 1;
                            for (; J <= end__237; J += 1)
                            {
                                Aow.Units.GFXE.GFXE.BltProcs[I](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
                            }
                        }

                        Stop = Aow.Units.GFXE.GFXE.GetElapsedMilliSeconds();
                        if ((Stop - Start) < BestTime)
                        {
                            BestTime = Stop - Start;
                            Best = I;
                        }
                    }
                }

                Res.Index = Best;
                try
                {
                    Res.Bandwidth = Aow.Rtl.Rtl.Round(((double)(T.Width * T.Height)) / ((((double)1000) / Count) * BestTime));
                }
                catch (System.Exception)
                {
                    Res.Bandwidth = 0;
                }
            }

            Result = Res;
            return Result;
        }

        // GFXE.pas:1582
        public static Aow.Units.GFXE.TBltTestResult TestWordAligned(Aow.Units.GFXE.TBltTest T, int Count)
        {
            Aow.Units.GFXE.TBltTestResult Result = default;
            int I = default;
            Aow.Units.GFXE.TBltTestResult Res = default;
            int Start = default;
            int Stop = default;
            int BestTime = default;
            int Best = default;
            int J = default;
            Best = -1;
            BestTime = 16777215;
            T.Src = Aow.Rtl.Rtl.AlignPointer(T.Src + 7, ~7);
            T.Dst = Aow.Rtl.Rtl.AlignPointer(T.Dst + 7, ~7) + 2;
            T.Width = T.Width - 16;
            if (T.Width > 0)
            {
                Aow.Units.GFXE.GFXE.BltProcs[0](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
                {
                    I = 0;
                    int end__238 = Aow.Units.GFXE.GFXE.BltProcCount - 1;
                    for (; I <= end__238; I += 1)
                    {
                        Start = Aow.Units.GFXE.GFXE.GetElapsedMilliSeconds();
                        {
                            J = 0;
                            int end__239 = Count - 1;
                            for (; J <= end__239; J += 1)
                            {
                                Aow.Units.GFXE.GFXE.BltProcs[I](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
                            }
                        }

                        Stop = Aow.Units.GFXE.GFXE.GetElapsedMilliSeconds();
                        if ((Stop - Start) < BestTime)
                        {
                            BestTime = Stop - Start;
                            Best = I;
                        }
                    }
                }

                Res.Index = Best;
                try
                {
                    Res.Bandwidth = Aow.Rtl.Rtl.Round(((double)(T.Width * T.Height)) / ((((double)1000) / Count) * BestTime));
                }
                catch (System.Exception)
                {
                    Res.Bandwidth = 0;
                }
            }

            Result = Res;
            return Result;
        }

        // GFXE.pas:1619
        public static Aow.Units.GFXE.TBltTestResult TestMissAligned(Aow.Units.GFXE.TBltTest T, int Count)
        {
            Aow.Units.GFXE.TBltTestResult Result = default;
            int I = default;
            Aow.Units.GFXE.TBltTestResult Res = default;
            int Start = default;
            int Stop = default;
            int BestTime = default;
            int Best = default;
            int J = default;
            Best = -1;
            BestTime = 16777215;
            T.Src = Aow.Rtl.Rtl.AlignPointer(T.Src + 7, ~7);
            T.Dst = Aow.Rtl.Rtl.AlignPointer(T.Dst + 7, ~7) + 1;
            T.Width = T.Width - 16;
            if (T.Width > 0)
            {
                Aow.Units.GFXE.GFXE.BltProcs[0](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
                {
                    I = 0;
                    int end__240 = Aow.Units.GFXE.GFXE.BltProcCount - 1;
                    for (; I <= end__240; I += 1)
                    {
                        Start = Aow.Units.GFXE.GFXE.GetElapsedMilliSeconds();
                        {
                            J = 0;
                            int end__241 = Count - 1;
                            for (; J <= end__241; J += 1)
                            {
                                Aow.Units.GFXE.GFXE.BltProcs[I](T.Src, T.Dst, T.SrcPitch, T.DstPitch, T.Width, T.Height);
                            }
                        }

                        Stop = Aow.Units.GFXE.GFXE.GetElapsedMilliSeconds();
                        if ((Stop - Start) < BestTime)
                        {
                            BestTime = Stop - Start;
                            Best = I;
                        }
                    }
                }

                Res.Index = Best;
                try
                {
                    Res.Bandwidth = Aow.Rtl.Rtl.Round(((double)(T.Width * T.Height)) / ((((double)1000) / Count) * BestTime));
                }
                catch (System.Exception)
                {
                    Res.Bandwidth = 0;
                }
            }

            Result = Res;
            return Result;
        }

        // GFXE.pas:8752
        // { The BMP file format: TBMPImageFile and its helpers. }
        public static int LoadBMP(Aow.Rtl.AnsiString FileName, ref Aow.Units.GFXE.TSurfaceDesc SD, int X, int Y)
        {
            /* TODO(T-5.8) Untyped file buffers require a stream override */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:8968
        public static Aow.Units.GFXE.TSurface CreateSurfaceFromBMP(Aow.Rtl.AnsiString FileName)
        {
            /* TODO(T-5.8) Untyped file buffers require a stream override */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:9009
        public static int GetBMPPixelFormat(Aow.Rtl.AnsiString FileName)
        {
            /* TODO(T-5.8) Untyped file buffers require a stream override */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:9529
        public static void GenerateLinearAlphaBlendTable(Aow.Units.GFXE.TBlendTable Table, float Alpha)
        {
            int I = default;
            int J = default;
            float F = default;
            if (Table is Aow.Units.GFXE.TBlendTable5Bit)
            {
                {
                    I = 0;
                    byte end__242 = 31;
                    for (; I <= end__242; I += 1)
                    {
                        if (Alpha > 0)
                        {
                            F = (float)(((double)I) / (31 * Alpha));
                        }
                        else
                        {
                            F = 1;
                        }

                        if (F > 1)
                        {
                            F = 1;
                        }

                        {
                            J = 0;
                            byte end__243 = 31;
                            for (; J <= end__243; J += 1)
                            {
                                ((Aow.Units.GFXE.TBlendTable5Bit)Table).Table[I][J] = unchecked((byte)Aow.Rtl.Rtl.Round((I * F) + ((1 - F) * J)));
                            }
                        }
                    }
                }
            }
            else
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"BlendTable format not supported");
            }
        }

        // GFXE.pas:11039
        public static void InitBlendTables(int Bits, int Count)
        {
            Aow.Units.GFXE.GFXE.AlphaBlendTable.GenerateLookupTables(Bits, Aow.Units.GFXE.GFXE.GenerateAlphaBlendTable, Count);
            Aow.Units.GFXE.GFXE.IntensityBlendTable.GenerateLookupTables(Bits, Aow.Units.GFXE.GFXE.GenerateIntensityBlendTable, Count);
            Aow.Units.GFXE.GFXE.BrightenBlendTable.GenerateLookupTables(Bits, Aow.Units.GFXE.GFXE.GenerateBrightenBlendTable, Count);
            Aow.Units.GFXE.GFXE.ShadowBlendTable.GenerateLookupTables(Bits, Aow.Units.GFXE.GFXE.GenerateShadowBlendTable, Count);
            Aow.Units.GFXE.GFXE.LinearAlphaBlendTable.GenerateLookupTables(Bits, Aow.Units.GFXE.GFXE.GenerateLinearAlphaBlendTable, Count);
        }

        // GFXE.pas:12554
        // { DirectDraw: display modes and the DirectDraw object }
        public static int DisplayModeSupportedCallback(Ptr<Aow.Units.GFXE.TDDSurfaceDesc> DDSD, object Context)
        {
            int Result = default;
            if (((Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwRBitMask == DDSD.Value.ddpfPixelFormat.dwRBitMask) && (Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwGBitMask == DDSD.Value.ddpfPixelFormat.dwGBitMask)) && (Aow.Units.GFXE.GFXE.EnumDDSD.ddpfPixelFormat.dwBBitMask == DDSD.Value.ddpfPixelFormat.dwBBitMask))
            {
                Aow.Units.GFXE.GFXE.ModeCount += 1;
            }

            Result = Aow.Units.GFXE.GFXE.DDENUMRET_OK;
            return Result;
        }

        // GFXE.pas:12657
        // { ListPixelFormatID is never set (original): every mode is listed. }
        public static int ListDisplayModesCallback(Ptr<Aow.Units.GFXE.TDDSurfaceDesc> DDSD, object Context)
        {
            int Result = default;
            int PFID = default;
            var call__244 = Aow.Units.GFXE.GFXE.MASKtoPFID(DDSD.Value.ddpfPixelFormat.dwRGBBitCount, DDSD.Value.ddpfPixelFormat.dwRBitMask, DDSD.Value.ddpfPixelFormat.dwGBitMask, DDSD.Value.ddpfPixelFormat.dwBBitMask, DDSD.Value.ddpfPixelFormat.dwRGBAlphaBitMask);
            PFID = call__244;
            if ((Aow.Units.GFXE.GFXE.ListPixelFormatID == 0) || (PFID == Aow.Units.GFXE.GFXE.ListPixelFormatID))
            {
                ((Aow.Units.GFXE.TDisplayModeList)Context).Add(DDSD.Value.dwWidth, DDSD.Value.dwHeight, PFID);
            }

            Result = Aow.Units.GFXE.GFXE.DDENUMRET_OK;
            return Result;
        }

        // GFXE.pas:12761
        public static bool ActivateDirectDraw()
        {
            /* TODO(T-shim) Platform or editor boundary ActiveX.CoInitialize */
            throw new System.NotImplementedException();
        }

        // GFXE.pas:12778
        public static void DeactivateDirectDraw()
        {
            if (Aow.Units.GFXE.GFXE.FDirectDrawActivated)
            {
                Aow.Units.GFXE.GFXE.FDirectDraw = null;
                Aow.Rtl.Rtl.CoUninitialize();
            }
        }

        public static void Initialize()
        {
            var call__245 = Aow.Rtl.Rtl.QueryPerformanceFrequency(ref Aow.Units.GFXE.GFXE.PerformanceFrequency);
            Aow.Units.GFXE.GFXE.PerformanceCounterAvailable = call__245;
            var call__246 = Aow.Units.GFXE.TLookupBlendTableCollection100.Create();
            Aow.Units.GFXE.GFXE.AlphaBlendTable = call__246;
            var call__247 = Aow.Units.GFXE.TLookupBlendTableCollection100.Create();
            Aow.Units.GFXE.GFXE.IntensityBlendTable = call__247;
            var call__248 = Aow.Units.GFXE.TLookupBlendTableCollection100.Create();
            Aow.Units.GFXE.GFXE.BrightenBlendTable = call__248;
            var call__249 = Aow.Units.GFXE.TLookupBlendTableCollection100.Create();
            Aow.Units.GFXE.GFXE.ShadowBlendTable = call__249;
            var call__250 = Aow.Units.GFXE.TLookupBlendTableCollection100.Create();
            Aow.Units.GFXE.GFXE.LinearAlphaBlendTable = call__250;
            Aow.Units.GFXE.GFXE.InitBlendTables(5, 31);
            Aow.Units.GFXE.GFXE.FDirectDrawActivated = false;
            Aow.Units.GFXE.GFXE.BltProcCount = 0;
            Aow.Units.GFXE.GFXE.ForcedPixelFormatID = 0;
            var call__251 = Aow.Units.GFXE.TImageFileClassList.Create();
            Aow.Units.GFXE.GFXE.ImageFileClassList = call__251;
            Aow.Units.GFXE.GFXE.RegisterImageFileClasses([Aow.Units.GFXE.TImageFile.Meta.Of<Aow.Units.GFXE.TBMPImageFile>()]);
            Aow.Units.GFXE.GFXE.RegisterBltProc(Aow.Units.GFXE.GFXE.RegisterBltRect, (AnsiString)"Register Blt");
            if (Aow.Units.GFXE.GFXE.MMXsupport())
            {
                Aow.Units.GFXE.GFXE.RegisterBltProc(Aow.Units.GFXE.GFXE.MMXBltRect, (AnsiString)"MMX Blt");
            }

            Aow.Units.GFXE.GFXE.RegisterBltProc(Aow.Units.GFXE.GFXE.RepMovsdBltRect, (AnsiString)"MOVSD Blt");
        }

        public static void Finalize()
        {
            Aow.Units.GFXE.GFXE.AlphaBlendTable.Free();
            Aow.Units.GFXE.GFXE.IntensityBlendTable.Free();
            Aow.Units.GFXE.GFXE.BrightenBlendTable.Free();
            Aow.Units.GFXE.GFXE.ShadowBlendTable.Free();
            Aow.Units.GFXE.GFXE.LinearAlphaBlendTable.Free();
            Aow.Units.GFXE.GFXE.DeactivateDirectDraw();
            Aow.Units.GFXE.GFXE.ImageFileClassList.Free();
        }
    }
}
