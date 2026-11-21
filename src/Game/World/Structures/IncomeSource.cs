#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

public class IncomeSource(CustomNode owner) : EngineObject(owner)
{

    // Player.PAS:202
    public virtual string Name() => "";

    // Player.PAS:207
    public virtual int Income() => 0;

    // Player.PAS:212
    public virtual MapCoordinate Location => MapCoordinate.None;

    // Player.PAS:219
    public virtual void CenterToLocation()
    {
        MapCoordinate coordinate33 = Location;
        sbyte x = coordinate33.X;
        sbyte y = coordinate33.Y;
        sbyte l = coordinate33.MapLevel;
        if (x != -1)
        {
            StrategicMap.Current.CenterViewTo(x, y, l);
            StrategicMap.Current.PlayCenterAnimation(x, y, l);
        }
    }
}
