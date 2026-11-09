using Aow.Rtl;
using Aow.Units.Engine;

namespace Aow.Units.AoWE;

public partial class THero
{
    // Hero.PAS:615
    public THero(TECustomNode owner) : base(owner)
    {
        FHeroItems = new THeroItems(null);
        FHeroItems.Hero = this;
        FInventory = new THeroInventory(null);
        FInventory.Hero = this;
        FPowerSource = new THeroPowerSource(null);
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

public partial class THeroItems
{
    internal THero Hero;

    public override TEngine GetEngine() => Hero != null ? Hero.GetEngine() : base.GetEngine();
    public override void GetLocation(ref sbyte x, ref sbyte y, ref sbyte level) => Hero.GetLocation(ref x, ref y, ref level);
}

public partial class THeroInventory
{
    internal THero Hero;

    public override TEngine GetEngine() => Hero != null ? Hero.GetEngine() : base.GetEngine();
    public override void GetLocation(ref sbyte x, ref sbyte y, ref sbyte level) => Hero.GetLocation(ref x, ref y, ref level);
}

public partial class TPlayerList
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
