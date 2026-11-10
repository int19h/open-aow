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

public static class GameTables
{
    public static bool ValidateAoWEngine()
    {
        if (AoWEngine == null && ToolServices == null)
        {
            global::Aow.Game.Engine.AoWEngine.Create(null);
        }

        return AoWEngine != null;
    }

    internal static FixedArray_string_9 TMagicSphereRStr = FixedArray_string_9.From([]);

    internal static FixedArray_string_3 TSpellTypeRStr = FixedArray_string_3.From([]);

    internal static readonly string UnknownRStr = "Unknown";

    internal static readonly string NoneRStr = "None";

    internal static readonly string AggressorRStr = "Aggressor";

    internal static readonly string AirRStr = "Air";

    internal static readonly string AllianceRStr = "Alliance";

    internal static readonly string AzracPluralRStr = "Azracs";

    internal static readonly string AzracRStr = "Azrac";

    internal static readonly string CPUEmperorRStr = "CPU Emperor";

    internal static readonly string CPUKingRStr = "CPU King";

    internal static readonly string CPUKnightRStr = "CPU Knight";

    internal static readonly string CPULordRStr = "CPU Lord";

    internal static readonly string CPUWarriorRStr = "CPU Squire";

    internal static readonly string FireRStr = "Fire";

    internal static readonly string CombatRStr = "Combat";

    internal static readonly string CosmosRStr = "Cosmos";

    internal static readonly string DarkElfPluralRStr = "Dark Elves";

    internal static readonly string DarkElfRStr = "DarkElf";

    internal static readonly string DeathRStr = "Death";

    internal static readonly string DeathStormRStr = "Death Storm";

    internal static readonly string DefenderRStr = "Defender";

    internal static readonly string DivineStormRStr = "Divine Storm";

    internal static readonly string DwarfPluralRStr = "Dwarves";

    internal static readonly string DwarfRStr = "Dwarf";

    internal static readonly string ElfPluralRStr = "Elves";

    internal static readonly string ElfRStr = "Elf";

    internal static readonly string EnchantmentRStr = "Enchantment";

    internal static readonly string EnhancementRStr = "Enhancement";

    internal static readonly string ExpanderRStr = "Expander";

    internal static readonly string FireStormRStr = "Fire Storm";

    internal static readonly string FreeRStr = "Free";

    internal static readonly string FriendlyRStr = "Friendly";

    internal static readonly string FrostlingPluralRStr = "Frostlings";

    internal static readonly string FrostlingRStr = "Frostling";

    internal static readonly string GlobalRStr = "Global";

    internal static readonly string GoblinPluralRStr = "Goblins";

    internal static readonly string GoblinRStr = "Goblin";

    internal static readonly string HalflingPluralRStr = "Halflings";

    internal static readonly string HalflingRStr = "Halfling";

    internal static readonly string HandicapRStr = "Handicap";

    internal static readonly string HighmanPluralRStr = "Highmen";

    internal static readonly string HighmanRStr = "Highman";

    internal static readonly string HostileRStr = "Hostile";

    internal static readonly string HumanPluralRStr = "Humans";

    internal static readonly string HumanRStr = "Human";

    internal static readonly string IceStormRStr = "Ice Storm";

    internal static readonly string IndependentRStr = "Independent";

    internal static readonly string LifeRStr = "Life";

    internal static readonly string LightningStormRStr = "Lightning Storm";

    internal static readonly string LizardPluralRStr = "Lizards";

    internal static readonly string LizardRStr = "Lizard";

    internal static readonly string MovementRStr = "Movement";

    internal static readonly string NeutralRStr = "Neutral";

    internal static readonly string NormalRStr = "Normal";

    internal static readonly string OrcPluralRStr = "Orcs";

    internal static readonly string OrcRStr = "Orc";

    internal static readonly string PestilenceStormRStr = "Pestilence cloud";

    internal static readonly string PoliteRStr = "Polite";

    internal static readonly string RandomRStr = "Random";

    internal static readonly string ScorcherRStr = "Scorcher";

    internal static readonly string SecretRStr = "Secret";

    internal static readonly string UnitRStr = "Unit";

    internal static readonly string WaryRStr = "Wary";

    public static global::Aow.Game.Engine.AoWEngine AoWEngine;

    internal static FixedArray_string_5 TAbilityTypeRStr = FixedArray_string_5.From([]);

    internal static FixedArray_string_8 TPlayerTypeRStr = FixedArray_string_8.From([]);

    internal static FixedArray_string_6 TPlayerBehaviorRStr = FixedArray_string_6.From([]);

    internal static FixedArray_string_4 TDiplomaticRelationRStr = FixedArray_string_4.From([]);

    internal static string[] TStatusRStr = Strings(5);

    internal static string[] RaceNamesPluralRStr = Strings(13);

    internal static FixedArray_string_13 RaceNamesRStr = FixedArray_string_13.From([]);

    internal static readonly string UndeadRStr = "Undead";

    internal static readonly string UndeadPluralRStr = "Undead(plural)";

    internal static readonly string IndependentsRStr = "Independents";

    internal static readonly string WaterRStr = "Water";

    internal static readonly string EarthRStr = "Earth";

    internal static FixedArray_string_7 TStormTypeRStr = FixedArray_string_7.From([Aow.Game.Magic.Abilities.GameTables.NoneRStr, Aow.Game.Magic.Abilities.GameTables.FireStormRStr, Aow.Game.Magic.Abilities.GameTables.DeathStormRStr, Aow.Game.Magic.Abilities.GameTables.DivineStormRStr, Aow.Game.Magic.Abilities.GameTables.IceStormRStr, Aow.Game.Magic.Abilities.GameTables.LightningStormRStr, Aow.Game.Magic.Abilities.GameTables.PestilenceStormRStr]);

    internal static FixedArray_float_20 P3HitProbabilities;

    internal static readonly string P5WarRStr = "War";

    internal static readonly string P5PeaceRStr = "Peace";

    internal static void P3InitializeHitProbabilities()
    {
        for (int I = 0; I <= 19; I++)
        {
            P3HitProbabilities[I] = (float)((double)(20 - I) * 0.05);
        }
    }

    internal static AbilityPlugIn AoWAbilityPlugIn;

    internal static void Initialize()
    {
        TAbilityTypeRStr[(int)AbilityType.Enhancement] = EnhancementRStr;
        TAbilityTypeRStr[(int)AbilityType.Handicap] = HandicapRStr;
        TAbilityTypeRStr[(int)AbilityType.Movement] = MovementRStr;
        TAbilityTypeRStr[(int)AbilityType.Combat] = CombatRStr;
        TAbilityTypeRStr[(int)AbilityType.Enchantment] = EnchantmentRStr;
        TPlayerTypeRStr[0] = HumanRStr;
        TPlayerTypeRStr[1] = CPUWarriorRStr;
        TPlayerTypeRStr[2] = CPUKnightRStr;
        TPlayerTypeRStr[3] = CPULordRStr;
        TPlayerTypeRStr[4] = IndependentRStr;
        TPlayerTypeRStr[5] = FreeRStr;
        TPlayerTypeRStr[6] = CPUKingRStr;
        TPlayerTypeRStr[7] = CPUEmperorRStr;
        // Original InitResStrings table: race-name entries
        RaceNamesRStr[0] = HumanRStr;
        RaceNamesRStr[1] = AzracRStr;
        RaceNamesRStr[2] = LizardRStr;
        RaceNamesRStr[3] = FrostlingRStr;
        RaceNamesRStr[4] = ElfRStr;
        RaceNamesRStr[5] = HalflingRStr;
        RaceNamesRStr[6] = DwarfRStr;
        RaceNamesRStr[7] = HighmanRStr;
        RaceNamesRStr[8] = DarkElfRStr;
        RaceNamesRStr[9] = OrcRStr;
        RaceNamesRStr[10] = GoblinRStr;
        RaceNamesRStr[11] = UndeadRStr;
        RaceNamesRStr[12] = IndependentRStr;
        RaceNamesPluralRStr[0] = HumanPluralRStr;
        RaceNamesPluralRStr[1] = AzracPluralRStr;
        RaceNamesPluralRStr[2] = LizardPluralRStr;
        RaceNamesPluralRStr[3] = FrostlingPluralRStr;
        RaceNamesPluralRStr[4] = ElfPluralRStr;
        RaceNamesPluralRStr[5] = HalflingPluralRStr;
        RaceNamesPluralRStr[6] = DwarfPluralRStr;
        RaceNamesPluralRStr[7] = HighmanPluralRStr;
        RaceNamesPluralRStr[8] = DarkElfPluralRStr;
        RaceNamesPluralRStr[9] = OrcPluralRStr;
        RaceNamesPluralRStr[10] = GoblinPluralRStr;
        RaceNamesPluralRStr[11] = UndeadPluralRStr;
        RaceNamesPluralRStr[12] = IndependentsRStr;
        // Original AoWE initialization
        P3InitializeHitProbabilities();
        ValidateAoWEngine();
        AoWAbilityPlugIn = new AbilityPlugIn();
    }

    internal static void Finalize()
    {
        AoWAbilityPlugIn.Release();
        AoWEngine?.Dispose();
        AoWEngine = null;
    }

    internal static void InitializeMorePathsRelations()
    {
        TDiplomaticRelationRStr[(int)DiplomaticRelation.Alliance] = AllianceRStr;
        TDiplomaticRelationRStr[(int)DiplomaticRelation.Peace] = P5PeaceRStr;
        TDiplomaticRelationRStr[(int)DiplomaticRelation.War] = P5WarRStr;
        TDiplomaticRelationRStr[(int)DiplomaticRelation.Unknown] = UnknownRStr;
    }

    static GameTables()
    {
        TMagicSphereRStr[(int)Aow.Game.Magic.MagicSphere.Cosmos] = CosmosRStr;
        TMagicSphereRStr[(int)Aow.Game.Magic.MagicSphere.Life] = LifeRStr;
        TMagicSphereRStr[(int)Aow.Game.Magic.MagicSphere.Death] = DeathRStr;
        TMagicSphereRStr[(int)Aow.Game.Magic.MagicSphere.Earth] = EarthRStr;
        TMagicSphereRStr[(int)Aow.Game.Magic.MagicSphere.Air] = AirRStr;
        TMagicSphereRStr[(int)Aow.Game.Magic.MagicSphere.Fire] = FireRStr;
        TMagicSphereRStr[(int)Aow.Game.Magic.MagicSphere.Water] = WaterRStr;
        TMagicSphereRStr[(int)Aow.Game.Magic.MagicSphere.Secret] = SecretRStr;
        TMagicSphereRStr[(int)Aow.Game.Magic.MagicSphere.None] = NoneRStr;
        TStatusRStr[(int)Status.Hostile] = HostileRStr;
        TStatusRStr[(int)Status.Wary] = WaryRStr;
        TStatusRStr[(int)Status.Neutral] = NeutralRStr;
        TStatusRStr[(int)Status.Polite] = PoliteRStr;
        TStatusRStr[(int)Status.Friendly] = FriendlyRStr;
        TPlayerBehaviorRStr[(int)Aow.Game.Session.Player.Behavior.Normal] = NormalRStr;
        TPlayerBehaviorRStr[(int)Aow.Game.Session.Player.Behavior.Aggressor] = AggressorRStr;
        TPlayerBehaviorRStr[(int)Aow.Game.Session.Player.Behavior.Defender] = DefenderRStr;
        TPlayerBehaviorRStr[(int)Aow.Game.Session.Player.Behavior.Expander] = ExpanderRStr;
        TPlayerBehaviorRStr[(int)Aow.Game.Session.Player.Behavior.Scorcher] = ScorcherRStr;
        TPlayerBehaviorRStr[(int)Aow.Game.Session.Player.Behavior.Random] = RandomRStr;
        TSpellTypeRStr[(int)SpellType.Combat] = CombatRStr;
        TSpellTypeRStr[(int)SpellType.Unit] = UnitRStr;
        TSpellTypeRStr[(int)SpellType.Global] = GlobalRStr;
        InitializeMorePathsRelations();
    }

}
