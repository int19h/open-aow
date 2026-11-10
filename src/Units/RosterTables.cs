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

internal static class RosterTables
{
    internal static TUnitEnchantmentAbility_Expand__TOriginalAssertText TUnitEnchantmentAbility_Expand__OriginalAssertFile = new TUnitEnchantmentAbility_Expand__TOriginalAssertText
    {
        RefCount = -1,
        Len = 28,
        Text = FixedArray_byte_128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 65, 111, 87, 65, 98, 46, 80, 65, 83, 0])
    };

    internal static TUnitEnchantmentAbility_Expand__TOriginalAssertText TUnitEnchantmentAbility_Expand__OriginalAssertMessage = new TUnitEnchantmentAbility_Expand__TOriginalAssertText
    {
        RefCount = -1,
        Len = 17,
        Text = FixedArray_byte_128.From([65, 115, 115, 101, 114, 116, 105, 111, 110, 32, 102, 97, 105, 108, 117, 114, 101, 0])
    };

    internal static int GetUnitIndexAIPriority(int UnitIndex, sbyte Player, sbyte Relation)
    {
        int Result;
        TUnitResource Resource;
        int Value;
        TUnit Unit_;
        TAdjustableUnit DefaultUnit;
        Resource = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(UnitIndex);
        if (Resource != null && Resource.GetTransportCapacity() == 0)
        {
            if (Player != 0)
            {
                if (Resource.FRace != -1)
                {
                    Value = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(Resource.FRace).GetPlayerRelationValue(Player);
                }
                else
                {
                    var players = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player);
                    var alignment = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(players.FPlayerRace).GetAlignment();
                    Value = Aow.Game.Magic.Abilities.AlignmentTables.dAlignmentToStatusValue[(int)Resource.FAlignment][(int)alignment];
                }
            }
            else
            {
                var iRtoAlignment = Aow.Game.Magic.Abilities.AbilityRules.IRtoAlignment(Relation);
                Value = Aow.Game.Magic.Abilities.AlignmentTables.dAlignmentToStatusValue[(int)Resource.FAlignment][(int)iRtoAlignment];
            }

            if (Value >= 50)
            {
                Unit_ = new TUnit(null);
                Unit_.SetUnitResource(Resource);
                DefaultUnit = new TAdjustableUnit(null);
                DefaultUnit.MakeDefaultLevelUnit(1);
                if (Unit_.GetCombatValue(DefaultUnit, 0) > 0)
                {
                    Result = Unit_.GetStrengthMax(Aow.Game.Magic.Abilities.AbilityRules.dtAll - TSet<TDamageType>.Of(TDamageType.dtWall)) / 100 + (Resource.FLevel * 100 + Resource.FMoves * 10 + Value / 2);
                    if (Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(57) || Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(58))
                    {
                        Result += 100;
                    }

                    if (Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(13))
                    {
                        Result += 150;
                    }

                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDay > 20 && (Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(20) || Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(19) || Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(18) || Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(121) || Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(88) || Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(89)))
                    {
                        Result += 100;
                    }

                    if (Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(1) || Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(36) || Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(66) || Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(24) || Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(44) || Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(117) && Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(21))
                    {
                        Result += 100;
                    }

                    if (Resource.FAbilities[(int)TUnitRank.urNone].GetAbSet(65))
                    {
                        Result = Result / 2;
                    }
                }
                else
                {
                    Result = -1;
                }

                DefaultUnit?.Dispose();
                Unit_?.Dispose();
            }
            else
            {
                Result = -1;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    internal static EClass P4UnitSelectionClass = EClass.Of<TUnitUserList>();

    internal static TSet<THeroType> AllHeroTypes = TSet<THeroType>.Of(THeroType.htHero, THeroType.htLeader, THeroType.htHeroLeader, THeroType.htMerchant);

}
