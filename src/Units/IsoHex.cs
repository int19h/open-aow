// Translated from IsoHex.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes
{

    // FDynamic, FOpenSides (bit HN-1: the neighbour HN is no hexagon of this kind), FSides (bits
    // 0.5: the neighbour HN has another terrain, bits 6.7: the image variant)
    public partial class IsometricHexagon : AbstractHexagon
    {
        public IsometricHexagon(ECustomNode owner) : base(owner)
        {
            FSides = (byte)(Random(4) << 6);
        }

        // IsoHex.pas:214
        public new const int ClassID = 131437;

        protected internal DynamicIsometricHexagon FDynamic;
        protected internal byte FOpenSides;
        protected internal byte FSides;

        // IsoHex.pas:204
        public override void Dispose()
        {
            if (FDynamic != null)
            {
                FDynamic?.Dispose();
                FDynamic = null;
            }

            base.Dispose();
        }

        // IsoHex.pas:198

        // IsoHex.pas:224
        public override void Disconnect()
        {
            if (FDynamic != null)
            {
                FDynamic?.Dispose();
                FDynamic = null;
            }

            base.Disconnect();
        }

        // IsoHex.pas:234
        public override void NeighbourTerrainChanged(int HN, in HSTerrainInfo Info)
        {
            UpdateTransitions();
        }

        // IsoHex.pas:239
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            CustomImageLibrary IL;
            int Xhp = default;
            int Yhp = default;
            Aow.Game.World.Map.HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
            Xhp = Xhp - XhpSurf - 8;
            Yhp = Yhp - YhpSurf;
            IL = FResource.ImageSequenceList.ImageSequences[0].ImageLibrary;
            IL[(int)(((uint)FSides) >> 6) + 40].Show(Xhp, Yhp, SurfaceDesc);
            Xhp -= 6;
            Yhp -= 9;
            if ((FSides & 1) != 0)
            {
                IL[31].Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((FSides & 2) != 0)
            {
                IL[32].Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((FSides & 32) != 0)
            {
                IL[30].Show(Xhp, Yhp, SurfaceDesc);
            }
        }

        // IsoHex.pas:256
        public virtual void ShowDynamic(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
        }

        // IsoHex.pas:50

    }
}

namespace Aow.Game.Draw
{

    // owned by its TIsometricHexagon (FOwner set directly, so it is not added as a child) and shown
    // from the map field of that hexagon
    public partial class DynamicIsometricHexagon : HexagonSprite
    {
        // IsoHex.pas:126
        public new const int ClassID = 131439;

        // IsoHex.pas:136
        public override Set<EControlStyleElement> GetControlStyle() => Set<EControlStyleElement>.Of();

        // IsoHex.pas:161
        public override void SetVisible(bool Value)
        {
        }

        // IsoHex.pas:165
        public override bool GetVisible() => false;

        // IsoHex.pas:131
        public override bool CanSelect(int Xhp, int Yhp) => false;

        // IsoHex.pas:141
        public override Set<ControlModeElement> ControlMode() => Set<ControlModeElement>.Of();

    }
}

namespace Aow.Game.World.Hexes
{

    // FDynamicLevel (26)
    public partial class IsometricHexagonResource : AbstractHexagonResource
    {
        public IsometricHexagonResource(ECustomNode owner) : base(owner)
        {
            FDynamicLevel = 26;
        }

        // IsoHex.pas:268
        public new const int ClassID = 131438;

        protected internal byte FDynamicLevel;

        // IsoHex.pas:262

        // IsoHex.pas:302
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 65621)
            {
                Msg.Result = 5;
            }
        }

        // IsoHex.pas:273
        public override Aow.Game.World.MapObject PlaceResourceUser(HSMap Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            MapField MapField;
            MapField = Map.MapContainer.GetField(Xhx, Yhx, Lhx);
            Aow.Game.World.MapObject Result = (Aow.Game.World.MapObject)MapField.FindOwnedHS(typeof(IsometricHexagon));
            if (Result != null)
            {
                if (Result.GetTerrain(0) != FTerrainList.GetTerrain(0))
                {
                    Result.SetResource(this);
                    MapField.UpdateTerrainType();
                    ((AbstractHexagon)Result).UpdateTransitions();
                }
                else
                {
                    Result.SetResource(this);
                }

                Map.InvalidateStaticScene(Xhx, Yhx, Lhx);
            }
            else
            {
                Result = (Aow.Game.World.MapObject)CreateResourceUser();
                if (!Map.PlaceHX(Result, Xhx, Yhx, Lhx))
                {
                    Result?.Dispose();
                    Result = null;
                }
            }

            return Result;
        }

        // IsoHex.pas:64

    }

    public partial class UpperIsometricHexagon : IsometricHexagon
    {
        public UpperIsometricHexagon(ECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 131440;

        // IsoHex.pas:355
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            CustomImageLibrary IL;
            int Xhp = default;
            int Yhp = default;
            if (FDynamic == null)
            {
                Aow.Game.World.Map.HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
                Xhp = Xhp - XhpSurf - 8;
                Yhp = Yhp - YhpSurf;
                IL = FResource.ImageSequenceList.ImageSequences[0].ImageLibrary;
                IL[(int)(((uint)FSides) >> 6) + 40].Show(Xhp, Yhp, SurfaceDesc);
                Xhp -= 6;
                Yhp -= 9;
                if ((FSides & 1) != 0)
                {
                    IL[31].Show(Xhp, Yhp, SurfaceDesc);
                }

                if ((FSides & 2) != 0)
                {
                    IL[32].Show(Xhp, Yhp, SurfaceDesc);
                }

                if ((FSides & 32) != 0)
                {
                    IL[30].Show(Xhp, Yhp, SurfaceDesc);
                }
            }
        }

        public new partial class Resource : IsometricHexagonResource
        {
            public Resource(ECustomNode owner) : base(owner)
            { }

            public new const int ClassID = 131441;

            // IsoHex.pas:443
            public override int ResourceUserClassID() => 131440;

            // IsoHex.pas:448
            public override string ResourceEditName() => Aow.Game.World.Hexes.UpperIsometricHexagon.Resource.UpperIsometricHexagonStr;

            private static readonly string UpperIsometricHexagonStr = "Upper Isometric Hexagon";

        }

    }

    public partial class LowerIsometricHexagon : IsometricHexagon
    {
        public LowerIsometricHexagon(ECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 131442;

        public new partial class Resource : IsometricHexagonResource
        {
            public Resource(ECustomNode owner) : base(owner)
            { }

            public new const int ClassID = 131443;

            // IsoHex.pas:581
            public override int ResourceUserClassID() => 131442;

            // IsoHex.pas:576
            public override string ResourceEditName() => Aow.Game.World.Hexes.LowerIsometricHexagon.Resource.LowerIsometricHexagonStr;

            private static readonly string LowerIsometricHexagonStr = "Lower Isometric Hexagon";

        }

    }
}
