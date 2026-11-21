#nullable disable warnings
using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities;

public partial class Hero
{
    // Hero.PAS:615
    public Hero(CustomNode? owner) : base(owner)
    {
        HeroItems = new HeroItems(null)
        {
            Holder = CreationHolder(),
            Hero = this
        };
        Inventory = new HeroInventory(null)
        {
            Holder = CreationHolder(),
            Hero = this
        };
        PowerSource = new HeroPowerSource(null)
        {
            Holder = CreationHolder(),
            Hero = this
        };
        HeroResourceIndex = -1;
        HeroStatus = default;
        Level = 1;
        CastingSpellId = 0;
        CastingManaValue = 0;
        CastingProgress = 0;
        LibraryId = -1;
    }
}
