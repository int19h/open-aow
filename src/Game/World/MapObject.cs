#nullable disable
// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.World;

// a hexagon sprite drawn from a resource. FResource (the resource. until the object is linked
// it holds the resource id, persisted as id 1), FStatus
public partial class MapObject(CustomNode owner) : HexSprite(owner)
{
    public new const int ClassId = 65878;

    private MapObjectResource _resource;

    // MAPOBJ.PAS:88
    protected internal bool GetResourceLinked() => Status.Contains(MapObjectStatusElement.ResourceLinked);

    // MAPOBJ.PAS:93
    protected internal void SetResourceLinked(bool value)
    {
        Status = value
            ? Status + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.ResourceLinked)
            : Status - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.ResourceLinked);
    }

    // MAPOBJ.PAS:138
    public override sbyte GetTerrain(int index)
    {
        sbyte result = _resource.TerrainList.GetTerrain(index);
        return Status.Contains(MapObjectStatusElement.EditMode) && result >= 0 ? (sbyte)-1 : result;
    }

    // MAPOBJ.PAS:145
    public override void SetTerrain(int index, sbyte value) => _resource.TerrainList.SetTerrain(index, value);

    // MAPOBJ.PAS:150
    public override int GetTerrainCount() => _resource.TerrainList.Count;

    // MAPOBJ.PAS:155
    public override sbyte GetOverlay(int index)
    {
        sbyte result = _resource.TerrainList.GetOverlay(index);
        return Status.Contains(MapObjectStatusElement.EditMode) && result >= 0 ? (sbyte)-1 : result;
    }

    // MAPOBJ.PAS:162
    public override void SetOverlay(int index, sbyte value) => _resource.TerrainList.SetOverlay(index, value);

    // MAPOBJ.PAS:167
    public override int GetOverlayCount() => _resource.TerrainList.Count;

    // MAPOBJ.PAS:16
    public override byte GetLevel() => _resource.Level;

    // MAPOBJ.PAS:8
    public override void SetVisible(bool value)
    {
        if (value)
        {
            Status += Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Visible);
        }
        else
        {
            if (Status.Contains(MapObjectStatusElement.Visible))
            {
                Status -= Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Visible);
            }
        }
    }

    // MAPOBJ.PAS:29
    public override bool GetVisible() => Status.Contains(MapObjectStatusElement.Visible);

    // MAPOBJ.PAS:133
    public override bool GetSelected() => Status.Contains(MapObjectStatusElement.Selected);

    // MAPOBJ.PAS:45
    public override sbyte GetHexX()
    {
        Assert(Owner is not null, "Owner not set", "MAPOBJ.PAS", 47);
        return Owner.HexX;
    }

    // MAPOBJ.PAS:51
    public override sbyte GetHexY()
    {
        Assert(Owner is not null, "Owner not set", "MAPOBJ.PAS", 53);
        return Owner.HexY;
    }

    // MAPOBJ.PAS:57
    public override sbyte GetHexLevel()
    {
        Assert(Owner is not null, "Owner not set", "MAPOBJ.PAS", 59);
        return Owner.HexLevel;
    }

    // MAPOBJ.PAS:40
    public override bool GetEditMode() => Status.Contains(MapObjectStatusElement.EditMode);

    // MAPOBJ.PAS:35
    // { Value is not used: the edit mode is only ever switched on }
    public override void SetEditMode(bool value) => Status += Set<MapObjectStatusElement>.Of(MapObjectStatusElement.EditMode);

    // MAPOBJ.PAS:214
    public override int GetShowPriority()
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        return _resource.ShowPriorityOffset + (pixelY << 13) + pixelX;
    }

    // MAPOBJ.PAS:235
    public override void Connect() => Status += Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);

    // MAPOBJ.PAS:240
    public override void Disconnect()
    {
        Owner = null;
        Status -= Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
    }

    // MAPOBJ.PAS:222
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        if (_resource.ImageSequenceList is null)
        {
            _resource.ImageLibrary.Get(0).Show(pixelX - pixelXSurface - 8, pixelY - pixelYSurface, lockedSurface);
        }
        else
        {
            _resource.ShowComplexImage(pixelX - pixelXSurface - 8, pixelY - pixelYSurface, lockedSurface, 0, (uint)(Owner.HexX + (Owner.HexY * 16)));
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
            MapField mapField = Owner;
            mapField.Owner.Map.InvalidateStaticScene(mapField.HexX, mapField.HexY, mapField.HexLevel);
        }

        if (!GetEditMode())
        {
            MapField mapField = Owner;
            mapField.Owner.Map.TerrainChangedRadius(mapField.HexX, mapField.HexY, mapField.HexLevel, 0);
        }

        return true;
    }

    // MAPOBJ.PAS:266
    public override void RemoveFromMap()
    {
        if (Status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            if (Owner is not null)
            {
                MapField mapField = Owner;
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

            Status -= Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
        }
    }

    // MAPOBJ.PAS:341
    public override bool MainPlace(MapContainer mapContainer, int pixelX, int pixelY, int hexLevel)
    {
        bool result;
        if (!Status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            HexGeometry.PixelToHex(pixelX, pixelY, out int hexX, out int hexY);
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
                return false;
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // MAPOBJ.PAS:363
    public override void MainRemove()
    {
        if (Status.Contains(MapObjectStatusElement.Selected))
        {
            Unselect();
        }

        if (Status.Contains(MapObjectStatusElement.Activated))
        {
            Deactivate();
        }

        if (Status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            Remove();
        }
    }

    // MAPOBJ.PAS:206
    public override bool Place(MapContainer mapContainer, int pixelX, int pixelY, int hexLevel)
    {
        HexGeometry.PixelToHex(pixelX, pixelY, out int hexX, out int hexY);
        return PlaceHex(mapContainer, (sbyte)hexX, (sbyte)hexY, (sbyte)hexLevel);
    }

    // MAPOBJ.PAS:291
    public override void Remove()
    {
        MapField mapField = Owner;
        RemoveFromMap();
        if (GetVisible())
        {
            _ = GetLevel() <= 15
                ? mapField.Owner.MapContainer.Map.StaticVisibleHexSprite.RemoveChild(this)
                : mapField.Owner.MapContainer.Map.DynamicVisibleHexSprite.RemoveChild(this);

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
    public override void Activate() => Status += Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Activated);

    // MAPOBJ.PAS:335
    public override void Deactivate()
    {
        base.Deactivate();
        Status -= Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Activated);
    }

    // MAPOBJ.PAS:117
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if (!Status.Contains(MapObjectStatusElement.Visible))
        {
            Status += Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Visible);
            _ = visibilityList.AddSortChild(this, GetShowPriority());
        }
    }

    // MAPOBJ.PAS:126
    public override bool CanSelect(int pixelX, int pixelY) => _resource.Engine.HexSpriteMap.Status.Contains(HexSpriteMapStatusElement.Editting) && _resource.TerrainList.Count != 0 && (_resource.TerrainList.GetTerrain(0) >= 0 || _resource.TerrainList.GetOverlay(0) >= 0);

    // MAPOBJ.PAS:172
    public override void Select() => Status += Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Selected);

    // MAPOBJ.PAS:177
    public override void Unselect()
    {
        base.Unselect();
        Status -= Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Selected);
    }

    // MAPOBJ.PAS:21
    public override string EditName() => IsEmpty(_resource.Name) ? _resource.ResourceEditName() : _resource.Name;

    // MAPOBJ.PAS:63
    public virtual MapObjectResourceList GetResourceList(GameEngine engine) => engine.HexSpriteSet.MapObjectResourceList;

    // MAPOBJ.PAS:68
    public virtual void SetResource(MapObjectResource value) => _resource = value;

    // MAPOBJ.PAS:307
    public virtual bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        if (hexX >= 0 && hexY >= 0 && hexLevel >= 0 && hexX < mapContainer.Width && hexY < mapContainer.Height && hexLevel < mapContainer.LevelCount)
        {
            MapField mapField = mapContainer.GetField(hexX, hexY, hexLevel);
            HexSprite hexSprite = mapField.GetMaxLevelNoneTransparentHexSprite();
            bool condition = hexSprite is null || hexSprite.GetLevel() < 10;
            if (condition)
            {
                condition = mapField.CanChangeTerrain(overlay: GetOverlay(0), sender: this, terrain: GetTerrain(0));
            }

            return condition;
        }
        else
        {
            return false;
        }
    }

    // MAPOBJ.PAS:324
    public virtual bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        Owner = mapContainer.GetField(hexX, hexY, hexLevel);
        return PlaceOnMap();
    }

    public virtual MapObjectResource Resource => _resource;

    protected T GetResource<T>() where T : MapObjectResource => (T)_resource;

    internal void SetResourceWithoutOverrides(MapObjectResource value) => _resource = value;

    public int ResourceId { get => GetResourceId(); set => SetResourceId(value); }
    public bool ResourceLinked { get => GetResourceLinked(); set => SetResourceLinked(value); }
    public Set<MapObjectStatusElement> Status { get; protected set; }
}
