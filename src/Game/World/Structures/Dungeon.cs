// Translated from Dungeon.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// Prisoners, PrisonersMode
public partial class Dungeon : ExplorationSite
{
    public Dungeon(ECustomNode owner) : base(owner)
    {
        _prisonersMode = DungeonPrisonersMode.Normal;
        Prisoners = new PrisonersArmy(null);
    }

    // Dungeon.pas:108
    public new const int ClassId = 131882;

    public PrisonersArmy Prisoners { get; }

    private DungeonPrisonersMode _prisonersMode;

    // Dungeon.pas:102
    public override void Dispose()
    {
        base.Dispose();
        Prisoners?.Dispose();
    }

    // Dungeon.pas:114
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(Prisoners, 8);
        stream.rwData(Bytes.Of(ref _prisonersMode, 1), 1, 9);
    }

    // Dungeon.pas:95

    // Dungeon.pas:249
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
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
            result = ExecuteResult.Done;
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
    public override bool GetExplored() => Prisoners.Count == 0;

    // Dungeon.pas:121
    public override void SetupCombat(Combat.Battle combat, ArmyHexSprite army)
    {
        base.SetupCombat(combat, army);
        combat.Items = Prisoners;
    }

    // Dungeon.pas:127
    public override void ExecuteSearchDone(CombatType combatType, CombatResult combatResult)
    {
        ArmyHexSprite army;
        PrisonersJoinEventLog log;
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.NoDefenders).Contains(combatResult))
        {
            army = (ArmyHexSprite)Owner.FindOwnedChild(131607);
            log = new PrisonersJoinEventLog(null);
            log.X = GetHexX();
            log.Y = GetHexY();
            log.MapLevel = GetHexLevel();
            log._player = army.Army.Player;
            log.Selection = 255;
            log.Gold = 0;
            log.JoinMode = JoinMode.Default;
            log.FirstUnitId = Prisoners.GetUnit(0).UnitId;
            for (int I = 0, last = Prisoners.Count - 1; I <= last; I++)
            {
                var troop = Prisoners.GetUnit(I);
                log.UnitIds.Add(troop.UnitId);
            }

            if (Prisoners.Count == 1)
            {
                var liberatePrisonerMessage = AbilityRules.TranslateResourceString(s_liberatePrisonerMessageText);
                log.JoinMessage.Text = liberatePrisonerMessage;
            }
            else
            {
                var liberatePrisonersMessage = AbilityRules.TranslateResourceString(s_liberatePrisonersMessageText);
                log.JoinMessage.Text = liberatePrisonersMessage;
            }

            log.Execute(false, ExecuteTrigger.Event, null);
            log.Release();
        }
    }

    // Dungeon.pas:156
    public void GeneratePrisoners()
    {
        int collection;
        UnitIndexList list;
        UnitIndexListCollection collections;
        RandSeed = StrategicMap.Current.DefendersSeed;
        if (_prisonersMode == DungeonPrisonersMode.Random)
        {
            _prisonersMode = (DungeonPrisonersMode)(Random(3) + 1);
        }

        collection = ((Resource)_resource).PrisonerCollections.GetRandomCollection();
        if (collection != -1)
        {
            list = new UnitIndexList();
            collections = ((Resource)_resource).PrisonerCollections;
            for (int I = collections.GetCollectionCount(collection) - 1; I >= 0; I--)
            {
                list.Add(collections.GetU(collection, I));
            }

            Int32List.FillWithRandomUnits(list, Prisoners, 4, (int)_prisonersMode * 50, Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature, UnitType.Machine));
            list?.Dispose();
        }
    }

    // Dungeon.pas:178
    public void LiberatePrisoners(sbyte player)
    {
        LocationTextEventLog locationLog;
        ArmyInfoEventLog armyLog;
        Hero hero;
        ArmyHexSprite army;
        locationLog = new LocationTextEventLog(null);
        locationLog._player = GetPlayer();
        var name = GetName();
        locationLog.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(s_exploredFormat), [name]);
        var hexLevel = GetHexLevel();
        var hexY = GetHexY();
        locationLog.SetLocation(GetHexX(), hexY, hexLevel);
        var hexY2 = GetHexY();
        var hexLevel2 = GetHexLevel();
        EventLog.DistributeLocationEventLog(locationLog, player, GetHexX(), hexY2, hexLevel2, false);
        locationLog.Release();
        armyLog = new ArmyInfoEventLog(null);
        armyLog.ArmyView.Setup(Prisoners, 255);
        var hexX = GetHexX();
        armyLog.ArmyView.X = hexX;
        var hexY3 = GetHexY();
        armyLog.ArmyView.Y = hexY3;
        var hexLevel3 = GetHexLevel();
        armyLog.ArmyView.MapLevel = hexLevel3;
        armyLog.Selection = 255;
        armyLog._player = GetPlayer();
        armyLog.Title = AbilityRules.TranslateResourceString(PrisonerLiberationText);
        armyLog.Description = "";
        hero = Prisoners.Hero(255, Set<HeroType>.Of(HeroType.Hero));
        if (hero == null)
        {
            if (Prisoners.Count == 1)
            {
                armyLog.Text = AbilityRules.TranslateResourceString(s_onePrisonerLiberatedText);
                var name2 = StrategicMap.Current.Players.GetPlayers(player).GetName();
                var format = RtlUnit.Format(AbilityRules.TranslateResourceString(s_afterLiberationOneUnitJoinedForcesFormat), [name2]);
                armyLog.Messages.Text = format;
            }
            else
            {
                var count = Prisoners.Count;
                armyLog.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(s_prisonersLiberatedFormat), [IntToStr(count)]);
                var fPrisonersCount = Prisoners.Count;
                var name2 = StrategicMap.Current.Players.GetPlayers(player).GetName();
                var format = RtlUnit.Format(AbilityRules.TranslateResourceString(s_afterLiberationUnitsJoinedForcesFormat), [fPrisonersCount, name2]);
                armyLog.Messages.Text = format;
            }
        }
        else
        {
            armyLog.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(s_liberatedFormat), [hero.HeroName]);
            var name2 = StrategicMap.Current.Players.GetPlayers(player).GetName();
            var format = RtlUnit.Format(AbilityRules.TranslateResourceString(s_afterLiberationJoinedForcesFormat), [hero.HeroName, name2]);
            armyLog.Messages.Text = format;
        }

        var hexY4 = GetHexY();
        var hexLevel4 = GetHexLevel();
        EventLog.DistributeLocationEventLog(armyLog, -1, GetHexX(), hexY4, hexLevel4, false);
        armyLog.Release();
        army = new ArmyHexSprite(null);
        army.Army.BeginUpdate();
        army.Army.SetPlayer(player);
        army.Army.AddUnits(Prisoners);
        var hexLevel5 = GetHexLevel();
        var hexY5 = GetHexY();
        army.Place(GetHexX(), hexY5, hexLevel5, Set<ArmyPlaceModeElement>.Of(ArmyPlaceModeElement.Safe, ArmyPlaceModeElement.ScanArea));
        army.Activate();
        army.Army.EndUpdate();
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
    public new partial class Resource : ExplorationSiteResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            PrisonerCollections = new UnitIndexListCollection();
        }

        // Dungeon.pas:280
        public new const int ClassId = 131883;

        public UnitIndexListCollection PrisonerCollections { get; }

        // Dungeon.pas:271
        public override void Dispose()
        {
            PrisonerCollections?.Dispose();
            base.Dispose();
        }

        // Dungeon.pas:286
        public override void ReadWrite(EStorageStream stream)
        {
            base.ReadWrite(stream);
            stream.rwStreamProc(30, PrisonerCollections.LoadFromStream, PrisonerCollections.SaveToStream);
        }

        // Dungeon.pas:265

        // Dungeon.pas:283
        public override int ResourceUserClassId() => 131882;

        // Dungeon.pas:53

        // Dungeon.pas:277
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_dungeonText);

        private static readonly string s_dungeonText = "Dungeon";

    }

    // Dungeon.pas:33

    // Dungeon.pas:33

}

public enum DungeonPrisonersMode : byte
{
    UserDefined = 0, Low = 1, Normal = 2, High = 3, Random = 4
}
