// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// UnitIds. Player. FX, FY, FL
public partial class ArmyId : EngineObject
{
    public new const int ClassId = 131616;

    // Army.PAS:1
    public ArmyId(ECustomNode owner) : base(owner)
    {
        UnitIds = new Int32List();
    }

    public Int32List UnitIds { get; }

    public sbyte Player;

    public sbyte X { get; set; }
    public sbyte Y { get; set; }
    public sbyte MapLevel { get; set; }

    private byte _locationFlag;

    // Army.PAS:7
    public override void Dispose()
    {
        UnitIds?.Dispose();
        base.Dispose();
    }

    // Army.PAS:24
    public bool Valid()
    {
        bool result;
        ArmyHexSprite armyHexSprite;
        int I;
        if (X != -1)
        {
            armyHexSprite = StrategicMap.Current.MapContainer.GetField(X, Y, MapLevel).GetArmyHexSprite();
            if (armyHexSprite != null && armyHexSprite.Army.Player == Player)
            {
                I = UnitIds.Count;
                result = true;
                while (result && I != 0)
                {
                    I -= 1;
                    var item = UnitIds[I];
                    result = armyHexSprite.Army.IndexOfId(item) != -1;
                }
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Army.PAS:45
    public bool RetrieveArmy(ref ArmyHexSprite armyHexSprite, ref byte selection)
    {
        bool result;
        int I;
        int index;
        if (X != -1)
        {
            selection = 0;
            armyHexSprite = StrategicMap.Current.MapContainer.GetField(X, Y, MapLevel).GetArmyHexSprite();
            if (armyHexSprite != null && armyHexSprite.Army.Player == Player)
            {
                I = UnitIds.Count;
                result = true;
                while (result && I != 0)
                {
                    I -= 1;
                    var item = UnitIds[I];
                    index = armyHexSprite.Army.IndexOfId(item);
                    // { Original shifts by the missing unit index before reporting failure. }
                    selection = (byte)(selection | (1 << index));
                    result = index != -1;
                }
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }

        if (!result)
        {
            armyHexSprite = null;
            selection = 0;
        }

        return result;
    }

    // intf_P2_07.inc:411

}
