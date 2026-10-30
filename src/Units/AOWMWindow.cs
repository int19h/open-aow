// Translated from AOWMWindow.pas.
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
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWFileDialog;
using static Aow.Units.AoWFileDialog.AoWFileDialog;

namespace Aow.Units.AOWMWindow
{
    // { TMWindow: size $70; $5C saved map rectangle, $6D/$6F saved alignment. VMT 00450C38. }
    public partial class TMWindow : Aow.Rtl.TDataModule
    {
        public TMWindow(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Units.AoWWindow.TAOWWindow MapWindow;
        // { +0044 }
        public Aow.Units.HSEngine.THSMapViewer MapViewer;
        // { +0048 }
        public Aow.Rtl.TRect FWorldRect;
        // { $4C }
        public Aow.Rtl.TRect FMapRect;
        // { $5C }
        // { conflict: FCombatRect: TRect; }
        public Aow.Units.AoWComp.TAOWAlignWidth FWorldAlignWidth;
        // { $6C }
        public Aow.Units.AoWComp.TAOWAlignWidth FMapAlignWidth;
        // { $6D }
        // { conflict: FCombatAlignWidth: TAOWAlignWidth; }
        public Aow.Units.AoWComp.TAOWAlignHeight FWorldAlignHeight;
        // { $6E }
        public Aow.Units.AoWComp.TAOWAlignHeight FMapAlignHeight;
        // AOWMWindow.pas:61
        // { $6F }
        // { conflict: FCombatAlignHeight: TAOWAlignHeight; }
        public bool InsideMap(int X, int Y)
        {
            bool Result = default;
            Result = (((X > (this.MapWindow.WinLeft + 12)) && (X < ((this.MapWindow.WinLeft + this.MapWindow.WinWidth) - 12))) && (Y > (this.MapWindow.WinTop + 12))) && (Y < ((this.MapWindow.WinTop + this.MapWindow.WinHeight) - 12));

            return Result;
        }

        // AOWMWindow.pas:53
        // { 00450DE8 }
        public void MWindowCreate()
        {
            MapWindow.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            MapWindow.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            FMapAlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
            FMapAlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth;
        }

        // AOWMWindow.pas:68
        public void MapWindowMouseDown(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (InsideMap(X, Y))
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.Mouse.Down(Button, Shift, (X - 12) - MapWindow.WinLeft, (Y - 12) - MapWindow.WinTop);
            }
        }

        // AOWMWindow.pas:74
        public void MapWindowMouseUp(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (InsideMap(X, Y))
            {
                X = (X - 12) - MapWindow.WinLeft;
                var window = MapWindow;
                Aow.Units.AoWE.AoWE.AoWHSMap.Mouse.Up(Button, Shift, X, (Y - 12) - window.WinTop);
            }
            else
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.Mouse.Up(Button, Shift, -1, -1);
            }
        }

        // AOWMWindow.pas:82
        public void MapWindowMouseMove(Aow.Units.AoWComp.TAoWComponent Sender, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            // { Tried coordinate identity assignments; no exact match. }
            var window = MapWindow;
            if ((((X > (window.WinLeft + 11)) && (X < ((MapWindow.WinLeft + window.WinWidth) - 11))) && (Y > (window.WinTop + 11))) && (Y < ((window.WinTop + window.WinHeight) - 11)))
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.Mouse.Move(Shift, (X - 12) - MapWindow.WinLeft, (Y - 12) - window.WinTop);
                MapWindow.Cursor = 9999;
            }
        }

        // AOWMWindow.pas:93
        public void MapViewerSceneChanged()
        {
            // { Tried scanner-first comparison; no exact match. }
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer != -1)
            {
                Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetTWidth(Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneWidth / 32);
                Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetTHeight(Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneHeight / 32);
                Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetTLeft((MapViewer.SceneX / 32) - 6);
                Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetTTop((MapViewer.SceneY / 32) - 6);
                Aow.Units.AoWExeForms.AoWExeForms.TCScanner.Scanner.SetTWidth(Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneWidth / 32);
                Aow.Units.AoWExeForms.AoWExeForms.TCScanner.Scanner.SetTHeight(Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneHeight / 32);
                Aow.Units.AoWExeForms.AoWExeForms.TCScanner.Scanner.SetTLeft((Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneX / 32) - 6);
                Aow.Units.AoWExeForms.AoWExeForms.TCScanner.Scanner.SetTTop((Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneY / 32) - 6);
            }

            if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null)
            {
                var call__4 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(MapViewer.SceneL);
                if (call__4 != Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.FLevel)
                {
                    var scanner2 = Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner;
                    scanner2.FUpdateStep = -1;
                    var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(MapViewer.SceneL);
                    scanner2.SetLevel(call__6);

                    Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerTab.SetIndex(MapViewer.SceneL);
                }
            }

            if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null)
            {
                var hsMapMouse = Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap.Mouse;
                hsMapMouse.Move(TSet<Aow.Rtl.TShiftState>.Of(), hsMapMouse.X, hsMapMouse.Y);
            }
            else
            {
                var hsMapMouse2 = Aow.Units.AoWE.AoWE.AoWHSMap.Mouse;
                hsMapMouse2.Move(TSet<Aow.Rtl.TShiftState>.Of(), hsMapMouse2.X, hsMapMouse2.Y);
            }
        }

        // AOWMWindow.pas:122
        public void MapWindowResize()
        {
            this.MapWindow.Alignment.LeftOffset = this.MapWindow.WinLeft;
            this.MapWindow.Alignment.TopOffset = this.MapWindow.WinTop;
            this.MapWindow.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
            this.MapWindow.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;

            MapViewer.SceneWidth = MapWindow.WinWidth - 24;
            MapViewer.SceneHeight = MapWindow.WinHeight - 24;
        }

        // AOWMWindow.pas:136
        public void MapWindowExit(Aow.Rtl.TObject Sender)
        {
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.Mouse.Move(TSet<Aow.Rtl.TShiftState>.Of(), -1, -1);
            }
        }

        // AOWMWindow.pas:150
        public void MapWindowPreDrawSurface(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurface Surface, int X, int Y)
        {
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                MapViewer.SceneWidth = MapWindow.WinWidth - 24;
                MapViewer.SceneHeight = MapWindow.WinHeight - 24;
                if (MapWindow.DoubleBuffered)
                {
                    MapViewer.ShowScene(12, 12, MapWindow.FBackSurface);
                }
                else
                {
                    MapViewer.ShowScene(X + 12, Y + 12, Surface);
                }
            }
        }

        // AOWMWindow.pas:163
        public void MapWindowKeyDown(Aow.Units.AoWComp.TAoWComponent Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            ushort K = default;
            if ((Aow.Units.AoWE.AoWE.AoWHSMap != null) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap == null))
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.FKeyboard.KeyDown(ref Key, Shift);
            }

            K = unchecked((ushort)Key);
            switch (Aow.Units.AoWE.AoWE.AoWEngine.FLanguage)
            {
                case 4:
                    switch (Key)
                    {
                        case 65:
                            if (Shift.Contains(Aow.Rtl.TShiftState.ssCtrl))
                            {
                                K = 79;
                            }

                            break;
                        case 71:
                            if (Shift.Contains(Aow.Rtl.TShiftState.ssCtrl))
                            {
                                K = 83;
                            }

                            break;
                    }

                    break;
            }

            if ((MapViewer != null) && (Aow.Units.AoWE.AoWE.AoWHSMap != null))
            {
                switch (K)
                {
                    case 37:
                        Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneX = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneX - 32;
                        Sender.FHandled = true;
                        break;
                    case 39:
                        Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneX = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneX + 32;
                        Sender.FHandled = true;
                        break;
                    case 38:
                        Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneY = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneY - 32;
                        Sender.FHandled = true;
                        break;
                    case 40:
                        Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneY = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneY + 32;
                        Sender.FHandled = true;
                        break;
                    case 33:
                        Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneL = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneL - 1;
                        break;
                    case 34:
                        Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneL = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneL + 1;
                        break;
                    case 77:
                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null)
                        {
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.MoveUnits();
                        }

                        Sender.FHandled = true;
                        break;
                    case 78:
                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null)
                        {
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.SelectNextUnit(true);
                        }

                        Sender.FHandled = true;
                        break;
                    case 80:
                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null)
                        {
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.SelectNextUnit(false);
                        }

                        Sender.FHandled = true;
                        break;
                    case 69:
                    case 13:
                        if (Shift.Contains(Aow.Rtl.TShiftState.ssCtrl) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null))
                        {
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.EndTurn(false);
                            Sender.FHandled = true;
                        }

                        break;
                    case 79:
                    case 114:
                        if ((Aow.Units.AoWTC.AoWTC.AoWCombatMap == null) && ((K == 114) || Shift.Contains(Aow.Rtl.TShiftState.ssCtrl)))
                        {
                            Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FMode = 0;
                            Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FileDialog.Visible = true;
                        }

                        break;
                    case 83:
                    case 115:
                        if ((Aow.Units.AoWTC.AoWTC.AoWCombatMap == null) && ((K == 115) || Shift.Contains(Aow.Rtl.TShiftState.ssCtrl)))
                        {
                            Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FMode = 1;
                            Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FileDialog.Visible = true;
                        }

                        break;
                    case 116:
                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null)
                        {
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FShowGrid = !Aow.Units.AoWTC.AoWTC.AoWCombatMap.FShowGrid;
                        }
                        else
                        {
                            Aow.Units.AoWE.AoWE.AoWHSMap.FShowGrid = !Aow.Units.AoWE.AoWE.AoWHSMap.FShowGrid;
                        }

                        break;
                    case 117:
                        if (Aow.Units.AoWE.AoWE.AoWEngine.FDebug && (Aow.Units.AoWE.AoWE.AoWHSMap.FConnectionType != Aow.Units.AoWE.TPlayerConnectionType.pcNetwork))
                        {
                            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null)
                            {
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.SetFogEnabled(!Aow.Units.AoWTC.AoWTC.AoWCombatMap.FFogEnabled);
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FFogEnabled)
                                {
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.ShowMessage((AnsiString)"Fog Enabled");
                                }
                                else
                                {
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.ShowMessage((AnsiString)"Fog Disabled");
                                }
                            }
                            else
                            {
                                if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
                                {
                                    Aow.Units.AoWE.AoWE.AoWHSMap.SetFogEnabled(!Aow.Units.AoWE.AoWE.AoWHSMap.FFogEnabled);
                                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FFogEnabled)
                                    {
                                        Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage((AnsiString)"Fog Enabled");
                                    }
                                    else
                                    {
                                        Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage((AnsiString)"Fog Disabled");
                                    }
                                }
                            }
                        }

                        break;
                    case 118:
                        if (Aow.Units.AoWE.AoWE.AoWEngine.FDebug && (Aow.Units.AoWE.AoWE.AoWHSMap.FConnectionType != Aow.Units.AoWE.TPlayerConnectionType.pcNetwork))
                        {
                            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null)
                            {
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.SetExplorationEnabled(!Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled);
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled)
                                {
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.ShowMessage((AnsiString)"Exploration Enabled");
                                }
                                else
                                {
                                    // { Original text omits the space. }
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.ShowMessage((AnsiString)"ExplorationEnabled");
                                }
                            }
                            else
                            {
                                if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
                                {
                                    Aow.Units.AoWE.AoWE.AoWHSMap.SetExplorationEnabled(!Aow.Units.AoWE.AoWE.AoWHSMap.FExplorationEnabled);
                                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FExplorationEnabled)
                                    {
                                        Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage((AnsiString)"Exploration Enabled");
                                    }
                                    else
                                    {
                                        Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage((AnsiString)"Exploration Disabled");
                                    }
                                }
                            }
                        }

                        break;
                    case 119:
                        bool bool__10 = !Aow.Units.AoWE.AoWE.AoWEngine.FDemo;
                        if (bool__10)
                        {
                            var call__11 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetDeveloper();
                            bool__10 = call__11;
                        }

                        if (bool__10)
                        {
                            if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null)
                            {
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.EndCombat(this);
                            }
                        }

                        break;
                }
            }
        }
    }

    public static partial class AOWMWindow
    {
        public static Aow.Units.AOWMWindow.TMWindow MWindow;
    }
}
