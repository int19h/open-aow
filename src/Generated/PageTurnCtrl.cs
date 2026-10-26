// Generated from PageTurnCtrl.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;

namespace Aow.Units.PageTurnCtrl
{
    // { TPageTurnControl (instance size 2228, VMT 55809D40) }
    public partial class TPageTurnControl : Aow.Rtl.TObject
    {
        public float FScale;
        // { +$04 }
        public float FShade;
        // { +$08 }
        public float FHalfWidth;
        // { +$0C }
        public int FSpeed;
        // { +$10 }
        public Aow.Units.PageTurnCtrl.TPageTurnMode FMode;
        // { +$14 }
        public Aow.Units.FixedArray_byte_3 FReserved15;
        public int FWidth;
        // { +$18 }
        public int FHeight;
        // { +$1C }
        public Aow.Units.GFXE.TSurfaceDesc FSource1;
        // { +$20 }
        public int FSourceX1;
        public int FSourceY1;
        // { +$5C, +$60 }
        public Aow.Units.GFXE.TSurfaceDesc FSource2;
        // { +$64 }
        public int FSourceX2;
        public int FSourceY2;
        // { +$A0, +$A4 }
        public Aow.Units.FixedArray_float_512 FInvCos;
        // { +$A8 }
        public float FRadius;
        // { +$8A8 }
        public float FShadow;
        // { +$8AC }
        public int FPosition;
        // PageTurnCtrl.pas:51
        // { +$8B0 }
        public void Create__init_TPageTurnControl()
        {
            ;
            FScale = (float)0.5;
            FSpeed = 2;
            FShade = (float)2;
            GenerateInvCosTable();
        }

        // PageTurnCtrl.pas:60
        public void SetPageDimensions(int Width, int Height)
        {
            FWidth = Width;
            FHeight = Height;
            FHalfWidth = (int)(((uint)FWidth) >> 1);
        }

        // PageTurnCtrl.pas:67
        public float GetInvCos(float Value)
        {
            float Result = default;
            Aow.Rtl.Rtl.Assert((Value >= -1) && (Value <= 1), (AnsiString)"", "D:\\git\\aow\\src\\delphi3\\AoWEPACK\\PageTurnCtrl.pas", 85);
            if (Value >= 0)
            {
                Result = FInvCos[Aow.Rtl.Rtl.Round(Value * 511)];
            }
            else
            {
                var call__1 = System.Math.PI;
                Result = (float)(call__1 - FInvCos[Aow.Rtl.Rtl.Round(-Value * 511)]);
            }

            return Result;
        }

        // PageTurnCtrl.pas:90
        public void GenerateInvCosTable()
        {
            float CosValue = default;
            float Angle = default;
            float Target = default;
            int I = default;
            Angle = 0;
            I = 511;
            while (Angle < (((double)System.Math.PI) / 2))
            {
                CosValue = (float)System.Math.Cos(Angle);
                Target = (float)(((double)I) / 511);
                if ((CosValue - Target) > 0)
                {
                    FInvCos[I] = Angle;
                }
                else
                {
                    I -= 1;
                }

                Angle = (float)(Angle + 1E-05);
            }
        }

        // PageTurnCtrl.pas:119
        public void BltVerticalScanLine(int SourceX, int DestX, int DestY, in Aow.Units.GFXE.TSurfaceDesc Dest, int Top, int Bottom)
        {
            int SourceY = default;
            int Height = default;
            int Y = default;
            object SourceP = default;
            object DestP = default;
            int SourcePitch = default;
            if ((Dest.ClipRect.Left <= DestX) && (DestX < Dest.ClipRect.Right))
            {
                SourceY = Top;
                Y = DestY + Top;
                Height = Bottom - Top;
                if (Y < Dest.ClipRect.Top)
                {
                    Height -= Dest.ClipRect.Top - Y;
                    SourceY += Dest.ClipRect.Top - Y;
                    Y = Dest.ClipRect.Top;
                }

                if ((Height + Y) > Dest.ClipRect.Bottom)
                {
                    Height -= (Height + Y) - Dest.ClipRect.Bottom;
                }

                if (Height > 0)
                {
                    if (SourceX < (FWidth * 2))
                    {
                        SourceP = ((Aow.Rtl.Rtl.BytePointer(FSource1.Data) + ((FSourceX1 + SourceX) * 2)) + ((FSourceY1 + SourceY) * FSource1.Pitch)).Box();
                        SourcePitch = FSource1.Pitch;
                    }
                    else
                    {
                        SourceP = ((Aow.Rtl.Rtl.BytePointer(FSource2.Data) + (((SourceX - (FWidth * 2)) + FSourceX2) * 2)) + ((FSourceY2 + SourceY) * FSource2.Pitch)).Box();
                        SourcePitch = FSource2.Pitch;
                    }

                    DestP = ((Aow.Rtl.Rtl.BytePointer(Dest.Data) + (DestX * 2)) + (Dest.Pitch * Y)).Box();
                    Aow.Units.PageTurnCtrl.PageTurnCtrl.BltColumn(SourceP, SourcePitch, DestP, Dest.Pitch, Height);
                }
            }
        }

        // PageTurnCtrl.pas:151
        public void Start(Aow.Units.PageTurnCtrl.TPageTurnMode Mode)
        {
            FMode = Mode;
            switch (FMode)
            {
                case Aow.Units.PageTurnCtrl.TPageTurnMode.ptLeft:
                    FRadius = FHalfWidth;
                    FShadow = (float)(FRadius * 0.5);
                    FPosition = FWidth;
                    break;
                case Aow.Units.PageTurnCtrl.TPageTurnMode.ptRight:
                    FRadius = 1;
                    FShadow = (float)(FRadius * 0.5);
                    FPosition = 0;
                    break;
                case Aow.Units.PageTurnCtrl.TPageTurnMode.ptOpen:
                    FPosition = 0;
                    break;
                case Aow.Units.PageTurnCtrl.TPageTurnMode.ptClose:
                    FPosition = 180;
                    break;
            }
        }

        // PageTurnCtrl.pas:164
        public bool UpdateScene()
        {
            bool Result = default;
            switch (FMode)
            {
                case Aow.Units.PageTurnCtrl.TPageTurnMode.ptLeft:
                    FShade = (float)3.5;
                    if (FPosition > 0)
                    {
                        FPosition -= FSpeed;
                        if (FPosition < 0)
                        {
                            FPosition = 0;
                        }
                    }
                    else
                    {
                        if (FRadius > 0)
                        {
                            var call__2 = System.Math.PI;
                            FRadius = (float)(FRadius - (((double)((FSpeed * 1.25) * 2)) / call__2));
                            if (FRadius < 1)
                            {
                                FRadius = 0;
                            }

                            FShadow = (float)(FRadius * 0.5);
                        }
                    }

                    Result = FRadius >= 1;
                    break;
                case Aow.Units.PageTurnCtrl.TPageTurnMode.ptRight:
                    FShade = (float)2;
                    if (FRadius < FHalfWidth)
                    {
                        var call__3 = System.Math.PI;
                        FRadius = (float)(FRadius + (((double)((FSpeed * 1.5) * 2)) / call__3));
                        if (FRadius > FHalfWidth)
                        {
                            FRadius = FHalfWidth;
                        }

                        FShadow = (float)(FRadius * 0.5);
                    }
                    else
                    {
                        FPosition += FSpeed;
                        if (FPosition > FWidth)
                        {
                            FPosition = FWidth;
                        }
                    }

                    Result = FPosition < FWidth;
                    break;
                case Aow.Units.PageTurnCtrl.TPageTurnMode.ptOpen:
                    FPosition += FSpeed;
                    if (FPosition > 180)
                    {
                        FPosition = 180;
                    }

                    Result = FPosition < 180;
                    break;
                case Aow.Units.PageTurnCtrl.TPageTurnMode.ptClose:
                    FPosition -= FSpeed;
                    if (FPosition < 0)
                    {
                        FPosition = 0;
                    }

                    Result = FPosition > 0;
                    break;
                default:
                    Result = false;
                    break;
            }

            return Result;
        }

        // PageTurnCtrl.pas:215
        public void RenderLeftRightScene(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc Dest)
        {
            int OriginalLength = default;
            int Offset = default;
            int BaseX = default;
            int Span = default;
            float Step = default;
            float Angle = default;
            float CosValue = default;
            float Height = default;
            float Width = default;
            float RadiusFactor = default;
            int ArcLength = default;
            int SourceBase = default;
            int Limit = default;
            Aow.Rtl.TRect R = default;
            int SourceOffset = default;
            int I = default;
            if (FRadius >= 1)
            {
                Height = (float)(FShadow * FScale);
                Width = (float)(FRadius * FScale);
                R.Left = FSourceX1;
                R.Top = FSourceY1;
                R.Right = (FSourceX1 + FWidth) + FPosition;
                R.Bottom = FSourceY1 + FHeight;
                Aow.Units.GFXE.GFXE.BltSDRectClipped(FSource1, R, Dest, X, Y);
                R.Left = (FSourceX2 + FWidth) + FPosition;
                R.Top = FSourceY2;
                R.Right = (FWidth * 2) + FSourceX2;
                R.Bottom = FSourceY2 + FHeight;
                Aow.Units.GFXE.GFXE.BltSDRectClipped(FSource2, R, Dest, (X + FWidth) + FPosition, Y);
                var call__4 = System.Math.PI;
                OriginalLength = Aow.Rtl.Rtl.Round((call__4 * FRadius) * 0.25);
                var call__5 = System.Math.PI;
                ArcLength = Aow.Rtl.Rtl.Round((call__5 * FRadius) * 0.25);
                SourceBase = FWidth + FPosition;
                RadiusFactor = (float)((((double)2) / System.Math.PI) * ArcLength);
                BaseX = (X + FWidth) + FPosition;
                Span = Aow.Rtl.Rtl.Round(FShadow);
                Limit = FWidth - FPosition;
                Step = (float)(((double)1) / FShadow);
                CosValue = 0;
                SourceOffset = 0;
                I = 0;
                while ((SourceOffset < Limit) && (I < Span))
                {
                    Angle = GetInvCos(CosValue);
                    SourceOffset = ArcLength - Aow.Rtl.Rtl.Round(RadiusFactor * Angle);
                    Offset = Aow.Rtl.Rtl.Round(Height - (System.Math.Sin(Angle) * Height));
                    if (SourceOffset < Limit)
                    {
                        if (((ArcLength - SourceOffset) + ArcLength) < Limit)
                        {
                            BltVerticalScanLine(SourceBase + SourceOffset, ((X + FWidth) + FPosition) + I, Y - Offset, Dest, FHeight - Aow.Rtl.Rtl.Round(Width), FHeight);
                        }
                        else
                        {
                            BltVerticalScanLine(SourceBase + SourceOffset, BaseX + I, Y - Offset, Dest, 0, FHeight);
                        }
                    }

                    CosValue = (float)(CosValue + Step);
                    I += 1;
                }

                var call__6 = System.Math.PI;
                ArcLength = Aow.Rtl.Rtl.Round((call__6 * FRadius) * 0.25);
                SourceBase = ((FWidth * 3) - FPosition) - ArcLength;
                RadiusFactor = (float)((((double)2) / System.Math.PI) * ArcLength);
                BaseX = (X + FWidth) + FPosition;
                Span = Aow.Rtl.Rtl.Round(FShadow);
                I = Span;
                CosValue = 1;
                Limit = (FWidth * 2) - SourceBase;
                while ((SourceOffset >= Limit) && (I >= 0))
                {
                    Angle = GetInvCos(CosValue);
                    SourceOffset = -Aow.Rtl.Rtl.Round(RadiusFactor * Angle);
                    Offset = Aow.Rtl.Rtl.Round((System.Math.Sin(Angle) * Height) + Height);
                    if (SourceOffset >= Limit)
                    {
                        BltVerticalScanLine(SourceBase + SourceOffset, BaseX + I, Y - Offset, Dest, 0, FHeight);
                    }

                    CosValue = (float)(CosValue - Step);
                    I -= 1;
                }

                var call__7 = System.Math.PI;
                ArcLength = Aow.Rtl.Rtl.Round(((call__7 * FRadius) * 0.25) * FShade);
                SourceBase = (((FWidth * 3) - FPosition) - (OriginalLength * 2)) - ArcLength;
                Span = Aow.Rtl.Rtl.Round(FShadow * FShade);
                BaseX = ((X + FWidth) + FPosition) - Span;
                Height = (float)((FShadow * FScale) * 3.4142);
                Width = (float)((FRadius * FScale) * 0.707);
                Limit = (FWidth * 2) - SourceBase;
                SourceOffset = Aow.Rtl.Rtl.Round(ArcLength);
                I = Span;
                CosValue = 0;
                Step = (float)(((double)0.707) / (FShadow * FShade));
                if (Step > 1)
                {
                    Step = (float)1;
                }

                while ((SourceOffset >= Limit) && (I >= 0))
                {
                    Angle = GetInvCos(CosValue);
                    var call__8 = System.Math.PI;
                    var call__9 = System.Math.PI;
                    SourceOffset = Aow.Rtl.Rtl.Round((ArcLength * (Angle - (((double)call__8) / 4))) * (((double)4) / call__9));
                    Offset = Aow.Rtl.Rtl.Round((System.Math.Sin(Angle) * Height) - Width);
                    if (SourceOffset >= Limit)
                    {
                        BltVerticalScanLine(SourceBase + SourceOffset, BaseX + I, Y - Offset, Dest, 0, FHeight);
                    }

                    CosValue = (float)(CosValue + Step);
                    I -= 1;
                }

                BaseX = ((X + FWidth) + FPosition) - (Span * 2);
                SourceOffset = 0;
                I = Span;
                while (((SourceOffset >= Limit) && (I >= 0)) && (CosValue > 0))
                {
                    CosValue = (float)(CosValue - Step);
                    Angle = GetInvCos(CosValue);
                    var call__10 = System.Math.PI;
                    var call__11 = System.Math.PI;
                    SourceOffset = -Aow.Rtl.Rtl.Round((ArcLength * (((double)4) / call__10)) * (Angle - (((double)call__11) / 4)));
                    Offset = Aow.Rtl.Rtl.Round((1 - System.Math.Sin(Angle)) * Height);
                    if (SourceOffset >= Limit)
                    {
                        BltVerticalScanLine(SourceBase + SourceOffset, BaseX + I, Y - Offset, Dest, 0, FHeight);
                    }

                    I -= 1;
                }

                R.Left = FSourceX2;
                R.Top = FSourceY2;
                R.Right = ((FSourceX2 + FWidth) - FPosition) - Aow.Rtl.Rtl.Round(OriginalLength * ((2 * FShade) + 2));
                R.Bottom = FSourceY2 + FHeight;
                if (R.Right > R.Left)
                {
                    Aow.Units.GFXE.GFXE.BltSDRectClipped(FSource2, R, Dest, BaseX - (R.Right - R.Left), Y);
                }
            }
        }

        // PageTurnCtrl.pas:324
        public void RenderOpenCloseScene(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc Dest)
        {
            int BaseX = default;
            int Height = default;
            int Width = default;
            float Step = default;
            float SourceX = default;
            int Offset = default;
            Aow.Rtl.TRect R = default;
            int I = default;
            int PageWidth = default;
            R.Left = FSourceX2 + FWidth;
            R.Top = FSourceY2;
            R.Right = FSourceX2 + (FWidth * 2);
            R.Bottom = FSourceY2 + FHeight;
            Aow.Units.GFXE.GFXE.BltSDRectClipped(FSource2, R, Dest, X + FWidth, Y);
            var call__12 = System.Math.PI;
            var call__13 = System.Math.Sin(((double)(FPosition * call__12)) / 180);
            Height = Aow.Rtl.Rtl.Round(call__13 * FWidth);
            var call__14 = System.Math.PI;
            var call__15 = System.Math.Cos(((double)(FPosition * call__14)) / 180);
            Width = Aow.Rtl.Rtl.Round(call__15 * FWidth);
            if (Width > 0)
            {
                Step = (float)(((double)FWidth) / Width);
                BaseX = X + FWidth;
                PageWidth = FWidth;
                SourceX = PageWidth;
                {
                    I = 0;
                    int end__16 = Width - 1;
                    for (; I <= end__16; I += 1)
                    {
                        Offset = Aow.Rtl.Rtl.Round(((((double)I) / Width) * Height) * FScale);
                        BltVerticalScanLine(Aow.Rtl.Rtl.Round(SourceX), BaseX + I, Y - Offset, Dest, 0, FHeight);
                        SourceX = (float)(SourceX + Step);
                    }
                }
            }
            else
            {
                if (Width < 0)
                {
                    Width = -Width;
                    Step = (float)(((double)FWidth) / Width);
                    BaseX = X + FWidth;
                    SourceX = FWidth * 3;
                    {
                        I = 0;
                        int end__17 = Width - 1;
                        for (; I <= end__17; I += 1)
                        {
                            Offset = Aow.Rtl.Rtl.Round(((((double)I) / Width) * Height) * FScale);
                            BltVerticalScanLine(Aow.Rtl.Rtl.Round(SourceX), BaseX - I, Y - Offset, Dest, 0, FHeight);
                            SourceX = (float)(SourceX - Step);
                        }
                    }
                }
            }
        }

        // PageTurnCtrl.pas:361
        public void RenderScene(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc Dest, int X1, int Y1, in Aow.Units.GFXE.TSurfaceDesc Source1, int X2, int Y2, in Aow.Units.GFXE.TSurfaceDesc Source2, ref Aow.Rtl.TRect R)
        {
            switch (FMode)
            {
                case Aow.Units.PageTurnCtrl.TPageTurnMode.ptLeft:
                    FSource1 = Source1;
                    FSourceX1 = X1;
                    FSourceY1 = Y1;
                    FSource2 = Source2;
                    FSourceX2 = X2;
                    FSourceY2 = Y2;
                    RenderLeftRightScene(X, Y, Dest);
                    R.Left = X;
                    R.Right = X + (FWidth * 2);
                    R.Top = Y - (Aow.Rtl.Rtl.Round(FScale * FRadius) + 1);
                    R.Bottom = Y + FHeight;
                    break;
                case Aow.Units.PageTurnCtrl.TPageTurnMode.ptRight:
                    FSource1 = Source2;
                    FSourceX1 = X2;
                    FSourceY1 = Y2;
                    FSource2 = Source1;
                    FSourceX2 = X1;
                    FSourceY2 = Y1;
                    RenderLeftRightScene(X, Y, Dest);
                    R.Left = X;
                    R.Right = X + (FWidth * 2);
                    R.Top = Y - (Aow.Rtl.Rtl.Round(FScale * FRadius) + 1);
                    R.Bottom = Y + FHeight;
                    break;
                case Aow.Units.PageTurnCtrl.TPageTurnMode.ptOpen:
                    FSource1 = Source1;
                    FSourceX1 = X1;
                    FSourceY1 = Y1;
                    FSource2 = Source2;
                    FSourceX2 = X2;
                    FSourceY2 = Y2;
                    RenderOpenCloseScene(X, Y, Dest);
                    R.Left = X;
                    R.Right = X + (FWidth * 2);
                    var call__18 = System.Math.PI;
                    var call__19 = System.Math.Sin(((double)(FPosition * call__18)) / 180);
                    R.Top = Y - Aow.Rtl.Rtl.Round(((call__19 * FScale) * FWidth) + 1);
                    R.Bottom = Y + FHeight;
                    break;
                case Aow.Units.PageTurnCtrl.TPageTurnMode.ptClose:
                    FSource1 = Source2;
                    FSourceX1 = X2;
                    FSourceY1 = Y2;
                    FSource2 = Source1;
                    FSourceX2 = X1;
                    FSourceY2 = Y1;
                    RenderOpenCloseScene(X, Y, Dest);
                    R.Left = X;
                    R.Right = X + (FWidth * 2);
                    var call__20 = System.Math.PI;
                    var call__21 = System.Math.Sin(((double)(FPosition * call__20)) / 180);
                    R.Top = Y - Aow.Rtl.Rtl.Round(((call__21 * FScale) * FWidth) + 1);
                    R.Bottom = Y + FHeight;
                    break;
            }
        }

        // PageTurnCtrl.pas:36
        public static Aow.Units.PageTurnCtrl.TPageTurnControl Create()
        {
            Aow.Units.PageTurnCtrl.TPageTurnControl o = new Aow.Units.PageTurnCtrl.TPageTurnControl();
            try
            {
                o.Create__init_TPageTurnControl();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public enum TPageTurnMode : byte
    {
        ptLeft = 0,
        ptRight = 1,
        ptOpen = 2,
        ptClose = 3
    }

    public static partial class PageTurnCtrl
    {
        // PageTurnCtrl.pas:108
        public static void BltColumn(object Source, int SourcePitch, object Dest, int DestPitch, int Height)
        {
            while (Height > 0)
            {
                {
                    ushort value__22 = unchecked((ushort)MemoryMarshal.Read<ushort>(Aow.Rtl.Rtl.BytePointer(Source).AsSpan(2)));
                    MemoryMarshal.Write<ushort>(Aow.Rtl.Rtl.BytePointer(Dest).AsSpan(2), in value__22);
                }

                Source = (Aow.Rtl.Rtl.BytePointer(Source) + SourcePitch).Box();
                Dest = (Aow.Rtl.Rtl.BytePointer(Dest) + DestPitch).Box();
                Height -= 1;
            }
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
