// Translated from CombatSpells.pas.
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Units;
using Aow.Rtl;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat.Actions
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
        public override void Execute(Aow.Game.Combat.TCombat Combat)
        {
            Aow.Game.Combat.TCombatObject Source;
            base.Execute(Combat);
            if (FManaCost != 0 && Combat.FStatus.Contains(Aow.Game.Combat.TCombatStatusElement.cExecuting))
            {
                Source = Combat.FCombatData.FindID(FSourceID);
                AoWHSSet.FSpellControl.GetSpell(FSpellID).CombatCastingDone(Source, FManaCost);
            }
        }

        // CombatSpells.pas:471
        public override bool Play(Aow.Game.Combat.TCombatViewer Viewer)
        {
            if (Viewer.FTime == 0)
            {
                AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }

            if (Viewer.FDelay == Viewer.FTime)
            {
                Execute(Viewer.FCombat);
            }

            return Viewer.FDelay * 2 > Viewer.FTime;
        }

        // CombatSpells.pas:479
        public override void ShowObject(Aow.Game.Combat.TCombatViewer Viewer, Aow.Game.Combat.TCombatObject Obj, int Frame, int Direction, int X, int Y, in TSurfaceDesc Surface)
        {
            base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
            if (Obj.FID == FSourceID)
            {
                var spell = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.FSpellName, Surface);
            }
            else
            {
                if (Obj.FID == FTargetID)
                {
                    AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
                }
            }
        }

        // CombatSpells.pas:490
        public override void ShowFieldObject(Aow.Game.Combat.TCombatViewer Viewer, Aow.Game.Combat.TCombatObject Obj, int Frame, int Direction, int X, int Y, in TSurfaceDesc Surface)
        {
            // { Original calls ShowObject here, bypassing inherited field markers. }
            base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
            if (Obj.FID == FSourceID)
            {
                var spell = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.FSpellName, Surface);
            }
            else
            {
                if (Obj.FID == FTargetID)
                {
                    AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
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
        public override void Execute(Aow.Game.Combat.TCombat Combat)
        {
            Aow.Game.Combat.TCombatObject Source;
            if (FManaCost != 0 && Combat.FStatus.Contains(Aow.Game.Combat.TCombatStatusElement.cExecuting))
            {
                Source = Combat.FCombatData.FindID(FSourceID);
                AoWHSSet.FSpellControl.GetSpell(FSpellID).CombatCastingDone(Source, FManaCost);
            }
        }

        // CombatSpells.pas:524
        public override bool Play(Aow.Game.Combat.TCombatViewer Viewer)
        {
            if (Viewer.FTime == 0)
            {
                AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }

            if (Viewer.FDelay == Viewer.FTime)
            {
                Execute(Viewer.FCombat);
            }

            return Viewer.FDelay * 3 > Viewer.FTime;
        }

        // CombatSpells.pas:532
        public override void ShowObject(Aow.Game.Combat.TCombatViewer Viewer, Aow.Game.Combat.TCombatObject Obj, int Frame, int Direction, int X, int Y, in TSurfaceDesc Surface)
        {
            if (Obj.FID == FSourceID)
            {
                Obj.ShowAttackAnimation(Viewer.FTime, Direction, X, Y, Surface);
            }
            else
            {
                base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
            }

            if (Obj.FID == FSourceID)
            {
                var spell = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.FSpellName, Surface);
            }
            else
            {
                if (Obj.FID == FTargetID)
                {
                    AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
                }
            }
        }

        // CombatSpells.pas:545
        public override void ShowFieldObject(Aow.Game.Combat.TCombatViewer Viewer, Aow.Game.Combat.TCombatObject Obj, int Frame, int Direction, int X, int Y, in TSurfaceDesc Surface)
        {
            // { Original calls ShowObject here, bypassing inherited field markers. }
            base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
            if (Obj.FID == FSourceID)
            {
                var spell = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.FSpellName, Surface);
            }
            else
            {
                if (Obj.FID == FTargetID)
                {
                    AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
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
        public override void Dispose()
        {
            FTargetIDs?.Dispose();
            base.Dispose();
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
        public override void Execute(Aow.Game.Combat.TCombat Combat)
        {
            Aow.Game.Combat.TCombatObject Source;
            if (FManaCost != 0 && Combat.FStatus.Contains(Aow.Game.Combat.TCombatStatusElement.cExecuting))
            {
                Source = Combat.FCombatData.FindID(FSourceID);
                AoWHSSet.FSpellControl.GetSpell(FSpellID).CombatCastingDone(Source, FManaCost);
            }
        }

        // CombatSpells.pas:591
        public override bool Play(Aow.Game.Combat.TCombatViewer Viewer)
        {
            if (Viewer.FTime == 0)
            {
                AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }

            if (Viewer.FDelay == Viewer.FTime)
            {
                Execute(Viewer.FCombat);
            }

            return Viewer.FDelay * 5 > Viewer.FTime;
        }

        // CombatSpells.pas:599
        public override void ShowObject(Aow.Game.Combat.TCombatViewer Viewer, Aow.Game.Combat.TCombatObject Obj, int Frame, int Direction, int X, int Y, in TSurfaceDesc Surface)
        {
            if (Obj.FID == FSourceID)
            {
                Obj.ShowAttackAnimation(Viewer.FTime, Direction, X, Y, Surface);
            }
            else
            {
                base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
            }

            if (Obj.FID == FSourceID)
            {
                var spell = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.FSpellName, Surface);
            }
            else
            {
                if (FTargetIDs.IndexOf(Obj.FID) != -1)
                {
                    AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
                }
            }
        }

        // CombatSpells.pas:612
        public override void ShowFieldObject(Aow.Game.Combat.TCombatViewer Viewer, Aow.Game.Combat.TCombatObject Obj, int Frame, int Direction, int X, int Y, in TSurfaceDesc Surface)
        {
            // { Original calls ShowObject here, bypassing inherited field markers. }
            base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
            if (Obj.FID == FSourceID)
            {
                var spell = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                AoWEngine.FCombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.FSpellName, Surface);
            }
            else
            {
                if (FTargetIDs.IndexOf(Obj.FID) != -1)
                {
                    AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
                }
            }
        }

        // CombatSpells.pas:624
        public override void ShowFightingObjects(Aow.Game.Combat.TCombatViewer Viewer, int Frame, int X, int Y, in TSurfaceDesc Surface)
        {
            Aow.Game.Combat.TCombatObject Source;
            Source = Viewer.FCombat.FCombatData.FindID(FSourceID);
            Viewer.DefaultShowFightingObjects(null, Source, Frame, X, Y, Surface);
        }

    }
}

namespace Aow.Game.Magic.Spells.Combat
{

    // combat statistics (attack, damage, types, repeats, range, radius, targets). damage mode
    public partial class TCombatSpell : Aow.Game.Magic.Spells.TSpell
    {
        public Aow.Game.Magic.Spells.TSpellCombatInfo FCombatInfo;
        public byte FDamageMode;

        // CombatSpells.pas:632
        public TCombatSpell(TECustomNode owner) : base(owner)
        {
            FSpellType = Aow.Game.Magic.Spells.TSpellType.stCombat;
            FSpellControlTypes = TSet<Aow.Game.Magic.Spells.TSpellControlTypeElement>.Of(Aow.Game.Magic.Spells.TSpellControlTypeElement.sctCombat);
            FCombatInfo.Radius = 1;
            FCombatInfo.TargetTypes = TSet<TCombatTargetTypeElement>.Of(TCombatTargetTypeElement.cttEnemy);
            FCastDuration = 5;
            FDamageMode = 0;
        }

        // CombatSpells.pas:655
        public override TCombatAction CreateCA(TCombatObject Source, TCombatObject Target, bool ChargeMana)
        {
            TCombatSpellCA Action;
            Action = new TCombatSpellCA(null);
            Action.FSpellID = FSpellID;
            Action.GenerateEx(Source, Target, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, FDamageMode);
            if (ChargeMana)
            {
                Action.FManaCost = ((THero)((TCombatUnit)Source).FUnit).CastingMana(this);
            }

            return Action;
        }

        // CombatSpells.pas:643
        public override void GetCombatInfo(TAbstractUnit Source, ref Aow.Game.Magic.Spells.TSpellCombatInfo Info)
        {
            Info = FCombatInfo;
        }

        // CombatSpells.pas:648
        public override int GetCombatDamageValue(TCombatObject Source, TCombatObject Target)
        {
            var hitPoints = Target.GetHitPoints();
            var defense = Target.GetDefense();
            var resistance = Target.GetResistance();
            var immunityTypes = Target.GetImmunityTypes();
            var protectionTypes = Target.GetProtectionTypes();
            return StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, hitPoints, defense, resistance, immunityTypes, protectionTypes) * FCombatInfo.RepeatCount;
        }

        // CombatSpells.pas:668
        public override void GetCombatDamageValueEx(TCombatObject Source, TCombatObject Target, ref TAbilityDamageValues Values)
        {
            TSet<TDamageType> Protection;
            TSet<TDamageType> Immunity;
            Protection = Target.GetProtectionTypes();
            Immunity = Target.GetImmunityTypes();
            Values.DamageTypes = FCombatInfo.DamageTypes;
            var hitPoints = Target.GetHitPoints();
            var defense = Target.GetDefense();
            var resistance = Target.GetResistance();
            var statisticsToLimitedDV = StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, hitPoints, defense, resistance, Immunity, Protection);
            Values.SourceDamage = statisticsToLimitedDV * FCombatInfo.RepeatCount;
            // { Original recomputes the value, using resistance in place of defense for mode 1. }
            if (FDamageMode == 1)
            {
                var targetHitPoints = Target.GetHitPoints();
                var targetResistance = Target.GetResistance();
                var resistance2 = Target.GetResistance();
                var statisticsToLimitedDV2 = StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, targetHitPoints, targetResistance, resistance2, Immunity, Protection);
                Values.SourceDamage = statisticsToLimitedDV2 * FCombatInfo.RepeatCount;
            }
            else
            {
                var a2HitPoints = Target.GetHitPoints();
                var targetDefense = Target.GetDefense();
                var a2Resistance = Target.GetResistance();
                var statisticsToLimitedDV2 = StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, a2HitPoints, targetDefense, a2Resistance, Immunity, Protection);
                Values.SourceDamage = statisticsToLimitedDV2 * FCombatInfo.RepeatCount;
            }

            var statisticsToMaxDamage = StatisticsToMaxDamage(FCombatInfo.Damage, FCombatInfo.DamageTypes, Protection, Immunity);
            Values.SourceCost = statisticsToMaxDamage * FCombatInfo.RepeatCount;
            Values.TargetDamage = 0;
            Values.TargetCost = 0;
        }

        // CombatSpells.pas:754
        public override void fcPrefetchCombatCommands(TFastCombatCommandList Commands)
        {
            TAbstractUnit Hero;
            int Distance;
            string Reason = "";
            TCombatObject Target;
            Aow.Game.Magic.Spells.TSpellCC Command;
            TAbilityDamageValues Values = default;
            Hero = ((TCombatUnit)Commands.FSource).FUnit;
            if (Hero is THero && ((THero)Hero).CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason))
            {
                Distance = Commands.FCombat.GetRoundDistance();
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

                for (int I = 0, last = Commands.FTargetSide.GetCount() - 1; I <= last; I++)
                {
                    Target = Commands.FTargetSide.GetObjects(I);
                    if (Target.GetEnabled() && Target.fcRoundDistance() <= Distance)
                    {
                        var wallSituation = Commands.FSource.FCombat.GetWallSituation(Commands.FSource, Target);
                        fcGetDamageValueEx(Commands.FSource, Target, wallSituation, ref Values);
                        if (Values.SourceDamage != 0)
                        {
                            Command = new Aow.Game.Magic.Spells.TSpellCC(52);
                            Command.FTarget = Commands.FTargetSide.GetObjects(I);
                            Command.FMoveCost = 1;
                            Command.FSpellID = FSpellID;
                            Command.FExpectedDV = DVtoDEV(Values.SourceDamage, Values.TargetDamage, Commands.FSource, Target);
                            Commands.Add(Command);
                        }
                    }
                }
            }
        }

        // CombatSpells.pas:791
        public override bool fcExecuteCombatCommand(TCombatObject Source, TFastCombatCommand Command)
        {
            int Count;
            TCombatAction Action;
            TCombatObject Other;
            int Distance;
            TCombatObject Target;
            TList Targets;
            TCombat Combat;
            Target = Command.FTarget;
            for (int I = 0, last = FCombatInfo.RepeatCount - 1; I <= last; I++)
            {
                if (!Target.FStatus.Contains(TCombatObjectStatusElement.csDestroyed))
                {
                    Action = CreateCA(Source, Target, I == 0);
                    Source.FCombat.ExecuteCombatAction(Action);
                    Action.Release();
                }
            }

            if (FCombatInfo.Radius > 1)
            {
                Combat = Source.FCombat;
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
                            Action = CreateCA(Source, Other, false);
                            Source.FCombat.ExecuteCombatAction(Action);
                            Action.Release();
                        }
                    }

                    Targets.Remove(Other);
                    Count -= 1;
                }

                Targets?.Dispose();
            }

            return true;
        }

        // CombatSpells.pas:693
        public override void fcGetDamageValueEx(TCombatObject Source, TCombatObject Target, TWallSituation WallSituation, ref TAbilityDamageValues Values)
        {
            TSet<TDamageType> DamageTypes;
            TSet<TDamageType> Protection;
            TSet<TDamageType> Immunity;
            int Count;
            int Distance;
            TCombatObject Other;
            TCombat Combat;
            int Damage;
            int Defense;
            if (WallSituation == TWallSituation.wsNone || FCombatInfo.Range == -1 || FCombatInfo.Range >= 8)
            {
                Protection = Target.GetProtectionTypes();
                Immunity = Target.GetImmunityTypes();
                DamageTypes = FCombatInfo.DamageTypes;
                Damage = FCombatInfo.Damage;
                Defense = Target.GetDefense();
                if (WallSituation == TWallSituation.wsTarget && (TSet<TDamageType>.Of(TDamageType.dtMagic, TDamageType.dtDeath, TDamageType.dtHoly) & FCombatInfo.DamageTypes) == TSet<TDamageType>.Of())
                {
                    Defense += 2;
                }

                Values.DamageTypes = DamageTypes;
                var hitPoints = Target.GetHitPoints();
                var resistance = Target.GetResistance();
                var statisticsToLimitedDV = StatisticsToLimitedDV(FCombatInfo.Attack, Damage, DamageTypes, hitPoints, Defense, resistance, Immunity, Protection);
                Values.SourceDamage = statisticsToLimitedDV * FCombatInfo.RepeatCount;
                if (FCombatInfo.Radius > 1)
                {
                    Values.SourceDamage = (int)(((uint)(Values.SourceDamage * FCombatInfo.Radius)) >> 1);
                }

                var statisticsToMaxDamage = StatisticsToMaxDamage(Damage, DamageTypes, Protection, Immunity);
                Values.SourceCost = statisticsToMaxDamage * FCombatInfo.RepeatCount;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                if (Values.SourceDamage != 0 && FCombatInfo.Radius > 1)
                {
                    Combat = Source.FCombat;
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

                    for (int I = Combat.FSides[(int)Target.GetSide()].GetCount() - 1; I >= 0; I--)
                    {
                        var side = Target.GetSide();
                        Other = Combat.FSides[(int)side].GetObjects(I);
                        if (Target != Other && Other.GetEnabled() && (int)(((uint)Other.FPosition) >> 4) == (int)(((uint)Target.FPosition) >> 4) && Other.fcRoundDistance() <= Distance && DamageTypes - Other.GetImmunityTypes() != TSet<TDamageType>.Of())
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

                        Values.SourceDamage += Round((double)(Values.SourceDamage * Count) * 0.5);
                    }
                }
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = TSet<TDamageType>.Of();
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
}

namespace Aow.Game.Combat.Actions
{

    public partial class TTurnUndeadSpellCA : TCombatSpellCA
    {
        // CombatSpells.pas:861
        public new const int ClassID = 131788;

        public TTurnUndeadSpellCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:867
        public override void Execute(Aow.Game.Combat.TCombat Combat)
        {
            Aow.Game.Combat.TCombatObject Source;
            Aow.Game.Combat.TCombatUnit Target;
            TUnitEnchantmentAbilityData Data;
            base.Execute(Combat);
            if (FDamage != 0)
            {
                Source = Combat.FCombatData.FindID(FSourceID);
                Target = (Aow.Game.Combat.TCombatUnit)Combat.FCombatData.FindID(FTargetID);
                if (!Target.FStatus.Contains(Aow.Game.Combat.TCombatObjectStatusElement.csDestroyed))
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
}

namespace Aow.Game.Magic.Spells.Combat
{

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
        public override TCombatAction CreateCA(TCombatObject Source, TCombatObject Target, bool ChargeMana)
        {
            TTurnUndeadSpellCA Action;
            Action = new TTurnUndeadSpellCA(null);
            Action.FSpellID = FSpellID;
            Action.FSourceID = Source.FID;
            Action.FTargetID = Target.FID;
            if (ChargeMana)
            {
                Action.FManaCost = FBaseManaCost;
            }

            bool condition = Target is TCombatUnit;
            if (condition)
            {
                var unitType = ((TCombatUnit)Target).FUnit.GetUnitType();
                condition = TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature).Contains(unitType);
            }

            if (condition && (((TCombatUnit)Target).FUnit.GetRace() == 11 || ((TCombatUnit)Target).FUnit.GetAbilityEnabled(129)))
            {
                Action.GenerateEx(Source, Target, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, FDamageMode);
            }

            return Action;
        }

        // CombatSpells.pas:922
        public override void fcGetDamageValueEx(TCombatObject Source, TCombatObject Target, TWallSituation WallSituation, ref TAbilityDamageValues Values)
        {
            bool condition = Target is TCombatUnit;
            if (condition)
            {
                var unitType = ((TCombatUnit)Target).FUnit.GetUnitType();
                condition = TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature).Contains(unitType);
            }

            if (condition && (((TCombatUnit)Target).FUnit.GetRace() == 11 || ((TCombatUnit)Target).FUnit.GetAbilityEnabled(129)))
            {
                base.fcGetDamageValueEx(Source, Target, WallSituation, ref Values);
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = TSet<TDamageType>.Of();
            }
        }
    }
}

namespace Aow.Game.Combat.Actions
{

    public partial class TFastCombatHighPrayerCA : TMultiTargetCombatSpellCA
    {
        // CombatSpells.pas:937
        public new const int ClassID = 131783;

        public TFastCombatHighPrayerCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:943
        public override void Execute(Aow.Game.Combat.TCombat Combat)
        {
            Aow.Game.Combat.TCombatObject Target;
            base.Execute(Combat);
            for (int I = FTargetIDs.Count - 1; I >= 0; I--)
            {
                var targetID = FTargetIDs[I];
                Target = Combat.FCombatData.FindID(targetID);
                Target.SetHitPoints((byte)(Target.GetHitPoints() + 5));
                AoWHSSet.FAbilityControl.GetAbility(100).Expand(((Aow.Game.Combat.TCombatUnit)Target).FUnit);
            }
        }

        // CombatSpells.pas:958
        public void Generate(Aow.Game.Combat.TCombatObject Source)
        {
            Aow.Game.Combat.TCombatSide Side;
            Aow.Game.Combat.TCombatObject Target;
            FSpellID = 102;
            FSourceID = Source.FID;
            var combat = Source.FCombat;
            var side = Source.GetSide();
            Side = combat.FSides[(int)side];

            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Target = Side.GetObjects(I);
                if (Target is Aow.Game.Combat.TCombatUnit && !Target.FStatus.Contains(Aow.Game.Combat.TCombatObjectStatusElement.csDestroyed))
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
        public override void Execute(Aow.Game.Combat.TCombat Combat)
        {
            Aow.Game.Combat.TCombatObject Target;
            base.Execute(Combat);
            if (FSucceeded)
            {
                Target = Combat.FCombatData.FindID(FTargetID);
                Target.SetHitPoints((byte)(Target.GetHitPoints() + 5));
                AoWHSSet.FAbilityControl.GetAbility(100).Expand(((Aow.Game.Combat.TCombatUnit)Target).FUnit);
            }
        }

    }
}

namespace Aow.Game.Magic.Spells.Combat
{

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
        public override TCombatAction CreateCA(TCombatObject Source, TCombatObject Target, bool ChargeMana)
        {
            TTacticalCombatHighPrayerCA Action;
            Action = new TTacticalCombatHighPrayerCA(null);
            Action.FSucceeded = true;
            Action.FSourceID = Source.FID;
            Action.FTargetID = Target.FID;
            Action.FSpellID = FSpellID;
            if (ChargeMana)
            {
                Action.FManaCost = FBaseManaCost;
            }

            return Action;
        }

        // CombatSpells.pas:1104
        public override void tcGetDamageValueEx(TCombatObject Source, TCombatObject Target, ref TAbilityDamageValues Values)
        {
            int Healing;
            int Value;
            Healing = Target.GetHits() - Target.GetHitPoints();
            if (Healing > 5)
            {
                Healing = 5;
            }

            Value = Healing * 100;
            if (Value != 0)
            {
                Values.SourceDamage = Value;
                Values.SourceCost = Value / 100;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtDeath);
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = TSet<TDamageType>.Of();
            }
        }

        // CombatSpells.pas:1026
        public override void fcPrefetchCombatCommands(TFastCombatCommandList Commands)
        {
            TCombatSide Side;
            TCombatObject Target;
            int Value;
            Aow.Game.Magic.Spells.TSpellCC Command;
            Value = 0;
            var side = Commands.FSource.GetSide();
            Side = Commands.FCombat.FSides[(int)side];
            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Target = Side.GetObjects(I);
                if (Target.FCombatValue != 0 && Target is TCombatUnit)
                {
                    Value += DVtoDEV((Target.GetHits() - Target.GetHitPoints()) * 100, 0, Commands.FSource, Target);
                }
            }

            if (Value != 0)
            {
                Command = new Aow.Game.Magic.Spells.TSpellCC(52);
                Command.FTarget = null;
                Command.FMoveCost = 1;
                Command.FSpellID = FSpellID;
                Command.FExpectedDV = Value;
                Commands.Add(Command);
            }
        }

        // CombatSpells.pas:1054
        public override bool fcExecuteCombatCommand(TCombatObject Source, TFastCombatCommand Command)
        {
            TFastCombatHighPrayerCA Action;
            Action = new TFastCombatHighPrayerCA(null);
            Action.Generate(Source);
            Action.FManaCost = ((THero)((TCombatUnit)Source).FUnit).CastingMana(this);
            Source.FCombat.ExecuteCombatAction(Action);
            Action.Release();
            return true;
        }

        // CombatSpells.pas:1067
        public override void fcGetDamageValueEx(TCombatObject Source, TCombatObject Target, TWallSituation WallSituation, ref TAbilityDamageValues Values)
        {
            int Value;
            int Healing;
            TCombatSide Side;
            TCombatObject Ally;
            Value = 0;
            var side = Source.GetSide();
            Side = Source.FCombat.FSides[(int)side];
            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Ally = Side.GetObjects(I);
                if (Ally.FCombatValue != 0 && Ally is TCombatUnit)
                {
                    Healing = Ally.GetHits() - Ally.GetHitPoints();
                    if (Healing > 5)
                    {
                        Healing = 5;
                    }

                    Value += Healing * 100;
                }
            }

            if (Value != 0)
            {
                Values.SourceDamage = Value;
                Values.SourceCost = Value / 100;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtDeath);
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = TSet<TDamageType>.Of();
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
}

namespace Aow.Game.Combat.Actions
{

    public partial class TMindDecayCA : TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1162
        public new const int ClassID = 131787;

        public TMindDecayCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:1168
        public override void Execute(Aow.Game.Combat.TCombat Combat)
        {
            Aow.Game.Combat.TCombatObject Source;
            Aow.Game.Combat.TCombatUnit Target;
            base.Execute(Combat);
            if (FSucceeded)
            {
                Source = Combat.FCombatData.FindID(FSourceID);
                Target = (Aow.Game.Combat.TCombatUnit)Combat.FCombatData.FindID(FTargetID);
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
}

namespace Aow.Game.Magic.Spells.Combat
{

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
        public override TCombatAction CreateCA(TCombatObject Source, TCombatObject Target, bool ChargeMana)
        {
            TMindDecayCA Action;
            Action = new TMindDecayCA(null);
            Action.FSourceID = Source.FID;
            Action.FTargetID = Target.FID;
            Action.FSpellID = FSpellID;
            if (ChargeMana)
            {
                Action.FManaCost = FBaseManaCost;
            }

            if (Target is TCombatUnit && ((TCombatUnit)Target).FUnit.GetUnitType() != TUnitType.utMachine && ((TCombatUnit)Target).FUnit.GetRace() != 11 && !((TCombatUnit)Target).FUnit.GetAbilityEnabled(129) && !(((TCombatUnit)Target).FUnit is TLeader))
            {
                var resistance = Target.GetResistance();
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
}

namespace Aow.Game.Combat.Actions
{

    public partial class TSlowCA : TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1257
        public new const int ClassID = 131792;

        public TSlowCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:1263
        public override void Execute(Aow.Game.Combat.TCombat Combat)
        {
            Aow.Game.Combat.TCombatUnit Target;
            Aow.Game.Combat.TCombatObject Source;
            TUnitEnchantment Enchantment;
            base.Execute(Combat);
            if (FSucceeded)
            {
                Target = (Aow.Game.Combat.TCombatUnit)Combat.FCombatData.FindID(FTargetID);
                Source = Combat.FCombatData.FindID(FSourceID);
                if (Target.FUnit.ExpandAbility(132))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.FUnit.GetAbilityData(132)).FEnchantment;
                    Enchantment.SetUpkeep(0);
                    Enchantment.SetSource(Source.GetPlayer());
                }
            }
        }

    }
}

namespace Aow.Game.Magic.Spells.Combat
{

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
        public override TCombatAction CreateCA(TCombatObject Source, TCombatObject Target, bool ChargeMana)
        {
            TSlowCA Action;
            Action = new TSlowCA(null);
            Action.FSourceID = Source.FID;
            Action.FTargetID = Target.FID;
            Action.FSpellID = FSpellID;
            if (ChargeMana)
            {
                Action.FManaCost = FBaseManaCost;
            }

            if (!Target.GetAbilityEnabled(132))
            {
                var resistance = Target.GetResistance();
                Action.FSucceeded = HitRole(FCombatInfo.Attack - resistance);
            }

            return Action;
        }

        // CombatSpells.pas:1300
        public override void GetCombatDamageValueEx(TCombatObject Source, TCombatObject Target, ref TAbilityDamageValues Values)
        {
            ref var abilityDamageValues = ref Values;
            abilityDamageValues.SourceDamage = 0;
            abilityDamageValues.SourceCost = 0;
            abilityDamageValues.TargetDamage = 0;
            abilityDamageValues.TargetCost = 0;
            abilityDamageValues.DamageTypes = TSet<TDamageType>.Of();
        }
    }
}

namespace Aow.Game.Combat.Actions
{

    public partial class TEntangleSpellCA : TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1328
        public new const int ClassID = 131784;

        public TEntangleSpellCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:1334
        public override void Execute(Aow.Game.Combat.TCombat Combat)
        {
            Aow.Game.Combat.TCombatObject Source;
            Aow.Game.Combat.TCombatUnit Target;
            TUnitEnchantmentAbilityData Data;
            base.Execute(Combat);
            if (FSucceeded)
            {
                Source = Combat.FCombatData.FindID(FSourceID);
                Target = (Aow.Game.Combat.TCombatUnit)Combat.FCombatData.FindID(FTargetID);
                AoWHSSet.FAbilityControl.GetAbility(94).Expand(Target.FUnit);
                Data = (TUnitEnchantmentAbilityData)Target.FUnit.GetAbilityData(94);
                if (Data != null)
                {
                    Data.FEnchantmentPlayerIndex = Source.FCombatPlayer;
                }
            }
        }

    }
}

namespace Aow.Game.Magic.Spells.Combat
{

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
        public override TCombatAction CreateCA(TCombatObject Source, TCombatObject Target, bool ChargeMana)
        {
            TEntangleSpellCA Action;
            Action = new TEntangleSpellCA(null);
            Action.FSourceID = Source.FID;
            Action.FTargetID = Target.FID;
            Action.FSpellID = FSpellID;
            if (ChargeMana)
            {
                Action.FManaCost = FBaseManaCost;
            }

            if (!Target.GetAbilityEnabled(1) && !Target.GetAbilityEnabled(94))
            {
                var resistance = Target.GetResistance();
                Action.FSucceeded = HitRole(FCombatInfo.Attack - resistance);
            }

            return Action;
        }

        // CombatSpells.pas:1383
        public override void GetCombatDamageValueEx(TCombatObject Source, TCombatObject Target, ref TAbilityDamageValues Values)
        {
            if (!Target.GetAbilityEnabled(1) && !Target.GetAbilityEnabled(94))
            {
                var resistance = Target.GetResistance();
                var hitRoleProbability = HitRoleProbability(FCombatInfo.Attack - resistance);
                var hitPoints = Target.GetHitPoints();
                Values.SourceDamage = Round((double)(hitPoints * 100) * hitRoleProbability);
                var targetHitPoints = Target.GetHitPoints();
                Values.SourceCost = targetHitPoints;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtNone);
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = TSet<TDamageType>.Of();
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
        public override TCombatAction CreateCA(TCombatObject Source, TCombatObject Target, bool ChargeMana)
        {
            TCombatSpellCA Action;
            Action = new TCombatSpellCA(null);
            Action.FSpellID = FSpellID;
            if (Target.GetAbilityEnabled(1) || Target.GetAbilityEnabled(59))
            {
                Action.Generate(Source, Target, FCombatInfo.Attack, (sbyte)(FCombatInfo.Damage / 2), FCombatInfo.DamageTypes);
            }
            else
            {
                Action.Generate(Source, Target, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes);
            }

            if (ChargeMana)
            {
                Action.FManaCost = ((THero)((TCombatUnit)Source).FUnit).CastingMana(this);
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
        public override void GetCombatDamageValueEx(TCombatObject Source, TCombatObject Target, ref TAbilityDamageValues Values)
        {
            Target.GetProtectionTypes();
            Target.GetImmunityTypes();
            Values.DamageTypes = FCombatInfo.DamageTypes;
            var resistance = Target.GetResistance();
            var hitRoleProbability = HitRoleProbability(FCombatInfo.Attack - resistance);
            Values.SourceDamage = Round((double)hitRoleProbability * 200);
            Values.SourceCost = 200;
            Values.TargetDamage = 0;
            Values.TargetCost = 0;
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
}

namespace Aow.Game.Combat.Actions
{

    public partial class TFastCombatTerrorCA : TMultiTargetCombatSpellCA
    {
        // CombatSpells.pas:1666
        public new const int ClassID = 131777;

        public TFastCombatTerrorCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:1672
        public override void Execute(Aow.Game.Combat.TCombat Combat)
        {
            Aow.Game.Combat.TCombatUnit Target;
            base.Execute(Combat);
            for (int I = FTargetIDs.Count - 1; I >= 0; I--)
            {
                var targetID = FTargetIDs[I];
                Target = (Aow.Game.Combat.TCombatUnit)Combat.FCombatData.FindID(targetID);
                AoWHSSet.FAbilityControl.GetAbility(108).Expand(Target.FUnit);
            }
        }

        // CombatSpells.pas:1686
        public void Generate(Aow.Game.Combat.TCombatObject Source)
        {
            Aow.Game.Combat.TCombatSide Side;
            Aow.Game.Combat.TCombatObject Target;
            FSpellID = 105;
            FSourceID = Source.FID;
            var combat = Source.FCombat;
            var opponentSide = Source.GetOpponentSide();
            Side = combat.FSides[(int)opponentSide];

            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Target = Side.GetObjects(I);
                if (!Target.FStatus.Contains(Aow.Game.Combat.TCombatObjectStatusElement.csDestroyed) && !Target.GetAbilityEnabled(108) && !Target.GetAbilityEnabled(67) && Target is Aow.Game.Combat.TCombatUnit && !Target.GetImmunityTypes().Contains(Aow.Game.Combat.TDamageType.dtDeath) && ((Aow.Game.Combat.TCombatUnit)Target).FUnit.GetUnitType() != TUnitType.utMachine && HitRole(6 - Target.GetResistance()))
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
        public override void Execute(Aow.Game.Combat.TCombat Combat)
        {
            Aow.Game.Combat.TCombatUnit Target;
            Aow.Game.Combat.TCombatUnit Source;
            if (FSucceeded)
            {
                Target = (Aow.Game.Combat.TCombatUnit)Combat.FCombatData.FindID(FTargetID);
                AoWHSSet.FAbilityControl.GetAbility(108).Expand(Target.FUnit);
            }

            if (FManaCost != 0 && Combat.FStatus.Contains(Aow.Game.Combat.TCombatStatusElement.cExecuting))
            {
                Source = (Aow.Game.Combat.TCombatUnit)Combat.FCombatData.FindID(FSourceID);
                AoWHSSet.FSpellControl.GetSpell(105).CastingDone(Source.FUnit, FManaCost);
            }
        }

        // CombatSpells.pas:1740
        public override bool Play(Aow.Game.Combat.TCombatViewer Viewer)
        {
            Execute(Viewer.FCombat);
            return false;
        }

        // CombatSpells.pas:1747
        public void Generate(Aow.Game.Combat.TCombatObject Source, Aow.Game.Combat.TCombatObject Target)
        {
            FSucceeded = false;
            FSourceID = Source.FID;
            FTargetID = Target.FID;
            if (!Target.GetAbilityEnabled(108) && !Target.GetAbilityEnabled(67) && Target is Aow.Game.Combat.TCombatUnit && !Target.GetImmunityTypes().Contains(Aow.Game.Combat.TDamageType.dtDeath) && ((Aow.Game.Combat.TCombatUnit)Target).FUnit.GetUnitType() != TUnitType.utMachine)
            {
                FSucceeded = HitRole(6 - Target.GetResistance());
            }
        }

    }
}

namespace Aow.Game.Magic.Spells.Combat
{

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
        public override TCombatAction CreateCA(TCombatObject Source, TCombatObject Target, bool ChargeMana)
        {
            TTacticalCombatTerrorCA Action;
            Action = new TTacticalCombatTerrorCA(null);
            Action.Generate(Source, Target);
            if (ChargeMana)
            {
                Action.FManaCost = ((THero)((TCombatUnit)Source).FUnit).CastingMana(this);
            }

            return Action;
        }

        // CombatSpells.pas:1862
        public override void tcGetDamageValueEx(TCombatObject Source, TCombatObject Target, ref TAbilityDamageValues Values)
        {
            int Value;
            if (!Target.GetAbilityEnabled(108) && Target.FCombatValue != 0 && Target is TCombatUnit && !Target.GetImmunityTypes().Contains(TDamageType.dtDeath))
            {
                Value = Round((double)HitRoleProbability(6 - Target.GetResistance()) * 100);
            }
            else
            {
                Value = 0;
            }

            if (Value != 0)
            {
                Values.SourceDamage = Value;
                Values.SourceCost = Value / 100;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtDeath);
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = TSet<TDamageType>.Of();
            }
        }

        // CombatSpells.pas:1786
        public override void fcPrefetchCombatCommands(TFastCombatCommandList Commands)
        {
            TCombatSide Side;
            TCombatObject Target;
            int Value;
            Aow.Game.Magic.Spells.TSpellCC Command;
            Value = 0;
            var opponentSide = Commands.FSource.GetOpponentSide();
            Side = Commands.FCombat.FSides[(int)opponentSide];
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
                Command = new Aow.Game.Magic.Spells.TSpellCC(52);
                Command.FTarget = null;
                Command.FMoveCost = 1;
                Command.FSpellID = FSpellID;
                Command.FExpectedDV = Value;
                Commands.Add(Command);
            }
        }

        // CombatSpells.pas:1815
        public override bool fcExecuteCombatCommand(TCombatObject Source, TFastCombatCommand Command)
        {
            TFastCombatTerrorCA Action;
            Action = new TFastCombatTerrorCA(null);
            Action.Generate(Source);
            Action.FManaCost = ((THero)((TCombatUnit)Source).FUnit).CastingMana(this);
            Source.FCombat.ExecuteCombatAction(Action);
            Action.Release();
            return true;
        }

        // CombatSpells.pas:1828
        public override void fcGetDamageValueEx(TCombatObject Source, TCombatObject Target, TWallSituation WallSituation, ref TAbilityDamageValues Values)
        {
            int Value;
            TCombatSide Side;
            TCombatObject Opponent;
            Value = 0;
            var opponentSide = Source.GetOpponentSide();
            Side = Source.FCombat.FSides[(int)opponentSide];
            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Opponent = Side.GetObjects(I);
                if (!Opponent.GetAbilityEnabled(108) && Opponent.FCombatValue != 0 && Opponent is TCombatUnit && !Opponent.GetImmunityTypes().Contains(TDamageType.dtDeath))
                {
                    Value += Round((double)HitRoleProbability(6 - Opponent.GetResistance()) * 100);
                }
            }

            if (Value != 0)
            {
                Values.SourceDamage = Value;
                Values.SourceCost = Value / 100;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = TSet<TDamageType>.Of(TDamageType.dtDeath);
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = TSet<TDamageType>.Of();
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
        public override TCombatAction CreateCA(TCombatObject Source, TCombatObject Target, bool ChargeMana)
        {
            TCombatSpellCA Action;
            Action = new TCombatSpellCA(null);
            Action.FSpellID = FSpellID;
            if (Target.GetAbilityEnabled(1) || Target.GetAbilityEnabled(59))
            {
                Action.Generate(Source, Target, (sbyte)(FCombatInfo.Attack + 2), FCombatInfo.Damage, FCombatInfo.DamageTypes);
            }
            else
            {
                Action.Generate(Source, Target, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes);
            }

            if (ChargeMana)
            {
                Action.FManaCost = ((THero)((TCombatUnit)Source).FUnit).CastingMana(this);
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
}

namespace Aow.Game.Combat.Actions
{

    // inherited source ID, target ID, mana, success, spell ID
    public partial class TRecallSpiritCA : TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1950
        public new const int ClassID = 131781;

        public TRecallSpiritCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:1953
        public override void Execute(Aow.Game.Combat.TCombat Combat)
        {
            Aow.Game.Combat.TCombatUnit Source;
            Aow.Game.Combat.TCombatUnit Target;
            TUnit NewUnit;
            base.Execute(Combat);
            if (FSucceeded)
            {
                Source = (Aow.Game.Combat.TCombatUnit)Combat.FCombatData.FindID(FSourceID);
                Target = (Aow.Game.Combat.TCombatUnit)Combat.FCombatData.FindID(FTargetID);
                NewUnit = new TUnit(null);
                NewUnit.SetUnitResource(AoWHSSet.FUnitResources.GetUnitResource(130));
                NewUnit.SetPlayer(Source.FUnit.FPlayer);
                NewUnit.ExpandAbility(130);
                NewUnit.Activate();
                Target.SetUnit(NewUnit);
                NewUnit.Release();
                Target.SetStatus(Target.FStatus - TSet<Aow.Game.Combat.TCombatObjectStatusElement>.Of(Aow.Game.Combat.TCombatObjectStatusElement.csDestroyed));
                Target.SetCombatPlayer((byte)Source.FCombatPlayer);
            }
        }

    }
}

namespace Aow.Game.Magic.Spells.Combat
{

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
        public override TCombatAction CreateCA(TCombatObject Source, TCombatObject Target, bool ChargeMana)
        {
            TRecallSpiritCA Action;
            Action = new TRecallSpiritCA(null);
            Action.FSourceID = Source.FID;
            Action.FTargetID = Target.FID;
            Action.FSucceeded = Target.FStatus.Contains(TCombatObjectStatusElement.csDestroyed);
            Action.FSpellID = FSpellID;
            if (ChargeMana)
            {
                Action.FManaCost = FBaseManaCost;
            }

            return Action;
        }

        // CombatSpells.pas:1990
        public override void fcGetDamageValueEx(TCombatObject Source, TCombatObject Target, TWallSituation WallSituation, ref TAbilityDamageValues Values)
        {
            if (Target.FStatus.Contains(TCombatObjectStatusElement.csDestroyed))
            {
                // { Original leaves the other damage-value fields unchanged on success. }
                Values.SourceDamage = 200;
                Values.DamageTypes = TSet<TDamageType>.Of();
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = TSet<TDamageType>.Of();
            }
        }
    }
}

namespace Aow.Game.Combat.Actions
{

    // inherited source ID, target ID, mana, success, spell ID
    public partial class TAnimateDeadCA : TExclusiveCombatSpellCA
    {
        // CombatSpells.pas:2021
        public new const int ClassID = 131782;

        public TAnimateDeadCA(TECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:2024
        public override void Execute(Aow.Game.Combat.TCombat Combat)
        {
            Aow.Game.Combat.TCombatUnit Source;
            Aow.Game.Combat.TCombatUnit Target;
            TUnit NewUnit;
            base.Execute(Combat);
            if (FSucceeded)
            {
                Source = (Aow.Game.Combat.TCombatUnit)Combat.FCombatData.FindID(FSourceID);
                Target = (Aow.Game.Combat.TCombatUnit)Combat.FCombatData.FindID(FTargetID);
                NewUnit = new TUnit(null);
                NewUnit.SetUnitResource(AoWHSSet.FUnitResources.GetUnitResource(201));
                NewUnit.SetPlayer(Source.FUnit.FPlayer);
                NewUnit.ExpandAbility(130);
                NewUnit.Activate();
                Target.SetUnit(NewUnit);
                NewUnit.Release();
                Target.SetStatus(Target.FStatus - TSet<Aow.Game.Combat.TCombatObjectStatusElement>.Of(Aow.Game.Combat.TCombatObjectStatusElement.csDestroyed));
                Target.SetCombatPlayer((byte)Source.FCombatPlayer);
            }
        }

    }
}

namespace Aow.Game.Magic.Spells.Combat
{

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
        public override TCombatAction CreateCA(TCombatObject Source, TCombatObject Target, bool ChargeMana)
        {
            TAnimateDeadCA Action;
            Action = new TAnimateDeadCA(null);
            Action.FSourceID = Source.FID;
            Action.FTargetID = Target.FID;
            Action.FSucceeded = Target.FStatus.Contains(TCombatObjectStatusElement.csDestroyed);
            Action.FSpellID = FSpellID;
            if (ChargeMana)
            {
                Action.FManaCost = FBaseManaCost;
            }

            return Action;
        }

        // CombatSpells.pas:2061
        public override void fcGetDamageValueEx(TCombatObject Source, TCombatObject Target, TWallSituation WallSituation, ref TAbilityDamageValues Values)
        {
            if (Target.FStatus.Contains(TCombatObjectStatusElement.csDestroyed))
            {
                // { Original leaves the other damage-value fields unchanged on success. }
                Values.SourceDamage = 200;
                Values.DamageTypes = TSet<TDamageType>.Of();
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = TSet<TDamageType>.Of();
            }
        }
    }

    public static partial class CombatSpellsUnit
    {
        // CombatSpells.pas:2092
        public static void RegisterCombatSpells(TAoWEngine Engine)
        {
            Aow.Game.Magic.Spells.TSpellControl Control;
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
            Aow.Game.Magic.Spells.TSpellControl Control;
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
