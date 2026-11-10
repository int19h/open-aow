// Translated from UnitSpells.pas.
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat.Actions
{

    // spell ID, mana cost charged after execution
    public partial class DispelMagicSpellCA : DispelMagicCA
    {
        // UnitSpells.pas:975
        public new const int ClassID = 131793;

        public DispelMagicSpellCA(ECustomNode owner) : base(owner)
        { }

        public int FSpellID;
        public int FManaCost;

        // UnitSpells.pas:980
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FSpellID, 60);
            Stream.rwInteger(ref FManaCost, 61);
        }

        // UnitSpells.pas:987
        public override void Execute(Combat Combat)
        {
            CombatUnit Source;
            base.Execute(Combat);
            if (FManaCost != 0 && Combat.FStatus.Contains(CombatStatusElement.Executing))
            {
                if (Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID) != null)
                {
                    Source = (CombatUnit)Combat.FCombatData.FindID(FSourceID);
                    Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CastingDone(Source.FUnit, FManaCost);
                }
            }
        }

    }
}

namespace Aow.Game.Magic.Spells.Unit
{

    // inherited multiple targets, target range, dispel resistance, AI value
    public partial class DispelMagic : UnitSpell
    {
        // UnitSpells.pas:1000
        public DispelMagic(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.DispelMagicRStr);
            FSpellID = 8;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Cosmos;
            FTargetRange = 4;
            FAllowMultipleTargets = true;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        }

        // UnitSpells.pas:1023
        public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
        {
            DispelMagicSpellCA Action;
            Action = new DispelMagicSpellCA(null);
            Action.FSpellID = FSpellID;
            Action.FSourceID = Source.FID;
            Action.FTargetID = Target.FID;
            Action.Generate(Source, Target, GetDispelMana(((CombatUnit)Source).FUnit));
            if (ChargeMana)
            {
                Action.FManaCost = FBaseManaCost;
            }

            return Action;
        }

        // UnitSpells.pas:1074
        public override void GetCombatInfo(AbstractUnit Source, ref SpellCombatInfo Info)
        {
            base.GetCombatInfo(Source, ref Info);
            Info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Friend, CombatTargetTypeElement.Enemy);
        }

        // UnitSpells.pas:1068
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && Target.CanDispelEnchantment(Source.FPlayer);
        }

        // UnitSpells.pas:1018
        public override bool ValidTargetParty(AbstractUnit Source, ArmyHS Party) => Party.FArmy.FPlayer == Source.FPlayer;

        // UnitSpells.pas:1036
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            int Count;
            EnchantmentList Enchantments;
            int Chance;
            if (ValidTargetUnit(Source, Target))
            {
                Count = 0;
                Enchantments = new EnchantmentList(null);
                Target.ListEnchantments(Enchantments);
                for (int I = Enchantments.GetCount() - 1; I >= 0; I--)
                {
                    var dispelMana = GetDispelMana(Source);
                    Chance = Enchantments.GetEnchantments(I).DispelChance(dispelMana);
                    if (Chance > Aow.Game.World.Map.AoWHSMap.Current.Random(100))
                    {
                        Count += 1;
                        Enchantments.GetEnchantments(I).Dispel();
                    }
                }

                if (Source.FPlayer == World.Map.AoWHSMap.Current.FSeatedPlayer)
                {
                    if (Count > 0)
                    {
                        var format = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.XSpellsSuccessfullyDispelledRStr), [IntToStr(Count)]);
                        Aow.Game.World.Map.AoWHSMap.Current.ShowMessage(format);
                    }
                    else
                    {
                        var dispelMagicFailed = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.DispelMagicFailedRStr);
                        Aow.Game.World.Map.AoWHSMap.Current.ShowMessage(dispelMagicFailed);
                    }
                }

                Result = Enchantments.GetCount() > 0;
                Enchantments?.Dispose();
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // UnitSpells.pas:1013
        public int GetDispelMana(AbstractUnit Source) => Source.GetAbLevel(52) * 15 + 25;
    }

    public partial class Haste : UnitSpell
    {
        // UnitSpells.pas:215
        public Haste(ECustomNode owner) : base(owner)
        {
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.HasteRStr);
            FSpellID = 15;
            FBaseManaCost = 5;
            FManaUpkeep = 2;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Air;
            FTargetRange = 12;
            FUnitDispelResistance = 3;
        }

        // UnitSpells.pas:250
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(152);
        }

        // UnitSpells.pas:229
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            UnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(152))
                {
                    Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(152)).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(Source.FPlayer);
                    Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    public partial class StoneSkin : UnitSpell
    {
        // UnitSpells.pas:256
        public StoneSkin(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.StoneSkinRStr);
            FSpellID = 17;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Earth;
            FTargetRange = 12;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
            FUnitDispelResistance = 3;
            FAIValue = 200;
            FCastDuration = 5;
        }

        // UnitSpells.pas:290
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(153);
        }

        // UnitSpells.pas:271
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            UnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(153))
                {
                    Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(153)).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(Source.FPlayer);
                    Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
                }

                // { Original succeeds even when expansion fails. }
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    public partial class EnchantWeapon : UnitSpell
    {
        // UnitSpells.pas:296
        public EnchantWeapon(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(EnchantWeaponRStr);
            FSpellID = 16;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Cosmos;
            FTargetRange = 12;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
            FUnitDispelResistance = 3;
            FAIValue = 225;
            FCastDuration = 5;
        }

        // UnitSpells.pas:330
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(154) && Target.GetAbilityEnabled(21);
        }

        // UnitSpells.pas:311
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            UnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(154))
                {
                    Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(154)).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(Source.FPlayer);
                    Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
                }

                // { Original succeeds even when expansion fails. }
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        private static readonly string EnchantWeaponRStr = "Enchant Weapon";
    }

    public partial class FreeMovement : UnitSpell
    {
        // UnitSpells.pas:386
        public FreeMovement(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.FreeMovementRStr);
            FSpellID = 32;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = MagicSphere.Earth;
            FTargetRange = 12;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
            FUnitDispelResistance = 4;
        }

        // UnitSpells.pas:418
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(157) && (!Target.GetAbilityEnabled(3) || !Target.GetAbilityEnabled(5) || !Target.GetAbilityEnabled(4));
        }

        // UnitSpells.pas:399
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            UnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(157))
                {
                    Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(157)).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(Source.FPlayer);
                    Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
                }

                // { Original succeeds even when expansion fails. }
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    public partial class Fury : UnitSpell
    {
        // UnitSpells.pas:336
        public Fury(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.FuryRStr);
            FSpellID = 33;
            FAIValue = 175;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Fire;
            FTargetRange = 12;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
            FUnitDispelResistance = 3;
        }

        // UnitSpells.pas:369
        public override void PlayDefaultCastEffects(sbyte Player, sbyte X, sbyte Y, sbyte Layer)
        {
            base.PlayDefaultCastEffects(Player, X, Y, Layer);
            if (Player == World.Map.AoWHSMap.Current.FSeatedPlayer || Aow.Game.World.Map.AoWHSMap.Current.Watching(X, Y, Layer, 0))
            {
                Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(1);
            }
        }

        // UnitSpells.pas:376
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            Result = Result && !Target.GetAbilityEnabled(156);
            if (Result)
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Target.FPlayer).FPlayerType != PlayerType.Human && Target is Leader)
                {
                    Result = false;
                }
            }

            return Result;
        }

        // UnitSpells.pas:350
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            UnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(156))
                {
                    Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(156)).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(Source.FPlayer);
                    Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
                }

                // { Original succeeds even when expansion fails. }
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    public partial class Remedy : UnitSpell
    {
        // UnitSpells.pas:424
        public Remedy(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(RemedyRStr);
            FSpellID = 35;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Cosmos;
            FTargetRange = 12;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        }

        // UnitSpells.pas:458
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && (Target.GetHitPoints() != Target.GetHits() || Target.GetAbilityEnabled(96) || Target.GetAbilityEnabled(92) || Target.GetAbilityEnabled(108) || Target.GetAbilityEnabled(53) || Target.GetAbilityEnabled(93) || Target.GetAbilityEnabled(127));
        }

        // UnitSpells.pas:436
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            int HitPoints;
            if (ValidTargetUnit(Source, Target))
            {
                HitPoints = Target.GetHitPoints() + 5;
                if (HitPoints > Target.GetHits())
                {
                    HitPoints = Target.GetHits();
                }

                Target.SetHitPoints((sbyte)HitPoints);
                Target.RemoveAbility(93);
                Target.RemoveAbility(96);
                Target.RemoveAbility(108);
                Target.RemoveAbility(53);
                Target.RemoveAbility(92);
                Target.RemoveAbility(127);
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        private static readonly string RemedyRStr = "Remedy";
    }

    public partial class HealingWater : UnitSpell
    {
        // UnitSpells.pas:467
        public HealingWater(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(HealingWaterRStr);
            FSpellID = 55;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Cosmos;
            FTargetRange = 12;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        }

        // UnitSpells.pas:491
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && (Target.GetHitPoints() != Target.GetHits() || Target.GetAbilityEnabled(127));
        }

        // UnitSpells.pas:479
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            if (ValidTargetUnit(Source, Target))
            {
                Target.RemoveAbility(127);
                Target.SetHitPoints(Target.GetHits());
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        private static readonly string HealingWaterRStr = "Healing Water";
    }

    public partial class HolyChampion : UnitSpell
    {
        // UnitSpells.pas:497
        public HolyChampion(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.HolyChampionRStr);
            FSpellID = 36;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = MagicSphere.Life;
            FTargetRange = 12;
            FUnitDispelResistance = 4;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        }

        // UnitSpells.pas:529
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(146) && !Target.GetAbilityEnabled(160) && Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(146).CanExpand(Target);
        }

        // UnitSpells.pas:510
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            UnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(160))
                {
                    Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(160)).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(Source.FPlayer);
                    Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
                }

                // { Original succeeds even when expansion fails. }
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    public partial class UnholyChampion : UnitSpell
    {
        // UnitSpells.pas:537
        public UnholyChampion(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.UnholyChampionRStr);
            FSpellID = 37;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = MagicSphere.Death;
            FTargetRange = 12;
            FUnitDispelResistance = 4;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        }

        // UnitSpells.pas:569
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(147) && !Target.GetAbilityEnabled(161) && Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(147).CanExpand(Target);
        }

        // UnitSpells.pas:550
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            UnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(161))
                {
                    Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(161)).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(Source.FPlayer);
                    Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
                }

                // { Original succeeds even when expansion fails. }
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    public partial class Concealment : UnitSpell
    {
        // UnitSpells.pas:577
        public Concealment(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.ConcealmentRStr);
            FSpellID = 39;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = MagicSphere.Earth;
            FTargetRange = 12;
            FUnitDispelResistance = 5;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global);
        }

        // UnitSpells.pas:609
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(145) && !Target.GetAbilityEnabled(162) && !Target.GetAbilityEnabled(54);
        }

        // UnitSpells.pas:590
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            UnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(162))
                {
                    Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(162)).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(Source.FPlayer);
                    Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
                }

                // { Original succeeds even when expansion fails. }
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    public partial class FireAura : UnitSpell
    {
        // UnitSpells.pas:616
        public FireAura(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.FireAuraRStr);
            FSpellID = 40;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = MagicSphere.Fire;
            FTargetRange = 12;
            FUnitDispelResistance = 4;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        }

        // UnitSpells.pas:629
        public override void PlayDefaultCastEffects(sbyte Player, sbyte X, sbyte Y, sbyte Layer)
        {
            base.PlayDefaultCastEffects(Player, X, Y, Layer);
            if (Player == World.Map.AoWHSMap.Current.FSeatedPlayer || Aow.Game.World.Map.AoWHSMap.Current.Watching(X, Y, Layer, 0))
            {
                Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(1);
            }
        }

        // UnitSpells.pas:655
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && (!Target.GetAbilityEnabled(7) || !Target.GetAbilityEnabled(43)) && !Target.GetAbilityEnabled(163);
        }

        // UnitSpells.pas:636
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            UnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(163))
                {
                    Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(163)).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(Source.FPlayer);
                    Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
                }

                // { Original succeeds even when expansion fails. }
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    public partial class WaterWalking : UnitSpell
    {
        // UnitSpells.pas:662
        public WaterWalking(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.WaterWalkingRStr);
            FSpellID = 42;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = MagicSphere.Water;
            FTargetRange = 12;
            FUnitDispelResistance = 6;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        }

        // UnitSpells.pas:694
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(23) && !Target.GetAbilityEnabled(2) && !Target.GetAbilityEnabled(164) && !Target.GetAbilityEnabled(1) && !Target.GetAbilityEnabled(59);
        }

        // UnitSpells.pas:675
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            UnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(164))
                {
                    Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(164)).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(Source.FPlayer);
                    Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
                }

                // { Original succeeds even when expansion fails. }
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    public partial class WindWalking : UnitSpell
    {
        // UnitSpells.pas:702
        public WindWalking(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.WindWalkingRStr);
            FSpellID = 43;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = MagicSphere.Air;
            FTargetRange = 12;
            FUnitDispelResistance = 6;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        }

        // UnitSpells.pas:734
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(1) && !Target.GetAbilityEnabled(165) && !Target.GetAbilityEnabled(59);
        }

        // UnitSpells.pas:715
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            UnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(165))
                {
                    Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(165)).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(Source.FPlayer);
                    Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
                }

                // { Original succeeds even when expansion fails. }
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    public partial class LiquidForm : UnitSpell
    {
        // UnitSpells.pas:741
        public LiquidForm(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.LiquidFormRStr);
            FSpellID = 49;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = MagicSphere.Water;
            FAIValue = 300;
            FTargetRange = 12;
            FUnitDispelResistance = 5;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        }

        // UnitSpells.pas:783
        public override void PlayDefaultCastEffects(sbyte Player, sbyte X, sbyte Y, sbyte Layer)
        {
            base.PlayDefaultCastEffects(Player, X, Y, Layer);
            if (Player == World.Map.AoWHSMap.Current.FSeatedPlayer || Aow.Game.World.Map.AoWHSMap.Current.Watching(X, Y, Layer, 0))
            {
                Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(1);
            }
        }

        // UnitSpells.pas:774
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            Result = Result && !Target.GetAbilityEnabled(166);
            return Result && (!Target.GetAbilityEnabled(13) && !Target.GetAbilityEnabled(77) || !Target.GetAbilityEnabled(2) && !Target.GetAbilityEnabled(1) && !Target.GetAbilityEnabled(59));
        }

        // UnitSpells.pas:755
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            UnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(166))
                {
                    Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(166)).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(Source.FPlayer);
                    Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
                }

                // { Original succeeds even when expansion fails. }
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    public partial class Bless : UnitSpell
    {
        // UnitSpells.pas:790
        public Bless(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(BlessRStr);
            FSpellID = 56;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = MagicSphere.Water;
            FTargetRange = 12;
            FUnitDispelResistance = 3;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
            FAIValue = 225;
            FCastDuration = 5;
        }

        // UnitSpells.pas:824
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(158);
        }

        // UnitSpells.pas:805
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            UnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(158))
                {
                    Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(158)).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(Source.FPlayer);
                    Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
                }

                // { Original succeeds even when expansion fails. }
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        private static readonly string BlessRStr = "Bless";
    }

    public partial class DarkGift : UnitSpell
    {
        // UnitSpells.pas:830
        public DarkGift(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.DarkGiftRStr);
            FSpellID = 77;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = MagicSphere.Death;
            FTargetRange = 12;
            FUnitDispelResistance = 3;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
            FAIValue = 225;
            FCastDuration = 5;
        }

        // UnitSpells.pas:864
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(169);
        }

        // UnitSpells.pas:845
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            UnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(169))
                {
                    Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(169)).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(Source.FPlayer);
                    Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
                }

                // { Original succeeds even when expansion fails. }
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    // replacement face index, target unit ID
    public partial class CosmeticSurgerySpellCaster : SpellCaster
    {
        public CosmeticSurgerySpellCaster(ECustomNode owner) : base(owner)
        { }

        public int FFaceIndex;
        public int FTargetUnitID;

        // UnitSpells.pas:870
        public override bool CanCast(ref string Reason) => base.CanCast(ref Reason);

        // UnitSpells.pas:875
        public override bool Cast()
        {
            bool Result;
            AbstractUnit Source;
            AbstractUnit Target;
            UnitSpellTE Event;
            string Reason = "";
            ((UnitSpell)Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(59)).ResetTargetSelection();
            Source = Aow.Game.World.Map.AoWHSMap.Current.FUnitControl.FindUnit(FCasterSourceID);
            Target = Aow.Game.World.Map.AoWHSMap.Current.FUnitControl.FindUnit(FTargetUnitID);
            if (Source != null && Target != null && CanCast(ref Reason) && !Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new UnitSpellTE(null);
                var spell = Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(59);
                Event.Initialize(FCasterSpellID, Source, null, spell.FBaseManaCost);
                Event.FTargetUnitIDs.Add(FTargetUnitID);
                Event.FSpellData[0] = FFaceIndex;
                Event.FTargetPlayer = (byte)Target.FPlayer;
                Aow.Game.World.Map.AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

    }

    public partial class CosmeticSurgery : UnitSpell
    {
        // UnitSpells.pas:901
        public CosmeticSurgery(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(DeformFaceRStr);
            FSpellID = 59;
            FSpellType = SpellType.Unit;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Secret;
            FAllowMultipleTargets = false;
            FUnitDispelResistance = 10;
            FTargetRange = -1;
            FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global);
        }

        // UnitSpells.pas:968
        public override void GetCombatInfo(AbstractUnit Source, ref SpellCombatInfo Info)
        {
            // { The original saves Self and Source as well as Info; this compiler saves only Info. }
            base.GetCombatInfo(Source, ref Info);
            Info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
        }

        // UnitSpells.pas:940
        public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(Target.GetUnitType());
        }

        // UnitSpells.pas:946
        public override bool CastSpell(AbstractUnit Source, UnitList Targets)
        {
            bool Result;
            CosmeticSurgerySpellCaster Caster;
            if (Targets.GetCount() == 1)
            {
                Caster = new CosmeticSurgerySpellCaster(null);
                Caster.FCasterSourceID = Source.FUnitID;
                Caster.FCasterSpellID = FSpellID;
                Caster.FTargetUnitID = Targets.GetUnit(0).FUnitID;
                if (Targets.GetUnit(0) is Hero)
                {
                    Caster.FFaceIndex = ((Hero)Targets.GetUnit(0)).FFaceIndex;
                }
                else
                {
                    Caster.FFaceIndex = 0;
                }

                Aow.Game.World.Map.AoWHSMap.Current.FCosmeticSurgeryEventsStore.Raise(Caster);
                Caster.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // UnitSpells.pas:915
        public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            CosmeticSurgeryEnchantment Enchantment;
            bool Expanded;
            if (ValidTargetUnit(Source, Target))
            {
                if (!Target.GetAbilityEnabled(126))
                {
                    Expanded = Target.ExpandAbility(126);
                }
                else
                {
                    Expanded = true;
                }

                if (Expanded)
                {
                    Enchantment = (CosmeticSurgeryEnchantment)((UnitEnchantmentAbilityData)Target.GetAbilityData(126)).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.FFaceIndex = Data[0];
                    Enchantment.SetSource(Source.FPlayer);
                }

                // { Original succeeds even when expansion fails. }
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        private static readonly string DeformFaceRStr = "Cosmagic Surgery";
    }
}
