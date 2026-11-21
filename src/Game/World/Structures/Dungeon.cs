#nullable disable warnings
// Translated from Dungeon.pas.
using System.Globalization;
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Structures;

// Prisoners, PrisonersMode
public partial class Dungeon : ExplorationSite
{
    public Dungeon(CustomNode owner) : base(owner)
    {
        _prisonersMode = DungeonPrisonersMode.Normal;
        Prisoners = new PrisonersArmy(null) { Holder = CreationHolder() };
    }

    // Dungeon.pas:108
    public new const int ClassId = 131_882;

    public PrisonersArmy Prisoners { get; }

    private DungeonPrisonersMode _prisonersMode;

    // Dungeon.pas:102
    public override void Dispose()
    {
        base.Dispose();
        Prisoners?.Dispose();
    }

    // Dungeon.pas:114
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Prisoners", objectValue: Prisoners);
        _prisonersMode = persister.Enum("PrisonersMode", value: _prisonersMode);
    }

    // Dungeon.pas:249
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        Prisoners.MessageProc(ref message, sender);
    }

    // Dungeon.pas:237
    public override void Activate()
    {
        base.Activate();
        Prisoners.Activate();
    }

    // Dungeon.pas:243
    public override void Deactivate()
    {
        Prisoners.Deactivate();
        base.Deactivate();
    }

    // Dungeon.pas:255
    public override ExecuteResult ExecuteTokenEvent(StructureTokenEvent tokenEvent, TokenExecutor executor)
    {
        ExecuteResult result = base.ExecuteTokenEvent(tokenEvent, executor);
        if (tokenEvent.Action == 1)
        {
            LiberatePrisoners((sbyte)tokenEvent.Param);
            return ExecuteResult.Done;
        }

        return result;
    }

    // Dungeon.pas:231
    public override void NewDay()
    {
        base.NewDay();
        if (StrategicMap.Current.Day == 1)
        {
            GeneratePrisoners();
        }
    }

    // Dungeon.pas:111
    public override bool Explored => Prisoners.Units.Count == 0;

    // Dungeon.pas:121
    public override void SetupCombat(Battle combat, ArmyHexSprite army)
    {
        base.SetupCombat(combat, army);
        combat.Items = Prisoners;
    }

    // Dungeon.pas:127
    public override void ExecuteSearchDone(CombatType combatType, CombatResult combatResult)
    {
        if (combatResult is CombatResult.Victory or CombatResult.NoDefenders)
        {
            ArmyHexSprite army = Owner.GetArmyHexSprite();
            PrisonersJoinEventLog log = new(null)
            {
                Holder = CreationHolder(),
                Location = new MapCoordinate(HexX, HexY, HexLevel),
                Player = army.Army.Player,
                Selection = 255,
                Gold = 0,
                JoinMode = JoinMode.Default,
                FirstUnitId = Prisoners.Units.GetUnit(0).UnitId
            };
            for (int i = 0, last = Prisoners.Units.Count - 1; i <= last; i++)
            {
                Troop? troop = Prisoners.Units.GetUnit(i);
                _ = log.UnitIds.Add(troop.UnitId);
            }

            if (Prisoners.Units.Count == 1)
            {
                string liberatePrisonerMessage = AbilityRules.TranslateResourceString(GetEngine(), S: s_liberatePrisonerMessageText);
                log.JoinMessage.Text = liberatePrisonerMessage;
            }
            else
            {
                string liberatePrisonersMessage = AbilityRules.TranslateResourceString(GetEngine(), S: s_liberatePrisonersMessageText);
                log.JoinMessage.Text = liberatePrisonersMessage;
            }

            _ = log.Execute(false, trigger: ExecuteTrigger.Event, done: null);
            _ = log.Release();
        }
    }

    // Dungeon.pas:156
    public void GeneratePrisoners()
    {
        RandSeed = StrategicMap.Current.DefendersSeed;
        if (_prisonersMode == DungeonPrisonersMode.Random)
        {
            _prisonersMode = (DungeonPrisonersMode)(Random(3) + 1);
        }

        int collection = GetResource<Resource>().PrisonerCollections.GetRandomCollection();
        if (collection != -1)
        {
            UnitIndexList list = new();
            UnitIndexListCollection collections = GetResource<Resource>().PrisonerCollections;
            for (int i = collections.GetCollectionCount(collection) - 1; i >= 0; i--)
            {
                _ = list.Add(collections.GetU(collection, index: i));
            }

            _ = Int32List.FillWithRandomUnits(list, Prisoners.Units, level: 4, value: (int)_prisonersMode * 50, types: UnitTypes.Humanoid | UnitTypes.Creature | UnitTypes.Machine);
            list?.Dispose();
        }
    }

    // Dungeon.pas:178
    public void LiberatePrisoners(sbyte player)
    {
        LocationTextEventLog locationLog = new(null)
        {
            Holder = CreationHolder(),
            Player = GetPlayer()
        };
        string name = GetName();
        locationLog.Text = Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_exploredFormat), args: [name]);
        locationLog.SetLocation(level: HexLevel, y: HexY, x: HexX);
        EventLog.DistributeLocationEventLog(y: HexY, level: HexLevel, log: locationLog, player: player, x: HexX, execute: false);
        _ = locationLog.Release();
        ArmyInfoEventLog armyLog = new(null) { Holder = CreationHolder() };
        armyLog.ArmyView.Setup(Prisoners, selection: 255);
        sbyte hexX = HexX;
        armyLog.ArmyView.Location = armyLog.ArmyView.Location with { X = hexX };
        sbyte hexY3 = HexY;
        armyLog.ArmyView.Location = armyLog.ArmyView.Location with { Y = hexY3 };
        sbyte hexLevel3 = HexLevel;
        armyLog.ArmyView.Location = armyLog.ArmyView.Location with { MapLevel = hexLevel3 };
        armyLog.Selection = 255;
        armyLog.Player = GetPlayer();
        armyLog.Title = AbilityRules.TranslateResourceString(GetEngine(), S: PrisonerLiberationText);
        armyLog.Description = "";
        Hero? hero = Prisoners.Hero(255, types: HeroTypes.Hero);
        if (hero is null)
        {
            if (Prisoners.Units.Count == 1)
            {
                armyLog.Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_onePrisonerLiberatedText);
                string name2 = StrategicMap.Current.Players.GetPlayers(player).GetName();
                string format = Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_afterLiberationOneUnitJoinedForcesFormat), args: [name2]);
                armyLog.Messages.Text = format;
            }
            else
            {
                int count = Prisoners.Units.Count;
                armyLog.Text = Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_prisonersLiberatedFormat), args: [count.ToString(CultureInfo.InvariantCulture)]);
                int fPrisonersCount = Prisoners.Units.Count;
                string name2 = StrategicMap.Current.Players.GetPlayers(player).GetName();
                string format = Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_afterLiberationUnitsJoinedForcesFormat), args: [fPrisonersCount, name2]);
                armyLog.Messages.Text = format;
            }
        }
        else
        {
            armyLog.Text = Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_liberatedFormat), args: [hero.HeroName]);
            string name2 = StrategicMap.Current.Players.GetPlayers(player).GetName();
            string format = Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_afterLiberationJoinedForcesFormat), args: [hero.HeroName, name2]);
            armyLog.Messages.Text = format;
        }
        EventLog.DistributeLocationEventLog(y: HexY, level: HexLevel, log: armyLog, player: -1, x: HexX, execute: false);
        _ = armyLog.Release();
        ArmyHexSprite army = new(null) { Holder = CreationHolder() };
        using (army.Army.BeginUpdate())
        {
            army.Army.SetPlayer(player);
            _ = army.Army.AddUnits(Prisoners.Units);
            _ = army.Place(mapLevel: HexLevel, y: HexY, x: HexX, mode: ArmyPlaceModes.Safe | ArmyPlaceModes.ScanArea);
            army.Activate();
        }
    }

    private static readonly string s_afterLiberationOneUnitJoinedForcesFormat = "After liberation 1 unit joined the forces of %s";
    private static readonly string s_afterLiberationJoinedForcesFormat = "After liberation %s joined the forces of %s";
    private static readonly string s_afterLiberationUnitsJoinedForcesFormat = "After liberation %d units joined the forces of %s";
    private static readonly string s_liberatePrisonerMessageText = "In gratitude for rescue, the captive offers to join you. Do you accept?";
    private static readonly string s_liberatePrisonersMessageText = "Grateful to be rescued, the captives offer to join you. Do you accept?";
    private static readonly string s_onePrisonerLiberatedText = "1 prisoner liberated";

    internal static string PrisonerLiberationText { get; } = "Prisoner Liberation";

    private static readonly string s_exploredFormat = "%s explored";
    private static readonly string s_liberatedFormat = "%s liberated";
    private static readonly string s_prisonersLiberatedFormat = "%s prisoners liberated";

    // PrisonerCollections
    public new class Resource(CustomNode owner) : ExplorationSiteResource(owner)
    {

        // Dungeon.pas:280
        public new const int ClassId = 131_883;

        public UnitIndexListCollection PrisonerCollections { get; } = new UnitIndexListCollection();

        // Dungeon.pas:271
        public override void Dispose()
        {
            PrisonerCollections?.Dispose();
            base.Dispose();
        }

        // Dungeon.pas:286
        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            persister.UnitIndexListCollection(name: "PrisonerCollections", collection: PrisonerCollections);
        }

        // Dungeon.pas:283
        public override int ResourceUserClassId() => Dungeon.ClassId;

        // Dungeon.pas:277
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_dungeonText);

        private static readonly string s_dungeonText = "Dungeon";

    }
}

public enum DungeonPrisonersMode : byte
{
    UserDefined = 0, Low = 1, Normal = 2, High = 3, Random = 4
}
