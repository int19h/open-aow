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
    public SpellCastingAbility(ECustomNode owner) : base(owner)
    {
        AbilityId = 52;
        AbilityName = AbilityRules.TranslateResourceString(s_spellCastingText);
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
        FastBattle combat;
        CombatObject source;
        string error = "";
        PlayerMagicControl magic;
        Spell spell;
        combat = commands.Combat;
        source = commands.Source;
        if (source is CombatUnit)
        {
            var players = combat.CombatData.Players.GetPlayers(source.CombatPlayer);
            if (AoWHSMap.Current.Players.GetPlayers(players.Player).PlayerType != PlayerType.Human || ((Hero)((CombatUnit)source).Troop).CastingSpellId == 0)
            {
                var side = source.GetSide();
                var opponentSide = source.GetOpponentSide();
                if (combat.Sides[(int)side].Strength < combat.Sides[(int)opponentSide].Strength * 2 || source.GetHitPoints() < 10 || combat.CombatData.WallType != 0 && !combat.CombatData.WallBreached && CanCastBreachWallSpell(((CombatUnit)source).Troop))
                {
                    var player = commands.Source.GetPlayer();
                    magic = AoWHSMap.Current.Players.GetPlayers(player).MagicControl;
                    if (!magic.GetCheatEnabled())
                    {
                        for (int I = magic.Spells.Count - 1; I >= 0; I--)
                        {
                            var item = magic.Spells[I];
                            spell = AoWHSSet.Current.SpellControl.GetSpell(item);
                            if (spell != null && spell.SpellControlTypes.Contains(SpellControlTypeElement.Combat))
                            {
                                if (((CombatUnit)commands.Source).Troop is Hero)
                                {
                                    if (((Hero)((CombatUnit)commands.Source).Troop).CanCastSpellInstantly(spell.SpellId, spell.BaseManaCost, ref error))
                                    {
                                        spell.FastBattlePrefetchCombatCommands(commands);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int I = 0, last = AoWHSSet.Current.SpellControl.GetCount() - 1; I <= last; I++)
                        {
                            spell = AoWHSSet.Current.SpellControl.GetSpell(I);
                            if (spell != null && spell.SpellControlTypes.Contains(SpellControlTypeElement.Combat))
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
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command) => AoWHSSet.Current.SpellControl.GetSpell(((FastBattleSpellCommand)command).SpellId).FastBattleExecuteCombatCommand(source, command);

    // AoWAb.PAS:4479
    public override bool CanActivate(AbilityOwner owner, ref string reason)
    {
        bool result;
        if (owner is Hero)
        {
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWAb.PAS:4465
    public override bool Expand(AbilityOwner owner)
    {
        bool result = base.Expand(owner);
        if (result)
        {
            if (owner is Troop && AoWHSMap.Current != null)
            {
                AoWHSMap.Current.Players.GetPlayers(((Troop)owner).Player).MagicControl.Update();
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
            if (owner is Troop && AoWHSMap.Current != null)
            {
                AoWHSMap.Current.Players.GetPlayers(((Troop)owner).Player).MagicControl.Update();
            }
        }

        return result;
    }

    // AoWAb.PAS:4538
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        Spell spell;
        Troop source;
        PlayerMagicControl magic;
        AbilityDamageValues values = default;
        damageValues = default;
        if (attacker is CombatUnit)
        {
            var player = attacker.GetPlayer();
            magic = AoWHSMap.Current.Players.GetPlayers(player).MagicControl;
            source = ((CombatUnit)attacker).Troop;
            for (int I = magic.Spells.Count - 1; I >= 0; I--)
            {
                var item = magic.Spells[I];
                spell = AoWHSSet.Current.SpellControl.GetSpell(item);
                if (spell != null && spell.SpellControlTypes.Contains(SpellControlTypeElement.Combat) && source.GetCastingPointsMax() >= spell.BaseManaCost)
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
        Spell spell;
        Troop source;
        PlayerMagicControl magic;
        AbilityDamageValues values = default;
        damageValues = default;
        if (attacker is CombatUnit)
        {
            var player = attacker.GetPlayer();
            magic = AoWHSMap.Current.Players.GetPlayers(player).MagicControl;
            source = ((CombatUnit)attacker).Troop;
            for (int I = magic.Spells.Count - 1; I >= 0; I--)
            {
                var item = magic.Spells[I];
                spell = AoWHSSet.Current.SpellControl.GetSpell(item);
                if (spell != null && spell.SpellControlTypes.Contains(SpellControlTypeElement.Combat) && source.GetCastingPoints() >= spell.BaseManaCost)
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
        string result = "";
        switch (level)
        {
            case 0:
                result = AbilityRules.TranslateResourceString(s_spellCastingText);
                break;
            case 1:
                result = AbilityRules.TranslateResourceString(s_spellCastingText) + " I";
                break;
            case 2:
                result = AbilityRules.TranslateResourceString(s_spellCastingText) + " II";
                break;
            case 3:
                result = AbilityRules.TranslateResourceString(s_spellCastingText) + " III";
                break;
            case 4:
                result = AbilityRules.TranslateResourceString(s_spellCastingText) + " IV";
                break;
            case 5:
                result = AbilityRules.TranslateResourceString(s_spellCastingText) + " V";
                break;
        }

        return result;
    }

    // AoWAb.PAS:4488
    public bool CanCastBreachWallSpell(Troop troop)
    {
        bool result;
        int I;
        Spell spell;
        SpellCombatInfo info = default;
        string error = "";
        PlayerMagicControl magic;
        if (troop is Hero)
        {
            magic = AoWHSMap.Current.Players.GetPlayers(troop.Player).MagicControl;
            result = magic.GetCheatEnabled();
            I = magic.Spells.Count;
            while (!result && I != 0)
            {
                I -= 1;
                var item = magic.Spells[I];
                spell = AoWHSSet.Current.SpellControl.GetSpell(item);
                if (spell != null && spell.SpellType == SpellType.Combat)
                {
                    spell.GetCombatInfo(troop, ref info);
                    bool condition = info.DamageTypes.Contains(DamageType.Wall);
                    if (condition)
                    {
                        var fSpellsItem = magic.Spells[I];
                        condition = ((Hero)troop).CanCastCombatSpell(fSpellsItem, ref error);
                    }

                    result = condition;
                }
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWAb.PAS:4515
    public bool AICanCastCombatSpell(CombatUnit source)
    {
        Hero hero;
        int sourceStrength;
        int targetStrength;
        float ratio;
        hero = (Hero)source.Troop;
        bool result = AoWHSMap.Current.Players.GetPlayers(hero.Player).PlayerType != PlayerType.Human || hero.CastingSpellId == 0;
        if (result)
        {
            var side = source.GetSide();
            sourceStrength = source.Combat.Sides[(int)side].Strength;
            if (sourceStrength <= 0)
            {
                sourceStrength = 0;
            }

            var opponentSide = source.GetOpponentSide();
            targetStrength = source.Combat.Sides[(int)opponentSide].Strength;
            if (targetStrength <= 0)
            {
                targetStrength = 0;
            }

            // { Original divides by zero when the opponent strength is zero. }
            ratio = (float)((double)sourceStrength / targetStrength);
            if (((double)ratio >= 1.5 || hero.CastingSpellId != 0) && (double)ratio >= 1 && (double)((double)source.GetHitPoints() * ratio) >= 7)
            {
                result = false;
            }
            else
            {
                result = true;
            }
        }

        return result;
    }

    private static readonly string s_spellCastingText = "Spell Casting";
}
