// Translated from AoWHex.pas.
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
using Aow.Units.AoWReg;
using static Aow.Units.AoWReg.AoWReg;

namespace Aow.Units.AoWHex
{
    // inherited transition images [0.2], side flags
    public partial class TAoWHexagon : Aow.Units.Hexagon.THexagon
    {
        public TAoWHexagon(TECustomNode owner) : base(owner)
        {}

        // AoWHex.pas:119
        public override void ChangeTerrain(in Aow.Units.HSEngine.THSTerrainInfo Msg)
        {
            Aow.Units.HSEngine.TMapObjectResource R = default;
            bool bool__1 = (Msg.HN == 0);
            if (bool__1)
            {
                var call__2 = GetTerrain(0);
                bool__1 = (call__2 != Msg.TerrainType);
            }

            if (bool__1)
            {
                var call__3 = FResource.EClass.ClassID;
                var call__4 = Aow.Units.AoWE.AoWE.AoWHSSet.HexagonControl.GetRndResource(Msg.TerrainType, call__3);
                R = call__4;
                if (R != null)
                {
                    SetResource(R);
                    UpdateTransitions();
                    Aow.Units.AoWE.AoWE.AoWHSMap.InvalidateStaticScene(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ((Aow.Units.HSEngine.TMapField)Owner).Lhx);
                }
                else
                {
                    var call__5 = GetLhx();
                    var call__6 = GetYhx();
                    var call__7 = GetXhx();
                    FResource.Engine.HSSet.HexagonControl.Place(Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer, unchecked((sbyte)call__7), unchecked((sbyte)call__6), unchecked((sbyte)call__5), unchecked((sbyte)Msg.TerrainType));
                }
            }
        }

        // AoWHex.pas:139
        public override void TerrainChanged(in Aow.Units.HSEngine.THSTerrainInfo Msg)
        {
            if (TSet<byte>.WithBounds(0, 255).With(0).With(6).With(14).Contains((byte)Msg.TerrainType))
            {
                if (unchecked((short)Msg.TerrainType) != 14)
                {
                    var call__8 = GetLhx();
                    var call__9 = GetYhx();
                    var call__10 = GetXhx();
                    FResource.Engine.HSSet.HexagonControl.Place(Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer, unchecked((sbyte)call__10), unchecked((sbyte)call__9), unchecked((sbyte)call__8), unchecked((sbyte)Msg.TerrainType));
                }
                else
                {
                    var call__11 = GetLhx();
                    var call__12 = GetYhx();
                    var call__13 = GetXhx();
                    FResource.Engine.HSSet.HexagonControl.Place(Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer, unchecked((sbyte)call__13), unchecked((sbyte)call__12), unchecked((sbyte)call__11), 0);
                }
            }
        }

        // AoWHex.pas:109
        public override void NeighbourTerrainChanged(int HN, in Aow.Units.HSEngine.THSTerrainInfo Info)
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
            bool Shifted = default;
            int NXhx = default;
            int NYhx = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            int NeighbourTerrain = default;
            int OwnTerrain = default;
            Aow.Units.ImageLib.TCustomImageLibrary IL = default;
            int Image = default;
            Shifted = false;
            FTransitions[Side] = 255;
            FTransitionSides = unchecked((byte)(FTransitionSides & Aow.Units.AoWHex.AoWHex.SideClear[Side]));
            MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            Aow.Units.HSEngine.HSEngine.HNtoHX(Aow.Units.AoWHex.AoWHex.SideHN[Side], ref NXhx, ref NYhx);
            Aow.Units.HSEngine.HSEngine.TranslateHX(0, 0, ((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, NXhx, NYhx, ref NXhx, ref NYhx);
            if ((((NXhx >= 0) && (NYhx >= 0)) && (MapLevel.Width > NXhx)) && (MapLevel.Height > NYhx))
            {
                var call__14 = MapLevel.GetField(NXhx, NYhx);
                NeighbourTerrain = call__14.TerrainType;
                OwnTerrain = GetTerrain(0);
                if (((((((OwnTerrain != NeighbourTerrain) && (OwnTerrain >= 0)) && (NeighbourTerrain >= 0)) && (NeighbourTerrain != 8)) && (NeighbourTerrain != 7)) && (NeighbourTerrain != 15)) && (OwnTerrain != 15))
                {
                    if (OwnTerrain == 9)
                    {
                        if ((NeighbourTerrain == 10) || (NeighbourTerrain == 13))
                        {
                            var call__15 = Aow.Rtl.Rtl.Random(3);
                            FTransitions[Side] = unchecked((byte)((call__15 * 3) + (Side + 182)));
                        }
                        else
                        {
                            if ((NeighbourTerrain != 8) && (NeighbourTerrain != 7))
                            {
                                FTransitions[Side] = unchecked((byte)(Side + 147));
                                Shifted = true;
                            }
                        }
                    }
                    else
                    {
                        if (NeighbourTerrain == 9)
                        {
                            if ((OwnTerrain == 10) || (OwnTerrain == 13))
                            {
                                var call__16 = Aow.Rtl.Rtl.Random(3);
                                FTransitions[Side] = unchecked((byte)((call__16 * 3) + (Side + 182)));
                            }
                            else
                            {
                                if ((OwnTerrain != 8) && (OwnTerrain != 7))
                                {
                                    FTransitions[Side] = unchecked((byte)(Side + 144));
                                }
                            }
                        }
                        else
                        {
                            if (OwnTerrain == 13)
                            {
                                if (NeighbourTerrain == 10)
                                {
                                    FTransitions[Side] = unchecked((byte)(Side + 208));
                                }
                                else
                                {
                                    FTransitions[Side] = unchecked((byte)(Side + 217));
                                    Shifted = true;
                                }
                            }
                            else
                            {
                                if (NeighbourTerrain == 13)
                                {
                                    if (OwnTerrain == 10)
                                    {
                                        FTransitions[Side] = unchecked((byte)(Side + 211));
                                        Shifted = true;
                                    }
                                    else
                                    {
                                        FTransitions[Side] = unchecked((byte)(Side + 214));
                                    }
                                }
                                else
                                {
                                    if (OwnTerrain == 10)
                                    {
                                        FTransitions[Side] = unchecked((byte)(Side + 163));
                                        Shifted = true;
                                    }
                                    else
                                    {
                                        if (NeighbourTerrain == 10)
                                        {
                                            FTransitions[Side] = unchecked((byte)(Side + 160));
                                        }
                                        else
                                        {
                                            if ((NeighbourTerrain != 0) && (NeighbourTerrain != 6))
                                            {
                                                if (OwnTerrain == 6)
                                                {
                                                    // { Original unreachable branch retained: the outer condition excludes terrain 0. }
                                                    if (NeighbourTerrain == 0)
                                                    {
                                                        FTransitions[Side] = unchecked((byte)(Side + 96));
                                                    }
                                                    else
                                                    {
                                                        FTransitions[Side] = unchecked((byte)((unchecked((byte)(NeighbourTerrain << 4)) + 3) + Side));
                                                        Shifted = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (OwnTerrain == 0)
                                                    {
                                                        FTransitions[Side] = unchecked((byte)(Side + 3));
                                                        Shifted = true;
                                                    }
                                                    else
                                                    {
                                                        var call__17 = Aow.Rtl.Rtl.Random(3);
                                                        FTransitions[Side] = unchecked((byte)((call__17 * 3) + ((unchecked((byte)(OwnTerrain << 4)) + Side) + 6)));
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
                IL = ((Aow.Units.AoWHex.TAoWHexagonResource)FResource).FTransitionLibrary;
                Image = FTransitions[Side];
                if (Image < IL.Count)
                {
                    var call__18 = IL[Image];
                    if (call__18 != null)
                    {
                        FTransitionSides = (byte)(((int)FTransitionSides) + (1 << Side));
                        if (Shifted)
                        {
                            FTransitionSides = (byte)(((int)FTransitionSides) + (1 << (Side + 3)));
                        }
                    }
                }
            }
        }
    }

    public partial class TAoWHexagonResource : Aow.Units.Hexagon.THexagonResource
    {
        public TAoWHexagonResource(TECustomNode owner) : base(owner)
        {}

        // AoWHex.pas:254
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            if (Msg.MsgID == 65556)
            {
                var call__19 = FTerrainList.Terrain[0];
                if (TSet<byte>.WithBounds(0, 255).With(0).With(6).Contains((byte)call__19))
                {
                    return;
                }
            }

            base.MsgProc(ref Msg, Sender);
        }
    }

    public partial class TAoWBorderHexagon : Aow.Units.BorderHx.TBorderHexagon
    {
        public TAoWBorderHexagon(TECustomNode owner) : base(owner)
        {}

        // AoWHex.pas:261
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            if (((Info.HN == 0) && (Aow.Units.AoWE.AoWE.AoWHSMap != null)) && (Aow.Units.AoWE.AoWE.AoWHSMap.FDay == 0))
            {
                ((Aow.Units.AoWE.TAoWMapField)((System.Object)Info.MapField)).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)((System.Object)Info.MapField)).FFieldFlags & -4096;
            }
        }
    }

    // FAnimationFrame, FAnimationIndex, FShoreTerrain[1.6]
    public partial class TAoWWaterHexagon : Aow.Units.IsoHex.TLowerIsometricHexagon
    {
        public TAoWWaterHexagon(TECustomNode owner) : base(owner)
        {
            var call__20 = Aow.Rtl.Rtl.Random(20);
            FAnimationFrame = unchecked((sbyte)(-1 - call__20));
            var call__21 = Aow.Rtl.Rtl.Random(4);
            this.FSides = unchecked((byte)(this.FSides | (call__21 << 6)));
        }

        // AoWHex.pas:275
        public new const int ClassID = 132925;

        public sbyte FAnimationFrame;
        public byte FAnimationIndex;
        public Aow.Units.FixedArray_byte_6 FShoreTerrain;
        // AoWHex.pas:268


        // AoWHex.pas:48


    }

    public partial class TAoWWaterHexagonResource : Aow.Units.IsoHex.TLowerIsometricHexagonResource
    {
        public TAoWWaterHexagonResource(TECustomNode owner) : base(owner)
        {
            this.FDynamicLevel = 23;
        }

        // AoWHex.pas:452
        public new const int ClassID = 132926;

        // AoWHex.pas:446


        // AoWHex.pas:458
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132925;
            return Result;
        }

        // AoWHex.pas:58


        // AoWHex.pas:455
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__22 = Aow.Units.AoWE.AoWE.WaterHexagonRStr;
            var call__23 = Aow.Units.AoWE.AoWE.TranslateRStr(call__22);
            Result = call__23;
            return Result;
        }

    }

    [StructLayout(LayoutKind.Explicit, Size = 28)]
    public struct TAoWFieldFlags
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_24 Reserved;
        [FieldOffset(24)]
        public int Flags;
    }

    public struct TIsoFields
    {
        public Aow.Units.FixedArray_byte_16 Reserved;
        public Aow.Units.IsoHex.TDynamicIsometricHexagon Dynamic;
        public byte OpenSides;
        public byte Sides;
    }

    [StructLayout(LayoutKind.Explicit, Size = 53)]
    public struct TIsoResourceFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_52 Reserved;
        [FieldOffset(52)]
        public byte DynamicLevel;
    }

    [StructLayout(LayoutKind.Explicit, Size = 17)]
    public struct TWaterViewFlags
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_16 Reserved;
        [FieldOffset(16)]
        public byte Flags;
    }

    [StructLayout(LayoutKind.Explicit, Size = 20)]
    public struct THexagonFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_16 Reserved;
        [FieldOffset(16)]
        public Aow.Units.FixedArray_byte_3 Transitions;
        [FieldOffset(19)]
        public byte Sides;
    }

    public struct THexagonResourceFields
    {
        public Aow.Units.FixedArray_byte_52 Reserved;
        public Aow.Units.ImageLib.TCustomImageLibrary TransitionLibrary;
    }

    public static partial class AoWHex
    {
        // AoWHex.pas:461
        public static void RegisterAoWHexagon(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.AoWHex.TAoWHexagon>(), Aow.Units.Engine.EClass.Of<Aow.Units.AoWHex.TAoWHexagonResource>()]);
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.AoWHex.TAoWBorderHexagon>(), Aow.Units.Engine.EClass.Of<Aow.Units.BorderHx.TBorderHexagonResource>()]);
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.AoWHex.TAoWWaterHexagon>(), Aow.Units.Engine.EClass.Of<Aow.Units.AoWHex.TAoWWaterHexagonResource>()]);
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.IsoHex.TUpperIsometricHexagon>(), Aow.Units.Engine.EClass.Of<Aow.Units.IsoHex.TUpperIsometricHexagonResource>()]);
        }

        public static Aow.Units.FixedArray_byte_3 SideHN = Aow.Units.FixedArray_byte_3.From([6, 1, 2]);
        public static Aow.Units.FixedArray_byte_3 SideClear = Aow.Units.FixedArray_byte_3.From([246, 237, 219]);
        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
