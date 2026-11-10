// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

namespace Aow.Game.World.Map;

// the base of the terrain hexagons
public partial class AbstractHexagon : MapObject
{
    public new const int ClassID = 65904;

    public AbstractHexagon(ECustomNode owner) : base(owner)
    { }

    // MAPOBJ.PAS:682
    public override byte GetLevel() => 0;

    // MAPOBJ.PAS:671
    public override int GetShowPriority()
    {
        int Xhp;
        int Yhp;
        Xhp = ((MapField)Owner).FXhx * 32 + 8;
        Yhp = (((MapField)Owner).FXhx & 1) * 16 + ((MapField)Owner).FYhx * 32;
        return Xhp + (Yhp << 13);
    }

    // MAPOBJ.PAS:704
    public override void ChangeTerrain(in HSTerrainInfo Msg)
    {
        HexagonControl Control;
        MapObjectResource R;
        if (Msg.HN == 0 && GetTerrain(0) != Msg.TerrainType)
        {
            Control = ((AbstractHexagonResource)FResource).GetHexagonControl();
            if (Control != null)
            {
                var classID = FResource.EClass.ClassID;
                R = Control.GetRndResource(Msg.TerrainType, classID);
                if (R != null)
                {
                    SetResource(R);
                    UpdateTransitions();
                    var lhx = GetLhx();
                    var yhx = GetYhx();
                    var xhx = GetXhx();
                    FResource.FEngine.FHSMap.FMapContainer.FMap.InvalidateStaticScene(xhx, yhx, lhx);
                }
                else
                {
                    var lhx = GetLhx();
                    var yhx = GetYhx();
                    var xhx = GetXhx();
                    ((AbstractHexagonResource)FResource).GetHexagonControl().Place((MapContainer)Owner.Owner.Owner, xhx, yhx, lhx, Msg.TerrainType);
                }
            }
        }
    }

    // MAPOBJ.PAS:728
    public override void TerrainChanged(in HSTerrainInfo Msg)
    {
        ChangeTerrain(Msg);
    }

    // MAPOBJ.PAS:762
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        if (Msg.MsgID != 16)
        {
            base.MapFieldMsgProc(ref Msg);
        }
    }

    // MAPOBJ.PAS:698
    public override void Activate()
    {
        base.Activate();
        UpdateTransitions();
    }

    // MAPOBJ.PAS:691
    public override void SetResource(MapObjectResource Value)
    {
        base.SetResource(Value);
        if (FStatus.Contains(MapObjectStatusElement.Activated))
        {
            UpdateTransitions();
        }
    }

    // MAPOBJ.PAS:733
    public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        bool Result;
        MapField MapField;
        HexagonSprite HS;
        if (Xhx >= 0 && Yhx >= 0 && Lhx >= 0 && Xhx < MapContainer.FWidth && Yhx < MapContainer.FHeight && Lhx < MapContainer.FLevelCount)
        {
            MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
            HS = MapField.GetMaxLevelNoneTransparentHS();
            bool condition = HS == null;
            if (!condition)
            {
                bool test = HS.GetLevel() < 10;
                if (!test)
                {
                    var overlay = GetOverlay(0);
                    test = MapField.CanChangeTerrain(this, GetTerrain(0), overlay);
                }

                condition = test && !(HS is AbstractBorderHexagon);
            }

            Result = condition;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // MAPOBJ.PAS:750
    public override bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        HexagonSprite HS;
        HS = MapContainer.GetField(Xhx, Yhx, Lhx).FindOwnedHS(typeof(AbstractHexagon));
        if (HS != null)
        {
            HS.Dispose();
        }

        bool Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
        if (Result)
        {
            UpdateTransitions();
        }

        return Result;
    }

    // MAPOBJ.PAS:687
    public virtual void UpdateTransitions()
    {
    }

}
