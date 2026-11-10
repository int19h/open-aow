// Translated from Bridge.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World;

// { The Bridge unit of HSEPack.dpl: a bridge over water (TBridge, ClassID $20174, an overlay at level
//   10) whose image is chosen from the roads and shores around it (TBridgeResource, ClassID $20175). }
// FTransitions (two bits per side HN 1.6: 0 none, 1 a road, 2 the shore the bridge reaches)
public partial class Bridge : AbstractRoad
{
    public Bridge(ECustomNode owner) : base(owner)
    {
    }

    // Bridge.pas:48
    public new const int ClassID = 131444;

    internal ushort FTransitions;

    // Bridge.pas:43

    // Bridge.pas:53
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
        ((Resource)FResource).Show(Xhp - XhpSurf - 8 - 6, Yhp - YhpSurf - 9, SurfaceDesc, this);
    }

    // Bridge.pas:63
    // { Original oddity: the overlay of the owner of the previous owner (a map level, not a map field)
    //   is updated. }
    public override bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        MapField MapField;
        HexagonSprite OldRoad;
        MapField OldOwner;
        MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
        OldRoad = MapField.FindHS(typeof(AbstractRoad));
        if (OldRoad != null)
        {
            OldRoad.Dispose();
        }

        OldOwner = (MapField)Owner;
        Owner = MapField;
        FStatus = FStatus + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
        // { The original refreshes the overlay of the previous field's owner, a map level read as a map field, which is undefined, so skipped. }
        if (OldOwner?.Owner is MapField OldOwnerField)
        {
            OldOwnerField.UpdateOverlayType();
        }

        MapField.UpdateOverlayType();
        UpdateTransitions();
        MapContainer.Map.InvalidateStaticScene(Xhx, Yhx, Lhx);
        return true;
    }

    // Bridge.pas:82
    public override void UpdateTransitions()
    {
        FixedArray_byte_6 Sides = default;
        int Transitions;
        int MaxDistance;
        int MaxSide;
        bool HasRoad;
        bool InShore;
        int Xhx = default;
        int Yhx = default;
        int Shores;
        sbyte TerrainType;
        MapLevel Level;
        int Start;
        int Distance;
        MapField MapField;
        bool IsShore;
        Transitions = 0;
        Level = (MapLevel)Owner.Owner;
        Shores = 0;
        InShore = false;
        for (int HN = 1; HN <= 6; HN++)
        {
            HexGeometry.HNtoHX(HN, ref Xhx, ref Yhx);
            HexGeometry.TranslateHX(0, 0, ((MapField)Owner).Xhx, ((MapField)Owner).Yhx, Xhx, Yhx, ref Xhx, ref Yhx);
            if (Xhx >= 0 && Yhx >= 0 && Level.Width > Xhx && Level.Height > Yhx)
            {
                MapField = Level.GetField(Xhx, Yhx);
                TerrainType = MapField.TerrainType;
                HasRoad = MapField.FindHS(typeof(AbstractRoad)) != null;
                IsShore = TerrainType != 0 && FResource.TerrainList.GetTerrain(0) != TerrainType && MapField.FindHS(typeof(BorderHexagon)) == null;
                if (HasRoad)
                {
                    Sides[HN - 1] = 1;
                    InShore = false;
                }
                else
                {
                    if (IsShore)
                    {
                        if (!InShore)
                        {
                            Shores += 1;
                        }

                        Sides[HN - 1] = (byte)(Shores + 2);
                        InShore = true;
                    }
                    else
                    {
                        Sides[HN - 1] = 0;
                        InShore = false;
                    }
                }
            }
            else
            {
                Sides[HN - 1] = 0;
            }
        }

        // { the shores on side 1 and side 6 are one shore }
        if (Sides[1 - 1] > 2 && Sides[6 - 1] > 2 && Sides[1 - 1] != Sides[6 - 1])
        {
            for (int HN = 3; HN <= 6; HN++)
            {
                if (Sides[HN - 1] == Sides[6 - 1])
                {
                    Sides[HN - 1] = Sides[1 - 1];
                }
            }

            Shores -= 1;
        }

        // { the side the bridge starts from: a road, else the first side without water }
        Start = 1;
        while (Start <= 6 && Sides[Start - 1] != 1)
        {
            Start += 1;
        }

        if (Start == 1)
        {
            if (Sides[2 - 1] == 1)
            {
                Start = 2;
            }
            else
            {
                if (Sides[3 - 1] == 1)
                {
                    Start = 3;
                }
            }
        }

        if (Start == 7)
        {
            Start = 1;
            while (Start <= 6 && Sides[Start - 1] == 0)
            {
                Start += 1;
            }

            if (Start == 1)
            {
                if (Sides[2 - 1] != 0)
                {
                    Start = 2;
                }
                else
                {
                    if (Sides[3 - 1] == 0)
                    {
                        Start = 3;
                    }
                }
            }
        }

        if (Start <= 6)
        {
            // { of every shore keep the side farthest from the start }
            for (int Shore = 3; Shore <= Shores + 2; Shore++)
            {
                MaxDistance = 0;
                MaxSide = 0;
                if (Shore != Sides[Start - 1] || Shores == 1)
                {
                    for (int HN = 1; HN <= 6; HN++)
                    {
                        if (Shore == Sides[HN - 1])
                        {
                            Distance = Math.Abs(HN - Start);
                            if (Distance > 3)
                            {
                                Distance = Math.Abs(Distance - 6);
                            }

                            if (Distance > MaxDistance)
                            {
                                MaxDistance = Distance;
                                MaxSide = HN;
                            }
                        }
                    }
                }
                else
                {
                    MaxSide = Start;
                }

                for (int HN = 1; HN <= 6; HN++)
                {
                    if (HN != Start && Shore == Sides[HN - 1])
                    {
                        if (HN != MaxSide)
                        {
                            Sides[HN - 1] = 0;
                        }
                        else
                        {
                            Sides[HN - 1] = 2;
                        }
                    }
                }
            }

            for (int HN = 1; HN <= 6; HN++)
            {
                Transitions += Sides[HN - 1] << ((HN - 1) * 2);
            }
        }

        FTransitions = (ushort)Transitions;
    }

    public new partial class Resource : AbstractRoadResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            SetLevel(10);
        }

        // Bridge.pas:205
        public new const int ClassID = 131445;

        // Bridge.pas:189

        // Bridge.pas:195
        public override int ResourceUserClassID() => 131444;

        // Bridge.pas:210
        public void Show(int Xhp, int Yhp, in SurfaceDesc SurfaceDesc, Bridge Bridge)
        {
            int Transitions;
            int Base;
            Transitions = Bridge.FTransitions;
            Base = 1;
            FImageLibrary[Base + 18].Show(Xhp, Yhp, SurfaceDesc);
            if ((Transitions & 3) != 0)
            {
                if ((Transitions & 3) == 1)
                {
                    FImageLibrary[Base + 6].Show(Xhp, Yhp, SurfaceDesc);
                }

                FImageLibrary[Base + 12].Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((Transitions & 12) != 0)
            {
                if ((Transitions & 12) == 4)
                {
                    FImageLibrary[Base + 7].Show(Xhp, Yhp, SurfaceDesc);
                }

                FImageLibrary[Base + 13].Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((Transitions & 48) != 0)
            {
                if ((Transitions & 48) == 16)
                {
                    FImageLibrary[Base + 8].Show(Xhp, Yhp, SurfaceDesc);
                }
                else
                {
                    FImageLibrary[Base + 2].Show(Xhp, Yhp, SurfaceDesc);
                }

                FImageLibrary[Base + 14].Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((Transitions & 192) != 0)
            {
                if ((Transitions & 192) == 64)
                {
                    FImageLibrary[Base + 9].Show(Xhp, Yhp, SurfaceDesc);
                }
                else
                {
                    FImageLibrary[Base + 3].Show(Xhp, Yhp, SurfaceDesc);
                }

                FImageLibrary[Base + 15].Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((Transitions & 768) != 0)
            {
                if ((Transitions & 768) == 256)
                {
                    FImageLibrary[Base + 10].Show(Xhp, Yhp, SurfaceDesc);
                }
                else
                {
                    FImageLibrary[Base + 4].Show(Xhp, Yhp, SurfaceDesc);
                }

                FImageLibrary[Base + 16].Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((Transitions & 3072) != 0)
            {
                if ((Transitions & 3072) == 1024)
                {
                    FImageLibrary[Base + 11].Show(Xhp, Yhp, SurfaceDesc);
                }

                FImageLibrary[Base + 17].Show(Xhp, Yhp, SurfaceDesc);
            }
        }

        // Bridge.pas:253
        public override EditorClass Editor()
        {
            EditorClass Result = FEngine.Editors.FindEditor(131445);
            if (Result == null)
            {
                Result = base.Editor();
            }

            return Result;
        }

        // Bridge.pas:259
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            if (Msg.MsgID != 65556)
            {
                base.MsgProc(ref Msg, Sender);
            }
        }

        // Bridge.pas:28

        // Bridge.pas:200
        public override string ResourceEditName() => HexText.BridgeStr;

    }

    // Bridge.pas:18

}
