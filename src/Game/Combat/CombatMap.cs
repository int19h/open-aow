// Translated from AoWTC.pas.
using Aow.Engine;
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Audio;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// FPlayers. FCombat. FUnitPositions. FSeatedCombatPlayer. FCurrentCombatPlayer. turn/player
// events. FMoveControl. FUnitSelection. selection/unit events. FAI. FEffects. FWalls.
// FFleeLocations. FEndTurnPending. FPlaySounds. FPositionStatus. FRound
public partial class CombatMap : CustomCombatMap
{
    // AoWTC.pas:2183
    public new const int ClassID = 2228480;

    public TacticalBattlePlayerList FPlayers;
    public TacticalBattle FCombat;
    public UnitPositionControl FUnitPositions;
    public sbyte FSeatedCombatPlayer;
    public sbyte FCurrentCombatPlayer;

    internal EventStore<sbyte> FChangeTurnEventsStore;

    public event NotifyEvent<sbyte> FChangeTurnEvents
    {
        add => FChangeTurnEventsStore.Add(value);
        remove => FChangeTurnEventsStore.Remove(value);
    }

    internal EventStore<CombatMap> FPlayerChangedEventsStore;

    public event NotifyEvent<CombatMap> FPlayerChangedEvents
    {
        add => FPlayerChangedEventsStore.Add(value);
        remove => FPlayerChangedEventsStore.Remove(value);
    }

    internal EventStore<sbyte> FSeatedPlayerChangedEventsStore;

    public event NotifyEvent<sbyte> FSeatedPlayerChangedEvents
    {
        add => FSeatedPlayerChangedEventsStore.Add(value);
        remove => FSeatedPlayerChangedEventsStore.Remove(value);
    }

    public CombatMoveControl FMoveControl;
    public CombatUnitSelectionControl FUnitSelection;

    internal EventStore<TacticalBattle> FSelectEventsStore;

    public event NotifyEvent<TacticalBattle> FSelectEvents
    {
        add => FSelectEventsStore.Add(value);
        remove => FSelectEventsStore.Remove(value);
    }

    internal EventStore<TacticalBattle> FDeselectEventsStore;

    public event NotifyEvent<TacticalBattle> FDeselectEvents
    {
        add => FDeselectEventsStore.Add(value);
        remove => FDeselectEventsStore.Remove(value);
    }

    public MapObject FCenterViewLocation;

    internal EventStore<HexSprite> FMouseOverTargetEventsStore;

    public event NotifyEvent<HexSprite> FMouseOverTargetEvents
    {
        add => FMouseOverTargetEventsStore.Add(value);
        remove => FMouseOverTargetEventsStore.Remove(value);
    }

    // CMapCtrl

    internal EventStore<TacticalBattleUnitHexSprite> FUnitChangedEventsStore;

    public event NotifyEvent<TacticalBattleUnitHexSprite> FUnitChangedEvents
    {
        add => FUnitChangedEventsStore.Add(value);
        remove => FUnitChangedEventsStore.Remove(value);
    }

    // CMapCtrl
    public BattleAI FAI;
    public List<BattleEffectSprite> FEffects;
    public List<World.Terrain.CityWall> FWalls;
    public List<FleeLocation> FFleeLocations;
    public bool FEndTurnPending;
    public bool FPlaySounds;
    public ByteArray8 FPositionStatus;
    public int FRound;

    // AoWTC.pas:2213
    public override void Dispose()
    {
        while (FEffects.Count > 0)
        {
            var effect = FEffects[0];
            DynamicFixedVisibleHS.RemoveChild((EngineObject)effect);
            var effect2 = FEffects[0];
            DynamicVisibleHS.RemoveChild((EngineObject)effect2);
            (FEffects[0] as IDisposable)?.Dispose();
            FEffects.RemoveAt(0);
        }

        base.Dispose();
        Current = null;
        FUnitPositions?.Dispose();
        FChangeTurnEventsStore.Clear();
        FPlayerChangedEventsStore.Clear();
        FSeatedPlayerChangedEventsStore.Clear();
        FSelectEventsStore.Clear();
        FDeselectEventsStore.Clear();
        FMouseOverTargetEventsStore.Clear();
        FUnitChangedEventsStore.Clear();
        FMoveControl?.Dispose();
        FUnitSelection?.Dispose();
        FEffects?.Clear();
        FWalls?.Clear();
        FFleeLocations?.Clear();
        FAI?.Dispose();
    }

    // AoWTC.pas:2189
    private protected sealed override void InitializeBeforeInherited() => Current = this;

    // AoWTC.pas:2186
    public CombatMap(ECustomNode owner) : base(owner)
    {
        FUnitPositions = new UnitPositionControl(null);
        FChangeTurnEventsStore = new();
        FPlayerChangedEventsStore = new();
        FSeatedPlayerChangedEventsStore = new();
        FMoveControl = new CombatMoveControl(this);
        FSelectEventsStore = new();
        FDeselectEventsStore = new();
        FMouseOverTargetEventsStore = new();
        FUnitChangedEventsStore = new();
        FPlayers = new TacticalBattlePlayerList();
        FUnitSelection = new CombatUnitSelectionControl();
        ((GameMapMouse)Mouse).FRectangleSelectEvents += FUnitSelection.RectangleSelectEvent;
        FEffects = new List<BattleEffectSprite>();
        FWalls = new List<World.Terrain.CityWall>();
        FFleeLocations = new List<FleeLocation>();
        FAI = new BattleAI();
        Mouse.SetCapture(this);
        Mouse.OnMove += MouseMoveEvent;
        Mouse.CursorMode = 256;
        for (int I = 0; I <= 7; I++)
        {
            FPositionStatus[I] = 2;
        }
    }

    // AoWTC.pas:2249
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        if (Msg.MsgID == 536870920 && !Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            SetCombat((TacticalBattle)AoWHSMap.Current.FCombat);
        }

        base.MsgProc(ref Msg, Sender);
    }

    // AoWTC.pas:2240
    public override HexSpriteMapMouse CreateMouse() => new CombatMapMouse();

    // AoWTC.pas:2267
    public override void InitializeNewMap()
    {
        HexControl Control;
        for (int X = 6, last = MapContainer.Width - 7; X <= last; X++)
        {
            for (int Y = 6, lastY = MapContainer.Height - 7; Y <= lastY; Y++)
            {
                AoWHSSet.Current.FCombatHexagons.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 1);
            }
        }

        Control = Engine.HSSet.HexagonControl;
        for (int X = 0, last = MapContainer.Width - 1; X <= last; X++)
        {
            for (int Y = 0; Y <= 5; Y++)
            {
                Control.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 15);
            }

            for (int Y = MapContainer.Height - 6, lastY = MapContainer.Height - 1; Y <= lastY; Y++)
            {
                Control.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 15);
            }
        }

        for (int Y = 6, lastY = MapContainer.Height - 7; Y <= lastY; Y++)
        {
            for (int X = 0; X <= 5; X++)
            {
                Control.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 15);
            }

            for (int X = MapContainer.Width - 6, last = MapContainer.Width - 1; X <= last; X++)
            {
                Control.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 15);
            }
        }
    }

    // AoWTC.pas:2338
    public override void DrawDynamicScene(ref LockedSurface SD)
    {
        base.DrawDynamicScene(ref SD);
        if (AoWHSMap.Current != null)
        {
            DrawFog(SD);
        }

        if (FShowGrid)
        {
            DrawGrid(SD);
        }

        DrawMoveRoute(SD);
        if (((GameMapMouse)Mouse).FSelectionRectangle.Left != -1)
        {
            DrawSelectionRect(ViewXhp, ViewYhp, ((GameMapMouse)Mouse).FSelectionRectangle, SD);
        }
    }

    // AoWTC.pas:2429
    public override void NewFrame()
    {
        int I;
        BattleEffectSprite Effect;
        if (!Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            I = 0;
            while (I < FEffects.Count)
            {
                Effect = FEffects[I];
                if (Effect.FDone)
                {
                    DynamicFixedVisibleHS.RemoveChild(Effect);
                    DynamicVisibleHS.RemoveChild(Effect);
                    Effect?.Dispose();
                    FEffects.RemoveAt(I);
                }
                else
                {
                    I += 1;
                }
            }
        }

        base.NewFrame();
        if (!Status.Contains(HexSpriteMapStatusElement.Editting) && FCombat != null)
        {
            UpdateCursor();
            if (FEndTurnPending && (AoWHSMap.Current.FPlayers.GetPlayers(FCurrentPlayer).FPlayerType != PlayerType.Human && AoWHSMap.Current.FPlayerCommunicator.PCC.GetPlayers(FCombat.FHostPlayer).ConnectionMode != ConnectionMode.Remote || FPlayers.GetPlayers(FCurrentCombatPlayer).FAuto && AoWHSMap.Current.FPlayerCommunicator.PCC.GetPlayers(FCurrentPlayer).ConnectionMode != ConnectionMode.Remote))
            {
                switch (FAI.Status)
                {
                    case BattleAI.AIStatus.Idle:
                        GameTables.AoWEngine.OutputDebugStr("AI for Player " + IntToStr(FCurrentPlayer) + " turn begins.");
                        FAI.BeginTurn();
                        break;
                    case BattleAI.AIStatus.Busy:
                        FAI.ContinueTurn();
                        break;
                    case BattleAI.AIStatus.Done:
                        if (EndTurn(true))
                        {
                            FAI.Status = BattleAI.AIStatus.Idle;
                            GameTables.AoWEngine.OutputDebugStr("AI for Player " + IntToStr(FCurrentPlayer) + " turn ends.");
                        }

                        break;
                }
            }
        }
    }

    // AoWTC.pas:2515
    public override void DropHeroItem(Hero Hero, Item Item)
    {
        sbyte X;
        sbyte Y;
        sbyte L;
        ItemHexSprite HS;
        int I;
        X = -1;
        Y = -1;
        L = -1;
        I = FCombat.FCombatData.GetCount();
        while (I != 0 && X == -1)
        {
            I -= 1;
            if (FCombat.FCombatData.GetObjects(I) is TacticalBattleUnit && ((CombatUnit)FCombat.FCombatData.GetObjects(I)).FUnit == Hero)
            {
                X = ((TacticalBattleUnit)FCombat.FCombatData.GetObjects(I)).FUnitHS.GetXhx();
                Y = ((TacticalBattleUnit)FCombat.FCombatData.GetObjects(I)).FUnitHS.GetYhx();
                L = 0;
            }
        }

        if (X > -1)
        {
            HS = (ItemHexSprite)MapContainer.GetField(X, Y, L).FindOwnedChild(131697);
            if (HS == null)
            {
                HS = new TacticalBattleItemHexSprite(null);
                // { Original bug: a failed placement leaves the new item sprite allocated. }
                if (Current.PlaceHX(HS, X, Y, L))
                {
                    Item.SetOwner(HS.FItems);
                }
            }
            else
            {
                Item.SetOwner(HS.FItems);
            }
        }
    }

    // AoWTC.pas:2544
    public override ItemList GetHeroItemsOnGround(Hero Hero)
    {
        ItemHexSprite HS;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        ItemList Result = null;
        GetUnitLocation(Hero, ref X, ref Y, ref L);
        if (X > -1)
        {
            HS = (ItemHexSprite)MapContainer.GetField(X, Y, L).FindOwnedChild(131697);
            if (HS != null)
            {
                Result = HS.FItems;
            }
        }

        return Result;
    }

    // AoWTC.pas:2243
    public void SetCombat(TacticalBattle Combat)
    {
        FCombat = Combat;
        if (FCombat != null)
        {
            FPlayers.FPlayers = FCombat.FCombatData.FPlayers;
        }
    }

    // AoWTC.pas:2256
    public void MouseMoveEvent(object Sender)
    {
        TacticalBattleUnitHexSprite HS;
        if (!Current.Status.Contains(HexSpriteMapStatusElement.Editting) && !FPlayers.GetPlayers(FCurrentCombatPlayer).FAuto && Mouse.Xhx != -1 && Mouse.Yhx != -1 && Mouse.Capture == null)
        {
            Mouse.CursorMode = 0;
            var yhx = Mouse.Yhx;
            var xhx = Mouse.Xhx;
            HS = (TacticalBattleUnitHexSprite)MapContainer.MapLevels[0].Field[xhx, yhx].FindOwnedChild(2228484);
            if (HS != null)
            {
                Mouse.CursorMode = 257;
            }
        }
    }

    // AoWTC.pas:2288
    public void SetSeatedCombatPlayer(sbyte Player)
    {
        FSeatedCombatPlayer = Player;
        if (Player == -1)
        {
            SetSeatedPlayer(-1);
        }
        else
        {
            SetSeatedPlayer(FCombat.FCombatData.FPlayers.GetPlayers(FSeatedCombatPlayer).FPlayer);
        }

        FSeatedPlayerChangedEventsStore.Raise(FCurrentCombatPlayer);
    }

    // AoWTC.pas:2296
    public void NewTurn(sbyte Player)
    {
        TacticalBattleUnit U;
        EngineMessage Msg = default;
        FCurrentCombatPlayer = Player;
        if (Player == 0)
        {
            FRound += 1;
        }

        SetCurrentPlayer(FPlayers.GetPlayers(FCurrentCombatPlayer).FPlayer);
        FCombat.NewTurn((byte)FCurrentCombatPlayer);
        FUnitSelection.Unselect();
        AoWHSMap.Current.FTokenManager.TokenExecuter.ActivateTC(FCurrentPlayer);
        if (FPlayers.GetPlayers(FCurrentCombatPlayer).FPlayer == 0)
        {
            var ind = AbilityRules.TranslateRStr(IndRStr);
            var beginRound = AbilityRules.TranslateRStr(BeginRoundRStr);
            ShowMessage(ind + " " + beginRound);
        }
        else
        {
            var player = FPlayers.GetPlayers(FCurrentCombatPlayer);
            var playerPlayerRace = AoWHSMap.Current.FPlayers.GetPlayers(player.FPlayer);
            var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[playerPlayerRace.FPlayerRace]);
            var beginRound = AbilityRules.TranslateRStr(BeginRoundRStr);
            ShowMessage(raceNamesPlural + " " + beginRound);
        }

        var playerPlayer = FPlayers.GetPlayers(FCurrentCombatPlayer);
        if (AoWHSMap.Current.FPlayers.GetPlayers(playerPlayer.FPlayer).FPlayerType == PlayerType.Human)
        {
            var player = FPlayers.GetPlayers(FCurrentCombatPlayer);
            if (AoWHSMap.Current.FPlayerCommunicator.PCC.GetPlayers(player.FPlayer).ConnectionMode != ConnectionMode.Remote)
            {
                SetSeatedCombatPlayer(FCurrentCombatPlayer);
                if (FPlayers.GetPlayers(FCurrentCombatPlayer).FLastX == 0)
                {
                    U = GetNextUnit(null, true, true);
                    if (U != null)
                    {
                        var xhx = U.FUnitHS.GetXhx();
                        FPlayers.GetPlayers(FCurrentCombatPlayer).FLastX = xhx;
                        var yhx = U.FUnitHS.GetYhx();
                        FPlayers.GetPlayers(FCurrentCombatPlayer).FLastY = yhx;
                        if (FExplorationEnabled)
                        {
                            for (int I = 0, last = FPlayers.GetCount() - 1; I <= last; I++)
                            {
                                if (FCurrentCombatPlayer != I)
                                {
                                    var fUnitHSXhx = U.FUnitHS.GetXhx();
                                    FPlayers.GetPlayers((sbyte)I).FLastX = fUnitHSXhx;
                                    var fUnitHSYhx = U.FUnitHS.GetYhx();
                                    FPlayers.GetPlayers((sbyte)I).FLastY = fUnitHSYhx;
                                }
                            }
                        }
                    }
                }

                var playerLastY = FPlayers.GetPlayers(FSeatedCombatPlayer);
                CenterViewTo(FPlayers.GetPlayers(FSeatedCombatPlayer).FLastX, playerLastY.FLastY, 0);
            }
        }

        AudioPlayback.PlaySample("Sfx\\Events\\NewEvent.wav");
        EngineMessage.InitMsg(ref Msg, 537006081, this);
        SendMsg(ref Msg, this);
        if (FExplorationEnabled)
        {
            var rect = new Rect(0, 0, 65, 58);
            TriggerMapChangedEvent(rect, 0);
        }

        FUnitSelection.Unselect();
        FChangeTurnEventsStore.Raise(FCurrentCombatPlayer);
    }

    // AoWTC.pas:2348
    public void DrawMoveRoute(in LockedSurface SD)
    {
        FUnitSelection.ShowMovePaths(SD);
    }

    // AoWTC.pas:2351
    public bool EndTurn(bool Automatic)
    {
        TacticalBattleEndTurnTokenEvent TE;
        bool Result = false;
        if (!Automatic)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(FCurrentPlayer).FPlayerType != PlayerType.Human)
            {
                return Result;
            }

            if (Current.FSeatedCombatPlayer != Current.FCurrentCombatPlayer)
            {
                return Result;
            }
        }

        if (AoWHSMap.Current.FTokenManager.PlayerReady(FCurrentPlayer))
        {
            TE = CreateTCEndTurnTE(FCurrentPlayer);
            if (TE != null)
            {
                AoWHSMap.Current.FTokenManager.GetTokenControl(FCurrentPlayer).AddEvent(TE);
                TE.Release();
                Result = true;
                FEndTurnPending = false;
            }
        }

        return Result;
    }

    // AoWTC.pas:2370
    public void CancelEndTurn()
    {
    }

    // AoWTC.pas:2373
    // { Intentionally empty in the original. }
    public bool GetEndTurnStatus()
    {
        bool Result = true;
        for (int I = 0, last = FPlayers.GetCount() - 1; I <= last; I++)
        {
            Result = Result && FPlayers.GetPlayers((sbyte)I).FEndTurn;
        }

        return Result;
    }

    // AoWTC.pas:2381
    public TacticalBattleUnit GetNextUnit(TacticalBattleUnit U, bool AllUnits, bool Forward)
    {
        int Index;
        int I;
        int Start;
        int Step;
        if (U != null && !U.GetEnabled())
        {
            U = null;
        }

        TacticalBattleUnit Result = U;
        if (Forward)
        {
            Step = 1;
        }
        else
        {
            Step = -1;
        }

        Index = 0;
        I = 0;
        Start = 0;
        if (U != null)
        {
            while (I < this.FCombat.FCombatData.GetCount())
            {
                if (this.FCombat.FCombatData.GetObjects(I) == U)
                {
                    Index = I + Step;
                    Start = I;
                    I = this.FCombat.FCombatData.GetCount();
                }

                I += 1;
            }
        }

        while (Index < this.FCombat.FCombatData.GetCount() && Index > -1)
        {
            if (this.FCombat.FCombatData.GetObjects(Index) != null && this.FCombat.FCombatData.GetObjects(Index) is CombatUnit && (AllUnits || ((TacticalBattleUnit)this.FCombat.FCombatData.GetObjects(Index)).FMovePoints > 0) && this.FCombat.FCombatData.GetObjects(Index).GetEnabled() && this.FCombat.FCombatData.GetObjects(Index).FCombatPlayer == FCurrentCombatPlayer)
            {
                Result = (TacticalBattleUnit)this.FCombat.FCombatData.GetObjects(Index);
                Index = -10;
            }

            Index = Index + Step;
        }

        if (Result == U)
        {
            if (Forward)
            {
                Index = 0;
            }
            else
            {
                Index = this.FCombat.FCombatData.GetCount() - 1;
            }

            while (Index != Start && Index > -1 && Index < this.FCombat.FCombatData.GetCount())
            {
                if (this.FCombat.FCombatData.GetObjects(Index) != null && this.FCombat.FCombatData.GetObjects(Index) is CombatUnit && (AllUnits || ((TacticalBattleUnit)this.FCombat.FCombatData.GetObjects(Index)).FMovePoints > 0) && this.FCombat.FCombatData.GetObjects(Index).GetEnabled() && this.FCombat.FCombatData.GetObjects(Index).FCombatPlayer == FCurrentCombatPlayer)
                {
                    Result = (TacticalBattleUnit)this.FCombat.FCombatData.GetObjects(Index);
                    Index = -10;
                }

                Index = Index + Step;
            }
        }

        return Result;
    }

    // AoWTC.pas:2466
    public BattleEffectSprite AddEffect(BattleEffectType EffectType)
    {
        BattleEffectSprite Result = new BattleEffectSprite(null);
        FEffects.Add(Result);
        Result.Activate();
        Result.FEffectType = EffectType;
        DynamicFixedVisibleHS.AddChild(Result);
        return Result;
    }

    // AoWTC.pas:2475
    public void UpdateCursor()
    {
        if (!((FSeatedPlayer == -1 || AoWHSMap.Current.FTokenManager.PlayerReady(FSeatedPlayer)) && FSeatedPlayer == FCurrentPlayer && !FPlayers.GetPlayers(FCurrentCombatPlayer).FAuto && Current.FSeatedCombatPlayer == Current.FCurrentCombatPlayer))
        {
            Mouse.Cursor = (int)(((uint)Frame) >> 2) % 4 + 6;
        }
        else
        {
            if (FUnitSelection.GetTCUnitHS() != null)
            {
                FUnitSelection.UpdateMoveCursor();
            }

            switch (Mouse.CursorMode)
            {
                case 0:
                    Mouse.Cursor = 0;
                    break;
                case 257:
                    Mouse.Cursor = (int)(((uint)Frame) >> 2) % 4 + 52;
                    break;
                case 258:
                    Mouse.Cursor = (((int)(((uint)Frame) >> 2)) & 1) + 49;
                    break;
            }
        }
    }

    // AoWTC.pas:2489
    public void EndCombat()
    {
        TerminateCombatTokenEvent TE;
        TE = new TerminateCombatTokenEvent(null);
        AoWHSMap.Current.FTokenManager.TokenExecuter.AddSystemEvent(TE);
        TE.Release();
    }

    // AoWTC.pas:2497
    public void SelectNextUnit(bool Forward)
    {
        TacticalBattleUnit U;
        int X = default;
        int Y = default;
        if (FSeatedCombatPlayer == FCurrentCombatPlayer)
        {
            U = GetNextUnit(FPlayers.GetPlayers(FSeatedCombatPlayer).FLastUnit, false, Forward);
            if (U != null)
            {
                if (FUnitSelection.GetTCUnitHS() != U.FUnitHS)
                {
                    FPlayers.GetPlayers(FSeatedCombatPlayer).FLastUnit = U;
                    var yhx = U.FUnitHS.GetYhx();
                    CenterViewTo(U.FUnitHS.GetXhx(), yhx, 0);
                    Selection.UnselectLocation();
                    var fUnitHSYhx = U.FUnitHS.GetYhx();
                    HexGeometry.HXtoHP(U.FUnitHS.GetXhx(), fUnitHSYhx, ref X, ref Y);
                    Selection.SelectLocation(X, Y, 0);
                }
            }
        }
    }

    // AoWTC.pas:2557
    // The combat object list holds the wall beside the units. The original casts every object to
    // TTacticalCombatUnit and reads FUnit. On a wall that offset is FWallType, FHitPoints and FLocked,
    // a small integer, so the unit comparison fails and the search continues. The port skips objects
    // that are not tactical units, which has the same result.
    public void GetUnitLocation(Troop U, ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        int I;
        X = -1;
        I = 0;
        while (I < FCombat.FCombatData.GetCount())
        {
            if (FCombat.FCombatData.GetObjects(I) is TacticalBattleUnit && ((CombatUnit)FCombat.FCombatData.GetObjects(I)).FUnit == U && FCombat.FCombatData.GetObjects(I).GetEnabled())
            {
                X = ((TacticalBattleUnit)FCombat.FCombatData.GetObjects(I)).FUnitHS.GetXhx();
                Y = ((TacticalBattleUnit)FCombat.FCombatData.GetObjects(I)).FUnitHS.GetYhx();
                L = 0;
                I = FCombat.FCombatData.GetCount();
            }
            else
            {
                I += 1;
            }
        }
    }

    // AoWTC.pas:2572
    public void Surrender(sbyte Mode)
    {
        TacticalBattleEndCombatTokenEvent TE;
        if (AoWHSMap.Current.FTokenManager.PlayerReady(Current.FCurrentPlayer) && FCurrentCombatPlayer == FSeatedCombatPlayer)
        {
            FUnitSelection.Unselect();
            TE = CreateTCEndCombatTE();
            if (TE != null)
            {
                AoWHSMap.Current.FTokenManager.GetTokenControl(FCurrentPlayer).AddEvent(TE);
                TE.Release();
            }
        }
    }

    private static readonly string IndRStr = "Independents";

    private static readonly string BeginRoundRStr = "begin their round.";

    private static TacticalBattleEndCombatTokenEvent CreateTCEndCombatTE() => new TacticalBattleEndCombatTokenEvent(null);

    private static TacticalBattleEndTurnTokenEvent CreateTCEndTurnTE(sbyte Player)
    {
        TacticalBattleEndTurnTokenEvent Result;
        if (Player != -1 && AoWHSMap.Current.FTokenManager.PlayerReady(Player))
        {
            Result = new TacticalBattleEndTurnTokenEvent(null);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    internal static void PlaceOverviewUnit(CombatMap Map, Troop U, int Position, int Index)
    {
        OverviewUnitHexSprite HS;
        sbyte Overlay;
        TacticalBattleUnitHexSprite Occupant;
        int X0;
        int Y0;
        int X = default;
        int Y = default;
        int I;
        MapCoordinate P;
        HS = new OverviewUnitHexSprite(null);
        HS.SetU(U);
        P = Map.FUnitPositions.GetUnitPosition(Position, Index);
        X0 = P.X;
        Y0 = P.Y;
        I = 0;
        do
        {
            HexGeometry.CenterHNtoHX(X0, Y0, I, ref X, ref Y);
            Overlay = Current.MapContainer.MapLevels[0].Field[X, Y].OverlayType;
            Occupant = (TacticalBattleUnitHexSprite)Current.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2228484);
            I += 1;
        }
        while (!(Overlay != 8 && Occupant == null));
        if (!Map.PlaceHX(HS, X, Y, 0))
        {
            HS?.Dispose();
        }
    }

    internal static CombatMap Current;

    // AoWTC.pas:484

    // AoWTC.pas:484

}
