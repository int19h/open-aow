// Translated from CombatSpells.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.CombatSpells
{
    // spell ID, casting mana. source and target IDs inherited
    public partial class TCombatSpellCA : TDamageCA
    {
        // CombatSpells.pas:447
        public new const int ClassID = 131778;

        public TCombatSpellCA(TECustomNode owner) : base(owner)
        { }

        public int FSpellID;
        public int FManaCost;
        // CombatSpells.pas:452
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FSpellID, 30);
            Stream.rwInteger(ref FManaCost, 31);
        }

        // CombatSpells.pas:459
        public override void Execute(TCombat A1)
        {
            TCombatObject Source = default;
            base.Execute(A1);
            if (FManaCost != 0 && A1.FStatus.Contains(TCombatStatusElement.cExecuting))
            {
                var call__1 = A1.FCombatData.FindID(FSourceID);
                Source = call__1;
                var call__2 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__2.CombatCastingDone(Source, FManaCost);
            }
        }

        // CombatSpells.pas:471
        public override bool Play(TCombatViewer A1)
        {
            bool Result = default;
            if (A1.FTime == 0)
            {
                var call__3 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__3.FCastSounds.Play(0);
            }

            if (A1.FDelay == A1.FTime)
            {
                Execute(A1.FCombat);
            }

            Result = A1.FDelay * 2 > A1.FTime;
            return Result;
        }

        // CombatSpells.pas:479
        public override void ShowObject(TCombatViewer A1, TCombatObject A2, int A3, int A4, int A5, int A6, in TSurfaceDesc A7)
        {
            base.ShowObject(A1, A2, A3, A4, A5, A6, A7);
            if (A2.FID == FSourceID)
            {
                var call__4 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(A5 + 32, A6 + 32 + 8, call__4.FSpellName, A7);
            }
            else
            {
                if (A2.FID == FTargetID)
                {
                    var call__5 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__5.FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
                }
            }
        }

        // CombatSpells.pas:490
        public override void ShowFieldObject(TCombatViewer A1, TCombatObject A2, int A3, int A4, int A5, int A6, in TSurfaceDesc A7)
        {
            // { Original calls ShowObject here, bypassing inherited field markers. }
            base.ShowObject(A1, A2, A3, A4, A5, A6, A7);
            if (A2.FID == FSourceID)
            {
                var call__6 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(A5 + 32, A6 + 32 + 8, call__6.FSpellName, A7);
            }
            else
            {
                if (A2.FID == FTargetID)
                {
                    var call__7 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__7.FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
                }
            }
        }


    }

    // source ID, target ID, casting mana, success, spell ID
    public partial class TExclusiveCombatSpellCA : TCombatAction
    {
        public TExclusiveCombatSpellCA(TECustomNode owner) : base(owner)
        { }

        public byte FSourceID;
        public byte FTargetID;
        public int FManaCost;
        public bool FSucceeded;
        public int FSpellID;
        // CombatSpells.pas:502
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref FSourceID, 20);
            Stream.rwByte(ref FTargetID, 21);
            Stream.rwInteger(ref FManaCost, 22);
            Stream.rwBool(ref FSucceeded, 23);
            Stream.rwInteger(ref FSpellID, 24);
        }

        // CombatSpells.pas:512
        public override void Execute(TCombat A1)
        {
            TCombatObject Source = default;
            if (FManaCost != 0 && A1.FStatus.Contains(TCombatStatusElement.cExecuting))
            {
                var call__9 = A1.FCombatData.FindID(FSourceID);
                Source = call__9;
                var call__10 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__10.CombatCastingDone(Source, FManaCost);
            }
        }

        // CombatSpells.pas:524
        public override bool Play(TCombatViewer A1)
        {
            bool Result = default;
            if (A1.FTime == 0)
            {
                var call__11 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__11.FCastSounds.Play(0);
            }

            if (A1.FDelay == A1.FTime)
            {
                Execute(A1.FCombat);
            }

            Result = A1.FDelay * 3 > A1.FTime;
            return Result;
        }

        // CombatSpells.pas:532
        public override void ShowObject(TCombatViewer A1, TCombatObject A2, int A3, int A4, int A5, int A6, in TSurfaceDesc A7)
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
                var call__12 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(A5 + 32, A6 + 32 + 8, call__12.FSpellName, A7);
            }
            else
            {
                if (A2.FID == FTargetID)
                {
                    var call__13 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__13.FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
                }
            }
        }

        // CombatSpells.pas:545
        public override void ShowFieldObject(TCombatViewer A1, TCombatObject A2, int A3, int A4, int A5, int A6, in TSurfaceDesc A7)
        {
            // { Original calls ShowObject here, bypassing inherited field markers. }
            base.ShowObject(A1, A2, A3, A4, A5, A6, A7);
            if (A2.FID == FSourceID)
            {
                var call__14 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(A5 + 32, A6 + 32 + 8, call__14.FSpellName, A7);
            }
            else
            {
                if (A2.FID == FTargetID)
                {
                    var call__15 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__15.FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
                }
            }
        }
    }

    // source ID, target byte list, casting mana, success, spell ID
    public partial class TMultiTargetCombatSpellCA : TCombatAction
    {
        public byte FSourceID;
        public TByteList FTargetIDs;
        public int FManaCost;
        public bool FSucceeded;
        public int FSpellID;
        // CombatSpells.pas:563
        public override void Destroy()
        {
            FTargetIDs.Free();
            base.Destroy();
        }

        // CombatSpells.pas:569
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref FSourceID, 20);
            Stream.rwStreamProc(21, FTargetIDs.LoadFromStream, FTargetIDs.SaveToStream);
            Stream.rwInteger(ref FManaCost, 22);
            Stream.rwBool(ref FSucceeded, 23);
            Stream.rwInteger(ref FSpellID, 24);
        }

        // CombatSpells.pas:557
        public TMultiTargetCombatSpellCA(TECustomNode owner) : base(owner)
        {
            var call__16 = new TByteList();
            FTargetIDs = call__16;
        }

        // CombatSpells.pas:579
        public override void Execute(TCombat A1)
        {
            TCombatObject Source = default;
            if (FManaCost != 0 && A1.FStatus.Contains(TCombatStatusElement.cExecuting))
            {
                var call__18 = A1.FCombatData.FindID(FSourceID);
                Source = call__18;
                var call__19 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__19.CombatCastingDone(Source, FManaCost);
            }
        }

        // CombatSpells.pas:591
        public override bool Play(TCombatViewer A1)
        {
            bool Result = default;
            if (A1.FTime == 0)
            {
                var call__20 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__20.FCastSounds.Play(0);
            }

            if (A1.FDelay == A1.FTime)
            {
                Execute(A1.FCombat);
            }

            Result = A1.FDelay * 5 > A1.FTime;
            return Result;
        }

        // CombatSpells.pas:599
        public override void ShowObject(TCombatViewer A1, TCombatObject A2, int A3, int A4, int A5, int A6, in TSurfaceDesc A7)
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
                var call__21 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(A5 + 32, A6 + 32 + 8, call__21.FSpellName, A7);
            }
            else
            {
                var call__22 = FTargetIDs.IndexOf(A2.FID);
                if (call__22 != -1)
                {
                    var call__23 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__23.FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
                }
            }
        }

        // CombatSpells.pas:612
        public override void ShowFieldObject(TCombatViewer A1, TCombatObject A2, int A3, int A4, int A5, int A6, in TSurfaceDesc A7)
        {
            // { Original calls ShowObject here, bypassing inherited field markers. }
            base.ShowObject(A1, A2, A3, A4, A5, A6, A7);
            if (A2.FID == FSourceID)
            {
                var call__24 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(A5 + 32, A6 + 32 + 8, call__24.FSpellName, A7);
            }
            else
            {
                var call__25 = FTargetIDs.IndexOf(A2.FID);
                if (call__25 != -1)
                {
                    var call__26 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__26.FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
                }
            }
        }

        // CombatSpells.pas:624
        public override void ShowFightingObjects(TCombatViewer A1, int A2, int A3, int A4, in TSurfaceDesc A5)
        {
            TCombatObject Source = default;
            var call__27 = A1.FCombat.FCombatData.FindID(FSourceID);
            Source = call__27;
            A1.DefaultShowFightingObjects(null, Source, A2, A3, A4, A5);
        }

    }

    // combat statistics (attack, damage, types, repeats, range, radius, targets). damage mode
    public partial class TCombatSpell : TSpell
    {
        public TSpellCombatInfo FCombatInfo;
        public byte FDamageMode;
        // CombatSpells.pas:632
        public TCombatSpell(TECustomNode owner) : base(owner)
        {
            FSpellType = TSpellType.stCombat;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctCombat);
            FCombatInfo.Radius = 1;
            FCombatInfo.TargetTypes = TSet<TCombatTargetTypeElement>.Of(TCombatTargetTypeElement.cttEnemy);
            FCastDuration = 5;
            FDamageMode = 0;
        }

        // CombatSpells.pas:655
        public override TCombatAction CreateCA(TCombatObject A1, TCombatObject A2, bool A3)
        {
            TCombatAction Result = default;
            TCombatSpellCA Action = default;
            Action = new TCombatSpellCA(null);
            Action.FSpellID = FSpellID;
            Action.GenerateEx(A1, A2, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, FDamageMode);
            if (A3)
            {
                var call__28 = ((THero)((TCombatUnit)A1).FUnit).CastingMana(this);
                Action.FManaCost = call__28;
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:643
        public override void GetCombatInfo(TAbstractUnit Source, ref TSpellCombatInfo Info)
        {
            Info = FCombatInfo;
        }

        // CombatSpells.pas:648
        public override int GetCombatDamageValue(TCombatObject A1, TCombatObject A2)
        {
            int Result = default;
            var call__29 = A2.GetHitPoints();
            var call__30 = A2.GetDefense();
            var call__31 = A2.GetResistance();
            var call__32 = A2.GetImmunityTypes();
            var call__33 = A2.GetProtectionTypes();
            var call__34 = StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, call__29, call__30, call__31, call__32, call__33);
            Result = call__34 * FCombatInfo.RepeatCount;
            return Result;
        }

        // CombatSpells.pas:668
        public override void GetCombatDamageValueEx(TCombatObject A1, TCombatObject A2, ref TAbilityDamageValues A3)
        {
            TSet<TDamageType> Protection = default;
            TSet<TDamageType> Immunity = default;
            Protection = A2.GetProtectionTypes();
            Immunity = A2.GetImmunityTypes();
            A3.DamageTypes = FCombatInfo.DamageTypes;
            var call__35 = A2.GetHitPoints();
            var call__36 = A2.GetDefense();
            var call__37 = A2.GetResistance();
            var call__38 = StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, call__35, call__36, call__37, Immunity, Protection);
            A3.SourceDamage = call__38 * FCombatInfo.RepeatCount;
            // { Original recomputes the value, using resistance in place of defense for mode 1. }
            if (FDamageMode == 1)
            {
                var call__39 = A2.GetHitPoints();
                var call__40 = A2.GetResistance();
                var call__41 = A2.GetResistance();
                var call__42 = StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, call__39, call__40, call__41, Immunity, Protection);
                A3.SourceDamage = call__42 * FCombatInfo.RepeatCount;
            }
            else
            {
                var call__43 = A2.GetHitPoints();
                var call__44 = A2.GetDefense();
                var call__45 = A2.GetResistance();
                var call__46 = StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, call__43, call__44, call__45, Immunity, Protection);
                A3.SourceDamage = call__46 * FCombatInfo.RepeatCount;
            }

            var call__47 = StatisticsToMaxDamage(FCombatInfo.Damage, FCombatInfo.DamageTypes, Protection, Immunity);
            A3.SourceCost = call__47 * FCombatInfo.RepeatCount;
            A3.TargetDamage = 0;
            A3.TargetCost = 0;
        }

        // CombatSpells.pas:754
        public override void fcPrefetchCombatCommands(TFastCombatCommandList A1)
        {
            TAbstractUnit Hero = default;
            int Distance = default;
            int I = default;
            AnsiString Reason = default;
            TCombatObject Target = default;
            TSpellCC Command = default;
            TAbilityDamageValues Values = default;
            Hero = ((TCombatUnit)A1.FSource).FUnit;
            bool bool__48 = Hero is THero;
            if (bool__48)
            {
                var call__49 = ((THero)Hero).CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason);
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
                            bool__54 = call__55 <= Distance;
                        }

                        if (bool__54)
                        {
                            var call__56 = ((TFastCombat)A1.FSource.FCombat).GetWallSituation(A1.FSource, Target);
                            fcGetDamageValueEx(A1.FSource, Target, call__56, ref Values);
                            if (Values.SourceDamage != 0)
                            {
                                Command = new TSpellCC(52);
                                var call__57 = A1.FTargetSide.GetObjects(I);
                                Command.FTarget = call__57;
                                Command.FMoveCost = 1;
                                Command.FSpellID = FSpellID;
                                var call__58 = DVtoDEV(Values.SourceDamage, Values.TargetDamage, A1.FSource, Target);
                                Command.FExpectedDV = call__58;
                                A1.Add(Command);
                            }
                        }
                    }
                }
            }
        }

        // CombatSpells.pas:791
        public override bool fcExecuteCombatCommand(TCombatObject A1, TFastCombatCommand A2)
        {
            bool Result = default;
            int I = default;
            int J = default;
            int Count = default;
            TCombatAction Action = default;
            TCombatObject Other = default;
            int Distance = default;
            TCombatObject Target = default;
            TList Targets = default;
            TFastCombat Combat = default;
            Target = A2.FTarget;
            {
                I = 0;
                int end__59 = FCombatInfo.RepeatCount - 1;
                for (; I <= end__59; I += 1)
                {
                    if (!Target.FStatus.Contains(TCombatObjectStatusElement.csDestroyed))
                    {
                        Action = CreateCA(A1, Target, I == 0);
                        A1.FCombat.ExecuteCombatAction(Action);
                        Action.Release();
                    }
                }
            }

            if (FCombatInfo.Radius > 1)
            {
                Combat = (TFastCombat)A1.FCombat;
                Targets = new TList();
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
                        bool bool__63 = Other != Target;
                        if (bool__63)
                        {
                            var call__64 = Other.GetEnabled();
                            bool__63 = call__64;
                        }

                        bool bool__65 = bool__63 && (int)(((uint)Other.FPosition) >> 4) == (int)(((uint)Target.FPosition) >> 4);
                        if (bool__65)
                        {
                            var call__66 = Other.fcRoundDistance();
                            bool__65 = Distance >= call__66;
                        }

                        bool bool__67 = bool__65;
                        if (bool__67)
                        {
                            var call__68 = Other.GetImmunityTypes();
                            bool__67 = FCombatInfo.DamageTypes - call__68 != TSet<TDamageType>.Of();
                        }

                        if (bool__67)
                        {
                            Targets.Add(Other);
                        }
                    }
                }

                // { Original shifts Random(4) by two, so this adds no random extra targets. }
                var call__69 = AoWHSMap.Random(4);
                Count = (int)(((uint)call__69) >> 2) + FCombatInfo.Radius;
                while (Count != 0 && Targets.Count != 0)
                {
                    var call__70 = AoWHSMap.Random(Targets.Count);
                    var call__71 = Targets[call__70];
                    Other = (TCombatObject)call__71;
                    {
                        J = 0;
                        int end__72 = FCombatInfo.RepeatCount - 1;
                        for (; J <= end__72; J += 1)
                        {
                            if (!Other.FStatus.Contains(TCombatObjectStatusElement.csDestroyed))
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
        public override void fcGetDamageValueEx(TCombatObject A1, TCombatObject A2, TWallSituation A3, ref TAbilityDamageValues A4)
        {
            TSet<TDamageType> DamageTypes = default;
            TSet<TDamageType> Protection = default;
            TSet<TDamageType> Immunity = default;
            int Count = default;
            int I = default;
            int Distance = default;
            TCombatObject Other = default;
            TFastCombat Combat = default;
            int Damage = default;
            int Defense = default;
            if (A3 == TWallSituation.wsNone || FCombatInfo.Range == -1 || FCombatInfo.Range >= 8)
            {
                Protection = A2.GetProtectionTypes();
                Immunity = A2.GetImmunityTypes();
                DamageTypes = FCombatInfo.DamageTypes;
                Damage = FCombatInfo.Damage;
                Defense = A2.GetDefense();
                if (A3 == TWallSituation.wsTarget && (TSet<TDamageType>.Of(TDamageType.dtMagic, TDamageType.dtDeath, TDamageType.dtHoly) & FCombatInfo.DamageTypes) == TSet<TDamageType>.Of())
                {
                    Defense += 2;
                }

                A4.DamageTypes = DamageTypes;
                var call__74 = A2.GetHitPoints();
                var call__75 = A2.GetResistance();
                var call__76 = StatisticsToLimitedDV(FCombatInfo.Attack, Damage, DamageTypes, call__74, Defense, call__75, Immunity, Protection);
                A4.SourceDamage = call__76 * FCombatInfo.RepeatCount;
                if (FCombatInfo.Radius > 1)
                {
                    A4.SourceDamage = (int)(((uint)(A4.SourceDamage * FCombatInfo.Radius)) >> 1);
                }

                var call__77 = StatisticsToMaxDamage(Damage, DamageTypes, Protection, Immunity);
                A4.SourceCost = call__77 * FCombatInfo.RepeatCount;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                if (A4.SourceDamage != 0 && FCombatInfo.Radius > 1)
                {
                    Combat = (TFastCombat)A1.FCombat;
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
                            bool bool__81 = A2 != Other;
                            if (bool__81)
                            {
                                var call__82 = Other.GetEnabled();
                                bool__81 = call__82;
                            }

                            bool bool__83 = bool__81 && (int)(((uint)Other.FPosition) >> 4) == (int)(((uint)A2.FPosition) >> 4);
                            if (bool__83)
                            {
                                var call__84 = Other.fcRoundDistance();
                                bool__83 = call__84 <= Distance;
                            }

                            bool bool__85 = bool__83;
                            if (bool__85)
                            {
                                var call__86 = Other.GetImmunityTypes();
                                bool__85 = DamageTypes - call__86 != TSet<TDamageType>.Of();
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

                        A4.SourceDamage += Round((double)(A4.SourceDamage * Count) * 0.5);
                    }
                }
            }
            else
            {
                A4.SourceDamage = 0;
                A4.SourceCost = 0;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                A4.DamageTypes = TSet<TDamageType>.Of();
            }
        }
    }

    // inherited combat statistics, damage mode
    public partial class TSolarFlare : TCombatSpell
    {
        // CombatSpells.pas:845
        public TSolarFlare(TECustomNode owner) : base(owner)
        {
            var call__87 = TranslateRStr(SolarFlareRStr);
            FSpellName = call__87;
            FSpellID = 100;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 4;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtHoly);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }
    }

    public partial class TTurnUndeadSpellCA : TCombatSpellCA
    {
        // CombatSpells.pas:861
        public new const int ClassID = 131788;

        public TTurnUndeadSpellCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:867
        public override void Execute(TCombat A1)
        {
            TCombatObject Source = default;
            TCombatUnit Target = default;
            TUnitEnchantmentAbilityData Data = default;
            base.Execute(A1);
            if (FDamage != 0)
            {
                var call__89 = A1.FCombatData.FindID(FSourceID);
                Source = call__89;
                var call__90 = A1.FCombatData.FindID(FTargetID);
                Target = (TCombatUnit)call__90;
                if (!Target.FStatus.Contains(TCombatObjectStatusElement.csDestroyed))
                {
                    var call__91 = AoWHSSet.FAbilityControl.GetAbility(34);
                    call__91.Expand(Target.FUnit);
                    var call__92 = Target.FUnit.GetAbilityData(34);
                    Data = (TUnitEnchantmentAbilityData)call__92;
                    if (Data != null)
                    {
                        Data.FEnchantmentPlayerIndex = Source.FCombatPlayer;
                    }
                }
            }
        }


    }

    // inherited combat statistics, damage mode
    public partial class TTurnUndead : TCombatSpell
    {
        // CombatSpells.pas:888
        public TTurnUndead(TECustomNode owner) : base(owner)
        {
            var call__93 = TranslateRStr(TurnUndeadRStr);
            FSpellName = call__93;
            FSpellID = 101;
            FDamageMode = 1;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtHoly);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        // CombatSpells.pas:905
        public override TCombatAction CreateCA(TCombatObject A1, TCombatObject A2, bool A3)
        {
            TCombatAction Result = default;
            TTurnUndeadSpellCA Action = default;
            Action = new TTurnUndeadSpellCA(null);
            Action.FSpellID = FSpellID;
            Action.FSourceID = A1.FID;
            Action.FTargetID = A2.FID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            bool bool__95 = A2 is TCombatUnit;
            if (bool__95)
            {
                var call__96 = ((TCombatUnit)A2).FUnit.GetUnitType();
                bool__95 = TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature).Contains(call__96);
            }

            bool bool__97 = bool__95;
            if (bool__97)
            {
                var call__98 = ((TCombatUnit)A2).FUnit.GetRace();
                bool bool__99 = call__98 == 11;
                if (!bool__99)
                {
                    var call__100 = ((TCombatUnit)A2).FUnit.GetAbilityEnabled(129);
                    bool__99 = call__100;
                }

                bool__97 = bool__99;
            }

            if (bool__97)
            {
                Action.GenerateEx(A1, A2, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, FDamageMode);
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:922
        public override void fcGetDamageValueEx(TCombatObject A1, TCombatObject A2, TWallSituation A3, ref TAbilityDamageValues A4)
        {
            bool bool__101 = A2 is TCombatUnit;
            if (bool__101)
            {
                var call__102 = ((TCombatUnit)A2).FUnit.GetUnitType();
                bool__101 = TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature).Contains(call__102);
            }

            bool bool__103 = bool__101;
            if (bool__103)
            {
                var call__104 = ((TCombatUnit)A2).FUnit.GetRace();
                bool bool__105 = call__104 == 11;
                if (!bool__105)
                {
                    var call__106 = ((TCombatUnit)A2).FUnit.GetAbilityEnabled(129);
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
                A4.DamageTypes = TSet<TDamageType>.Of();
            }
        }
    }

    public partial class TFastCombatHighPrayerCA : TMultiTargetCombatSpellCA
    {
        // CombatSpells.pas:937
        public new const int ClassID = 131783;

        public TFastCombatHighPrayerCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:943
        public override void Execute(TCombat A1)
        {
            int I = default;
            TCombatObject Target = default;
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
                    Target.SetHitPoints((byte)(call__110 + 5));
                    var call__111 = AoWHSSet.FAbilityControl.GetAbility(100);
                    call__111.Expand(((TCombatUnit)Target).FUnit);
                }
            }
        }

        // CombatSpells.pas:958
        public void Generate(TCombatObject A1)
        {
            int I = default;
            TCombatSide Side = default;
            TCombatObject Target = default;
            FSpellID = 102;
            FSourceID = A1.FID;
            var combat = A1.FCombat;
            var call__113 = A1.GetSide();
            Side = combat.FSides[(int)call__113];

            {
                I = Side.GetCount() - 1;
                byte end__114 = 0;
                for (; I >= end__114; I -= 1)
                {
                    Target = Side.GetObjects(I);
                    if (Target is TCombatUnit && !Target.FStatus.Contains(TCombatObjectStatusElement.csDestroyed))
                    {
                        FTargetIDs.Add(Target.FID);
                    }
                }
            }
        }


    }

    public partial class TTacticalCombatHighPrayerCA : TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:976
        public new const int ClassID = 131786;

        public TTacticalCombatHighPrayerCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:982
        public override void Execute(TCombat A1)
        {
            TCombatObject Target = default;
            base.Execute(A1);
            if (FSucceeded)
            {
                var call__115 = A1.FCombatData.FindID(FTargetID);
                Target = call__115;
                var call__116 = Target.GetHitPoints();
                Target.SetHitPoints((byte)(call__116 + 5));
                var call__117 = AoWHSSet.FAbilityControl.GetAbility(100);
                call__117.Expand(((TCombatUnit)Target).FUnit);
            }
        }


    }

    // inherited combat statistics, damage mode
    public partial class THighPrayer : TCombatSpell
    {
        // CombatSpells.pas:996
        public THighPrayer(TECustomNode owner) : base(owner)
        {
            var call__118 = TranslateRStr(HighPrayerRStr);
            FSpellName = call__118;
            FSpellID = 102;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of();
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = -1;
            spellCombatInfo.TargetTypes = TSet<TCombatTargetTypeElement>.Of(TCombatTargetTypeElement.cttFriend);
        }

        // CombatSpells.pas:1013
        public override TCombatAction CreateCA(TCombatObject A1, TCombatObject A2, bool A3)
        {
            TCombatAction Result = default;
            TTacticalCombatHighPrayerCA Action = default;
            Action = new TTacticalCombatHighPrayerCA(null);
            Action.FSucceeded = true;
            Action.FSourceID = A1.FID;
            Action.FTargetID = A2.FID;
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:1104
        public override void tcGetDamageValueEx(TCombatObject A1, TCombatObject A2, ref TAbilityDamageValues A3)
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
                A3.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtDeath);
            }
            else
            {
                A3.SourceDamage = 0;
                A3.SourceCost = 0;
                A3.TargetDamage = 0;
                A3.TargetCost = 0;
                A3.DamageTypes = TSet<TDamageType>.Of();
            }
        }

        // CombatSpells.pas:1026
        public override void fcPrefetchCombatCommands(TFastCombatCommandList A1)
        {
            int I = default;
            TCombatSide Side = default;
            TCombatObject Target = default;
            int Value = default;
            TSpellCC Command = default;
            Value = 0;
            var call__122 = A1.FSource.GetSide();
            Side = A1.FCombat.FSides[(int)call__122];
            {
                I = Side.GetCount() - 1;
                byte end__123 = 0;
                for (; I >= end__123; I -= 1)
                {
                    Target = Side.GetObjects(I);
                    if (Target.FCombatValue != 0 && Target is TCombatUnit)
                    {
                        var call__124 = Target.GetHits();
                        var call__125 = Target.GetHitPoints();
                        var call__126 = DVtoDEV((call__124 - call__125) * 100, 0, A1.FSource, Target);
                        Value += call__126;
                    }
                }
            }

            if (Value != 0)
            {
                Command = new TSpellCC(52);
                Command.FTarget = null;
                Command.FMoveCost = 1;
                Command.FSpellID = FSpellID;
                Command.FExpectedDV = Value;
                A1.Add(Command);
            }
        }

        // CombatSpells.pas:1054
        public override bool fcExecuteCombatCommand(TCombatObject A1, TFastCombatCommand A2)
        {
            bool Result = default;
            TFastCombatHighPrayerCA Action = default;
            Action = new TFastCombatHighPrayerCA(null);
            Action.Generate(A1);
            var call__127 = ((THero)((TCombatUnit)A1).FUnit).CastingMana(this);
            Action.FManaCost = call__127;
            A1.FCombat.ExecuteCombatAction(Action);
            Action.Release();
            Result = true;
            return Result;
        }

        // CombatSpells.pas:1067
        public override void fcGetDamageValueEx(TCombatObject A1, TCombatObject A2, TWallSituation A3, ref TAbilityDamageValues A4)
        {
            int I = default;
            int Value = default;
            int Healing = default;
            TCombatSide Side = default;
            TCombatObject Target = default;
            Value = 0;
            var call__128 = A1.GetSide();
            Side = A1.FCombat.FSides[(int)call__128];
            {
                I = Side.GetCount() - 1;
                byte end__129 = 0;
                for (; I >= end__129; I -= 1)
                {
                    Target = Side.GetObjects(I);
                    if (Target.FCombatValue != 0 && Target is TCombatUnit)
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
                A4.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtDeath);
            }
            else
            {
                A4.SourceDamage = 0;
                A4.SourceCost = 0;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                A4.DamageTypes = TSet<TDamageType>.Of();
            }
        }
    }

    // inherited combat statistics, damage mode
    public partial class TDeathRay : TCombatSpell
    {
        // CombatSpells.pas:1130
        public TDeathRay(TECustomNode owner) : base(owner)
        {
            var call__132 = TranslateRStr(DeathRayRStr);
            FSpellName = call__132;
            FSpellID = 103;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 4;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtDeath);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TDiseaseCloud : TCombatSpell
    {
        // CombatSpells.pas:1146
        public TDiseaseCloud(TECustomNode owner) : base(owner)
        {
            var call__134 = TranslateRStr(DiseaseCloudRStr);
            FSpellName = call__134;
            FSpellID = 104;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 3;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtDeath);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }
    }

    public partial class TMindDecayCA : TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1162
        public new const int ClassID = 131787;

        public TMindDecayCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:1168
        public override void Execute(TCombat A1)
        {
            TCombatObject Source = default;
            TCombatUnit Target = default;
            int I = default;
            base.Execute(A1);
            if (FSucceeded)
            {
                var call__136 = A1.FCombatData.FindID(FSourceID);
                Source = call__136;
                var call__137 = A1.FCombatData.FindID(FTargetID);
                Target = (TCombatUnit)call__137;
                var call__138 = Source.GetExperience();
                var call__139 = Target.GetLevel();
                Source.SetExperience(call__138 + call__139);
                {
                    I = 0;
                    byte end__140 = 2;
                    for (; I <= end__140; I += 1)
                    {
                        var call__141 = AoWHSSet.FAbilityControl.GetAbility(CommandAbilityIDs[I]);
                        ((TCommandAbility)call__141).ResetCommandedUnits(Target);
                    }
                }

                {
                    I = 0;
                    byte end__142 = 2;
                    for (; I <= end__142; I += 1)
                    {
                        var call__143 = AoWHSSet.FAbilityControl.GetAbility(CommandAbilityIDs[I]);
                        ((TCommandAbility)call__143).Uncommand(Target);
                    }
                }

                var call__144 = AoWHSSet.FAbilityControl.GetAbility(131);
                call__144.Expand(Target.FUnit);
                Target.SetCombatPlayer((byte)Source.FCombatPlayer);
            }
        }


    }

    // inherited combat statistics, damage mode
    public partial class TMindDecay : TCombatSpell
    {
        // CombatSpells.pas:1190
        public TMindDecay(TECustomNode owner) : base(owner)
        {
            var call__145 = TranslateRStr(MindDecayRStr);
            FSpellName = call__145;
            FSpellID = 106;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 5;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtNone);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 3;
        }

        // CombatSpells.pas:1207
        public override TCombatAction CreateCA(TCombatObject A1, TCombatObject A2, bool A3)
        {
            TCombatAction Result = default;
            TMindDecayCA Action = default;
            Action = new TMindDecayCA(null);
            Action.FSourceID = A1.FID;
            Action.FTargetID = A2.FID;
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            bool bool__147 = A2 is TCombatUnit;
            if (bool__147)
            {
                var call__148 = ((TCombatUnit)A2).FUnit.GetUnitType();
                bool__147 = call__148 != TUnitType.utMachine;
            }

            bool bool__149 = bool__147;
            if (bool__149)
            {
                var call__150 = ((TCombatUnit)A2).FUnit.GetRace();
                bool__149 = call__150 != 11;
            }

            bool bool__151 = bool__149;
            if (bool__151)
            {
                var call__152 = ((TCombatUnit)A2).FUnit.GetAbilityEnabled(129);
                bool__151 = !call__152;
            }

            if (bool__151 && !(((TCombatUnit)A2).FUnit is TLeader))
            {
                var call__153 = A2.GetResistance();
                var call__154 = HitRole(FCombatInfo.Attack - call__153);
                Action.FSucceeded = call__154;
            }

            Result = Action;
            return Result;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TChainLightning : TCombatSpell
    {
        // CombatSpells.pas:1224
        public TChainLightning(TECustomNode owner) : base(owner)
        {
            var call__155 = TranslateRStr(ChainLightningRStr);
            FSpellName = call__155;
            FSpellID = 107;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 4;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtLightning);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 3;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TColdBreath : TCombatSpell
    {
        // CombatSpells.pas:1241
        public TColdBreath(TECustomNode owner) : base(owner)
        {
            var call__157 = TranslateRStr(ColdBreathRStr);
            FSpellName = call__157;
            FSpellID = 108;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtCold);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 4;
        }
    }

    public partial class TSlowCA : TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1257
        public new const int ClassID = 131792;

        public TSlowCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:1263
        public override void Execute(TCombat A1)
        {
            TCombatUnit Target = default;
            TCombatObject Source = default;
            TUnitEnchantment Enchantment = default;
            base.Execute(A1);
            if (FSucceeded)
            {
                var call__159 = A1.FCombatData.FindID(FTargetID);
                Target = (TCombatUnit)call__159;
                var call__160 = A1.FCombatData.FindID(FSourceID);
                Source = call__160;
                var call__161 = Target.FUnit.ExpandAbility(132);
                if (call__161)
                {
                    var call__162 = Target.FUnit.GetAbilityData(132);
                    Enchantment = ((TUnitEnchantmentAbilityData)call__162).FEnchantment;
                    Enchantment.SetUpkeep(0);
                    var call__163 = Source.GetPlayer();
                    Enchantment.SetSource(call__163);
                }
            }
        }


    }

    // inherited combat statistics, damage mode
    public partial class TSlow : TCombatSpell
    {
        // CombatSpells.pas:1284
        public TSlow(TECustomNode owner) : base(owner)
        {
            var call__164 = TranslateRStr(SlowRStr);
            FSpellName = call__164;
            FSpellID = 110;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 9;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtNone);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        // CombatSpells.pas:1313
        public override TCombatAction CreateCA(TCombatObject A1, TCombatObject A2, bool A3)
        {
            TCombatAction Result = default;
            TSlowCA Action = default;
            Action = new TSlowCA(null);
            Action.FSourceID = A1.FID;
            Action.FTargetID = A2.FID;
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            if (!A2.GetAbilityEnabled(132))
            {
                var call__166 = A2.GetResistance();
                var call__167 = HitRole(FCombatInfo.Attack - call__166);
                Action.FSucceeded = call__167;
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:1300
        public override void GetCombatDamageValueEx(TCombatObject A1, TCombatObject A2, ref TAbilityDamageValues A3)
        {
            ref var abilityDamageValues = ref A3;
            abilityDamageValues.SourceDamage = 0;
            abilityDamageValues.SourceCost = 0;
            abilityDamageValues.TargetDamage = 0;
            abilityDamageValues.TargetCost = 0;
            abilityDamageValues.DamageTypes = TSet<TDamageType>.Of();
        }
    }

    public partial class TEntangleSpellCA : TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1328
        public new const int ClassID = 131784;

        public TEntangleSpellCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:1334
        public override void Execute(TCombat A1)
        {
            TCombatObject Source = default;
            TCombatUnit Target = default;
            TUnitEnchantmentAbilityData Data = default;
            base.Execute(A1);
            if (FSucceeded)
            {
                var call__169 = A1.FCombatData.FindID(FSourceID);
                Source = call__169;
                var call__170 = A1.FCombatData.FindID(FTargetID);
                Target = (TCombatUnit)call__170;
                var call__171 = AoWHSSet.FAbilityControl.GetAbility(94);
                call__171.Expand(Target.FUnit);
                var call__172 = Target.FUnit.GetAbilityData(94);
                Data = (TUnitEnchantmentAbilityData)call__172;
                if (Data != null)
                {
                    Data.FEnchantmentPlayerIndex = Source.FCombatPlayer;
                }
            }
        }


    }

    // inherited combat statistics, damage mode
    public partial class TEntangle : TCombatSpell
    {
        // CombatSpells.pas:1352
        public TEntangle(TECustomNode owner) : base(owner)
        {
            var call__173 = TranslateRStr(EntangleRStr);
            FSpellName = call__173;
            FSpellID = 111;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 7;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtNone);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        // CombatSpells.pas:1368
        public override TCombatAction CreateCA(TCombatObject A1, TCombatObject A2, bool A3)
        {
            TCombatAction Result = default;
            TEntangleSpellCA Action = default;
            Action = new TEntangleSpellCA(null);
            Action.FSourceID = A1.FID;
            Action.FTargetID = A2.FID;
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
                var call__179 = HitRole(FCombatInfo.Attack - call__178);
                Action.FSucceeded = call__179;
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:1383
        public override void GetCombatDamageValueEx(TCombatObject A1, TCombatObject A2, ref TAbilityDamageValues A3)
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
                var call__184 = HitRoleProbability(FCombatInfo.Attack - call__183);
                var call__185 = A2.GetHitPoints();
                A3.SourceDamage = Round((double)(call__185 * 100) * call__184);
                var call__186 = A2.GetHitPoints();
                A3.SourceCost = call__186;
                A3.TargetDamage = 0;
                A3.TargetCost = 0;
                A3.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtNone);
            }
            else
            {
                A3.SourceDamage = 0;
                A3.SourceCost = 0;
                A3.TargetDamage = 0;
                A3.TargetCost = 0;
                A3.DamageTypes = TSet<TDamageType>.Of();
            }
        }
    }

    // inherited combat statistics, damage mode
    public partial class TTremors : TCombatSpell
    {
        // CombatSpells.pas:1404
        public TTremors(TECustomNode owner) : base(owner)
        {
            // { Original leaves this spell name untranslated. }
            FSpellName = TremorsRStr;
            FSpellID = 112;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtPhysical, TDamageType.dtWall);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = -1;
            spellCombatInfo.Radius = 5;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TCallFlames : TCombatSpell
    {
        // CombatSpells.pas:1421
        public TCallFlames(TECustomNode owner) : base(owner)
        {
            var call__188 = TranslateRStr(CallFlamesRStr);
            FSpellName = call__188;
            FSpellID = 113;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 9;
            spellCombatInfo.Damage = 3;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtFire);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 8;
            spellCombatInfo.Radius = 1;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TFireBreath : TCombatSpell
    {
        // CombatSpells.pas:1438
        public TFireBreath(TECustomNode owner) : base(owner)
        {
            var call__190 = TranslateRStr(FireBreathRStr);
            FSpellName = call__190;
            FSpellID = 114;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtFire);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 4;
            spellCombatInfo.Radius = 3;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TSwarm : TCombatSpell
    {
        // CombatSpells.pas:1455
        public TSwarm(TECustomNode owner) : base(owner)
        {
            var call__192 = TranslateRStr(SwarmRStr);
            FSpellName = call__192;
            FSpellID = 115;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 2;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtFire);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 5;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TGreatHail : TCombatSpell
    {
        // CombatSpells.pas:1472
        public TGreatHail(TECustomNode owner) : base(owner)
        {
            var call__194 = TranslateRStr(GreatHailRStr);
            FSpellName = call__194;
            FSpellID = 116;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 5;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtPhysical, TDamageType.dtWall);
            spellCombatInfo.RepeatCount = 3;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 2;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TFireBall : TCombatSpell
    {
        // CombatSpells.pas:1489
        public TFireBall(TECustomNode owner) : base(owner)
        {
            var call__196 = TranslateRStr(FireBallRStr);
            FSpellName = call__196;
            FSpellID = 117;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 6;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtFire, TDamageType.dtWall);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TGeyser : TCombatSpell
    {
        // CombatSpells.pas:1505
        public TGeyser(TECustomNode owner) : base(owner)
        {
            var call__198 = TranslateRStr(GeyserRStr);
            FSpellName = call__198;
            FSpellID = 118;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 9;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtPhysical);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 1;
        }

        // CombatSpells.pas:1522
        public override TCombatAction CreateCA(TCombatObject A1, TCombatObject A2, bool A3)
        {
            TCombatAction Result = default;
            TCombatSpellCA Action = default;
            Action = new TCombatSpellCA(null);
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
                Action.Generate(A1, A2, FCombatInfo.Attack, (sbyte)(FCombatInfo.Damage / 2), FCombatInfo.DamageTypes);
            }
            else
            {
                Action.Generate(A1, A2, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes);
            }

            if (A3)
            {
                var call__203 = ((THero)((TCombatUnit)A1).FUnit).CastingMana(this);
                Action.FManaCost = call__203;
            }

            Result = Action;
            return Result;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TIceShards : TCombatSpell
    {
        // CombatSpells.pas:1537
        public TIceShards(TECustomNode owner) : base(owner)
        {
            var call__204 = TranslateRStr(IceShardsRStr);
            FSpellName = call__204;
            FSpellID = 119;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 4;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtPhysical);
            spellCombatInfo.RepeatCount = 3;
            spellCombatInfo.Range = 12;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TFrostBeams : TCombatSpell
    {
        // CombatSpells.pas:1553
        public TFrostBeams(TECustomNode owner) : base(owner)
        {
            var call__206 = TranslateRStr(FrostBeamsRStr);
            FSpellName = call__206;
            FSpellID = 120;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 9;
            spellCombatInfo.Damage = 3;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtCold);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 3;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TVaporize : TCombatSpell
    {
        // CombatSpells.pas:1570
        public TVaporize(TECustomNode owner) : base(owner)
        {
            var call__208 = TranslateRStr(VaporizeRStr);
            FSpellName = call__208;
            FSpellID = 121;
            FDamageMode = 1;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 7;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtPhysical);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TFlamingArrow : TCombatSpell
    {
        // CombatSpells.pas:1603
        public TFlamingArrow(TECustomNode owner) : base(owner)
        {
            var call__210 = TranslateRStr(FlamingArrowRStr);
            FSpellName = call__210;
            FSpellID = 123;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 4;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtFire, TDamageType.dtPhysical);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TStoning : TCombatSpell
    {
        // CombatSpells.pas:1587
        public TStoning(TECustomNode owner) : base(owner)
        {
            var call__212 = TranslateRStr(StoningRStr);
            FSpellName = call__212;
            FSpellID = 122;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 5;
            spellCombatInfo.Damage = 2;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtPhysical);
            spellCombatInfo.RepeatCount = 6;
            spellCombatInfo.Range = 12;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TOoze : TCombatSpell
    {
        // CombatSpells.pas:1619
        public TOoze(TECustomNode owner) : base(owner)
        {
            var call__214 = TranslateRStr(OozeRStr);
            FSpellName = call__214;
            FSpellID = 124;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 0;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of();
            spellCombatInfo.RepeatCount = 0;
            spellCombatInfo.Range = 8;
        }

        // CombatSpells.pas:1635
        public override void GetCombatDamageValueEx(TCombatObject A1, TCombatObject A2, ref TAbilityDamageValues A3)
        {
            TSet<TDamageType> Protection = default;
            TSet<TDamageType> Immunity = default;
            Protection = A2.GetProtectionTypes();
            Immunity = A2.GetImmunityTypes();
            A3.DamageTypes = FCombatInfo.DamageTypes;
            var call__216 = A2.GetResistance();
            var call__217 = HitRoleProbability(FCombatInfo.Attack - call__216);
            A3.SourceDamage = Round((double)call__217 * 200);
            A3.SourceCost = 200;
            A3.TargetDamage = 0;
            A3.TargetCost = 0;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TSacredWrath : TCombatSpell
    {
        // CombatSpells.pas:1649
        public TSacredWrath(TECustomNode owner) : base(owner)
        {
            var call__218 = TranslateRStr(SacredWrathRStr);
            FSpellName = call__218;
            FSpellID = 125;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtHoly);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 8;
            spellCombatInfo.Radius = 3;
        }
    }

    public partial class TFastCombatTerrorCA : TMultiTargetCombatSpellCA
    {
        // CombatSpells.pas:1666
        public new const int ClassID = 131777;

        public TFastCombatTerrorCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:1672
        public override void Execute(TCombat A1)
        {
            int I = default;
            TCombatUnit Target = default;
            base.Execute(A1);
            {
                I = FTargetIDs.Count - 1;
                byte end__220 = 0;
                for (; I >= end__220; I -= 1)
                {
                    var call__221 = FTargetIDs[I];
                    var call__222 = A1.FCombatData.FindID(call__221);
                    Target = (TCombatUnit)call__222;
                    var call__223 = AoWHSSet.FAbilityControl.GetAbility(108);
                    call__223.Expand(Target.FUnit);
                }
            }
        }

        // CombatSpells.pas:1686
        public void Generate(TCombatObject A1)
        {
            int I = default;
            TCombatSide Side = default;
            TCombatObject Target = default;
            FSpellID = 105;
            FSourceID = A1.FID;
            var combat = A1.FCombat;
            var call__225 = A1.GetOpponentSide();
            Side = combat.FSides[(int)call__225];

            {
                I = Side.GetCount() - 1;
                byte end__226 = 0;
                for (; I >= end__226; I -= 1)
                {
                    Target = Side.GetObjects(I);
                    bool bool__227 = !Target.FStatus.Contains(TCombatObjectStatusElement.csDestroyed);
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

                    bool bool__231 = bool__229 && Target is TCombatUnit;
                    if (bool__231)
                    {
                        var call__232 = Target.GetImmunityTypes();
                        bool__231 = !call__232.Contains(TDamageType.dtDeath);
                    }

                    bool bool__233 = bool__231;
                    if (bool__233)
                    {
                        var call__234 = ((TCombatUnit)Target).FUnit.GetUnitType();
                        bool__233 = call__234 != TUnitType.utMachine;
                    }

                    bool bool__235 = bool__233;
                    if (bool__235)
                    {
                        var call__236 = Target.GetResistance();
                        var call__237 = HitRole(6 - call__236);
                        bool__235 = call__237;
                    }

                    if (bool__235)
                    {
                        FTargetIDs.Add(Target.FID);
                    }
                }
            }
        }


    }

    // source ID, target ID, casting mana, success
    public partial class TTacticalCombatTerrorCA : TCombatAction
    {
        // CombatSpells.pas:1707
        public new const int ClassID = 131785;

        public TTacticalCombatTerrorCA(TECustomNode owner) : base(owner)
        { }

        public byte FSourceID;
        public byte FTargetID;
        public int FManaCost;
        public bool FSucceeded;
        // CombatSpells.pas:1713
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref FSourceID, 20);
            Stream.rwByte(ref FTargetID, 21);
            Stream.rwInteger(ref FManaCost, 22);
            Stream.rwBool(ref FSucceeded, 23);
        }

        // CombatSpells.pas:1723
        public override void Execute(TCombat A1)
        {
            TCombatUnit Target = default;
            TCombatUnit Source = default;
            if (FSucceeded)
            {
                var call__238 = A1.FCombatData.FindID(FTargetID);
                Target = (TCombatUnit)call__238;
                var call__239 = AoWHSSet.FAbilityControl.GetAbility(108);
                call__239.Expand(Target.FUnit);
            }

            if (FManaCost != 0 && A1.FStatus.Contains(TCombatStatusElement.cExecuting))
            {
                var call__240 = A1.FCombatData.FindID(FSourceID);
                Source = (TCombatUnit)call__240;
                var call__241 = AoWHSSet.FSpellControl.GetSpell(105);
                call__241.CastingDone(Source.FUnit, FManaCost);
            }
        }

        // CombatSpells.pas:1740
        public override bool Play(TCombatViewer A1)
        {
            bool Result = default;
            Execute(A1.FCombat);
            Result = false;
            return Result;
        }

        // CombatSpells.pas:1747
        public void Generate(TCombatObject A1, TCombatObject A2)
        {
            FSucceeded = false;
            FSourceID = A1.FID;
            FTargetID = A2.FID;
            var call__242 = A2.GetAbilityEnabled(108);
            bool bool__243 = !call__242;
            if (bool__243)
            {
                var call__244 = A2.GetAbilityEnabled(67);
                bool__243 = !call__244;
            }

            bool bool__245 = bool__243 && A2 is TCombatUnit;
            if (bool__245)
            {
                var call__246 = A2.GetImmunityTypes();
                bool__245 = !call__246.Contains(TDamageType.dtDeath);
            }

            bool bool__247 = bool__245;
            if (bool__247)
            {
                var call__248 = ((TCombatUnit)A2).FUnit.GetUnitType();
                bool__247 = call__248 != TUnitType.utMachine;
            }

            if (bool__247)
            {
                var call__249 = A2.GetResistance();
                var call__250 = HitRole(6 - call__249);
                FSucceeded = call__250;
            }
        }


    }

    // inherited combat statistics, damage mode
    public partial class TTerror : TCombatSpell
    {
        // CombatSpells.pas:1759
        public TTerror(TECustomNode owner) : base(owner)
        {
            var call__251 = TranslateRStr(TerrorRStr);
            FSpellName = call__251;
            FSpellID = 105;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtDeath);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = -1;
        }

        // CombatSpells.pas:1775
        public override TCombatAction CreateCA(TCombatObject A1, TCombatObject A2, bool A3)
        {
            TCombatAction Result = default;
            TTacticalCombatTerrorCA Action = default;
            Action = new TTacticalCombatTerrorCA(null);
            Action.Generate(A1, A2);
            if (A3)
            {
                var call__253 = ((THero)((TCombatUnit)A1).FUnit).CastingMana(this);
                Action.FManaCost = call__253;
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:1862
        public override void tcGetDamageValueEx(TCombatObject A1, TCombatObject A2, ref TAbilityDamageValues A3)
        {
            int Value = default;
            var call__254 = A2.GetAbilityEnabled(108);
            bool bool__255 = !call__254 && A2.FCombatValue != 0 && A2 is TCombatUnit;
            if (bool__255)
            {
                var call__256 = A2.GetImmunityTypes();
                bool__255 = !call__256.Contains(TDamageType.dtDeath);
            }

            if (bool__255)
            {
                var call__257 = A2.GetResistance();
                var call__258 = HitRoleProbability(6 - call__257);
                Value = Round((double)call__258 * 100);
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
                A3.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtDeath);
            }
            else
            {
                A3.SourceDamage = 0;
                A3.SourceCost = 0;
                A3.TargetDamage = 0;
                A3.TargetCost = 0;
                A3.DamageTypes = TSet<TDamageType>.Of();
            }
        }

        // CombatSpells.pas:1786
        public override void fcPrefetchCombatCommands(TFastCombatCommandList A1)
        {
            int I = default;
            TCombatSide Side = default;
            TCombatObject Target = default;
            int Value = default;
            TSpellCC Command = default;
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
                    bool bool__262 = !call__261 && Target.FCombatValue != 0 && Target is TCombatUnit;
                    if (bool__262)
                    {
                        var call__263 = Target.GetImmunityTypes();
                        bool__262 = !call__263.Contains(TDamageType.dtDeath);
                    }

                    if (bool__262)
                    {
                        Value += Target.GetTargetPriority();
                    }
                }
            }

            if (Value != 0)
            {
                Command = new TSpellCC(52);
                Command.FTarget = null;
                Command.FMoveCost = 1;
                Command.FSpellID = FSpellID;
                Command.FExpectedDV = Value;
                A1.Add(Command);
            }
        }

        // CombatSpells.pas:1815
        public override bool fcExecuteCombatCommand(TCombatObject A1, TFastCombatCommand A2)
        {
            bool Result = default;
            TFastCombatTerrorCA Action = default;
            Action = new TFastCombatTerrorCA(null);
            Action.Generate(A1);
            var call__264 = ((THero)((TCombatUnit)A1).FUnit).CastingMana(this);
            Action.FManaCost = call__264;
            A1.FCombat.ExecuteCombatAction(Action);
            Action.Release();
            Result = true;
            return Result;
        }

        // CombatSpells.pas:1828
        public override void fcGetDamageValueEx(TCombatObject A1, TCombatObject A2, TWallSituation A3, ref TAbilityDamageValues A4)
        {
            int I = default;
            int Value = default;
            TCombatSide Side = default;
            TCombatObject Target = default;
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
                    bool bool__268 = !call__267 && Target.FCombatValue != 0 && Target is TCombatUnit;
                    if (bool__268)
                    {
                        var call__269 = Target.GetImmunityTypes();
                        bool__268 = !call__269.Contains(TDamageType.dtDeath);
                    }

                    if (bool__268)
                    {
                        var call__270 = Target.GetResistance();
                        var call__271 = HitRoleProbability(6 - call__270);
                        Value += Round((double)call__271 * 100);
                    }
                }
            }

            if (Value != 0)
            {
                A4.SourceDamage = Value;
                A4.SourceCost = Value / 100;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                A4.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtDeath);
            }
            else
            {
                A4.SourceDamage = 0;
                A4.SourceCost = 0;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                A4.DamageTypes = TSet<TDamageType>.Of();
            }
        }
    }

    // inherited spell ID, combat statistics, damage mode
    public partial class TWindsOfFury : TCombatSpell
    {
        // CombatSpells.pas:1889
        public TWindsOfFury(TECustomNode owner) : base(owner)
        {
            var call__272 = TranslateRStr(WindsOfFuryRStr);
            FSpellName = call__272;
            FSpellID = 126;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtPhysical);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        // CombatSpells.pas:1904
        public override TCombatAction CreateCA(TCombatObject A1, TCombatObject A2, bool A3)
        {
            TCombatAction Result = default;
            TCombatSpellCA Action = default;
            Action = new TCombatSpellCA(null);
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
                Action.Generate(A1, A2, (sbyte)(FCombatInfo.Attack + 2), FCombatInfo.Damage, FCombatInfo.DamageTypes);
            }
            else
            {
                Action.Generate(A1, A2, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes);
            }

            if (A3)
            {
                var call__277 = ((THero)((TCombatUnit)A1).FUnit).CastingMana(this);
                Action.FManaCost = call__277;
            }

            Result = Action;
            return Result;
        }
    }

    // inherited spell ID, combat statistics, damage mode
    public partial class TShockwave : TCombatSpell
    {
        // CombatSpells.pas:1919
        public TShockwave(TECustomNode owner) : base(owner)
        {
            var call__278 = TranslateRStr(ShockwaveRStr);
            FSpellName = call__278;
            FSpellID = 127;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtPhysical, TDamageType.dtWall);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }
    }

    // inherited spell ID, combat statistics, damage mode
    public partial class TSacrificialFlame : TCombatSpell
    {
        // CombatSpells.pas:1934
        public TSacrificialFlame(TECustomNode owner) : base(owner)
        {
            var call__280 = TranslateRStr(SacrificialFlameRStr);
            FSpellName = call__280;
            FSpellID = 128;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtFire);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 3;
        }
    }

    // inherited source ID, target ID, mana, success, spell ID
    public partial class TRecallSpiritCA : TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1950
        public new const int ClassID = 131781;

        public TRecallSpiritCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:1953
        public override void Execute(TCombat A1)
        {
            TCombatUnit Source = default;
            TCombatUnit Target = default;
            TUnit NewUnit = default;
            base.Execute(A1);
            if (FSucceeded)
            {
                var call__282 = A1.FCombatData.FindID(FSourceID);
                Source = (TCombatUnit)call__282;
                var call__283 = A1.FCombatData.FindID(FTargetID);
                Target = (TCombatUnit)call__283;
                NewUnit = new TUnit(null);
                var call__284 = AoWHSSet.FUnitResources.GetUnitResource(130);
                NewUnit.SetUnitResource(call__284);
                NewUnit.SetPlayer(Source.FUnit.FPlayer);
                NewUnit.ExpandAbility(130);
                NewUnit.Activate();
                Target.SetUnit(NewUnit);
                NewUnit.Release();
                Target.SetStatus(Target.FStatus - TSet<TCombatObjectStatusElement>.Of(TCombatObjectStatusElement.csDestroyed));
                Target.SetCombatPlayer((byte)Source.FCombatPlayer);
            }
        }


    }

    // inherited spell ID, combat statistics, damage mode
    public partial class TRecallSpirit : TCombatSpell
    {
        // CombatSpells.pas:1975
        public TRecallSpirit(TECustomNode owner) : base(owner)
        {
            var call__285 = TranslateRStr(RecallSpiritRStr);
            FSpellName = call__285;
            FSpellID = 129;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 0;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of();
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = -1;
        }

        // CombatSpells.pas:2008
        public override TCombatAction CreateCA(TCombatObject A1, TCombatObject A2, bool A3)
        {
            TCombatAction Result = default;
            TRecallSpiritCA Action = default;
            Action = new TRecallSpiritCA(null);
            Action.FSourceID = A1.FID;
            Action.FTargetID = A2.FID;
            Action.FSucceeded = A2.FStatus.Contains(TCombatObjectStatusElement.csDestroyed);
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:1990
        public override void fcGetDamageValueEx(TCombatObject A1, TCombatObject A2, TWallSituation A3, ref TAbilityDamageValues A4)
        {
            if (A2.FStatus.Contains(TCombatObjectStatusElement.csDestroyed))
            {
                // { Original leaves the other damage-value fields unchanged on success. }
                A4.SourceDamage = 200;
                A4.DamageTypes = TSet<TDamageType>.Of();
            }
            else
            {
                A4.SourceDamage = 0;
                A4.SourceCost = 0;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                A4.DamageTypes = TSet<TDamageType>.Of();
            }
        }
    }

    // inherited source ID, target ID, mana, success, spell ID
    public partial class TAnimateDeadCA : TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:2021
        public new const int ClassID = 131782;

        public TAnimateDeadCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:2024
        public override void Execute(TCombat A1)
        {
            TCombatUnit Source = default;
            TCombatUnit Target = default;
            TUnit NewUnit = default;
            base.Execute(A1);
            if (FSucceeded)
            {
                var call__287 = A1.FCombatData.FindID(FSourceID);
                Source = (TCombatUnit)call__287;
                var call__288 = A1.FCombatData.FindID(FTargetID);
                Target = (TCombatUnit)call__288;
                NewUnit = new TUnit(null);
                var call__289 = AoWHSSet.FUnitResources.GetUnitResource(201);
                NewUnit.SetUnitResource(call__289);
                NewUnit.SetPlayer(Source.FUnit.FPlayer);
                NewUnit.ExpandAbility(130);
                NewUnit.Activate();
                Target.SetUnit(NewUnit);
                NewUnit.Release();
                Target.SetStatus(Target.FStatus - TSet<TCombatObjectStatusElement>.Of(TCombatObjectStatusElement.csDestroyed));
                Target.SetCombatPlayer((byte)Source.FCombatPlayer);
            }
        }


    }

    // inherited spell ID, combat statistics, damage mode
    public partial class TAnimateDead : TCombatSpell
    {
        // CombatSpells.pas:2046
        public TAnimateDead(TECustomNode owner) : base(owner)
        {
            var call__290 = TranslateRStr(AnimateDeadRStr);
            FSpellName = call__290;
            FSpellID = 130;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 0;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = TSet<TDamageType>.Of();
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = -1;
        }

        // CombatSpells.pas:2079
        public override TCombatAction CreateCA(TCombatObject A1, TCombatObject A2, bool A3)
        {
            TCombatAction Result = default;
            TAnimateDeadCA Action = default;
            Action = new TAnimateDeadCA(null);
            Action.FSourceID = A1.FID;
            Action.FTargetID = A2.FID;
            Action.FSucceeded = A2.FStatus.Contains(TCombatObjectStatusElement.csDestroyed);
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            Result = Action;
            return Result;
        }

        // CombatSpells.pas:2061
        public override void fcGetDamageValueEx(TCombatObject A1, TCombatObject A2, TWallSituation A3, ref TAbilityDamageValues A4)
        {
            if (A2.FStatus.Contains(TCombatObjectStatusElement.csDestroyed))
            {
                // { Original leaves the other damage-value fields unchanged on success. }
                A4.SourceDamage = 200;
                A4.DamageTypes = TSet<TDamageType>.Of();
            }
            else
            {
                A4.SourceDamage = 0;
                A4.SourceCost = 0;
                A4.TargetDamage = 0;
                A4.TargetCost = 0;
                A4.DamageTypes = TSet<TDamageType>.Of();
            }
        }
    }

    public static partial class CombatSpells
    {
        // CombatSpells.pas:2092
        public static void RegisterCombatSpells(TAoWEngine Engine)
        {
            TSpellControl Control = default;
            Control = Engine.FGameSet.FSpellControl;
            Engine.RegisterEClasses([EClass.Of<TCombatSpellCA>(), EClass.Of<TFastCombatTerrorCA>(), EClass.Of<TTacticalCombatTerrorCA>(), EClass.Of<TRecallSpiritCA>(), EClass.Of<TAnimateDeadCA>(), EClass.Of<TFastCombatHighPrayerCA>(), EClass.Of<TTacticalCombatHighPrayerCA>(), EClass.Of<TEntangleSpellCA>(), EClass.Of<TMindDecayCA>(), EClass.Of<TTurnUndeadSpellCA>(), EClass.Of<TSlowCA>()]);
            var call__292 = new TSolarFlare(null);
            Control.RegisterSpell(call__292);
            var call__293 = new TTurnUndead(null);
            Control.RegisterSpell(call__293);
            var call__294 = new THighPrayer(null);
            Control.RegisterSpell(call__294);
            var call__295 = new TDeathRay(null);
            Control.RegisterSpell(call__295);
            var call__296 = new TDiseaseCloud(null);
            Control.RegisterSpell(call__296);
            var call__297 = new TTerror(null);
            Control.RegisterSpell(call__297);
            var call__298 = new TMindDecay(null);
            Control.RegisterSpell(call__298);
            var call__299 = new TChainLightning(null);
            Control.RegisterSpell(call__299);
            var call__300 = new TColdBreath(null);
            Control.RegisterSpell(call__300);
            var call__301 = new TSlow(null);
            Control.RegisterSpell(call__301);
            var call__302 = new TEntangle(null);
            Control.RegisterSpell(call__302);
            var call__303 = new TTremors(null);
            Control.RegisterSpell(call__303);
            var call__304 = new TCallFlames(null);
            Control.RegisterSpell(call__304);
            var call__305 = new TFireBreath(null);
            Control.RegisterSpell(call__305);
            var call__306 = new TSwarm(null);
            Control.RegisterSpell(call__306);
            var call__307 = new TGreatHail(null);
            Control.RegisterSpell(call__307);
            var call__308 = new TFireBall(null);
            Control.RegisterSpell(call__308);
            var call__309 = new TGeyser(null);
            Control.RegisterSpell(call__309);
            var call__310 = new TIceShards(null);
            Control.RegisterSpell(call__310);
            var call__311 = new TFrostBeams(null);
            Control.RegisterSpell(call__311);
            var call__312 = new TVaporize(null);
            Control.RegisterSpell(call__312);
            var call__313 = new TStoning(null);
            Control.RegisterSpell(call__313);
            var call__314 = new TFlamingArrow(null);
            Control.RegisterSpell(call__314);
            var call__315 = new TOoze(null);
            Control.RegisterSpell(call__315);
            var call__316 = new TSacredWrath(null);
            Control.RegisterSpell(call__316);
            var call__317 = new TAnimateDead(null);
            Control.RegisterSpell(call__317);
            var call__318 = new TRecallSpirit(null);
            Control.RegisterSpell(call__318);
            var call__319 = new TWindsOfFury(null);
            Control.RegisterSpell(call__319);
            var call__320 = new TShockwave(null);
            Control.RegisterSpell(call__320);
            var call__321 = new TSacrificialFlame(null);
            Control.RegisterSpell(call__321);
        }

        // CombatSpells.pas:2130
        public static void UnregisterCombatSpells(TAoWEngine Engine)
        {
            TSpellControl Control = default;
            Control = Engine.FGameSet.FSpellControl;
            Engine.UnregisterEClasses([EClass.Of<TCombatSpellCA>(), EClass.Of<TFastCombatTerrorCA>(), EClass.Of<TTacticalCombatTerrorCA>(), EClass.Of<TRecallSpiritCA>(), EClass.Of<TAnimateDeadCA>(), EClass.Of<TFastCombatHighPrayerCA>(), EClass.Of<TTacticalCombatHighPrayerCA>(), EClass.Of<TEntangleSpellCA>(), EClass.Of<TMindDecayCA>(), EClass.Of<TTurnUndeadSpellCA>(), EClass.Of<TSlowCA>()]);
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
