// Generated from CityWall.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
    // { TCityWall (instance size 48, VMT 00405008) }
    public partial class TCityWall : Aow.Units.CombatILTer.TCombatILTerrain
    {
        public int FHitPoints;
        // { $20 }
        public int FDamageImage;
        // { $24 }
        public int FFlagID;
        // { $28 }
        public bool FPlaySound;
        // { $2C }
        public byte FWallType;
        // { $2D }
        public Aow.Units.FixedArray_byte_2 F_CityWall_2E;
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

        // CityWall.pas:322
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
            var call__1 = GetYhx();
            var call__2 = GetXhx();
            var call__3 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
            var call__4 = call__3.GetField(call__2, call__1);
            switch (call__4.TerrainType)
            {
                case 3:
                    TerrainOffset = 10;
                    break;
                case 5:
                case 7:
                case 12:
                    TerrainOffset = 20;
                    break;
                default:
                    TerrainOffset = 0;
                    break;
            }

            var call__5 = FResource.ImageSequenceList.GetImageSequence(Aow.Units.AoWTC.AoWTC.WallISIndex[FWallType - 1] + (TerrainOffset + FDamageImage));
            Sequence = call__5;
            if (Sequence != null)
            {
                do
                {
                    switch (Sequence.Layer)
                    {
                        case 7:
                            var call__6 = Sequence.GetFrameIndex(Aow.Units.AoWTC.AoWTC.AoWCombatMap.Frame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                            ImageIndex = call__6;
                            if (ImageIndex != -1)
                            {
                                if (!Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
                                {
                                    var call__7 = GetXhx();
                                    var call__8 = GetYhx();
                                    Aow.Units.AoWE.AoWE.AoWHSSet.FFlagControl.ShowFlagEx((XhpSurf + 21) + Sequence.dXshift, (YhpSurf + Sequence.dYshift) + 16, (call__7 + Aow.Units.AoWE.AoWE.AoWHSMap.Frame) + (call__8 * 100), unchecked((byte)FFlagID), SurfaceDesc, false);
                                }
                            }

                            break;
                        default:
                        {
                            var call__9 = Sequence.GetUnloopedFrameIndex(FImageIndex);
                            ImageIndex = call__9;
                            if (ImageIndex != -1)
                            {
                                var call__10 = Sequence.ImageLibrary[ImageIndex];
                                call__10.Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                            }
                        }

                            break;
                    }

                    Sequence = Sequence.Next;
                }
                while (!(Sequence == null));
            }
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
                        var call__11 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(65);
                        call__11.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__12 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__12;
                        var call__13 = GetDefense();
                        var call__14 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__13);
                        SetHitPoints(FHitPoints - call__14);
                        break;
                    case 200705:
                        var call__15 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(24);
                        call__15.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__16 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__16;
                        var call__17 = GetDefense();
                        var call__18 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__17);
                        SetHitPoints(FHitPoints - call__18);
                        break;
                    case 200706:
                        var call__19 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(44);
                        call__19.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__20 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__20;
                        var call__21 = GetDefense();
                        var call__22 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__21);
                        SetHitPoints(FHitPoints - call__22);
                        break;
                    case 200707:
                        var call__23 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(117);
                        call__23.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                        var call__24 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__24;
                        var call__25 = GetDefense();
                        var call__26 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__25);
                        SetHitPoints(FHitPoints - call__26);
                        break;
                    case 200714:
                        if (FWallType == 1)
                        {
                            var call__27 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(31);
                            call__27.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                            var call__28 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__28;
                            var call__29 = GetDefense();
                            var call__30 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__29);
                            SetHitPoints(FHitPoints - call__30);
                        }

                        break;
                    case 200715:
                        if (FWallType == 1)
                        {
                            var call__31 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(113);
                            call__31.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                            var call__32 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__32;
                            var call__33 = GetDefense();
                            var call__34 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__33);
                            SetHitPoints(FHitPoints - call__34);
                        }

                        break;
                    case 200710:
                        var call__35 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(116);
                        call__35.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                        var call__36 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__36;
                        var call__37 = GetDefense();
                        var call__38 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__37);
                        SetHitPoints(FHitPoints - call__38);
                        break;
                    case 200717:
                        var call__39 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(127);
                        call__39.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                        var call__40 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__40;
                        var call__41 = GetDefense();
                        var call__42 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__41);
                        SetHitPoints(FHitPoints - call__42);
                        break;
                    case 200718:
                        if (FWallType == 1)
                        {
                            var call__43 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(128);
                            call__43.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                            var call__44 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__44;
                            var call__45 = GetDefense();
                            var call__46 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__45);
                            SetHitPoints(FHitPoints - call__46);
                        }

                        break;
                    case 200708:
                        FPlaySound = Msg.Param[1].Value == 0;
                        var call__47 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(112);
                        call__47.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                        var call__48 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__48;
                        var call__49 = GetDefense();
                        var call__50 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__49);
                        SetHitPoints(FHitPoints - call__50);
                        Msg.Param[1] = 1;
                        break;
                    case 200711:
                        if (FWallType == 1)
                        {
                            var call__51 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__51;
                            var call__52 = GetDefense();
                            var call__53 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(Aow.Units.AoWTC.AoWTC.TCDamage[1], Aow.Units.AoWTC.AoWTC.TCAttack[1] - call__52);
                            SetHitPoints(FHitPoints - call__53);
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
                        bool bool__54 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmWallCrushing);
                        if (bool__54)
                        {
                            var call__55 = GetXhx();
                            bool__54 = (call__55 == ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX);
                        }

                        bool bool__56 = bool__54;
                        if (bool__56)
                        {
                            var call__57 = GetYhx();
                            bool__56 = (call__57 == ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY);
                        }

                        if (bool__56)
                        {
                            MoveResult.Code = 0;
                        }
                        else
                        {
                            var call__58 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1);
                            bool bool__59 = call__58;
                            if (!bool__59)
                            {
                                var call__60 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66);
                                bool__59 = call__60;
                            }

                            bool bool__61 = bool__59;
                            if (!bool__61)
                            {
                                var call__62 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36);
                                bool__61 = call__62;
                            }

                            if (bool__61)
                            {
                                MoveResult.Code = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][-1 - -1];
                            }
                            else
                            {
                                var call__63 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
                                if (call__63)
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
                bool bool__64 = (FHitPoints == 0);
                if (!bool__64)
                {
                    var call__65 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66);
                    bool__64 = call__65;
                }

                bool bool__66 = bool__64;
                if (!bool__66)
                {
                    var call__67 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36);
                    bool__66 = call__67;
                }

                if ((bool__66 || ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmClimbing)) || ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiWall))
                {
                    Result = 1;
                }
                else
                {
                    var call__68 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
                    if (call__68)
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
                var call__69 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(Aow.Units.AoWTC.AoWTC.CombatLLoc);
                var call__70 = call__69.GetField(Aow.Units.AoWTC.AoWTC.CombatXLoc, Aow.Units.AoWTC.AoWTC.CombatYLoc);
                var call__71 = call__70.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                Structure = (Aow.Units.AoWE.TPlayerStructure)call__71;
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
                        var call__72 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer);
                        FFlagID = call__72.FPlayerRace;
                    }
                }
                else
                {
                    var call__73 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(Aow.Units.AoWTC.AoWTC.CombatLLoc);
                    var call__74 = call__73.GetField(Aow.Units.AoWTC.AoWTC.CombatXLoc, Aow.Units.AoWTC.AoWTC.CombatYLoc);
                    var call__75 = call__74.FindNoneTransparentHS(typeof(Aow.Units.Tower.TTower));
                    Structure = (Aow.Units.AoWE.TPlayerStructure)call__75;
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
                            var call__76 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer);
                            FFlagID = call__76.FPlayerRace;
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
                    var call__77 = GetYhx();
                    var call__78 = GetXhx();
                    var call__79 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
                    UpdateMapFields(call__79, call__78, call__77);
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

                    var call__80 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxWallSmoke);
                    Effect = call__80;
                    Point = Aow.Units.aowFX.TXYZ.Create();
                    Effect.FPath.Add(Point);
                    Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Point.X, ref Point.Y);
                    var call__81 = Aow.Rtl.Rtl.Random(20);
                    Point.X = (call__81 + Point.X) + 6;
                    var call__82 = Aow.Rtl.Rtl.Random(20);
                    Point.Y = (call__82 + Point.Y) - 20;
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

        public new partial class Meta : Aow.Units.CombatILTer.TCombatILTerrain.Meta
        {
            // CityWall.pas:78
            public override int ClassID()
            {
                int Result = default;
                Result = 2229508;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CityWall.TCityWall => (Meta)MetaRegistry.Of(typeof(T));
            // ILTer.pas:20
            public new Aow.Units.CityWall.TCityWall Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CityWall.TCityWall o = (Aow.Units.CityWall.TCityWall)NewInstance();
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

    // { TCityWallResource (instance size 76, VMT 004051B4) }
    public partial class TCityWallResource : Aow.Units.CombatILTer.TCombatILTerrainResource
    {
        public byte FDirection;
        // { $48 }
        public Aow.Units.FixedArray_byte_3 F_CityWallResource_49;
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

        public new partial class Meta : Aow.Units.CombatILTer.TCombatILTerrainResource.Meta
        {
            // CityWall.pas:366
            public override int ClassID()
            {
                int Result = default;
                Result = 2229509;
                return Result;
            }

            // CityWall.pas:375
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Combat City Wall";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CityWall.TCityWallResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.CityWall.TCityWallResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CityWall.TCityWallResource o = (Aow.Units.CityWall.TCityWallResource)NewInstance();
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

    // { TCityWallDoor (instance size 48, VMT 004052AC) }
    public partial class TCityWallDoor : Aow.Units.CityWall.TCityWall
    {
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
                var call__83 = GetYhx();
                var call__84 = GetXhx();
                var call__85 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
                var call__86 = call__85.GetField(call__84, call__83);
                var call__87 = call__86.FindOwnedChild(2228484);
                CombatUnit = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__87;
                if (CombatUnit != null)
                {
                    var call__88 = CombatUnit.FUnit.GetSide();
                    if (call__88 != Aow.Units.AoWE.TCombatSideIndex.csDefender)
                    {
                        CombatUnit = null;
                    }
                }

                bool bool__89 = (FHitPoints == 0);
                if (!bool__89)
                {
                    var call__90 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FPlayers.IndexOfPlayer(unchecked((byte)unchecked((byte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentPlayer)));
                    var call__91 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)call__90));
                    bool__89 = ((call__91.FSideIndex != 1) && (CombatUnit == null));
                }

                if (bool__89)
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
                        bool bool__92 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmWallCrushing);
                        if (bool__92)
                        {
                            var call__93 = GetXhx();
                            bool__92 = (call__93 == ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX);
                        }

                        bool bool__94 = bool__92;
                        if (bool__94)
                        {
                            var call__95 = GetYhx();
                            bool__94 = (call__95 == ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY);
                        }

                        if (bool__94)
                        {
                            MoveResult.Code = 0;
                        }
                        else
                        {
                            var call__96 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1);
                            bool bool__97 = call__96;
                            if (!bool__97)
                            {
                                var call__98 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66);
                                bool__97 = call__98;
                            }

                            bool bool__99 = bool__97;
                            if (!bool__99)
                            {
                                var call__100 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                                bool__99 = (call__100 == Aow.Units.AoWE.TCombatSideIndex.csDefender);
                            }

                            if (bool__99)
                            {
                                MoveResult.Code = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][-1 - -1];
                            }
                            else
                            {
                                var call__101 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
                                if (call__101)
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
                bool bool__102 = (FHitPoints == 0);
                if (!bool__102)
                {
                    var call__103 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                    bool__102 = (call__103 == Aow.Units.AoWE.TCombatSideIndex.csDefender);
                }

                bool bool__104 = bool__102;
                if (!bool__104)
                {
                    var call__105 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(66);
                    bool__104 = call__105;
                }

                bool bool__106 = bool__104;
                if (!bool__106)
                {
                    var call__107 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(36);
                    bool__106 = call__107;
                }

                if ((bool__106 || ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmClimbing)) || ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiWall))
                {
                    Result = 1;
                }
                else
                {
                    var call__108 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
                    if (call__108)
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

        public new partial class Meta : Aow.Units.CityWall.TCityWall.Meta
        {
            // CityWall.pas:381
            public override int ClassID()
            {
                int Result = default;
                Result = 2229510;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CityWall.TCityWallDoor => (Meta)MetaRegistry.Of(typeof(T));
            // ILTer.pas:20
            public new Aow.Units.CityWall.TCityWallDoor Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CityWall.TCityWallDoor o = (Aow.Units.CityWall.TCityWallDoor)NewInstance();
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

    // { TCityWallDoorResource (instance size 76, VMT 00405460) }
    public partial class TCityWallDoorResource : Aow.Units.CityWall.TCityWallResource
    {
        // CityWall.pas:457
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229510;
            return Result;
        }

        public new partial class Meta : Aow.Units.CityWall.TCityWallResource.Meta
        {
            // CityWall.pas:451
            public override int ClassID()
            {
                int Result = default;
                Result = 2229511;
                return Result;
            }

            // CityWall.pas:454
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Combat City Door";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CityWall.TCityWallDoorResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.CityWall.TCityWallDoorResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CityWall.TCityWallDoorResource o = (Aow.Units.CityWall.TCityWallDoorResource)NewInstance();
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

    public static partial class CityWall
    {
        // CityWall.pas:460
        public static void RegisterCityWall(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CityWall.TCityWall>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CityWall.TCityWallResource>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CityWall.TCityWallDoor>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CityWall.TCityWallDoorResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
