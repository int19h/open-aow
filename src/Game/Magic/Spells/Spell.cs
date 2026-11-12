// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.AI;
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
public partial class Spell : EngineObject
{

    // Spells.PAS:10
    public Spell(ECustomNode owner) : base(owner)
    {
        SpellText = new StringList();
        CastSounds = new SfxLibrary(null);
        CastImages = new ImageSequenceList(GamePack.Current.ImageLibraryManager, null);
        ResearchCost = 1;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global);
    }

    public string SpellName { get; protected set; } = "";
    public Set<SpellControlTypeElement> SpellControlTypes { get; protected set; }
    public int SpellId { get; protected set; }

    public int BaseManaCost;
    public int ResearchCost;
    public int ManaUpkeep;
    public MagicSphere MagicSphere;
    public byte SpellLevel;

    public SpellType SpellType { get; protected set; }
    // stGlobal guard
    public StringList SpellText { get; }
    public SfxLibrary CastSounds { get; }
    public ImageSequenceList CastImages { get; set; }
    public int CastDuration { get; protected set; }

    // Spells.PAS:20
    public override void Dispose()
    {
        CastSounds?.Dispose();
        CastImages?.Dispose();
        SpellText?.Dispose();
        base.Dispose();
    }

    // Spells.PAS:31
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwStrings(SpellText, 10);
        stream.rwEObject(CastSounds, 11);
        stream.rwStreamProc(12, CastImages.LoadFromStream, CastImages.SaveToStream);
        stream.rwInteger(ref BaseManaCost, 13);
        stream.rwInteger(ref ManaUpkeep, 14);
        stream.rwInteger(ref ResearchCost, 15);
        stream.rwData(Bytes.Of(ref MagicSphere, 1), 1, 16);
        stream.rwByte(ref SpellLevel, 17);
    }

    // Spells.PAS:224
    public virtual void PlayDefaultCastEffects(sbyte player, sbyte X, sbyte Y, sbyte layer)
    {
        ImageSequenceAnimation animation;
        if (StrategicMap.Current.Visible(X, Y, layer, 0))
        {
            StrategicMap.Current.ValidateObservation(player, X, Y, layer, 0);
            if (GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.GetImageSequence(0) != null)
            {
                animation = new ImageSequenceAnimation(null);
                animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.GetImageSequence(0));
                StrategicMap.Current.PlaceHex(animation, X, Y, layer);
            }

            if (StrategicMap.Current.SeatedPlayer == player || StrategicMap.Current.Watching(X, Y, layer, 0))
            {
                GamePack.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(0);
            }
        }
    }

    // Spells.PAS:312
    public virtual bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount) => false;

    // Spells.PAS:413
    public virtual void AIPrefetchCastSpellActions(Troop source, AICastSpellActionList actions)
    {
    }

    // Spells.PAS:415
    public virtual bool AIExecuteCastSpellAction(Troop source) => false;

    // Spells.PAS:27
    public virtual bool GetEnabled() => true;

    // Spells.PAS:43
    public virtual bool CanActivate(Troop source, ref string messageText)
    {
        bool result;
        if (SpellType == SpellType.Global)
        {
            if (StrategicMap.Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentType.SpellWard))
            {
                messageText = AbilityRules.TranslateResourceString(GameText.CannotCastGlobalEnchantmentWhenSpellWardIsActiveText);
                result = false;
            }
            else
            {
                result = true;
            }
        }
        else
        {
            result = true;
        }

        return result;
    }

    public bool CanActivateStatic(Troop source, ref string messageText)
    {
        bool result;
        if (SpellType == SpellType.Global)
        {
            if (StrategicMap.Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentType.SpellWard))
            {
                messageText = AbilityRules.TranslateResourceString(GameText.CannotCastGlobalEnchantmentWhenSpellWardIsActiveText);
                result = false;
            }
            else
            {
                result = true;
            }
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Spells.PAS:54
    public virtual bool Activate(Troop source)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        if (StrategicMap.Current.SeatedPlayer == source.Player)
        {
            source.GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                StrategicMap.Current.MakeVisible(X, Y, L, 3);
            }
        }

        // { Original returns False after optionally centering the view. }
        return false;
    }

    public bool ActivateStatic(Troop source)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        if (StrategicMap.Current.SeatedPlayer == source.Player)
        {
            source.GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                StrategicMap.Current.MakeVisible(X, Y, L, 3);
            }
        }

        // { Original returns False after optionally centering the view. }
        return false;
    }

    // Spells.PAS:90
    public virtual bool ActivateCombat(Troop source)
    {
        bool result;
        CombatSpellCaster caster;
        string reason = "";
        if (CanActivateCombat(source, ref reason))
        {
            caster = new CombatSpellCaster(null);
            caster.CombatCasterSpellId = SpellId;
            caster.CombatCasterSourceId = source.UnitId;
            StrategicMap.Current.CombatMap.CombatStartedEventsStore.Raise(caster);
            caster.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:158
    public virtual CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana) => default;

    // Spells.PAS:178
    public virtual int GetCombatDamageValue(CombatObject source, CombatObject target) => 0;

    // Spells.PAS:182
    public virtual void GetCombatDamageValues(CombatObject source, CombatObject target, ref AbilityDamageValues values)
    {
        values.SourceDamage = 0;
        values.SourceCost = 0;
        values.TargetDamage = 0;
        values.TargetCost = 0;
        values.DamageTypes = Set<DamageType>.Of();
    }

    // Spells.PAS:326
    public virtual void TacticalBattleGetDamageValues(CombatObject source, CombatObject target, ref AbilityDamageValues values) => this.GetCombatDamageValues(source, target, ref values);

    // Spells.PAS:339
    public virtual int TacticalBattleGetDamageValue(CombatObject source, CombatObject target) => this.GetCombatDamageValue(source, target);

    // Spells.PAS:320
    public virtual void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
    }

    // Spells.PAS:322
    public virtual bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command) => false;

    // Spells.PAS:316
    public virtual void FastBattleGetDamageValues(CombatObject source, CombatObject target, WallSituation wallSituation, ref AbilityDamageValues values)
    {
        values = default;
    }

    // Spells.PAS:193
    public void CastingDone(Troop source, int mana)
    {
        if (source is Hero)
        {
            ((Hero)source).CastingDone(SpellId, mana);
        }
    }

    // Spells.PAS:198
    public void CombatCastingDone(CombatObject source, int mana)
    {
        if (source is CombatUnit && ((CombatUnit)source).Troop is Hero)
        {
            var combatUnit = (CombatUnit)source;
            ((Hero)combatUnit.Troop).CastingDone(SpellId, mana);
            combatUnit.UpdateWallCombatFeatures();
            if (source.Combat is FastBattle)
            {
                ((FastBattleUnit)source).UpdateWallSiegeObjectRegistration();
            }
        }
    }

    // Spells.PAS:371
    public bool AIRequestCastSpellExpense(Troop source, int budget, int mana, int upkeep)
    {
        bool result;
        AIBudgetManager manager;
        if (upkeep > 0)
        {
            mana -= StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.AIRequestOverrideUpkeep(Round((double)upkeep * 0.8));
            if (mana < 0)
            {
                mana = 0;
            }
        }

        manager = StrategicMap.Current.Players.GetPlayers(source.Player).AIBudget;
        if (!StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.GetCheatEnabled())
        {
            result = manager.RequestBudgetExpense(0, budget, 0, mana, manager.DefaultBudget, 1 != 0);
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Spells.PAS:396
    public bool AICastSpellExpenseMade(Troop source, int budget, int mana, int upkeep)
    {
        bool result;
        AIBudgetManager manager;
        if (AIRequestCastSpellExpense(source, budget, mana, upkeep))
        {
            manager = StrategicMap.Current.Players.GetPlayers(source.Player).AIBudget;
            if (!StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.GetCheatEnabled())
            {
                result = manager.BudgetExpenseMade(0, budget, 0, mana, manager.DefaultBudget, (AIBudgetManager.ExpenseMode)2);
            }
            else
            {
                result = true;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    public virtual bool CanActivateCombat(Troop source, ref string reason) => StrategicMap.Current.Combat != null;

    public virtual void GetCombatInfo(Troop source, ref SpellCombatInfo info)
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

    public virtual void SetupCastSpellAction(Troop source, AICastSpellAction action)
    { action.SpellId = SpellId; action.SourceUnitId = source.UnitId; }

    // Spells.PAS:215. The casting view aliases the combat unit's hero.
    public void CombatSpellCast(CombatUnit source) =>
        CombatCastingDone(source, ((Hero)source.Troop).CastingMana(this));
}
