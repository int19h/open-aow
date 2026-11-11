// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Units;

// FNextUnitID. FUnitTable. FUnits
public partial class UnitControl : EngineObject
{
    // Unit.PAS:1
    public UnitControl(ECustomNode owner) : base(owner)
    {
        FUnitTable = new Dictionary<int, Troop>();
        FUnits = new List<Troop>();
        FNextUnitID = 1;
    }

    public int FNextUnitID;
    public Dictionary<int, Troop> FUnitTable;
    public List<Troop> FUnits;

    // Unit.PAS:8
    public override void Dispose()
    {
        FUnits?.Clear();
        base.Dispose();
    }

    // Unit.PAS:14
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FNextUnitID, 10);
        if (FNextUnitID <= 0)
        {
            FNextUnitID = 1;
        }
    }

    // Unit.PAS:20
    public int GetCount() => FUnits.Count;

    // Unit.PAS:24
    public void TriggerUnitsChanged()
    {
        for (int I = FUnits.Count - 1; I >= 0; I--)
        {
            (FUnits[I]).Changed();
        }
    }

    // Unit.PAS:31
    public Troop GetUnit(int Index) => FUnits[Index];

    // Unit.PAS:35
    public Troop FindUnit(int ID) => FUnitTable.TryGetValue(ID, out Troop found) ? found : null;

    // Null when the id is missing or the stored unit is not a T.
    public T FindUnit<T>(int id) where T : Troop =>
        FUnitTable.TryGetValue(id, out Troop found) && found is T typed ? typed : null;

    // Unit.PAS:39
    public int GetFreeID()
    {
        while (FUnitTable.ContainsKey(FNextUnitID))
        {
            FNextUnitID += 1;
            if (FNextUnitID > 268435455)
            {
                FNextUnitID = 1;
            }
        }

        return FNextUnitID;
    }

    // Unit.PAS:48
    public void RegisterUnit(Troop AUnit)
    {
        Troop ExistingUnit;
        if (!AUnit.FUnitStatus.Contains(UnitStatusElement.Registered))
        {
            if (AUnit.FUnitID == -1)
            {
                AUnit.FUnitID = GetFreeID();
                AUnit.FUnitStatus = AUnit.FUnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Registered);
                HashTables.Add(FUnitTable, key: AUnit.FUnitID, value: AUnit);
                FUnits.Add(AUnit);
            }
            else
            {
                ExistingUnit = FUnitTable.TryGetValue(AUnit.FUnitID, out Troop found) ? found : null;
                if (ExistingUnit != AUnit)
                {
                    FUnits.Add(AUnit);
                    AUnit.FUnitStatus = AUnit.FUnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Registered);
                    if (ExistingUnit == null)
                    {
                        HashTables.Add(FUnitTable, key: AUnit.FUnitID, value: AUnit);
                    }
                    else
                    {
                        if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                        {
                            AUnit.FUnitID = GetFreeID();
                            HashTables.Add(FUnitTable, key: AUnit.FUnitID, value: AUnit);
                        }
                        else
                        {
                            try
                            {
                                throw new InvalidOperationException("UnitID Failure");
                            }
                            catch
                            {
                                AUnit.FUnitID = GetFreeID();
                                HashTables.Add(FUnitTable, key: AUnit.FUnitID, value: AUnit);
                            }
                        }
                    }
                }
            }
        }
    }

    // Unit.PAS:86
    public void UnRegisterUnit(Troop AUnit)
    {
        if (AUnit.FUnitStatus.Contains(UnitStatusElement.Registered))
        {
            AUnit.FUnitStatus = AUnit.FUnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Registered);
            if (FUnitTable.TryGetValue(AUnit.FUnitID, out Troop stored) && stored == AUnit)
            {
                FUnitTable.Remove(AUnit.FUnitID);
            }

            FUnits.Remove(AUnit);
        }
    }

    // intf_P2_05.inc:21

}
