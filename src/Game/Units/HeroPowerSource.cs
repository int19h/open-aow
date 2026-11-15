// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Units;

// Hero
public partial class HeroPowerSource : PowerSource
{
    public HeroPowerSource(CustomNode owner) : base(owner)
    { }

    public Hero Hero { get; set; }

    // Hero.PAS:587
    public override string Name() => Hero.HeroName;

    // Hero.PAS:592
    public override int Power() => Hero.GetPowerGeneration();

    // Hero.PAS:610
    public override void GetLocation(out sbyte X, out sbyte Y, out sbyte L)
    {
        Hero.GetLocation(out X, out Y, out L);
    }

    // Hero.PAS:597
    public override void Select()
    {
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        ArmyHexSprite armyHexSprite;
        GetLocation(out x, out y, out l);
        if (x != -1)
        {
            armyHexSprite = StrategicMap.Current.MapContainer.GetField(x, y, l).GetArmyHexSprite();
            if (armyHexSprite != null)
            {
                StrategicMap.Current.Select(armyHexSprite, true);
            }
        }
    }

    // Engine.pas:524

}
