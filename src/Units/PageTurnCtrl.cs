// Translated from PageTurnCtrl.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using System.Runtime.InteropServices;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXEUnit;

namespace Aow.Units.PageTurnCtrl;

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
        float Result;
        Assert((double)Value >= -1 && (double)Value <= 1, "", "PageTurnCtrl.pas", 85);
        if ((double)Value >= 0)
        {
            Result = FInvCos[Round((double)Value * 511)];
        }
        else
        {
            var pi = Math.PI;
            Result = (float)((double)pi - FInvCos[Round((double)-Value * 511)]);
        }

        return Result;
    }

    // PageTurnCtrl.pas:90
    public void GenerateInvCosTable()
    {
        float CosValue;
        float Angle;
        float Target;
        int I;
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
        int SourceY;
        int Height;
        int Y;
        object SourceP;
        object DestP;
        int SourcePitch;
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
                PageTurnCtrlUnit.BltColumn(SourceP, SourcePitch, DestP, Dest.Pitch, Height);
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
        bool Result;
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
                        var pi = Math.PI;
                        FRadius = (float)((double)FRadius - (double)((double)((double)FSpeed * 1.25) * 2) / pi);
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
                    var pi = Math.PI;
                    FRadius = (float)((double)FRadius + (double)((double)((double)FSpeed * 1.5) * 2) / pi);
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
        int OriginalLength;
        int Offset;
        int BaseX;
        int Span;
        float Step;
        float Angle;
        float CosValue;
        float Height;
        float Width;
        float RadiusFactor;
        int ArcLength;
        int SourceBase;
        int Limit;
        TRect R = default;
        int SourceOffset;
        int I;
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
            var pi = Math.PI;
            OriginalLength = Round((double)((double)pi * FRadius) * 0.25);
            var mathPI = Math.PI;
            ArcLength = Round((double)((double)mathPI * FRadius) * 0.25);
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

            var pi2 = Math.PI;
            ArcLength = Round((double)((double)pi2 * FRadius) * 0.25);
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

            var pi3 = Math.PI;
            ArcLength = Round((double)((double)((double)pi3 * FRadius) * 0.25) * FShade);
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
                var pi4 = Math.PI;
                var pi5 = Math.PI;
                SourceOffset = Round((double)((double)ArcLength * ((double)Angle - (double)pi4 / 4)) * ((double)4 / pi5));
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
                var pi4 = Math.PI;
                var pi5 = Math.PI;
                SourceOffset = -Round((double)((double)ArcLength * ((double)4 / pi4)) * ((double)Angle - (double)pi5 / 4));
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
        int BaseX;
        int Height;
        int Width;
        float Step;
        float SourceX;
        int Offset;
        TRect R = default;
        int PageWidth;
        R.Left = FSourceX2 + FWidth;
        R.Top = FSourceY2;
        R.Right = FSourceX2 + FWidth * 2;
        R.Bottom = FSourceY2 + FHeight;
        BltSDRectClipped(FSource2, R, Dest, X + FWidth, Y);
        var pi = Math.PI;
        Height = Round((double)Math.Sin((double)((double)FPosition * pi) / 180) * FWidth);
        var mathPI = Math.PI;
        Width = Round((double)Math.Cos((double)((double)FPosition * mathPI) / 180) * FWidth);
        if (Width > 0)
        {
            Step = (float)((double)FWidth / Width);
            BaseX = X + FWidth;
            PageWidth = FWidth;
            SourceX = PageWidth;
            for (int I = 0; I <= Width - 1; I++)
            {
                Offset = Round((double)((double)((double)I / Width) * Height) * FScale);
                BltVerticalScanLine(Round(SourceX), BaseX + I, Y - Offset, Dest, 0, FHeight);
                SourceX = (float)((double)SourceX + Step);
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
                for (int I = 0; I <= Width - 1; I++)
                {
                    Offset = Round((double)((double)((double)I / Width) * Height) * FScale);
                    BltVerticalScanLine(Round(SourceX), BaseX - I, Y - Offset, Dest, 0, FHeight);
                    SourceX = (float)((double)SourceX - Step);
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
                var pi = Math.PI;
                var sin = Math.Sin((double)((double)FPosition * pi) / 180);
                R.Top = Y - Round((double)((double)((double)sin * FScale) * FWidth) + 1);
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
                var mathPI = Math.PI;
                var mathSin = Math.Sin((double)((double)FPosition * mathPI) / 180);
                R.Top = Y - Round((double)((double)((double)mathSin * FScale) * FWidth) + 1);
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

public static partial class PageTurnCtrlUnit
{
    // PageTurnCtrl.pas:108
    public static void BltColumn(object Source, int SourcePitch, object Dest, int DestPitch, int Height)
    {
        while (Height > 0)
        {
            ushort value__22 = MemoryMarshal.Read<ushort>(BytePointer(Source).AsSpan(2));
            MemoryMarshal.Write(BytePointer(Dest).AsSpan(2), in value__22);

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
