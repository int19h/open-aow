// Translated from MagicSanctuary.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Draw
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
            Aow.Game.World.Map.HexGeometry.HXtoHP(FStructure.FXhx, FStructure.FYhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            Sequence = FStructure.FResource.ImageSequenceList.GetImageSequence(19);
            if (Sequence != null)
            {
                Blend = 50;
                if (FFadeEndFrame != -1 && FFadeEndFrame > Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame)
                {
                    FramesLeft = FFadeEndFrame - Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame;
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
                Sequence.ImageLibrary[FrameIndex].ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, Aow.Game.Draw.GfxState.IntensityBlendTable.LookupTables[Blend]);
            }
        }

        // MagicSanctuary.pas:105
        public void FadeUp()
        {
            FFadeEndFrame = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame + 20;
        }

        // MagicSanctuary.pas:22

    }
}

namespace Aow.Game.World.Structures
{

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
        public override void Dispose()
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.DynamicVisibleHS.RemoveChild(FOverlayHS);
            FOverlayHS?.Dispose();
            base.Dispose();
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
            if (!FStatus.Contains(Aow.Game.World.TMapObjectStatusElement.moVisible))
            {
                base.MakeVisible(VisibilityList);
                FOverlayHS.MakeVisible(VisibilityList);
            }
        }

        // MagicSanctuary.pas:143
        public override void ArmyPlaced(TArmyHS PlacedArmy)
        {
            base.ArmyPlaced(PlacedArmy);
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
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TMagicSanctuaryResource.MagicSanctuaryRStr);

        private static readonly string MagicSanctuaryRStr = "Magic Sanctuary";

    }
}
