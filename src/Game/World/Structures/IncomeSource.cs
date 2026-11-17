// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

public partial class IncomeSource : EngineObject
{
    public IncomeSource(CustomNode owner) : base(owner)
    { }

    // Player.PAS:202
    public virtual string Name() => "";

    // Player.PAS:207
    public virtual int Income() => 0;

    // Player.PAS:212
    public virtual MapCoordinate GetLocation() => MapCoordinate.None;

    // Player.PAS:219
    public virtual void CenterToLocation()
    {
        MapCoordinate coordinate33 = GetLocation();
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
