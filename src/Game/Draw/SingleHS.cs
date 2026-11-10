// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Draw;

// a sprite on one hexagon. FStatus: bit 1 visible, 2 edit mode, 4 selected, 8 activated (a
// Byte: the original sets the bits with or/and, which a set with Include does not give)
public partial class SingleHS : HexagonSprite
{
    public SingleHS(ECustomNode owner) : base(owner)
    { }

    internal byte FStatus;

    // HXSPRITE.PAS:640
    public override void SetVisible(bool Value)
    {
        if (Value)
        {
            FStatus = (byte)(FStatus | 1);
        }
        else
        {
            FStatus = (byte)(FStatus & 254);
        }
    }

    // HXSPRITE.PAS:646
    public override bool GetVisible() => (FStatus & 1) != 0;

    // HXSPRITE.PAS:683
    public override bool GetSelected() => (FStatus & 4) != 0;

    // HXSPRITE.PAS:660
    public override bool GetEditMode() => (FStatus & 2) != 0;

    // HXSPRITE.PAS:665
    public override void SetEditMode(bool Value)
    {
        if (Value)
        {
            FStatus = (byte)(FStatus | 2);
        }
        else
        {
            FStatus = (byte)(FStatus & 253);
        }
    }

    // HXSPRITE.PAS:713
    public override int GetShowPriority()
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(((MapField)Owner).FXhx, ((MapField)Owner).FYhx, ref Xhp, ref Yhp);
        return (GetLevel() << 26) + ((Yhp + 32) << 13) + (Xhp + 16);
    }

    // HXSPRITE.PAS:704
    public override void Connect()
    {
    }

    // HXSPRITE.PAS:708
    public override void Disconnect()
    {
        SetOwner(null);
    }

    // HXSPRITE.PAS:749
    public override bool Place(MapContainer Map, int Xhp, int Yhp, int Lhx)
    {
        int Xhx = default;
        int Yhx = default;
        HexGeometry.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
        return PlaceHX(Map, (sbyte)Xhx, (sbyte)Yhx, (sbyte)Lhx);
    }

    // HXSPRITE.PAS:721
    public override void Remove()
    {
        MapField MapF;
        if (Owner != null)
        {
            MapF = (MapField)Owner;
            RemoveFromMap();
            if (GetVisible())
            {
                if (GetLevel() <= 15)
                {
                    ((MapLevel)MapF.Owner).FMapContainer.FMap.FStaticVisibleHS.RemoveChild(this);
                    ((MapLevel)MapF.Owner).FMapContainer.FMap.FStaticSceneInvalid = true;
                }
                else
                {
                    ((MapLevel)MapF.Owner).FMapContainer.FMap.FDynamicVisibleHS.RemoveChild(this);
                }
            }

            SetVisible(false);
        }
    }

    // HXSPRITE.PAS:688
    public override void Activate()
    {
        FStatus = (byte)(FStatus | 8);
    }

    // HXSPRITE.PAS:693
    public override void Deactivate()
    {
        base.Deactivate();
        FStatus = (byte)(FStatus & 247);
    }

    // HXSPRITE.PAS:651
    public override void MakeVisible(VisibilityList VisibilityList)
    {
        if ((FStatus & 1) == 0)
        {
            FStatus = (byte)(FStatus | 1);
            VisibilityList.AddSortChild(this, GetShowPriority());
        }
    }

    // HXSPRITE.PAS:671
    public override void Select()
    {
        base.Select();
        FStatus = (byte)(FStatus | 4);
    }

    // HXSPRITE.PAS:677
    public override void Unselect()
    {
        base.Unselect();
        FStatus = (byte)(FStatus & 251);
    }

    // HXSPRITE.PAS:743
    public virtual bool CanPlace(MapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx) => Xhx >= 0 && Yhx >= 0 && Lhx >= 0 && Xhx < Map.FWidth && Yhx < Map.FHeight && Lhx < Map.FLevelCount;

    // HXSPRITE.PAS:757
    public virtual bool PlaceHX(MapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        bool Result;
        if (GetEditMode() || CanPlace(Map, Xhx, Yhx, Lhx))
        {
            Remove();
            SetOwner(Map.GetField(Xhx, Yhx, Lhx));
            Result = PlaceOnMap();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // HXSPRITE.PAS:699
    public bool Activated() => (FStatus & 8) != 0;
}
