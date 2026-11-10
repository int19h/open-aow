// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// FUnitIDs. FPlayer. FX, FY, FL
public partial class ArmyID : EObject
{
    public new const int ClassID = 131616;

    // Army.PAS:1
    public ArmyID(ECustomNode owner) : base(owner)
    {
        FUnitIDs = new IntegerList();
    }

    public IntegerList FUnitIDs;
    public sbyte FPlayer;
    public sbyte FX;
    public sbyte FY;
    public sbyte FL;
    public byte F_ArmyID_13;

    // Army.PAS:7
    public override void Dispose()
    {
        FUnitIDs?.Dispose();
        base.Dispose();
    }

    // Army.PAS:24
    public bool Valid()
    {
        bool Result;
        ArmyHS ArmyHS;
        int I;
        if (FX != -1)
        {
            ArmyHS = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(FX, FY, FL)).GetArmyHS();
            if (ArmyHS != null && ArmyHS.FArmy.FPlayer == FPlayer)
            {
                I = FUnitIDs.Count;
                Result = true;
                while (Result && I != 0)
                {
                    I -= 1;
                    var item = FUnitIDs[I];
                    Result = ArmyHS.FArmy.IndexOfID(item) != -1;
                }
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

        return Result;
    }

    // Army.PAS:45
    public bool RetrieveArmy(ref ArmyHS ArmyHS, ref byte Selection)
    {
        bool Result;
        int I;
        int Index;
        if (FX != -1)
        {
            Selection = 0;
            ArmyHS = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(FX, FY, FL)).GetArmyHS();
            if (ArmyHS != null && ArmyHS.FArmy.FPlayer == FPlayer)
            {
                I = FUnitIDs.Count;
                Result = true;
                while (Result && I != 0)
                {
                    I -= 1;
                    var item = FUnitIDs[I];
                    Index = ArmyHS.FArmy.IndexOfID(item);
                    // { Original shifts by the missing unit index before reporting failure. }
                    Selection = (byte)(Selection | (1 << Index));
                    Result = Index != -1;
                }
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

        if (!Result)
        {
            ArmyHS = null;
            Selection = 0;
        }

        return Result;
    }

    // intf_P2_07.inc:411

}
