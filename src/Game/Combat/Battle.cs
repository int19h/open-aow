// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat.Actions;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// FCombatStatus. CombatData. NextCombatObjectId. FResult. Logbook. Sides. FExecutedEvents.
// ExecutedLock. OnExecuted. Terminated. OnTerminated. FVisibilityMode
public partial class Battle : EngineObject
{

    // Combat.pas:6067
    public Battle(ECustomNode owner) : base(owner)
    {
        _executedEventsStore = new();
        var combatSide = new CombatSide(null);
        Sides[(int)CombatSideIndex.Attacker] = combatSide;
        var combatSide2 = new CombatSide(null);
        Sides[(int)CombatSideIndex.Defender] = combatSide2;
        Sides[(int)CombatSideIndex.Attacker].SideIndex = 0;
        Sides[(int)CombatSideIndex.Defender].SideIndex = 1;
        Sides[(int)CombatSideIndex.Attacker].Opponent = Sides[(int)CombatSideIndex.Defender];
        Sides[(int)CombatSideIndex.Defender].Opponent = Sides[(int)CombatSideIndex.Attacker];
        CombatData = new CombatData(null);
        Logbook = new CombatLogbook(null);
        Clear();
    }

    public Set<CombatStatusElement> Status { get; set; }
    public CombatData CombatData { get; init; }

    private int _nextCombatObjectId;

    public CombatResult CombatResult { get; protected set; }

    public CombatLogbook Logbook;

    public CombatSide[] Sides { get; } = new CombatSide[2];

    private EventStore<Battle> _executedEventsStore;

    public event NotifyEvent<Battle> FExecutedEvents
    {
        add => _executedEventsStore.Add(value);
        remove => _executedEventsStore.Remove(value);
    }

    private int _executedLock;

    public ExecutedEvent OnExecuted { get; set; }
    public bool Terminated { get; private set; }
    public NotifyEvent<Battle> OnTerminated { get; protected set; }
    public byte ExplorationSite { get; set; }
    public EngineObject Items { get; set; }

    // item list or prisoners army
    // Combat.pas:6082
    public override void Dispose()
    {
        if (Status.Contains(CombatStatusElement.Activated))
        {
            Deactivate();
        }

        if (Status.Contains(CombatStatusElement.Executing))
        {
            Finalize();
        }

        _executedEventsStore.Clear();
        Sides[(int)CombatSideIndex.Attacker]?.Dispose();
        Sides[(int)CombatSideIndex.Defender]?.Dispose();
        CombatData?.Dispose();
        Logbook.Release();
        base.Dispose();
    }

    // Combat.pas:6103
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(Sides[(int)CombatSideIndex.Attacker], 10);
        stream.rwEObject(Sides[(int)CombatSideIndex.Defender], 11);
        stream.rwCreateEObject(ref Logbook, null, 12);
        stream.rwEObject(CombatData, 13);
    }

    // Combat.pas:6134
    public override void Clear()
    {
        base.Clear();
        Logbook.Clear();
        _nextCombatObjectId = 1;
        CombatData.Clear();
        Status = Set<CombatStatusElement>.Of();
    }

    // Combat.pas:6625
    public virtual void UpdateStatus()
    {
        int attackers;
        int defenders;
        attackers = Sides[(int)CombatSideIndex.Attacker].GetUndestroyedConquerObjects();
        defenders = Sides[(int)CombatSideIndex.Defender].GetUndestroyedConquerObjects();
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
                    for (int I = CombatData.GetCount() - 1; I >= 0; I--)
                    {
                        if (CombatData.GetObjects(I).Status.Contains(CombatObjectStatusElement.Fled))
                        {
                            CombatResult = CombatResult.Fled;
                        }
                    }
                }

                break;
        }
    }

    // Combat.pas:6227
    public virtual void ObjectDestroyed(CombatObject combatObject)
    {
        for (int I = CombatData.GetCount() - 1; I >= 0; I--)
        {
            CombatData.GetObjects(I).ObjectDestroyed(combatObject);
        }

        _executedEventsStore.Raise(this);
        CheckTerminate();
    }

    // Combat.pas:6218
    public virtual void ObjectSideChanged(CombatObject combatObject)
    {
        for (int I = CombatData.GetCount() - 1; I >= 0; I--)
        {
            if (!combatObject.Status.Contains(CombatObjectStatusElement.Destroyed))
            {
                CombatData.GetObjects(I).ObjectSideChanged(combatObject);
            }
        }

        CheckTerminate();
    }

    // Combat.pas:6163
    public virtual CombatUnit CreateCombatUnit()
    {
        CombatUnit result = new FastBattleUnit(null);
        result.Id = CreateUniqueCombatObjectId();
        return result;
    }

    // Combat.pas:6289
    public virtual CombatPlayer CreateCombatPlayer() => new CombatPlayer(null);

    // Combat.pas:6143
    public virtual bool Setup(byte attackerPlayer, byte attackerRelation, int attackerX, int attackerY, int attackerL, byte defenderPlayer, byte defenderRelation, int X, int Y, int L, bool logActions)
    {
        CombatData data;
        Clear();
        if (logActions)
        {
            Status = Status + Set<CombatStatusElement>.Of(CombatStatusElement.LogCombatActions);
        }
        else
        {
            Status = Status - Set<CombatStatusElement>.Of(CombatStatusElement.LogCombatActions);
        }

        data = CombatData;
        data.X = X;
        data.Y = Y;
        data.MapLevel = L;
        data.AttackerX = attackerX;
        data.AttackerY = attackerY;
        data.AttackerL = attackerL;
        data.AttackerPlayer = (sbyte)AddCombatPlayer(attackerPlayer, attackerRelation, CombatSideIndex.Attacker);
        data.DefenderPlayer = (sbyte)AddCombatPlayer(defenderPlayer, defenderRelation, CombatSideIndex.Defender);
        var fieldValue = AoWHSMap.Current.MapContainer.GetField(data.X, data.Y, data.MapLevel);
        data.TerrainAndOverlayTypes[0] = (byte)fieldValue.TerrainType;
        var mapContainerField = AoWHSMap.Current.MapContainer.GetField(data.X, data.Y, data.MapLevel);
        data.TerrainAndOverlayTypes[1] = (byte)mapContainerField.OverlayType;
        return true;
    }

    // Combat.pas:6619
    public virtual CombatResult Execute()
    {
        RandSeed = AoWHSMap.Current.Random(16777215);
        return CombatResult.Idle;
    }

    // Combat.pas:6405
    public virtual void Initialize()
    {
        int J;
        CombatWall wall;
        byte wallType;
        byte player;
        LockExecuted();
        Status = Status + Set<CombatStatusElement>.Of(CombatStatusElement.Executing);
        wallType = 0;
        J = 0;
        while (wallType == 0 && J != CombatData.Parties.Count)
        {
            if (CombatData.Parties.GetParty((byte)J) != null)
            {
                wallType = CombatData.Parties.GetParty((byte)J).Wall;
            }

            J += 1;
        }

        if (wallType != 0)
        {
            wall = new CombatWall(null);
            wall.Setup(CreateUniqueCombatObjectId(), wallType, (byte)CombatData.DefenderPlayer);
            CombatData.AddObject(wall);
        }

        if (CombatData.Parties.GetParty(0) != null)
        {
            var party = CombatData.Parties.GetParty(0);
            CombatData.WallType = party.Wall;
        }

        CombatData.TurnOrder.Add((byte)CombatData.DefenderPlayer);
        for (int I = 0, last = CombatData.Players.Count - 1; I <= last; I++)
        {
            player = (byte)I;
            if (CombatData.TurnOrder.IndexOf(player) == -1 && CombatData.Players.GetPlayers((sbyte)player).SideIndex == 1)
            {
                CombatData.TurnOrder.Add(player);
            }
        }

        CombatData.TurnOrder.Add((byte)CombatData.AttackerPlayer);
        for (int I = 0, last = CombatData.Players.Count - 1; I <= last; I++)
        {
            player = (byte)I;
            if (CombatData.TurnOrder.IndexOf(player) == -1 && CombatData.Players.GetPlayers((sbyte)player).SideIndex == 0)
            {
                CombatData.TurnOrder.Add(player);
            }
        }

        for (int I = CombatData.GetCount() - 1; I >= 0; I--)
        {
            CombatData.GetObjects(I).Initialize(this);
        }

        Logbook.SetInitialData(CombatData);
    }

    // Combat.pas:6446
    public virtual void Finalize()
    {
        int playerIndex;
        int armyIndex;
        int unitIndex;
        Player player;
        bool alive;
        FinalizeMode mode;
        CombatData data;
        CombatData finalData;
        MapFieldMessage message = default;
        Assert((Status & Set<CombatStatusElement>.Of(CombatStatusElement.Executing, CombatStatusElement.Activated)) == Set<CombatStatusElement>.Of(CombatStatusElement.Executing), "Combat Finalize Failure", "Combat.pas", 6523);
        for (int I = 0, last = CombatData.DefeatedLeaders.Count - 1; I <= last; I++)
        {
            data = CombatData;
            var item = data.DefeatedLeaders[I];
            var players = data.Players.GetPlayers((sbyte)item);
            AoWHSMap.Current.GameOver(players.Player, GameOverStatus.Lost);
            Logbook.PlayerEliminated = true;
        }

        Logbook.SetFinalData(CombatData);
        Status = Status - Set<CombatStatusElement>.Of(CombatStatusElement.Executing);
        data = CombatData;
        for (int I = data.GetCount() - 1; I >= 0; I--)
        {
            CombatData.GetObjects(I).CombatDone();
        }

        for (int modeOrdinal = (int)FinalizeMode.Original; modeOrdinal <= (int)FinalizeMode.All; modeOrdinal++)
        {
            mode = (FinalizeMode)modeOrdinal;
            for (int I = data.GetCount() - 1; I >= 0; I--)
            {
                CombatData.GetObjects(I).Finalize(mode);
            }
        }

        var combatResultText = GetCombatResultText();
        Logbook.FinalData.Header = combatResultText;
        Logbook.FinalData.CombatResult = (byte)(int)CombatResult;
        playerIndex = CombatData.Players.Count;
        while (playerIndex != 0 && !Logbook.PlayerEliminated)
        {
            playerIndex -= 1;
            var players = CombatData.Players.GetPlayers((sbyte)playerIndex);
            player = AoWHSMap.Current.Players.GetPlayers(players.Player);
            armyIndex = player.Armies.Count;
            alive = false;
            while (armyIndex != 0 && !alive)
            {
                armyIndex -= 1;
                unitIndex = player.Armies.GetArmy(armyIndex).Count;
                while (!alive && unitIndex != 0)
                {
                    unitIndex -= 1;
                    alive = player.Armies.GetArmy(armyIndex).GetUnit(unitIndex).GetHitPoints() != 0;
                }
            }

            Logbook.PlayerEliminated = !alive;
        }

        finalData = CombatData;
        if (finalData.X != -1)
        {
            message.MessageId = 4368;
            message.Param[0] = this;
            message.Result = 0;
            AoWHSMap.Current.MapContainer.GetField(finalData.X, finalData.Y, finalData.MapLevel).SendMapFieldMessage(ref message);
        }

        UnlockExecuted();
    }

    // Combat.pas:6577
    public virtual void Activate()
    {
        Assert(!Status.Contains(CombatStatusElement.Activated), "Combat already activated", "Combat.pas", 6595);
        Status = Status + Set<CombatStatusElement>.Of(CombatStatusElement.Activated);
        CombatData.Parties.Activate(this);
        for (int I = CombatData.GetCount() - 1; I >= 0; I--)
        {
            CombatData.GetObjects(I).Activate(this);
        }

        if (Status.Contains(CombatStatusElement.Executing))
        {
            for (int I = CombatData.GetCount() - 1; I >= 0; I--)
            {
                CombatData.GetObjects(I).SetupTargets();
            }

            UpdateSidesSettings();
        }
    }

    // Combat.pas:6608
    public virtual void Deactivate()
    {
        Assert(Status.Contains(CombatStatusElement.Activated), "Combat Failure during Deactivating", "Combat.pas", 6612);
        Status = Status - Set<CombatStatusElement>.Of(CombatStatusElement.Activated);
        for (int I = CombatData.GetCount() - 1; I >= 0; I--)
        {
            CombatData.GetObjects(I).Deactivate();
        }

        CombatData.Parties.Deactivate(this);
    }

    // Combat.pas:6180
    public virtual void NewTurn(byte combatPlayer)
    {
        NewCombatTurnCombatAction A;
        A = new NewCombatTurnCombatAction(null);
        A.CombatPlayer = combatPlayer;
        ExecuteCombatAction(A);
        A.Release();
        UpdateSidesSettings();
    }

    // Combat.pas:6169
    public virtual void NewRound()
    {
        NewCombatRoundCombatAction A;
        A = new NewCombatRoundCombatAction(null);
        ExecuteCombatAction(A);
        A.Release();
    }

    // Combat.pas:6094
    public bool GetLogCombatActions() => Status.Contains(CombatStatusElement.LogCombatActions);

    // Combat.pas:6097
    // { }
    public void SetLogCombatActions(byte value)
    {
        if (value != 0)
        {
            Status = Status + Set<CombatStatusElement>.Of(CombatStatusElement.LogCombatActions);
        }
        else
        {
            Status = Status - Set<CombatStatusElement>.Of(CombatStatusElement.LogCombatActions);
        }
    }

    // Combat.pas:6112
    // { }
    public int GetRound() => CombatData.Round;

    // Combat.pas:6115
    // The tactical AI's damage estimates call these through an unchecked TFastCombat cast in the
    // original. They read only the combat data, so they live on TCombat for both combat kinds.
    // AIGroup.pas:7133
    public int GetRoundDistance()
    {
        int distance;
        distance = (CombatData.Round - 1) * 2;
        if (this.CombatData.Players.GetPlayers((sbyte)this.CombatData.CurrentPlayer).SideIndex == 0)
        {
            distance += 1;
        }

        return distance;
    }

    // AIGroup.pas:7180
    public bool GetWallInBetween(byte source, byte target)
    {
        bool result;
        if ((source & 15) <= 7 && (target & 15) <= 7 && CombatData.Wall != null && !CombatData.Wall.Status.Contains(CombatObjectStatusElement.Destroyed))
        {
            source = (byte)(int)(((uint)source) >> 4);
            target = (byte)(int)(((uint)target) >> 4);
            if (source < 7 && target < 7)
            {
                var party = CombatData.Parties.GetParty(source);
                var fPartiesParty = CombatData.Parties.GetParty(target);
                result = (party.Wall != 0) ^ (fPartiesParty.Wall != 0);
            }
            else
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

    // AIGroup.pas:7195
    public WallSituation GetWallSituation(CombatObject source, CombatObject target)
    {
        WallSituation result;
        if (GetWallInBetween(source.Position, target.Position))
        {
            if (source.GetSide() == CombatSideIndex.Defender)
            {
                result = WallSituation.Source;
            }
            else
            {
                result = WallSituation.Target;
            }
        }
        else
        {
            result = WallSituation.None;
        }

        return result;
    }

    // { }
    public void ExecuteCombatAction(CombatAction action)
    {
        if (Status.Contains(CombatStatusElement.LogCombatActions))
        {
            Logbook.Actions.AddChild(action);
        }

        action.Execute(this);
    }

    // Combat.pas:6121
    public bool PlayerTypeInvolved(Set<PlayerType> types)
    {
        bool result = false;
        for (int I = 0, last = CombatData.Parties.Count - 1; I <= last; I++)
        {
            if (CombatData.Parties.GetParty((byte)I) != null)
            {
                var party = CombatData.Parties.GetParty((byte)I);
                var players = CombatData.Players.GetPlayers((sbyte)party.PlayerIndex);
                if (types.Contains(AoWHSMap.Current.Players.GetPlayers(players.Player).PlayerType))
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
    // { }
    public void UpdateSidesSettings()
    {
        Sides[(int)CombatSideIndex.Attacker].UpdateSettings();
        Sides[(int)CombatSideIndex.Defender].UpdateSettings();
    }

    // Combat.pas:6190
    // { }
    public void NextTurn()
    {
        int I;
        if (CombatData.TurnOrder.Count != 0)
        {
            I = CombatData.TurnOrder.IndexOf(CombatData.CurrentPlayer) + 1;
            if (I >= CombatData.TurnOrder.Count)
            {
                I = 0;
            }

            NewTurn(CombatData.TurnOrder[I]);
        }
    }

    // Combat.pas:6201
    // { }
    public void Terminate()
    {
        if (!Terminated)
        {
            Terminated = true;
            if (OnTerminated != null)
            {
                OnTerminated(this);
            }
        }
    }

    // Combat.pas:6210
    // { }
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
    // { }
    public void SetLogbookHeader(string header)
    {
        UpdateHeaderCombatAction A;
        A = new UpdateHeaderCombatAction(null);
        A.Header = header;
        ExecuteCombatAction(A);
        A.Release();
    }

    // Combat.pas:6245
    // { }
    public void AddAdjacentArmies()
    {
        CombatData data;
        MapLevel level;
        HexNumberWalkData walk = default;
        ArmyHexSprite armyHexSprite;
        CombatSideIndex side;
        data = CombatData;
        level = AoWHSMap.Current.MapContainer.GetMapLevel(data.MapLevel);
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, data.X, data.Y, 0);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                armyHexSprite = (ArmyHexSprite)level.GetField(walk.X, walk.Y).FindOwnedChild(131607);
                if (armyHexSprite != null)
                {
                    if (walk.HexNumber != 0)
                    {
                        var players = data.Players.GetPlayers(data.DefenderPlayer);
                        var fPlayersPlayers = data.Players.GetPlayers(data.DefenderPlayer);
                        var players2 = data.Players.GetPlayers(data.AttackerPlayer);
                        side = CombatAbilityTables.GetCombatSide(data.Players.GetPlayers(data.AttackerPlayer).Player, (sbyte)players2.IndependentRelation, fPlayersPlayers.Player, (sbyte)players.IndependentRelation, armyHexSprite.Army.Player, armyHexSprite.Army.IndependentRelation);
                    }
                    else
                    {
                        side = CombatSideIndex.Defender;
                    }

                    if (side != CombatSideIndex.Neutral)
                    {
                        AddArmy(armyHexSprite.Army, side, walk.X, walk.Y, data.MapLevel);
                    }
                }
                else
                {
                    if (walk.HexNumber == 0)
                    {
                        var players = data.Players.GetPlayers(data.DefenderPlayer);
                        AddEmptyArmy((byte)data.Players.GetPlayers(data.DefenderPlayer).Player, players.IndependentRelation, walk.X, walk.Y, data.MapLevel);
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // Combat.pas:6280
    // { }
    public byte GetPartyPosition(int X, int Y, int layer)
    {
        byte result;
        CombatData data;
        data = CombatData;
        if (layer == data.MapLevel && HexGeometry.DeltaHexToRadius(data.X, data.Y, X, Y) <= 1)
        {
            result = (byte)HexGeometry.DeltaHexToHexNumberFast(data.X, data.Y, X, Y);
        }
        else
        {
            result = 7;
        }

        return result;
    }

    // Combat.pas:6292
    public byte AddCombatPlayer(byte player, byte independentRelation, CombatSideIndex side)
    {
        CombatPlayer playerLocal;
        playerLocal = CreateCombatPlayer();
        playerLocal.SetOwner(CombatData.Players);
        playerLocal.Player = (sbyte)player;
        playerLocal.IndependentRelation = independentRelation;
        playerLocal.SideIndex = (byte)(int)side;
        return (byte)(CombatData.Players.Count - 1);
    }

    // Combat.pas:6303
    public CombatParty AddArmy(Army army, CombatSideIndex side, int X, int Y, int L)
    {
        CombatParty party;
        bool valid;
        bool transport;
        byte player;
        byte position;
        int index;
        CombatUnit unitObject;
        MapFieldMessage message = default;
        CombatData data;
        Troop transporter;
        army.BeginUpdate();
        player = (byte)CombatData.Players.IndexOf((byte)army.Player, (byte)army.IndependentRelation);
        if (player == 255)
        {
            player = AddCombatPlayer((byte)army.Player, (byte)army.IndependentRelation, side);
        }

        position = GetPartyPosition(X, Y, L);
        party = CombatData.Parties.CreateParty(this, position, player, X, Y, L, army);
        transporter = army.Transporter(255);
        transport = transporter != null && transporter.ValidTerrain((sbyte)CombatData.TerrainAndOverlayTypes[0], (sbyte)CombatData.TerrainAndOverlayTypes[1]);
        index = 0;
        for (int I = army.Count - 1; I >= 0; I--)
        {
            valid = army.GetUnit(I).ValidTerrain((sbyte)CombatData.TerrainAndOverlayTypes[0], (sbyte)CombatData.TerrainAndOverlayTypes[1]);
            if (valid || transport)
            {
                unitObject = CreateCombatUnit();
                var troop = army.GetUnit(I);
                unitObject.Setup(CreateUniqueCombatObjectId(), (byte)(int)side, troop, (byte)((position << 4) + index), player);
                if (!valid)
                {
                    unitObject.Disabled = true;
                    unitObject.ControlMode = unitObject.ControlMode - Set<CombatObjectControlModeElement>.Of(CombatObjectControlModeElement.ConquerObject);
                }

                CombatData.AddObject(unitObject);
                index += 1;
            }
        }

        if (X != -1)
        {
            message.MessageId = 4387;
            var fieldValue = AoWHSMap.Current.MapContainer.GetField(X, Y, L);
            message.Param[0] = fieldValue;
            message.Result = 0;
            data = CombatData;
            AoWHSMap.Current.MapContainer.GetField(data.X, data.Y, data.MapLevel).SendMapFieldMessage(ref message);
            if (message.Result.Int != 0)
            {
                party.Wall = (byte)message.Result.Int;
            }
        }

        army.EndUpdate();
        return party;
    }

    // Combat.pas:6345
    public CombatParty AddArmy(Army army, CombatSideIndex side, int X, int Y, int L, byte position)
    {
        CombatParty party;
        byte player;
        int index;
        CombatUnit unitObject;
        army.BeginUpdate();
        player = (byte)CombatData.Players.IndexOf((byte)army.Player, (byte)army.IndependentRelation);
        if (player == 255)
        {
            player = AddCombatPlayer((byte)army.Player, (byte)army.IndependentRelation, side);
        }

        party = CombatData.Parties.CreateParty(this, (byte)(position << 0), player, X, Y, L, army);
        index = 0;
        for (int I = army.Count - 1; I >= 0; I--)
        {
            unitObject = CreateCombatUnit();
            var troop = army.GetUnit(I);
            unitObject.Setup(CreateUniqueCombatObjectId(), (byte)(int)side, troop, (byte)((position << 4) + index), player);
            CombatData.AddObject(unitObject);
            index += 1;
        }

        army.EndUpdate();
        return party;
    }

    // Combat.pas:6364
    public void AddEmptyArmy(byte player, byte independentRelation, int X, int Y, int L)
    {
        CombatData data;
        byte position;
        byte playerLocal;
        CombatSideIndex side;
        CombatParty party;
        MapFieldMessage message = default;
        data = CombatData;
        position = GetPartyPosition(X, Y, L);
        var players = data.Players.GetPlayers(data.DefenderPlayer);
        var fPlayersPlayers = data.Players.GetPlayers(data.DefenderPlayer);
        var players2 = data.Players.GetPlayers(data.AttackerPlayer);
        side = CombatAbilityTables.GetCombatSide(data.Players.GetPlayers(data.AttackerPlayer).Player, (sbyte)players2.IndependentRelation, fPlayersPlayers.Player, (sbyte)players.IndependentRelation, (sbyte)player, (sbyte)independentRelation);
        playerLocal = (byte)CombatData.Players.IndexOf(player, independentRelation);
        if (playerLocal == 255)
        {
            playerLocal = AddCombatPlayer(player, independentRelation, side);
        }

        party = CombatData.Parties.CreateParty(this, position, playerLocal, X, Y, L, null);
        message.MessageId = 4387;
        var fieldValue = AoWHSMap.Current.MapContainer.GetField(X, Y, L);
        message.Param[0] = fieldValue;
        message.Result = 0;
        AoWHSMap.Current.MapContainer.GetField(X, Y, L).SendMapFieldMessage(ref message);
        if (message.Result.Int != 0)
        {
            party.Wall = (byte)message.Result.Int;
        }
    }

    // Combat.pas:6385
    // { }
    public void InitializeDefaultArmies(Army army, int X, int Y, int L)
    {
        CombatData data;
        ArmyHexSprite armyHexSprite;
        data = CombatData;
        if (GetPartyPosition(X, Y, L) == 7)
        {
            ExplorationSite = 2;
            AddArmy(army, CombatSideIndex.Attacker, X, Y, L);
            armyHexSprite = (ArmyHexSprite)AoWHSMap.Current.MapContainer.GetField(data.X, data.Y, data.MapLevel).FindOwnedChild(131607);
            if (armyHexSprite != null)
            {
                AddArmy(armyHexSprite.Army, CombatSideIndex.Defender, data.X, data.Y, data.MapLevel);
            }
            else
            {
                var players = data.Players.GetPlayers(data.DefenderPlayer);
                AddEmptyArmy((byte)data.Players.GetPlayers(data.DefenderPlayer).Player, players.IndependentRelation, data.X, data.Y, data.MapLevel);
            }
        }
        else
        {
            AddAdjacentArmies();
        }
    }

    // Combat.pas:6568
    public void LockExecuted()
    {
        _executedLock += 1;
    }

    // Combat.pas:6571
    // { }
    public void UnlockExecuted()
    {
        _executedLock -= 1;
        if (_executedLock == 0 && OnExecuted != null)
        {
            OnExecuted(this);
        }
    }

    // Combat.pas:6642
    // { }
    public string GetCombatResultText()
    {
        var query = new CombatResultQuery();
        ref var result = ref query.Text;
        MapFieldMessage message = default;
        result = "";
        message.MessageId = 4361;
        message.Result = query;
        message.Param[0] = this;
        // { direct combat data }
        AoWHSMap.Current.MapContainer.GetField(CombatData.X, CombatData.Y, CombatData.MapLevel).SendMapFieldMessage(ref message);
        if (IsEmpty(result))
        {
            if (CombatResult == CombatResult.Victory)
            {
                result = AbilityRules.TranslateResourceString(s_battleWonText);
            }
            else
            {
                if (CombatResult == CombatResult.Defeat)
                {
                    result = AbilityRules.TranslateResourceString(s_battleLostText);
                }
                else
                {
                    if (CombatResult == CombatResult.CannotBreachWall)
                    {
                        result = AbilityRules.TranslateResourceString(GameText.CannotBreachWallText);
                    }
                    else
                    {
                        if (CombatResult == CombatResult.CannotInflictDamage)
                        {
                            result = AbilityRules.TranslateResourceString(GameText.CannotInflictDamageText);
                        }
                        else
                        {
                            result = AbilityRules.TranslateResourceString(s_battleDrawnText);
                        }
                    }
                }
            }
        }

        return result;
    }

    // Combat.pas:6659
    public bool DefendersAvailable()
    {
        int I;
        CombatObject objectValue;
        bool result = false;
        I = 0;
        while (!result && I != CombatData.GetCount())
        {
            objectValue = CombatData.GetObjects(I);
            result = objectValue.CombatPlayer != -1 && CombatData.Players.GetPlayers(objectValue.CombatPlayer).SideIndex == 1 && !objectValue.Status.Contains(CombatObjectStatusElement.Destroyed);
            I += 1;
        }

        return result;
    }

    private static readonly string s_battleWonText = "Battle Won";
    private static readonly string s_battleLostText = "Battle Lost";
    private static readonly string s_battleDrawnText = "Battle Drawn";

    public delegate void ExecutedEvent(Battle pCombat);

    // intf_P3_02.inc:318

    // intf_P3_02.inc:318

}
