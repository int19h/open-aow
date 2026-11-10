using Aow.Game.Combat;
using Aow.Game.Magic;
using Aow.Game.World.Structures;
using static Aow.Rtl.RtlUnit;

// What each structure kind shows in the info window, opens from the info window button or a double
// click on the map, and fights on in tactical combat. The keys name views, and each screen holds
// the renderers of its keys, so this file names no screen. A mod structure kind overrides these
// and registers renderers for keys of its own.

namespace Aow.Game.World.Structures
{
    // A view key. Instances are the keys, the name serves the reports.
    public sealed class StructureView(string name)
    {
        public string Name => name;

        public override string ToString() => name;

        // Nothing to open, or the kind already ran its own action.
        public static readonly StructureView None = new("None");
        // Screens the info window button and a double click on the map open.
        public static readonly StructureView CityScreen = new("CityScreen");
        public static readonly StructureView ProductionScreen = new("ProductionScreen");
        // Info window panels (AoWInfoWin.pas:616).
        public static readonly StructureView Generic = new("Generic");
        public static readonly StructureView Building = new("Building");
        public static readonly StructureView Razed = new("Razed");
        public static readonly StructureView OtherPlayer = new("OtherPlayer");
        public static readonly StructureView City = new("City");
        public static readonly StructureView WarCity = new("WarCity");
        public static readonly StructureView Production = new("Production");
        public static readonly StructureView ProductionIncome = new("ProductionIncome");
        public static readonly StructureView Mine = new("Mine");
        public static readonly StructureView PowerNode = new("PowerNode");
        public static readonly StructureView Farm = new("Farm");
        public static readonly StructureView WatchTower = new("WatchTower");
        public static readonly StructureView Altar = new("Altar");
        public static readonly StructureView Sign = new("Sign");
        public static readonly StructureView Cave = new("Cave");
        public static readonly StructureView Teleport = new("Teleport");
        public static readonly StructureView ExplorationSite = new("ExplorationSite");
        public static readonly StructureView WizardsTower = new("WizardsTower");
        public static readonly StructureView Arena = new("Arena");
    }
}

namespace Aow.Game.Combat
{

    // The combat hex as the tactical map generator sees it when it picks the map file
    // (AoWTC.pas:4156). A kind reads the combat state and the neighbor fields, and reports back the
    // terrain mode, whether the map lays out the combat's items and which neighbor tags it fixes.
    public sealed class CombatMapSite(bool siteCombat, bool randomTerrain, Func<int, Type, bool> neighborHolds)
    {
        // The combat is an exploration site fight.
        public bool SiteCombat => siteCombat;

        // The generator randomizes the terrain around the map. A kind turns it on or off.
        public bool RandomTerrain { get; set; } = randomTerrain;

        // The map lays out the items of the combat.
        public bool TakesCombatItems { get; set; }

        // Bit I set: the generator fixes the terrain tag of neighbor I (1 to 6).
        public int FixedTags { get; private set; }

        // Neighbor Direction (1 to 6) of the combat hex holds a sprite of Kind that is not transparent.
        public bool NeighborHolds(int Direction, Type Kind) => neighborHolds(Direction, Kind);

        public void FixTag(int Direction) => FixedTags |= 1 << Direction;
    }
}

namespace Aow.Game.World.Structures
{

    public partial class Structure
    {
        // The info window panel for the seated player Viewer (AoWInfoWin.pas:616).
        public virtual StructureView InfoPanel(sbyte Viewer) => StatePanel() ?? StructureView.Generic;

        // The info window button (AoWInfoWin.pas:190): a screen to open, or None after the kind ran
        // its own action.
        public virtual StructureView InfoButton(sbyte Viewer) => StructureView.None;

        // A double click on the selected structure on the map (aowMapEvents.pas:518).
        public virtual StructureView DoubleClick(sbyte Viewer) => StructureView.None;

        // The tactical map file stem under TCMaps (AoWTC.pas:4156). The original matched exact class
        // ids, and every other structure fights on the blank map.
        public virtual string CombatMapName(CombatMapSite Site) => "Blank000";

        // A structure under construction or razed shows that state before its kind.
        protected StructureView StatePanel()
        {
            if (GetBuilding())
            {
                return StructureView.Building;
            }

            if (GetRazed())
            {
                return StructureView.Razed;
            }

            return null;
        }
    }

    public partial class PlayerStructure
    {
        public override StructureView InfoPanel(sbyte Viewer) => StatePanel() ?? (ShownToOwner(Viewer) ? OwnerPanel() : StructureView.OtherPlayer);

        // The panel the owner sees, and every player in debug mode.
        protected virtual StructureView OwnerPanel() => StructureView.Generic;

        protected bool ShownToOwner(sbyte Viewer) => FPlayer == Viewer || Magic.Abilities.GameTables.AoWEngine.FDebug;
    }
}

namespace Aow.Game.World.Structures
{
    public partial class City
    {
        // War mode shows the war panel to every player.
        public override StructureView InfoPanel(sbyte Viewer)
        {
            var state = StatePanel();
            if (state != null)
            {
                return state;
            }

            if (ShownToOwner(Viewer))
            {
                return FMode.Contains(ModeElement.Warmode) ? StructureView.WarCity : StructureView.City;
            }

            return FMode.Contains(ModeElement.Warmode) ? StructureView.WarCity : StructureView.OtherPlayer;
        }

        public override StructureView InfoButton(sbyte Viewer) => StructureView.CityScreen;

        public override StructureView DoubleClick(sbyte Viewer) => FPlayer == Viewer ? StructureView.CityScreen : StructureView.None;

        // "0", the directions of the neighbors that hold the city, "x" up to four characters, "C000".
        public override string CombatMapName(CombatMapSite Site)
        {
            string Stem = "0";
            for (int I = 1; I <= 6; I += 1)
            {
                if (Site.NeighborHolds(I, typeof(City)))
                {
                    Stem = Stem + IntToStr(I);
                    Site.FixTag(I);
                }
            }

            while (Stem.Length < 4)
            {
                Stem = Stem + "x";
            }

            return Stem + "C000";
        }
    }
}

namespace Aow.Game.World.Structures
{
    public partial class ProductionPlace
    {
        protected override StructureView OwnerPanel() => GetMagicSphere() == MagicSphere.None ? StructureView.Production : StructureView.ProductionIncome;

        public override StructureView InfoButton(sbyte Viewer) => StructureView.ProductionScreen;

        public override StructureView DoubleClick(sbyte Viewer) => CanDblClick() && FPlayer == Viewer ? StructureView.ProductionScreen : StructureView.None;
    }
}

namespace Aow.Game.World.Structures
{
    public partial class Mine
    {
        protected override StructureView OwnerPanel() => StructureView.Mine;

        public override string CombatMapName(CombatMapSite Site) => "Minex000";
    }
}

namespace Aow.Game.World.Structures
{
    public partial class PowerNode
    {
        protected override StructureView OwnerPanel() => StructureView.PowerNode;

        public override string CombatMapName(CombatMapSite Site) => "NodeC000";
    }
}

namespace Aow.Game.World.Structures
{
    public partial class Farm
    {
        protected override StructureView OwnerPanel() => StructureView.Farm;

        public override string CombatMapName(CombatMapSite Site) => "Farmx000";
    }
}

namespace Aow.Game.World.Structures
{
    public partial class Tower
    {
        protected override StructureView OwnerPanel() => StructureView.WatchTower;

        public override string CombatMapName(CombatMapSite Site) => "TFort000";
    }
}

namespace Aow.Game.World.Structures
{
    public partial class Altar
    {
        protected override StructureView OwnerPanel() => StructureView.Altar;

        // Starts targeting a storm, or cancels the targeting under way.
        public override StructureView InfoButton(sbyte Viewer)
        {
            string Reason = "";
            if (!GetTargeting())
            {
                if (CanTarget(ref Reason))
                {
                    Target();
                }
                else
                {
                    Aow.Game.World.Map.AoWHSMap.Current.ShowWarning(Reason);
                }
            }
            else
            {
                CancelTargeting();
            }

            return StructureView.None;
        }
    }
}

namespace Aow.Game.World.Structures
{
    public partial class Sign
    {
        public override StructureView InfoPanel(sbyte Viewer) => StatePanel() ?? StructureView.Sign;
    }
}

namespace Aow.Game.World.Structures
{
    public partial class Cave
    {
        public override StructureView InfoPanel(sbyte Viewer) => StatePanel() ?? StructureView.Cave;

        public override StructureView InfoButton(sbyte Viewer)
        {
            Enter(Viewer);
            return StructureView.None;
        }

        public override string CombatMapName(CombatMapSite Site) => Site.RandomTerrain ? "CaveN000" : "CaveE000";
    }
}

namespace Aow.Game.World.Structures
{
    public partial class Teleport
    {
        public override StructureView InfoPanel(sbyte Viewer) => StatePanel() ?? StructureView.Teleport;

        public override StructureView InfoButton(sbyte Viewer)
        {
            TeleportPlayer(Viewer);
            return StructureView.None;
        }

        // The original read FRandomTerrain here. It equals RandomTerrain at this point, because the
        // only other writer is the bridge arm of the generator, and a bridge is no structure.
        public override string CombatMapName(CombatMapSite Site)
        {
            string Stem = Site.RandomTerrain ? "TeleN000" : "Telep000";
            Site.RandomTerrain = true;
            return Stem;
        }
    }
}

namespace Aow.Game.World.Structures
{
    public partial class ExplorationSite
    {
        public override StructureView InfoPanel(sbyte Viewer) => StatePanel() ?? StructureView.ExplorationSite;

        public override StructureView InfoButton(sbyte Viewer)
        {
            Search();
            return StructureView.None;
        }

        // A site fight takes one of Variants maps and fixes every neighbor tag. Another fight at the
        // site takes the blank map.
        protected static string SiteCombatMapName(CombatMapSite Site, string Prefix, int Variants, bool TakesItems)
        {
            if (!Site.SiteCombat)
            {
                return "Blank000";
            }

            Site.TakesCombatItems = TakesItems;
            string Stem = Prefix + IntToStr(Random(Variants));
            for (int I = 1; I <= 6; I += 1)
            {
                Site.FixTag(I);
            }

            return Stem;
        }
    }
}

namespace Aow.Game.World.Structures
{
    public partial class WizardsTower
    {
        public override StructureView InfoPanel(sbyte Viewer) => StatePanel() ?? StructureView.WizardsTower;

        public override StructureView InfoButton(sbyte Viewer)
        {
            Enter();
            return StructureView.None;
        }

        public override StructureView DoubleClick(sbyte Viewer)
        {
            Enter();
            return StructureView.None;
        }
    }
}

namespace Aow.Game.World.Structures
{
    public partial class Arena
    {
        public override StructureView InfoPanel(sbyte Viewer) => StatePanel() ?? StructureView.Arena;

        public override StructureView InfoButton(sbyte Viewer)
        {
            Enter();
            return StructureView.None;
        }

        public override StructureView DoubleClick(sbyte Viewer)
        {
            Enter();
            return StructureView.None;
        }
    }
}

namespace Aow.Game.World.Structures
{
    public partial class SpringOfLife
    {
        public override string CombatMapName(CombatMapSite Site) => "SLife000";
    }
}

namespace Aow.Game.World.Structures
{
    public partial class AirNode
    {
        public override string CombatMapName(CombatMapSite Site) => "NodeA000";
    }
}

namespace Aow.Game.World.Structures
{
    public partial class DeathNode
    {
        public override string CombatMapName(CombatMapSite Site) => "NodeD000";
    }
}

namespace Aow.Game.World.Structures
{
    public partial class EarthNode
    {
        public override string CombatMapName(CombatMapSite Site) => "NodeE000";
    }
}

namespace Aow.Game.World.Structures
{
    public partial class FireNode
    {
        public override string CombatMapName(CombatMapSite Site) => "NodeF000";
    }
}

namespace Aow.Game.World.Structures
{
    public partial class LifeNode
    {
        public override string CombatMapName(CombatMapSite Site) => "NodeL000";
    }
}

namespace Aow.Game.World.Structures
{
    public partial class WaterNode
    {
        public override string CombatMapName(CombatMapSite Site) => "NodeW000";
    }
}

namespace Aow.Game.World.Structures
{
    public partial class ReflectingPool
    {
        public override string CombatMapName(CombatMapSite Site) => "RPool000";
    }
}

namespace Aow.Game.World.Structures
{
    public partial class BuildersGuild
    {
        public override string CombatMapName(CombatMapSite Site) => "BGuil000";
    }
}

namespace Aow.Game.World.Structures
{
    public partial class Shipyard
    {
        public override string CombatMapName(CombatMapSite Site) => "SYard000";
    }
}

namespace Aow.Game.World.Structures
{
    public partial class Monsterlair
    {
        public override string CombatMapName(CombatMapSite Site) => SiteCombatMapName(Site, "eMrLr00", 4, true);
    }
}

namespace Aow.Game.World.Structures
{
    public partial class Pyramid
    {
        public override string CombatMapName(CombatMapSite Site) => SiteCombatMapName(Site, "eZigg00", 2, true);
    }
}

namespace Aow.Game.World.Structures
{
    public partial class Crypt
    {
        public override string CombatMapName(CombatMapSite Site) => SiteCombatMapName(Site, "eCrpt00", 2, true);
    }
}

namespace Aow.Game.World.Structures
{
    public partial class Ruin
    {
        public override string CombatMapName(CombatMapSite Site) => SiteCombatMapName(Site, "eRuin00", 2, true);
    }
}

namespace Aow.Game.World.Structures
{
    public partial class Dungeon
    {
        // The dungeon fight lays out no items.
        public override string CombatMapName(CombatMapSite Site) => SiteCombatMapName(Site, "eDung00", 2, false);
    }
}
