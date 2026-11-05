// Translated from MagicSanctuary.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.MagicSanctuary
{
    // FStructure. FFadeEndFrame
    public partial class TMagicSanctuaryOverlayHS : TSingleHS
    {
        public TMagicSanctuaryOverlayHS(TECustomNode owner) : base(owner)
        {
            FFadeEndFrame = -1;
        }

        public TMagicSanctuary FStructure;
        public int FFadeEndFrame;
        // MagicSanctuary.pas:67


        // MagicSanctuary.pas:73
        public override int GetShowPriority() => FStructure.GetShowPriority() + 134217728;

        // MagicSanctuary.pas:78
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TImageSequence Sequence;
            int FrameIndex;
            int Blend;
            int Xhp = default;
            int Yhp = default;
            int FramesLeft;
            HXtoHP(FStructure.FXhx, FStructure.FYhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            Sequence = FStructure.FResource.ImageSequenceList.GetImageSequence(19);
            if (Sequence != null)
            {
                Blend = 50;
                if (FFadeEndFrame != -1 && FFadeEndFrame > AoWHSMap.Frame)
                {
                    FramesLeft = FFadeEndFrame - AoWHSMap.Frame;
                    if (FramesLeft <= 20)
                    {
                        Blend += (10 - Math.Abs(FramesLeft - 10)) * 3;
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
                Sequence.ImageLibrary[FrameIndex].ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, IntensityBlendTable.LookupTables[Blend]);
            }
        }

        // MagicSanctuary.pas:105
        public void FadeUp()
        {
            FFadeEndFrame = AoWHSMap.Frame + 20;
        }

        // MagicSanctuary.pas:22

    }

    // FOverlayHS
    public partial class TMagicSanctuary : TStructure
    {
        public TMagicSanctuary(TECustomNode owner) : base(owner)
        {
            FOverlayHS = new TMagicSanctuaryOverlayHS(null);
            FOverlayHS.FStructure = this;
        }

        // MagicSanctuary.pas:124
        public new const int ClassID = 133274;

        public TMagicSanctuaryOverlayHS FOverlayHS;
        // MagicSanctuary.pas:117
        public override void Destroy()
        {
            AoWHSMap.DynamicVisibleHS.RemoveChild(FOverlayHS);
            FOverlayHS.Free();
            base.Destroy();
        }

        // MagicSanctuary.pas:110


        // MagicSanctuary.pas:136
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (FResource.TerrainList.GetTerrain(Msg.HN) >= 0)
            {
                if (Msg.MsgID == 4393)
                {
                    Msg.Param[0] = 1;
                }
            }
        }

        // MagicSanctuary.pas:127
        public override void MakeVisible(TVisibilityList VisibilityList)
        {
            if (!FStatus.Contains(TMapObjectStatusElement.moVisible))
            {
                base.MakeVisible(VisibilityList);
                FOverlayHS.MakeVisible(VisibilityList);
            }
        }

        // MagicSanctuary.pas:143
        public override void ArmyPlaced(TArmyHS A1)
        {
            base.ArmyPlaced(A1);
            FOverlayHS.FadeUp();
        }

        // MagicSanctuary.pas:33


    }

    public partial class TMagicSanctuaryResource : TStructureResource
    {
        public TMagicSanctuaryResource(TECustomNode owner) : base(owner)
        {
            FTerrainBound = false;
            FRazeable = false;
            FConnectRoads = false;
        }

        // MagicSanctuary.pas:160
        public new const int ClassID = 133275;

        // MagicSanctuary.pas:149


        // MagicSanctuary.pas:163
        public override int ResourceUserClassID() => 133274;

        // MagicSanctuary.pas:43


        // MagicSanctuary.pas:157
        public override AnsiString ResourceEditName() => TranslateRStr(MagicSanctuaryRStr);

    }

    public static partial class MagicSanctuary
    {
        // MagicSanctuary.pas:166
        public static void RegisterMagicSanctuary(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TMagicSanctuary>(), EClass.Of<TMagicSanctuaryResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
