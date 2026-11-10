// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Units;

// FHero
public partial class HeroPowerSource : PowerSource
{
    public HeroPowerSource(ECustomNode owner) : base(owner)
    { }

    public Hero FHero;

    // Hero.PAS:587
    public override string Name() => FHero.FHeroName;

    // Hero.PAS:592
    public override int Power() => FHero.GetPowerGeneration();

    // Hero.PAS:610
    public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        FHero.GetLocation(ref X, ref Y, ref L);
    }

    // Hero.PAS:597
    public override void Select()
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        ArmyHS ArmyHS;
        GetLocation(ref X, ref Y, ref L);
        if (X != -1)
        {
            ArmyHS = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L)).GetArmyHS();
            if (ArmyHS != null)
            {
                AoWHSMap.Current.Select(ArmyHS, true);
            }
        }
    }

    // Engine.pas:524

}
