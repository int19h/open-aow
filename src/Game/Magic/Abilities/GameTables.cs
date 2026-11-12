
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

    public static GameEngine GameEngine { get; set; }

    internal static StringArray5 s_abilityTypeTexts = StringArray5.From([]);

    internal static StringArray8 s_playerTypeTexts = StringArray8.From([]);

    internal static StringArray6 s_playerBehaviorTexts = StringArray6.From([]);

    internal static StringArray4 s_diplomaticRelationTexts = StringArray4.From([]);

    internal static string[] StatusTexts { get; } = Strings(5);

    internal static string[] RaceNamesPluralTexts { get; } = Strings(13);

    internal static StringArray13 s_raceNames = StringArray13.From([]);

    private static readonly string s_undeadText = "Undead";

    private static readonly string s_undeadPluralText = "Undead(plural)";

    internal static string IndependentsText { get; } = "Independents";

    internal static string WaterText { get; } = "Water";

    internal static string EarthText { get; } = "Earth";

    internal static StringArray7 s_stormTypeTexts = StringArray7.From([NoneText, FireStormText, DeathStormText, DivineStormText, IceStormText, LightningStormText, PestilenceStormText]);

    private static SingleArray20 s_hitProbabilities;

    internal static string WarText { get; } = "War";

    internal static string PeaceText { get; } = "Peace";

    internal static void InitializeHitProbabilities()
    {
        for (int I = 0; I <= 19; I++)
        {
            s_hitProbabilities[I] = (float)((double)(20 - I) * 0.05);
        }
    }

    private static AbilityPlugin s_abilityPlugin;

    internal static void Initialize()
    {
        s_abilityTypeTexts[(int)AbilityType.Enhancement] = s_enhancementText;
        s_abilityTypeTexts[(int)AbilityType.Handicap] = s_handicapText;
        s_abilityTypeTexts[(int)AbilityType.Movement] = s_movementText;
        s_abilityTypeTexts[(int)AbilityType.Combat] = CombatText;
        s_abilityTypeTexts[(int)AbilityType.Enchantment] = EnchantmentText;
        s_playerTypeTexts[0] = HumanText;
        s_playerTypeTexts[1] = s_cpuWarriorText;
        s_playerTypeTexts[2] = s_cpuKnightText;
        s_playerTypeTexts[3] = s_cpuLordText;
        s_playerTypeTexts[4] = s_independentText;
        s_playerTypeTexts[5] = s_freeText;
        s_playerTypeTexts[6] = s_cpuKingText;
        s_playerTypeTexts[7] = s_cpuEmperorText;
        // Original InitResStrings table: race-name entries
        s_raceNames[0] = HumanText;
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
        s_diplomaticRelationTexts[(int)DiplomaticRelation.Peace] = PeaceText;
        s_diplomaticRelationTexts[(int)DiplomaticRelation.War] = WarText;
        s_diplomaticRelationTexts[(int)DiplomaticRelation.Unknown] = UnknownText;
    }

    static GameTables()
    {
        s_magicSphereTexts[(int)MagicSphere.Cosmos] = s_cosmosText;
        s_magicSphereTexts[(int)MagicSphere.Life] = s_lifeText;
        s_magicSphereTexts[(int)MagicSphere.Death] = DeathText;
        s_magicSphereTexts[(int)MagicSphere.Earth] = EarthText;
        s_magicSphereTexts[(int)MagicSphere.Air] = s_airText;
        s_magicSphereTexts[(int)MagicSphere.Fire] = FireText;
        s_magicSphereTexts[(int)MagicSphere.Water] = WaterText;
        s_magicSphereTexts[(int)MagicSphere.Secret] = s_secretText;
        s_magicSphereTexts[(int)MagicSphere.None] = NoneText;
        StatusTexts[(int)Status.Hostile] = s_hostileText;
        StatusTexts[(int)Status.Wary] = s_waryText;
        StatusTexts[(int)Status.Neutral] = NeutralText;
        StatusTexts[(int)Status.Polite] = s_politeText;
        StatusTexts[(int)Status.Friendly] = s_friendlyText;
        s_playerBehaviorTexts[(int)PlayerBehavior.Normal] = s_normalText;
        s_playerBehaviorTexts[(int)PlayerBehavior.Aggressor] = s_aggressorText;
        s_playerBehaviorTexts[(int)PlayerBehavior.Defender] = s_defenderText;
        s_playerBehaviorTexts[(int)PlayerBehavior.Expander] = s_expanderText;
        s_playerBehaviorTexts[(int)PlayerBehavior.Scorcher] = s_scorcherText;
        s_playerBehaviorTexts[(int)PlayerBehavior.Random] = s_randomText;
        s_spellTypeTexts[(int)SpellType.Combat] = CombatText;
        s_spellTypeTexts[(int)SpellType.Unit] = s_unitText;
        s_spellTypeTexts[(int)SpellType.Global] = GlobalText;
        InitializeMorePathsRelations();
    }

}
