// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class SpellCastingAbility : MultiLevelAbility
{
    // AoWAb.PAS:4439
    public SpellCastingAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 52;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_spellCastingText);
        AbilityType = AbilityType.Enhancement;
        MaxLevel = 5;
        LevelCosts[1] = 20;
        LevelCosts[2] = 20;
        LevelCosts[3] = 20;
        LevelCosts[4] = 20;
        LevelCosts[5] = 20;
    }

    // AoWAb.PAS:4463
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner owner) => Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.Action, AbilityControlTypeElement.CombatAction);

    // AoWAb.PAS:4588
    public override void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
        FastBattle combat = commands.Combat;
        CombatObject source = commands.Source;
        if (source is CombatUnit sourceUnit)
        {
            CombatPlayer players = combat.CombatData.Players.GetPlayers(source.CombatPlayer);
            if (StrategicMap.Current.Players.GetPlayers(players.Player).PlayerType != PlayerType.Human || sourceUnit.Troop.GetCastingSpellId() == 0)
            {
                CombatSideIndex side = source.GetSide();
                CombatSideIndex opponentSide = source.GetOpponentSide();
                if (combat.Sides[(int)side].Strength < combat.Sides[(int)opponentSide].Strength * 2 || source.GetHitPoints() < 10 || (combat.CombatData.WallType != 0 && !combat.CombatData.WallBreached && CanCastBreachWallSpell(sourceUnit.Troop)))
                {
                    sbyte player = commands.Source.GetPlayer();
                    PlayerMagicControl magic = StrategicMap.Current.Players.GetPlayers(player).MagicControl;
                    Spell spell;
                    if (!magic.GetCheatEnabled())
                    {
                        string error = "";
                        for (int i = magic.Spells.Count - 1; i >= 0; i--)
                        {
                            int item = magic.Spells[i];
                            spell = GamePack.Current.SpellControl.GetSpell(item);
                            if (spell is not null && spell.SpellControlTypes.Contains(SpellControlTypeElement.Combat))
                            {
                                if (sourceUnit.Troop.CanCastSpellInstantly(spell.SpellId, spell.BaseManaCost, ref error))
                                {
                                    spell.FastBattlePrefetchCombatCommands(commands);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0, last = GamePack.Current.SpellControl.GetCount() - 1; i <= last; i++)
                        {
                            spell = GamePack.Current.SpellControl.GetSpell(i);
                            if (spell is not null && spell.SpellControlTypes.Contains(SpellControlTypeElement.Combat))
                            {
                                spell.FastBattlePrefetchCombatCommands(commands);
                            }
                        }
                    }
                }
            }
        }
    }

    // AoWAb.PAS:4484
    // The command list stores every command as the base command. Spell casting runs a spell command.
    // Another command fails. Null stays null and the spell id read fails on it.
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand command)
    {
        FastBattleSpellCommand spellCommand = (FastBattleSpellCommand)command;
        return GamePack.Current.SpellControl.GetSpell(spellCommand.SpellId).FastBattleExecuteCombatCommand(source, command: spellCommand);
    }

    // AoWAb.PAS:4479
    public override bool CanActivate(AbilityOwner owner, ref string reason) => owner is Hero;

    // AoWAb.PAS:4465
    public override bool Expand(AbilityOwner owner)
    {
        bool result = base.Expand(owner);
        if (result)
        {
            if (owner is Troop troop && StrategicMap.Current is not null)
            {
                StrategicMap.Current.Players.GetPlayers(troop.Player).MagicControl.Update();
            }
        }

        return result;
    }

    // AoWAb.PAS:4472
    public override bool Remove(AbilityOwner owner)
    {
        bool result = base.Remove(owner);
        if (result)
        {
            if (owner is Troop troop && StrategicMap.Current is not null)
            {
                StrategicMap.Current.Players.GetPlayers(troop.Player).MagicControl.Update();
            }
        }

        return result;
    }

    // AoWAb.PAS:4538
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        damageValues = default;
        if (attacker is CombatUnit attackerUnit)
        {
            sbyte player = attacker.GetPlayer();
            PlayerMagicControl magic = StrategicMap.Current.Players.GetPlayers(player).MagicControl;
            Troop source = attackerUnit.Troop;
            AbilityDamageValues values = default;
            for (int i = magic.Spells.Count - 1; i >= 0; i--)
            {
                int item = magic.Spells[i];
                Spell spell = GamePack.Current.SpellControl.GetSpell(item);
                if (spell is not null && spell.SpellControlTypes.Contains(SpellControlTypeElement.Combat) && source.GetCastingPointsMax() >= spell.BaseManaCost)
                {
                    spell.FastBattleGetDamageValues(attacker, defender, wallSituation, ref values);
                    if (damageValues.SourceDamage < values.SourceDamage)
                    {
                        damageValues = values;
                    }
                }
            }
        }
    }

    // AoWAb.PAS:4563
    public override void TacticalBattleGetDamageValues(CombatObject attacker, CombatObject defender, ref AbilityDamageValues damageValues)
    {
        damageValues = default;
        if (attacker is CombatUnit attackerUnit)
        {
            sbyte player = attacker.GetPlayer();
            PlayerMagicControl magic = StrategicMap.Current.Players.GetPlayers(player).MagicControl;
            Troop source = attackerUnit.Troop;
            AbilityDamageValues values = default;
            for (int i = magic.Spells.Count - 1; i >= 0; i--)
            {
                int item = magic.Spells[i];
                Spell spell = GamePack.Current.SpellControl.GetSpell(item);
                if (spell is not null && spell.SpellControlTypes.Contains(SpellControlTypeElement.Combat) && source.GetCastingPoints() >= spell.BaseManaCost)
                {
                    spell.FastBattleGetDamageValues(attacker, defender, WallSituation.None, ref values);
                    if (damageValues.SourceDamage < values.SourceDamage)
                    {
                        damageValues = values;
                    }
                }
            }
        }
    }

    // AoWAb.PAS:4452
    public override string GetLevelName(int level)
    {
        return level switch
        {
            0 => AbilityRules.TranslateResourceString(GetEngine(), s_spellCastingText),
            1 => $"{AbilityRules.TranslateResourceString(GetEngine(), s_spellCastingText)} I",
            2 => $"{AbilityRules.TranslateResourceString(GetEngine(), s_spellCastingText)} II",
            3 => $"{AbilityRules.TranslateResourceString(GetEngine(), s_spellCastingText)} III",
            4 => $"{AbilityRules.TranslateResourceString(GetEngine(), s_spellCastingText)} IV",
            5 => $"{AbilityRules.TranslateResourceString(GetEngine(), s_spellCastingText)} V",
            _ => "",
        };
    }

    // AoWAb.PAS:4488
    public bool CanCastBreachWallSpell(Troop troop)
    {
        bool result;
        if (troop is Hero hero)
        {
            PlayerMagicControl magic = StrategicMap.Current.Players.GetPlayers(troop.Player).MagicControl;
            result = magic.GetCheatEnabled();
            int i = magic.Spells.Count;
            SpellCombatInfo info = default;
            string error = "";
            while (!result && i != 0)
            {
                i -= 1;
                int item = magic.Spells[i];
                Spell spell = GamePack.Current.SpellControl.GetSpell(item);
                if (spell is not null && spell.SpellType == SpellType.Combat)
                {
                    spell.GetCombatInfo(troop, ref info);
                    bool condition = info.DamageTypes.Contains(DamageType.Wall);
                    if (condition)
                    {
                        condition = hero.CanCastCombatSpell(magic.Spells[i], reason: ref error);
                    }

                    result = condition;
                }
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // AoWAb.PAS:4515
    public bool AICanCastCombatSpell(CombatUnit source)
    {
        Troop troop = source.Troop;
        bool result = StrategicMap.Current.Players.GetPlayers(troop.Player).PlayerType != PlayerType.Human || troop.GetCastingSpellId() == 0;
        if (result)
        {
            CombatSideIndex side = source.GetSide();
            int sourceStrength = source.Combat.Sides[(int)side].Strength;
            if (sourceStrength <= 0)
            {
                sourceStrength = 0;
            }

            CombatSideIndex opponentSide = source.GetOpponentSide();
            int targetStrength = source.Combat.Sides[(int)opponentSide].Strength;
            if (targetStrength <= 0)
            {
                targetStrength = 0;
            }

            // { Original divides by zero when the opponent strength is zero. }
            float ratio = (float)((double)sourceStrength / targetStrength);
            return ((double)ratio < 1.5 && troop.GetCastingSpellId() == 0) || (double)ratio < 1 || (double)((double)source.GetHitPoints() * ratio) < 7;
        }

        return result;
    }

    private static readonly string s_spellCastingText = "Spell Casting";
}
