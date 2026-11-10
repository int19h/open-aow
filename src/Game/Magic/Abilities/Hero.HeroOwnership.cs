using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities;

public partial class Hero
{
    // Hero.PAS:615
    public Hero(ECustomNode owner) : base(owner)
    {
        FHeroItems = new HeroItems(null);
        FHeroItems.Hero = this;
        FInventory = new HeroInventory(null);
        FInventory.Hero = this;
        FPowerSource = new HeroPowerSource(null);
        FPowerSource.FHero = this;
        FHeroResourceIndex = -1;
        FHeroStatus = default;
        FLevel = 1;
        FCastingSpellID = 0;
        FCastingMana = 0;
        FCastingProgress = 0;
        FLibraryID = -1;
    }
}
