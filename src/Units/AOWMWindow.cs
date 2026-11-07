// Translated from AOWMWindow.pas.
using Aow.Platform;
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWE;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWSWindow;
using Aow.Units.AoWWindow;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.AoWFileDialog.AoWFileDialogUnit;
using static Aow.Units.AoWTC.AoWTCUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;

namespace Aow.Units.AOWMWindow;

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
        MapWindow.ILFrame = aowIntGfxModUnit.IntGfxMod.GenericF;
        MapWindow.WinManager = General.Manager;
        FMapAlignWidth = TAOWAlignWidth.awBoth;
        FMapAlignHeight = TAOWAlignHeight.ahBoth;
    }

    // AOWMWindow.pas:68
    public void MapWindowMouseDown(TAoWComponent Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (InsideMap(X, Y))
        {
            X = X - 12 - MapWindow.WinLeft;
            Y = Y - 12 - MapWindow.WinTop;
            // Port extension: a scene larger than the map shows the map centered (THSMapViewer.ShowScene).
            if (MapViewer.SceneToMapMouse(ref X, ref Y))
            {
                AoWHSMap.Mouse.Down(Button, Shift, X, Y);
            }
        }
    }

    // AOWMWindow.pas:74
    public void MapWindowMouseUp(TAoWComponent Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (InsideMap(X, Y))
        {
            X = X - 12 - MapWindow.WinLeft;
            var window = MapWindow;
            Y = Y - 12 - window.WinTop;
            // Port extension: the black area around a centered map is outside the map.
            if (!MapViewer.SceneToMapMouse(ref X, ref Y))
            {
                X = -1;
                Y = -1;
            }

            AoWHSMap.Mouse.Up(Button, Shift, X, Y);
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
            X = X - 12 - MapWindow.WinLeft;
            Y = Y - 12 - window.WinTop;
            // Port extension: the black area around a centered map is outside the map.
            if (!MapViewer.SceneToMapMouse(ref X, ref Y))
            {
                X = -1;
                Y = -1;
            }

            AoWHSMap.Mouse.Move(Shift, X, Y);
            MapWindow.Cursor = 9999;
        }
    }

    // AOWMWindow.pas:93
    public void MapViewerSceneChanged()
    {
        if (AoWHSMap.FSeatedPlayer != -1)
        {
            AoWSWindowUnit.SWindow.Scanner.SetTWidth(AOWMWindowUnit.MWindow.MapViewer.SceneWidth / 32);
            AoWSWindowUnit.SWindow.Scanner.SetTHeight(AOWMWindowUnit.MWindow.MapViewer.SceneHeight / 32);
            AoWSWindowUnit.SWindow.Scanner.SetTLeft(MapViewer.SceneX / 32 - 6);
            AoWSWindowUnit.SWindow.Scanner.SetTTop(MapViewer.SceneY / 32 - 6);
            TCScanner.Scanner.SetTWidth(AOWMWindowUnit.MWindow.MapViewer.SceneWidth / 32);
            TCScanner.Scanner.SetTHeight(AOWMWindowUnit.MWindow.MapViewer.SceneHeight / 32);
            TCScanner.Scanner.SetTLeft(AOWMWindowUnit.MWindow.MapViewer.SceneX / 32 - 6);
            TCScanner.Scanner.SetTTop(AOWMWindowUnit.MWindow.MapViewer.SceneY / 32 - 6);
        }

        if (AoWHSMap.FCombatMap == null)
        {
            if (AoWHSMap.MapContainer.GetMapLevel(MapViewer.SceneL) != AoWSWindowUnit.SWindow.Scanner.FLevel)
            {
                var scanner = AoWSWindowUnit.SWindow.Scanner;
                scanner.FUpdateStep = -1;
                scanner.SetLevel(AoWHSMap.MapContainer.GetMapLevel(MapViewer.SceneL));

                AoWSWindowUnit.SWindow.ScannerTab.SetIndex(MapViewer.SceneL);
            }
        }

        if (AoWHSMap.FCombatMap != null)
        {
            var hsMapMouse = AoWHSMap.FCombatMap.Mouse;
            hsMapMouse.Move(TSet<TShiftState>.Of(), hsMapMouse.X, hsMapMouse.Y);
        }
        else
        {
            var hsMapMouse = AoWHSMap.Mouse;
            hsMapMouse.Move(TSet<TShiftState>.Of(), hsMapMouse.X, hsMapMouse.Y);
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
                    AOWMWindowUnit.MWindow.MapViewer.SceneX = AOWMWindowUnit.MWindow.MapViewer.SceneX - 32;
                    Sender.FHandled = true;
                    break;
                case 39:
                    AOWMWindowUnit.MWindow.MapViewer.SceneX = AOWMWindowUnit.MWindow.MapViewer.SceneX + 32;
                    Sender.FHandled = true;
                    break;
                case 38:
                    AOWMWindowUnit.MWindow.MapViewer.SceneY = AOWMWindowUnit.MWindow.MapViewer.SceneY - 32;
                    Sender.FHandled = true;
                    break;
                case 40:
                    AOWMWindowUnit.MWindow.MapViewer.SceneY = AOWMWindowUnit.MWindow.MapViewer.SceneY + 32;
                    Sender.FHandled = true;
                    break;
                case 33:
                    AOWMWindowUnit.MWindow.MapViewer.SceneL = AOWMWindowUnit.MWindow.MapViewer.SceneL - 1;
                    break;
                case 34:
                    AOWMWindowUnit.MWindow.MapViewer.SceneL = AOWMWindowUnit.MWindow.MapViewer.SceneL + 1;
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
                                AoWCombatMap.ShowMessage("Fog Enabled");
                            }
                            else
                            {
                                AoWCombatMap.ShowMessage("Fog Disabled");
                            }
                        }
                        else
                        {
                            if (AoWHSMap != null)
                            {
                                AoWHSMap.SetFogEnabled(!AoWHSMap.FFogEnabled);
                                if (AoWHSMap.FFogEnabled)
                                {
                                    AoWHSMap.ShowMessage("Fog Enabled");
                                }
                                else
                                {
                                    AoWHSMap.ShowMessage("Fog Disabled");
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
                                AoWCombatMap.ShowMessage("Exploration Enabled");
                            }
                            else
                            {
                                // { Original text omits the space. }
                                AoWCombatMap.ShowMessage("ExplorationEnabled");
                            }
                        }
                        else
                        {
                            if (AoWHSMap != null)
                            {
                                AoWHSMap.SetExplorationEnabled(!AoWHSMap.FExplorationEnabled);
                                if (AoWHSMap.FExplorationEnabled)
                                {
                                    AoWHSMap.ShowMessage("Exploration Enabled");
                                }
                                else
                                {
                                    AoWHSMap.ShowMessage("Exploration Disabled");
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

    // AOWMWindow.pas:141. Frame invalidation and measured FPS use the active host.
    public void MapWindowUpdate()
    {
        if (AoWHSMap is null) return;
        AoWHSMap.UpdateFPS(FrameFixtureInputs.MeasuredFrameRate ?? GameHost.MainForm.Display.CurrentFrameRate);
        MapWindow.FDrawn &= MapWindow.FDoubleBuffered;
    }
}

public static partial class AOWMWindowUnit
{
    public static TMWindow MWindow;
}
