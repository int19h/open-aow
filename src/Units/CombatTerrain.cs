// Translated from CombatTerrain.pas.
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
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CombatTerrain
{
    // { TCombatTerrainShadow (instance size 12, VMT 0040FB0C) }
    public partial class TCombatTerrainShadow : Aow.Units.HSEngine.THexagonSprite
    {
        public TCombatTerrainShadow(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        public Aow.Units.CombatTerrain.TCombatTerrain FTerrain;
        // CombatTerrain.pas:72
        // { $08 }
        public override int GetShowPriority()
        {
            int Result = default;
            var call__1 = FTerrain.GetShowPriority();
            Result = (call__1 & 67108863) - 134217728;
            return Result;
        }

        // CombatTerrain.pas:77
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            FTerrain.ShowShadow(XhpSurf, YhpSurf, SurfaceDesc);
        }

        // Engine.pas:524

    }

    // { TCombatTerrain (instance size 36, VMT 0040FCA0) }
    public partial class TCombatTerrain : Aow.Units.Terrain.TTerrainMO
    {
        public TCombatTerrain(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // CombatTerrain.pas:82
        public new const int ClassID = 2228582;

        public Aow.Units.CombatTerrain.TCombatTerrainShadow FShadow;
        // { $14 }
        public int FHitPoints;
        // { $18 }
        public bool FDamaged;
        // { $1C }
        public Aow.Units.FixedArray_byte_3 F_CombatTerrain_1D;
        public int FDefense;
        // CombatTerrain.pas:85
        // { $20 }
        public override void Destroy()
        {
            DestroyShadow();
            base.Destroy();
        }

        // CombatTerrain.pas:269
        public override bool PlaceOnMap()
        {
            bool Result = default;
            Result = base.PlaceOnMap();
            if (Result)
            {
                var call__2 = Yhx;
                var call__3 = Xhx;
                UpdateMapFields((Aow.Units.HSEngine.TMapLevel)Owner.Owner, call__3, call__2);
            }

            return Result;
        }

        // CombatTerrain.pas:275
        public override void RemoveFromMap()
        {
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(MapLevel, FXhx, FYhx);
        }

        // CombatTerrain.pas:149
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            Aow.Units.AoWE.TAbilityCombatInfo AbilityInfo = default;
            Aow.Units.AoWE.TSpellCombatInfo SpellInfo = default;
            base.MapFieldMsgProc(ref Msg);
            if (Msg.HN == 0)
            {
                if (TSet<byte>.WithBounds(0, 255).WithRange(1, 3).Contains((byte)((Aow.Units.Terrain.TTerrainMOR)FResource).DecorationType))
                {
                    switch (Msg.MsgID)
                    {
                        case 200704:
                            var call__4 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(65);
                            call__4.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                            var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__5;
                            var call__6 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__6);
                            break;
                        case 200705:
                            var call__7 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(24);
                            call__7.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                            var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__8;
                            var call__9 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__9);
                            break;
                        case 200706:
                            var call__10 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(44);
                            call__10.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                            var call__11 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__11;
                            var call__12 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__12);
                            break;
                        case 200707:
                            var call__13 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(117);
                            call__13.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                            var call__14 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__14;
                            var call__15 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__15);
                            break;
                        case 200714:
                            var call__16 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(31);
                            call__16.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                            var call__17 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__17;
                            var call__18 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__18);
                            break;
                        case 200715:
                            var call__19 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(113);
                            call__19.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                            var call__20 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__20;
                            var call__21 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__21);
                            break;
                        case 200722:
                            if ((Msg.Param[1].Value == 30) || (Msg.Param[1].Value == 86))
                            {
                                var call__22 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Msg.Param[1].Value);
                                call__22.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                                var call__23 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                Aow.Rtl.Rtl.RandSeed = call__23;
                                var call__24 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - FDefense);
                                SetHitPoints(FHitPoints - call__24);
                            }

                            break;
                        case 200723:
                            if (Msg.Param[1].Value == 114)
                            {
                                var call__25 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Msg.Param[1].Value);
                                call__25.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                                var call__26 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                Aow.Rtl.Rtl.RandSeed = call__26;
                                var call__27 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                                SetHitPoints(FHitPoints - call__27);
                            }

                            break;
                        case 200710:
                            var call__28 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(116);
                            call__28.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                            var call__29 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__29;
                            var call__30 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__30);
                            break;
                        case 200717:
                            var call__31 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(127);
                            call__31.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                            var call__32 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__32;
                            var call__33 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__33);
                            break;
                        case 200718:
                            var call__34 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(128);
                            call__34.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                            var call__35 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__35;
                            var call__36 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__36);
                            break;
                        case 200708:
                            FDamaged = Msg.Param[1].Value == 0;
                            var call__37 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(112);
                            call__37.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                            var call__38 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__38;
                            var call__39 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__39);
                            Msg.Param[1] = 1;
                            break;
                        case 200711:
                            var call__40 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__40;
                            var call__41 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(Aow.Units.AoWTC.AoWTC.TCDamage[1], Aow.Units.AoWTC.AoWTC.TCAttack[1] - FDefense);
                            SetHitPoints(FHitPoints - call__41);
                            break;
                    }
                }

                switch (Msg.MsgID)
                {
                    case 135169:
                        var call__42 = FResource.TerrainList.GetTerrain(Msg.HN);
                        if ((call__42 >= 0) && (Msg.Param[0].Value < ((Aow.Units.CombatTerrain.TCombatTerrainResource)FResource).FDefense))
                        {
                            Msg.Param[0] = ((Aow.Units.CombatTerrain.TCombatTerrainResource)FResource).FDefense;
                        }

                        break;
                }
            }
        }

        // CombatTerrain.pas:291
        public override void CanMoveOn(in Aow.Units.HSEngine.THSMoveOnInfo Info, ref Aow.Units.HSEngine.THSMoveOnResult MoveResult)
        {
            var call__43 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
            if (call__43)
            {
                var call__44 = FResource.TerrainList.GetOverlay(Info.HN);
                if (call__44 == 8)
                {
                    MoveResult.Code = -1;
                    MoveResult.Done = true;
                }
            }
        }

        // CombatTerrain.pas:254
        public override void Activate()
        {
            base.Activate();
            if (TSet<byte>.WithBounds(0, 255).WithRange(1, 3).Contains((byte)((Aow.Units.Terrain.TTerrainMOR)FResource).DecorationType))
            {
                FHitPoints = 3;
            }
            else
            {
                FHitPoints = 0;
            }

            FDefense = -2;
            CreateShadow();
        }

        // CombatTerrain.pas:263
        public override void Deactivate()
        {
            DestroyShadow();
            base.Deactivate();
        }

        // CombatTerrain.pas:284
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            base.UpdateMapField(Info);
            if (Info.HN < FResource.TerrainList.Count)
            {
                ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags | 16384;
            }
        }

        // CombatTerrain.pas:124
        public override void MakeVisible(Aow.Units.HSEngine.TVisibilityList VisibilityList)
        {
            if (!FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moVisible))
            {
                base.MakeVisible(VisibilityList);
                if (FShadow != null)
                {
                    var call__45 = FShadow.GetShowPriority();
                    VisibilityList.AddSortChild(FShadow, call__45);
                }
            }
        }

        // CombatTerrain.pas:91
        public override Aow.Units.HSEngine.TMapObjectResourceList GetResourceList(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.HSEngine.TMapObjectResourceList Result = default;
            Result = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources;
            return Result;
        }

        // CombatTerrain.pas:106
        public override bool CanPlaceOnHS(Aow.Units.HSEngine.THexagonSprite HS)
        {
            bool Result = default;
            Result = base.CanPlaceOnHS(HS) || (HS is Aow.Units.HSEngine.TAbstractRoad);
            return Result;
        }

        // CombatTerrain.pas:96
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

        // CombatTerrain.pas:111
        public void CreateShadow()
        {
            var call__46 = FResource.ImageSequenceList.GetImageSequence(40);
            if (call__46 != null)
            {
                if (FShadow != null)
                {
                    DestroyShadow();
                }

                if ((Aow.Units.AoWE.AoWE.AoWHSMap == null) || Aow.Units.AoWE.AoWE.AoWHSMap.FShowUnitShadows)
                {
                    var call__47 = new Aow.Units.CombatTerrain.TCombatTerrainShadow(null);
                    FShadow = call__47;
                    FShadow.FTerrain = this;
                }
            }
        }

        // CombatTerrain.pas:133
        public void SetHitPoints(int Value)
        {
            // { Original ignores Value and only marks the terrain damaged. }
            FDamaged = true;
        }

        // CombatTerrain.pas:138
        public void ShowShadow(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(FXhx, FYhx, ref X, ref Y);
            XhpSurf = X - XhpSurf;
            YhpSurf = Y - YhpSurf;
            FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 40, unchecked((uint)(((Aow.Units.HSEngine.TMapField)Owner).Xhx + (((Aow.Units.HSEngine.TMapField)Owner).Yhx << 4))));
        }

    }

    // { TCombatTerrainResource (instance size 76, VMT 0040FE54) }
    public partial class TCombatTerrainResource : Aow.Units.Terrain.TTerrainMOR
    {
        public TCombatTerrainResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // CombatTerrain.pas:301
        public new const int ClassID = 2228583;

        public sbyte FBurnValue;
        // { $48; AoWEd 0040B0AB }
        public sbyte FDefense;
        // { $49 }
        public byte FHitPoints;
        // { $4A; AoWEd 0040B0C2 }
        public bool FMeleeTarget;
        // CombatTerrain.pas:310
        // { $4B; AoWEd 0040B0D6 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDefense), 20);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FBurnValue), 21);
            Stream.rwByte(ref FHitPoints, 22);
            Stream.rwBool(ref FMeleeTarget, 23);
        }

        // CombatTerrain.pas:304
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2228582;
            return Result;
        }

        // CombatTerrain.pas:307
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Combat Terrain";
            return Result;
        }

    }

    public static partial class CombatTerrain
    {
        // CombatTerrain.pas:319
        public static void RegisterCombatTerrain(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.CombatTerrain.TCombatTerrain>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatTerrain.TCombatTerrainResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
