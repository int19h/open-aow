// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Units;

// NextUnitId. UnitTable. Units
public partial class UnitControl : EngineObject
{
    // Unit.PAS:1
    public UnitControl(ECustomNode owner) : base(owner)
    {
        UnitTable = new Dictionary<int, Troop>();
        Units = new List<Troop>();
        NextUnitId = 1;
    }

    public int NextUnitId;
    public Dictionary<int, Troop> UnitTable;
    public List<Troop> Units;

    // Unit.PAS:8
    public override void Dispose()
    {
        Units?.Clear();
        base.Dispose();
    }

    // Unit.PAS:14
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref NextUnitId, 10);
        if (NextUnitId <= 0)
        {
            NextUnitId = 1;
        }
    }

    // Unit.PAS:20
    public int GetCount() => Units.Count;

    // Unit.PAS:24
    public void TriggerUnitsChanged()
    {
        for (int I = Units.Count - 1; I >= 0; I--)
        {
            (Units[I]).Changed();
        }
    }

    // Unit.PAS:31
    public Troop GetUnit(int index) => Units[index];

    // Unit.PAS:35
    public Troop FindUnit(int id) => UnitTable.TryGetValue(id, out Troop found) ? found : null;

    // Null when the id is missing or the stored unit is not a T.
    public T FindUnit<T>(int id) where T : Troop =>
        UnitTable.TryGetValue(id, out Troop found) && found is T typed ? typed : null;

    // Unit.PAS:39
    public int GetFreeId()
    {
        while (UnitTable.ContainsKey(NextUnitId))
        {
            NextUnitId += 1;
            if (NextUnitId > 268435455)
            {
                NextUnitId = 1;
            }
        }

        return NextUnitId;
    }

    // Unit.PAS:48
    public void RegisterUnit(Troop troop)
    {
        Troop existingUnit;
        if (!troop.UnitStatus.Contains(UnitStatusElement.Registered))
        {
            if (troop.UnitId == -1)
            {
                troop.UnitId = GetFreeId();
                troop.UnitStatus = troop.UnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Registered);
                HashTables.Add(UnitTable, key: troop.UnitId, value: troop);
                Units.Add(troop);
            }
            else
            {
                existingUnit = UnitTable.TryGetValue(troop.UnitId, out Troop found) ? found : null;
                if (existingUnit != troop)
                {
                    Units.Add(troop);
                    troop.UnitStatus = troop.UnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Registered);
                    if (existingUnit == null)
                    {
                        HashTables.Add(UnitTable, key: troop.UnitId, value: troop);
                    }
                    else
                    {
                        if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                        {
                            troop.UnitId = GetFreeId();
                            HashTables.Add(UnitTable, key: troop.UnitId, value: troop);
                        }
                        else
                        {
                            try
                            {
                                throw new InvalidOperationException("UnitID Failure");
                            }
                            catch
                            {
                                troop.UnitId = GetFreeId();
                                HashTables.Add(UnitTable, key: troop.UnitId, value: troop);
                            }
                        }
                    }
                }
            }
        }
    }

    // Unit.PAS:86
    public void UnregisterUnit(Troop troop)
    {
        if (troop.UnitStatus.Contains(UnitStatusElement.Registered))
        {
            troop.UnitStatus = troop.UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Registered);
            if (UnitTable.TryGetValue(troop.UnitId, out Troop stored) && stored == troop)
            {
                UnitTable.Remove(troop.UnitId);
            }

            Units.Remove(troop);
        }
    }

    // intf_P2_05.inc:21

}
