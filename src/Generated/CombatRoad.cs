// Generated from CombatRoad.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.CombatTerrain;
using static Aow.Units.CombatTerrain.CombatTerrain;

namespace Aow.Units.CombatRoad
{
    // { TCombatRoad (instance size 24, VMT 00410C0C) }
    public partial class TCombatRoad : Aow.Units.HSEngine.TAbstractRoad
    {
        public Aow.Units.FixedArray_byte_7 FImages;
        // { $10 main image, $11..$16 neighbour images }
        public byte FReserved17;
        // CombatRoad.pas:108
        public override void NeighbourTerrainChanged(int HN, in Aow.Units.HSEngine.THSTerrainInfo Msg)
        {
            if (HN == 0)
            {
                UpdateTransition(unchecked((byte)Msg.HN));
            }
        }

        // CombatRoad.pas:113
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TCustomImageLibrary Images = default;
            int X = default;
            int Y = default;
            int I = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref X, ref Y);
            X = (X - XhpSurf) - 8;
            Y = Y - YhpSurf;
            var call__1 = FResource.ImageSequenceList.GetImageSequence(0);
            Images = call__1.ImageLibrary;
            var call__2 = Images.Get(FImages[0]);
            call__2.Show(X, Y, SurfaceDesc);
            {
                I = 1;
                byte end__3 = 6;
                for (; I <= end__3; I += 1)
                {
                    if (FImages[I] != 255)
                    {
                        var call__4 = Images.Get(FImages[I]);
                        call__4.Show(X - 8, Y - 16, SurfaceDesc);
                    }
                }
            }
        }

        // CombatRoad.pas:127
        public override void Activate()
        {
            base.Activate();
            var call__5 = Aow.Rtl.Rtl.Random(12);
            FImages[0] = unchecked((byte)(call__5 + 10));
        }

        // CombatRoad.pas:54
        public override Aow.Units.HSEngine.TMapObjectResourceList GetResourceList(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.HSEngine.TMapObjectResourceList Result = default;
            Result = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources;
            return Result;
        }

        // CombatRoad.pas:57
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            if ((((((Xhx >= 0) && (Yhx >= 0)) && (Lhx >= 0)) && (Xhx < MapContainer.Width)) && (Yhx < MapContainer.Height)) && (Lhx < MapContainer.LevelCount))
            {
                var call__6 = MapContainer.GetField(Xhx, Yhx, Lhx);
                bool bool__7 = (call__6.OverlayType == 4);
                if (!bool__7)
                {
                    var call__8 = MapContainer.GetField(Xhx, Yhx, Lhx);
                    bool__7 = (call__8.TerrainType == 15);
                }

                Result = !bool__7;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // CombatRoad.pas:66
        public override bool PlaceHX(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Aow.Units.HSEngine.THexagonSprite OldRoad = default;
            Aow.Units.HSEngine.TMapField MapField = default;
            MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
            OldRoad = MapField.FindHS(typeof(Aow.Units.HSEngine.TAbstractRoad));
            if (OldRoad != null)
            {
                OldRoad.Destroy();
            }

            Result = base.PlaceHX(MapContainer, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)Lhx));
            return Result;
        }

        // CombatRoad.pas:101
        public override void UpdateTransitions()
        {
            int I = default;
            {
                I = 1;
                byte end__9 = 6;
                for (; I <= end__9; I += 1)
                {
                    UpdateTransition(unchecked((byte)I));
                }
            }
        }

        // CombatRoad.pas:77
        public void UpdateTransition(byte HN)
        {
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            sbyte Overlay = default;
            int X = default;
            int Y = default;
            MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, HN, ref X, ref Y);
            if ((((X >= 0) && (Y >= 0)) && (X < MapLevel.Width)) && (Y < MapLevel.Height))
            {
                var call__10 = MapLevel.GetField(X, Y);
                Overlay = unchecked((sbyte)call__10.OverlayType);
                if (Overlay != 4)
                {
                    switch (HN)
                    {
                        case 1:
                            var call__11 = Aow.Rtl.Rtl.Random(2);
                            FImages[HN] = unchecked((byte)((call__11 * 3) + 1));
                            break;
                        case 2:
                            var call__12 = Aow.Rtl.Rtl.Random(2);
                            FImages[HN] = unchecked((byte)((call__12 * 3) + 2));
                            break;
                        case 3:
                            FImages[HN] = 6;
                            break;
                        case 4:
                            FImages[HN] = 7;
                            break;
                        case 5:
                            FImages[HN] = 8;
                            break;
                        case 6:
                            var call__13 = Aow.Rtl.Rtl.Random(2);
                            FImages[HN] = unchecked((byte)(call__13 * 3));
                            break;
                    }
                }
                else
                {
                    FImages[HN] = 255;
                }
            }
        }

        public new partial class Meta : Aow.Units.HSEngine.TAbstractRoad.Meta
        {
            // CombatRoad.pas:51
            public override int ClassID()
            {
                int Result = default;
                Result = 2229506;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatRoad.TCombatRoad => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.CombatRoad.TCombatRoad Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatRoad.TCombatRoad o = (Aow.Units.CombatRoad.TCombatRoad)NewInstance();
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

    // { TCombatRoadResource (instance size 52, VMT 00410DA0) }
    public partial class TCombatRoadResource : Aow.Units.HSEngine.TAbstractRoadResource
    {
        // CombatRoad.pas:144
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 65621)
            {
                Msg.Result = 5;
            }
        }

        // CombatRoad.pas:153
        public override Aow.Units.ImageLib.TLibraryImage GetCellImage()
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            Result = null;
            bool bool__14 = (ImageSequenceList != null) && (ImageSequenceList.Count > 0);
            if (bool__14)
            {
                var call__15 = ImageSequenceList.GetImageSequence(0);
                bool__14 = (call__15 != null);
            }

            if (bool__14)
            {
                var call__16 = ImageSequenceList.GetImageSequence(0);
                var call__17 = call__16.ImageLibrary.Get(10);
                Result = call__17;
            }

            if (Result == null)
            {
                Result = base.GetCellImage();
            }

            return Result;
        }

        // CombatRoad.pas:150
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229506;
            return Result;
        }

        // CombatRoad.pas:139
        public override Aow.Units.HSEngine.TRoadControl GetRoadControl()
        {
            Aow.Units.HSEngine.TRoadControl Result = default;
            Result = ((Aow.Units.AoWE.TAoWHSSet)Engine.HSSet).FCombatRoads;
            return Result;
        }

        public new partial class Meta : Aow.Units.HSEngine.TMapObjectResource.Meta
        {
            // CombatRoad.pas:133
            public override int ClassID()
            {
                int Result = default;
                Result = 2229507;
                return Result;
            }

            // CombatRoad.pas:136
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Combat Road";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatRoad.TCombatRoadResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1124
            public new Aow.Units.CombatRoad.TCombatRoadResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatRoad.TCombatRoadResource o = (Aow.Units.CombatRoad.TCombatRoadResource)NewInstance();
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

    public static partial class CombatRoad
    {
        // CombatRoad.pas:162
        public static void RegisterCombatRoad(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatRoad.TCombatRoadResource>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatRoad.TCombatRoad>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
