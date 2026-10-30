// Translated from CombatILTer.pas.
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
using Aow.Units.CombatHex;
using static Aow.Units.CombatHex.CombatHex;

namespace Aow.Units.CombatILTer
{
    // { TCombatILTerrainShadow (instance size 12, VMT 004041E4) }
    public partial class TCombatILTerrainShadow : Aow.Units.HSEngine.THexagonSprite
    {
        public TCombatILTerrainShadow(TECustomNode owner) : base(owner)
        {}

        public Aow.Units.CombatILTer.TCombatILTerrain FTerrain;
        // CombatILTer.pas:82
        // { $08 }
        public override int GetShowPriority()
        {
            int Result = default;
            var call__1 = FTerrain.GetShowPriority();
            Result = (call__1 & 67108863) - 1677721600;
            return Result;
        }

        // CombatILTer.pas:96
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            FTerrain.ShowShadow(XhpSurf, YhpSurf, SurfaceDesc);
        }

        // Engine.pas:524

    }

    // { TCombatILTerrain (instance size 32, VMT 00404378) }
    public partial class TCombatILTerrain : Aow.Units.ILTer.TILTerrainMO
    {
        public TCombatILTerrain(TECustomNode owner) : base(owner)
        {}

        // CombatILTer.pas:105
        public new const int ClassID = 2228534;

        public Aow.Units.CombatILTer.TCombatILTerrainShadow FShadow;
        // CombatILTer.pas:99
        // { $1C }
        public override void Destroy()
        {
            DestroyShadow();
            base.Destroy();
        }

        // CombatILTer.pas:166
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 135169)
            {
                var call__2 = FResource.TerrainList.GetTerrain(Msg.HN);
                if (call__2 >= 0)
                {
                    if (Msg.Param[0].Value < ((Aow.Units.CombatILTer.TCombatILTerrainResource)FResource).FDefense)
                    {
                        Msg.Param[0] = ((Aow.Units.CombatILTer.TCombatILTerrainResource)FResource).FDefense;
                    }
                }
            }
        }

        // CombatILTer.pas:151
        public override void Activate()
        {
            base.Activate();
            CreateShadow();
        }

        // CombatILTer.pas:157
        public override void Deactivate()
        {
            DestroyShadow();
            base.Deactivate();
        }

        // CombatILTer.pas:141
        public override void MakeVisible(Aow.Units.HSEngine.TVisibilityList VisibilityList)
        {
            if (!FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moVisible))
            {
                base.MakeVisible(VisibilityList);
                if (FShadow != null)
                {
                    var call__3 = FShadow.GetShowPriority();
                    VisibilityList.AddSortChild(FShadow, call__3);
                }
            }
        }

        // CombatILTer.pas:163
        public override Aow.Units.HSEngine.TMapObjectResourceList GetResourceList(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.HSEngine.TMapObjectResourceList Result = default;
            Result = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources;
            return Result;
        }

        // CombatILTer.pas:93
        public override bool CanPlaceOnHS(Aow.Units.HSEngine.THexagonSprite HS)
        {
            bool Result = default;
            Result = base.CanPlaceOnHS(HS) || (HS is Aow.Units.HSEngine.TAbstractRoad);
            return Result;
        }

        // CombatILTer.pas:108
        public virtual void ShowShadow(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 40, unchecked((uint)(((Aow.Units.HSEngine.TMapField)Owner).Xhx + (((Aow.Units.HSEngine.TMapField)Owner).Yhx << 4))));
        }

        // CombatILTer.pas:87
        // { +138 }
        public sbyte GetDefense()
        {
            sbyte Result = default;
            Result = unchecked((sbyte)((Aow.Units.CombatILTer.TCombatILTerrainResource)FResource).FDefense);
            return Result;
        }

        // CombatILTer.pas:90
        public sbyte GetBurnValue()
        {
            sbyte Result = default;
            Result = unchecked((sbyte)((Aow.Units.CombatILTer.TCombatILTerrainResource)FResource).FBurnValue);
            return Result;
        }

        // CombatILTer.pas:118
        public void DestroyShadow()
        {
            if (FShadow != null)
            {
                if (GetVisible())
                {
                    FResource.Engine.HSMap.DynamicVisibleHS.RemoveChild(FShadow);
                }

                FShadow.Free();
                FShadow = null;
            }
        }

        // CombatILTer.pas:128
        public void CreateShadow()
        {
            var call__4 = FResource.ImageSequenceList.GetImageSequence(40);
            if (call__4 != null)
            {
                if (FShadow != null)
                {
                    DestroyShadow();
                }

                if ((Aow.Units.AoWE.AoWE.AoWHSMap == null) || Aow.Units.AoWE.AoWE.AoWHSMap.FShowUnitShadows)
                {
                    var call__5 = new Aow.Units.CombatILTer.TCombatILTerrainShadow(null);
                    FShadow = call__5;
                    FShadow.FTerrain = this;
                }
            }
        }

    }

    // { TCombatILTerrainResource (instance size 72, VMT 00404538) }
    public partial class TCombatILTerrainResource : Aow.Units.ILTer.TILTerrainMOR
    {
        public TCombatILTerrainResource(TECustomNode owner) : base(owner)
        {}

        // CombatILTer.pas:175
        public new const int ClassID = 2228535;

        public sbyte FBurnValue;
        // { $44 }
        public sbyte FDefense;
        // { $45 }
        public Aow.Units.FixedArray_byte_2 F_CombatILTerrainResource_46;
        // CombatILTer.pas:181
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDefense), 20);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FBurnValue), 21);
        }

        // CombatILTer.pas:188
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2228534;
            return Result;
        }

        // CombatILTer.pas:178
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Combat ILTerrain";
            return Result;
        }

    }

    // { TCombatFixedILTerrain (instance size 32, VMT 00404644) }
    public partial class TCombatFixedILTerrain : Aow.Units.CombatILTer.TCombatILTerrain
    {
        public TCombatFixedILTerrain(TECustomNode owner) : base(owner)
        {}

        // CombatILTer.pas:191
        public new const int ClassID = 2228536;

        // CombatILTer.pas:197
        public override bool CanChangeTerrain(in Aow.Units.HSEngine.THSSenderInfo Info)
        {
            bool Result = default;
            var call__6 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__6 >= 0)
            {
                var call__7 = ValidTerrainType(Info.MapField.TerrainType);
                bool bool__8 = call__7;
                if (bool__8)
                {
                    var call__9 = FResource.TerrainList.GetTerrain(Info.HN);
                    var call__10 = GetMostFrequentMapFieldTerrainType((Aow.Units.HSEngine.TMapLevel)Owner.Owner, unchecked((sbyte)FXhx), unchecked((sbyte)FYhx), unchecked((sbyte)call__9));
                    bool__8 = (call__10 == Info.MapField.TerrainType);
                }

                Result = bool__8;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // CombatILTer.pas:206
        public override void ChangeTerrain(in Aow.Units.HSEngine.THSTerrainInfo Info)
        {
            var call__11 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__11 >= 0)
            {
                var call__12 = ValidTerrainType(Info.TerrainType);
                if (call__12)
                {
                    FTerrainType = unchecked((sbyte)Info.TerrainType);
                    var call__13 = GetTerrainTypeImage(Info.TerrainType);
                    FImageIndex = call__13;
                }
            }
        }

        // CombatILTer.pas:216
        public override void TerrainChanged(in Aow.Units.HSEngine.THSTerrainInfo Info)
        {
            var call__14 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__14 >= 0)
            {
                var call__15 = ValidTerrainType(Info.TerrainType);
                if (call__15)
                {
                    FTerrainType = unchecked((sbyte)Info.TerrainType);
                    var call__16 = GetTerrainTypeImage(Info.TerrainType);
                    FImageIndex = call__16;
                }
                else
                {
                    throw Aow.Rtl.Exception.Create((AnsiString)"TILFixedTerrain Failure");
                }
            }
        }

        // CombatILTer.pas:194
        public override sbyte ForceTerrainType(Aow.Units.HSEngine.TMapLevel MapLevel, sbyte Xhx, sbyte Yhx)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-1);
            return Result;
        }

    }

    // { TCombatFixedILTerrainResource (instance size 72, VMT 0040480C) }
    public partial class TCombatFixedILTerrainResource : Aow.Units.CombatILTer.TCombatILTerrainResource
    {
        public TCombatFixedILTerrainResource(TECustomNode owner) : base(owner)
        {}

        // CombatILTer.pas:227
        public new const int ClassID = 2228537;

        // CombatILTer.pas:233
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2228536;
            return Result;
        }

        // CombatILTer.pas:230
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Combat FixedILTerrain";
            return Result;
        }

    }

    public static partial class CombatILTer
    {
        // CombatILTer.pas:236
        public static void RegisterCombatILTerrain(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.CombatILTer.TCombatILTerrain>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatILTer.TCombatILTerrainResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatILTer.TCombatFixedILTerrain>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatILTer.TCombatFixedILTerrainResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
