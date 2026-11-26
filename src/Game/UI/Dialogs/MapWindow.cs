#nullable disable warnings
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
public sealed partial class MapWindow(UIContext ui) : UIModule(ui)
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
    public void MapWindowMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (InsideMap(X, Y))
        {
            X = X - 12 - MainWindow.WinLeft;
            Y = Y - 12 - MainWindow.WinTop;
            // Port extension: a scene larger than the map shows the map centered (THSMapViewer.ShowScene).
            if (MapViewer.SceneToMapMouse(ref X, ref Y))
            {
                UI.Engine.StrategicMap.Mouse.Down(button, shift, X, Y);
            }
        }
    }

    // AOWMWindow.pas:74
    public void MapWindowMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
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

            UI.Engine.StrategicMap.Mouse.Up(button, shift, X, Y);
        }
        else
        {
            UI.Engine.StrategicMap.Mouse.Up(button, shift, -1, -1);
        }
    }

    // AOWMWindow.pas:82
    public void MapWindowMouseMove(ShiftStates shift, int X, int Y)
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

            UI.Engine.StrategicMap.Mouse.Move(shift, X, Y);
            MainWindow.Cursor = 9_999;
        }
    }

    // AOWMWindow.pas:93
    public void MapViewerSceneChanged()
    {
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        if (strategicMap.SeatedPlayer is not null)
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

        if (strategicMap.CombatMap is null)
        {
            if (strategicMap.MapContainer.GetMapLevel(MapViewer.SceneL) != UI.ScannerWindow.Scanner.Level)
            {
                Scanner scanner = UI.ScannerWindow.Scanner;
                scanner.UpdateStep = -1;
                scanner.SetLevel(strategicMap.MapContainer.GetMapLevel(MapViewer.SceneL));

                UI.ScannerWindow.ScannerTabs.SetIndex(MapViewer.SceneL);
            }
        }

        StrategicMap engineStrategicMap = UI.Engine.StrategicMap;
        if (engineStrategicMap.CombatMap is not null)
        {
            HexSpriteMapMouse hexSpriteMapMouse = engineStrategicMap.CombatMap.Mouse;
            hexSpriteMapMouse.Move(ShiftStates.None, hexSpriteMapMouse.X, hexSpriteMapMouse.Y);
        }
        else
        {
            HexSpriteMapMouse hexSpriteMapMouse = UI.Engine.StrategicMap.Mouse;
            hexSpriteMapMouse.Move(ShiftStates.None, hexSpriteMapMouse.X, hexSpriteMapMouse.Y);
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
    public void MapWindowExit() => UI.Engine.StrategicMap?.Mouse.Move(ShiftStates.None, -1, -1);

    // AOWMWindow.pas:150
    public void MapWindowPreDrawSurface(Surface surface, int X, int Y)
    {
        if (UI.Engine.StrategicMap is not null)
        {
            MapViewer.SceneWidth = MainWindow.WinWidth - 24;
            MapViewer.SceneHeight = MainWindow.WinHeight - 24;
            if (MainWindow.DoubleBuffered)
            {
                MapViewer.ShowScene(12, 12, surface: MainWindow.BackSurface);
            }
            else
            {
                MapViewer.ShowScene(X + 12, Y + 12, surface);
            }
        }
    }

    // AOWMWindow.pas:163
    public void MapWindowKeyDown(UIComponent sender, ref ushort key, ShiftStates shift)
    {
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        if (strategicMap is not null && UI.Engine.CombatMap is null)
        {
            strategicMap.Keyboard.KeyDown(key, shift);
            PressEndTurnShortcut(key, shift);
        }

        int k = key;
        switch (UI.Engine.Language)
        {
            case (byte)GameSettings.Language.Spanish:
                switch (key)
                {
                    case 65:
                        if (shift.Contains(ShiftStates.Ctrl))
                        {
                            k = 79;
                        }

                        break;
                    case 71:
                        if (shift.Contains(ShiftStates.Ctrl))
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

        if (MapViewer is not null && UI.Engine.StrategicMap is not null)
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
                    UI.Engine.CombatMap?.UnitSelection.MoveUnits();

                    sender.Handled = true;
                    break;
                case 78:
                    UI.Engine.CombatMap?.SelectNextUnit(true);

                    sender.Handled = true;
                    break;
                case 80:
                    UI.Engine.CombatMap?.SelectNextUnit(false);

                    sender.Handled = true;
                    break;
                case 69:
                case 13:
                    if (shift.Contains(ShiftStates.Ctrl) && UI.Engine.CombatMap is not null)
                    {
                        _ = UI.Engine.CombatMap.EndTurn(false);
                        sender.Handled = true;
                    }

                    break;
                case 79:
                case 114:
                    if (UI.Engine.CombatMap is null && (k == 114 || shift.Contains(ShiftStates.Ctrl)))
                    {
                        UI.FileDialogWindow.Mode = 0;
                        UI.FileDialogWindow.Window.Visible = true;
                    }

                    break;
                case 83:
                case 115:
                    if (UI.Engine.CombatMap is null && (k == 115 || shift.Contains(ShiftStates.Ctrl)))
                    {
                        UI.FileDialogWindow.Mode = 1;
                        UI.FileDialogWindow.Window.Visible = true;
                    }

                    break;
                case 116:
                    if (UI.Engine.CombatMap is not null)
                    {
                        CombatMap combatMap = UI.Engine.CombatMap;
                        combatMap.ShowGrid = !combatMap.ShowGrid;
                    }
                    else
                    {
                        StrategicMap engineStrategicMap = UI.Engine.StrategicMap;
                        engineStrategicMap.ShowGrid = !engineStrategicMap.ShowGrid;
                    }

                    break;
                case 117:
                    if (UI.Engine.Debug && UI.Engine.StrategicMap.ConnectionType != PlayerConnectionType.Network)
                    {
                        CombatMap engineCombatMap = UI.Engine.CombatMap;
                        if (engineCombatMap is not null)
                        {
                            engineCombatMap.SetFogEnabled(!engineCombatMap.FogEnabled);
                            CombatMap showMessageCombatMap = UI.Engine.CombatMap;
                            if (showMessageCombatMap.FogEnabled)
                            {
                                showMessageCombatMap.ShowMessage("Fog Enabled");
                            }
                            else
                            {
                                UI.Engine.CombatMap.ShowMessage("Fog Disabled");
                            }
                        }
                        else
                        {
                            StrategicMap setFogEnabledStrategicMap = UI.Engine.StrategicMap;
                            if (setFogEnabledStrategicMap is not null)
                            {
                                setFogEnabledStrategicMap.SetFogEnabled(!setFogEnabledStrategicMap.FogEnabled);
                                StrategicMap showMessageStrategicMap = UI.Engine.StrategicMap;
                                if (showMessageStrategicMap.FogEnabled)
                                {
                                    showMessageStrategicMap.ShowMessage("Fog Enabled");
                                }
                                else
                                {
                                    UI.Engine.StrategicMap.ShowMessage("Fog Disabled");
                                }
                            }
                        }
                    }

                    break;
                case 118:
                    if (UI.Engine.Debug && UI.Engine.StrategicMap.ConnectionType != PlayerConnectionType.Network)
                    {
                        CombatMap setExplorationEnabledCombatMap = UI.Engine.CombatMap;
                        if (setExplorationEnabledCombatMap is not null)
                        {
                            setExplorationEnabledCombatMap.SetExplorationEnabled(!setExplorationEnabledCombatMap.ExplorationEnabled);
                            CombatMap explorationEnabledCombatMap = UI.Engine.CombatMap;
                            if (explorationEnabledCombatMap.ExplorationEnabled)
                            {
                                explorationEnabledCombatMap.ShowMessage("Exploration Enabled");
                            }
                            else
                            {
                                // { Original text omits the space. }
                                UI.Engine.CombatMap.ShowMessage("ExplorationEnabled");
                            }
                        }
                        else
                        {
                            StrategicMap setExplorationEnabledStrategicMap = UI.Engine.StrategicMap;
                            if (setExplorationEnabledStrategicMap is not null)
                            {
                                setExplorationEnabledStrategicMap.SetExplorationEnabled(!setExplorationEnabledStrategicMap.ExplorationEnabled);
                                StrategicMap explorationEnabledStrategicMap = UI.Engine.StrategicMap;
                                if (explorationEnabledStrategicMap.ExplorationEnabled)
                                {
                                    explorationEnabledStrategicMap.ShowMessage("Exploration Enabled");
                                }
                                else
                                {
                                    UI.Engine.StrategicMap.ShowMessage("Exploration Disabled");
                                }
                            }
                        }
                    }

                    break;
                case 119:
                    if (!UI.Engine.Demo && UI.Engine.Registry.Developer)
                    {
                        if (UI.Engine.StrategicMap.CombatMap is not null)
                        {
                            UI.Engine.CombatMap.EndCombat();
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
        StrategicMap strategicMap = UI.Engine.StrategicMap;
        if (strategicMap is null)
        {
            return;
        }

        strategicMap.UpdateFPS(FrameFixtureInputs.MeasuredFrameRate ?? GameHost.MainForm.Display.CurrentFrameRate);
        MainWindow.IsDrawn &= MainWindow.DoubleBuffered;
    }
}
