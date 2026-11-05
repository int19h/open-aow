// Translated from Arena.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.NetworkE;
using Aow.Units.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Arena;

// FUnitIDs
public partial class TArenaTE : TStructureTE
{
    public TArenaTE(TECustomNode owner) : base(owner)
    {
        FUnitIDs = new TIntegerList();
    }

    // Arena.pas:86
    public new const int ClassID = 133294;

    public TIntegerList FUnitIDs;
    // Arena.pas:80
    public override void Destroy()
    {
        FUnitIDs.Free();
        base.Destroy();
    }

    // Arena.pas:89
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(40, FUnitIDs.LoadFromStream, FUnitIDs.SaveToStream);
    }

    // Arena.pas:74


    // Arena.pas:23


}

public partial class TArena : TStructure
{
    public TArena(TECustomNode owner) : base(owner)
    {}

    // Arena.pas:95
    public new const int ClassID = 133276;

    // Arena.pas:212
    public override TStructureTE CreateTE() => new TArenaTE(null);

    // Arena.pas:135
    public override TExecuteResult ExecuteTE(TStructureTE TE, TTokenExecuter Executer)
    {
        TAbstractUnit AUnit;
        string ErrorText = "";
        TExecuteResult Result = base.ExecuteTE(TE, Executer);
        if (TE.FAction == 1)
        {
            if (CanTrainUnits((sbyte)TE.FParam, ((TArenaTE)TE).FUnitIDs, ref ErrorText))
            {
                for (int I = 0, last = ((TArenaTE)TE).FUnitIDs.Count - 1; I <= last; I++)
                {
                    var unitID = ((TArenaTE)TE).FUnitIDs[I];
                    AUnit = AoWHSMap.FUnitControl.FindUnit(unitID);
                    if (AUnit != null)
                    {
                        var player = AoWHSMap.FPlayers.GetPlayers(TE.FParam);
                        var unitTrainCost = UnitTrainCost(AUnit);
                        AoWHSMap.FPlayers.GetPlayers(TE.FParam).SetGems(player.FGems - unitTrainCost);
                        ExecuteTrainUnit(AUnit);
                    }
                }

                if (AoWHSMap.FSeatedPlayer == TE.FParam)
                {
                    var arenaTE = (TArenaTE)TE;
                    if (arenaTE.FUnitIDs.Count == 1)
                    {
                        var oneUnitTrained = TranslateRStr(OneUnitTrainedRStr);
                        AoWHSMap.ShowMessage(oneUnitTrained);
                    }
                    else
                    {
                        var format = Rtl.Rtl.Format(TranslateRStr(XUnitsTrainedRStr), [IntToStr(arenaTE.FUnitIDs.Count)]);
                        AoWHSMap.ShowMessage(format);
                    }
                }
            }
        }

        return Result;
    }

    // Arena.pas:98
    public override bool CanDblClick() => CanEnter(AoWHSMap.FSeatedPlayer);

    // Arena.pas:101
    public int UnitTrainCost(TAbstractUnit AUnit)
    {
        int Result;
        if (AUnit is THero)
        {
            // { The original rejects heroes, although ExecuteTrainUnit supports them. }
            return -1;
        }

        if (AUnit is TUnit)
        {
            var unit = (TUnit)AUnit;
            if (unit.GetRank() != TUnitRank.urGold)
            {
                Result = (unit.GetNextLevelExperience() - AUnit.GetExperience()) * 5;
            }
            else
            {
                Result = -1;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Arena.pas:116
    public void ExecuteTrainUnit(TAbstractUnit AUnit)
    {
        if (AUnit is THero)
        {
            var hero = (THero)AUnit;
            hero.SetExperience(hero.GetNextLevelExperience());
            hero.Changed();

            return;
        }

        if (AUnit is TUnit)
        {
            var unit = (TUnit)AUnit;
            unit.SetExperience(unit.GetNextLevelExperience());
            unit.Changed();
        }
    }

    // Arena.pas:159
    public bool CanTrainUnits(sbyte Player, TIntegerList Units, ref string ErrorText)
    {
        bool Result;
        int Total;
        int I;
        int Cost;
        TAbstractUnit AUnit;
        if (Units.Count != 0)
        {
            Total = 0;
            I = Units.Count;
            Result = true;
            while (Result && I != 0)
            {
                I -= 1;
                var unit = Units[I];
                AUnit = AoWHSMap.FUnitControl.FindUnit(unit);
                if (AUnit != null)
                {
                    Cost = UnitTrainCost(AUnit);
                    if (Cost != -1)
                    {
                        Total += Cost;
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
            }

            if (Result)
            {
                if (AoWHSMap.FPlayers.GetPlayers(Player).FGems >= Total)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                    ErrorText = TranslateRStr(NotEnoughGoldRStr);
                }
            }
        }
        else
        {
            Result = false;
            ErrorText = TranslateRStr(NoUnitSelectedRStr);
        }

        return Result;
    }

    // Arena.pas:194
    public bool TrainUnits(sbyte Player, TIntegerList Units, ref string ErrorText)
    {
        bool Result;
        TArenaTE TE;
        ErrorText = "";
        if (CanTrainUnits(Player, Units, ref ErrorText) && !AoWHSMap.FPlayers.GetPlayers(Player).GetBusy())
        {
            TE = (TArenaTE)CreateTE();
            SetupTE(TE);
            TE.FUnitIDs.Assign(Units);
            TE.FParam = Player;
            TE.FAction = 1;
            AoWHSMap.FTokenManager.GetTokenControl(Player).AddEvent(TE);
            TE.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Arena.pas:215
    public bool CanEnter(sbyte Player)
    {
        bool Result;
        TArmyHS Army;
        Army = (TArmyHS)FOwner.FindOwnedChild(131607);
        if (Army == null || Army.FArmy.FPlayer != Player)
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Arena.pas:223
    public bool Enter()
    {
        bool Result = default;
        TEnterArenaEventLog Log;
        TArmyHS Army;
        if (CanEnter(AoWHSMap.FSeatedPlayer))
        {
            Log = new TEnterArenaEventLog(null);
            Log.FArena = this;
            Army = (TArmyHS)FOwner.FindOwnedChild(131607);
            for (int I = 0, last = Army.FArmy.Count - 1; I <= last; I++)
            {
                var unit = Army.FArmy.GetUnit(I);
                Log.FUnitIDs.Add(unit.FUnitID);
                if (UnitTrainCost(Army.FArmy.GetUnit(I)) != -1)
                {
                    Log.FTrainable = (byte)(Log.FTrainable | (1 << I));
                }
            }

            Log.Execute(true, TExecuteTrigger.etUser, null);
            Log.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

}

public partial class TArenaResource : TStructureResource
{
    public TArenaResource(TECustomNode owner) : base(owner)
    {
        FCastsShadow = true;
    }

    // Arena.pas:253
    public new const int ClassID = 133277;

    // Arena.pas:244


    // Arena.pas:256
    public override int ResourceUserClassID() => 133276;

    // Arena.pas:45


    // Arena.pas:250
    public override string ResourceEditName() => TranslateRStr(ArenaRStr);

}

// FArena, FSelection, FTrainable, FUnitIDs
public partial class TEnterArenaEventLog : TEventLog
{
    // Arena.pas:259
    public TEnterArenaEventLog(TECustomNode owner) : base(owner)
    {
        FUnitIDs = new TIntegerList();
    }

    public TArena FArena;
    public byte FSelection;
    public byte FTrainable;
    public TIntegerList FUnitIDs;
    // Arena.pas:265
    public override void Destroy()
    {
        FUnitIDs.Free();
        base.Destroy();
    }

    // Arena.pas:271
    public int GetCount() => FUnitIDs.Count;

    // Arena.pas:274
    public TAbstractUnit GetUnit(int Index)
    {
        TAbstractUnit Result;
        if (Index >= 0 && Index < FUnitIDs.Count)
        {
            var unitID = FUnitIDs[Index];
            Result = AoWHSMap.FUnitControl.FindUnit(unitID);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Arena.pas:281
    public void SetSelection(byte Value)
    {
        FSelection = (byte)(Value & FTrainable);
    }

    // Arena.pas:284
    public bool Train(ref string ErrorText)
    {
        TIntegerList Units;
        Units = new TIntegerList();
        for (int I = 0, last = FUnitIDs.Count - 1; I <= last; I++)
        {
            if (((1 << I) & FSelection) != 0)
            {
                Units.Add(FUnitIDs[I]);
            }
        }

        bool Result = FArena.TrainUnits(AoWHSMap.FSeatedPlayer, Units, ref ErrorText);
        Units.Free();
        return Result;
    }

    // Arena.pas:294
    public bool CanTrain(ref string ErrorText)
    {
        TIntegerList Units;
        Units = new TIntegerList();
        for (int I = 0, last = FUnitIDs.Count - 1; I <= last; I++)
        {
            if (((1 << I) & FSelection) != 0)
            {
                Units.Add(FUnitIDs[I]);
            }
        }

        bool Result = FArena.CanTrainUnits(AoWHSMap.FSeatedPlayer, Units, ref ErrorText);
        Units.Free();
        return Result;
    }

    // Arena.pas:304
    public string GetSelectionInfo()
    {
        string Result;
        int Total;
        TAbstractUnit AUnit;
        Total = 0;
        for (int I = 0, last = FUnitIDs.Count - 1; I <= last; I++)
        {
            if (((1 << I) & FSelection) != 0)
            {
                var unitID = FUnitIDs[I];
                AUnit = AoWHSMap.FUnitControl.FindUnit(unitID);
                if (AUnit != null)
                {
                    Total += FArena.UnitTrainCost(AUnit);
                }
            }
        }

        if (Total != 0)
        {
            Result = Rtl.Rtl.Format(TranslateRStr(TotalCostXRStr), [IntToStr(Total)]);
        }
        else
        {
            Result = "";
        }

        return Result;
    }

}

public static partial class Arena
{
    // Arena.pas:318
    public static void RegisterArena(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TArena>(), EClass.Of<TArenaTE>(), EClass.Of<TArenaResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
