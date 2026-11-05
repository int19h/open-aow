// Translated from CombatILTer.pas.
using Aow.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.ILTer;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.CombatILTer
{
    public partial class TCombatILTerrainShadow : THexagonSprite
    {
        public TCombatILTerrainShadow(TECustomNode owner) : base(owner)
        {}

        public TCombatILTerrain FTerrain;
        // CombatILTer.pas:82
        public override int GetShowPriority()
        {
            int Result = default;
            var call__1 = FTerrain.GetShowPriority();
            Result = (call__1 & 67108863) - 1677721600;
            return Result;
        }

        // CombatILTer.pas:96
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            FTerrain.ShowShadow(XhpSurf, YhpSurf, SurfaceDesc);
        }

        // Engine.pas:524

    }

    public partial class TCombatILTerrain : TILTerrainMO
    {
        public TCombatILTerrain(TECustomNode owner) : base(owner)
        {}

        // CombatILTer.pas:105
        public new const int ClassID = 2228534;

        public TCombatILTerrainShadow FShadow;
        // CombatILTer.pas:99
        public override void Destroy()
        {
            DestroyShadow();
            base.Destroy();
        }

        // CombatILTer.pas:166
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 135169)
            {
                var call__2 = FResource.TerrainList.GetTerrain(Msg.HN);
                if (call__2 >= 0)
                {
                    if (Msg.Param[0].Value < ((TCombatILTerrainResource)FResource).FDefense)
                    {
                        Msg.Param[0] = ((TCombatILTerrainResource)FResource).FDefense;
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
        public override void MakeVisible(TVisibilityList VisibilityList)
        {
            if (!FStatus.Contains(TMapObjectStatusElement.moVisible))
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
        public override TMapObjectResourceList GetResourceList(TEngine AEngine)
        {
            TMapObjectResourceList Result = default;
            Result = AoWHSSet.FCombatResources;
            return Result;
        }

        // CombatILTer.pas:93
        public override bool CanPlaceOnHS(THexagonSprite HS)
        {
            bool Result = default;
            Result = base.CanPlaceOnHS(HS) || HS is TAbstractRoad;
            return Result;
        }

        // CombatILTer.pas:108
        public virtual void ShowShadow(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 40, (uint)(((TMapField)Owner).Xhx + (((TMapField)Owner).Yhx << 4)));
        }

        // CombatILTer.pas:87
        public sbyte GetDefense()
        {
            sbyte Result = default;
            Result = ((TCombatILTerrainResource)FResource).FDefense;
            return Result;
        }

        // CombatILTer.pas:90
        public sbyte GetBurnValue()
        {
            sbyte Result = default;
            Result = ((TCombatILTerrainResource)FResource).FBurnValue;
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

                if (AoWHSMap == null || AoWHSMap.FShowUnitShadows)
                {
                    var call__5 = new TCombatILTerrainShadow(null);
                    FShadow = call__5;
                    FShadow.FTerrain = this;
                }
            }
        }

    }

    public partial class TCombatILTerrainResource : TILTerrainMOR
    {
        public TCombatILTerrainResource(TECustomNode owner) : base(owner)
        {}

        // CombatILTer.pas:175
        public new const int ClassID = 2228535;

        public sbyte FBurnValue;
        public sbyte FDefense;
        // CombatILTer.pas:181
        public override void ReadWrite(TEStorageStream Stream)
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
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat ILTerrain";
            return Result;
        }

    }

    public partial class TCombatFixedILTerrain : TCombatILTerrain
    {
        public TCombatFixedILTerrain(TECustomNode owner) : base(owner)
        {}

        // CombatILTer.pas:191
        public new const int ClassID = 2228536;

        // CombatILTer.pas:197
        public override bool CanChangeTerrain(in THSSenderInfo Info)
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
                    var call__10 = GetMostFrequentMapFieldTerrainType((TMapLevel)Owner.Owner, FXhx, FYhx, call__9);
                    bool__8 = call__10 == Info.MapField.TerrainType;
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
        public override void ChangeTerrain(in THSTerrainInfo Info)
        {
            var call__11 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__11 >= 0)
            {
                var call__12 = ValidTerrainType(Info.TerrainType);
                if (call__12)
                {
                    FTerrainType = Info.TerrainType;
                    var call__13 = GetTerrainTypeImage(Info.TerrainType);
                    FImageIndex = call__13;
                }
            }
        }

        // CombatILTer.pas:216
        public override void TerrainChanged(in THSTerrainInfo Info)
        {
            var call__14 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__14 >= 0)
            {
                var call__15 = ValidTerrainType(Info.TerrainType);
                if (call__15)
                {
                    FTerrainType = Info.TerrainType;
                    var call__16 = GetTerrainTypeImage(Info.TerrainType);
                    FImageIndex = call__16;
                }
                else
                {
                    throw Exception.Create((AnsiString)"TILFixedTerrain Failure");
                }
            }
        }

        // CombatILTer.pas:194
        public override sbyte ForceTerrainType(TMapLevel MapLevel, sbyte Xhx, sbyte Yhx)
        {
            sbyte Result = default;
            Result = -1;
            return Result;
        }

    }

    public partial class TCombatFixedILTerrainResource : TCombatILTerrainResource
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
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat FixedILTerrain";
            return Result;
        }

    }

    public static partial class CombatILTer
    {
        // CombatILTer.pas:236
        public static void RegisterCombatILTerrain(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TCombatILTerrain>(), EClass.Of<TCombatILTerrainResource>(), EClass.Of<TCombatFixedILTerrain>(), EClass.Of<TCombatFixedILTerrainResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
