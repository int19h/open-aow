// Translated from CombatStructure.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Units.Sound.Sound;
using Aow.Units.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.CombatILTer;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.CombatRoad;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CombatStructure
{
    public partial class TCombatStructure : TCombatFixedILTerrain
    {
        public TCombatStructure(TECustomNode owner) : base(owner)
        {
            SetHitPoints(5);
        }

        // CombatStructure.pas:57
        public new const int ClassID = 2229510;

        public int FHitPoints;
        public bool FPlaySound;
        // CombatStructure.pas:51


        // CombatStructure.pas:229
        public override sbyte GetOverlay(int Index)
        {
            sbyte Result = default;
            var call__1 = FResource.TerrainList.GetTerrain(Index);
            if (call__1 >= 0)
            {
                if (FHitPoints == 0)
                {
                    Result = 6;
                }
                else
                {
                    Result = 8;
                }
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // CombatStructure.pas:104
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            TAbilityCombatInfo AbilityInfo = default;
            TSpellCombatInfo SpellInfo = default;
            base.MapFieldMsgProc(ref Msg);
            if (Msg.HN == 0)
            {
                switch (Msg.MsgID)
                {
                    case 200704:
                        var call__2 = AoWHSSet.FAbilityControl.GetAbility(65);
                        call__2.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__3 = AoWHSMap.Random(16777215);
                        RandSeed = call__3;
                        var call__4 = GetDefense();
                        var call__5 = ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__4);
                        SetHitPoints(FHitPoints - call__5);
                        break;
                    case 200705:
                        var call__6 = AoWHSSet.FAbilityControl.GetAbility(24);
                        call__6.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__7 = AoWHSMap.Random(16777215);
                        RandSeed = call__7;
                        var call__8 = GetDefense();
                        var call__9 = ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__8);
                        SetHitPoints(FHitPoints - call__9);
                        break;
                    case 200706:
                        var call__10 = AoWHSSet.FAbilityControl.GetAbility(44);
                        call__10.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__11 = AoWHSMap.Random(16777215);
                        RandSeed = call__11;
                        var call__12 = GetDefense();
                        var call__13 = ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__12);
                        SetHitPoints(FHitPoints - call__13);
                        break;
                    case 200707:
                        var call__14 = AoWHSSet.FSpellControl.GetSpell(117);
                        call__14.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                        var call__15 = AoWHSMap.Random(16777215);
                        RandSeed = call__15;
                        var call__16 = GetDefense();
                        var call__17 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__16);
                        SetHitPoints(FHitPoints - call__17);
                        break;
                    case 200714:
                        var call__18 = AoWHSSet.FAbilityControl.GetAbility(31);
                        call__18.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__19 = AoWHSMap.Random(16777215);
                        RandSeed = call__19;
                        var call__20 = GetDefense();
                        var call__21 = ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__20);
                        SetHitPoints(FHitPoints - call__21);
                        break;
                    case 200715:
                        var call__22 = AoWHSSet.FSpellControl.GetSpell(113);
                        call__22.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                        var call__23 = AoWHSMap.Random(16777215);
                        RandSeed = call__23;
                        var call__24 = GetDefense();
                        var call__25 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__24);
                        SetHitPoints(FHitPoints - call__25);
                        break;
                    case 200722:
                        if (Msg.Param[1].Value == 30 || Msg.Param[1].Value == 86)
                        {
                            var call__26 = AoWHSSet.FAbilityControl.GetAbility(Msg.Param[1].Value);
                            call__26.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                            var call__27 = AoWHSMap.Random(16777215);
                            RandSeed = call__27;
                            var call__28 = GetDefense();
                            var call__29 = ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__28);
                            SetHitPoints(FHitPoints - call__29);
                        }

                        break;
                    case 200723:
                        if (Msg.Param[1].Value == 114)
                        {
                            var call__30 = AoWHSSet.FSpellControl.GetSpell(Msg.Param[1].Value);
                            call__30.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                            var call__31 = AoWHSMap.Random(16777215);
                            RandSeed = call__31;
                            var call__32 = GetDefense();
                            var call__33 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__32);
                            SetHitPoints(FHitPoints - call__33);
                        }

                        break;
                    case 200710:
                        var call__34 = AoWHSSet.FSpellControl.GetSpell(116);
                        call__34.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                        var call__35 = AoWHSMap.Random(16777215);
                        RandSeed = call__35;
                        var call__36 = GetDefense();
                        var call__37 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__36);
                        SetHitPoints(FHitPoints - call__37);
                        break;
                    case 200717:
                        var call__38 = AoWHSSet.FSpellControl.GetSpell(127);
                        call__38.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                        var call__39 = AoWHSMap.Random(16777215);
                        RandSeed = call__39;
                        var call__40 = GetDefense();
                        var call__41 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__40);
                        SetHitPoints(FHitPoints - call__41);
                        break;
                    case 200718:
                        var call__42 = AoWHSSet.FSpellControl.GetSpell(128);
                        call__42.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                        var call__43 = AoWHSMap.Random(16777215);
                        RandSeed = call__43;
                        var call__44 = GetDefense();
                        var call__45 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__44);
                        SetHitPoints(FHitPoints - call__45);
                        break;
                    case 200708:
                        FPlaySound = Msg.Param[1].Value == 0;
                        var call__46 = AoWHSSet.FSpellControl.GetSpell(112);
                        call__46.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                        var call__47 = AoWHSMap.Random(16777215);
                        RandSeed = call__47;
                        var call__48 = GetDefense();
                        var call__49 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__48);
                        SetHitPoints(FHitPoints - call__49);
                        Msg.Param[1] = 1;
                        break;
                    case 200711:
                        var call__50 = AoWHSMap.Random(16777215);
                        RandSeed = call__50;
                        var call__51 = GetDefense();
                        var call__52 = ExecuteDamageRole(TCDamage[1], TCAttack[1] - call__51);
                        SetHitPoints(FHitPoints - call__52);
                        break;
                }
            }
        }

        // CombatStructure.pas:216
        public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
        {
            var call__53 = FResource.TerrainList.GetUsed(Info.HN);
            if (call__53)
            {
                if (FHitPoints == 0)
                {
                    MoveResult.Code = ((TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][6 - -1];
                }
                else
                {
                    var call__54 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
                    if (call__54)
                    {
                        MoveResult.Code = -1;
                    }
                    else
                    {
                        MoveResult.Code = ((TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1];
                    }
                }

                MoveResult.Done = true;
            }
        }

        // CombatStructure.pas:60
        public override void Activate()
        {
            TPlayerStructure Structure = default;
            bool Razed = default;
            base.Activate();
            if (!AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                FPlaySound = true;
                Razed = false;
                var call__55 = AoWHSMap.MapContainer.GetMapLevel(CombatLLoc);
                var call__56 = call__55.GetField(CombatXLoc, CombatYLoc);
                var call__57 = call__56.FindNoneTransparentHS(typeof(TPlayerStructure));
                Structure = (TPlayerStructure)call__57;
                if (Structure != null)
                {
                    Razed = Structure.GetRazed();
                }

                if (Razed)
                {
                    SetHitPoints(0);
                }
                else
                {
                    SetHitPoints(5);
                }
            }
            else
            {
                SetHitPoints(5);
            }
        }

        // CombatStructure.pas:208
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            base.UpdateMapField(Info);
            if (Info.HN == 0)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 16384 | 8192;
            }
        }

        // CombatStructure.pas:253
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = base.CanPlace(MapContainer, Xhx, Yhx, Lhx);
            return Result;
        }

        // CombatStructure.pas:250
        public override bool CanPlaceOnHS(THexagonSprite HS)
        {
            bool Result = default;
            Result = base.CanPlaceOnHS(HS) || HS is TCombatRoad;
            return Result;
        }

        // CombatStructure.pas:247
        public override bool ValidTerrainType(int TerrainType)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // CombatStructure.pas:237
        public override int GetTerrainTypeImage(int TerrainType)
        {
            int Result = default;
            if (TerrainType == 3)
            {
                Result = 20;
            }
            else
            {
                if (TSet<byte>.WithBounds(0, 255).With(5).With(12).Contains((byte)TerrainType))
                {
                    Result = 40;
                }
                else
                {
                    Result = 0;
                }
            }

            return Result;
        }

        // CombatStructure.pas:244
        public override void ShowShadow(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
        }

        // CombatStructure.pas:79
        public void SetHitPoints(int Value)
        {
            TfxHS Effect = default;
            TXYZ Point = default;
            if (Value < 0)
            {
                Value = 0;
            }

            if (Value < FHitPoints)
            {
                if (Value == 0)
                {
                    if (FPlaySound && AoWCombatMap.FPlaySounds)
                    {
                        PlaySample((AnsiString)"SFX\\HIT_DEATH\\D_WOODLG.WAV");
                    }

                    var call__58 = AoWCombatMap.AddEffect(TfxType.fxWallSmoke);
                    Effect = call__58;
                    Point = new TXYZ();
                    Effect.FPath.Add(Point);
                    HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Point.X, ref Point.Y);
                    var call__59 = Random(20);
                    Point.X = call__59 + Point.X + 6;
                    var call__60 = Random(20);
                    Point.Y = call__60 + Point.Y - 20;
                }
                else
                {
                    if (FPlaySound && AoWCombatMap.FPlaySounds)
                    {
                        PlaySample((AnsiString)"SFX\\HIT_DEATH\\D_WOODMD.WAV");
                    }
                }
            }

            FHitPoints = Value;
            FPlaySound = true;
        }

        // CombatStructure.pas:23


    }

    public partial class TCombatStructureResource : TCombatFixedILTerrainResource
    {
        public TCombatStructureResource(TECustomNode owner) : base(owner)
        {}

        // CombatStructure.pas:256
        public new const int ClassID = 2229511;

        // CombatStructure.pas:262
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229510;
            return Result;
        }

        // CombatStructure.pas:259
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat Structure";
            return Result;
        }

    }

    public static partial class CombatStructure
    {
        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
