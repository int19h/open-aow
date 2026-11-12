
using Aow.Engine;
using Aow.Engine.Buffers;

using Aow.Game.Combat;

using Aow.Game.Session;

namespace Aow.Game.Magic.Abilities;

internal static class CombatAbilityTables
{
    internal static Int32Array3 s_commandingPriorityAbilityIds = Int32Array3.From([29, 28, 148]);

    internal static Int32Array3 s_commandingStrengthAbilityIds = Int32Array3.From([29, 28, 148]);

    internal static CombatSideIndex GetCombatSide(sbyte attacker, sbyte attackerRelation, sbyte defender, sbyte defenderRelation, sbyte player, sbyte relation)
    {
        CombatSideIndex result;
        if (player != 0 && attacker == 0 && defender == 0)
        {
            result = CombatSideIndex.Neutral;
        }
        else
        {
            if (player == attacker && (player != 0 || attackerRelation == relation))
            {
                result = CombatSideIndex.Attacker;
            }
            else
            {
                if (player == defender && (player != 0 || defenderRelation == relation))
                {
                    result = CombatSideIndex.Defender;
                }
                else
                {
                    if (player == 0)
                    {
                        if (attacker != 0 && AbilityRules.DiplomaticRelation(attacker, attackerRelation, player, relation) == DiplomaticRelation.Peace && AbilityRules.DiplomaticRelation(defender, defenderRelation, player, relation) != DiplomaticRelation.Peace)
                        {
                            result = CombatSideIndex.Attacker;
                        }
                        else
                        {
                            if (defender != 0 && AbilityRules.DiplomaticRelation(defender, defenderRelation, player, relation) == DiplomaticRelation.Peace && AbilityRules.DiplomaticRelation(attacker, attackerRelation, player, relation) != DiplomaticRelation.Peace)
                            {
                                result = CombatSideIndex.Defender;
                            }
                            else
                            {
                                result = CombatSideIndex.Neutral;
                            }
                        }
                    }
                    else
                    {
                        if (attacker != 0 && AbilityRules.DiplomaticRelation(attacker, attackerRelation, player, relation) == DiplomaticRelation.Alliance && AbilityRules.DiplomaticRelation(defender, defenderRelation, player, relation) != DiplomaticRelation.Alliance)
                        {
                            result = CombatSideIndex.Attacker;
                        }
                        else
                        {
                            if (defender != 0 && AbilityRules.DiplomaticRelation(defender, defenderRelation, player, relation) == DiplomaticRelation.Alliance && AbilityRules.DiplomaticRelation(attacker, attackerRelation, player, relation) != DiplomaticRelation.Alliance)
                            {
                                result = CombatSideIndex.Defender;
                            }
                            else
                            {
                                result = CombatSideIndex.Neutral;
                            }
                        }
                    }
                }
            }
        }

        return result;
    }

    internal static int CombatDebugChannel { get; set; }

}
