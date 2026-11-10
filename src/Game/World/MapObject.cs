// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World;

// a hexagon sprite drawn from a resource. FResource (the resource. until the object is linked
// it holds the resource id, persisted as id 1), FStatus
public partial class MapObject : HexagonSprite
{
    public MapObject(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 65878;

    protected internal MapObjectResource FResource;
    protected internal Set<MapObjectStatusElement> FStatus;

    // MAPOBJ.PAS:88
    protected internal bool GetResourceLinked() => FStatus.Contains(MapObjectStatusElement.ResourceLinked);

    // MAPOBJ.PAS:93
    protected internal void SetResourceLinked(bool Value)
    {
        if (Value)
        {
            FStatus = FStatus + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.ResourceLinked);
        }
        else
        {
            FStatus = FStatus - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.ResourceLinked);
        }
    }

    // MAPOBJ.PAS:138
    public override sbyte GetTerrain(int Index)
    {
        sbyte Result = FResource.FTerrainList.GetTerrain(Index);
        if (FStatus.Contains(MapObjectStatusElement.EditMode) && Result >= 0)
        {
            Result = -1;
        }

        return Result;
    }

    // MAPOBJ.PAS:145
    public override void SetTerrain(int Index, sbyte Value)
    {
        FResource.FTerrainList.SetTerrain(Index, Value);
    }

    // MAPOBJ.PAS:150
    public override int GetTerrainCount() => FResource.FTerrainList.FCount;

    // MAPOBJ.PAS:155
    public override sbyte GetOverlay(int Index)
    {
        sbyte Result = FResource.FTerrainList.GetOverlay(Index);
        if (FStatus.Contains(MapObjectStatusElement.EditMode) && Result >= 0)
        {
            Result = -1;
        }

        return Result;
    }

    // MAPOBJ.PAS:162
    public override void SetOverlay(int Index, sbyte Value)
    {
        FResource.FTerrainList.SetOverlay(Index, Value);
    }

    // MAPOBJ.PAS:167
    public override int GetOverlayCount() => FResource.FTerrainList.FCount;

    // MAPOBJ.PAS:16
    public override byte GetLevel() => FResource.FLevel;

    // MAPOBJ.PAS:8
    public override void SetVisible(bool Value)
    {
        if (Value)
        {
            FStatus = FStatus + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Visible);
        }
        else
        {
            if (FStatus.Contains(MapObjectStatusElement.Visible))
            {
                FStatus = FStatus - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Visible);
            }
        }
    }

    // MAPOBJ.PAS:29
    public override bool GetVisible() => FStatus.Contains(MapObjectStatusElement.Visible);

    // MAPOBJ.PAS:133
    public override bool GetSelected() => FStatus.Contains(MapObjectStatusElement.Selected);

    // MAPOBJ.PAS:45
    public override sbyte GetXhx()
    {
        Assert(Owner != null, "Owner not set", "MAPOBJ.PAS", 47);
        return ((MapField)Owner).FXhx;
    }

    // MAPOBJ.PAS:51
    public override sbyte GetYhx()
    {
        Assert(Owner != null, "Owner not set", "MAPOBJ.PAS", 53);
        return ((MapField)Owner).FYhx;
    }

    // MAPOBJ.PAS:57
    public override sbyte GetLhx()
    {
        Assert(Owner != null, "Owner not set", "MAPOBJ.PAS", 59);
        return ((MapField)Owner).FLhx;
    }

    // MAPOBJ.PAS:40
    public override bool GetEditMode() => FStatus.Contains(MapObjectStatusElement.EditMode);

    // MAPOBJ.PAS:35
    // { Value is not used: the edit mode is only ever switched on }
    public override void SetEditMode(bool Value)
    {
        FStatus = FStatus + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.EditMode);
    }

    // MAPOBJ.PAS:214
    public override int GetShowPriority()
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(((MapField)Owner).FXhx, ((MapField)Owner).FYhx, ref Xhp, ref Yhp);
        return FResource.FShowPriorityOffset + (Yhp << 13) + Xhp;
    }

    // MAPOBJ.PAS:235
    public override void Connect()
    {
        FStatus = FStatus + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
    }

    // MAPOBJ.PAS:240
    public override void Disconnect()
    {
        Owner = null;
        FStatus = FStatus - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
    }

    // MAPOBJ.PAS:222
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(((MapField)Owner).FXhx, ((MapField)Owner).FYhx, ref Xhp, ref Yhp);
        if (FResource.FImageSequenceList == null)
        {
            FResource.FImageLibrary.Get(0).Show(Xhp - XhpSurf - 8, Yhp - YhpSurf, SurfaceDesc);
        }
        else
        {
            FResource.ShowComplexImage(Xhp - XhpSurf - 8, Yhp - YhpSurf, SurfaceDesc, 0, (uint)(((MapField)Owner).FXhx + ((MapField)Owner).FYhx * 16));
        }
    }

    // MAPOBJ.PAS:252
    public override bool PlaceOnMap()
    {
        Connect();
        if (FResource.FTerrainList.GetTerrain(0) >= 0 || FResource.FTerrainList.GetOverlay(0) >= 0)
        {
            ((MapField)Owner).UpdateTerrain();
        }

        if (GetLevel() <= 15)
        {
            var mapField = (MapField)Owner;
            ((MapLevel)mapField.Owner).FMap.InvalidateStaticScene(mapField.FXhx, mapField.FYhx, mapField.FLhx);
        }

        if (!GetEditMode())
        {
            var mapField = (MapField)Owner;
            ((MapLevel)mapField.Owner).FMap.TerrainChangedRad(mapField.FXhx, mapField.FYhx, mapField.FLhx, 0);
        }

        return true;
    }

    // MAPOBJ.PAS:266
    public override void RemoveFromMap()
    {
        MapField MapField;
        if (FStatus.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            if (Owner != null)
            {
                MapField = (MapField)Owner;
                Disconnect();
                if (!((MapLevel)MapField.Owner).FMap.FStatus.Contains(HSMapStatusElement.Destroying))
                {
                    if (GetTerrain(0) >= 0 || GetOverlay(0) >= 0)
                    {
                        MapField.UpdateTerrain();
                    }

                    if (GetLevel() <= 15 && GetVisible())
                    {
                        ((MapLevel)MapField.Owner).FMap.FStaticSceneInvalid = true;
                    }
                }

                if (!GetEditMode())
                {
                    ((MapLevel)MapField.Owner).FMap.TerrainChangedRad(MapField.FXhx, MapField.FYhx, MapField.FLhx, 0);
                }
            }

            FStatus = FStatus - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
        }
    }

    // MAPOBJ.PAS:341
    public override bool MainPlace(MapContainer MapContainer, int Xhp, int Yhp, int Lhx)
    {
        bool Result;
        int Xhx = default;
        int Yhx = default;
        if (!FStatus.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            HexGeometry.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
            if (GetEditMode() || CanPlace(MapContainer, (sbyte)Xhx, (sbyte)Yhx, (sbyte)Lhx))
            {
                Result = PlaceHX(MapContainer, (sbyte)Xhx, (sbyte)Yhx, (sbyte)Lhx);
                if (Result)
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
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // MAPOBJ.PAS:363
    public override void MainRemove()
    {
        if (FStatus.Contains(MapObjectStatusElement.Selected))
        {
            Unselect();
        }

        if (FStatus.Contains(MapObjectStatusElement.Activated))
        {
            Deactivate();
        }

        if (FStatus.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            Remove();
        }
    }

    // MAPOBJ.PAS:206
    public override bool Place(MapContainer MapContainer, int Xhp, int Yhp, int Lhx)
    {
        int Xhx = default;
        int Yhx = default;
        HexGeometry.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
        return PlaceHX(MapContainer, (sbyte)Xhx, (sbyte)Yhx, (sbyte)Lhx);
    }

    // MAPOBJ.PAS:291
    public override void Remove()
    {
        MapField MapField;
        MapField = (MapField)Owner;
        RemoveFromMap();
        if (GetVisible())
        {
            if (GetLevel() <= 15)
            {
                ((MapLevel)MapField.Owner).FMapContainer.FMap.FStaticVisibleHS.RemoveChild(this);
            }
            else
            {
                ((MapLevel)MapField.Owner).FMapContainer.FMap.FDynamicVisibleHS.RemoveChild(this);
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
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 16)
        {
            if (GetTerrain(Msg.HN) >= 0 || Msg.HN == 0)
            {
                Dispose();
            }
        }
    }

    // MAPOBJ.PAS:330
    public override void Activate()
    {
        FStatus = FStatus + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Activated);
    }

    // MAPOBJ.PAS:335
    public override void Deactivate()
    {
        base.Deactivate();
        FStatus = FStatus - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Activated);
    }

    // MAPOBJ.PAS:117
    public override void MakeVisible(VisibilityList VisibilityList)
    {
        if (!FStatus.Contains(MapObjectStatusElement.Visible))
        {
            FStatus = FStatus + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Visible);
            VisibilityList.AddSortChild(this, GetShowPriority());
        }
    }

    // MAPOBJ.PAS:126
    public override bool CanSelect(int Xhp, int Yhp) => FResource.FEngine.FHSMap.FStatus.Contains(HSMapStatusElement.Editting) && FResource.FTerrainList.FCount != 0 && (FResource.FTerrainList.GetTerrain(0) >= 0 || FResource.FTerrainList.GetOverlay(0) >= 0);

    // MAPOBJ.PAS:172
    public override void Select()
    {
        FStatus = FStatus + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Selected);
    }

    // MAPOBJ.PAS:177
    public override void Unselect()
    {
        base.Unselect();
        FStatus = FStatus - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Selected);
    }

    // MAPOBJ.PAS:21
    public override string EditName()
    {
        string Result;
        if (IsEmpty(FResource.FName))
        {
            Result = FResource.ResourceEditName();
        }
        else
        {
            Result = FResource.FName;
        }

        return Result;
    }

    // MAPOBJ.PAS:63
    public virtual MapObjectResourceList GetResourceList(GameEngine AEngine) => AEngine.FHSSet.FMapObjectResourceList;

    // MAPOBJ.PAS:68
    public virtual void SetResource(MapObjectResource Value)
    {
        FResource = Value;
    }

    // MAPOBJ.PAS:307
    public virtual bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        bool Result;
        HexagonSprite HS;
        MapField MapField;
        if (Xhx >= 0 && Yhx >= 0 && Lhx >= 0 && Xhx < MapContainer.FWidth && Yhx < MapContainer.FHeight && Lhx < MapContainer.FLevelCount)
        {
            MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
            HS = MapField.GetMaxLevelNoneTransparentHS();
            bool condition = HS == null || HS.GetLevel() < 10;
            if (condition)
            {
                var overlay = GetOverlay(0);
                condition = MapField.CanChangeTerrain(this, GetTerrain(0), overlay);
            }

            Result = condition;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // MAPOBJ.PAS:324
    public virtual bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        Owner = MapContainer.GetField(Xhx, Yhx, Lhx);
        return PlaceOnMap();
    }

    public MapObjectResource Resource { get => FResource; set => SetResource(value); }
    public int ResourceID { get => GetResourceID(); set => SetResourceID(value); }
    public bool ResourceLinked { get => GetResourceLinked(); set => SetResourceLinked(value); }
    public Set<MapObjectStatusElement> Status { get => FStatus; }

}
