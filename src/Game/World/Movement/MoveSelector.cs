#nullable disable warnings
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Graphics;

using Aow.Rtl;

namespace Aow.Game.World.Movement;

public class MoveSelector : IDisposable
{
    // Structur.PAS:4109
    // control settings, moves, current move, map, status
    public MoveSelector(EngineObject creator)
    {
        Creator = creator;
        MoveControlSettings = CreateMoveControlSettings();
        MoveSettingsList = new MoveSettingsList(null) { Holder = Creator.CreationHolder() };
        CurrentMove = -1;
        Map = null;
    }

    // The node that created this selector. It and the nodes it creates reach the engine through it.
    public EngineObject Creator { get; }

    public MoveControlSettings MoveControlSettings { get; }
    public MoveSettingsList MoveSettingsList { get; }
    public int CurrentMove { get; protected set; }
    public GameMap? Map { get; private set; }
    public MoveSelectorStatuses Status { get; private set; }

    // Structur.PAS:4118
    public virtual void Dispose()
    {
        Clear();
        _ = MoveControlSettings.Release();
        MoveSettingsList?.Dispose();
    }

    // Structur.PAS:4192
    public virtual void MouseMoveEvent(object sender) => UpdateMoveCursor();

    // Structur.PAS:4214
    public virtual void MouseDownEvent(object sender)
    {
        HexSpriteMapMouse mouse = Map.Mouse;
        object? capture = mouse.Capture;
        if ((capture != this && capture != Creator.GetEngine().StrategicMap.TargetSelection) || mouse.Button == MouseButton.Right)
        {
            Unselect();
        }
        else
        {
            HexSpriteMapMouse hexSpriteMapMouse = Map.Mouse;
            if (capture == this && hexSpriteMapMouse.Button == MouseButton.Left)
            {
                int hexY = Map.Mouse.HexY;
                SelectPath(hexSpriteMapMouse.HexX.TruncateToSByte(), hexY.TruncateToSByte(), L: Map.ViewMapLevel.TruncateToSByte());
            }
        }
    }

    // Structur.PAS:4197
    public virtual void CalculateMovePath(int index, int X, int Y, int L)
    {
    }

    // Structur.PAS:4189
    public virtual void UpdateMoveCursor()
    {
    }

    // Structur.PAS:4126
    public virtual MoveControlSettings CreateMoveControlSettings() => new(null) { Holder = Creator.CreationHolder() };

    // Structur.PAS:4138
    public virtual void SetCurrentMove(int value) => CurrentMove = value;

    // Structur.PAS:4200
    public virtual void SelectPath(int X, int Y, int L)
    {
        if (CurrentMove != -1 && X != -1)
        {
            MoveSettings moveSettings = MoveSettingsList.GetMoveSettings(CurrentMove);
            if (moveSettings.Path.Count != 0 && moveSettings.Path[0].Location.X == X && moveSettings.Path[0].Location.Y == Y && moveSettings.Path[0].Location.MapLevel == L)
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
    public virtual void Activate(GameMap? map)
    {
        if (Status.Contains(MoveSelectorStatuses.Activated))
        {
            Deactivate();
        }

        Map = map;
        Map.Mouse.SetCapture(this);
        Map.Mouse.ButtonPressed += MouseDownEvent;
        Map.Mouse.Moved += MouseMoveEvent;
        Map.Mouse.CaptureChanged += MouseDownEvent;
        Map.Mouse.CursorMode = 256;
        UpdateMoveCursor();
        Status |= MoveSelectorStatuses.Activated;
        Map.ShowHexSelector();
    }

    // Structur.PAS:4239
    public virtual void Deactivate()
    {
        if (Status.Contains(MoveSelectorStatuses.Activated))
        {
            Status &= ~MoveSelectorStatuses.Activated;
            Map.Mouse.ButtonPressed -= MouseDownEvent;
            Map.Mouse.Moved -= MouseMoveEvent;
            Map.Mouse.CaptureChanged -= MouseDownEvent;
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
        if (Status.Contains(MoveSelectorStatuses.Activated) && MoveSettingsList.Count != 0)
        {
            int maxX = Map.ViewHexX + Map.ViewWidthHex - 1;
            int maxY = Map.ViewHexY + Map.ViewHeightHex - 1;

            for (int i = 0, last = MoveSettingsList.Count - 1; i <= last; i++)
            {
                MoveSettings settings = MoveSettingsList.GetMoveSettings(i);
                int pointsLeft = settings.PathMovePoints;
                int pixelX;
                int pixelY;
                MapField mapField;
                if (settings.Path.Count > 1)
                {
                    int j = 0;
                    int limit = settings.Path.Count - 2;
                    int imageIndex;
                    for (; j <= limit; j += 1)
                    {
                        if (settings.Path[j].Location.X >= Map.ViewHexX && settings.Path[j].Location.X <= maxX && settings.Path[j].Location.Y >= Map.ViewHexY && settings.Path[j].Location.Y <= maxY && settings.Path[j].Location.MapLevel == Map.ViewMapLevel)
                        {
                            int x1 = settings.Path[j].Location.X;
                            int y1 = settings.Path[j].Location.Y;
                            int x2 = settings.Path[j + 1].Location.X;
                            int y2 = settings.Path[j + 1].Location.Y;
                            if (HexGeometry.DeltaHexToRadius(x1, y1, x2, y2) == 1)
                            {
                                int direction = HexGeometry.DeltaHexToHexNumberFast(x1, y1, x2, y2);
                                if (direction != 0)
                                {
                                    imageIndex = pointsLeft <= settings.MovePoints ? direction + 34 : direction + 41;

                                    HexGeometry.HexToPixel(x1, y1, out pixelX, out pixelY);
                                    if (CurrentMove == i || CurrentMove == -1)
                                    {
                                        Creator.GetEngine().GamePack.CursorImages[imageIndex].Show(pixelX - Map.ViewPixelX - 8, pixelY - Map.ViewPixelY - 8, lockedSurface: surface);
                                    }
                                    else
                                    {
                                        Creator.GetEngine().GamePack.CursorImages[imageIndex].ShowTransparent(pixelX - Map.ViewPixelX - 8, pixelY - Map.ViewPixelY - 8, lockedSurface: surface);
                                    }
                                }
                            }
                            else
                            {
                                if (CurrentMove == i)
                                {
                                    imageIndex = pointsLeft <= settings.MovePoints ? 41 : 48;

                                    HexGeometry.HexToPixel(x1, y1, out pixelX, out pixelY);
                                    GamePack gamePack = Creator.GetEngine().GamePack;
                                    gamePack.CursorImages[imageIndex].Show(pixelX - Map.ViewPixelX - 8, pixelY - Map.ViewPixelY - 8, lockedSurface: surface);
                                    if (pointsLeft - settings.Path[j].MoveCost != 0 && settings.Path[j + 1].Location.MapLevel == Map.ViewMapLevel)
                                    {
                                        imageIndex = pointsLeft - settings.Path[j].MoveCost <= settings.MovePoints ? 41 : 48;

                                        HexGeometry.HexToPixel(x2, y2, out pixelX, out pixelY);
                                        gamePack.CursorImages[imageIndex].Show(pixelX - Map.ViewPixelX - 8, pixelY - Map.ViewPixelY - 8, lockedSurface: surface);
                                    }
                                }
                                else
                                {
                                    imageIndex = pointsLeft <= settings.MovePoints ? 41 : 48;

                                    HexGeometry.HexToPixel(x1, y1, out pixelX, out pixelY);
                                    GamePack gamePack = Creator.GetEngine().GamePack;
                                    gamePack.CursorImages[imageIndex].ShowTransparent(pixelX - Map.ViewPixelX - 8, pixelY - Map.ViewPixelY - 8, lockedSurface: surface);
                                    imageIndex = pointsLeft - settings.Path[j].MoveCost <= settings.MovePoints ? 41 : 48;

                                    HexGeometry.HexToPixel(x2, y2, out pixelX, out pixelY);
                                    gamePack.CursorImages[imageIndex].ShowTransparent(pixelX - Map.ViewPixelX - 8, pixelY - Map.ViewPixelY - 8, lockedSurface: surface);
                                }
                            }
                        }

                        pointsLeft -= settings.Path[j].MoveCost;
                    }

                    if (settings.Path[0].Location.X >= Map.ViewHexX && settings.Path[0].Location.X <= maxX && settings.Path[0].Location.Y >= Map.ViewHexY && settings.Path[0].Location.Y <= maxY && settings.Path[0].Location.MapLevel == Map.ViewMapLevel)
                    {
                        PathStep item = settings.Path[0];
                        PathStep fPathItem = settings.Path[0];
                        PathStep item2 = settings.Path[0];
                        mapField = Map.MapContainer.GetField(item2.Location.X, fPathItem.Location.Y, item.Location.MapLevel);
                        HexGeometry.HexToPixel(mapField.HexX, mapField.HexY, out pixelX, out pixelY);
                        pixelX = pixelX - Map.ViewPixelX - 8;
                        pixelY = pixelY - Map.ViewPixelY - 8;
                        if (settings.MovePoints < settings.PathMovePoints)
                        {
                            imageIndex = 48;
                            Creator.GetEngine().GamePack.CursorImages[imageIndex].Show(pixelX, pixelY, lockedSurface: surface);
                            // { Original sets the font color before checking the font for nil. }
                            Creator.GetEngine().CombatFont.Color = Color.Black;
                            Creator.GetEngine().CombatFont?.SetTextCentered(pixelX + 25, pixelY + 9, text: (((settings.PathMovePoints - settings.MovePoints - 1) / settings.Moves) + 1).ToString(CultureInfo.InvariantCulture), lockedSurface: surface);
                        }
                        else
                        {
                            imageIndex = 41;
                            if (CurrentMove == i || CurrentMove == -1)
                            {
                                Creator.GetEngine().GamePack.CursorImages[imageIndex].Show(pixelX, pixelY, lockedSurface: surface);
                            }
                            else
                            {
                                Creator.GetEngine().GamePack.CursorImages[imageIndex].ShowTransparent(pixelX, pixelY, lockedSurface: surface);
                            }
                        }
                    }

                    if (settings.MovePoints < settings.PathMovePoints)
                    {
                        j = settings.Path.MovePointsToPosition(settings.MovePoints);
                        if (j > 0 && j < settings.Path.Count - 1 && settings.Path[j].Location.X >= Map.ViewHexX && settings.Path[j].Location.X <= maxX && settings.Path[j].Location.Y >= Map.ViewHexY && settings.Path[j].Location.Y <= maxY && settings.Path[j].Location.MapLevel == Map.ViewMapLevel)
                        {
                            PathStep item = settings.Path[j];
                            HexGeometry.HexToPixel(settings.Path[j].Location.X, item.Location.Y, out pixelX, out pixelY);
                            pixelX = pixelX - Map.ViewPixelX - 8;
                            pixelY = pixelY - Map.ViewPixelY - 8;
                            imageIndex = 41;
                            if (CurrentMove == i || CurrentMove == -1)
                            {
                                Creator.GetEngine().GamePack.CursorImages[imageIndex].Show(pixelX, pixelY, lockedSurface: surface);
                            }
                            else
                            {
                                Creator.GetEngine().GamePack.CursorImages[imageIndex].ShowTransparent(pixelX, pixelY, lockedSurface: surface);
                            }
                        }
                    }
                }
                else
                {
                    if (settings.Path.Count == 1 && Creator.GetEngine().StrategicMap.Combat is not null)
                    {
                        PathStep item = settings.Path[0];
                        PathStep fPathItem = settings.Path[0];
                        PathStep item2 = settings.Path[0];
                        mapField = Map.MapContainer.GetField(item2.Location.X, fPathItem.Location.Y, item.Location.MapLevel);
                        HexGeometry.HexToPixel(mapField.HexX, mapField.HexY, out pixelX, out pixelY);
                        pixelX = pixelX - Map.ViewPixelX - 8;
                        pixelY = pixelY - Map.ViewPixelY - 8;
                        Creator.GetEngine().GamePack.CursorImages[41].Show(pixelX, pixelY, lockedSurface: surface);
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
    public int AddMoveObject(HexSprite? objectValue)
    {
        MoveSettings settings = new(null)
        {
            Holder = Creator.CreationHolder(),
            MoveObject = objectValue
        };
        settings.SetOwner(MoveSettingsList);
        return MoveSettingsList.Count - 1;
    }

    // Structur.PAS:4167
    public void DeleteMoveObject(int index)
    {
        object? objectValue = CurrentMove != -1 ? MoveSettingsList.GetMoveSettings(CurrentMove).MoveObject : null;
        (MoveSettingsList[index] as IDisposable)?.Dispose();
        if (objectValue is not null)
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
}
