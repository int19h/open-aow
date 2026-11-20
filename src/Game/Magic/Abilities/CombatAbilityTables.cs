using Aow.Game.Combat;
using Aow.Game.Session;

namespace Aow.Game.Magic.Abilities;

internal static class CombatAbilityTables
{
    internal static ReadOnlySpan<int> CommandingPriorityAbilityIds => [29, 28, 148];

    internal static ReadOnlySpan<int> CommandingStrengthAbilityIds => [29, 28, 148];

    internal static CombatSideIndex GetCombatSide(sbyte attacker, sbyte attackerRelation, sbyte defender, sbyte defenderRelation, sbyte player, sbyte relation)
    {
        return player != 0 && attacker == 0 && defender == 0
            ? CombatSideIndex.Neutral
            : player == attacker && (player != 0 || attackerRelation == relation)
                ? CombatSideIndex.Attacker
                : player == defender && (player != 0 || defenderRelation == relation)
                    ? CombatSideIndex.Defender
                    : player == 0
                        ? attacker != 0 && AbilityRules.DiplomaticRelation(attacker, attackerRelation, player, relation) == DiplomaticRelation.Peace && AbilityRules.DiplomaticRelation(defender, defenderRelation, player, relation) != DiplomaticRelation.Peace
                            ? CombatSideIndex.Attacker
                            : defender != 0 && AbilityRules.DiplomaticRelation(defender, defenderRelation, player, relation) == DiplomaticRelation.Peace && AbilityRules.DiplomaticRelation(attacker, attackerRelation, player, relation) != DiplomaticRelation.Peace
                                ? CombatSideIndex.Defender
                                : CombatSideIndex.Neutral
                        : attacker != 0 && AbilityRules.DiplomaticRelation(attacker, attackerRelation, player, relation) == DiplomaticRelation.Alliance && AbilityRules.DiplomaticRelation(defender, defenderRelation, player, relation) != DiplomaticRelation.Alliance
                            ? CombatSideIndex.Attacker
                            : defender != 0 && AbilityRules.DiplomaticRelation(defender, defenderRelation, player, relation) == DiplomaticRelation.Alliance && AbilityRules.DiplomaticRelation(attacker, attackerRelation, player, relation) != DiplomaticRelation.Alliance
                                ? CombatSideIndex.Defender
                                : CombatSideIndex.Neutral;
    }

}
