// Translated from CombatTerrain.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.Terrain;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CombatTerrain
{
    public partial class TCombatTerrainShadow : THexagonSprite
    {
        public TCombatTerrainShadow(TECustomNode owner) : base(owner)
        {}

        public TCombatTerrain FTerrain;
        // CombatTerrain.pas:72
        public override int GetShowPriority()
        {
            int Result = default;
            var call__1 = FTerrain.GetShowPriority();
            Result = (call__1 & 67108863) - 134217728;
            return Result;
        }

        // CombatTerrain.pas:77
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            FTerrain.ShowShadow(XhpSurf, YhpSurf, SurfaceDesc);
        }

        // Engine.pas:524

    }

    public partial class TCombatTerrain : TTerrainMO
    {
        public TCombatTerrain(TECustomNode owner) : base(owner)
        {}

        // CombatTerrain.pas:82
        public new const int ClassID = 2228582;

        public TCombatTerrainShadow FShadow;
        public int FHitPoints;
        public bool FDamaged;
        public int FDefense;
        // CombatTerrain.pas:85
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
                UpdateMapFields((TMapLevel)Owner.Owner, call__3, call__2);
            }

            return Result;
        }

        // CombatTerrain.pas:275
        public override void RemoveFromMap()
        {
            TMapLevel MapLevel = default;
            MapLevel = (TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(MapLevel, FXhx, FYhx);
        }

        // CombatTerrain.pas:149
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            TAbilityCombatInfo AbilityInfo = default;
            TSpellCombatInfo SpellInfo = default;
            base.MapFieldMsgProc(ref Msg);
            if (Msg.HN == 0)
            {
                if (TSet<byte>.WithBounds(0, 255).WithRange(1, 3).Contains((byte)((TTerrainMOR)FResource).DecorationType))
                {
                    switch (Msg.MsgID)
                    {
                        case 200704:
                            var call__4 = AoWHSSet.FAbilityControl.GetAbility(65);
                            call__4.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                            var call__5 = AoWHSMap.Random(16777215);
                            RandSeed = call__5;
                            var call__6 = ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__6);
                            break;
                        case 200705:
                            var call__7 = AoWHSSet.FAbilityControl.GetAbility(24);
                            call__7.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                            var call__8 = AoWHSMap.Random(16777215);
                            RandSeed = call__8;
                            var call__9 = ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__9);
                            break;
                        case 200706:
                            var call__10 = AoWHSSet.FAbilityControl.GetAbility(44);
                            call__10.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                            var call__11 = AoWHSMap.Random(16777215);
                            RandSeed = call__11;
                            var call__12 = ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__12);
                            break;
                        case 200707:
                            var call__13 = AoWHSSet.FSpellControl.GetSpell(117);
                            call__13.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                            var call__14 = AoWHSMap.Random(16777215);
                            RandSeed = call__14;
                            var call__15 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__15);
                            break;
                        case 200714:
                            var call__16 = AoWHSSet.FAbilityControl.GetAbility(31);
                            call__16.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                            var call__17 = AoWHSMap.Random(16777215);
                            RandSeed = call__17;
                            var call__18 = ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__18);
                            break;
                        case 200715:
                            var call__19 = AoWHSSet.FSpellControl.GetSpell(113);
                            call__19.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                            var call__20 = AoWHSMap.Random(16777215);
                            RandSeed = call__20;
                            var call__21 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__21);
                            break;
                        case 200722:
                            if (Msg.Param[1].Value == 30 || Msg.Param[1].Value == 86)
                            {
                                var call__22 = AoWHSSet.FAbilityControl.GetAbility(Msg.Param[1].Value);
                                call__22.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                                var call__23 = AoWHSMap.Random(16777215);
                                RandSeed = call__23;
                                var call__24 = ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - FDefense);
                                SetHitPoints(FHitPoints - call__24);
                            }

                            break;
                        case 200723:
                            if (Msg.Param[1].Value == 114)
                            {
                                var call__25 = AoWHSSet.FSpellControl.GetSpell(Msg.Param[1].Value);
                                call__25.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                                var call__26 = AoWHSMap.Random(16777215);
                                RandSeed = call__26;
                                var call__27 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                                SetHitPoints(FHitPoints - call__27);
                            }

                            break;
                        case 200710:
                            var call__28 = AoWHSSet.FSpellControl.GetSpell(116);
                            call__28.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                            var call__29 = AoWHSMap.Random(16777215);
                            RandSeed = call__29;
                            var call__30 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__30);
                            break;
                        case 200717:
                            var call__31 = AoWHSSet.FSpellControl.GetSpell(127);
                            call__31.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                            var call__32 = AoWHSMap.Random(16777215);
                            RandSeed = call__32;
                            var call__33 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__33);
                            break;
                        case 200718:
                            var call__34 = AoWHSSet.FSpellControl.GetSpell(128);
                            call__34.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                            var call__35 = AoWHSMap.Random(16777215);
                            RandSeed = call__35;
                            var call__36 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__36);
                            break;
                        case 200708:
                            FDamaged = Msg.Param[1].Value == 0;
                            var call__37 = AoWHSSet.FSpellControl.GetSpell(112);
                            call__37.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                            var call__38 = AoWHSMap.Random(16777215);
                            RandSeed = call__38;
                            var call__39 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - call__39);
                            Msg.Param[1] = 1;
                            break;
                        case 200711:
                            var call__40 = AoWHSMap.Random(16777215);
                            RandSeed = call__40;
                            var call__41 = ExecuteDamageRole(TCDamage[1], TCAttack[1] - FDefense);
                            SetHitPoints(FHitPoints - call__41);
                            break;
                    }
                }

                switch (Msg.MsgID)
                {
                    case 135169:
                        var call__42 = FResource.TerrainList.GetTerrain(Msg.HN);
                        if (call__42 >= 0 && Msg.Param[0].Value < ((TCombatTerrainResource)FResource).FDefense)
                        {
                            Msg.Param[0] = ((TCombatTerrainResource)FResource).FDefense;
                        }

                        break;
                }
            }
        }

        // CombatTerrain.pas:291
        public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
        {
            var call__43 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
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
            if (TSet<byte>.WithBounds(0, 255).WithRange(1, 3).Contains((byte)((TTerrainMOR)FResource).DecorationType))
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
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            base.UpdateMapField(Info);
            if (Info.HN < FResource.TerrainList.Count)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 16384;
            }
        }

        // CombatTerrain.pas:124
        public override void MakeVisible(TVisibilityList VisibilityList)
        {
            if (!FStatus.Contains(TMapObjectStatusElement.moVisible))
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
        public override TMapObjectResourceList GetResourceList(TEngine AEngine)
        {
            TMapObjectResourceList Result = default;
            Result = AoWHSSet.FCombatResources;
            return Result;
        }

        // CombatTerrain.pas:106
        public override bool CanPlaceOnHS(THexagonSprite HS)
        {
            bool Result = default;
            Result = base.CanPlaceOnHS(HS) || HS is TAbstractRoad;
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

                if (AoWHSMap == null || AoWHSMap.FShowUnitShadows)
                {
                    var call__47 = new TCombatTerrainShadow(null);
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
        public void ShowShadow(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            HXtoHP(FXhx, FYhx, ref X, ref Y);
            XhpSurf = X - XhpSurf;
            YhpSurf = Y - YhpSurf;
            FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 40, (uint)(((TMapField)Owner).Xhx + (((TMapField)Owner).Yhx << 4)));
        }

    }

    public partial class TCombatTerrainResource : TTerrainMOR
    {
        public TCombatTerrainResource(TECustomNode owner) : base(owner)
        {}

        // CombatTerrain.pas:301
        public new const int ClassID = 2228583;

        public sbyte FBurnValue;
        public sbyte FDefense;
        public byte FHitPoints;
        public bool FMeleeTarget;
        // CombatTerrain.pas:310
        public override void ReadWrite(TEStorageStream Stream)
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
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat Terrain";
            return Result;
        }

    }

    public static partial class CombatTerrain
    {
        // CombatTerrain.pas:319
        public static void RegisterCombatTerrain(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TCombatTerrain>(), EClass.Of<TCombatTerrainResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
