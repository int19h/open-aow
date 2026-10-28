// Translated from CombatStructure.pas.
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
using Aow.Units.CombatRoad;
using static Aow.Units.CombatRoad.CombatRoad;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CombatStructure
{
    // { TCombatStructure (instance size 40, VMT 0043258C) }
    public partial class TCombatStructure : Aow.Units.CombatILTer.TCombatFixedILTerrain
    {
        public int FHitPoints;
        // { $20 }
        public bool FPlaySound;
        // { $24 }
        public Aow.Units.FixedArray_byte_3 F_CombatStructure_25;
        // CombatStructure.pas:51
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            SetHitPoints(5);
        }

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
                Result = unchecked((sbyte)-1);
            }

            return Result;
        }

        // CombatStructure.pas:104
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
                        var call__2 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(65);
                        call__2.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__3;
                        var call__4 = GetDefense();
                        var call__5 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__4);
                        SetHitPoints(FHitPoints - call__5);
                        break;
                    case 200705:
                        var call__6 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(24);
                        call__6.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__7;
                        var call__8 = GetDefense();
                        var call__9 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__8);
                        SetHitPoints(FHitPoints - call__9);
                        break;
                    case 200706:
                        var call__10 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(44);
                        call__10.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__11 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__11;
                        var call__12 = GetDefense();
                        var call__13 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__12);
                        SetHitPoints(FHitPoints - call__13);
                        break;
                    case 200707:
                        var call__14 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(117);
                        call__14.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                        var call__15 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__15;
                        var call__16 = GetDefense();
                        var call__17 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__16);
                        SetHitPoints(FHitPoints - call__17);
                        break;
                    case 200714:
                        var call__18 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(31);
                        call__18.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                        var call__19 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__19;
                        var call__20 = GetDefense();
                        var call__21 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__20);
                        SetHitPoints(FHitPoints - call__21);
                        break;
                    case 200715:
                        var call__22 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(113);
                        call__22.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                        var call__23 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__23;
                        var call__24 = GetDefense();
                        var call__25 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__24);
                        SetHitPoints(FHitPoints - call__25);
                        break;
                    case 200722:
                        if ((Msg.Param[1].Value == 30) || (Msg.Param[1].Value == 86))
                        {
                            var call__26 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Msg.Param[1].Value);
                            call__26.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref AbilityInfo);
                            var call__27 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__27;
                            var call__28 = GetDefense();
                            var call__29 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - call__28);
                            SetHitPoints(FHitPoints - call__29);
                        }

                        break;
                    case 200723:
                        if (Msg.Param[1].Value == 114)
                        {
                            var call__30 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Msg.Param[1].Value);
                            call__30.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                            var call__31 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__31;
                            var call__32 = GetDefense();
                            var call__33 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__32);
                            SetHitPoints(FHitPoints - call__33);
                        }

                        break;
                    case 200710:
                        var call__34 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(116);
                        call__34.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                        var call__35 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__35;
                        var call__36 = GetDefense();
                        var call__37 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__36);
                        SetHitPoints(FHitPoints - call__37);
                        break;
                    case 200717:
                        var call__38 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(127);
                        call__38.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                        var call__39 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__39;
                        var call__40 = GetDefense();
                        var call__41 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__40);
                        SetHitPoints(FHitPoints - call__41);
                        break;
                    case 200718:
                        var call__42 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(128);
                        call__42.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                        var call__43 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__43;
                        var call__44 = GetDefense();
                        var call__45 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__44);
                        SetHitPoints(FHitPoints - call__45);
                        break;
                    case 200708:
                        FPlaySound = Msg.Param[1].Value == 0;
                        var call__46 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(112);
                        call__46.GetCombatInfo(Msg.Param[0].As<Aow.Units.AoWE.TCombatUnit>().FUnit, ref SpellInfo);
                        var call__47 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__47;
                        var call__48 = GetDefense();
                        var call__49 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - call__48);
                        SetHitPoints(FHitPoints - call__49);
                        Msg.Param[1] = 1;
                        break;
                    case 200711:
                        var call__50 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__50;
                        var call__51 = GetDefense();
                        var call__52 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(Aow.Units.AoWTC.AoWTC.TCDamage[1], Aow.Units.AoWTC.AoWTC.TCAttack[1] - call__51);
                        SetHitPoints(FHitPoints - call__52);
                        break;
                }
            }
        }

        // CombatStructure.pas:216
        public override void CanMoveOn(in Aow.Units.HSEngine.THSMoveOnInfo Info, ref Aow.Units.HSEngine.THSMoveOnResult MoveResult)
        {
            var call__53 = FResource.TerrainList.GetUsed(Info.HN);
            if (call__53)
            {
                if (FHitPoints == 0)
                {
                    MoveResult.Code = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][6 - -1];
                }
                else
                {
                    var call__54 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
                    if (call__54)
                    {
                        MoveResult.Code = -1;
                    }
                    else
                    {
                        MoveResult.Code = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][8 - -1];
                    }
                }

                MoveResult.Done = true;
            }
        }

        // CombatStructure.pas:60
        public override void Activate()
        {
            Aow.Units.AoWE.TPlayerStructure Structure = default;
            bool Razed = default;
            base.Activate();
            if (!Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                FPlaySound = true;
                Razed = false;
                var call__55 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(Aow.Units.AoWTC.AoWTC.CombatLLoc);
                var call__56 = call__55.GetField(Aow.Units.AoWTC.AoWTC.CombatXLoc, Aow.Units.AoWTC.AoWTC.CombatYLoc);
                var call__57 = call__56.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TPlayerStructure));
                Structure = (Aow.Units.AoWE.TPlayerStructure)call__57;
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
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            base.UpdateMapField(Info);
            if (Info.HN == 0)
            {
                ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags = (((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags | 16384) | 8192;
            }
        }

        // CombatStructure.pas:253
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = base.CanPlace(MapContainer, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)Lhx));
            return Result;
        }

        // CombatStructure.pas:250
        public override bool CanPlaceOnHS(Aow.Units.HSEngine.THexagonSprite HS)
        {
            bool Result = default;
            Result = base.CanPlaceOnHS(HS) || (HS is Aow.Units.CombatRoad.TCombatRoad);
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
        public override void ShowShadow(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
        }

        // CombatStructure.pas:79
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
                    if (FPlaySound && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                    {
                        Aow.Units.Sound.Sound.PlaySample((AnsiString)"SFX\\HIT_DEATH\\D_WOODLG.WAV");
                    }

                    var call__58 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxWallSmoke);
                    Effect = call__58;
                    Point = Aow.Units.aowFX.TXYZ.Create();
                    Effect.FPath.Add(Point);
                    Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Point.X, ref Point.Y);
                    var call__59 = Aow.Rtl.Rtl.Random(20);
                    Point.X = (call__59 + Point.X) + 6;
                    var call__60 = Aow.Rtl.Rtl.Random(20);
                    Point.Y = (call__60 + Point.Y) - 20;
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
            FPlaySound = true;
        }

        // CombatStructure.pas:23
        public static new Aow.Units.CombatStructure.TCombatStructure Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.CombatStructure.TCombatStructure o = new Aow.Units.CombatStructure.TCombatStructure();
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

        public new partial class Meta : Aow.Units.CombatILTer.TCombatFixedILTerrain.Meta
        {
            // CombatStructure.pas:57
            public override int ClassID()
            {
                int Result = default;
                Result = 2229510;
                return Result;
            }

            // CombatStructure.pas:23
            public new Aow.Units.CombatStructure.TCombatStructure Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatStructure.TCombatStructure o = (Aow.Units.CombatStructure.TCombatStructure)NewInstance();
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

            public static new Meta Of<T>()
                where T : Aow.Units.CombatStructure.TCombatStructure => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TCombatStructureResource (instance size 72, VMT 00432750) }
    public partial class TCombatStructureResource : Aow.Units.CombatILTer.TCombatFixedILTerrainResource
    {
        // CombatStructure.pas:262
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229510;
            return Result;
        }

        public new partial class Meta : Aow.Units.CombatILTer.TCombatFixedILTerrainResource.Meta
        {
            // CombatStructure.pas:256
            public override int ClassID()
            {
                int Result = default;
                Result = 2229511;
                return Result;
            }

            // CombatStructure.pas:259
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Combat Structure";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatStructure.TCombatStructureResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.CombatStructure.TCombatStructureResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatStructure.TCombatStructureResource o = (Aow.Units.CombatStructure.TCombatStructureResource)NewInstance();
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
