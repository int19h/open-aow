// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

public partial class PowerSource : EngineObject
{
    public PowerSource(ECustomNode owner) : base(owner)
    { }

    // Spells.PAS:1782
    public virtual string Name() => "";

    // Spells.PAS:1786
    public virtual int Power() => 0;

    // Spells.PAS:1790
    public virtual void GetLocation(out sbyte X, out sbyte Y, out sbyte L)
    {
        X = -1;
        Y = -1;
        L = -1;
    }

    // Spells.PAS:1796
    public virtual void CenterToLocation()
    {
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        GetLocation(out x, out y, out l);
        if (x != -1)
        {
            StrategicMap.Current.CenterViewTo(x, y, l);
            StrategicMap.Current.PlayCenterAnimation(x, y, l);
        }
    }

    // Spells.PAS:1826
    // AoW.exe
    public virtual void Select()
    {
    }
}
