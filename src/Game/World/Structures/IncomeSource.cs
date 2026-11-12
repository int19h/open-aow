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
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        GetLocation(out X, out Y, out L);
        if (X != -1)
        {
            StrategicMap.Current.CenterViewTo(X, Y, L);
            StrategicMap.Current.PlayCenterAnimation(X, Y, L);
        }
    }
}
