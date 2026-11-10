// Translated from AoWHex.pas.
using System.Runtime.InteropServices;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

// inherited transition images [0.2], side flags
public partial class AoWHexagon : Hexagon
{
    public AoWHexagon(ECustomNode owner) : base(owner)
    { }

    // AoWHex.pas:119
    public override void ChangeTerrain(in HSTerrainInfo Msg)
    {
        Aow.Game.World.MapObjectResource R;
        if (Msg.HN == 0 && GetTerrain(0) != Msg.TerrainType)
        {
            var classID = FResource.EClass.ClassID;
            R = Aow.Game.Engine.AoWHSSet.Current.HexagonControl.GetRndResource(Msg.TerrainType, classID);
            if (R != null)
            {
                SetResource(R);
                UpdateTransitions();
                Aow.Game.World.Map.AoWHSMap.Current.InvalidateStaticScene(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ((MapField)Owner).Lhx);
            }
            else
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                FResource.Engine.HSSet.HexagonControl.Place(Aow.Game.World.Map.AoWHSMap.Current.MapContainer, xhx, yhx, lhx, Msg.TerrainType);
            }
        }
    }

    // AoWHex.pas:139
    public override void TerrainChanged(in HSTerrainInfo Msg)
    {
        if (Set<byte>.WithBounds(0, 255).With(0).With(6).With(14).Contains((byte)Msg.TerrainType))
        {
            if (Msg.TerrainType != 14)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                FResource.Engine.HSSet.HexagonControl.Place(Aow.Game.World.Map.AoWHSMap.Current.MapContainer, xhx, yhx, lhx, Msg.TerrainType);
            }
            else
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                FResource.Engine.HSSet.HexagonControl.Place(Aow.Game.World.Map.AoWHSMap.Current.MapContainer, xhx, yhx, lhx, 0);
            }
        }
    }

    // AoWHex.pas:109
    public override void NeighbourTerrainChanged(int HN, in HSTerrainInfo Info)
    {
        if (HN == 0)
        {
            switch (Info.HN)
            {
                case 6:
                    UpdateTransition(0);
                    break;
                case 1:
                    UpdateTransition(1);
                    break;
                case 2:
                    UpdateTransition(2);
                    break;
            }
        }
    }

    // AoWHex.pas:150
    public override void UpdateTransition(byte Side)
    {
        bool Shifted;
        int NXhx = default;
        int NYhx = default;
        MapLevel MapLevel;
        int NeighbourTerrain;
        int OwnTerrain;
        CustomImageLibrary IL;
        int Image;
        Shifted = false;
        FTransitions[Side] = 255;
        FTransitionSides = (byte)(FTransitionSides & Aow.Game.World.Hexes.AoWHexagon.SideClear[Side]);
        MapLevel = (global::Aow.Game.World.Map.MapLevel)Owner.Owner;
        Aow.Game.World.Map.HexGeometry.HNtoHX(Aow.Game.World.Hexes.AoWHexagon.SideHN[Side], ref NXhx, ref NYhx);
        Aow.Game.World.Map.HexGeometry.TranslateHX(0, 0, ((MapField)Owner).Xhx, ((MapField)Owner).Yhx, NXhx, NYhx, ref NXhx, ref NYhx);
        if (NXhx >= 0 && NYhx >= 0 && MapLevel.Width > NXhx && MapLevel.Height > NYhx)
        {
            NeighbourTerrain = MapLevel.GetField(NXhx, NYhx).TerrainType;
            OwnTerrain = GetTerrain(0);
            if (OwnTerrain != NeighbourTerrain && OwnTerrain >= 0 && NeighbourTerrain >= 0 && NeighbourTerrain != 8 && NeighbourTerrain != 7 && NeighbourTerrain != 15 && OwnTerrain != 15)
            {
                if (OwnTerrain == 9)
                {
                    if (NeighbourTerrain == 10 || NeighbourTerrain == 13)
                    {
                        var random = Random(3);
                        FTransitions[Side] = (byte)(random * 3 + (Side + 182));
                    }
                    else
                    {
                        if (NeighbourTerrain != 8 && NeighbourTerrain != 7)
                        {
                            FTransitions[Side] = (byte)(Side + 147);
                            Shifted = true;
                        }
                    }
                }
                else
                {
                    if (NeighbourTerrain == 9)
                    {
                        if (OwnTerrain == 10 || OwnTerrain == 13)
                        {
                            var random = Random(3);
                            FTransitions[Side] = (byte)(random * 3 + (Side + 182));
                        }
                        else
                        {
                            if (OwnTerrain != 8 && OwnTerrain != 7)
                            {
                                FTransitions[Side] = (byte)(Side + 144);
                            }
                        }
                    }
                    else
                    {
                        if (OwnTerrain == 13)
                        {
                            if (NeighbourTerrain == 10)
                            {
                                FTransitions[Side] = (byte)(Side + 208);
                            }
                            else
                            {
                                FTransitions[Side] = (byte)(Side + 217);
                                Shifted = true;
                            }
                        }
                        else
                        {
                            if (NeighbourTerrain == 13)
                            {
                                if (OwnTerrain == 10)
                                {
                                    FTransitions[Side] = (byte)(Side + 211);
                                    Shifted = true;
                                }
                                else
                                {
                                    FTransitions[Side] = (byte)(Side + 214);
                                }
                            }
                            else
                            {
                                if (OwnTerrain == 10)
                                {
                                    FTransitions[Side] = (byte)(Side + 163);
                                    Shifted = true;
                                }
                                else
                                {
                                    if (NeighbourTerrain == 10)
                                    {
                                        FTransitions[Side] = (byte)(Side + 160);
                                    }
                                    else
                                    {
                                        if (NeighbourTerrain != 0 && NeighbourTerrain != 6)
                                        {
                                            if (OwnTerrain == 6)
                                            {
                                                // { Original unreachable branch retained: the outer condition excludes terrain 0. }
                                                if (NeighbourTerrain == 0)
                                                {
                                                    FTransitions[Side] = (byte)(Side + 96);
                                                }
                                                else
                                                {
                                                    FTransitions[Side] = (byte)((byte)(NeighbourTerrain << 4) + 3 + Side);
                                                    Shifted = true;
                                                }
                                            }
                                            else
                                            {
                                                if (OwnTerrain == 0)
                                                {
                                                    FTransitions[Side] = (byte)(Side + 3);
                                                    Shifted = true;
                                                }
                                                else
                                                {
                                                    var random = Random(3);
                                                    FTransitions[Side] = (byte)(random * 3 + ((byte)(OwnTerrain << 4) + Side + 6));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (FTransitions[Side] != 255)
        {
            IL = ((HexagonResource)FResource).FTransitionLibrary;
            Image = FTransitions[Side];
            if (Image < IL.Count)
            {
                if (IL[Image] != null)
                {
                    FTransitionSides = (byte)(FTransitionSides + (1 << Side));
                    if (Shifted)
                    {
                        FTransitionSides = (byte)(FTransitionSides + (1 << (Side + 3)));
                    }
                }
            }
        }
    }

    private static global::Aow.Game.Engine.Buffers.FixedArray_byte_3 SideHN = global::Aow.Game.Engine.Buffers.FixedArray_byte_3.From((global::System.ReadOnlySpan<byte>)new byte[3] { 6, 1, 2 });
    private static global::Aow.Game.Engine.Buffers.FixedArray_byte_3 SideClear = global::Aow.Game.Engine.Buffers.FixedArray_byte_3.From((global::System.ReadOnlySpan<byte>)new byte[3] { 246, 237, 219 });

    public new partial class Resource : HexagonResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // AoWHex.pas:254
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            if (Msg.MsgID == 65556)
            {
                var terrain = FTerrainList.Terrain[0];
                if (Set<byte>.WithBounds(0, 255).With(0).With(6).Contains((byte)terrain))
                {
                    return;
                }
            }

            base.MsgProc(ref Msg, Sender);
        }
    }
}

public partial class AoWBorderHexagon : BorderHexagon
{
    public AoWBorderHexagon(ECustomNode owner) : base(owner)
    { }

    // AoWHex.pas:261
    public override void UpdateMapField(in HSMapFieldInfo Info)
    {
        if (Info.HN == 0 && Aow.Game.World.Map.AoWHSMap.Current != null && Aow.Game.World.Map.AoWHSMap.Current.FDay == 0)
        {
            ((AoWMapField)(object)Info.MapField).FFieldFlags = ((AoWMapField)(object)Info.MapField).FFieldFlags & -4096;
        }
    }
}

// FAnimationFrame, FAnimationIndex, FShoreTerrain[1.6]
public partial class AoWWaterHexagon : LowerIsometricHexagon
{
    public AoWWaterHexagon(ECustomNode owner) : base(owner)
    {
        FAnimationFrame = (sbyte)(-1 - Random(20));
        var random = Random(4);
        this.FSides = (byte)(this.FSides | (random << 6));
    }

    // AoWHex.pas:275
    public new const int ClassID = 132925;

    public sbyte FAnimationFrame;
    public byte FAnimationIndex;
    public FixedArray_byte_6 FShoreTerrain;

    // AoWHex.pas:268

    // AoWHex.pas:48
    // AoWHex.pas:315. The base texture includes an owner-address phase.
    public override void Show(int xSurface, int ySurface, in SurfaceDesc surface)
    {
        var field = (MapField)Owner;
        int x = 0, y = 0;
        Aow.Game.World.Map.HexGeometry.HXtoHP(field.Xhx, field.Yhx, ref x, ref y);
        x -= xSurface + 8;
        y -= ySurface;
        var library = FResource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        int phase = FrameFixtureInputs.WaterBasePhase(Owner);
        library[((phase + field.Xhx + field.Yhx * 2 + (FSides >> 6)) & 15) + 140].Show(x, y, surface);
        x -= 6;
        y -= 9;
        if ((FSides & 1) != 0) library[36].Show(x, y, surface);
        if ((FSides & 2) != 0) library[37].Show(x, y, surface);
        if ((FSides & 32) != 0) library[35].Show(x, y, surface);
    }

    public override void UpdateTransitions() => UpdateTransitionCore(typeof(LowerIsometricHexagon), true,
        (hn, terrain) => FShoreTerrain[hn - 1] = (byte)terrain);

    public new partial class Resource : Aow.Game.World.Hexes.LowerIsometricHexagon.Resource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            this.FDynamicLevel = 23;
        }

        // AoWHex.pas:452
        public new const int ClassID = 132926;

        // AoWHex.pas:446

        // AoWHex.pas:458
        public override int ResourceUserClassID() => 132925;

        // AoWHex.pas:58

        // AoWHex.pas:455
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Hexes.AoWWaterHexagon.Resource.WaterHexagonRStr);

        private static readonly string WaterHexagonRStr = "Water/Ice Hexagon";

    }
}

[StructLayout(LayoutKind.Explicit, Size = 28)]
public struct AoWFieldFlags
{
    [FieldOffset(0)]
    public FixedArray_byte_24 Reserved;
    [FieldOffset(24)]
    public int Flags;
}

public struct IsoFields
{
    public FixedArray_byte_16 Reserved;
    public DynamicIsometricHexagon Dynamic;
    public byte OpenSides;
    public byte Sides;
}

[StructLayout(LayoutKind.Explicit, Size = 53)]
public struct IsoResourceFields
{
    [FieldOffset(0)]
    public FixedArray_byte_52 Reserved;
    [FieldOffset(52)]
    public byte DynamicLevel;
}

[StructLayout(LayoutKind.Explicit, Size = 17)]
public struct WaterViewFlags
{
    [FieldOffset(0)]
    public FixedArray_byte_16 Reserved;
    [FieldOffset(16)]
    public byte Flags;
}

[StructLayout(LayoutKind.Explicit, Size = 20)]
public struct HexagonFields
{
    [FieldOffset(0)]
    public FixedArray_byte_16 Reserved;
    [FieldOffset(16)]
    public FixedArray_byte_3 Transitions;
    [FieldOffset(19)]
    public byte Sides;
}

public struct HexagonResourceFields
{
    public FixedArray_byte_52 Reserved;
    public CustomImageLibrary TransitionLibrary;
}
