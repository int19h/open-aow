#nullable disable warnings
// Translated from Scanner.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Grids;

// { ot=1 }
// half-row inset, libraries, icons, viewport, change event, update steps, map, frame library,
// frame index, map dimensions, level, icons drawn, previous size, drawable height, terrain/icon
// surfaces, update step, incremental
public sealed class Scanner(Component owner) : Control(owner)
{

    private int _halfRowInset;
    private ImageLibrary _tilesImageLibrary;
    private ImageLibrary _highlightImageLibrary;
    private ImageLibrary _fogImageLibrary;
    private ImageLibrary _objectsImageLibrary;

    public int TTop { get; private set; } = -1;

    private int _tHeight = -1;

    public int TLeft { get; private set; } = -1;

    private int _tWidth = -1;
    private bool _buildings;
    private bool _armies;
    private bool _naturals;
    [Published]
    public event NotifyEvent<Scanner>? ChangeTileBox;
    private int _steps = 1;
    private GameMap? _map;
    private InterfaceImageLibrary? _imageLibraryFrame;
    private int _imageLibraryIndexFrame;
    private int _mapWidth;
    private int _mapHeight;

    public MapLevel Level { get; private set; }

    private bool _objectsDrawn;

    public int PreviousWidth { get; set; }

    private int _previousHeight;
    private int _drawableHeight;
    private Surface? _mapSurface;
    private Surface _objectSurface;

    public int UpdateStep { get; set; }
    public bool Incremental { get; set; }

    // Scanner.pas:126
    public override void Dispose()
    {
        _mapSurface?.Dispose();

        _objectSurface?.Dispose();

        base.Dispose();
    }

    // Scanner.pas:398
    public override void Validate()
    {
        InterfaceImageLibrary? frame = _imageLibraryFrame;
        if (frame is not null)
        {
            _imageLibraryFrame = frame.CheckImageLibrary(_imageLibraryIndexFrame, count: 8, sender: this);
        }

        if (_imageLibraryFrame is not null && _tilesImageLibrary is not null && _objectsImageLibrary is not null && _map is not null && _fogImageLibrary is not null && _highlightImageLibrary is not null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // Scanner.pas:409
    public override void SetSize()
    {
        if ((WinWidth != PreviousWidth || WinHeight != _previousHeight) && WinWidth > 0 && WinHeight > 0)
        {
            _halfRowInset = WinHeight / _mapHeight / 2;
            _drawableHeight = WinHeight - _halfRowInset;
            if (_mapSurface is not null)
            {
                _ = _objectSurface.Resize(WinWidth, WinHeight);
                _ = _objectSurface.Clear(0x00_FF_00);
                _ = _mapSurface.Resize(WinWidth, WinHeight);
                LockedSurface lockedSurface = default;
                _ = _mapSurface.Lock(ref lockedSurface);
                for (int x = 0, last = WinWidth / _fogImageLibrary[0].Width; x <= last; x++)
                {
                    for (int y = 0, lastY = WinHeight / _fogImageLibrary[0].Height; y <= lastY; y++)
                    {
                        LibraryImage? fogImageLibraryHeight = _fogImageLibrary[0];
                        LibraryImage? fogImageLibraryWidth = _fogImageLibrary[0];
                        _fogImageLibrary[0].ShowClipped(x * fogImageLibraryWidth.Width, y * fogImageLibraryHeight.Height, lockedSurface);
                    }
                }

                _ = _mapSurface.Unlock();
                if (!Incremental)
                {
                    DrawMap(0, 0, _mapWidth, _mapHeight);
                    UpdateStep = _steps;
                }
                else
                {
                    UpdateStep = -1;
                }
            }

            PreviousWidth = WinWidth;
            _previousHeight = WinHeight;
        }

        base.SetSize();
    }

    // Scanner.pas:325
    public override void Update()
    {
        base.Update();
        if (Visible && _mapSurface is not null)
        {
            if (!Incremental)
            {
                Incremental = true;
            }

            if (UpdateStep < _steps)
            {
                if (UpdateStep > -1)
                {
                    DrawMap(0, _mapHeight * UpdateStep / _steps, _mapWidth, (_mapHeight / _steps) + 1);
                }

                UpdateStep += 1;
            }
        }
    }

    // Scanner.pas:445
    public override void Draw(LockedSurface lockedSurface)
    {
        LockedSurface surfaceDescription = default;
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(this, surfDesc: lockedSurface);

                if (_mapSurface is not null && _mapSurface.PixelFormatId != lockedSurface.PixelFormatId)
                {
                    _objectSurface?.Dispose();
                    _mapSurface?.Dispose();
                    _mapSurface = null;
                }

                if (_mapSurface is null)
                {
                    _objectSurface = new DataSurface(WinWidth, WinHeight, lockedSurface.PixelFormatId, palette: null);
                    _ = _objectSurface.Clear(0x00_FF_00);
                    _mapSurface = new DataSurface(WinWidth, WinHeight, lockedSurface.PixelFormatId, palette: null);
                    _ = _mapSurface.Lock(ref surfaceDescription);
                    for (int x = 0, last = WinWidth / _fogImageLibrary[0].Width; x <= last; x++)
                    {
                        for (int y = 0, lastY = WinHeight / _fogImageLibrary[0].Height; y <= lastY; y++)
                        {
                            LibraryImage? fogImageLibraryHeight = _fogImageLibrary[0];
                            LibraryImage? fogImageLibraryWidth = _fogImageLibrary[0];
                            _fogImageLibrary[0].ShowClipped(x * fogImageLibraryWidth.Width, y * fogImageLibraryHeight.Height, lockedSurface: surfaceDescription);
                        }
                    }

                    _ = _mapSurface.Unlock();
                    if (!Incremental)
                    {
                        DrawMap(0, 0, _mapWidth, _mapHeight);
                        UpdateStep = _steps;
                    }
                    else
                    {
                        UpdateStep = -1;
                    }
                }

                if (!_objectsDrawn)
                {
                    DrawObjects();
                }

                Rect r = new(0, 0, WinWidth, WinHeight);
                surfaceDescription = lockedSurface;
                Rect rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, Window.AbsoluteLeft + WinLeft + WinWidth, Window.AbsoluteTop + WinTop + WinHeight);
                surfaceDescription.ClipRectangle = rectangle;
                _ = _mapSurface.Lock(ref lockedSurface);
                _ = LockedSurface.BlitSurfaceDescriptionRectangleClipped(lockedSurface, sourceRectangle: r, destination: surfaceDescription, Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop);
                _ = _mapSurface.Unlock();
                _ = _objectSurface.Lock(ref lockedSurface);
                _ = LockedSurface.BlitSpriteClipped16(lockedSurface, sourceRectangle: r, destination: surfaceDescription, Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, colorKey: 0x00_FF_00);
                _ = _objectSurface.Unlock();
                if (_mapWidth > 0 && _mapHeight > 0)
                {
                    SizedRectangle rectangle2 = new(Window.AbsoluteLeft + WinLeft + (TLeft * WinWidth / _mapWidth), Window.AbsoluteTop + WinTop + (TTop * WinHeight / _mapHeight), _tWidth * WinWidth / _mapWidth, (_tHeight + 2) * WinHeight / _mapHeight);
                    _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, area: rectangle2, lockedSurface: ref surfaceDescription);
                }

                RaiseDraw(this, surfDesc: surfaceDescription);
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // Scanner.pas:449
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            IsMouseDown = true;
            X = (X - Window.AbsoluteLeft - WinLeft) * _mapWidth / WinWidth;
            int mapY = (Y - Window.AbsoluteTop - WinTop) * _mapHeight / _drawableHeight;
            int oldLeft = TLeft;
            int oldTop = TTop;
            TLeft = X - (_tWidth / 2);
            TTop = mapY - (_tHeight / 2);
            if (TTop != oldTop || oldLeft != TLeft)
            {
                Drawn = false;
                ChangeTileBox?.Invoke(this);
            }
        }
    }

    // Scanner.pas:452
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseUp(this, button, shift, X, Y);
        }

        IsMouseDown = false;
    }

    // Scanner.pas:455
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X >= Window.AbsoluteLeft + WinLeft && X <= Window.AbsoluteLeft + WinLeft + WinWidth - 1 && Y >= Window.AbsoluteTop + WinTop && Y <= Window.AbsoluteTop + WinTop + WinHeight - 1)
        {
            Handled = true;
            if (IsMouseDown)
            {
                int oldLeft = TLeft;
                int oldTop = TTop;
                int mapX = (X - Window.AbsoluteLeft - WinLeft) * _mapWidth / WinWidth;
                int mapY = (Y - Window.AbsoluteTop - WinTop) * _mapHeight / _drawableHeight;
                TLeft = mapX - (_tWidth / 2);
                TTop = mapY - (_tHeight / 2);
                if (TTop != oldTop || TLeft != oldLeft)
                {
                    Drawn = false;
                    ChangeTileBox?.Invoke(this);
                }
            }

            RaiseMouseMove(this, shift, X, Y);
        }
    }

    // Scanner.pas:142
    public void SetImageLibraryFrame(InterfaceImageLibrary? value)
    {
        if (value != _imageLibraryFrame)
        {
            Valid = false;
            _imageLibraryFrame = value;
        }
    }

    // Scanner.pas:133
    public void SetILFIndex(int value)
    {
        if (value != _imageLibraryIndexFrame)
        {
            _imageLibraryIndexFrame = value;
            Valid = false;
        }
    }

    // Scanner.pas:151
    public void SetObjectImageLibrary(ImageLibrary value)
    {
        if (value != _objectsImageLibrary)
        {
            Valid = false;
            _objectsImageLibrary = value;
        }
    }

    // Scanner.pas:160
    public void SetArmies(bool value)
    {
        if (value != _armies)
        {
            _armies = value;
            SetObDrawn(false);
            Drawn = false;
        }
    }

    // Scanner.pas:170
    public void SetBuildings(bool value)
    {
        if (value != _buildings)
        {
            _buildings = value;
            SetObDrawn(false);
            Drawn = false;
        }
    }

    // Scanner.pas:180
    public void SetNaturals(bool value)
    {
        if (value != _naturals)
        {
            _naturals = value;
            SetObDrawn(false);
            Drawn = false;
        }
    }

    // Scanner.pas:190
    public void SetLevel(MapLevel value)
    {
        Level = value;
        if (StrategicMap.Current.CombatMap is not null)
        {
            _mapWidth = Level.Width - 16;
            _mapHeight = Level.Height - 12;
        }
        else
        {
            _mapWidth = Level.Width - 12;
            _mapHeight = Level.Height - 12;
        }
    }

    // Scanner.pas:205
    public void SetMapWidth(int value) => _mapWidth = StrategicMap.Current.CombatMap is not null ? value - 16 : value - 12;

    // Scanner.pas:211
    public void SetMapHeight(int value)
    {
        if (StrategicMap.Current.CombatMap is not null)
        {
            _mapHeight = value - 12;
        }
        else
        {
            // { Both original branches subtract 12. }
            _mapHeight = value - 12;
        }
    }

    // Scanner.pas:217
    public byte GetTerrain(int X, int Y)
    {
        return StrategicMap.Current.CombatMap is not null
            ? (byte)Level.GetField(X + 8, Y + 6).TerrainType
            : (byte)Level.GetField(X + 6, Y + 6).TerrainType;
    }

    // Scanner.pas:223
    public void SetTerrain(int X, int Y, int value)
    {
    }

    // Scanner.pas:226
    public int GetOverlay(int X, int Y)
    {
        return StrategicMap.Current.CombatMap is not null
            ? Level.GetField(X + 8, Y + 6).GetDecorationType() switch
            {
                1 or 2 or 3 or 9 => 1,
                4 or 5 => 9,
                6 or 7 or 8 or (>= 14 and <= 18) => 8,
                10 => 3,
                11 or 12 => 6,
                13 => 2,
                _ => Level.GetField(X + 8, Y + 6).OverlayType,
            }
            : Level.GetField(X + 6, Y + 6).OverlayType;
    }

    // Scanner.pas:243
    public void SetOverlay(int X, int Y, int value)
    {
    }

    // Scanner.pas:246
    public byte GetExplored(int X, int Y)
    {
        _ = StrategicMap.Current.CombatMap is not null ? Level.GetField(X + 8, Y + 6) : Level.GetField(X + 6, Y + 6);

        MapField field = StrategicMap.Current.CombatMap is not null ? Level.GetField(X + 8, Y + 6) : Level.GetField(X + 6, Y + 6);

        return _map.ExplorationEnabled
            ? field.GetExplored(_map.SeatedPlayer) ? _map.FogEnabled ? field.FogCount == 0 ? (byte)2 : (byte)1 : (byte)1 : (byte)0
            : _map.FogEnabled ? field.FogCount == 0 ? (byte)2 : (byte)1 : (byte)1;
    }

    // Scanner.pas:277
    public void SetExplored(int X, int Y, int value)
    {
    }

    // Scanner.pas:280
    public int GetObject(int X, int Y)
    {
        return StrategicMap.Current.CombatMap is not null
            ? Level.GetField(X + 8, Y + 6).GetScannerIcon()
            : Level.GetField(X + 6, Y + 6).GetScannerIcon();
    }

    // Scanner.pas:287
    public byte ObjectSize => 1;

    // Scanner.pas:292
    public void SetObjectSize(int X, int Y, int value)
    {
    }

    // Scanner.pas:295
    public void SetImageLibrary(ImageLibrary value)
    {
        _tilesImageLibrary = value;
        Valid = false;
    }

    // Scanner.pas:301
    public void SetHimgLibrary(ImageLibrary value)
    {
        _highlightImageLibrary = value;
        Valid = false;
    }

    // Scanner.pas:307
    public void SetFogimgLibrary(ImageLibrary value)
    {
        _fogImageLibrary = value;
        Valid = false;
    }

    // Scanner.pas:313
    public void SetSteps(int value)
    {
        _steps = value;
        if (_steps < 1)
        {
            _steps = 1;
        }
    }

    // Scanner.pas:319
    public void SetObDrawn(bool value)
    {
        _objectsDrawn = value;
        if (!_objectsDrawn)
        {
            Drawn = false;
        }
    }

    // Scanner.pas:341
    public void SetTLeft(int value)
    {
        if (value != TLeft)
        {
            Drawn = false;
            TLeft = value;
        }
    }

    // Scanner.pas:350
    public void SetTTop(int value)
    {
        if (value != TTop)
        {
            Drawn = false;
            TTop = value;
        }
    }

    // Scanner.pas:359
    public void SetTWidth(int value)
    {
        if (value != _tWidth)
        {
            Drawn = false;
            _tWidth = value;
        }
    }

    // Scanner.pas:368
    public void SetTHeight(int value)
    {
        if (value != _tHeight)
        {
            Drawn = false;
            _tHeight = value;
        }
    }

    // Scanner.pas:377
    public void SetMap(GameMap? value)
    {
        _map = value;
        if (_map is not null)
        {
            Level = _map.MapContainer.GetMapLevel(0);
            if (StrategicMap.Current.CombatMap is not null)
            {
                _mapWidth = Level.Width - 16;
                _mapHeight = Level.Height - 12;
            }
            else
            {
                _mapWidth = Level.Width - 12;
                _mapHeight = Level.Height - 12;
            }

            Incremental = false;
            Sized = false;
        }
    }

    // Scanner.pas:440
    public void DrawMap(int X, int Y, int width, int height)
    {
        LockedSurface lockedSurface = default;
        try
        {
            if (Valid && _mapSurface is not null && Visible)
            {
                if (X < 0)
                {
                    width += X;
                    X = 0;
                }

                if (Y < 0)
                {
                    height += Y;
                    Y = 0;
                }

                int yOffset = X == X / 2 * 2 ? 0 : _halfRowInset;

                if (_mapSurface.Lock(ref lockedSurface) >= 0)
                {
                    if (_mapWidth < width + X)
                    {
                        width = _mapWidth - X;
                    }

                    if (_mapHeight < Y + height)
                    {
                        height = _mapHeight - Y;
                    }

                    for (int mapX = X; mapX <= width + X - 1; mapX++)
                    {
                        for (int mapY = Y; mapY <= Y + height - 1; mapY++)
                        {
                            Rect rectangle = new(WinWidth * mapX / _mapWidth, (_drawableHeight * mapY / _mapHeight) + yOffset, (mapX + 1) * WinWidth / _mapWidth, ((mapY + 1) * _drawableHeight / _mapHeight) + yOffset);
                            lockedSurface.ClipRectangle = rectangle;
                            int explored = GetExplored(mapX, mapY);
                            if (explored == 0)
                            {
                                for (int tileX = WinWidth * mapX / _mapWidth / _fogImageLibrary[0].Width, last = (mapX + 1) * WinWidth / _mapWidth / _fogImageLibrary[0].Width; tileX <= last; tileX++)
                                {
                                    for (int tileY = ((_drawableHeight * mapY / _mapHeight) + yOffset) / _fogImageLibrary[0].Height, lastTileY = (((mapY + 1) * _drawableHeight / _mapHeight) + yOffset) / _fogImageLibrary[0].Height; tileY <= lastTileY; tileY++)
                                    {
                                        LibraryImage? fogImageLibraryHeight = _fogImageLibrary[0];
                                        LibraryImage? fogImageLibraryWidth = _fogImageLibrary[0];
                                        _fogImageLibrary[0].ShowClipped(tileX * fogImageLibraryWidth.Width, tileY * fogImageLibraryHeight.Height, lockedSurface);
                                    }
                                }
                            }
                            else
                            {
                                int terrain = GetTerrain(mapX, mapY);
                                int overlay = GetOverlay(mapX, mapY);
                                int overlayIndex = overlay is > 11 or 7 ? 0 : overlay + 1;

                                int imageIndex = (overlayIndex * 16) + terrain;
                                if (terrain == 15)
                                {
                                    for (int tileX = WinWidth * mapX / _mapWidth / _fogImageLibrary[0].Width, last = (mapX + 1) * WinWidth / _mapWidth / _fogImageLibrary[0].Width; tileX <= last; tileX++)
                                    {
                                        for (int tileY = ((_drawableHeight * mapY / _mapHeight) + yOffset) / _fogImageLibrary[0].Height, lastTileY = (((mapY + 1) * _drawableHeight / _mapHeight) + yOffset) / _fogImageLibrary[0].Height; tileY <= lastTileY; tileY++)
                                        {
                                            LibraryImage? fogImageLibraryHeight = _fogImageLibrary[0];
                                            LibraryImage? fogImageLibraryWidth = _fogImageLibrary[0];
                                            _fogImageLibrary[0].ShowClipped(tileX * fogImageLibraryWidth.Width, tileY * fogImageLibraryHeight.Height, lockedSurface);
                                        }
                                    }
                                }
                                else
                                {
                                    if (imageIndex < _tilesImageLibrary.Count && _tilesImageLibrary[imageIndex] is not null)
                                    {
                                        for (int tileX = WinWidth * mapX / _mapWidth / _tilesImageLibrary[imageIndex].Width, last = (mapX + 1) * WinWidth / _mapWidth / _tilesImageLibrary[imageIndex].Width; tileX <= last; tileX++)
                                        {
                                            for (int tileY = ((_drawableHeight * mapY / _mapHeight) + yOffset) / _tilesImageLibrary[imageIndex].Height, lastTileY = (((mapY + 1) * _drawableHeight / _mapHeight) + yOffset) / _tilesImageLibrary[imageIndex].Height; tileY <= lastTileY; tileY++)
                                            {
                                                LibraryImage? tilesImageLibraryHeight = _tilesImageLibrary[imageIndex];
                                                LibraryImage? tilesImageLibraryWidth = _tilesImageLibrary[imageIndex];
                                                _tilesImageLibrary[imageIndex].ShowClipped(tileX * tilesImageLibraryWidth.Width, tileY * tilesImageLibraryHeight.Height, lockedSurface);
                                            }
                                        }
                                    }

                                    if ((overlay is 0 or 10) && HighLights && mapY < _mapHeight - 1 && mapX > 0 && GetOverlay(mapX - 1, mapY + 1) != overlay)
                                    {
                                        _highlightImageLibrary[1].ShowClipped(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, lockedSurface);
                                    }

                                    if ((overlay is 0 or 10) && HighLights && mapY > 0 && mapX < _mapWidth - 1 && GetOverlay(mapX + 1, mapY - 1) != overlay)
                                    {
                                        _highlightImageLibrary[0].ShowClipped(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, lockedSurface);
                                    }

                                    if (explored == 2)
                                    {
                                        for (int tileX = WinWidth * mapX / _mapWidth / _fogImageLibrary[1].Width, last = (mapX + 1) * WinWidth / _mapWidth / _fogImageLibrary[1].Width; tileX <= last; tileX++)
                                        {
                                            for (int tileY = ((_drawableHeight * mapY / _mapHeight) + yOffset) / _fogImageLibrary[1].Height, lastTileY = (((mapY + 1) * _drawableHeight / _mapHeight) + yOffset) / _fogImageLibrary[1].Height; tileY <= lastTileY; tileY++)
                                            {
                                                LibraryImage? fogImageLibraryHeight = _fogImageLibrary[1];
                                                LibraryImage? fogImageLibraryWidth = _fogImageLibrary[1];
                                                _fogImageLibrary[1].ShowClipped(tileX * fogImageLibraryWidth.Width, tileY * fogImageLibraryHeight.Height, lockedSurface);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        yOffset = yOffset == 0 ? _halfRowInset : 0;
                    }

                    _ = _mapSurface.Unlock();
                    SetObDrawn(false);
                    Drawn = false;
                }
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during DrawMap", caption: $"Error - {Name}", flags: 0);
        }
    }

    // Scanner.pas:443
    public void DrawObjects()
    {
        if (_objectSurface is not null)
        {
            _ = _objectSurface.Clear(0x00_FF_00);
            LockedSurface lockedSurface = default;
            if (_objectSurface.Lock(ref lockedSurface) >= 0)
            {
                int yOffset = 0;
                for (int x = 0, last = _mapWidth - 1; x <= last; x++)
                {
                    for (int y = 0, lastY = _mapHeight - 1; y <= lastY; y++)
                    {
                        if (GetExplored(x, y) == 1)
                        {
                            int objectValue = GetObject(x, y);
                            if (objectValue != 0)
                            {
                                int army = objectValue & 0xFF;
                                int natural = objectValue & 0xFF00;
                                int building = objectValue & 0xFF_00_00;
                                int imageIndex;
                                if (building > 0 && _buildings)
                                {
                                    imageIndex = 7 - ((building >>> 22) & 0b0011) + (((building >>> 16) & 0b0011_1111) * 6);
                                    if (imageIndex < _objectsImageLibrary.Count && imageIndex > -1 && _objectsImageLibrary[imageIndex] is not null)
                                    {
                                        LibraryImage? objectsImageLibraryHeight = _objectsImageLibrary[imageIndex];
                                        LibraryImage? objectsImageLibraryWidth = _objectsImageLibrary[imageIndex];
                                        _objectsImageLibrary[imageIndex].ShowClipped((WinWidth / (_mapWidth * 2)) + (x * WinWidth / _mapWidth) - (objectsImageLibraryWidth.Width / 2), (WinHeight / (_mapHeight * 2)) + (y * WinHeight / _mapHeight) - (objectsImageLibraryHeight.Height / 2) + yOffset, lockedSurface);
                                    }
                                }
                                else
                                {
                                    if (natural > 0 && _naturals)
                                    {
                                        imageIndex = ((natural >>> 8) * 6) + 9;
                                        if (imageIndex < _objectsImageLibrary.Count && imageIndex > -1 && _objectsImageLibrary[imageIndex] is not null)
                                        {
                                            LibraryImage? objectsImageLibraryHeight = _objectsImageLibrary[imageIndex];
                                            LibraryImage? objectsImageLibraryWidth = _objectsImageLibrary[imageIndex];
                                            _objectsImageLibrary[imageIndex].ShowClipped((WinWidth / (_mapWidth * 2)) + (x * WinWidth / _mapWidth) - (objectsImageLibraryWidth.Width / 2), (WinHeight / (_mapHeight * 2)) + (y * WinHeight / _mapHeight) - (objectsImageLibraryHeight.Height / 2) + yOffset, lockedSurface);
                                        }
                                    }
                                }

                                if (army > 0 && _armies)
                                {
                                    imageIndex = (army * 6) + 8;
                                    if (imageIndex < _objectsImageLibrary.Count && imageIndex > -1 && _objectsImageLibrary[imageIndex] is not null)
                                    {
                                        LibraryImage? objectsImageLibraryHeight = _objectsImageLibrary[imageIndex];
                                        LibraryImage? objectsImageLibraryWidth = _objectsImageLibrary[imageIndex];
                                        _objectsImageLibrary[imageIndex].ShowClipped((WinWidth / (_mapWidth * 2)) + (x * WinWidth / _mapWidth) - (objectsImageLibraryWidth.Width / 2), (WinHeight / (_mapHeight * 2)) + (y * WinHeight / _mapHeight) - (objectsImageLibraryHeight.Height / 2) + yOffset, lockedSurface);
                                    }
                                }
                            }
                        }
                    }

                    yOffset = yOffset == 0 ? _halfRowInset : 0;
                }

                _ = _objectSurface.Unlock();
                Drawn = false;
                SetObDrawn(true);
            }
        }
    }

    [Published]
    public ImageLibrary TilesImageLibrary { get => _tilesImageLibrary; set => SetImageLibrary(value); }

    [Published]
    public ImageLibrary HighlightsImageLibrary { get => _highlightImageLibrary; set => SetHimgLibrary(value); }

    [Published]
    public ImageLibrary FogImageLibrary { get => _fogImageLibrary; set => SetFogimgLibrary(value); }

    [Published]
    public ImageLibrary ImageLibraryObjects { get => _objectsImageLibrary; set => SetObjectImageLibrary(value); }

    [Published]
    public int TileBoxTop { get => TTop; set => SetTTop(value); }

    [Published]
    public int TileBoxLeft { get => TLeft; set => SetTLeft(value); }

    [Published]
    public int TileBoxWidth { get => _tWidth; set => SetTWidth(value); }

    [Published]
    public int TileBoxHeight { get => _tHeight; set => SetTHeight(value); }

    [Published]
    public bool HighLights { get; set; }

    [Published]
    public bool Buildings { get => _buildings; set => SetBuildings(value); }

    [Published]
    public bool Armies { get => _armies; set => SetArmies(value); }

    [Published]
    public bool Naturals { get => _naturals; set => SetNaturals(value); }

    [Published]
    public int MapDrawSteps { get => _steps; set => SetSteps(value); }

    [Published]
    public InterfaceImageLibrary? ImageLibraryFrame { get => _imageLibraryFrame; set => SetImageLibraryFrame(value); }

    [Published]
    public int ImageLibraryIndexFrame { get => _imageLibraryIndexFrame; set => SetILFIndex(value); }

    public new event NotifyEvent<Scanner> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, Scanner>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, Scanner>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<Scanner> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, Scanner>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, Scanner>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<Scanner> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, Scanner>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, Scanner>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<Scanner> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, Scanner>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, Scanner>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<Scanner> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, Scanner>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, Scanner>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<Scanner> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, Scanner>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, Scanner>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Scanner> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, Scanner>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, Scanner>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Scanner> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Scanner>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Scanner>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Scanner> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, Scanner>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, Scanner>(this, slot: nameof(Updated), value: value);
    }
}
