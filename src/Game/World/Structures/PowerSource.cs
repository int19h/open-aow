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
    public virtual void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = -1;
        Y = -1;
        L = -1;
    }

    // Spells.PAS:1796
    public virtual void CenterToLocation()
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        GetLocation(ref X, ref Y, ref L);
        if (X != -1)
        {
            StrategicMap.Current.CenterViewTo(X, Y, L);
            StrategicMap.Current.PlayCenterAnimation(X, Y, L);
        }
    }

    // Spells.PAS:1826
    // AoW.exe
    public virtual void Select()
    {
    }
}
