// Translated from ExploreS.pas.
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.EventLog;
using Aow.Units.HSEngine;
using Aow.Units.NetworkE;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.HSEngine.HSEngineUnit;

namespace Aow.Units.ExploreS;

// { ot=1 }
public partial class TDefendersArmy : TArmy
{
    public TDefendersArmy(TECustomNode owner) : base(owner)
    { }

    // ExploreS.pas:71
    public override bool CanAddUnit(TAbstractUnit AUnit)
    {
        bool Result = base.CanAddUnit(AUnit);
        return Result && !AUnit.GetTransporter() && AUnit.ValidTerrain(1, 4);
    }

    // intf_P2_07.inc:445

}

// FDefendersMode, FDefenders
public partial class TExplorationSite : TStructure
{
    public TExplorationSite(TECustomNode owner) : base(owner)
    {
        FDefendersMode = TDefendersMode.dmNormal;
        FDefenders = new TDefendersArmy(null);
    }

    // ExploreS.pas:90
    public new const int ClassID = 132033;

    public TDefendersMode FDefendersMode;
    public TDefendersArmy FDefenders;

    // ExploreS.pas:84
    public override void Destroy()
    {
        base.Destroy();
        FDefenders.Free();
    }

    // ExploreS.pas:293
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FDefenders, 5);
        Stream.rwData(Bytes.Of(ref FDefendersMode, 1), 1, 6);
    }

    // ExploreS.pas:77

    // ExploreS.pas:351
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
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
        if (!AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
        {
            HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
            if (((TMapField)Owner).Xhx == Xhx && ((TMapField)Owner).Yhx == Yhx)
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
        if (!GetExplored() || AoWHSMap.FDay == 0)
        {
            Result = TerrainType;
        }
        else
        {
            Image = TerrainType + 100;
            if (((TExplorationSiteResource)FResource).ImageSequenceList.ImageSequences[Image << 0] != null)
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
    public override TExecuteResult ExecuteTE(TStructureTE TE, TTokenExecuter Executer)
    {
        TExecuteResult Result = base.ExecuteTE(TE, Executer);
        if (TE.FAction == 0)
        {
            if (ExecuteSearch((TCombatType)TE.FParam))
            {
                Result = TExecuteResult.erDone;
            }
            else
            {
                Result = TExecuteResult.erDoneHalt;
            }
        }

        return Result;
    }

    // ExploreS.pas:326
    public override void NewDay()
    {
        base.NewDay();
        if (AoWHSMap.FDay == 1 && FDefendersMode != TDefendersMode.dmUserDefined)
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
    public virtual void SetupCombat(TCombat Combat, TArmyHS Army)
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
        Combat.AddArmy(Army.FArmy, TCombatSideIndex.csAttacker, xhx2, yhx3, lhx3);
        Combat.AddArmyEx(FDefenders, TCombatSideIndex.csDefender, -1, -1, -1, 1);
        Combat.FExplorationSite = 1;
    }

    // ExploreS.pas:110
    public virtual void ExecuteSearchDone(TCombatType CombatType, TCombatResult CombatResult)
    {
    }

    // ExploreS.pas:148
    public virtual bool ExecuteSearch(TCombatType CombatType)
    {
        bool Result = default;
        TArmyHS ArmyHS;
        TCombat Combat;
        sbyte Player;
        TCombatEventLog Event;
        ArmyHS = (TArmyHS)Owner.FindOwnedChild(131607);
        if (ArmyHS != null && CanSearch(ArmyHS.FArmy.FPlayer))
        {
            Player = ArmyHS.FArmy.FPlayer;
            if (FDefenders.GetCount() == 0)
            {
                ExecuteSearchDone(TCombatType.ctFast, TCombatResult.crVictory);
            }
            else
            {
                if (CombatType == TCombatType.ctFast)
                {
                    AoWHSMap.LockGameOverEvent();
                    Combat = AoWHSMap.CreateCombat(131760);
                    SetupCombat(Combat, ArmyHS);
                    Combat.LockExecuted();
                    Combat.Initialize();
                    Combat.Activate();
                    Combat.Execute();
                    Combat.Deactivate();
                    Combat.Finalize();
                    var name = GetName();
                    Combat.FLogbook.FFinalData.FHeader = name + " " + Combat.FLogbook.FFinalData.FHeader;
                    Event = new TCombatEventLog(null);
                    Event.SetCombatLogbook(Combat.FLogbook);
                    AoWHSMap.FPlayers.GetPlayers(Player).FEventLogbook.AddEvent(Event);
                    if (AoWHSMap.FSeatedPlayer == Player && AoWHSMap.FPlayers.GetPlayers(Player).FPlayerType == TPlayerType.ptHuman)
                    {
                        Event.Execute(false, TExecuteTrigger.etEvent, null);
                    }

                    Event.Release();
                    Combat.UnlockExecuted();
                    AoWHSMap.DestroyCombat();
                    AoWHSMap.UnlockGameOverEvent();
                }
                else
                {
                    Combat = AoWHSMap.CreateCombat(2228496);
                    SetupCombat(Combat, ArmyHS);
                    Combat.Initialize();
                    Combat.Activate();
                    if (Combat.Execute() != TCombatResult.crBusy)
                    {
                        Combat.Deactivate();
                        Combat.Finalize();
                        var name = GetName();
                        Combat.FLogbook.FFinalData.FHeader = name + " " + Combat.FLogbook.FFinalData.FHeader;
                        Event = new TCombatEventLog(null);
                        Event.SetCombatLogbook(Combat.FLogbook);
                        AoWHSMap.FPlayers.GetPlayers(Player).FEventLogbook.AddEvent(Event);
                        if (AoWHSMap.FSeatedPlayer == Player && AoWHSMap.FPlayers.GetPlayers(Player).FPlayerType == TPlayerType.ptHuman)
                        {
                            Event.Execute(false, TExecuteTrigger.etEvent, null);
                        }

                        Event.Release();
                        AoWHSMap.DestroyCombat();
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
        TArmyHS ArmyHS = default;
        TCombatTypeRequestEventLog Request = default;
        TStructureTE TE = default;
        ArmyHS = (TArmyHS)Owner.FindOwnedChild(131607);
        if (ArmyHS != null && CanSearch(ArmyHS.FArmy.FPlayer))
        {
            var playerList = AoWHSMap.FPlayers;
            if (playerList.FLocalPlayers.Count == 1 || AoWHSMap.FRequestCombatType)
            {
                Request = new TCombatTypeRequestEventLog(null);
                var lhx = GetLhx();
                var yhx = GetYhx();
                Request.SetCenterLocation((byte)GetXhx(), (byte)yhx, lhx);
                Request.FPlayer = ArmyHS.FArmy.FPlayer;
                Request.FDefenderPlayer = 0;
                AoWHSMap.TriggerExecuteEventLog(Request, true, RequestCombatTypeCallBack);
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
                    AoWHSMap.FTokenManager.GetTokenControl(TE.FPlayer).AddEvent(TE);
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
    public void CombatExecuted(TCombat Combat)
    {
        TCombatData Data;
        TCombatObject Obj;
        TAbstractUnit AUnit;
        Data = Combat.FCombatData;
        for (int I = 0, last = Data.GetCount() - 1; I <= last; I++)
        {
            Obj = Data.GetObjects(I);
            if (Obj is TCombatUnit && Obj.GetPlayer() == 0)
            {
                AUnit = ((TCombatUnit)Obj).FUnit;
                AUnit.AddRef();
                ((TCombatUnit)Obj).SetUnit(null);
                FDefenders.AddUnit(AUnit);
                AUnit.Release();
            }
        }

        if (Combat is TFastCombat)
        {
            ExecuteSearchDone(TCombatType.ctFast, Combat.FCombatResult);
        }
        else
        {
            ExecuteSearchDone(TCombatType.ctTactical, Combat.FCombatResult);
        }
    }

    // ExploreS.pas:230
    public void RequestCombatTypeCallBack(TEventLog Log)
    {
        TStructureTE TE;
        var combatTypeRequestEventLog = (TCombatTypeRequestEventLog)Log;
        if (combatTypeRequestEventLog.FCombatType != TCombatType.ctNone)
        {
            TE = CreateTE();
            SetupTE(TE);
            TE.FAction = 0;
            TE.FParam = (int)combatTypeRequestEventLog.FCombatType;
            AoWHSMap.FTokenManager.GetTokenControl(TE.FPlayer).AddEvent(TE);
            TE.Release();
        }
    }

    // ExploreS.pas:246
    public bool CanSearch(sbyte Player)
    {
        bool Result;
        TArmyHS ArmyHS;
        if (!GetExplored())
        {
            ArmyHS = (TArmyHS)Owner.FindOwnedChild(131607);
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
        TUnitIndexList List;
        TUnitIndexListCollection Collections;
        RandSeed = AoWHSMap.FDefendersSeed;
        if (FDefendersMode == TDefendersMode.dmRandom)
        {
            FDefendersMode = (TDefendersMode)(Random(3) + 1);
        }

        Collection = ((TExplorationSiteResource)FResource).FDefenderCollections.GetRndCollection();
        if (Collection != -1)
        {
            List = new TUnitIndexList();
            Collections = ((TExplorationSiteResource)FResource).FDefenderCollections;
            for (int I = Collections.GetCollectionCount(Collection) - 1; I >= 0; I--)
            {
                List.Add(Collections.GetU(Collection, I));
            }

            try
            {
                FillWithRandomUnits(List, FDefenders, 4, (int)FDefendersMode * 50, TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature, TUnitType.utMachine));
            }
            catch
            {
            }

            // { Original silently ignores defender-generation failures. }
            List.Free();
        }
    }

    // ExploreS.pas:33

}

// FDefenderCollections, the available defender army compositions
public partial class TExplorationSiteResource : TStructureResource
{
    public TExplorationSiteResource(TECustomNode owner) : base(owner)
    {
        FDefenderCollections = new TUnitIndexListCollection();
        FTerrainBound = false;
    }

    // ExploreS.pas:373
    public new const int ClassID = 132034;

    public TUnitIndexListCollection FDefenderCollections;

    // ExploreS.pas:364
    public override void Destroy()
    {
        FDefenderCollections.Free();
        base.Destroy();
    }

    // ExploreS.pas:376
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(25, FDefenderCollections.LoadFromStream, FDefenderCollections.SaveToStream);
    }

    // ExploreS.pas:357

    // ExploreS.pas:382
    public override int ResourceUserClassID() => 132033;

    // ExploreS.pas:61

    // ExploreS.pas:370
    public override string ResourceEditName() => "";

}

public enum TDefendersMode : byte
{
    dmUserDefined = 0,
    dmLow = 1,
    dmNormal = 2,
    dmHigh = 3,
    dmRandom = 4
}

public static partial class ExploreSUnit
{
    // ExploreS.pas:385
    public static void RegisterExplorationSite(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TExplorationSite>(), EClass.Of<TExplorationSiteResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
