// Translated from UnitSpells.pas.
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

namespace Aow.Units.UnitSpells
{
    // spell ID, mana cost charged after execution
    public partial class TDispelMagicSpellCA : Aow.Units.AoWE.TDispelMagicCA
    {
        // UnitSpells.pas:975
        public new const int ClassID = 131793;

        public TDispelMagicSpellCA(TECustomNode owner) : base(owner)
        { }

        public int FSpellID;
        public int FManaCost;
        // UnitSpells.pas:980
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FSpellID, 60);
            Stream.rwInteger(ref FManaCost, 61);
        }

        // UnitSpells.pas:987
        public override void Execute(Aow.Units.AoWE.TCombat A1)
        {
            Aow.Units.AoWE.TCombatUnit Source = default;
            base.Execute(A1);
            if ((FManaCost != 0) && A1.FStatus.Contains(Aow.Units.AoWE.TCombatStatusElement.cExecuting))
            {
                var call__1 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                if (call__1 != null)
                {
                    var call__2 = A1.FCombatData.FindID(FSourceID);
                    Source = (Aow.Units.AoWE.TCombatUnit)call__2;
                    var call__3 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__3.CastingDone(Source.FUnit, FManaCost);
                }
            }
        }


    }

    // inherited multiple targets, target range, dispel resistance, AI value
    public partial class TDispelMagic : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:1000
        public TDispelMagic(TECustomNode owner) : base(owner)
        {
            var call__4 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DispelMagicRStr);
            FSpellName = call__4;
            FSpellID = 8;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msCosmos;
            FTargetRange = 4;
            FAllowMultipleTargets = true;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:1023
        public override Aow.Units.AoWE.TCombatAction CreateCA(Aow.Units.AoWE.TCombatObject A1, Aow.Units.AoWE.TCombatObject A2, bool A3)
        {
            Aow.Units.AoWE.TCombatAction Result = default;
            Aow.Units.UnitSpells.TDispelMagicSpellCA Action = default;
            Action = new Aow.Units.UnitSpells.TDispelMagicSpellCA(null);
            Action.FSpellID = FSpellID;
            Action.FSourceID = unchecked((byte)A1.FID);
            Action.FTargetID = unchecked((byte)A2.FID);
            var call__5 = GetDispelMana(((Aow.Units.AoWE.TCombatUnit)A1).FUnit);
            Action.Generate(A1, A2, call__5);
            if (A3)
            {
                Action.FManaCost = FBaseManaCost;
            }

            Result = Action;
            return Result;
        }

        // UnitSpells.pas:1074
        public override void GetCombatInfo(Aow.Units.AoWE.TAbstractUnit Source, ref Aow.Units.AoWE.TSpellCombatInfo Info)
        {
            base.GetCombatInfo(Source, ref Info);
            Info.TargetTypes = TSet<Aow.Units.AoWE.TCombatTargetTypeElement>.Of(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend, Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy);
        }

        // UnitSpells.pas:1068
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            bool bool__6 = Result;
            if (bool__6)
            {
                var call__7 = Target.CanDispelEnchantment(unchecked((sbyte)Source.FPlayer));
                bool__6 = call__7;
            }

            Result = bool__6;
            return Result;
        }

        // UnitSpells.pas:1018
        public override bool ValidTargetParty(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TArmyHS Party)
        {
            bool Result = default;
            Result = Party.FArmy.FPlayer == Source.FPlayer;
            return Result;
        }

        // UnitSpells.pas:1036
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            int Count = default;
            Aow.Units.AoWE.TEnchantmentList Enchantments = default;
            int I = default;
            int Chance = default;
            if (ValidTargetUnit(Source, Target))
            {
                Count = 0;
                Enchantments = new Aow.Units.AoWE.TEnchantmentList(null);
                Target.ListEnchantments(Enchantments);
                {
                    I = Enchantments.GetCount() - 1;
                    byte end__8 = 0;
                    for (; I >= end__8; I -= 1)
                    {
                        var call__9 = GetDispelMana(Source);
                        var call__10 = Enchantments.GetEnchantments(I);
                        var call__11 = call__10.DispelChance(call__9);
                        Chance = call__11;
                        var call__12 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(100);
                        if (Chance > call__12)
                        {
                            Count += 1;
                            var call__13 = Enchantments.GetEnchantments(I);
                            call__13.Dispel();
                        }
                    }
                }

                if (Source.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
                {
                    if (Count > 0)
                    {
                        var call__14 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XSpellsSuccessfullyDispelledRStr);
                        var call__15 = Aow.Rtl.Rtl.Format(call__14, [Aow.Rtl.Rtl.IntToStr(Count)]);
                        Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(call__15);
                    }
                    else
                    {
                        var call__16 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DispelMagicFailedRStr);
                        Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(call__16);
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
        public int GetDispelMana(Aow.Units.AoWE.TAbstractUnit Source)
        {
            int Result = default;
            Result = (Source.GetAbLevel(52) * 15) + 25;
            return Result;
        }
    }

    public partial class THaste : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:215
        public THaste(TECustomNode owner) : base(owner)
        {
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
            var call__17 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HasteRStr);
            FSpellName = call__17;
            FSpellID = 15;
            FBaseManaCost = 5;
            FManaUpkeep = 2;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msAir;
            FTargetRange = 12;
            FUnitDispelResistance = 3;
        }

        // UnitSpells.pas:250
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            Result = Result && !Target.GetAbilityEnabled(152);
            return Result;
        }

        // UnitSpells.pas:229
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(152))
                {
                    var call__18 = Target.GetAbilityData(152);
                    Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__18).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(unchecked((sbyte)Source.FPlayer));
                    Enchantment.FDispelResistance = unchecked((sbyte)FUnitDispelResistance);
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

    public partial class TStoneSkin : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:256
        public TStoneSkin(TECustomNode owner) : base(owner)
        {
            var call__19 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.StoneSkinRStr);
            FSpellName = call__19;
            FSpellID = 17;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msEarth;
            FTargetRange = 12;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
            FUnitDispelResistance = 3;
            FAIValue = 200;
            FCastDuration = 5;
        }

        // UnitSpells.pas:290
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            Result = Result && !Target.GetAbilityEnabled(153);
            return Result;
        }

        // UnitSpells.pas:271
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(153))
                {
                    var call__20 = Target.GetAbilityData(153);
                    Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__20).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(unchecked((sbyte)Source.FPlayer));
                    Enchantment.FDispelResistance = unchecked((sbyte)FUnitDispelResistance);
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

    public partial class TEnchantWeapon : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:296
        public TEnchantWeapon(TECustomNode owner) : base(owner)
        {
            var call__21 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnchantWeaponRStr);
            FSpellName = call__21;
            FSpellID = 16;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msCosmos;
            FTargetRange = 12;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
            FUnitDispelResistance = 3;
            FAIValue = 225;
            FCastDuration = 5;
        }

        // UnitSpells.pas:330
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            bool bool__22 = Result;
            if (bool__22)
            {
                var call__23 = Target.GetAbilityEnabled(154);
                bool__22 = !call__23;
            }

            bool bool__24 = bool__22;
            if (bool__24)
            {
                var call__25 = Target.GetAbilityEnabled(21);
                bool__24 = call__25;
            }

            Result = bool__24;
            return Result;
        }

        // UnitSpells.pas:311
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(154))
                {
                    var call__26 = Target.GetAbilityData(154);
                    Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__26).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(unchecked((sbyte)Source.FPlayer));
                    Enchantment.FDispelResistance = unchecked((sbyte)FUnitDispelResistance);
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

    public partial class TFreeMovement : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:386
        public TFreeMovement(TECustomNode owner) : base(owner)
        {
            var call__27 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FreeMovementRStr);
            FSpellName = call__27;
            FSpellID = 32;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msEarth;
            FTargetRange = 12;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
            FUnitDispelResistance = 4;
        }

        // UnitSpells.pas:418
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            bool bool__28 = Result;
            if (bool__28)
            {
                var call__29 = Target.GetAbilityEnabled(157);
                bool__28 = !call__29;
            }

            bool bool__30 = bool__28;
            if (bool__30)
            {
                var call__31 = Target.GetAbilityEnabled(3);
                bool bool__32 = !call__31;
                if (!bool__32)
                {
                    var call__33 = Target.GetAbilityEnabled(5);
                    bool__32 = !call__33;
                }

                bool bool__34 = bool__32;
                if (!bool__34)
                {
                    var call__35 = Target.GetAbilityEnabled(4);
                    bool__34 = !call__35;
                }

                bool__30 = bool__34;
            }

            Result = bool__30;
            return Result;
        }

        // UnitSpells.pas:399
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(157))
                {
                    var call__36 = Target.GetAbilityData(157);
                    Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__36).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(unchecked((sbyte)Source.FPlayer));
                    Enchantment.FDispelResistance = unchecked((sbyte)FUnitDispelResistance);
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

    public partial class TFury : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:336
        public TFury(TECustomNode owner) : base(owner)
        {
            var call__37 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FuryRStr);
            FSpellName = call__37;
            FSpellID = 33;
            FAIValue = 175;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msFire;
            FTargetRange = 12;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
            FUnitDispelResistance = 3;
        }

        // UnitSpells.pas:369
        public override void PlayDefaultCastEffects(sbyte Player, sbyte X, sbyte Y, sbyte Layer)
        {
            base.PlayDefaultCastEffects(unchecked((sbyte)Player), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer));
            bool bool__38 = (Player == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (!bool__38)
            {
                var call__39 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer), 0);
                bool__38 = call__39;
            }

            if (bool__38)
            {
                var call__40 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__40.FCastSounds.Play(1);
            }
        }

        // UnitSpells.pas:376
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            Result = Result && !Target.GetAbilityEnabled(156);
            if (Result)
            {
                var call__41 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Target.FPlayer);
                if ((call__41.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman) && (Target is Aow.Units.AoWE.TLeader))
                {
                    Result = false;
                }
            }

            return Result;
        }

        // UnitSpells.pas:350
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(156))
                {
                    var call__42 = Target.GetAbilityData(156);
                    Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__42).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(unchecked((sbyte)Source.FPlayer));
                    Enchantment.FDispelResistance = unchecked((sbyte)FUnitDispelResistance);
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

    public partial class TRemedy : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:424
        public TRemedy(TECustomNode owner) : base(owner)
        {
            var call__43 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RemedyRStr);
            FSpellName = call__43;
            FSpellID = 35;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msCosmos;
            FTargetRange = 12;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:458
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            bool bool__44 = Result;
            if (bool__44)
            {
                var call__45 = Target.GetHitPoints();
                var call__46 = Target.GetHits();
                bool bool__47 = (call__45 != call__46);
                if (!bool__47)
                {
                    var call__48 = Target.GetAbilityEnabled(96);
                    bool__47 = call__48;
                }

                bool bool__49 = bool__47;
                if (!bool__49)
                {
                    var call__50 = Target.GetAbilityEnabled(92);
                    bool__49 = call__50;
                }

                bool bool__51 = bool__49;
                if (!bool__51)
                {
                    var call__52 = Target.GetAbilityEnabled(108);
                    bool__51 = call__52;
                }

                bool bool__53 = bool__51;
                if (!bool__53)
                {
                    var call__54 = Target.GetAbilityEnabled(53);
                    bool__53 = call__54;
                }

                bool bool__55 = bool__53;
                if (!bool__55)
                {
                    var call__56 = Target.GetAbilityEnabled(93);
                    bool__55 = call__56;
                }

                bool bool__57 = bool__55;
                if (!bool__57)
                {
                    var call__58 = Target.GetAbilityEnabled(127);
                    bool__57 = call__58;
                }

                bool__44 = bool__57;
            }

            Result = bool__44;
            return Result;
        }

        // UnitSpells.pas:436
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            int HitPoints = default;
            if (ValidTargetUnit(Source, Target))
            {
                HitPoints = Target.GetHitPoints() + 5;
                if (HitPoints > Target.GetHits())
                {
                    HitPoints = Target.GetHits();
                }

                Target.SetHitPoints(unchecked((sbyte)HitPoints));
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

    public partial class THealingWater : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:467
        public THealingWater(TECustomNode owner) : base(owner)
        {
            var call__59 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HealingWaterRStr);
            FSpellName = call__59;
            FSpellID = 55;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msCosmos;
            FTargetRange = 12;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:491
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            bool bool__60 = Result;
            if (bool__60)
            {
                var call__61 = Target.GetHitPoints();
                var call__62 = Target.GetHits();
                bool bool__63 = (call__61 != call__62);
                if (!bool__63)
                {
                    var call__64 = Target.GetAbilityEnabled(127);
                    bool__63 = call__64;
                }

                bool__60 = bool__63;
            }

            Result = bool__60;
            return Result;
        }

        // UnitSpells.pas:479
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            if (ValidTargetUnit(Source, Target))
            {
                Target.RemoveAbility(127);
                var call__65 = Target.GetHits();
                Target.SetHitPoints(unchecked((sbyte)call__65));
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    public partial class THolyChampion : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:497
        public THolyChampion(TECustomNode owner) : base(owner)
        {
            var call__66 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HolyChampionRStr);
            FSpellName = call__66;
            FSpellID = 36;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msLife;
            FTargetRange = 12;
            FUnitDispelResistance = 4;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:529
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            bool bool__67 = Result;
            if (bool__67)
            {
                var call__68 = Target.GetAbilityEnabled(146);
                bool__67 = !call__68;
            }

            bool bool__69 = bool__67;
            if (bool__69)
            {
                var call__70 = Target.GetAbilityEnabled(160);
                bool__69 = !call__70;
            }

            bool bool__71 = bool__69;
            if (bool__71)
            {
                var call__72 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(146);
                var call__73 = call__72.CanExpand(Target);
                bool__71 = call__73;
            }

            Result = bool__71;
            return Result;
        }

        // UnitSpells.pas:510
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(160))
                {
                    var call__74 = Target.GetAbilityData(160);
                    Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__74).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(unchecked((sbyte)Source.FPlayer));
                    Enchantment.FDispelResistance = unchecked((sbyte)FUnitDispelResistance);
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

    public partial class TUnholyChampion : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:537
        public TUnholyChampion(TECustomNode owner) : base(owner)
        {
            var call__75 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UnholyChampionRStr);
            FSpellName = call__75;
            FSpellID = 37;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msDeath;
            FTargetRange = 12;
            FUnitDispelResistance = 4;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:569
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            bool bool__76 = Result;
            if (bool__76)
            {
                var call__77 = Target.GetAbilityEnabled(147);
                bool__76 = !call__77;
            }

            bool bool__78 = bool__76;
            if (bool__78)
            {
                var call__79 = Target.GetAbilityEnabled(161);
                bool__78 = !call__79;
            }

            bool bool__80 = bool__78;
            if (bool__80)
            {
                var call__81 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(147);
                var call__82 = call__81.CanExpand(Target);
                bool__80 = call__82;
            }

            Result = bool__80;
            return Result;
        }

        // UnitSpells.pas:550
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(161))
                {
                    var call__83 = Target.GetAbilityData(161);
                    Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__83).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(unchecked((sbyte)Source.FPlayer));
                    Enchantment.FDispelResistance = unchecked((sbyte)FUnitDispelResistance);
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

    public partial class TConcealment : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:577
        public TConcealment(TECustomNode owner) : base(owner)
        {
            var call__84 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ConcealmentRStr);
            FSpellName = call__84;
            FSpellID = 39;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msEarth;
            FTargetRange = 12;
            FUnitDispelResistance = 5;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal);
        }

        // UnitSpells.pas:609
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            bool bool__85 = Result;
            if (bool__85)
            {
                var call__86 = Target.GetAbilityEnabled(145);
                bool__85 = !call__86;
            }

            bool bool__87 = bool__85;
            if (bool__87)
            {
                var call__88 = Target.GetAbilityEnabled(162);
                bool__87 = !call__88;
            }

            bool bool__89 = bool__87;
            if (bool__89)
            {
                var call__90 = Target.GetAbilityEnabled(54);
                bool__89 = !call__90;
            }

            Result = bool__89;
            return Result;
        }

        // UnitSpells.pas:590
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(162))
                {
                    var call__91 = Target.GetAbilityData(162);
                    Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__91).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(unchecked((sbyte)Source.FPlayer));
                    Enchantment.FDispelResistance = unchecked((sbyte)FUnitDispelResistance);
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

    public partial class TFireAura : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:616
        public TFireAura(TECustomNode owner) : base(owner)
        {
            var call__92 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FireAuraRStr);
            FSpellName = call__92;
            FSpellID = 40;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msFire;
            FTargetRange = 12;
            FUnitDispelResistance = 4;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:629
        public override void PlayDefaultCastEffects(sbyte Player, sbyte X, sbyte Y, sbyte Layer)
        {
            base.PlayDefaultCastEffects(unchecked((sbyte)Player), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer));
            bool bool__93 = (Player == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (!bool__93)
            {
                var call__94 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer), 0);
                bool__93 = call__94;
            }

            if (bool__93)
            {
                var call__95 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__95.FCastSounds.Play(1);
            }
        }

        // UnitSpells.pas:655
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            bool bool__96 = Result;
            if (bool__96)
            {
                var call__97 = Target.GetAbilityEnabled(7);
                bool bool__98 = !call__97;
                if (!bool__98)
                {
                    var call__99 = Target.GetAbilityEnabled(43);
                    bool__98 = !call__99;
                }

                bool__96 = bool__98;
            }

            bool bool__100 = bool__96;
            if (bool__100)
            {
                var call__101 = Target.GetAbilityEnabled(163);
                bool__100 = !call__101;
            }

            Result = bool__100;
            return Result;
        }

        // UnitSpells.pas:636
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(163))
                {
                    var call__102 = Target.GetAbilityData(163);
                    Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__102).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(unchecked((sbyte)Source.FPlayer));
                    Enchantment.FDispelResistance = unchecked((sbyte)FUnitDispelResistance);
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

    public partial class TWaterWalking : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:662
        public TWaterWalking(TECustomNode owner) : base(owner)
        {
            var call__103 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WaterWalkingRStr);
            FSpellName = call__103;
            FSpellID = 42;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msWater;
            FTargetRange = 12;
            FUnitDispelResistance = 6;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:694
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            bool bool__104 = Result;
            if (bool__104)
            {
                var call__105 = Target.GetAbilityEnabled(23);
                bool__104 = !call__105;
            }

            bool bool__106 = bool__104;
            if (bool__106)
            {
                var call__107 = Target.GetAbilityEnabled(2);
                bool__106 = !call__107;
            }

            bool bool__108 = bool__106;
            if (bool__108)
            {
                var call__109 = Target.GetAbilityEnabled(164);
                bool__108 = !call__109;
            }

            bool bool__110 = bool__108;
            if (bool__110)
            {
                var call__111 = Target.GetAbilityEnabled(1);
                bool__110 = !call__111;
            }

            bool bool__112 = bool__110;
            if (bool__112)
            {
                var call__113 = Target.GetAbilityEnabled(59);
                bool__112 = !call__113;
            }

            Result = bool__112;
            return Result;
        }

        // UnitSpells.pas:675
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(164))
                {
                    var call__114 = Target.GetAbilityData(164);
                    Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__114).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(unchecked((sbyte)Source.FPlayer));
                    Enchantment.FDispelResistance = unchecked((sbyte)FUnitDispelResistance);
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

    public partial class TWindWalking : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:702
        public TWindWalking(TECustomNode owner) : base(owner)
        {
            var call__115 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WindWalkingRStr);
            FSpellName = call__115;
            FSpellID = 43;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msAir;
            FTargetRange = 12;
            FUnitDispelResistance = 6;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:734
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            bool bool__116 = Result;
            if (bool__116)
            {
                var call__117 = Target.GetAbilityEnabled(1);
                bool__116 = !call__117;
            }

            bool bool__118 = bool__116;
            if (bool__118)
            {
                var call__119 = Target.GetAbilityEnabled(165);
                bool__118 = !call__119;
            }

            bool bool__120 = bool__118;
            if (bool__120)
            {
                var call__121 = Target.GetAbilityEnabled(59);
                bool__120 = !call__121;
            }

            Result = bool__120;
            return Result;
        }

        // UnitSpells.pas:715
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(165))
                {
                    var call__122 = Target.GetAbilityData(165);
                    Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__122).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(unchecked((sbyte)Source.FPlayer));
                    Enchantment.FDispelResistance = unchecked((sbyte)FUnitDispelResistance);
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

    public partial class TLiquidForm : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:741
        public TLiquidForm(TECustomNode owner) : base(owner)
        {
            var call__123 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LiquidFormRStr);
            FSpellName = call__123;
            FSpellID = 49;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msWater;
            FAIValue = 300;
            FTargetRange = 12;
            FUnitDispelResistance = 5;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
        }

        // UnitSpells.pas:783
        public override void PlayDefaultCastEffects(sbyte Player, sbyte X, sbyte Y, sbyte Layer)
        {
            base.PlayDefaultCastEffects(unchecked((sbyte)Player), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer));
            bool bool__124 = (Player == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (!bool__124)
            {
                var call__125 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer), 0);
                bool__124 = call__125;
            }

            if (bool__124)
            {
                var call__126 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__126.FCastSounds.Play(1);
            }
        }

        // UnitSpells.pas:774
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            Result = Result && !Target.GetAbilityEnabled(166);
            bool bool__127 = Result;
            if (bool__127)
            {
                var call__128 = Target.GetAbilityEnabled(13);
                bool bool__129 = !call__128;
                if (bool__129)
                {
                    var call__130 = Target.GetAbilityEnabled(77);
                    bool__129 = !call__130;
                }

                bool bool__131 = bool__129;
                if (!bool__131)
                {
                    var call__132 = Target.GetAbilityEnabled(2);
                    bool bool__133 = !call__132;
                    if (bool__133)
                    {
                        var call__134 = Target.GetAbilityEnabled(1);
                        bool__133 = !call__134;
                    }

                    bool bool__135 = bool__133;
                    if (bool__135)
                    {
                        var call__136 = Target.GetAbilityEnabled(59);
                        bool__135 = !call__136;
                    }

                    bool__131 = bool__135;
                }

                bool__127 = bool__131;
            }

            Result = bool__127;
            return Result;
        }

        // UnitSpells.pas:755
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(166))
                {
                    var call__137 = Target.GetAbilityData(166);
                    Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__137).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(unchecked((sbyte)Source.FPlayer));
                    Enchantment.FDispelResistance = unchecked((sbyte)FUnitDispelResistance);
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

    public partial class TBless : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:790
        public TBless(TECustomNode owner) : base(owner)
        {
            var call__138 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.BlessRStr);
            FSpellName = call__138;
            FSpellID = 56;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msWater;
            FTargetRange = 12;
            FUnitDispelResistance = 3;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
            FAIValue = 225;
            FCastDuration = 5;
        }

        // UnitSpells.pas:824
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            Result = Result && !Target.GetAbilityEnabled(158);
            return Result;
        }

        // UnitSpells.pas:805
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(158))
                {
                    var call__139 = Target.GetAbilityData(158);
                    Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__139).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(unchecked((sbyte)Source.FPlayer));
                    Enchantment.FDispelResistance = unchecked((sbyte)FUnitDispelResistance);
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

    public partial class TDarkGift : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:830
        public TDarkGift(TECustomNode owner) : base(owner)
        {
            var call__140 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DarkGiftRStr);
            FSpellName = call__140;
            FSpellID = 77;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msDeath;
            FTargetRange = 12;
            FUnitDispelResistance = 3;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
            FAIValue = 225;
            FCastDuration = 5;
        }

        // UnitSpells.pas:864
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            Result = Result && !Target.GetAbilityEnabled(169);
            return Result;
        }

        // UnitSpells.pas:845
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitEnchantment Enchantment = default;
            if (ValidTargetUnit(Source, Target))
            {
                if (Target.ExpandAbility(169))
                {
                    var call__141 = Target.GetAbilityData(169);
                    Enchantment = ((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__141).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.SetSource(unchecked((sbyte)Source.FPlayer));
                    Enchantment.FDispelResistance = unchecked((sbyte)FUnitDispelResistance);
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
    public partial class TCosmeticSurgerySpellCaster : Aow.Units.AoWE.TSpellCaster
    {
        public TCosmeticSurgerySpellCaster(TECustomNode owner) : base(owner)
        { }

        public int FFaceIndex;
        public int FTargetUnitID;
        // UnitSpells.pas:870
        public override bool CanCast(ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            Result = base.CanCast(ref Reason);
            return Result;
        }

        // UnitSpells.pas:875
        public override bool Cast()
        {
            bool Result = default;
            Aow.Units.AoWE.TAbstractUnit Source = default;
            Aow.Units.AoWE.TAbstractUnit Target = default;
            Aow.Units.AoWE.TUnitSpellTE Event = default;
            Aow.Rtl.AnsiString Reason = default;
            var call__142 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(59);
            ((Aow.Units.AoWE.TUnitSpell)call__142).ResetTargetSelection();
            var call__143 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FCasterSourceID);
            Source = call__143;
            var call__144 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FTargetUnitID);
            Target = call__144;
            bool bool__145 = (Source != null) && (Target != null);
            if (bool__145)
            {
                var call__146 = CanCast(ref Reason);
                bool__145 = call__146;
            }

            bool bool__147 = bool__145;
            if (bool__147)
            {
                var call__148 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Source.FPlayer);
                var call__149 = call__148.GetBusy();
                bool__147 = !call__149;
            }

            if (bool__147)
            {
                Event = new Aow.Units.AoWE.TUnitSpellTE(null);
                var call__150 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(59);
                Event.Initialize(FCasterSpellID, Source, null, call__150.FBaseManaCost);
                Event.FTargetUnitIDs.Add(FTargetUnitID);
                Event.FSpellData[0] = FFaceIndex;
                Event.FTargetPlayer = unchecked((byte)Target.FPlayer);
                var call__151 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer);
                call__151.AddEvent(Event);
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

    public partial class TCosmeticSurgery : Aow.Units.AoWE.TUnitSpell
    {
        // UnitSpells.pas:901
        public TCosmeticSurgery(TECustomNode owner) : base(owner)
        {
            var call__152 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DeformFaceRStr);
            FSpellName = call__152;
            FSpellID = 59;
            FSpellType = Aow.Units.AoWE.TSpellType.stUnit;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msSecret;
            FAllowMultipleTargets = false;
            FUnitDispelResistance = 10;
            FTargetRange = -1;
            FSpellControlTypes = TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal);
        }

        // UnitSpells.pas:968
        public override void GetCombatInfo(Aow.Units.AoWE.TAbstractUnit Source, ref Aow.Units.AoWE.TSpellCombatInfo Info)
        {
            // { The original saves Self and Source as well as Info; this compiler saves only Info. }
            base.GetCombatInfo(Source, ref Info);
            Info.TargetTypes = TSet<Aow.Units.AoWE.TCombatTargetTypeElement>.Of(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy);
        }

        // UnitSpells.pas:940
        public override bool ValidTargetUnit(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target)
        {
            bool Result = default;
            Result = base.ValidTargetUnit(Source, Target);
            Result = Result && TSet<Aow.Units.AoWE.TUnitType>.Of(Aow.Units.AoWE.TUnitType.utHumanoid, Aow.Units.AoWE.TUnitType.utCreature).Contains(Target.GetUnitType());
            return Result;
        }

        // UnitSpells.pas:946
        public override bool CastSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TUnitList Targets)
        {
            bool Result = default;
            Aow.Units.UnitSpells.TCosmeticSurgerySpellCaster Caster = default;
            if (Targets.GetCount() == 1)
            {
                Caster = new Aow.Units.UnitSpells.TCosmeticSurgerySpellCaster(null);
                Caster.FCasterSourceID = Source.FUnitID;
                Caster.FCasterSpellID = FSpellID;
                var call__153 = Targets.GetUnit(0);
                Caster.FTargetUnitID = call__153.FUnitID;
                if (Targets.GetUnit(0) is Aow.Units.AoWE.THero)
                {
                    var call__154 = Targets.GetUnit(0);
                    Caster.FFaceIndex = ((Aow.Units.AoWE.THero)call__154).FFaceIndex;
                }
                else
                {
                    Caster.FFaceIndex = 0;
                }

                Aow.Units.AoWE.AoWE.AoWHSMap.FCosmeticSurgeryEvents.TriggerNotifyEvents(Caster);
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
        public override bool ExecuteSpell(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TAbstractUnit Target, int Mana, ref Aow.Units.FixedArray_int_2 Data)
        {
            bool Result = default;
            Aow.Units.AoWE.TCosmeticSurgeryEnchantment Enchantment = default;
            bool Expanded = default;
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
                    var call__155 = Target.GetAbilityData(126);
                    Enchantment = (Aow.Units.AoWE.TCosmeticSurgeryEnchantment)((Aow.Units.AoWE.TUnitEnchantmentAbilityData)call__155).FEnchantment;
                    Enchantment.SetUpkeep(FManaUpkeep);
                    Enchantment.FFaceIndex = Data[0];
                    Enchantment.SetSource(unchecked((sbyte)Source.FPlayer));
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
        public static void RegisterUnitSpells(Aow.Units.AoWE.TAoWEngine Engine)
        {
            Aow.Units.AoWE.TSpellControl Control = default;
            Control = Engine.FGameSet.FSpellControl;
            Engine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.UnitSpells.TDispelMagicSpellCA>()]);
            var call__156 = new Aow.Units.UnitSpells.TDispelMagic(null);
            Control.RegisterSpell(call__156);
            var call__157 = new Aow.Units.UnitSpells.THaste(null);
            Control.RegisterSpell(call__157);
            var call__158 = new Aow.Units.UnitSpells.TStoneSkin(null);
            Control.RegisterSpell(call__158);
            var call__159 = new Aow.Units.UnitSpells.TEnchantWeapon(null);
            Control.RegisterSpell(call__159);
            var call__160 = new Aow.Units.UnitSpells.TFury(null);
            Control.RegisterSpell(call__160);
            var call__161 = new Aow.Units.UnitSpells.TFreeMovement(null);
            Control.RegisterSpell(call__161);
            var call__162 = new Aow.Units.UnitSpells.TRemedy(null);
            Control.RegisterSpell(call__162);
            var call__163 = new Aow.Units.UnitSpells.THolyChampion(null);
            Control.RegisterSpell(call__163);
            var call__164 = new Aow.Units.UnitSpells.TUnholyChampion(null);
            Control.RegisterSpell(call__164);
            var call__165 = new Aow.Units.UnitSpells.TConcealment(null);
            Control.RegisterSpell(call__165);
            var call__166 = new Aow.Units.UnitSpells.TFireAura(null);
            Control.RegisterSpell(call__166);
            var call__167 = new Aow.Units.UnitSpells.TWaterWalking(null);
            Control.RegisterSpell(call__167);
            var call__168 = new Aow.Units.UnitSpells.TWindWalking(null);
            Control.RegisterSpell(call__168);
            var call__169 = new Aow.Units.UnitSpells.TLiquidForm(null);
            Control.RegisterSpell(call__169);
            var call__170 = new Aow.Units.UnitSpells.THealingWater(null);
            Control.RegisterSpell(call__170);
            var call__171 = new Aow.Units.UnitSpells.TBless(null);
            Control.RegisterSpell(call__171);
            var call__172 = new Aow.Units.UnitSpells.TCosmeticSurgery(null);
            Control.RegisterSpell(call__172);
            var call__173 = new Aow.Units.UnitSpells.TDarkGift(null);
            Control.RegisterSpell(call__173);
        }

        // UnitSpells.pas:1107
        public static void UnregisterUnitSpells(Aow.Units.AoWE.TAoWEngine Engine)
        {
            Aow.Units.AoWE.TSpellControl Control = default;
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
            Engine.UnregisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.UnitSpells.TDispelMagicSpellCA>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
