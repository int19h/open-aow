// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Units;

// FNextUnitID. FUnitTable. FUnits
public partial class UnitControl : EObject
{
    // Unit.PAS:1
    public UnitControl(ECustomNode owner) : base(owner)
    {
        FUnitTable = new IntegerHashTable256();
        FUnits = new DelphiList();
        FNextUnitID = 1;
    }

    public int FNextUnitID;
    public IntegerHashTable256 FUnitTable;
    public DelphiList FUnits;

    // Unit.PAS:8
    public override void Dispose()
    {
        FUnits?.Dispose();
        FUnitTable?.Dispose();
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
            ((AbilityOwner)FUnits.List[I]).Changed();
        }
    }

    // Unit.PAS:31
    public AbstractUnit GetUnit(int Index) => (AbstractUnit)FUnits.List[Index];

    // Unit.PAS:35
    public AbstractUnit FindUnit(int ID) => (AbstractUnit)FUnitTable.Find(ID);

    // Null when the id is missing or the stored unit is not a T.
    public T FindUnit<T>(int id) where T : AbstractUnit => FUnitTable.Find(id) is T found ? found : null;

    // Unit.PAS:39
    public int GetFreeID()
    {
        while (FUnitTable.Find(FNextUnitID) != null)
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
    public void RegisterUnit(AbstractUnit AUnit)
    {
        AbstractUnit ExistingUnit;
        if (!AUnit.FUnitStatus.Contains(UnitStatusElement.Registered))
        {
            if (AUnit.FUnitID == -1)
            {
                AUnit.FUnitID = GetFreeID();
                AUnit.FUnitStatus = AUnit.FUnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Registered);
                FUnitTable.Add(AUnit, AUnit.FUnitID);
                FUnits.Add(AUnit);
            }
            else
            {
                ExistingUnit = (AbstractUnit)FUnitTable.Find(AUnit.FUnitID);
                if (ExistingUnit != AUnit)
                {
                    FUnits.Add(AUnit);
                    AUnit.FUnitStatus = AUnit.FUnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.Registered);
                    if (ExistingUnit == null)
                    {
                        FUnitTable.Add(AUnit, AUnit.FUnitID);
                    }
                    else
                    {
                        if (AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
                        {
                            AUnit.FUnitID = GetFreeID();
                            FUnitTable.Add(AUnit, AUnit.FUnitID);
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
                                FUnitTable.Add(AUnit, AUnit.FUnitID);
                            }
                        }
                    }
                }
            }
        }
    }

    // Unit.PAS:86
    public void UnRegisterUnit(AbstractUnit AUnit)
    {
        if (AUnit.FUnitStatus.Contains(UnitStatusElement.Registered))
        {
            AUnit.FUnitStatus = AUnit.FUnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Registered);
            FUnitTable.DeleteSafe(AUnit, AUnit.FUnitID);
            FUnits.Remove(AUnit);
        }
    }

    // intf_P2_05.inc:21

}
