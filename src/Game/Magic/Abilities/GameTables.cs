
using Aow.Engine.Buffers;

using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

public static class GameTables
{
    public static bool ValidateGameEngine()
    {
        if (GameEngine == null && ToolServices == null)
        {
            GameEngine.Create(null);
        }

        return GameEngine != null;
    }

    internal static StringArray9 s_magicSphereTexts = StringArray9.From([]);

    internal static StringArray3 s_spellTypeTexts = StringArray3.From([]);

    internal static readonly string s_unknownText = "Unknown";

    internal static readonly string s_noneText = "None";

    internal static readonly string s_aggressorText = "Aggressor";

    internal static readonly string s_airText = "Air";

    internal static readonly string s_allianceText = "Alliance";

    internal static readonly string s_azracPluralText = "Azracs";

    internal static readonly string s_azracText = "Azrac";

    internal static readonly string s_cpuEmperorText = "CPU Emperor";

    internal static readonly string s_cpuKingText = "CPU King";

    internal static readonly string s_cpuKnightText = "CPU Knight";

    internal static readonly string s_cpuLordText = "CPU Lord";

    internal static readonly string s_cpuWarriorText = "CPU Squire";

    internal static readonly string s_fireText = "Fire";

    internal static readonly string s_combatText = "Combat";

    internal static readonly string s_cosmosText = "Cosmos";

    internal static readonly string s_darkElfPluralText = "Dark Elves";

    internal static readonly string s_darkElfText = "DarkElf";

    internal static readonly string s_deathText = "Death";

    internal static readonly string s_deathStormText = "Death Storm";

    internal static readonly string s_defenderText = "Defender";

    internal static readonly string s_divineStormText = "Divine Storm";

    internal static readonly string s_dwarfPluralText = "Dwarves";

    internal static readonly string s_dwarfText = "Dwarf";

    internal static readonly string s_elfPluralText = "Elves";

    internal static readonly string s_elfText = "Elf";

    internal static readonly string s_enchantmentText = "Enchantment";

    internal static readonly string s_enhancementText = "Enhancement";

    internal static readonly string s_expanderText = "Expander";

    internal static readonly string s_fireStormText = "Fire Storm";

    internal static readonly string s_freeText = "Free";

    internal static readonly string s_friendlyText = "Friendly";

    internal static readonly string s_frostlingPluralText = "Frostlings";

    internal static readonly string s_frostlingText = "Frostling";

    internal static readonly string s_globalText = "Global";

    internal static readonly string s_goblinPluralText = "Goblins";

    internal static readonly string s_goblinText = "Goblin";

    internal static readonly string s_halflingPluralText = "Halflings";

    internal static readonly string s_halflingText = "Halfling";

    internal static readonly string s_handicapText = "Handicap";

    internal static readonly string s_highmanPluralText = "Highmen";

    internal static readonly string s_highmanText = "Highman";

    internal static readonly string s_hostileText = "Hostile";

    internal static readonly string s_humanPluralText = "Humans";

    internal static readonly string s_humanText = "Human";

    internal static readonly string s_iceStormText = "Ice Storm";

    internal static readonly string s_independentText = "Independent";

    internal static readonly string s_lifeText = "Life";

    internal static readonly string s_lightningStormText = "Lightning Storm";

    internal static readonly string s_lizardPluralText = "Lizards";

    internal static readonly string s_lizardText = "Lizard";

    internal static readonly string s_movementText = "Movement";

    internal static readonly string s_neutralText = "Neutral";

    internal static readonly string s_normalText = "Normal";

    internal static readonly string s_orcPluralText = "Orcs";

    internal static readonly string s_orcText = "Orc";

    internal static readonly string s_pestilenceStormText = "Pestilence cloud";

    internal static readonly string s_politeText = "Polite";

    internal static readonly string s_randomText = "Random";

    internal static readonly string s_scorcherText = "Scorcher";

    internal static readonly string s_secretText = "Secret";

    internal static readonly string s_unitText = "Unit";

    internal static readonly string s_waryText = "Wary";

    public static GameEngine GameEngine;

    internal static StringArray5 s_abilityTypeTexts = StringArray5.From([]);

    internal static StringArray8 s_playerTypeTexts = StringArray8.From([]);

    internal static StringArray6 s_playerBehaviorTexts = StringArray6.From([]);

    internal static StringArray4 s_diplomaticRelationTexts = StringArray4.From([]);

    internal static string[] s_statusTexts = Strings(5);

    internal static string[] s_raceNamesPluralTexts = Strings(13);

    internal static StringArray13 s_raceNames = StringArray13.From([]);

    internal static readonly string s_undeadText = "Undead";

    internal static readonly string s_undeadPluralText = "Undead(plural)";

    internal static readonly string s_independentsText = "Independents";

    internal static readonly string s_waterText = "Water";

    internal static readonly string s_earthText = "Earth";

    internal static StringArray7 s_stormTypeTexts = StringArray7.From([s_noneText, s_fireStormText, s_deathStormText, s_divineStormText, s_iceStormText, s_lightningStormText, s_pestilenceStormText]);

    internal static SingleArray20 s_hitProbabilities;

    internal static readonly string s_warText = "War";

    internal static readonly string s_peaceText = "Peace";

    internal static void InitializeHitProbabilities()
    {
        for (int I = 0; I <= 19; I++)
        {
            s_hitProbabilities[I] = (float)((double)(20 - I) * 0.05);
        }
    }

    internal static AbilityPlugin s_abilityPlugin;

    internal static void Initialize()
    {
        s_abilityTypeTexts[(int)AbilityType.Enhancement] = s_enhancementText;
        s_abilityTypeTexts[(int)AbilityType.Handicap] = s_handicapText;
        s_abilityTypeTexts[(int)AbilityType.Movement] = s_movementText;
        s_abilityTypeTexts[(int)AbilityType.Combat] = s_combatText;
        s_abilityTypeTexts[(int)AbilityType.Enchantment] = s_enchantmentText;
        s_playerTypeTexts[0] = s_humanText;
        s_playerTypeTexts[1] = s_cpuWarriorText;
        s_playerTypeTexts[2] = s_cpuKnightText;
        s_playerTypeTexts[3] = s_cpuLordText;
        s_playerTypeTexts[4] = s_independentText;
        s_playerTypeTexts[5] = s_freeText;
        s_playerTypeTexts[6] = s_cpuKingText;
        s_playerTypeTexts[7] = s_cpuEmperorText;
        // Original InitResStrings table: race-name entries
        s_raceNames[0] = s_humanText;
        s_raceNames[1] = s_azracText;
        s_raceNames[2] = s_lizardText;
        s_raceNames[3] = s_frostlingText;
        s_raceNames[4] = s_elfText;
        s_raceNames[5] = s_halflingText;
        s_raceNames[6] = s_dwarfText;
        s_raceNames[7] = s_highmanText;
        s_raceNames[8] = s_darkElfText;
        s_raceNames[9] = s_orcText;
        s_raceNames[10] = s_goblinText;
        s_raceNames[11] = s_undeadText;
        s_raceNames[12] = s_independentText;
        s_raceNamesPluralTexts[0] = s_humanPluralText;
        s_raceNamesPluralTexts[1] = s_azracPluralText;
        s_raceNamesPluralTexts[2] = s_lizardPluralText;
        s_raceNamesPluralTexts[3] = s_frostlingPluralText;
        s_raceNamesPluralTexts[4] = s_elfPluralText;
        s_raceNamesPluralTexts[5] = s_halflingPluralText;
        s_raceNamesPluralTexts[6] = s_dwarfPluralText;
        s_raceNamesPluralTexts[7] = s_highmanPluralText;
        s_raceNamesPluralTexts[8] = s_darkElfPluralText;
        s_raceNamesPluralTexts[9] = s_orcPluralText;
        s_raceNamesPluralTexts[10] = s_goblinPluralText;
        s_raceNamesPluralTexts[11] = s_undeadPluralText;
        s_raceNamesPluralTexts[12] = s_independentsText;
        // Original AoWE initialization
        InitializeHitProbabilities();
        ValidateGameEngine();
        s_abilityPlugin = new AbilityPlugin();
    }

    internal static void Finalize()
    {
        s_abilityPlugin.Release();
        GameEngine?.Dispose();
        GameEngine = null;
    }

    internal static void InitializeMorePathsRelations()
    {
        s_diplomaticRelationTexts[(int)DiplomaticRelation.Alliance] = s_allianceText;
        s_diplomaticRelationTexts[(int)DiplomaticRelation.Peace] = s_peaceText;
        s_diplomaticRelationTexts[(int)DiplomaticRelation.War] = s_warText;
        s_diplomaticRelationTexts[(int)DiplomaticRelation.Unknown] = s_unknownText;
    }

    static GameTables()
    {
        s_magicSphereTexts[(int)MagicSphere.Cosmos] = s_cosmosText;
        s_magicSphereTexts[(int)MagicSphere.Life] = s_lifeText;
        s_magicSphereTexts[(int)MagicSphere.Death] = s_deathText;
        s_magicSphereTexts[(int)MagicSphere.Earth] = s_earthText;
        s_magicSphereTexts[(int)MagicSphere.Air] = s_airText;
        s_magicSphereTexts[(int)MagicSphere.Fire] = s_fireText;
        s_magicSphereTexts[(int)MagicSphere.Water] = s_waterText;
        s_magicSphereTexts[(int)MagicSphere.Secret] = s_secretText;
        s_magicSphereTexts[(int)MagicSphere.None] = s_noneText;
        s_statusTexts[(int)Status.Hostile] = s_hostileText;
        s_statusTexts[(int)Status.Wary] = s_waryText;
        s_statusTexts[(int)Status.Neutral] = s_neutralText;
        s_statusTexts[(int)Status.Polite] = s_politeText;
        s_statusTexts[(int)Status.Friendly] = s_friendlyText;
        s_playerBehaviorTexts[(int)PlayerBehavior.Normal] = s_normalText;
        s_playerBehaviorTexts[(int)PlayerBehavior.Aggressor] = s_aggressorText;
        s_playerBehaviorTexts[(int)PlayerBehavior.Defender] = s_defenderText;
        s_playerBehaviorTexts[(int)PlayerBehavior.Expander] = s_expanderText;
        s_playerBehaviorTexts[(int)PlayerBehavior.Scorcher] = s_scorcherText;
        s_playerBehaviorTexts[(int)PlayerBehavior.Random] = s_randomText;
        s_spellTypeTexts[(int)SpellType.Combat] = s_combatText;
        s_spellTypeTexts[(int)SpellType.Unit] = s_unitText;
        s_spellTypeTexts[(int)SpellType.Global] = s_globalText;
        InitializeMorePathsRelations();
    }

}
