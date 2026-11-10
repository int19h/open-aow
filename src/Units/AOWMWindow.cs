// Translated from AOWMWindow.pas.
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Host;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

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
        MapWindow.ILFrame = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.GenericF;
        MapWindow.WinManager = Aow.Game.Ui.Dialogs.DialogHosts.General.Manager;
        FMapAlignWidth = TAOWAlignWidth.awBoth;
        FMapAlignHeight = TAOWAlignHeight.ahBoth;
    }

    // AOWMWindow.pas:68
    public void MapWindowMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (InsideMap(X, Y))
        {
            X = X - 12 - MapWindow.WinLeft;
            Y = Y - 12 - MapWindow.WinTop;
            // Port extension: a scene larger than the map shows the map centered (THSMapViewer.ShowScene).
            if (MapViewer.SceneToMapMouse(ref X, ref Y))
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Mouse.Down(Button, Shift, X, Y);
            }
        }
    }

    // AOWMWindow.pas:74
    public void MapWindowMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
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

            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Mouse.Up(Button, Shift, X, Y);
        }
        else
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Mouse.Up(Button, Shift, -1, -1);
        }
    }

    // AOWMWindow.pas:82
    public void MapWindowMouseMove(TSet<TShiftState> Shift, int X, int Y)
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

            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Mouse.Move(Shift, X, Y);
            MapWindow.Cursor = 9999;
        }
    }

    // AOWMWindow.pas:93
    public void MapViewerSceneChanged()
    {
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer != -1)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTWidth(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneWidth / 32);
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTHeight(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneHeight / 32);
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTLeft(MapViewer.SceneX / 32 - 6);
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTTop(MapViewer.SceneY / 32 - 6);
            Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.Scanner.SetTWidth(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneWidth / 32);
            Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.Scanner.SetTHeight(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneHeight / 32);
            Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.Scanner.SetTLeft(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneX / 32 - 6);
            Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.Scanner.SetTTop(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneY / 32 - 6);
        }

        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap == null)
        {
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetMapLevel(MapViewer.SceneL) != Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.FLevel)
            {
                var scanner = Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner;
                scanner.FUpdateStep = -1;
                scanner.SetLevel(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetMapLevel(MapViewer.SceneL));

                Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerTab.SetIndex(MapViewer.SceneL);
            }
        }

        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap != null)
        {
            var hsMapMouse = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap.Mouse;
            hsMapMouse.Move(TSet<TShiftState>.Of(), hsMapMouse.X, hsMapMouse.Y);
        }
        else
        {
            var hsMapMouse = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Mouse;
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
    public void MapWindowExit()
    {
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Mouse.Move(TSet<TShiftState>.Of(), -1, -1);
        }
    }

    // AOWMWindow.pas:150
    public void MapWindowPreDrawSurface(TSurface Surface, int X, int Y)
    {
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
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
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap == null)
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FKeyboard.KeyDown(ref Key, Shift);
            PressEndTurnShortcut(Key, Shift);
        }

        K = Key;
        switch (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FLanguage)
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

        if (MapViewer != null && Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
        {
            switch (K)
            {
                case 37:
                    Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneX = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneX - 32;
                    Sender.FHandled = true;
                    break;
                case 39:
                    Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneX = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneX + 32;
                    Sender.FHandled = true;
                    break;
                case 38:
                    Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneY = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneY - 32;
                    Sender.FHandled = true;
                    break;
                case 40:
                    Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneY = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneY + 32;
                    Sender.FHandled = true;
                    break;
                case 33:
                    Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneL = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneL - 1;
                    break;
                case 34:
                    Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneL = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneL + 1;
                    break;
                case 77:
                    if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap != null)
                    {
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FUnitSelection.MoveUnits();
                    }

                    Sender.FHandled = true;
                    break;
                case 78:
                    if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap != null)
                    {
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.SelectNextUnit(true);
                    }

                    Sender.FHandled = true;
                    break;
                case 80:
                    if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap != null)
                    {
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.SelectNextUnit(false);
                    }

                    Sender.FHandled = true;
                    break;
                case 69:
                case 13:
                    if (Shift.Contains(TShiftState.ssCtrl) && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap != null)
                    {
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.EndTurn(false);
                        Sender.FHandled = true;
                    }

                    break;
                case 79:
                case 114:
                    if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap == null && (K == 114 || Shift.Contains(TShiftState.ssCtrl)))
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FMode = 0;
                        Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FileDialog.Visible = true;
                    }

                    break;
                case 83:
                case 115:
                    if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap == null && (K == 115 || Shift.Contains(TShiftState.ssCtrl)))
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FMode = 1;
                        Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FileDialog.Visible = true;
                    }

                    break;
                case 116:
                    if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap != null)
                    {
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FShowGrid = !Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FShowGrid;
                    }
                    else
                    {
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FShowGrid = !Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FShowGrid;
                    }

                    break;
                case 117:
                    if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDebug && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FConnectionType != TPlayerConnectionType.pcNetwork)
                    {
                        if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap != null)
                        {
                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.SetFogEnabled(!Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FFogEnabled);
                            if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FFogEnabled)
                            {
                                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.ShowMessage("Fog Enabled");
                            }
                            else
                            {
                                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.ShowMessage("Fog Disabled");
                            }
                        }
                        else
                        {
                            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
                            {
                                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.SetFogEnabled(!Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FFogEnabled);
                                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FFogEnabled)
                                {
                                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowMessage("Fog Enabled");
                                }
                                else
                                {
                                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowMessage("Fog Disabled");
                                }
                            }
                        }
                    }

                    break;
                case 118:
                    if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDebug && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FConnectionType != TPlayerConnectionType.pcNetwork)
                    {
                        if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap != null)
                        {
                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.SetExplorationEnabled(!Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FExplorationEnabled);
                            if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FExplorationEnabled)
                            {
                                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.ShowMessage("Exploration Enabled");
                            }
                            else
                            {
                                // { Original text omits the space. }
                                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.ShowMessage("ExplorationEnabled");
                            }
                        }
                        else
                        {
                            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
                            {
                                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.SetExplorationEnabled(!Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FExplorationEnabled);
                                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FExplorationEnabled)
                                {
                                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowMessage("Exploration Enabled");
                                }
                                else
                                {
                                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowMessage("Exploration Disabled");
                                }
                            }
                        }
                    }

                    break;
                case 119:
                    if (!Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDemo && Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetDeveloper())
                    {
                        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap != null)
                        {
                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.EndCombat();
                        }
                    }

                    break;
            }
        }
    }

    // AOWMWindow.pas:141. Frame invalidation and measured FPS use the active host.
    public void MapWindowUpdate()
    {
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap is null) return;
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.UpdateFPS(FrameFixtureInputs.MeasuredFrameRate ?? GameHost.MainForm.Display.CurrentFrameRate);
        MapWindow.FDrawn &= MapWindow.FDoubleBuffered;
    }
}
