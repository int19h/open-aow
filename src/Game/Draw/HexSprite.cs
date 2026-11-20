#nullable disable
// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;

namespace Aow.Game.Draw;

// the base of everything placed on the map. Owner is the TMapField the sprite stands on (for
// the multi-hex sprites the field of the base hexagon)
public class HexSprite(CustomNode owner) : EngineObject(owner), IOwned<MapField>, IOwnedSink<MapField>
{

    // The field the sprite stands on. AcceptsOwner admits only a field, so the read never fails.
    public new MapField Owner { get => GetOwner<MapField>(); set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(CustomNode owner) => owner is MapField;

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
    public virtual int TerrainCount => 0;

    // HXSPRITE.PAS:130
    public virtual sbyte GetOverlay(int hexNumber) => -2;

    // HXSPRITE.PAS:135
    public virtual void SetOverlay(int hexNumber, sbyte value)
    {
    }

    // HXSPRITE.PAS:139
    public virtual int OverlayCount => 0;

    // HXSPRITE.PAS:52
    public virtual byte Level => throw new InvalidOperationException("No HexagonSprite Level defined");

    // HXSPRITE.PAS:47
    public virtual bool Visible
    {
        get => false;
        // HXSPRITE.PAS:43
        set
        {
        }
    }

    // HXSPRITE.PAS:72
    public virtual bool GetSelected() => false;

    // HXSPRITE.PAS:87
    public virtual sbyte HexX => Owner.HexX;

    // HXSPRITE.PAS:92
    public virtual sbyte HexY => Owner.HexY;

    // HXSPRITE.PAS:97
    public virtual sbyte HexLevel => (sbyte)Owner.Owner.HexLevel;

    // HXSPRITE.PAS:102
    public virtual MapCoordinate MapCoordinate => HexGeometry.PackMapCoordinate(HexX, HexY, HexLevel);

    // HXSPRITE.PAS:153
    public virtual bool EditMode
    {
        get => false;
        // HXSPRITE.PAS:158
        set
        {
        }
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
    public virtual void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface) => throw AbstractErrorException.Create("Abstract Error");

    // HXSPRITE.PAS:197
    public virtual bool PlaceOnMap()
    {
        Connect();
        return true;
    }

    // HXSPRITE.PAS:203
    public virtual void RemoveFromMap() => Disconnect();

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
        Assert(!Visible, "Hexagon Sprite Still Visible!", "HXSPRITE.PAS", 589);
    }

    // HXSPRITE.PAS:592
    public virtual bool MainLoaded() => Loaded();

    // HXSPRITE.PAS:184
    public virtual bool Place(MapContainer map, int pixelX, int pixelY, int hexLevel) => PlaceOnMap();

    // HXSPRITE.PAS:208
    public virtual void Remove() => RemoveFromMap();

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
        MapField mapField = Owner;
        mapField?.Owner.Engine.HexSpriteMap.HexSpriteChangedEventsStore.Raise(this);
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
        if (!Visible)
        {
            Visible = true;
            _ = visibilityList.AddSortChild(this, GetShowPriority());
        }
    }

    // HXSPRITE.PAS:82
    public virtual void GetBaseHex(out int mapCoordinate) => mapCoordinate = Owner.HexX + (Owner.HexY << 8) + (Owner.Owner.HexLevel << 16);

    // HXSPRITE.PAS:57
    public virtual bool CanSelect(int pixelX, int pixelY) => Owner is not null && Owner.Owner.Engine.HexSpriteMap.Status.Contains(HexSpriteMapStatusElement.Editting);

    // HXSPRITE.PAS:107
    public virtual void Select()
    {
    }

    // HXSPRITE.PAS:111
    public virtual void Unselect() => Owner.Owner.Map.Selection.Unselect(this);

    // HXSPRITE.PAS:17
    public virtual string EditName() => "Hexagon Sprite";

    // HXSPRITE.PAS:219
    public bool PlaceOnMap(MapContainer map, int pixelX, int pixelY, int hexLevel, int width, int height)
    {
        HexGeometry.PixelToHex(pixelX - 8, pixelY, out int minX, out int minYOdd);
        HexGeometry.PixelToHex(pixelX - 8 + 32, pixelY, out _, out int y);
        int minYEven;
        if (!((minX & 1) != 0))
        {
            minYEven = minYOdd;
            minYOdd = y;
        }
        else
        {
            minYEven = y;
        }

        y = pixelY + height - 1;
        HexGeometry.PixelToHex(width + pixelX + 8 - 1, y, out int maxX, out int maxYOdd);
        HexGeometry.PixelToHex(width + pixelX + 8 - 1 + 32, y, out _, out y);
        int maxYEven;
        if (!((maxX & 1) != 0))
        {
            maxYEven = maxYOdd;
            maxYOdd = y;
        }
        else
        {
            maxYEven = y;
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

            MapLevel mapL = map.GetMapLevel(hexLevel);
            int x = (minX + maxX) >>> 1;
            y = !((x & 1) != 0) ? (minYEven + maxYEven) >>> 1 : (minYOdd + maxYOdd) >>> 1;

            SetOwnerWithoutNotify(mapL.GetField(x, y));
            x = minX;
            int last = maxX;
            for (; x <= last; x += 1)
            {
                if (!((x & 1) != 0))
                {
                    y = minYEven;
                    for (; y <= maxYEven; y += 1)
                    {
                        _ = mapL.GetField(x, y).AddHexSprite(this, 1);
                    }
                }
                else
                {
                    y = minYOdd;
                    int limit = maxYOdd;
                    for (; y <= limit; y += 1)
                    {
                        _ = mapL.GetField(x, y).AddHexSprite(this, 1);
                    }
                }
            }

            _ = Owner.SetHexNumberOfHexSprite(this, 0);
            return true;
        }
        else
        {
            return false;
        }
    }

    // HXSPRITE.PAS:295
    public void RemoveFromMap(int pixelX, int pixelY, int width, int height)
    {
        MapLevel mapL = Owner.Owner;
        HexGeometry.PixelToHex(pixelX - 8, pixelY, out int minX, out int minYOdd);
        HexGeometry.PixelToHex(pixelX - 8 + 32, pixelY, out _, out int y);
        int minYEven;
        if (!((minX & 1) != 0))
        {
            minYEven = minYOdd;
            minYOdd = y;
        }
        else
        {
            minYEven = y;
        }

        y = pixelY + height - 1;
        HexGeometry.PixelToHex(width + pixelX + 8 - 1, y, out int maxX, out int maxYOdd);
        HexGeometry.PixelToHex(width + pixelX + 8 - 1 + 32, y, out _, out y);
        int maxYEven;
        if (!((maxX & 1) != 0))
        {
            maxYEven = maxYOdd;
            maxYOdd = y;
        }
        else
        {
            maxYEven = y;
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

            int x = minX;
            int last = maxX;
            for (; x <= last; x += 1)
            {
                if (!((x & 1) != 0))
                {
                    y = minYEven;
                    for (; y <= maxYEven; y += 1)
                    {
                        _ = mapL.GetField(x, y).RemoveHexSprite(this);
                    }
                }
                else
                {
                    y = minYOdd;
                    int limit = maxYOdd;
                    for (; y <= limit; y += 1)
                    {
                        _ = mapL.GetField(x, y).RemoveHexSprite(this);
                    }
                }
            }
        }

        SetOwnerWithoutNotify(null);
    }

    // HXSPRITE.PAS:362
    public bool PlaceUserOnMap(MapContainer map, int pixelX, int pixelY, int hexLevel, int width, int height)
    {
        HexGeometry.PixelToHex(pixelX - 8, pixelY, out int minX, out int minYOdd);
        HexGeometry.PixelToHex(pixelX - 8 + 32, pixelY, out _, out int y);
        int minYEven;
        if (!((minX & 1) != 0))
        {
            minYEven = minYOdd;
            minYOdd = y;
        }
        else
        {
            minYEven = y;
        }

        y = pixelY + height - 1;
        HexGeometry.PixelToHex(width + pixelX + 8 - 1, y, out int maxX, out int maxYOdd);
        HexGeometry.PixelToHex(width + pixelX + 8 - 1 + 32, y, out _, out y);
        int maxYEven;
        if (!((maxX & 1) != 0))
        {
            maxYEven = maxYOdd;
            maxYOdd = y;
        }
        else
        {
            maxYEven = y;
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

            MapLevel mapL = map.GetMapLevel(hexLevel);
            int x = minX;
            int last = maxX;
            for (; x <= last; x += 1)
            {
                if (!((x & 1) != 0))
                {
                    y = minYEven;
                    for (; y <= maxYEven; y += 1)
                    {
                        _ = mapL.GetField(x, y).AddHexSprite(this, 1);
                    }
                }
                else
                {
                    y = minYOdd;
                    int limit = maxYOdd;
                    for (; y <= limit; y += 1)
                    {
                        _ = mapL.GetField(x, y).AddHexSprite(this, 1);
                    }
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    // HXSPRITE.PAS:431
    public void RemoveUserFromMap(MapContainer map, int pixelX, int pixelY, int hexLevel, int width, int height)
    {
        MapLevel mapL = map.GetMapLevel(hexLevel);
        HexGeometry.PixelToHex(pixelX - 8, pixelY, out int minX, out int minYOdd);
        HexGeometry.PixelToHex(pixelX - 8 + 32, pixelY, out _, out int y);
        int minYEven;
        if (!((minX & 1) != 0))
        {
            minYEven = minYOdd;
            minYOdd = y;
        }
        else
        {
            minYEven = y;
        }

        y = pixelY + height - 1;
        HexGeometry.PixelToHex(width + pixelX + 8 - 1, y, out int maxX, out int maxYOdd);
        HexGeometry.PixelToHex(width + pixelX + 8 - 1 + 32, y, out _, out y);
        int maxYEven;
        if (!((maxX & 1) != 0))
        {
            maxYEven = maxYOdd;
            maxYOdd = y;
        }
        else
        {
            maxYEven = y;
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

            int x = minX;
            int last = maxX;
            for (; x <= last; x += 1)
            {
                if (!((x & 1) != 0))
                {
                    y = minYEven;
                    for (; y <= maxYEven; y += 1)
                    {
                        _ = mapL.GetField(x, y).RemoveHexSprite(this);
                    }
                }
                else
                {
                    y = minYOdd;
                    int limit = maxYOdd;
                    for (; y <= limit; y += 1)
                    {
                        _ = mapL.GetField(x, y).RemoveHexSprite(this);
                    }
                }
            }
        }
    }

    // HXSPRITE.PAS:497
    public void SortMapField(MapContainer map, int pixelX, int pixelY, int hexLevel, int width, int height)
    {
        MapLevel mapL = map.GetMapLevel(hexLevel);
        HexGeometry.PixelToHex(pixelX - 8, pixelY, out int minX, out int minYOdd);
        HexGeometry.PixelToHex(pixelX - 8 + 32, pixelY, out _, out int y);
        int minYEven;
        if (!((minX & 1) != 0))
        {
            minYEven = minYOdd;
            minYOdd = y;
        }
        else
        {
            minYEven = y;
        }

        y = pixelY + height - 1;
        HexGeometry.PixelToHex(width + pixelX + 8 - 1, y, out int maxX, out int maxYOdd);
        HexGeometry.PixelToHex(width + pixelX + 8 - 1 + 32, y, out _, out y);
        int maxYEven;
        if (!((maxX & 1) != 0))
        {
            maxYEven = maxYOdd;
            maxYOdd = y;
        }
        else
        {
            maxYEven = y;
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

            int x = minX;
            int last = maxX;
            for (; x <= last; x += 1)
            {
                if (!((x & 1) != 0))
                {
                    y = minYEven;
                    for (; y <= maxYEven; y += 1)
                    {
                        mapL.GetField(x, y).Sort();
                    }
                }
                else
                {
                    y = minYOdd;
                    int limit = maxYOdd;
                    for (; y <= limit; y += 1)
                    {
                        mapL.GetField(x, y).Sort();
                    }
                }
            }
        }
    }

    public bool Selected => GetSelected();

    // Computed from GetHexX, GetHexY and GetHexLevel. A sprite that stores the position overrides this.
    public virtual MapCoordinate Location
    {
        get => MapCoordinate;
        set => throw new InvalidOperationException("This sprite computes its location from its hex.");
    }

    public int ShowPriority => GetShowPriority();

    // HXSPRITE.PAS:77
    public virtual Set<ControlModeElement> ControlMode() => Set<ControlModeElement>.Of();

}
