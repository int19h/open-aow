// Generated from Scanner.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
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
using Aow.Units.AoWControl;
using static Aow.Units.AoWControl.AoWControl;
using Aow.Units.AOWInterfaceImgLib;
using static Aow.Units.AOWInterfaceImgLib.AOWInterfaceImgLib;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Scanner
{
    // { ot=1 }
    // { TScanner: instance size $184; $114 half-row inset, $118/$11C/$120 libraries,
//     $124 icons, $128..$134 viewport, $13C change event, $144 update steps,
//     $148 map, $14C frame library, $150 frame index, $154/$158 map dimensions,
//     $15C level, $160 icons drawn, $164/$168 previous size,
//     $170 drawable height, $174/$178 terrain/icon surfaces, $17C update step, $180 incremental. }
    public partial class TScanner : Aow.Units.AoWControl.TAOWControl
    {
        public int FHalfRowInset;
        // { $114 }
        public Aow.Units.ImageLib.TImageLibrary FTilesImageLib;
        // { $118 }
        public Aow.Units.ImageLib.TImageLibrary FHighlightImageLib;
        // { $11C }
        public Aow.Units.ImageLib.TImageLibrary FFogImageLib;
        // { $120 }
        public Aow.Units.ImageLib.TImageLibrary FObjectsImageLib;
        // { $124 }
        public int FTTop;
        // { $128 }
        public int FTHeight;
        // { $12C }
        public int FTLeft;
        // { $130 }
        public int FTWidth;
        // { $134 }
        public bool FReserved138;
        public bool FBuildings;
        // { $139 }
        public bool FArmies;
        // { $13A }
        public bool FNaturals;
        // { $13B }
        public Aow.Rtl.TNotifyEvent FOnMapPositionChange;
        // { $13C }
        public int FSteps;
        // { $144 }
        public Aow.Units.AoWE.TAbstractAoWHSMap FMap;
        // { $148 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILFrame;
        // { $14C }
        public int FILIndexFrame;
        // { $150 }
        public int FMapWidth;
        // { $154 }
        public int FMapHeight;
        // { $158 }
        public Aow.Units.HSEngine.TMapLevel FLevel;
        // { $15C }
        public bool FObjectsDrawn;
        // { $160 }
        public int FPreviousWidth;
        // { $164 }
        public int FPreviousHeight;
        // { $168 }
        public int FReserved16C;
        public int FDrawableHeight;
        // { $170 }
        public Aow.Units.GFXE.TSurface FMapSurface;
        // { $174 }
        public Aow.Units.GFXE.TSurface FObjectSurface;
        // { $178 }
        public int FUpdateStep;
        // { $17C }
        public bool FIncremental;
        // Scanner.pas:126
        // { $180 }
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
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            FSteps = 1;
            FTLeft = -1;
            FTTop = -1;
            FTWidth = -1;
            FTHeight = -1;
        }

        // Scanner.pas:398
        public override void Validate()
        {
            Aow.Units.AOWInterfaceImgLib.TInterfaceIL Frame = default;
            Frame = FILFrame;
            if (Frame != null)
            {
                var call__1 = Frame.CheckIL(FILIndexFrame, 8, this);
                FILFrame = call__1;
            }

            if ((((((FILFrame != null) && (FTilesImageLib != null)) && (FObjectsImageLib != null)) && (FMap != null)) && (FFogImageLib != null)) && (FHighlightImageLib != null))
            {
                FValid = true;
            }

            base.Validate();
        }

        // Scanner.pas:409
        public override void SetSize()
        {
            int X = default;
            int Y = default;
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            if ((((FWinWidth != FPreviousWidth) || (FWinHeight != FPreviousHeight)) && (FWinWidth > 0)) && (FWinHeight > 0))
            {
                FHalfRowInset = (FWinHeight / FMapHeight) / 2;
                FDrawableHeight = FWinHeight - FHalfRowInset;
                if (FMapSurface != null)
                {
                    FObjectSurface.Resize(FWinWidth, FWinHeight);
                    FObjectSurface.Cls(65280);
                    FMapSurface.Resize(FWinWidth, FWinHeight);
                    FMapSurface.Lock(ref SD);
                    {
                        X = 0;
                        int end__2 = FWinWidth / FFogImageLib[0].Width;
                        for (; X <= end__2; X += 1)
                        {
                            {
                                Y = 0;
                                int end__3 = FWinHeight / FFogImageLib[0].Height;
                                for (; Y <= end__3; Y += 1)
                                {
                                    var call__4 = FFogImageLib[0];
                                    var call__5 = FFogImageLib[0];
                                    var call__6 = FFogImageLib[0];
                                    call__6.ShowClipped(X * call__5.Width, Y * call__4.Height, SD);
                                }
                            }
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
            if (FVisible && (FMapSurface != null))
            {
                if (!FIncremental)
                {
                    FIncremental = true;
                }

                if (FUpdateStep < FSteps)
                {
                    if (FUpdateStep > -1)
                    {
                        DrawMap(0, (FMapHeight * FUpdateStep) / FSteps, FMapWidth, (FMapHeight / FSteps) + 1);
                    }

                    FUpdateStep += 1;
                }
            }
        }

        // Scanner.pas:445
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // Scanner.pas:449
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int MapY = default;
            int OldLeft = default;
            int OldTop = default;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FMouseDown = true;
                X = (((X - FAOWWindow.FWinLeft) - FWinLeft) * FMapWidth) / FWinWidth;
                MapY = (((Y - FAOWWindow.FWinTop) - FWinTop) * FMapHeight) / FDrawableHeight;
                OldLeft = FTLeft;
                OldTop = FTTop;
                FTLeft = X - (FTWidth / 2);
                FTTop = MapY - (FTHeight / 2);
                if ((FTTop != OldTop) || (OldLeft != FTLeft))
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
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
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
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int MapX = default;
            int MapY = default;
            int OldLeft = default;
            int OldTop = default;
            base.CheckMouseMove(Shift, X, Y);
            FHandled = false;
            if ((((X >= (FAOWWindow.FWinLeft + FWinLeft)) && (X <= (((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - 1))) && (Y >= (FAOWWindow.FWinTop + FWinTop))) && (Y <= (((FAOWWindow.FWinTop + FWinTop) + FWinHeight) - 1)))
            {
                FHandled = true;
                if (FMouseDown == true)
                {
                    OldLeft = FTLeft;
                    OldTop = FTTop;
                    MapX = (((X - FAOWWindow.FWinLeft) - FWinLeft) * FMapWidth) / FWinWidth;
                    MapY = (((Y - FAOWWindow.FWinTop) - FWinTop) * FMapHeight) / FDrawableHeight;
                    FTLeft = MapX - (FTWidth / 2);
                    FTTop = MapY - (FTHeight / 2);
                    if ((FTTop != OldTop) || (FTLeft != OldLeft))
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
        public virtual void SetILFrame(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILFrame)
            {
                FValid = false;
                FILFrame = Value;
            }
        }

        // Scanner.pas:133
        public void SetILFIndex(int A1)
        {
            if (A1 != FILIndexFrame)
            {
                FILIndexFrame = A1;
                FValid = false;
            }
        }

        // Scanner.pas:151
        public void SetObjIL(Aow.Units.ImageLib.TImageLibrary Value)
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
        public void SetLevel(Aow.Units.HSEngine.TMapLevel Value)
        {
            FLevel = Value;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null)
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
        public void SetMapWidth(int A1)
        {
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null)
            {
                FMapWidth = A1 - 16;
            }
            else
            {
                FMapWidth = A1 - 12;
            }
        }

        // Scanner.pas:211
        public void SetMapHeight(int A1)
        {
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null)
            {
                FMapHeight = A1 - 12;
            }
            else
            {
                // { Both original branches subtract 12. }
                FMapHeight = A1 - 12;
            }
        }

        // Scanner.pas:217
        public byte GetTerrain(int A1, int A2)
        {
            byte Result = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null)
            {
                var call__7 = FLevel.GetField(A1 + 8, A2 + 6);
                Result = unchecked((byte)call__7.TerrainType);
            }
            else
            {
                var call__8 = FLevel.GetField(A1 + 6, A2 + 6);
                Result = unchecked((byte)call__8.TerrainType);
            }

            return Result;
        }

        // Scanner.pas:223
        public void SetTerrain(int A1, int A2, int A3)
        {
        }

        // Scanner.pas:226
        // { draft }
        public int GetOverlay(int X, int Y)
        {
            int Result = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null)
            {
                var call__9 = FLevel.GetField(X + 8, Y + 6);
                var call__10 = call__9.GetDecorationType();
                switch (call__10)
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
                        var call__11 = FLevel.GetField(X + 8, Y + 6);
                        Result = call__11.OverlayType;
                        break;
                }
            }
            else
            {
                var call__12 = FLevel.GetField(X + 6, Y + 6);
                Result = call__12.OverlayType;
            }

            return Result;
        }

        // Scanner.pas:243
        public void SetOverlay(int A1, int A2, int A3)
        {
        }

        // Scanner.pas:246
        // { draft }
        public byte GetExplored(int X, int Y)
        {
            byte Result = default;
            Aow.Units.AoWE.TAoWMapField Field = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null)
            {
                var call__13 = FLevel.GetField(X + 8, Y + 6);
                Field = (Aow.Units.AoWE.TAoWMapField)call__13;
            }
            else
            {
                var call__14 = FLevel.GetField(X + 6, Y + 6);
                Field = (Aow.Units.AoWE.TAoWMapField)call__14;
            }

            if (FMap.FExplorationEnabled)
            {
                var call__15 = Field.GetExplored(FMap.FSeatedPlayer);
                if (call__15)
                {
                    if (FMap.FFogEnabled)
                    {
                        if (Unsafe.As<ushort, Aow.Rtl.WordRec>(ref Unsafe.As<int, Aow.Rtl.LongRec>(ref Field.FFieldFlags).Hi).Lo == 0)
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
                    if (Unsafe.As<ushort, Aow.Rtl.WordRec>(ref Unsafe.As<int, Aow.Rtl.LongRec>(ref Field.FFieldFlags).Hi).Lo == 0)
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
        public void SetExplored(int A1, int A2, int A3)
        {
        }

        // Scanner.pas:280
        // { draft }
        public int GetObject(int X, int Y)
        {
            int Result = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null)
            {
                var call__16 = FLevel.GetField(X + 8, Y + 6);
                var call__17 = ((Aow.Units.AoWE.TAoWMapField)call__16).GetScannerIcon();
                Result = call__17;
            }
            else
            {
                var call__18 = FLevel.GetField(X + 6, Y + 6);
                var call__19 = ((Aow.Units.AoWE.TAoWMapField)call__18).GetScannerIcon();
                Result = call__19;
            }

            return Result;
        }

        // Scanner.pas:287
        public byte GetObjectSize()
        {
            byte Result = default;
            Result = 1;
            return Result;
        }

        // Scanner.pas:292
        public void SetObjectSize(int A1, int A2, int A3)
        {
        }

        // Scanner.pas:295
        // { draft }
        public void SetImgLib(Aow.Units.ImageLib.TImageLibrary Value)
        {
            FTilesImageLib = Value;
            FValid = false;
        }

        // Scanner.pas:301
        public void SetHimgLib(Aow.Units.ImageLib.TImageLibrary Value)
        {
            FHighlightImageLib = Value;
            FValid = false;
        }

        // Scanner.pas:307
        public void SetFogimgLib(Aow.Units.ImageLib.TImageLibrary Value)
        {
            FFogImageLib = Value;
            FValid = false;
        }

        // Scanner.pas:313
        public void SetSteps(int A1)
        {
            FSteps = A1;
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
        public void SetTLeft(int A1)
        {
            if (A1 != FTLeft)
            {
                FDrawn = false;
                FTLeft = A1;
            }
        }

        // Scanner.pas:350
        public void SetTTop(int A1)
        {
            if (A1 != FTTop)
            {
                FDrawn = false;
                FTTop = A1;
            }
        }

        // Scanner.pas:359
        public void SetTWidth(int A1)
        {
            if (A1 != FTWidth)
            {
                FDrawn = false;
                FTWidth = A1;
            }
        }

        // Scanner.pas:368
        public void SetTHeight(int A1)
        {
            if (A1 != FTHeight)
            {
                FDrawn = false;
                FTHeight = A1;
            }
        }

        // Scanner.pas:377
        public void SetMap(Aow.Units.AoWE.TAbstractAoWHSMap Value)
        {
            FMap = Value;
            if (FMap != null)
            {
                var call__20 = FMap.MapContainer.GetMapLevel(0);
                FLevel = call__20;
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null)
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
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // Scanner.pas:443
        // { draft body }
        public void DrawObjects()
        {
            int ImageIndex = default;
            int YOffset = default;
            int Army = default;
            int Natural = default;
            int Building = default;
            int X = default;
            int Y = default;
            int Obj = default;
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            if (FObjectSurface != null)
            {
                FObjectSurface.Cls(65280);
                YOffset = 0;
                var call__21 = FObjectSurface.Lock(ref SD);
                if (call__21 >= 0)
                {
                    {
                        X = 0;
                        int end__22 = FMapWidth - 1;
                        for (; X <= end__22; X += 1)
                        {
                            {
                                Y = 0;
                                int end__23 = FMapHeight - 1;
                                for (; Y <= end__23; Y += 1)
                                {
                                    if (unchecked((int)GetExplored(X, Y)) == 1)
                                    {
                                        Obj = GetObject(X, Y);
                                        if (Obj != 0)
                                        {
                                            Army = Obj & 255;
                                            Natural = Obj & 65280;
                                            Building = Obj & 16711680;
                                            if ((Building > 0) && FBuildings)
                                            {
                                                ImageIndex = (7 - (((int)(((uint)Building) >> 22)) & 3)) + ((((int)(((uint)Building) >> 16)) & 63) * 6);
                                                bool bool__24 = (ImageIndex < FObjectsImageLib.Count) && (ImageIndex > -1);
                                                if (bool__24)
                                                {
                                                    var call__25 = FObjectsImageLib[ImageIndex];
                                                    bool__24 = (call__25 != null);
                                                }

                                                if (bool__24)
                                                {
                                                    var call__26 = FObjectsImageLib[ImageIndex];
                                                    var call__27 = FObjectsImageLib[ImageIndex];
                                                    var call__28 = FObjectsImageLib[ImageIndex];
                                                    call__28.ShowClipped(((FWinWidth / (FMapWidth * 2)) + ((X * FWinWidth) / FMapWidth)) - (call__27.Width / 2), (((FWinHeight / (FMapHeight * 2)) + ((Y * FWinHeight) / FMapHeight)) - (call__26.Height / 2)) + YOffset, SD);
                                                }
                                            }
                                            else
                                            {
                                                if ((Natural > 0) && FNaturals)
                                                {
                                                    ImageIndex = (((int)(((uint)Natural) >> 8)) * 6) + 9;
                                                    bool bool__29 = (ImageIndex < FObjectsImageLib.Count) && (ImageIndex > -1);
                                                    if (bool__29)
                                                    {
                                                        var call__30 = FObjectsImageLib[ImageIndex];
                                                        bool__29 = (call__30 != null);
                                                    }

                                                    if (bool__29)
                                                    {
                                                        var call__31 = FObjectsImageLib[ImageIndex];
                                                        var call__32 = FObjectsImageLib[ImageIndex];
                                                        var call__33 = FObjectsImageLib[ImageIndex];
                                                        call__33.ShowClipped(((FWinWidth / (FMapWidth * 2)) + ((X * FWinWidth) / FMapWidth)) - (call__32.Width / 2), (((FWinHeight / (FMapHeight * 2)) + ((Y * FWinHeight) / FMapHeight)) - (call__31.Height / 2)) + YOffset, SD);
                                                    }
                                                }
                                            }

                                            if ((Army > 0) && FArmies)
                                            {
                                                ImageIndex = (Army * 6) + 8;
                                                bool bool__34 = (ImageIndex < FObjectsImageLib.Count) && (ImageIndex > -1);
                                                if (bool__34)
                                                {
                                                    var call__35 = FObjectsImageLib[ImageIndex];
                                                    bool__34 = (call__35 != null);
                                                }

                                                if (bool__34)
                                                {
                                                    var call__36 = FObjectsImageLib[ImageIndex];
                                                    var call__37 = FObjectsImageLib[ImageIndex];
                                                    var call__38 = FObjectsImageLib[ImageIndex];
                                                    call__38.ShowClipped(((FWinWidth / (FMapWidth * 2)) + ((X * FWinWidth) / FMapWidth)) - (call__37.Width / 2), (((FWinHeight / (FMapHeight * 2)) + ((Y * FWinHeight) / FMapHeight)) - (call__36.Height / 2)) + YOffset, SD);
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
                    }

                    FObjectSurface.Unlock();
                    FDrawn = false;
                    SetObDrawn(true);
                }
            }
        }

        [Published]
        public Aow.Units.ImageLib.TImageLibrary TilesImageLib { get => FTilesImageLib; set => SetImgLib(value); }

        [Published]
        public Aow.Units.ImageLib.TImageLibrary HiLightsImageLib { get => FHighlightImageLib; set => SetHimgLib(value); }

        [Published]
        public Aow.Units.ImageLib.TImageLibrary FogImageLib { get => FFogImageLib; set => SetFogimgLib(value); }

        [Published]
        public Aow.Units.ImageLib.TImageLibrary ILObjects { get => FObjectsImageLib; set => SetObjIL(value); }

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
        public Aow.Rtl.TNotifyEvent OnChangeTileBox { get => FOnMapPositionChange; set => FOnMapPositionChange = value; }

        [Published]
        public int MapDrawSteps { get => FSteps; set => SetSteps(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILFrame { get => FILFrame; set => SetILFrame(value); }

        [Published]
        public int ILIndexFrame { get => FILIndexFrame; set => SetILFIndex(value); }

        // Scanner.pas:53
        public static new Aow.Units.Scanner.TScanner Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.Scanner.TScanner o = new Aow.Units.Scanner.TScanner();
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

    public enum TOType : byte
    {
        otBuilding = 0,
        otArmy = 1,
        otNatural = 2
    }

    public static partial class Scanner
    {
        // Scanner.pas:458
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Tools", [typeof(Aow.Units.Scanner.TScanner)]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
