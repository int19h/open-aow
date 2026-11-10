
namespace Aow.Game.World.Structures;

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
