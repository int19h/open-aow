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
    public Rect WorldRectangle;
    public Rect MapRectangle;

    public AlignWidth WorldAlignWidth { get; set; }
    public AlignWidth MapAlignWidth { get; set; }
    public AlignHeight WorldAlignHeight { get; set; }
    public AlignHeight MapAlignHeight { get; set; }

    // AOWMWindow.pas:61
    public bool InsideMap(int X, int Y) => X > this.MainWindow.WinLeft + 12 && X < this.MainWindow.WinLeft + this.MainWindow.WinWidth - 12 && Y > this.MainWindow.WinTop + 12 && Y < this.MainWindow.WinTop + this.MainWindow.WinHeight - 12;

    // AOWMWindow.pas:53
    public void MWindowCreate()
    {
        MainWindow.ImageLibraryFrame = DialogHosts.UIGraphics.GenericF;
        MainWindow.WindowManager = DialogHosts.General.Manager;
        MapAlignWidth = AlignWidth.Both;
        MapAlignHeight = AlignHeight.Both;
    }

    // AOWMWindow.pas:68
    public void MapWindowMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (InsideMap(X, Y))
        {
            X = X - 12 - MainWindow.WinLeft;
            Y = Y - 12 - MainWindow.WinTop;
            // Port extension: a scene larger than the map shows the map centered (THSMapViewer.ShowScene).
            if (MapViewer.SceneToMapMouse(ref X, ref Y))
            {
                AoWHSMap.Current.Mouse.Down(button, shift, X, Y);
            }
        }
    }

    // AOWMWindow.pas:74
    public void MapWindowMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
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

            AoWHSMap.Current.Mouse.Up(button, shift, X, Y);
        }
        else
        {
            AoWHSMap.Current.Mouse.Up(button, shift, -1, -1);
        }
    }

    // AOWMWindow.pas:82
    public void MapWindowMouseMove(Set<ShiftState> shift, int X, int Y)
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

            AoWHSMap.Current.Mouse.Move(shift, X, Y);
            MainWindow.Cursor = 9999;
        }
    }

    // AOWMWindow.pas:93
    public void MapViewerSceneChanged()
    {
        if (AoWHSMap.Current.SeatedPlayer != -1)
        {
            DialogHosts.ScannerWindow.Scanner.SetTWidth(DialogHosts.MapWindow.MapViewer.SceneWidth / 32);
            DialogHosts.ScannerWindow.Scanner.SetTHeight(DialogHosts.MapWindow.MapViewer.SceneHeight / 32);
            DialogHosts.ScannerWindow.Scanner.SetTLeft(MapViewer.SceneX / 32 - 6);
            DialogHosts.ScannerWindow.Scanner.SetTTop(MapViewer.SceneY / 32 - 6);
            DialogHosts.CombatScanner.Scanner.SetTWidth(DialogHosts.MapWindow.MapViewer.SceneWidth / 32);
            DialogHosts.CombatScanner.Scanner.SetTHeight(DialogHosts.MapWindow.MapViewer.SceneHeight / 32);
            DialogHosts.CombatScanner.Scanner.SetTLeft(DialogHosts.MapWindow.MapViewer.SceneX / 32 - 6);
            DialogHosts.CombatScanner.Scanner.SetTTop(DialogHosts.MapWindow.MapViewer.SceneY / 32 - 6);
        }

        if (AoWHSMap.Current.CombatMap == null)
        {
            if (AoWHSMap.Current.MapContainer.GetMapLevel(MapViewer.SceneL) != DialogHosts.ScannerWindow.Scanner.Level)
            {
                var scanner = DialogHosts.ScannerWindow.Scanner;
                scanner.UpdateStep = -1;
                scanner.SetLevel(AoWHSMap.Current.MapContainer.GetMapLevel(MapViewer.SceneL));

                DialogHosts.ScannerWindow.ScannerTab.SetIndex(MapViewer.SceneL);
            }
        }

        if (AoWHSMap.Current.CombatMap != null)
        {
            var hexSpriteMapMouse = AoWHSMap.Current.CombatMap.Mouse;
            hexSpriteMapMouse.Move(Set<ShiftState>.Of(), hexSpriteMapMouse.X, hexSpriteMapMouse.Y);
        }
        else
        {
            var hexSpriteMapMouse = AoWHSMap.Current.Mouse;
            hexSpriteMapMouse.Move(Set<ShiftState>.Of(), hexSpriteMapMouse.X, hexSpriteMapMouse.Y);
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
    public void MapWindowPreDrawSurface(Surface surface, int X, int Y)
    {
        if (AoWHSMap.Current != null)
        {
            MapViewer.SceneWidth = MainWindow.WinWidth - 24;
            MapViewer.SceneHeight = MainWindow.WinHeight - 24;
            if (MainWindow.DoubleBuffered)
            {
                MapViewer.ShowScene(12, 12, MainWindow.BackSurface);
            }
            else
            {
                MapViewer.ShowScene(X + 12, Y + 12, surface);
            }
        }
    }

    // AOWMWindow.pas:163
    public void MapWindowKeyDown(UIComponent sender, ref ushort key, Set<ShiftState> shift)
    {
        ushort K;
        if (AoWHSMap.Current != null && CombatMap.Current == null)
        {
            AoWHSMap.Current.Keyboard.KeyDown(ref key, shift);
            PressEndTurnShortcut(key, shift);
        }

        K = key;
        switch (GameTables.GameEngine.Language)
        {
            case 4:
                switch (key)
                {
                    case 65:
                        if (shift.Contains(ShiftState.Ctrl))
                        {
                            K = 79;
                        }

                        break;
                    case 71:
                        if (shift.Contains(ShiftState.Ctrl))
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
                    DialogHosts.MapWindow.MapViewer.SceneX = DialogHosts.MapWindow.MapViewer.SceneX - 32;
                    sender.Handled = true;
                    break;
                case 39:
                    DialogHosts.MapWindow.MapViewer.SceneX = DialogHosts.MapWindow.MapViewer.SceneX + 32;
                    sender.Handled = true;
                    break;
                case 38:
                    DialogHosts.MapWindow.MapViewer.SceneY = DialogHosts.MapWindow.MapViewer.SceneY - 32;
                    sender.Handled = true;
                    break;
                case 40:
                    DialogHosts.MapWindow.MapViewer.SceneY = DialogHosts.MapWindow.MapViewer.SceneY + 32;
                    sender.Handled = true;
                    break;
                case 33:
                    DialogHosts.MapWindow.MapViewer.SceneL = DialogHosts.MapWindow.MapViewer.SceneL - 1;
                    break;
                case 34:
                    DialogHosts.MapWindow.MapViewer.SceneL = DialogHosts.MapWindow.MapViewer.SceneL + 1;
                    break;
                case 77:
                    if (CombatMap.Current != null)
                    {
                        CombatMap.Current.UnitSelection.MoveUnits();
                    }

                    sender.Handled = true;
                    break;
                case 78:
                    if (CombatMap.Current != null)
                    {
                        CombatMap.Current.SelectNextUnit(true);
                    }

                    sender.Handled = true;
                    break;
                case 80:
                    if (CombatMap.Current != null)
                    {
                        CombatMap.Current.SelectNextUnit(false);
                    }

                    sender.Handled = true;
                    break;
                case 69:
                case 13:
                    if (shift.Contains(ShiftState.Ctrl) && CombatMap.Current != null)
                    {
                        CombatMap.Current.EndTurn(false);
                        sender.Handled = true;
                    }

                    break;
                case 79:
                case 114:
                    if (CombatMap.Current == null && (K == 114 || shift.Contains(ShiftState.Ctrl)))
                    {
                        DialogHosts.FileDialogWindow.Mode = 0;
                        DialogHosts.FileDialogWindow.FileDialog.Visible = true;
                    }

                    break;
                case 83:
                case 115:
                    if (CombatMap.Current == null && (K == 115 || shift.Contains(ShiftState.Ctrl)))
                    {
                        DialogHosts.FileDialogWindow.Mode = 1;
                        DialogHosts.FileDialogWindow.FileDialog.Visible = true;
                    }

                    break;
                case 116:
                    if (CombatMap.Current != null)
                    {
                        CombatMap.Current.ShowGrid = !CombatMap.Current.ShowGrid;
                    }
                    else
                    {
                        AoWHSMap.Current.ShowGrid = !AoWHSMap.Current.ShowGrid;
                    }

                    break;
                case 117:
                    if (GameTables.GameEngine.Debug && AoWHSMap.Current.ConnectionType != PlayerConnectionType.Network)
                    {
                        if (CombatMap.Current != null)
                        {
                            CombatMap.Current.SetFogEnabled(!CombatMap.Current.FogEnabled);
                            if (CombatMap.Current.FogEnabled)
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
                                AoWHSMap.Current.SetFogEnabled(!AoWHSMap.Current.FogEnabled);
                                if (AoWHSMap.Current.FogEnabled)
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
                    if (GameTables.GameEngine.Debug && AoWHSMap.Current.ConnectionType != PlayerConnectionType.Network)
                    {
                        if (CombatMap.Current != null)
                        {
                            CombatMap.Current.SetExplorationEnabled(!CombatMap.Current.ExplorationEnabled);
                            if (CombatMap.Current.ExplorationEnabled)
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
                                AoWHSMap.Current.SetExplorationEnabled(!AoWHSMap.Current.ExplorationEnabled);
                                if (AoWHSMap.Current.ExplorationEnabled)
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
                    if (!GameTables.GameEngine.Demo && GameTables.GameEngine.Registry.GetDeveloper())
                    {
                        if (AoWHSMap.Current.CombatMap != null)
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
        MainWindow.Drawn &= MainWindow._doubleBuffered;
    }
}
