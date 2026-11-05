// Translated from AOWMWindow.pas.
using Aow.Rtl;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using Aow.Units.AoWWindow;
using Aow.Units.AoWComp;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.AoWTC.AoWTC;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.AoWFileDialog.AoWFileDialog;

namespace Aow.Units.AOWMWindow
{
    // saved map rectangle, saved alignment
    public partial class TMWindow : TDataModule
    {
        public TMWindow(TComponent owner) : base(owner)
        { }

        public TAOWWindow MapWindow;
        public THSMapViewer MapViewer;
        public TRect FWorldRect;
        public TRect FMapRect;
        public TAOWAlignWidth FWorldAlignWidth;
        public TAOWAlignWidth FMapAlignWidth;
        public TAOWAlignHeight FWorldAlignHeight;
        public TAOWAlignHeight FMapAlignHeight;
        // AOWMWindow.pas:61
        public bool InsideMap(int X, int Y) => X > this.MapWindow.WinLeft + 12 && X < this.MapWindow.WinLeft + this.MapWindow.WinWidth - 12 && Y > this.MapWindow.WinTop + 12 && Y < this.MapWindow.WinTop + this.MapWindow.WinHeight - 12;

        // AOWMWindow.pas:53
        public void MWindowCreate()
        {
            MapWindow.ILFrame = aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            MapWindow.WinManager = General.Manager;
            FMapAlignWidth = TAOWAlignWidth.awBoth;
            FMapAlignHeight = TAOWAlignHeight.ahBoth;
        }

        // AOWMWindow.pas:68
        public void MapWindowMouseDown(TAoWComponent Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (InsideMap(X, Y))
            {
                AoWHSMap.Mouse.Down(Button, Shift, X - 12 - MapWindow.WinLeft, Y - 12 - MapWindow.WinTop);
            }
        }

        // AOWMWindow.pas:74
        public void MapWindowMouseUp(TAoWComponent Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (InsideMap(X, Y))
            {
                X = X - 12 - MapWindow.WinLeft;
                var window = MapWindow;
                AoWHSMap.Mouse.Up(Button, Shift, X, Y - 12 - window.WinTop);
            }
            else
            {
                AoWHSMap.Mouse.Up(Button, Shift, -1, -1);
            }
        }

        // AOWMWindow.pas:82
        public void MapWindowMouseMove(TAoWComponent Sender, TSet<TShiftState> Shift, int X, int Y)
        {
            var window = MapWindow;
            if (X > window.WinLeft + 11 && X < MapWindow.WinLeft + window.WinWidth - 11 && Y > window.WinTop + 11 && Y < window.WinTop + window.WinHeight - 11)
            {
                AoWHSMap.Mouse.Move(Shift, X - 12 - MapWindow.WinLeft, Y - 12 - window.WinTop);
                MapWindow.Cursor = 9999;
            }
        }

        // AOWMWindow.pas:93
        public void MapViewerSceneChanged()
        {
            if (AoWHSMap.FSeatedPlayer != -1)
            {
                AoWSWindow.AoWSWindow.SWindow.Scanner.SetTWidth(AOWMWindow.MWindow.MapViewer.SceneWidth / 32);
                AoWSWindow.AoWSWindow.SWindow.Scanner.SetTHeight(AOWMWindow.MWindow.MapViewer.SceneHeight / 32);
                AoWSWindow.AoWSWindow.SWindow.Scanner.SetTLeft(MapViewer.SceneX / 32 - 6);
                AoWSWindow.AoWSWindow.SWindow.Scanner.SetTTop(MapViewer.SceneY / 32 - 6);
                TCScanner.Scanner.SetTWidth(AOWMWindow.MWindow.MapViewer.SceneWidth / 32);
                TCScanner.Scanner.SetTHeight(AOWMWindow.MWindow.MapViewer.SceneHeight / 32);
                TCScanner.Scanner.SetTLeft(AOWMWindow.MWindow.MapViewer.SceneX / 32 - 6);
                TCScanner.Scanner.SetTTop(AOWMWindow.MWindow.MapViewer.SceneY / 32 - 6);
            }

            if (AoWHSMap.FCombatMap == null)
            {
                if (AoWHSMap.MapContainer.GetMapLevel(MapViewer.SceneL) != AoWSWindow.AoWSWindow.SWindow.Scanner.FLevel)
                {
                    var scanner2 = AoWSWindow.AoWSWindow.SWindow.Scanner;
                    scanner2.FUpdateStep = -1;
                    scanner2.SetLevel(AoWHSMap.MapContainer.GetMapLevel(MapViewer.SceneL));

                    AoWSWindow.AoWSWindow.SWindow.ScannerTab.SetIndex(MapViewer.SceneL);
                }
            }

            if (AoWHSMap.FCombatMap != null)
            {
                var hsMapMouse = AoWHSMap.FCombatMap.Mouse;
                hsMapMouse.Move(TSet<TShiftState>.Of(), hsMapMouse.X, hsMapMouse.Y);
            }
            else
            {
                var hsMapMouse2 = AoWHSMap.Mouse;
                hsMapMouse2.Move(TSet<TShiftState>.Of(), hsMapMouse2.X, hsMapMouse2.Y);
            }
        }

        // AOWMWindow.pas:122
        public void MapWindowResize()
        {
            this.MapWindow.Alignment.LeftOffset = this.MapWindow.WinLeft;
            this.MapWindow.Alignment.TopOffset = this.MapWindow.WinTop;
            this.MapWindow.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
            this.MapWindow.Alignment.AlignHeight = TAOWAlignHeight.ahTop;

            MapViewer.SceneWidth = MapWindow.WinWidth - 24;
            MapViewer.SceneHeight = MapWindow.WinHeight - 24;
        }

        // AOWMWindow.pas:136
        public void MapWindowExit(TObject Sender)
        {
            if (AoWHSMap != null)
            {
                AoWHSMap.Mouse.Move(TSet<TShiftState>.Of(), -1, -1);
            }
        }

        // AOWMWindow.pas:150
        public void MapWindowPreDrawSurface(TObject Sender, TSurface Surface, int X, int Y)
        {
            if (AoWHSMap != null)
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
        public void MapWindowKeyDown(TAoWComponent Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            ushort K;
            if (AoWHSMap != null && AoWCombatMap == null)
            {
                AoWHSMap.FKeyboard.KeyDown(ref Key, Shift);
            }

            K = Key;
            switch (AoWEngine.FLanguage)
            {
                case 4:
                    switch (Key)
                    {
                        case 65:
                            if (Shift.Contains(TShiftState.ssCtrl))
                            {
                                K = 79;
                            }

                            break;
                        case 71:
                            if (Shift.Contains(TShiftState.ssCtrl))
                            {
                                K = 83;
                            }

                            break;
                    }

                    break;
            }

            if (MapViewer != null && AoWHSMap != null)
            {
                switch (K)
                {
                    case 37:
                        AOWMWindow.MWindow.MapViewer.SceneX = AOWMWindow.MWindow.MapViewer.SceneX - 32;
                        Sender.FHandled = true;
                        break;
                    case 39:
                        AOWMWindow.MWindow.MapViewer.SceneX = AOWMWindow.MWindow.MapViewer.SceneX + 32;
                        Sender.FHandled = true;
                        break;
                    case 38:
                        AOWMWindow.MWindow.MapViewer.SceneY = AOWMWindow.MWindow.MapViewer.SceneY - 32;
                        Sender.FHandled = true;
                        break;
                    case 40:
                        AOWMWindow.MWindow.MapViewer.SceneY = AOWMWindow.MWindow.MapViewer.SceneY + 32;
                        Sender.FHandled = true;
                        break;
                    case 33:
                        AOWMWindow.MWindow.MapViewer.SceneL = AOWMWindow.MWindow.MapViewer.SceneL - 1;
                        break;
                    case 34:
                        AOWMWindow.MWindow.MapViewer.SceneL = AOWMWindow.MWindow.MapViewer.SceneL + 1;
                        break;
                    case 77:
                        if (AoWCombatMap != null)
                        {
                            AoWCombatMap.FUnitSelection.MoveUnits();
                        }

                        Sender.FHandled = true;
                        break;
                    case 78:
                        if (AoWCombatMap != null)
                        {
                            AoWCombatMap.SelectNextUnit(true);
                        }

                        Sender.FHandled = true;
                        break;
                    case 80:
                        if (AoWCombatMap != null)
                        {
                            AoWCombatMap.SelectNextUnit(false);
                        }

                        Sender.FHandled = true;
                        break;
                    case 69:
                    case 13:
                        if (Shift.Contains(TShiftState.ssCtrl) && AoWCombatMap != null)
                        {
                            AoWCombatMap.EndTurn(false);
                            Sender.FHandled = true;
                        }

                        break;
                    case 79:
                    case 114:
                        if (AoWCombatMap == null && (K == 114 || Shift.Contains(TShiftState.ssCtrl)))
                        {
                            FileDlgWindow.FMode = 0;
                            FileDlgWindow.FileDialog.Visible = true;
                        }

                        break;
                    case 83:
                    case 115:
                        if (AoWCombatMap == null && (K == 115 || Shift.Contains(TShiftState.ssCtrl)))
                        {
                            FileDlgWindow.FMode = 1;
                            FileDlgWindow.FileDialog.Visible = true;
                        }

                        break;
                    case 116:
                        if (AoWCombatMap != null)
                        {
                            AoWCombatMap.FShowGrid = !AoWCombatMap.FShowGrid;
                        }
                        else
                        {
                            AoWHSMap.FShowGrid = !AoWHSMap.FShowGrid;
                        }

                        break;
                    case 117:
                        if (AoWEngine.FDebug && AoWHSMap.FConnectionType != TPlayerConnectionType.pcNetwork)
                        {
                            if (AoWCombatMap != null)
                            {
                                AoWCombatMap.SetFogEnabled(!AoWCombatMap.FFogEnabled);
                                if (AoWCombatMap.FFogEnabled)
                                {
                                    AoWCombatMap.ShowMessage((AnsiString)"Fog Enabled");
                                }
                                else
                                {
                                    AoWCombatMap.ShowMessage((AnsiString)"Fog Disabled");
                                }
                            }
                            else
                            {
                                if (AoWHSMap != null)
                                {
                                    AoWHSMap.SetFogEnabled(!AoWHSMap.FFogEnabled);
                                    if (AoWHSMap.FFogEnabled)
                                    {
                                        AoWHSMap.ShowMessage((AnsiString)"Fog Enabled");
                                    }
                                    else
                                    {
                                        AoWHSMap.ShowMessage((AnsiString)"Fog Disabled");
                                    }
                                }
                            }
                        }

                        break;
                    case 118:
                        if (AoWEngine.FDebug && AoWHSMap.FConnectionType != TPlayerConnectionType.pcNetwork)
                        {
                            if (AoWCombatMap != null)
                            {
                                AoWCombatMap.SetExplorationEnabled(!AoWCombatMap.FExplorationEnabled);
                                if (AoWCombatMap.FExplorationEnabled)
                                {
                                    AoWCombatMap.ShowMessage((AnsiString)"Exploration Enabled");
                                }
                                else
                                {
                                    // { Original text omits the space. }
                                    AoWCombatMap.ShowMessage((AnsiString)"ExplorationEnabled");
                                }
                            }
                            else
                            {
                                if (AoWHSMap != null)
                                {
                                    AoWHSMap.SetExplorationEnabled(!AoWHSMap.FExplorationEnabled);
                                    if (AoWHSMap.FExplorationEnabled)
                                    {
                                        AoWHSMap.ShowMessage((AnsiString)"Exploration Enabled");
                                    }
                                    else
                                    {
                                        AoWHSMap.ShowMessage((AnsiString)"Exploration Disabled");
                                    }
                                }
                            }
                        }

                        break;
                    case 119:
                        if (!AoWEngine.FDemo && AoWEngine.FRegistry.GetDeveloper())
                        {
                            if (AoWHSMap.FCombatMap != null)
                            {
                                AoWCombatMap.EndCombat(this);
                            }
                        }

                        break;
                }
            }
        }
    }

    public static partial class AOWMWindow
    {
        public static TMWindow MWindow;
    }
}
