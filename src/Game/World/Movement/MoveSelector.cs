// Translated from AoWE.pas.
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Movement;

public partial class MoveSelector : IDisposable
{
    // Structur.PAS:4109
    // control settings, moves, current move, map, status
    public MoveSelector()
    {
        FMoveControlSettings = CreateMCS();
        FMoveSettingsList = new MoveSettingsList(null);
        FCurrentMove = -1;
        FMap = null;
    }

    public MoveControlSettings FMoveControlSettings;
    public MoveSettingsList FMoveSettingsList;
    public int FCurrentMove;
    public GameMap FMap;
    public Set<MoveSelectorStatusElement> FStatus;

    // Structur.PAS:4118
    public virtual void Dispose()
    {
        Clear();
        FMoveControlSettings.Release();
        FMoveSettingsList?.Dispose();
    }

    // Structur.PAS:4192
    public virtual void MouseMoveEvent(object Sender)
    {
        UpdateMoveCursor();
    }

    // Structur.PAS:4214
    public virtual void MouseDownEvent(object Sender)
    {
        object Capture;
        HexSpriteMapMouse Mouse;
        Mouse = FMap.Mouse;
        Capture = Mouse.Capture;
        if ((Capture != this && Capture != AoWHSMap.Current.FTargetSelection) || Mouse.Button == MouseButton.Right)
        {
            Unselect();
        }
        else
        {
            var hsMapMouse = FMap.Mouse;
            if (Capture == this && hsMapMouse.Button == MouseButton.Left)
            {
                var yhx = FMap.Mouse.Yhx;
                SelectPath((sbyte)hsMapMouse.Xhx, (sbyte)yhx, (sbyte)FMap.ViewL);
            }
        }
    }

    // Structur.PAS:4197
    public virtual void CalculateMovePath(int Index, sbyte X, sbyte Y, sbyte L)
    {
    }

    // Structur.PAS:4189
    public virtual void UpdateMoveCursor()
    {
    }

    // Structur.PAS:4126
    public virtual MoveControlSettings CreateMCS() => new MoveControlSettings(null);

    // Structur.PAS:4138
    public virtual void SetCurrentMove(int Value)
    {
        FCurrentMove = Value;
    }

    // Structur.PAS:4200
    public virtual void SelectPath(sbyte X, sbyte Y, sbyte L)
    {
        if (FCurrentMove != -1 && X != -1)
        {
            var moveSettings = FMoveSettingsList.GetMoveSettings(FCurrentMove);
            if (moveSettings.FPath.Count != 0 && moveSettings.FPath[0].X == X && moveSettings.FPath[0].Y == Y && moveSettings.FPath[0].L == L)
            {
                ExecuteMove();
            }
            else
            {
                if (ValidateSelection((AoWMapField)FMap.MapContainer.GetField(X, Y, L)))
                {
                    CalculateMovePath(FCurrentMove, X, Y, L);
                }
                else
                {
                    moveSettings.FPath.Clear();
                }
            }
        }
    }

    // Structur.PAS:4143
    public virtual void Unselect()
    {
        Deactivate();
        Clear();
    }

    // Structur.PAS:4149
    public virtual bool ValidateSelection(AoWMapField Field) => true;

    // Structur.PAS:4154
    public virtual void ExecuteMove()
    {
    }

    // Structur.PAS:4225
    public virtual void Activate(GameMap Map)
    {
        if (FStatus.Contains(MoveSelectorStatusElement.Activated))
        {
            Deactivate();
        }

        FMap = Map;
        FMap.Mouse.SetCapture(this);
        FMap.Mouse.OnDown += MouseDownEvent;
        FMap.Mouse.OnMove += MouseMoveEvent;
        FMap.Mouse.OnCaptureChanged += MouseDownEvent;
        FMap.Mouse.CursorMode = 256;
        UpdateMoveCursor();
        FStatus = FStatus + Set<MoveSelectorStatusElement>.Of(MoveSelectorStatusElement.Activated);
        FMap.ShowHexagonSelector();
    }

    // Structur.PAS:4239
    public virtual void Deactivate()
    {
        if (FStatus.Contains(MoveSelectorStatusElement.Activated))
        {
            FStatus = FStatus - Set<MoveSelectorStatusElement>.Of(MoveSelectorStatusElement.Activated);
            FMap.Mouse.OnDown -= MouseDownEvent;
            FMap.Mouse.OnMove -= MouseMoveEvent;
            FMap.Mouse.OnCaptureChanged -= MouseDownEvent;
            if (FMap.Mouse.Capture == this)
            {
                FMap.Mouse.SetCapture(null);
            }

            FMap.HideHexagonSelector();
            UpdateMoveCursor();
        }
    }

    // Structur.PAS:4253
    public virtual void ShowMovePaths(in LockedSurface Surface)
    {
        int X1;
        int Y1;
        int X2;
        int Y2;
        int J;
        int ImageIndex;
        int MaxX;
        int MaxY;
        int Xhp = default;
        int Yhp = default;
        int PointsLeft;
        int Direction;
        MoveSettings Settings;
        MapField MapField;
        if (FStatus.Contains(MoveSelectorStatusElement.Activated) && FMoveSettingsList.Count != 0)
        {
            MaxX = this.FMap.ViewXhx + this.FMap.ViewWidthHX - 1;
            MaxY = this.FMap.ViewYhx + this.FMap.ViewHeightHX - 1;

            for (int I = 0, last = FMoveSettingsList.Count - 1; I <= last; I++)
            {
                Settings = FMoveSettingsList.GetMoveSettings(I);
                PointsLeft = Settings.FPathMovePoints;
                if (Settings.FPath.Count > 1)
                {
                    J = 0;
                    int limit = Settings.FPath.Count - 2;
                    for (; J <= limit; J += 1)
                    {
                        if (Settings.FPath[J].X >= FMap.ViewXhx && Settings.FPath[J].X <= MaxX && Settings.FPath[J].Y >= FMap.ViewYhx && Settings.FPath[J].Y <= MaxY && Settings.FPath[J].L == FMap.ViewL)
                        {
                            X1 = Settings.FPath[J].X;
                            Y1 = Settings.FPath[J].Y;
                            X2 = Settings.FPath[J + 1].X;
                            Y2 = Settings.FPath[J + 1].Y;
                            if (HexGeometry.dHXtoRad(X1, Y1, X2, Y2) == 1)
                            {
                                Direction = HexGeometry.dHXtoHNfast(X1, Y1, X2, Y2);
                                if (Direction != 0)
                                {
                                    if (PointsLeft <= Settings.FMovePoints)
                                    {
                                        ImageIndex = Direction + 34;
                                    }
                                    else
                                    {
                                        ImageIndex = Direction + 41;
                                    }

                                    HexGeometry.HXtoHP(X1, Y1, ref Xhp, ref Yhp);
                                    if (FCurrentMove == I || FCurrentMove == -1)
                                    {
                                        AoWHSSet.Current.FCursorImages[ImageIndex].Show(Xhp - FMap.ViewXhp - 8, Yhp - FMap.ViewYhp - 8, Surface);
                                    }
                                    else
                                    {
                                        AoWHSSet.Current.FCursorImages[ImageIndex].ShowTransparent(Xhp - FMap.ViewXhp - 8, Yhp - FMap.ViewYhp - 8, Surface);
                                    }
                                }
                            }
                            else
                            {
                                if (FCurrentMove == I)
                                {
                                    if (PointsLeft <= Settings.FMovePoints)
                                    {
                                        ImageIndex = 41;
                                    }
                                    else
                                    {
                                        ImageIndex = 48;
                                    }

                                    HexGeometry.HXtoHP(X1, Y1, ref Xhp, ref Yhp);
                                    AoWHSSet.Current.FCursorImages[ImageIndex].Show(Xhp - FMap.ViewXhp - 8, Yhp - FMap.ViewYhp - 8, Surface);
                                    if (PointsLeft - Settings.FPath[J].Flag != 0 && Settings.FPath[J + 1].L == FMap.ViewL)
                                    {
                                        if (PointsLeft - Settings.FPath[J].Flag <= Settings.FMovePoints)
                                        {
                                            ImageIndex = 41;
                                        }
                                        else
                                        {
                                            ImageIndex = 48;
                                        }

                                        HexGeometry.HXtoHP(X2, Y2, ref Xhp, ref Yhp);
                                        AoWHSSet.Current.FCursorImages[ImageIndex].Show(Xhp - FMap.ViewXhp - 8, Yhp - FMap.ViewYhp - 8, Surface);
                                    }
                                }
                                else
                                {
                                    if (PointsLeft <= Settings.FMovePoints)
                                    {
                                        ImageIndex = 41;
                                    }
                                    else
                                    {
                                        ImageIndex = 48;
                                    }

                                    HexGeometry.HXtoHP(X1, Y1, ref Xhp, ref Yhp);
                                    AoWHSSet.Current.FCursorImages[ImageIndex].ShowTransparent(Xhp - FMap.ViewXhp - 8, Yhp - FMap.ViewYhp - 8, Surface);
                                    if (PointsLeft - Settings.FPath[J].Flag <= Settings.FMovePoints)
                                    {
                                        ImageIndex = 41;
                                    }
                                    else
                                    {
                                        ImageIndex = 48;
                                    }

                                    HexGeometry.HXtoHP(X2, Y2, ref Xhp, ref Yhp);
                                    AoWHSSet.Current.FCursorImages[ImageIndex].ShowTransparent(Xhp - FMap.ViewXhp - 8, Yhp - FMap.ViewYhp - 8, Surface);
                                }
                            }
                        }

                        PointsLeft -= Settings.FPath[J].Flag;
                    }

                    if (Settings.FPath[0].X >= FMap.ViewXhx && Settings.FPath[0].X <= MaxX && Settings.FPath[0].Y >= FMap.ViewYhx && Settings.FPath[0].Y <= MaxY && Settings.FPath[0].L == FMap.ViewL)
                    {
                        var item = Settings.FPath[0];
                        var fPathItem = Settings.FPath[0];
                        var item2 = Settings.FPath[0];
                        MapField = FMap.MapContainer.GetField(item2.X, fPathItem.Y, item.L);
                        HexGeometry.HXtoHP(MapField.Xhx, MapField.Yhx, ref Xhp, ref Yhp);
                        Xhp = Xhp - FMap.ViewXhp - 8;
                        Yhp = Yhp - FMap.ViewYhp - 8;
                        if (Settings.FMovePoints < Settings.FPathMovePoints)
                        {
                            ImageIndex = 48;
                            AoWHSSet.Current.FCursorImages[ImageIndex].Show(Xhp, Yhp, Surface);
                            // { Original sets the font color before checking the font for nil. }
                            GameTables.AoWEngine.FCombatFont.Color = 0;
                            if (GameTables.AoWEngine.FCombatFont != null)
                            {
                                GameTables.AoWEngine.FCombatFont.SetTextCentered(Xhp + 25, Yhp + 9, IntToStr((Settings.FPathMovePoints - Settings.FMovePoints - 1) / Settings.FMoves + 1), Surface);
                            }
                        }
                        else
                        {
                            ImageIndex = 41;
                            if (FCurrentMove == I || FCurrentMove == -1)
                            {
                                AoWHSSet.Current.FCursorImages[ImageIndex].Show(Xhp, Yhp, Surface);
                            }
                            else
                            {
                                AoWHSSet.Current.FCursorImages[ImageIndex].ShowTransparent(Xhp, Yhp, Surface);
                            }
                        }
                    }

                    if (Settings.FMovePoints < Settings.FPathMovePoints)
                    {
                        J = Settings.FPath.MovePointsToPos(Settings.FMovePoints);
                        if (J > 0 && J < Settings.FPath.Count - 1 && Settings.FPath[J].X >= FMap.ViewXhx && Settings.FPath[J].X <= MaxX && Settings.FPath[J].Y >= FMap.ViewYhx && Settings.FPath[J].Y <= MaxY && Settings.FPath[J].L == FMap.ViewL)
                        {
                            var item = Settings.FPath[J];
                            HexGeometry.HXtoHP(Settings.FPath[J].X, item.Y, ref Xhp, ref Yhp);
                            Xhp = Xhp - FMap.ViewXhp - 8;
                            Yhp = Yhp - FMap.ViewYhp - 8;
                            ImageIndex = 41;
                            if (FCurrentMove == I || FCurrentMove == -1)
                            {
                                AoWHSSet.Current.FCursorImages[ImageIndex].Show(Xhp, Yhp, Surface);
                            }
                            else
                            {
                                AoWHSSet.Current.FCursorImages[ImageIndex].ShowTransparent(Xhp, Yhp, Surface);
                            }
                        }
                    }
                }
                else
                {
                    if (Settings.FPath.Count == 1 && AoWHSMap.Current.FCombat != null)
                    {
                        var item = Settings.FPath[0];
                        var fPathItem = Settings.FPath[0];
                        var item2 = Settings.FPath[0];
                        MapField = FMap.MapContainer.GetField(item2.X, fPathItem.Y, item.L);
                        HexGeometry.HXtoHP(MapField.Xhx, MapField.Yhx, ref Xhp, ref Yhp);
                        Xhp = Xhp - FMap.ViewXhp - 8;
                        Yhp = Yhp - FMap.ViewYhp - 8;
                        AoWHSSet.Current.FCursorImages[41].Show(Xhp, Yhp, Surface);
                    }
                }
            }
        }
    }

    // Structur.PAS:4109
    // control settings, moves, current move, map, status

    // Structur.PAS:4131
    public void Clear()
    {
        FMoveControlSettings.Clear();
        FMoveSettingsList.Clear();
        FCurrentMove = -1;
    }

    // Structur.PAS:4157
    public int AddMoveObject(HexSprite Obj)
    {
        MoveSettings Settings;
        Settings = new MoveSettings(null);
        Settings.FMoveObject = Obj;
        Settings.SetOwner(FMoveSettingsList);
        return FMoveSettingsList.Count - 1;
    }

    // Structur.PAS:4167
    public void DeleteMoveObject(int Index)
    {
        object Obj;
        if (FCurrentMove != -1)
        {
            Obj = FMoveSettingsList.GetMoveSettings(FCurrentMove).FMoveObject;
        }
        else
        {
            Obj = null;
        }

        (
        ((object)FMoveSettingsList.FChildren.List[Index]) as IDisposable)?.Dispose();
        if (Obj != null)
        {
            Index = FMoveSettingsList.IndexOfMoveObject(Obj);
            if (Index == -1)
            {
                SetCurrentMove(FMoveSettingsList.Count - 1);
            }
            else
            {
                SetCurrentMove(Index);
            }
        }
    }

    // Structur.PAS:4183
    public void DeleteMoves()
    {
        while (FMoveSettingsList.Count != 0)
        {
            DeleteMoveObject(FMoveSettingsList.Count - 1);
        }
    }

    // intf_P1_04.inc:237

}
