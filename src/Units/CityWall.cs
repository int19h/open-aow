// Translated from CityWall.pas.
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
using Aow.Units.CombatILTer;
using static Aow.Units.CombatILTer.CombatILTer;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.Tower;
using static Aow.Units.Tower.Tower;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CityWall
{
    public partial class TCityWall : Aow.Units.CombatILTer.TCombatILTerrain
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
                Result = unchecked((sbyte)base.GetOverlay(Index));
            }

            return Result;
        }

        // CityWall.pas:230
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            Aow.Units.AoWE.TAbilityCombatInfo AbilityInfo = default;
            Aow.Units.AoWE.TSpellCombatInfo SpellInfo = default;
            base.MapFieldMsgProc(ref Msg);
            if (Msg.HN == 0)
            {
                switch (Msg.MsgID)
                {
                    case 200704:
                        var call__1 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(65);
                        call__1.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__2 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__2;
                        var call__3 = GetDefense();
                        var call__4 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__3);
                        SetHitPoints(FHitPoints - call__4);
                        break;
                    case 200705:
                        var call__5 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(24);
                        call__5.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__6;
                        var call__7 = GetDefense();
                        var call__8 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__7);
                        SetHitPoints(FHitPoints - call__8);
                        break;
                    case 200706:
                        var call__9 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(44);
                        call__9.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__10 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__10;
                        var call__11 = GetDefense();
                        var call__12 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__11);
                        SetHitPoints(FHitPoints - call__12);
                        break;
                    case 200707:
                        var call__13 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(117);
                        call__13.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                        var call__14 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__14;
                        var call__15 = GetDefense();
                        var call__16 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__15);
                        SetHitPoints(FHitPoints - call__16);
                        break;
                    case 200714:
                        if (FWallType == 1)
                        {
                            var call__17 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(31);
                            call__17.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                            var call__18 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__18;
                            var call__19 = GetDefense();
                            var call__20 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__19);
                            SetHitPoints(FHitPoints - call__20);
                        }

                        break;
                    case 200715:
                        if (FWallType == 1)
                        {
                            var call__21 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(113);
                            call__21.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                            var call__22 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__22;
                            var call__23 = GetDefense();
                            var call__24 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__23);
                            SetHitPoints(FHitPoints - call__24);
                        }

                        break;
                    case 200710:
                        var call__25 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(116);
                        call__25.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                        var call__26 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__26;
                        var call__27 = GetDefense();
                        var call__28 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__27);
                        SetHitPoints(FHitPoints - call__28);
                        break;
                    case 200717:
                        var call__29 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(127);
                        call__29.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                        var call__30 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__30;
                        var call__31 = GetDefense();
                        var call__32 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__31);
                        SetHitPoints(FHitPoints - call__32);
                        break;
                    case 200718:
                        if (FWallType == 1)
                        {
                            var call__33 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(128);
                            call__33.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                            var call__34 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__34;
                            var call__35 = GetDefense();
                            var call__36 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__35);
                            SetHitPoints(FHitPoints - call__36);
                        }

                        break;
                    case 200708:
                        FPlaySound = Msg.Param[1].Value == 0;
                        var call__37 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(112);
                        call__37.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                        var call__38 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__38;
                        var call__39 = GetDefense();
                        var call__40 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__39);
                        SetHitPoints(FHitPoints - call__40);
                        Msg.Param[1] = 1;
                        break;
                    case 200711:
                        if (FWallType == 1)
                        {
                            var call__41 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__41;
                            var call__42 = GetDefense();
                            var call__43 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(Aow.Units.AoWTC.AoWTC.TCDamage[1], Aow.Units.AoWTC.AoWTC.TCAttack[1] - call__42);
                            SetHitPoints(FHitPoints - call__43);
                        }

                        break;
                }
            }
        }

        // CityWall.pas:155
        public override void CanMoveOn(in Aow.Units.HSEngine.THSMoveOnInfo Info, ref Aow.Units.HSEngine.THSMoveOnResult MoveResult)
        {
            if (Info.HN == 0)
            {
                if (FHitPoints == 0)
                {
                    MoveResult.Code = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][6 - -1];
                }
                else
                {
                    if (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiWall))
                    {
                        MoveResult.Code = 25;
                        MoveResult.Done = true;
                    }
                    else
                    {
                        bool bool__44 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmWallCrushing);
                        if (bool__44)
                        {
                            var call__45 = GetXhx();
                            bool__44 = (call__45 == ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX);
                        }

                        bool bool__46 = bool__44;
                        if (bool__46)
                        {
                            var call__47 = GetYhx();
                            bool__46 = (call__47 == ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY);
                        }

                        if (bool__46)
                        {
                            MoveResult.Code = 0;
                        }
                        else
                        {
                            var call__48 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1);
                            bool bool__49 = call__48;
                            if (!bool__49)
                            {
                                var call__50 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66);
                                bool__49 = call__50;
                            }

                            bool bool__51 = bool__49;
                            if (!bool__51)
                            {
                                var call__52 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36);
                                bool__51 = call__52;
                            }

                            if (bool__51)
                            {
                                MoveResult.Code = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][-1 - -1];
                            }
                            else
                            {
                                var call__53 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
                                if (call__53)
                                {
                                    MoveResult.Code = -1;
                                }
                                else
                                {
                                    MoveResult.Code = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1];
                                }
                            }
                        }
                    }
                }

                MoveResult.Done = true;
            }
        }

        // CityWall.pas:180
        public override int CanMoveOver(in Aow.Units.HSEngine.THSSenderInfo Info)
        {
            int Result = default;
            if (Info.HN == 0)
            {
                bool bool__54 = (FHitPoints == 0);
                if (!bool__54)
                {
                    var call__55 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66);
                    bool__54 = call__55;
                }

                bool bool__56 = bool__54;
                if (!bool__56)
                {
                    var call__57 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36);
                    bool__56 = call__57;
                }

                if ((bool__56 || ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmClimbing)) || ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiWall))
                {
                    Result = 1;
                }
                else
                {
                    var call__58 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
                    if (call__58)
                    {
                        Result = 0;
                    }
                    else
                    {
                        if (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1] != 0)
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
            Aow.Units.AoWE.TPlayerStructure Structure = default;
            bool Razed = default;
            base.Activate();
            if (!Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                Razed = false;
                FPlaySound = true;
                var call__59 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(Aow.Units.AoWTC.AoWTC.CombatLLoc);
                var call__60 = call__59.GetField(Aow.Units.AoWTC.AoWTC.CombatXLoc, Aow.Units.AoWTC.AoWTC.CombatYLoc);
                var call__61 = call__60.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                Structure = (Aow.Units.AoWE.TPlayerStructure)call__61;
                if (Structure != null)
                {
                    FWallType = unchecked((byte)((Aow.Units.City.TCity)Structure).FWallType);
                    Razed = Structure.GetRazed();
                    if (Structure.FPlayer == -1)
                    {
                        FFlagID = -1;
                    }
                    else
                    {
                        var call__62 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer);
                        FFlagID = call__62.FPlayerRace;
                    }
                }
                else
                {
                    var call__63 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(Aow.Units.AoWTC.AoWTC.CombatLLoc);
                    var call__64 = call__63.GetField(Aow.Units.AoWTC.AoWTC.CombatXLoc, Aow.Units.AoWTC.AoWTC.CombatYLoc);
                    var call__65 = call__64.FindNoneTransparentHS(typeof(Aow.Units.Tower.TTower));
                    Structure = (Aow.Units.AoWE.TPlayerStructure)call__65;
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
                            var call__66 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer);
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
                    var call__69 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
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
                        SetHitPoints(Aow.Units.AoWTC.AoWTC.WallMaxHP[FWallType - 1]);
                    }

                    if (FFlagID == -1)
                    {
                        FFlagID = 32;
                    }

                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FWalls.Add(this);
                }
            }
            else
            {
                FWallType = 2;
            }
        }

        // CityWall.pas:147
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            base.UpdateMapField(Info);
            if (Info.HN == 0)
            {
                ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags = (((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags | 16384) | 8192;
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
        public override void ShowShadow(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
        }

        // CityWall.pas:130
        public byte GetDirection()
        {
            byte Result = default;
            Result = unchecked((byte)((Aow.Units.CityWall.TCityWallResource)FResource).FDirection);
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
            Aow.Units.AoWTC.TfxHS Effect = default;
            Aow.Units.aowFX.TXYZ Point = default;
            if (Value < 0)
            {
                Value = 0;
            }

            if (Value < FHitPoints)
            {
                if (Value == 0)
                {
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallBreached = true;
                    if (FPlaySound && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                    {
                        Aow.Units.Sound.Sound.PlaySample((AnsiString)"SFX\\HIT_DEATH\\D_WOODLG.WAV");
                    }

                    var call__70 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxWallSmoke);
                    Effect = call__70;
                    Point = new Aow.Units.aowFX.TXYZ();
                    Effect.FPath.Add(Point);
                    Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Point.X, ref Point.Y);
                    var call__71 = Aow.Rtl.Rtl.Random(20);
                    Point.X = (call__71 + Point.X) + 6;
                    var call__72 = Aow.Rtl.Rtl.Random(20);
                    Point.Y = (call__72 + Point.Y) - 20;
                }
                else
                {
                    if (FPlaySound && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                    {
                        Aow.Units.Sound.Sound.PlaySample((AnsiString)"SFX\\HIT_DEATH\\D_WOODMD.WAV");
                    }
                }
            }

            FHitPoints = Value;
            if (Aow.Units.AoWTC.AoWTC.WallMaxHP[FWallType - 1] == FHitPoints)
            {
                FDamageImage = 0;
            }
            else
            {
                if ((Aow.Units.AoWTC.AoWTC.WallMaxHP[FWallType - 1] / 2) < FHitPoints)
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

    public partial class TCityWallResource : Aow.Units.CombatILTer.TCombatILTerrainResource
    {
        public TCityWallResource(TECustomNode owner) : base(owner)
        {}

        // CityWall.pas:366
        public new const int ClassID = 2229509;

        public byte FDirection;
        // CityWall.pas:369
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
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
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Combat City Wall";
            return Result;
        }

    }

    public partial class TCityWallDoor : Aow.Units.CityWall.TCityWall
    {
        public TCityWallDoor(TECustomNode owner) : base(owner)
        {}

        // CityWall.pas:381
        public new const int ClassID = 2229510;

        // CityWall.pas:428
        public override sbyte GetOverlay(int Index)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-1);
            return Result;
        }

        // CityWall.pas:431
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.AoWTC.TTacticalCombatUnitHS CombatUnit = default;
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting) || (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat == null))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
            else
            {
                var call__73 = GetYhx();
                var call__74 = GetXhx();
                var call__75 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
                var call__76 = call__75.GetField(call__74, call__73);
                var call__77 = call__76.FindOwnedChild(2228484);
                CombatUnit = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__77;
                if (CombatUnit != null)
                {
                    var call__78 = CombatUnit.FUnit.GetSide();
                    if (call__78 != Aow.Units.AoWE.TCombatSideIndex.csDefender)
                    {
                        CombatUnit = null;
                    }
                }

                bool bool__79 = (FHitPoints == 0);
                if (!bool__79)
                {
                    var call__80 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FPlayers.IndexOfPlayer(unchecked((byte)unchecked((byte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentPlayer)));
                    var call__81 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)call__80));
                    bool__79 = ((call__81.FSideIndex != 1) && (CombatUnit == null));
                }

                if (bool__79)
                {
                    base.Show(XhpSurf, YhpSurf, SurfaceDesc);
                }
            }
        }

        // CityWall.pas:384
        public override void CanMoveOn(in Aow.Units.HSEngine.THSMoveOnInfo Info, ref Aow.Units.HSEngine.THSMoveOnResult MoveResult)
        {
            if (Info.HN == 0)
            {
                if (FHitPoints == 0)
                {
                    MoveResult.Code = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][6 - -1];
                }
                else
                {
                    if (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiWall))
                    {
                        MoveResult.Code = 25;
                        MoveResult.Done = true;
                    }
                    else
                    {
                        bool bool__82 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmWallCrushing);
                        if (bool__82)
                        {
                            var call__83 = GetXhx();
                            bool__82 = (call__83 == ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX);
                        }

                        bool bool__84 = bool__82;
                        if (bool__84)
                        {
                            var call__85 = GetYhx();
                            bool__84 = (call__85 == ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY);
                        }

                        if (bool__84)
                        {
                            MoveResult.Code = 0;
                        }
                        else
                        {
                            var call__86 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1);
                            bool bool__87 = call__86;
                            if (!bool__87)
                            {
                                var call__88 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66);
                                bool__87 = call__88;
                            }

                            bool bool__89 = bool__87;
                            if (!bool__89)
                            {
                                var call__90 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                                bool__89 = (call__90 == Aow.Units.AoWE.TCombatSideIndex.csDefender);
                            }

                            if (bool__89)
                            {
                                MoveResult.Code = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][-1 - -1];
                            }
                            else
                            {
                                var call__91 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
                                if (call__91)
                                {
                                    MoveResult.Code = -1;
                                }
                                else
                                {
                                    MoveResult.Code = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1];
                                }
                            }
                        }
                    }
                }

                MoveResult.Done = true;
            }
        }

        // CityWall.pas:409
        public override int CanMoveOver(in Aow.Units.HSEngine.THSSenderInfo Info)
        {
            int Result = default;
            if (Info.HN == 0)
            {
                bool bool__92 = (FHitPoints == 0);
                if (!bool__92)
                {
                    var call__93 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                    bool__92 = (call__93 == Aow.Units.AoWE.TCombatSideIndex.csDefender);
                }

                bool bool__94 = bool__92;
                if (!bool__94)
                {
                    var call__95 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66);
                    bool__94 = call__95;
                }

                bool bool__96 = bool__94;
                if (!bool__96)
                {
                    var call__97 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36);
                    bool__96 = call__97;
                }

                if ((bool__96 || ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmClimbing)) || ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiWall))
                {
                    Result = 1;
                }
                else
                {
                    var call__98 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
                    if (call__98)
                    {
                        Result = 0;
                    }
                    else
                    {
                        if (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1] != 0)
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

    public partial class TCityWallDoorResource : Aow.Units.CityWall.TCityWallResource
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
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Combat City Door";
            return Result;
        }

    }

    public static partial class CityWall
    {
        // CityWall.pas:460
        public static void RegisterCityWall(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.CityWall.TCityWall>(), Aow.Units.Engine.EClass.Of<Aow.Units.CityWall.TCityWallResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.CityWall.TCityWallDoor>(), Aow.Units.Engine.EClass.Of<Aow.Units.CityWall.TCityWallDoorResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
