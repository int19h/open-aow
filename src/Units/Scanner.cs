// Translated from Scanner.pas.
using System.Runtime.CompilerServices;
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWControl;
using Aow.Units.AoWE;
using Aow.Units.AOWInterfaceImgLib;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using Aow.Units.ImageLib;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.GFXE.GFXEUnit;

namespace Aow.Units.Scanner;

// { ot=1 }
// half-row inset, libraries, icons, viewport, change event, update steps, map, frame library,
// frame index, map dimensions, level, icons drawn, previous size, drawable height, terrain/icon
// surfaces, update step, incremental
public partial class TScanner : TAOWControl
{
    public int FHalfRowInset;
    public TImageLibrary FTilesImageLib;
    public TImageLibrary FHighlightImageLib;
    public TImageLibrary FFogImageLib;
    public TImageLibrary FObjectsImageLib;
    public int FTTop;
    public int FTHeight;
    public int FTLeft;
    public int FTWidth;
    public bool FReserved138;
    public bool FBuildings;
    public bool FArmies;
    public bool FNaturals;
    public TNotifyEvent<TScanner> FOnMapPositionChange;
    public int FSteps;
    public AoWE.TAbstractAoWHSMap FMap;
    public TInterfaceIL FILFrame;
    public int FILIndexFrame;
    public int FMapWidth;
    public int FMapHeight;
    public TMapLevel FLevel;
    public bool FObjectsDrawn;
    public int FPreviousWidth;
    public int FPreviousHeight;
    public int FDrawableHeight;
    public TSurface FMapSurface;
    public TSurface FObjectSurface;
    public int FUpdateStep;
    public bool FIncremental;

    // Scanner.pas:126
    public override void Destroy()
    {
        if (FMapSurface != null)
        {
            FMapSurface.Destroy();
        }

        if (FObjectSurface != null)
        {
            FObjectSurface.Destroy();
        }

        base.Destroy();
    }

    // Scanner.pas:116
    public TScanner(TComponent owner) : base(owner)
    {
        FSteps = 1;
        FTLeft = -1;
        FTTop = -1;
        FTWidth = -1;
        FTHeight = -1;
    }

    // Scanner.pas:398
    public override void Validate()
    {
        TInterfaceIL Frame;
        Frame = FILFrame;
        if (Frame != null)
        {
            FILFrame = Frame.CheckIL(FILIndexFrame, 8, this);
        }

        if (FILFrame != null && FTilesImageLib != null && FObjectsImageLib != null && FMap != null && FFogImageLib != null && FHighlightImageLib != null)
        {
            FValid = true;
        }

        base.Validate();
    }

    // Scanner.pas:409
    public override void SetSize()
    {
        TSurfaceDesc SD = default;
        if ((FWinWidth != FPreviousWidth || FWinHeight != FPreviousHeight) && FWinWidth > 0 && FWinHeight > 0)
        {
            FHalfRowInset = FWinHeight / FMapHeight / 2;
            FDrawableHeight = FWinHeight - FHalfRowInset;
            if (FMapSurface != null)
            {
                FObjectSurface.Resize(FWinWidth, FWinHeight);
                FObjectSurface.Cls(65280);
                FMapSurface.Resize(FWinWidth, FWinHeight);
                FMapSurface.Lock(ref SD);
                for (int X = 0, last = FWinWidth / FFogImageLib[0].Width; X <= last; X++)
                {
                    for (int Y = 0, lastY = FWinHeight / FFogImageLib[0].Height; Y <= lastY; Y++)
                    {
                        var fogImageLibHeight = FFogImageLib[0];
                        var fogImageLibWidth = FFogImageLib[0];
                        FFogImageLib[0].ShowClipped(X * fogImageLibWidth.Width, Y * fogImageLibHeight.Height, SD);
                    }
                }

                FMapSurface.Unlock();
                if (!FIncremental)
                {
                    DrawMap(0, 0, FMapWidth, FMapHeight);
                    FUpdateStep = FSteps;
                }
                else
                {
                    FUpdateStep = -1;
                }
            }

            FPreviousWidth = FWinWidth;
            FPreviousHeight = FWinHeight;
        }

        base.SetSize();
    }

    // Scanner.pas:325
    public override void Update()
    {
        base.Update();
        if (FVisible && FMapSurface != null)
        {
            if (!FIncremental)
            {
                FIncremental = true;
            }

            if (FUpdateStep < FSteps)
            {
                if (FUpdateStep > -1)
                {
                    DrawMap(0, FMapHeight * FUpdateStep / FSteps, FMapWidth, FMapHeight / FSteps + 1);
                }

                FUpdateStep += 1;
            }
        }
    }

    // Scanner.pas:445
    public override void Draw(TSurfaceDesc SurfDesc)
    {
        TRect R = default;
        TSurfaceDesc SD = default;
        try
        {
            if (FValid && FVisible)
            {
                if (FOnPreDraw != null)
                {
                    FOnPreDraw(this, SurfDesc);
                }

                if (FMapSurface != null && FMapSurface.PixelFormatID != SurfDesc.PixelFormatID)
                {
                    FObjectSurface.Free();
                    FMapSurface.Free();
                    FMapSurface = null;
                }

                if (FMapSurface == null)
                {
                    FObjectSurface = new TDataSurface(FWinWidth, FWinHeight, SurfDesc.PixelFormatID, null);
                    FObjectSurface.Cls(65280);
                    FMapSurface = new TDataSurface(FWinWidth, FWinHeight, SurfDesc.PixelFormatID, null);
                    FMapSurface.Lock(ref SD);
                    for (int X = 0, last = FWinWidth / FFogImageLib[0].Width; X <= last; X++)
                    {
                        for (int Y = 0, lastY = FWinHeight / FFogImageLib[0].Height; Y <= lastY; Y++)
                        {
                            var fogImageLibHeight = FFogImageLib[0];
                            var fogImageLibWidth = FFogImageLib[0];
                            FFogImageLib[0].ShowClipped(X * fogImageLibWidth.Width, Y * fogImageLibHeight.Height, SD);
                        }
                    }

                    FMapSurface.Unlock();
                    if (!FIncremental)
                    {
                        DrawMap(0, 0, FMapWidth, FMapHeight);
                        FUpdateStep = FSteps;
                    }
                    else
                    {
                        FUpdateStep = -1;
                    }
                }

                if (!FObjectsDrawn)
                {
                    DrawObjects();
                }

                R = Rect(0, 0, FWinWidth, FWinHeight);
                SD = SurfDesc;
                var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FAOWWindow.FWinLeft + FWinLeft + FWinWidth, FAOWWindow.FWinTop + FWinTop + FWinHeight);
                SD.ClipRect = rect;
                FMapSurface.Lock(ref SurfDesc);
                BltSDRectClipped(SurfDesc, R, SD, FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop);
                FMapSurface.Unlock();
                FObjectSurface.Lock(ref SurfDesc);
                BltSpriteClipped16(SurfDesc, R, SD, FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, 65280);
                FObjectSurface.Unlock();
                if (FMapWidth > 0 && FMapHeight > 0)
                {
                    var rect2 = Rect(FAOWWindow.FWinLeft + FWinLeft + FTLeft * FWinWidth / FMapWidth, FAOWWindow.FWinTop + FWinTop + FTTop * FWinHeight / FMapHeight, FTWidth * FWinWidth / FMapWidth, (FTHeight + 2) * FWinHeight / FMapHeight);
                    FILFrame.DrawILF(FILIndexFrame, rect2, ref SD);
                }

                if (FOnDraw != null)
                {
                    FOnDraw(this, SD);
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // Scanner.pas:449
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        int MapY;
        int OldLeft;
        int OldTop;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FMouseDown = true;
            X = (X - FAOWWindow.FWinLeft - FWinLeft) * FMapWidth / FWinWidth;
            MapY = (Y - FAOWWindow.FWinTop - FWinTop) * FMapHeight / FDrawableHeight;
            OldLeft = FTLeft;
            OldTop = FTTop;
            FTLeft = X - FTWidth / 2;
            FTTop = MapY - FTHeight / 2;
            if (FTTop != OldTop || OldLeft != FTLeft)
            {
                FDrawn = false;
                if (FOnMapPositionChange != null)
                {
                    FOnMapPositionChange(this);
                }
            }
        }
    }

    // Scanner.pas:452
    public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }

        FMouseDown = false;
    }

    // Scanner.pas:455
    public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        int MapX = default;
        int MapY = default;
        int OldLeft = default;
        int OldTop = default;
        base.CheckMouseMove(Shift, X, Y);
        FHandled = false;
        if (X >= FAOWWindow.FWinLeft + FWinLeft && X <= FAOWWindow.FWinLeft + FWinLeft + FWinWidth - 1 && Y >= FAOWWindow.FWinTop + FWinTop && Y <= FAOWWindow.FWinTop + FWinTop + FWinHeight - 1)
        {
            FHandled = true;
            if (FMouseDown == true)
            {
                OldLeft = FTLeft;
                OldTop = FTTop;
                MapX = (X - FAOWWindow.FWinLeft - FWinLeft) * FMapWidth / FWinWidth;
                MapY = (Y - FAOWWindow.FWinTop - FWinTop) * FMapHeight / FDrawableHeight;
                FTLeft = MapX - FTWidth / 2;
                FTTop = MapY - FTHeight / 2;
                if (FTTop != OldTop || FTLeft != OldLeft)
                {
                    FDrawn = false;
                    if (FOnMapPositionChange != null)
                    {
                        FOnMapPositionChange(this);
                    }
                }
            }

            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X, Y);
            }
        }
    }

    // Scanner.pas:142
    public virtual void SetILFrame(TInterfaceIL Value)
    {
        if (Value != FILFrame)
        {
            FValid = false;
            FILFrame = Value;
        }
    }

    // Scanner.pas:133
    public void SetILFIndex(int Value)
    {
        if (Value != FILIndexFrame)
        {
            FILIndexFrame = Value;
            FValid = false;
        }
    }

    // Scanner.pas:151
    public void SetObjIL(TImageLibrary Value)
    {
        if (Value != FObjectsImageLib)
        {
            FValid = false;
            FObjectsImageLib = Value;
        }
    }

    // Scanner.pas:160
    public void SetArmies(bool Value)
    {
        if (Value != FArmies)
        {
            FArmies = Value;
            SetObDrawn(false);
            FDrawn = false;
        }
    }

    // Scanner.pas:170
    public void SetBuildings(bool Value)
    {
        if (Value != FBuildings)
        {
            FBuildings = Value;
            SetObDrawn(false);
            FDrawn = false;
        }
    }

    // Scanner.pas:180
    public void SetNaturals(bool Value)
    {
        if (Value != FNaturals)
        {
            FNaturals = Value;
            SetObDrawn(false);
            FDrawn = false;
        }
    }

    // Scanner.pas:190
    public void SetLevel(TMapLevel Value)
    {
        FLevel = Value;
        if (AoWHSMap.FCombatMap != null)
        {
            FMapWidth = FLevel.Width - 16;
            FMapHeight = FLevel.Height - 12;
        }
        else
        {
            FMapWidth = FLevel.Width - 12;
            FMapHeight = FLevel.Height - 12;
        }
    }

    // Scanner.pas:205
    public void SetMapWidth(int Value)
    {
        if (AoWHSMap.FCombatMap != null)
        {
            FMapWidth = Value - 16;
        }
        else
        {
            FMapWidth = Value - 12;
        }
    }

    // Scanner.pas:211
    public void SetMapHeight(int Value)
    {
        if (AoWHSMap.FCombatMap != null)
        {
            FMapHeight = Value - 12;
        }
        else
        {
            // { Both original branches subtract 12. }
            FMapHeight = Value - 12;
        }
    }

    // Scanner.pas:217
    public byte GetTerrain(int X, int Y)
    {
        byte Result;
        if (AoWHSMap.FCombatMap != null)
        {
            Result = (byte)FLevel.GetField(X + 8, Y + 6).TerrainType;
        }
        else
        {
            Result = (byte)FLevel.GetField(X + 6, Y + 6).TerrainType;
        }

        return Result;
    }

    // Scanner.pas:223
    public void SetTerrain(int X, int Y, int Value)
    {
    }

    // Scanner.pas:226
    public int GetOverlay(int X, int Y)
    {
        int Result;
        if (AoWHSMap.FCombatMap != null)
        {
            switch (FLevel.GetField(X + 8, Y + 6).GetDecorationType())
            {
                case 1:
                case 2:
                case 3:
                case 9:
                    Result = 1;
                    break;
                case 4:
                case 5:
                    Result = 9;
                    break;
                case 6:
                case 7:
                case 8:
                case >= 14 and <= 18:
                    Result = 8;
                    break;
                case 10:
                    Result = 3;
                    break;
                case 11:
                case 12:
                    Result = 6;
                    break;
                case 13:
                    Result = 2;
                    break;
                default:
                    Result = FLevel.GetField(X + 8, Y + 6).OverlayType;
                    break;
            }
        }
        else
        {
            Result = FLevel.GetField(X + 6, Y + 6).OverlayType;
        }

        return Result;
    }

    // Scanner.pas:243
    public void SetOverlay(int X, int Y, int Value)
    {
    }

    // Scanner.pas:246
    public byte GetExplored(int X, int Y)
    {
        byte Result;
        TAoWMapField Field;
        if (AoWHSMap.FCombatMap != null)
        {
            Field = (TAoWMapField)FLevel.GetField(X + 8, Y + 6);
        }
        else
        {
            Field = (TAoWMapField)FLevel.GetField(X + 6, Y + 6);
        }

        if (FMap.FExplorationEnabled)
        {
            if (Field.GetExplored(FMap.FSeatedPlayer))
            {
                if (FMap.FFogEnabled)
                {
                    if (Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref Field.FFieldFlags).Hi).Lo == 0)
                    {
                        Result = 2;
                    }
                    else
                    {
                        Result = 1;
                    }
                }
                else
                {
                    Result = 1;
                }
            }
            else
            {
                Result = 0;
            }
        }
        else
        {
            if (FMap.FFogEnabled)
            {
                if (Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref Field.FFieldFlags).Hi).Lo == 0)
                {
                    Result = 2;
                }
                else
                {
                    Result = 1;
                }
            }
            else
            {
                Result = 1;
            }
        }

        return Result;
    }

    // Scanner.pas:277
    public void SetExplored(int X, int Y, int Value)
    {
    }

    // Scanner.pas:280
    public int GetObject(int X, int Y)
    {
        int Result;
        if (AoWHSMap.FCombatMap != null)
        {
            Result = ((TAoWMapField)FLevel.GetField(X + 8, Y + 6)).GetScannerIcon();
        }
        else
        {
            Result = ((TAoWMapField)FLevel.GetField(X + 6, Y + 6)).GetScannerIcon();
        }

        return Result;
    }

    // Scanner.pas:287
    public byte GetObjectSize() => 1;

    // Scanner.pas:292
    public void SetObjectSize(int X, int Y, int Value)
    {
    }

    // Scanner.pas:295
    public void SetImgLib(TImageLibrary Value)
    {
        FTilesImageLib = Value;
        FValid = false;
    }

    // Scanner.pas:301
    public void SetHimgLib(TImageLibrary Value)
    {
        FHighlightImageLib = Value;
        FValid = false;
    }

    // Scanner.pas:307
    public void SetFogimgLib(TImageLibrary Value)
    {
        FFogImageLib = Value;
        FValid = false;
    }

    // Scanner.pas:313
    public void SetSteps(int Value)
    {
        FSteps = Value;
        if (FSteps < 1)
        {
            FSteps = 1;
        }
    }

    // Scanner.pas:319
    public void SetObDrawn(bool Value)
    {
        FObjectsDrawn = Value;
        if (!FObjectsDrawn)
        {
            FDrawn = false;
        }
    }

    // Scanner.pas:341
    public void SetTLeft(int Value)
    {
        if (Value != FTLeft)
        {
            FDrawn = false;
            FTLeft = Value;
        }
    }

    // Scanner.pas:350
    public void SetTTop(int Value)
    {
        if (Value != FTTop)
        {
            FDrawn = false;
            FTTop = Value;
        }
    }

    // Scanner.pas:359
    public void SetTWidth(int Value)
    {
        if (Value != FTWidth)
        {
            FDrawn = false;
            FTWidth = Value;
        }
    }

    // Scanner.pas:368
    public void SetTHeight(int Value)
    {
        if (Value != FTHeight)
        {
            FDrawn = false;
            FTHeight = Value;
        }
    }

    // Scanner.pas:377
    public void SetMap(AoWE.TAbstractAoWHSMap Value)
    {
        FMap = Value;
        if (FMap != null)
        {
            FLevel = FMap.MapContainer.GetMapLevel(0);
            if (AoWHSMap.FCombatMap != null)
            {
                FMapWidth = FLevel.Width - 16;
                FMapHeight = FLevel.Height - 12;
            }
            else
            {
                FMapWidth = FLevel.Width - 12;
                FMapHeight = FLevel.Height - 12;
            }

            FIncremental = false;
            FSized = false;
        }
    }

    // Scanner.pas:440
    public void DrawMap(int X, int Y, int Width, int Height)
    {
        int ImageIndex;
        int Overlay;
        int Explored;
        int YOffset;
        int Terrain;
        int OverlayIndex;
        TSurfaceDesc SD = default;
        try
        {
            if (FValid && FMapSurface != null && FVisible)
            {
                if (X < 0)
                {
                    Width += X;
                    X = 0;
                }

                if (Y < 0)
                {
                    Height += Y;
                    Y = 0;
                }

                if (X == X / 2 * 2)
                {
                    YOffset = 0;
                }
                else
                {
                    YOffset = FHalfRowInset;
                }

                if (FMapSurface.Lock(ref SD) >= 0)
                {
                    if (FMapWidth < Width + X)
                    {
                        Width = FMapWidth - X;
                    }

                    if (FMapHeight < Y + Height)
                    {
                        Height = FMapHeight - Y;
                    }

                    for (int MapX = X; MapX <= Width + X - 1; MapX++)
                    {
                        for (int MapY = Y; MapY <= Y + Height - 1; MapY++)
                        {
                            var rect = Rect(FWinWidth * MapX / FMapWidth, FDrawableHeight * MapY / FMapHeight + YOffset, (MapX + 1) * FWinWidth / FMapWidth, (MapY + 1) * FDrawableHeight / FMapHeight + YOffset);
                            SD.ClipRect = rect;
                            Explored = GetExplored(MapX, MapY);
                            if (Explored == 0)
                            {
                                for (int TileX = FWinWidth * MapX / FMapWidth / FFogImageLib[0].Width, last = (MapX + 1) * FWinWidth / FMapWidth / FFogImageLib[0].Width; TileX <= last; TileX++)
                                {
                                    for (int TileY = (FDrawableHeight * MapY / FMapHeight + YOffset) / FFogImageLib[0].Height, lastTileY = ((MapY + 1) * FDrawableHeight / FMapHeight + YOffset) / FFogImageLib[0].Height; TileY <= lastTileY; TileY++)
                                    {
                                        var fogImageLibHeight = FFogImageLib[0];
                                        var fogImageLibWidth = FFogImageLib[0];
                                        FFogImageLib[0].ShowClipped(TileX * fogImageLibWidth.Width, TileY * fogImageLibHeight.Height, SD);
                                    }
                                }
                            }
                            else
                            {
                                Terrain = GetTerrain(MapX, MapY);
                                Overlay = GetOverlay(MapX, MapY);
                                if (Overlay > 11 || Overlay == 7)
                                {
                                    OverlayIndex = 0;
                                }
                                else
                                {
                                    OverlayIndex = Overlay + 1;
                                }

                                ImageIndex = OverlayIndex * 16 + Terrain;
                                if (Terrain == 15)
                                {
                                    for (int TileX = FWinWidth * MapX / FMapWidth / FFogImageLib[0].Width, last = (MapX + 1) * FWinWidth / FMapWidth / FFogImageLib[0].Width; TileX <= last; TileX++)
                                    {
                                        for (int TileY = (FDrawableHeight * MapY / FMapHeight + YOffset) / FFogImageLib[0].Height, lastTileY = ((MapY + 1) * FDrawableHeight / FMapHeight + YOffset) / FFogImageLib[0].Height; TileY <= lastTileY; TileY++)
                                        {
                                            var fogImageLibHeight = FFogImageLib[0];
                                            var fogImageLibWidth = FFogImageLib[0];
                                            FFogImageLib[0].ShowClipped(TileX * fogImageLibWidth.Width, TileY * fogImageLibHeight.Height, SD);
                                        }
                                    }
                                }
                                else
                                {
                                    if (ImageIndex < FTilesImageLib.Count && FTilesImageLib[ImageIndex] != null)
                                    {
                                        for (int TileX = FWinWidth * MapX / FMapWidth / FTilesImageLib[ImageIndex].Width, last = (MapX + 1) * FWinWidth / FMapWidth / FTilesImageLib[ImageIndex].Width; TileX <= last; TileX++)
                                        {
                                            for (int TileY = (FDrawableHeight * MapY / FMapHeight + YOffset) / FTilesImageLib[ImageIndex].Height, lastTileY = ((MapY + 1) * FDrawableHeight / FMapHeight + YOffset) / FTilesImageLib[ImageIndex].Height; TileY <= lastTileY; TileY++)
                                            {
                                                var tilesImageLibHeight = FTilesImageLib[ImageIndex];
                                                var tilesImageLibWidth = FTilesImageLib[ImageIndex];
                                                FTilesImageLib[ImageIndex].ShowClipped(TileX * tilesImageLibWidth.Width, TileY * tilesImageLibHeight.Height, SD);
                                            }
                                        }
                                    }

                                    if ((Overlay == 0 || Overlay == 10) && FReserved138 == true && MapY < FMapHeight - 1 && MapX > 0 && GetOverlay(MapX - 1, MapY + 1) != Overlay)
                                    {
                                        FHighlightImageLib[1].ShowClipped(SD.ClipRect.Left, SD.ClipRect.Top, SD);
                                    }

                                    if ((Overlay == 0 || Overlay == 10) && FReserved138 == true && MapY > 0 && MapX < FMapWidth - 1 && GetOverlay(MapX + 1, MapY - 1) != Overlay)
                                    {
                                        FHighlightImageLib[0].ShowClipped(SD.ClipRect.Left, SD.ClipRect.Top, SD);
                                    }

                                    if (Explored == 2)
                                    {
                                        for (int TileX = FWinWidth * MapX / FMapWidth / FFogImageLib[1].Width, last = (MapX + 1) * FWinWidth / FMapWidth / FFogImageLib[1].Width; TileX <= last; TileX++)
                                        {
                                            for (int TileY = (FDrawableHeight * MapY / FMapHeight + YOffset) / FFogImageLib[1].Height, lastTileY = ((MapY + 1) * FDrawableHeight / FMapHeight + YOffset) / FFogImageLib[1].Height; TileY <= lastTileY; TileY++)
                                            {
                                                var fogImageLibHeight = FFogImageLib[1];
                                                var fogImageLibWidth = FFogImageLib[1];
                                                FFogImageLib[1].ShowClipped(TileX * fogImageLibWidth.Width, TileY * fogImageLibHeight.Height, SD);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (YOffset == 0)
                        {
                            YOffset = FHalfRowInset;
                        }
                        else
                        {
                            YOffset = 0;
                        }
                    }

                    FMapSurface.Unlock();
                    SetObDrawn(false);
                    FDrawn = false;
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during DrawMap", "Error - " + Name, 0);
        }
    }

    // Scanner.pas:443
    public void DrawObjects()
    {
        int ImageIndex;
        int YOffset;
        int Army;
        int Natural;
        int Building;
        int Obj;
        TSurfaceDesc SD = default;
        if (FObjectSurface != null)
        {
            FObjectSurface.Cls(65280);
            YOffset = 0;
            if (FObjectSurface.Lock(ref SD) >= 0)
            {
                for (int X = 0, last = FMapWidth - 1; X <= last; X++)
                {
                    for (int Y = 0, lastY = FMapHeight - 1; Y <= lastY; Y++)
                    {
                        if (GetExplored(X, Y) == 1)
                        {
                            Obj = GetObject(X, Y);
                            if (Obj != 0)
                            {
                                Army = Obj & 255;
                                Natural = Obj & 65280;
                                Building = Obj & 16711680;
                                if (Building > 0 && FBuildings)
                                {
                                    ImageIndex = 7 - (((int)(((uint)Building) >> 22)) & 3) + (((int)(((uint)Building) >> 16)) & 63) * 6;
                                    if (ImageIndex < FObjectsImageLib.Count && ImageIndex > -1 && FObjectsImageLib[ImageIndex] != null)
                                    {
                                        var objectsImageLibHeight = FObjectsImageLib[ImageIndex];
                                        var objectsImageLibWidth = FObjectsImageLib[ImageIndex];
                                        FObjectsImageLib[ImageIndex].ShowClipped(FWinWidth / (FMapWidth * 2) + X * FWinWidth / FMapWidth - objectsImageLibWidth.Width / 2, FWinHeight / (FMapHeight * 2) + Y * FWinHeight / FMapHeight - objectsImageLibHeight.Height / 2 + YOffset, SD);
                                    }
                                }
                                else
                                {
                                    if (Natural > 0 && FNaturals)
                                    {
                                        ImageIndex = (int)(((uint)Natural) >> 8) * 6 + 9;
                                        if (ImageIndex < FObjectsImageLib.Count && ImageIndex > -1 && FObjectsImageLib[ImageIndex] != null)
                                        {
                                            var objectsImageLibHeight = FObjectsImageLib[ImageIndex];
                                            var objectsImageLibWidth = FObjectsImageLib[ImageIndex];
                                            FObjectsImageLib[ImageIndex].ShowClipped(FWinWidth / (FMapWidth * 2) + X * FWinWidth / FMapWidth - objectsImageLibWidth.Width / 2, FWinHeight / (FMapHeight * 2) + Y * FWinHeight / FMapHeight - objectsImageLibHeight.Height / 2 + YOffset, SD);
                                        }
                                    }
                                }

                                if (Army > 0 && FArmies)
                                {
                                    ImageIndex = Army * 6 + 8;
                                    if (ImageIndex < FObjectsImageLib.Count && ImageIndex > -1 && FObjectsImageLib[ImageIndex] != null)
                                    {
                                        var objectsImageLibHeight = FObjectsImageLib[ImageIndex];
                                        var objectsImageLibWidth = FObjectsImageLib[ImageIndex];
                                        FObjectsImageLib[ImageIndex].ShowClipped(FWinWidth / (FMapWidth * 2) + X * FWinWidth / FMapWidth - objectsImageLibWidth.Width / 2, FWinHeight / (FMapHeight * 2) + Y * FWinHeight / FMapHeight - objectsImageLibHeight.Height / 2 + YOffset, SD);
                                    }
                                }
                            }
                        }
                    }

                    if (YOffset == 0)
                    {
                        YOffset = FHalfRowInset;
                    }
                    else
                    {
                        YOffset = 0;
                    }
                }

                FObjectSurface.Unlock();
                FDrawn = false;
                SetObDrawn(true);
            }
        }
    }

    [Published]
    public TImageLibrary TilesImageLib { get => FTilesImageLib; set => SetImgLib(value); }

    [Published]
    public TImageLibrary HiLightsImageLib { get => FHighlightImageLib; set => SetHimgLib(value); }

    [Published]
    public TImageLibrary FogImageLib { get => FFogImageLib; set => SetFogimgLib(value); }

    [Published]
    public TImageLibrary ILObjects { get => FObjectsImageLib; set => SetObjIL(value); }

    [Published]
    public int TileBoxTop { get => FTTop; set => SetTTop(value); }

    [Published]
    public int TileBoxLeft { get => FTLeft; set => SetTLeft(value); }

    [Published]
    public int TileBoxWidth { get => FTWidth; set => SetTWidth(value); }

    [Published]
    public int TileBoxHeight { get => FTHeight; set => SetTHeight(value); }

    [Published]
    public bool HighLights { get => FReserved138; set => FReserved138 = value; }

    [Published]
    public bool Buildings { get => FBuildings; set => SetBuildings(value); }

    [Published]
    public bool Armies { get => FArmies; set => SetArmies(value); }

    [Published]
    public bool Naturals { get => FNaturals; set => SetNaturals(value); }

    [Published]
    public TNotifyEvent<TScanner> OnChangeTileBox { get => FOnMapPositionChange; set => FOnMapPositionChange = value; }

    [Published]
    public int MapDrawSteps { get => FSteps; set => SetSteps(value); }

    [Published]
    public TInterfaceIL ILFrame { get => FILFrame; set => SetILFrame(value); }

    [Published]
    public int ILIndexFrame { get => FILIndexFrame; set => SetILFIndex(value); }

    public new TNotifyEvent<TScanner> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new TMouseEvent<TScanner> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new TMouseEvent<TScanner> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new TMouseMoveEvent<TScanner> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new TKeyPressEvent<TScanner> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new TKeyEvent<TScanner> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new TAoWOnDrawEvent<TScanner> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new TAoWOnDrawEvent<TScanner> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new TNotifyEvent<TScanner> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}

public enum TOType : byte
{
    otBuilding = 0,
    otArmy = 1,
    otNatural = 2
}

public static partial class ScannerUnit
{
    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
