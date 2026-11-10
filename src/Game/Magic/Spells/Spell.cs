// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Ai;

using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;

using Aow.Game.Draw.Animations;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// (name, control types, spell ID, base mana cost, sphere, level, type, text, sounds, images.
// remain unresolved Integers)
public partial class Spell : EObject
{

    // Spells.PAS:10
    public Spell(ECustomNode owner) : base(owner)
    {
        FSpellText = new StringList();
        FCastSounds = new SFXLibrary(null);
        FCastImages = new ImageSequenceList(AoWHSSet.Current.ImageLibraryManager, null);
        FResearchCost = 1;
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global);
    }

    public string FSpellName = "";
    public Set<SpellControlTypeElement> FSpellControlTypes;
    public int FSpellID;
    public int FBaseManaCost;
    public int FResearchCost;
    public int FManaUpkeep;
    public MagicSphere FMagicSphere;
    public byte FSpellLevel;
    public SpellType FSpellType;
    // stGlobal guard
    public StringList FSpellText;
    public SFXLibrary FCastSounds;
    public ImageSequenceList FCastImages;
    public int FCastDuration;

    // Spells.PAS:20
    public override void Dispose()
    {
        FCastSounds?.Dispose();
        FCastImages?.Dispose();
        FSpellText?.Dispose();
        base.Dispose();
    }

    // Spells.PAS:31
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FSpellText, 10);
        Stream.rwEObject(FCastSounds, 11);
        Stream.rwStreamProc(12, FCastImages.LoadFromStream, FCastImages.SaveToStream);
        Stream.rwInteger(ref FBaseManaCost, 13);
        Stream.rwInteger(ref FManaUpkeep, 14);
        Stream.rwInteger(ref FResearchCost, 15);
        Stream.rwData(Bytes.Of(ref FMagicSphere, 1), 1, 16);
        Stream.rwByte(ref FSpellLevel, 17);
    }

    // Spells.PAS:224
    public virtual void PlayDefaultCastEffects(sbyte Player, sbyte X, sbyte Y, sbyte Layer)
    {
        ImageSequenceAnimation Animation;
        if (AoWHSMap.Current.Visible(X, Y, Layer, 0))
        {
            AoWHSMap.Current.ValidateObservation(Player, X, Y, Layer, 0);
            if (AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(0) != null)
            {
                Animation = new ImageSequenceAnimation(null);
                Animation.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(0));
                AoWHSMap.Current.PlaceHX(Animation, X, Y, Layer);
            }

            if (AoWHSMap.Current.FSeatedPlayer == Player || AoWHSMap.Current.Watching(X, Y, Layer, 0))
            {
                AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }
        }
    }

    // Spells.PAS:312
    public virtual bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount) => false;

    // Spells.PAS:413
    public virtual void AIPrefetchCastSpellActions(AbstractUnit Source, AICastSpellActionList Actions)
    {
    }

    // Spells.PAS:415
    public virtual bool AIExecuteCastSpellAction(AbstractUnit Source) => false;

    // Spells.PAS:27
    public virtual bool GetEnabled() => true;

    // Spells.PAS:43
    public virtual bool CanActivate(AbstractUnit Source, ref string MessageText)
    {
        bool Result;
        if (FSpellType == SpellType.Global)
        {
            if (AoWHSMap.Current.FGlobalMagicControl.FActiveEnchantments.Contains(GlobalEnchantmentType.SpellWard))
            {
                MessageText = AbilityRules.TranslateRStr(GameText.CannotCastGlobalEnchantmentWhenSpellWardIsActiveRStr);
                Result = false;
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    public bool CanActivate__static(AbstractUnit Source, ref string MessageText)
    {
        bool Result;
        if (FSpellType == SpellType.Global)
        {
            if (AoWHSMap.Current.FGlobalMagicControl.FActiveEnchantments.Contains(GlobalEnchantmentType.SpellWard))
            {
                MessageText = AbilityRules.TranslateRStr(GameText.CannotCastGlobalEnchantmentWhenSpellWardIsActiveRStr);
                Result = false;
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Spells.PAS:54
    public virtual bool Activate(AbstractUnit Source)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        if (AoWHSMap.Current.FSeatedPlayer == Source.FPlayer)
        {
            Source.GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                AoWHSMap.Current.MakeVisible(X, Y, L, 3);
            }
        }

        // { Original returns False after optionally centering the view. }
        return false;
    }

    public bool Activate__static(AbstractUnit Source)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        if (AoWHSMap.Current.FSeatedPlayer == Source.FPlayer)
        {
            Source.GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                AoWHSMap.Current.MakeVisible(X, Y, L, 3);
            }
        }

        // { Original returns False after optionally centering the view. }
        return false;
    }

    // Spells.PAS:90
    public virtual bool ActivateCombat(AbstractUnit Source)
    {
        bool Result;
        CombatSpellCaster Caster;
        string Reason = "";
        if (CanActivateCombat(Source, ref Reason))
        {
            Caster = new CombatSpellCaster(null);
            Caster.FCombatCasterSpellID = FSpellID;
            Caster.FCombatCasterSourceID = Source.FUnitID;
            AoWHSMap.Current.FCombatMap.FCombatStartedEventsStore.Raise(Caster);
            Caster.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:158
    public virtual CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana) => default;

    // Spells.PAS:178
    public virtual int GetCombatDamageValue(CombatObject Source, CombatObject Target) => 0;

    // Spells.PAS:182
    public virtual void GetCombatDamageValueEx(CombatObject Source, CombatObject Target, ref AbilityDamageValues Values)
    {
        ref var p4DamageValue = ref Unsafe.As<AbilityDamageValues, P4DamageValue>(ref Values);
        p4DamageValue.V0 = 0;
        p4DamageValue.V1 = 0;
        p4DamageValue.V2 = 0;
        p4DamageValue.V3 = 0;
        p4DamageValue.DamageTypes = Set<DamageType>.Of();
    }

    // Spells.PAS:326
    public virtual void tcGetDamageValueEx(CombatObject Source, CombatObject Target, ref AbilityDamageValues Values) => this.GetCombatDamageValueEx(Source, Target, ref Values);

    // Spells.PAS:339
    public virtual int tcGetDamageValue(CombatObject Source, CombatObject Target) => this.GetCombatDamageValue(Source, Target);

    // Spells.PAS:320
    public virtual void fcPrefetchCombatCommands(FastCombatCommandList Commands)
    {
    }

    // Spells.PAS:322
    public virtual bool fcExecuteCombatCommand(CombatObject Source, FastCombatCommand Command) => false;

    // Spells.PAS:316
    public virtual void fcGetDamageValueEx(CombatObject Source, CombatObject Target, WallSituation WallSituation, ref AbilityDamageValues Values)
    {
        Values = default;
    }

    // Spells.PAS:193
    public void CastingDone(AbstractUnit Source, int Mana)
    {
        if (Source is Hero)
        {
            ((Hero)Source).CastingDone(FSpellID, Mana);
        }
    }

    // Spells.PAS:198
    public void CombatCastingDone(CombatObject Source, int Mana)
    {
        if (Source is CombatUnit && ((CombatUnit)Source).FUnit is Hero)
        {
            var combatUnit = (CombatUnit)Source;
            ((Hero)combatUnit.FUnit).CastingDone(FSpellID, Mana);
            combatUnit.UpdateWallCombatFeatures();
            if (Source.FCombat is FastBattle)
            {
                ((FastCombatUnit)Source).UpdateWallSiegeObjectRegistration();
            }
        }
    }

    // Spells.PAS:371
    public bool AIRequestCastSpellExpense(AbstractUnit Source, int Budget, int Mana, int Upkeep)
    {
        bool Result;
        AIBudgetManager Manager;
        if (Upkeep > 0)
        {
            Mana -= AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FMagicControl.AIRequestOverrideUpkeep(Round((double)Upkeep * 0.8));
            if (Mana < 0)
            {
                Mana = 0;
            }
        }

        Manager = AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FAIBudget;
        if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FMagicControl.GetCheatEnabled())
        {
            Result = Manager.RequestBudgetExpenseEx(0, Budget, 0, Mana, Manager.FDefaultBudget, 1 != 0);
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Spells.PAS:396
    public bool AICastSpellExpenseMade(AbstractUnit Source, int Budget, int Mana, int Upkeep)
    {
        bool Result;
        AIBudgetManager Manager;
        if (AIRequestCastSpellExpense(Source, Budget, Mana, Upkeep))
        {
            Manager = AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FAIBudget;
            if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FMagicControl.GetCheatEnabled())
            {
                Result = Manager.BudgetExpenseMadeEx(0, Budget, 0, Mana, Manager.FDefaultBudget, (AIBudgetManager.BudgetExpenseMode)2);
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    public virtual bool CanActivateCombat(AbstractUnit source, ref string reason) => AoWHSMap.Current.FCombat != null;

    public virtual void GetCombatInfo(AbstractUnit source, ref SpellCombatInfo info)
    {
        info.Attack = 0; info.Damage = 0; info.DamageTypes = default;
        info.RepeatCount = 1; info.Range = 0; info.Radius = 0; info.TargetTypes = default;
    }

    public virtual AICastSpellAction CreateCastSpellAction()
    {
        var action = new AICastSpellAction(null);
        // TEObject.Create only calls SetOwner. It bypasses descendant constructor bodies.
        try { action.SetOwner(null); } catch { action.Dispose(); throw; }
        return action;
    }

    public virtual void SetupCastSpellAction(AbstractUnit source, AICastSpellAction action)
    { action.FSpellID = FSpellID; action.FSourceUnitID = source.FUnitID; }

    // Spells.PAS:215. The casting view aliases the combat unit's hero.
    public void CombatSpellCast(CombatUnit source) =>
        CombatCastingDone(source, ((Hero)source.FUnit).CastingMana(this));
}
