// Translated from AoWE.pas.
using System.Collections.Immutable;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.AI.Groups;
using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI;

// player, actions, action, action index, cycles, move executer, phase, progress
public partial class AIPlayerControl : EngineObject
{
    // AI.pas:84
    public AIPlayerControl(ECustomNode owner) : base(owner)
    {
        Actions = new AIPlayerActionList(null) { Holder = CreationHolder() };
        MoveExecutor = new AIMoveExecutor(null) { Holder = CreationHolder() };
        _processCyclesMax = 10;
    }

    public Player Player { get; set; }
    public AIPlayerActionList Actions { get; private set; }

    private AIPlayerAction _action;
    private int _actionIndex;
    private int _processCyclesMax;

    public AIMoveExecutor MoveExecutor { get; private set; }
    public AIPlayerProcessPhase Phase { get; private set; }

    public float DefenseStrengthMultiplier;

    // AI.pas:92
    public override void Dispose()
    {
        Reset();
        try
        {
            Actions?.Dispose();
            Actions = null;
        }
        catch
        {
        }

        try
        {
            MoveExecutor?.Dispose();
            MoveExecutor = null;
        }
        catch
        {
        }

        base.Dispose();
    }

    // AI.pas:108
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Bytes(Bytes.Of(ref DefenseStrengthMultiplier, 4), 4, 10);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        DefenseStrengthMultiplier = persister.Single("DefenseStrengthMultiplier", value: DefenseStrengthMultiplier);
    }

    // AI.pas:114
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536936471)
        {
            Deactivate();
        }
    }

    // AI.pas:120
    public int ListAvailableTransporters(UnitList units)
    {
        Troop troop;
        UnitUserList group;
        int result = 0;
        var player = Player;
        for (int I = 0, last = player.Armies.Count - 1; I <= last; I++)
        {
            troop = player.Armies.GetArmy(I).Transporter(255);
            if (troop != null)
            {
                group = troop.Group;
                if (group == null || group.EngineClass.ClassId == 136295)
                {
                    units.AddUnit(troop);
                    result += 1;
                }
            }
        }

        return result;
    }

    // AI.pas:143
    public int UnitIndexToPriority(int unitIndex) => RosterTables.GetUnitIndexAIPriority(unitIndex, Player.PlayerIndex, 0, engine: GetEngine());

    // AI.pas:148
    public void Reset()
    {
        Phase = AIPlayerProcessPhase.None;
        _actionIndex = -1;
        DeactivateAction();
        Actions.Clear();
    }

    // AI.pas:156
    public void SetupProcessPhase(AIPlayerProcessPhase phase)
    {
        AIPlayerActionClass actionClass;
        AIPlayerAction action;
        Actions.Clear();
        for (int I = 0, last = StrategicMap.Current.AIPlayerActionManager.GetCount() - 1; I <= last; I++)
        {
            actionClass = StrategicMap.Current.AIPlayerActionManager.GetRegisteredClasses(I);
            if (actionClass.SupportedPhases.Contains(phase))
            {
                action = actionClass.CreateInstance(null, holder: CreationHolder());
                if (action.Setup(this, phase))
                {
                    action.SetOwner(Actions);
                }
                else
                {
                    action?.Dispose();
                }
            }
        }

        Phase = phase;
    }

    // AI.pas:176
    public int GetProcessCyclesMax()
    {
        int result;
        if (!Player.SingleProcessCycle)
        {
            result = _processCyclesMax;
        }
        else
        {
            result = 1;
        }

        return result;
    }

    // AI.pas:182
    public void Activate(AIExecutor executor)
    {
        Reset();
        if (StrategicMap.Current.Day < 10)
        {
            DefenseStrengthMultiplier = (float)((double)StrategicMap.Current.Day * 0.1);
        }
        else
        {
            DefenseStrengthMultiplier = 1;
        }

        SetupProcessPhase(AIPlayerProcessPhase.Main);
        switch (Player.PlayerType)
        {
            case PlayerType.Cpu1:
                _processCyclesMax = 5;
                break;
            case PlayerType.Cpu2:
                _processCyclesMax = 7;
                break;
            case PlayerType.Cpu3:
                _processCyclesMax = 9;
                break;
            case PlayerType.Cpu4:
                _processCyclesMax = 9;
                break;
            case PlayerType.Cpu5:
                _processCyclesMax = 9;
                break;
            case PlayerType.Independent:
                _processCyclesMax = 3;
                break;
        }
    }

    // AI.pas:198
    public void Deactivate()
    {
        Reset();
    }

    // AI.pas:203
    public GroupBehavior GetRandomGroupBehavior()
    {
        GroupBehavior result;
        Player player;
        player = Player;
        if (Player.PlayerIndex == 0)
        {
            var random = Random(10);
            var independentAIMode = StrategicMap.Current.GetIndependentAIMode();
            result = s_independentGroupBehaviorTable[(int)independentAIMode][random];
        }
        else
        {
            var random = Random(10);
            result = s_player2GroupBehaviorTable[(int)player.Behavior][random];
        }

        return result;
    }

    // AI.pas:213
    public void UpdateSkirmishGroups()
    {
        int skirmishCount;
        int wantedCount;
        int groupPriority;
        int unitPriority;
        AIGroup group;
        Troop troop;
        DoubleItemList<Troop> units;
        DoubleItem<Troop> item;
        skirmishCount = 0;
        for (int I = 0, last = Player.AIGroups.Count - 1; I <= last; I++)
        {
            if (Player.AIGroups.GetGroups(I).EngineClass.ClassId == 136294)
            {
                skirmishCount += 1;
            }
        }

        if (StrategicMap.Current.Day > 5)
        {
            wantedCount = Player.AIGroups.Count / 6;
            if (wantedCount > 4)
            {
                wantedCount = 4;
            }

            if (wantedCount == 0 && (StrategicMap.Current.Day & 16) == 16)
            {
                wantedCount = 1;
            }
        }
        else
        {
            wantedCount = 2;
        }

        if (skirmishCount < wantedCount)
        {
            units = new DoubleItemList<Troop>();
            for (int I = Player.AIGroups.Count - 1; I >= 0; I--)
            {
                group = Player.AIGroups.GetGroups(I);
                if (group.TargetType != AIGroupTargetStatus.Execute && group.EngineClass.ClassId != 136281 && group.EngineClass.ClassId != 136294 && group.EngineClass.ClassId != 136273 && group.EngineClass.ClassId != 136275 && group.EngineClass.ClassId != 136295)
                {
                    groupPriority = group.GetProcessPriority() * -10;
                    for (int J = group.Count - 1; J >= 0; J--)
                    {
                        troop = group.GetUnit(J);
                        if (!(troop is Hero))
                        {
                            unitPriority = troop.GetMoves();
                            if (troop.GetMoveTypes().Contains(MoveType.Flying))
                            {
                                unitPriority += 15;
                            }

                            units.AddAbility(troop, unitPriority + groupPriority);
                        }
                    }
                }
            }

            units.SortDescendingItemB();
            while (skirmishCount < wantedCount && units.Count != 0)
            {
                group = StrategicMap.Current.AIGroupManager.CreateGroup(Player.PlayerIndex, 0, GroupBehavior.Skirmish);
                item = units[0];
                group.AddUnit(item.First);
                group.Release();
                skirmishCount += 1;
                units.Delete(0);
            }

            units?.Dispose();
        }
    }

    // AI.pas:270
    public bool ActivateAction(int index)
    {
        DeactivateAction();
        _action = Actions.GetActions(index);
        bool result = _action.Activate();
        if (!result)
        {
            _action = null;
        }

        return result;
    }

    // AI.pas:278
    public void DeactivateAction()
    {
        AIPlayerAction action;
        action = _action;
        if (action != null)
        {
            action.Deactivate();
            _action = null;
        }
    }

    // AI.pas:290
    public void ListPeaceWarUnknownPlayers(ByteList peace, ByteList war, ByteList unknown)
    {
        DiplomaticRelation relation;
        peace.Clear();
        war.Clear();
        peace.Add((byte)Player.PlayerIndex);
        for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            if (I != Player.PlayerIndex)
            {
                if (StrategicMap.Current.Players.GetPlayers(I).Status == PlayerStatus.Playing)
                {
                    relation = StrategicMap.Current.Players.GetDiplomaticRelation(Player.PlayerIndex, (sbyte)I);
                    if (Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Alliance).Contains(relation))
                    {
                        peace.Add((byte)I);
                    }
                    else
                    {
                        if (relation == DiplomaticRelation.War)
                        {
                            war.Add((byte)I);
                        }
                        else
                        {
                            if (relation == DiplomaticRelation.Unknown)
                            {
                                if (Player.GetFirstDaySeen((sbyte)I) != -1)
                                {
                                    unknown.Add((byte)I);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // AI.pas:311
    // The original checks player status. Unknown is not cleared before appending.
    public void ListIdealPeaceWarPlayers(ByteList peace, ByteList war)
    {
        DoubleItemList Players;
        DiplomaticRelation relation;
        int priority;
        Status raceRelation;
        int days;
        int middlePlayer;
        DiplomaticAction action;
        DoubleItemList.DoubleItem item;
        peace.Clear();
        war.Clear();
        Players = new DoubleItemList();
        Players.AddAbility(Player.PlayerIndex, 999);
        for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            if (I != Player.PlayerIndex && StrategicMap.Current.Players.GetPlayers(I).Status == PlayerStatus.Playing)
            {
                priority = 0;
                relation = StrategicMap.Current.Players.GetDiplomaticRelation(Player.PlayerIndex, (sbyte)I);
                var players = StrategicMap.Current.Players.GetPlayers(I);
                var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(Player.PlayerIndex);
                raceRelation = StrategicMap.Current.Races.GetRace(fPlayersPlayers.PlayerRace).GetDefaultRaceRelation(players.PlayerRace);
                action = Player.DiplomaticActions.FindLastAction(DeclareWar.Kind, DiplomaticActionReaction.None, (sbyte)I, Player.PlayerIndex, -1);
                if (action != null)
                {
                    days = StrategicMap.Current.Day - action.Day;
                    if (days < 30)
                    {
                        priority = days + (priority - 30);
                    }
                }

                switch (raceRelation)
                {
                    case Status.Hostile:
                    case Status.Wary:
                        priority += 0;
                        break;
                    case Status.Neutral:
                        priority += 10;
                        break;
                    case Status.Polite:
                    case Status.Friendly:
                        priority += 20;
                        break;
                }

                if (relation == DiplomaticRelation.Alliance)
                {
                    priority += 50;
                }
                else
                {
                    if (relation == DiplomaticRelation.Peace)
                    {
                        priority += 15;
                    }
                }

                if (StrategicMap.Current.Players.GetPlayers(I).PlayerType == PlayerType.Human && Set<PlayerType>.Of(PlayerType.Cpu3, PlayerType.Cpu4, PlayerType.Cpu5).Contains(Player.PlayerType))
                {
                    priority -= 5;
                }

                Players.AddAbility(I, priority);
            }
        }

        Players.SortDescendingItemB();
        for (int I = 0, last = Players.Count / 2 - 1; I <= last; I++)
        {
            item = Players[I];
            peace.Add((byte)item.First);
        }

        for (int I = Players.Count - 1, last = (Players.Count + 1) / 2; I >= last; I--)
        {
            item = Players[I];
            war.Add((byte)item.First);
        }

        if ((Players.Count & 1) != 0)
        {
            item = Players[Players.Count / 2];
            middlePlayer = item.First;
            // { The middle player relation is evaluated directly below. }
            var players = StrategicMap.Current.Players.GetPlayers(middlePlayer);
            var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(Player.PlayerIndex);
            var defaultRaceRelation = StrategicMap.Current.Races.GetRace(fPlayersPlayers.PlayerRace).GetDefaultRaceRelation(players.PlayerRace);
            if (Set<Status>.Of(Status.Hostile, Status.Wary).Contains(defaultRaceRelation))
            {
                war.Add((byte)middlePlayer);
            }
            else
            {
                peace.Add((byte)middlePlayer);
            }
        }

        Players?.Dispose();
    }

    // AI.pas:375
    public bool CanOfferPeace(byte player)
    {
        bool result;
        ByteList peace;
        ByteList war;
        ByteList unknown;
        ByteList idealPeace;
        ByteList idealWar;
        int peaceCount;
        int warCount;
        int peaceIndex;
        int warIndex;
        int index;
        if (!(Player.DiplomaticRelations.GetHistory((sbyte)player).GetRelationOnDay(0) == DiplomaticRelation.War && Player.DiplomaticRelations.GetDiplomaticRelation((sbyte)player) == DiplomaticRelation.War))
        {
            idealPeace = new ByteList();
            idealWar = new ByteList();
            peace = new ByteList();
            war = new ByteList();
            unknown = new ByteList();
            ListPeaceWarUnknownPlayers(peace, war, unknown);
            ListIdealPeaceWarPlayers(idealPeace, idealWar);
            war.Remove(player);
            unknown.Remove(player);
            peace.Remove(player);
            peaceCount = peace.Count;
            warCount = war.Count;
            peaceIndex = idealPeace.IndexOf(player);
            warIndex = idealWar.IndexOf(player);
            for (int I = 0, last = unknown.Count - 1; I <= last; I++)
            {
                index = idealPeace.IndexOf(unknown[I]);
                if (index != -1)
                {
                    if (peaceIndex == -1 || index < peaceIndex)
                    {
                        peaceCount += 1;
                    }
                    else
                    {
                        warCount += 1;
                    }
                }

                index = idealWar.IndexOf(unknown[I]);
                if (index != -1)
                {
                    if (warIndex == -1 || index < warIndex)
                    {
                        warCount += 1;
                    }
                    else
                    {
                        peaceCount += 1;
                    }
                }
            }

            result = !(peaceCount > warCount && !(warCount == 0 && peaceCount == 1 && StrategicMap.Current.Players.ActivePlayers.Count > 2));
            if (Set<PlayerType>.Of(PlayerType.Cpu3, PlayerType.Cpu4, PlayerType.Cpu5).Contains(Player.PlayerType))
            {
                var players = StrategicMap.Current.Players.GetPlayers((sbyte)player);
                var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(Player.PlayerIndex);
                var defaultRaceRelation = StrategicMap.Current.Races.GetRace(fPlayersPlayers.PlayerRace).GetDefaultRaceRelation(players.PlayerRace);
                if (Set<Status>.Of(Status.Hostile, Status.Wary).Contains(defaultRaceRelation))
                {
                    result = false;
                }
            }

            peace?.Dispose();
            war?.Dispose();
            unknown?.Dispose();
            idealPeace?.Dispose();
            idealWar?.Dispose();
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AI.pas:426
    public bool CanDeclareWar(byte player)
    {
        bool result;
        ByteList peace;
        ByteList war;
        ByteList unknown;
        ByteList idealPeace;
        ByteList idealWar;
        int peaceCount;
        int warCount;
        int peaceIndex;
        int warIndex;
        int index;
        if (!(Player.DiplomaticRelations.GetHistory((sbyte)player).GetRelationOnDay(0) == DiplomaticRelation.Peace && Player.DiplomaticRelations.GetDiplomaticRelation((sbyte)player) == DiplomaticRelation.Peace))
        {
            idealPeace = new ByteList();
            idealWar = new ByteList();
            peace = new ByteList();
            war = new ByteList();
            unknown = new ByteList();
            ListPeaceWarUnknownPlayers(peace, war, unknown);
            ListIdealPeaceWarPlayers(idealPeace, idealWar);
            peace.Remove(player);
            unknown.Remove(player);
            war.Remove(player);
            peaceCount = peace.Count;
            warCount = war.Count;
            peaceIndex = idealPeace.IndexOf(player);
            warIndex = idealWar.IndexOf(player);
            for (int I = 0, last = unknown.Count - 1; I <= last; I++)
            {
                index = idealPeace.IndexOf(unknown[I]);
                if (index != -1)
                {
                    if (peaceIndex == -1 || index < peaceIndex)
                    {
                        peaceCount += 1;
                    }
                    else
                    {
                        warCount += 1;
                    }
                }

                index = idealWar.IndexOf(unknown[I]);
                if (index != -1)
                {
                    if (warIndex == -1 || index < warIndex)
                    {
                        warCount += 1;
                    }
                    else
                    {
                        peaceCount += 1;
                    }
                }
            }

            result = !(warCount > peaceCount && Player.DiplomaticRelations.GetHistory((sbyte)player).GetRelationOnDay(0) != DiplomaticRelation.War);
            peace?.Dispose();
            war?.Dispose();
            unknown?.Dispose();
            idealPeace?.Dispose();
            idealWar?.Dispose();
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AI.pas:472
    public bool CanProposeAlliance(byte player)
    {
        bool result;
        if (CanOfferPeace(player) && Player.DiplomaticRelations.GetDiplomaticRelation((sbyte)player) == DiplomaticRelation.Peace)
        {
            result = StrategicMap.Current.Day > 10;
            var lastModifiedOnDay = Player.DiplomaticRelations.GetHistory((sbyte)player).GetLastModifiedOnDay();
            if (StrategicMap.Current.Day - lastModifiedOnDay < 8)
            {
                result = false;
            }

            var players = StrategicMap.Current.Players.GetPlayers((sbyte)player);
            var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(Player.PlayerIndex);
            var defaultRaceRelation = StrategicMap.Current.Races.GetRace(fPlayersPlayers.PlayerRace).GetDefaultRaceRelation(players.PlayerRace);
            if (Set<Status>.Of(Status.Hostile, Status.Wary).Contains(defaultRaceRelation))
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly ImmutableArray<ImmutableArray<GroupBehavior>> s_player2GroupBehaviorTable = [[GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal], [GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive], [GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive], [GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand], [GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch]];
    private static readonly ImmutableArray<ImmutableArray<GroupBehavior>> s_independentGroupBehaviorTable = [[GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive], [GroupBehavior.Patrol, GroupBehavior.Scout, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive], [GroupBehavior.Patrol, GroupBehavior.Patrol, GroupBehavior.Scout, GroupBehavior.Scout, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive], [GroupBehavior.Patrol, GroupBehavior.Patrol, GroupBehavior.Scout, GroupBehavior.Scout, GroupBehavior.Suicidal, GroupBehavior.Suicidal, GroupBehavior.Raid, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive]];

    // intf_P5_06.inc:65

    // intf_P5_06.inc:65

}
