// Translated from SingleCombatHex.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.SingleCombatHex
{
    public partial class TSingleCombatHexagon : Aow.Units.HSEngine.TAbstractHexagon
    {
        public TSingleCombatHexagon(TECustomNode owner) : base(owner)
        {}

        // SingleCombatHex.pas:41
        public new const int ClassID = 2229534;

        // SingleCombatHex.pas:47
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref X, ref Y);
            X = (X - XhpSurf) - 8;
            Y = Y - YhpSurf;
            var call__1 = FResource.ImageSequenceList.GetImageSequence(0);
            Sequence = call__1;
            var call__2 = Sequence.ImageLibrary.Get(Sequence.ImageIndex);
            call__2.Show(X, Y, SurfaceDesc);
        }

        // SingleCombatHex.pas:44
        public override Aow.Units.HSEngine.TMapObjectResourceList GetResourceList(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.HSEngine.TMapObjectResourceList Result = default;
            Result = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources;
            return Result;
        }

    }

    public partial class TSingleCombatHexagonResource : Aow.Units.HSEngine.TAbstractHexagonResource
    {
        public TSingleCombatHexagonResource(TECustomNode owner) : base(owner)
        {}

        // SingleCombatHex.pas:59
        public new const int ClassID = 2229535;

        // SingleCombatHex.pas:73
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 65621)
            {
                Msg.Result = 5;
            }
        }

        // SingleCombatHex.pas:79
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229534;
            return Result;
        }

        // SingleCombatHex.pas:65
        public override void SetLevel(byte Value)
        {
            base.SetLevel(0);
        }

        // SingleCombatHex.pas:70
        public override Aow.Units.HSEngine.THexagonControl GetHexagonControl()
        {
            Aow.Units.HSEngine.THexagonControl Result = default;
            Result = null;
            return Result;
        }

        // SingleCombatHex.pas:62
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Combat Single Hexagon";
            return Result;
        }

    }

    public static partial class SingleCombatHex
    {
        // SingleCombatHex.pas:82
        public static void RegisterSingleCombatHexagon(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.SingleCombatHex.TSingleCombatHexagonResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.SingleCombatHex.TSingleCombatHexagon>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
