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

// FCombatStatus. FCombatData. FNextCOID. FResult. FLogbook. FSides. FExecutedEvents.
// FExecutedLock. FOnExecuted. FTerminated. FOnTerminated. FVisibilityMode
public partial class Battle : EngineObject
{

    // Combat.pas:6067
    public Battle(ECustomNode owner) : base(owner)
    {
        FExecutedEventsStore = new();
        var combatSide = new CombatSide(null);
        FSides[(int)CombatSideIndex.Attacker] = combatSide;
        var combatSide2 = new CombatSide(null);
        FSides[(int)CombatSideIndex.Defender] = combatSide2;
        FSides[(int)CombatSideIndex.Attacker].FSideIndex = 0;
        FSides[(int)CombatSideIndex.Defender].FSideIndex = 1;
        FSides[(int)CombatSideIndex.Attacker].FOpponent = FSides[(int)CombatSideIndex.Defender];
        FSides[(int)CombatSideIndex.Defender].FOpponent = FSides[(int)CombatSideIndex.Attacker];
        FCombatData = new CombatData(null);
        FLogbook = new CombatLogbook(null);
        Clear();
    }

    public Set<CombatStatusElement> FStatus;
    public CombatData FCombatData;
    public int FNextCOID;
    public CombatResult FCombatResult;
    public CombatLogbook FLogbook;
    public CombatSideArray2 FSides;

    internal EventStore<Battle> FExecutedEventsStore;

    public event NotifyEvent<Battle> FExecutedEvents
    {
        add => FExecutedEventsStore.Add(value);
        remove => FExecutedEventsStore.Remove(value);
    }

    public int FExecutedLock;
    public ExecutedEvent FOnExecuted;
    public bool FTerminated;
    public NotifyEvent<Battle> FOnTerminated;
    public byte FExplorationSite;
    public EngineObject FItems;

    // item list or prisoners army
    // Combat.pas:6082
    public override void Dispose()
    {
        if (FStatus.Contains(CombatStatusElement.Activated))
        {
            Deactivate();
        }

        if (FStatus.Contains(CombatStatusElement.Executing))
        {
            Finalize();
        }

        FExecutedEventsStore.Clear();
        FSides[(int)CombatSideIndex.Attacker]?.Dispose();
        FSides[(int)CombatSideIndex.Defender]?.Dispose();
        FCombatData?.Dispose();
        FLogbook.Release();
        base.Dispose();
    }

    // Combat.pas:6103
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FSides[(int)CombatSideIndex.Attacker], 10);
        Stream.rwEObject(FSides[(int)CombatSideIndex.Defender], 11);
        Stream.rwCreateEObject(ref FLogbook, null, 12);
        Stream.rwEObject(FCombatData, 13);
    }

    // Combat.pas:6134
    public override void Clear()
    {
        base.Clear();
        FLogbook.Clear();
        FNextCOID = 1;
        FCombatData.Clear();
        FStatus = Set<CombatStatusElement>.Of();
    }

    // Combat.pas:6625
    public virtual void UpdateStatus()
    {
        int Attackers;
        int Defenders;
        Attackers = FSides[(int)CombatSideIndex.Attacker].GetUndestroyedConquerObjects();
        Defenders = FSides[(int)CombatSideIndex.Defender].GetUndestroyedConquerObjects();
        if (Attackers != 0 && Defenders == 0)
        {
            FCombatResult = CombatResult.Victory;
        }
        else
        {
            if (Attackers == 0 && Defenders != 0)
            {
                FCombatResult = CombatResult.Defeat;
            }
            else
            {
                if (Attackers == 0 && Defenders == 0)
                {
                    FCombatResult = CombatResult.BothDefeated;
                }
                else
                {
                    if (Attackers != 0 && Defenders != 0 && FCombatResult != CombatResult.Undecided)
                    {
                        FCombatResult = CombatResult.CannotInflictDamage;
                    }
                }
            }
        }

        switch ((int)FCombatResult)
        {
            case >= 4 and <= 5:
                {
                    for (int I = FCombatData.GetCount() - 1; I >= 0; I--)
                    {
                        if (FCombatData.GetObjects(I).FStatus.Contains(CombatObjectStatusElement.Fled))
                        {
                            FCombatResult = CombatResult.Fled;
                        }
                    }
                }

                break;
        }
    }

    // Combat.pas:6227
    public virtual void ObjectDestroyed(CombatObject CombatObject)
    {
        for (int I = FCombatData.GetCount() - 1; I >= 0; I--)
        {
            FCombatData.GetObjects(I).ObjectDestroyed(CombatObject);
        }

        FExecutedEventsStore.Raise(this);
        CheckTerminate();
    }

    // Combat.pas:6218
    public virtual void ObjectSideChanged(CombatObject CombatObject)
    {
        for (int I = FCombatData.GetCount() - 1; I >= 0; I--)
        {
            if (!CombatObject.FStatus.Contains(CombatObjectStatusElement.Destroyed))
            {
                FCombatData.GetObjects(I).ObjectSideChanged(CombatObject);
            }
        }

        CheckTerminate();
    }

    // Combat.pas:6163
    public virtual CombatUnit CreateCombatUnit()
    {
        CombatUnit Result = new FastBattleUnit(null);
        Result.FID = CreateUniqueCOID();
        return Result;
    }

    // Combat.pas:6289
    public virtual CombatPlayer CreateCombatPlayer() => new CombatPlayer(null);

    // Combat.pas:6143
    public virtual bool Setup(byte AttackerPlayer, byte AttackerRelation, int AttackerX, int AttackerY, int AttackerL, byte DefenderPlayer, byte DefenderRelation, int X, int Y, int L, bool LogActions)
    {
        CombatData Data;
        Clear();
        if (LogActions)
        {
            FStatus = FStatus + Set<CombatStatusElement>.Of(CombatStatusElement.LogCombatActions);
        }
        else
        {
            FStatus = FStatus - Set<CombatStatusElement>.Of(CombatStatusElement.LogCombatActions);
        }

        Data = FCombatData;
        Data.FX = X;
        Data.FY = Y;
        Data.FL = L;
        Data.FAttackerX = AttackerX;
        Data.FAttackerY = AttackerY;
        Data.FAttackerL = AttackerL;
        Data.FAttackerPlayer = (sbyte)AddCombatPlayer(AttackerPlayer, AttackerRelation, CombatSideIndex.Attacker);
        Data.FDefenderPlayer = (sbyte)AddCombatPlayer(DefenderPlayer, DefenderRelation, CombatSideIndex.Defender);
        var fieldValue = AoWHSMap.Current.MapContainer.GetField(Data.FX, Data.FY, Data.FL);
        Data.F_CombatData_20[0] = (byte)fieldValue.TerrainType;
        var mapContainerField = AoWHSMap.Current.MapContainer.GetField(Data.FX, Data.FY, Data.FL);
        Data.F_CombatData_20[1] = (byte)mapContainerField.OverlayType;
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
        CombatWall Wall;
        byte WallType;
        byte Player;
        LockExecuted();
        FStatus = FStatus + Set<CombatStatusElement>.Of(CombatStatusElement.Executing);
        WallType = 0;
        J = 0;
        while (WallType == 0 && J != FCombatData.FParties.Count)
        {
            if (FCombatData.FParties.GetParty((byte)J) != null)
            {
                WallType = FCombatData.FParties.GetParty((byte)J).FWall;
            }

            J += 1;
        }

        if (WallType != 0)
        {
            Wall = new CombatWall(null);
            Wall.Setup(CreateUniqueCOID(), WallType, (byte)FCombatData.FDefenderPlayer);
            FCombatData.AddObject(Wall);
        }

        if (FCombatData.FParties.GetParty(0) != null)
        {
            var party = FCombatData.FParties.GetParty(0);
            FCombatData.FWallType = party.FWall;
        }

        FCombatData.FTurnOrder.Add((byte)FCombatData.FDefenderPlayer);
        for (int I = 0, last = FCombatData.FPlayers.Count - 1; I <= last; I++)
        {
            Player = (byte)I;
            if (FCombatData.FTurnOrder.IndexOf(Player) == -1 && FCombatData.FPlayers.GetPlayers((sbyte)Player).FSideIndex == 1)
            {
                FCombatData.FTurnOrder.Add(Player);
            }
        }

        FCombatData.FTurnOrder.Add((byte)FCombatData.FAttackerPlayer);
        for (int I = 0, last = FCombatData.FPlayers.Count - 1; I <= last; I++)
        {
            Player = (byte)I;
            if (FCombatData.FTurnOrder.IndexOf(Player) == -1 && FCombatData.FPlayers.GetPlayers((sbyte)Player).FSideIndex == 0)
            {
                FCombatData.FTurnOrder.Add(Player);
            }
        }

        for (int I = FCombatData.GetCount() - 1; I >= 0; I--)
        {
            FCombatData.GetObjects(I).Initialize(this);
        }

        FLogbook.SetInitialData(FCombatData);
    }

    // Combat.pas:6446
    public virtual void Finalize()
    {
        int PlayerIndex;
        int ArmyIndex;
        int UnitIndex;
        Player Player;
        bool Alive;
        FinalizeMode Mode;
        CombatData Data;
        CombatData FinalData;
        MapFieldMessage Msg = default;
        Assert((FStatus & Set<CombatStatusElement>.Of(CombatStatusElement.Executing, CombatStatusElement.Activated)) == Set<CombatStatusElement>.Of(CombatStatusElement.Executing), "Combat Finalize Failure", "Combat.pas", 6523);
        for (int I = 0, last = FCombatData.FDefeatedLeaders.Count - 1; I <= last; I++)
        {
            Data = FCombatData;
            var item = Data.FDefeatedLeaders[I];
            var players = Data.FPlayers.GetPlayers((sbyte)item);
            AoWHSMap.Current.GameOver(players.FPlayer, GameOverStatus.Lost);
            FLogbook.F_CombatLogbook_19 = true;
        }

        FLogbook.SetFinalData(FCombatData);
        FStatus = FStatus - Set<CombatStatusElement>.Of(CombatStatusElement.Executing);
        Data = FCombatData;
        for (int I = Data.GetCount() - 1; I >= 0; I--)
        {
            FCombatData.GetObjects(I).CombatDone();
        }

        for (int modeOrdinal = (int)FinalizeMode.Original; modeOrdinal <= (int)FinalizeMode.All; modeOrdinal++)
        {
            Mode = (FinalizeMode)modeOrdinal;
            for (int I = Data.GetCount() - 1; I >= 0; I--)
            {
                FCombatData.GetObjects(I).Finalize(Mode);
            }
        }

        var combatResultText = GetCombatResultText();
        FLogbook.FFinalData.FHeader = combatResultText;
        FLogbook.FFinalData.F_CombatData_30 = (byte)(int)FCombatResult;
        PlayerIndex = FCombatData.FPlayers.Count;
        while (PlayerIndex != 0 && !FLogbook.F_CombatLogbook_19)
        {
            PlayerIndex -= 1;
            var players = FCombatData.FPlayers.GetPlayers((sbyte)PlayerIndex);
            Player = AoWHSMap.Current.FPlayers.GetPlayers(players.FPlayer);
            ArmyIndex = Player.FArmies.Count;
            Alive = false;
            while (ArmyIndex != 0 && !Alive)
            {
                ArmyIndex -= 1;
                UnitIndex = Player.FArmies.GetArmy(ArmyIndex).Count;
                while (!Alive && UnitIndex != 0)
                {
                    UnitIndex -= 1;
                    Alive = Player.FArmies.GetArmy(ArmyIndex).GetUnit(UnitIndex).GetHitPoints() != 0;
                }
            }

            FLogbook.F_CombatLogbook_19 = !Alive;
        }

        FinalData = FCombatData;
        if (FinalData.FX != -1)
        {
            Msg.MsgID = 4368;
            Msg.Param[0] = this;
            Msg.Result = 0;
            AoWHSMap.Current.MapContainer.GetField(FinalData.FX, FinalData.FY, FinalData.FL).SendMapFieldMsg(ref Msg);
        }

        UnlockExecuted();
    }

    // Combat.pas:6577
    public virtual void Activate()
    {
        Assert(!FStatus.Contains(CombatStatusElement.Activated), "Combat already activated", "Combat.pas", 6595);
        FStatus = FStatus + Set<CombatStatusElement>.Of(CombatStatusElement.Activated);
        FCombatData.FParties.Activate(this);
        for (int I = FCombatData.GetCount() - 1; I >= 0; I--)
        {
            FCombatData.GetObjects(I).Activate(this);
        }

        if (FStatus.Contains(CombatStatusElement.Executing))
        {
            for (int I = FCombatData.GetCount() - 1; I >= 0; I--)
            {
                FCombatData.GetObjects(I).SetupTargets();
            }

            UpdateSidesSettings();
        }
    }

    // Combat.pas:6608
    public virtual void Deactivate()
    {
        Assert(FStatus.Contains(CombatStatusElement.Activated), "Combat Failure during Deactivating", "Combat.pas", 6612);
        FStatus = FStatus - Set<CombatStatusElement>.Of(CombatStatusElement.Activated);
        for (int I = FCombatData.GetCount() - 1; I >= 0; I--)
        {
            FCombatData.GetObjects(I).Deactivate();
        }

        FCombatData.FParties.Deactivate(this);
    }

    // Combat.pas:6180
    public virtual void NewTurn(byte CombatPlayer)
    {
        NewCombatTurnCombatAction A;
        A = new NewCombatTurnCombatAction(null);
        A.F_NewCombatTurnCA_0C = CombatPlayer;
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
    public bool GetLogCombatActions() => FStatus.Contains(CombatStatusElement.LogCombatActions);

    // Combat.pas:6097
    // { }
    public void SetLogCombatActions(byte Value)
    {
        if (Value != 0)
        {
            FStatus = FStatus + Set<CombatStatusElement>.Of(CombatStatusElement.LogCombatActions);
        }
        else
        {
            FStatus = FStatus - Set<CombatStatusElement>.Of(CombatStatusElement.LogCombatActions);
        }
    }

    // Combat.pas:6112
    // { }
    public int GetRound() => FCombatData.FRound;

    // Combat.pas:6115
    // The tactical AI's damage estimates call these through an unchecked TFastCombat cast in the
    // original. They read only the combat data, so they live on TCombat for both combat kinds.
    // AIGroup.pas:7133
    public int GetRoundDistance()
    {
        int Distance;
        Distance = (FCombatData.FRound - 1) * 2;
        if (this.FCombatData.FPlayers.GetPlayers((sbyte)this.FCombatData.FCurrentPlayer).FSideIndex == 0)
        {
            Distance += 1;
        }

        return Distance;
    }

    // AIGroup.pas:7180
    public bool GetWallInBetween(byte Source, byte Target)
    {
        bool Result;
        if ((Source & 15) <= 7 && (Target & 15) <= 7 && FCombatData.FWall != null && !FCombatData.FWall.FStatus.Contains(CombatObjectStatusElement.Destroyed))
        {
            Source = (byte)(int)(((uint)Source) >> 4);
            Target = (byte)(int)(((uint)Target) >> 4);
            if (Source < 7 && Target < 7)
            {
                var party = FCombatData.FParties.GetParty(Source);
                var fPartiesParty = FCombatData.FParties.GetParty(Target);
                Result = (party.FWall != 0) ^ (fPartiesParty.FWall != 0);
            }
            else
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

    // AIGroup.pas:7195
    public WallSituation GetWallSituation(CombatObject Source, CombatObject Target)
    {
        WallSituation Result;
        if (GetWallInBetween(Source.FPosition, Target.FPosition))
        {
            if (Source.GetSide() == CombatSideIndex.Defender)
            {
                Result = WallSituation.Source;
            }
            else
            {
                Result = WallSituation.Target;
            }
        }
        else
        {
            Result = WallSituation.None;
        }

        return Result;
    }

    // { }
    public void ExecuteCombatAction(CombatAction Action)
    {
        if (FStatus.Contains(CombatStatusElement.LogCombatActions))
        {
            FLogbook.FActions.AddChild(Action);
        }

        Action.Execute(this);
    }

    // Combat.pas:6121
    public bool PlayerTypeInvolved(Set<PlayerType> Types)
    {
        bool Result = false;
        for (int I = 0, last = FCombatData.FParties.Count - 1; I <= last; I++)
        {
            if (FCombatData.FParties.GetParty((byte)I) != null)
            {
                var party = FCombatData.FParties.GetParty((byte)I);
                var players = FCombatData.FPlayers.GetPlayers((sbyte)party.FPlayerIndex);
                if (Types.Contains(AoWHSMap.Current.FPlayers.GetPlayers(players.FPlayer).FPlayerType))
                {
                    Result = true;
                }
            }
        }

        return Result;
    }

    // Combat.pas:6131
    public byte CreateUniqueCOID()
    {
        byte Result = (byte)FNextCOID;
        FNextCOID += 1;
        return Result;
    }

    // Combat.pas:6177
    // { }
    public void UpdateSidesSettings()
    {
        FSides[(int)CombatSideIndex.Attacker].UpdateSettings();
        FSides[(int)CombatSideIndex.Defender].UpdateSettings();
    }

    // Combat.pas:6190
    // { }
    public void NextTurn()
    {
        int I;
        if (FCombatData.FTurnOrder.Count != 0)
        {
            I = FCombatData.FTurnOrder.IndexOf(FCombatData.FCurrentPlayer) + 1;
            if (I >= FCombatData.FTurnOrder.Count)
            {
                I = 0;
            }

            NewTurn(FCombatData.FTurnOrder[I]);
        }
    }

    // Combat.pas:6201
    // { }
    public void Terminate()
    {
        if (!FTerminated)
        {
            FTerminated = true;
            if (FOnTerminated != null)
            {
                FOnTerminated(this);
            }
        }
    }

    // Combat.pas:6210
    // { }
    public void CheckTerminate()
    {
        if (!FTerminated)
        {
            if (FSides[(int)CombatSideIndex.Attacker].GetUndestroyedConquerObjects() == 0 || FSides[(int)CombatSideIndex.Defender].GetUndestroyedConquerObjects() == 0 || FCombatData.FDefeatedLeaders.Count != 0)
            {
                Terminate();
            }
        }
    }

    // Combat.pas:6236
    // { }
    public void SetLogbookHeader(string Header)
    {
        UpdateHeaderCombatAction A;
        A = new UpdateHeaderCombatAction(null);
        A.F_UpdateHeaderCA_0C = Header;
        ExecuteCombatAction(A);
        A.Release();
    }

    // Combat.pas:6245
    // { }
    public void AddAdjacentArmies()
    {
        CombatData Data;
        MapLevel Level;
        HexNumberWalkData Walk = default;
        ArmyHexSprite ArmyHS;
        CombatSideIndex Side;
        Data = FCombatData;
        Level = AoWHSMap.Current.MapContainer.GetMapLevel(Data.FL);
        HexNumberWalkData.InitHNWalk(ref Walk, Data.FX, Data.FY, 0);
        while (Walk.Radius <= 1)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                ArmyHS = (ArmyHexSprite)Level.GetField(Walk.X, Walk.Y).FindOwnedChild(131607);
                if (ArmyHS != null)
                {
                    if (Walk.HN != 0)
                    {
                        var players = Data.FPlayers.GetPlayers(Data.FDefenderPlayer);
                        var fPlayersPlayers = Data.FPlayers.GetPlayers(Data.FDefenderPlayer);
                        var players2 = Data.FPlayers.GetPlayers(Data.FAttackerPlayer);
                        Side = CombatAbilityTables.GetCombatSide(Data.FPlayers.GetPlayers(Data.FAttackerPlayer).FPlayer, (sbyte)players2.FIndependentRelation, fPlayersPlayers.FPlayer, (sbyte)players.FIndependentRelation, ArmyHS.FArmy.FPlayer, ArmyHS.FArmy.FIndependentRelation);
                    }
                    else
                    {
                        Side = CombatSideIndex.Defender;
                    }

                    if (Side != CombatSideIndex.Neutral)
                    {
                        AddArmy(ArmyHS.FArmy, Side, Walk.X, Walk.Y, Data.FL);
                    }
                }
                else
                {
                    if (Walk.HN == 0)
                    {
                        var players = Data.FPlayers.GetPlayers(Data.FDefenderPlayer);
                        AddEmptyArmy((byte)Data.FPlayers.GetPlayers(Data.FDefenderPlayer).FPlayer, players.FIndependentRelation, Walk.X, Walk.Y, Data.FL);
                    }
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }
    }

    // Combat.pas:6280
    // { }
    public byte GetPartyPosition(int X, int Y, int Layer)
    {
        byte Result;
        CombatData Data;
        Data = FCombatData;
        if (Layer == Data.FL && HexGeometry.dHXtoRad(Data.FX, Data.FY, X, Y) <= 1)
        {
            Result = (byte)HexGeometry.dHXtoHNfast(Data.FX, Data.FY, X, Y);
        }
        else
        {
            Result = 7;
        }

        return Result;
    }

    // Combat.pas:6292
    public byte AddCombatPlayer(byte APlayer, byte IndependentRelation, CombatSideIndex Side)
    {
        CombatPlayer Player;
        Player = CreateCombatPlayer();
        Player.SetOwner(FCombatData.FPlayers);
        Player.FPlayer = (sbyte)APlayer;
        Player.FIndependentRelation = IndependentRelation;
        Player.FSideIndex = (byte)(int)Side;
        return (byte)(FCombatData.FPlayers.Count - 1);
    }

    // Combat.pas:6303
    public CombatParty AddArmy(Army Army, CombatSideIndex Side, int X, int Y, int L)
    {
        CombatParty Party;
        bool Valid;
        bool Transport;
        byte Player;
        byte Position;
        int Index;
        CombatUnit UnitObj;
        MapFieldMessage Msg = default;
        CombatData Data;
        Troop Transporter;
        Army.BeginUpdate();
        Player = (byte)FCombatData.FPlayers.IndexOf((byte)Army.FPlayer, (byte)Army.FIndependentRelation);
        if (Player == 255)
        {
            Player = AddCombatPlayer((byte)Army.FPlayer, (byte)Army.FIndependentRelation, Side);
        }

        Position = GetPartyPosition(X, Y, L);
        Party = FCombatData.FParties.CreateParty(this, Position, Player, X, Y, L, Army);
        Transporter = Army.Transporter(255);
        Transport = Transporter != null && Transporter.ValidTerrain((sbyte)FCombatData.F_CombatData_20[0], (sbyte)FCombatData.F_CombatData_20[1]);
        Index = 0;
        for (int I = Army.Count - 1; I >= 0; I--)
        {
            Valid = Army.GetUnit(I).ValidTerrain((sbyte)FCombatData.F_CombatData_20[0], (sbyte)FCombatData.F_CombatData_20[1]);
            if (Valid || Transport)
            {
                UnitObj = CreateCombatUnit();
                var unit = Army.GetUnit(I);
                UnitObj.Setup(CreateUniqueCOID(), (byte)(int)Side, unit, (byte)((Position << 4) + Index), Player);
                if (!Valid)
                {
                    UnitObj.FDisabled = true;
                    UnitObj.FControlMode = UnitObj.FControlMode - Set<CombatObjectControlModeElement>.Of(CombatObjectControlModeElement.ConquerObject);
                }

                FCombatData.AddObject(UnitObj);
                Index += 1;
            }
        }

        if (X != -1)
        {
            Msg.MsgID = 4387;
            var fieldValue = AoWHSMap.Current.MapContainer.GetField(X, Y, L);
            Msg.Param[0] = fieldValue;
            Msg.Result = 0;
            Data = FCombatData;
            AoWHSMap.Current.MapContainer.GetField(Data.FX, Data.FY, Data.FL).SendMapFieldMsg(ref Msg);
            if (Msg.Result.Int != 0)
            {
                Party.FWall = (byte)Msg.Result.Int;
            }
        }

        Army.EndUpdate();
        return Party;
    }

    // Combat.pas:6345
    public CombatParty AddArmyEx(Army Army, CombatSideIndex Side, int X, int Y, int L, byte Position)
    {
        CombatParty Party;
        byte Player;
        int Index;
        CombatUnit UnitObj;
        Army.BeginUpdate();
        Player = (byte)FCombatData.FPlayers.IndexOf((byte)Army.FPlayer, (byte)Army.FIndependentRelation);
        if (Player == 255)
        {
            Player = AddCombatPlayer((byte)Army.FPlayer, (byte)Army.FIndependentRelation, Side);
        }

        Party = FCombatData.FParties.CreateParty(this, (byte)(Position << 0), Player, X, Y, L, Army);
        Index = 0;
        for (int I = Army.Count - 1; I >= 0; I--)
        {
            UnitObj = CreateCombatUnit();
            var unit = Army.GetUnit(I);
            UnitObj.Setup(CreateUniqueCOID(), (byte)(int)Side, unit, (byte)((Position << 4) + Index), Player);
            FCombatData.AddObject(UnitObj);
            Index += 1;
        }

        Army.EndUpdate();
        return Party;
    }

    // Combat.pas:6364
    public void AddEmptyArmy(byte APlayer, byte IndependentRelation, int X, int Y, int L)
    {
        CombatData Data;
        byte Position;
        byte Player;
        CombatSideIndex Side;
        CombatParty Party;
        MapFieldMessage Msg = default;
        Data = FCombatData;
        Position = GetPartyPosition(X, Y, L);
        var players = Data.FPlayers.GetPlayers(Data.FDefenderPlayer);
        var fPlayersPlayers = Data.FPlayers.GetPlayers(Data.FDefenderPlayer);
        var players2 = Data.FPlayers.GetPlayers(Data.FAttackerPlayer);
        Side = CombatAbilityTables.GetCombatSide(Data.FPlayers.GetPlayers(Data.FAttackerPlayer).FPlayer, (sbyte)players2.FIndependentRelation, fPlayersPlayers.FPlayer, (sbyte)players.FIndependentRelation, (sbyte)APlayer, (sbyte)IndependentRelation);
        Player = (byte)FCombatData.FPlayers.IndexOf(APlayer, IndependentRelation);
        if (Player == 255)
        {
            Player = AddCombatPlayer(APlayer, IndependentRelation, Side);
        }

        Party = FCombatData.FParties.CreateParty(this, Position, Player, X, Y, L, null);
        Msg.MsgID = 4387;
        var fieldValue = AoWHSMap.Current.MapContainer.GetField(X, Y, L);
        Msg.Param[0] = fieldValue;
        Msg.Result = 0;
        AoWHSMap.Current.MapContainer.GetField(X, Y, L).SendMapFieldMsg(ref Msg);
        if (Msg.Result.Int != 0)
        {
            Party.FWall = (byte)Msg.Result.Int;
        }
    }

    // Combat.pas:6385
    // { }
    public void InitDefaultArmies(Army Army, int X, int Y, int L)
    {
        CombatData Data;
        ArmyHexSprite ArmyHS;
        Data = FCombatData;
        if (GetPartyPosition(X, Y, L) == 7)
        {
            FExplorationSite = 2;
            AddArmy(Army, CombatSideIndex.Attacker, X, Y, L);
            ArmyHS = (ArmyHexSprite)AoWHSMap.Current.MapContainer.GetField(Data.FX, Data.FY, Data.FL).FindOwnedChild(131607);
            if (ArmyHS != null)
            {
                AddArmy(ArmyHS.FArmy, CombatSideIndex.Defender, Data.FX, Data.FY, Data.FL);
            }
            else
            {
                var players = Data.FPlayers.GetPlayers(Data.FDefenderPlayer);
                AddEmptyArmy((byte)Data.FPlayers.GetPlayers(Data.FDefenderPlayer).FPlayer, players.FIndependentRelation, Data.FX, Data.FY, Data.FL);
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
        FExecutedLock += 1;
    }

    // Combat.pas:6571
    // { }
    public void UnlockExecuted()
    {
        FExecutedLock -= 1;
        if (FExecutedLock == 0 && FOnExecuted != null)
        {
            FOnExecuted(this);
        }
    }

    // Combat.pas:6642
    // { }
    public string GetCombatResultText()
    {
        var query = new CombatResultQuery();
        ref var Result = ref query.Text;
        MapFieldMessage Msg = default;
        Result = "";
        Msg.MsgID = 4361;
        Msg.Result = query;
        Msg.Param[0] = this;
        // { direct combat data }
        AoWHSMap.Current.MapContainer.GetField(FCombatData.FX, FCombatData.FY, FCombatData.FL).SendMapFieldMsg(ref Msg);
        if (IsEmpty(Result))
        {
            if (FCombatResult == CombatResult.Victory)
            {
                Result = AbilityRules.TranslateRStr(BattleWonRStr);
            }
            else
            {
                if (FCombatResult == CombatResult.Defeat)
                {
                    Result = AbilityRules.TranslateRStr(BattleLostRStr);
                }
                else
                {
                    if (FCombatResult == CombatResult.CannotBreachWall)
                    {
                        Result = AbilityRules.TranslateRStr(GameText.CannotBreachWallRStr);
                    }
                    else
                    {
                        if (FCombatResult == CombatResult.CannotInflictDamage)
                        {
                            Result = AbilityRules.TranslateRStr(GameText.CannotInflictDamageRStr);
                        }
                        else
                        {
                            Result = AbilityRules.TranslateRStr(BattleDrawnRStr);
                        }
                    }
                }
            }
        }

        return Result;
    }

    // Combat.pas:6659
    public bool DefendersAvailable()
    {
        int I;
        CombatObject Obj;
        bool Result = false;
        I = 0;
        while (!Result && I != FCombatData.GetCount())
        {
            Obj = FCombatData.GetObjects(I);
            Result = Obj.FCombatPlayer != -1 && FCombatData.FPlayers.GetPlayers(Obj.FCombatPlayer).FSideIndex == 1 && !Obj.FStatus.Contains(CombatObjectStatusElement.Destroyed);
            I += 1;
        }

        return Result;
    }

    private static readonly string BattleWonRStr = "Battle Won";
    private static readonly string BattleLostRStr = "Battle Lost";
    private static readonly string BattleDrawnRStr = "Battle Drawn";

    public delegate void ExecutedEvent(Battle pCombat);

    // intf_P3_02.inc:318

    // intf_P3_02.inc:318

}
