#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
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

using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Spells;

// (name, control types, spell ID, base mana cost, sphere, level, type, text, sounds, images.
// remain unresolved Integers)
public class Spell : EngineObject
{

    // Spells.PAS:10
    public Spell(CustomNode owner) : base(owner)
    {
        SpellText = new DelphiStringList();
        CastSounds = new SfxLibrary(null) { Holder = CreationHolder() };
        CastImages = new ImageSequenceList(GamePack.Current.ImageLibraryManager, imageLibrary: null);
        ResearchCost = 1;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global);
    }

    public string SpellName { get; protected set; } = "";
    public Set<SpellControlTypeElement> SpellControlTypes { get; protected set; }
    public int SpellId { get; protected set; }

    public int BaseManaCost { get; protected set; }
    public int ResearchCost { get; private set; }
    public int ManaUpkeep { get; protected set; }
    public MagicSphere MagicSphere { get; protected set; }
    public byte SpellLevel { get; protected set; }

    public SpellType SpellType { get; protected set; }
    // stGlobal guard
    public DelphiStringList SpellText { get; }
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
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.Lines(persister, name: "SpellText", lines: SpellText);
        persister.Object("CastSounds", objectValue: CastSounds);
        PersistSupport.ImageSequences(persister, name: "CastImages", list: CastImages);
        BaseManaCost = persister.Int32("BaseManaCost", value: BaseManaCost);
        ManaUpkeep = persister.Int32("ManaUpkeep", value: ManaUpkeep);
        ResearchCost = persister.Int32("ResearchCost", value: ResearchCost);
        MagicSphere = persister.Enum("MagicSphere", value: MagicSphere);
        SpellLevel = persister.Byte("SpellLevel", value: SpellLevel);
    }

    // Spells.PAS:224
    public virtual void PlayDefaultCastEffects(sbyte player, sbyte X, sbyte Y, sbyte layer)
    {
        if (StrategicMap.Current.Visible(X, Y, L: layer, radius: 0))
        {
            _ = StrategicMap.Current.ValidateObservation(player, X, Y, layer, radius: 0);
            if (GamePack.Current.SpellControl[SpellId].CastImages.GetImageSequence(0) is not null)
            {
                ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
                animation.SetImageSequence(GamePack.Current.SpellControl[SpellId].CastImages.GetImageSequence(0));
                _ = StrategicMap.Current.PlaceHex(animation, X, Y, L: layer);
            }

            if (StrategicMap.Current.SeatedPlayer == player || StrategicMap.Current.Watching(X, Y, L: layer, radius: 0))
            {
                _ = GamePack.Current.SpellControl[SpellId].CastSounds.Play(0);
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
    public virtual bool Enabled => true;

    // Spells.PAS:43
    public virtual bool CanActivate(Troop source, ref string messageText)
    {
        if (SpellType == SpellType.Global)
        {
            if (StrategicMap.Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentType.SpellWard))
            {
                messageText = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CannotCastGlobalEnchantmentWhenSpellWardIsActiveText);
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }

    public bool CanActivateStatic(Troop source, ref string messageText)
    {
        if (SpellType == SpellType.Global)
        {
            if (StrategicMap.Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentType.SpellWard))
            {
                messageText = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CannotCastGlobalEnchantmentWhenSpellWardIsActiveText);
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }

    // Spells.PAS:54
    public virtual bool Activate(Troop source)
    {
        if (StrategicMap.Current.SeatedPlayer == source.Player)
        {
            MapCoordinate coordinate77 = source.GetLocation();
            sbyte x = coordinate77.X;
            sbyte y = coordinate77.Y;
            sbyte l = coordinate77.MapLevel;
            if (x != -1)
            {
                StrategicMap.Current.MakeVisible(x, y, l, radius: 3);
            }
        }

        // { Original returns False after optionally centering the view. }
        return false;
    }

    public bool ActivateStatic(Troop source)
    {
        if (StrategicMap.Current.SeatedPlayer == source.Player)
        {
            MapCoordinate coordinate78 = source.GetLocation();
            sbyte x = coordinate78.X;
            sbyte y = coordinate78.Y;
            sbyte l = coordinate78.MapLevel;
            if (x != -1)
            {
                StrategicMap.Current.MakeVisible(x, y, l, radius: 3);
            }
        }

        // { Original returns False after optionally centering the view. }
        return false;
    }

    // Spells.PAS:90
    public virtual bool ActivateCombat(Troop source)
    {
        string reason = "";
        if (CanActivateCombat(source, ref reason))
        {
            CombatSpellCaster caster = new(null)
            {
                Holder = CreationHolder(),
                CombatCasterSpellId = SpellId,
                CombatCasterSourceId = source.UnitId
            };
            StrategicMap.Current.CombatMap.CombatStartedEventsStore.Raise(caster);
            _ = caster.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Spells.PAS:158
    public virtual CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana) => default;

    // The caller names the action this spell builds. Another action class throws. Null stays null.
    public TAction CreateCombatAction<TAction>(CombatObject source, CombatObject target, bool chargeMana) where TAction : CombatAction
        => (TAction)CreateCombatAction(source, target: target, chargeMana: chargeMana);

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
    public virtual void TacticalBattleGetDamageValues(CombatObject source, CombatObject target, ref AbilityDamageValues values) => GetCombatDamageValues(source, target, ref values);

    // Spells.PAS:339
    public virtual int TacticalBattleGetDamageValue(CombatObject source, CombatObject target) => GetCombatDamageValue(source, target);

    // Spells.PAS:320
    public virtual void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
    }

    // Spells.PAS:322
    public virtual bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand command) => false;

    // Spells.PAS:316
    public virtual void FastBattleGetDamageValues(CombatObject source, CombatObject target, WallSituation wallSituation, ref AbilityDamageValues values) => values = default;

    // Spells.PAS:193
    public void CastingDone(Troop source, int mana)
    {
        if (source is Hero hero)
        {
            hero.CastingDone(SpellId, mana);
        }
    }

    // Spells.PAS:198
    public void CombatCastingDone(CombatObject source, int mana)
    {
        if (source is CombatUnit combatUnit && combatUnit.Troop is Hero hero)
        {
            hero.CastingDone(SpellId, mana);
            combatUnit.UpdateWallCombatFeatures();
            // Spells.PAS:198 tests the battle. A fast battle creates only fast battle units, and a
            // plain battle also creates them (the replay viewer), so both tests stay.
            if (source is FastBattleUnit { Combat: FastBattle } fastUnit)
            {
                fastUnit.UpdateWallSiegeObjectRegistration();
            }
        }
    }

    // Spells.PAS:371
    public bool AIRequestCastSpellExpense(Troop source, int budget, int mana, int upkeep)
    {
        if (upkeep > 0)
        {
            mana -= StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.AIRequestOverrideUpkeep(Round(upkeep * 0.8));
            if (mana < 0)
            {
                mana = 0;
            }
        }

        AIBudgetManager manager = StrategicMap.Current.Players.GetPlayers(source.Player).AIBudget;
        return StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.GetCheatEnabled() || manager.RequestBudgetExpense(0, mana: budget, goldUpkeep: 0, manaUpkeep: mana, budget: manager.DefaultBudget, usePool: 1 != 0);
    }

    // Spells.PAS:396
    public bool AICastSpellExpenseMade(Troop source, int budget, int mana, int upkeep)
    {
        if (AIRequestCastSpellExpense(source, budget, mana, upkeep))
        {
            AIBudgetManager manager = StrategicMap.Current.Players.GetPlayers(source.Player).AIBudget;
            return StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.GetCheatEnabled() || manager.BudgetExpenseMade(0, mana: budget, goldUpkeep: 0, manaUpkeep: mana, budget: manager.DefaultBudget, mode: (AIBudgetManager.ExpenseMode)2);
        }
        else
        {
            return false;
        }
    }

    public virtual bool CanActivateCombat(Troop source, ref string reason) => StrategicMap.Current.Combat is not null;

    public virtual void GetCombatInfo(Troop source, ref SpellCombatInfo info)
    {
        info.Attack = 0;
        info.Damage = 0;
        info.DamageTypes = default;
        info.RepeatCount = 1;
        info.Range = 0;
        info.Radius = 0;
        info.TargetTypes = default;
    }

    public virtual AICastSpellAction CreateCastSpellAction()
    {
        AICastSpellAction action = new(null) { Holder = CreationHolder() };
        // TEObject.Create only calls SetOwner. It bypasses descendant constructor bodies.
        try
        {
            action.SetOwner(null);
        }
        catch
        {
            action.Dispose();
            throw;
        }
        return action;
    }

    public virtual void SetupCastSpellAction(Troop source, AICastSpellAction action)
    {
        action.SpellId = SpellId;
        action.SourceUnitId = source.UnitId;
    }

    // Spells.PAS:215. The casting view aliases the combat unit's hero.
    public void CombatSpellCast(CombatUnit source)
        => CombatCastingDone(source, mana: source.Troop.CastingMana(this));
}
