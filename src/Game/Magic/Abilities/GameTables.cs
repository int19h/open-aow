using System.Collections.Immutable;

using Aow.Engine.Buffers;

using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

public static class GameTables
{
    internal static string[] MagicSphereTexts { get; } = ["", "", "", "", "", "", "", "", ""];

    internal static string[] SpellTypeTexts { get; } = ["", "", ""];

    internal static string UnknownText { get; } = "Unknown";

    internal static string NoneText { get; } = "None";

    private static readonly string s_aggressorText = "Aggressor";

    private static readonly string s_airText = "Air";

    private static readonly string s_allianceText = "Alliance";

    private static readonly string s_azracPluralText = "Azracs";

    private static readonly string s_azracText = "Azrac";

    private static readonly string s_cpuEmperorText = "CPU Emperor";

    private static readonly string s_cpuKingText = "CPU King";

    private static readonly string s_cpuKnightText = "CPU Knight";

    private static readonly string s_cpuLordText = "CPU Lord";

    private static readonly string s_cpuWarriorText = "CPU Squire";

    internal static string FireText { get; } = "Fire";

    internal static string CombatText { get; } = "Combat";

    private static readonly string s_cosmosText = "Cosmos";

    private static readonly string s_darkElfPluralText = "Dark Elves";

    private static readonly string s_darkElfText = "DarkElf";

    internal static string DeathText { get; } = "Death";

    internal static string DeathStormText { get; } = "Death Storm";

    private static readonly string s_defenderText = "Defender";

    internal static string DivineStormText { get; } = "Divine Storm";

    private static readonly string s_dwarfPluralText = "Dwarves";

    private static readonly string s_dwarfText = "Dwarf";

    private static readonly string s_elfPluralText = "Elves";

    private static readonly string s_elfText = "Elf";

    internal static string EnchantmentText { get; } = "Enchantment";

    private static readonly string s_enhancementText = "Enhancement";

    private static readonly string s_expanderText = "Expander";

    internal static string FireStormText { get; } = "Fire Storm";

    private static readonly string s_freeText = "Free";

    private static readonly string s_friendlyText = "Friendly";

    private static readonly string s_frostlingPluralText = "Frostlings";

    private static readonly string s_frostlingText = "Frostling";

    internal static string GlobalText { get; } = "Global";

    private static readonly string s_goblinPluralText = "Goblins";

    private static readonly string s_goblinText = "Goblin";

    private static readonly string s_halflingPluralText = "Halflings";

    private static readonly string s_halflingText = "Halfling";

    private static readonly string s_handicapText = "Handicap";

    private static readonly string s_highmanPluralText = "Highmen";

    private static readonly string s_highmanText = "Highman";

    private static readonly string s_hostileText = "Hostile";

    private static readonly string s_humanPluralText = "Humans";

    internal static string HumanText { get; } = "Human";

    internal static string IceStormText { get; } = "Ice Storm";

    private static readonly string s_independentText = "Independent";

    private static readonly string s_lifeText = "Life";

    internal static string LightningStormText { get; } = "Lightning Storm";

    private static readonly string s_lizardPluralText = "Lizards";

    private static readonly string s_lizardText = "Lizard";

    private static readonly string s_movementText = "Movement";

    internal static string NeutralText { get; } = "Neutral";

    private static readonly string s_normalText = "Normal";

    private static readonly string s_orcPluralText = "Orcs";

    private static readonly string s_orcText = "Orc";

    internal static string PestilenceStormText { get; } = "Pestilence cloud";

    private static readonly string s_politeText = "Polite";

    private static readonly string s_randomText = "Random";

    private static readonly string s_scorcherText = "Scorcher";

    private static readonly string s_secretText = "Secret";

    private static readonly string s_unitText = "Unit";

    private static readonly string s_waryText = "Wary";

    // The engine global of AoWE. The hosts own the engine and the interface reaches it through its
    // context (issue 224). It stays for the engine nodes whose owner walk does not reach the engine,
    // such as the rules a registry creates without an owner, until the nodes carry their engine.
    public static GameEngine GameEngine { get; set; }

    internal static string[] AbilityTypeTexts { get; } = ["", "", "", "", ""];

    internal static string[] PlayerTypeTexts { get; } = ["", "", "", "", "", "", "", ""];

    internal static string[] PlayerBehaviorTexts { get; } = ["", "", "", "", "", ""];

    internal static string[] DiplomaticRelationTexts { get; } = ["", "", "", ""];

    internal static string[] StatusTexts { get; } = Strings(5);

    internal static string[] RaceNamesPluralTexts { get; } = Strings(13);

    internal static string[] RaceNames { get; } = ["", "", "", "", "", "", "", "", "", "", "", "", ""];

    private static readonly string s_undeadText = "Undead";

    private static readonly string s_undeadPluralText = "Undead(plural)";

    internal static string IndependentsText { get; } = "Independents";

    internal static string WaterText { get; } = "Water";

    internal static string EarthText { get; } = "Earth";

    internal static ImmutableArray<string> StormTypeTexts { get; } = [NoneText, FireStormText, DeathStormText, DivineStormText, IceStormText, LightningStormText, PestilenceStormText];

    private static readonly float[] s_hitProbabilities = new float[20];

    internal static string WarText { get; } = "War";

    internal static string PeaceText { get; } = "Peace";

    internal static void InitializeHitProbabilities()
    {
        for (int I = 0; I <= 19; I++)
        {
            s_hitProbabilities[I] = (float)((double)(20 - I) * 0.05);
        }
    }

    internal static void Initialize()
    {
        AbilityTypeTexts[(int)AbilityType.Enhancement] = s_enhancementText;
        AbilityTypeTexts[(int)AbilityType.Handicap] = s_handicapText;
        AbilityTypeTexts[(int)AbilityType.Movement] = s_movementText;
        AbilityTypeTexts[(int)AbilityType.Combat] = CombatText;
        AbilityTypeTexts[(int)AbilityType.Enchantment] = EnchantmentText;
        PlayerTypeTexts[0] = HumanText;
        PlayerTypeTexts[1] = s_cpuWarriorText;
        PlayerTypeTexts[2] = s_cpuKnightText;
        PlayerTypeTexts[3] = s_cpuLordText;
        PlayerTypeTexts[4] = s_independentText;
        PlayerTypeTexts[5] = s_freeText;
        PlayerTypeTexts[6] = s_cpuKingText;
        PlayerTypeTexts[7] = s_cpuEmperorText;
        // Original InitResStrings table: race-name entries
        RaceNames[0] = HumanText;
        RaceNames[1] = s_azracText;
        RaceNames[2] = s_lizardText;
        RaceNames[3] = s_frostlingText;
        RaceNames[4] = s_elfText;
        RaceNames[5] = s_halflingText;
        RaceNames[6] = s_dwarfText;
        RaceNames[7] = s_highmanText;
        RaceNames[8] = s_darkElfText;
        RaceNames[9] = s_orcText;
        RaceNames[10] = s_goblinText;
        RaceNames[11] = s_undeadText;
        RaceNames[12] = s_independentText;
        RaceNamesPluralTexts[0] = s_humanPluralText;
        RaceNamesPluralTexts[1] = s_azracPluralText;
        RaceNamesPluralTexts[2] = s_lizardPluralText;
        RaceNamesPluralTexts[3] = s_frostlingPluralText;
        RaceNamesPluralTexts[4] = s_elfPluralText;
        RaceNamesPluralTexts[5] = s_halflingPluralText;
        RaceNamesPluralTexts[6] = s_dwarfPluralText;
        RaceNamesPluralTexts[7] = s_highmanPluralText;
        RaceNamesPluralTexts[8] = s_darkElfPluralText;
        RaceNamesPluralTexts[9] = s_orcPluralText;
        RaceNamesPluralTexts[10] = s_goblinPluralText;
        RaceNamesPluralTexts[11] = s_undeadPluralText;
        RaceNamesPluralTexts[12] = IndependentsText;
        // Original AoWE initialization
        InitializeHitProbabilities();
    }

    internal static void InitializeMorePathsRelations()
    {
        DiplomaticRelationTexts[(int)DiplomaticRelation.Alliance] = s_allianceText;
        DiplomaticRelationTexts[(int)DiplomaticRelation.Peace] = PeaceText;
        DiplomaticRelationTexts[(int)DiplomaticRelation.War] = WarText;
        DiplomaticRelationTexts[(int)DiplomaticRelation.Unknown] = UnknownText;
    }

    static GameTables()
    {
        MagicSphereTexts[(int)MagicSphere.Cosmos] = s_cosmosText;
        MagicSphereTexts[(int)MagicSphere.Life] = s_lifeText;
        MagicSphereTexts[(int)MagicSphere.Death] = DeathText;
        MagicSphereTexts[(int)MagicSphere.Earth] = EarthText;
        MagicSphereTexts[(int)MagicSphere.Air] = s_airText;
        MagicSphereTexts[(int)MagicSphere.Fire] = FireText;
        MagicSphereTexts[(int)MagicSphere.Water] = WaterText;
        MagicSphereTexts[(int)MagicSphere.Secret] = s_secretText;
        MagicSphereTexts[(int)MagicSphere.None] = NoneText;
        StatusTexts[(int)Status.Hostile] = s_hostileText;
        StatusTexts[(int)Status.Wary] = s_waryText;
        StatusTexts[(int)Status.Neutral] = NeutralText;
        StatusTexts[(int)Status.Polite] = s_politeText;
        StatusTexts[(int)Status.Friendly] = s_friendlyText;
        PlayerBehaviorTexts[(int)PlayerBehavior.Normal] = s_normalText;
        PlayerBehaviorTexts[(int)PlayerBehavior.Aggressor] = s_aggressorText;
        PlayerBehaviorTexts[(int)PlayerBehavior.Defender] = s_defenderText;
        PlayerBehaviorTexts[(int)PlayerBehavior.Expander] = s_expanderText;
        PlayerBehaviorTexts[(int)PlayerBehavior.Scorcher] = s_scorcherText;
        PlayerBehaviorTexts[(int)PlayerBehavior.Random] = s_randomText;
        SpellTypeTexts[(int)SpellType.Combat] = CombatText;
        SpellTypeTexts[(int)SpellType.Unit] = s_unitText;
        SpellTypeTexts[(int)SpellType.Global] = GlobalText;
        InitializeMorePathsRelations();
    }

}
