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
    public int HalfRowInset;
    public ImageLibrary _tilesImageLibrary;
    public ImageLibrary HighlightImageLibrary;
    public ImageLibrary _fogImageLibrary;
    public ImageLibrary ObjectsImageLibrary;
    public int TTop;
    public int THeight;
    public int TLeft;
    public int TWidth;
    public bool ShowHighlights;
    public bool _buildings;
    public bool _armies;
    public bool _naturals;
    public NotifyEvent<Scanner> OnMapPositionChange;
    public int Steps;
    public GameMap Map;
    public InterfaceImageLibrary _imageLibraryFrame;
    public int _imageLibraryIndexFrame;
    public int MapWidth;
    public int MapHeight;
    public MapLevel Level;
    public bool ObjectsDrawn;
    public int PreviousWidth;
    public int PreviousHeight;
    public int DrawableHeight;
    public Surface MapSurface;
    public Surface ObjectSurface;
    public int UpdateStep;
    public bool Incremental;

    // Scanner.pas:126
    public override void Dispose()
    {
        if (MapSurface != null)
        {
            MapSurface.Dispose();
        }

        if (ObjectSurface != null)
        {
            ObjectSurface.Dispose();
        }

        base.Dispose();
    }

    // Scanner.pas:116
    public Scanner(Component owner) : base(owner)
    {
        Steps = 1;
        TLeft = -1;
        TTop = -1;
        TWidth = -1;
        THeight = -1;
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

        if (_imageLibraryFrame != null && _tilesImageLibrary != null && ObjectsImageLibrary != null && Map != null && _fogImageLibrary != null && HighlightImageLibrary != null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // Scanner.pas:409
    public override void SetSize()
    {
        LockedSurface lockedSurface = default;
        if ((_winWidth != PreviousWidth || _winHeight != PreviousHeight) && _winWidth > 0 && _winHeight > 0)
        {
            HalfRowInset = _winHeight / MapHeight / 2;
            DrawableHeight = _winHeight - HalfRowInset;
            if (MapSurface != null)
            {
                ObjectSurface.Resize(_winWidth, _winHeight);
                ObjectSurface.Clear(65280);
                MapSurface.Resize(_winWidth, _winHeight);
                MapSurface.Lock(ref lockedSurface);
                for (int X = 0, last = _winWidth / _fogImageLibrary[0].Width; X <= last; X++)
                {
                    for (int Y = 0, lastY = _winHeight / _fogImageLibrary[0].Height; Y <= lastY; Y++)
                    {
                        var fogImageLibraryHeight = _fogImageLibrary[0];
                        var fogImageLibraryWidth = _fogImageLibrary[0];
                        _fogImageLibrary[0].ShowClipped(X * fogImageLibraryWidth.Width, Y * fogImageLibraryHeight.Height, lockedSurface);
                    }
                }

                MapSurface.Unlock();
                if (!Incremental)
                {
                    DrawMap(0, 0, MapWidth, MapHeight);
                    UpdateStep = Steps;
                }
                else
                {
                    UpdateStep = -1;
                }
            }

            PreviousWidth = _winWidth;
            PreviousHeight = _winHeight;
        }

        base.SetSize();
    }

    // Scanner.pas:325
    public override void Update()
    {
        base.Update();
        if (_visible && MapSurface != null)
        {
            if (!Incremental)
            {
                Incremental = true;
            }

            if (UpdateStep < Steps)
            {
                if (UpdateStep > -1)
                {
                    DrawMap(0, MapHeight * UpdateStep / Steps, MapWidth, MapHeight / Steps + 1);
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
            if (Valid && _visible)
            {
                if (_onPreDraw != null)
                {
                    _onPreDraw(this, lockedSurface);
                }

                if (MapSurface != null && MapSurface.PixelFormatId != lockedSurface.PixelFormatId)
                {
                    ObjectSurface?.Dispose();
                    MapSurface?.Dispose();
                    MapSurface = null;
                }

                if (MapSurface == null)
                {
                    ObjectSurface = new DataSurface(_winWidth, _winHeight, lockedSurface.PixelFormatId, null);
                    ObjectSurface.Clear(65280);
                    MapSurface = new DataSurface(_winWidth, _winHeight, lockedSurface.PixelFormatId, null);
                    MapSurface.Lock(ref surfaceDescription);
                    for (int X = 0, last = _winWidth / _fogImageLibrary[0].Width; X <= last; X++)
                    {
                        for (int Y = 0, lastY = _winHeight / _fogImageLibrary[0].Height; Y <= lastY; Y++)
                        {
                            var fogImageLibraryHeight = _fogImageLibrary[0];
                            var fogImageLibraryWidth = _fogImageLibrary[0];
                            _fogImageLibrary[0].ShowClipped(X * fogImageLibraryWidth.Width, Y * fogImageLibraryHeight.Height, surfaceDescription);
                        }
                    }

                    MapSurface.Unlock();
                    if (!Incremental)
                    {
                        DrawMap(0, 0, MapWidth, MapHeight);
                        UpdateStep = Steps;
                    }
                    else
                    {
                        UpdateStep = -1;
                    }
                }

                if (!ObjectsDrawn)
                {
                    DrawObjects();
                }

                R = new Rect(0, 0, _winWidth, _winHeight);
                surfaceDescription = lockedSurface;
                var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, _window._winLeft + _winLeft + _winWidth, _window._winTop + _winTop + _winHeight);
                surfaceDescription.ClipRectangle = rectangle;
                MapSurface.Lock(ref lockedSurface);
                LockedSurface.BlitSurfaceDescriptionRectangleClipped(lockedSurface, R, surfaceDescription, _window._winLeft + _winLeft, _window._winTop + _winTop);
                MapSurface.Unlock();
                ObjectSurface.Lock(ref lockedSurface);
                LockedSurface.BlitSpriteClipped16(lockedSurface, R, surfaceDescription, _window._winLeft + _winLeft, _window._winTop + _winTop, 65280);
                ObjectSurface.Unlock();
                if (MapWidth > 0 && MapHeight > 0)
                {
                    var rectangle2 = new Rect(_window._winLeft + _winLeft + TLeft * _winWidth / MapWidth, _window._winTop + _winTop + TTop * _winHeight / MapHeight, TWidth * _winWidth / MapWidth, (THeight + 2) * _winHeight / MapHeight);
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
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            MouseDown = true;
            X = (X - _window._winLeft - _winLeft) * MapWidth / _winWidth;
            mapY = (Y - _window._winTop - _winTop) * MapHeight / DrawableHeight;
            oldLeft = TLeft;
            oldTop = TTop;
            TLeft = X - TWidth / 2;
            TTop = mapY - THeight / 2;
            if (TTop != oldTop || oldLeft != TLeft)
            {
                Drawn = false;
                if (OnMapPositionChange != null)
                {
                    OnMapPositionChange(this);
                }
            }
        }
    }

    // Scanner.pas:452
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
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
        if (X >= _window._winLeft + _winLeft && X <= _window._winLeft + _winLeft + _winWidth - 1 && Y >= _window._winTop + _winTop && Y <= _window._winTop + _winTop + _winHeight - 1)
        {
            Handled = true;
            if (MouseDown == true)
            {
                oldLeft = TLeft;
                oldTop = TTop;
                mapX = (X - _window._winLeft - _winLeft) * MapWidth / _winWidth;
                mapY = (Y - _window._winTop - _winTop) * MapHeight / DrawableHeight;
                TLeft = mapX - TWidth / 2;
                TTop = mapY - THeight / 2;
                if (TTop != oldTop || TLeft != oldLeft)
                {
                    Drawn = false;
                    if (OnMapPositionChange != null)
                    {
                        OnMapPositionChange(this);
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
        if (value != ObjectsImageLibrary)
        {
            Valid = false;
            ObjectsImageLibrary = value;
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
            MapWidth = Level.Width - 16;
            MapHeight = Level.Height - 12;
        }
        else
        {
            MapWidth = Level.Width - 12;
            MapHeight = Level.Height - 12;
        }
    }

    // Scanner.pas:205
    public void SetMapWidth(int value)
    {
        if (AoWHSMap.Current.CombatMap != null)
        {
            MapWidth = value - 16;
        }
        else
        {
            MapWidth = value - 12;
        }
    }

    // Scanner.pas:211
    public void SetMapHeight(int value)
    {
        if (AoWHSMap.Current.CombatMap != null)
        {
            MapHeight = value - 12;
        }
        else
        {
            // { Both original branches subtract 12. }
            MapHeight = value - 12;
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

        if (Map.ExplorationEnabled)
        {
            if (field.GetExplored(Map.SeatedPlayer))
            {
                if (Map.FogEnabled)
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
            if (Map.FogEnabled)
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
        HighlightImageLibrary = value;
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
        Steps = value;
        if (Steps < 1)
        {
            Steps = 1;
        }
    }

    // Scanner.pas:319
    public void SetObDrawn(bool value)
    {
        ObjectsDrawn = value;
        if (!ObjectsDrawn)
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
        if (value != TWidth)
        {
            Drawn = false;
            TWidth = value;
        }
    }

    // Scanner.pas:368
    public void SetTHeight(int value)
    {
        if (value != THeight)
        {
            Drawn = false;
            THeight = value;
        }
    }

    // Scanner.pas:377
    public void SetMap(GameMap value)
    {
        Map = value;
        if (Map != null)
        {
            Level = Map.MapContainer.GetMapLevel(0);
            if (AoWHSMap.Current.CombatMap != null)
            {
                MapWidth = Level.Width - 16;
                MapHeight = Level.Height - 12;
            }
            else
            {
                MapWidth = Level.Width - 12;
                MapHeight = Level.Height - 12;
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
            if (Valid && MapSurface != null && _visible)
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
                    yOffset = HalfRowInset;
                }

                if (MapSurface.Lock(ref lockedSurface) >= 0)
                {
                    if (MapWidth < width + X)
                    {
                        width = MapWidth - X;
                    }

                    if (MapHeight < Y + height)
                    {
                        height = MapHeight - Y;
                    }

                    for (int mapX = X; mapX <= width + X - 1; mapX++)
                    {
                        for (int mapY = Y; mapY <= Y + height - 1; mapY++)
                        {
                            var rectangle = new Rect(_winWidth * mapX / MapWidth, DrawableHeight * mapY / MapHeight + yOffset, (mapX + 1) * _winWidth / MapWidth, (mapY + 1) * DrawableHeight / MapHeight + yOffset);
                            lockedSurface.ClipRectangle = rectangle;
                            explored = GetExplored(mapX, mapY);
                            if (explored == 0)
                            {
                                for (int tileX = _winWidth * mapX / MapWidth / _fogImageLibrary[0].Width, last = (mapX + 1) * _winWidth / MapWidth / _fogImageLibrary[0].Width; tileX <= last; tileX++)
                                {
                                    for (int tileY = (DrawableHeight * mapY / MapHeight + yOffset) / _fogImageLibrary[0].Height, lastTileY = ((mapY + 1) * DrawableHeight / MapHeight + yOffset) / _fogImageLibrary[0].Height; tileY <= lastTileY; tileY++)
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
                                    for (int tileX = _winWidth * mapX / MapWidth / _fogImageLibrary[0].Width, last = (mapX + 1) * _winWidth / MapWidth / _fogImageLibrary[0].Width; tileX <= last; tileX++)
                                    {
                                        for (int tileY = (DrawableHeight * mapY / MapHeight + yOffset) / _fogImageLibrary[0].Height, lastTileY = ((mapY + 1) * DrawableHeight / MapHeight + yOffset) / _fogImageLibrary[0].Height; tileY <= lastTileY; tileY++)
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
                                        for (int tileX = _winWidth * mapX / MapWidth / _tilesImageLibrary[imageIndex].Width, last = (mapX + 1) * _winWidth / MapWidth / _tilesImageLibrary[imageIndex].Width; tileX <= last; tileX++)
                                        {
                                            for (int tileY = (DrawableHeight * mapY / MapHeight + yOffset) / _tilesImageLibrary[imageIndex].Height, lastTileY = ((mapY + 1) * DrawableHeight / MapHeight + yOffset) / _tilesImageLibrary[imageIndex].Height; tileY <= lastTileY; tileY++)
                                            {
                                                var tilesImageLibraryHeight = _tilesImageLibrary[imageIndex];
                                                var tilesImageLibraryWidth = _tilesImageLibrary[imageIndex];
                                                _tilesImageLibrary[imageIndex].ShowClipped(tileX * tilesImageLibraryWidth.Width, tileY * tilesImageLibraryHeight.Height, lockedSurface);
                                            }
                                        }
                                    }

                                    if ((overlay == 0 || overlay == 10) && ShowHighlights == true && mapY < MapHeight - 1 && mapX > 0 && GetOverlay(mapX - 1, mapY + 1) != overlay)
                                    {
                                        HighlightImageLibrary[1].ShowClipped(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, lockedSurface);
                                    }

                                    if ((overlay == 0 || overlay == 10) && ShowHighlights == true && mapY > 0 && mapX < MapWidth - 1 && GetOverlay(mapX + 1, mapY - 1) != overlay)
                                    {
                                        HighlightImageLibrary[0].ShowClipped(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, lockedSurface);
                                    }

                                    if (explored == 2)
                                    {
                                        for (int tileX = _winWidth * mapX / MapWidth / _fogImageLibrary[1].Width, last = (mapX + 1) * _winWidth / MapWidth / _fogImageLibrary[1].Width; tileX <= last; tileX++)
                                        {
                                            for (int tileY = (DrawableHeight * mapY / MapHeight + yOffset) / _fogImageLibrary[1].Height, lastTileY = ((mapY + 1) * DrawableHeight / MapHeight + yOffset) / _fogImageLibrary[1].Height; tileY <= lastTileY; tileY++)
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
                            yOffset = HalfRowInset;
                        }
                        else
                        {
                            yOffset = 0;
                        }
                    }

                    MapSurface.Unlock();
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
        if (ObjectSurface != null)
        {
            ObjectSurface.Clear(65280);
            yOffset = 0;
            if (ObjectSurface.Lock(ref lockedSurface) >= 0)
            {
                for (int X = 0, last = MapWidth - 1; X <= last; X++)
                {
                    for (int Y = 0, lastY = MapHeight - 1; Y <= lastY; Y++)
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
                                    if (imageIndex < ObjectsImageLibrary.Count && imageIndex > -1 && ObjectsImageLibrary[imageIndex] != null)
                                    {
                                        var objectsImageLibraryHeight = ObjectsImageLibrary[imageIndex];
                                        var objectsImageLibraryWidth = ObjectsImageLibrary[imageIndex];
                                        ObjectsImageLibrary[imageIndex].ShowClipped(_winWidth / (MapWidth * 2) + X * _winWidth / MapWidth - objectsImageLibraryWidth.Width / 2, _winHeight / (MapHeight * 2) + Y * _winHeight / MapHeight - objectsImageLibraryHeight.Height / 2 + yOffset, lockedSurface);
                                    }
                                }
                                else
                                {
                                    if (natural > 0 && _naturals)
                                    {
                                        imageIndex = (int)(((uint)natural) >> 8) * 6 + 9;
                                        if (imageIndex < ObjectsImageLibrary.Count && imageIndex > -1 && ObjectsImageLibrary[imageIndex] != null)
                                        {
                                            var objectsImageLibraryHeight = ObjectsImageLibrary[imageIndex];
                                            var objectsImageLibraryWidth = ObjectsImageLibrary[imageIndex];
                                            ObjectsImageLibrary[imageIndex].ShowClipped(_winWidth / (MapWidth * 2) + X * _winWidth / MapWidth - objectsImageLibraryWidth.Width / 2, _winHeight / (MapHeight * 2) + Y * _winHeight / MapHeight - objectsImageLibraryHeight.Height / 2 + yOffset, lockedSurface);
                                        }
                                    }
                                }

                                if (army > 0 && _armies)
                                {
                                    imageIndex = army * 6 + 8;
                                    if (imageIndex < ObjectsImageLibrary.Count && imageIndex > -1 && ObjectsImageLibrary[imageIndex] != null)
                                    {
                                        var objectsImageLibraryHeight = ObjectsImageLibrary[imageIndex];
                                        var objectsImageLibraryWidth = ObjectsImageLibrary[imageIndex];
                                        ObjectsImageLibrary[imageIndex].ShowClipped(_winWidth / (MapWidth * 2) + X * _winWidth / MapWidth - objectsImageLibraryWidth.Width / 2, _winHeight / (MapHeight * 2) + Y * _winHeight / MapHeight - objectsImageLibraryHeight.Height / 2 + yOffset, lockedSurface);
                                    }
                                }
                            }
                        }
                    }

                    if (yOffset == 0)
                    {
                        yOffset = HalfRowInset;
                    }
                    else
                    {
                        yOffset = 0;
                    }
                }

                ObjectSurface.Unlock();
                Drawn = false;
                SetObDrawn(true);
            }
        }
    }

    [Published]
    public ImageLibrary TilesImageLibrary { get => _tilesImageLibrary; set => SetImageLibrary(value); }

    [Published]
    public ImageLibrary HighlightsImageLibrary { get => HighlightImageLibrary; set => SetHimgLibrary(value); }

    [Published]
    public ImageLibrary FogImageLibrary { get => _fogImageLibrary; set => SetFogimgLibrary(value); }

    [Published]
    public ImageLibrary ImageLibraryObjects { get => ObjectsImageLibrary; set => SetObjectImageLibrary(value); }

    [Published]
    public int TileBoxTop { get => TTop; set => SetTTop(value); }

    [Published]
    public int TileBoxLeft { get => TLeft; set => SetTLeft(value); }

    [Published]
    public int TileBoxWidth { get => TWidth; set => SetTWidth(value); }

    [Published]
    public int TileBoxHeight { get => THeight; set => SetTHeight(value); }

    [Published]
    public bool HighLights { get => ShowHighlights; set => ShowHighlights = value; }

    [Published]
    public bool Buildings { get => _buildings; set => SetBuildings(value); }

    [Published]
    public bool Armies { get => _armies; set => SetArmies(value); }

    [Published]
    public bool Naturals { get => _naturals; set => SetNaturals(value); }

    [Published]
    public NotifyEvent<Scanner> OnChangeTileBox { get => OnMapPositionChange; set => OnMapPositionChange = value; }

    [Published]
    public int MapDrawSteps { get => Steps; set => SetSteps(value); }

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
