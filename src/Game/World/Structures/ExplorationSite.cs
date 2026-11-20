#nullable disable
// Translated from ExploreS.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
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
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Structures;

// DefendersMode, Defenders
public partial class ExplorationSite : Structure
{
    public ExplorationSite(CustomNode owner) : base(owner)
    {
        _defendersMode = ExplorationSiteDefendersMode.Normal;
        _defenders = new DefendersArmy(null) { Holder = CreationHolder() };
    }

    // ExploreS.pas:90
    public new const int ClassId = 132033;

    private ExplorationSiteDefendersMode _defendersMode;
    private readonly DefendersArmy _defenders;

    // ExploreS.pas:84
    public override void Dispose()
    {
        base.Dispose();
        _defenders?.Dispose();
    }

    // ExploreS.pas:293
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Defenders", objectValue: _defenders);
        _defendersMode = persister.Enum("DefendersMode", value: _defendersMode);
    }

    // ExploreS.pas:77

    // ExploreS.pas:351
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        _defenders.MessageProc(ref message, sender);
    }

    // ExploreS.pas:339
    public override void Activate()
    {
        base.Activate();
        _defenders.Activate();
    }

    // ExploreS.pas:345
    public override void Deactivate()
    {
        _defenders.Deactivate();
        base.Deactivate();
    }

    // ExploreS.pas:217
    public override bool CanSelect(int pixelX, int pixelY)
    {
        bool result;
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            HexGeometry.PixelToHex(pixelX, pixelY, out int hexX, out int hexY);
            result = Owner.HexX == hexX && Owner.HexY == hexY && base.CanSelect(pixelX, pixelY);
        }
        else
        {
            result = base.CanSelect(pixelX, pixelY);
        }

        return result;
    }

    // ExploreS.pas:96
    public override int GetTerrainTypeImage(int terrainType)
    {
        int result;
        if (!GetExplored() || StrategicMap.Current.Day == 0)
        {
            result = terrainType;
        }
        else
        {
            int image = terrainType + 100;
            result = Resource.ImageSequenceList.ImageSequences[image << 0] is not null ? image : terrainType;
        }

        return result;
    }

    // ExploreS.pas:209
    public override ExecuteResult ExecuteTokenEvent(StructureTokenEvent tokenEvent, TokenExecutor executor)
    {
        ExecuteResult result = base.ExecuteTokenEvent(tokenEvent, executor);
        if (tokenEvent.Action == 0)
        {
            result = ExecuteSearch((CombatType)tokenEvent.Param) ? ExecuteResult.Done : ExecuteResult.DoneHalt;
        }

        return result;
    }

    // ExploreS.pas:326
    public override void NewDay()
    {
        base.NewDay();
        if (StrategicMap.Current.Day == 1 && _defendersMode != ExplorationSiteDefendersMode.UserDefined)
        {
            GenerateDefenders();
        }
    }

    // ExploreS.pas:333
    public override void Update()
    {
        base.Update();
        ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // ExploreS.pas:93
    public virtual bool GetExplored() => true;

    // ExploreS.pas:138
    public virtual void SetupCombat(Battle combat, ArmyHexSprite army)
    {
        combat.SetExecutedHandler(CombatExecuted);
        sbyte hexY = army.GetHexY();
        sbyte hexLevel = army.GetHexLevel();
        sbyte hexX = GetHexX();
        sbyte hexY2 = GetHexY();
        sbyte hexLevel2 = GetHexLevel();
        sbyte armyHexX = army.GetHexX();
        _ = combat.Setup((byte)army.Army.Player, (byte)army.Army.IndependentRelation, armyHexX, hexY, hexLevel, (byte)_defenders.Player, (byte)_defenders.IndependentRelation, hexX, hexY2, hexLevel2, true);
        sbyte hexY3 = GetHexY();
        sbyte hexLevel3 = GetHexLevel();
        sbyte hexX2 = GetHexX();
        _ = combat.AddArmy(army.Army, CombatSideIndex.Attacker, hexX2, hexY3, hexLevel3);
        _ = combat.AddArmy(_defenders, CombatSideIndex.Defender, -1, -1, -1, 1);
        combat.ExplorationSite = 1;
    }

    // ExploreS.pas:110
    public virtual void ExecuteSearchDone(CombatType combatType, CombatResult combatResult)
    {
    }

    // ExploreS.pas:148
    public virtual bool ExecuteSearch(CombatType combatType)
    {
        ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
        bool result;
        if (armyHexSprite is not null && CanSearch(armyHexSprite.Army.Player))
        {
            sbyte player = armyHexSprite.Army.Player;
            if (_defenders.Units.Count == 0)
            {
                ExecuteSearchDone(CombatType.Fast, CombatResult.Victory);
            }
            else
            {
                Battle combat;
                CombatEventLog eventValue;
                if (combatType == CombatType.Fast)
                {
                    StrategicMap.Current.LockGameOverEvent();
                    combat = StrategicMap.Current.CreateCombat(131760);
                    SetupCombat(combat, armyHexSprite);
                    combat.LockExecuted();
                    combat.Initialize();
                    combat.Activate();
                    _ = combat.Execute();
                    combat.Deactivate();
                    combat.Finalize();
                    string name = GetName();
                    combat.Logbook.FinalData.Header = $"{name} {combat.Logbook.FinalData.Header}";
                    eventValue = new CombatEventLog(null) { Holder = CreationHolder() };
                    eventValue.SetCombatLogbook(combat.Logbook);
                    StrategicMap.Current.Players.GetPlayers(player).EventLogbook.AddEvent(eventValue);
                    if (StrategicMap.Current.SeatedPlayer == player && StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
                    {
                        _ = eventValue.Execute(false, ExecuteTrigger.Event, null);
                    }

                    _ = eventValue.Release();
                    combat.UnlockExecuted();
                    StrategicMap.Current.DestroyCombat();
                    StrategicMap.Current.UnlockGameOverEvent();
                }
                else
                {
                    combat = StrategicMap.Current.CreateCombat(2228496);
                    SetupCombat(combat, armyHexSprite);
                    combat.Initialize();
                    combat.Activate();
                    if (combat.Execute() != CombatResult.Busy)
                    {
                        combat.Deactivate();
                        combat.Finalize();
                        string name = GetName();
                        combat.Logbook.FinalData.Header = $"{name} {combat.Logbook.FinalData.Header}";
                        eventValue = new CombatEventLog(null) { Holder = CreationHolder() };
                        eventValue.SetCombatLogbook(combat.Logbook);
                        StrategicMap.Current.Players.GetPlayers(player).EventLogbook.AddEvent(eventValue);
                        if (StrategicMap.Current.SeatedPlayer == player && StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
                        {
                            _ = eventValue.Execute(false, ExecuteTrigger.Event, null);
                        }

                        _ = eventValue.Release();
                        StrategicMap.Current.DestroyCombat();
                    }
                }
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // ExploreS.pas:258
    public virtual bool Search()
    {
        ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
        bool result;
        if (armyHexSprite is not null && CanSearch(armyHexSprite.Army.Player))
        {
            PlayerCollection playerList = StrategicMap.Current.Players;
            if (playerList.LocalPlayers.Count == 1 || StrategicMap.Current.RequestCombatType)
            {
                CombatTypeRequestEventLog request = new(null) { Holder = CreationHolder() };
                sbyte hexLevel = GetHexLevel();
                sbyte hexY = GetHexY();
                request.SetCenterLocation((byte)GetHexX(), (byte)hexY, hexLevel);
                request.AttackerPlayer = armyHexSprite.Army.Player;
                request.DefenderPlayer = 0;
                StrategicMap.Current.TriggerTypedExecuteEventLog(request, true, RequestCombatTypeCallback);
                _ = request.Release();
                result = true;
            }
            else
            {
                if (!playerList.GetPlayers(armyHexSprite.Army.Player).GetBusy())
                {
                    StructureTokenEvent tokenEvent = CreateTokenEvent();
                    SetupTokenEvent(tokenEvent);
                    tokenEvent.Action = 0;
                    tokenEvent.Param = 1;
                    StrategicMap.Current.TokenManager.GetTokenControl(tokenEvent.Player).AddEvent(tokenEvent);
                    _ = tokenEvent.Release();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // ExploreS.pas:113
    public void CombatExecuted(Battle combat)
    {
        CombatData data = combat.CombatData;
        for (int i = 0, last = data.GetCount() - 1; i <= last; i++)
        {
            CombatObject objectValue = data.GetObjects(i);
            if (objectValue is CombatUnit combatUnit && objectValue.GetPlayer() == 0)
            {
                Troop troop = combatUnit.Troop;
                _ = troop.AddRef();
                combatUnit.SetUnit(null);
                _ = _defenders.AddUnit(troop);
                _ = troop.Release();
            }
        }

        if (combat is FastBattle)
        {
            ExecuteSearchDone(CombatType.Fast, combat.CombatResult);
        }
        else
        {
            ExecuteSearchDone(CombatType.Tactical, combat.CombatResult);
        }
    }

    // ExploreS.pas:230
    public void RequestCombatTypeCallback(CombatTypeRequestEventLog log)
    {
        if (log.CombatType != CombatType.None)
        {
            StructureTokenEvent tokenEvent = CreateTokenEvent();
            SetupTokenEvent(tokenEvent);
            tokenEvent.Action = 0;
            tokenEvent.Param = (int)log.CombatType;
            StrategicMap.Current.TokenManager.GetTokenControl(tokenEvent.Player).AddEvent(tokenEvent);
            _ = tokenEvent.Release();
        }
    }

    // ExploreS.pas:246
    public bool CanSearch(sbyte player)
    {
        bool result;
        if (!GetExplored())
        {
            ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
            if (armyHexSprite is null || armyHexSprite.Army.Player != player)
            {
                result = false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // ExploreS.pas:300
    public void GenerateDefenders()
    {
        RandSeed = StrategicMap.Current.DefendersSeed;
        if (_defendersMode == ExplorationSiteDefendersMode.Random)
        {
            _defendersMode = (ExplorationSiteDefendersMode)(Random(3) + 1);
        }

        // Defender lists live on the exploration resource. Another resource throws, as the old cast did.
        ExplorationSiteResource siteResource = GetResource<ExplorationSiteResource>();
        int collection = siteResource.DefenderCollections.GetRandomCollection();
        if (collection != -1)
        {
            UnitIndexList list = new();
            UnitIndexListCollection collections = siteResource.DefenderCollections;
            for (int i = collections.GetCollectionCount(collection) - 1; i >= 0; i--)
            {
                _ = list.Add(collections.GetU(collection, i));
            }

            try
            {
                _ = Int32List.FillWithRandomUnits(list, _defenders.Units, 4, (int)_defendersMode * 50, Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature, UnitType.Machine));
            }
            catch
            {
            }

            // { Original silently ignores defender-generation failures. }
            list?.Dispose();
        }
    }

    // ExploreS.pas:33

}

public enum ExplorationSiteDefendersMode : byte
{
    UserDefined = 0, Low = 1, Normal = 2, High = 3, Random = 4
}
