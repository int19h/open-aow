#nullable disable
// Translated from AoWE.pas and AoWTC.pas.
using Aow.Engine;
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

// The combat event lists (TAbstractAoWCombatMap). FPlayers. FCombat. FUnitPositions.
// FSeatedCombatPlayer. FCurrentCombatPlayer. turn/player events. MoveControl. UnitSelection.
// selection/unit events. AI. Effects. Walls. FleeLocations. EndTurnPending. PlaySounds.
// PositionStatus. Round
public partial class CombatMap : GameMap
{
    internal EventStore<CombatSpellCaster> CombatStartedEventsStore { get; }

    public event NotifyEvent<CombatSpellCaster> CombatStarted
    {
        add => CombatStartedEventsStore.Add(value);
        remove => CombatStartedEventsStore.Remove(value);
    }

    internal EventStore<AbilityActivation> AbilityActivatedEventsStore { get; }

    public event NotifyEvent<AbilityActivation> AbilityActivated
    {
        add => AbilityActivatedEventsStore.Add(value);
        remove => AbilityActivatedEventsStore.Remove(value);
    }

    // AoWTC.pas:2183
    public new const int ClassId = 2228480;

    public TacticalBattlePlayerList Players { get; }
    public TacticalBattle Combat { get; private set; }
    public UnitPositionControl UnitPositions { get; }
    public sbyte SeatedCombatPlayer { get; private set; }
    public sbyte CurrentCombatPlayer { get; private set; }

    internal EventStore<sbyte> ChangeTurnEventsStore { get; }

    public event NotifyEvent<sbyte> TurnChanged
    {
        add => ChangeTurnEventsStore.Add(value);
        remove => ChangeTurnEventsStore.Remove(value);
    }

    private readonly EventStore<CombatMap> _playerChangedEventsStore;

    public event NotifyEvent<CombatMap> PlayerChanged
    {
        add => _playerChangedEventsStore.Add(value);
        remove => _playerChangedEventsStore.Remove(value);
    }

    private readonly EventStore<sbyte> _seatedPlayerChangedEventsStore;

    public event NotifyEvent<sbyte> SeatedPlayerChanged
    {
        add => _seatedPlayerChangedEventsStore.Add(value);
        remove => _seatedPlayerChangedEventsStore.Remove(value);
    }

    public CombatMoveControl MoveControl { get; }
    public CombatUnitSelectionControl UnitSelection { get; }

    internal EventStore<TacticalBattle> SelectEventsStore { get; }

    public event NotifyEvent<TacticalBattle> Selected
    {
        add => SelectEventsStore.Add(value);
        remove => SelectEventsStore.Remove(value);
    }

    internal EventStore<TacticalBattle> DeselectEventsStore { get; }

    public event NotifyEvent<TacticalBattle> Deselected
    {
        add => DeselectEventsStore.Add(value);
        remove => DeselectEventsStore.Remove(value);
    }

    public MapObject CenterViewLocation { get; set; }

    internal EventStore<HexSprite> MouseOverTargetEventsStore { get; }

    public event NotifyEvent<HexSprite> MouseOverTarget
    {
        add => MouseOverTargetEventsStore.Add(value);
        remove => MouseOverTargetEventsStore.Remove(value);
    }

    // CMapCtrl

    internal EventStore<TacticalBattleUnitHexSprite> UnitChangedEventsStore { get; }

    public event NotifyEvent<TacticalBattleUnitHexSprite> UnitChanged
    {
        add => UnitChangedEventsStore.Add(value);
        remove => UnitChangedEventsStore.Remove(value);
    }

    // CMapCtrl
    public BattleAI AI { get; }
    public List<BattleEffectSprite> Effects { get; }
    public List<World.Terrain.CityWall> Walls { get; }
    public List<FleeLocation> FleeLocations { get; }
    public bool EndTurnPending { get; set; }
    public bool PlaySounds { get; set; }

    public byte[] PositionStatus { get; } = new byte[8];

    public int Round { get; private set; }

    // AoWTC.pas:2213 around AoWHSMap.PAS:914.
    public override void Dispose()
    {
        while (Effects.Count > 0)
        {
            BattleEffectSprite effect = Effects[0];
            DynamicFixedVisibleHexSprite.RemoveChild(effect);
            BattleEffectSprite effect2 = Effects[0];
            DynamicVisibleHexSprite.RemoveChild(effect2);
            (Effects[0] as IDisposable)?.Dispose();
            Effects.RemoveAt(0);
        }

        base.Dispose();
        CombatStartedEventsStore.Clear();
        AbilityActivatedEventsStore.Clear();

        Current = null;
        UnitPositions?.Dispose();
        ChangeTurnEventsStore.Clear();
        _playerChangedEventsStore.Clear();
        _seatedPlayerChangedEventsStore.Clear();
        SelectEventsStore.Clear();
        DeselectEventsStore.Clear();
        MouseOverTargetEventsStore.Clear();
        UnitChangedEventsStore.Clear();
        MoveControl?.Dispose();
        UnitSelection?.Dispose();
        Effects?.Clear();
        Walls?.Clear();
        FleeLocations?.Clear();
        AI?.Dispose();
    }

    // AoWTC.pas:2189
    private protected sealed override void InitializeBeforeInherited() => Current = this;

    // AoWHSMap.PAS:907, then AoWTC.pas:2186.
    public CombatMap(CustomNode owner) : base(owner)
    {
        CombatStartedEventsStore = new();
        AbilityActivatedEventsStore = new();

        UnitPositions = new UnitPositionControl(null) { Holder = CreationHolder() };
        ChangeTurnEventsStore = new();
        _playerChangedEventsStore = new();
        _seatedPlayerChangedEventsStore = new();
        MoveControl = new CombatMoveControl(this);
        SelectEventsStore = new();
        DeselectEventsStore = new();
        MouseOverTargetEventsStore = new();
        UnitChangedEventsStore = new();
        Players = new TacticalBattlePlayerList();
        UnitSelection = new CombatUnitSelectionControl(this);
        ((GameMapMouse)Mouse).RectangleSelected += UnitSelection.RectangleSelectEvent;
        Effects = new List<BattleEffectSprite>();
        Walls = new List<World.Terrain.CityWall>();
        FleeLocations = new List<FleeLocation>();
        AI = new BattleAI(this);
        Mouse.SetCapture(this);
        Mouse.OnMove += MouseMoveEvent;
        Mouse.CursorMode = 256;
        for (int i = 0; i <= 7; i++)
        {
            PositionStatus[i] = 2;
        }
    }

    // AoWTC.pas:2249
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        if (message.MessageId == 536870920 && !Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            SetCombat((TacticalBattle)StrategicMap.Current.Combat);
        }

        base.MessageProc(ref message, sender);
    }

    // AoWTC.pas:2240
    public override HexSpriteMapMouse CreateMouse() => new CombatMapMouse();

    // AoWTC.pas:2267
    public override void InitializeNewMap()
    {
        HexControl control;
        for (int x = 6, last = MapContainer.Width - 7; x <= last; x++)
        {
            for (int y = 6, lastY = MapContainer.Height - 7; y <= lastY; y++)
            {
                GamePack.Current.CombatHexes.Place(MapContainer, (sbyte)x, (sbyte)y, 0, 1);
            }
        }

        control = Engine.HexSpriteSet.HexControl;
        for (int x = 0, last = MapContainer.Width - 1; x <= last; x++)
        {
            for (int y = 0; y <= 5; y++)
            {
                control.Place(MapContainer, (sbyte)x, (sbyte)y, 0, 15);
            }

            for (int y = MapContainer.Height - 6, lastY = MapContainer.Height - 1; y <= lastY; y++)
            {
                control.Place(MapContainer, (sbyte)x, (sbyte)y, 0, 15);
            }
        }

        for (int y = 6, lastY = MapContainer.Height - 7; y <= lastY; y++)
        {
            for (int x = 0; x <= 5; x++)
            {
                control.Place(MapContainer, (sbyte)x, (sbyte)y, 0, 15);
            }

            for (int x = MapContainer.Width - 6, last = MapContainer.Width - 1; x <= last; x++)
            {
                control.Place(MapContainer, (sbyte)x, (sbyte)y, 0, 15);
            }
        }
    }

    // AoWTC.pas:2338
    public override void DrawDynamicScene(ref LockedSurface lockedSurface)
    {
        base.DrawDynamicScene(ref lockedSurface);
        if (StrategicMap.Current != null)
        {
            DrawFog(lockedSurface);
        }

        if (ShowGrid)
        {
            DrawGrid(lockedSurface);
        }

        DrawMoveRoute(lockedSurface);
        if (((GameMapMouse)Mouse).SelectionRectangle.Left != -1)
        {
            DrawSelectionRectangle(ViewPixelX, ViewPixelY, ((GameMapMouse)Mouse).SelectionRectangle, lockedSurface);
        }
    }

    // AoWTC.pas:2429
    public override void NewFrame()
    {
        int i;
        BattleEffectSprite effect;
        if (!Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            i = 0;
            while (i < Effects.Count)
            {
                effect = Effects[i];
                if (effect.Done)
                {
                    DynamicFixedVisibleHexSprite.RemoveChild(effect);
                    DynamicVisibleHexSprite.RemoveChild(effect);
                    effect?.Dispose();
                    Effects.RemoveAt(i);
                }
                else
                {
                    i += 1;
                }
            }
        }

        base.NewFrame();
        if (!Status.Contains(HexSpriteMapStatusElement.Editting) && Combat != null)
        {
            UpdateCursor();
            if (EndTurnPending && ((StrategicMap.Current.Players.GetPlayers(CurrentPlayer).PlayerType != PlayerType.Human && StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl.GetPlayers(Combat.HostPlayer).ConnectionMode != ConnectionMode.Remote) || (Players.GetPlayers(CurrentCombatPlayer).Auto && StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl.GetPlayers(CurrentPlayer).ConnectionMode != ConnectionMode.Remote)))
            {
                switch (AI.Status)
                {
                    case BattleAI.AIStatus.Idle:
                        GetEngine().OutputDebugString("AI for Player " + IntToStr(CurrentPlayer) + " turn begins.");
                        AI.BeginTurn();
                        break;
                    case BattleAI.AIStatus.Busy:
                        AI.ContinueTurn();
                        break;
                    case BattleAI.AIStatus.Done:
                        if (EndTurn(true))
                        {
                            AI.Status = BattleAI.AIStatus.Idle;
                            GetEngine().OutputDebugString("AI for Player " + IntToStr(CurrentPlayer) + " turn ends.");
                        }

                        break;
                }
            }
        }
    }

    // AoWTC.pas:2515
    public override void DropHeroItem(Hero hero, Item item)
    {
        sbyte x;
        sbyte y;
        sbyte l;
        ItemHexSprite hexSprite;
        int i;
        x = -1;
        y = -1;
        l = -1;
        i = Combat.CombatData.GetCount();
        while (i != 0 && x == -1)
        {
            i -= 1;
            if (Combat.CombatData.GetObjects(i) is TacticalBattleUnit unit && unit.Troop == hero)
            {
                x = unit.UnitHexSprite.GetHexX();
                y = unit.UnitHexSprite.GetHexY();
                l = 0;
            }
        }

        if (x > -1)
        {
            hexSprite = (ItemHexSprite)MapContainer.GetField(x, y, l).FindOwnedChild(131697);
            if (hexSprite == null)
            {
                hexSprite = new TacticalBattleItemHexSprite(null) { Holder = CreationHolder() };
                // { Original bug: a failed placement leaves the new item sprite allocated. }
                if (Current.PlaceHex(hexSprite, x, y, l))
                {
                    item.SetOwner(hexSprite.Items);
                }
            }
            else
            {
                item.SetOwner(hexSprite.Items);
            }
        }
    }

    // AoWTC.pas:2544
    public override ItemList GetHeroItemsOnGround(Hero hero)
    {
        ItemHexSprite hexSprite;
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        ItemList result = null;
        GetUnitLocation(hero, out x, ref y, ref l);
        if (x > -1)
        {
            hexSprite = (ItemHexSprite)MapContainer.GetField(x, y, l).FindOwnedChild(131697);
            if (hexSprite != null)
            {
                result = hexSprite.Items;
            }
        }

        return result;
    }

    // AoWTC.pas:2243
    public void SetCombat(TacticalBattle combat)
    {
        Combat = combat;
        if (Combat != null)
        {
            Players.Players = Combat.CombatData.Players;
        }
    }

    // AoWTC.pas:2256
    public void MouseMoveEvent(object sender)
    {
        TacticalBattleUnitHexSprite hexSprite;
        if (!Current.Status.Contains(HexSpriteMapStatusElement.Editting) && !Players.GetPlayers(CurrentCombatPlayer).Auto && Mouse.HexX != -1 && Mouse.HexY != -1 && Mouse.Capture == null)
        {
            Mouse.CursorMode = 0;
            int hexY = Mouse.HexY;
            int hexX = Mouse.HexX;
            hexSprite = (TacticalBattleUnitHexSprite)MapContainer.MapLevels[0].Field[hexX, hexY].FindOwnedChild(2228484);
            if (hexSprite != null)
            {
                Mouse.CursorMode = 257;
            }
        }
    }

    // AoWTC.pas:2288
    public void SetSeatedCombatPlayer(sbyte player)
    {
        SeatedCombatPlayer = player;
        if (player == -1)
        {
            SetSeatedPlayer(-1);
        }
        else
        {
            SetSeatedPlayer(Combat.CombatData.Players.GetPlayers(SeatedCombatPlayer).Player);
        }

        _seatedPlayerChangedEventsStore.Raise(CurrentCombatPlayer);
    }

    // AoWTC.pas:2296
    public void NewTurn(sbyte player)
    {
        TacticalBattleUnit u;
        EngineMessage message = default;
        CurrentCombatPlayer = player;
        if (player == 0)
        {
            Round += 1;
        }

        SetCurrentPlayer(Players.GetPlayers(CurrentCombatPlayer).Player);
        Combat.NewTurn((byte)CurrentCombatPlayer);
        UnitSelection.Unselect();
        StrategicMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(CurrentPlayer);
        if (Players.GetPlayers(CurrentCombatPlayer).Player == 0)
        {
            string ind = AbilityRules.TranslateResourceString(GetEngine(), s_independentsText);
            string beginRound = AbilityRules.TranslateResourceString(GetEngine(), s_beginRoundText);
            ShowMessage(ind + " " + beginRound);
        }
        else
        {
            TacticalBattlePlayer playerLocal = Players.GetPlayers(CurrentCombatPlayer);
            Player playerPlayerRace = StrategicMap.Current.Players.GetPlayers(playerLocal.Player);
            string raceNamesPlural = AbilityRules.TranslateResourceString(GetEngine(), GameTables.RaceNamesPluralTexts[playerPlayerRace.PlayerRace]);
            string beginRound = AbilityRules.TranslateResourceString(GetEngine(), s_beginRoundText);
            ShowMessage(raceNamesPlural + " " + beginRound);
        }

        TacticalBattlePlayer playerPlayer = Players.GetPlayers(CurrentCombatPlayer);
        if (StrategicMap.Current.Players.GetPlayers(playerPlayer.Player).PlayerType == PlayerType.Human)
        {
            TacticalBattlePlayer playerLocal = Players.GetPlayers(CurrentCombatPlayer);
            if (StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl.GetPlayers(playerLocal.Player).ConnectionMode != ConnectionMode.Remote)
            {
                SetSeatedCombatPlayer(CurrentCombatPlayer);
                if (Players.GetPlayers(CurrentCombatPlayer).LastX == 0)
                {
                    u = GetNextUnit(null, true, true);
                    if (u != null)
                    {
                        sbyte hexX = u.UnitHexSprite.GetHexX();
                        Players.GetPlayers(CurrentCombatPlayer).LastX = hexX;
                        sbyte hexY = u.UnitHexSprite.GetHexY();
                        Players.GetPlayers(CurrentCombatPlayer).LastY = hexY;
                        if (ExplorationEnabled)
                        {
                            for (int i = 0, last = Players.GetCount() - 1; i <= last; i++)
                            {
                                if (CurrentCombatPlayer != i)
                                {
                                    sbyte fUnitHexSpriteHexX = u.UnitHexSprite.GetHexX();
                                    Players.GetPlayers((sbyte)i).LastX = fUnitHexSpriteHexX;
                                    sbyte fUnitHexSpriteHexY = u.UnitHexSprite.GetHexY();
                                    Players.GetPlayers((sbyte)i).LastY = fUnitHexSpriteHexY;
                                }
                            }
                        }
                    }
                }

                TacticalBattlePlayer playerLastY = Players.GetPlayers(SeatedCombatPlayer);
                CenterViewTo(Players.GetPlayers(SeatedCombatPlayer).LastX, playerLastY.LastY, 0);
            }
        }

        AudioPlayback.PlaySample("Sfx\\Events\\NewEvent.wav");
        EngineMessage.InitializeMessage(out message, 537006081, this);
        SendMessage(ref message, this);
        if (ExplorationEnabled)
        {
            Rect rectangle = new(0, 0, 65, 58);
            TriggerMapChangedEvent(rectangle, 0);
        }

        UnitSelection.Unselect();
        ChangeTurnEventsStore.Raise(CurrentCombatPlayer);
    }

    // AoWTC.pas:2348
    public void DrawMoveRoute(in LockedSurface lockedSurface) => UnitSelection.ShowMovePaths(lockedSurface);

    // AoWTC.pas:2351
    public bool EndTurn(bool automatic)
    {
        TacticalBattleEndTurnTokenEvent tokenEvent;
        bool result = false;
        if (!automatic)
        {
            if (StrategicMap.Current.Players.GetPlayers(CurrentPlayer).PlayerType != PlayerType.Human)
            {
                return result;
            }

            if (Current.SeatedCombatPlayer != Current.CurrentCombatPlayer)
            {
                return result;
            }
        }

        if (StrategicMap.Current.TokenManager.PlayerReady(CurrentPlayer))
        {
            tokenEvent = CreateTacticalBattleEndTurnTokenEvent(CurrentPlayer);
            if (tokenEvent != null)
            {
                StrategicMap.Current.TokenManager.GetTokenControl(CurrentPlayer).AddEvent(tokenEvent);
                tokenEvent.Release();
                result = true;
                EndTurnPending = false;
            }
        }

        return result;
    }

    // AoWTC.pas:2370
    public void CancelEndTurn()
    {
    }

    // AoWTC.pas:2373
    // { Intentionally empty in the original. }
    public bool GetEndTurnStatus()
    {
        bool result = true;
        for (int i = 0, last = Players.GetCount() - 1; i <= last; i++)
        {
            result = result && Players.GetPlayers((sbyte)i).EndTurn;
        }

        return result;
    }

    // AoWTC.pas:2381
    public TacticalBattleUnit GetNextUnit(TacticalBattleUnit U, bool allUnits, bool forward)
    {
        int index;
        int i;
        int start;
        int step;
        if (U != null && !U.GetEnabled())
        {
            U = null;
        }

        TacticalBattleUnit result = U;
        if (forward)
        {
            step = 1;
        }
        else
        {
            step = -1;
        }

        index = 0;
        i = 0;
        start = 0;
        if (U != null)
        {
            while (i < Combat.CombatData.GetCount())
            {
                if (Combat.CombatData.GetObjects(i) == U)
                {
                    index = i + step;
                    start = i;
                    i = Combat.CombatData.GetCount();
                }

                i += 1;
            }
        }

        while (index < Combat.CombatData.GetCount() && index > -1)
        {
            if (Combat.CombatData.GetObjects(index) != null && Combat.CombatData.GetObjects(index) is CombatUnit && (allUnits || Combat.CombatData.GetObjects<TacticalBattleUnit>(index).MovePoints > 0) && Combat.CombatData.GetObjects(index).GetEnabled() && Combat.CombatData.GetObjects(index).CombatPlayer == CurrentCombatPlayer)
            {
                result = Combat.CombatData.GetObjects<TacticalBattleUnit>(index);
                index = -10;
            }

            index = index + step;
        }

        if (result == U)
        {
            if (forward)
            {
                index = 0;
            }
            else
            {
                index = Combat.CombatData.GetCount() - 1;
            }

            while (index != start && index > -1 && index < Combat.CombatData.GetCount())
            {
                if (Combat.CombatData.GetObjects(index) != null && Combat.CombatData.GetObjects(index) is CombatUnit && (allUnits || Combat.CombatData.GetObjects<TacticalBattleUnit>(index).MovePoints > 0) && Combat.CombatData.GetObjects(index).GetEnabled() && Combat.CombatData.GetObjects(index).CombatPlayer == CurrentCombatPlayer)
                {
                    result = Combat.CombatData.GetObjects<TacticalBattleUnit>(index);
                    index = -10;
                }

                index = index + step;
            }
        }

        return result;
    }

    // AoWTC.pas:2466
    public BattleEffectSprite AddEffect(BattleEffectType effectType)
    {
        BattleEffectSprite result = new(null) { Holder = CreationHolder() };
        Effects.Add(result);
        result.Activate();
        result.EffectType = effectType;
        DynamicFixedVisibleHexSprite.AddChild(result);
        return result;
    }

    // AoWTC.pas:2475
    public void UpdateCursor()
    {
        if (!((SeatedPlayer == -1 || StrategicMap.Current.TokenManager.PlayerReady(SeatedPlayer)) && SeatedPlayer == CurrentPlayer && !Players.GetPlayers(CurrentCombatPlayer).Auto && Current.SeatedCombatPlayer == Current.CurrentCombatPlayer))
        {
            Mouse.Cursor = ((int)(((uint)Frame) >> 2) % 4) + 6;
        }
        else
        {
            if (UnitSelection.GetTacticalBattleUnitHexSprite() != null)
            {
                UnitSelection.UpdateMoveCursor();
            }

            switch (Mouse.CursorMode)
            {
                case 0:
                    Mouse.Cursor = 0;
                    break;
                case 257:
                    Mouse.Cursor = ((int)(((uint)Frame) >> 2) % 4) + 52;
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
        TerminateCombatTokenEvent tokenEvent;
        tokenEvent = new TerminateCombatTokenEvent(null) { Holder = CreationHolder() };
        StrategicMap.Current.TokenManager.TokenExecutor.AddSystemEvent(tokenEvent);
        tokenEvent.Release();
    }

    // AoWTC.pas:2497
    public void SelectNextUnit(bool forward)
    {
        TacticalBattleUnit u;
        int x = default;
        int y = default;
        if (SeatedCombatPlayer == CurrentCombatPlayer)
        {
            u = GetNextUnit(Players.GetPlayers(SeatedCombatPlayer).LastUnit, false, forward);
            if (u != null)
            {
                if (UnitSelection.GetTacticalBattleUnitHexSprite() != u.UnitHexSprite)
                {
                    Players.GetPlayers(SeatedCombatPlayer).LastUnit = u;
                    sbyte hexY = u.UnitHexSprite.GetHexY();
                    CenterViewTo(u.UnitHexSprite.GetHexX(), hexY, 0);
                    Selection.UnselectLocation();
                    sbyte fUnitHexSpriteHexY = u.UnitHexSprite.GetHexY();
                    HexGeometry.HexToPixel(u.UnitHexSprite.GetHexX(), fUnitHexSpriteHexY, out x, out y);
                    Selection.SelectLocation(x, y, 0);
                }
            }
        }
    }

    // AoWTC.pas:2557
    // The combat object list holds the wall beside the units. The original casts every object to
    // TTacticalCombatUnit and reads FUnit. On a wall that offset is FWallType, FHitPoints and FLocked,
    // a small integer, so the unit comparison fails and the search continues. The port skips objects
    // that are not tactical units, which has the same result.
    public void GetUnitLocation(Troop unit, out sbyte x, ref sbyte y, ref sbyte level)
    {
        int i;
        x = -1;
        i = 0;
        while (i < Combat.CombatData.GetCount())
        {
            if (Combat.CombatData.GetObjects(i) is TacticalBattleUnit combatUnit && combatUnit.Troop == unit && Combat.CombatData.GetObjects(i).GetEnabled())
            {
                x = combatUnit.UnitHexSprite.GetHexX();
                y = combatUnit.UnitHexSprite.GetHexY();
                level = 0;
                i = Combat.CombatData.GetCount();
            }
            else
            {
                i += 1;
            }
        }
    }

    // AoWTC.pas:2572
    public void Surrender(sbyte mode)
    {
        TacticalBattleEndCombatTokenEvent tokenEvent;
        if (StrategicMap.Current.TokenManager.PlayerReady(Current.CurrentPlayer) && CurrentCombatPlayer == SeatedCombatPlayer)
        {
            UnitSelection.Unselect();
            tokenEvent = CreateTacticalBattleEndCombatTokenEvent();
            if (tokenEvent != null)
            {
                StrategicMap.Current.TokenManager.GetTokenControl(CurrentPlayer).AddEvent(tokenEvent);
                tokenEvent.Release();
            }
        }
    }

    private static readonly string s_independentsText = "Independents";

    private static readonly string s_beginRoundText = "begin their round.";

    private TacticalBattleEndCombatTokenEvent CreateTacticalBattleEndCombatTokenEvent() => new(null) { Holder = CreationHolder() };

    private TacticalBattleEndTurnTokenEvent CreateTacticalBattleEndTurnTokenEvent(sbyte player)
    {
        TacticalBattleEndTurnTokenEvent result;
        if (player != -1 && StrategicMap.Current.TokenManager.PlayerReady(player))
        {
            result = new TacticalBattleEndTurnTokenEvent(null) { Holder = CreationHolder() };
        }
        else
        {
            result = null;
        }

        return result;
    }

    internal static void PlaceOverviewUnit(CombatMap map, Troop unit, int position, int index)
    {
        OverviewUnitHexSprite hexSprite;
        sbyte overlay;
        TacticalBattleUnitHexSprite occupant;
        int x0;
        int y0;
        int x = default;
        int y = default;
        int i;
        MapCoordinate p;
        hexSprite = new OverviewUnitHexSprite(null) { Holder = map };
        hexSprite.SetU(unit);
        p = map.UnitPositions.GetUnitPosition(position, index).Location;
        x0 = p.X;
        y0 = p.Y;
        i = 0;
        do
        {
            HexGeometry.CenterHexNumberToHex(x0, y0, i, ref x, ref y);
            overlay = Current.MapContainer.MapLevels[0].Field[x, y].OverlayType;
            occupant = (TacticalBattleUnitHexSprite)Current.MapContainer.MapLevels[0].Field[x, y].FindOwnedChild(2228484);
            i += 1;
        }
        while (!(overlay != 8 && occupant == null));
        if (!map.PlaceHex(hexSprite, x, y, 0))
        {
            hexSprite?.Dispose();
        }
    }

    internal static CombatMap Current { get; set; }

    // AoWTC.pas:484

    // AoWTC.pas:484
}
