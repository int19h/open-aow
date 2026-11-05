// Translated from PageTurnCtrl.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.InteropServices;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;

namespace Aow.Units.PageTurnCtrl
{
    public partial class TPageTurnControl : TObject
    {
        public float FScale;
        public float FShade;
        public float FHalfWidth;
        public int FSpeed;
        public TPageTurnMode FMode;
        public int FWidth;
        public int FHeight;
        public TSurfaceDesc FSource1;
        public int FSourceX1;
        public int FSourceY1;
        public TSurfaceDesc FSource2;
        public int FSourceX2;
        public int FSourceY2;
        public FixedArray_float_512 FInvCos;
        public float FRadius;
        public float FShadow;
        public int FPosition;
        // PageTurnCtrl.pas:51
        public TPageTurnControl()
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
            Assert((double)Value >= -1 && (double)Value <= 1, (AnsiString)"", "PageTurnCtrl.pas", 85);
            if ((double)Value >= 0)
            {
                Result = FInvCos[Round((double)Value * 511)];
            }
            else
            {
                var call__1 = Math.PI;
                Result = (float)((double)call__1 - FInvCos[Round((double)-Value * 511)]);
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
            while ((double)Angle < (double)Math.PI / 2)
            {
                CosValue = (float)Math.Cos(Angle);
                Target = (float)((double)I / 511);
                if ((double)((double)CosValue - Target) > 0)
                {
                    FInvCos[I] = Angle;
                }
                else
                {
                    I -= 1;
                }

                Angle = (float)((double)Angle + 1E-05);
            }
        }

        // PageTurnCtrl.pas:119
        public void BltVerticalScanLine(int SourceX, int DestX, int DestY, in TSurfaceDesc Dest, int Top, int Bottom)
        {
            int SourceY = default;
            int Height = default;
            int Y = default;
            object SourceP = default;
            object DestP = default;
            int SourcePitch = default;
            if (Dest.ClipRect.Left <= DestX && DestX < Dest.ClipRect.Right)
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

                if (Height + Y > Dest.ClipRect.Bottom)
                {
                    Height -= Height + Y - Dest.ClipRect.Bottom;
                }

                if (Height > 0)
                {
                    if (SourceX < FWidth * 2)
                    {
                        SourceP = (BytePointer(FSource1.Data) + (FSourceX1 + SourceX) * 2 + (FSourceY1 + SourceY) * FSource1.Pitch).Box();
                        SourcePitch = FSource1.Pitch;
                    }
                    else
                    {
                        SourceP = (BytePointer(FSource2.Data) + (SourceX - FWidth * 2 + FSourceX2) * 2 + (FSourceY2 + SourceY) * FSource2.Pitch).Box();
                        SourcePitch = FSource2.Pitch;
                    }

                    DestP = (BytePointer(Dest.Data) + DestX * 2 + Dest.Pitch * Y).Box();
                    PageTurnCtrl.BltColumn(SourceP, SourcePitch, DestP, Dest.Pitch, Height);
                }
            }
        }

        // PageTurnCtrl.pas:151
        public void Start(TPageTurnMode Mode)
        {
            FMode = Mode;
            switch (FMode)
            {
                case TPageTurnMode.ptLeft:
                    FRadius = FHalfWidth;
                    FShadow = (float)((double)FRadius * 0.5);
                    FPosition = FWidth;
                    break;
                case TPageTurnMode.ptRight:
                    FRadius = 1;
                    FShadow = (float)((double)FRadius * 0.5);
                    FPosition = 0;
                    break;
                case TPageTurnMode.ptOpen:
                    FPosition = 0;
                    break;
                case TPageTurnMode.ptClose:
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
                case TPageTurnMode.ptLeft:
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
                        if ((double)FRadius > 0)
                        {
                            var call__2 = Math.PI;
                            FRadius = (float)((double)FRadius - (double)((double)((double)FSpeed * 1.25) * 2) / call__2);
                            if ((double)FRadius < 1)
                            {
                                FRadius = 0;
                            }

                            FShadow = (float)((double)FRadius * 0.5);
                        }
                    }

                    Result = (double)FRadius >= 1;
                    break;
                case TPageTurnMode.ptRight:
                    FShade = (float)2;
                    if ((double)FRadius < FHalfWidth)
                    {
                        var call__3 = Math.PI;
                        FRadius = (float)((double)FRadius + (double)((double)((double)FSpeed * 1.5) * 2) / call__3);
                        if ((double)FRadius > FHalfWidth)
                        {
                            FRadius = FHalfWidth;
                        }

                        FShadow = (float)((double)FRadius * 0.5);
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
                case TPageTurnMode.ptOpen:
                    FPosition += FSpeed;
                    if (FPosition > 180)
                    {
                        FPosition = 180;
                    }

                    Result = FPosition < 180;
                    break;
                case TPageTurnMode.ptClose:
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
        public void RenderLeftRightScene(int X, int Y, in TSurfaceDesc Dest)
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
            TRect R = default;
            int SourceOffset = default;
            int I = default;
            if ((double)FRadius >= 1)
            {
                Height = (float)((double)FShadow * FScale);
                Width = (float)((double)FRadius * FScale);
                R.Left = FSourceX1;
                R.Top = FSourceY1;
                R.Right = FSourceX1 + FWidth + FPosition;
                R.Bottom = FSourceY1 + FHeight;
                BltSDRectClipped(FSource1, R, Dest, X, Y);
                R.Left = FSourceX2 + FWidth + FPosition;
                R.Top = FSourceY2;
                R.Right = FWidth * 2 + FSourceX2;
                R.Bottom = FSourceY2 + FHeight;
                BltSDRectClipped(FSource2, R, Dest, X + FWidth + FPosition, Y);
                var call__4 = Math.PI;
                OriginalLength = Round((double)((double)call__4 * FRadius) * 0.25);
                var call__5 = Math.PI;
                ArcLength = Round((double)((double)call__5 * FRadius) * 0.25);
                SourceBase = FWidth + FPosition;
                RadiusFactor = (float)((double)((double)2 / Math.PI) * ArcLength);
                BaseX = X + FWidth + FPosition;
                Span = Round(FShadow);
                Limit = FWidth - FPosition;
                Step = (float)((double)1 / FShadow);
                CosValue = 0;
                SourceOffset = 0;
                I = 0;
                while (SourceOffset < Limit && I < Span)
                {
                    Angle = GetInvCos(CosValue);
                    SourceOffset = ArcLength - Round((double)RadiusFactor * Angle);
                    Offset = Round((double)Height - (double)Math.Sin(Angle) * Height);
                    if (SourceOffset < Limit)
                    {
                        if (ArcLength - SourceOffset + ArcLength < Limit)
                        {
                            BltVerticalScanLine(SourceBase + SourceOffset, X + FWidth + FPosition + I, Y - Offset, Dest, FHeight - Round(Width), FHeight);
                        }
                        else
                        {
                            BltVerticalScanLine(SourceBase + SourceOffset, BaseX + I, Y - Offset, Dest, 0, FHeight);
                        }
                    }

                    CosValue = (float)((double)CosValue + Step);
                    I += 1;
                }

                var call__6 = Math.PI;
                ArcLength = Round((double)((double)call__6 * FRadius) * 0.25);
                SourceBase = FWidth * 3 - FPosition - ArcLength;
                RadiusFactor = (float)((double)((double)2 / Math.PI) * ArcLength);
                BaseX = X + FWidth + FPosition;
                Span = Round(FShadow);
                I = Span;
                CosValue = 1;
                Limit = FWidth * 2 - SourceBase;
                while (SourceOffset >= Limit && I >= 0)
                {
                    Angle = GetInvCos(CosValue);
                    SourceOffset = -Round((double)RadiusFactor * Angle);
                    Offset = Round((double)((double)Math.Sin(Angle) * Height) + Height);
                    if (SourceOffset >= Limit)
                    {
                        BltVerticalScanLine(SourceBase + SourceOffset, BaseX + I, Y - Offset, Dest, 0, FHeight);
                    }

                    CosValue = (float)((double)CosValue - Step);
                    I -= 1;
                }

                var call__7 = Math.PI;
                ArcLength = Round((double)((double)((double)call__7 * FRadius) * 0.25) * FShade);
                SourceBase = FWidth * 3 - FPosition - OriginalLength * 2 - ArcLength;
                Span = Round((double)FShadow * FShade);
                BaseX = X + FWidth + FPosition - Span;
                Height = (float)((double)((double)FShadow * FScale) * 3.4142);
                Width = (float)((double)((double)FRadius * FScale) * 0.707);
                Limit = FWidth * 2 - SourceBase;
                SourceOffset = Round(ArcLength);
                I = Span;
                CosValue = 0;
                Step = (float)((double)0.707 / ((double)FShadow * FShade));
                if ((double)Step > 1)
                {
                    Step = (float)1;
                }

                while (SourceOffset >= Limit && I >= 0)
                {
                    Angle = GetInvCos(CosValue);
                    var call__8 = Math.PI;
                    var call__9 = Math.PI;
                    SourceOffset = Round((double)((double)ArcLength * ((double)Angle - (double)call__8 / 4)) * ((double)4 / call__9));
                    Offset = Round((double)((double)Math.Sin(Angle) * Height) - Width);
                    if (SourceOffset >= Limit)
                    {
                        BltVerticalScanLine(SourceBase + SourceOffset, BaseX + I, Y - Offset, Dest, 0, FHeight);
                    }

                    CosValue = (float)((double)CosValue + Step);
                    I -= 1;
                }

                BaseX = X + FWidth + FPosition - Span * 2;
                SourceOffset = 0;
                I = Span;
                while (SourceOffset >= Limit && I >= 0 && (double)CosValue > 0)
                {
                    CosValue = (float)((double)CosValue - Step);
                    Angle = GetInvCos(CosValue);
                    var call__10 = Math.PI;
                    var call__11 = Math.PI;
                    SourceOffset = -Round((double)((double)ArcLength * ((double)4 / call__10)) * ((double)Angle - (double)call__11 / 4));
                    Offset = Round((double)((double)1 - Math.Sin(Angle)) * Height);
                    if (SourceOffset >= Limit)
                    {
                        BltVerticalScanLine(SourceBase + SourceOffset, BaseX + I, Y - Offset, Dest, 0, FHeight);
                    }

                    I -= 1;
                }

                R.Left = FSourceX2;
                R.Top = FSourceY2;
                R.Right = FSourceX2 + FWidth - FPosition - Round((double)OriginalLength * ((double)((double)2 * FShade) + 2));
                R.Bottom = FSourceY2 + FHeight;
                if (R.Right > R.Left)
                {
                    BltSDRectClipped(FSource2, R, Dest, BaseX - (R.Right - R.Left), Y);
                }
            }
        }

        // PageTurnCtrl.pas:324
        public void RenderOpenCloseScene(int X, int Y, in TSurfaceDesc Dest)
        {
            int BaseX = default;
            int Height = default;
            int Width = default;
            float Step = default;
            float SourceX = default;
            int Offset = default;
            TRect R = default;
            int I = default;
            int PageWidth = default;
            R.Left = FSourceX2 + FWidth;
            R.Top = FSourceY2;
            R.Right = FSourceX2 + FWidth * 2;
            R.Bottom = FSourceY2 + FHeight;
            BltSDRectClipped(FSource2, R, Dest, X + FWidth, Y);
            var call__12 = Math.PI;
            var call__13 = Math.Sin((double)((double)FPosition * call__12) / 180);
            Height = Round((double)call__13 * FWidth);
            var call__14 = Math.PI;
            var call__15 = Math.Cos((double)((double)FPosition * call__14) / 180);
            Width = Round((double)call__15 * FWidth);
            if (Width > 0)
            {
                Step = (float)((double)FWidth / Width);
                BaseX = X + FWidth;
                PageWidth = FWidth;
                SourceX = PageWidth;
                {
                    I = 0;
                    int end__16 = Width - 1;
                    for (; I <= end__16; I += 1)
                    {
                        Offset = Round((double)((double)((double)I / Width) * Height) * FScale);
                        BltVerticalScanLine(Round(SourceX), BaseX + I, Y - Offset, Dest, 0, FHeight);
                        SourceX = (float)((double)SourceX + Step);
                    }
                }
            }
            else
            {
                if (Width < 0)
                {
                    Width = -Width;
                    Step = (float)((double)FWidth / Width);
                    BaseX = X + FWidth;
                    SourceX = FWidth * 3;
                    {
                        I = 0;
                        int end__17 = Width - 1;
                        for (; I <= end__17; I += 1)
                        {
                            Offset = Round((double)((double)((double)I / Width) * Height) * FScale);
                            BltVerticalScanLine(Round(SourceX), BaseX - I, Y - Offset, Dest, 0, FHeight);
                            SourceX = (float)((double)SourceX - Step);
                        }
                    }
                }
            }
        }

        // PageTurnCtrl.pas:361
        public void RenderScene(int X, int Y, in TSurfaceDesc Dest, int X1, int Y1, in TSurfaceDesc Source1, int X2, int Y2, in TSurfaceDesc Source2, ref TRect R)
        {
            switch (FMode)
            {
                case TPageTurnMode.ptLeft:
                    FSource1 = Source1;
                    FSourceX1 = X1;
                    FSourceY1 = Y1;
                    FSource2 = Source2;
                    FSourceX2 = X2;
                    FSourceY2 = Y2;
                    RenderLeftRightScene(X, Y, Dest);
                    R.Left = X;
                    R.Right = X + FWidth * 2;
                    R.Top = Y - (Round((double)FScale * FRadius) + 1);
                    R.Bottom = Y + FHeight;
                    break;
                case TPageTurnMode.ptRight:
                    FSource1 = Source2;
                    FSourceX1 = X2;
                    FSourceY1 = Y2;
                    FSource2 = Source1;
                    FSourceX2 = X1;
                    FSourceY2 = Y1;
                    RenderLeftRightScene(X, Y, Dest);
                    R.Left = X;
                    R.Right = X + FWidth * 2;
                    R.Top = Y - (Round((double)FScale * FRadius) + 1);
                    R.Bottom = Y + FHeight;
                    break;
                case TPageTurnMode.ptOpen:
                    FSource1 = Source1;
                    FSourceX1 = X1;
                    FSourceY1 = Y1;
                    FSource2 = Source2;
                    FSourceX2 = X2;
                    FSourceY2 = Y2;
                    RenderOpenCloseScene(X, Y, Dest);
                    R.Left = X;
                    R.Right = X + FWidth * 2;
                    var call__18 = Math.PI;
                    var call__19 = Math.Sin((double)((double)FPosition * call__18) / 180);
                    R.Top = Y - Round((double)((double)((double)call__19 * FScale) * FWidth) + 1);
                    R.Bottom = Y + FHeight;
                    break;
                case TPageTurnMode.ptClose:
                    FSource1 = Source2;
                    FSourceX1 = X2;
                    FSourceY1 = Y2;
                    FSource2 = Source1;
                    FSourceX2 = X1;
                    FSourceY2 = Y1;
                    RenderOpenCloseScene(X, Y, Dest);
                    R.Left = X;
                    R.Right = X + FWidth * 2;
                    var call__20 = Math.PI;
                    var call__21 = Math.Sin((double)((double)FPosition * call__20) / 180);
                    R.Top = Y - Round((double)((double)((double)call__21 * FScale) * FWidth) + 1);
                    R.Bottom = Y + FHeight;
                    break;
            }
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
                    ushort value__22 = MemoryMarshal.Read<ushort>(BytePointer(Source).AsSpan(2));
                    MemoryMarshal.Write(BytePointer(Dest).AsSpan(2), in value__22);
                }

                Source = (BytePointer(Source) + SourcePitch).Box();
                Dest = (BytePointer(Dest) + DestPitch).Box();
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
