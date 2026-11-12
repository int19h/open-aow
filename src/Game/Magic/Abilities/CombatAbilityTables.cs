
using Aow.Engine;
using Aow.Engine.Buffers;

using Aow.Game.Combat;

using Aow.Game.Session;

namespace Aow.Game.Magic.Abilities;

internal static class CombatAbilityTables
{
    internal static Int32Array3 s_commandingPriorityAbilityIds = Int32Array3.From([29, 28, 148]);

    internal static Int32Array3 s_commandingStrengthAbilityIds = Int32Array3.From([29, 28, 148]);

    internal static CombatPredictor_AddWall__TOriginalAssertText TCombatPredictor_AddWall__OriginalAssertFile = new CombatPredictor_AddWall__TOriginalAssertText
    {
        RefCount = -1,
        Len = 38,
        Text = ByteArray128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 67, 111, 109, 98, 97, 116, 80, 114, 101, 100, 105, 99, 116, 111, 114, 46, 112, 97, 115, 0])
    };

    internal static CombatPredictor_AddWall__TOriginalAssertText TCombatPredictor_AddWall__OriginalAssertMessage = new CombatPredictor_AddWall__TOriginalAssertText
    {
        RefCount = -1,
        Len = 23,
        Text = ByteArray128.From([85, 115, 101, 32, 68, 101, 108, 101, 116, 101, 87, 97, 108, 108, 32, 105, 110, 115, 116, 101, 97, 100, 33, 0])
    };

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

    internal static int s_combatDebugChannel;

    internal static EngineClass s_combatCasterClass = EngineClass.Of<CombatSpellCaster>();

}
