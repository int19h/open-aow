using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

internal static class CombatAbilityTables
{
    internal static FixedArray_int_3 TCombatUnit_CommandingTargetPriority__CommandAbilityIDs = FixedArray_int_3.From([29, 28, 148]);

    internal static FixedArray_int_3 TCombatUnit_CommandingTargetStrength__CommandAbilityIDs = FixedArray_int_3.From([29, 28, 148]);

    internal static TCombatPredictor_AddWall__TOriginalAssertText TCombatPredictor_AddWall__OriginalAssertFile = new TCombatPredictor_AddWall__TOriginalAssertText
    {
        RefCount = -1,
        Len = 38,
        Text = FixedArray_byte_128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 67, 111, 109, 98, 97, 116, 80, 114, 101, 100, 105, 99, 116, 111, 114, 46, 112, 97, 115, 0])
    };

    internal static TCombatPredictor_AddWall__TOriginalAssertText TCombatPredictor_AddWall__OriginalAssertMessage = new TCombatPredictor_AddWall__TOriginalAssertText
    {
        RefCount = -1,
        Len = 23,
        Text = FixedArray_byte_128.From([85, 115, 101, 32, 68, 101, 108, 101, 116, 101, 87, 97, 108, 108, 32, 105, 110, 115, 116, 101, 97, 100, 33, 0])
    };

    internal static TCombatSideIndex GetCombatSide(sbyte Attacker, sbyte AttackerRelation, sbyte Defender, sbyte DefenderRelation, sbyte Player, sbyte Relation)
    {
        TCombatSideIndex Result;
        if (Player != 0 && Attacker == 0 && Defender == 0)
        {
            Result = TCombatSideIndex.csNeutral;
        }
        else
        {
            if (Player == Attacker && (Player != 0 || AttackerRelation == Relation))
            {
                Result = TCombatSideIndex.csAttacker;
            }
            else
            {
                if (Player == Defender && (Player != 0 || DefenderRelation == Relation))
                {
                    Result = TCombatSideIndex.csDefender;
                }
                else
                {
                    if (Player == 0)
                    {
                        if (Attacker != 0 && Aow.Game.Magic.Abilities.AbilityRules.DiplomaticRelation(Attacker, AttackerRelation, Player, Relation) == TDiplomaticRelation.drPeace && Aow.Game.Magic.Abilities.AbilityRules.DiplomaticRelation(Defender, DefenderRelation, Player, Relation) != TDiplomaticRelation.drPeace)
                        {
                            Result = TCombatSideIndex.csAttacker;
                        }
                        else
                        {
                            if (Defender != 0 && Aow.Game.Magic.Abilities.AbilityRules.DiplomaticRelation(Defender, DefenderRelation, Player, Relation) == TDiplomaticRelation.drPeace && Aow.Game.Magic.Abilities.AbilityRules.DiplomaticRelation(Attacker, AttackerRelation, Player, Relation) != TDiplomaticRelation.drPeace)
                            {
                                Result = TCombatSideIndex.csDefender;
                            }
                            else
                            {
                                Result = TCombatSideIndex.csNeutral;
                            }
                        }
                    }
                    else
                    {
                        if (Attacker != 0 && Aow.Game.Magic.Abilities.AbilityRules.DiplomaticRelation(Attacker, AttackerRelation, Player, Relation) == TDiplomaticRelation.drAlliance && Aow.Game.Magic.Abilities.AbilityRules.DiplomaticRelation(Defender, DefenderRelation, Player, Relation) != TDiplomaticRelation.drAlliance)
                        {
                            Result = TCombatSideIndex.csAttacker;
                        }
                        else
                        {
                            if (Defender != 0 && Aow.Game.Magic.Abilities.AbilityRules.DiplomaticRelation(Defender, DefenderRelation, Player, Relation) == TDiplomaticRelation.drAlliance && Aow.Game.Magic.Abilities.AbilityRules.DiplomaticRelation(Attacker, AttackerRelation, Player, Relation) != TDiplomaticRelation.drAlliance)
                            {
                                Result = TCombatSideIndex.csDefender;
                            }
                            else
                            {
                                Result = TCombatSideIndex.csNeutral;
                            }
                        }
                    }
                }
            }
        }

        return Result;
    }

    internal static int CombatDID;

    internal static EClass P4CombatCasterClass = EClass.Of<TCombatSpellCaster>();

}
