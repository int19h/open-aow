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
    public MWindow(Component owner) : base(owner)
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
        MapWindow.ILFrame = DialogHosts.IntGfxMod.GenericF;
        MapWindow.WinManager = DialogHosts.General.Manager;
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
                AoWHSMap.Current.Mouse.Down(Button, Shift, X, Y);
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

            AoWHSMap.Current.Mouse.Move(Shift, X, Y);
            MapWindow.Cursor = 9999;
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
        if (AoWHSMap.Current != null && Combat.AoWCombatMap.Current == null)
        {
            AoWHSMap.Current.FKeyboard.KeyDown(ref Key, Shift);
            PressEndTurnShortcut(Key, Shift);
        }

        K = Key;
        switch (Magic.Abilities.GameTables.AoWEngine.FLanguage)
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
                    if (Combat.AoWCombatMap.Current != null)
                    {
                        Combat.AoWCombatMap.Current.FUnitSelection.MoveUnits();
                    }

                    Sender.FHandled = true;
                    break;
                case 78:
                    if (Combat.AoWCombatMap.Current != null)
                    {
                        Combat.AoWCombatMap.Current.SelectNextUnit(true);
                    }

                    Sender.FHandled = true;
                    break;
                case 80:
                    if (Combat.AoWCombatMap.Current != null)
                    {
                        Combat.AoWCombatMap.Current.SelectNextUnit(false);
                    }

                    Sender.FHandled = true;
                    break;
                case 69:
                case 13:
                    if (Shift.Contains(ShiftState.Ctrl) && Combat.AoWCombatMap.Current != null)
                    {
                        Combat.AoWCombatMap.Current.EndTurn(false);
                        Sender.FHandled = true;
                    }

                    break;
                case 79:
                case 114:
                    if (Combat.AoWCombatMap.Current == null && (K == 114 || Shift.Contains(ShiftState.Ctrl)))
                    {
                        DialogHosts.FileDlgWindow.FMode = 0;
                        DialogHosts.FileDlgWindow.FileDialog.Visible = true;
                    }

                    break;
                case 83:
                case 115:
                    if (Combat.AoWCombatMap.Current == null && (K == 115 || Shift.Contains(ShiftState.Ctrl)))
                    {
                        DialogHosts.FileDlgWindow.FMode = 1;
                        DialogHosts.FileDlgWindow.FileDialog.Visible = true;
                    }

                    break;
                case 116:
                    if (Combat.AoWCombatMap.Current != null)
                    {
                        Combat.AoWCombatMap.Current.FShowGrid = !Combat.AoWCombatMap.Current.FShowGrid;
                    }
                    else
                    {
                        AoWHSMap.Current.FShowGrid = !AoWHSMap.Current.FShowGrid;
                    }

                    break;
                case 117:
                    if (Magic.Abilities.GameTables.AoWEngine.FDebug && AoWHSMap.Current.FConnectionType != PlayerConnectionType.Network)
                    {
                        if (Combat.AoWCombatMap.Current != null)
                        {
                            Combat.AoWCombatMap.Current.SetFogEnabled(!Combat.AoWCombatMap.Current.FFogEnabled);
                            if (Combat.AoWCombatMap.Current.FFogEnabled)
                            {
                                Combat.AoWCombatMap.Current.ShowMessage("Fog Enabled");
                            }
                            else
                            {
                                Combat.AoWCombatMap.Current.ShowMessage("Fog Disabled");
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
                    if (Magic.Abilities.GameTables.AoWEngine.FDebug && AoWHSMap.Current.FConnectionType != PlayerConnectionType.Network)
                    {
                        if (Combat.AoWCombatMap.Current != null)
                        {
                            Combat.AoWCombatMap.Current.SetExplorationEnabled(!Combat.AoWCombatMap.Current.FExplorationEnabled);
                            if (Combat.AoWCombatMap.Current.FExplorationEnabled)
                            {
                                Combat.AoWCombatMap.Current.ShowMessage("Exploration Enabled");
                            }
                            else
                            {
                                // { Original text omits the space. }
                                Combat.AoWCombatMap.Current.ShowMessage("ExplorationEnabled");
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
                    if (!Magic.Abilities.GameTables.AoWEngine.FDemo && Magic.Abilities.GameTables.AoWEngine.FRegistry.GetDeveloper())
                    {
                        if (AoWHSMap.Current.FCombatMap != null)
                        {
                            Combat.AoWCombatMap.Current.EndCombat();
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
        MapWindow.FDrawn &= MapWindow.FDoubleBuffered;
    }
}
