#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Combat.Actions;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Combat;

// FCombatStatus. CombatData. NextCombatObjectId. FResult. Logbook. Sides. Executed.
// ExecutedLock. OnExecuted. Terminated. OnTerminated. FVisibilityMode
public class Battle : EngineObject
{

    // Combat.pas:6067
    public Battle(CustomNode? owner) : base(owner)
    {
        CombatSide combatSide = new(null) { Holder = CreationHolder() };
        Sides[(int)CombatSideIndex.Attacker] = combatSide;
        CombatSide combatSide2 = new(null) { Holder = CreationHolder() };
        Sides[(int)CombatSideIndex.Defender] = combatSide2;
        Sides[(int)CombatSideIndex.Attacker].SideIndex = 0;
        Sides[(int)CombatSideIndex.Defender].SideIndex = 1;
        Sides[(int)CombatSideIndex.Attacker].Opponent = Sides[(int)CombatSideIndex.Defender];
        Sides[(int)CombatSideIndex.Defender].Opponent = Sides[(int)CombatSideIndex.Attacker];
        CombatData = new CombatData(null) { Holder = CreationHolder() };
        Logbook = new CombatLogbook(null) { Holder = CreationHolder() };
        Clear();
    }

    public CombatStatuses Status { get; set; }
    public CombatData CombatData { get; init; }

    private int _nextCombatObjectId;

    public CombatResult CombatResult { get; protected set; }

    public CombatLogbook? Logbook { get; private set; }

    public CombatSide[] Sides { get; } = new CombatSide[2];

    public event NotifyEvent<Battle>? Executed;

    private int _executedLock;

    private Action<Battle> _executed;

    internal void SetExecutedHandler(Action<Battle> handler) => _executed = handler;

    public bool Terminated { get; private set; }

    private NotifyEvent<Battle> _terminated;

    protected void SetTerminatedHandler(NotifyEvent<Battle> handler) => _terminated = handler;

    public byte ExplorationSite { get; set; }
    public EngineObject? Items { get; set; }

    // item list or prisoners army
    // Combat.pas:6082
    public override void Dispose()
    {
        if (Status.Contains(CombatStatuses.Activated))
        {
            Deactivate();
        }

        if (Status.Contains(CombatStatuses.Executing))
        {
            Finalize();
        }

        Executed = null;
        Sides[(int)CombatSideIndex.Attacker]?.Dispose();
        Sides[(int)CombatSideIndex.Defender]?.Dispose();
        CombatData?.Dispose();
        _ = Logbook.Release();
        base.Dispose();
    }

    // Combat.pas:6103
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("AttackerSide", objectValue: Sides[(int)CombatSideIndex.Attacker]);
        persister.Object("DefenderSide", objectValue: Sides[(int)CombatSideIndex.Defender]);
        Logbook = persister.Created("Logbook", objectValue: Logbook);
        persister.Object("CombatData", objectValue: CombatData);
    }

    // Combat.pas:6134
    public override void Clear()
    {
        base.Clear();
        Logbook.Clear();
        _nextCombatObjectId = 1;
        CombatData.Clear();
        Status = CombatStatuses.None;
    }

    // Combat.pas:6625
    public virtual void UpdateStatus()
    {
        int attackers = Sides[(int)CombatSideIndex.Attacker].GetUndestroyedConquerObjects();
        int defenders = Sides[(int)CombatSideIndex.Defender].GetUndestroyedConquerObjects();
        if (attackers != 0 && defenders == 0)
        {
            CombatResult = CombatResult.Victory;
        }
        else
        {
            if (attackers == 0 && defenders != 0)
            {
                CombatResult = CombatResult.Defeat;
            }
            else
            {
                if (attackers == 0 && defenders == 0)
                {
                    CombatResult = CombatResult.BothDefeated;
                }
                else
                {
                    if (attackers != 0 && defenders != 0 && CombatResult != CombatResult.Undecided)
                    {
                        CombatResult = CombatResult.CannotInflictDamage;
                    }
                }
            }
        }

        switch ((int)CombatResult)
        {
            case >= 4 and <= 5:
                {
                    for (int i = CombatData.GetCount() - 1; i >= 0; i--)
                    {
                        if (CombatData.GetObjects(i).Status.Contains(CombatObjectStatuses.Fled))
                        {
                            CombatResult = CombatResult.Fled;
                        }
                    }
                }

                break;
            default:
                break;
        }
    }

    // Combat.pas:6227
    public virtual void ObjectDestroyed(CombatObject combatObject)
    {
        for (int i = CombatData.GetCount() - 1; i >= 0; i--)
        {
            CombatData.GetObjects(i).ObjectDestroyed(combatObject);
        }

        NotifyEvent<Battle>? handlers = Executed;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(Executed));
        handlers?.Invoke(this);
        CheckTerminate();
    }

    // Combat.pas:6218
    public virtual void ObjectSideChanged(CombatObject combatObject)
    {
        for (int i = CombatData.GetCount() - 1; i >= 0; i--)
        {
            if (!combatObject.Status.Contains(CombatObjectStatuses.Destroyed))
            {
                CombatData.GetObjects(i).ObjectSideChanged(combatObject);
            }
        }

        CheckTerminate();
    }

    // Combat.pas:6163
    public virtual CombatUnit CreateCombatUnit()
    {
        return new FastBattleUnit(null)
        {
            Holder = CreationHolder(),
            Id = CreateUniqueCombatObjectId()
        };
    }

    // Combat.pas:6289
    public virtual CombatPlayer CreateCombatPlayer() => new(null) { Holder = CreationHolder() };

    // Combat.pas:6143
    public virtual bool Setup(byte attackerPlayer, int attackerRelation, int attackerX, int attackerY, int attackerL, byte defenderPlayer, int defenderRelation, int X, int Y, int L, bool logActions)
    {
        Clear();
        Status = logActions
            ? Status | CombatStatuses.LogCombatActions
            : Status & ~CombatStatuses.LogCombatActions;

        CombatData data = CombatData;
        data.Location = new MapCoordinate((sbyte)X, (sbyte)Y, (sbyte)L);
        data.AttackerLocation = new MapCoordinate((sbyte)attackerX, (sbyte)attackerY, (sbyte)attackerL);
        data.AttackerPlayer = (sbyte)AddCombatPlayer(attackerPlayer, independentRelation: attackerRelation.TruncateToByte(), side: CombatSideIndex.Attacker);
        data.DefenderPlayer = (sbyte)AddCombatPlayer(defenderPlayer, independentRelation: defenderRelation, side: CombatSideIndex.Defender);
        MapField fieldValue = StrategicMap.Current.MapContainer.GetField(data.Location.X, data.Location.Y, data.Location.MapLevel);
        data.TerrainAndOverlayTypes[0] = (byte)fieldValue.TerrainNumber;
        MapField mapContainerField = StrategicMap.Current.MapContainer.GetField(data.Location.X, data.Location.Y, data.Location.MapLevel);
        data.TerrainAndOverlayTypes[1] = (byte)mapContainerField.OverlayType.ToOriginal();
        return true;
    }

    // Combat.pas:6619
    public virtual CombatResult Execute()
    {
        RandSeed = StrategicMap.Current.Random(16777215);
        return CombatResult.Idle;
    }

    // Combat.pas:6405
    public virtual void Initialize()
    {
        LockExecuted();
        Status |= CombatStatuses.Executing;
        int wallType = CombatData.Parties.FirstOrDefault(static candidate => candidate is not null && candidate.Wall != 0)?.Wall ?? 0;

        if (wallType != 0)
        {
            CombatWall wall = new(null) { Holder = CreationHolder() };
            wall.Setup(CreateUniqueCombatObjectId(), wallType, combatPlayer: (byte)CombatData.DefenderPlayer);
            CombatData.AddObject(wall);
        }

        if (CombatData.Parties.GetParty(0) is not null)
        {
            CombatParty? party = CombatData.Parties.GetParty(0);
            CombatData.WallType = party.Wall;
        }

        _ = CombatData.TurnOrder.Add(CombatData.DefenderPlayer.TruncateToByte());
        byte player;
        for (int i = 0, last = CombatData.Players.Count - 1; i <= last; i++)
        {
            player = (byte)i;
            if (CombatData.TurnOrder.IndexOf(player) == -1 && CombatData.Players.GetPlayers((sbyte)player).SideIndex == 1)
            {
                _ = CombatData.TurnOrder.Add(player);
            }
        }

        _ = CombatData.TurnOrder.Add(CombatData.AttackerPlayer.TruncateToByte());
        for (int i = 0, last = CombatData.Players.Count - 1; i <= last; i++)
        {
            player = (byte)i;
            if (CombatData.TurnOrder.IndexOf(player) == -1 && CombatData.Players.GetPlayers((sbyte)player).SideIndex == 0)
            {
                _ = CombatData.TurnOrder.Add(player);
            }
        }

        for (int i = CombatData.GetCount() - 1; i >= 0; i--)
        {
            CombatData.GetObjects(i).Initialize(this);
        }

        Logbook.SetInitialData(CombatData);
    }

    // Combat.pas:6446
    public virtual void Finalize()
    {
        MapFieldMessage message = default;
        Assert((Status & (CombatStatuses.Executing | CombatStatuses.Activated)) == CombatStatuses.Executing, message: "Combat Finalize Failure", file: "Combat.pas", line: 6523);
        CombatData data;
        for (int i = 0, last = CombatData.DefeatedLeaders.Count - 1; i <= last; i++)
        {
            data = CombatData;
            int item = data.DefeatedLeaders[i];
            CombatPlayer players = data.Players.GetPlayers((sbyte)item);
            StrategicMap.Current.GameOver(PlayerId.FromOriginal(players.Player), status: GameOverStatus.Lost);
            Logbook.PlayerEliminated = true;
        }

        Logbook.SetFinalData(CombatData);
        Status &= ~CombatStatuses.Executing;
        data = CombatData;
        for (int i = data.GetCount() - 1; i >= 0; i--)
        {
            CombatData.GetObjects(i).CombatDone();
        }

        for (int modeOrdinal = (int)FinalizeMode.Original; modeOrdinal <= (int)FinalizeMode.All; modeOrdinal++)
        {
            FinalizeMode mode = (FinalizeMode)modeOrdinal;
            for (int i = data.GetCount() - 1; i >= 0; i--)
            {
                CombatData.GetObjects(i).Finalize(mode);
            }
        }

        string combatResultText = GetCombatResultText();
        Logbook.FinalData.Header = combatResultText;
        Logbook.FinalData.CombatResult = ((int)CombatResult).TruncateToByte();
        int playerIndex = CombatData.Players.Count;
        while (playerIndex != 0 && !Logbook.PlayerEliminated)
        {
            playerIndex -= 1;
            CombatPlayer players = CombatData.Players.GetPlayers((sbyte)playerIndex);
            Player? player = StrategicMap.Current.Players.GetPlayers(players.Player);
            int armyIndex = player.Armies.Count;
            bool alive = false;
            while (armyIndex != 0 && !alive)
            {
                armyIndex -= 1;
                alive = player.Armies.GetArmy(armyIndex).Units.FindLastIndex(static troop => troop.GetHitPoints() != 0) != -1;
            }

            Logbook.PlayerEliminated = !alive;
        }

        CombatData finalData = CombatData;
        if (finalData.Location.X != -1)
        {
            message.MessageId = 4368;
            MapFieldMessageParameters param = message.Param;
            param[0] = this;
            message.Param = param;
            message.Result = 0;
            StrategicMap.Current.MapContainer.GetField(finalData.Location.X, finalData.Location.Y, finalData.Location.MapLevel).SendMapFieldMessage(ref message);
        }

        UnlockExecuted();
    }

    // Combat.pas:6577
    public virtual void Activate()
    {
        Assert(!Status.Contains(CombatStatuses.Activated), message: "Combat already activated", file: "Combat.pas", line: 6595);
        Status |= CombatStatuses.Activated;
        CombatData.Parties.Activate(this);
        for (int i = CombatData.GetCount() - 1; i >= 0; i--)
        {
            CombatData.GetObjects(i).Activate(this);
        }

        if (Status.Contains(CombatStatuses.Executing))
        {
            for (int i = CombatData.GetCount() - 1; i >= 0; i--)
            {
                CombatData.GetObjects(i).SetupTargets();
            }

            UpdateSidesSettings();
        }
    }

    // Combat.pas:6608
    public virtual void Deactivate()
    {
        Assert(Status.Contains(CombatStatuses.Activated), message: "Combat Failure during Deactivating", file: "Combat.pas", line: 6612);
        Status &= ~CombatStatuses.Activated;
        for (int i = CombatData.GetCount() - 1; i >= 0; i--)
        {
            CombatData.GetObjects(i).Deactivate();
        }

        CombatData.Parties.Deactivate(this);
    }

    // Combat.pas:6180
    public virtual void NewTurn(byte combatPlayer)
    {
        NewCombatTurnCombatAction a = new(null)
        {
            Holder = CreationHolder(),
            CombatPlayer = combatPlayer
        };
        ExecuteCombatAction(a);
        _ = a.Release();
        UpdateSidesSettings();
    }

    // Combat.pas:6169
    public virtual void NewRound()
    {
        NewCombatRoundCombatAction a = new(null) { Holder = CreationHolder() };
        ExecuteCombatAction(a);
        _ = a.Release();
    }

    // Combat.pas:6094
    public bool LogCombatActions => Status.Contains(CombatStatuses.LogCombatActions);

    // Combat.pas:6097
    public void SetLogCombatActions(int value)
    {
        Status = value != 0
            ? Status | CombatStatuses.LogCombatActions
            : Status & ~CombatStatuses.LogCombatActions;
    }

    // Combat.pas:6112
    public int Round => CombatData.Round;

    // Combat.pas:6115
    // The tactical AI's damage estimates call these through an unchecked TFastCombat cast in the
    // original. They read only the combat data, so they live on TCombat for both combat kinds.
    // AIGroup.pas:7133
    public int RoundDistance
    {
        get
        {
            int distance = (CombatData.Round - 1) * 2;
            if (CombatData.Players.GetPlayers((sbyte)CombatData.CurrentPlayer).SideIndex == 0)
            {
                distance += 1;
            }

            return distance;
        }
    }

    // AIGroup.pas:7180
    public bool GetWallInBetween(int source, int target)
    {
        if ((source & 0xF) <= 7 && (target & 0xF) <= 7 && CombatData.Wall is not null && !CombatData.Wall.Status.Contains(CombatObjectStatuses.Destroyed))
        {
            source >>>= 4;
            target >>>= 4;
            if (source < 7 && target < 7)
            {
                CombatParty? party = CombatData.Parties.GetParty((byte)source);
                CombatParty? fPartiesParty = CombatData.Parties.GetParty((byte)target);
                return (party.Wall != 0) ^ (fPartiesParty.Wall != 0);
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    // AIGroup.pas:7195
    public WallSituation GetWallSituation(CombatObject source, CombatObject? target)
    {
        return GetWallInBetween(source.Position, target: target.Position)
            ? source.Side == CombatSideIndex.Defender ? WallSituation.Source : WallSituation.Target
            : WallSituation.None;
    }

    public void ExecuteCombatAction(CombatAction? action)
    {
        if (Status.Contains(CombatStatuses.LogCombatActions))
        {
            _ = Logbook.Actions.AddChild(action);
        }

        action.Execute(this);
    }

    // Combat.pas:6121
    public bool PlayerTypeInvolved(PlayerTypes types)
    {
        bool result = false;
        for (int i = 0, last = CombatData.Parties.Count - 1; i <= last; i++)
        {
            if (CombatData.Parties.GetParty((byte)i) is not null)
            {
                CombatParty? party = CombatData.Parties.GetParty((byte)i);
                CombatPlayer players = CombatData.Players.GetPlayers((sbyte)party.PlayerIndex);
                if (types.Contains(StrategicMap.Current.Players.GetPlayers(players.Player).PlayerType.ToFlag()))
                {
                    result = true;
                }
            }
        }

        return result;
    }

    // Combat.pas:6131
    public byte CreateUniqueCombatObjectId()
    {
        byte result = (byte)_nextCombatObjectId;
        _nextCombatObjectId += 1;
        return result;
    }

    // Combat.pas:6177
    public void UpdateSidesSettings()
    {
        Sides[(int)CombatSideIndex.Attacker].UpdateSettings();
        Sides[(int)CombatSideIndex.Defender].UpdateSettings();
    }

    // Combat.pas:6190
    public void NextTurn()
    {
        if (CombatData.TurnOrder.Count != 0)
        {
            int i = CombatData.TurnOrder.IndexOf(CombatData.CurrentPlayer) + 1;
            if (i >= CombatData.TurnOrder.Count)
            {
                i = 0;
            }

            NewTurn(CombatData.TurnOrder[i]);
        }
    }

    // Combat.pas:6201
    public void Terminate()
    {
        if (!Terminated)
        {
            Terminated = true;
            _terminated?.Invoke(this);
        }
    }

    // Combat.pas:6210
    public void CheckTerminate()
    {
        if (!Terminated)
        {
            if (Sides[(int)CombatSideIndex.Attacker].GetUndestroyedConquerObjects() == 0 || Sides[(int)CombatSideIndex.Defender].GetUndestroyedConquerObjects() == 0 || CombatData.DefeatedLeaders.Count != 0)
            {
                Terminate();
            }
        }
    }

    // Combat.pas:6236
    public void SetLogbookHeader(string header)
    {
        UpdateHeaderCombatAction a = new(null)
        {
            Holder = CreationHolder(),
            Header = header
        };
        ExecuteCombatAction(a);
        _ = a.Release();
    }

    // Combat.pas:6245
    public void AddAdjacentArmies()
    {
        CombatData data = CombatData;
        MapLevel level = StrategicMap.Current.MapContainer.GetMapLevel(data.Location.MapLevel);
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, data.Location.X, data.Location.Y, radius: 0);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                ArmyHexSprite armyHexSprite = level.GetField(walk.X, walk.Y).GetArmyHexSprite();
                if (armyHexSprite is not null)
                {
                    CombatSideIndex side;
                    if (walk.HexNumber != 0)
                    {
                        CombatPlayer players = data.Players.GetPlayers(data.DefenderPlayer);
                        CombatPlayer fPlayersPlayers = data.Players.GetPlayers(data.DefenderPlayer);
                        CombatPlayer players2 = data.Players.GetPlayers(data.AttackerPlayer);
                        side = CombatAbilityTables.GetCombatSide(data.Players.GetPlayers(data.AttackerPlayer).Player, attackerRelation: ((byte)players2.IndependentRelation).TruncateToSByte(), defender: PlayerId.FromOriginal(fPlayersPlayers.Player), defenderRelation: players.IndependentRelation.TruncateToSByte(), player: (sbyte)PlayerIdExtensions.ToOriginal(armyHexSprite.Army.Player), relation: armyHexSprite.Army.IndependentRelation);
                    }
                    else
                    {
                        side = CombatSideIndex.Defender;
                    }

                    if (side != CombatSideIndex.Neutral)
                    {
                        _ = AddArmy(armyHexSprite.Army, side, walk.X, walk.Y, L: data.Location.MapLevel);
                    }
                }
                else
                {
                    if (walk.HexNumber == 0)
                    {
                        CombatPlayer players = data.Players.GetPlayers(data.DefenderPlayer);
                        AddEmptyArmy((byte)data.Players.GetPlayers(data.DefenderPlayer).Player, players.IndependentRelation, walk.X, walk.Y, L: data.Location.MapLevel);
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // Combat.pas:6280
    public byte GetPartyPosition(int X, int Y, int layer)
    {
        CombatData data = CombatData;
        return layer == data.Location.MapLevel && HexGeometry.DeltaHexToRadius(data.Location.X, data.Location.Y, X, Y) <= 1
            ? (byte)HexGeometry.DeltaHexToHexNumberFast(data.Location.X, data.Location.Y, X, Y)
            : (byte)7;
    }

    // Combat.pas:6292
    public byte AddCombatPlayer(byte player, int independentRelation, CombatSideIndex side)
    {
        CombatPlayer playerLocal = CreateCombatPlayer();
        playerLocal.SetOwner(CombatData.Players);
        playerLocal.Player = (sbyte)player;
        playerLocal.IndependentRelation = independentRelation;
        playerLocal.SideIndex = ((int)side).TruncateToByte();
        return (byte)(CombatData.Players.Count - 1);
    }

    // Combat.pas:6303
    public CombatParty AddArmy(Army? army, CombatSideIndex side, int X, int Y, int L)
    {
        CombatParty party;
        MapFieldMessage message = default;
        using (army.BeginUpdate())
        {
            byte player = (byte)CombatData.Players.IndexOf((byte)PlayerIdExtensions.ToOriginal(army.Player), relation: army.IndependentRelation.TruncateToByte());
            if (player == 255)
            {
                player = AddCombatPlayer((byte)PlayerIdExtensions.ToOriginal(army.Player), independentRelation: army.IndependentRelation.TruncateToByte(), side);
            }

            byte position = GetPartyPosition(X, Y, layer: L);
            party = CombatData.Parties.CreateParty(this, position, combatPlayer: player, X, Y, L, army);
            Troop? transporter = army.Transporter(255);
            bool transport = transporter is not null && transporter.ValidTerrain(CombatData.TerrainAndOverlayTypes[0].TruncateToSByte(), overlayType: CombatData.TerrainAndOverlayTypes[1].TruncateToSByte());
            int index = 0;
            for (int i = army.Units.Count - 1; i >= 0; i--)
            {
                bool valid = army.Units.GetUnit(i).ValidTerrain(CombatData.TerrainAndOverlayTypes[0].TruncateToSByte(), overlayType: CombatData.TerrainAndOverlayTypes[1].TruncateToSByte());
                if (valid || transport)
                {
                    CombatUnit unitObject = CreateCombatUnit();
                    unitObject.Setup(troop: army.Units.GetUnit(i), id: CreateUniqueCombatObjectId(), side: ((int)side).TruncateToByte(), position: (byte)((position << 4) + index), combatPlayer: player);
                    if (!valid)
                    {
                        unitObject.Disabled = true;
                        unitObject.ControlMode &= ~CombatObjectControlModes.ConquerObject;
                    }

                    CombatData.AddObject(unitObject);
                    index += 1;
                }
            }

            if (X != -1)
            {
                message.MessageId = 4387;
                MapField fieldValue = StrategicMap.Current.MapContainer.GetField(X, Y, L);
                MapFieldMessageParameters param = message.Param;
                param[0] = fieldValue;
                message.Param = param;
                message.Result = 0;
                CombatData data = CombatData;
                StrategicMap.Current.MapContainer.GetField(data.Location.X, data.Location.Y, data.Location.MapLevel).SendMapFieldMessage(ref message);
                if (message.Result.Int != 0)
                {
                    party.Wall = message.Result.Int.TruncateToByte();
                }
            }
        }
        return party;
    }

    // Combat.pas:6345
    public CombatParty AddArmy(Army army, CombatSideIndex side, int X, int Y, int L, byte position)
    {
        CombatParty party;
        using (army.BeginUpdate())
        {
            byte player = (byte)CombatData.Players.IndexOf((byte)PlayerIdExtensions.ToOriginal(army.Player), relation: army.IndependentRelation.TruncateToByte());
            if (player == 255)
            {
                player = AddCombatPlayer((byte)PlayerIdExtensions.ToOriginal(army.Player), independentRelation: army.IndependentRelation.TruncateToByte(), side);
            }

            party = CombatData.Parties.CreateParty(this, position: (byte)(position << 0), combatPlayer: player, X, Y, L, army);
            int index = 0;
            for (int i = army.Units.Count - 1; i >= 0; i--)
            {
                CombatUnit unitObject = CreateCombatUnit();
                unitObject.Setup(troop: army.Units.GetUnit(i), id: CreateUniqueCombatObjectId(), side: ((int)side).TruncateToByte(), position: (byte)((position << 4) + index), combatPlayer: player);
                CombatData.AddObject(unitObject);
                index += 1;
            }
        }
        return party;
    }

    // Combat.pas:6364
    public void AddEmptyArmy(byte player, int independentRelation, int X, int Y, int L)
    {
        MapFieldMessage message = default;
        CombatData data = CombatData;
        byte position = GetPartyPosition(X, Y, layer: L);
        CombatPlayer players = data.Players.GetPlayers(data.DefenderPlayer);
        CombatPlayer fPlayersPlayers = data.Players.GetPlayers(data.DefenderPlayer);
        CombatPlayer players2 = data.Players.GetPlayers(data.AttackerPlayer);
        CombatSideIndex side = CombatAbilityTables.GetCombatSide(data.Players.GetPlayers(data.AttackerPlayer).Player, attackerRelation: ((byte)players2.IndependentRelation).TruncateToSByte(), defender: PlayerId.FromOriginal(fPlayersPlayers.Player), defenderRelation: players.IndependentRelation.TruncateToSByte(), player: (sbyte)player, relation: independentRelation.TruncateToSByte());
        byte playerLocal = (byte)CombatData.Players.IndexOf(player, relation: independentRelation);
        if (playerLocal == 255)
        {
            playerLocal = AddCombatPlayer(player, independentRelation, side);
        }

        CombatParty party = CombatData.Parties.CreateParty(this, position, combatPlayer: playerLocal, X, Y, L, army: null);
        message.MessageId = 4387;
        MapField fieldValue = StrategicMap.Current.MapContainer.GetField(X, Y, L);
        MapFieldMessageParameters param = message.Param;
        param[0] = fieldValue;
        message.Param = param;
        message.Result = 0;
        StrategicMap.Current.MapContainer.GetField(X, Y, L).SendMapFieldMessage(ref message);
        if (message.Result.Int != 0)
        {
            party.Wall = message.Result.Int.TruncateToByte();
        }
    }

    // Combat.pas:6385
    public void InitializeDefaultArmies(Army? army, int X, int Y, int L)
    {
        CombatData data = CombatData;
        if (GetPartyPosition(X, Y, layer: L) == 7)
        {
            ExplorationSite = 2;
            _ = AddArmy(army, side: CombatSideIndex.Attacker, X, Y, L);
            ArmyHexSprite armyHexSprite = StrategicMap.Current.MapContainer.GetField(data.Location.X, data.Location.Y, data.Location.MapLevel).GetArmyHexSprite();
            if (armyHexSprite is not null)
            {
                _ = AddArmy(armyHexSprite.Army, side: CombatSideIndex.Defender, data.Location.X, data.Location.Y, L: data.Location.MapLevel);
            }
            else
            {
                CombatPlayer players = data.Players.GetPlayers(data.DefenderPlayer);
                AddEmptyArmy((byte)data.Players.GetPlayers(data.DefenderPlayer).Player, players.IndependentRelation, data.Location.X, data.Location.Y, L: data.Location.MapLevel);
            }
        }
        else
        {
            AddAdjacentArmies();
        }
    }

    // Combat.pas:6568
    public void LockExecuted() => _executedLock += 1;

    // Combat.pas:6571
    public void UnlockExecuted()
    {
        _executedLock -= 1;
        if (_executedLock == 0 && _executed is not null)
        {
            _executed(this);
        }
    }

    // Combat.pas:6642
    public string GetCombatResultText()
    {
        CombatResultQuery query = new();
        string result = "";
        query.Text = result;
        MapFieldMessage message = default;
        message.MessageId = 4361;
        message.Result = query;
        MapFieldMessageParameters param = message.Param;
        param[0] = this;
        message.Param = param;
        // { direct combat data }
        StrategicMap.Current.MapContainer.GetField(CombatData.Location.X, CombatData.Location.Y, CombatData.Location.MapLevel).SendMapFieldMessage(ref message);
        // Handlers write the query through the message. The text read here is that write.
        result = query.Text;
        if (IsEmpty(result))
        {
            result = CombatResult == CombatResult.Victory
                ? AbilityRules.TranslateResourceString(GetEngine(), S: s_battleWonText)
                : CombatResult == CombatResult.Defeat
                    ? AbilityRules.TranslateResourceString(GetEngine(), S: s_battleLostText)
                    : CombatResult == CombatResult.CannotBreachWall
                        ? AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CannotBreachWallText)
                        : CombatResult == CombatResult.CannotInflictDamage
                            ? AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CannotInflictDamageText)
                            : AbilityRules.TranslateResourceString(GetEngine(), S: s_battleDrawnText);

            query.Text = result;
        }

        return result;
    }

    // Combat.pas:6659
    public bool DefendersAvailable()
    {
        bool result = false;
        int i = 0;
        while (!result && i != CombatData.GetCount())
        {
            CombatObject objectValue = CombatData.GetObjects(i);
            result = objectValue.CombatPlayer != -1 && CombatData.Players.GetPlayers(objectValue.CombatPlayer).SideIndex == 1 && !objectValue.Status.Contains(CombatObjectStatuses.Destroyed);
            i += 1;
        }

        return result;
    }

    private static readonly string s_battleWonText = "Battle Won";
    private static readonly string s_battleLostText = "Battle Lost";
    private static readonly string s_battleDrawnText = "Battle Drawn";
}
