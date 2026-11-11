// Translated from AOWMWindow.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// saved map rectangle, saved alignment
public partial class MapWindow : DataModule
{
    public MapWindow(Component owner) : base(owner)
    { }

    public Window MainWindow;
    public HexSpriteMapViewer MapViewer;
    public Rect FWorldRect;
    public Rect FMapRect;
    public AlignWidth FWorldAlignWidth;
    public AlignWidth FMapAlignWidth;
    public AlignHeight FWorldAlignHeight;
    public AlignHeight FMapAlignHeight;

    // AOWMWindow.pas:61
    public bool InsideMap(int X, int Y) => X > this.MainWindow.WinLeft + 12 && X < this.MainWindow.WinLeft + this.MainWindow.WinWidth - 12 && Y > this.MainWindow.WinTop + 12 && Y < this.MainWindow.WinTop + this.MainWindow.WinHeight - 12;

    // AOWMWindow.pas:53
    public void MWindowCreate()
    {
        MainWindow.ILFrame = DialogHosts.IntGfxMod.GenericF;
        MainWindow.WinManager = DialogHosts.General.Manager;
        FMapAlignWidth = AlignWidth.Both;
        FMapAlignHeight = AlignHeight.Both;
    }

    // AOWMWindow.pas:68
    public void MapWindowMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (InsideMap(X, Y))
        {
            X = X - 12 - MainWindow.WinLeft;
            Y = Y - 12 - MainWindow.WinTop;
            // Port extension: a scene larger than the map shows the map centered (THSMapViewer.ShowScene).
            if (MapViewer.SceneToMapMouse(ref X, ref Y))
            {
                AoWHSMap.Current.Mouse.Down(Button, Shift, X, Y);
            }
        }
    }

    // AOWMWindow.pas:74
    public void MapWindowMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (InsideMap(X, Y))
        {
            X = X - 12 - MainWindow.WinLeft;
            var window = MainWindow;
            Y = Y - 12 - window.WinTop;
            // Port extension: the black area around a centered map is outside the map.
            if (!MapViewer.SceneToMapMouse(ref X, ref Y))
            {
                X = -1;
                Y = -1;
            }

            AoWHSMap.Current.Mouse.Up(Button, Shift, X, Y);
        }
        else
        {
            AoWHSMap.Current.Mouse.Up(Button, Shift, -1, -1);
        }
    }

    // AOWMWindow.pas:82
    public void MapWindowMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        var window = MainWindow;
        if (X > window.WinLeft + 11 && X < MainWindow.WinLeft + window.WinWidth - 11 && Y > window.WinTop + 11 && Y < window.WinTop + window.WinHeight - 11)
        {
            X = X - 12 - MainWindow.WinLeft;
            Y = Y - 12 - window.WinTop;
            // Port extension: the black area around a centered map is outside the map.
            if (!MapViewer.SceneToMapMouse(ref X, ref Y))
            {
                X = -1;
                Y = -1;
            }

            AoWHSMap.Current.Mouse.Move(Shift, X, Y);
            MainWindow.Cursor = 9999;
        }
    }

    // AOWMWindow.pas:93
    public void MapViewerSceneChanged()
    {
        if (AoWHSMap.Current.FSeatedPlayer != -1)
        {
            DialogHosts.SWindow.Scanner.SetTWidth(DialogHosts.MWindow.MapViewer.SceneWidth / 32);
            DialogHosts.SWindow.Scanner.SetTHeight(DialogHosts.MWindow.MapViewer.SceneHeight / 32);
            DialogHosts.SWindow.Scanner.SetTLeft(MapViewer.SceneX / 32 - 6);
            DialogHosts.SWindow.Scanner.SetTTop(MapViewer.SceneY / 32 - 6);
            DialogHosts.TCScanner.Scanner.SetTWidth(DialogHosts.MWindow.MapViewer.SceneWidth / 32);
            DialogHosts.TCScanner.Scanner.SetTHeight(DialogHosts.MWindow.MapViewer.SceneHeight / 32);
            DialogHosts.TCScanner.Scanner.SetTLeft(DialogHosts.MWindow.MapViewer.SceneX / 32 - 6);
            DialogHosts.TCScanner.Scanner.SetTTop(DialogHosts.MWindow.MapViewer.SceneY / 32 - 6);
        }

        if (AoWHSMap.Current.FCombatMap == null)
        {
            if (AoWHSMap.Current.MapContainer.GetMapLevel(MapViewer.SceneL) != DialogHosts.SWindow.Scanner.FLevel)
            {
                var scanner = DialogHosts.SWindow.Scanner;
                scanner.FUpdateStep = -1;
                scanner.SetLevel(AoWHSMap.Current.MapContainer.GetMapLevel(MapViewer.SceneL));

                DialogHosts.SWindow.ScannerTab.SetIndex(MapViewer.SceneL);
            }
        }

        if (AoWHSMap.Current.FCombatMap != null)
        {
            var hsMapMouse = AoWHSMap.Current.FCombatMap.Mouse;
            hsMapMouse.Move(Set<ShiftState>.Of(), hsMapMouse.X, hsMapMouse.Y);
        }
        else
        {
            var hsMapMouse = AoWHSMap.Current.Mouse;
            hsMapMouse.Move(Set<ShiftState>.Of(), hsMapMouse.X, hsMapMouse.Y);
        }
    }

    // AOWMWindow.pas:122
    public void MapWindowResize()
    {
        this.MainWindow.Alignment.LeftOffset = this.MainWindow.WinLeft;
        this.MainWindow.Alignment.TopOffset = this.MainWindow.WinTop;
        this.MainWindow.Alignment.AlignWidth = AlignWidth.Left;
        this.MainWindow.Alignment.AlignHeight = AlignHeight.Top;

        MapViewer.SceneWidth = MainWindow.WinWidth - 24;
        MapViewer.SceneHeight = MainWindow.WinHeight - 24;
    }

    // AOWMWindow.pas:136
    public void MapWindowExit()
    {
        if (AoWHSMap.Current != null)
        {
            AoWHSMap.Current.Mouse.Move(Set<ShiftState>.Of(), -1, -1);
        }
    }

    // AOWMWindow.pas:150
    public void MapWindowPreDrawSurface(Surface Surface, int X, int Y)
    {
        if (AoWHSMap.Current != null)
        {
            MapViewer.SceneWidth = MainWindow.WinWidth - 24;
            MapViewer.SceneHeight = MainWindow.WinHeight - 24;
            if (MainWindow.DoubleBuffered)
            {
                MapViewer.ShowScene(12, 12, MainWindow.FBackSurface);
            }
            else
            {
                MapViewer.ShowScene(X + 12, Y + 12, Surface);
            }
        }
    }

    // AOWMWindow.pas:163
    public void MapWindowKeyDown(UIComponent Sender, ref ushort Key, Set<ShiftState> Shift)
    {
        ushort K;
        if (AoWHSMap.Current != null && CombatMap.Current == null)
        {
            AoWHSMap.Current.FKeyboard.KeyDown(ref Key, Shift);
            PressEndTurnShortcut(Key, Shift);
        }

        K = Key;
        switch (GameTables.AoWEngine.FLanguage)
        {
            case 4:
                switch (Key)
                {
                    case 65:
                        if (Shift.Contains(ShiftState.Ctrl))
                        {
                            K = 79;
                        }

                        break;
                    case 71:
                        if (Shift.Contains(ShiftState.Ctrl))
                        {
                            K = 83;
                        }

                        break;
                }

                break;
        }

        if (MapViewer != null && AoWHSMap.Current != null)
        {
            switch (K)
            {
                case 37:
                    DialogHosts.MWindow.MapViewer.SceneX = DialogHosts.MWindow.MapViewer.SceneX - 32;
                    Sender.FHandled = true;
                    break;
                case 39:
                    DialogHosts.MWindow.MapViewer.SceneX = DialogHosts.MWindow.MapViewer.SceneX + 32;
                    Sender.FHandled = true;
                    break;
                case 38:
                    DialogHosts.MWindow.MapViewer.SceneY = DialogHosts.MWindow.MapViewer.SceneY - 32;
                    Sender.FHandled = true;
                    break;
                case 40:
                    DialogHosts.MWindow.MapViewer.SceneY = DialogHosts.MWindow.MapViewer.SceneY + 32;
                    Sender.FHandled = true;
                    break;
                case 33:
                    DialogHosts.MWindow.MapViewer.SceneL = DialogHosts.MWindow.MapViewer.SceneL - 1;
                    break;
                case 34:
                    DialogHosts.MWindow.MapViewer.SceneL = DialogHosts.MWindow.MapViewer.SceneL + 1;
                    break;
                case 77:
                    if (CombatMap.Current != null)
                    {
                        CombatMap.Current.FUnitSelection.MoveUnits();
                    }

                    Sender.FHandled = true;
                    break;
                case 78:
                    if (CombatMap.Current != null)
                    {
                        CombatMap.Current.SelectNextUnit(true);
                    }

                    Sender.FHandled = true;
                    break;
                case 80:
                    if (CombatMap.Current != null)
                    {
                        CombatMap.Current.SelectNextUnit(false);
                    }

                    Sender.FHandled = true;
                    break;
                case 69:
                case 13:
                    if (Shift.Contains(ShiftState.Ctrl) && CombatMap.Current != null)
                    {
                        CombatMap.Current.EndTurn(false);
                        Sender.FHandled = true;
                    }

                    break;
                case 79:
                case 114:
                    if (CombatMap.Current == null && (K == 114 || Shift.Contains(ShiftState.Ctrl)))
                    {
                        DialogHosts.FileDlgWindow.FMode = 0;
                        DialogHosts.FileDlgWindow.FileDialog.Visible = true;
                    }

                    break;
                case 83:
                case 115:
                    if (CombatMap.Current == null && (K == 115 || Shift.Contains(ShiftState.Ctrl)))
                    {
                        DialogHosts.FileDlgWindow.FMode = 1;
                        DialogHosts.FileDlgWindow.FileDialog.Visible = true;
                    }

                    break;
                case 116:
                    if (CombatMap.Current != null)
                    {
                        CombatMap.Current.FShowGrid = !CombatMap.Current.FShowGrid;
                    }
                    else
                    {
                        AoWHSMap.Current.FShowGrid = !AoWHSMap.Current.FShowGrid;
                    }

                    break;
                case 117:
                    if (GameTables.AoWEngine.FDebug && AoWHSMap.Current.FConnectionType != PlayerConnectionType.Network)
                    {
                        if (CombatMap.Current != null)
                        {
                            CombatMap.Current.SetFogEnabled(!CombatMap.Current.FFogEnabled);
                            if (CombatMap.Current.FFogEnabled)
                            {
                                CombatMap.Current.ShowMessage("Fog Enabled");
                            }
                            else
                            {
                                CombatMap.Current.ShowMessage("Fog Disabled");
                            }
                        }
                        else
                        {
                            if (AoWHSMap.Current != null)
                            {
                                AoWHSMap.Current.SetFogEnabled(!AoWHSMap.Current.FFogEnabled);
                                if (AoWHSMap.Current.FFogEnabled)
                                {
                                    AoWHSMap.Current.ShowMessage("Fog Enabled");
                                }
                                else
                                {
                                    AoWHSMap.Current.ShowMessage("Fog Disabled");
                                }
                            }
                        }
                    }

                    break;
                case 118:
                    if (GameTables.AoWEngine.FDebug && AoWHSMap.Current.FConnectionType != PlayerConnectionType.Network)
                    {
                        if (CombatMap.Current != null)
                        {
                            CombatMap.Current.SetExplorationEnabled(!CombatMap.Current.FExplorationEnabled);
                            if (CombatMap.Current.FExplorationEnabled)
                            {
                                CombatMap.Current.ShowMessage("Exploration Enabled");
                            }
                            else
                            {
                                // { Original text omits the space. }
                                CombatMap.Current.ShowMessage("ExplorationEnabled");
                            }
                        }
                        else
                        {
                            if (AoWHSMap.Current != null)
                            {
                                AoWHSMap.Current.SetExplorationEnabled(!AoWHSMap.Current.FExplorationEnabled);
                                if (AoWHSMap.Current.FExplorationEnabled)
                                {
                                    AoWHSMap.Current.ShowMessage("Exploration Enabled");
                                }
                                else
                                {
                                    AoWHSMap.Current.ShowMessage("Exploration Disabled");
                                }
                            }
                        }
                    }

                    break;
                case 119:
                    if (!GameTables.AoWEngine.FDemo && GameTables.AoWEngine.FRegistry.GetDeveloper())
                    {
                        if (AoWHSMap.Current.FCombatMap != null)
                        {
                            CombatMap.Current.EndCombat();
                        }
                    }

                    break;
            }
        }
    }

    // AOWMWindow.pas:141. Frame invalidation and measured FPS use the active host.
    public void MapWindowUpdate()
    {
        if (AoWHSMap.Current is null) return;
        AoWHSMap.Current.UpdateFPS(FrameFixtureInputs.MeasuredFrameRate ?? GameHost.MainForm.Display.CurrentFrameRate);
        MainWindow.FDrawn &= MainWindow.FDoubleBuffered;
    }
}
