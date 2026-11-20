#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Units;

// Hero
public class HeroPowerSource(CustomNode owner) : PowerSource(owner)
{
    public Hero Hero { get; init; }

    // Hero.PAS:587
    public override string Name() => Hero.HeroName;

    // Hero.PAS:592
    public override int Power() => Hero.GetPowerGeneration();

    // Hero.PAS:610
    public override MapCoordinate GetLocation() => Hero.GetLocation();

    // Hero.PAS:597
    public override void Select()
    {
        MapCoordinate coordinate27 = GetLocation();
        sbyte x = coordinate27.X;
        sbyte y = coordinate27.Y;
        sbyte l = coordinate27.MapLevel;
        if (x != -1)
        {
            ArmyHexSprite armyHexSprite = StrategicMap.Current.MapContainer.GetField(x, y, l).GetArmyHexSprite();
            if (armyHexSprite is not null)
            {
                StrategicMap.Current.Select(armyHexSprite, true);
            }
        }
    }
}
