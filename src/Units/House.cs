// Translated from House.pas.
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.City;
using Aow.Units.CombatStructure;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using Aow.Units.ImageLib;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWTC.AoWTCUnit;
using static Aow.Units.HSEngine.HSEngineUnit;

namespace Aow.Units.House;

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
        if (!AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
        {
            Structure = (TPlayerStructure)AoWHSMap.MapContainer.GetMapLevel(CombatLLoc).GetField(CombatXLoc, CombatYLoc).FindNoneTransparentHS(typeof(TPlayerStructure));
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
                    FFlagID = AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer).FPlayerRace;
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
        HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
        XhpSurf = Xhp - XhpSurf;
        YhpSurf = Yhp - YhpSurf;
        if (FHitPoints == 0)
        {
            Sequence = AoWHSSet.FImageSequences.GetImageSequence(50);
            var yhx = GetYhx();
            var xhx = GetXhx();
            switch (AoWCombatMap.MapContainer.GetMapLevel(0).GetField(xhx, yhx).TerrainType)
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
                    ImageIndex = Sequence.GetUnloopedFrameIndex(InstanceId % 3 + TerrainOffset * 3);
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
            switch (AoWCombatMap.MapContainer.GetMapLevel(0).GetField(xhx, yhx).TerrainType)
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
                            ImageIndex = Sequence.GetFrameIndex(AoWCombatMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                            if (ImageIndex != -1)
                            {
                                if (!AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
                                {
                                    var xhx2 = GetXhx();
                                    var yhx2 = GetYhx();
                                    AoWHSSet.FFlagControl.ShowStructureFlagEx(XhpSurf + 21 + Sequence.dXshift, YhpSurf + Sequence.dYshift + 16, xhx2 + AoWHSMap.Frame + yhx2 * 100, (byte)FFlagID, SurfaceDesc, false);
                                }
                            }

                            break;
                        default:
                            {
                                ImageIndex = Sequence.GetFrameIndex(AoWCombatMap.Frame, FrameFixtureInputs.AnimationOffset(this));
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
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            Sequence = FResource.ImageSequenceList.GetImageSequence(FRace + 60);
            if (Sequence != null)
            {
                do
                {
                    ImageIndex = Sequence.GetFrameIndex(AoWCombatMap.Frame, FrameFixtureInputs.AnimationOffset(this));
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

public static partial class HouseUnit
{
    // House.pas:154
    public static void RegisterHouse(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<THouseResource>(), EClass.Of<THouse>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
