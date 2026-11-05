// Translated from CityWall.pas.
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
using Aow.Units.City;
using Aow.Units.Tower;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CityWall
{
    public partial class TCityWall : TCombatILTerrain
    {
        public TCityWall(TECustomNode owner) : base(owner)
        {}

        // CityWall.pas:78
        public new const int ClassID = 2229508;

        public int FHitPoints;
        public int FDamageImage;
        public int FFlagID;
        public bool FPlaySound;
        public byte FWallType;
        // CityWall.pas:133
        public override sbyte GetOverlay(int Index)
        {
            sbyte Result = default;
            if (Index == 0)
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
                Result = base.GetOverlay(Index);
            }

            return Result;
        }

        // CityWall.pas:230
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
                        var call__1 = AoWHSSet.FAbilityControl.GetAbility(65);
                        call__1.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__2 = AoWHSMap.Random(16777215);
                        RandSeed = call__2;
                        var call__3 = GetDefense();
                        var call__4 = ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__3);
                        SetHitPoints(FHitPoints - call__4);
                        break;
                    case 200705:
                        var call__5 = AoWHSSet.FAbilityControl.GetAbility(24);
                        call__5.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__6 = AoWHSMap.Random(16777215);
                        RandSeed = call__6;
                        var call__7 = GetDefense();
                        var call__8 = ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__7);
                        SetHitPoints(FHitPoints - call__8);
                        break;
                    case 200706:
                        var call__9 = AoWHSSet.FAbilityControl.GetAbility(44);
                        call__9.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__10 = AoWHSMap.Random(16777215);
                        RandSeed = call__10;
                        var call__11 = GetDefense();
                        var call__12 = ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__11);
                        SetHitPoints(FHitPoints - call__12);
                        break;
                    case 200707:
                        var call__13 = AoWHSSet.FSpellControl.GetSpell(117);
                        call__13.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                        var call__14 = AoWHSMap.Random(16777215);
                        RandSeed = call__14;
                        var call__15 = GetDefense();
                        var call__16 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__15);
                        SetHitPoints(FHitPoints - call__16);
                        break;
                    case 200714:
                        if (FWallType == 1)
                        {
                            var call__17 = AoWHSSet.FAbilityControl.GetAbility(31);
                            call__17.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                            var call__18 = AoWHSMap.Random(16777215);
                            RandSeed = call__18;
                            var call__19 = GetDefense();
                            var call__20 = ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__19);
                            SetHitPoints(FHitPoints - call__20);
                        }

                        break;
                    case 200715:
                        if (FWallType == 1)
                        {
                            var call__21 = AoWHSSet.FSpellControl.GetSpell(113);
                            call__21.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                            var call__22 = AoWHSMap.Random(16777215);
                            RandSeed = call__22;
                            var call__23 = GetDefense();
                            var call__24 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__23);
                            SetHitPoints(FHitPoints - call__24);
                        }

                        break;
                    case 200710:
                        var call__25 = AoWHSSet.FSpellControl.GetSpell(116);
                        call__25.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                        var call__26 = AoWHSMap.Random(16777215);
                        RandSeed = call__26;
                        var call__27 = GetDefense();
                        var call__28 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__27);
                        SetHitPoints(FHitPoints - call__28);
                        break;
                    case 200717:
                        var call__29 = AoWHSSet.FSpellControl.GetSpell(127);
                        call__29.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                        var call__30 = AoWHSMap.Random(16777215);
                        RandSeed = call__30;
                        var call__31 = GetDefense();
                        var call__32 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__31);
                        SetHitPoints(FHitPoints - call__32);
                        break;
                    case 200718:
                        if (FWallType == 1)
                        {
                            var call__33 = AoWHSSet.FSpellControl.GetSpell(128);
                            call__33.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                            var call__34 = AoWHSMap.Random(16777215);
                            RandSeed = call__34;
                            var call__35 = GetDefense();
                            var call__36 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__35);
                            SetHitPoints(FHitPoints - call__36);
                        }

                        break;
                    case 200708:
                        FPlaySound = Msg.Param[1].Value == 0;
                        var call__37 = AoWHSSet.FSpellControl.GetSpell(112);
                        call__37.GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                        var call__38 = AoWHSMap.Random(16777215);
                        RandSeed = call__38;
                        var call__39 = GetDefense();
                        var call__40 = ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__39);
                        SetHitPoints(FHitPoints - call__40);
                        Msg.Param[1] = 1;
                        break;
                    case 200711:
                        if (FWallType == 1)
                        {
                            var call__41 = AoWHSMap.Random(16777215);
                            RandSeed = call__41;
                            var call__42 = GetDefense();
                            var call__43 = ExecuteDamageRole(TCDamage[1], TCAttack[1] - call__42);
                            SetHitPoints(FHitPoints - call__43);
                        }

                        break;
                }
            }
        }

        // CityWall.pas:155
        public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
        {
            if (Info.HN == 0)
            {
                if (FHitPoints == 0)
                {
                    MoveResult.Code = ((TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][6 - -1];
                }
                else
                {
                    if (((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmaiWall))
                    {
                        MoveResult.Code = 25;
                        MoveResult.Done = true;
                    }
                    else
                    {
                        bool bool__44 = ((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmWallCrushing);
                        if (bool__44)
                        {
                            var call__45 = GetXhx();
                            bool__44 = call__45 == ((TCombatMoveControl)Info.Sender).FDestinationX;
                        }

                        bool bool__46 = bool__44;
                        if (bool__46)
                        {
                            var call__47 = GetYhx();
                            bool__46 = call__47 == ((TCombatMoveControl)Info.Sender).FDestinationY;
                        }

                        if (bool__46)
                        {
                            MoveResult.Code = 0;
                        }
                        else
                        {
                            var call__48 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1);
                            bool bool__49 = call__48;
                            if (!bool__49)
                            {
                                var call__50 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66);
                                bool__49 = call__50;
                            }

                            bool bool__51 = bool__49;
                            if (!bool__51)
                            {
                                var call__52 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36);
                                bool__51 = call__52;
                            }

                            if (bool__51)
                            {
                                MoveResult.Code = ((TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][-1 - -1];
                            }
                            else
                            {
                                var call__53 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
                                if (call__53)
                                {
                                    MoveResult.Code = -1;
                                }
                                else
                                {
                                    MoveResult.Code = ((TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1];
                                }
                            }
                        }
                    }
                }

                MoveResult.Done = true;
            }
        }

        // CityWall.pas:180
        public override int CanMoveOver(in THSSenderInfo Info)
        {
            int Result = default;
            if (Info.HN == 0)
            {
                bool bool__54 = FHitPoints == 0;
                if (!bool__54)
                {
                    var call__55 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66);
                    bool__54 = call__55;
                }

                bool bool__56 = bool__54;
                if (!bool__56)
                {
                    var call__57 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36);
                    bool__56 = call__57;
                }

                if (bool__56 || ((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmClimbing) || ((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmaiWall))
                {
                    Result = 1;
                }
                else
                {
                    var call__58 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
                    if (call__58)
                    {
                        Result = 0;
                    }
                    else
                    {
                        if (((TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1] != 0)
                        {
                            Result = 1;
                        }
                        else
                        {
                            Result = 0;
                        }
                    }
                }
            }
            else
            {
                Result = 1;
            }

            return Result;
        }

        // CityWall.pas:81
        public override void Activate()
        {
            TPlayerStructure Structure = default;
            bool Razed = default;
            base.Activate();
            if (!AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                Razed = false;
                FPlaySound = true;
                var call__59 = AoWHSMap.MapContainer.GetMapLevel(CombatLLoc);
                var call__60 = call__59.GetField(CombatXLoc, CombatYLoc);
                var call__61 = call__60.FindNoneTransparentHS(typeof(TCity));
                Structure = (TPlayerStructure)call__61;
                if (Structure != null)
                {
                    FWallType = ((TCity)Structure).FWallType;
                    Razed = Structure.GetRazed();
                    if (Structure.FPlayer == -1)
                    {
                        FFlagID = -1;
                    }
                    else
                    {
                        var call__62 = AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer);
                        FFlagID = call__62.FPlayerRace;
                    }
                }
                else
                {
                    var call__63 = AoWHSMap.MapContainer.GetMapLevel(CombatLLoc);
                    var call__64 = call__63.GetField(CombatXLoc, CombatYLoc);
                    var call__65 = call__64.FindNoneTransparentHS(typeof(TTower));
                    Structure = (TPlayerStructure)call__65;
                    if (Structure != null)
                    {
                        FWallType = 1;
                        Razed = Structure.GetRazed();
                        if (Structure.FPlayer == -1)
                        {
                            FFlagID = -1;
                        }
                        else
                        {
                            var call__66 = AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer);
                            FFlagID = call__66.FPlayerRace;
                        }
                    }
                    else
                    {
                        FWallType = 0;
                    }
                }

                if (FWallType == 0)
                {
                    RemoveFromMap();
                    var call__67 = GetYhx();
                    var call__68 = GetXhx();
                    var call__69 = AoWCombatMap.MapContainer.GetMapLevel(0);
                    UpdateMapFields(call__69, call__68, call__67);
                    this.Free();
                }
                else
                {
                    if (Razed)
                    {
                        SetHitPoints(0);
                    }
                    else
                    {
                        SetHitPoints(WallMaxHP[FWallType - 1]);
                    }

                    if (FFlagID == -1)
                    {
                        FFlagID = 32;
                    }

                    AoWCombatMap.FWalls.Add(this);
                }
            }
            else
            {
                FWallType = 2;
            }
        }

        // CityWall.pas:147
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            base.UpdateMapField(Info);
            if (Info.HN == 0)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 16384 | 8192;
            }
        }

        // CityWall.pas:141
        public override bool ValidTerrainType(int TerrainType)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // CityWall.pas:144
        public override int GetTerrainTypeImage(int TerrainType)
        {
            int Result = default;
            Result = 0;
            return Result;
        }

        // CityWall.pas:363
        public override void ShowShadow(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
        }

        // CityWall.pas:130
        public byte GetDirection()
        {
            byte Result = default;
            Result = ((TCityWallResource)FResource).FDirection;
            return Result;
        }

        // CityWall.pas:197
        public new int GetDefense()
        {
            int Result = default;
            Result = -2;
            return Result;
        }

        // CityWall.pas:200
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
                    AoWCombatMap.FCombat.FCombatData.FWallBreached = true;
                    if (FPlaySound && AoWCombatMap.FPlaySounds)
                    {
                        PlaySample((AnsiString)"SFX\\HIT_DEATH\\D_WOODLG.WAV");
                    }

                    var call__70 = AoWCombatMap.AddEffect(TfxType.fxWallSmoke);
                    Effect = call__70;
                    Point = new TXYZ();
                    Effect.FPath.Add(Point);
                    HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Point.X, ref Point.Y);
                    var call__71 = Random(20);
                    Point.X = call__71 + Point.X + 6;
                    var call__72 = Random(20);
                    Point.Y = call__72 + Point.Y - 20;
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
            if (WallMaxHP[FWallType - 1] == FHitPoints)
            {
                FDamageImage = 0;
            }
            else
            {
                if (WallMaxHP[FWallType - 1] / 2 < FHitPoints)
                {
                    FDamageImage = 1;
                }
                else
                {
                    if (FHitPoints > 0)
                    {
                        FDamageImage = 2;
                    }
                    else
                    {
                        FDamageImage = 3;
                    }
                }
            }

            FPlaySound = true;
        }

    }

    public partial class TCityWallResource : TCombatILTerrainResource
    {
        public TCityWallResource(TECustomNode owner) : base(owner)
        {}

        // CityWall.pas:366
        public new const int ClassID = 2229509;

        public byte FDirection;
        // CityWall.pas:369
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref FDirection, 25);
        }

        // CityWall.pas:378
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229508;
            return Result;
        }

        // CityWall.pas:375
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat City Wall";
            return Result;
        }

    }

    public partial class TCityWallDoor : TCityWall
    {
        public TCityWallDoor(TECustomNode owner) : base(owner)
        {}

        // CityWall.pas:381
        public new const int ClassID = 2229510;

        // CityWall.pas:428
        public override sbyte GetOverlay(int Index)
        {
            sbyte Result = default;
            Result = -1;
            return Result;
        }

        // CityWall.pas:431
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TTacticalCombatUnitHS CombatUnit = default;
            if (AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting) || AoWCombatMap.FCombat == null)
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
            else
            {
                var call__73 = GetYhx();
                var call__74 = GetXhx();
                var call__75 = AoWCombatMap.MapContainer.GetMapLevel(0);
                var call__76 = call__75.GetField(call__74, call__73);
                var call__77 = call__76.FindOwnedChild(2228484);
                CombatUnit = (TTacticalCombatUnitHS)call__77;
                if (CombatUnit != null)
                {
                    var call__78 = CombatUnit.FUnit.GetSide();
                    if (call__78 != TCombatSideIndex.csDefender)
                    {
                        CombatUnit = null;
                    }
                }

                bool bool__79 = FHitPoints == 0;
                if (!bool__79)
                {
                    var call__80 = AoWCombatMap.FCombat.FCombatData.FPlayers.IndexOfPlayer((byte)AoWCombatMap.FCurrentPlayer);
                    var call__81 = AoWCombatMap.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)call__80);
                    bool__79 = call__81.FSideIndex != 1 && CombatUnit == null;
                }

                if (bool__79)
                {
                    base.Show(XhpSurf, YhpSurf, SurfaceDesc);
                }
            }
        }

        // CityWall.pas:384
        public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
        {
            if (Info.HN == 0)
            {
                if (FHitPoints == 0)
                {
                    MoveResult.Code = ((TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][6 - -1];
                }
                else
                {
                    if (((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmaiWall))
                    {
                        MoveResult.Code = 25;
                        MoveResult.Done = true;
                    }
                    else
                    {
                        bool bool__82 = ((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmWallCrushing);
                        if (bool__82)
                        {
                            var call__83 = GetXhx();
                            bool__82 = call__83 == ((TCombatMoveControl)Info.Sender).FDestinationX;
                        }

                        bool bool__84 = bool__82;
                        if (bool__84)
                        {
                            var call__85 = GetYhx();
                            bool__84 = call__85 == ((TCombatMoveControl)Info.Sender).FDestinationY;
                        }

                        if (bool__84)
                        {
                            MoveResult.Code = 0;
                        }
                        else
                        {
                            var call__86 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1);
                            bool bool__87 = call__86;
                            if (!bool__87)
                            {
                                var call__88 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66);
                                bool__87 = call__88;
                            }

                            bool bool__89 = bool__87;
                            if (!bool__89)
                            {
                                var call__90 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                                bool__89 = call__90 == TCombatSideIndex.csDefender;
                            }

                            if (bool__89)
                            {
                                MoveResult.Code = ((TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][-1 - -1];
                            }
                            else
                            {
                                var call__91 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
                                if (call__91)
                                {
                                    MoveResult.Code = -1;
                                }
                                else
                                {
                                    MoveResult.Code = ((TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1];
                                }
                            }
                        }
                    }
                }

                MoveResult.Done = true;
            }
        }

        // CityWall.pas:409
        public override int CanMoveOver(in THSSenderInfo Info)
        {
            int Result = default;
            if (Info.HN == 0)
            {
                bool bool__92 = FHitPoints == 0;
                if (!bool__92)
                {
                    var call__93 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                    bool__92 = call__93 == TCombatSideIndex.csDefender;
                }

                bool bool__94 = bool__92;
                if (!bool__94)
                {
                    var call__95 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66);
                    bool__94 = call__95;
                }

                bool bool__96 = bool__94;
                if (!bool__96)
                {
                    var call__97 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36);
                    bool__96 = call__97;
                }

                if (bool__96 || ((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmClimbing) || ((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmaiWall))
                {
                    Result = 1;
                }
                else
                {
                    var call__98 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
                    if (call__98)
                    {
                        Result = 0;
                    }
                    else
                    {
                        if (((TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1] != 0)
                        {
                            Result = 1;
                        }
                        else
                        {
                            Result = 0;
                        }
                    }
                }
            }
            else
            {
                Result = 1;
            }

            return Result;
        }

    }

    public partial class TCityWallDoorResource : TCityWallResource
    {
        public TCityWallDoorResource(TECustomNode owner) : base(owner)
        {}

        // CityWall.pas:451
        public new const int ClassID = 2229511;

        // CityWall.pas:457
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229510;
            return Result;
        }

        // CityWall.pas:454
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat City Door";
            return Result;
        }

    }

    public static partial class CityWall
    {
        // CityWall.pas:460
        public static void RegisterCityWall(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TCityWall>(), EClass.Of<TCityWallResource>(), EClass.Of<TCityWallDoor>(), EClass.Of<TCityWallDoorResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
