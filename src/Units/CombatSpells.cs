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
            TCombatObject Source;
            base.Execute(A1);
            if (FManaCost != 0 && A1.FStatus.Contains(TCombatStatusElement.cExecuting))
            {
                Source = A1.FCombatData.FindID(FSourceID);
                AoWHSSet.FSpellControl.GetSpell(FSpellID).CombatCastingDone(Source, FManaCost);
            }
        }

        // CombatSpells.pas:471
        public override bool Play(TCombatViewer A1)
        {
            if (A1.FTime == 0)
            {
                AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }

            if (A1.FDelay == A1.FTime)
            {
                Execute(A1.FCombat);
            }

            return A1.FDelay * 2 > A1.FTime;
        }

        // CombatSpells.pas:479
        public override void ShowObject(TCombatViewer A1, TCombatObject A2, int A3, int A4, int A5, int A6, in TSurfaceDesc A7)
        {
            base.ShowObject(A1, A2, A3, A4, A5, A6, A7);
            if (A2.FID == FSourceID)
            {
                var spell = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(A5 + 32, A6 + 32 + 8, spell.FSpellName, A7);
            }
            else
            {
                if (A2.FID == FTargetID)
                {
                    AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
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
                var spell = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(A5 + 32, A6 + 32 + 8, spell.FSpellName, A7);
            }
            else
            {
                if (A2.FID == FTargetID)
                {
                    AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
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
            TCombatObject Source;
            if (FManaCost != 0 && A1.FStatus.Contains(TCombatStatusElement.cExecuting))
            {
                Source = A1.FCombatData.FindID(FSourceID);
                AoWHSSet.FSpellControl.GetSpell(FSpellID).CombatCastingDone(Source, FManaCost);
            }
        }

        // CombatSpells.pas:524
        public override bool Play(TCombatViewer A1)
        {
            if (A1.FTime == 0)
            {
                AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }

            if (A1.FDelay == A1.FTime)
            {
                Execute(A1.FCombat);
            }

            return A1.FDelay * 3 > A1.FTime;
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
                var spell = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(A5 + 32, A6 + 32 + 8, spell.FSpellName, A7);
            }
            else
            {
                if (A2.FID == FTargetID)
                {
                    AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
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
                var spell = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(A5 + 32, A6 + 32 + 8, spell.FSpellName, A7);
            }
            else
            {
                if (A2.FID == FTargetID)
                {
                    AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
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
            FTargetIDs = new TByteList();
        }

        // CombatSpells.pas:579
        public override void Execute(TCombat A1)
        {
            TCombatObject Source;
            if (FManaCost != 0 && A1.FStatus.Contains(TCombatStatusElement.cExecuting))
            {
                Source = A1.FCombatData.FindID(FSourceID);
                AoWHSSet.FSpellControl.GetSpell(FSpellID).CombatCastingDone(Source, FManaCost);
            }
        }

        // CombatSpells.pas:591
        public override bool Play(TCombatViewer A1)
        {
            if (A1.FTime == 0)
            {
                AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }

            if (A1.FDelay == A1.FTime)
            {
                Execute(A1.FCombat);
            }

            return A1.FDelay * 5 > A1.FTime;
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
                var spell = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(A5 + 32, A6 + 32 + 8, spell.FSpellName, A7);
            }
            else
            {
                if (FTargetIDs.IndexOf(A2.FID) != -1)
                {
                    AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
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
                var spell = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(A5 + 32, A6 + 32 + 8, spell.FSpellName, A7);
            }
            else
            {
                if (FTargetIDs.IndexOf(A2.FID) != -1)
                {
                    AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, A5 + 32, A6 + 32, A1.FTime, A7);
                }
            }
        }

        // CombatSpells.pas:624
        public override void ShowFightingObjects(TCombatViewer A1, int A2, int A3, int A4, in TSurfaceDesc A5)
        {
            TCombatObject Source;
            Source = A1.FCombat.FCombatData.FindID(FSourceID);
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
            TCombatSpellCA Action;
            Action = new TCombatSpellCA(null);
            Action.FSpellID = FSpellID;
            Action.GenerateEx(A1, A2, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, FDamageMode);
            if (A3)
            {
                Action.FManaCost = ((THero)((TCombatUnit)A1).FUnit).CastingMana(this);
            }

            return Action;
        }

        // CombatSpells.pas:643
        public override void GetCombatInfo(TAbstractUnit Source, ref TSpellCombatInfo Info)
        {
            Info = FCombatInfo;
        }

        // CombatSpells.pas:648
        public override int GetCombatDamageValue(TCombatObject A1, TCombatObject A2)
        {
            var hitPoints = A2.GetHitPoints();
            var defense = A2.GetDefense();
            var resistance = A2.GetResistance();
            var immunityTypes = A2.GetImmunityTypes();
            var protectionTypes = A2.GetProtectionTypes();
            return StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, hitPoints, defense, resistance, immunityTypes, protectionTypes) * FCombatInfo.RepeatCount;
        }

        // CombatSpells.pas:668
        public override void GetCombatDamageValueEx(TCombatObject A1, TCombatObject A2, ref TAbilityDamageValues A3)
        {
            TSet<TDamageType> Protection;
            TSet<TDamageType> Immunity;
            Protection = A2.GetProtectionTypes();
            Immunity = A2.GetImmunityTypes();
            A3.DamageTypes = FCombatInfo.DamageTypes;
            var hitPoints = A2.GetHitPoints();
            var defense = A2.GetDefense();
            var resistance = A2.GetResistance();
            var statisticsToLimitedDV = StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, hitPoints, defense, resistance, Immunity, Protection);
            A3.SourceDamage = statisticsToLimitedDV * FCombatInfo.RepeatCount;
            // { Original recomputes the value, using resistance in place of defense for mode 1. }
            if (FDamageMode == 1)
            {
                var a2HitPoints = A2.GetHitPoints();
                var a2Resistance = A2.GetResistance();
                var resistance2 = A2.GetResistance();
                var statisticsToLimitedDV2 = StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, a2HitPoints, a2Resistance, resistance2, Immunity, Protection);
                A3.SourceDamage = statisticsToLimitedDV2 * FCombatInfo.RepeatCount;
            }
            else
            {
                var a2HitPoints = A2.GetHitPoints();
                var a2Defense = A2.GetDefense();
                var a2Resistance = A2.GetResistance();
                var statisticsToLimitedDV2 = StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, a2HitPoints, a2Defense, a2Resistance, Immunity, Protection);
                A3.SourceDamage = statisticsToLimitedDV2 * FCombatInfo.RepeatCount;
            }

            var statisticsToMaxDamage = StatisticsToMaxDamage(FCombatInfo.Damage, FCombatInfo.DamageTypes, Protection, Immunity);
            A3.SourceCost = statisticsToMaxDamage * FCombatInfo.RepeatCount;
            A3.TargetDamage = 0;
            A3.TargetCost = 0;
        }

        // CombatSpells.pas:754
        public override void fcPrefetchCombatCommands(TFastCombatCommandList A1)
        {
            TAbstractUnit Hero;
            int Distance;
            AnsiString Reason = default;
            TCombatObject Target;
            TSpellCC Command;
            TAbilityDamageValues Values = default;
            Hero = ((TCombatUnit)A1.FSource).FUnit;
            if (Hero is THero && ((THero)Hero).CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason))
            {
                Distance = A1.FCombat.GetRoundDistance();
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

                for (int I = 0, last = A1.FTargetSide.GetCount() - 1; I <= last; I++)
                {
                    Target = A1.FTargetSide.GetObjects(I);
                    if (Target.GetEnabled() && Target.fcRoundDistance() <= Distance)
                    {
                        var wallSituation = ((TFastCombat)A1.FSource.FCombat).GetWallSituation(A1.FSource, Target);
                        fcGetDamageValueEx(A1.FSource, Target, wallSituation, ref Values);
                        if (Values.SourceDamage != 0)
                        {
                            Command = new TSpellCC(52);
                            Command.FTarget = A1.FTargetSide.GetObjects(I);
                            Command.FMoveCost = 1;
                            Command.FSpellID = FSpellID;
                            Command.FExpectedDV = DVtoDEV(Values.SourceDamage, Values.TargetDamage, A1.FSource, Target);
                            A1.Add(Command);
                        }
                    }
                }
            }
        }

        // CombatSpells.pas:791
        public override bool fcExecuteCombatCommand(TCombatObject A1, TFastCombatCommand A2)
        {
            int Count;
            TCombatAction Action;
            TCombatObject Other;
            int Distance;
            TCombatObject Target;
            TList Targets;
            TFastCombat Combat;
            Target = A2.FTarget;
            for (int I = 0, last = FCombatInfo.RepeatCount - 1; I <= last; I++)
            {
                if (!Target.FStatus.Contains(TCombatObjectStatusElement.csDestroyed))
                {
                    Action = CreateCA(A1, Target, I == 0);
                    A1.FCombat.ExecuteCombatAction(Action);
                    Action.Release();
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

                for (int I = Combat.FSides[(int)Target.GetSide()].GetCount() - 1; I >= 0; I--)
                {
                    var side = Target.GetSide();
                    Other = Combat.FSides[(int)side].GetObjects(I);
                    bool condition = Other != Target && Other.GetEnabled() && (int)(((uint)Other.FPosition) >> 4) == (int)(((uint)Target.FPosition) >> 4) && Distance >= Other.fcRoundDistance();
                    if (condition)
                    {
                        var immunityTypes = Other.GetImmunityTypes();
                        condition = FCombatInfo.DamageTypes - immunityTypes != TSet<TDamageType>.Of();
                    }

                    if (condition)
                    {
                        Targets.Add(Other);
                    }
                }

                // { Original shifts Random(4) by two, so this adds no random extra targets. }
                Count = (int)(((uint)AoWHSMap.Random(4)) >> 2) + FCombatInfo.Radius;
                while (Count != 0 && Targets.Count != 0)
                {
                    Other = (TCombatObject)Targets[AoWHSMap.Random(Targets.Count)];
                    for (int J = 0, last = FCombatInfo.RepeatCount - 1; J <= last; J++)
                    {
                        if (!Other.FStatus.Contains(TCombatObjectStatusElement.csDestroyed))
                        {
                            Action = CreateCA(A1, Other, false);
                            A1.FCombat.ExecuteCombatAction(Action);
                            Action.Release();
                        }
                    }

                    Targets.Remove(Other);
                    Count -= 1;
                }

                Targets.Free();
            }

            return true;
        }

        // CombatSpells.pas:693
        public override void fcGetDamageValueEx(TCombatObject A1, TCombatObject A2, TWallSituation A3, ref TAbilityDamageValues A4)
        {
            TSet<TDamageType> DamageTypes;
            TSet<TDamageType> Protection;
            TSet<TDamageType> Immunity;
            int Count;
            int Distance;
            TCombatObject Other;
            TFastCombat Combat;
            int Damage;
            int Defense;
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
                var hitPoints = A2.GetHitPoints();
                var resistance = A2.GetResistance();
                var statisticsToLimitedDV = StatisticsToLimitedDV(FCombatInfo.Attack, Damage, DamageTypes, hitPoints, Defense, resistance, Immunity, Protection);
                A4.SourceDamage = statisticsToLimitedDV * FCombatInfo.RepeatCount;
                if (FCombatInfo.Radius > 1)
                {
                    A4.SourceDamage = (int)(((uint)(A4.SourceDamage * FCombatInfo.Radius)) >> 1);
                }

                var statisticsToMaxDamage = StatisticsToMaxDamage(Damage, DamageTypes, Protection, Immunity);
                A4.SourceCost = statisticsToMaxDamage * FCombatInfo.RepeatCount;
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

                    for (int I = Combat.FSides[(int)A2.GetSide()].GetCount() - 1; I >= 0; I--)
                    {
                        var side = A2.GetSide();
                        Other = Combat.FSides[(int)side].GetObjects(I);
                        if (A2 != Other && Other.GetEnabled() && (int)(((uint)Other.FPosition) >> 4) == (int)(((uint)A2.FPosition) >> 4) && Other.fcRoundDistance() <= Distance && DamageTypes - Other.GetImmunityTypes() != TSet<TDamageType>.Of())
                        {
                            Count += 1;
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
            FSpellName = TranslateRStr(SolarFlareRStr);
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
            TCombatObject Source;
            TCombatUnit Target;
            TUnitEnchantmentAbilityData Data;
            base.Execute(A1);
            if (FDamage != 0)
            {
                Source = A1.FCombatData.FindID(FSourceID);
                Target = (TCombatUnit)A1.FCombatData.FindID(FTargetID);
                if (!Target.FStatus.Contains(TCombatObjectStatusElement.csDestroyed))
                {
                    AoWHSSet.FAbilityControl.GetAbility(34).Expand(Target.FUnit);
                    Data = (TUnitEnchantmentAbilityData)Target.FUnit.GetAbilityData(34);
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
            FSpellName = TranslateRStr(TurnUndeadRStr);
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
            TTurnUndeadSpellCA Action;
            Action = new TTurnUndeadSpellCA(null);
            Action.FSpellID = FSpellID;
            Action.FSourceID = A1.FID;
            Action.FTargetID = A2.FID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            bool condition = A2 is TCombatUnit;
            if (condition)
            {
                var unitType = ((TCombatUnit)A2).FUnit.GetUnitType();
                condition = TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature).Contains(unitType);
            }

            if (condition && (((TCombatUnit)A2).FUnit.GetRace() == 11 || ((TCombatUnit)A2).FUnit.GetAbilityEnabled(129)))
            {
                Action.GenerateEx(A1, A2, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, FDamageMode);
            }

            return Action;
        }

        // CombatSpells.pas:922
        public override void fcGetDamageValueEx(TCombatObject A1, TCombatObject A2, TWallSituation A3, ref TAbilityDamageValues A4)
        {
            bool condition = A2 is TCombatUnit;
            if (condition)
            {
                var unitType = ((TCombatUnit)A2).FUnit.GetUnitType();
                condition = TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature).Contains(unitType);
            }

            if (condition && (((TCombatUnit)A2).FUnit.GetRace() == 11 || ((TCombatUnit)A2).FUnit.GetAbilityEnabled(129)))
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
            TCombatObject Target;
            base.Execute(A1);
            for (int I = FTargetIDs.Count - 1; I >= 0; I--)
            {
                var targetID = FTargetIDs[I];
                Target = A1.FCombatData.FindID(targetID);
                Target.SetHitPoints((byte)(Target.GetHitPoints() + 5));
                AoWHSSet.FAbilityControl.GetAbility(100).Expand(((TCombatUnit)Target).FUnit);
            }
        }

        // CombatSpells.pas:958
        public void Generate(TCombatObject A1)
        {
            TCombatSide Side;
            TCombatObject Target;
            FSpellID = 102;
            FSourceID = A1.FID;
            var combat = A1.FCombat;
            var side = A1.GetSide();
            Side = combat.FSides[(int)side];

            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Target = Side.GetObjects(I);
                if (Target is TCombatUnit && !Target.FStatus.Contains(TCombatObjectStatusElement.csDestroyed))
                {
                    FTargetIDs.Add(Target.FID);
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
            TCombatObject Target;
            base.Execute(A1);
            if (FSucceeded)
            {
                Target = A1.FCombatData.FindID(FTargetID);
                Target.SetHitPoints((byte)(Target.GetHitPoints() + 5));
                AoWHSSet.FAbilityControl.GetAbility(100).Expand(((TCombatUnit)Target).FUnit);
            }
        }


    }

    // inherited combat statistics, damage mode
    public partial class THighPrayer : TCombatSpell
    {
        // CombatSpells.pas:996
        public THighPrayer(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(HighPrayerRStr);
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
            TTacticalCombatHighPrayerCA Action;
            Action = new TTacticalCombatHighPrayerCA(null);
            Action.FSucceeded = true;
            Action.FSourceID = A1.FID;
            Action.FTargetID = A2.FID;
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            return Action;
        }

        // CombatSpells.pas:1104
        public override void tcGetDamageValueEx(TCombatObject A1, TCombatObject A2, ref TAbilityDamageValues A3)
        {
            int Healing;
            int Value;
            Healing = A2.GetHits() - A2.GetHitPoints();
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
            TCombatSide Side;
            TCombatObject Target;
            int Value;
            TSpellCC Command;
            Value = 0;
            var side = A1.FSource.GetSide();
            Side = A1.FCombat.FSides[(int)side];
            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Target = Side.GetObjects(I);
                if (Target.FCombatValue != 0 && Target is TCombatUnit)
                {
                    Value += DVtoDEV((Target.GetHits() - Target.GetHitPoints()) * 100, 0, A1.FSource, Target);
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
            TFastCombatHighPrayerCA Action;
            Action = new TFastCombatHighPrayerCA(null);
            Action.Generate(A1);
            Action.FManaCost = ((THero)((TCombatUnit)A1).FUnit).CastingMana(this);
            A1.FCombat.ExecuteCombatAction(Action);
            Action.Release();
            return true;
        }

        // CombatSpells.pas:1067
        public override void fcGetDamageValueEx(TCombatObject A1, TCombatObject A2, TWallSituation A3, ref TAbilityDamageValues A4)
        {
            int Value;
            int Healing;
            TCombatSide Side;
            TCombatObject Target;
            Value = 0;
            var side = A1.GetSide();
            Side = A1.FCombat.FSides[(int)side];
            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Target = Side.GetObjects(I);
                if (Target.FCombatValue != 0 && Target is TCombatUnit)
                {
                    Healing = Target.GetHits() - Target.GetHitPoints();
                    if (Healing > 5)
                    {
                        Healing = 5;
                    }

                    Value += Healing * 100;
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
            FSpellName = TranslateRStr(DeathRayRStr);
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
            FSpellName = TranslateRStr(DiseaseCloudRStr);
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
            TCombatObject Source;
            TCombatUnit Target;
            base.Execute(A1);
            if (FSucceeded)
            {
                Source = A1.FCombatData.FindID(FSourceID);
                Target = (TCombatUnit)A1.FCombatData.FindID(FTargetID);
                Source.SetExperience(Source.GetExperience() + Target.GetLevel());
                for (int I = 0; I <= 2; I++)
                {
                    ((TCommandAbility)AoWHSSet.FAbilityControl.GetAbility(CommandAbilityIDs[I])).ResetCommandedUnits(Target);
                }

                for (int I = 0; I <= 2; I++)
                {
                    ((TCommandAbility)AoWHSSet.FAbilityControl.GetAbility(CommandAbilityIDs[I])).Uncommand(Target);
                }

                AoWHSSet.FAbilityControl.GetAbility(131).Expand(Target.FUnit);
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
            FSpellName = TranslateRStr(MindDecayRStr);
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
            TMindDecayCA Action;
            Action = new TMindDecayCA(null);
            Action.FSourceID = A1.FID;
            Action.FTargetID = A2.FID;
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            if (A2 is TCombatUnit && ((TCombatUnit)A2).FUnit.GetUnitType() != TUnitType.utMachine && ((TCombatUnit)A2).FUnit.GetRace() != 11 && !((TCombatUnit)A2).FUnit.GetAbilityEnabled(129) && !(((TCombatUnit)A2).FUnit is TLeader))
            {
                var resistance = A2.GetResistance();
                Action.FSucceeded = HitRole(FCombatInfo.Attack - resistance);
            }

            return Action;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TChainLightning : TCombatSpell
    {
        // CombatSpells.pas:1224
        public TChainLightning(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(ChainLightningRStr);
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
            FSpellName = TranslateRStr(ColdBreathRStr);
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
            TCombatUnit Target;
            TCombatObject Source;
            TUnitEnchantment Enchantment;
            base.Execute(A1);
            if (FSucceeded)
            {
                Target = (TCombatUnit)A1.FCombatData.FindID(FTargetID);
                Source = A1.FCombatData.FindID(FSourceID);
                if (Target.FUnit.ExpandAbility(132))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.FUnit.GetAbilityData(132)).FEnchantment;
                    Enchantment.SetUpkeep(0);
                    Enchantment.SetSource(Source.GetPlayer());
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
            FSpellName = TranslateRStr(SlowRStr);
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
            TSlowCA Action;
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
                var resistance = A2.GetResistance();
                Action.FSucceeded = HitRole(FCombatInfo.Attack - resistance);
            }

            return Action;
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
            TCombatObject Source;
            TCombatUnit Target;
            TUnitEnchantmentAbilityData Data;
            base.Execute(A1);
            if (FSucceeded)
            {
                Source = A1.FCombatData.FindID(FSourceID);
                Target = (TCombatUnit)A1.FCombatData.FindID(FTargetID);
                AoWHSSet.FAbilityControl.GetAbility(94).Expand(Target.FUnit);
                Data = (TUnitEnchantmentAbilityData)Target.FUnit.GetAbilityData(94);
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
            FSpellName = TranslateRStr(EntangleRStr);
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
            TEntangleSpellCA Action;
            Action = new TEntangleSpellCA(null);
            Action.FSourceID = A1.FID;
            Action.FTargetID = A2.FID;
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            if (!A2.GetAbilityEnabled(1) && !A2.GetAbilityEnabled(94))
            {
                var resistance = A2.GetResistance();
                Action.FSucceeded = HitRole(FCombatInfo.Attack - resistance);
            }

            return Action;
        }

        // CombatSpells.pas:1383
        public override void GetCombatDamageValueEx(TCombatObject A1, TCombatObject A2, ref TAbilityDamageValues A3)
        {
            if (!A2.GetAbilityEnabled(1) && !A2.GetAbilityEnabled(94))
            {
                var resistance = A2.GetResistance();
                var hitRoleProbability = HitRoleProbability(FCombatInfo.Attack - resistance);
                var hitPoints = A2.GetHitPoints();
                A3.SourceDamage = Round((double)(hitPoints * 100) * hitRoleProbability);
                var a2HitPoints = A2.GetHitPoints();
                A3.SourceCost = a2HitPoints;
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
            FSpellName = TranslateRStr(CallFlamesRStr);
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
            FSpellName = TranslateRStr(FireBreathRStr);
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
            FSpellName = TranslateRStr(SwarmRStr);
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
            FSpellName = TranslateRStr(GreatHailRStr);
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
            FSpellName = TranslateRStr(FireBallRStr);
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
            FSpellName = TranslateRStr(GeyserRStr);
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
            TCombatSpellCA Action;
            Action = new TCombatSpellCA(null);
            Action.FSpellID = FSpellID;
            if (A2.GetAbilityEnabled(1) || A2.GetAbilityEnabled(59))
            {
                Action.Generate(A1, A2, FCombatInfo.Attack, (sbyte)(FCombatInfo.Damage / 2), FCombatInfo.DamageTypes);
            }
            else
            {
                Action.Generate(A1, A2, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes);
            }

            if (A3)
            {
                Action.FManaCost = ((THero)((TCombatUnit)A1).FUnit).CastingMana(this);
            }

            return Action;
        }
    }

    // inherited combat statistics, damage mode
    public partial class TIceShards : TCombatSpell
    {
        // CombatSpells.pas:1537
        public TIceShards(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(IceShardsRStr);
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
            FSpellName = TranslateRStr(FrostBeamsRStr);
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
            FSpellName = TranslateRStr(VaporizeRStr);
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
            FSpellName = TranslateRStr(FlamingArrowRStr);
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
            FSpellName = TranslateRStr(StoningRStr);
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
            FSpellName = TranslateRStr(OozeRStr);
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
            A2.GetProtectionTypes();
            A2.GetImmunityTypes();
            A3.DamageTypes = FCombatInfo.DamageTypes;
            var resistance = A2.GetResistance();
            var hitRoleProbability = HitRoleProbability(FCombatInfo.Attack - resistance);
            A3.SourceDamage = Round((double)hitRoleProbability * 200);
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
            FSpellName = TranslateRStr(SacredWrathRStr);
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
            TCombatUnit Target;
            base.Execute(A1);
            for (int I = FTargetIDs.Count - 1; I >= 0; I--)
            {
                var targetID = FTargetIDs[I];
                Target = (TCombatUnit)A1.FCombatData.FindID(targetID);
                AoWHSSet.FAbilityControl.GetAbility(108).Expand(Target.FUnit);
            }
        }

        // CombatSpells.pas:1686
        public void Generate(TCombatObject A1)
        {
            TCombatSide Side;
            TCombatObject Target;
            FSpellID = 105;
            FSourceID = A1.FID;
            var combat = A1.FCombat;
            var opponentSide = A1.GetOpponentSide();
            Side = combat.FSides[(int)opponentSide];

            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Target = Side.GetObjects(I);
                if (!Target.FStatus.Contains(TCombatObjectStatusElement.csDestroyed) && !Target.GetAbilityEnabled(108) && !Target.GetAbilityEnabled(67) && Target is TCombatUnit && !Target.GetImmunityTypes().Contains(TDamageType.dtDeath) && ((TCombatUnit)Target).FUnit.GetUnitType() != TUnitType.utMachine && HitRole(6 - Target.GetResistance()))
                {
                    FTargetIDs.Add(Target.FID);
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
            TCombatUnit Target;
            TCombatUnit Source;
            if (FSucceeded)
            {
                Target = (TCombatUnit)A1.FCombatData.FindID(FTargetID);
                AoWHSSet.FAbilityControl.GetAbility(108).Expand(Target.FUnit);
            }

            if (FManaCost != 0 && A1.FStatus.Contains(TCombatStatusElement.cExecuting))
            {
                Source = (TCombatUnit)A1.FCombatData.FindID(FSourceID);
                AoWHSSet.FSpellControl.GetSpell(105).CastingDone(Source.FUnit, FManaCost);
            }
        }

        // CombatSpells.pas:1740
        public override bool Play(TCombatViewer A1)
        {
            Execute(A1.FCombat);
            return false;
        }

        // CombatSpells.pas:1747
        public void Generate(TCombatObject A1, TCombatObject A2)
        {
            FSucceeded = false;
            FSourceID = A1.FID;
            FTargetID = A2.FID;
            if (!A2.GetAbilityEnabled(108) && !A2.GetAbilityEnabled(67) && A2 is TCombatUnit && !A2.GetImmunityTypes().Contains(TDamageType.dtDeath) && ((TCombatUnit)A2).FUnit.GetUnitType() != TUnitType.utMachine)
            {
                FSucceeded = HitRole(6 - A2.GetResistance());
            }
        }


    }

    // inherited combat statistics, damage mode
    public partial class TTerror : TCombatSpell
    {
        // CombatSpells.pas:1759
        public TTerror(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(TerrorRStr);
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
            TTacticalCombatTerrorCA Action;
            Action = new TTacticalCombatTerrorCA(null);
            Action.Generate(A1, A2);
            if (A3)
            {
                Action.FManaCost = ((THero)((TCombatUnit)A1).FUnit).CastingMana(this);
            }

            return Action;
        }

        // CombatSpells.pas:1862
        public override void tcGetDamageValueEx(TCombatObject A1, TCombatObject A2, ref TAbilityDamageValues A3)
        {
            int Value;
            if (!A2.GetAbilityEnabled(108) && A2.FCombatValue != 0 && A2 is TCombatUnit && !A2.GetImmunityTypes().Contains(TDamageType.dtDeath))
            {
                Value = Round((double)HitRoleProbability(6 - A2.GetResistance()) * 100);
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
            TCombatSide Side;
            TCombatObject Target;
            int Value;
            TSpellCC Command;
            Value = 0;
            var opponentSide = A1.FSource.GetOpponentSide();
            Side = A1.FCombat.FSides[(int)opponentSide];
            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Target = Side.GetObjects(I);
                if (!Target.GetAbilityEnabled(108) && Target.FCombatValue != 0 && Target is TCombatUnit && !Target.GetImmunityTypes().Contains(TDamageType.dtDeath))
                {
                    Value += Target.GetTargetPriority();
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
            TFastCombatTerrorCA Action;
            Action = new TFastCombatTerrorCA(null);
            Action.Generate(A1);
            Action.FManaCost = ((THero)((TCombatUnit)A1).FUnit).CastingMana(this);
            A1.FCombat.ExecuteCombatAction(Action);
            Action.Release();
            return true;
        }

        // CombatSpells.pas:1828
        public override void fcGetDamageValueEx(TCombatObject A1, TCombatObject A2, TWallSituation A3, ref TAbilityDamageValues A4)
        {
            int Value;
            TCombatSide Side;
            TCombatObject Target;
            Value = 0;
            var opponentSide = A1.GetOpponentSide();
            Side = A1.FCombat.FSides[(int)opponentSide];
            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Target = Side.GetObjects(I);
                if (!Target.GetAbilityEnabled(108) && Target.FCombatValue != 0 && Target is TCombatUnit && !Target.GetImmunityTypes().Contains(TDamageType.dtDeath))
                {
                    Value += Round((double)HitRoleProbability(6 - Target.GetResistance()) * 100);
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
            FSpellName = TranslateRStr(WindsOfFuryRStr);
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
            TCombatSpellCA Action;
            Action = new TCombatSpellCA(null);
            Action.FSpellID = FSpellID;
            if (A2.GetAbilityEnabled(1) || A2.GetAbilityEnabled(59))
            {
                Action.Generate(A1, A2, (sbyte)(FCombatInfo.Attack + 2), FCombatInfo.Damage, FCombatInfo.DamageTypes);
            }
            else
            {
                Action.Generate(A1, A2, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes);
            }

            if (A3)
            {
                Action.FManaCost = ((THero)((TCombatUnit)A1).FUnit).CastingMana(this);
            }

            return Action;
        }
    }

    // inherited spell ID, combat statistics, damage mode
    public partial class TShockwave : TCombatSpell
    {
        // CombatSpells.pas:1919
        public TShockwave(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(ShockwaveRStr);
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
            FSpellName = TranslateRStr(SacrificialFlameRStr);
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
            TCombatUnit Source;
            TCombatUnit Target;
            TUnit NewUnit;
            base.Execute(A1);
            if (FSucceeded)
            {
                Source = (TCombatUnit)A1.FCombatData.FindID(FSourceID);
                Target = (TCombatUnit)A1.FCombatData.FindID(FTargetID);
                NewUnit = new TUnit(null);
                NewUnit.SetUnitResource(AoWHSSet.FUnitResources.GetUnitResource(130));
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
            FSpellName = TranslateRStr(RecallSpiritRStr);
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
            TRecallSpiritCA Action;
            Action = new TRecallSpiritCA(null);
            Action.FSourceID = A1.FID;
            Action.FTargetID = A2.FID;
            Action.FSucceeded = A2.FStatus.Contains(TCombatObjectStatusElement.csDestroyed);
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            return Action;
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
            TCombatUnit Source;
            TCombatUnit Target;
            TUnit NewUnit;
            base.Execute(A1);
            if (FSucceeded)
            {
                Source = (TCombatUnit)A1.FCombatData.FindID(FSourceID);
                Target = (TCombatUnit)A1.FCombatData.FindID(FTargetID);
                NewUnit = new TUnit(null);
                NewUnit.SetUnitResource(AoWHSSet.FUnitResources.GetUnitResource(201));
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
            FSpellName = TranslateRStr(AnimateDeadRStr);
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
            TAnimateDeadCA Action;
            Action = new TAnimateDeadCA(null);
            Action.FSourceID = A1.FID;
            Action.FTargetID = A2.FID;
            Action.FSucceeded = A2.FStatus.Contains(TCombatObjectStatusElement.csDestroyed);
            Action.FSpellID = FSpellID;
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            return Action;
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
            TSpellControl Control;
            Control = Engine.FGameSet.FSpellControl;
            Engine.RegisterEClasses([EClass.Of<TCombatSpellCA>(), EClass.Of<TFastCombatTerrorCA>(), EClass.Of<TTacticalCombatTerrorCA>(), EClass.Of<TRecallSpiritCA>(), EClass.Of<TAnimateDeadCA>(), EClass.Of<TFastCombatHighPrayerCA>(), EClass.Of<TTacticalCombatHighPrayerCA>(), EClass.Of<TEntangleSpellCA>(), EClass.Of<TMindDecayCA>(), EClass.Of<TTurnUndeadSpellCA>(), EClass.Of<TSlowCA>()]);
            Control.RegisterSpell(new TSolarFlare(null));
            Control.RegisterSpell(new TTurnUndead(null));
            Control.RegisterSpell(new THighPrayer(null));
            Control.RegisterSpell(new TDeathRay(null));
            Control.RegisterSpell(new TDiseaseCloud(null));
            Control.RegisterSpell(new TTerror(null));
            Control.RegisterSpell(new TMindDecay(null));
            Control.RegisterSpell(new TChainLightning(null));
            Control.RegisterSpell(new TColdBreath(null));
            Control.RegisterSpell(new TSlow(null));
            Control.RegisterSpell(new TEntangle(null));
            Control.RegisterSpell(new TTremors(null));
            Control.RegisterSpell(new TCallFlames(null));
            Control.RegisterSpell(new TFireBreath(null));
            Control.RegisterSpell(new TSwarm(null));
            Control.RegisterSpell(new TGreatHail(null));
            Control.RegisterSpell(new TFireBall(null));
            Control.RegisterSpell(new TGeyser(null));
            Control.RegisterSpell(new TIceShards(null));
            Control.RegisterSpell(new TFrostBeams(null));
            Control.RegisterSpell(new TVaporize(null));
            Control.RegisterSpell(new TStoning(null));
            Control.RegisterSpell(new TFlamingArrow(null));
            Control.RegisterSpell(new TOoze(null));
            Control.RegisterSpell(new TSacredWrath(null));
            Control.RegisterSpell(new TAnimateDead(null));
            Control.RegisterSpell(new TRecallSpirit(null));
            Control.RegisterSpell(new TWindsOfFury(null));
            Control.RegisterSpell(new TShockwave(null));
            Control.RegisterSpell(new TSacrificialFlame(null));
        }

        // CombatSpells.pas:2130
        public static void UnregisterCombatSpells(TAoWEngine Engine)
        {
            TSpellControl Control;
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
