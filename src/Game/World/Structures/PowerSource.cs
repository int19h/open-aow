#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

public class PowerSource(CustomNode owner) : EngineObject(owner)
{

    // Spells.PAS:1782
    public virtual string Name() => "";

    // Spells.PAS:1786
    public virtual int Power() => 0;

    // Spells.PAS:1790
    public virtual MapCoordinate GetLocation() => MapCoordinate.None;

    // Spells.PAS:1796
    public virtual void CenterToLocation()
    {
        MapCoordinate coordinate34 = GetLocation();
        int x = coordinate34.X;
        int y = coordinate34.Y;
        int l = coordinate34.MapLevel;
        if (x != -1)
        {
            GetEngine().StrategicMap.CenterViewTo(x, y, l);
            GetEngine().StrategicMap.PlayCenterAnimation(x, y, l);
        }
    }

    // Spells.PAS:1826
    // AoW.exe
    public virtual void Select()
    {
    }
}
