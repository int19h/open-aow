#nullable disable warnings
// Translated from AoWE.pas.
using System.Collections.Immutable;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI.Groups;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.AI;

// player, actions, action, action index, cycles, move executer, phase, progress
public sealed partial class AIPlayerControl : EngineObject
{
    // AI.pas:84
    public AIPlayerControl(CustomNode? owner) : base(owner)
    {
        Actions = new AIPlayerActionList(null) { Holder = CreationHolder() };
        MoveExecutor = new AIMoveExecutor(null) { Holder = CreationHolder() };
        _processCyclesMax = 10;
    }

    public Player Player { get; init; }
    public AIPlayerActionList? Actions { get; private set; }

    private AIPlayerAction? _action;
    private int _actionIndex;
    private int _processCyclesMax;

    public AIMoveExecutor? MoveExecutor { get; private set; }
    public AIPlayerProcessPhase Phase { get; private set; }

    public float DefenseStrengthMultiplier { get; set; }

    // AI.pas:92
    public override void Dispose()
    {
        Reset();
        try
        {
            Actions?.Dispose();
            Actions = null;
        }
        catch (Exception error)
        {
            HostLog.FallbackAtCallSite(RtlHost.Logger, error: error);
        }

        try
        {
            MoveExecutor?.Dispose();
            MoveExecutor = null;
        }
        catch (Exception error)
        {
            HostLog.FallbackAtCallSite(RtlHost.Logger, error: error);
        }

        base.Dispose();
    }

    // AI.pas:108
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        DefenseStrengthMultiplier = persister.Single("DefenseStrengthMultiplier", value: DefenseStrengthMultiplier);
    }

    // AI.pas:114
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
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
        int result = 0;
        Player player = Player;
        for (int i = 0, last = player.Armies.Count - 1; i <= last; i++)
        {
            Troop? troop = player.Armies.GetArmy(i).Transporter(255);
            if (troop is not null)
            {
                AIGroup? group = troop.Group;
                if (group is null || group.EngineClass.ClassId == TransportAIGroup.ClassId)
                {
                    _ = units.AddUnit(troop);
                    result += 1;
                }
            }
        }

        return result;
    }

    // AI.pas:143
    public int UnitIndexToPriority(int unitIndex) => RosterTables.GetUnitIndexAIPriority(unitIndex, player: Player.PlayerIndex, relation: 0, engine: GetEngine());

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
        Actions.Clear();
        for (int i = 0, last = StrategicMap.Current.AIPlayerActionManager.GetCount() - 1; i <= last; i++)
        {
            AIPlayerActionClass? actionClass = StrategicMap.Current.AIPlayerActionManager.GetRegisteredClasses(i);
            if (actionClass.SupportedPhases.Contains(phase.ToFlag()))
            {
                AIPlayerAction action = actionClass.CreateInstance(null, holder: CreationHolder());
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
    public int GetProcessCyclesMax() => !Player.SingleProcessCycle ? _processCyclesMax : 1;

    // AI.pas:182
    public void Activate(AIExecutor executor)
    {
        Reset();
        DefenseStrengthMultiplier = StrategicMap.Current.Day < 10 ? (float)(StrategicMap.Current.Day * 0.1) : 1;

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
            case PlayerType.Human:
            case PlayerType.Free:
            default:
                break;
        }
    }

    // AI.pas:198
    public void Deactivate() => Reset();

    // AI.pas:203
    public GroupBehavior GetRandomGroupBehavior()
    {
        Player player = Player;
        if (Player.PlayerIndex == PlayerId.Independent)
        {
            int random = Random(10);
            StrategicMapIndependentAIMode independentAIMode = StrategicMap.Current.IndependentAIMode;
            return s_independentGroupBehaviorTable[(int)independentAIMode][random];
        }
        else
        {
            int random = Random(10);
            return s_player2GroupBehaviorTable[(int)player.Behavior][random];
        }
    }

    // AI.pas:213
    public void UpdateSkirmishGroups()
    {
        // GetGroups(i) is the enumerated group. EngineClass caches on every group, as the loop did.
        int skirmishCount = Player.AIGroups.Count(static group => group.EngineClass.ClassId == SkirmishAIGroup.ClassId);

        int wantedCount;
        if (StrategicMap.Current.Day > 5)
        {
            wantedCount = Player.AIGroups.Count / 6;
            if (wantedCount > 4)
            {
                wantedCount = 4;
            }

            if (wantedCount == 0 && (StrategicMap.Current.Day & 0b0001_0000) == 0b0001_0000)
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
            DoubleItemList<Troop> units = [];
            AIGroup? group;
            for (int i = Player.AIGroups.Count - 1; i >= 0; i--)
            {
                group = Player.AIGroups.GetGroups(i);
                if (group.TargetType != AIGroupTargetStatus.Execute && group.EngineClass.ClassId != FortifyAIGroup.ClassId && group.EngineClass.ClassId != SkirmishAIGroup.ClassId && group.EngineClass.ClassId != GuardAreaAIGroup.ClassId && group.EngineClass.ClassId != GuardAIGroup.ClassId && group.EngineClass.ClassId != TransportAIGroup.ClassId)
                {
                    int groupPriority = group.GetProcessPriority() * -10;
                    for (int j = group.Units.Count - 1; j >= 0; j--)
                    {
                        Troop? troop = group.Units.GetUnit(j);
                        if (troop is not Hero)
                        {
                            int unitPriority = troop.Moves;
                            if (troop.GetMoveTypes().Contains(MoveTypes.Flying))
                            {
                                unitPriority += 15;
                            }

                            _ = units.AddAbility(troop, unitPriority + groupPriority);
                        }
                    }
                }
            }

            units.SortDescendingItemB();
            while (skirmishCount < wantedCount && units.Count != 0)
            {
                group = StrategicMap.Current.AIGroupManager.CreateGroup(Player.PlayerIndex, relation: 0, behavior: GroupBehavior.Skirmish);
                DoubleItem<Troop> item = units[0];
                _ = group.AddUnit(item.First);
                _ = group.Release();
                skirmishCount += 1;
                units.Delete(0);
            }
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
        AIPlayerAction? action = _action;
        if (action is not null)
        {
            action.Deactivate();
            _action = null;
        }
    }

    // AI.pas:290
    public void ListPeaceWarUnknownPlayers(ByteList peace, ByteList war, ByteList unknown)
    {
        peace.Clear();
        war.Clear();
        _ = peace.Add(PlayerIdExtensions.ToOriginal(Player.PlayerIndex).TruncateToByte());
        for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            if (i != PlayerIdExtensions.ToOriginal(Player.PlayerIndex))
            {
                if (StrategicMap.Current.Players.GetPlayers(i).Status == PlayerStatus.Playing)
                {
                    DiplomaticRelation relation = StrategicMap.Current.Players.GetDiplomaticRelation(Player.PlayerIndex, otherPlayer: PlayerId.FromOriginal((sbyte)i));
                    if (relation is DiplomaticRelation.Peace or DiplomaticRelation.Alliance)
                    {
                        _ = peace.Add(i.TruncateToByte());
                    }
                    else
                    {
                        if (relation == DiplomaticRelation.War)
                        {
                            _ = war.Add(i.TruncateToByte());
                        }
                        else
                        {
                            if (relation == DiplomaticRelation.Unknown)
                            {
                                if (Player.GetFirstDaySeen(PlayerId.FromOriginal((sbyte)i)) != -1)
                                {
                                    _ = unknown.Add(i.TruncateToByte());
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
        peace.Clear();
        war.Clear();
        DoubleItemList doubleItemList = [];
        _ = doubleItemList.AddAbility(PlayerIdExtensions.ToOriginal(Player.PlayerIndex), 999);
        for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            if (i != PlayerIdExtensions.ToOriginal(Player.PlayerIndex) && StrategicMap.Current.Players.GetPlayers(i).Status == PlayerStatus.Playing)
            {
                int priority = 0;
                DiplomaticRelation relation = StrategicMap.Current.Players.GetDiplomaticRelation(Player.PlayerIndex, otherPlayer: PlayerId.FromOriginal((sbyte)i));
                Player? players = StrategicMap.Current.Players.GetPlayers(i);
                Player? fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(Player.PlayerIndex);
                Status raceRelation = StrategicMap.Current.Races.GetRace(fPlayersPlayers.PlayerRace).GetDefaultRaceRelation(players.PlayerRace);
                DiplomaticAction action = Player.DiplomaticActions.FindLastAction(DeclareWar.Kind, reaction: DiplomaticActionReaction.None, source: PlayerId.FromOriginal(i.TruncateToSByte()), target: Player.PlayerIndex, other: -1);
                if (action is not null)
                {
                    int days = StrategicMap.Current.Day - action.Day;
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
                    default:
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

                if (StrategicMap.Current.Players.GetPlayers(i).PlayerType == PlayerType.Human && Player.PlayerType is PlayerType.Cpu3 or PlayerType.Cpu4 or PlayerType.Cpu5)
                {
                    priority -= 5;
                }

                _ = doubleItemList.AddAbility(i, priority);
            }
        }

        doubleItemList.SortDescendingItemB();
        DoubleItemList.DoubleItem item;
        // The first half, in stored order. Add appends into the byte list.
        foreach (DoubleItemList.DoubleItem entry in doubleItemList.Take(doubleItemList.Count / 2))
        {
            _ = peace.Add(entry.First.TruncateToByte());
        }

        // The second half from the end, so the higher index is added first.
        foreach (DoubleItemList.DoubleItem entry in doubleItemList.Reverse().Take(doubleItemList.Count / 2))
        {
            _ = war.Add(entry.First.TruncateToByte());
        }

        if ((doubleItemList.Count & 1) != 0)
        {
            item = doubleItemList[doubleItemList.Count / 2];
            int middlePlayer = item.First;
            // { The middle player relation is evaluated directly below. }
            Player? players = StrategicMap.Current.Players.GetPlayers(middlePlayer);
            Player? fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(Player.PlayerIndex);
            Status defaultRaceRelation = StrategicMap.Current.Races.GetRace(fPlayersPlayers.PlayerRace).GetDefaultRaceRelation(players.PlayerRace);
            _ = defaultRaceRelation is Status.Hostile or Status.Wary
                ? war.Add(middlePlayer.TruncateToByte())
                : peace.Add(middlePlayer.TruncateToByte());
        }
    }

    // AI.pas:375
    public bool CanOfferPeace(PlayerId? player)
    {
        bool result;
        if (!(Player.DiplomaticRelations.GetHistory(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(player))).GetRelationOnDay(0) == DiplomaticRelation.War && Player.DiplomaticRelations.GetDiplomaticRelation(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(player))) == DiplomaticRelation.War))
        {
            ByteList idealPeace = [];
            ByteList idealWar = [];
            ByteList peace = [];
            ByteList war = [];
            ByteList unknown = [];
            ListPeaceWarUnknownPlayers(peace, war, unknown);
            ListIdealPeaceWarPlayers(idealPeace, war: idealWar);
            _ = war.Remove(PlayerIdExtensions.ToOriginal(player));
            _ = unknown.Remove(PlayerIdExtensions.ToOriginal(player));
            _ = peace.Remove(PlayerIdExtensions.ToOriginal(player));
            int peaceCount = peace.Count;
            int warCount = war.Count;
            int peaceIndex = idealPeace.IndexOf((byte)PlayerIdExtensions.ToOriginal(player));
            int warIndex = idealWar.IndexOf((byte)PlayerIdExtensions.ToOriginal(player));
            for (int i = 0, last = unknown.Count - 1; i <= last; i++)
            {
                int index = idealPeace.IndexOf(unknown[i]);
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

                index = idealWar.IndexOf(unknown[i]);
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
            if (Player.PlayerType is PlayerType.Cpu3 or PlayerType.Cpu4 or PlayerType.Cpu5)
            {
                Player? players = StrategicMap.Current.Players.GetPlayers((sbyte)PlayerIdExtensions.ToOriginal(player));
                Player? fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(Player.PlayerIndex);
                Status defaultRaceRelation = StrategicMap.Current.Races.GetRace(fPlayersPlayers.PlayerRace).GetDefaultRaceRelation(players.PlayerRace);
                if (defaultRaceRelation is Status.Hostile or Status.Wary)
                {
                    result = false;
                }
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // AI.pas:426
    public bool CanDeclareWar(PlayerId? player)
    {
        bool result;
        if (!(Player.DiplomaticRelations.GetHistory(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(player))).GetRelationOnDay(0) == DiplomaticRelation.Peace && Player.DiplomaticRelations.GetDiplomaticRelation(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(player))) == DiplomaticRelation.Peace))
        {
            ByteList idealPeace = [];
            ByteList idealWar = [];
            ByteList peace = [];
            ByteList war = [];
            ByteList unknown = [];
            ListPeaceWarUnknownPlayers(peace, war, unknown);
            ListIdealPeaceWarPlayers(idealPeace, war: idealWar);
            _ = peace.Remove(PlayerIdExtensions.ToOriginal(player));
            _ = unknown.Remove(PlayerIdExtensions.ToOriginal(player));
            _ = war.Remove(PlayerIdExtensions.ToOriginal(player));
            int peaceCount = peace.Count;
            int warCount = war.Count;
            int peaceIndex = idealPeace.IndexOf((byte)PlayerIdExtensions.ToOriginal(player));
            int warIndex = idealWar.IndexOf((byte)PlayerIdExtensions.ToOriginal(player));
            for (int i = 0, last = unknown.Count - 1; i <= last; i++)
            {
                int index = idealPeace.IndexOf(unknown[i]);
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

                index = idealWar.IndexOf(unknown[i]);
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

            result = !(warCount > peaceCount && Player.DiplomaticRelations.GetHistory(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(player))).GetRelationOnDay(0) != DiplomaticRelation.War);
        }
        else
        {
            return false;
        }

        return result;
    }

    // AI.pas:472
    public bool CanProposeAlliance(PlayerId? player)
    {
        bool result;
        if (CanOfferPeace(player) && Player.DiplomaticRelations.GetDiplomaticRelation(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(player))) == DiplomaticRelation.Peace)
        {
            result = StrategicMap.Current.Day > 10;
            int lastModifiedOnDay = Player.DiplomaticRelations.GetHistory(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(player))).LastModifiedOnDay;
            if (StrategicMap.Current.Day - lastModifiedOnDay < 8)
            {
                result = false;
            }

            Player? players = StrategicMap.Current.Players.GetPlayers((sbyte)PlayerIdExtensions.ToOriginal(player));
            Player? fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(Player.PlayerIndex);
            Status defaultRaceRelation = StrategicMap.Current.Races.GetRace(fPlayersPlayers.PlayerRace).GetDefaultRaceRelation(players.PlayerRace);
            if (defaultRaceRelation is Status.Hostile or Status.Wary)
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    private static readonly ImmutableArray<ImmutableArray<GroupBehavior>> s_player2GroupBehaviorTable = [[GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal], [GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive], [GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive], [GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand], [GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch]];
    private static readonly ImmutableArray<ImmutableArray<GroupBehavior>> s_independentGroupBehaviorTable = [[GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive], [GroupBehavior.Patrol, GroupBehavior.Scout, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive], [GroupBehavior.Patrol, GroupBehavior.Patrol, GroupBehavior.Scout, GroupBehavior.Scout, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive], [GroupBehavior.Patrol, GroupBehavior.Patrol, GroupBehavior.Scout, GroupBehavior.Scout, GroupBehavior.Suicidal, GroupBehavior.Suicidal, GroupBehavior.Raid, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive]];
}
