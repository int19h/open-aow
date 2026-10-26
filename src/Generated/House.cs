// Generated from House.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.CombatStructure;
using static Aow.Units.CombatStructure.CombatStructure;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.House
{
    // { THouse (instance size 48, VMT 00434584) }
    public partial class THouse : Aow.Units.CombatStructure.TCombatStructure
    {
        public int FRace;
        // { $28 }
        public int FFlagID;
        // House.pas:65
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int ImageIndex = default;
            int Xhp = default;
            int Yhp = default;
            int TerrainOffset = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            if (FHitPoints == 0)
            {
                var call__1 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.GetImageSequence(50);
                Sequence = call__1;
                var call__2 = GetYhx();
                var call__3 = GetXhx();
                var call__4 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
                var call__5 = call__4.GetField(call__3, call__2);
                switch (call__5.TerrainType)
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
                        ImageIndex = Sequence.GetUnloopedFrameIndex((Aow.Rtl.Rtl.ObjectToInt(this) % 3) + (TerrainOffset * 3));
                        if (ImageIndex != -1)
                        {
                            var call__6 = Sequence.ImageLibrary[ImageIndex];
                            call__6.Show((XhpSurf + Sequence.dXshift) + 16, (YhpSurf + Sequence.dYshift) + 16, SurfaceDesc);
                        }

                        Sequence = Sequence.Next;
                    }
                    while (!(Sequence == null));
                }
            }
            else
            {
                var call__7 = GetYhx();
                var call__8 = GetXhx();
                var call__9 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
                var call__10 = call__9.GetField(call__8, call__7);
                switch (call__10.TerrainType)
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

                var call__11 = FResource.ImageSequenceList.GetImageSequence(TerrainOffset + FRace);
                Sequence = call__11;
                if (Sequence != null)
                {
                    do
                    {
                        switch (Sequence.Layer)
                        {
                            case 7:
                                var call__12 = Sequence.GetFrameIndex(Aow.Units.AoWTC.AoWTC.AoWCombatMap.Frame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                                ImageIndex = call__12;
                                if (ImageIndex != -1)
                                {
                                    if (!Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
                                    {
                                        var call__13 = GetXhx();
                                        var call__14 = GetYhx();
                                        Aow.Units.AoWE.AoWE.AoWHSSet.FFlagControl.ShowStructureFlagEx((XhpSurf + 21) + Sequence.dXshift, (YhpSurf + Sequence.dYshift) + 16, (call__13 + Aow.Units.AoWE.AoWE.AoWHSMap.Frame) + (call__14 * 100), unchecked((byte)FFlagID), SurfaceDesc, false);
                                    }
                                }

                                break;
                            default:
                            {
                                var call__15 = Sequence.GetFrameIndex(Aow.Units.AoWTC.AoWTC.AoWCombatMap.Frame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                                ImageIndex = call__15;
                                if (ImageIndex != -1)
                                {
                                    var call__16 = Sequence.ImageLibrary[ImageIndex];
                                    call__16.Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
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

        // House.pas:45
        public override void Activate()
        {
            Aow.Units.AoWE.TPlayerStructure Structure = default;
            base.Activate();
            if (!Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                var call__17 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(Aow.Units.AoWTC.AoWTC.CombatLLoc);
                var call__18 = call__17.GetField(Aow.Units.AoWTC.AoWTC.CombatXLoc, Aow.Units.AoWTC.AoWTC.CombatYLoc);
                var call__19 = call__18.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TPlayerStructure));
                Structure = (Aow.Units.AoWE.TPlayerStructure)call__19;
                if (Structure != null)
                {
                    if (Structure is Aow.Units.City.TCity)
                    {
                        FRace = ((Aow.Units.City.TCity)Structure).FRace;
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
                        var call__20 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer);
                        FFlagID = call__20.FPlayerRace;
                    }

                    if (FFlagID == -1)
                    {
                        FFlagID = 32;
                    }
                }
            }
        }

        // House.pas:123
        public override void ShowShadow(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int ImageIndex = default;
            int Xhp = default;
            int Yhp = default;
            if (FHitPoints > 0)
            {
                Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
                XhpSurf = Xhp - XhpSurf;
                YhpSurf = Yhp - YhpSurf;
                var call__21 = FResource.ImageSequenceList.GetImageSequence(FRace + 60);
                Sequence = call__21;
                if (Sequence != null)
                {
                    do
                    {
                        var call__22 = Sequence.GetFrameIndex(Aow.Units.AoWTC.AoWTC.AoWCombatMap.Frame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                        ImageIndex = call__22;
                        if (ImageIndex != -1)
                        {
                            var call__23 = Sequence.ImageLibrary[ImageIndex];
                            call__23.Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                        }

                        Sequence = Sequence.Next;
                    }
                    while (!(Sequence == null));
                }
            }
        }

        public new partial class Meta : Aow.Units.CombatStructure.TCombatStructure.Meta
        {
            // House.pas:42
            // { $2C }
            public override int ClassID()
            {
                int Result = default;
                Result = 2229522;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.House.THouse => (Meta)MetaRegistry.Of(typeof(T));
            // CombatStructure.pas:23
            public new Aow.Units.House.THouse Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.House.THouse o = (Aow.Units.House.THouse)NewInstance();
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

    // { THouseResource (instance size 72, VMT 00434728) }
    public partial class THouseResource : Aow.Units.CombatStructure.TCombatStructureResource
    {
        // House.pas:151
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229522;
            return Result;
        }

        public new partial class Meta : Aow.Units.CombatStructure.TCombatStructureResource.Meta
        {
            // House.pas:145
            public override int ClassID()
            {
                int Result = default;
                Result = 2229523;
                return Result;
            }

            // House.pas:148
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Combat House";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.House.THouseResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.House.THouseResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.House.THouseResource o = (Aow.Units.House.THouseResource)NewInstance();
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

    public static partial class House
    {
        // House.pas:154
        public static void RegisterHouse(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.House.THouseResource>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.House.THouse>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
