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
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat.Actions
{

    // spell ID, casting mana. source and target IDs inherited
    public partial class CombatSpellCA : DamageCA
    {
        // CombatSpells.pas:447
        public new const int ClassID = 131778;

        public CombatSpellCA(ECustomNode owner) : base(owner)
        { }

        public int FSpellID;
        public int FManaCost;

        // CombatSpells.pas:452
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FSpellID, 30);
            Stream.rwInteger(ref FManaCost, 31);
        }

        // CombatSpells.pas:459
        public override void Execute(global::Aow.Game.Combat.Combat Combat)
        {
            Aow.Game.Combat.CombatObject Source;
            base.Execute(Combat);
            if (FManaCost != 0 && Combat.FStatus.Contains(Aow.Game.Combat.CombatStatusElement.Executing))
            {
                Source = Combat.FCombatData.FindID(FSourceID);
                Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CombatCastingDone(Source, FManaCost);
            }
        }

        // CombatSpells.pas:471
        public override bool Play(Aow.Game.Combat.CombatViewer Viewer)
        {
            if (Viewer.FTime == 0)
            {
                Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }

            if (Viewer.FDelay == Viewer.FTime)
            {
                Execute(Viewer.FCombat);
            }

            return Viewer.FDelay * 2 > Viewer.FTime;
        }

        // CombatSpells.pas:479
        public override void ShowObject(Aow.Game.Combat.CombatViewer Viewer, Aow.Game.Combat.CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
        {
            base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
            if (Obj.FID == FSourceID)
            {
                var spell = Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
                Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.FSpellName, Surface);
            }
            else
            {
                if (Obj.FID == FTargetID)
                {
                    Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
                }
            }
        }

        // CombatSpells.pas:490
        public override void ShowFieldObject(Aow.Game.Combat.CombatViewer Viewer, Aow.Game.Combat.CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
        {
            // { Original calls ShowObject here, bypassing inherited field markers. }
            base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
            if (Obj.FID == FSourceID)
            {
                var spell = Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
                Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.FSpellName, Surface);
            }
            else
            {
                if (Obj.FID == FTargetID)
                {
                    Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
                }
            }
        }

    }

    // source ID, target ID, casting mana, success, spell ID
    public partial class ExclusiveCombatSpellCA : CombatAction
    {
        public ExclusiveCombatSpellCA(ECustomNode owner) : base(owner)
        { }

        public byte FSourceID;
        public byte FTargetID;
        public int FManaCost;
        public bool FSucceeded;
        public int FSpellID;

        // CombatSpells.pas:502
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref FSourceID, 20);
            Stream.rwByte(ref FTargetID, 21);
            Stream.rwInteger(ref FManaCost, 22);
            Stream.rwBool(ref FSucceeded, 23);
            Stream.rwInteger(ref FSpellID, 24);
        }

        // CombatSpells.pas:512
        public override void Execute(global::Aow.Game.Combat.Combat Combat)
        {
            Aow.Game.Combat.CombatObject Source;
            if (FManaCost != 0 && Combat.FStatus.Contains(Aow.Game.Combat.CombatStatusElement.Executing))
            {
                Source = Combat.FCombatData.FindID(FSourceID);
                Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CombatCastingDone(Source, FManaCost);
            }
        }

        // CombatSpells.pas:524
        public override bool Play(Aow.Game.Combat.CombatViewer Viewer)
        {
            if (Viewer.FTime == 0)
            {
                Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }

            if (Viewer.FDelay == Viewer.FTime)
            {
                Execute(Viewer.FCombat);
            }

            return Viewer.FDelay * 3 > Viewer.FTime;
        }

        // CombatSpells.pas:532
        public override void ShowObject(Aow.Game.Combat.CombatViewer Viewer, Aow.Game.Combat.CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
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
                var spell = Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
                Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.FSpellName, Surface);
            }
            else
            {
                if (Obj.FID == FTargetID)
                {
                    Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
                }
            }
        }

        // CombatSpells.pas:545
        public override void ShowFieldObject(Aow.Game.Combat.CombatViewer Viewer, Aow.Game.Combat.CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
        {
            // { Original calls ShowObject here, bypassing inherited field markers. }
            base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
            if (Obj.FID == FSourceID)
            {
                var spell = Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
                Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.FSpellName, Surface);
            }
            else
            {
                if (Obj.FID == FTargetID)
                {
                    Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
                }
            }
        }
    }

    // source ID, target byte list, casting mana, success, spell ID
    public partial class MultiTargetCombatSpellCA : CombatAction
    {
        public byte FSourceID;
        public ByteList FTargetIDs;
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
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref FSourceID, 20);
            Stream.rwStreamProc(21, FTargetIDs.LoadFromStream, FTargetIDs.SaveToStream);
            Stream.rwInteger(ref FManaCost, 22);
            Stream.rwBool(ref FSucceeded, 23);
            Stream.rwInteger(ref FSpellID, 24);
        }

        // CombatSpells.pas:557
        public MultiTargetCombatSpellCA(ECustomNode owner) : base(owner)
        {
            FTargetIDs = new ByteList();
        }

        // CombatSpells.pas:579
        public override void Execute(global::Aow.Game.Combat.Combat Combat)
        {
            Aow.Game.Combat.CombatObject Source;
            if (FManaCost != 0 && Combat.FStatus.Contains(Aow.Game.Combat.CombatStatusElement.Executing))
            {
                Source = Combat.FCombatData.FindID(FSourceID);
                Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CombatCastingDone(Source, FManaCost);
            }
        }

        // CombatSpells.pas:591
        public override bool Play(Aow.Game.Combat.CombatViewer Viewer)
        {
            if (Viewer.FTime == 0)
            {
                Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }

            if (Viewer.FDelay == Viewer.FTime)
            {
                Execute(Viewer.FCombat);
            }

            return Viewer.FDelay * 5 > Viewer.FTime;
        }

        // CombatSpells.pas:599
        public override void ShowObject(Aow.Game.Combat.CombatViewer Viewer, Aow.Game.Combat.CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
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
                var spell = Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
                Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.FSpellName, Surface);
            }
            else
            {
                if (FTargetIDs.IndexOf(Obj.FID) != -1)
                {
                    Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
                }
            }
        }

        // CombatSpells.pas:612
        public override void ShowFieldObject(Aow.Game.Combat.CombatViewer Viewer, Aow.Game.Combat.CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
        {
            // { Original calls ShowObject here, bypassing inherited field markers. }
            base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
            if (Obj.FID == FSourceID)
            {
                var spell = Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
                Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCombatFont.SetTextCentered(X + 32, Y + 32 + 8, spell.FSpellName, Surface);
            }
            else
            {
                if (FTargetIDs.IndexOf(Obj.FID) != -1)
                {
                    Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.Show(11, X + 32, Y + 32, Viewer.FTime, Surface);
                }
            }
        }

        // CombatSpells.pas:624
        public override void ShowFightingObjects(Aow.Game.Combat.CombatViewer Viewer, int Frame, int X, int Y, in SurfaceDesc Surface)
        {
            Aow.Game.Combat.CombatObject Source;
            Source = Viewer.FCombat.FCombatData.FindID(FSourceID);
            Viewer.DefaultShowFightingObjects(null, Source, Frame, X, Y, Surface);
        }

    }
}

namespace Aow.Game.Magic.Spells.Combat
{

    // combat statistics (attack, damage, types, repeats, range, radius, targets). damage mode
    public partial class CombatSpell : Aow.Game.Magic.Spells.Spell
    {
        public Aow.Game.Magic.Spells.SpellCombatInfo FCombatInfo;
        public byte FDamageMode;

        // CombatSpells.pas:632
        public CombatSpell(ECustomNode owner) : base(owner)
        {
            FSpellType = Aow.Game.Magic.Spells.SpellType.Combat;
            FSpellControlTypes = Set<Aow.Game.Magic.Spells.SpellControlTypeElement>.Of(Aow.Game.Magic.Spells.SpellControlTypeElement.Combat);
            FCombatInfo.Radius = 1;
            FCombatInfo.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
            FCastDuration = 5;
            FDamageMode = 0;
        }

        // CombatSpells.pas:655
        public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
        {
            CombatSpellCA Action;
            Action = new CombatSpellCA(null);
            Action.FSpellID = FSpellID;
            Action.GenerateEx(Source, Target, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, FDamageMode);
            if (ChargeMana)
            {
                Action.FManaCost = ((Hero)((CombatUnit)Source).FUnit).CastingMana(this);
            }

            return Action;
        }

        // CombatSpells.pas:643
        public override void GetCombatInfo(AbstractUnit Source, ref Aow.Game.Magic.Spells.SpellCombatInfo Info)
        {
            Info = FCombatInfo;
        }

        // CombatSpells.pas:648
        public override int GetCombatDamageValue(CombatObject Source, CombatObject Target)
        {
            var hitPoints = Target.GetHitPoints();
            var defense = Target.GetDefense();
            var resistance = Target.GetResistance();
            var immunityTypes = Target.GetImmunityTypes();
            var protectionTypes = Target.GetProtectionTypes();
            return Aow.Game.Magic.Abilities.AbilityRules.StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, hitPoints, defense, resistance, immunityTypes, protectionTypes) * FCombatInfo.RepeatCount;
        }

        // CombatSpells.pas:668
        public override void GetCombatDamageValueEx(CombatObject Source, CombatObject Target, ref AbilityDamageValues Values)
        {
            Set<DamageType> Protection;
            Set<DamageType> Immunity;
            Protection = Target.GetProtectionTypes();
            Immunity = Target.GetImmunityTypes();
            Values.DamageTypes = FCombatInfo.DamageTypes;
            var hitPoints = Target.GetHitPoints();
            var defense = Target.GetDefense();
            var resistance = Target.GetResistance();
            var statisticsToLimitedDV = Aow.Game.Magic.Abilities.AbilityRules.StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, hitPoints, defense, resistance, Immunity, Protection);
            Values.SourceDamage = statisticsToLimitedDV * FCombatInfo.RepeatCount;
            // { Original recomputes the value, using resistance in place of defense for mode 1. }
            if (FDamageMode == 1)
            {
                var targetHitPoints = Target.GetHitPoints();
                var targetResistance = Target.GetResistance();
                var resistance2 = Target.GetResistance();
                var statisticsToLimitedDV2 = Aow.Game.Magic.Abilities.AbilityRules.StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, targetHitPoints, targetResistance, resistance2, Immunity, Protection);
                Values.SourceDamage = statisticsToLimitedDV2 * FCombatInfo.RepeatCount;
            }
            else
            {
                var a2HitPoints = Target.GetHitPoints();
                var targetDefense = Target.GetDefense();
                var a2Resistance = Target.GetResistance();
                var statisticsToLimitedDV2 = Aow.Game.Magic.Abilities.AbilityRules.StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, a2HitPoints, targetDefense, a2Resistance, Immunity, Protection);
                Values.SourceDamage = statisticsToLimitedDV2 * FCombatInfo.RepeatCount;
            }

            var statisticsToMaxDamage = Aow.Game.Magic.Abilities.AbilityRules.StatisticsToMaxDamage(FCombatInfo.Damage, FCombatInfo.DamageTypes, Protection, Immunity);
            Values.SourceCost = statisticsToMaxDamage * FCombatInfo.RepeatCount;
            Values.TargetDamage = 0;
            Values.TargetCost = 0;
        }

        // CombatSpells.pas:754
        public override void fcPrefetchCombatCommands(FastCombatCommandList Commands)
        {
            AbstractUnit Hero;
            int Distance;
            string Reason = "";
            CombatObject Target;
            Aow.Game.Magic.Spells.SpellCC Command;
            AbilityDamageValues Values = default;
            Hero = ((CombatUnit)Commands.FSource).FUnit;
            if (Hero is global::Aow.Game.Magic.Abilities.Hero && ((global::Aow.Game.Magic.Abilities.Hero)Hero).CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason))
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
                            Command = new Aow.Game.Magic.Spells.SpellCC(52);
                            Command.FTarget = Commands.FTargetSide.GetObjects(I);
                            Command.FMoveCost = 1;
                            Command.FSpellID = FSpellID;
                            Command.FExpectedDV = Aow.Game.Magic.Abilities.AbilityRules.DVtoDEV(Values.SourceDamage, Values.TargetDamage, Commands.FSource, Target);
                            Commands.Add(Command);
                        }
                    }
                }
            }
        }

        // CombatSpells.pas:791
        public override bool fcExecuteCombatCommand(CombatObject Source, FastCombatCommand Command)
        {
            int Count;
            CombatAction Action;
            CombatObject Other;
            int Distance;
            CombatObject Target;
            DelphiList Targets;
            global::Aow.Game.Combat.Combat Combat;
            Target = Command.FTarget;
            for (int I = 0, last = FCombatInfo.RepeatCount - 1; I <= last; I++)
            {
                if (!Target.FStatus.Contains(CombatObjectStatusElement.Destroyed))
                {
                    Action = CreateCA(Source, Target, I == 0);
                    Source.FCombat.ExecuteCombatAction(Action);
                    Action.Release();
                }
            }

            if (FCombatInfo.Radius > 1)
            {
                Combat = Source.FCombat;
                Targets = new DelphiList();
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
                        condition = FCombatInfo.DamageTypes - immunityTypes != Set<DamageType>.Of();
                    }

                    if (condition)
                    {
                        Targets.Add(Other);
                    }
                }

                // { Original shifts Random(4) by two, so this adds no random extra targets. }
                Count = (int)(((uint)Aow.Game.World.Map.AoWHSMap.Current.Random(4)) >> 2) + FCombatInfo.Radius;
                while (Count != 0 && Targets.Count != 0)
                {
                    Other = (CombatObject)Targets[Aow.Game.World.Map.AoWHSMap.Current.Random(Targets.Count)];
                    for (int J = 0, last = FCombatInfo.RepeatCount - 1; J <= last; J++)
                    {
                        if (!Other.FStatus.Contains(CombatObjectStatusElement.Destroyed))
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
        public override void fcGetDamageValueEx(CombatObject Source, CombatObject Target, WallSituation WallSituation, ref AbilityDamageValues Values)
        {
            Set<DamageType> DamageTypes;
            Set<DamageType> Protection;
            Set<DamageType> Immunity;
            int Count;
            int Distance;
            CombatObject Other;
            global::Aow.Game.Combat.Combat Combat;
            int Damage;
            int Defense;
            if (WallSituation == global::Aow.Game.Combat.WallSituation.None || FCombatInfo.Range == -1 || FCombatInfo.Range >= 8)
            {
                Protection = Target.GetProtectionTypes();
                Immunity = Target.GetImmunityTypes();
                DamageTypes = FCombatInfo.DamageTypes;
                Damage = FCombatInfo.Damage;
                Defense = Target.GetDefense();
                if (WallSituation == global::Aow.Game.Combat.WallSituation.Target && (Set<DamageType>.Of(DamageType.Magic, DamageType.Death, DamageType.Holy) & FCombatInfo.DamageTypes) == Set<DamageType>.Of())
                {
                    Defense += 2;
                }

                Values.DamageTypes = DamageTypes;
                var hitPoints = Target.GetHitPoints();
                var resistance = Target.GetResistance();
                var statisticsToLimitedDV = Aow.Game.Magic.Abilities.AbilityRules.StatisticsToLimitedDV(FCombatInfo.Attack, Damage, DamageTypes, hitPoints, Defense, resistance, Immunity, Protection);
                Values.SourceDamage = statisticsToLimitedDV * FCombatInfo.RepeatCount;
                if (FCombatInfo.Radius > 1)
                {
                    Values.SourceDamage = (int)(((uint)(Values.SourceDamage * FCombatInfo.Radius)) >> 1);
                }

                var statisticsToMaxDamage = Aow.Game.Magic.Abilities.AbilityRules.StatisticsToMaxDamage(Damage, DamageTypes, Protection, Immunity);
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
                        if (Target != Other && Other.GetEnabled() && (int)(((uint)Other.FPosition) >> 4) == (int)(((uint)Target.FPosition) >> 4) && Other.fcRoundDistance() <= Distance && DamageTypes - Other.GetImmunityTypes() != Set<DamageType>.Of())
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
                Values.DamageTypes = Set<DamageType>.Of();
            }
        }
    }

    // inherited combat statistics, damage mode
    public partial class SolarFlare : CombatSpell
    {
        // CombatSpells.pas:845
        public SolarFlare(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.SolarFlare.SolarFlareRStr);
            FSpellID = 100;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 4;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Holy);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        private static readonly string SolarFlareRStr = "Solar Flare";
    }
}

namespace Aow.Game.Combat.Actions
{

    public partial class TurnUndeadSpellCA : CombatSpellCA
    {
        // CombatSpells.pas:861
        public new const int ClassID = 131788;

        public TurnUndeadSpellCA(ECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:867
        public override void Execute(global::Aow.Game.Combat.Combat Combat)
        {
            Aow.Game.Combat.CombatObject Source;
            Aow.Game.Combat.CombatUnit Target;
            UnitEnchantmentAbilityData Data;
            base.Execute(Combat);
            if (FDamage != 0)
            {
                Source = Combat.FCombatData.FindID(FSourceID);
                Target = (Aow.Game.Combat.CombatUnit)Combat.FCombatData.FindID(FTargetID);
                if (!Target.FStatus.Contains(Aow.Game.Combat.CombatObjectStatusElement.Destroyed))
                {
                    Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(34).Expand(Target.FUnit);
                    Data = (UnitEnchantmentAbilityData)Target.FUnit.GetAbilityData(34);
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
    public partial class TurnUndead : CombatSpell
    {
        // CombatSpells.pas:888
        public TurnUndead(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TurnUndeadRStr);
            FSpellID = 101;
            FDamageMode = 1;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Holy);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        // CombatSpells.pas:905
        public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
        {
            TurnUndeadSpellCA Action;
            Action = new TurnUndeadSpellCA(null);
            Action.FSpellID = FSpellID;
            Action.FSourceID = Source.FID;
            Action.FTargetID = Target.FID;
            if (ChargeMana)
            {
                Action.FManaCost = FBaseManaCost;
            }

            bool condition = Target is CombatUnit;
            if (condition)
            {
                var unitType = ((CombatUnit)Target).FUnit.GetUnitType();
                condition = Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType);
            }

            if (condition && (((CombatUnit)Target).FUnit.GetRace() == 11 || ((CombatUnit)Target).FUnit.GetAbilityEnabled(129)))
            {
                Action.GenerateEx(Source, Target, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, FDamageMode);
            }

            return Action;
        }

        // CombatSpells.pas:922
        public override void fcGetDamageValueEx(CombatObject Source, CombatObject Target, WallSituation WallSituation, ref AbilityDamageValues Values)
        {
            bool condition = Target is CombatUnit;
            if (condition)
            {
                var unitType = ((CombatUnit)Target).FUnit.GetUnitType();
                condition = Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType);
            }

            if (condition && (((CombatUnit)Target).FUnit.GetRace() == 11 || ((CombatUnit)Target).FUnit.GetAbilityEnabled(129)))
            {
                base.fcGetDamageValueEx(Source, Target, WallSituation, ref Values);
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = Set<DamageType>.Of();
            }
        }
    }
}

namespace Aow.Game.Combat.Actions
{

    public partial class FastCombatHighPrayerCA : MultiTargetCombatSpellCA
    {
        // CombatSpells.pas:937
        public new const int ClassID = 131783;

        public FastCombatHighPrayerCA(ECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:943
        public override void Execute(global::Aow.Game.Combat.Combat Combat)
        {
            Aow.Game.Combat.CombatObject Target;
            base.Execute(Combat);
            for (int I = FTargetIDs.Count - 1; I >= 0; I--)
            {
                var targetID = FTargetIDs[I];
                Target = Combat.FCombatData.FindID(targetID);
                Target.SetHitPoints((byte)(Target.GetHitPoints() + 5));
                Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(100).Expand(((Aow.Game.Combat.CombatUnit)Target).FUnit);
            }
        }

        // CombatSpells.pas:958
        public void Generate(Aow.Game.Combat.CombatObject Source)
        {
            Aow.Game.Combat.CombatSide Side;
            Aow.Game.Combat.CombatObject Target;
            FSpellID = 102;
            FSourceID = Source.FID;
            var combat = Source.FCombat;
            var side = Source.GetSide();
            Side = combat.FSides[(int)side];

            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Target = Side.GetObjects(I);
                if (Target is Aow.Game.Combat.CombatUnit && !Target.FStatus.Contains(Aow.Game.Combat.CombatObjectStatusElement.Destroyed))
                {
                    FTargetIDs.Add(Target.FID);
                }
            }
        }

    }

    public partial class TacticalCombatHighPrayerCA : ExclusiveCombatSpellCA
    {
        // CombatSpells.pas:976
        public new const int ClassID = 131786;

        public TacticalCombatHighPrayerCA(ECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:982
        public override void Execute(global::Aow.Game.Combat.Combat Combat)
        {
            Aow.Game.Combat.CombatObject Target;
            base.Execute(Combat);
            if (FSucceeded)
            {
                Target = Combat.FCombatData.FindID(FTargetID);
                Target.SetHitPoints((byte)(Target.GetHitPoints() + 5));
                Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(100).Expand(((Aow.Game.Combat.CombatUnit)Target).FUnit);
            }
        }

    }
}

namespace Aow.Game.Magic.Spells.Combat
{

    // inherited combat statistics, damage mode
    public partial class HighPrayer : CombatSpell
    {
        // CombatSpells.pas:996
        public HighPrayer(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.HighPrayer.HighPrayerRStr);
            FSpellID = 102;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of();
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = -1;
            spellCombatInfo.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Friend);
        }

        // CombatSpells.pas:1013
        public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
        {
            TacticalCombatHighPrayerCA Action;
            Action = new TacticalCombatHighPrayerCA(null);
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
        public override void tcGetDamageValueEx(CombatObject Source, CombatObject Target, ref AbilityDamageValues Values)
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
                Values.DamageTypes = Set<DamageType>.Of(DamageType.Death);
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = Set<DamageType>.Of();
            }
        }

        // CombatSpells.pas:1026
        public override void fcPrefetchCombatCommands(FastCombatCommandList Commands)
        {
            CombatSide Side;
            CombatObject Target;
            int Value;
            Aow.Game.Magic.Spells.SpellCC Command;
            Value = 0;
            var side = Commands.FSource.GetSide();
            Side = Commands.FCombat.FSides[(int)side];
            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Target = Side.GetObjects(I);
                if (Target.FCombatValue != 0 && Target is CombatUnit)
                {
                    Value += Aow.Game.Magic.Abilities.AbilityRules.DVtoDEV((Target.GetHits() - Target.GetHitPoints()) * 100, 0, Commands.FSource, Target);
                }
            }

            if (Value != 0)
            {
                Command = new Aow.Game.Magic.Spells.SpellCC(52);
                Command.FTarget = null;
                Command.FMoveCost = 1;
                Command.FSpellID = FSpellID;
                Command.FExpectedDV = Value;
                Commands.Add(Command);
            }
        }

        // CombatSpells.pas:1054
        public override bool fcExecuteCombatCommand(CombatObject Source, FastCombatCommand Command)
        {
            FastCombatHighPrayerCA Action;
            Action = new FastCombatHighPrayerCA(null);
            Action.Generate(Source);
            Action.FManaCost = ((Hero)((CombatUnit)Source).FUnit).CastingMana(this);
            Source.FCombat.ExecuteCombatAction(Action);
            Action.Release();
            return true;
        }

        // CombatSpells.pas:1067
        public override void fcGetDamageValueEx(CombatObject Source, CombatObject Target, WallSituation WallSituation, ref AbilityDamageValues Values)
        {
            int Value;
            int Healing;
            CombatSide Side;
            CombatObject Ally;
            Value = 0;
            var side = Source.GetSide();
            Side = Source.FCombat.FSides[(int)side];
            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Ally = Side.GetObjects(I);
                if (Ally.FCombatValue != 0 && Ally is CombatUnit)
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
                Values.DamageTypes = Set<DamageType>.Of(DamageType.Death);
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = Set<DamageType>.Of();
            }
        }

        private static readonly string HighPrayerRStr = "High Prayer";
    }

    // inherited combat statistics, damage mode
    public partial class DeathRay : CombatSpell
    {
        // CombatSpells.pas:1130
        public DeathRay(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.DeathRay.DeathRayRStr);
            FSpellID = 103;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 4;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Death);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        private static readonly string DeathRayRStr = "Death Ray";
    }

    // inherited combat statistics, damage mode
    public partial class DiseaseCloud : CombatSpell
    {
        // CombatSpells.pas:1146
        public DiseaseCloud(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.DiseaseCloud.DiseaseCloudRStr);
            FSpellID = 104;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 3;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Death);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        private static readonly string DiseaseCloudRStr = "Disease Cloud";
    }
}

namespace Aow.Game.Combat.Actions
{

    public partial class MindDecayCA : ExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1162
        public new const int ClassID = 131787;

        public MindDecayCA(ECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:1168
        public override void Execute(global::Aow.Game.Combat.Combat Combat)
        {
            Aow.Game.Combat.CombatObject Source;
            Aow.Game.Combat.CombatUnit Target;
            base.Execute(Combat);
            if (FSucceeded)
            {
                Source = Combat.FCombatData.FindID(FSourceID);
                Target = (Aow.Game.Combat.CombatUnit)Combat.FCombatData.FindID(FTargetID);
                Source.SetExperience(Source.GetExperience() + Target.GetLevel());
                for (int I = 0; I <= 2; I++)
                {
                    ((CommandAbility)Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(Aow.Game.Magic.Abilities.AbilityRules.CommandAbilityIDs[I])).ResetCommandedUnits(Target);
                }

                for (int I = 0; I <= 2; I++)
                {
                    ((CommandAbility)Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(Aow.Game.Magic.Abilities.AbilityRules.CommandAbilityIDs[I])).Uncommand(Target);
                }

                Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(131).Expand(Target.FUnit);
                Target.SetCombatPlayer((byte)Source.FCombatPlayer);
            }
        }

    }
}

namespace Aow.Game.Magic.Spells.Combat
{

    // inherited combat statistics, damage mode
    public partial class MindDecay : CombatSpell
    {
        // CombatSpells.pas:1190
        public MindDecay(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.MindDecay.MindDecayRStr);
            FSpellID = 106;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 5;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.None);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 3;
        }

        // CombatSpells.pas:1207
        public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
        {
            MindDecayCA Action;
            Action = new MindDecayCA(null);
            Action.FSourceID = Source.FID;
            Action.FTargetID = Target.FID;
            Action.FSpellID = FSpellID;
            if (ChargeMana)
            {
                Action.FManaCost = FBaseManaCost;
            }

            if (Target is CombatUnit && ((CombatUnit)Target).FUnit.GetUnitType() != UnitType.Machine && ((CombatUnit)Target).FUnit.GetRace() != 11 && !((CombatUnit)Target).FUnit.GetAbilityEnabled(129) && !(((CombatUnit)Target).FUnit is Leader))
            {
                var resistance = Target.GetResistance();
                Action.FSucceeded = Aow.Game.Magic.Abilities.HitChance.HitRole(FCombatInfo.Attack - resistance);
            }

            return Action;
        }

        private static readonly string MindDecayRStr = "Mind Decay";
    }

    // inherited combat statistics, damage mode
    public partial class ChainLightning : CombatSpell
    {
        // CombatSpells.pas:1224
        public ChainLightning(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.ChainLightning.ChainLightningRStr);
            FSpellID = 107;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 4;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Lightning);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 3;
        }

        private static readonly string ChainLightningRStr = "Chain Lighting";
    }

    // inherited combat statistics, damage mode
    public partial class ColdBreath : CombatSpell
    {
        // CombatSpells.pas:1241
        public ColdBreath(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.ColdBreath.ColdBreathRStr);
            FSpellID = 108;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Cold);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 4;
        }

        private static readonly string ColdBreathRStr = "Cold Breath";
    }
}

namespace Aow.Game.Combat.Actions
{

    public partial class SlowCA : ExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1257
        public new const int ClassID = 131792;

        public SlowCA(ECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:1263
        public override void Execute(global::Aow.Game.Combat.Combat Combat)
        {
            Aow.Game.Combat.CombatUnit Target;
            Aow.Game.Combat.CombatObject Source;
            UnitEnchantment Enchantment;
            base.Execute(Combat);
            if (FSucceeded)
            {
                Target = (Aow.Game.Combat.CombatUnit)Combat.FCombatData.FindID(FTargetID);
                Source = Combat.FCombatData.FindID(FSourceID);
                if (Target.FUnit.ExpandAbility(132))
                {
                    Enchantment = ((UnitEnchantmentAbilityData)Target.FUnit.GetAbilityData(132)).FEnchantment;
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
    public partial class Slow : CombatSpell
    {
        // CombatSpells.pas:1284
        public Slow(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.SlowRStr);
            FSpellID = 110;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 9;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.None);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        // CombatSpells.pas:1313
        public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
        {
            SlowCA Action;
            Action = new SlowCA(null);
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
                Action.FSucceeded = Aow.Game.Magic.Abilities.HitChance.HitRole(FCombatInfo.Attack - resistance);
            }

            return Action;
        }

        // CombatSpells.pas:1300
        public override void GetCombatDamageValueEx(CombatObject Source, CombatObject Target, ref AbilityDamageValues Values)
        {
            ref var abilityDamageValues = ref Values;
            abilityDamageValues.SourceDamage = 0;
            abilityDamageValues.SourceCost = 0;
            abilityDamageValues.TargetDamage = 0;
            abilityDamageValues.TargetCost = 0;
            abilityDamageValues.DamageTypes = Set<DamageType>.Of();
        }
    }
}

namespace Aow.Game.Combat.Actions
{

    public partial class EntangleSpellCA : ExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1328
        public new const int ClassID = 131784;

        public EntangleSpellCA(ECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:1334
        public override void Execute(global::Aow.Game.Combat.Combat Combat)
        {
            Aow.Game.Combat.CombatObject Source;
            Aow.Game.Combat.CombatUnit Target;
            UnitEnchantmentAbilityData Data;
            base.Execute(Combat);
            if (FSucceeded)
            {
                Source = Combat.FCombatData.FindID(FSourceID);
                Target = (Aow.Game.Combat.CombatUnit)Combat.FCombatData.FindID(FTargetID);
                Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(94).Expand(Target.FUnit);
                Data = (UnitEnchantmentAbilityData)Target.FUnit.GetAbilityData(94);
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
    public partial class Entangle : CombatSpell
    {
        // CombatSpells.pas:1352
        public Entangle(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.EntangleRStr);
            FSpellID = 111;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 7;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.None);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        // CombatSpells.pas:1368
        public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
        {
            EntangleSpellCA Action;
            Action = new EntangleSpellCA(null);
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
                Action.FSucceeded = Aow.Game.Magic.Abilities.HitChance.HitRole(FCombatInfo.Attack - resistance);
            }

            return Action;
        }

        // CombatSpells.pas:1383
        public override void GetCombatDamageValueEx(CombatObject Source, CombatObject Target, ref AbilityDamageValues Values)
        {
            if (!Target.GetAbilityEnabled(1) && !Target.GetAbilityEnabled(94))
            {
                var resistance = Target.GetResistance();
                var hitRoleProbability = Aow.Game.Magic.Abilities.HitChance.HitRoleProbability(FCombatInfo.Attack - resistance);
                var hitPoints = Target.GetHitPoints();
                Values.SourceDamage = Round((double)(hitPoints * 100) * hitRoleProbability);
                var targetHitPoints = Target.GetHitPoints();
                Values.SourceCost = targetHitPoints;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = Set<DamageType>.Of(DamageType.None);
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = Set<DamageType>.Of();
            }
        }
    }

    // inherited combat statistics, damage mode
    public partial class Tremors : CombatSpell
    {
        // CombatSpells.pas:1404
        public Tremors(ECustomNode owner) : base(owner)
        {
            // { Original leaves this spell name untranslated. }
            FSpellName = Aow.Game.Magic.Spells.Combat.Tremors.TremorsRStr;
            FSpellID = 112;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical, DamageType.Wall);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = -1;
            spellCombatInfo.Radius = 5;
        }

        private static readonly string TremorsRStr = "Tremors";
    }

    // inherited combat statistics, damage mode
    public partial class CallFlames : CombatSpell
    {
        // CombatSpells.pas:1421
        public CallFlames(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.CallFlames.CallFlamesRStr);
            FSpellID = 113;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 9;
            spellCombatInfo.Damage = 3;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Fire);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 8;
            spellCombatInfo.Radius = 1;
        }

        private static readonly string CallFlamesRStr = "Call Flames";
    }

    // inherited combat statistics, damage mode
    public partial class FireBreath : CombatSpell
    {
        // CombatSpells.pas:1438
        public FireBreath(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.FireBreath.FireBreathRStr);
            FSpellID = 114;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Fire);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 4;
            spellCombatInfo.Radius = 3;
        }

        private static readonly string FireBreathRStr = "Fire Breath";
    }

    // inherited combat statistics, damage mode
    public partial class Swarm : CombatSpell
    {
        // CombatSpells.pas:1455
        public Swarm(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.Swarm.SwarmRStr);
            FSpellID = 115;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 2;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Fire);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 5;
        }

        private static readonly string SwarmRStr = "Swarm";
    }

    // inherited combat statistics, damage mode
    public partial class GreatHail : CombatSpell
    {
        // CombatSpells.pas:1472
        public GreatHail(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.GreatHail.GreatHailRStr);
            FSpellID = 116;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 5;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical, DamageType.Wall);
            spellCombatInfo.RepeatCount = 3;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 2;
        }

        private static readonly string GreatHailRStr = "Great Hail";
    }

    // inherited combat statistics, damage mode
    public partial class FireBall : CombatSpell
    {
        // CombatSpells.pas:1489
        public FireBall(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.FireBall.FireBallRStr);
            FSpellID = 117;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 6;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Fire, DamageType.Wall);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        private static readonly string FireBallRStr = "Fireball";
    }

    // inherited combat statistics, damage mode
    public partial class Geyser : CombatSpell
    {
        // CombatSpells.pas:1505
        public Geyser(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.Geyser.GeyserRStr);
            FSpellID = 118;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 9;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 1;
        }

        // CombatSpells.pas:1522
        public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
        {
            CombatSpellCA Action;
            Action = new CombatSpellCA(null);
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
                Action.FManaCost = ((Hero)((CombatUnit)Source).FUnit).CastingMana(this);
            }

            return Action;
        }

        private static readonly string GeyserRStr = "Geyser";
    }

    // inherited combat statistics, damage mode
    public partial class IceShards : CombatSpell
    {
        // CombatSpells.pas:1537
        public IceShards(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.IceShards.IceShardsRStr);
            FSpellID = 119;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 4;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
            spellCombatInfo.RepeatCount = 3;
            spellCombatInfo.Range = 12;
        }

        private static readonly string IceShardsRStr = "Ice Shards";
    }

    // inherited combat statistics, damage mode
    public partial class FrostBeams : CombatSpell
    {
        // CombatSpells.pas:1553
        public FrostBeams(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.FrostBeams.FrostBeamsRStr);
            FSpellID = 120;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 9;
            spellCombatInfo.Damage = 3;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Cold);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 3;
        }

        private static readonly string FrostBeamsRStr = "Frost Beam";
    }

    // inherited combat statistics, damage mode
    public partial class Vaporize : CombatSpell
    {
        // CombatSpells.pas:1570
        public Vaporize(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.Vaporize.VaporizeRStr);
            FSpellID = 121;
            FDamageMode = 1;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 7;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        private static readonly string VaporizeRStr = "Vaporize";
    }

    // inherited combat statistics, damage mode
    public partial class FlamingArrow : CombatSpell
    {
        // CombatSpells.pas:1603
        public FlamingArrow(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.FlamingArrow.FlamingArrowRStr);
            FSpellID = 123;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 4;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Fire, DamageType.Physical);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        private static readonly string FlamingArrowRStr = "Flaming Arrow";
    }

    // inherited combat statistics, damage mode
    public partial class Stoning : CombatSpell
    {
        // CombatSpells.pas:1587
        public Stoning(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.Stoning.StoningRStr);
            FSpellID = 122;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 5;
            spellCombatInfo.Damage = 2;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
            spellCombatInfo.RepeatCount = 6;
            spellCombatInfo.Range = 12;
        }

        private static readonly string StoningRStr = "Stoning";
    }

    // inherited combat statistics, damage mode
    public partial class Ooze : CombatSpell
    {
        // CombatSpells.pas:1619
        public Ooze(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.Ooze.OozeRStr);
            FSpellID = 124;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 0;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of();
            spellCombatInfo.RepeatCount = 0;
            spellCombatInfo.Range = 8;
        }

        // CombatSpells.pas:1635
        public override void GetCombatDamageValueEx(CombatObject Source, CombatObject Target, ref AbilityDamageValues Values)
        {
            Target.GetProtectionTypes();
            Target.GetImmunityTypes();
            Values.DamageTypes = FCombatInfo.DamageTypes;
            var resistance = Target.GetResistance();
            var hitRoleProbability = Aow.Game.Magic.Abilities.HitChance.HitRoleProbability(FCombatInfo.Attack - resistance);
            Values.SourceDamage = Round((double)hitRoleProbability * 200);
            Values.SourceCost = 200;
            Values.TargetDamage = 0;
            Values.TargetCost = 0;
        }

        private static readonly string OozeRStr = "Ooze";
    }

    // inherited combat statistics, damage mode
    public partial class SacredWrath : CombatSpell
    {
        // CombatSpells.pas:1649
        public SacredWrath(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.SacredWrath.SacredWrathRStr);
            FSpellID = 125;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Holy);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 8;
            spellCombatInfo.Radius = 3;
        }

        private static readonly string SacredWrathRStr = "Sacred Wrath";
    }
}

namespace Aow.Game.Combat.Actions
{

    public partial class FastCombatTerrorCA : MultiTargetCombatSpellCA
    {
        // CombatSpells.pas:1666
        public new const int ClassID = 131777;

        public FastCombatTerrorCA(ECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:1672
        public override void Execute(global::Aow.Game.Combat.Combat Combat)
        {
            Aow.Game.Combat.CombatUnit Target;
            base.Execute(Combat);
            for (int I = FTargetIDs.Count - 1; I >= 0; I--)
            {
                var targetID = FTargetIDs[I];
                Target = (Aow.Game.Combat.CombatUnit)Combat.FCombatData.FindID(targetID);
                Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(108).Expand(Target.FUnit);
            }
        }

        // CombatSpells.pas:1686
        public void Generate(Aow.Game.Combat.CombatObject Source)
        {
            Aow.Game.Combat.CombatSide Side;
            Aow.Game.Combat.CombatObject Target;
            FSpellID = 105;
            FSourceID = Source.FID;
            var combat = Source.FCombat;
            var opponentSide = Source.GetOpponentSide();
            Side = combat.FSides[(int)opponentSide];

            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Target = Side.GetObjects(I);
                if (!Target.FStatus.Contains(Aow.Game.Combat.CombatObjectStatusElement.Destroyed) && !Target.GetAbilityEnabled(108) && !Target.GetAbilityEnabled(67) && Target is Aow.Game.Combat.CombatUnit && !Target.GetImmunityTypes().Contains(Aow.Game.Combat.DamageType.Death) && ((Aow.Game.Combat.CombatUnit)Target).FUnit.GetUnitType() != UnitType.Machine && Aow.Game.Magic.Abilities.HitChance.HitRole(6 - Target.GetResistance()))
                {
                    FTargetIDs.Add(Target.FID);
                }
            }
        }

    }

    // source ID, target ID, casting mana, success
    public partial class TacticalCombatTerrorCA : CombatAction
    {
        // CombatSpells.pas:1707
        public new const int ClassID = 131785;

        public TacticalCombatTerrorCA(ECustomNode owner) : base(owner)
        { }

        public byte FSourceID;
        public byte FTargetID;
        public int FManaCost;
        public bool FSucceeded;

        // CombatSpells.pas:1713
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref FSourceID, 20);
            Stream.rwByte(ref FTargetID, 21);
            Stream.rwInteger(ref FManaCost, 22);
            Stream.rwBool(ref FSucceeded, 23);
        }

        // CombatSpells.pas:1723
        public override void Execute(global::Aow.Game.Combat.Combat Combat)
        {
            Aow.Game.Combat.CombatUnit Target;
            Aow.Game.Combat.CombatUnit Source;
            if (FSucceeded)
            {
                Target = (Aow.Game.Combat.CombatUnit)Combat.FCombatData.FindID(FTargetID);
                Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(108).Expand(Target.FUnit);
            }

            if (FManaCost != 0 && Combat.FStatus.Contains(Aow.Game.Combat.CombatStatusElement.Executing))
            {
                Source = (Aow.Game.Combat.CombatUnit)Combat.FCombatData.FindID(FSourceID);
                Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(105).CastingDone(Source.FUnit, FManaCost);
            }
        }

        // CombatSpells.pas:1740
        public override bool Play(Aow.Game.Combat.CombatViewer Viewer)
        {
            Execute(Viewer.FCombat);
            return false;
        }

        // CombatSpells.pas:1747
        public void Generate(Aow.Game.Combat.CombatObject Source, Aow.Game.Combat.CombatObject Target)
        {
            FSucceeded = false;
            FSourceID = Source.FID;
            FTargetID = Target.FID;
            if (!Target.GetAbilityEnabled(108) && !Target.GetAbilityEnabled(67) && Target is Aow.Game.Combat.CombatUnit && !Target.GetImmunityTypes().Contains(Aow.Game.Combat.DamageType.Death) && ((Aow.Game.Combat.CombatUnit)Target).FUnit.GetUnitType() != UnitType.Machine)
            {
                FSucceeded = Aow.Game.Magic.Abilities.HitChance.HitRole(6 - Target.GetResistance());
            }
        }

    }
}

namespace Aow.Game.Magic.Spells.Combat
{

    // inherited combat statistics, damage mode
    public partial class Terror : CombatSpell
    {
        // CombatSpells.pas:1759
        public Terror(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.Terror.TerrorRStr);
            FSpellID = 105;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 6;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Death);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = -1;
        }

        // CombatSpells.pas:1775
        public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
        {
            TacticalCombatTerrorCA Action;
            Action = new TacticalCombatTerrorCA(null);
            Action.Generate(Source, Target);
            if (ChargeMana)
            {
                Action.FManaCost = ((Hero)((CombatUnit)Source).FUnit).CastingMana(this);
            }

            return Action;
        }

        // CombatSpells.pas:1862
        public override void tcGetDamageValueEx(CombatObject Source, CombatObject Target, ref AbilityDamageValues Values)
        {
            int Value;
            if (!Target.GetAbilityEnabled(108) && Target.FCombatValue != 0 && Target is CombatUnit && !Target.GetImmunityTypes().Contains(DamageType.Death))
            {
                Value = Round((double)Aow.Game.Magic.Abilities.HitChance.HitRoleProbability(6 - Target.GetResistance()) * 100);
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
                Values.DamageTypes = Set<DamageType>.Of(DamageType.Death);
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = Set<DamageType>.Of();
            }
        }

        // CombatSpells.pas:1786
        public override void fcPrefetchCombatCommands(FastCombatCommandList Commands)
        {
            CombatSide Side;
            CombatObject Target;
            int Value;
            Aow.Game.Magic.Spells.SpellCC Command;
            Value = 0;
            var opponentSide = Commands.FSource.GetOpponentSide();
            Side = Commands.FCombat.FSides[(int)opponentSide];
            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Target = Side.GetObjects(I);
                if (!Target.GetAbilityEnabled(108) && Target.FCombatValue != 0 && Target is CombatUnit && !Target.GetImmunityTypes().Contains(DamageType.Death))
                {
                    Value += Target.GetTargetPriority();
                }
            }

            if (Value != 0)
            {
                Command = new Aow.Game.Magic.Spells.SpellCC(52);
                Command.FTarget = null;
                Command.FMoveCost = 1;
                Command.FSpellID = FSpellID;
                Command.FExpectedDV = Value;
                Commands.Add(Command);
            }
        }

        // CombatSpells.pas:1815
        public override bool fcExecuteCombatCommand(CombatObject Source, FastCombatCommand Command)
        {
            FastCombatTerrorCA Action;
            Action = new FastCombatTerrorCA(null);
            Action.Generate(Source);
            Action.FManaCost = ((Hero)((CombatUnit)Source).FUnit).CastingMana(this);
            Source.FCombat.ExecuteCombatAction(Action);
            Action.Release();
            return true;
        }

        // CombatSpells.pas:1828
        public override void fcGetDamageValueEx(CombatObject Source, CombatObject Target, WallSituation WallSituation, ref AbilityDamageValues Values)
        {
            int Value;
            CombatSide Side;
            CombatObject Opponent;
            Value = 0;
            var opponentSide = Source.GetOpponentSide();
            Side = Source.FCombat.FSides[(int)opponentSide];
            for (int I = Side.GetCount() - 1; I >= 0; I--)
            {
                Opponent = Side.GetObjects(I);
                if (!Opponent.GetAbilityEnabled(108) && Opponent.FCombatValue != 0 && Opponent is CombatUnit && !Opponent.GetImmunityTypes().Contains(DamageType.Death))
                {
                    Value += Round((double)Aow.Game.Magic.Abilities.HitChance.HitRoleProbability(6 - Opponent.GetResistance()) * 100);
                }
            }

            if (Value != 0)
            {
                Values.SourceDamage = Value;
                Values.SourceCost = Value / 100;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = Set<DamageType>.Of(DamageType.Death);
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = Set<DamageType>.Of();
            }
        }

        private static readonly string TerrorRStr = "Terror";
    }

    // inherited spell ID, combat statistics, damage mode
    public partial class WindsOfFury : CombatSpell
    {
        // CombatSpells.pas:1889
        public WindsOfFury(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.WindsOfFury.WindsOfFuryRStr);
            FSpellID = 126;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        // CombatSpells.pas:1904
        public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
        {
            CombatSpellCA Action;
            Action = new CombatSpellCA(null);
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
                Action.FManaCost = ((Hero)((CombatUnit)Source).FUnit).CastingMana(this);
            }

            return Action;
        }

        private static readonly string WindsOfFuryRStr = "Winds Of Fury";
    }

    // inherited spell ID, combat statistics, damage mode
    public partial class Shockwave : CombatSpell
    {
        // CombatSpells.pas:1919
        public Shockwave(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.Shockwave.ShockwaveRStr);
            FSpellID = 127;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical, DamageType.Wall);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
        }

        private static readonly string ShockwaveRStr = "Shock Wave";
    }

    // inherited spell ID, combat statistics, damage mode
    public partial class SacrificialFlame : CombatSpell
    {
        // CombatSpells.pas:1934
        public SacrificialFlame(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.SacrificialFlame.SacrificialFlameRStr);
            FSpellID = 128;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 8;
            spellCombatInfo.Damage = 5;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Fire);
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = 12;
            spellCombatInfo.Radius = 3;
        }

        private static readonly string SacrificialFlameRStr = "Sacrificial Flame";
    }
}

namespace Aow.Game.Combat.Actions
{

    // inherited source ID, target ID, mana, success, spell ID
    public partial class RecallSpiritCA : ExclusiveCombatSpellCA
    {
        // CombatSpells.pas:1950
        public new const int ClassID = 131781;

        public RecallSpiritCA(ECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:1953
        public override void Execute(global::Aow.Game.Combat.Combat Combat)
        {
            Aow.Game.Combat.CombatUnit Source;
            Aow.Game.Combat.CombatUnit Target;
            Unit NewUnit;
            base.Execute(Combat);
            if (FSucceeded)
            {
                Source = (Aow.Game.Combat.CombatUnit)Combat.FCombatData.FindID(FSourceID);
                Target = (Aow.Game.Combat.CombatUnit)Combat.FCombatData.FindID(FTargetID);
                NewUnit = new Unit(null);
                NewUnit.SetUnitResource(Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(130));
                NewUnit.SetPlayer(Source.FUnit.FPlayer);
                NewUnit.ExpandAbility(130);
                NewUnit.Activate();
                Target.SetUnit(NewUnit);
                NewUnit.Release();
                Target.SetStatus(Target.FStatus - Set<Aow.Game.Combat.CombatObjectStatusElement>.Of(Aow.Game.Combat.CombatObjectStatusElement.Destroyed));
                Target.SetCombatPlayer((byte)Source.FCombatPlayer);
            }
        }

    }
}

namespace Aow.Game.Magic.Spells.Combat
{

    // inherited spell ID, combat statistics, damage mode
    public partial class RecallSpirit : CombatSpell
    {
        // CombatSpells.pas:1975
        public RecallSpirit(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.RecallSpirit.RecallSpiritRStr);
            FSpellID = 129;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 0;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of();
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = -1;
        }

        // CombatSpells.pas:2008
        public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
        {
            RecallSpiritCA Action;
            Action = new RecallSpiritCA(null);
            Action.FSourceID = Source.FID;
            Action.FTargetID = Target.FID;
            Action.FSucceeded = Target.FStatus.Contains(CombatObjectStatusElement.Destroyed);
            Action.FSpellID = FSpellID;
            if (ChargeMana)
            {
                Action.FManaCost = FBaseManaCost;
            }

            return Action;
        }

        // CombatSpells.pas:1990
        public override void fcGetDamageValueEx(CombatObject Source, CombatObject Target, WallSituation WallSituation, ref AbilityDamageValues Values)
        {
            if (Target.FStatus.Contains(CombatObjectStatusElement.Destroyed))
            {
                // { Original leaves the other damage-value fields unchanged on success. }
                Values.SourceDamage = 200;
                Values.DamageTypes = Set<DamageType>.Of();
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = Set<DamageType>.Of();
            }
        }

        private static readonly string RecallSpiritRStr = "Recall Spirits";
    }
}

namespace Aow.Game.Combat.Actions
{

    // inherited source ID, target ID, mana, success, spell ID
    public partial class AnimateDeadCA : ExclusiveCombatSpellCA
    {
        // CombatSpells.pas:2021
        public new const int ClassID = 131782;

        public AnimateDeadCA(ECustomNode owner) : base(owner)
        { }

        // CombatSpells.pas:2024
        public override void Execute(global::Aow.Game.Combat.Combat Combat)
        {
            Aow.Game.Combat.CombatUnit Source;
            Aow.Game.Combat.CombatUnit Target;
            Unit NewUnit;
            base.Execute(Combat);
            if (FSucceeded)
            {
                Source = (Aow.Game.Combat.CombatUnit)Combat.FCombatData.FindID(FSourceID);
                Target = (Aow.Game.Combat.CombatUnit)Combat.FCombatData.FindID(FTargetID);
                NewUnit = new Unit(null);
                NewUnit.SetUnitResource(Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(201));
                NewUnit.SetPlayer(Source.FUnit.FPlayer);
                NewUnit.ExpandAbility(130);
                NewUnit.Activate();
                Target.SetUnit(NewUnit);
                NewUnit.Release();
                Target.SetStatus(Target.FStatus - Set<Aow.Game.Combat.CombatObjectStatusElement>.Of(Aow.Game.Combat.CombatObjectStatusElement.Destroyed));
                Target.SetCombatPlayer((byte)Source.FCombatPlayer);
            }
        }

    }
}

namespace Aow.Game.Magic.Spells.Combat
{

    // inherited spell ID, combat statistics, damage mode
    public partial class AnimateDead : CombatSpell
    {
        // CombatSpells.pas:2046
        public AnimateDead(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.Combat.AnimateDead.AnimateDeadRStr);
            FSpellID = 130;
            ref var spellCombatInfo = ref FCombatInfo;
            spellCombatInfo.Attack = 0;
            spellCombatInfo.Damage = 0;
            spellCombatInfo.DamageTypes = Set<DamageType>.Of();
            spellCombatInfo.RepeatCount = 1;
            spellCombatInfo.Range = -1;
        }

        // CombatSpells.pas:2079
        public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
        {
            AnimateDeadCA Action;
            Action = new AnimateDeadCA(null);
            Action.FSourceID = Source.FID;
            Action.FTargetID = Target.FID;
            Action.FSucceeded = Target.FStatus.Contains(CombatObjectStatusElement.Destroyed);
            Action.FSpellID = FSpellID;
            if (ChargeMana)
            {
                Action.FManaCost = FBaseManaCost;
            }

            return Action;
        }

        // CombatSpells.pas:2061
        public override void fcGetDamageValueEx(CombatObject Source, CombatObject Target, WallSituation WallSituation, ref AbilityDamageValues Values)
        {
            if (Target.FStatus.Contains(CombatObjectStatusElement.Destroyed))
            {
                // { Original leaves the other damage-value fields unchanged on success. }
                Values.SourceDamage = 200;
                Values.DamageTypes = Set<DamageType>.Of();
            }
            else
            {
                Values.SourceDamage = 0;
                Values.SourceCost = 0;
                Values.TargetDamage = 0;
                Values.TargetCost = 0;
                Values.DamageTypes = Set<DamageType>.Of();
            }
        }

        private static readonly string AnimateDeadRStr = "Animate Dead";
    }
}
