// Translated from Hexagon.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;

namespace Aow.Units.Hexagon
{
    // { The Hexagon unit of HSEPack.dpl: the plain terrain hexagon (THexagon, ClassID $20152) drawn from
//   the first image of its resource's image library, with the transitions to the neighbour terrain
//   on three sides from HexTrans.ILB (THexagonResource, ClassID $20153). }
    // FTransitions (the transition image of the sides 0.2), FTransitionSides (bits 0.2: the sides
    // with a transition, bits 3.5: the shifted variants)
    public partial class THexagon : TAbstractHexagon
    {
        public THexagon(TECustomNode owner) : base(owner)
        {}

        public new const int ClassID = 131410;

        protected internal FixedArray_byte_3 FTransitions;
        protected internal byte FTransitionSides;
        // Hexagon.pas:60
        public override void NeighbourTerrainChanged(int HN, in THSTerrainInfo Info)
        {
            if (HN == 0)
            {
                switch (Info.HN)
                {
                    case 1:
                        UpdateTransition(0);
                        break;
                    case 2:
                        UpdateTransition(1);
                        break;
                    case 6:
                        UpdateTransition(2);
                        break;
                }
            }
        }

        // Hexagon.pas:52
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            ((THexagonResource)FResource).Show(Xhp - XhpSurf - 8, Yhp - YhpSurf, SurfaceDesc, this);
        }

        // Hexagon.pas:74
        public override void UpdateTransitions()
        {
            byte Side = default;
            {
                int first__3 = 0;
                int end__1 = 2;
                for (int ordinal__2 = first__3; ordinal__2 <= end__1; ordinal__2++)
                {
                    Side = (byte)ordinal__2;
                    UpdateTransition(Side);
                }
            }
        }

        // Hexagon.pas:70
        public virtual void UpdateTransition(byte Side)
        {
        }

    }

    // FTransitionLibrary (HexTrans.ILB)
    public partial class THexagonResource : TAbstractHexagonResource
    {
        public THexagonResource(TECustomNode owner) : base(owner)
        {
            SetLevel(0);
        }

        // Hexagon.pas:89
        public new const int ClassID = 131411;

        protected internal TCustomImageLibrary FTransitionLibrary;
        // Hexagon.pas:83


        // Hexagon.pas:99
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 536870920)
            {
                var call__4 = FEngine.HSSet.ImageLibraryManager.LinkToIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), (AnsiString)"HexTrans.ILB");
                FTransitionLibrary = call__4;
            }

            if (Msg.MsgID == 65621)
            {
                Msg.Result = 5;
            }
        }

        // Hexagon.pas:94
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131410;
            return Result;
        }

        // Hexagon.pas:112
        public override TMapObject PlaceResourceUser(THSMap Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            TMapObject Result = default;
            TMapField MapField = default;
            var call__5 = Map.MapContainer.GetField(Xhx, Yhx, Lhx);
            MapField = call__5;
            var call__6 = ResourceUserClassID();
            var call__7 = MapField.FindOwnedChild(call__6);
            Result = (TMapObject)call__7;
            if (Result != null)
            {
                var call__8 = Result.GetTerrain(0);
                var call__9 = FTerrainList.GetTerrain(0);
                if (call__8 != call__9)
                {
                    Result.SetResource(this);
                    MapField.UpdateTerrainType();
                    ((TAbstractHexagon)Result).UpdateTransitions();
                }
                else
                {
                    Result.SetResource(this);
                }

                Map.InvalidateStaticScene(Xhx, Yhx, Lhx);
            }
            else
            {
                Result = (TMapObject)CreateResourceUser();
                var call__10 = Result.PlaceHX(Map.MapContainer, Xhx, Yhx, Lhx);
                if (!call__10)
                {
                    Result.Free();
                    Result = null;
                }
            }

            return Result;
        }

        // Hexagon.pas:141
        public void Show(int Xhp, int Yhp, in TSurfaceDesc SurfaceDesc, THexagon Hexagon)
        {
            byte Sides = default;
            var call__11 = FImageLibrary[0];
            call__11.Show(Xhp, Yhp, SurfaceDesc);
            if ((Hexagon.FTransitionSides & 7) != 0)
            {
                Sides = Hexagon.FTransitionSides;
                if ((Sides & 2) != 0)
                {
                    if ((Sides & 16) != 0)
                    {
                        var call__12 = FTransitionLibrary[Hexagon.FTransitions[1]];
                        call__12.Show(Xhp - 6, Yhp - 9 - 32, SurfaceDesc);
                    }
                    else
                    {
                        var call__13 = FTransitionLibrary[Hexagon.FTransitions[1]];
                        call__13.Show(Xhp - 6, Yhp - 9, SurfaceDesc);
                    }
                }

                if ((Sides & 1) != 0)
                {
                    if ((Sides & 8) != 0)
                    {
                        var call__14 = FTransitionLibrary[Hexagon.FTransitions[0]];
                        call__14.Show(Xhp - 6 - 32, Yhp - 9 - 16, SurfaceDesc);
                    }
                    else
                    {
                        var call__15 = FTransitionLibrary[Hexagon.FTransitions[0]];
                        call__15.Show(Xhp - 6, Yhp - 9, SurfaceDesc);
                    }
                }

                if ((Sides & 4) != 0)
                {
                    if ((Sides & 32) != 0)
                    {
                        var call__16 = FTransitionLibrary[Hexagon.FTransitions[2]];
                        call__16.Show(Xhp - 6 + 32, Yhp - 9 - 16, SurfaceDesc);
                    }
                    else
                    {
                        var call__17 = FTransitionLibrary[Hexagon.FTransitions[2]];
                        call__17.Show(Xhp - 6, Yhp - 9, SurfaceDesc);
                    }
                }
            }
        }

        // Hexagon.pas:32


        // Hexagon.pas:107
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = HexagonStr;
            return Result;
        }

    }

    public static partial class Hexagon
    {
        // Hexagon.pas:168
        public static void RegisterHexagon(TEngine Engine)
        {
            Engine.RegisterEClasses([EClass.Of<THexagon>(), EClass.Of<THexagonResource>()]);
        }
    }
}
