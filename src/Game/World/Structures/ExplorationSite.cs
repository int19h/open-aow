// Translated from ExploreS.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// DefendersMode, Defenders
public partial class ExplorationSite : Structure
{
    public ExplorationSite(ECustomNode owner) : base(owner)
    {
        _defendersMode = ExplorationSiteDefendersMode.Normal;
        _defenders = new DefendersArmy(null);
    }

    // ExploreS.pas:90
    public new const int ClassId = 132033;

    private ExplorationSiteDefendersMode _defendersMode;
    private DefendersArmy _defenders;

    // ExploreS.pas:84
    public override void Dispose()
    {
        base.Dispose();
        _defenders?.Dispose();
    }

    // ExploreS.pas:293
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(_defenders, 5);
        stream.rwData(Bytes.Of(ref _defendersMode, 1), 1, 6);
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
        int hexX = default;
        int hexY = default;
        if (!AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            HexGeometry.PixelToHex(pixelX, pixelY, ref hexX, ref hexY);
            if (Owner.HexX == hexX && Owner.HexY == hexY)
            {
                result = base.CanSelect(pixelX, pixelY);
            }
            else
            {
                result = false;
            }
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
        int image;
        if (!GetExplored() || AoWHSMap.Current.Day == 0)
        {
            result = terrainType;
        }
        else
        {
            image = terrainType + 100;
            if (_resource.ImageSequenceList.ImageSequences[image << 0] != null)
            {
                result = image;
            }
            else
            {
                result = terrainType;
            }
        }

        return result;
    }

    // ExploreS.pas:209
    public override ExecuteResult ExecuteTokenEvent(StructureTokenEvent tokenEvent, TokenExecutor executor)
    {
        ExecuteResult result = base.ExecuteTokenEvent(tokenEvent, executor);
        if (tokenEvent.Action == 0)
        {
            if (ExecuteSearch((CombatType)tokenEvent.Param))
            {
                result = ExecuteResult.Done;
            }
            else
            {
                result = ExecuteResult.DoneHalt;
            }
        }

        return result;
    }

    // ExploreS.pas:326
    public override void NewDay()
    {
        base.NewDay();
        if (AoWHSMap.Current.Day == 1 && _defendersMode != ExplorationSiteDefendersMode.UserDefined)
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
    public virtual void SetupCombat(Combat.Battle combat, ArmyHexSprite army)
    {
        combat.OnExecuted = CombatExecuted;
        var hexY = army.GetHexY();
        var hexLevel = army.GetHexLevel();
        var hexX = GetHexX();
        var hexY2 = GetHexY();
        var hexLevel2 = GetHexLevel();
        var armyHexX = army.GetHexX();
        combat.Setup((byte)army.Army.Player, (byte)army.Army.IndependentRelation, armyHexX, hexY, hexLevel, (byte)_defenders.Player, (byte)_defenders.IndependentRelation, hexX, hexY2, hexLevel2, true);
        var hexY3 = GetHexY();
        var hexLevel3 = GetHexLevel();
        var hexX2 = GetHexX();
        combat.AddArmy(army.Army, CombatSideIndex.Attacker, hexX2, hexY3, hexLevel3);
        combat.AddArmy(_defenders, CombatSideIndex.Defender, -1, -1, -1, 1);
        combat.ExplorationSite = 1;
    }

    // ExploreS.pas:110
    public virtual void ExecuteSearchDone(CombatType combatType, CombatResult combatResult)
    {
    }

    // ExploreS.pas:148
    public virtual bool ExecuteSearch(CombatType combatType)
    {
        bool result = default;
        ArmyHexSprite armyHexSprite;
        Combat.Battle combat;
        sbyte player;
        CombatEventLog eventValue;
        armyHexSprite = (ArmyHexSprite)Owner.FindOwnedChild(131607);
        if (armyHexSprite != null && CanSearch(armyHexSprite.Army.Player))
        {
            player = armyHexSprite.Army.Player;
            if (_defenders.GetCount() == 0)
            {
                ExecuteSearchDone(CombatType.Fast, CombatResult.Victory);
            }
            else
            {
                if (combatType == CombatType.Fast)
                {
                    AoWHSMap.Current.LockGameOverEvent();
                    combat = AoWHSMap.Current.CreateCombat(131760);
                    SetupCombat(combat, armyHexSprite);
                    combat.LockExecuted();
                    combat.Initialize();
                    combat.Activate();
                    combat.Execute();
                    combat.Deactivate();
                    combat.Finalize();
                    var name = GetName();
                    combat.Logbook.FinalData.Header = name + " " + combat.Logbook.FinalData.Header;
                    eventValue = new CombatEventLog(null);
                    eventValue.SetCombatLogbook(combat.Logbook);
                    AoWHSMap.Current.Players.GetPlayers(player).EventLogbook.AddEvent(eventValue);
                    if (AoWHSMap.Current.SeatedPlayer == player && AoWHSMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
                    {
                        eventValue.Execute(false, ExecuteTrigger.Event, null);
                    }

                    eventValue.Release();
                    combat.UnlockExecuted();
                    AoWHSMap.Current.DestroyCombat();
                    AoWHSMap.Current.UnlockGameOverEvent();
                }
                else
                {
                    combat = AoWHSMap.Current.CreateCombat(2228496);
                    SetupCombat(combat, armyHexSprite);
                    combat.Initialize();
                    combat.Activate();
                    if (combat.Execute() != CombatResult.Busy)
                    {
                        combat.Deactivate();
                        combat.Finalize();
                        var name = GetName();
                        combat.Logbook.FinalData.Header = name + " " + combat.Logbook.FinalData.Header;
                        eventValue = new CombatEventLog(null);
                        eventValue.SetCombatLogbook(combat.Logbook);
                        AoWHSMap.Current.Players.GetPlayers(player).EventLogbook.AddEvent(eventValue);
                        if (AoWHSMap.Current.SeatedPlayer == player && AoWHSMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
                        {
                            eventValue.Execute(false, ExecuteTrigger.Event, null);
                        }

                        eventValue.Release();
                        AoWHSMap.Current.DestroyCombat();
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
        bool result = default;
        ArmyHexSprite armyHexSprite = default;
        CombatTypeRequestEventLog request = default;
        StructureTokenEvent tokenEvent = default;
        armyHexSprite = (ArmyHexSprite)Owner.FindOwnedChild(131607);
        if (armyHexSprite != null && CanSearch(armyHexSprite.Army.Player))
        {
            var playerList = AoWHSMap.Current.Players;
            if (playerList.LocalPlayers.Count == 1 || AoWHSMap.Current.RequestCombatType)
            {
                request = new CombatTypeRequestEventLog(null);
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                request.SetCenterLocation((byte)GetHexX(), (byte)hexY, hexLevel);
                request.Player = armyHexSprite.Army.Player;
                request.DefenderPlayer = 0;
                AoWHSMap.Current.TriggerExecuteEventLog(request, true, RequestCombatTypeCallback);
                request.Release();
                result = true;
            }
            else
            {
                if (!playerList.GetPlayers(armyHexSprite.Army.Player).GetBusy())
                {
                    tokenEvent = CreateTokenEvent();
                    SetupTokenEvent(tokenEvent);
                    tokenEvent.Action = 0;
                    tokenEvent.Param = 1;
                    AoWHSMap.Current.TokenManager.GetTokenControl(tokenEvent.Player).AddEvent(tokenEvent);
                    tokenEvent.Release();
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
    public void CombatExecuted(Combat.Battle combat)
    {
        CombatData data;
        CombatObject objectValue;
        Troop troop;
        data = combat.CombatData;
        for (int I = 0, last = data.GetCount() - 1; I <= last; I++)
        {
            objectValue = data.GetObjects(I);
            if (objectValue is CombatUnit && objectValue.GetPlayer() == 0)
            {
                troop = ((CombatUnit)objectValue).Troop;
                troop.AddRef();
                ((CombatUnit)objectValue).SetUnit(null);
                _defenders.AddUnit(troop);
                troop.Release();
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
    public void RequestCombatTypeCallback(EventLog log)
    {
        StructureTokenEvent tokenEvent;
        var combatTypeRequestEventLog = (CombatTypeRequestEventLog)log;
        if (combatTypeRequestEventLog.CombatType != CombatType.None)
        {
            tokenEvent = CreateTokenEvent();
            SetupTokenEvent(tokenEvent);
            tokenEvent.Action = 0;
            tokenEvent.Param = (int)combatTypeRequestEventLog.CombatType;
            AoWHSMap.Current.TokenManager.GetTokenControl(tokenEvent.Player).AddEvent(tokenEvent);
            tokenEvent.Release();
        }
    }

    // ExploreS.pas:246
    public bool CanSearch(sbyte player)
    {
        bool result;
        ArmyHexSprite armyHexSprite;
        if (!GetExplored())
        {
            armyHexSprite = (ArmyHexSprite)Owner.FindOwnedChild(131607);
            if (armyHexSprite == null || armyHexSprite.Army.Player != player)
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
        int collection;
        UnitIndexList list;
        UnitIndexListCollection collections;
        RandSeed = AoWHSMap.Current.DefendersSeed;
        if (_defendersMode == ExplorationSiteDefendersMode.Random)
        {
            _defendersMode = (ExplorationSiteDefendersMode)(Random(3) + 1);
        }

        collection = ((ExplorationSiteResource)_resource).DefenderCollections.GetRandomCollection();
        if (collection != -1)
        {
            list = new UnitIndexList();
            collections = ((ExplorationSiteResource)_resource).DefenderCollections;
            for (int I = collections.GetCollectionCount(collection) - 1; I >= 0; I--)
            {
                list.Add(collections.GetU(collection, I));
            }

            try
            {
                Int32List.FillWithRandomUnits(list, _defenders, 4, (int)_defendersMode * 50, Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature, UnitType.Machine));
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
