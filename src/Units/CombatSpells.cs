// Translated from CombatSpells.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
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

namespace Aow.Units.CombatSpells
{
    // { TCombatSpellCA (instance size 32, VMT 557F3CC8):
//     $18 spell ID, $1C casting mana; source and target IDs inherited at $0D/$0E. }
    public partial class TCombatSpellCA : Aow.Units.AoWE.TDamageCA
    {
        // CombatSpells.pas:447
        public new const int ClassID = 131778;

        public TCombatSpellCA(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        public int FSpellID;
        public int FManaCost;
        // CombatSpells.pas:452
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FSpellID, 30);
            Stream.rwInteger(ref FManaCost, 31);
        }

        // CombatSpells.pas:459
        public override void Execute(Aow.Units.AoWE.TCombat A1)
        {
            Aow.Units.AoWE.TCombatObject Source = default;
            base.Execute(A1);
            if ((FManaCost != 0) && A1.FStatus.Contains(Aow.Units.AoWE.TCombatStatusElement.cExecuting))
            {
                var call__1 = A1.FCombatData.FindID(FSourceID);
                Source = call__1;
                var call__2 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__2.CombatCastingDone(Source, FManaCost);
            }
        }

        // CombatSpells.pas:471
        public override bool Play(Aow.Units.AoWE.TCombatViewer A1)
        {
            bool Result = default;
            if (A1.FTime == 0)
            {
                var call__3 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__3.FCastSounds.Play(0);
            }

            if (A1.FDelay == A1.FTime)
            {
                Execute(A1.FCombat);
            }

            Result = (A1.FDelay * 2) > A1.FTime;
            return Result;
        }

        // CombatSpells.pas:479
        public override void ShowObject(Aow.Units.AoWE.TCombatViewer A1, Aow.Units.AoWE.TCombatObject A2, int A3, int A4, int A5, int A6, in Aow.Units.GFXE.TSurfaceDesc A7)
        {
            base.ShowObject(A1, A2, A3, A4, A5, A6, A7);
            if (A2.FID == FSourceID)
            {
                var call__4 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                Aow.Units.AoWE.AoWE.AoWEngine.FCombatFont.SetTextCentered(A5 + 32, (A6 + 32) + 8, call__4.FSpellName, A7);
            }
            else
            {
                if (A2.FID == FTargetID)
                {
                    var call__5 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__5.FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
                }
            }
        }

        // CombatSpells.pas:490
        public override void ShowFieldObject(Aow.Units.AoWE.TCombatViewer A1, Aow.Units.AoWE.TCombatObject A2, int A3, int A4, int A5, int A6, in Aow.Units.GFXE.TSurfaceDesc A7)
        {
            // { Original calls ShowObject here, bypassing inherited field markers. }
            base.ShowObject(A1, A2, A3, A4, A5, A6, A7);
            if (A2.FID == FSourceID)
            {
                var call__6 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                Aow.Units.AoWE.AoWE.AoWEngine.FCombatFont.SetTextCentered(A5 + 32, (A6 + 32) + 8, call__6.FSpellName, A7);
            }
            else
            {
                if (A2.FID == FTargetID)
                {
                    var call__7 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__7.FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
                }
            }
        }


    }

    // { TExclusiveCombatSpellCA (instance size 28, VMT 557F3DB8):
//     $0C source ID, $0D target ID, $10 casting mana, $14 success, $18 spell ID. }
    public partial class TExclusiveCombatSpellCA : Aow.Units.AoWE.TCombatAction
    {
        public TExclusiveCombatSpellCA(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        public byte FSourceID;
        public byte FTargetID;
        public Aow.Units.FixedArray_byte_2 F_ExclusiveCombatSpellCA_0E;
        public int FManaCost;
        public bool FSucceeded;
        public Aow.Units.FixedArray_byte_3 F_ExclusiveCombatSpellCA_15;
        public int FSpellID;
        // CombatSpells.pas:502
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref FSourceID, 20);
            Stream.rwByte(ref FTargetID, 21);
            Stream.rwInteger(ref FManaCost, 22);
            Stream.rwBool(ref FSucceeded, 23);
            Stream.rwInteger(ref FSpellID, 24);
        }

        // CombatSpells.pas:512
        public override void Execute(Aow.Units.AoWE.TCombat A1)
        {
            Aow.Units.AoWE.TCombatObject Source = default;
            if ((FManaCost != 0) && A1.FStatus.Contains(Aow.Units.AoWE.TCombatStatusElement.cExecuting))
            {
                var call__9 = A1.FCombatData.FindID(FSourceID);
                Source = call__9;
                var call__10 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__10.CombatCastingDone(Source, FManaCost);
            }
        }

        // CombatSpells.pas:524
        public override bool Play(Aow.Units.AoWE.TCombatViewer A1)
        {
            bool Result = default;
            if (A1.FTime == 0)
            {
                var call__11 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__11.FCastSounds.Play(0);
            }

            if (A1.FDelay == A1.FTime)
            {
                Execute(A1.FCombat);
            }

            Result = (A1.FDelay * 3) > A1.FTime;
            return Result;
        }

        // CombatSpells.pas:532
        public override void ShowObject(Aow.Units.AoWE.TCombatViewer A1, Aow.Units.AoWE.TCombatObject A2, int A3, int A4, int A5, int A6, in Aow.Units.GFXE.TSurfaceDesc A7)
        {
            if (A2.FID == FSourceID)
            {
                A2.ShowAttackAnimation(A1.FTime, A4, A5, A6, A7);
            }
            else
            {
                base.ShowObject(A1, A2, A3, A4, A5, A6, A7);
            }

            if (A2.FID == FSourceID)
            {
                var call__12 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                Aow.Units.AoWE.AoWE.AoWEngine.FCombatFont.SetTextCentered(A5 + 32, (A6 + 32) + 8, call__12.FSpellName, A7);
            }
            else
            {
                if (A2.FID == FTargetID)
                {
                    var call__13 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__13.FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
                }
            }
        }

        // CombatSpells.pas:545
        public override void ShowFieldObject(Aow.Units.AoWE.TCombatViewer A1, Aow.Units.AoWE.TCombatObject A2, int A3, int A4, int A5, int A6, in Aow.Units.GFXE.TSurfaceDesc A7)
        {
            // { Original calls ShowObject here, bypassing inherited field markers. }
            base.ShowObject(A1, A2, A3, A4, A5, A6, A7);
            if (A2.FID == FSourceID)
            {
                var call__14 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                Aow.Units.AoWE.AoWE.AoWEngine.FCombatFont.SetTextCentered(A5 + 32, (A6 + 32) + 8, call__14.FSpellName, A7);
            }
            else
            {
                if (A2.FID == FTargetID)
                {
                    var call__15 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__15.FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
                }
            }
        }
    }

    // { TMultiTargetCombatSpellCA (instance size 32, VMT 557F3EAC):
//     $0C source ID, $10 target byte list, $14 casting mana, $18 success, $1C spell ID. }
    public partial class TMultiTargetCombatSpellCA : Aow.Units.AoWE.TCombatAction
    {
        public byte FSourceID;
        public Aow.Units.FixedArray_byte_3 F_MultiTargetCombatSpellCA_0D;
        public Aow.Units.Engine.TByteList FTargetIDs;
        public int FManaCost;
        public bool FSucceeded;
        public Aow.Units.FixedArray_byte_3 F_MultiTargetCombatSpellCA_19;
        public int FSpellID;
        // CombatSpells.pas:563
        public override void Destroy()
        {
            FTargetIDs.Free();
            base.Destroy();
        }

        // CombatSpells.pas:569
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref FSourceID, 20);
            Stream.rwStreamProc(21, FTargetIDs.LoadFromStream, FTargetIDs.SaveToStream);
            Stream.rwInteger(ref FManaCost, 22);
            Stream.rwBool(ref FSucceeded, 23);
            Stream.rwInteger(ref FSpellID, 24);
        }

        // CombatSpells.pas:557
        public TMultiTargetCombatSpellCA(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__16 = new Aow.Units.Engine.TByteList();
            FTargetIDs = call__16;
        }

        // CombatSpells.pas:579
        public override void Execute(Aow.Units.AoWE.TCombat A1)
        {
            Aow.Units.AoWE.TCombatObject Source = default;
            if ((FManaCost != 0) && A1.FStatus.Contains(Aow.Units.AoWE.TCombatStatusElement.cExecuting))
            {
                var call__18 = A1.FCombatData.FindID(FSourceID);
                Source = call__18;
                var call__19 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__19.CombatCastingDone(Source, FManaCost);
            }
        }

        // CombatSpells.pas:591
        public override bool Play(Aow.Units.AoWE.TCombatViewer A1)
        {
            bool Result = default;
            if (A1.FTime == 0)
            {
                var call__20 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__20.FCastSounds.Play(0);
            }

            if (A1.FDelay == A1.FTime)
            {
                Execute(A1.FCombat);
            }

            Result = (A1.FDelay * 5) > A1.FTime;
            return Result;
        }

        // CombatSpells.pas:599
        public override void ShowObject(Aow.Units.AoWE.TCombatViewer A1, Aow.Units.AoWE.TCombatObject A2, int A3, int A4, int A5, int A6, in Aow.Units.GFXE.TSurfaceDesc A7)
        {
            if (A2.FID == FSourceID)
            {
                A2.ShowAttackAnimation(A1.FTime, A4, A5, A6, A7);
            }
            else
            {
                base.ShowObject(A1, A2, A3, A4, A5, A6, A7);
            }

            if (A2.FID == FSourceID)
            {
                var call__21 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                Aow.Units.AoWE.AoWE.AoWEngine.FCombatFont.SetTextCentered(A5 + 32, (A6 + 32) + 8, call__21.FSpellName, A7);
            }
            else
            {
                var call__22 = FTargetIDs.IndexOf(unchecked((byte)A2.FID));
                if (call__22 != -1)
                {
                    var call__23 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__23.FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
                }
            }
        }

        // CombatSpells.pas:612
        public override void ShowFieldObject(Aow.Units.AoWE.TCombatViewer A1, Aow.Units.AoWE.TCombatObject A2, int A3, int A4, int A5, int A6, in Aow.Units.GFXE.TSurfaceDesc A7)
        {
            // { Original calls ShowObject here, bypassing inherited field markers. }
            base.ShowObject(A1, A2, A3, A4, A5, A6, A7);
            if (A2.FID == FSourceID)
            {
                var call__24 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                Aow.Units.AoWE.AoWE.AoWEngine.FCombatFont.SetTextCentered(A5 + 32, (A6 + 32) + 8, call__24.FSpellName, A7);
            }
            else
            {
                var call__25 = FTargetIDs.IndexOf(unchecked((byte)A2.FID));
                if (call__25 != -1)
                {
                    var call__26 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__26.FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
                }
            }
        }

        // CombatSpells.pas:624
        public override void ShowFightingObjects(Aow.Units.AoWE.TCombatViewer A1, int A2, int A3, int A4, in Aow.Units.GFXE.TSurfaceDesc A5)
        {
            Aow.Units.AoWE.TCombatObject Source = default;
            var call__27 = A1.FCombat.FCombatData.FindID(FSourceID);
            Source = call__27;
            A1.DefaultShowFightingObjects(null, Source, A2, A3, A4, A5);
        }

    }

    // { TCombatSpell (instance size 64, VMT 557F3FA4):
//     $34 combat statistics (attack, damage, types, repeats, range, radius, targets);
//     $3C damage mode, $3D padding. }
    public partial class TCombatSpell : Aow.Units.AoWE.TSpell
    {
        public Aow.Units.AoWE.TSpellCombatInfo FCombatInfo;
        // { $34 }
        public byte FDamageMode;
        // { $3C }
        public Aow.Units.FixedArray_byte_3 F_CombatSpell_3D;
        // CombatSpells.pas:632
        private protected TCombatSpell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            FSpellType = Aow.Units.AoWE.TSpellType.stCombat;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
            FCombatInfo.Radius = 1;
            FCombatInfo.TargetTypes = TSet<Aow.Units.AoWE.TCombatTargetTypeElement>.Of(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy);
            FCastDuration = 5;
            FDamageMode = 0;
        }

        // CombatSpells.pas:655
        public override Aow.Units.AoWE.TCombatAction CreateCA(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, bool A3)
        {
            Aow.Units.AoWE.TCombatAction Result = default;
            Aow.Units.CombatSpells.TCombatSpellCA Action = default;
            Action = new Aow.Units.CombatSpells.TCombatSpellCA(null);
            Action.FSpellID = FSpellID;
            Action.GenerateEx(A1, A2, unchecked((sbyte)FCombatInfo.Attack), unchecked((sbyte)FCombatInfo.Damage), FCombatInfo.DamageTypes, unchecked((byte)FDamageMode));
            if (A3)
            {
                var call__28 = ((Aow.Units.AoWE.THero)((Aow.Units.AoWE.TCombatUnit)A1).FUnit).CastingMana(this);
                Action.FManaCost = call__28;
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:643
        public override void GetCombatInfo(Aow.Units.AoWE.TAbstractUnit Source, ref Aow.Units.AoWE.TSpellCombatInfo Info)
        {
            Info = FCombatInfo;
        }

        // CombatSpells.pas:648
        public override int GetCombatDamageValue(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2)
        {
            int Result = default;
            var call__29 = A2.GetHitPoints();
            var call__30 = A2.GetDefense();
            var call__31 = A2.GetResistance();
            var call__32 = A2.GetImmunityTypes();
            var call__33 = A2.GetProtectionTypes();
            var call__34 = Aow.Units.AoWE.AoWE.StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, call__29, call__30, call__31, call__32, call__33);
            Result = call__34 * FCombatInfo.RepeatCount;
            return Result;
        }

        // CombatSpells.pas:668
        public override void GetCombatDamageValueEx(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, ref Aow.Units.AoWE.TAbilityDamageValues A3)
        {
            TSet<Aow.Units.AoWE.TDamageType> Protection = default;
            TSet<Aow.Units.AoWE.TDamageType> Immunity = default;
            Protection = A2.GetProtectionTypes();
            Immunity = A2.GetImmunityTypes();
            A3.DamageTypes = FCombatInfo.DamageTypes;
            var call__35 = A2.GetHitPoints();
            var call__36 = A2.GetDefense();
            var call__37 = A2.GetResistance();
            var call__38 = Aow.Units.AoWE.AoWE.StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, call__35, call__36, call__37, Immunity, Protection);
            A3.SourceDamage = call__38 * FCombatInfo.RepeatCount;
            // { Original recomputes the value, using resistance in place of defense for mode 1. }
            if (FDamageMode == 1)
            {
                var call__39 = A2.GetHitPoints();
                var call__40 = A2.GetResistance();
                var call__41 = A2.GetResistance();
                var call__42 = Aow.Units.AoWE.AoWE.StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, call__39, call__40, call__41, Immunity, Protection);
                A3.SourceDamage = call__42 * FCombatInfo.RepeatCount;
            }
            else
            {
                var call__43 = A2.GetHitPoints();
                var call__44 = A2.GetDefense();
                var call__45 = A2.GetResistance();
                var call__46 = Aow.Units.AoWE.AoWE.StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, call__43, call__44, call__45, Immunity, Protection);
                A3.SourceDamage = call__46 * FCombatInfo.RepeatCount;
            }

            var call__47 = Aow.Units.AoWE.AoWE.StatisticsToMaxDamage(FCombatInfo.Damage, FCombatInfo.DamageTypes, Protection, Immunity);
            A3.SourceCost = call__47 * FCombatInfo.RepeatCount;
            A3.TargetDamage = 0;
            A3.TargetCost = 0;
        }

        // CombatSpells.pas:754
        public override void fcPrefetchCombatCommands(Aow.Units.AoWE.TFastCombatCommandList A1)
        {
            Aow.Units.AoWE.TAbstractUnit Hero = default;
            int Distance = default;
            int I = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWE.TCombatObject Target = default;
            Aow.Units.AoWE.TSpellCC Command = default;
            Aow.Units.AoWE.TAbilityDamageValues Values = default;
            Hero = ((Aow.Units.AoWE.TCombatUnit)A1.FSource).FUnit;
            bool bool__48 = (Hero is Aow.Units.AoWE.THero);
            if (bool__48)
            {
                var call__49 = ((Aow.Units.AoWE.THero)Hero).CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason);
                bool__48 = call__49;
            }

            if (bool__48)
            {
                var call__50 = A1.FCombat.GetRoundDistance();
                Distance = call__50;
                if (FCombatInfo.Range == -1)
                {
                    Distance += 8;
                }
                else
                {
                    if (FCombatInfo.Range >= 12)
                    {
                        Distance += 4;
                    }
                    else
                    {
                        if (FCombatInfo.Range >= 8)
                        {
                            Distance += 2;
                        }
                    }
                }

                {
                    I = 0;
                    int end__51 = A1.FTargetSide.GetCount() - 1;
                    for (; I <= end__51; I += 1)
                    {
                        var call__52 = A1.FTargetSide.GetObjects(I);
                        Target = call__52;
                        var call__53 = Target.GetEnabled();
                        bool bool__54 = call__53;
                        if (bool__54)
                        {
                            var call__55 = Target.fcRoundDistance();
                            bool__54 = (call__55 <= Distance);
                        }

                        if (bool__54)
                        {
                            var call__56 = ((Aow.Units.AoWE.TFastCombat)A1.FSource.FCombat).GetWallSituation(A1.FSource, Target);
                            fcGetDamageValueEx(A1.FSource, Target, call__56, ref Values);
                            if (Values.SourceDamage != 0)
                            {
                                Command = new Aow.Units.AoWE.TSpellCC(52);
                                var call__57 = A1.FTargetSide.GetObjects(I);
                                Command.FTarget = call__57;
                                Command.FMoveCost = 1;
                                Command.FSpellID = FSpellID;
                                var call__58 = Aow.Units.AoWE.AoWE.DVtoDEV(Values.SourceDamage, Values.TargetDamage, A1.FSource, Target);
                                Command.FExpectedDV = call__58;
                                A1.Add(Command);
                            }
                        }
                    }
                }
            }
        }

        // CombatSpells.pas:791
        public override bool fcExecuteCombatCommand(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TFastCombatCommand A2)
        {
            bool Result = default;
            int I = default;
            int J = default;
            int Count = default;
            Aow.Units.AoWE.TCombatAction Action = default;
            Aow.Units.AoWE.TCombatObject Other = default;
            int Distance = default;
            Aow.Units.AoWE.TCombatObject Target = default;
            Aow.Rtl.TList Targets = default;
            Aow.Units.AoWE.TFastCombat Combat = default;
            Target = A2.FTarget;
            {
                I = 0;
                int end__59 = FCombatInfo.RepeatCount - 1;
                for (; I <= end__59; I += 1)
                {
                    if (!Target.FStatus.Contains(Aow.Units.AoWE.TCombatObjectStatusElement.csDestroyed))
                    {
                        Action = CreateCA(A1, Target, I == 0);
                        A1.FCombat.ExecuteCombatAction(Action);
                        Action.Release();
                    }
                }
            }

            if (FCombatInfo.Radius > 1)
            {
                Combat = (Aow.Units.AoWE.TFastCombat)A1.FCombat;
                Targets = new Aow.Rtl.TList();
                Distance = Combat.GetRoundDistance();
                if (FCombatInfo.Range == -1)
                {
                    Distance += 8;
                }
                else
                {
                    if (FCombatInfo.Range >= 12)
                    {
                        Distance += 4;
                    }
                    else
                    {
                        if (FCombatInfo.Range >= 8)
                        {
                            Distance += 2;
                        }
                    }
                }

                {
                    I = Combat.FSides[(int)Target.GetSide()].GetCount() - 1;
                    byte end__60 = 0;
                    for (; I >= end__60; I -= 1)
                    {
                        var call__61 = Target.GetSide();
                        var call__62 = Combat.FSides[(int)call__61].GetObjects(I);
                        Other = call__62;
                        bool bool__63 = (Other != Target);
                        if (bool__63)
                        {
                            var call__64 = Other.GetEnabled();
                            bool__63 = call__64;
                        }

                        bool bool__65 = bool__63 && (((int)(((uint)Other.FPosition) >> 4)) == ((int)(((uint)Target.FPosition) >> 4)));
                        if (bool__65)
                        {
                            var call__66 = Other.fcRoundDistance();
                            bool__65 = (Distance >= call__66);
                        }

                        bool bool__67 = bool__65;
                        if (bool__67)
                        {
                            var call__68 = Other.GetImmunityTypes();
                            bool__67 = ((FCombatInfo.DamageTypes - call__68) != TSet<Aow.Units.AoWE.TDamageType>.Of());
                        }

                        if (bool__67)
                        {
                            Targets.Add(Other);
                        }
                    }
                }

                // { Original shifts Random(4) by two, so this adds no random extra targets. }
                var call__69 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(4);
                Count = ((int)(((uint)call__69) >> 2)) + FCombatInfo.Radius;
                while ((Count != 0) && (Targets.Count != 0))
                {
                    var call__70 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(Targets.Count);
                    var call__71 = Targets[call__70];
                    Other = (Aow.Units.AoWE.TCombatObject)call__71;
                    {
                        J = 0;
                        int end__72 = FCombatInfo.RepeatCount - 1;
                        for (; J <= end__72; J += 1)
                        {
                            if (!Other.FStatus.Contains(Aow.Units.AoWE.TCombatObjectStatusElement.csDestroyed))
                            {
                                Action = CreateCA(A1, Other, false);
                                A1.FCombat.ExecuteCombatAction(Action);
                                Action.Release();
                            }
                        }
                    }

                    Targets.Remove(Other);
                    Count -= 1;
                }

                Targets.Free();
            }

            Result = true;
            return Result;
        }

        // CombatSpells.pas:693
        public override void fcGetDamageValueEx(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, Aow.Units.AoWE.TWallSituation A3, ref Aow.Units.AoWE.TAbilityDamageValues A4)
        {
            TSet<Aow.Units.AoWE.TDamageType> DamageTypes = default;
            TSet<Aow.Units.AoWE.TDamageType> Protection = default;
            TSet<Aow.Units.AoWE.TDamageType> Immunity = default;
            int Count = default;
            int I = default;
            int Distance = default;
            Aow.Units.AoWE.TCombatObject Other = default;
            Aow.Units.AoWE.TFastCombat Combat = default;
            int Damage = default;
            int Defense = default;
            if (((A3 == Aow.Units.AoWE.TWallSituation.wsNone) || (FCombatInfo.Range == -1)) || (FCombatInfo.Range >= 8))
            {
                Protection = A2.GetProtectionTypes();
                Immunity = A2.GetImmunityTypes();
                DamageTypes = FCombatInfo.DamageTypes;
                Damage = FCombatInfo.Damage;
                Defense = A2.GetDefense();
                if ((A3 == Aow.Units.AoWE.TWallSituation.wsTarget) && ((TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtMagic, Aow.Units.AoWE.TDamageType.dtDeath, Aow.Units.AoWE.TDamageType.dtHoly) & FCombatInfo.DamageTypes) == TSet<Aow.Units.AoWE.TDamageType>.Of()))
                {
                    Defense += 2;
                }

                A4.DamageTypes = DamageTypes;
                var call__74 = A2.GetHitPoints();
                var call__75 = A2.GetResistance();
                var call__76 = Aow.Units.AoWE.AoWE.StatisticsToLimitedDV(FCombatInfo.Attack, Damage, DamageTypes, call__74, Defense, call__75, Immunity, Protection);
                A4.SourceDamage = call__76 * FCombatInfo.RepeatCount;
                if (FCombatInfo.Radius > 1)
                {
                    A4.SourceDamage = (int)(((uint)(A4.SourceDamage * FCombatInfo.Radius)) >> 1);
                }

                var call__77 = Aow.Units.AoWE.AoWE.StatisticsToMaxDamage(Damage, DamageTypes, Protection, Immunity);
                A4.SourceCost = call__77 * FCombatInfo.RepeatCount;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                if ((A4.SourceDamage != 0) && (FCombatInfo.Radius > 1))
                {
                    Combat = (Aow.Units.AoWE.TFastCombat)A1.FCombat;
                    Count = 0;
                    Distance = Combat.GetRoundDistance();
                    if (FCombatInfo.Range == -1)
                    {
                        Distance += 8;
                    }
                    else
                    {
                        if (FCombatInfo.Range >= 12)
                        {
                            Distance += 4;
                        }
                        else
                        {
                            if (FCombatInfo.Range >= 8)
                            {
                                Distance += 2;
                            }
                        }
                    }

                    {
                        I = Combat.FSides[(int)A2.GetSide()].GetCount() - 1;
                        byte end__78 = 0;
                        for (; I >= end__78; I -= 1)
                        {
                            var call__79 = A2.GetSide();
                            var call__80 = Combat.FSides[(int)call__79].GetObjects(I);
                            Other = call__80;
                            bool bool__81 = (A2 != Other);
                            if (bool__81)
                            {
                                var call__82 = Other.GetEnabled();
                                bool__81 = call__82;
                            }

                            bool bool__83 = bool__81 && (((int)(((uint)Other.FPosition) >> 4)) == ((int)(((uint)A2.FPosition) >> 4)));
                            if (bool__83)
                            {
                                var call__84 = Other.fcRoundDistance();
                                bool__83 = (call__84 <= Distance);
                            }

                            bool bool__85 = bool__83;
                            if (bool__85)
                            {
                                var call__86 = Other.GetImmunityTypes();
                                bool__85 = ((DamageTypes - call__86) != TSet<Aow.Units.AoWE.TDamageType>.Of());
                            }

                            if (bool__85)
                            {
                                Count += 1;
                            }
                        }
                    }

                    if (Count != 0)
                    {
                        if (FCombatInfo.Radius < Count)
                        {
                            Count = FCombatInfo.Radius;
                        }

                        A4.SourceDamage += Aow.Rtl.Rtl.Round(((double)(A4.SourceDamage * Count)) * 0.5);
                    }
                }
            }
            else
            {
                A4.SourceDamage = 0;
                A4.SourceCost = 0;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                A4.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
            }
        }

        public static new Aow.Units.CombatSpells.TCombatSpell Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TCombatSpell(context, owner));
    }

    // { TSolarFlare (instance size 64, VMT 557F40BC):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TSolarFlare : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:845
        private protected TSolarFlare(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__87 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SolarFlareRStr);
            FSpellName = call__87;
            FSpellID = 100;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 4;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtHoly);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        public static new Aow.Units.CombatSpells.TSolarFlare Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSolarFlare(context, owner));
    }

    // { TTurnUndeadSpellCA (instance size 32, VMT 557F41D0) }
    public partial class TTurnUndeadSpellCA : Aow.Units.CombatSpells.TCombatSpellCA
    {
        // CombatSpells.pas:861
        public new const int ClassID = 131788;

        public TTurnUndeadSpellCA(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // CombatSpells.pas:867
        public override void Execute(Aow.Units.AoWE.TCombat A1)
        {
            Aow.Units.AoWE.TCombatObject Source = default;
            Aow.Units.AoWE.TCombatUnit Target = default;
            Aow.Units.AoWE.TUnitEnchantmentAbilityData Data = default;
            base.Execute(A1);
            if (FDamage != 0)
            {
                var call__89 = A1.FCombatData.FindID(FSourceID);
                Source = call__89;
                var call__90 = A1.FCombatData.FindID(FTargetID);
                Target = (Aow.Units.AoWE.TCombatUnit)call__90;
                if (!Target.FStatus.Contains(Aow.Units.AoWE.TCombatObjectStatusElement.csDestroyed))
                {
                    var call__91 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(34);
                    call__91.Expand(Target.FUnit);
                    var call__92 = Target.FUnit.GetAbilityData(34);
                    Data = (Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__92;
                    if (Data != null)
                    {
                        Data.FEnchantmentPlayerIndex = unchecked((sbyte)Source.FCombatPlayer);
                    }
                }
            }
        }


    }

    // { TTurnUndead (instance size 64, VMT 557F42C8):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TTurnUndead : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:888
        private protected TTurnUndead(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__93 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TurnUndeadRStr);
            FSpellName = call__93;
            FSpellID = 101;
            FDamageMode = 1;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtHoly);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        // CombatSpells.pas:905
        public override Aow.Units.AoWE.TCombatAction CreateCA(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, bool A3)
        {
            Aow.Units.AoWE.TCombatAction Result = default;
            Aow.Units.CombatSpells.TTurnUndeadSpellCA Action = default;
            Action = new Aow.Units.CombatSpells.TTurnUndeadSpellCA(null);
            Action.FSpellID = FSpellID;
            Action.FSourceID = unchecked((byte)A1.FID);
            Action.FTargetID = unchecked((byte)A2.FID);
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            bool bool__95 = (A2 is Aow.Units.AoWE.TCombatUnit);
            if (bool__95)
            {
                var call__96 = ((Aow.Units.AoWE.TCombatUnit)A2).FUnit.GetUnitType();
                bool__95 = TSet<Aow.Units.AoWE.TUnitType>.Of(Aow.Units.AoWE.TUnitType.utHumanoid, Aow.Units.AoWE.TUnitType.utCreature).Contains(call__96);
            }

            bool bool__97 = bool__95;
            if (bool__97)
            {
                var call__98 = ((Aow.Units.AoWE.TCombatUnit)A2).FUnit.GetRace();
                bool bool__99 = (call__98 == 11);
                if (!bool__99)
                {
                    var call__100 = ((Aow.Units.AoWE.TCombatUnit)A2).FUnit.GetAbilityEnabled(129);
                    bool__99 = call__100;
                }

                bool__97 = bool__99;
            }

            if (bool__97)
            {
                Action.GenerateEx(A1, A2, unchecked((sbyte)FCombatInfo.Attack), unchecked((sbyte)FCombatInfo.Damage), FCombatInfo.DamageTypes, unchecked((byte)FDamageMode));
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:922
        public override void fcGetDamageValueEx(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, Aow.Units.AoWE.TWallSituation A3, ref Aow.Units.AoWE.TAbilityDamageValues A4)
        {
            bool bool__101 = (A2 is Aow.Units.AoWE.TCombatUnit);
            if (bool__101)
            {
                var call__102 = ((Aow.Units.AoWE.TCombatUnit)A2).FUnit.GetUnitType();
                bool__101 = TSet<Aow.Units.AoWE.TUnitType>.Of(Aow.Units.AoWE.TUnitType.utHumanoid, Aow.Units.AoWE.TUnitType.utCreature).Contains(call__102);
            }

            bool bool__103 = bool__101;
            if (bool__103)
            {
                var call__104 = ((Aow.Units.AoWE.TCombatUnit)A2).FUnit.GetRace();
                bool bool__105 = (call__104 == 11);
                if (!bool__105)
                {
                    var call__106 = ((Aow.Units.AoWE.TCombatUnit)A2).FUnit.GetAbilityEnabled(129);
                    bool__105 = call__106;
                }

                bool__103 = bool__105;
            }

            if (bool__103)
            {
                base.fcGetDamageValueEx(A1, A2, A3, ref A4);
            }
            else
            {
                A4.SourceDamage = 0;
                A4.SourceCost = 0;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                A4.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
            }
        }

        public static new Aow.Units.CombatSpells.TTurnUndead Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TTurnUndead(context, owner));
    }

    // { TFastCombatHighPrayerCA (instance size 32, VMT 557F43DC) }
    public partial class TFastCombatHighPrayerCA : Aow.Units.CombatSpells.TMultiTargetCombatSpellCA
    {
        // CombatSpells.pas:937
        public new const int ClassID = 131783;

        public TFastCombatHighPrayerCA(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // CombatSpells.pas:943
        public override void Execute(Aow.Units.AoWE.TCombat A1)
        {
            int I = default;
            Aow.Units.AoWE.TCombatObject Target = default;
            base.Execute(A1);
            {
                I = FTargetIDs.Count - 1;
                byte end__107 = 0;
                for (; I >= end__107; I -= 1)
                {
                    var call__108 = FTargetIDs[I];
                    var call__109 = A1.FCombatData.FindID(call__108);
                    Target = call__109;
                    var call__110 = Target.GetHitPoints();
                    Target.SetHitPoints(unchecked((byte)(call__110 + 5)));
                    var call__111 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(100);
                    call__111.Expand(((Aow.Units.AoWE.TCombatUnit)Target).FUnit);
                }
            }
        }

        // CombatSpells.pas:958
        public void Generate(Aow.Units.AoWE.TCombatObject A1)
        {
            int I = default;
            Aow.Units.AoWE.TCombatSide Side = default;
            Aow.Units.AoWE.TCombatObject Target = default;
            FSpellID = 102;
            FSourceID = unchecked((byte)A1.FID);
            var combat = A1.FCombat;
            var call__113 = A1.GetSide();
            Side = combat.FSides[(int)call__113];

            {
                I = Side.GetCount() - 1;
                byte end__114 = 0;
                for (; I >= end__114; I -= 1)
                {
                    Target = Side.GetObjects(I);
                    if ((Target is Aow.Units.AoWE.TCombatUnit) && !Target.FStatus.Contains(Aow.Units.AoWE.TCombatObjectStatusElement.csDestroyed))
                    {
                        FTargetIDs.Add(unchecked((byte)Target.FID));
                    }
                }
            }
        }


    }

    // { TTacticalCombatHighPrayerCA (instance size 28, VMT 557F44D0) }
    public partial class TTacticalCombatHighPrayerCA : Aow.Units.CombatSpells.TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:976
        public new const int ClassID = 131786;

        public TTacticalCombatHighPrayerCA(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // CombatSpells.pas:982
        public override void Execute(Aow.Units.AoWE.TCombat A1)
        {
            Aow.Units.AoWE.TCombatObject Target = default;
            base.Execute(A1);
            if (FSucceeded)
            {
                var call__115 = A1.FCombatData.FindID(FTargetID);
                Target = call__115;
                var call__116 = Target.GetHitPoints();
                Target.SetHitPoints(unchecked((byte)(call__116 + 5)));
                var call__117 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(100);
                call__117.Expand(((Aow.Units.AoWE.TCombatUnit)Target).FUnit);
            }
        }


    }

    // { THighPrayer (instance size 64, VMT 557F45CC):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class THighPrayer : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:996
        private protected THighPrayer(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__118 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HighPrayerRStr);
            FSpellName = call__118;
            FSpellID = 102;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = unchecked((sbyte)-1);
            spellCombatInfo.TargetTypes = TSet<Aow.Units.AoWE.TCombatTargetTypeElement>.Of(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend);
        }

        // CombatSpells.pas:1013
        public override Aow.Units.AoWE.TCombatAction CreateCA(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, bool A3)
        {
            Aow.Units.AoWE.TCombatAction Result = default;
            Aow.Units.CombatSpells.TTacticalCombatHighPrayerCA Action = default;
            Action = new Aow.Units.CombatSpells.TTacticalCombatHighPrayerCA(null);
            Action.FSucceeded = true;
            Action.FSourceID = unchecked((byte)A1.FID);
            Action.FTargetID = unchecked((byte)A2.FID);
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:1104
        public override void tcGetDamageValueEx(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, ref Aow.Units.AoWE.TAbilityDamageValues A3)
        {
            int Healing = default;
            int Value = default;
            var call__120 = A2.GetHits();
            var call__121 = A2.GetHitPoints();
            Healing = call__120 - call__121;
            if (Healing > 5)
            {
                Healing = 5;
            }

            Value = Healing * 100;
            if (Value != 0)
            {
                A3.SourceDamage = Value;
                A3.SourceCost = Value / 100;
                A3.TargetDamage = 0;
                A3.TargetCost = 0;
                A3.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtDeath);
            }
            else
            {
                A3.SourceDamage = 0;
                A3.SourceCost = 0;
                A3.TargetDamage = 0;
                A3.TargetCost = 0;
                A3.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
            }
        }

        // CombatSpells.pas:1026
        public override void fcPrefetchCombatCommands(Aow.Units.AoWE.TFastCombatCommandList A1)
        {
            int I = default;
            Aow.Units.AoWE.TCombatSide Side = default;
            Aow.Units.AoWE.TCombatObject Target = default;
            int Value = default;
            Aow.Units.AoWE.TSpellCC Command = default;
            Value = 0;
            var call__122 = A1.FSource.GetSide();
            Side = A1.FCombat.FSides[(int)call__122];
            {
                I = Side.GetCount() - 1;
                byte end__123 = 0;
                for (; I >= end__123; I -= 1)
                {
                    Target = Side.GetObjects(I);
                    if ((Target.FCombatValue != 0) && (Target is Aow.Units.AoWE.TCombatUnit))
                    {
                        var call__124 = Target.GetHits();
                        var call__125 = Target.GetHitPoints();
                        var call__126 = Aow.Units.AoWE.AoWE.DVtoDEV((call__124 - call__125) * 100, 0, A1.FSource, Target);
                        Value += call__126;
                    }
                }
            }

            if (Value != 0)
            {
                Command = new Aow.Units.AoWE.TSpellCC(52);
                Command.FTarget = null;
                Command.FMoveCost = 1;
                Command.FSpellID = FSpellID;
                Command.FExpectedDV = Value;
                A1.Add(Command);
            }
        }

        // CombatSpells.pas:1054
        public override bool fcExecuteCombatCommand(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TFastCombatCommand A2)
        {
            bool Result = default;
            Aow.Units.CombatSpells.TFastCombatHighPrayerCA Action = default;
            Action = new Aow.Units.CombatSpells.TFastCombatHighPrayerCA(null);
            Action.Generate(A1);
            var call__127 = ((Aow.Units.AoWE.THero)((Aow.Units.AoWE.TCombatUnit)A1).FUnit).CastingMana(this);
            Action.FManaCost = call__127;
            A1.FCombat.ExecuteCombatAction(Action);
            Action.Release();
            Result = true;
            return Result;
        }

        // CombatSpells.pas:1067
        public override void fcGetDamageValueEx(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, Aow.Units.AoWE.TWallSituation A3, ref Aow.Units.AoWE.TAbilityDamageValues A4)
        {
            int I = default;
            int Value = default;
            int Healing = default;
            Aow.Units.AoWE.TCombatSide Side = default;
            Aow.Units.AoWE.TCombatObject Target = default;
            Value = 0;
            var call__128 = A1.GetSide();
            Side = A1.FCombat.FSides[(int)call__128];
            {
                I = Side.GetCount() - 1;
                byte end__129 = 0;
                for (; I >= end__129; I -= 1)
                {
                    Target = Side.GetObjects(I);
                    if ((Target.FCombatValue != 0) && (Target is Aow.Units.AoWE.TCombatUnit))
                    {
                        var call__130 = Target.GetHits();
                        var call__131 = Target.GetHitPoints();
                        Healing = call__130 - call__131;
                        if (Healing > 5)
                        {
                            Healing = 5;
                        }

                        Value += Healing * 100;
                    }
                }
            }

            if (Value != 0)
            {
                A4.SourceDamage = Value;
                A4.SourceCost = Value / 100;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                A4.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtDeath);
            }
            else
            {
                A4.SourceDamage = 0;
                A4.SourceCost = 0;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                A4.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
            }
        }

        public static new Aow.Units.CombatSpells.THighPrayer Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new THighPrayer(context, owner));
    }

    // { TDeathRay (instance size 64, VMT 557F46E0):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TDeathRay : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1130
        private protected TDeathRay(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__132 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DeathRayRStr);
            FSpellName = call__132;
            FSpellID = 103;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 4;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtDeath);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        public static new Aow.Units.CombatSpells.TDeathRay Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TDeathRay(context, owner));
    }

    // { TDiseaseCloud (instance size 64, VMT 557F47F0):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TDiseaseCloud : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1146
        private protected TDiseaseCloud(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__134 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DiseaseCloudRStr);
            FSpellName = call__134;
            FSpellID = 104;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 3;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtDeath);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        public static new Aow.Units.CombatSpells.TDiseaseCloud Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TDiseaseCloud(context, owner));
    }

    // { TMindDecayCA (instance size 28, VMT 557F4908) }
    public partial class TMindDecayCA : Aow.Units.CombatSpells.TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1162
        public new const int ClassID = 131787;

        public TMindDecayCA(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // CombatSpells.pas:1168
        public override void Execute(Aow.Units.AoWE.TCombat A1)
        {
            Aow.Units.AoWE.TCombatObject Source = default;
            Aow.Units.AoWE.TCombatUnit Target = default;
            int I = default;
            base.Execute(A1);
            if (FSucceeded)
            {
                var call__136 = A1.FCombatData.FindID(FSourceID);
                Source = call__136;
                var call__137 = A1.FCombatData.FindID(FTargetID);
                Target = (Aow.Units.AoWE.TCombatUnit)call__137;
                var call__138 = Source.GetExperience();
                var call__139 = Target.GetLevel();
                Source.SetExperience(call__138 + call__139);
                {
                    I = 0;
                    byte end__140 = 2;
                    for (; I <= end__140; I += 1)
                    {
                        var call__141 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Units.AoWE.AoWE.CommandAbilityIDs[I]);
                        ((Aow.Units.AoWE.TCommandAbility)call__141).ResetCommandedUnits(Target);
                    }
                }

                {
                    I = 0;
                    byte end__142 = 2;
                    for (; I <= end__142; I += 1)
                    {
                        var call__143 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Units.AoWE.AoWE.CommandAbilityIDs[I]);
                        ((Aow.Units.AoWE.TCommandAbility)call__143).Uncommand(Target);
                    }
                }

                var call__144 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(131);
                call__144.Expand(Target.FUnit);
                Target.SetCombatPlayer(unchecked((byte)Source.FCombatPlayer));
            }
        }


    }

    // { TMindDecay (instance size 64, VMT 557F49E8):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TMindDecay : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1190
        private protected TMindDecay(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__145 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MindDecayRStr);
            FSpellName = call__145;
            FSpellID = 106;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 5;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtNone);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 3;
        }

        // CombatSpells.pas:1207
        public override Aow.Units.AoWE.TCombatAction CreateCA(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, bool A3)
        {
            Aow.Units.AoWE.TCombatAction Result = default;
            Aow.Units.CombatSpells.TMindDecayCA Action = default;
            Action = new Aow.Units.CombatSpells.TMindDecayCA(null);
            Action.FSourceID = unchecked((byte)A1.FID);
            Action.FTargetID = unchecked((byte)A2.FID);
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            bool bool__147 = (A2 is Aow.Units.AoWE.TCombatUnit);
            if (bool__147)
            {
                var call__148 = ((Aow.Units.AoWE.TCombatUnit)A2).FUnit.GetUnitType();
                bool__147 = (call__148 != Aow.Units.AoWE.TUnitType.utMachine);
            }

            bool bool__149 = bool__147;
            if (bool__149)
            {
                var call__150 = ((Aow.Units.AoWE.TCombatUnit)A2).FUnit.GetRace();
                bool__149 = (call__150 != 11);
            }

            bool bool__151 = bool__149;
            if (bool__151)
            {
                var call__152 = ((Aow.Units.AoWE.TCombatUnit)A2).FUnit.GetAbilityEnabled(129);
                bool__151 = !call__152;
            }

            if (bool__151 && !(((Aow.Units.AoWE.TCombatUnit)A2).FUnit is Aow.Units.AoWE.TLeader))
            {
                var call__153 = A2.GetResistance();
                var call__154 = Aow.Units.AoWE.AoWE.HitRole(FCombatInfo.Attack - call__153);
                Action.FSucceeded = call__154;
            }

            Result = Action;
            return Result;
        }

        public static new Aow.Units.CombatSpells.TMindDecay Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TMindDecay(context, owner));
    }

    // { TChainLightning (instance size 64, VMT 557F4AFC):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TChainLightning : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1224
        private protected TChainLightning(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__155 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ChainLightningRStr);
            FSpellName = call__155;
            FSpellID = 107;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 4;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtLightning);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 3;
        }

        public static new Aow.Units.CombatSpells.TChainLightning Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TChainLightning(context, owner));
    }

    // { TColdBreath (instance size 64, VMT 557F4C18):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TColdBreath : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1241
        private protected TColdBreath(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__157 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ColdBreathRStr);
            FSpellName = call__157;
            FSpellID = 108;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtCold);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 4;
        }

        public static new Aow.Units.CombatSpells.TColdBreath Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TColdBreath(context, owner));
    }

    // { TSlowCA (instance size 28, VMT 557F4D2C) }
    public partial class TSlowCA : Aow.Units.CombatSpells.TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1257
        public new const int ClassID = 131792;

        public TSlowCA(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // CombatSpells.pas:1263
        public override void Execute(Aow.Units.AoWE.TCombat A1)
        {
            Aow.Units.AoWE.TCombatUnit Target = default;
            Aow.Units.AoWE.TCombatObject Source = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            base.Execute(A1);
            if (FSucceeded)
            {
                var call__159 = A1.FCombatData.FindID(FTargetID);
                Target = (Aow.Units.AoWE.TCombatUnit)call__159;
                var call__160 = A1.FCombatData.FindID(FSourceID);
                Source = call__160;
                var call__161 = Target.FUnit.ExpandAbility(132);
                if (call__161)
                {
                    var call__162 = Target.FUnit.GetAbilityData(132);
                    Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__162).FEnchantment;
                    Enchantment.SetUpkeep(0);
                    var call__163 = Source.GetPlayer();
                    Enchantment.SetSource(unchecked((sbyte)call__163));
                }
            }
        }


    }

    // { TSlow (instance size 64, VMT 557F4E00):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TSlow : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1284
        private protected TSlow(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__164 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SlowRStr);
            FSpellName = call__164;
            FSpellID = 110;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 9;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtNone);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        // CombatSpells.pas:1313
        public override Aow.Units.AoWE.TCombatAction CreateCA(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, bool A3)
        {
            Aow.Units.AoWE.TCombatAction Result = default;
            Aow.Units.CombatSpells.TSlowCA Action = default;
            Action = new Aow.Units.CombatSpells.TSlowCA(null);
            Action.FSourceID = unchecked((byte)A1.FID);
            Action.FTargetID = unchecked((byte)A2.FID);
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            if (!A2.GetAbilityEnabled(132))
            {
                var call__166 = A2.GetResistance();
                var call__167 = Aow.Units.AoWE.AoWE.HitRole(FCombatInfo.Attack - call__166);
                Action.FSucceeded = call__167;
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:1300
        public override void GetCombatDamageValueEx(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, ref Aow.Units.AoWE.TAbilityDamageValues A3)
        {
            ref var abilityDamageValues = ref A3;
            abilityDamageValues.SourceDamage = 0;
            abilityDamageValues.SourceCost = 0;
            abilityDamageValues.TargetDamage = 0;
            abilityDamageValues.TargetCost = 0;
            abilityDamageValues.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
        }

        public static new Aow.Units.CombatSpells.TSlow Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSlow(context, owner));
    }

    // { TEntangleSpellCA (instance size 28, VMT 557F4F08) }
    public partial class TEntangleSpellCA : Aow.Units.CombatSpells.TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1328
        public new const int ClassID = 131784;

        public TEntangleSpellCA(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // CombatSpells.pas:1334
        public override void Execute(Aow.Units.AoWE.TCombat A1)
        {
            Aow.Units.AoWE.TCombatObject Source = default;
            Aow.Units.AoWE.TCombatUnit Target = default;
            Aow.Units.AoWE.TUnitEnchantmentAbilityData Data = default;
            base.Execute(A1);
            if (FSucceeded)
            {
                var call__169 = A1.FCombatData.FindID(FSourceID);
                Source = call__169;
                var call__170 = A1.FCombatData.FindID(FTargetID);
                Target = (Aow.Units.AoWE.TCombatUnit)call__170;
                var call__171 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(94);
                call__171.Expand(Target.FUnit);
                var call__172 = Target.FUnit.GetAbilityData(94);
                Data = (Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__172;
                if (Data != null)
                {
                    Data.FEnchantmentPlayerIndex = unchecked((sbyte)Source.FCombatPlayer);
                }
            }
        }


    }

    // { TEntangle (instance size 64, VMT 557F4FF0):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TEntangle : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1352
        private protected TEntangle(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__173 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EntangleRStr);
            FSpellName = call__173;
            FSpellID = 111;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 7;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtNone);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        // CombatSpells.pas:1368
        public override Aow.Units.AoWE.TCombatAction CreateCA(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, bool A3)
        {
            Aow.Units.AoWE.TCombatAction Result = default;
            Aow.Units.CombatSpells.TEntangleSpellCA Action = default;
            Action = new Aow.Units.CombatSpells.TEntangleSpellCA(null);
            Action.FSourceID = unchecked((byte)A1.FID);
            Action.FTargetID = unchecked((byte)A2.FID);
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            var call__175 = A2.GetAbilityEnabled(1);
            bool bool__176 = !call__175;
            if (bool__176)
            {
                var call__177 = A2.GetAbilityEnabled(94);
                bool__176 = !call__177;
            }

            if (bool__176)
            {
                var call__178 = A2.GetResistance();
                var call__179 = Aow.Units.AoWE.AoWE.HitRole(FCombatInfo.Attack - call__178);
                Action.FSucceeded = call__179;
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:1383
        public override void GetCombatDamageValueEx(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, ref Aow.Units.AoWE.TAbilityDamageValues A3)
        {
            var call__180 = A2.GetAbilityEnabled(1);
            bool bool__181 = !call__180;
            if (bool__181)
            {
                var call__182 = A2.GetAbilityEnabled(94);
                bool__181 = !call__182;
            }

            if (bool__181)
            {
                var call__183 = A2.GetResistance();
                var call__184 = Aow.Units.AoWE.AoWE.HitRoleProbability(FCombatInfo.Attack - call__183);
                var call__185 = A2.GetHitPoints();
                A3.SourceDamage = Aow.Rtl.Rtl.Round(((double)(call__185 * 100)) * call__184);
                var call__186 = A2.GetHitPoints();
                A3.SourceCost = call__186;
                A3.TargetDamage = 0;
                A3.TargetCost = 0;
                A3.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtNone);
            }
            else
            {
                A3.SourceDamage = 0;
                A3.SourceCost = 0;
                A3.TargetDamage = 0;
                A3.TargetCost = 0;
                A3.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
            }
        }

        public static new Aow.Units.CombatSpells.TEntangle Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TEntangle(context, owner));
    }

    // { TTremors (instance size 64, VMT 557F5100):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TTremors : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1404
        private protected TTremors(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            // { Original leaves this spell name untranslated. }
            FSpellName = Aow.Units.AoWE.AoWE.TremorsRStr;
            FSpellID = 112;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPhysical, Aow.Units.AoWE.TDamageType.dtWall);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = unchecked((sbyte)-1);
            spellCombatInfo.Radius = 5;
        }

        public static new Aow.Units.CombatSpells.TTremors Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TTremors(context, owner));
    }

    // { TCallFlames (instance size 64, VMT 557F5210):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TCallFlames : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1421
        private protected TCallFlames(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__188 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CallFlamesRStr);
            FSpellName = call__188;
            FSpellID = 113;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 9;
            spellCombatInfo.Damage = 3;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtFire);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 8;
            spellCombatInfo.Radius = 1;
        }

        public static new Aow.Units.CombatSpells.TCallFlames Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TCallFlames(context, owner));
    }

    // { TFireBreath (instance size 64, VMT 557F5324):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TFireBreath : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1438
        private protected TFireBreath(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__190 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FireBreathRStr);
            FSpellName = call__190;
            FSpellID = 114;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtFire);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 4;
            spellCombatInfo.Radius = 3;
        }

        public static new Aow.Units.CombatSpells.TFireBreath Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TFireBreath(context, owner));
    }

    // { TSwarm (instance size 64, VMT 557F5438):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TSwarm : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1455
        private protected TSwarm(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__192 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SwarmRStr);
            FSpellName = call__192;
            FSpellID = 115;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 2;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtFire);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 5;
        }

        public static new Aow.Units.CombatSpells.TSwarm Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSwarm(context, owner));
    }

    // { TGreatHail (instance size 64, VMT 557F5544):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TGreatHail : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1472
        private protected TGreatHail(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__194 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GreatHailRStr);
            FSpellName = call__194;
            FSpellID = 116;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 5;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPhysical, Aow.Units.AoWE.TDamageType.dtWall);
            spellCombatInfo.RepeatCount = 3;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 2;
        }

        public static new Aow.Units.CombatSpells.TGreatHail Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TGreatHail(context, owner));
    }

    // { TFireBall (instance size 64, VMT 557F5658):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TFireBall : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1489
        private protected TFireBall(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__196 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FireBallRStr);
            FSpellName = call__196;
            FSpellID = 117;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 6;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtFire, Aow.Units.AoWE.TDamageType.dtWall);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        public static new Aow.Units.CombatSpells.TFireBall Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TFireBall(context, owner));
    }

    // { TGeyser (instance size 64, VMT 557F5768):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TGeyser : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1505
        private protected TGeyser(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__198 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GeyserRStr);
            FSpellName = call__198;
            FSpellID = 118;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 9;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPhysical);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 1;
        }

        // CombatSpells.pas:1522
        public override Aow.Units.AoWE.TCombatAction CreateCA(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, bool A3)
        {
            Aow.Units.AoWE.TCombatAction Result = default;
            Aow.Units.CombatSpells.TCombatSpellCA Action = default;
            Action = new Aow.Units.CombatSpells.TCombatSpellCA(null);
            Action.FSpellID = FSpellID;
            var call__200 = A2.GetAbilityEnabled(1);
            bool bool__201 = call__200;
            if (!bool__201)
            {
                var call__202 = A2.GetAbilityEnabled(59);
                bool__201 = call__202;
            }

            if (bool__201)
            {
                Action.Generate(A1, A2, unchecked((sbyte)FCombatInfo.Attack), unchecked((sbyte)(FCombatInfo.Damage / 2)), FCombatInfo.DamageTypes);
            }
            else
            {
                Action.Generate(A1, A2, unchecked((sbyte)FCombatInfo.Attack), unchecked((sbyte)FCombatInfo.Damage), FCombatInfo.DamageTypes);
            }

            if (A3)
            {
                var call__203 = ((Aow.Units.AoWE.THero)((Aow.Units.AoWE.TCombatUnit)A1).FUnit).CastingMana(this);
                Action.FManaCost = call__203;
            }

            Result = Action;
            return Result;
        }

        public static new Aow.Units.CombatSpells.TGeyser Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TGeyser(context, owner));
    }

    // { TIceShards (instance size 64, VMT 557F5874):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TIceShards : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1537
        private protected TIceShards(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__204 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.IceShardsRStr);
            FSpellName = call__204;
            FSpellID = 119;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 4;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPhysical);
            spellCombatInfo.RepeatCount = 3;
            spellCombatInfo.Range = 12;
        }

        public static new Aow.Units.CombatSpells.TIceShards Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TIceShards(context, owner));
    }

    // { TFrostBeams (instance size 64, VMT 557F5988):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TFrostBeams : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1553
        private protected TFrostBeams(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__206 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FrostBeamsRStr);
            FSpellName = call__206;
            FSpellID = 120;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 9;
            spellCombatInfo.Damage = 3;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtCold);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 3;
        }

        public static new Aow.Units.CombatSpells.TFrostBeams Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TFrostBeams(context, owner));
    }

    // { TVaporize (instance size 64, VMT 557F5A9C):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TVaporize : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1570
        private protected TVaporize(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__208 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.VaporizeRStr);
            FSpellName = call__208;
            FSpellID = 121;
            FDamageMode = 1;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 7;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPhysical);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        public static new Aow.Units.CombatSpells.TVaporize Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TVaporize(context, owner));
    }

    // { TFlamingArrow (instance size 64, VMT 557F5BAC):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TFlamingArrow : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1603
        private protected TFlamingArrow(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__210 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FlamingArrowRStr);
            FSpellName = call__210;
            FSpellID = 123;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 4;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtFire, Aow.Units.AoWE.TDamageType.dtPhysical);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        public static new Aow.Units.CombatSpells.TFlamingArrow Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TFlamingArrow(context, owner));
    }

    // { TStoning (instance size 64, VMT 557F5CC4):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TStoning : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1587
        private protected TStoning(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__212 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.StoningRStr);
            FSpellName = call__212;
            FSpellID = 122;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 5;
            spellCombatInfo.Damage = 2;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPhysical);
            spellCombatInfo.RepeatCount = 6;
            spellCombatInfo.Range = 12;
        }

        public static new Aow.Units.CombatSpells.TStoning Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TStoning(context, owner));
    }

    // { TOoze (instance size 64, VMT 557F5DD4):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TOoze : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1619
        private protected TOoze(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__214 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.OozeRStr);
            FSpellName = call__214;
            FSpellID = 124;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 0;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
            spellCombatInfo.RepeatCount = 0;
            spellCombatInfo.Range = 8;
        }

        // CombatSpells.pas:1635
        public override void GetCombatDamageValueEx(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, ref Aow.Units.AoWE.TAbilityDamageValues A3)
        {
            TSet<Aow.Units.AoWE.TDamageType> Protection = default;
            TSet<Aow.Units.AoWE.TDamageType> Immunity = default;
            Protection = A2.GetProtectionTypes();
            Immunity = A2.GetImmunityTypes();
            A3.DamageTypes = FCombatInfo.DamageTypes;
            var call__216 = A2.GetResistance();
            var call__217 = Aow.Units.AoWE.AoWE.HitRoleProbability(FCombatInfo.Attack - call__216);
            A3.SourceDamage = Aow.Rtl.Rtl.Round(((double)call__217) * 200);
            A3.SourceCost = 200;
            A3.TargetDamage = 0;
            A3.TargetCost = 0;
        }

        public static new Aow.Units.CombatSpells.TOoze Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TOoze(context, owner));
    }

    // { TSacredWrath (instance size 64, VMT 557F5EDC):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TSacredWrath : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1649
        private protected TSacredWrath(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__218 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SacredWrathRStr);
            FSpellName = call__218;
            FSpellID = 125;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtHoly);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 8;
            spellCombatInfo.Radius = 3;
        }

        public static new Aow.Units.CombatSpells.TSacredWrath Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSacredWrath(context, owner));
    }

    // { TFastCombatTerrorCA (instance size 32, VMT 557F5FF4) }
    public partial class TFastCombatTerrorCA : Aow.Units.CombatSpells.TMultiTargetCombatSpellCA
    {
        // CombatSpells.pas:1666
        public new const int ClassID = 131777;

        public TFastCombatTerrorCA(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // CombatSpells.pas:1672
        public override void Execute(Aow.Units.AoWE.TCombat A1)
        {
            int I = default;
            Aow.Units.AoWE.TCombatUnit Target = default;
            base.Execute(A1);
            {
                I = FTargetIDs.Count - 1;
                byte end__220 = 0;
                for (; I >= end__220; I -= 1)
                {
                    var call__221 = FTargetIDs[I];
                    var call__222 = A1.FCombatData.FindID(call__221);
                    Target = (Aow.Units.AoWE.TCombatUnit)call__222;
                    var call__223 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(108);
                    call__223.Expand(Target.FUnit);
                }
            }
        }

        // CombatSpells.pas:1686
        public void Generate(Aow.Units.AoWE.TCombatObject A1)
        {
            int I = default;
            Aow.Units.AoWE.TCombatSide Side = default;
            Aow.Units.AoWE.TCombatObject Target = default;
            FSpellID = 105;
            FSourceID = unchecked((byte)A1.FID);
            var combat = A1.FCombat;
            var call__225 = A1.GetOpponentSide();
            Side = combat.FSides[(int)call__225];

            {
                I = Side.GetCount() - 1;
                byte end__226 = 0;
                for (; I >= end__226; I -= 1)
                {
                    Target = Side.GetObjects(I);
                    bool bool__227 = !Target.FStatus.Contains(Aow.Units.AoWE.TCombatObjectStatusElement.csDestroyed);
                    if (bool__227)
                    {
                        var call__228 = Target.GetAbilityEnabled(108);
                        bool__227 = !call__228;
                    }

                    bool bool__229 = bool__227;
                    if (bool__229)
                    {
                        var call__230 = Target.GetAbilityEnabled(67);
                        bool__229 = !call__230;
                    }

                    bool bool__231 = bool__229 && (Target is Aow.Units.AoWE.TCombatUnit);
                    if (bool__231)
                    {
                        var call__232 = Target.GetImmunityTypes();
                        bool__231 = !call__232.Contains(Aow.Units.AoWE.TDamageType.dtDeath);
                    }

                    bool bool__233 = bool__231;
                    if (bool__233)
                    {
                        var call__234 = ((Aow.Units.AoWE.TCombatUnit)Target).FUnit.GetUnitType();
                        bool__233 = (call__234 != Aow.Units.AoWE.TUnitType.utMachine);
                    }

                    bool bool__235 = bool__233;
                    if (bool__235)
                    {
                        var call__236 = Target.GetResistance();
                        var call__237 = Aow.Units.AoWE.AoWE.HitRole(6 - call__236);
                        bool__235 = call__237;
                    }

                    if (bool__235)
                    {
                        FTargetIDs.Add(unchecked((byte)Target.FID));
                    }
                }
            }
        }


    }

    // { TTacticalCombatTerrorCA (instance size 24, VMT 557F60E0):
//     $0C source ID, $0D target ID, $10 casting mana, $14 success. }
    public partial class TTacticalCombatTerrorCA : Aow.Units.AoWE.TCombatAction
    {
        // CombatSpells.pas:1707
        public new const int ClassID = 131785;

        public TTacticalCombatTerrorCA(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        public byte FSourceID;
        // { $0C }
        public byte FTargetID;
        // { $0D }
        public Aow.Units.FixedArray_byte_2 F_TacticalCombatTerrorCA_0E;
        public int FManaCost;
        // { $10 }
        public bool FSucceeded;
        // { $14 }
        public Aow.Units.FixedArray_byte_3 F_TacticalCombatTerrorCA_15;
        // CombatSpells.pas:1713
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref FSourceID, 20);
            Stream.rwByte(ref FTargetID, 21);
            Stream.rwInteger(ref FManaCost, 22);
            Stream.rwBool(ref FSucceeded, 23);
        }

        // CombatSpells.pas:1723
        public override void Execute(Aow.Units.AoWE.TCombat A1)
        {
            Aow.Units.AoWE.TCombatUnit Target = default;
            Aow.Units.AoWE.TCombatUnit Source = default;
            if (FSucceeded)
            {
                var call__238 = A1.FCombatData.FindID(FTargetID);
                Target = (Aow.Units.AoWE.TCombatUnit)call__238;
                var call__239 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(108);
                call__239.Expand(Target.FUnit);
            }

            if ((FManaCost != 0) && A1.FStatus.Contains(Aow.Units.AoWE.TCombatStatusElement.cExecuting))
            {
                var call__240 = A1.FCombatData.FindID(FSourceID);
                Source = (Aow.Units.AoWE.TCombatUnit)call__240;
                var call__241 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(105);
                call__241.CastingDone(Source.FUnit, FManaCost);
            }
        }

        // CombatSpells.pas:1740
        public override bool Play(Aow.Units.AoWE.TCombatViewer A1)
        {
            bool Result = default;
            Execute(A1.FCombat);
            Result = false;
            return Result;
        }

        // CombatSpells.pas:1747
        public void Generate(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2)
        {
            FSucceeded = false;
            FSourceID = unchecked((byte)A1.FID);
            FTargetID = unchecked((byte)A2.FID);
            var call__242 = A2.GetAbilityEnabled(108);
            bool bool__243 = !call__242;
            if (bool__243)
            {
                var call__244 = A2.GetAbilityEnabled(67);
                bool__243 = !call__244;
            }

            bool bool__245 = bool__243 && (A2 is Aow.Units.AoWE.TCombatUnit);
            if (bool__245)
            {
                var call__246 = A2.GetImmunityTypes();
                bool__245 = !call__246.Contains(Aow.Units.AoWE.TDamageType.dtDeath);
            }

            bool bool__247 = bool__245;
            if (bool__247)
            {
                var call__248 = ((Aow.Units.AoWE.TCombatUnit)A2).FUnit.GetUnitType();
                bool__247 = (call__248 != Aow.Units.AoWE.TUnitType.utMachine);
            }

            if (bool__247)
            {
                var call__249 = A2.GetResistance();
                var call__250 = Aow.Units.AoWE.AoWE.HitRole(6 - call__249);
                FSucceeded = call__250;
            }
        }


    }

    // { TTerror (instance size 64, VMT 557F61D4):
//     inherited $34 combat statistics, $3C damage mode. }
    public partial class TTerror : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1759
        private protected TTerror(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__251 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TerrorRStr);
            FSpellName = call__251;
            FSpellID = 105;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtDeath);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = unchecked((sbyte)-1);
        }

        // CombatSpells.pas:1775
        public override Aow.Units.AoWE.TCombatAction CreateCA(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, bool A3)
        {
            Aow.Units.AoWE.TCombatAction Result = default;
            Aow.Units.CombatSpells.TTacticalCombatTerrorCA Action = default;
            Action = new Aow.Units.CombatSpells.TTacticalCombatTerrorCA(null);
            Action.Generate(A1, A2);
            if (A3)
            {
                var call__253 = ((Aow.Units.AoWE.THero)((Aow.Units.AoWE.TCombatUnit)A1).FUnit).CastingMana(this);
                Action.FManaCost = call__253;
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:1862
        public override void tcGetDamageValueEx(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, ref Aow.Units.AoWE.TAbilityDamageValues A3)
        {
            int Value = default;
            var call__254 = A2.GetAbilityEnabled(108);
            bool bool__255 = (!call__254 && (A2.FCombatValue != 0)) && (A2 is Aow.Units.AoWE.TCombatUnit);
            if (bool__255)
            {
                var call__256 = A2.GetImmunityTypes();
                bool__255 = !call__256.Contains(Aow.Units.AoWE.TDamageType.dtDeath);
            }

            if (bool__255)
            {
                var call__257 = A2.GetResistance();
                var call__258 = Aow.Units.AoWE.AoWE.HitRoleProbability(6 - call__257);
                Value = Aow.Rtl.Rtl.Round(((double)call__258) * 100);
            }
            else
            {
                Value = 0;
            }

            if (Value != 0)
            {
                A3.SourceDamage = Value;
                A3.SourceCost = Value / 100;
                A3.TargetDamage = 0;
                A3.TargetCost = 0;
                A3.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtDeath);
            }
            else
            {
                A3.SourceDamage = 0;
                A3.SourceCost = 0;
                A3.TargetDamage = 0;
                A3.TargetCost = 0;
                A3.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
            }
        }

        // CombatSpells.pas:1786
        public override void fcPrefetchCombatCommands(Aow.Units.AoWE.TFastCombatCommandList A1)
        {
            int I = default;
            Aow.Units.AoWE.TCombatSide Side = default;
            Aow.Units.AoWE.TCombatObject Target = default;
            int Value = default;
            Aow.Units.AoWE.TSpellCC Command = default;
            Value = 0;
            var call__259 = A1.FSource.GetOpponentSide();
            Side = A1.FCombat.FSides[(int)call__259];
            {
                I = Side.GetCount() - 1;
                byte end__260 = 0;
                for (; I >= end__260; I -= 1)
                {
                    Target = Side.GetObjects(I);
                    var call__261 = Target.GetAbilityEnabled(108);
                    bool bool__262 = (!call__261 && (Target.FCombatValue != 0)) && (Target is Aow.Units.AoWE.TCombatUnit);
                    if (bool__262)
                    {
                        var call__263 = Target.GetImmunityTypes();
                        bool__262 = !call__263.Contains(Aow.Units.AoWE.TDamageType.dtDeath);
                    }

                    if (bool__262)
                    {
                        Value += Target.GetTargetPriority();
                    }
                }
            }

            if (Value != 0)
            {
                Command = new Aow.Units.AoWE.TSpellCC(52);
                Command.FTarget = null;
                Command.FMoveCost = 1;
                Command.FSpellID = FSpellID;
                Command.FExpectedDV = Value;
                A1.Add(Command);
            }
        }

        // CombatSpells.pas:1815
        public override bool fcExecuteCombatCommand(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TFastCombatCommand A2)
        {
            bool Result = default;
            Aow.Units.CombatSpells.TFastCombatTerrorCA Action = default;
            Action = new Aow.Units.CombatSpells.TFastCombatTerrorCA(null);
            Action.Generate(A1);
            var call__264 = ((Aow.Units.AoWE.THero)((Aow.Units.AoWE.TCombatUnit)A1).FUnit).CastingMana(this);
            Action.FManaCost = call__264;
            A1.FCombat.ExecuteCombatAction(Action);
            Action.Release();
            Result = true;
            return Result;
        }

        // CombatSpells.pas:1828
        public override void fcGetDamageValueEx(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, Aow.Units.AoWE.TWallSituation A3, ref Aow.Units.AoWE.TAbilityDamageValues A4)
        {
            int I = default;
            int Value = default;
            Aow.Units.AoWE.TCombatSide Side = default;
            Aow.Units.AoWE.TCombatObject Target = default;
            Value = 0;
            var call__265 = A1.GetOpponentSide();
            Side = A1.FCombat.FSides[(int)call__265];
            {
                I = Side.GetCount() - 1;
                byte end__266 = 0;
                for (; I >= end__266; I -= 1)
                {
                    Target = Side.GetObjects(I);
                    var call__267 = Target.GetAbilityEnabled(108);
                    bool bool__268 = (!call__267 && (Target.FCombatValue != 0)) && (Target is Aow.Units.AoWE.TCombatUnit);
                    if (bool__268)
                    {
                        var call__269 = Target.GetImmunityTypes();
                        bool__268 = !call__269.Contains(Aow.Units.AoWE.TDamageType.dtDeath);
                    }

                    if (bool__268)
                    {
                        var call__270 = Target.GetResistance();
                        var call__271 = Aow.Units.AoWE.AoWE.HitRoleProbability(6 - call__270);
                        Value += Aow.Rtl.Rtl.Round(((double)call__271) * 100);
                    }
                }
            }

            if (Value != 0)
            {
                A4.SourceDamage = Value;
                A4.SourceCost = Value / 100;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                A4.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtDeath);
            }
            else
            {
                A4.SourceDamage = 0;
                A4.SourceCost = 0;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                A4.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
            }
        }

        public static new Aow.Units.CombatSpells.TTerror Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TTerror(context, owner));
    }

    // { TWindsOfFury (instance size 64, VMT 557F62E0): inherited $10 spell ID, $34 combat statistics, $3C damage mode. }
    public partial class TWindsOfFury : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1889
        private protected TWindsOfFury(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__272 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WindsOfFuryRStr);
            FSpellName = call__272;
            FSpellID = 126;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPhysical);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        // CombatSpells.pas:1904
        public override Aow.Units.AoWE.TCombatAction CreateCA(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, bool A3)
        {
            Aow.Units.AoWE.TCombatAction Result = default;
            Aow.Units.CombatSpells.TCombatSpellCA Action = default;
            Action = new Aow.Units.CombatSpells.TCombatSpellCA(null);
            Action.FSpellID = FSpellID;
            var call__274 = A2.GetAbilityEnabled(1);
            bool bool__275 = call__274;
            if (!bool__275)
            {
                var call__276 = A2.GetAbilityEnabled(59);
                bool__275 = call__276;
            }

            if (bool__275)
            {
                Action.Generate(A1, A2, unchecked((sbyte)(FCombatInfo.Attack + 2)), unchecked((sbyte)FCombatInfo.Damage), FCombatInfo.DamageTypes);
            }
            else
            {
                Action.Generate(A1, A2, unchecked((sbyte)FCombatInfo.Attack), unchecked((sbyte)FCombatInfo.Damage), FCombatInfo.DamageTypes);
            }

            if (A3)
            {
                var call__277 = ((Aow.Units.AoWE.THero)((Aow.Units.AoWE.TCombatUnit)A1).FUnit).CastingMana(this);
                Action.FManaCost = call__277;
            }

            Result = Action;
            return Result;
        }

        public static new Aow.Units.CombatSpells.TWindsOfFury Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TWindsOfFury(context, owner));
    }

    // { TShockwave (instance size 64, VMT 557F63F8): inherited $10 spell ID, $34 combat statistics, $3C damage mode. }
    public partial class TShockwave : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1919
        private protected TShockwave(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__278 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ShockwaveRStr);
            FSpellName = call__278;
            FSpellID = 127;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPhysical, Aow.Units.AoWE.TDamageType.dtWall);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        public static new Aow.Units.CombatSpells.TShockwave Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TShockwave(context, owner));
    }

    // { TSacrificialFlame (instance size 64, VMT 557F650C): inherited $10 spell ID, $34 combat statistics, $3C damage mode. }
    public partial class TSacrificialFlame : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1934
        private protected TSacrificialFlame(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__280 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SacrificialFlameRStr);
            FSpellName = call__280;
            FSpellID = 128;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtFire);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 3;
        }

        public static new Aow.Units.CombatSpells.TSacrificialFlame Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSacrificialFlame(context, owner));
    }

    // { TRecallSpiritCA (instance size 28, VMT 557F662C): inherited $0C source ID, $0D target ID, $10 mana, $14 success, $18 spell ID. }
    public partial class TRecallSpiritCA : Aow.Units.CombatSpells.TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1950
        public new const int ClassID = 131781;

        public TRecallSpiritCA(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // CombatSpells.pas:1953
        public override void Execute(Aow.Units.AoWE.TCombat A1)
        {
            Aow.Units.AoWE.TCombatUnit Source = default;
            Aow.Units.AoWE.TCombatUnit Target = default;
            Aow.Units.AoWE.TUnit NewUnit = default;
            base.Execute(A1);
            if (FSucceeded)
            {
                var call__282 = A1.FCombatData.FindID(FSourceID);
                Source = (Aow.Units.AoWE.TCombatUnit)call__282;
                var call__283 = A1.FCombatData.FindID(FTargetID);
                Target = (Aow.Units.AoWE.TCombatUnit)call__283;
                NewUnit = new Aow.Units.AoWE.TUnit(null);
                var call__284 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(130);
                NewUnit.SetUnitResource(call__284);
                NewUnit.SetPlayer(unchecked((sbyte)Source.FUnit.FPlayer));
                NewUnit.ExpandAbility(130);
                NewUnit.Activate();
                Target.SetUnit(NewUnit);
                NewUnit.Release();
                Target.SetStatus(Target.FStatus - TSet<Aow.Units.AoWE.TCombatObjectStatusElement>.Of(Aow.Units.AoWE.TCombatObjectStatusElement.csDestroyed));
                Target.SetCombatPlayer(unchecked((byte)Source.FCombatPlayer));
            }
        }


    }

    // { TRecallSpirit (instance size 64, VMT 557F6710): inherited $10 spell ID, $34 combat statistics, $3C damage mode. }
    public partial class TRecallSpirit : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:1975
        private protected TRecallSpirit(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__285 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RecallSpiritRStr);
            FSpellName = call__285;
            FSpellID = 129;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 0;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = unchecked((sbyte)-1);
        }

        // CombatSpells.pas:2008
        public override Aow.Units.AoWE.TCombatAction CreateCA(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, bool A3)
        {
            Aow.Units.AoWE.TCombatAction Result = default;
            Aow.Units.CombatSpells.TRecallSpiritCA Action = default;
            Action = new Aow.Units.CombatSpells.TRecallSpiritCA(null);
            Action.FSourceID = unchecked((byte)A1.FID);
            Action.FTargetID = unchecked((byte)A2.FID);
            Action.FSucceeded = A2.FStatus.Contains(Aow.Units.AoWE.TCombatObjectStatusElement.csDestroyed);
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:1990
        public override void fcGetDamageValueEx(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, Aow.Units.AoWE.TWallSituation A3, ref Aow.Units.AoWE.TAbilityDamageValues A4)
        {
            if (A2.FStatus.Contains(Aow.Units.AoWE.TCombatObjectStatusElement.csDestroyed))
            {
                // { Original leaves the other damage-value fields unchanged on success. }
                A4.SourceDamage = 200;
                A4.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
            }
            else
            {
                A4.SourceDamage = 0;
                A4.SourceCost = 0;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                A4.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
            }
        }

        public static new Aow.Units.CombatSpells.TRecallSpirit Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TRecallSpirit(context, owner));
    }

    // { TAnimateDeadCA (instance size 28, VMT 557F6828): inherited $0C source ID, $0D target ID, $10 mana, $14 success, $18 spell ID. }
    public partial class TAnimateDeadCA : Aow.Units.CombatSpells.TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:2021
        public new const int ClassID = 131782;

        public TAnimateDeadCA(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // CombatSpells.pas:2024
        public override void Execute(Aow.Units.AoWE.TCombat A1)
        {
            Aow.Units.AoWE.TCombatUnit Source = default;
            Aow.Units.AoWE.TCombatUnit Target = default;
            Aow.Units.AoWE.TUnit NewUnit = default;
            base.Execute(A1);
            if (FSucceeded)
            {
                var call__287 = A1.FCombatData.FindID(FSourceID);
                Source = (Aow.Units.AoWE.TCombatUnit)call__287;
                var call__288 = A1.FCombatData.FindID(FTargetID);
                Target = (Aow.Units.AoWE.TCombatUnit)call__288;
                NewUnit = new Aow.Units.AoWE.TUnit(null);
                var call__289 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(201);
                NewUnit.SetUnitResource(call__289);
                NewUnit.SetPlayer(unchecked((sbyte)Source.FUnit.FPlayer));
                NewUnit.ExpandAbility(130);
                NewUnit.Activate();
                Target.SetUnit(NewUnit);
                NewUnit.Release();
                Target.SetStatus(Target.FStatus - TSet<Aow.Units.AoWE.TCombatObjectStatusElement>.Of(Aow.Units.AoWE.TCombatObjectStatusElement.csDestroyed));
                Target.SetCombatPlayer(unchecked((byte)Source.FCombatPlayer));
            }
        }


    }

    // { TAnimateDead (instance size 64, VMT 557F690C): inherited $10 spell ID, $34 combat statistics, $3C damage mode. }
    public partial class TAnimateDead : Aow.Units.CombatSpells.TCombatSpell
    {
        // CombatSpells.pas:2046
        private protected TAnimateDead(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__290 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AnimateDeadRStr);
            FSpellName = call__290;
            FSpellID = 130;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 0;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = unchecked((sbyte)-1);
        }

        // CombatSpells.pas:2079
        public override Aow.Units.AoWE.TCombatAction CreateCA(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, bool A3)
        {
            Aow.Units.AoWE.TCombatAction Result = default;
            Aow.Units.CombatSpells.TAnimateDeadCA Action = default;
            Action = new Aow.Units.CombatSpells.TAnimateDeadCA(null);
            Action.FSourceID = unchecked((byte)A1.FID);
            Action.FTargetID = unchecked((byte)A2.FID);
            Action.FSucceeded = A2.FStatus.Contains(Aow.Units.AoWE.TCombatObjectStatusElement.csDestroyed);
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:2061
        public override void fcGetDamageValueEx(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, Aow.Units.AoWE.TWallSituation A3, ref Aow.Units.AoWE.TAbilityDamageValues A4)
        {
            if (A2.FStatus.Contains(Aow.Units.AoWE.TCombatObjectStatusElement.csDestroyed))
            {
                // { Original leaves the other damage-value fields unchanged on success. }
                A4.SourceDamage = 200;
                A4.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
            }
            else
            {
                A4.SourceDamage = 0;
                A4.SourceCost = 0;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                A4.DamageTypes = TSet<Aow.Units.AoWE.TDamageType>.Of();
            }
        }

        public static new Aow.Units.CombatSpells.TAnimateDead Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TAnimateDead(context, owner));
    }

    public static partial class CombatSpells
    {
        // CombatSpells.pas:2092
        public static void RegisterCombatSpells(Aow.Units.AoWE.TAoWEngine Engine)
        {
            Aow.Units.AoWE.TSpellControl Control = default;
            Control = Engine.FGameSet.FSpellControl;
            Engine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TCombatSpellCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TFastCombatTerrorCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TTacticalCombatTerrorCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TRecallSpiritCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TAnimateDeadCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TFastCombatHighPrayerCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TTacticalCombatHighPrayerCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TEntangleSpellCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TMindDecayCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TTurnUndeadSpellCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TSlowCA>()]);
            var call__292 = Aow.Units.CombatSpells.TSolarFlare.Create(null);
            Control.RegisterSpell(call__292);
            var call__293 = Aow.Units.CombatSpells.TTurnUndead.Create(null);
            Control.RegisterSpell(call__293);
            var call__294 = Aow.Units.CombatSpells.THighPrayer.Create(null);
            Control.RegisterSpell(call__294);
            var call__295 = Aow.Units.CombatSpells.TDeathRay.Create(null);
            Control.RegisterSpell(call__295);
            var call__296 = Aow.Units.CombatSpells.TDiseaseCloud.Create(null);
            Control.RegisterSpell(call__296);
            var call__297 = Aow.Units.CombatSpells.TTerror.Create(null);
            Control.RegisterSpell(call__297);
            var call__298 = Aow.Units.CombatSpells.TMindDecay.Create(null);
            Control.RegisterSpell(call__298);
            var call__299 = Aow.Units.CombatSpells.TChainLightning.Create(null);
            Control.RegisterSpell(call__299);
            var call__300 = Aow.Units.CombatSpells.TColdBreath.Create(null);
            Control.RegisterSpell(call__300);
            var call__301 = Aow.Units.CombatSpells.TSlow.Create(null);
            Control.RegisterSpell(call__301);
            var call__302 = Aow.Units.CombatSpells.TEntangle.Create(null);
            Control.RegisterSpell(call__302);
            var call__303 = Aow.Units.CombatSpells.TTremors.Create(null);
            Control.RegisterSpell(call__303);
            var call__304 = Aow.Units.CombatSpells.TCallFlames.Create(null);
            Control.RegisterSpell(call__304);
            var call__305 = Aow.Units.CombatSpells.TFireBreath.Create(null);
            Control.RegisterSpell(call__305);
            var call__306 = Aow.Units.CombatSpells.TSwarm.Create(null);
            Control.RegisterSpell(call__306);
            var call__307 = Aow.Units.CombatSpells.TGreatHail.Create(null);
            Control.RegisterSpell(call__307);
            var call__308 = Aow.Units.CombatSpells.TFireBall.Create(null);
            Control.RegisterSpell(call__308);
            var call__309 = Aow.Units.CombatSpells.TGeyser.Create(null);
            Control.RegisterSpell(call__309);
            var call__310 = Aow.Units.CombatSpells.TIceShards.Create(null);
            Control.RegisterSpell(call__310);
            var call__311 = Aow.Units.CombatSpells.TFrostBeams.Create(null);
            Control.RegisterSpell(call__311);
            var call__312 = Aow.Units.CombatSpells.TVaporize.Create(null);
            Control.RegisterSpell(call__312);
            var call__313 = Aow.Units.CombatSpells.TStoning.Create(null);
            Control.RegisterSpell(call__313);
            var call__314 = Aow.Units.CombatSpells.TFlamingArrow.Create(null);
            Control.RegisterSpell(call__314);
            var call__315 = Aow.Units.CombatSpells.TOoze.Create(null);
            Control.RegisterSpell(call__315);
            var call__316 = Aow.Units.CombatSpells.TSacredWrath.Create(null);
            Control.RegisterSpell(call__316);
            var call__317 = Aow.Units.CombatSpells.TAnimateDead.Create(null);
            Control.RegisterSpell(call__317);
            var call__318 = Aow.Units.CombatSpells.TRecallSpirit.Create(null);
            Control.RegisterSpell(call__318);
            var call__319 = Aow.Units.CombatSpells.TWindsOfFury.Create(null);
            Control.RegisterSpell(call__319);
            var call__320 = Aow.Units.CombatSpells.TShockwave.Create(null);
            Control.RegisterSpell(call__320);
            var call__321 = Aow.Units.CombatSpells.TSacrificialFlame.Create(null);
            Control.RegisterSpell(call__321);
        }

        // CombatSpells.pas:2130
        public static void UnregisterCombatSpells(Aow.Units.AoWE.TAoWEngine Engine)
        {
            Aow.Units.AoWE.TSpellControl Control = default;
            Control = Engine.FGameSet.FSpellControl;
            Engine.UnregisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TCombatSpellCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TFastCombatTerrorCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TTacticalCombatTerrorCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TRecallSpiritCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TAnimateDeadCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TFastCombatHighPrayerCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TTacticalCombatHighPrayerCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TEntangleSpellCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TMindDecayCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TTurnUndeadSpellCA>(), Aow.Units.Engine.EClass.Of<Aow.Units.CombatSpells.TSlowCA>()]);
            Control.UnregisterSpellID(100);
            Control.UnregisterSpellID(101);
            Control.UnregisterSpellID(102);
            Control.UnregisterSpellID(103);
            Control.UnregisterSpellID(104);
            Control.UnregisterSpellID(105);
            Control.UnregisterSpellID(106);
            Control.UnregisterSpellID(107);
            Control.UnregisterSpellID(108);
            Control.UnregisterSpellID(110);
            Control.UnregisterSpellID(111);
            Control.UnregisterSpellID(112);
            Control.UnregisterSpellID(113);
            Control.UnregisterSpellID(114);
            Control.UnregisterSpellID(115);
            Control.UnregisterSpellID(116);
            Control.UnregisterSpellID(117);
            Control.UnregisterSpellID(118);
            Control.UnregisterSpellID(119);
            Control.UnregisterSpellID(120);
            Control.UnregisterSpellID(121);
            Control.UnregisterSpellID(122);
            Control.UnregisterSpellID(123);
            Control.UnregisterSpellID(124);
            Control.UnregisterSpellID(125);
            Control.UnregisterSpellID(130);
            Control.UnregisterSpellID(129);
            Control.UnregisterSpellID(126);
            Control.UnregisterSpellID(127);
            Control.UnregisterSpellID(128);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
