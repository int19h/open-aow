// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// the base of everything placed on the map. Owner is the TMapField the sprite stands on (for
// the multi-hex sprites the field of the base hexagon)
public partial class HexSprite : EngineObject, IOwned<MapField>, IOwnedSink<MapField>
{
    public HexSprite(ECustomNode owner) : base(owner)
    { }

    // The field the sprite stands on. AcceptsOwner admits only a field, so the cast never fails.
    public new MapField Owner { get => (MapField)base.Owner; set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(ECustomNode owner) => owner is MapField;

    void IOwnedSink<MapField>.SetOwner(MapField owner) => SetOwner(owner);

    public new const int ClassId = 65872;

    // HXSPRITE.PAS:6
    // { HXSPRITE.PAS: THexagonSprite, THPBasedHS, TSingleHS, TFixedVisibleHS, TCustomFixedVisibleHS
    //   (include file of HSEngine.pas; the original Asserts name D:\GEngine\HSENGINE\inc\HXSPRITE.PAS) }
    public override void Dispose()
    {
        MainRemove();
        base.Dispose();
    }

    // HXSPRITE.PAS:116
    public virtual sbyte GetTerrain(int hexNumber) => -2;

    // HXSPRITE.PAS:121
    public virtual void SetTerrain(int hexNumber, sbyte value)
    {
    }

    // HXSPRITE.PAS:125
    public virtual int GetTerrainCount() => 0;

    // HXSPRITE.PAS:130
    public virtual sbyte GetOverlay(int hexNumber) => -2;

    // HXSPRITE.PAS:135
    public virtual void SetOverlay(int hexNumber, sbyte value)
    {
    }

    // HXSPRITE.PAS:139
    public virtual int GetOverlayCount() => 0;

    // HXSPRITE.PAS:52
    public virtual byte GetLevel()
    {
        throw new InvalidOperationException("No HexagonSprite Level defined");
    }

    // HXSPRITE.PAS:43
    public virtual void SetVisible(bool value)
    {
    }

    // HXSPRITE.PAS:47
    public virtual bool GetVisible() => false;

    // HXSPRITE.PAS:72
    public virtual bool GetSelected() => false;

    // HXSPRITE.PAS:87
    public virtual sbyte GetHexX() => Owner.HexX;

    // HXSPRITE.PAS:92
    public virtual sbyte GetHexY() => Owner.HexY;

    // HXSPRITE.PAS:97
    public virtual sbyte GetHexLevel() => (sbyte)Owner.Owner.HexLevel;

    // HXSPRITE.PAS:102
    public virtual int GetMapCoordinate() => GetHexX() + (GetHexY() << 8) + (GetHexLevel() << 16);

    // HXSPRITE.PAS:153
    public virtual bool GetEditMode() => false;

    // HXSPRITE.PAS:158
    public virtual void SetEditMode(bool value)
    {
    }

    // HXSPRITE.PAS:162
    public virtual int GetShowPriority() => 2130706432;

    // HXSPRITE.PAS:189
    public virtual void Connect()
    {
    }

    // HXSPRITE.PAS:193
    public virtual void Disconnect()
    {
    }

    // HXSPRITE.PAS:167
    public virtual bool CanChangeTerrain(in HexSpriteSenderInfo info) => true;

    // HXSPRITE.PAS:172
    public virtual void ChangeTerrain(in HexSpriteTerrainInfo info)
    {
    }

    // HXSPRITE.PAS:176
    public virtual void TerrainChanged(in HexSpriteTerrainInfo info)
    {
    }

    // HXSPRITE.PAS:180
    public virtual void NeighbourTerrainChanged(int hexNumber, in HexSpriteTerrainInfo info)
    {
    }

    // HSEngine.pas:881
    public virtual void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // HXSPRITE.PAS:197
    public virtual bool PlaceOnMap()
    {
        Connect();
        return true;
    }

    // HXSPRITE.PAS:203
    public virtual void RemoveFromMap()
    {
        Disconnect();
    }

    // HXSPRITE.PAS:573
    public virtual bool MainPlace(MapContainer map, int pixelX, int pixelY, int hexLevel)
    {
        bool result = Place(map, pixelX, pixelY, hexLevel);
        if (result)
        {
            try
            {
                Activate();
            }
            catch
            {
                result = false;
                Deactivate();
            }
        }

        return result;
    }

    // HXSPRITE.PAS:585
    public virtual void MainRemove()
    {
        Deactivate();
        Remove();
        Assert(!GetVisible(), "Hexagon Sprite Still Visible!", "HXSPRITE.PAS", 589);
    }

    // HXSPRITE.PAS:592
    public virtual bool MainLoaded() => Loaded();

    // HXSPRITE.PAS:184
    public virtual bool Place(MapContainer map, int pixelX, int pixelY, int hexLevel) => PlaceOnMap();

    // HXSPRITE.PAS:208
    public virtual void Remove()
    {
        RemoveFromMap();
    }

    // HXSPRITE.PAS:213
    public virtual bool Loaded()
    {
        Connect();
        return true;
    }

    // HXSPRITE.PAS:26
    public virtual void MapFieldMessageProc(ref MapFieldMessage message)
    {
    }

    // HXSPRITE.PAS:30
    public virtual void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
    }

    // HXSPRITE.PAS:34
    public virtual int CanMoveOver(in HexSpriteSenderInfo info) => 1;

    // HXSPRITE.PAS:39
    public virtual void MoveExclusive(in HexSpriteSenderInfo info)
    {
    }

    // HXSPRITE.PAS:63
    public virtual void Changed()
    {
        MapField mapField;
        mapField = Owner;
        if (mapField != null)
        {
            ((GameEngine)mapField.Owner.Engine).HexSpriteMap.HexSpriteChangedEventsStore.Raise(this);
        }
    }

    // HXSPRITE.PAS:563
    public virtual void Activate()
    {
    }

    // HXSPRITE.PAS:567
    public virtual void Deactivate()
    {
        if (GetSelected())
        {
            Unselect();
        }
    }

    // HXSPRITE.PAS:22
    public virtual void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
    }

    // HXSPRITE.PAS:144
    public virtual void MakeVisible(VisibilityList visibilityList)
    {
        if (!GetVisible())
        {
            SetVisible(true);
            visibilityList.AddSortChild(this, GetShowPriority());
        }
    }

    // HXSPRITE.PAS:82
    public virtual void GetBaseHex(ref int mapCoordinate)
    {
        mapCoordinate = Owner.HexX + (Owner.HexY << 8) + (Owner.Owner.HexLevel << 16);
    }

    // HXSPRITE.PAS:57
    public virtual bool CanSelect(int pixelX, int pixelY) => Owner != null && ((GameEngine)Owner.Owner.Engine).HexSpriteMap.Status.Contains(HexSpriteMapStatusElement.Editting);

    // HXSPRITE.PAS:107
    public virtual void Select()
    {
    }

    // HXSPRITE.PAS:111
    public virtual void Unselect()
    {
        Owner.Owner.Map.Selection.Unselect(this);
    }

    // HXSPRITE.PAS:17
    public virtual string EditName() => "Hexagon Sprite";

    // HXSPRITE.PAS:219
    public bool PlaceOnMap(MapContainer map, int pixelX, int pixelY, int hexLevel, int width, int height)
    {
        bool result;
        int X = default;
        int Y = default;
        int minX = default;
        int minYOdd = default;
        int minYEven;
        int maxX = default;
        int maxYOdd = default;
        int maxYEven;
        MapLevel mapL;
        HexGeometry.PixelToHex(pixelX - 8, pixelY, ref minX, ref minYOdd);
        HexGeometry.PixelToHex(pixelX - 8 + 32, pixelY, ref X, ref Y);
        if (!((minX & 1) != 0))
        {
            minYEven = minYOdd;
            minYOdd = Y;
        }
        else
        {
            minYEven = Y;
        }

        Y = pixelY + height - 1;
        HexGeometry.PixelToHex(width + pixelX + 8 - 1, Y, ref maxX, ref maxYOdd);
        HexGeometry.PixelToHex(width + pixelX + 8 - 1 + 32, Y, ref X, ref Y);
        if (!((maxX & 1) != 0))
        {
            maxYEven = maxYOdd;
            maxYOdd = Y;
        }
        else
        {
            maxYEven = Y;
        }

        if (maxX >= 0 && map.Width > minX && (maxYEven >= 0 || maxYOdd >= 0) && (map.Height > minYEven || map.Height > minYOdd))
        {
            if (minX < 0)
            {
                minX = 0;
            }

            if (maxX >= map.Width)
            {
                maxX = map.Width - 1;
            }

            if (minYEven < 0)
            {
                minYEven = 0;
                if (maxYEven < 0)
                {
                    maxYEven = 0;
                }
            }

            if (maxYEven >= map.Height)
            {
                maxYEven = map.Height - 1;
                if (minYEven >= map.Height)
                {
                    minYEven = map.Height - 1;
                }
            }

            if (minYOdd < 0)
            {
                minYOdd = 0;
                if (maxYOdd < 0)
                {
                    maxYOdd = 0;
                }
            }

            if (maxYOdd >= map.Height)
            {
                maxYOdd = map.Height - 1;
                if (minYOdd >= map.Height)
                {
                    minYOdd = map.Height - 1;
                }
            }

            mapL = map.GetMapLevel(hexLevel);
            X = (int)(((uint)(minX + maxX)) >> 1);
            if (!((X & 1) != 0))
            {
                Y = (int)(((uint)(minYEven + maxYEven)) >> 1);
            }
            else
            {
                Y = (int)(((uint)(minYOdd + maxYOdd)) >> 1);
            }

            SetOwnerWithoutNotify(mapL.GetField(X, Y));
            X = minX;
            int last = maxX;
            for (; X <= last; X += 1)
            {
                if (!((X & 1) != 0))
                {
                    Y = minYEven;
                    for (; Y <= maxYEven; Y += 1)
                    {
                        mapL.GetField(X, Y).AddHexSprite(this, 1);
                    }
                }
                else
                {
                    Y = minYOdd;
                    int limit = maxYOdd;
                    for (; Y <= limit; Y += 1)
                    {
                        mapL.GetField(X, Y).AddHexSprite(this, 1);
                    }
                }
            }

            Owner.SetHexNumberOfHexSprite(this, 0);
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // HXSPRITE.PAS:295
    public void RemoveFromMap(int pixelX, int pixelY, int width, int height)
    {
        int X = default;
        int Y = default;
        int minX = default;
        int minYOdd = default;
        int minYEven;
        int maxX = default;
        int maxYOdd = default;
        int maxYEven;
        MapLevel mapL;
        mapL = Owner.Owner;
        HexGeometry.PixelToHex(pixelX - 8, pixelY, ref minX, ref minYOdd);
        HexGeometry.PixelToHex(pixelX - 8 + 32, pixelY, ref X, ref Y);
        if (!((minX & 1) != 0))
        {
            minYEven = minYOdd;
            minYOdd = Y;
        }
        else
        {
            minYEven = Y;
        }

        Y = pixelY + height - 1;
        HexGeometry.PixelToHex(width + pixelX + 8 - 1, Y, ref maxX, ref maxYOdd);
        HexGeometry.PixelToHex(width + pixelX + 8 - 1 + 32, Y, ref X, ref Y);
        if (!((maxX & 1) != 0))
        {
            maxYEven = maxYOdd;
            maxYOdd = Y;
        }
        else
        {
            maxYEven = Y;
        }

        if (maxX >= 0 && minX < mapL.Width && (maxYEven >= 0 || maxYOdd >= 0) && (mapL.Height > minYEven || mapL.Height > minYOdd))
        {
            if (minX < 0)
            {
                minX = 0;
            }

            if (maxX >= mapL.Width)
            {
                maxX = mapL.Width - 1;
            }

            if (minYEven < 0)
            {
                minYEven = 0;
                if (maxYEven < 0)
                {
                    maxYEven = 0;
                }
            }

            if (maxYEven >= mapL.Height)
            {
                maxYEven = mapL.Height - 1;
                if (minYEven >= mapL.Height)
                {
                    minYEven = mapL.Height - 1;
                }
            }

            if (minYOdd < 0)
            {
                minYOdd = 0;
                if (maxYOdd < 0)
                {
                    maxYOdd = 0;
                }
            }

            if (maxYOdd >= mapL.Height)
            {
                maxYOdd = mapL.Height - 1;
                if (minYOdd >= mapL.Height)
                {
                    minYOdd = mapL.Height - 1;
                }
            }

            X = minX;
            int last = maxX;
            for (; X <= last; X += 1)
            {
                if (!((X & 1) != 0))
                {
                    Y = minYEven;
                    for (; Y <= maxYEven; Y += 1)
                    {
                        mapL.GetField(X, Y).RemoveHexSprite(this);
                    }
                }
                else
                {
                    Y = minYOdd;
                    int limit = maxYOdd;
                    for (; Y <= limit; Y += 1)
                    {
                        mapL.GetField(X, Y).RemoveHexSprite(this);
                    }
                }
            }
        }

        SetOwnerWithoutNotify(null);
    }

    // HXSPRITE.PAS:362
    public bool PlaceUserOnMap(MapContainer map, int pixelX, int pixelY, int hexLevel, int width, int height)
    {
        bool result;
        int X = default;
        int Y = default;
        int minX = default;
        int minYOdd = default;
        int minYEven;
        int maxX = default;
        int maxYOdd = default;
        int maxYEven;
        MapLevel mapL;
        HexGeometry.PixelToHex(pixelX - 8, pixelY, ref minX, ref minYOdd);
        HexGeometry.PixelToHex(pixelX - 8 + 32, pixelY, ref X, ref Y);
        if (!((minX & 1) != 0))
        {
            minYEven = minYOdd;
            minYOdd = Y;
        }
        else
        {
            minYEven = Y;
        }

        Y = pixelY + height - 1;
        HexGeometry.PixelToHex(width + pixelX + 8 - 1, Y, ref maxX, ref maxYOdd);
        HexGeometry.PixelToHex(width + pixelX + 8 - 1 + 32, Y, ref X, ref Y);
        if (!((maxX & 1) != 0))
        {
            maxYEven = maxYOdd;
            maxYOdd = Y;
        }
        else
        {
            maxYEven = Y;
        }

        if (maxX >= 0 && map.Width > minX && (maxYEven >= 0 || maxYOdd >= 0) && (map.Height > minYEven || map.Height > minYOdd))
        {
            if (minX < 0)
            {
                minX = 0;
            }

            if (maxX >= map.Width)
            {
                maxX = map.Width - 1;
            }

            if (minYEven < 0)
            {
                minYEven = 0;
                if (maxYEven < 0)
                {
                    maxYEven = 0;
                }
            }

            if (maxYEven >= map.Height)
            {
                maxYEven = map.Height - 1;
                if (minYEven >= map.Height)
                {
                    minYEven = map.Height - 1;
                }
            }

            if (minYOdd < 0)
            {
                minYOdd = 0;
                if (maxYOdd < 0)
                {
                    maxYOdd = 0;
                }
            }

            if (maxYOdd >= map.Height)
            {
                maxYOdd = map.Height - 1;
                if (minYOdd >= map.Height)
                {
                    minYOdd = map.Height - 1;
                }
            }

            mapL = map.GetMapLevel(hexLevel);
            X = minX;
            int last = maxX;
            for (; X <= last; X += 1)
            {
                if (!((X & 1) != 0))
                {
                    Y = minYEven;
                    for (; Y <= maxYEven; Y += 1)
                    {
                        mapL.GetField(X, Y).AddHexSprite(this, 1);
                    }
                }
                else
                {
                    Y = minYOdd;
                    int limit = maxYOdd;
                    for (; Y <= limit; Y += 1)
                    {
                        mapL.GetField(X, Y).AddHexSprite(this, 1);
                    }
                }
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // HXSPRITE.PAS:431
    public void RemoveUserFromMap(MapContainer map, int pixelX, int pixelY, int hexLevel, int width, int height)
    {
        int X = default;
        int Y = default;
        int minX = default;
        int minYOdd = default;
        int minYEven;
        int maxX = default;
        int maxYOdd = default;
        int maxYEven;
        MapLevel mapL;
        mapL = map.GetMapLevel(hexLevel);
        HexGeometry.PixelToHex(pixelX - 8, pixelY, ref minX, ref minYOdd);
        HexGeometry.PixelToHex(pixelX - 8 + 32, pixelY, ref X, ref Y);
        if (!((minX & 1) != 0))
        {
            minYEven = minYOdd;
            minYOdd = Y;
        }
        else
        {
            minYEven = Y;
        }

        Y = pixelY + height - 1;
        HexGeometry.PixelToHex(width + pixelX + 8 - 1, Y, ref maxX, ref maxYOdd);
        HexGeometry.PixelToHex(width + pixelX + 8 - 1 + 32, Y, ref X, ref Y);
        if (!((maxX & 1) != 0))
        {
            maxYEven = maxYOdd;
            maxYOdd = Y;
        }
        else
        {
            maxYEven = Y;
        }

        if (maxX >= 0 && minX < mapL.Width && (maxYEven >= 0 || maxYOdd >= 0) && (mapL.Height > minYEven || mapL.Height > minYOdd))
        {
            if (minX < 0)
            {
                minX = 0;
            }

            if (maxX >= mapL.Width)
            {
                maxX = mapL.Width - 1;
            }

            if (minYEven < 0)
            {
                minYEven = 0;
                if (maxYEven < 0)
                {
                    maxYEven = 0;
                }
            }

            if (maxYEven >= mapL.Height)
            {
                maxYEven = mapL.Height - 1;
                if (minYEven >= mapL.Height)
                {
                    minYEven = mapL.Height - 1;
                }
            }

            if (minYOdd < 0)
            {
                minYOdd = 0;
                if (maxYOdd < 0)
                {
                    maxYOdd = 0;
                }
            }

            if (maxYOdd >= mapL.Height)
            {
                maxYOdd = mapL.Height - 1;
                if (minYOdd >= mapL.Height)
                {
                    minYOdd = mapL.Height - 1;
                }
            }

            X = minX;
            int last = maxX;
            for (; X <= last; X += 1)
            {
                if (!((X & 1) != 0))
                {
                    Y = minYEven;
                    for (; Y <= maxYEven; Y += 1)
                    {
                        mapL.GetField(X, Y).RemoveHexSprite(this);
                    }
                }
                else
                {
                    Y = minYOdd;
                    int limit = maxYOdd;
                    for (; Y <= limit; Y += 1)
                    {
                        mapL.GetField(X, Y).RemoveHexSprite(this);
                    }
                }
            }
        }
    }

    // HXSPRITE.PAS:497
    public void SortMapField(MapContainer map, int pixelX, int pixelY, int hexLevel, int width, int height)
    {
        int X = default;
        int Y = default;
        int minX = default;
        int minYOdd = default;
        int minYEven;
        int maxX = default;
        int maxYOdd = default;
        int maxYEven;
        MapLevel mapL;
        mapL = map.GetMapLevel(hexLevel);
        HexGeometry.PixelToHex(pixelX - 8, pixelY, ref minX, ref minYOdd);
        HexGeometry.PixelToHex(pixelX - 8 + 32, pixelY, ref X, ref Y);
        if (!((minX & 1) != 0))
        {
            minYEven = minYOdd;
            minYOdd = Y;
        }
        else
        {
            minYEven = Y;
        }

        Y = pixelY + height - 1;
        HexGeometry.PixelToHex(width + pixelX + 8 - 1, Y, ref maxX, ref maxYOdd);
        HexGeometry.PixelToHex(width + pixelX + 8 - 1 + 32, Y, ref X, ref Y);
        if (!((maxX & 1) != 0))
        {
            maxYEven = maxYOdd;
            maxYOdd = Y;
        }
        else
        {
            maxYEven = Y;
        }

        if (maxX >= 0 && minX < mapL.Width && (maxYEven >= 0 || maxYOdd >= 0) && (mapL.Height > minYEven || mapL.Height > minYOdd))
        {
            if (minX < 0)
            {
                minX = 0;
            }

            if (maxX >= mapL.Width)
            {
                maxX = mapL.Width - 1;
            }

            if (minYEven < 0)
            {
                minYEven = 0;
                if (maxYEven < 0)
                {
                    maxYEven = 0;
                }
            }

            if (maxYEven >= mapL.Height)
            {
                maxYEven = mapL.Height - 1;
                if (minYEven >= mapL.Height)
                {
                    minYEven = mapL.Height - 1;
                }
            }

            if (minYOdd < 0)
            {
                minYOdd = 0;
                if (maxYOdd < 0)
                {
                    maxYOdd = 0;
                }
            }

            if (maxYOdd >= mapL.Height)
            {
                maxYOdd = mapL.Height - 1;
                if (minYOdd >= mapL.Height)
                {
                    minYOdd = mapL.Height - 1;
                }
            }

            X = minX;
            int last = maxX;
            for (; X <= last; X += 1)
            {
                if (!((X & 1) != 0))
                {
                    Y = minYEven;
                    for (; Y <= maxYEven; Y += 1)
                    {
                        mapL.GetField(X, Y).Sort();
                    }
                }
                else
                {
                    Y = minYOdd;
                    int limit = maxYOdd;
                    for (; Y <= limit; Y += 1)
                    {
                        mapL.GetField(X, Y).Sort();
                    }
                }
            }
        }
    }

    public int TerrainCount { get => GetTerrainCount(); }
    public int OverlayCount { get => GetOverlayCount(); }
    public byte Level { get => GetLevel(); }
    public bool Visible { get => GetVisible(); set => SetVisible(value); }
    public bool Selected { get => GetSelected(); }
    public sbyte HexX { get => GetHexX(); }
    public sbyte HexY { get => GetHexY(); }
    public sbyte HexLevel { get => GetHexLevel(); }
    public int MapCoordinate { get => GetMapCoordinate(); }
    public bool EditMode { get => GetEditMode(); set => SetEditMode(value); }
    public int ShowPriority { get => GetShowPriority(); }

    // HXSPRITE.PAS:77
    public virtual Set<ControlModeElement> ControlMode() => Set<ControlModeElement>.Of();

}
