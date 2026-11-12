// Translated from Arena.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Events;

// Arena, Selection, Trainable, UnitIds
public partial class EnterArenaEventLog : EventLog
{
    // Arena.pas:259
    public EnterArenaEventLog(ECustomNode owner) : base(owner)
    {
        UnitIds = new Int32List();
    }

    public Arena Arena;
    public byte Selection;
    public byte Trainable;
    public Int32List UnitIds;

    // Arena.pas:265
    public override void Dispose()
    {
        UnitIds?.Dispose();
        base.Dispose();
    }

    // Arena.pas:271
    public int GetCount() => UnitIds.Count;

    // Arena.pas:274
    public Troop GetUnit(int index)
    {
        Troop result;
        if (index >= 0 && index < UnitIds.Count)
        {
            var unitId = UnitIds[index];
            result = AoWHSMap.Current.UnitControl.FindUnit(unitId);
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Arena.pas:281
    public void SetSelection(byte value)
    {
        Selection = (byte)(value & Trainable);
    }

    // Arena.pas:284
    public bool Train(ref string errorText)
    {
        Int32List units;
        units = new Int32List();
        for (int I = 0, last = UnitIds.Count - 1; I <= last; I++)
        {
            if (((1 << I) & Selection) != 0)
            {
                units.Add(UnitIds[I]);
            }
        }

        bool result = Arena.TrainUnits(AoWHSMap.Current.SeatedPlayer, units, ref errorText);
        units?.Dispose();
        return result;
    }

    // Arena.pas:294
    public bool CanTrain(ref string errorText)
    {
        Int32List units;
        units = new Int32List();
        for (int I = 0, last = UnitIds.Count - 1; I <= last; I++)
        {
            if (((1 << I) & Selection) != 0)
            {
                units.Add(UnitIds[I]);
            }
        }

        bool result = Arena.CanTrainUnits(AoWHSMap.Current.SeatedPlayer, units, ref errorText);
        units?.Dispose();
        return result;
    }

    // Arena.pas:304
    public string GetSelectionInfo()
    {
        string result;
        int total;
        Troop troop;
        total = 0;
        for (int I = 0, last = UnitIds.Count - 1; I <= last; I++)
        {
            if (((1 << I) & Selection) != 0)
            {
                var unitId = UnitIds[I];
                troop = AoWHSMap.Current.UnitControl.FindUnit(unitId);
                if (troop != null)
                {
                    total += Arena.UnitTrainCost(troop);
                }
            }
        }

        if (total != 0)
        {
            result = RtlUnit.Format(AbilityRules.TranslateResourceString(s_totalCostFormat), [IntToStr(total)]);
        }
        else
        {
            result = "";
        }

        return result;
    }

    private static readonly string s_totalCostFormat = "Total Cost: %s";

}
