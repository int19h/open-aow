using Aow.Game.Combat;
using Aow.Game.Session;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

internal static class CombatAbilityTables
{
    internal static ReadOnlySpan<int> CommandingPriorityAbilityIds => [29, 28, 148];

    internal static ReadOnlySpan<int> CommandingStrengthAbilityIds => [29, 28, 148];

    internal static CombatSideIndex GetCombatSide(int attacker, int attackerRelation, int defender, int defenderRelation, sbyte player, int relation)
    {
        return player != 0 && attacker == 0 && defender == 0
            ? CombatSideIndex.Neutral
            : player == attacker && (player != 0 || attackerRelation == relation)
                ? CombatSideIndex.Attacker
                : player == defender && (player != 0 || defenderRelation == relation)
                    ? CombatSideIndex.Defender
                    : player == 0
                        ? attacker != 0 && AbilityRules.DiplomaticRelation((sbyte)attacker, independentRelation: attackerRelation.TruncateToSByte(), otherPlayer: player, otherIndependentRelation: relation) == DiplomaticRelation.Peace && AbilityRules.DiplomaticRelation((sbyte)defender, independentRelation: defenderRelation, otherPlayer: player, otherIndependentRelation: relation) != DiplomaticRelation.Peace
                            ? CombatSideIndex.Attacker
                            : defender != 0 && AbilityRules.DiplomaticRelation((sbyte)defender, independentRelation: defenderRelation, otherPlayer: player, otherIndependentRelation: relation) == DiplomaticRelation.Peace && AbilityRules.DiplomaticRelation((sbyte)attacker, independentRelation: attackerRelation.TruncateToSByte(), otherPlayer: player, otherIndependentRelation: relation) != DiplomaticRelation.Peace
                                ? CombatSideIndex.Defender
                                : CombatSideIndex.Neutral
                        : attacker != 0 && AbilityRules.DiplomaticRelation((sbyte)attacker, independentRelation: attackerRelation.TruncateToSByte(), otherPlayer: player, otherIndependentRelation: relation) == DiplomaticRelation.Alliance && AbilityRules.DiplomaticRelation((sbyte)defender, independentRelation: defenderRelation, otherPlayer: player, otherIndependentRelation: relation) != DiplomaticRelation.Alliance
                            ? CombatSideIndex.Attacker
                            : defender != 0 && AbilityRules.DiplomaticRelation((sbyte)defender, independentRelation: defenderRelation, otherPlayer: player, otherIndependentRelation: relation) == DiplomaticRelation.Alliance && AbilityRules.DiplomaticRelation((sbyte)attacker, independentRelation: attackerRelation.TruncateToSByte(), otherPlayer: player, otherIndependentRelation: relation) != DiplomaticRelation.Alliance
                                ? CombatSideIndex.Defender
                                : CombatSideIndex.Neutral;
    }
}
