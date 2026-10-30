// Translated from BorderHx.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;

namespace Aow.Units.BorderHx
{
    // { The BorderHx unit of HSEPack.dpl: the border hexagon (TBorderHexagon, ClassID $20557) drawn at
//   level 11 over the map edge; it also adds itself to the hexagon above (HN 1) of its map field.
//   TBorderHexagonResource has ClassID $20558. }
    // { BorderHx.TBorderHexagon (instance size 16) }
    public partial class TBorderHexagon : Aow.Units.HSEngine.TAbstractBorderHexagon
    {
        public TBorderHexagon(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // BorderHx.pas:59
        public new const int ClassID = 132439;

        // BorderHx.pas:39
        // { TBorderHexagon }
        public override void SetOwner(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TECustomNode OldField = default;
            if (AOwner != this.Owner)
            {
                OldField = Owner;
                if (((OldField != null) && (OldField is Aow.Units.HSEngine.TMapField)) && (((Aow.Units.HSEngine.TMapField)OldField).Yhx != 0))
                {
                    var call__1 = ((Aow.Units.HSEngine.TMapLevel)OldField.Owner).GetField(((Aow.Units.HSEngine.TMapField)OldField).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx - 1);
                    call__1.RemoveChild(this);
                }

                base.SetOwner(AOwner);
                AOwner = Owner;
                if (((AOwner != null) && (AOwner is Aow.Units.HSEngine.TMapField)) && (((Aow.Units.HSEngine.TMapField)AOwner).Yhx != 0))
                {
                    var call__2 = ((Aow.Units.HSEngine.TMapLevel)AOwner.Owner).GetField(((Aow.Units.HSEngine.TMapField)AOwner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx - 1);
                    call__2.AddHS(this, 1);
                }
            }
            else
            {
                base.SetOwner(AOwner);
            }
        }

        // BorderHx.pas:64
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            var call__3 = FResource.ImageLibrary[0];
            call__3.Show((Xhp - XhpSurf) - 8, Yhp - YhpSurf, SurfaceDesc);
        }

        // BorderHx.pas:72
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // BorderHx.pas:77
        public override byte GetLevel()
        {
            byte Result = default;
            Result = 11;
            return Result;
        }

        // BorderHx.pas:82
        public override int GetShowPriority()
        {
            int Result = default;
            int Xhp = default;
            int Yhp = default;
            Xhp = (((Aow.Units.HSEngine.TMapField)Owner).Xhx * 32) + 8;
            Yhp = ((((Aow.Units.HSEngine.TMapField)Owner).Xhx & 1) * 16) + (((Aow.Units.HSEngine.TMapField)Owner).Yhx * 32);
            Result = (((Yhp + 32) << 13) + (11 << 26)) + (Xhp + 8);
            return Result;
        }

        // BorderHx.pas:95
        public override bool CanChangeTerrain(in Aow.Units.HSEngine.THSSenderInfo Info)
        {
            bool Result = default;
            Result = Info.HN != 0;
            return Result;
        }

    }

    // { BorderHx.TBorderHexagonResource (instance size 52) }
    public partial class TBorderHexagonResource : Aow.Units.HSEngine.TAbstractBorderHexagonResource
    {
        public TBorderHexagonResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        public new const int ClassID = 132440;

        // BorderHx.pas:112
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132439;
            return Result;
        }

        // BorderHx.pas:107
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Border Hexagon";
            return Result;
        }

    }

    public static partial class BorderHx
    {
        // BorderHx.pas:117
        public static void RegisterBorderHexagon(Aow.Units.Engine.TEngine Engine)
        {
            Engine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.BorderHx.TBorderHexagon>(), Aow.Units.Engine.EClass.Of<Aow.Units.BorderHx.TBorderHexagonResource>()]);
        }
    }
}
