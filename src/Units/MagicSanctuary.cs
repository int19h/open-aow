// Translated from MagicSanctuary.pas.
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
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
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

namespace Aow.Units.MagicSanctuary
{
    // { TMagicSanctuaryOverlayHS (instance size 20, VMT 557D404C):
//     $0C FStructure; $10 FFadeEndFrame. }
    public partial class TMagicSanctuaryOverlayHS : Aow.Units.HSEngine.TSingleHS
    {
        public TMagicSanctuaryOverlayHS(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FFadeEndFrame = -1;
        }

        public Aow.Units.MagicSanctuary.TMagicSanctuary FStructure;
        // { $0C }
        public int FFadeEndFrame;
        // MagicSanctuary.pas:67
        // { $10 }


        // MagicSanctuary.pas:73
        public override int GetShowPriority()
        {
            int Result = default;
            var call__1 = FStructure.GetShowPriority();
            Result = call__1 + 134217728;
            return Result;
        }

        // MagicSanctuary.pas:78
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int FrameIndex = default;
            int Blend = default;
            int Xhp = default;
            int Yhp = default;
            int FramesLeft = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(FStructure.FXhx, FStructure.FYhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            var call__2 = FStructure.FResource.ImageSequenceList.GetImageSequence(19);
            Sequence = call__2;
            if (Sequence != null)
            {
                Blend = 50;
                if ((FFadeEndFrame != -1) && (FFadeEndFrame > Aow.Units.AoWE.AoWE.AoWHSMap.Frame))
                {
                    FramesLeft = FFadeEndFrame - Aow.Units.AoWE.AoWE.AoWHSMap.Frame;
                    if (FramesLeft <= 20)
                    {
                        Blend += (10 - System.Math.Abs(FramesLeft - 10)) * 3;
                    }
                    else
                    {
                        FFadeEndFrame = -1;
                    }
                }
                else
                {
                    FFadeEndFrame = -1;
                }

                FrameIndex = Sequence.GetFrameIndex(0, 0);
                var call__3 = Sequence.ImageLibrary[FrameIndex];
                call__3.ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[Blend]);
            }
        }

        // MagicSanctuary.pas:105
        public void FadeUp()
        {
            FFadeEndFrame = Aow.Units.AoWE.AoWE.AoWHSMap.Frame + 20;
        }

        // MagicSanctuary.pas:22

    }

    // { TMagicSanctuary (instance size 52, VMT 557D41F4): $30 FOverlayHS. }
    public partial class TMagicSanctuary : Aow.Units.AoWE.TStructure
    {
        public TMagicSanctuary(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__4 = new Aow.Units.MagicSanctuary.TMagicSanctuaryOverlayHS(null);
            FOverlayHS = call__4;
            FOverlayHS.FStructure = this;
        }

        // MagicSanctuary.pas:124
        public new const int ClassID = 133274;

        public Aow.Units.MagicSanctuary.TMagicSanctuaryOverlayHS FOverlayHS;
        // MagicSanctuary.pas:117
        // { $30 }
        public override void Destroy()
        {
            Aow.Units.AoWE.AoWE.AoWHSMap.DynamicVisibleHS.RemoveChild(FOverlayHS);
            FOverlayHS.Free();
            base.Destroy();
        }

        // MagicSanctuary.pas:110


        // MagicSanctuary.pas:136
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            var call__5 = FResource.TerrainList.GetTerrain(Msg.HN);
            if (call__5 >= 0)
            {
                if (Msg.MsgID == 4393)
                {
                    Msg.Param[0] = 1;
                }
            }
        }

        // MagicSanctuary.pas:127
        public override void MakeVisible(Aow.Units.HSEngine.TVisibilityList VisibilityList)
        {
            if (!FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moVisible))
            {
                base.MakeVisible(VisibilityList);
                FOverlayHS.MakeVisible(VisibilityList);
            }
        }

        // MagicSanctuary.pas:143
        public override void ArmyPlaced(Aow.Units.AoWE.TArmyHS A1)
        {
            base.ArmyPlaced(A1);
            FOverlayHS.FadeUp();
        }

        // MagicSanctuary.pas:33


    }

    // { TMagicSanctuaryResource (instance size 100, VMT 557D4464) }
    public partial class TMagicSanctuaryResource : Aow.Units.AoWE.TStructureResource
    {
        public TMagicSanctuaryResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FTerrainBound = false;
            FRazeable = false;
            FConnectRoads = false;
        }

        // MagicSanctuary.pas:160
        public new const int ClassID = 133275;

        // MagicSanctuary.pas:149


        // MagicSanctuary.pas:163
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 133274;
            return Result;
        }

        // MagicSanctuary.pas:43


        // MagicSanctuary.pas:157
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__6 = Aow.Units.AoWE.AoWE.MagicSanctuaryRStr;
            var call__7 = Aow.Units.AoWE.AoWE.TranslateRStr(call__6);
            Result = call__7;
            return Result;
        }

    }

    public static partial class MagicSanctuary
    {
        // MagicSanctuary.pas:166
        public static void RegisterMagicSanctuary(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.MagicSanctuary.TMagicSanctuary>(), Aow.Units.Engine.EClass.Of<Aow.Units.MagicSanctuary.TMagicSanctuaryResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
