// Translated from CombatHex.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Game.Combat.AoWTCUnit;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.World.Map.HSEngineUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

public partial class TCombatHexagon : TAbstractHexagon
{
    public TCombatHexagon(TECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 2229504;

    public byte FImageVariant;
    public FixedArray_byte_3 FTransitions;

    // CombatHex.pas:174
    public override void TerrainChanged(in THSTerrainInfo Msg)
    {
        if (TSet<byte>.WithBounds(0, 255).With(0).With(6).With(14).Contains((byte)Msg.TerrainType))
        {
            if (Msg.TerrainType != 14)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                ((TAoWHSSet)FResource.Engine.HSSet).FCombatHexagons.Place(AoWCombatMap.MapContainer, xhx, yhx, lhx, Msg.TerrainType);
            }
            else
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                ((TAoWHSSet)FResource.Engine.HSSet).FCombatHexagons.Place(AoWCombatMap.MapContainer, xhx, yhx, lhx, 0);
            }
        }
    }

    // CombatHex.pas:138
    public override void NeighbourTerrainChanged(int HN, in THSTerrainInfo Info)
    {
        if (HN == 0)
        {
            switch (Info.HN)
            {
                case 6:
                    UpdateTransition(1);
                    break;
                case 1:
                    UpdateTransition(2);
                    break;
                case 2:
                    UpdateTransition(3);
                    break;
            }
        }
    }

    // CombatHex.pas:148
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        TCustomImageLibrary Images;
        if (((TMapField)Owner).OverlayType != 4)
        {
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            Xhp = Xhp - XhpSurf - 8;
            Yhp = Yhp - YhpSurf;
            Images = FResource.ImageSequenceList.GetImageSequence(0).ImageLibrary;
            Images[FImageVariant].Show(Xhp, Yhp, SurfaceDesc);
            Images = ((TCombatHexagonResource)FResource).FTransitionLibrary;
            for (int Side = 1; Side <= 3; Side++)
            {
                if (FTransitions[Side - 1] != 255)
                {
                    Images[FTransitions[Side - 1]].Show(Xhp - 8, Yhp - 16, SurfaceDesc);
                }
            }
        }
    }

    // CombatHex.pas:168
    public override void Activate()
    {
        base.Activate();
        FImageVariant = (byte)Random(12);
    }

    // CombatHex.pas:73
    public override Aow.Game.World.TMapObjectResourceList GetResourceList(TEngine AEngine) => AoWHSSet.FCombatResources;

    // CombatHex.pas:134
    public override void UpdateTransitions()
    {
        for (int Side = 1; Side <= 3; Side++)
        {
            UpdateTransition((byte)Side);
        }
    }

    // CombatHex.pas:76
    public void UpdateTransition(byte Side)
    {
        TMapLevel MapLevel;
        int ImageIndex;
        sbyte NeighbourTerrain;
        sbyte TerrainType;
        int X = default;
        int Y = default;
        MapLevel = (TMapLevel)Owner.Owner;
        CenterHNtoHX(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, CombatHexUnit.TransitionNeighbours[Side - 1], ref X, ref Y);
        if (X >= 0 && Y >= 0 && X < MapLevel.Width && Y < MapLevel.Height)
        {
            NeighbourTerrain = MapLevel.GetField(X, Y).TerrainType;
            TerrainType = FResource.TerrainList.GetTerrain(0);
            ImageIndex = -1;
            if (NeighbourTerrain != TerrainType && NeighbourTerrain != 7)
            {
                if (TerrainType == 0)
                {
                    if (NeighbourTerrain == 6)
                    {
                        ImageIndex = 10;
                    }
                    else
                    {
                        ImageIndex = NeighbourTerrain + 4;
                    }
                }
                else
                {
                    if (TerrainType == 6)
                    {
                        if (NeighbourTerrain == 0)
                        {
                            ImageIndex = 16;
                        }
                        else
                        {
                            ImageIndex = NeighbourTerrain + 10;
                        }
                    }
                    else
                    {
                        if (TerrainType == 12)
                        {
                            ImageIndex = 17;
                        }
                        else
                        {
                            if (TerrainType == 10)
                            {
                                if (NeighbourTerrain == 12)
                                {
                                    ImageIndex = 18;
                                }
                                else
                                {
                                    if (NeighbourTerrain == 13)
                                    {
                                        ImageIndex = 19;
                                    }
                                    else
                                    {
                                        if (NeighbourTerrain == 9)
                                        {
                                            ImageIndex = 20;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (TerrainType == 13)
                                {
                                    if (NeighbourTerrain == 12)
                                    {
                                        ImageIndex = 21;
                                    }
                                    else
                                    {
                                        if (NeighbourTerrain == 10)
                                        {
                                            ImageIndex = 22;
                                        }
                                        else
                                        {
                                            if (NeighbourTerrain == 9)
                                            {
                                                ImageIndex = 23;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (TerrainType == 9)
                                    {
                                        if (NeighbourTerrain == 12)
                                        {
                                            ImageIndex = 24;
                                        }
                                        else
                                        {
                                            ImageIndex = 25;
                                        }
                                    }
                                    else
                                    {
                                        ImageIndex = TerrainType - 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (ImageIndex != -1)
            {
                ImageIndex = Random(2) * 3 + ImageIndex * 6 + (Side - 1);
                if (((TCombatHexagonResource)FResource).FTransitionLibrary.GetSafe(ImageIndex) != null)
                {
                    FTransitions[Side - 1] = (byte)ImageIndex;
                }
                else
                {
                    FTransitions[Side - 1] = 255;
                }
            }
            else
            {
                FTransitions[Side - 1] = 255;
            }
        }
    }

}

public partial class TCombatHexagonResource : TAbstractHexagonResource
{
    public TCombatHexagonResource(TECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 2229505;

    public TCustomImageLibrary FTransitionLibrary;

    // CombatHex.pas:194
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        bool condition = Msg.MsgID != 65556;
        if (!condition)
        {
            var terrain = TerrainList.GetTerrain(0);
            condition = !TSet<byte>.WithBounds(0, 255).With(0).With(6).Contains((byte)terrain);
        }

        if (condition)
        {
            base.MsgProc(ref Msg, Sender);
        }

        if (Msg.MsgID == 65621)
        {
            Msg.Result = 5;
        }

        if (Msg.MsgID == 536870920)
        {
            FTransitionLibrary = AoWHSSet.ImageLibraryManager.LinkToIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), "TCombat\\Base_Hex\\CTransHx.ILB");
        }
    }

    // CombatHex.pas:207
    public override Aow.Game.Draw.Images.TLibraryImage GetCellImage()
    {
        Aow.Game.Draw.Images.TLibraryImage Result = null;
        if (ImageSequenceList != null && ImageSequenceList.Count > 0 && ImageSequenceList.GetImageSequence(0) != null)
        {
            Result = ImageSequenceList.GetImageSequence(0).ImageLibrary[10];
        }

        if (Result == null)
        {
            Result = base.GetCellImage();
        }

        return Result;
    }

    // CombatHex.pas:204
    public override int ResourceUserClassID() => 2229504;

    // CombatHex.pas:191
    public override THexagonControl GetHexagonControl() => ((TAoWHSSet)Engine.HSSet).FCombatHexagons;

    // CombatHex.pas:188
    public override string ResourceEditName() => "Combat Hexagon";

}

public partial class TWaterCombatHexagon : TAoWWaterHexagon
{
    public TWaterCombatHexagon(TECustomNode owner) : base(owner)
    { }

    // CombatHex.pas:219
    public new const int ClassID = 2229655;

    // CombatHex.pas:216
    public override Aow.Game.World.TMapObjectResourceList GetResourceList(TEngine AEngine) => AoWHSSet.FCombatResources;

}

public partial class TWaterCombatHexagonResource : TAoWWaterHexagonResource
{
    public TWaterCombatHexagonResource(TECustomNode owner) : base(owner)
    { }

    // CombatHex.pas:222
    public new const int ClassID = 2229656;

    // CombatHex.pas:228
    public override int ResourceUserClassID() => 2229655;

    // CombatHex.pas:231
    public override THexagonControl GetHexagonControl() => ((TAoWHSSet)Engine.HSSet).FCombatHexagons;

    // CombatHex.pas:225
    public override string ResourceEditName() => "Combat Water/Ice hex";

}

public static partial class CombatHexUnit
{
    // CombatHex.pas:234
    public static void RegisterCombatHexagon(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TCombatHexagonResource>(), EClass.Of<TCombatHexagon>(), EClass.Of<TWaterCombatHexagon>(), EClass.Of<TWaterCombatHexagonResource>()]);
    }

    public static FixedArray_byte_3 TransitionNeighbours = FixedArray_byte_3.From([6, 1, 2]);

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
