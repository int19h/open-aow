using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities
{

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
}

namespace Aow.Game.Units
{

    public partial class HeroItems
    {
        internal global::Aow.Game.Magic.Abilities.Hero Hero;

        public override global::Aow.Game.Engine.Engine GetEngine() => Hero != null ? Hero.GetEngine() : base.GetEngine();
        public override void GetLocation(ref sbyte x, ref sbyte y, ref sbyte level) => Hero.GetLocation(ref x, ref y, ref level);
    }

    public partial class HeroInventory
    {
        internal global::Aow.Game.Magic.Abilities.Hero Hero;

        public override global::Aow.Game.Engine.Engine GetEngine() => Hero != null ? Hero.GetEngine() : base.GetEngine();
        public override void GetLocation(ref sbyte x, ref sbyte y, ref sbyte level) => Hero.GetLocation(ref x, ref y, ref level);
    }
}

namespace Aow.Game.Session
{

    public partial class PlayerList
    {
        public override void Dispose()
        {
            base.Dispose();
            FLocalControlledPlayers?.Dispose();
            FLocalHumanPlayers?.Dispose();
            FActivePlayers?.Dispose();
            FLocalPlayers?.Dispose();
        }
    }
}
