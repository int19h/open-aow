using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities;

public partial class Hero
{
    // Hero.PAS:615
    public Hero(ECustomNode owner) : base(owner)
    {
        HeroItems = new HeroItems(null);
        HeroItems.Hero = this;
        Inventory = new HeroInventory(null);
        Inventory.Hero = this;
        PowerSource = new HeroPowerSource(null);
        PowerSource.Hero = this;
        HeroResourceIndex = -1;
        HeroStatus = default;
        Level = 1;
        CastingSpellId = 0;
        CastingManaValue = 0;
        CastingProgress = 0;
        LibraryId = -1;
    }
}
