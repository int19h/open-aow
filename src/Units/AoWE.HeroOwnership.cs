using Aow.Rtl;
using Aow.Units.Engine;

namespace Aow.Units.AoWE;

public partial class THero
{
    public override void Create__init(TECustomNode owner)
    {
        base.Create__init(owner);
        FHeroItems = THeroItems.Create(null);
        FHeroItems.Hero = this;
        FInventory = THeroInventory.Create(null);
        FInventory.Hero = this;
        FPowerSource = THeroPowerSource.Create(null);
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
    public override void Destroy()
    {
        base.Destroy();
        Rtl.Rtl.ReferenceFromId<TByteList>(F_PlayerList_14).Free();
        FLocalHumanPlayers.Free();
        FActivePlayers.Free();
        FLocalPlayers.Free();
    }
}
