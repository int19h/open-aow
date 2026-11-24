using Aow.Game.Combat;
using Aow.Game.Session;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

internal static class CombatAbilityTables
{
    internal static ReadOnlySpan<int> CommandingPriorityAbilityIds => [29, 28, 148];

    internal static ReadOnlySpan<int> CommandingStrengthAbilityIds => [29, 28, 148];

    internal static CombatSideIndex GetCombatSide(int attacker, int attackerRelation, PlayerId? defender, int defenderRelation, sbyte player, int relation)
    {
        return player != 0 && attacker == 0 && defender == PlayerId.Independent ? CombatSideIndex.Neutral
            : player == attacker && (player != 0 || attackerRelation == relation)
                ? CombatSideIndex.Attacker
                : player == PlayerIdExtensions.ToOriginal(defender) && (player != 0 || defenderRelation == relation)
                    ? CombatSideIndex.Defender
                    : player == 0
                        ? attacker != 0 && AbilityRules.DiplomaticRelation(PlayerId.FromOriginal((sbyte)attacker), independentRelation: attackerRelation.TruncateToSByte(), otherPlayer: PlayerId.FromOriginal(player), otherIndependentRelation: relation) == DiplomaticRelation.Peace && AbilityRules.DiplomaticRelation(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(defender)), independentRelation: defenderRelation, otherPlayer: PlayerId.FromOriginal(player), otherIndependentRelation: relation) != DiplomaticRelation.Peace
                            ? CombatSideIndex.Attacker
                            : defender != PlayerId.Independent && AbilityRules.DiplomaticRelation(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(defender)), independentRelation: defenderRelation, otherPlayer: PlayerId.FromOriginal(player), otherIndependentRelation: relation) == DiplomaticRelation.Peace && AbilityRules.DiplomaticRelation(PlayerId.FromOriginal((sbyte)attacker), independentRelation: attackerRelation.TruncateToSByte(), otherPlayer: PlayerId.FromOriginal(player), otherIndependentRelation: relation) != DiplomaticRelation.Peace
                                ? CombatSideIndex.Defender
                                : CombatSideIndex.Neutral
                        : attacker != 0 && AbilityRules.DiplomaticRelation(PlayerId.FromOriginal((sbyte)attacker), independentRelation: attackerRelation.TruncateToSByte(), otherPlayer: PlayerId.FromOriginal(player), otherIndependentRelation: relation) == DiplomaticRelation.Alliance && AbilityRules.DiplomaticRelation(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(defender)), independentRelation: defenderRelation, otherPlayer: PlayerId.FromOriginal(player), otherIndependentRelation: relation) != DiplomaticRelation.Alliance
                            ? CombatSideIndex.Attacker
                            : defender != PlayerId.Independent && AbilityRules.DiplomaticRelation(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(defender)), independentRelation: defenderRelation, otherPlayer: PlayerId.FromOriginal(player), otherIndependentRelation: relation) == DiplomaticRelation.Alliance && AbilityRules.DiplomaticRelation(PlayerId.FromOriginal((sbyte)attacker), independentRelation: attackerRelation.TruncateToSByte(), otherPlayer: PlayerId.FromOriginal(player), otherIndependentRelation: relation) != DiplomaticRelation.Alliance
                                ? CombatSideIndex.Defender
                                : CombatSideIndex.Neutral;
    }
}
