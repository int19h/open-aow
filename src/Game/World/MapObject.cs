// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World;

// a hexagon sprite drawn from a resource. FResource (the resource. until the object is linked
// it holds the resource id, persisted as id 1), FStatus
public partial class MapObject : HexSprite
{
    public MapObject(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 65878;

    protected internal MapObjectResource _resource;
    private Set<MapObjectStatusElement> _status;

    // MAPOBJ.PAS:88
    protected internal bool GetResourceLinked() => _status.Contains(MapObjectStatusElement.ResourceLinked);

    // MAPOBJ.PAS:93
    protected internal void SetResourceLinked(bool value)
    {
        if (value)
        {
            _status = _status + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.ResourceLinked);
        }
        else
        {
            _status = _status - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.ResourceLinked);
        }
    }

    // MAPOBJ.PAS:138
    public override sbyte GetTerrain(int index)
    {
        sbyte result = _resource.TerrainList.GetTerrain(index);
        if (_status.Contains(MapObjectStatusElement.EditMode) && result >= 0)
        {
            result = -1;
        }

        return result;
    }

    // MAPOBJ.PAS:145
    public override void SetTerrain(int index, sbyte value)
    {
        _resource.TerrainList.SetTerrain(index, value);
    }

    // MAPOBJ.PAS:150
    public override int GetTerrainCount() => _resource.TerrainList._count;

    // MAPOBJ.PAS:155
    public override sbyte GetOverlay(int index)
    {
        sbyte result = _resource.TerrainList.GetOverlay(index);
        if (_status.Contains(MapObjectStatusElement.EditMode) && result >= 0)
        {
            result = -1;
        }

        return result;
    }

    // MAPOBJ.PAS:162
    public override void SetOverlay(int index, sbyte value)
    {
        _resource.TerrainList.SetOverlay(index, value);
    }

    // MAPOBJ.PAS:167
    public override int GetOverlayCount() => _resource.TerrainList._count;

    // MAPOBJ.PAS:16
    public override byte GetLevel() => _resource._level;

    // MAPOBJ.PAS:8
    public override void SetVisible(bool value)
    {
        if (value)
        {
            _status = _status + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Visible);
        }
        else
        {
            if (_status.Contains(MapObjectStatusElement.Visible))
            {
                _status = _status - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Visible);
            }
        }
    }

    // MAPOBJ.PAS:29
    public override bool GetVisible() => _status.Contains(MapObjectStatusElement.Visible);

    // MAPOBJ.PAS:133
    public override bool GetSelected() => _status.Contains(MapObjectStatusElement.Selected);

    // MAPOBJ.PAS:45
    public override sbyte GetHexX()
    {
        Assert(Owner != null, "Owner not set", "MAPOBJ.PAS", 47);
        return Owner.HexX;
    }

    // MAPOBJ.PAS:51
    public override sbyte GetHexY()
    {
        Assert(Owner != null, "Owner not set", "MAPOBJ.PAS", 53);
        return Owner.HexY;
    }

    // MAPOBJ.PAS:57
    public override sbyte GetHexLevel()
    {
        Assert(Owner != null, "Owner not set", "MAPOBJ.PAS", 59);
        return Owner.HexLevel;
    }

    // MAPOBJ.PAS:40
    public override bool GetEditMode() => _status.Contains(MapObjectStatusElement.EditMode);

    // MAPOBJ.PAS:35
    // { Value is not used: the edit mode is only ever switched on }
    public override void SetEditMode(bool value)
    {
        _status = _status + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.EditMode);
    }

    // MAPOBJ.PAS:214
    public override int GetShowPriority()
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out pixelX, out pixelY);
        return _resource._showPriorityOffset + (pixelY << 13) + pixelX;
    }

    // MAPOBJ.PAS:235
    public override void Connect()
    {
        _status = _status + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
    }

    // MAPOBJ.PAS:240
    public override void Disconnect()
    {
        Owner = null;
        _status = _status - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
    }

    // MAPOBJ.PAS:222
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out pixelX, out pixelY);
        if (_resource.ImageSequenceList == null)
        {
            _resource.ImageLibrary.Get(0).Show(pixelX - pixelXSurface - 8, pixelY - pixelYSurface, lockedSurface);
        }
        else
        {
            _resource.ShowComplexImage(pixelX - pixelXSurface - 8, pixelY - pixelYSurface, lockedSurface, 0, (uint)(Owner.HexX + Owner.HexY * 16));
        }
    }

    // MAPOBJ.PAS:252
    public override bool PlaceOnMap()
    {
        Connect();
        if (_resource.TerrainList.GetTerrain(0) >= 0 || _resource.TerrainList.GetOverlay(0) >= 0)
        {
            Owner.UpdateTerrain();
        }

        if (GetLevel() <= 15)
        {
            var mapField = Owner;
            mapField.Owner.Map.InvalidateStaticScene(mapField.HexX, mapField.HexY, mapField.HexLevel);
        }

        if (!GetEditMode())
        {
            var mapField = Owner;
            mapField.Owner.Map.TerrainChangedRadius(mapField.HexX, mapField.HexY, mapField.HexLevel, 0);
        }

        return true;
    }

    // MAPOBJ.PAS:266
    public override void RemoveFromMap()
    {
        MapField mapField;
        if (_status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            if (Owner != null)
            {
                mapField = Owner;
                Disconnect();
                if (!mapField.Owner.Map.Status.Contains(HexSpriteMapStatusElement.Destroying))
                {
                    if (GetTerrain(0) >= 0 || GetOverlay(0) >= 0)
                    {
                        mapField.UpdateTerrain();
                    }

                    if (GetLevel() <= 15 && GetVisible())
                    {
                        mapField.Owner.Map.StaticSceneInvalid = true;
                    }
                }

                if (!GetEditMode())
                {
                    mapField.Owner.Map.TerrainChangedRadius(mapField.HexX, mapField.HexY, mapField.HexLevel, 0);
                }
            }

            _status = _status - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
        }
    }

    // MAPOBJ.PAS:341
    public override bool MainPlace(MapContainer mapContainer, int pixelX, int pixelY, int hexLevel)
    {
        bool result;
        int hexX = default;
        int hexY = default;
        if (!_status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            HexGeometry.PixelToHex(pixelX, pixelY, out hexX, out hexY);
            if (GetEditMode() || CanPlace(mapContainer, (sbyte)hexX, (sbyte)hexY, (sbyte)hexLevel))
            {
                result = PlaceHex(mapContainer, (sbyte)hexX, (sbyte)hexY, (sbyte)hexLevel);
                if (result)
                {
                    Activate();
                }
                else
                {
                    MainRemove();
                }
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // MAPOBJ.PAS:363
    public override void MainRemove()
    {
        if (_status.Contains(MapObjectStatusElement.Selected))
        {
            Unselect();
        }

        if (_status.Contains(MapObjectStatusElement.Activated))
        {
            Deactivate();
        }

        if (_status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            Remove();
        }
    }

    // MAPOBJ.PAS:206
    public override bool Place(MapContainer mapContainer, int pixelX, int pixelY, int hexLevel)
    {
        int hexX = default;
        int hexY = default;
        HexGeometry.PixelToHex(pixelX, pixelY, out hexX, out hexY);
        return PlaceHex(mapContainer, (sbyte)hexX, (sbyte)hexY, (sbyte)hexLevel);
    }

    // MAPOBJ.PAS:291
    public override void Remove()
    {
        MapField mapField;
        mapField = Owner;
        RemoveFromMap();
        if (GetVisible())
        {
            if (GetLevel() <= 15)
            {
                mapField.Owner.MapContainer.Map.StaticVisibleHexSprite.RemoveChild(this);
            }
            else
            {
                mapField.Owner.MapContainer.Map.DynamicVisibleHexSprite.RemoveChild(this);
            }

            SetVisible(false);
        }
    }

    // MAPOBJ.PAS:246
    public override bool Loaded()
    {
        Connect();
        return true;
    }

    // MAPOBJ.PAS:373
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 16)
        {
            if (GetTerrain(message.HexNumber) >= 0 || message.HexNumber == 0)
            {
                Dispose();
            }
        }
    }

    // MAPOBJ.PAS:330
    public override void Activate()
    {
        _status = _status + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Activated);
    }

    // MAPOBJ.PAS:335
    public override void Deactivate()
    {
        base.Deactivate();
        _status = _status - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Activated);
    }

    // MAPOBJ.PAS:117
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if (!_status.Contains(MapObjectStatusElement.Visible))
        {
            _status = _status + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Visible);
            visibilityList.AddSortChild(this, GetShowPriority());
        }
    }

    // MAPOBJ.PAS:126
    public override bool CanSelect(int pixelX, int pixelY) => _resource.Engine.HexSpriteMap.Status.Contains(HexSpriteMapStatusElement.Editting) && _resource.TerrainList._count != 0 && (_resource.TerrainList.GetTerrain(0) >= 0 || _resource.TerrainList.GetOverlay(0) >= 0);

    // MAPOBJ.PAS:172
    public override void Select()
    {
        _status = _status + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Selected);
    }

    // MAPOBJ.PAS:177
    public override void Unselect()
    {
        base.Unselect();
        _status = _status - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Selected);
    }

    // MAPOBJ.PAS:21
    public override string EditName()
    {
        string result;
        if (IsEmpty(_resource._name))
        {
            result = _resource.ResourceEditName();
        }
        else
        {
            result = _resource._name;
        }

        return result;
    }

    // MAPOBJ.PAS:63
    public virtual MapObjectResourceList GetResourceList(GameEngine engine) => engine.HexSpriteSet.MapObjectResourceList;

    // MAPOBJ.PAS:68
    public virtual void SetResource(MapObjectResource value)
    {
        _resource = value;
    }

    // MAPOBJ.PAS:307
    public virtual bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result;
        HexSprite hexSprite;
        MapField mapField;
        if (hexX >= 0 && hexY >= 0 && hexLevel >= 0 && hexX < mapContainer.Width && hexY < mapContainer.Height && hexLevel < mapContainer.LevelCount)
        {
            mapField = mapContainer.GetField(hexX, hexY, hexLevel);
            hexSprite = mapField.GetMaxLevelNoneTransparentHexSprite();
            bool condition = hexSprite == null || hexSprite.GetLevel() < 10;
            if (condition)
            {
                var overlay = GetOverlay(0);
                condition = mapField.CanChangeTerrain(this, GetTerrain(0), overlay);
            }

            result = condition;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // MAPOBJ.PAS:324
    public virtual bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        Owner = mapContainer.GetField(hexX, hexY, hexLevel);
        return PlaceOnMap();
    }

    public MapObjectResource Resource { get => _resource; set => SetResource(value); }
    public int ResourceId { get => GetResourceId(); set => SetResourceId(value); }
    public bool ResourceLinked { get => GetResourceLinked(); set => SetResourceLinked(value); }
    public Set<MapObjectStatusElement> Status { get => _status; protected set => _status = value; }

}
