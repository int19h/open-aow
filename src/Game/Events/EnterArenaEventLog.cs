#nullable disable
// Translated from Arena.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiFormat;

namespace Aow.Game.Events;

// Arena, Selection, Trainable, UnitIds
public partial class EnterArenaEventLog(CustomNode owner) : EventLog(owner)
{

    public Arena Arena { get; init; }
    public byte Selection { get; private set; }
    public byte Trainable { get; set; }
    public Int32List UnitIds { get; } = new Int32List();

    // Arena.pas:265
    public override void Dispose()
    {
        UnitIds?.Dispose();
        base.Dispose();
    }

    // Arena.pas:271
    public int GetCount() => UnitIds.Count;

    // Arena.pas:274
    public Troop this[int index]
    {
        get
        {
            if (index >= 0 && index < UnitIds.Count)
            {
                int unitId = UnitIds[index];
                return StrategicMap.Current.UnitControl.FindUnit(unitId);
            }
            else
            {
                return null;
            }
        }
    }

    // Arena.pas:281
    public void SetSelection(byte value) => Selection = (byte)(value & Trainable);

    // Arena.pas:284
    public bool Train(out string errorText)
    {
        Int32List units = new();
        for (int i = 0, last = UnitIds.Count - 1; i <= last; i++)
        {
            if (((1 << i) & Selection) != 0)
            {
                _ = units.Add(UnitIds[i]);
            }
        }

        bool result = Arena.TrainUnits(StrategicMap.Current.SeatedPlayer, units, out errorText);
        units?.Dispose();
        return result;
    }

    // Arena.pas:294
    public bool CanTrain(ref string errorText)
    {
        Int32List units = new();
        for (int i = 0, last = UnitIds.Count - 1; i <= last; i++)
        {
            if (((1 << i) & Selection) != 0)
            {
                _ = units.Add(UnitIds[i]);
            }
        }

        bool result = Arena.CanTrainUnits(StrategicMap.Current.SeatedPlayer, units, ref errorText);
        units?.Dispose();
        return result;
    }

    // Arena.pas:304
    public string GetSelectionInfo()
    {
        int total = 0;
        for (int i = 0, last = UnitIds.Count - 1; i <= last; i++)
        {
            if (((1 << i) & Selection) != 0)
            {
                int unitId = UnitIds[i];
                Troop troop = StrategicMap.Current.UnitControl.FindUnit(unitId);
                if (troop is not null)
                {
                    total += Arena.UnitTrainCost(troop);
                }
            }
        }

        return total != 0 ? Format(AbilityRules.TranslateResourceString(GetEngine(), s_totalCostFormat), [IntToStr(total)]) : "";
    }

    private static readonly string s_totalCostFormat = "Total Cost: %s";

}
