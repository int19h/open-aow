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

// FDefendersMode, FDefenders
public partial class ExplorationSite : Structure
{
    public ExplorationSite(ECustomNode owner) : base(owner)
    {
        FDefendersMode = DefendersMode.Normal;
        FDefenders = new DefendersArmy(null);
    }

    // ExploreS.pas:90
    public new const int ClassID = 132033;

    public DefendersMode FDefendersMode;
    public DefendersArmy FDefenders;

    // ExploreS.pas:84
    public override void Dispose()
    {
        base.Dispose();
        FDefenders?.Dispose();
    }

    // ExploreS.pas:293
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FDefenders, 5);
        Stream.rwData(Bytes.Of(ref FDefendersMode, 1), 1, 6);
    }

    // ExploreS.pas:77

    // ExploreS.pas:351
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        FDefenders.MsgProc(ref Msg, Sender);
    }

    // ExploreS.pas:339
    public override void Activate()
    {
        base.Activate();
        FDefenders.Activate();
    }

    // ExploreS.pas:345
    public override void Deactivate()
    {
        FDefenders.Deactivate();
        base.Deactivate();
    }

    // ExploreS.pas:217
    public override bool CanSelect(int Xhp, int Yhp)
    {
        bool Result;
        int Xhx = default;
        int Yhx = default;
        if (!AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            HexGeometry.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
            if (((MapField)Owner).Xhx == Xhx && ((MapField)Owner).Yhx == Yhx)
            {
                Result = base.CanSelect(Xhp, Yhp);
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = base.CanSelect(Xhp, Yhp);
        }

        return Result;
    }

    // ExploreS.pas:96
    public override int GetTerrainTypeImage(int TerrainType)
    {
        int Result;
        int Image;
        if (!GetExplored() || AoWHSMap.Current.FDay == 0)
        {
            Result = TerrainType;
        }
        else
        {
            Image = TerrainType + 100;
            if (FResource.ImageSequenceList.ImageSequences[Image << 0] != null)
            {
                Result = Image;
            }
            else
            {
                Result = TerrainType;
            }
        }

        return Result;
    }

    // ExploreS.pas:209
    public override ExecuteResult ExecuteTE(StructureTokenEvent TE, TokenExecutor Executer)
    {
        ExecuteResult Result = base.ExecuteTE(TE, Executer);
        if (TE.FAction == 0)
        {
            if (ExecuteSearch((CombatType)TE.FParam))
            {
                Result = ExecuteResult.Done;
            }
            else
            {
                Result = ExecuteResult.DoneHalt;
            }
        }

        return Result;
    }

    // ExploreS.pas:326
    public override void NewDay()
    {
        base.NewDay();
        if (AoWHSMap.Current.FDay == 1 && FDefendersMode != DefendersMode.UserDefined)
        {
            GenerateDefenders();
        }
    }

    // ExploreS.pas:333
    public override void Update()
    {
        base.Update();
        FImageIndex = GetTerrainTypeImage(FTerrainType);
    }

    // ExploreS.pas:93
    public virtual bool GetExplored() => true;

    // ExploreS.pas:138
    public virtual void SetupCombat(Combat.Battle Combat, ArmyHexSprite Army)
    {
        Combat.FOnExecuted = CombatExecuted;
        var yhx = Army.GetYhx();
        var lhx = Army.GetLhx();
        var xhx = GetXhx();
        var yhx2 = GetYhx();
        var lhx2 = GetLhx();
        var armyXhx = Army.GetXhx();
        Combat.Setup((byte)Army.FArmy.FPlayer, (byte)Army.FArmy.FIndependentRelation, armyXhx, yhx, lhx, (byte)FDefenders.FPlayer, (byte)FDefenders.FIndependentRelation, xhx, yhx2, lhx2, true);
        var yhx3 = GetYhx();
        var lhx3 = GetLhx();
        var xhx2 = GetXhx();
        Combat.AddArmy(Army.FArmy, CombatSideIndex.Attacker, xhx2, yhx3, lhx3);
        Combat.AddArmyEx(FDefenders, CombatSideIndex.Defender, -1, -1, -1, 1);
        Combat.FExplorationSite = 1;
    }

    // ExploreS.pas:110
    public virtual void ExecuteSearchDone(CombatType CombatType, CombatResult CombatResult)
    {
    }

    // ExploreS.pas:148
    public virtual bool ExecuteSearch(CombatType CombatType)
    {
        bool Result = default;
        ArmyHexSprite ArmyHS;
        Combat.Battle Combat;
        sbyte Player;
        CombatEventLog Event;
        ArmyHS = (ArmyHexSprite)Owner.FindOwnedChild(131607);
        if (ArmyHS != null && CanSearch(ArmyHS.FArmy.FPlayer))
        {
            Player = ArmyHS.FArmy.FPlayer;
            if (FDefenders.GetCount() == 0)
            {
                ExecuteSearchDone(CombatType.Fast, CombatResult.Victory);
            }
            else
            {
                if (CombatType == CombatType.Fast)
                {
                    AoWHSMap.Current.LockGameOverEvent();
                    Combat = AoWHSMap.Current.CreateCombat(131760);
                    SetupCombat(Combat, ArmyHS);
                    Combat.LockExecuted();
                    Combat.Initialize();
                    Combat.Activate();
                    Combat.Execute();
                    Combat.Deactivate();
                    Combat.Finalize();
                    var name = GetName();
                    Combat.FLogbook.FFinalData.FHeader = name + " " + Combat.FLogbook.FFinalData.FHeader;
                    Event = new CombatEventLog(null);
                    Event.SetCombatLogbook(Combat.FLogbook);
                    AoWHSMap.Current.FPlayers.GetPlayers(Player).FEventLogbook.AddEvent(Event);
                    if (AoWHSMap.Current.FSeatedPlayer == Player && AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType == PlayerType.Human)
                    {
                        Event.Execute(false, ExecuteTrigger.Event, null);
                    }

                    Event.Release();
                    Combat.UnlockExecuted();
                    AoWHSMap.Current.DestroyCombat();
                    AoWHSMap.Current.UnlockGameOverEvent();
                }
                else
                {
                    Combat = AoWHSMap.Current.CreateCombat(2228496);
                    SetupCombat(Combat, ArmyHS);
                    Combat.Initialize();
                    Combat.Activate();
                    if (Combat.Execute() != CombatResult.Busy)
                    {
                        Combat.Deactivate();
                        Combat.Finalize();
                        var name = GetName();
                        Combat.FLogbook.FFinalData.FHeader = name + " " + Combat.FLogbook.FFinalData.FHeader;
                        Event = new CombatEventLog(null);
                        Event.SetCombatLogbook(Combat.FLogbook);
                        AoWHSMap.Current.FPlayers.GetPlayers(Player).FEventLogbook.AddEvent(Event);
                        if (AoWHSMap.Current.FSeatedPlayer == Player && AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType == PlayerType.Human)
                        {
                            Event.Execute(false, ExecuteTrigger.Event, null);
                        }

                        Event.Release();
                        AoWHSMap.Current.DestroyCombat();
                    }
                }
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // ExploreS.pas:258
    public virtual bool Search()
    {
        bool Result = default;
        ArmyHexSprite ArmyHS = default;
        CombatTypeRequestEventLog Request = default;
        StructureTokenEvent TE = default;
        ArmyHS = (ArmyHexSprite)Owner.FindOwnedChild(131607);
        if (ArmyHS != null && CanSearch(ArmyHS.FArmy.FPlayer))
        {
            var playerList = AoWHSMap.Current.FPlayers;
            if (playerList.FLocalPlayers.Count == 1 || AoWHSMap.Current.FRequestCombatType)
            {
                Request = new CombatTypeRequestEventLog(null);
                var lhx = GetLhx();
                var yhx = GetYhx();
                Request.SetCenterLocation((byte)GetXhx(), (byte)yhx, lhx);
                Request.FPlayer = ArmyHS.FArmy.FPlayer;
                Request.FDefenderPlayer = 0;
                AoWHSMap.Current.TriggerExecuteEventLog(Request, true, RequestCombatTypeCallBack);
                Request.Release();
                Result = true;
            }
            else
            {
                if (!playerList.GetPlayers(ArmyHS.FArmy.FPlayer).GetBusy())
                {
                    TE = CreateTE();
                    SetupTE(TE);
                    TE.FAction = 0;
                    TE.FParam = 1;
                    AoWHSMap.Current.FTokenManager.GetTokenControl(TE.FPlayer).AddEvent(TE);
                    TE.Release();
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // ExploreS.pas:113
    public void CombatExecuted(Combat.Battle Combat)
    {
        CombatData Data;
        CombatObject Obj;
        Troop AUnit;
        Data = Combat.FCombatData;
        for (int I = 0, last = Data.GetCount() - 1; I <= last; I++)
        {
            Obj = Data.GetObjects(I);
            if (Obj is CombatUnit && Obj.GetPlayer() == 0)
            {
                AUnit = ((CombatUnit)Obj).FUnit;
                AUnit.AddRef();
                ((CombatUnit)Obj).SetUnit(null);
                FDefenders.AddUnit(AUnit);
                AUnit.Release();
            }
        }

        if (Combat is FastBattle)
        {
            ExecuteSearchDone(CombatType.Fast, Combat.FCombatResult);
        }
        else
        {
            ExecuteSearchDone(CombatType.Tactical, Combat.FCombatResult);
        }
    }

    // ExploreS.pas:230
    public void RequestCombatTypeCallBack(EventLog Log)
    {
        StructureTokenEvent TE;
        var combatTypeRequestEventLog = (CombatTypeRequestEventLog)Log;
        if (combatTypeRequestEventLog.FCombatType != CombatType.None)
        {
            TE = CreateTE();
            SetupTE(TE);
            TE.FAction = 0;
            TE.FParam = (int)combatTypeRequestEventLog.FCombatType;
            AoWHSMap.Current.FTokenManager.GetTokenControl(TE.FPlayer).AddEvent(TE);
            TE.Release();
        }
    }

    // ExploreS.pas:246
    public bool CanSearch(sbyte Player)
    {
        bool Result;
        ArmyHexSprite ArmyHS;
        if (!GetExplored())
        {
            ArmyHS = (ArmyHexSprite)Owner.FindOwnedChild(131607);
            if (ArmyHS == null || ArmyHS.FArmy.FPlayer != Player)
            {
                Result = false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // ExploreS.pas:300
    public void GenerateDefenders()
    {
        int Collection;
        UnitIndexList List;
        UnitIndexListCollection Collections;
        RandSeed = AoWHSMap.Current.FDefendersSeed;
        if (FDefendersMode == DefendersMode.Random)
        {
            FDefendersMode = (DefendersMode)(Random(3) + 1);
        }

        Collection = ((ExplorationSiteResource)FResource).FDefenderCollections.GetRndCollection();
        if (Collection != -1)
        {
            List = new UnitIndexList();
            Collections = ((ExplorationSiteResource)FResource).FDefenderCollections;
            for (int I = Collections.GetCollectionCount(Collection) - 1; I >= 0; I--)
            {
                List.Add(Collections.GetU(Collection, I));
            }

            try
            {
                Int32List.FillWithRandomUnits(List, FDefenders, 4, (int)FDefendersMode * 50, Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature, UnitType.Machine));
            }
            catch
            {
            }

            // { Original silently ignores defender-generation failures. }
            List?.Dispose();
        }
    }

    public enum DefendersMode : byte
    {
        UserDefined = 0, Low = 1, Normal = 2, High = 3, Random = 4
    }

    // ExploreS.pas:33

}
