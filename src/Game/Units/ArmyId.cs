#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// UnitIds. Player. FX, FY, FL
public sealed partial class ArmyId(CustomNode owner) : EngineObject(owner)
{

    public new const int ClassId = 131_616;

    public Int32List UnitIds { get; } = new Int32List();

    public sbyte Player { get; set; }

    public MapCoordinate Location { get; set; }

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
        if (Location.X != -1)
        {
            ArmyHexSprite armyHexSprite = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).GetArmyHexSprite();
            if (armyHexSprite is not null && armyHexSprite.Army.Player == Player)
            {
                int i = UnitIds.Count;
                result = true;
                while (result && i != 0)
                {
                    i -= 1;
                    int item = UnitIds[i];
                    result = armyHexSprite.Army.Units.IndexOfId(item) != -1;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // Army.PAS:45
    public bool RetrieveArmy(ref ArmyHexSprite armyHexSprite, ref byte selection)
    {
        bool result;
        if (Location.X != -1)
        {
            selection = 0;
            armyHexSprite = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).GetArmyHexSprite();
            if (armyHexSprite is not null && armyHexSprite.Army.Player == Player)
            {
                int i = UnitIds.Count;
                result = true;
                while (result && i != 0)
                {
                    i -= 1;
                    int item = UnitIds[i];
                    int index = armyHexSprite.Army.Units.IndexOfId(item);
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
}
