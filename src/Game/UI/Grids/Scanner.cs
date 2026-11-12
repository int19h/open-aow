// Translated from Scanner.pas.
using System.Runtime.CompilerServices;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Grids;

// { ot=1 }
// half-row inset, libraries, icons, viewport, change event, update steps, map, frame library,
// frame index, map dimensions, level, icons drawn, previous size, drawable height, terrain/icon
// surfaces, update step, incremental
public partial class Scanner : Controls.Control
{
    private int _halfRowInset;
    private ImageLibrary _tilesImageLibrary;
    private ImageLibrary _highlightImageLibrary;
    private ImageLibrary _fogImageLibrary;
    private ImageLibrary _objectsImageLibrary;

    public int TTop { get; private set; }

    private int _tHeight;

    public int TLeft { get; private set; }

    private int _tWidth;
    private bool _showHighlights;
    private bool _buildings;
    private bool _armies;
    private bool _naturals;
    private NotifyEvent<Scanner> _onMapPositionChange;
    private int _steps;
    private GameMap _map;
    private InterfaceImageLibrary _imageLibraryFrame;
    private int _imageLibraryIndexFrame;
    private int _mapWidth;
    private int _mapHeight;

    public MapLevel Level { get; private set; }

    private bool _objectsDrawn;

    public int PreviousWidth { get; set; }

    private int _previousHeight;
    private int _drawableHeight;
    private Surface _mapSurface;
    private Surface _objectSurface;

    public int UpdateStep { get; set; }
    public bool Incremental { get; set; }

    // Scanner.pas:126
    public override void Dispose()
    {
        if (_mapSurface != null)
        {
            _mapSurface.Dispose();
        }

        if (_objectSurface != null)
        {
            _objectSurface.Dispose();
        }

        base.Dispose();
    }

    // Scanner.pas:116
    public Scanner(Component owner) : base(owner)
    {
        _steps = 1;
        TLeft = -1;
        TTop = -1;
        _tWidth = -1;
        _tHeight = -1;
    }

    // Scanner.pas:398
    public override void Validate()
    {
        InterfaceImageLibrary frame;
        frame = _imageLibraryFrame;
        if (frame != null)
        {
            _imageLibraryFrame = frame.CheckImageLibrary(_imageLibraryIndexFrame, 8, this);
        }

        if (_imageLibraryFrame != null && _tilesImageLibrary != null && _objectsImageLibrary != null && _map != null && _fogImageLibrary != null && _highlightImageLibrary != null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // Scanner.pas:409
    public override void SetSize()
    {
        LockedSurface lockedSurface = default;
        if ((WinWidth != PreviousWidth || WinHeight != _previousHeight) && WinWidth > 0 && WinHeight > 0)
        {
            _halfRowInset = WinHeight / _mapHeight / 2;
            _drawableHeight = WinHeight - _halfRowInset;
            if (_mapSurface != null)
            {
                _objectSurface.Resize(WinWidth, WinHeight);
                _objectSurface.Clear(65280);
                _mapSurface.Resize(WinWidth, WinHeight);
                _mapSurface.Lock(ref lockedSurface);
                for (int X = 0, last = WinWidth / _fogImageLibrary[0].Width; X <= last; X++)
                {
                    for (int Y = 0, lastY = WinHeight / _fogImageLibrary[0].Height; Y <= lastY; Y++)
                    {
                        var fogImageLibraryHeight = _fogImageLibrary[0];
                        var fogImageLibraryWidth = _fogImageLibrary[0];
                        _fogImageLibrary[0].ShowClipped(X * fogImageLibraryWidth.Width, Y * fogImageLibraryHeight.Height, lockedSurface);
                    }
                }

                _mapSurface.Unlock();
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
        if (Visible && _mapSurface != null)
        {
            if (!Incremental)
            {
                Incremental = true;
            }

            if (UpdateStep < _steps)
            {
                if (UpdateStep > -1)
                {
                    DrawMap(0, _mapHeight * UpdateStep / _steps, _mapWidth, _mapHeight / _steps + 1);
                }

                UpdateStep += 1;
            }
        }
    }

    // Scanner.pas:445
    public override void Draw(LockedSurface lockedSurface)
    {
        Rect R = default;
        LockedSurface surfaceDescription = default;
        try
        {
            if (Valid && Visible)
            {
                if (_onPreDraw != null)
                {
                    _onPreDraw(this, lockedSurface);
                }

                if (_mapSurface != null && _mapSurface.PixelFormatId != lockedSurface.PixelFormatId)
                {
                    _objectSurface?.Dispose();
                    _mapSurface?.Dispose();
                    _mapSurface = null;
                }

                if (_mapSurface == null)
                {
                    _objectSurface = new DataSurface(WinWidth, WinHeight, lockedSurface.PixelFormatId, null);
                    _objectSurface.Clear(65280);
                    _mapSurface = new DataSurface(WinWidth, WinHeight, lockedSurface.PixelFormatId, null);
                    _mapSurface.Lock(ref surfaceDescription);
                    for (int X = 0, last = WinWidth / _fogImageLibrary[0].Width; X <= last; X++)
                    {
                        for (int Y = 0, lastY = WinHeight / _fogImageLibrary[0].Height; Y <= lastY; Y++)
                        {
                            var fogImageLibraryHeight = _fogImageLibrary[0];
                            var fogImageLibraryWidth = _fogImageLibrary[0];
                            _fogImageLibrary[0].ShowClipped(X * fogImageLibraryWidth.Width, Y * fogImageLibraryHeight.Height, surfaceDescription);
                        }
                    }

                    _mapSurface.Unlock();
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

                R = new Rect(0, 0, WinWidth, WinHeight);
                surfaceDescription = lockedSurface;
                var rectangle = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop, Window.WinLeft + WinLeft + WinWidth, Window.WinTop + WinTop + WinHeight);
                surfaceDescription.ClipRectangle = rectangle;
                _mapSurface.Lock(ref lockedSurface);
                LockedSurface.BlitSurfaceDescriptionRectangleClipped(lockedSurface, R, surfaceDescription, Window.WinLeft + WinLeft, Window.WinTop + WinTop);
                _mapSurface.Unlock();
                _objectSurface.Lock(ref lockedSurface);
                LockedSurface.BlitSpriteClipped16(lockedSurface, R, surfaceDescription, Window.WinLeft + WinLeft, Window.WinTop + WinTop, 65280);
                _objectSurface.Unlock();
                if (_mapWidth > 0 && _mapHeight > 0)
                {
                    var rectangle2 = new Rect(Window.WinLeft + WinLeft + TLeft * WinWidth / _mapWidth, Window.WinTop + WinTop + TTop * WinHeight / _mapHeight, _tWidth * WinWidth / _mapWidth, (_tHeight + 2) * WinHeight / _mapHeight);
                    _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle2, ref surfaceDescription);
                }

                if (_onDraw != null)
                {
                    _onDraw(this, surfaceDescription);
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // Scanner.pas:449
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        int mapY;
        int oldLeft;
        int oldTop;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            MouseDown = true;
            X = (X - Window.WinLeft - WinLeft) * _mapWidth / WinWidth;
            mapY = (Y - Window.WinTop - WinTop) * _mapHeight / _drawableHeight;
            oldLeft = TLeft;
            oldTop = TTop;
            TLeft = X - _tWidth / 2;
            TTop = mapY - _tHeight / 2;
            if (TTop != oldTop || oldLeft != TLeft)
            {
                Drawn = false;
                if (_onMapPositionChange != null)
                {
                    _onMapPositionChange(this);
                }
            }
        }
    }

    // Scanner.pas:452
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            if (_onMouseUp != null)
            {
                _onMouseUp(this, button, shift, X, Y);
            }
        }

        MouseDown = false;
    }

    // Scanner.pas:455
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        int mapX = default;
        int mapY = default;
        int oldLeft = default;
        int oldTop = default;
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X >= Window.WinLeft + WinLeft && X <= Window.WinLeft + WinLeft + WinWidth - 1 && Y >= Window.WinTop + WinTop && Y <= Window.WinTop + WinTop + WinHeight - 1)
        {
            Handled = true;
            if (MouseDown == true)
            {
                oldLeft = TLeft;
                oldTop = TTop;
                mapX = (X - Window.WinLeft - WinLeft) * _mapWidth / WinWidth;
                mapY = (Y - Window.WinTop - WinTop) * _mapHeight / _drawableHeight;
                TLeft = mapX - _tWidth / 2;
                TTop = mapY - _tHeight / 2;
                if (TTop != oldTop || TLeft != oldLeft)
                {
                    Drawn = false;
                    if (_onMapPositionChange != null)
                    {
                        _onMapPositionChange(this);
                    }
                }
            }

            if (_onMouseMove != null)
            {
                _onMouseMove(this, shift, X, Y);
            }
        }
    }

    // Scanner.pas:142
    public virtual void SetImageLibraryFrame(InterfaceImageLibrary value)
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
        if (AoWHSMap.Current.CombatMap != null)
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
    public void SetMapWidth(int value)
    {
        if (AoWHSMap.Current.CombatMap != null)
        {
            _mapWidth = value - 16;
        }
        else
        {
            _mapWidth = value - 12;
        }
    }

    // Scanner.pas:211
    public void SetMapHeight(int value)
    {
        if (AoWHSMap.Current.CombatMap != null)
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
        byte result;
        if (AoWHSMap.Current.CombatMap != null)
        {
            result = (byte)Level.GetField(X + 8, Y + 6).TerrainType;
        }
        else
        {
            result = (byte)Level.GetField(X + 6, Y + 6).TerrainType;
        }

        return result;
    }

    // Scanner.pas:223
    public void SetTerrain(int X, int Y, int value)
    {
    }

    // Scanner.pas:226
    public int GetOverlay(int X, int Y)
    {
        int result;
        if (AoWHSMap.Current.CombatMap != null)
        {
            switch (Level.GetField(X + 8, Y + 6).GetDecorationType())
            {
                case 1:
                case 2:
                case 3:
                case 9:
                    result = 1;
                    break;
                case 4:
                case 5:
                    result = 9;
                    break;
                case 6:
                case 7:
                case 8:
                case >= 14 and <= 18:
                    result = 8;
                    break;
                case 10:
                    result = 3;
                    break;
                case 11:
                case 12:
                    result = 6;
                    break;
                case 13:
                    result = 2;
                    break;
                default:
                    result = Level.GetField(X + 8, Y + 6).OverlayType;
                    break;
            }
        }
        else
        {
            result = Level.GetField(X + 6, Y + 6).OverlayType;
        }

        return result;
    }

    // Scanner.pas:243
    public void SetOverlay(int X, int Y, int value)
    {
    }

    // Scanner.pas:246
    public byte GetExplored(int X, int Y)
    {
        byte result;
        AoWMapField field;
        if (AoWHSMap.Current.CombatMap != null)
        {
            field = (AoWMapField)Level.GetField(X + 8, Y + 6);
        }
        else
        {
            field = (AoWMapField)Level.GetField(X + 6, Y + 6);
        }

        if (_map.ExplorationEnabled)
        {
            if (field.GetExplored(_map.SeatedPlayer))
            {
                if (_map.FogEnabled)
                {
                    if (Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref field.FieldFlags).Hi).Lo == 0)
                    {
                        result = 2;
                    }
                    else
                    {
                        result = 1;
                    }
                }
                else
                {
                    result = 1;
                }
            }
            else
            {
                result = 0;
            }
        }
        else
        {
            if (_map.FogEnabled)
            {
                if (Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref field.FieldFlags).Hi).Lo == 0)
                {
                    result = 2;
                }
                else
                {
                    result = 1;
                }
            }
            else
            {
                result = 1;
            }
        }

        return result;
    }

    // Scanner.pas:277
    public void SetExplored(int X, int Y, int value)
    {
    }

    // Scanner.pas:280
    public int GetObject(int X, int Y)
    {
        int result;
        if (AoWHSMap.Current.CombatMap != null)
        {
            result = ((AoWMapField)Level.GetField(X + 8, Y + 6)).GetScannerIcon();
        }
        else
        {
            result = ((AoWMapField)Level.GetField(X + 6, Y + 6)).GetScannerIcon();
        }

        return result;
    }

    // Scanner.pas:287
    public byte GetObjectSize() => 1;

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
    public void SetMap(GameMap value)
    {
        _map = value;
        if (_map != null)
        {
            Level = _map.MapContainer.GetMapLevel(0);
            if (AoWHSMap.Current.CombatMap != null)
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
        int imageIndex;
        int overlay;
        int explored;
        int yOffset;
        int terrain;
        int overlayIndex;
        LockedSurface lockedSurface = default;
        try
        {
            if (Valid && _mapSurface != null && Visible)
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

                if (X == X / 2 * 2)
                {
                    yOffset = 0;
                }
                else
                {
                    yOffset = _halfRowInset;
                }

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
                            var rectangle = new Rect(WinWidth * mapX / _mapWidth, _drawableHeight * mapY / _mapHeight + yOffset, (mapX + 1) * WinWidth / _mapWidth, (mapY + 1) * _drawableHeight / _mapHeight + yOffset);
                            lockedSurface.ClipRectangle = rectangle;
                            explored = GetExplored(mapX, mapY);
                            if (explored == 0)
                            {
                                for (int tileX = WinWidth * mapX / _mapWidth / _fogImageLibrary[0].Width, last = (mapX + 1) * WinWidth / _mapWidth / _fogImageLibrary[0].Width; tileX <= last; tileX++)
                                {
                                    for (int tileY = (_drawableHeight * mapY / _mapHeight + yOffset) / _fogImageLibrary[0].Height, lastTileY = ((mapY + 1) * _drawableHeight / _mapHeight + yOffset) / _fogImageLibrary[0].Height; tileY <= lastTileY; tileY++)
                                    {
                                        var fogImageLibraryHeight = _fogImageLibrary[0];
                                        var fogImageLibraryWidth = _fogImageLibrary[0];
                                        _fogImageLibrary[0].ShowClipped(tileX * fogImageLibraryWidth.Width, tileY * fogImageLibraryHeight.Height, lockedSurface);
                                    }
                                }
                            }
                            else
                            {
                                terrain = GetTerrain(mapX, mapY);
                                overlay = GetOverlay(mapX, mapY);
                                if (overlay > 11 || overlay == 7)
                                {
                                    overlayIndex = 0;
                                }
                                else
                                {
                                    overlayIndex = overlay + 1;
                                }

                                imageIndex = overlayIndex * 16 + terrain;
                                if (terrain == 15)
                                {
                                    for (int tileX = WinWidth * mapX / _mapWidth / _fogImageLibrary[0].Width, last = (mapX + 1) * WinWidth / _mapWidth / _fogImageLibrary[0].Width; tileX <= last; tileX++)
                                    {
                                        for (int tileY = (_drawableHeight * mapY / _mapHeight + yOffset) / _fogImageLibrary[0].Height, lastTileY = ((mapY + 1) * _drawableHeight / _mapHeight + yOffset) / _fogImageLibrary[0].Height; tileY <= lastTileY; tileY++)
                                        {
                                            var fogImageLibraryHeight = _fogImageLibrary[0];
                                            var fogImageLibraryWidth = _fogImageLibrary[0];
                                            _fogImageLibrary[0].ShowClipped(tileX * fogImageLibraryWidth.Width, tileY * fogImageLibraryHeight.Height, lockedSurface);
                                        }
                                    }
                                }
                                else
                                {
                                    if (imageIndex < _tilesImageLibrary.Count && _tilesImageLibrary[imageIndex] != null)
                                    {
                                        for (int tileX = WinWidth * mapX / _mapWidth / _tilesImageLibrary[imageIndex].Width, last = (mapX + 1) * WinWidth / _mapWidth / _tilesImageLibrary[imageIndex].Width; tileX <= last; tileX++)
                                        {
                                            for (int tileY = (_drawableHeight * mapY / _mapHeight + yOffset) / _tilesImageLibrary[imageIndex].Height, lastTileY = ((mapY + 1) * _drawableHeight / _mapHeight + yOffset) / _tilesImageLibrary[imageIndex].Height; tileY <= lastTileY; tileY++)
                                            {
                                                var tilesImageLibraryHeight = _tilesImageLibrary[imageIndex];
                                                var tilesImageLibraryWidth = _tilesImageLibrary[imageIndex];
                                                _tilesImageLibrary[imageIndex].ShowClipped(tileX * tilesImageLibraryWidth.Width, tileY * tilesImageLibraryHeight.Height, lockedSurface);
                                            }
                                        }
                                    }

                                    if ((overlay == 0 || overlay == 10) && _showHighlights == true && mapY < _mapHeight - 1 && mapX > 0 && GetOverlay(mapX - 1, mapY + 1) != overlay)
                                    {
                                        _highlightImageLibrary[1].ShowClipped(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, lockedSurface);
                                    }

                                    if ((overlay == 0 || overlay == 10) && _showHighlights == true && mapY > 0 && mapX < _mapWidth - 1 && GetOverlay(mapX + 1, mapY - 1) != overlay)
                                    {
                                        _highlightImageLibrary[0].ShowClipped(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, lockedSurface);
                                    }

                                    if (explored == 2)
                                    {
                                        for (int tileX = WinWidth * mapX / _mapWidth / _fogImageLibrary[1].Width, last = (mapX + 1) * WinWidth / _mapWidth / _fogImageLibrary[1].Width; tileX <= last; tileX++)
                                        {
                                            for (int tileY = (_drawableHeight * mapY / _mapHeight + yOffset) / _fogImageLibrary[1].Height, lastTileY = ((mapY + 1) * _drawableHeight / _mapHeight + yOffset) / _fogImageLibrary[1].Height; tileY <= lastTileY; tileY++)
                                            {
                                                var fogImageLibraryHeight = _fogImageLibrary[1];
                                                var fogImageLibraryWidth = _fogImageLibrary[1];
                                                _fogImageLibrary[1].ShowClipped(tileX * fogImageLibraryWidth.Width, tileY * fogImageLibraryHeight.Height, lockedSurface);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (yOffset == 0)
                        {
                            yOffset = _halfRowInset;
                        }
                        else
                        {
                            yOffset = 0;
                        }
                    }

                    _mapSurface.Unlock();
                    SetObDrawn(false);
                    Drawn = false;
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during DrawMap", "Error - " + Name, 0);
        }
    }

    // Scanner.pas:443
    public void DrawObjects()
    {
        int imageIndex;
        int yOffset;
        int army;
        int natural;
        int building;
        int objectValue;
        LockedSurface lockedSurface = default;
        if (_objectSurface != null)
        {
            _objectSurface.Clear(65280);
            yOffset = 0;
            if (_objectSurface.Lock(ref lockedSurface) >= 0)
            {
                for (int X = 0, last = _mapWidth - 1; X <= last; X++)
                {
                    for (int Y = 0, lastY = _mapHeight - 1; Y <= lastY; Y++)
                    {
                        if (GetExplored(X, Y) == 1)
                        {
                            objectValue = GetObject(X, Y);
                            if (objectValue != 0)
                            {
                                army = objectValue & 255;
                                natural = objectValue & 65280;
                                building = objectValue & 16711680;
                                if (building > 0 && _buildings)
                                {
                                    imageIndex = 7 - (((int)(((uint)building) >> 22)) & 3) + (((int)(((uint)building) >> 16)) & 63) * 6;
                                    if (imageIndex < _objectsImageLibrary.Count && imageIndex > -1 && _objectsImageLibrary[imageIndex] != null)
                                    {
                                        var objectsImageLibraryHeight = _objectsImageLibrary[imageIndex];
                                        var objectsImageLibraryWidth = _objectsImageLibrary[imageIndex];
                                        _objectsImageLibrary[imageIndex].ShowClipped(WinWidth / (_mapWidth * 2) + X * WinWidth / _mapWidth - objectsImageLibraryWidth.Width / 2, WinHeight / (_mapHeight * 2) + Y * WinHeight / _mapHeight - objectsImageLibraryHeight.Height / 2 + yOffset, lockedSurface);
                                    }
                                }
                                else
                                {
                                    if (natural > 0 && _naturals)
                                    {
                                        imageIndex = (int)(((uint)natural) >> 8) * 6 + 9;
                                        if (imageIndex < _objectsImageLibrary.Count && imageIndex > -1 && _objectsImageLibrary[imageIndex] != null)
                                        {
                                            var objectsImageLibraryHeight = _objectsImageLibrary[imageIndex];
                                            var objectsImageLibraryWidth = _objectsImageLibrary[imageIndex];
                                            _objectsImageLibrary[imageIndex].ShowClipped(WinWidth / (_mapWidth * 2) + X * WinWidth / _mapWidth - objectsImageLibraryWidth.Width / 2, WinHeight / (_mapHeight * 2) + Y * WinHeight / _mapHeight - objectsImageLibraryHeight.Height / 2 + yOffset, lockedSurface);
                                        }
                                    }
                                }

                                if (army > 0 && _armies)
                                {
                                    imageIndex = army * 6 + 8;
                                    if (imageIndex < _objectsImageLibrary.Count && imageIndex > -1 && _objectsImageLibrary[imageIndex] != null)
                                    {
                                        var objectsImageLibraryHeight = _objectsImageLibrary[imageIndex];
                                        var objectsImageLibraryWidth = _objectsImageLibrary[imageIndex];
                                        _objectsImageLibrary[imageIndex].ShowClipped(WinWidth / (_mapWidth * 2) + X * WinWidth / _mapWidth - objectsImageLibraryWidth.Width / 2, WinHeight / (_mapHeight * 2) + Y * WinHeight / _mapHeight - objectsImageLibraryHeight.Height / 2 + yOffset, lockedSurface);
                                    }
                                }
                            }
                        }
                    }

                    if (yOffset == 0)
                    {
                        yOffset = _halfRowInset;
                    }
                    else
                    {
                        yOffset = 0;
                    }
                }

                _objectSurface.Unlock();
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
    public bool HighLights { get => _showHighlights; set => _showHighlights = value; }

    [Published]
    public bool Buildings { get => _buildings; set => SetBuildings(value); }

    [Published]
    public bool Armies { get => _armies; set => SetArmies(value); }

    [Published]
    public bool Naturals { get => _naturals; set => SetNaturals(value); }

    [Published]
    public NotifyEvent<Scanner> OnChangeTileBox { get => _onMapPositionChange; set => _onMapPositionChange = value; }

    [Published]
    public int MapDrawSteps { get => _steps; set => SetSteps(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryFrame { get => _imageLibraryFrame; set => SetImageLibraryFrame(value); }

    [Published]
    public int ImageLibraryIndexFrame { get => _imageLibraryIndexFrame; set => SetILFIndex(value); }

    public new NotifyEvent<Scanner> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<Scanner> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<Scanner> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<Scanner> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<Scanner> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<Scanner> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<Scanner> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<Scanner> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<Scanner> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
