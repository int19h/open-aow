#nullable disable warnings
// Translated from HSEngine.pas.
using System.Globalization;
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.World;

// a hexagon sprite drawn from a resource. FResource (the resource. until the object is linked
// it holds the resource id, persisted as id 1), FStatus
public partial class MapObject(CustomNode? owner) : HexSprite(owner)
{
    public new const int ClassId = 65_878;

    private MapObjectResource? _resource;

    public override string ToString() => Describe(() =>
    {
        int resourceId = ResourceLinked && _resource is not null ? _resource.ResourceId : _unlinkedResourceId;
        string location = Owner is null ? "no-owner" : string.Create(CultureInfo.InvariantCulture, handler: $"({Owner.HexX},{Owner.HexY} L{Owner.HexLevel})");
        return string.Create(CultureInfo.InvariantCulture, handler: $"{GetType().Name} res={resourceId} status={Status} {location}");
    });

    // MAPOBJ.PAS:88
    public bool ResourceLinked
    {
        get => Status.Contains(MapObjectStatuses.ResourceLinked);
        // MAPOBJ.PAS:93
        set => Status = value
            ? Status | MapObjectStatuses.ResourceLinked
            : Status & ~MapObjectStatuses.ResourceLinked;
    }

    // MAPOBJ.PAS:138
    public override FootprintCell<TerrainType> GetTerrain(int index)
    {
        FootprintCell<TerrainType> result = _resource.TerrainList.GetTerrain(index);
        return Status.Contains(MapObjectStatuses.EditMode) && result.Value is not null ? FootprintCell<TerrainType>.Keep : result;
    }

    // MAPOBJ.PAS:145
    public override void SetTerrain(int index, int value) => _resource.TerrainList.SetTerrain(index, value);

    // MAPOBJ.PAS:150
    public override int TerrainCount => _resource.TerrainList.Count;

    // MAPOBJ.PAS:155
    public override FootprintCell<OverlayType> GetOverlay(int index)
    {
        FootprintCell<OverlayType> result = _resource.TerrainList.GetOverlay(index);
        return Status.Contains(MapObjectStatuses.EditMode) && result.Value is not null ? FootprintCell<OverlayType>.Keep : result;
    }

    // MAPOBJ.PAS:162
    public override void SetOverlay(int index, int value) => _resource.TerrainList.SetOverlay(index, value);

    // MAPOBJ.PAS:167
    public override int OverlayCount => _resource.TerrainList.Count;

    // MAPOBJ.PAS:16
    public override int Level => _resource.Level;

    // MAPOBJ.PAS:29
    public override bool Visible
    {
        get => Status.Contains(MapObjectStatuses.Visible);
        // MAPOBJ.PAS:8
        set
        {
            if (value)
            {
                Status |= MapObjectStatuses.Visible;
            }
            else
            {
                if (Status.Contains(MapObjectStatuses.Visible))
                {
                    Status &= ~MapObjectStatuses.Visible;
                }
            }
        }
    }

    // MAPOBJ.PAS:133
    public override bool GetSelected() => Status.Contains(MapObjectStatuses.Selected);

    // MAPOBJ.PAS:45
    public override sbyte HexX
    {
        get
        {
            Assert(Owner is not null, message: "Owner not set", file: "MAPOBJ.PAS", line: 47);
            return (sbyte)Owner.HexX;
        }
    }

    // MAPOBJ.PAS:51
    public override sbyte HexY
    {
        get
        {
            Assert(Owner is not null, message: "Owner not set", file: "MAPOBJ.PAS", line: 53);
            return (sbyte)Owner.HexY;
        }
    }

    // MAPOBJ.PAS:57
    public override sbyte HexLevel
    {
        get
        {
            Assert(Owner is not null, message: "Owner not set", file: "MAPOBJ.PAS", line: 59);
            return (sbyte)Owner.HexLevel;
        }
    }

    // MAPOBJ.PAS:40
    public override bool EditMode
    {
        get => Status.Contains(MapObjectStatuses.EditMode);
        // MAPOBJ.PAS:35
        // { Value is not used: the edit mode is only ever switched on }
        set => Status |= MapObjectStatuses.EditMode;
    }

    // MAPOBJ.PAS:214
    public override int GetShowPriority()
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        return _resource.ShowPriorityOffset + (pixelY << 13) + pixelX;
    }

    // MAPOBJ.PAS:235
    public override void Connect() => Status |= MapObjectStatuses.PlacedOnMap;

    // MAPOBJ.PAS:240
    public override void Disconnect()
    {
        Owner = null;
        Status &= ~MapObjectStatuses.PlacedOnMap;
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
            _resource.ShowComplexImage(pixelX - pixelXSurface - 8, pixelY - pixelYSurface, lockedSurface, index: 0, offset: (uint)(Owner.HexX + (Owner.HexY * 16)));
        }
    }

    // MAPOBJ.PAS:252
    public override bool PlaceOnMap()
    {
        Connect();
        if (_resource.TerrainList.GetTerrain(0).Value is not null || _resource.TerrainList.GetOverlay(0).Value is not null)
        {
            Owner.UpdateTerrain();
        }

        if (Level <= 15)
        {
            MapField? mapField = Owner;
            mapField.Owner.Map.InvalidateStaticScene(mapField.HexX, mapField.HexY, mapField.HexLevel);
        }

        if (!EditMode)
        {
            MapField? mapField = Owner;
            mapField.Owner.Map.TerrainChangedRadius(mapField.HexX, mapField.HexY, L: mapField.HexLevel, radius: 0);
        }

        return true;
    }

    // MAPOBJ.PAS:266
    public override void RemoveFromMap()
    {
        if (Status.Contains(MapObjectStatuses.PlacedOnMap))
        {
            if (Owner is not null)
            {
                MapField mapField = Owner;
                Disconnect();
                if (!mapField.Owner.Map.Status.Contains(HexSpriteMapStatuses.Destroying))
                {
                    if (GetTerrain(0).Value is not null || GetOverlay(0).Value is not null)
                    {
                        mapField.UpdateTerrain();
                    }

                    if (Level <= 15 && Visible)
                    {
                        mapField.Owner.Map.StaticSceneInvalid = true;
                    }
                }

                if (!EditMode)
                {
                    mapField.Owner.Map.TerrainChangedRadius(mapField.HexX, mapField.HexY, L: mapField.HexLevel, radius: 0);
                }
            }

            Status &= ~MapObjectStatuses.PlacedOnMap;
        }
    }

    // MAPOBJ.PAS:341
    public override bool MainPlace(MapContainer? mapContainer, int pixelX, int pixelY, int hexLevel)
    {
        bool result;
        if (!Status.Contains(MapObjectStatuses.PlacedOnMap))
        {
            HexGeometry.PixelToHex(pixelX, vertical: pixelY, out int hexX, out int hexY);
            if (EditMode || CanPlace(mapContainer, hexX.TruncateToSByte(), hexY.TruncateToSByte(), hexLevel.TruncateToSByte()))
            {
                result = PlaceHex(mapContainer, hexX.TruncateToSByte(), hexY.TruncateToSByte(), hexLevel.TruncateToSByte());
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
        if (Status.Contains(MapObjectStatuses.Selected))
        {
            Unselect();
        }

        if (Status.Contains(MapObjectStatuses.Activated))
        {
            Deactivate();
        }

        if (Status.Contains(MapObjectStatuses.PlacedOnMap))
        {
            Remove();
        }
    }

    // MAPOBJ.PAS:206
    public override bool Place(MapContainer? mapContainer, int pixelX, int pixelY, int hexLevel)
    {
        HexGeometry.PixelToHex(pixelX, vertical: pixelY, out int hexX, out int hexY);
        return PlaceHex(mapContainer, hexX.TruncateToSByte(), hexY.TruncateToSByte(), hexLevel.TruncateToSByte());
    }

    // MAPOBJ.PAS:291
    public override void Remove()
    {
        MapField? mapField = Owner;
        RemoveFromMap();
        if (Visible)
        {
            _ = Level <= 15
                ? mapField.Owner.MapContainer.Map.StaticVisibleHexSprite.RemoveChild(this)
                : mapField.Owner.MapContainer.Map.DynamicVisibleHexSprite.RemoveChild(this);

            Visible = false;
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
            if (GetTerrain(message.HexNumber).Value is not null || message.HexNumber == 0)
            {
                Dispose();
            }
        }
    }

    // MAPOBJ.PAS:330
    public override void Activate() => Status |= MapObjectStatuses.Activated;

    // MAPOBJ.PAS:335
    public override void Deactivate()
    {
        base.Deactivate();
        Status &= ~MapObjectStatuses.Activated;
    }

    // MAPOBJ.PAS:117
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if (!Status.Contains(MapObjectStatuses.Visible))
        {
            Status |= MapObjectStatuses.Visible;
            _ = visibilityList.AddSortChild(this, sortValue: GetShowPriority());
        }
    }

    // MAPOBJ.PAS:126
    public override bool CanSelect(int pixelX, int pixelY) => _resource.Engine.HexSpriteMap.Status.Contains(HexSpriteMapStatuses.Editting) && _resource.TerrainList.Count != 0 && (_resource.TerrainList.GetTerrain(0).Value is not null || _resource.TerrainList.GetOverlay(0).Value is not null);

    // MAPOBJ.PAS:172
    public override void Select() => Status |= MapObjectStatuses.Selected;

    // MAPOBJ.PAS:177
    public override void Unselect()
    {
        base.Unselect();
        Status &= ~MapObjectStatuses.Selected;
    }

    // MAPOBJ.PAS:21
    public override string EditName() => IsEmpty(_resource.Name) ? _resource.ResourceEditName() : _resource.Name;

    // MAPOBJ.PAS:63
    public virtual MapObjectResourceList GetResourceList(GameEngine engine) => engine.GamePack.MapObjectResourceList;

    // MAPOBJ.PAS:68
    public virtual void SetResource(MapObjectResource value) => _resource = value;

    // MAPOBJ.PAS:307
    public virtual bool CanPlace(MapContainer? mapContainer, int hexX, int hexY, int hexLevel)
    {
        if (hexX >= 0 && hexY >= 0 && hexLevel >= 0 && hexX < mapContainer.Width && hexY < mapContainer.Height && hexLevel < mapContainer.LevelCount)
        {
            MapField mapField = mapContainer.GetField(hexX, hexY, hexLevel);
            HexSprite? hexSprite = mapField.GetMaxLevelNoneTransparentHexSprite();
            bool condition = hexSprite is null || hexSprite.Level < 10;
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
    public virtual bool PlaceHex(MapContainer? mapContainer, int hexX, int hexY, int hexLevel)
    {
        Owner = mapContainer.GetField(hexX, hexY, hexLevel);
        return PlaceOnMap();
    }

    public virtual MapObjectResource? Resource => _resource;

    protected T GetResource<T>() where T : MapObjectResource => (T)_resource;

    internal void SetResourceWithoutOverrides(MapObjectResource value) => _resource = value;

    public MapObjectStatuses Status { get; protected set; }
}
