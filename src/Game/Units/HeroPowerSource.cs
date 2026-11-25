#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Units;

// Hero
public class HeroPowerSource(CustomNode? owner) : PowerSource(owner)
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
        int x = coordinate27.X;
        int y = coordinate27.Y;
        int l = coordinate27.MapLevel;
        if (x != -1)
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            ArmyHexSprite armyHexSprite = strategicMap.MapContainer.GetField(x, y, l).GetArmyHexSprite();
            if (armyHexSprite is not null)
            {
                strategicMap.Select(armyHexSprite, center: true);
            }
        }
    }
}
