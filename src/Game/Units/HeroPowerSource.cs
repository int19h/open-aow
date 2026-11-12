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
    public HeroPowerSource(ECustomNode owner) : base(owner)
    { }

    public Hero Hero { get; set; }

    // Hero.PAS:587
    public override string Name() => Hero.HeroName;

    // Hero.PAS:592
    public override int Power() => Hero.GetPowerGeneration();

    // Hero.PAS:610
    public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        Hero.GetLocation(ref X, ref Y, ref L);
    }

    // Hero.PAS:597
    public override void Select()
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        ArmyHexSprite armyHexSprite;
        GetLocation(ref X, ref Y, ref L);
        if (X != -1)
        {
            armyHexSprite = StrategicMap.Current.MapContainer.GetField(X, Y, L).GetArmyHexSprite();
            if (armyHexSprite != null)
            {
                StrategicMap.Current.Select(armyHexSprite, true);
            }
        }
    }

    // Engine.pas:524

}
