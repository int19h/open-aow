// Translated from UnitSpells.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.UnitSpells
{
    // spell ID, mana cost charged after execution
    public partial class TDispelMagicSpellCA : TDispelMagicCA
    {
        // UnitSpells.pas:975
        public new const int ClassID = 131793;

        public TDispelMagicSpellCA(TECustomNode owner) : base(owner)
        { }

        public int FSpellID;
        public int FManaCost;
        // UnitSpells.pas:980
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FSpellID, 60);
            Stream.rwInteger(ref FManaCost, 61);
        }

        // UnitSpells.pas:987
        public override void Execute(TCombat A1)
        {
            TCombatUnit Source;
            base.Execute(A1);
            if (FManaCost != 0 && A1.FStatus.Contains(TCombatStatusElement.cExecuting))
            {
                if (AoWHSSet.FSpellControl.GetSpell(FSpellID) != null)
                {
                    Source = (TCombatUnit)A1.FCombatData.FindID(FSourceID);
                    AoWHSSet.FSpellControl.GetSpell(FSpellID).CastingDone(Source.FUnit, FManaCost);
                }
            }
        }


    }

    // inherited multiple targets, target range, dispel resistance, AI value
    public partial class TDispelMagic : TUnitSpell
    {
        // UnitSpells.pas:1000
        public TDispelMagic(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(DispelMagicRStr);
            FSpellID = 8;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msCosmos;
            FTargetRange = 4;
            FAllowMultipleTargets = true;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:1023
        public override TCombatAction CreateCA(TCombatObject A1, TCombatObject A2, bool A3)
        {
            TDispelMagicSpellCA Action;
            Action = new TDispelMagicSpellCA(null);
            Action.FSpellID = FSpellID;
            Action.FSourceID = A1.FID;
            Action.FTargetID = A2.FID;
            Action.Generate(A1, A2, GetDispelMana(((TCombatUnit)A1).FUnit));
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            return Action;
        }

        // UnitSpells.pas:1074
        public override void GetCombatInfo(TAbstractUnit Source, ref TSpellCombatInfo Info)
        {
            base.GetCombatInfo(Source, ref Info);
            Info.TargetTypes = TSet<TCombatTargetTypeElement>.Of(TCombatTargetTypeElement.cttFriend, TCombatTargetTypeElement.cttEnemy);
        }

        // UnitSpells.pas:1068
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && Target.CanDispelEnchantment(Source.FPlayer);
        }

        // UnitSpells.pas:1018
        public override bool ValidTargetParty(TAbstractUnit Source, TArmyHS Party) => Party.FArmy.FPlayer == Source.FPlayer;

        // UnitSpells.pas:1036
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            int Count;
            TEnchantmentList Enchantments;
            int Chance;
            if (ValidTargetUnit(Source, Target))
            {
                Count = 0;
                Enchantments = new TEnchantmentList(null);
                Target.ListEnchantments(Enchantments);
                for (int I = Enchantments.GetCount() - 1; I >= 0; I--)
                {
                    var dispelMana = GetDispelMana(Source);
                    Chance = Enchantments.GetEnchantments(I).DispelChance(dispelMana);
                    if (Chance > AoWHSMap.Random(100))
                    {
                        Count += 1;
                        Enchantments.GetEnchantments(I).Dispel();
                    }
                }

                if (Source.FPlayer == AoWHSMap.FSeatedPlayer)
                {
                    if (Count > 0)
                    {
                        var format = Rtl.Rtl.Format(TranslateRStr(XSpellsSuccessfullyDispelledRStr), [IntToStr(Count)]);
                        AoWHSMap.ShowMessage(format);
                    }
                    else
                    {
                        var dispelMagicFailed = TranslateRStr(DispelMagicFailedRStr);
                        AoWHSMap.ShowMessage(dispelMagicFailed);
                    }
                }

                Result = Enchantments.GetCount() > 0;
                Enchantments.Free();
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // UnitSpells.pas:1013
        public int GetDispelMana(TAbstractUnit Source) => Source.GetAbLevel(52) * 15 + 25;
    }

    public partial class THaste : TUnitSpell
    {
        // UnitSpells.pas:215
        public THaste(TECustomNode owner) : base(owner)
        {
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
            FSpellName = TranslateRStr(HasteRStr);
            FSpellID = 15;
            FBaseManaCost = 5;
            FManaUpkeep = 2;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msAir;
            FTargetRange = 12;
            FUnitDispelResistance = 3;
        }

        // UnitSpells.pas:250
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(152);
        }

        // UnitSpells.pas:229
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            TUnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(152))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.GetAbilityData(152)).FEnchantment;
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

    public partial class TStoneSkin : TUnitSpell
    {
        // UnitSpells.pas:256
        public TStoneSkin(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(StoneSkinRStr);
            FSpellID = 17;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msEarth;
            FTargetRange = 12;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
            FUnitDispelResistance = 3;
            FAIValue = 200;
            FCastDuration = 5;
        }

        // UnitSpells.pas:290
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(153);
        }

        // UnitSpells.pas:271
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            TUnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(153))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.GetAbilityData(153)).FEnchantment;
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

    public partial class TEnchantWeapon : TUnitSpell
    {
        // UnitSpells.pas:296
        public TEnchantWeapon(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(EnchantWeaponRStr);
            FSpellID = 16;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msCosmos;
            FTargetRange = 12;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
            FUnitDispelResistance = 3;
            FAIValue = 225;
            FCastDuration = 5;
        }

        // UnitSpells.pas:330
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(154) && Target.GetAbilityEnabled(21);
        }

        // UnitSpells.pas:311
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            TUnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(154))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.GetAbilityData(154)).FEnchantment;
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

    public partial class TFreeMovement : TUnitSpell
    {
        // UnitSpells.pas:386
        public TFreeMovement(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(FreeMovementRStr);
            FSpellID = 32;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = TMagicSphere.msEarth;
            FTargetRange = 12;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
            FUnitDispelResistance = 4;
        }

        // UnitSpells.pas:418
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(157) && (!Target.GetAbilityEnabled(3) || !Target.GetAbilityEnabled(5) || !Target.GetAbilityEnabled(4));
        }

        // UnitSpells.pas:399
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            TUnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(157))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.GetAbilityData(157)).FEnchantment;
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

    public partial class TFury : TUnitSpell
    {
        // UnitSpells.pas:336
        public TFury(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(FuryRStr);
            FSpellID = 33;
            FAIValue = 175;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msFire;
            FTargetRange = 12;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
            FUnitDispelResistance = 3;
        }

        // UnitSpells.pas:369
        public override void PlayDefaultCastEffects(sbyte Player, sbyte X, sbyte Y, sbyte Layer)
        {
            base.PlayDefaultCastEffects(Player, X, Y, Layer);
            if (Player == AoWHSMap.FSeatedPlayer || AoWHSMap.Watching(X, Y, Layer, 0))
            {
                AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(1);
            }
        }

        // UnitSpells.pas:376
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            Result = Result && !Target.GetAbilityEnabled(156);
            if (Result)
            {
                if (AoWHSMap.FPlayers.GetPlayers(Target.FPlayer).FPlayerType != TPlayerType.ptHuman && Target is TLeader)
                {
                    Result = false;
                }
            }

            return Result;
        }

        // UnitSpells.pas:350
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            TUnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(156))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.GetAbilityData(156)).FEnchantment;
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

    public partial class TRemedy : TUnitSpell
    {
        // UnitSpells.pas:424
        public TRemedy(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(RemedyRStr);
            FSpellID = 35;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msCosmos;
            FTargetRange = 12;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:458
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && (Target.GetHitPoints() != Target.GetHits() || Target.GetAbilityEnabled(96) || Target.GetAbilityEnabled(92) || Target.GetAbilityEnabled(108) || Target.GetAbilityEnabled(53) || Target.GetAbilityEnabled(93) || Target.GetAbilityEnabled(127));
        }

        // UnitSpells.pas:436
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
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
    }

    public partial class THealingWater : TUnitSpell
    {
        // UnitSpells.pas:467
        public THealingWater(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(HealingWaterRStr);
            FSpellID = 55;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msCosmos;
            FTargetRange = 12;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:491
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && (Target.GetHitPoints() != Target.GetHits() || Target.GetAbilityEnabled(127));
        }

        // UnitSpells.pas:479
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
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
    }

    public partial class THolyChampion : TUnitSpell
    {
        // UnitSpells.pas:497
        public THolyChampion(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(HolyChampionRStr);
            FSpellID = 36;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = TMagicSphere.msLife;
            FTargetRange = 12;
            FUnitDispelResistance = 4;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:529
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(146) && !Target.GetAbilityEnabled(160) && AoWHSSet.FAbilityControl.GetAbility(146).CanExpand(Target);
        }

        // UnitSpells.pas:510
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            TUnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(160))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.GetAbilityData(160)).FEnchantment;
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

    public partial class TUnholyChampion : TUnitSpell
    {
        // UnitSpells.pas:537
        public TUnholyChampion(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(UnholyChampionRStr);
            FSpellID = 37;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = TMagicSphere.msDeath;
            FTargetRange = 12;
            FUnitDispelResistance = 4;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:569
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(147) && !Target.GetAbilityEnabled(161) && AoWHSSet.FAbilityControl.GetAbility(147).CanExpand(Target);
        }

        // UnitSpells.pas:550
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            TUnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(161))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.GetAbilityData(161)).FEnchantment;
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

    public partial class TConcealment : TUnitSpell
    {
        // UnitSpells.pas:577
        public TConcealment(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(ConcealmentRStr);
            FSpellID = 39;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msEarth;
            FTargetRange = 12;
            FUnitDispelResistance = 5;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal);
        }

        // UnitSpells.pas:609
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(145) && !Target.GetAbilityEnabled(162) && !Target.GetAbilityEnabled(54);
        }

        // UnitSpells.pas:590
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            TUnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(162))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.GetAbilityData(162)).FEnchantment;
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

    public partial class TFireAura : TUnitSpell
    {
        // UnitSpells.pas:616
        public TFireAura(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(FireAuraRStr);
            FSpellID = 40;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = TMagicSphere.msFire;
            FTargetRange = 12;
            FUnitDispelResistance = 4;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:629
        public override void PlayDefaultCastEffects(sbyte Player, sbyte X, sbyte Y, sbyte Layer)
        {
            base.PlayDefaultCastEffects(Player, X, Y, Layer);
            if (Player == AoWHSMap.FSeatedPlayer || AoWHSMap.Watching(X, Y, Layer, 0))
            {
                AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(1);
            }
        }

        // UnitSpells.pas:655
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && (!Target.GetAbilityEnabled(7) || !Target.GetAbilityEnabled(43)) && !Target.GetAbilityEnabled(163);
        }

        // UnitSpells.pas:636
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            TUnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(163))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.GetAbilityData(163)).FEnchantment;
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

    public partial class TWaterWalking : TUnitSpell
    {
        // UnitSpells.pas:662
        public TWaterWalking(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(WaterWalkingRStr);
            FSpellID = 42;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = TMagicSphere.msWater;
            FTargetRange = 12;
            FUnitDispelResistance = 6;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:694
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(23) && !Target.GetAbilityEnabled(2) && !Target.GetAbilityEnabled(164) && !Target.GetAbilityEnabled(1) && !Target.GetAbilityEnabled(59);
        }

        // UnitSpells.pas:675
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            TUnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(164))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.GetAbilityData(164)).FEnchantment;
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

    public partial class TWindWalking : TUnitSpell
    {
        // UnitSpells.pas:702
        public TWindWalking(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(WindWalkingRStr);
            FSpellID = 43;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msAir;
            FTargetRange = 12;
            FUnitDispelResistance = 6;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:734
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(1) && !Target.GetAbilityEnabled(165) && !Target.GetAbilityEnabled(59);
        }

        // UnitSpells.pas:715
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            TUnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(165))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.GetAbilityData(165)).FEnchantment;
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

    public partial class TLiquidForm : TUnitSpell
    {
        // UnitSpells.pas:741
        public TLiquidForm(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(LiquidFormRStr);
            FSpellID = 49;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msWater;
            FAIValue = 300;
            FTargetRange = 12;
            FUnitDispelResistance = 5;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:783
        public override void PlayDefaultCastEffects(sbyte Player, sbyte X, sbyte Y, sbyte Layer)
        {
            base.PlayDefaultCastEffects(Player, X, Y, Layer);
            if (Player == AoWHSMap.FSeatedPlayer || AoWHSMap.Watching(X, Y, Layer, 0))
            {
                AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(1);
            }
        }

        // UnitSpells.pas:774
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            Result = Result && !Target.GetAbilityEnabled(166);
            return Result && (!Target.GetAbilityEnabled(13) && !Target.GetAbilityEnabled(77) || !Target.GetAbilityEnabled(2) && !Target.GetAbilityEnabled(1) && !Target.GetAbilityEnabled(59));
        }

        // UnitSpells.pas:755
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            TUnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(166))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.GetAbilityData(166)).FEnchantment;
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

    public partial class TBless : TUnitSpell
    {
        // UnitSpells.pas:790
        public TBless(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(BlessRStr);
            FSpellID = 56;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msWater;
            FTargetRange = 12;
            FUnitDispelResistance = 3;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
            FAIValue = 225;
            FCastDuration = 5;
        }

        // UnitSpells.pas:824
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(158);
        }

        // UnitSpells.pas:805
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            TUnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(158))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.GetAbilityData(158)).FEnchantment;
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

    public partial class TDarkGift : TUnitSpell
    {
        // UnitSpells.pas:830
        public TDarkGift(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(DarkGiftRStr);
            FSpellID = 77;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msDeath;
            FTargetRange = 12;
            FUnitDispelResistance = 3;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat);
            FAIValue = 225;
            FCastDuration = 5;
        }

        // UnitSpells.pas:864
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && !Target.GetAbilityEnabled(169);
        }

        // UnitSpells.pas:845
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            TUnitEnchantment Enchantment;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(169))
                {
                    Enchantment = ((TUnitEnchantmentAbilityData)Target.GetAbilityData(169)).FEnchantment;
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
    public partial class TCosmeticSurgerySpellCaster : TSpellCaster
    {
        public TCosmeticSurgerySpellCaster(TECustomNode owner) : base(owner)
        { }

        public int FFaceIndex;
        public int FTargetUnitID;
        // UnitSpells.pas:870
        public override bool CanCast(ref AnsiString Reason) => base.CanCast(ref Reason);

        // UnitSpells.pas:875
        public override bool Cast()
        {
            bool Result;
            TAbstractUnit Source;
            TAbstractUnit Target;
            TUnitSpellTE Event;
            AnsiString Reason = default;
            ((TUnitSpell)AoWHSSet.FSpellControl.GetSpell(59)).ResetTargetSelection();
            Source = AoWHSMap.FUnitControl.FindUnit(FCasterSourceID);
            Target = AoWHSMap.FUnitControl.FindUnit(FTargetUnitID);
            if (Source != null && Target != null && CanCast(ref Reason) && !AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new TUnitSpellTE(null);
                var spell = AoWHSSet.FSpellControl.GetSpell(59);
                Event.Initialize(FCasterSpellID, Source, null, spell.FBaseManaCost);
                Event.FTargetUnitIDs.Add(FTargetUnitID);
                Event.FSpellData[0] = FFaceIndex;
                Event.FTargetPlayer = (byte)Target.FPlayer;
                AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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

    public partial class TCosmeticSurgery : TUnitSpell
    {
        // UnitSpells.pas:901
        public TCosmeticSurgery(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(DeformFaceRStr);
            FSpellID = 59;
            FSpellType = TSpellType.stUnit;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msSecret;
            FAllowMultipleTargets = false;
            FUnitDispelResistance = 10;
            FTargetRange = -1;
            FSpellControlTypes = TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal);
        }

        // UnitSpells.pas:968
        public override void GetCombatInfo(TAbstractUnit Source, ref TSpellCombatInfo Info)
        {
            // { The original saves Self and Source as well as Info; this compiler saves only Info. }
            base.GetCombatInfo(Source, ref Info);
            Info.TargetTypes = TSet<TCombatTargetTypeElement>.Of(TCombatTargetTypeElement.cttEnemy);
        }

        // UnitSpells.pas:940
        public override bool ValidTargetUnit(TAbstractUnit Source, TAbstractUnit Target)
        {
            bool Result = base.ValidTargetUnit(Source, Target);
            return Result && TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature).Contains(Target.GetUnitType());
        }

        // UnitSpells.pas:946
        public override bool CastSpell(TAbstractUnit Source, TUnitList Targets)
        {
            bool Result;
            TCosmeticSurgerySpellCaster Caster;
            if (Targets.GetCount() == 1)
            {
                Caster = new TCosmeticSurgerySpellCaster(null);
                Caster.FCasterSourceID = Source.FUnitID;
                Caster.FCasterSpellID = FSpellID;
                Caster.FTargetUnitID = Targets.GetUnit(0).FUnitID;
                if (Targets.GetUnit(0) is THero)
                {
                    Caster.FFaceIndex = ((THero)Targets.GetUnit(0)).FFaceIndex;
                }
                else
                {
                    Caster.FFaceIndex = 0;
                }

                AoWHSMap.FCosmeticSurgeryEvents.TriggerNotifyEvents(Caster);
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
        public override bool ExecuteSpell(TAbstractUnit Source, TAbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
        {
            bool Result;
            TCosmeticSurgeryEnchantment Enchantment;
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
                    Enchantment = (TCosmeticSurgeryEnchantment)((TUnitEnchantmentAbilityData)Target.GetAbilityData(126)).FEnchantment;
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
    }

    public static partial class UnitSpells
    {
        // UnitSpells.pas:1081
        public static void RegisterUnitSpells(TAoWEngine Engine)
        {
            TSpellControl Control;
            Control = Engine.FGameSet.FSpellControl;
            Engine.RegisterEClasses([EClass.Of<TDispelMagicSpellCA>()]);
            Control.RegisterSpell(new TDispelMagic(null));
            Control.RegisterSpell(new THaste(null));
            Control.RegisterSpell(new TStoneSkin(null));
            Control.RegisterSpell(new TEnchantWeapon(null));
            Control.RegisterSpell(new TFury(null));
            Control.RegisterSpell(new TFreeMovement(null));
            Control.RegisterSpell(new TRemedy(null));
            Control.RegisterSpell(new THolyChampion(null));
            Control.RegisterSpell(new TUnholyChampion(null));
            Control.RegisterSpell(new TConcealment(null));
            Control.RegisterSpell(new TFireAura(null));
            Control.RegisterSpell(new TWaterWalking(null));
            Control.RegisterSpell(new TWindWalking(null));
            Control.RegisterSpell(new TLiquidForm(null));
            Control.RegisterSpell(new THealingWater(null));
            Control.RegisterSpell(new TBless(null));
            Control.RegisterSpell(new TCosmeticSurgery(null));
            Control.RegisterSpell(new TDarkGift(null));
        }

        // UnitSpells.pas:1107
        public static void UnregisterUnitSpells(TAoWEngine Engine)
        {
            TSpellControl Control;
            Control = Engine.FGameSet.FSpellControl;
            Control.UnregisterSpellID(8);
            Control.UnregisterSpellID(15);
            Control.UnregisterSpellID(17);
            Control.UnregisterSpellID(16);
            Control.UnregisterSpellID(33);
            Control.UnregisterSpellID(32);
            Control.UnregisterSpellID(35);
            Control.UnregisterSpellID(37);
            Control.UnregisterSpellID(36);
            Control.UnregisterSpellID(39);
            Control.UnregisterSpellID(40);
            Control.UnregisterSpellID(42);
            Control.UnregisterSpellID(43);
            Control.UnregisterSpellID(49);
            Control.UnregisterSpellID(55);
            Control.UnregisterSpellID(56);
            Control.UnregisterSpellID(59);
            Control.UnregisterSpellID(77);
            Engine.UnregisterEClasses([EClass.Of<TDispelMagicSpellCA>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
