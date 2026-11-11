// Translated from Road.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

// { The Road unit of HSEPack.dpl: a road on a hexagon (TRoad, ClassID $20164) whose image is chosen
//   from the roads on the six neighbour hexagons (TRoadResource, ClassID $20165). }
// FTransitions (bit HN-1: a road on the neighbour HN. the pairs 2+6 and 3+5 become bits 7 and
// 6)
public partial class Road : AbstractRoad
{
    public Road(ECustomNode owner) : base(owner)
    {
    }

    // Road.pas:47
    public new const int ClassID = 131428;

    protected internal byte FTransitions;

    // Road.pas:42

    // Road.pas:52
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
        ((Resource)FResource).Show(Xhp - XhpSurf - 8 - 6, Yhp - YhpSurf - 9, SurfaceDesc, this);
    }

    // Road.pas:60
    public override bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        MapField MapField;
        HexSprite OldRoad;
        MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
        OldRoad = MapField.FindOwnedHS(typeof(AbstractRoad));
        if (OldRoad != null)
        {
            OldRoad.Dispose();
        }

        Owner = MapField;
        bool Result = PlaceOnMap();
        UpdateTransitions();
        UpdateNeighbourTransitions();
        return Result;
    }

    // Road.pas:74
    public override void UpdateTransitions()
    {
        int Xhx = default;
        int Yhx = default;
        MapLevel Level;
        MapField MapField;
        bool HasRoad;
        Level = (MapLevel)Owner.Owner;
        FTransitions = 0;
        for (int HN = 1; HN <= 6; HN++)
        {
            HexGeometry.HNtoHX(HN, ref Xhx, ref Yhx);
            HexGeometry.TranslateHX(0, 0, ((MapField)Owner).Xhx, ((MapField)Owner).Yhx, Xhx, Yhx, ref Xhx, ref Yhx);
            if (Xhx >= 0 && Yhx >= 0 && Level.Width > Xhx && Level.Height > Yhx)
            {
                MapField = Level.GetField(Xhx, Yhx);
                HasRoad = MapField.FindOwnedHS(typeof(AbstractRoad)) != null;
                if (HasRoad)
                {
                    FTransitions = (byte)(FTransitions + (1 << (HN - 1)));
                }
            }
        }

        if ((FTransitions & 34) == 34)
        {
            FTransitions = (byte)((FTransitions & 221) + 128);
        }

        if ((FTransitions & 20) == 20)
        {
            FTransitions = (byte)((FTransitions & 235) + 64);
        }
    }

    // Road.pas:101
    public void UpdateNeighbourTransitions()
    {
        int Xhx = default;
        int Yhx = default;
        MapLevel Level;
        MapField MapField;
        AbstractRoad Road;
        Level = (MapLevel)Owner.Owner;
        for (int HN = 1; HN <= 6; HN++)
        {
            HexGeometry.HNtoHX(HN, ref Xhx, ref Yhx);
            HexGeometry.TranslateHX(0, 0, ((MapField)Owner).Xhx, ((MapField)Owner).Yhx, Xhx, Yhx, ref Xhx, ref Yhx);
            if (Xhx >= 0 && Yhx >= 0 && Xhx < Level.Width && Yhx < Level.Height)
            {
                MapField = Level.GetField(Xhx, Yhx);
                Road = (AbstractRoad)MapField.FindChild(131428);
                if (Road != null)
                {
                    Road.UpdateTransitions();
                }
            }
        }
    }

    public new partial class Resource : AbstractRoadResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // Road.pas:136
        public new const int ClassID = 131429;

        // Road.pas:126
        public override int ResourceUserClassID() => 131428;

        // Road.pas:141
        public void Show(int Xhp, int Yhp, in LockedSurface SurfaceDesc, Road Road)
        {
            byte Transitions;
            byte Base;
            Transitions = Road.FTransitions;
            Base = 1;
            if (Transitions != 0)
            {
                if ((Transitions & 64) == 0)
                {
                    if ((Transitions & 16) != 0)
                    {
                        FImageLibrary[Base + 4].Show(Xhp, Yhp, SurfaceDesc);
                    }

                    if ((Transitions & 4) != 0)
                    {
                        FImageLibrary[Base + 2].Show(Xhp, Yhp, SurfaceDesc);
                    }
                }
                else
                {
                    FImageLibrary[Base + 6].Show(Xhp, Yhp, SurfaceDesc);
                }

                if ((Transitions & 128) == 0)
                {
                    if ((Transitions & 2) != 0)
                    {
                        FImageLibrary[Base + 1].Show(Xhp, Yhp, SurfaceDesc);
                    }

                    if ((Transitions & 32) != 0)
                    {
                        FImageLibrary[Base + 5].Show(Xhp, Yhp, SurfaceDesc);
                    }
                }
                else
                {
                    FImageLibrary[Base + 7].Show(Xhp, Yhp, SurfaceDesc);
                }

                if ((Transitions & 1) != 0)
                {
                    FImageLibrary[Base].Show(Xhp, Yhp, SurfaceDesc);
                }

                if ((Transitions & 8) != 0)
                {
                    FImageLibrary[Base + 3].Show(Xhp, Yhp, SurfaceDesc);
                }
            }
            else
            {
                FImageLibrary[Base + 6].Show(Xhp, Yhp, SurfaceDesc);
            }
        }

        // Road.pas:171
        public override EditorClass Editor()
        {
            EditorClass Result = FEngine.Editors.FindEditor(131429);
            if (Result == null)
            {
                Result = base.Editor();
            }

            return Result;
        }

        // Road.pas:131
        public override string ResourceEditName() => "Road";

    }

    // Road.pas:18

}
