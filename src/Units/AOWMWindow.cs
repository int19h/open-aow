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
public partial class MWindow : DataModule
{
    public MWindow(global::Aow.Rtl.Component owner) : base(owner)
    { }

    public AOWWindow MapWindow;
    public HSMapViewer MapViewer;
    public Rect FWorldRect;
    public Rect FMapRect;
    public AOWAlignWidth FWorldAlignWidth;
    public AOWAlignWidth FMapAlignWidth;
    public AOWAlignHeight FWorldAlignHeight;
    public AOWAlignHeight FMapAlignHeight;

    // AOWMWindow.pas:61
    public bool InsideMap(int X, int Y) => X > this.MapWindow.WinLeft + 12 && X < this.MapWindow.WinLeft + this.MapWindow.WinWidth - 12 && Y > this.MapWindow.WinTop + 12 && Y < this.MapWindow.WinTop + this.MapWindow.WinHeight - 12;

    // AOWMWindow.pas:53
    public void MWindowCreate()
    {
        MapWindow.ILFrame = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.GenericF;
        MapWindow.WinManager = Aow.Game.Ui.Dialogs.DialogHosts.General.Manager;
        FMapAlignWidth = AOWAlignWidth.Both;
        FMapAlignHeight = AOWAlignHeight.Both;
    }

    // AOWMWindow.pas:68
    public void MapWindowMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (InsideMap(X, Y))
        {
            X = X - 12 - MapWindow.WinLeft;
            Y = Y - 12 - MapWindow.WinTop;
            // Port extension: a scene larger than the map shows the map centered (THSMapViewer.ShowScene).
            if (MapViewer.SceneToMapMouse(ref X, ref Y))
            {
                Aow.Game.World.Map.AoWHSMap.Current.Mouse.Down(Button, Shift, X, Y);
            }
        }
    }

    // AOWMWindow.pas:74
    public void MapWindowMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
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

            Aow.Game.World.Map.AoWHSMap.Current.Mouse.Up(Button, Shift, X, Y);
        }
        else
        {
            Aow.Game.World.Map.AoWHSMap.Current.Mouse.Up(Button, Shift, -1, -1);
        }
    }

    // AOWMWindow.pas:82
    public void MapWindowMouseMove(Set<ShiftState> Shift, int X, int Y)
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

            Aow.Game.World.Map.AoWHSMap.Current.Mouse.Move(Shift, X, Y);
            MapWindow.Cursor = 9999;
        }
    }

    // AOWMWindow.pas:93
    public void MapViewerSceneChanged()
    {
        if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer != -1)
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

        if (Aow.Game.World.Map.AoWHSMap.Current.FCombatMap == null)
        {
            if (Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetMapLevel(MapViewer.SceneL) != Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.FLevel)
            {
                var scanner = Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner;
                scanner.FUpdateStep = -1;
                scanner.SetLevel(Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetMapLevel(MapViewer.SceneL));

                Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerTab.SetIndex(MapViewer.SceneL);
            }
        }

        if (Aow.Game.World.Map.AoWHSMap.Current.FCombatMap != null)
        {
            var hsMapMouse = Aow.Game.World.Map.AoWHSMap.Current.FCombatMap.Mouse;
            hsMapMouse.Move(Set<ShiftState>.Of(), hsMapMouse.X, hsMapMouse.Y);
        }
        else
        {
            var hsMapMouse = Aow.Game.World.Map.AoWHSMap.Current.Mouse;
            hsMapMouse.Move(Set<ShiftState>.Of(), hsMapMouse.X, hsMapMouse.Y);
        }
    }

    // AOWMWindow.pas:122
    public void MapWindowResize()
    {
        this.MapWindow.Alignment.LeftOffset = this.MapWindow.WinLeft;
        this.MapWindow.Alignment.TopOffset = this.MapWindow.WinTop;
        this.MapWindow.Alignment.AlignWidth = AOWAlignWidth.Left;
        this.MapWindow.Alignment.AlignHeight = AOWAlignHeight.Top;

        MapViewer.SceneWidth = MapWindow.WinWidth - 24;
        MapViewer.SceneHeight = MapWindow.WinHeight - 24;
    }

    // AOWMWindow.pas:136
    public void MapWindowExit()
    {
        if (Aow.Game.World.Map.AoWHSMap.Current != null)
        {
            Aow.Game.World.Map.AoWHSMap.Current.Mouse.Move(Set<ShiftState>.Of(), -1, -1);
        }
    }

    // AOWMWindow.pas:150
    public void MapWindowPreDrawSurface(Surface Surface, int X, int Y)
    {
        if (Aow.Game.World.Map.AoWHSMap.Current != null)
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
    public void MapWindowKeyDown(AoWComponent Sender, ref ushort Key, Set<ShiftState> Shift)
    {
        ushort K;
        if (Aow.Game.World.Map.AoWHSMap.Current != null && Aow.Game.Combat.AoWCombatMap.Current == null)
        {
            Aow.Game.World.Map.AoWHSMap.Current.FKeyboard.KeyDown(ref Key, Shift);
            PressEndTurnShortcut(Key, Shift);
        }

        K = Key;
        switch (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FLanguage)
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

        if (MapViewer != null && Aow.Game.World.Map.AoWHSMap.Current != null)
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
                    if (Aow.Game.Combat.AoWCombatMap.Current != null)
                    {
                        Aow.Game.Combat.AoWCombatMap.Current.FUnitSelection.MoveUnits();
                    }

                    Sender.FHandled = true;
                    break;
                case 78:
                    if (Aow.Game.Combat.AoWCombatMap.Current != null)
                    {
                        Aow.Game.Combat.AoWCombatMap.Current.SelectNextUnit(true);
                    }

                    Sender.FHandled = true;
                    break;
                case 80:
                    if (Aow.Game.Combat.AoWCombatMap.Current != null)
                    {
                        Aow.Game.Combat.AoWCombatMap.Current.SelectNextUnit(false);
                    }

                    Sender.FHandled = true;
                    break;
                case 69:
                case 13:
                    if (Shift.Contains(ShiftState.Ctrl) && Aow.Game.Combat.AoWCombatMap.Current != null)
                    {
                        Aow.Game.Combat.AoWCombatMap.Current.EndTurn(false);
                        Sender.FHandled = true;
                    }

                    break;
                case 79:
                case 114:
                    if (Aow.Game.Combat.AoWCombatMap.Current == null && (K == 114 || Shift.Contains(ShiftState.Ctrl)))
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FMode = 0;
                        Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FileDialog.Visible = true;
                    }

                    break;
                case 83:
                case 115:
                    if (Aow.Game.Combat.AoWCombatMap.Current == null && (K == 115 || Shift.Contains(ShiftState.Ctrl)))
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FMode = 1;
                        Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FileDialog.Visible = true;
                    }

                    break;
                case 116:
                    if (Aow.Game.Combat.AoWCombatMap.Current != null)
                    {
                        Aow.Game.Combat.AoWCombatMap.Current.FShowGrid = !Aow.Game.Combat.AoWCombatMap.Current.FShowGrid;
                    }
                    else
                    {
                        Aow.Game.World.Map.AoWHSMap.Current.FShowGrid = !Aow.Game.World.Map.AoWHSMap.Current.FShowGrid;
                    }

                    break;
                case 117:
                    if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDebug && Aow.Game.World.Map.AoWHSMap.Current.FConnectionType != PlayerConnectionType.Network)
                    {
                        if (Aow.Game.Combat.AoWCombatMap.Current != null)
                        {
                            Aow.Game.Combat.AoWCombatMap.Current.SetFogEnabled(!Aow.Game.Combat.AoWCombatMap.Current.FFogEnabled);
                            if (Aow.Game.Combat.AoWCombatMap.Current.FFogEnabled)
                            {
                                Aow.Game.Combat.AoWCombatMap.Current.ShowMessage("Fog Enabled");
                            }
                            else
                            {
                                Aow.Game.Combat.AoWCombatMap.Current.ShowMessage("Fog Disabled");
                            }
                        }
                        else
                        {
                            if (Aow.Game.World.Map.AoWHSMap.Current != null)
                            {
                                Aow.Game.World.Map.AoWHSMap.Current.SetFogEnabled(!Aow.Game.World.Map.AoWHSMap.Current.FFogEnabled);
                                if (Aow.Game.World.Map.AoWHSMap.Current.FFogEnabled)
                                {
                                    Aow.Game.World.Map.AoWHSMap.Current.ShowMessage("Fog Enabled");
                                }
                                else
                                {
                                    Aow.Game.World.Map.AoWHSMap.Current.ShowMessage("Fog Disabled");
                                }
                            }
                        }
                    }

                    break;
                case 118:
                    if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDebug && Aow.Game.World.Map.AoWHSMap.Current.FConnectionType != PlayerConnectionType.Network)
                    {
                        if (Aow.Game.Combat.AoWCombatMap.Current != null)
                        {
                            Aow.Game.Combat.AoWCombatMap.Current.SetExplorationEnabled(!Aow.Game.Combat.AoWCombatMap.Current.FExplorationEnabled);
                            if (Aow.Game.Combat.AoWCombatMap.Current.FExplorationEnabled)
                            {
                                Aow.Game.Combat.AoWCombatMap.Current.ShowMessage("Exploration Enabled");
                            }
                            else
                            {
                                // { Original text omits the space. }
                                Aow.Game.Combat.AoWCombatMap.Current.ShowMessage("ExplorationEnabled");
                            }
                        }
                        else
                        {
                            if (Aow.Game.World.Map.AoWHSMap.Current != null)
                            {
                                Aow.Game.World.Map.AoWHSMap.Current.SetExplorationEnabled(!Aow.Game.World.Map.AoWHSMap.Current.FExplorationEnabled);
                                if (Aow.Game.World.Map.AoWHSMap.Current.FExplorationEnabled)
                                {
                                    Aow.Game.World.Map.AoWHSMap.Current.ShowMessage("Exploration Enabled");
                                }
                                else
                                {
                                    Aow.Game.World.Map.AoWHSMap.Current.ShowMessage("Exploration Disabled");
                                }
                            }
                        }
                    }

                    break;
                case 119:
                    if (!Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDemo && Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetDeveloper())
                    {
                        if (Aow.Game.World.Map.AoWHSMap.Current.FCombatMap != null)
                        {
                            Aow.Game.Combat.AoWCombatMap.Current.EndCombat();
                        }
                    }

                    break;
            }
        }
    }

    // AOWMWindow.pas:141. Frame invalidation and measured FPS use the active host.
    public void MapWindowUpdate()
    {
        if (Aow.Game.World.Map.AoWHSMap.Current is null) return;
        Aow.Game.World.Map.AoWHSMap.Current.UpdateFPS(FrameFixtureInputs.MeasuredFrameRate ?? GameHost.MainForm.Display.CurrentFrameRate);
        MapWindow.FDrawn &= MapWindow.FDoubleBuffered;
    }
}
