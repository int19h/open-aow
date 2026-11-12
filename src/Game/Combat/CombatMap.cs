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

    private EventStore<CombatMap> _playerChangedEventsStore;

    public event NotifyEvent<CombatMap> PlayerChanged
    {
        add => _playerChangedEventsStore.Add(value);
        remove => _playerChangedEventsStore.Remove(value);
    }

    private EventStore<sbyte> _seatedPlayerChangedEventsStore;

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
            var effect = Effects[0];
            DynamicFixedVisibleHexSprite.RemoveChild((EngineObject)effect);
            var effect2 = Effects[0];
            DynamicVisibleHexSprite.RemoveChild((EngineObject)effect2);
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
    public CombatMap(ECustomNode owner) : base(owner)
    {
        CombatStartedEventsStore = new();
        AbilityActivatedEventsStore = new();

        UnitPositions = new UnitPositionControl(null);
        ChangeTurnEventsStore = new();
        _playerChangedEventsStore = new();
        _seatedPlayerChangedEventsStore = new();
        MoveControl = new CombatMoveControl(this);
        SelectEventsStore = new();
        DeselectEventsStore = new();
        MouseOverTargetEventsStore = new();
        UnitChangedEventsStore = new();
        Players = new TacticalBattlePlayerList();
        UnitSelection = new CombatUnitSelectionControl();
        ((GameMapMouse)Mouse).RectangleSelected += UnitSelection.RectangleSelectEvent;
        Effects = new List<BattleEffectSprite>();
        Walls = new List<World.Terrain.CityWall>();
        FleeLocations = new List<FleeLocation>();
        AI = new BattleAI();
        Mouse.SetCapture(this);
        Mouse.OnMove += MouseMoveEvent;
        Mouse.CursorMode = 256;
        for (int I = 0; I <= 7; I++)
        {
            PositionStatus[I] = 2;
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
        for (int X = 6, last = MapContainer.Width - 7; X <= last; X++)
        {
            for (int Y = 6, lastY = MapContainer.Height - 7; Y <= lastY; Y++)
            {
                GamePack.Current.CombatHexes.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 1);
            }
        }

        control = Engine.HexSpriteSet.HexControl;
        for (int X = 0, last = MapContainer.Width - 1; X <= last; X++)
        {
            for (int Y = 0; Y <= 5; Y++)
            {
                control.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 15);
            }

            for (int Y = MapContainer.Height - 6, lastY = MapContainer.Height - 1; Y <= lastY; Y++)
            {
                control.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 15);
            }
        }

        for (int Y = 6, lastY = MapContainer.Height - 7; Y <= lastY; Y++)
        {
            for (int X = 0; X <= 5; X++)
            {
                control.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 15);
            }

            for (int X = MapContainer.Width - 6, last = MapContainer.Width - 1; X <= last; X++)
            {
                control.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 15);
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
        int I;
        BattleEffectSprite effect;
        if (!Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            I = 0;
            while (I < Effects.Count)
            {
                effect = Effects[I];
                if (effect.Done)
                {
                    DynamicFixedVisibleHexSprite.RemoveChild(effect);
                    DynamicVisibleHexSprite.RemoveChild(effect);
                    effect?.Dispose();
                    Effects.RemoveAt(I);
                }
                else
                {
                    I += 1;
                }
            }
        }

        base.NewFrame();
        if (!Status.Contains(HexSpriteMapStatusElement.Editting) && Combat != null)
        {
            UpdateCursor();
            if (EndTurnPending && (StrategicMap.Current.Players.GetPlayers(CurrentPlayer).PlayerType != PlayerType.Human && StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl.GetPlayers(Combat.HostPlayer).ConnectionMode != ConnectionMode.Remote || Players.GetPlayers(CurrentCombatPlayer).Auto && StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl.GetPlayers(CurrentPlayer).ConnectionMode != ConnectionMode.Remote))
            {
                switch (AI.Status)
                {
                    case BattleAI.AIStatus.Idle:
                        GameTables.GameEngine.OutputDebugString("AI for Player " + IntToStr(CurrentPlayer) + " turn begins.");
                        AI.BeginTurn();
                        break;
                    case BattleAI.AIStatus.Busy:
                        AI.ContinueTurn();
                        break;
                    case BattleAI.AIStatus.Done:
                        if (EndTurn(true))
                        {
                            AI.Status = BattleAI.AIStatus.Idle;
                            GameTables.GameEngine.OutputDebugString("AI for Player " + IntToStr(CurrentPlayer) + " turn ends.");
                        }

                        break;
                }
            }
        }
    }

    // AoWTC.pas:2515
    public override void DropHeroItem(Hero hero, Item item)
    {
        sbyte X;
        sbyte Y;
        sbyte L;
        ItemHexSprite hexSprite;
        int I;
        X = -1;
        Y = -1;
        L = -1;
        I = Combat.CombatData.GetCount();
        while (I != 0 && X == -1)
        {
            I -= 1;
            if (Combat.CombatData.GetObjects(I) is TacticalBattleUnit && ((CombatUnit)Combat.CombatData.GetObjects(I)).Troop == hero)
            {
                X = ((TacticalBattleUnit)Combat.CombatData.GetObjects(I)).UnitHexSprite.GetHexX();
                Y = ((TacticalBattleUnit)Combat.CombatData.GetObjects(I)).UnitHexSprite.GetHexY();
                L = 0;
            }
        }

        if (X > -1)
        {
            hexSprite = (ItemHexSprite)MapContainer.GetField(X, Y, L).FindOwnedChild(131697);
            if (hexSprite == null)
            {
                hexSprite = new TacticalBattleItemHexSprite(null);
                // { Original bug: a failed placement leaves the new item sprite allocated. }
                if (Current.PlaceHex(hexSprite, X, Y, L))
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
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        ItemList result = null;
        GetUnitLocation(hero, ref X, ref Y, ref L);
        if (X > -1)
        {
            hexSprite = (ItemHexSprite)MapContainer.GetField(X, Y, L).FindOwnedChild(131697);
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
            var hexY = Mouse.HexY;
            var hexX = Mouse.HexX;
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
        TacticalBattleUnit U;
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
            var ind = AbilityRules.TranslateResourceString(s_independentsText);
            var beginRound = AbilityRules.TranslateResourceString(s_beginRoundText);
            ShowMessage(ind + " " + beginRound);
        }
        else
        {
            var playerLocal = Players.GetPlayers(CurrentCombatPlayer);
            var playerPlayerRace = StrategicMap.Current.Players.GetPlayers(playerLocal.Player);
            var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[playerPlayerRace.PlayerRace]);
            var beginRound = AbilityRules.TranslateResourceString(s_beginRoundText);
            ShowMessage(raceNamesPlural + " " + beginRound);
        }

        var playerPlayer = Players.GetPlayers(CurrentCombatPlayer);
        if (StrategicMap.Current.Players.GetPlayers(playerPlayer.Player).PlayerType == PlayerType.Human)
        {
            var playerLocal = Players.GetPlayers(CurrentCombatPlayer);
            if (StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl.GetPlayers(playerLocal.Player).ConnectionMode != ConnectionMode.Remote)
            {
                SetSeatedCombatPlayer(CurrentCombatPlayer);
                if (Players.GetPlayers(CurrentCombatPlayer).LastX == 0)
                {
                    U = GetNextUnit(null, true, true);
                    if (U != null)
                    {
                        var hexX = U.UnitHexSprite.GetHexX();
                        Players.GetPlayers(CurrentCombatPlayer).LastX = hexX;
                        var hexY = U.UnitHexSprite.GetHexY();
                        Players.GetPlayers(CurrentCombatPlayer).LastY = hexY;
                        if (ExplorationEnabled)
                        {
                            for (int I = 0, last = Players.GetCount() - 1; I <= last; I++)
                            {
                                if (CurrentCombatPlayer != I)
                                {
                                    var fUnitHexSpriteHexX = U.UnitHexSprite.GetHexX();
                                    Players.GetPlayers((sbyte)I).LastX = fUnitHexSpriteHexX;
                                    var fUnitHexSpriteHexY = U.UnitHexSprite.GetHexY();
                                    Players.GetPlayers((sbyte)I).LastY = fUnitHexSpriteHexY;
                                }
                            }
                        }
                    }
                }

                var playerLastY = Players.GetPlayers(SeatedCombatPlayer);
                CenterViewTo(Players.GetPlayers(SeatedCombatPlayer).LastX, playerLastY.LastY, 0);
            }
        }

        AudioPlayback.PlaySample("Sfx\\Events\\NewEvent.wav");
        EngineMessage.InitializeMessage(ref message, 537006081, this);
        SendMessage(ref message, this);
        if (ExplorationEnabled)
        {
            var rectangle = new Rect(0, 0, 65, 58);
            TriggerMapChangedEvent(rectangle, 0);
        }

        UnitSelection.Unselect();
        ChangeTurnEventsStore.Raise(CurrentCombatPlayer);
    }

    // AoWTC.pas:2348
    public void DrawMoveRoute(in LockedSurface lockedSurface)
    {
        UnitSelection.ShowMovePaths(lockedSurface);
    }

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
        for (int I = 0, last = Players.GetCount() - 1; I <= last; I++)
        {
            result = result && Players.GetPlayers((sbyte)I).EndTurn;
        }

        return result;
    }

    // AoWTC.pas:2381
    public TacticalBattleUnit GetNextUnit(TacticalBattleUnit U, bool allUnits, bool forward)
    {
        int index;
        int I;
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
        I = 0;
        start = 0;
        if (U != null)
        {
            while (I < this.Combat.CombatData.GetCount())
            {
                if (this.Combat.CombatData.GetObjects(I) == U)
                {
                    index = I + step;
                    start = I;
                    I = this.Combat.CombatData.GetCount();
                }

                I += 1;
            }
        }

        while (index < this.Combat.CombatData.GetCount() && index > -1)
        {
            if (this.Combat.CombatData.GetObjects(index) != null && this.Combat.CombatData.GetObjects(index) is CombatUnit && (allUnits || ((TacticalBattleUnit)this.Combat.CombatData.GetObjects(index)).MovePoints > 0) && this.Combat.CombatData.GetObjects(index).GetEnabled() && this.Combat.CombatData.GetObjects(index).CombatPlayer == CurrentCombatPlayer)
            {
                result = (TacticalBattleUnit)this.Combat.CombatData.GetObjects(index);
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
                index = this.Combat.CombatData.GetCount() - 1;
            }

            while (index != start && index > -1 && index < this.Combat.CombatData.GetCount())
            {
                if (this.Combat.CombatData.GetObjects(index) != null && this.Combat.CombatData.GetObjects(index) is CombatUnit && (allUnits || ((TacticalBattleUnit)this.Combat.CombatData.GetObjects(index)).MovePoints > 0) && this.Combat.CombatData.GetObjects(index).GetEnabled() && this.Combat.CombatData.GetObjects(index).CombatPlayer == CurrentCombatPlayer)
                {
                    result = (TacticalBattleUnit)this.Combat.CombatData.GetObjects(index);
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
        BattleEffectSprite result = new BattleEffectSprite(null);
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
            Mouse.Cursor = (int)(((uint)Frame) >> 2) % 4 + 6;
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
        TerminateCombatTokenEvent tokenEvent;
        tokenEvent = new TerminateCombatTokenEvent(null);
        StrategicMap.Current.TokenManager.TokenExecutor.AddSystemEvent(tokenEvent);
        tokenEvent.Release();
    }

    // AoWTC.pas:2497
    public void SelectNextUnit(bool forward)
    {
        TacticalBattleUnit U;
        int X = default;
        int Y = default;
        if (SeatedCombatPlayer == CurrentCombatPlayer)
        {
            U = GetNextUnit(Players.GetPlayers(SeatedCombatPlayer).LastUnit, false, forward);
            if (U != null)
            {
                if (UnitSelection.GetTacticalBattleUnitHexSprite() != U.UnitHexSprite)
                {
                    Players.GetPlayers(SeatedCombatPlayer).LastUnit = U;
                    var hexY = U.UnitHexSprite.GetHexY();
                    CenterViewTo(U.UnitHexSprite.GetHexX(), hexY, 0);
                    Selection.UnselectLocation();
                    var fUnitHexSpriteHexY = U.UnitHexSprite.GetHexY();
                    HexGeometry.HexToPixel(U.UnitHexSprite.GetHexX(), fUnitHexSpriteHexY, ref X, ref Y);
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
        while (I < Combat.CombatData.GetCount())
        {
            if (Combat.CombatData.GetObjects(I) is TacticalBattleUnit && ((CombatUnit)Combat.CombatData.GetObjects(I)).Troop == U && Combat.CombatData.GetObjects(I).GetEnabled())
            {
                X = ((TacticalBattleUnit)Combat.CombatData.GetObjects(I)).UnitHexSprite.GetHexX();
                Y = ((TacticalBattleUnit)Combat.CombatData.GetObjects(I)).UnitHexSprite.GetHexY();
                L = 0;
                I = Combat.CombatData.GetCount();
            }
            else
            {
                I += 1;
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

    private static TacticalBattleEndCombatTokenEvent CreateTacticalBattleEndCombatTokenEvent() => new TacticalBattleEndCombatTokenEvent(null);

    private static TacticalBattleEndTurnTokenEvent CreateTacticalBattleEndTurnTokenEvent(sbyte player)
    {
        TacticalBattleEndTurnTokenEvent result;
        if (player != -1 && StrategicMap.Current.TokenManager.PlayerReady(player))
        {
            result = new TacticalBattleEndTurnTokenEvent(null);
        }
        else
        {
            result = null;
        }

        return result;
    }

    internal static void PlaceOverviewUnit(CombatMap map, Troop U, int position, int index)
    {
        OverviewUnitHexSprite hexSprite;
        sbyte overlay;
        TacticalBattleUnitHexSprite occupant;
        int x0;
        int y0;
        int X = default;
        int Y = default;
        int I;
        MapCoordinate P;
        hexSprite = new OverviewUnitHexSprite(null);
        hexSprite.SetU(U);
        P = map.UnitPositions.GetUnitPosition(position, index);
        x0 = P.X;
        y0 = P.Y;
        I = 0;
        do
        {
            HexGeometry.CenterHexNumberToHex(x0, y0, I, ref X, ref Y);
            overlay = Current.MapContainer.MapLevels[0].Field[X, Y].OverlayType;
            occupant = (TacticalBattleUnitHexSprite)Current.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2228484);
            I += 1;
        }
        while (!(overlay != 8 && occupant == null));
        if (!map.PlaceHex(hexSprite, X, Y, 0))
        {
            hexSprite?.Dispose();
        }
    }

    internal static CombatMap Current { get; set; }

    // AoWTC.pas:484

    // AoWTC.pas:484
}
