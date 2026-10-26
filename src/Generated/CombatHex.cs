// Generated from CombatHex.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWHex;
using static Aow.Units.AoWHex.AoWHex;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.FleeLoc;
using static Aow.Units.FleeLoc.FleeLoc;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CombatHex
{
    // { TCombatHexagon (instance size 20, VMT 0040340C) }
    public partial class TCombatHexagon : Aow.Units.HSEngine.TAbstractHexagon
    {
        public byte FImageVariant;
        // { $10 }
        public Aow.Units.FixedArray_byte_3 FTransitions;
        // CombatHex.pas:174
        public override void TerrainChanged(in Aow.Units.HSEngine.THSTerrainInfo Msg)
        {
            if (TSet<byte>.WithBounds(0, 255).With(0).With(6).With(14).Contains((byte)Msg.TerrainType))
            {
                if (Msg.TerrainType != 14)
                {
                    var call__1 = GetLhx();
                    var call__2 = GetYhx();
                    var call__3 = GetXhx();
                    ((Aow.Units.AoWE.TAoWHSSet)FResource.Engine.HSSet).FCombatHexagons.Place(Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer, unchecked((sbyte)call__3), unchecked((sbyte)call__2), unchecked((sbyte)call__1), unchecked((sbyte)Msg.TerrainType));
                }
                else
                {
                    var call__4 = GetLhx();
                    var call__5 = GetYhx();
                    var call__6 = GetXhx();
                    ((Aow.Units.AoWE.TAoWHSSet)FResource.Engine.HSSet).FCombatHexagons.Place(Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer, unchecked((sbyte)call__6), unchecked((sbyte)call__5), unchecked((sbyte)call__4), 0);
                }
            }
        }

        // CombatHex.pas:138
        public override void NeighbourTerrainChanged(int HN, in Aow.Units.HSEngine.THSTerrainInfo Info)
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
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Units.ImageLib.TCustomImageLibrary Images = default;
            int Side = default;
            if (((Aow.Units.HSEngine.TMapField)Owner).OverlayType != 4)
            {
                Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
                Xhp = (Xhp - XhpSurf) - 8;
                Yhp = Yhp - YhpSurf;
                var call__7 = FResource.ImageSequenceList.GetImageSequence(0);
                Images = call__7.ImageLibrary;
                var call__8 = Images[FImageVariant];
                call__8.Show(Xhp, Yhp, SurfaceDesc);
                Images = ((Aow.Units.CombatHex.TCombatHexagonResource)FResource).FTransitionLibrary;
                {
                    Side = 1;
                    byte end__9 = 3;
                    for (; Side <= end__9; Side += 1)
                    {
                        if (FTransitions[Side - 1] != 255)
                        {
                            var call__10 = Images[FTransitions[Side - 1]];
                            call__10.Show(Xhp - 8, Yhp - 16, SurfaceDesc);
                        }
                    }
                }
            }
        }

        // CombatHex.pas:168
        public override void Activate()
        {
            base.Activate();
            var call__11 = Aow.Rtl.Rtl.Random(12);
            FImageVariant = unchecked((byte)call__11);
        }

        // CombatHex.pas:73
        public override Aow.Units.HSEngine.TMapObjectResourceList GetResourceList(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.HSEngine.TMapObjectResourceList Result = default;
            Result = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources;
            return Result;
        }

        // CombatHex.pas:134
        public override void UpdateTransitions()
        {
            int Side = default;
            {
                Side = 1;
                byte end__12 = 3;
                for (; Side <= end__12; Side += 1)
                {
                    UpdateTransition(unchecked((byte)Side));
                }
            }
        }

        // CombatHex.pas:76
        public void UpdateTransition(byte Side)
        {
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            int ImageIndex = default;
            sbyte NeighbourTerrain = default;
            sbyte TerrainType = default;
            int X = default;
            int Y = default;
            MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, Aow.Units.CombatHex.CombatHex.TransitionNeighbours[Side - 1], ref X, ref Y);
            if ((((X >= 0) && (Y >= 0)) && (X < MapLevel.Width)) && (Y < MapLevel.Height))
            {
                var call__13 = MapLevel.GetField(X, Y);
                NeighbourTerrain = unchecked((sbyte)call__13.TerrainType);
                var call__14 = FResource.TerrainList.GetTerrain(0);
                TerrainType = unchecked((sbyte)call__14);
                ImageIndex = -1;
                if ((NeighbourTerrain != TerrainType) && (NeighbourTerrain != 7))
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
                    ImageIndex = ((Aow.Rtl.Rtl.Random(2) * 3) + (ImageIndex * 6)) + (Side - 1);
                    var call__15 = ((Aow.Units.CombatHex.TCombatHexagonResource)FResource).FTransitionLibrary.GetSafe(ImageIndex);
                    if (call__15 != null)
                    {
                        FTransitions[Side - 1] = unchecked((byte)ImageIndex);
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

        public new partial class Meta : Aow.Units.HSEngine.TAbstractHexagon.Meta
        {
            // CombatHex.pas:70
            // { $11..$13 }
            public override int ClassID()
            {
                int Result = default;
                Result = 2229504;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatHex.TCombatHexagon => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.CombatHex.TCombatHexagon Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatHex.TCombatHexagon o = (Aow.Units.CombatHex.TCombatHexagon)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TCombatHexagonResource (instance size 56, VMT 004035A8) }
    public partial class TCombatHexagonResource : Aow.Units.HSEngine.TAbstractHexagonResource
    {
        public Aow.Units.ImageLib.TCustomImageLibrary FTransitionLibrary;
        // CombatHex.pas:194
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            bool bool__16 = (Msg.MsgID != 65556);
            if (!bool__16)
            {
                var call__17 = TerrainList.GetTerrain(0);
                bool__16 = !TSet<byte>.WithBounds(0, 255).With(0).With(6).Contains((byte)call__17);
            }

            if (bool__16)
            {
                base.MsgProc(ref Msg, Sender);
            }

            if (Msg.MsgID == 65621)
            {
                Msg.Result = 5;
            }

            if (Msg.MsgID == 536870920)
            {
                var call__18 = Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.LinkToIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TCombat\\Base_Hex\\CTransHx.ILB");
                FTransitionLibrary = call__18;
            }
        }

        // CombatHex.pas:207
        public override Aow.Units.ImageLib.TLibraryImage GetCellImage()
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            Result = null;
            bool bool__19 = (ImageSequenceList != null) && (ImageSequenceList.Count > 0);
            if (bool__19)
            {
                var call__20 = ImageSequenceList.GetImageSequence(0);
                bool__19 = (call__20 != null);
            }

            if (bool__19)
            {
                var call__21 = ImageSequenceList.GetImageSequence(0);
                var call__22 = call__21.ImageLibrary[10];
                Result = call__22;
            }

            if (Result == null)
            {
                Result = base.GetCellImage();
            }

            return Result;
        }

        // CombatHex.pas:204
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229504;
            return Result;
        }

        // CombatHex.pas:191
        public override Aow.Units.HSEngine.THexagonControl GetHexagonControl()
        {
            Aow.Units.HSEngine.THexagonControl Result = default;
            Result = ((Aow.Units.AoWE.TAoWHSSet)Engine.HSSet).FCombatHexagons;
            return Result;
        }

        public new partial class Meta : Aow.Units.HSEngine.TMapObjectResource.Meta
        {
            // CombatHex.pas:185
            // { $34 }
            public override int ClassID()
            {
                int Result = default;
                Result = 2229505;
                return Result;
            }

            // CombatHex.pas:188
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Combat Hexagon";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatHex.TCombatHexagonResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1223
            public new Aow.Units.CombatHex.TCombatHexagonResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatHex.TCombatHexagonResource o = (Aow.Units.CombatHex.TCombatHexagonResource)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TWaterCombatHexagon (instance size 32, VMT 004036A8) }
    public partial class TWaterCombatHexagon : Aow.Units.AoWHex.TAoWWaterHexagon
    {
        // CombatHex.pas:216
        public override Aow.Units.HSEngine.TMapObjectResourceList GetResourceList(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.HSEngine.TMapObjectResourceList Result = default;
            Result = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources;
            return Result;
        }

        public new partial class Meta : Aow.Units.AoWHex.TAoWWaterHexagon.Meta
        {
            // CombatHex.pas:219
            public override int ClassID()
            {
                int Result = default;
                Result = 2229655;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatHex.TWaterCombatHexagon => (Meta)MetaRegistry.Of(typeof(T));
            // AoWHex.pas:48
            public new Aow.Units.CombatHex.TWaterCombatHexagon Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatHex.TWaterCombatHexagon o = (Aow.Units.CombatHex.TWaterCombatHexagon)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TWaterCombatHexagonResource (instance size 56, VMT 00403854) }
    public partial class TWaterCombatHexagonResource : Aow.Units.AoWHex.TAoWWaterHexagonResource
    {
        // CombatHex.pas:228
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229655;
            return Result;
        }

        // CombatHex.pas:231
        public override Aow.Units.HSEngine.THexagonControl GetHexagonControl()
        {
            Aow.Units.HSEngine.THexagonControl Result = default;
            Result = ((Aow.Units.AoWE.TAoWHSSet)Engine.HSSet).FCombatHexagons;
            return Result;
        }

        public new partial class Meta : Aow.Units.AoWHex.TAoWWaterHexagonResource.Meta
        {
            // CombatHex.pas:222
            public override int ClassID()
            {
                int Result = default;
                Result = 2229656;
                return Result;
            }

            // CombatHex.pas:225
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Combat Water/Ice hex";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatHex.TWaterCombatHexagonResource => (Meta)MetaRegistry.Of(typeof(T));
            // AoWHex.pas:58
            public new Aow.Units.CombatHex.TWaterCombatHexagonResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatHex.TWaterCombatHexagonResource o = (Aow.Units.CombatHex.TWaterCombatHexagonResource)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    public static partial class CombatHex
    {
        // CombatHex.pas:234
        public static void RegisterCombatHexagon(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatHex.TCombatHexagonResource>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatHex.TCombatHexagon>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatHex.TWaterCombatHexagon>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatHex.TWaterCombatHexagonResource>()]);
        }

        public static Aow.Units.FixedArray_byte_3 TransitionNeighbours = Aow.Units.FixedArray_byte_3.From([6, 1, 2]);
        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
