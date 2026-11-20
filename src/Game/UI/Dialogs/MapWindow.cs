#nullable disable
// Translated from AOWMWindow.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Host;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// saved map rectangle, saved alignment
public partial class MapWindow(UIContext ui) : UIModule(ui)
{
    public Window MainWindow { get; set; }
    public HexSpriteMapViewer MapViewer { get; set; }

    public Rect WorldRectangle { get; internal set; }
    public Rect MapRectangle { get; internal set; }

    public AlignWidth WorldAlignWidth { get; set; }
    public AlignWidth MapAlignWidth { get; set; }
    public AlignHeight WorldAlignHeight { get; set; }
    public AlignHeight MapAlignHeight { get; set; }

    // AOWMWindow.pas:61
    public bool InsideMap(int X, int Y) => X > MainWindow.WinLeft + 12 && X < MainWindow.WinLeft + MainWindow.WinWidth - 12 && Y > MainWindow.WinTop + 12 && Y < MainWindow.WinTop + MainWindow.WinHeight - 12;

    // AOWMWindow.pas:53
    public void MWindowCreate()
    {
        MainWindow.ImageLibraryFrame = UI.Graphics.GenericFrames;
        MainWindow.WindowManager = UI.General.Manager;
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
                StrategicMap.Current.Mouse.Down(button, shift, X, Y);
            }
        }
    }

    // AOWMWindow.pas:74
    public void MapWindowMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (InsideMap(X, Y))
        {
            X = X - 12 - MainWindow.WinLeft;
            Window window = MainWindow;
            Y = Y - 12 - window.WinTop;
            // Port extension: the black area around a centered map is outside the map.
            if (!MapViewer.SceneToMapMouse(ref X, ref Y))
            {
                X = -1;
                Y = -1;
            }

            StrategicMap.Current.Mouse.Up(button, shift, X, Y);
        }
        else
        {
            StrategicMap.Current.Mouse.Up(button, shift, -1, -1);
        }
    }

    // AOWMWindow.pas:82
    public void MapWindowMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        Window window = MainWindow;
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

            StrategicMap.Current.Mouse.Move(shift, X, Y);
            MainWindow.Cursor = 9999;
        }
    }

    // AOWMWindow.pas:93
    public void MapViewerSceneChanged()
    {
        if (StrategicMap.Current.SeatedPlayer != -1)
        {
            UI.ScannerWindow.Scanner.SetTWidth(UI.MapWindow.MapViewer.SceneWidth / 32);
            UI.ScannerWindow.Scanner.SetTHeight(UI.MapWindow.MapViewer.SceneHeight / 32);
            UI.ScannerWindow.Scanner.SetTLeft((MapViewer.SceneX / 32) - 6);
            UI.ScannerWindow.Scanner.SetTTop((MapViewer.SceneY / 32) - 6);
            UI.CombatScanner.Scanner.SetTWidth(UI.MapWindow.MapViewer.SceneWidth / 32);
            UI.CombatScanner.Scanner.SetTHeight(UI.MapWindow.MapViewer.SceneHeight / 32);
            UI.CombatScanner.Scanner.SetTLeft((UI.MapWindow.MapViewer.SceneX / 32) - 6);
            UI.CombatScanner.Scanner.SetTTop((UI.MapWindow.MapViewer.SceneY / 32) - 6);
        }

        if (StrategicMap.Current.CombatMap is null)
        {
            if (StrategicMap.Current.MapContainer.GetMapLevel(MapViewer.SceneL) != UI.ScannerWindow.Scanner.Level)
            {
                Scanner scanner = UI.ScannerWindow.Scanner;
                scanner.UpdateStep = -1;
                scanner.SetLevel(StrategicMap.Current.MapContainer.GetMapLevel(MapViewer.SceneL));

                UI.ScannerWindow.ScannerTabs.SetIndex(MapViewer.SceneL);
            }
        }

        if (StrategicMap.Current.CombatMap is not null)
        {
            HexSpriteMapMouse hexSpriteMapMouse = StrategicMap.Current.CombatMap.Mouse;
            hexSpriteMapMouse.Move(Set<ShiftState>.Of(), hexSpriteMapMouse.X, hexSpriteMapMouse.Y);
        }
        else
        {
            HexSpriteMapMouse hexSpriteMapMouse = StrategicMap.Current.Mouse;
            hexSpriteMapMouse.Move(Set<ShiftState>.Of(), hexSpriteMapMouse.X, hexSpriteMapMouse.Y);
        }
    }

    // AOWMWindow.pas:122
    public void MapWindowResize()
    {
        MainWindow.Alignment.LeftOffset = MainWindow.WinLeft;
        MainWindow.Alignment.TopOffset = MainWindow.WinTop;
        MainWindow.Alignment.AlignWidth = AlignWidth.Left;
        MainWindow.Alignment.AlignHeight = AlignHeight.Top;

        MapViewer.SceneWidth = MainWindow.WinWidth - 24;
        MapViewer.SceneHeight = MainWindow.WinHeight - 24;
    }

    // AOWMWindow.pas:136
    public void MapWindowExit() => StrategicMap.Current?.Mouse.Move(Set<ShiftState>.Of(), -1, -1);

    // AOWMWindow.pas:150
    public void MapWindowPreDrawSurface(Surface surface, int X, int Y)
    {
        if (StrategicMap.Current is not null)
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
        if (StrategicMap.Current is not null && CombatMap.Current is null)
        {
            StrategicMap.Current.Keyboard.KeyDown(key, shift);
            PressEndTurnShortcut(key, shift);
        }

        ushort k = key;
        switch (UI.Engine.Language)
        {
            case 4:
                switch (key)
                {
                    case 65:
                        if (shift.Contains(ShiftState.Ctrl))
                        {
                            k = 79;
                        }

                        break;
                    case 71:
                        if (shift.Contains(ShiftState.Ctrl))
                        {
                            k = 83;
                        }

                        break;
                    default:
                        break;
                }

                break;
            default:
                break;
        }

        if (MapViewer is not null && StrategicMap.Current is not null)
        {
            switch (k)
            {
                case 37:
                    UI.MapWindow.MapViewer.SceneX = UI.MapWindow.MapViewer.SceneX - 32;
                    sender.Handled = true;
                    break;
                case 39:
                    UI.MapWindow.MapViewer.SceneX = UI.MapWindow.MapViewer.SceneX + 32;
                    sender.Handled = true;
                    break;
                case 38:
                    UI.MapWindow.MapViewer.SceneY = UI.MapWindow.MapViewer.SceneY - 32;
                    sender.Handled = true;
                    break;
                case 40:
                    UI.MapWindow.MapViewer.SceneY = UI.MapWindow.MapViewer.SceneY + 32;
                    sender.Handled = true;
                    break;
                case 33:
                    UI.MapWindow.MapViewer.SceneL = UI.MapWindow.MapViewer.SceneL - 1;
                    break;
                case 34:
                    UI.MapWindow.MapViewer.SceneL = UI.MapWindow.MapViewer.SceneL + 1;
                    break;
                case 77:
                    CombatMap.Current?.UnitSelection.MoveUnits();

                    sender.Handled = true;
                    break;
                case 78:
                    CombatMap.Current?.SelectNextUnit(true);

                    sender.Handled = true;
                    break;
                case 80:
                    CombatMap.Current?.SelectNextUnit(false);

                    sender.Handled = true;
                    break;
                case 69:
                case 13:
                    if (shift.Contains(ShiftState.Ctrl) && CombatMap.Current is not null)
                    {
                        _ = CombatMap.Current.EndTurn(false);
                        sender.Handled = true;
                    }

                    break;
                case 79:
                case 114:
                    if (CombatMap.Current is null && (k == 114 || shift.Contains(ShiftState.Ctrl)))
                    {
                        UI.FileDialogWindow.Mode = 0;
                        UI.FileDialogWindow.Window.Visible = true;
                    }

                    break;
                case 83:
                case 115:
                    if (CombatMap.Current is null && (k == 115 || shift.Contains(ShiftState.Ctrl)))
                    {
                        UI.FileDialogWindow.Mode = 1;
                        UI.FileDialogWindow.Window.Visible = true;
                    }

                    break;
                case 116:
                    if (CombatMap.Current is not null)
                    {
                        CombatMap.Current.ShowGrid = !CombatMap.Current.ShowGrid;
                    }
                    else
                    {
                        StrategicMap.Current.ShowGrid = !StrategicMap.Current.ShowGrid;
                    }

                    break;
                case 117:
                    if (UI.Engine.Debug && StrategicMap.Current.ConnectionType != PlayerConnectionType.Network)
                    {
                        if (CombatMap.Current is not null)
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
                            if (StrategicMap.Current is not null)
                            {
                                StrategicMap.Current.SetFogEnabled(!StrategicMap.Current.FogEnabled);
                                if (StrategicMap.Current.FogEnabled)
                                {
                                    StrategicMap.Current.ShowMessage("Fog Enabled");
                                }
                                else
                                {
                                    StrategicMap.Current.ShowMessage("Fog Disabled");
                                }
                            }
                        }
                    }

                    break;
                case 118:
                    if (UI.Engine.Debug && StrategicMap.Current.ConnectionType != PlayerConnectionType.Network)
                    {
                        if (CombatMap.Current is not null)
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
                            if (StrategicMap.Current is not null)
                            {
                                StrategicMap.Current.SetExplorationEnabled(!StrategicMap.Current.ExplorationEnabled);
                                if (StrategicMap.Current.ExplorationEnabled)
                                {
                                    StrategicMap.Current.ShowMessage("Exploration Enabled");
                                }
                                else
                                {
                                    StrategicMap.Current.ShowMessage("Exploration Disabled");
                                }
                            }
                        }
                    }

                    break;
                case 119:
                    if (!UI.Engine.Demo && UI.Engine.Registry.Developer)
                    {
                        if (StrategicMap.Current.CombatMap is not null)
                        {
                            CombatMap.Current.EndCombat();
                        }
                    }

                    break;
                default:
                    break;
            }
        }
    }

    // AOWMWindow.pas:141. Frame invalidation and measured FPS use the active host.
    public void MapWindowUpdate()
    {
        if (StrategicMap.Current is null)
        {
            return;
        }

        StrategicMap.Current.UpdateFPS(FrameFixtureInputs.MeasuredFrameRate ?? GameHost.MainForm.Display.CurrentFrameRate);
        MainWindow.Drawn &= MainWindow.DoubleBuffered;
    }
}
