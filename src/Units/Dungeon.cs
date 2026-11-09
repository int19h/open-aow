// Translated from Dungeon.pas.
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.EventLog;
using Aow.Units.ExploreS;
using Aow.Units.NetworkE;
using Aow.Units.Sound;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.EventLog.EventLogUnit;

namespace Aow.Units.Dungeon;

// { ot=1 }
public partial class TPrisonersArmy : TArmy
{
    public TPrisonersArmy(TECustomNode owner) : base(owner)
    { }

    // Dungeon.pas:89
    public override bool CanAddUnit(TAbstractUnit AUnit)
    {
        bool Result = base.CanAddUnit(AUnit);
        return Result && !AUnit.GetTransporter() && AUnit.ValidTerrain(1, 4);
    }

    // intf_P2_07.inc:445

}

// FPrisoners, FPrisonersMode
public partial class TDungeon : TExplorationSite
{
    public TDungeon(TECustomNode owner) : base(owner)
    {
        FPrisonersMode = TPrisonersMode.pmNormal;
        FPrisoners = new TPrisonersArmy(null);
    }

    // Dungeon.pas:108
    public new const int ClassID = 131882;

    public TPrisonersArmy FPrisoners;
    public TPrisonersMode FPrisonersMode;

    // Dungeon.pas:102
    public override void Dispose()
    {
        base.Dispose();
        FPrisoners?.Dispose();
    }

    // Dungeon.pas:114
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FPrisoners, 8);
        Stream.rwData(Bytes.Of(ref FPrisonersMode, 1), 1, 9);
    }

    // Dungeon.pas:95

    // Dungeon.pas:249
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        FPrisoners.MsgProc(ref Msg, Sender);
    }

    // Dungeon.pas:237
    public override void Activate()
    {
        base.Activate();
        FPrisoners.Activate();
    }

    // Dungeon.pas:243
    public override void Deactivate()
    {
        FPrisoners.Deactivate();
        base.Deactivate();
    }

    // Dungeon.pas:255
    public override TExecuteResult ExecuteTE(TStructureTE TE, TTokenExecuter Executer)
    {
        TExecuteResult Result = base.ExecuteTE(TE, Executer);
        if (TE.FAction == 1)
        {
            LiberatePrisoners((sbyte)TE.FParam);
            Result = TExecuteResult.erDone;
        }

        return Result;
    }

    // Dungeon.pas:231
    public override void NewDay()
    {
        base.NewDay();
        if (AoWHSMap.FDay == 1)
        {
            GeneratePrisoners();
        }
    }

    // Dungeon.pas:111
    public override bool GetExplored() => FPrisoners.Count == 0;

    // Dungeon.pas:121
    public override void SetupCombat(TCombat Combat, TArmyHS Army)
    {
        base.SetupCombat(Combat, Army);
        Combat.FItems = FPrisoners;
    }

    // Dungeon.pas:127
    public override void ExecuteSearchDone(TCombatType CombatType, TCombatResult CombatResult)
    {
        TArmyHS Army;
        TPrisonersJoinEventLog Log;
        if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crNoDefenders).Contains(CombatResult))
        {
            Army = (TArmyHS)Owner.FindOwnedChild(131607);
            Log = new TPrisonersJoinEventLog(null);
            Log.FX = GetXhx();
            Log.FY = GetYhx();
            Log.FL = GetLhx();
            Log.FPlayer = Army.FArmy.FPlayer;
            Log.FSelection = 255;
            Log.FGold = 0;
            Log.FJoinMode = TJoinMode.jmDefault;
            Log.FFirstUnitID = FPrisoners.GetUnit(0).FUnitID;
            for (int I = 0, last = FPrisoners.Count - 1; I <= last; I++)
            {
                var unit = FPrisoners.GetUnit(I);
                Log.FUnitIDs.Add(unit.FUnitID);
            }

            if (FPrisoners.Count == 1)
            {
                var liberatePrisonerMessage = TranslateRStr(LiberatePrisonerMessageRStr);
                Log.FJoinMessage.Text = liberatePrisonerMessage;
            }
            else
            {
                var liberatePrisonersMessage = TranslateRStr(LiberatePrisonersMessageRStr);
                Log.FJoinMessage.Text = liberatePrisonersMessage;
            }

            Log.Execute(false, TExecuteTrigger.etEvent, null);
            Log.Release();
        }
    }

    // Dungeon.pas:156
    public void GeneratePrisoners()
    {
        int Collection;
        TUnitIndexList List;
        TUnitIndexListCollection Collections;
        RandSeed = AoWHSMap.FDefendersSeed;
        if (FPrisonersMode == TPrisonersMode.pmRandom)
        {
            FPrisonersMode = (TPrisonersMode)(Random(3) + 1);
        }

        Collection = ((TDungeonResource)FResource).FPrisonerCollections.GetRndCollection();
        if (Collection != -1)
        {
            List = new TUnitIndexList();
            Collections = ((TDungeonResource)FResource).FPrisonerCollections;
            for (int I = Collections.GetCollectionCount(Collection) - 1; I >= 0; I--)
            {
                List.Add(Collections.GetU(Collection, I));
            }

            FillWithRandomUnits(List, FPrisoners, 4, (int)FPrisonersMode * 50, TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature, TUnitType.utMachine));
            List?.Dispose();
        }
    }

    // Dungeon.pas:178
    public void LiberatePrisoners(sbyte Player)
    {
        TLocationTextEventLog LocationLog;
        TArmyInfoEventLog ArmyLog;
        THero Hero;
        TArmyHS Army;
        LocationLog = new TLocationTextEventLog(null);
        LocationLog.FPlayer = GetPlayer();
        var name = GetName();
        LocationLog.FText = RtlUnit.Format(TranslateRStr(XExploredRStr), [name]);
        var lhx = GetLhx();
        var yhx = GetYhx();
        LocationLog.SetLocation(GetXhx(), yhx, lhx);
        var yhx2 = GetYhx();
        var lhx2 = GetLhx();
        DistributeLocationEventLog(LocationLog, Player, GetXhx(), yhx2, lhx2, false);
        LocationLog.Release();
        ArmyLog = new TArmyInfoEventLog(null);
        ArmyLog.FArmyView.Setup(FPrisoners, 255);
        var xhx = GetXhx();
        ArmyLog.FArmyView.FX = xhx;
        var yhx3 = GetYhx();
        ArmyLog.FArmyView.FY = yhx3;
        var lhx3 = GetLhx();
        ArmyLog.FArmyView.FL = lhx3;
        ArmyLog.FSelection = 255;
        ArmyLog.FPlayer = GetPlayer();
        ArmyLog.FTitle = TranslateRStr(PrisonerLiberationRStr);
        ArmyLog.FDescription = "";
        Hero = FPrisoners.Hero(255, TSet<THeroType>.Of(THeroType.htHero));
        if (Hero == null)
        {
            if (FPrisoners.Count == 1)
            {
                ArmyLog.FText = TranslateRStr(OnePrisonerLiberatedRStr);
                var name2 = AoWHSMap.FPlayers.GetPlayers(Player).GetName();
                var format = RtlUnit.Format(TranslateRStr(AfterLiberationOneUnitJoinedTheForcesOfXRStr), [name2]);
                ArmyLog.FMessages.Text = format;
            }
            else
            {
                var count = FPrisoners.Count;
                ArmyLog.FText = RtlUnit.Format(TranslateRStr(XPrisonersLiberatedRStr), [IntToStr(count)]);
                var fPrisonersCount = FPrisoners.Count;
                var name2 = AoWHSMap.FPlayers.GetPlayers(Player).GetName();
                var format = RtlUnit.Format(TranslateRStr(AfterLiberationXunitsJoinedTheForcesOfYRStr), [fPrisonersCount, name2]);
                ArmyLog.FMessages.Text = format;
            }
        }
        else
        {
            ArmyLog.FText = RtlUnit.Format(TranslateRStr(XLiberatedRStr), [Hero.FHeroName]);
            var name2 = AoWHSMap.FPlayers.GetPlayers(Player).GetName();
            var format = RtlUnit.Format(TranslateRStr(AfterLiberationXJoinedTheForcesOfYRStr), [Hero.FHeroName, name2]);
            ArmyLog.FMessages.Text = format;
        }

        var yhx4 = GetYhx();
        var lhx4 = GetLhx();
        DistributeLocationEventLog(ArmyLog, -1, GetXhx(), yhx4, lhx4, false);
        ArmyLog.Release();
        Army = new TArmyHS(null);
        Army.FArmy.BeginUpdate();
        Army.FArmy.SetPlayer(Player);
        Army.FArmy.AddUnits(FPrisoners);
        var lhx5 = GetLhx();
        var yhx5 = GetYhx();
        Army.PlaceEx(GetXhx(), yhx5, lhx5, TSet<TArmyPlaceModeElement>.Of(TArmyPlaceModeElement.apSafe, TArmyPlaceModeElement.apScanArea));
        Army.Activate();
        Army.FArmy.EndUpdate();
    }

    // Dungeon.pas:33

}

// FPrisonerCollections
public partial class TDungeonResource : TExplorationSiteResource
{
    public TDungeonResource(TECustomNode owner) : base(owner)
    {
        FPrisonerCollections = new TUnitIndexListCollection();
    }

    // Dungeon.pas:280
    public new const int ClassID = 131883;

    public TUnitIndexListCollection FPrisonerCollections;

    // Dungeon.pas:271
    public override void Dispose()
    {
        FPrisonerCollections?.Dispose();
        base.Dispose();
    }

    // Dungeon.pas:286
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(30, FPrisonerCollections.LoadFromStream, FPrisonerCollections.SaveToStream);
    }

    // Dungeon.pas:265

    // Dungeon.pas:283
    public override int ResourceUserClassID() => 131882;

    // Dungeon.pas:53

    // Dungeon.pas:277
    public override string ResourceEditName() => TranslateRStr(DungeonRStr);

}

// FX, FY, FL
public partial class TPrisonersJoinEventLog : TJoinEventLog
{
    public TPrisonersJoinEventLog(TECustomNode owner) : base(owner)
    { }

    public sbyte FX;
    public sbyte FY;
    public sbyte FL;

    // Dungeon.pas:292
    public override bool Accept(ref string ErrorText)
    {
        bool Result;
        TDungeon Dungeon;
        TStructureTE TE;
        Dungeon = (TDungeon)AoWHSMap.MapContainer.GetField(FX, FY, FL).FindOwnedChild(131882);
        if (Dungeon != null && !AoWHSMap.FPlayers.GetPlayers(FPlayer).GetBusy())
        {
            TE = Dungeon.CreateTE();
            Dungeon.SetupTE(TE);
            TE.FParam = FPlayer;
            TE.FAction = 1;
            AoWHSMap.FTokenManager.GetTokenControl(FPlayer).AddEvent(TE);
            TE.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Dungeon.pas:311
    public override bool Cancel()
    {
        TDungeon Dungeon;
        int Index;
        Dungeon = (TDungeon)AoWHSMap.MapContainer.GetField(FX, FY, FL).FindOwnedChild(131882);
        if (Dungeon != null)
        {
            for (int I = 0, last = Dungeon.FPrisoners.Count - 1; I <= last; I++)
            {
                var unitType = Dungeon.FPrisoners.GetUnit(I).GetUnitType();
                if (TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature).Contains(unitType))
                {
                    Index = Dungeon.FPrisoners.GetUnit(I).GetUnitGFXResourceIndex();
                    if (AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Index) != null)
                    {
                        var random = Random(15);
                        var unitGFXResource = AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Index);
                        unitGFXResource.FSFXLibraries[3].PlayEx(0, 100, false, random, TSamplePanning.spCenter, null);
                    }
                }
            }
        }

        return true;
    }

}

public enum TPrisonersMode : byte
{
    pmUserDefined = 0,
    pmLow = 1,
    pmNormal = 2,
    pmHigh = 3,
    pmRandom = 4
}

public static partial class DungeonUnit
{
    // Dungeon.pas:329
    public static void RegisterDungeon(TEngine Engine)
    {
        Engine.RegisterEClasses([EClass.Of<TDungeon>(), EClass.Of<TDungeonResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
