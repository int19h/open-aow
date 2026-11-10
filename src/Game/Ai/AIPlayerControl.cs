// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Ai.Groups;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ai;

// player, actions, action, action index, cycles, move executer, phase, progress
public partial class AIPlayerControl : EObject
{
    // AI.pas:84
    public AIPlayerControl(ECustomNode owner) : base(owner)
    {
        FActions = new AIPlayerActionList(null);
        FMoveExecuter = new AIMoveExecuter(null);
        FProcessCyclesMax = 10;
    }

    public Player FPlayer;
    public AIPlayerActionList FActions;
    public AIPlayerAction FAction;
    public int FActionIndex;
    public int FProcessCyclesMax;
    public AIMoveExecuter FMoveExecuter;
    public AIPlayerProcessPhase FPhase;
    public float FDefenseStrengthMultiplier;

    // AI.pas:92
    public override void Dispose()
    {
        Reset();
        try
        {
            FActions?.Dispose();
            FActions = null;
        }
        catch
        {
        }

        try
        {
            FMoveExecuter?.Dispose();
            FMoveExecuter = null;
        }
        catch
        {
        }

        base.Dispose();
    }

    // AI.pas:108
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwData(Bytes.Of(ref FDefenseStrengthMultiplier, 4), 4, 10);
    }

    // AI.pas:114
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536936471)
        {
            Deactivate();
        }
    }

    // AI.pas:120
    public int ListAvailableTransporters(UnitList Units)
    {
        AbstractUnit AUnit;
        UnitUserList Group;
        int Result = 0;
        var player = FPlayer;
        for (int I = 0, last = player.FArmies.Count - 1; I <= last; I++)
        {
            AUnit = player.FArmies.GetArmy(I).Transporter(255);
            if (AUnit != null)
            {
                Group = AUnit.FGroup;
                if (Group == null || Group.EClass.ClassID == 136295)
                {
                    Units.AddUnit(AUnit);
                    Result += 1;
                }
            }
        }

        return Result;
    }

    // AI.pas:143
    public int UnitIndexToPriority(int UnitIndex) => RosterTables.GetUnitIndexAIPriority(UnitIndex, FPlayer.FPlayerIndex, 0);

    // AI.pas:148
    public void Reset()
    {
        FPhase = AIPlayerProcessPhase.None;
        FActionIndex = -1;
        DeactivateAction();
        FActions.Clear();
    }

    // AI.pas:156
    public void SetupProcessPhase(AIPlayerProcessPhase Phase)
    {
        AIPlayerActionClass ActionClass;
        AIPlayerAction Action;
        FActions.Clear();
        for (int I = 0, last = AoWHSMap.Current.FAIPlayerActionManager.GetCount() - 1; I <= last; I++)
        {
            ActionClass = AoWHSMap.Current.FAIPlayerActionManager.GetRegisteredClasses(I);
            if (ActionClass.SupportedPhases.Contains(Phase))
            {
                Action = ActionClass.Create(null);
                if (Action.Setup(this, Phase))
                {
                    Action.SetOwner(FActions);
                }
                else
                {
                    Action?.Dispose();
                }
            }
        }

        FPhase = Phase;
    }

    // AI.pas:176
    public int GetProcessCyclesMax()
    {
        int Result;
        if (!FPlayer.FSingleProcessCycle)
        {
            Result = FProcessCyclesMax;
        }
        else
        {
            Result = 1;
        }

        return Result;
    }

    // AI.pas:182
    public void Activate(AIExecuter Executer)
    {
        Reset();
        if (AoWHSMap.Current.FDay < 10)
        {
            FDefenseStrengthMultiplier = (float)((double)AoWHSMap.Current.FDay * 0.1);
        }
        else
        {
            FDefenseStrengthMultiplier = 1;
        }

        SetupProcessPhase(AIPlayerProcessPhase.Main);
        switch (FPlayer.FPlayerType)
        {
            case PlayerType.CPU1:
                FProcessCyclesMax = 5;
                break;
            case PlayerType.CPU2:
                FProcessCyclesMax = 7;
                break;
            case PlayerType.CPU3:
                FProcessCyclesMax = 9;
                break;
            case PlayerType.CPU4:
                FProcessCyclesMax = 9;
                break;
            case PlayerType.CPU5:
                FProcessCyclesMax = 9;
                break;
            case PlayerType.Independent:
                FProcessCyclesMax = 3;
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
        GroupBehavior Result;
        Player Player;
        Player = FPlayer;
        if (FPlayer.FPlayerIndex == 0)
        {
            var random = Random(10);
            var independentAIMode = AoWHSMap.Current.GetIndependentAIMode();
            Result = IndependentGroupBehaviorTable[(int)independentAIMode][random];
        }
        else
        {
            var random = Random(10);
            Result = Player2GroupBehaviorTable[(int)Player.FBehavior][random];
        }

        return Result;
    }

    // AI.pas:213
    public void UpdateSkirmishGroups()
    {
        int SkirmishCount;
        int WantedCount;
        int GroupPriority;
        int UnitPriority;
        AIGroup Group;
        AbstractUnit AUnit;
        DoubleItemList<AbstractUnit> Units;
        DoubleItem<AbstractUnit> Item;
        SkirmishCount = 0;
        for (int I = 0, last = FPlayer.FAIGroups.Count - 1; I <= last; I++)
        {
            if (FPlayer.FAIGroups.GetGroups(I).EClass.ClassID == 136294)
            {
                SkirmishCount += 1;
            }
        }

        if (AoWHSMap.Current.FDay > 5)
        {
            WantedCount = FPlayer.FAIGroups.Count / 6;
            if (WantedCount > 4)
            {
                WantedCount = 4;
            }

            if (WantedCount == 0 && (AoWHSMap.Current.FDay & 16) == 16)
            {
                WantedCount = 1;
            }
        }
        else
        {
            WantedCount = 2;
        }

        if (SkirmishCount < WantedCount)
        {
            Units = new DoubleItemList<AbstractUnit>();
            for (int I = FPlayer.FAIGroups.Count - 1; I >= 0; I--)
            {
                Group = FPlayer.FAIGroups.GetGroups(I);
                if (Group.FTargetType != AIGroupTargetStatus.Execute && Group.EClass.ClassID != 136281 && Group.EClass.ClassID != 136294 && Group.EClass.ClassID != 136273 && Group.EClass.ClassID != 136275 && Group.EClass.ClassID != 136295)
                {
                    GroupPriority = Group.GetProcessPriority() * -10;
                    for (int J = Group.Count - 1; J >= 0; J--)
                    {
                        AUnit = Group.GetUnit(J);
                        if (!(AUnit is Hero))
                        {
                            UnitPriority = AUnit.GetMoves();
                            if (AUnit.GetMoveTypes().Contains(MoveType.Flying))
                            {
                                UnitPriority += 15;
                            }

                            Units.AddAB(AUnit, UnitPriority + GroupPriority);
                        }
                    }
                }
            }

            Units.SortDescendingItemB();
            while (SkirmishCount < WantedCount && Units.Count != 0)
            {
                Group = AoWHSMap.Current.FAIGroupManager.CreateGroup(FPlayer.FPlayerIndex, 0, GroupBehavior.Skirmish);
                Item = Units[0];
                Group.AddUnit(Item.A);
                Group.Release();
                SkirmishCount += 1;
                Units.Delete(0);
            }

            Units?.Dispose();
        }
    }

    // AI.pas:270
    public bool ActivateAction(int Index)
    {
        DeactivateAction();
        FAction = FActions.GetActions(Index);
        bool Result = FAction.Activate();
        if (!Result)
        {
            FAction = null;
        }

        return Result;
    }

    // AI.pas:278
    public void DeactivateAction()
    {
        AIPlayerAction Action;
        Action = FAction;
        if (Action != null)
        {
            Action.Deactivate();
            FAction = null;
        }
    }

    // AI.pas:290
    public void ListPeaceWarUnknownPlayers(ByteList Peace, ByteList War, ByteList Unknown)
    {
        DiplomaticRelation Relation;
        Peace.Clear();
        War.Clear();
        Peace.Add((byte)FPlayer.FPlayerIndex);
        for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            if (I != FPlayer.FPlayerIndex)
            {
                if (AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus == Player.Status.Playing)
                {
                    Relation = AoWHSMap.Current.FPlayers.GetDiplomaticRelation(FPlayer.FPlayerIndex, (sbyte)I);
                    if (Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Alliance).Contains(Relation))
                    {
                        Peace.Add((byte)I);
                    }
                    else
                    {
                        if (Relation == DiplomaticRelation.War)
                        {
                            War.Add((byte)I);
                        }
                        else
                        {
                            if (Relation == DiplomaticRelation.Unknown)
                            {
                                if (FPlayer.GetFirstDaySeen((sbyte)I) != -1)
                                {
                                    Unknown.Add((byte)I);
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
    public void ListIdealPeaceWarPlayers(ByteList Peace, ByteList War)
    {
        DoubleItemList Players;
        DiplomaticRelation Relation;
        int Priority;
        Status RaceRelation;
        int Days;
        int MiddlePlayer;
        DiplomaticAction Action;
        DoubleItemList.DoubleItem Item;
        Peace.Clear();
        War.Clear();
        Players = new DoubleItemList();
        Players.AddAB(FPlayer.FPlayerIndex, 999);
        for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            if (I != FPlayer.FPlayerIndex && AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus == Player.Status.Playing)
            {
                Priority = 0;
                Relation = AoWHSMap.Current.FPlayers.GetDiplomaticRelation(FPlayer.FPlayerIndex, (sbyte)I);
                var players = AoWHSMap.Current.FPlayers.GetPlayers(I);
                var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer.FPlayerIndex);
                RaceRelation = AoWHSMap.Current.FRaces.GetRace(fPlayersPlayers.FPlayerRace).GetDefaultRaceRelation(players.FPlayerRace);
                Action = FPlayer.FDiplomaticActions.FindLastAction(DeclareWar.Kind, DiplomaticActionReaction.None, (sbyte)I, FPlayer.FPlayerIndex, -1);
                if (Action != null)
                {
                    Days = AoWHSMap.Current.FDay - Action.FDay;
                    if (Days < 30)
                    {
                        Priority = Days + (Priority - 30);
                    }
                }

                switch (RaceRelation)
                {
                    case Status.Hostile:
                    case Status.Wary:
                        Priority += 0;
                        break;
                    case Status.Neutral:
                        Priority += 10;
                        break;
                    case Status.Polite:
                    case Status.Friendly:
                        Priority += 20;
                        break;
                }

                if (Relation == DiplomaticRelation.Alliance)
                {
                    Priority += 50;
                }
                else
                {
                    if (Relation == DiplomaticRelation.Peace)
                    {
                        Priority += 15;
                    }
                }

                if (AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType == PlayerType.Human && Set<PlayerType>.Of(PlayerType.CPU3, PlayerType.CPU4, PlayerType.CPU5).Contains(FPlayer.FPlayerType))
                {
                    Priority -= 5;
                }

                Players.AddAB(I, Priority);
            }
        }

        Players.SortDescendingItemB();
        for (int I = 0, last = Players.Count / 2 - 1; I <= last; I++)
        {
            Item = Players[I];
            Peace.Add((byte)Item.A);
        }

        for (int I = Players.Count - 1, last = (Players.Count + 1) / 2; I >= last; I--)
        {
            Item = Players[I];
            War.Add((byte)Item.A);
        }

        if ((Players.Count & 1) != 0)
        {
            Item = Players[Players.Count / 2];
            MiddlePlayer = Item.A;
            // { The middle player relation is evaluated directly below. }
            var players = AoWHSMap.Current.FPlayers.GetPlayers(MiddlePlayer);
            var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer.FPlayerIndex);
            var defaultRaceRelation = AoWHSMap.Current.FRaces.GetRace(fPlayersPlayers.FPlayerRace).GetDefaultRaceRelation(players.FPlayerRace);
            if (Set<Status>.Of(Status.Hostile, Status.Wary).Contains(defaultRaceRelation))
            {
                War.Add((byte)MiddlePlayer);
            }
            else
            {
                Peace.Add((byte)MiddlePlayer);
            }
        }

        Players?.Dispose();
    }

    // AI.pas:375
    public bool CanOfferPeace(byte Player)
    {
        bool Result;
        ByteList Peace;
        ByteList War;
        ByteList Unknown;
        ByteList IdealPeace;
        ByteList IdealWar;
        int PeaceCount;
        int WarCount;
        int PeaceIndex;
        int WarIndex;
        int Index;
        if (!(FPlayer.FDiplomaticRelations.GetHistory((sbyte)Player).GetRelationOnDay(0) == DiplomaticRelation.War && FPlayer.FDiplomaticRelations.GetDiplomaticRelation((sbyte)Player) == DiplomaticRelation.War))
        {
            IdealPeace = new ByteList();
            IdealWar = new ByteList();
            Peace = new ByteList();
            War = new ByteList();
            Unknown = new ByteList();
            ListPeaceWarUnknownPlayers(Peace, War, Unknown);
            ListIdealPeaceWarPlayers(IdealPeace, IdealWar);
            War.Remove(Player);
            Unknown.Remove(Player);
            Peace.Remove(Player);
            PeaceCount = Peace.Count;
            WarCount = War.Count;
            PeaceIndex = IdealPeace.IndexOf(Player);
            WarIndex = IdealWar.IndexOf(Player);
            for (int I = 0, last = Unknown.Count - 1; I <= last; I++)
            {
                Index = IdealPeace.IndexOf(Unknown[I]);
                if (Index != -1)
                {
                    if (PeaceIndex == -1 || Index < PeaceIndex)
                    {
                        PeaceCount += 1;
                    }
                    else
                    {
                        WarCount += 1;
                    }
                }

                Index = IdealWar.IndexOf(Unknown[I]);
                if (Index != -1)
                {
                    if (WarIndex == -1 || Index < WarIndex)
                    {
                        WarCount += 1;
                    }
                    else
                    {
                        PeaceCount += 1;
                    }
                }
            }

            Result = !(PeaceCount > WarCount && !(WarCount == 0 && PeaceCount == 1 && AoWHSMap.Current.FPlayers.FActivePlayers.Count > 2));
            if (Set<PlayerType>.Of(PlayerType.CPU3, PlayerType.CPU4, PlayerType.CPU5).Contains(FPlayer.FPlayerType))
            {
                var players = AoWHSMap.Current.FPlayers.GetPlayers((sbyte)Player);
                var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer.FPlayerIndex);
                var defaultRaceRelation = AoWHSMap.Current.FRaces.GetRace(fPlayersPlayers.FPlayerRace).GetDefaultRaceRelation(players.FPlayerRace);
                if (Set<Status>.Of(Status.Hostile, Status.Wary).Contains(defaultRaceRelation))
                {
                    Result = false;
                }
            }

            Peace?.Dispose();
            War?.Dispose();
            Unknown?.Dispose();
            IdealPeace?.Dispose();
            IdealWar?.Dispose();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AI.pas:426
    public bool CanDeclareWar(byte Player)
    {
        bool Result;
        ByteList Peace;
        ByteList War;
        ByteList Unknown;
        ByteList IdealPeace;
        ByteList IdealWar;
        int PeaceCount;
        int WarCount;
        int PeaceIndex;
        int WarIndex;
        int Index;
        if (!(FPlayer.FDiplomaticRelations.GetHistory((sbyte)Player).GetRelationOnDay(0) == DiplomaticRelation.Peace && FPlayer.FDiplomaticRelations.GetDiplomaticRelation((sbyte)Player) == DiplomaticRelation.Peace))
        {
            IdealPeace = new ByteList();
            IdealWar = new ByteList();
            Peace = new ByteList();
            War = new ByteList();
            Unknown = new ByteList();
            ListPeaceWarUnknownPlayers(Peace, War, Unknown);
            ListIdealPeaceWarPlayers(IdealPeace, IdealWar);
            Peace.Remove(Player);
            Unknown.Remove(Player);
            War.Remove(Player);
            PeaceCount = Peace.Count;
            WarCount = War.Count;
            PeaceIndex = IdealPeace.IndexOf(Player);
            WarIndex = IdealWar.IndexOf(Player);
            for (int I = 0, last = Unknown.Count - 1; I <= last; I++)
            {
                Index = IdealPeace.IndexOf(Unknown[I]);
                if (Index != -1)
                {
                    if (PeaceIndex == -1 || Index < PeaceIndex)
                    {
                        PeaceCount += 1;
                    }
                    else
                    {
                        WarCount += 1;
                    }
                }

                Index = IdealWar.IndexOf(Unknown[I]);
                if (Index != -1)
                {
                    if (WarIndex == -1 || Index < WarIndex)
                    {
                        WarCount += 1;
                    }
                    else
                    {
                        PeaceCount += 1;
                    }
                }
            }

            Result = !(WarCount > PeaceCount && FPlayer.FDiplomaticRelations.GetHistory((sbyte)Player).GetRelationOnDay(0) != DiplomaticRelation.War);
            Peace?.Dispose();
            War?.Dispose();
            Unknown?.Dispose();
            IdealPeace?.Dispose();
            IdealWar?.Dispose();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AI.pas:472
    public bool CanProposeAlliance(byte Player)
    {
        bool Result;
        if (CanOfferPeace(Player) && FPlayer.FDiplomaticRelations.GetDiplomaticRelation((sbyte)Player) == DiplomaticRelation.Peace)
        {
            Result = AoWHSMap.Current.FDay > 10;
            var lastModifiedOnDay = FPlayer.FDiplomaticRelations.GetHistory((sbyte)Player).GetLastModifiedOnDay();
            if (AoWHSMap.Current.FDay - lastModifiedOnDay < 8)
            {
                Result = false;
            }

            var players = AoWHSMap.Current.FPlayers.GetPlayers((sbyte)Player);
            var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer.FPlayerIndex);
            var defaultRaceRelation = AoWHSMap.Current.FRaces.GetRace(fPlayersPlayers.FPlayerRace).GetDefaultRaceRelation(players.FPlayerRace);
            if (Set<Status>.Of(Status.Hostile, Status.Wary).Contains(defaultRaceRelation))
            {
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static FixedArray_Aow_Units_FixedArray_Aow_Units_AoWE_TGroupBehavior_10_5 Player2GroupBehaviorTable = FixedArray_Aow_Units_FixedArray_Aow_Units_AoWE_TGroupBehavior_10_5.From([FixedArray_Aow_Units_AoWE_TGroupBehavior_10.From([GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal, GroupBehavior.Normal]), FixedArray_Aow_Units_AoWE_TGroupBehavior_10.From([GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive, GroupBehavior.Aggressive]), FixedArray_Aow_Units_AoWE_TGroupBehavior_10.From([GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive, GroupBehavior.Defensive]), FixedArray_Aow_Units_AoWE_TGroupBehavior_10.From([GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand, GroupBehavior.Expand]), FixedArray_Aow_Units_AoWE_TGroupBehavior_10.From([GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch, GroupBehavior.Scorch])]);
    private static FixedArray_Aow_Units_FixedArray_Aow_Units_AoWE_TGroupBehavior_10_4 IndependentGroupBehaviorTable = FixedArray_Aow_Units_FixedArray_Aow_Units_AoWE_TGroupBehavior_10_4.From([FixedArray_Aow_Units_AoWE_TGroupBehavior_10.From([GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive]), FixedArray_Aow_Units_AoWE_TGroupBehavior_10.From([GroupBehavior.Patrol, GroupBehavior.Scout, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive]), FixedArray_Aow_Units_AoWE_TGroupBehavior_10.From([GroupBehavior.Patrol, GroupBehavior.Patrol, GroupBehavior.Scout, GroupBehavior.Scout, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive]), FixedArray_Aow_Units_AoWE_TGroupBehavior_10.From([GroupBehavior.Patrol, GroupBehavior.Patrol, GroupBehavior.Scout, GroupBehavior.Scout, GroupBehavior.Suicidal, GroupBehavior.Suicidal, GroupBehavior.Raid, GroupBehavior.Passive, GroupBehavior.Passive, GroupBehavior.Passive])]);

    // intf_P5_06.inc:65

    // intf_P5_06.inc:65

}
