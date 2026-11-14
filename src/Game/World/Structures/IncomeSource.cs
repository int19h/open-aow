// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

public partial class IncomeSource : EngineObject
{
    public IncomeSource(ECustomNode owner) : base(owner)
    { }

    // Player.PAS:202
    public virtual string Name() => "";

    // Player.PAS:207
    public virtual int Income() => 0;

    // Player.PAS:212
    public virtual void GetLocation(out sbyte X, out sbyte Y, out sbyte L)
    {
        X = -1;
        Y = -1;
        L = -1;
    }

    // Player.PAS:219
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
}
