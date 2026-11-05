// Translated from CenterViewLoc.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CenterViewLoc
{
    public partial class TCenterViewLocation : TMapObject
    {
        public TCenterViewLocation(TECustomNode owner) : base(owner)
        {}

        // CenterViewLoc.pas:46
        public new const int ClassID = 2233640;

        // CenterViewLoc.pas:80
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            if (AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // CenterViewLoc.pas:63
        public override void Activate()
        {
            base.Activate();
            if (!GetEditMode())
            {
                if (AoWCombatMap.FCenterViewLocation != null)
                {
                    AoWCombatMap.FCenterViewLocation.Free();
                }

                AoWCombatMap.FCenterViewLocation = this;
            }
        }

        // CenterViewLoc.pas:73
        public override void Deactivate()
        {
            base.Deactivate();
            if (!GetEditMode())
            {
                if (AoWCombatMap.FCenterViewLocation == this)
                {
                    AoWCombatMap.FCenterViewLocation = null;
                }
            }
        }

        // CenterViewLoc.pas:55
        public override bool CanSelect(int Xhp, int Yhp) => true;

        // CenterViewLoc.pas:49
        public override TMapObjectResourceList GetResourceList(TEngine AEngine) => AoWHSSet.FCombatResources;

        // CenterViewLoc.pas:52
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx) => true;

        // CenterViewLoc.pas:58
        public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx) => base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);

    }

    public partial class TCenterViewLocationResource : TMapObjectResource
    {
        public TCenterViewLocationResource(TECustomNode owner) : base(owner)
        {
            SetLevel(27);
        }

        // CenterViewLoc.pas:91
        public new const int ClassID = 2233641;

        // CenterViewLoc.pas:85


        // CenterViewLoc.pas:102
        public override int ResourceUserClassID() => 2233640;

        // CenterViewLoc.pas:94
        public override void SetLevel(byte Value)
        {
            base.SetLevel(27);
        }

        // CenterViewLoc.pas:34


        // CenterViewLoc.pas:99
        public override string ResourceEditName() => "Center View Location";

    }

    public static partial class CenterViewLoc
    {
        // CenterViewLoc.pas:105
        public static void RegisterCenterViewLocation(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TCenterViewLocationResource>(), EClass.Of<TCenterViewLocation>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
