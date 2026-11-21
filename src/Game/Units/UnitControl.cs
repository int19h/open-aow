#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// NextUnitId. UnitTable. Units
public sealed class UnitControl(CustomNode? owner) : EngineObject(owner)
{

    private int _nextUnitId = 1;

    public Dictionary<int, Troop?> UnitTable { get; internal set; } = [];
    public List<Troop> Units { get; } = [];

    // Unit.PAS:8
    public override void Dispose()
    {
        Units?.Clear();
        base.Dispose();
    }

    // Unit.PAS:14
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _nextUnitId = persister.Int32("NextUnitId", value: _nextUnitId);
        if (_nextUnitId <= 0)
        {
            _nextUnitId = 1;
        }
    }

    // Unit.PAS:20
    public int GetCount() => Units.Count;

    // Unit.PAS:24
    public void TriggerUnitsChanged()
    {
        for (int i = Units.Count - 1; i >= 0; i--)
        {
            Units[i].Changed();
        }
    }

    // Unit.PAS:31
    public Troop this[int index] => Units[index];

    // Unit.PAS:35
    public Troop? FindUnit(int id) => UnitTable.TryGetValue(id, out Troop found) ? found : null;

    // Null when the id is missing or the stored unit is not a T.
    public T FindUnit<T>(int id) where T : Troop
        => UnitTable.TryGetValue(id, out Troop found) && found is T typed ? typed : null;

    // Unit.PAS:39
    public int GetFreeId()
    {
        while (UnitTable.ContainsKey(_nextUnitId))
        {
            _nextUnitId += 1;
            if (_nextUnitId > 268435455)
            {
                _nextUnitId = 1;
            }
        }

        return _nextUnitId;
    }

    // Unit.PAS:48
    public void RegisterUnit(Troop troop)
    {
        if (!troop.UnitStatus.Contains(UnitStatuses.Registered))
        {
            if (troop.UnitId == -1)
            {
                troop.UnitId = GetFreeId();
                troop.UnitStatus |= UnitStatuses.Registered;
                HashTables.Add(UnitTable, key: troop.UnitId, value: troop);
                Units.Add(troop);
            }
            else
            {
                Troop? existingUnit = UnitTable.TryGetValue(troop.UnitId, out Troop found) ? found : null;
                if (existingUnit != troop)
                {
                    Units.Add(troop);
                    troop.UnitStatus |= UnitStatuses.Registered;
                    if (existingUnit is null)
                    {
                        HashTables.Add(UnitTable, key: troop.UnitId, value: troop);
                    }
                    else
                    {
                        if (StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
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
        if (troop.UnitStatus.Contains(UnitStatuses.Registered))
        {
            troop.UnitStatus &= ~UnitStatuses.Registered;
            if (UnitTable.TryGetValue(troop.UnitId, out Troop stored) && stored == troop)
            {
                _ = UnitTable.Remove(troop.UnitId);
            }

            _ = Units.Remove(troop);
        }
    }
}
