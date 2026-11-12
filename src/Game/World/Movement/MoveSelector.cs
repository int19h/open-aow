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
        MoveControlSettings = CreateMoveControlSettings();
        MoveSettingsList = new MoveSettingsList(null);
        CurrentMove = -1;
        Map = null;
    }

    public MoveControlSettings MoveControlSettings { get; }
    public MoveSettingsList MoveSettingsList { get; }
    public int CurrentMove { get; protected set; }
    public GameMap Map { get; private set; }
    public Set<MoveSelectorStatusElement> Status { get; private set; }

    // Structur.PAS:4118
    public virtual void Dispose()
    {
        Clear();
        MoveControlSettings.Release();
        MoveSettingsList?.Dispose();
    }

    // Structur.PAS:4192
    public virtual void MouseMoveEvent(object sender)
    {
        UpdateMoveCursor();
    }

    // Structur.PAS:4214
    public virtual void MouseDownEvent(object sender)
    {
        object capture;
        HexSpriteMapMouse mouse;
        mouse = Map.Mouse;
        capture = mouse.Capture;
        if ((capture != this && capture != StrategicMap.Current.TargetSelection) || mouse.Button == MouseButton.Right)
        {
            Unselect();
        }
        else
        {
            var hexSpriteMapMouse = Map.Mouse;
            if (capture == this && hexSpriteMapMouse.Button == MouseButton.Left)
            {
                var hexY = Map.Mouse.HexY;
                SelectPath((sbyte)hexSpriteMapMouse.HexX, (sbyte)hexY, (sbyte)Map.ViewL);
            }
        }
    }

    // Structur.PAS:4197
    public virtual void CalculateMovePath(int index, sbyte X, sbyte Y, sbyte L)
    {
    }

    // Structur.PAS:4189
    public virtual void UpdateMoveCursor()
    {
    }

    // Structur.PAS:4126
    public virtual MoveControlSettings CreateMoveControlSettings() => new MoveControlSettings(null);

    // Structur.PAS:4138
    public virtual void SetCurrentMove(int value)
    {
        CurrentMove = value;
    }

    // Structur.PAS:4200
    public virtual void SelectPath(sbyte X, sbyte Y, sbyte L)
    {
        if (CurrentMove != -1 && X != -1)
        {
            var moveSettings = MoveSettingsList.GetMoveSettings(CurrentMove);
            if (moveSettings.Path.Count != 0 && moveSettings.Path[0].X == X && moveSettings.Path[0].Y == Y && moveSettings.Path[0].MapLevel == L)
            {
                ExecuteMove();
            }
            else
            {
                if (ValidateSelection(Map.MapContainer.GetField(X, Y, L)))
                {
                    CalculateMovePath(CurrentMove, X, Y, L);
                }
                else
                {
                    moveSettings.Path.Clear();
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
    public virtual bool ValidateSelection(MapField field) => true;

    // Structur.PAS:4154
    public virtual void ExecuteMove()
    {
    }

    // Structur.PAS:4225
    public virtual void Activate(GameMap map)
    {
        if (Status.Contains(MoveSelectorStatusElement.Activated))
        {
            Deactivate();
        }

        Map = map;
        Map.Mouse.SetCapture(this);
        Map.Mouse.OnDown += MouseDownEvent;
        Map.Mouse.OnMove += MouseMoveEvent;
        Map.Mouse.OnCaptureChanged += MouseDownEvent;
        Map.Mouse.CursorMode = 256;
        UpdateMoveCursor();
        Status = Status + Set<MoveSelectorStatusElement>.Of(MoveSelectorStatusElement.Activated);
        Map.ShowHexSelector();
    }

    // Structur.PAS:4239
    public virtual void Deactivate()
    {
        if (Status.Contains(MoveSelectorStatusElement.Activated))
        {
            Status = Status - Set<MoveSelectorStatusElement>.Of(MoveSelectorStatusElement.Activated);
            Map.Mouse.OnDown -= MouseDownEvent;
            Map.Mouse.OnMove -= MouseMoveEvent;
            Map.Mouse.OnCaptureChanged -= MouseDownEvent;
            if (Map.Mouse.Capture == this)
            {
                Map.Mouse.SetCapture(null);
            }

            Map.HideHexSelector();
            UpdateMoveCursor();
        }
    }

    // Structur.PAS:4253
    public virtual void ShowMovePaths(in LockedSurface surface)
    {
        int x1;
        int y1;
        int x2;
        int y2;
        int J;
        int imageIndex;
        int maxX;
        int maxY;
        int pixelX = default;
        int pixelY = default;
        int pointsLeft;
        int direction;
        MoveSettings settings;
        MapField mapField;
        if (Status.Contains(MoveSelectorStatusElement.Activated) && MoveSettingsList.Count != 0)
        {
            maxX = this.Map.ViewHexX + this.Map.ViewWidthHex - 1;
            maxY = this.Map.ViewHexY + this.Map.ViewHeightHex - 1;

            for (int I = 0, last = MoveSettingsList.Count - 1; I <= last; I++)
            {
                settings = MoveSettingsList.GetMoveSettings(I);
                pointsLeft = settings.PathMovePoints;
                if (settings.Path.Count > 1)
                {
                    J = 0;
                    int limit = settings.Path.Count - 2;
                    for (; J <= limit; J += 1)
                    {
                        if (settings.Path[J].X >= Map.ViewHexX && settings.Path[J].X <= maxX && settings.Path[J].Y >= Map.ViewHexY && settings.Path[J].Y <= maxY && settings.Path[J].MapLevel == Map.ViewL)
                        {
                            x1 = settings.Path[J].X;
                            y1 = settings.Path[J].Y;
                            x2 = settings.Path[J + 1].X;
                            y2 = settings.Path[J + 1].Y;
                            if (HexGeometry.DeltaHexToRadius(x1, y1, x2, y2) == 1)
                            {
                                direction = HexGeometry.DeltaHexToHexNumberFast(x1, y1, x2, y2);
                                if (direction != 0)
                                {
                                    if (pointsLeft <= settings.MovePoints)
                                    {
                                        imageIndex = direction + 34;
                                    }
                                    else
                                    {
                                        imageIndex = direction + 41;
                                    }

                                    HexGeometry.HexToPixel(x1, y1, ref pixelX, ref pixelY);
                                    if (CurrentMove == I || CurrentMove == -1)
                                    {
                                        GamePack.Current.CursorImages[imageIndex].Show(pixelX - Map.ViewPixelX - 8, pixelY - Map.ViewPixelY - 8, surface);
                                    }
                                    else
                                    {
                                        GamePack.Current.CursorImages[imageIndex].ShowTransparent(pixelX - Map.ViewPixelX - 8, pixelY - Map.ViewPixelY - 8, surface);
                                    }
                                }
                            }
                            else
                            {
                                if (CurrentMove == I)
                                {
                                    if (pointsLeft <= settings.MovePoints)
                                    {
                                        imageIndex = 41;
                                    }
                                    else
                                    {
                                        imageIndex = 48;
                                    }

                                    HexGeometry.HexToPixel(x1, y1, ref pixelX, ref pixelY);
                                    GamePack.Current.CursorImages[imageIndex].Show(pixelX - Map.ViewPixelX - 8, pixelY - Map.ViewPixelY - 8, surface);
                                    if (pointsLeft - settings.Path[J].Tag != 0 && settings.Path[J + 1].MapLevel == Map.ViewL)
                                    {
                                        if (pointsLeft - settings.Path[J].Tag <= settings.MovePoints)
                                        {
                                            imageIndex = 41;
                                        }
                                        else
                                        {
                                            imageIndex = 48;
                                        }

                                        HexGeometry.HexToPixel(x2, y2, ref pixelX, ref pixelY);
                                        GamePack.Current.CursorImages[imageIndex].Show(pixelX - Map.ViewPixelX - 8, pixelY - Map.ViewPixelY - 8, surface);
                                    }
                                }
                                else
                                {
                                    if (pointsLeft <= settings.MovePoints)
                                    {
                                        imageIndex = 41;
                                    }
                                    else
                                    {
                                        imageIndex = 48;
                                    }

                                    HexGeometry.HexToPixel(x1, y1, ref pixelX, ref pixelY);
                                    GamePack.Current.CursorImages[imageIndex].ShowTransparent(pixelX - Map.ViewPixelX - 8, pixelY - Map.ViewPixelY - 8, surface);
                                    if (pointsLeft - settings.Path[J].Tag <= settings.MovePoints)
                                    {
                                        imageIndex = 41;
                                    }
                                    else
                                    {
                                        imageIndex = 48;
                                    }

                                    HexGeometry.HexToPixel(x2, y2, ref pixelX, ref pixelY);
                                    GamePack.Current.CursorImages[imageIndex].ShowTransparent(pixelX - Map.ViewPixelX - 8, pixelY - Map.ViewPixelY - 8, surface);
                                }
                            }
                        }

                        pointsLeft -= settings.Path[J].Tag;
                    }

                    if (settings.Path[0].X >= Map.ViewHexX && settings.Path[0].X <= maxX && settings.Path[0].Y >= Map.ViewHexY && settings.Path[0].Y <= maxY && settings.Path[0].MapLevel == Map.ViewL)
                    {
                        var item = settings.Path[0];
                        var fPathItem = settings.Path[0];
                        var item2 = settings.Path[0];
                        mapField = Map.MapContainer.GetField(item2.X, fPathItem.Y, item.MapLevel);
                        HexGeometry.HexToPixel(mapField.HexX, mapField.HexY, ref pixelX, ref pixelY);
                        pixelX = pixelX - Map.ViewPixelX - 8;
                        pixelY = pixelY - Map.ViewPixelY - 8;
                        if (settings.MovePoints < settings.PathMovePoints)
                        {
                            imageIndex = 48;
                            GamePack.Current.CursorImages[imageIndex].Show(pixelX, pixelY, surface);
                            // { Original sets the font color before checking the font for nil. }
                            GameTables.GameEngine.CombatFont.Color = 0;
                            if (GameTables.GameEngine.CombatFont != null)
                            {
                                GameTables.GameEngine.CombatFont.SetTextCentered(pixelX + 25, pixelY + 9, IntToStr((settings.PathMovePoints - settings.MovePoints - 1) / settings.Moves + 1), surface);
                            }
                        }
                        else
                        {
                            imageIndex = 41;
                            if (CurrentMove == I || CurrentMove == -1)
                            {
                                GamePack.Current.CursorImages[imageIndex].Show(pixelX, pixelY, surface);
                            }
                            else
                            {
                                GamePack.Current.CursorImages[imageIndex].ShowTransparent(pixelX, pixelY, surface);
                            }
                        }
                    }

                    if (settings.MovePoints < settings.PathMovePoints)
                    {
                        J = settings.Path.MovePointsToPosition(settings.MovePoints);
                        if (J > 0 && J < settings.Path.Count - 1 && settings.Path[J].X >= Map.ViewHexX && settings.Path[J].X <= maxX && settings.Path[J].Y >= Map.ViewHexY && settings.Path[J].Y <= maxY && settings.Path[J].MapLevel == Map.ViewL)
                        {
                            var item = settings.Path[J];
                            HexGeometry.HexToPixel(settings.Path[J].X, item.Y, ref pixelX, ref pixelY);
                            pixelX = pixelX - Map.ViewPixelX - 8;
                            pixelY = pixelY - Map.ViewPixelY - 8;
                            imageIndex = 41;
                            if (CurrentMove == I || CurrentMove == -1)
                            {
                                GamePack.Current.CursorImages[imageIndex].Show(pixelX, pixelY, surface);
                            }
                            else
                            {
                                GamePack.Current.CursorImages[imageIndex].ShowTransparent(pixelX, pixelY, surface);
                            }
                        }
                    }
                }
                else
                {
                    if (settings.Path.Count == 1 && StrategicMap.Current.Combat != null)
                    {
                        var item = settings.Path[0];
                        var fPathItem = settings.Path[0];
                        var item2 = settings.Path[0];
                        mapField = Map.MapContainer.GetField(item2.X, fPathItem.Y, item.MapLevel);
                        HexGeometry.HexToPixel(mapField.HexX, mapField.HexY, ref pixelX, ref pixelY);
                        pixelX = pixelX - Map.ViewPixelX - 8;
                        pixelY = pixelY - Map.ViewPixelY - 8;
                        GamePack.Current.CursorImages[41].Show(pixelX, pixelY, surface);
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
        MoveControlSettings.Clear();
        MoveSettingsList.Clear();
        CurrentMove = -1;
    }

    // Structur.PAS:4157
    public int AddMoveObject(HexSprite objectValue)
    {
        MoveSettings settings;
        settings = new MoveSettings(null);
        settings.MoveObject = objectValue;
        settings.SetOwner(MoveSettingsList);
        return MoveSettingsList.Count - 1;
    }

    // Structur.PAS:4167
    public void DeleteMoveObject(int index)
    {
        object objectValue;
        if (CurrentMove != -1)
        {
            objectValue = MoveSettingsList.GetMoveSettings(CurrentMove).MoveObject;
        }
        else
        {
            objectValue = null;
        }

        (MoveSettingsList[index] as IDisposable)?.Dispose();
        if (objectValue != null)
        {
            index = MoveSettingsList.IndexOfMoveObject(objectValue);
            if (index == -1)
            {
                SetCurrentMove(MoveSettingsList.Count - 1);
            }
            else
            {
                SetCurrentMove(index);
            }
        }
    }

    // Structur.PAS:4183
    public void DeleteMoves()
    {
        while (MoveSettingsList.Count != 0)
        {
            DeleteMoveObject(MoveSettingsList.Count - 1);
        }
    }

    // intf_P1_04.inc:237

}
