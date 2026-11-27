#nullable disable warnings
// Translated from Arena.pas.
using System.Globalization;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Structures;
using static Aow.Rtl.DelphiFormat;

namespace Aow.Game.Events;

// Arena, Selection, Trainable, UnitIds
public partial class EnterArenaEventLog(CustomNode? owner, EngineObject holder) : EventLog(owner, holder: holder)
{

    public Arena Arena { get; init; }
    public int Selection { get; private set; }
    public int Trainable { get; set; }
    public Int32List UnitIds { get; } = [];

    // Arena.pas:265
    public override void Dispose() => base.Dispose();

    // Arena.pas:271
    public int GetCount() => UnitIds.Count;

    // Arena.pas:274
    public Troop? this[int index]
    {
        get
        {
            if (index >= 0 && index < UnitIds.Count)
            {
                int unitId = UnitIds[index];
                return GetEngine().StrategicMap.UnitControl.FindUnit(unitId);
            }
            else
            {
                return null;
            }
        }
    }

    // Arena.pas:281
    public void SetSelection(int value) => Selection = value & Trainable;

    // Arena.pas:284
    public bool Train(out string errorText)
    {
        Int32List units = [];
        for (int i = 0, last = UnitIds.Count - 1; i <= last; i++)
        {
            if (((1 << i) & Selection) != 0)
            {
                _ = units.Add(UnitIds[i]);
            }
        }

        bool result = Arena.TrainUnits(GetEngine().StrategicMap.SeatedPlayer, units, out errorText);
        return result;
    }

    // Arena.pas:294
    public bool CanTrain(ref string errorText)
    {
        Int32List units = [];
        for (int i = 0, last = UnitIds.Count - 1; i <= last; i++)
        {
            if (((1 << i) & Selection) != 0)
            {
                _ = units.Add(UnitIds[i]);
            }
        }

        bool result = Arena.CanTrainUnits(GetEngine().StrategicMap.SeatedPlayer, units, ref errorText);
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
                Troop? troop = GetEngine().StrategicMap.UnitControl.FindUnit(unitId);
                if (troop is not null)
                {
                    total += Arena.UnitTrainCost(troop);
                }
            }
        }

        return total != 0 ? Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_totalCostFormat), args: [total.ToString(CultureInfo.InvariantCulture)]) : "";
    }

    private static readonly string s_totalCostFormat = "Total Cost: %s";

}
