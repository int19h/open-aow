// Translated from House.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Terrain;

public partial class House : CombatStructure
{
    public House(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 2229522;

    public int FRace;
    public int FFlagID;

    // House.pas:45
    public override void Activate()
    {
        PlayerStructure Structure;
        base.Activate();
        if (!AoWCombatMap.Current.Status.Contains(HSMapStatusElement.Editting))
        {
            Structure = (PlayerStructure)AoWHSMap.Current.MapContainer.GetMapLevel(CombatPlacement.CombatLLoc).GetField(CombatPlacement.CombatXLoc, CombatPlacement.CombatYLoc).FindNoneTransparentHS(typeof(PlayerStructure));
            if (Structure != null)
            {
                if (Structure is City)
                {
                    FRace = ((City)Structure).FRace;
                }
                else
                {
                    FRace = 0;
                }

                if (Structure.FPlayer == -1)
                {
                    FFlagID = -1;
                }
                else
                {
                    FFlagID = AoWHSMap.Current.FPlayers.GetPlayers(Structure.FPlayer).FPlayerRace;
                }

                if (FFlagID == -1)
                {
                    FFlagID = 32;
                }
            }
        }
    }

    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        ImageSequence Sequence;
        int ImageIndex;
        int Xhp = default;
        int Yhp = default;
        int TerrainOffset;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
        XhpSurf = Xhp - XhpSurf;
        YhpSurf = Yhp - YhpSurf;
        if (FHitPoints == 0)
        {
            Sequence = AoWHSSet.Current.FImageSequences.GetImageSequence(50);
            var yhx = GetYhx();
            var xhx = GetXhx();
            switch (AoWCombatMap.Current.MapContainer.GetMapLevel(0).GetField(xhx, yhx).TerrainType)
            {
                case 3:
                    TerrainOffset = 1;
                    break;
                case 5:
                case 7:
                case 12:
                    TerrainOffset = 2;
                    break;
                default:
                    TerrainOffset = 0;
                    break;
            }

            if (Sequence != null)
            {
                do
                {
                    ImageIndex = Sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, 3) + TerrainOffset * 3);
                    if (ImageIndex != -1)
                    {
                        Sequence.ImageLibrary[ImageIndex].Show(XhpSurf + Sequence.dXshift + 16, YhpSurf + Sequence.dYshift + 16, SurfaceDesc);
                    }

                    Sequence = Sequence.Next;
                }
                while (!(Sequence == null));
            }
        }
        else
        {
            var yhx = GetYhx();
            var xhx = GetXhx();
            switch (AoWCombatMap.Current.MapContainer.GetMapLevel(0).GetField(xhx, yhx).TerrainType)
            {
                case 3:
                    TerrainOffset = 20;
                    break;
                case 5:
                case 7:
                case 12:
                    TerrainOffset = 40;
                    break;
                default:
                    TerrainOffset = 0;
                    break;
            }

            Sequence = FResource.ImageSequenceList.GetImageSequence(TerrainOffset + FRace);
            if (Sequence != null)
            {
                do
                {
                    switch (Sequence.Layer)
                    {
                        case 7:
                            ImageIndex = Sequence.GetFrameIndex(AoWCombatMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                            if (ImageIndex != -1)
                            {
                                if (!AoWCombatMap.Current.Status.Contains(HSMapStatusElement.Editting))
                                {
                                    var xhx2 = GetXhx();
                                    var yhx2 = GetYhx();
                                    AoWHSSet.Current.FFlagControl.ShowStructureFlagEx(XhpSurf + 21 + Sequence.dXshift, YhpSurf + Sequence.dYshift + 16, xhx2 + AoWHSMap.Current.Frame + yhx2 * 100, (byte)FFlagID, SurfaceDesc, false);
                                }
                            }

                            break;
                        default:
                            {
                                ImageIndex = Sequence.GetFrameIndex(AoWCombatMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                                if (ImageIndex != -1)
                                {
                                    Sequence.ImageLibrary[ImageIndex].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                                }
                            }

                            break;
                    }

                    Sequence = Sequence.Next;
                }
                while (!(Sequence == null));
            }
        }
    }

    public override void ShowShadow(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        ImageSequence Sequence;
        int ImageIndex;
        int Xhp = default;
        int Yhp = default;
        if (FHitPoints > 0)
        {
            HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            Sequence = FResource.ImageSequenceList.GetImageSequence(FRace + 60);
            if (Sequence != null)
            {
                do
                {
                    ImageIndex = Sequence.GetFrameIndex(AoWCombatMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                    if (ImageIndex != -1)
                    {
                        Sequence.ImageLibrary[ImageIndex].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                    }

                    Sequence = Sequence.Next;
                }
                while (!(Sequence == null));
            }
        }
    }

    public new partial class Resource : CombatStructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // House.pas:145
        public new const int ClassID = 2229523;

        // House.pas:151
        public override int ResourceUserClassID() => 2229522;

        // House.pas:148
        public override string ResourceEditName() => "Combat House";

    }
}
