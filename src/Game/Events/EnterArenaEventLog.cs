// Translated from Arena.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Events;

// FArena, FSelection, FTrainable, FUnitIDs
public partial class EnterArenaEventLog : EventLog
{
    // Arena.pas:259
    public EnterArenaEventLog(ECustomNode owner) : base(owner)
    {
        FUnitIDs = new Int32List();
    }

    public Arena FArena;
    public byte FSelection;
    public byte FTrainable;
    public Int32List FUnitIDs;

    // Arena.pas:265
    public override void Dispose()
    {
        FUnitIDs?.Dispose();
        base.Dispose();
    }

    // Arena.pas:271
    public int GetCount() => FUnitIDs.Count;

    // Arena.pas:274
    public Troop GetUnit(int Index)
    {
        Troop Result;
        if (Index >= 0 && Index < FUnitIDs.Count)
        {
            var unitID = FUnitIDs[Index];
            Result = AoWHSMap.Current.FUnitControl.FindUnit(unitID);
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
        Int32List Units;
        Units = new Int32List();
        for (int I = 0, last = FUnitIDs.Count - 1; I <= last; I++)
        {
            if (((1 << I) & FSelection) != 0)
            {
                Units.Add(FUnitIDs[I]);
            }
        }

        bool Result = FArena.TrainUnits(AoWHSMap.Current.FSeatedPlayer, Units, ref ErrorText);
        Units?.Dispose();
        return Result;
    }

    // Arena.pas:294
    public bool CanTrain(ref string ErrorText)
    {
        Int32List Units;
        Units = new Int32List();
        for (int I = 0, last = FUnitIDs.Count - 1; I <= last; I++)
        {
            if (((1 << I) & FSelection) != 0)
            {
                Units.Add(FUnitIDs[I]);
            }
        }

        bool Result = FArena.CanTrainUnits(AoWHSMap.Current.FSeatedPlayer, Units, ref ErrorText);
        Units?.Dispose();
        return Result;
    }

    // Arena.pas:304
    public string GetSelectionInfo()
    {
        string Result;
        int Total;
        Troop AUnit;
        Total = 0;
        for (int I = 0, last = FUnitIDs.Count - 1; I <= last; I++)
        {
            if (((1 << I) & FSelection) != 0)
            {
                var unitID = FUnitIDs[I];
                AUnit = AoWHSMap.Current.FUnitControl.FindUnit(unitID);
                if (AUnit != null)
                {
                    Total += FArena.UnitTrainCost(AUnit);
                }
            }
        }

        if (Total != 0)
        {
            Result = RtlUnit.Format(AbilityRules.TranslateRStr(TotalCostXRStr), [IntToStr(Total)]);
        }
        else
        {
            Result = "";
        }

        return Result;
    }

    private static readonly string TotalCostXRStr = "Total Cost: %s";

}
