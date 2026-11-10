// Translated from House.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

public partial class THouse : TCombatStructure
{
    public THouse(TECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 2229522;

    public int FRace;
    public int FFlagID;

    // House.pas:45
    public override void Activate()
    {
        TPlayerStructure Structure;
        base.Activate();
        if (!Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
        {
            Structure = (TPlayerStructure)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetMapLevel(Aow.Game.Combat.CombatPlacement.CombatLLoc).GetField(Aow.Game.Combat.CombatPlacement.CombatXLoc, Aow.Game.Combat.CombatPlacement.CombatYLoc).FindNoneTransparentHS(typeof(TPlayerStructure));
            if (Structure != null)
            {
                if (Structure is TCity)
                {
                    FRace = ((TCity)Structure).FRace;
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
                    FFlagID = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer).FPlayerRace;
                }

                if (FFlagID == -1)
                {
                    FFlagID = 32;
                }
            }
        }
    }

    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        TImageSequence Sequence;
        int ImageIndex;
        int Xhp = default;
        int Yhp = default;
        int TerrainOffset;
        Aow.Game.World.Map.HexGeometry.HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
        XhpSurf = Xhp - XhpSurf;
        YhpSurf = Yhp - YhpSurf;
        if (FHitPoints == 0)
        {
            Sequence = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FImageSequences.GetImageSequence(50);
            var yhx = GetYhx();
            var xhx = GetXhx();
            switch (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.GetMapLevel(0).GetField(xhx, yhx).TerrainType)
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
            switch (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.GetMapLevel(0).GetField(xhx, yhx).TerrainType)
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
                            ImageIndex = Sequence.GetFrameIndex(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                            if (ImageIndex != -1)
                            {
                                if (!Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
                                {
                                    var xhx2 = GetXhx();
                                    var yhx2 = GetYhx();
                                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FFlagControl.ShowStructureFlagEx(XhpSurf + 21 + Sequence.dXshift, YhpSurf + Sequence.dYshift + 16, xhx2 + Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame + yhx2 * 100, (byte)FFlagID, SurfaceDesc, false);
                                }
                            }

                            break;
                        default:
                            {
                                ImageIndex = Sequence.GetFrameIndex(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.Frame, FrameFixtureInputs.AnimationOffset(this));
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

    public override void ShowShadow(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        TImageSequence Sequence;
        int ImageIndex;
        int Xhp = default;
        int Yhp = default;
        if (FHitPoints > 0)
        {
            Aow.Game.World.Map.HexGeometry.HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            Sequence = FResource.ImageSequenceList.GetImageSequence(FRace + 60);
            if (Sequence != null)
            {
                do
                {
                    ImageIndex = Sequence.GetFrameIndex(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.Frame, FrameFixtureInputs.AnimationOffset(this));
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
}

public partial class THouseResource : TCombatStructureResource
{
    public THouseResource(TECustomNode owner) : base(owner)
    { }

    // House.pas:145
    public new const int ClassID = 2229523;

    // House.pas:151
    public override int ResourceUserClassID() => 2229522;

    // House.pas:148
    public override string ResourceEditName() => "Combat House";

}
