
namespace Aow.Game.World.Structures;

// A view key. Instances are the keys, the name serves the reports.
public sealed class StructureView(string name)
{
    public string Name => name;

    public override string ToString() => name;

    // Nothing to open, or the kind already ran its own action.
    public static StructureView None { get; } = new("None");
    // Screens the info window button and a double click on the map open.
    public static StructureView CityScreen { get; } = new("CityScreen");
    public static StructureView ProductionScreen { get; } = new("ProductionScreen");
    // Info window panels (AoWInfoWin.pas:616).
    public static StructureView Generic { get; } = new("Generic");
    public static StructureView Building { get; } = new("Building");
    public static StructureView Razed { get; } = new("Razed");
    public static StructureView OtherPlayer { get; } = new("OtherPlayer");
    public static StructureView City { get; } = new("City");
    public static StructureView WarCity { get; } = new("WarCity");
    public static StructureView Production { get; } = new("Production");
    public static StructureView ProductionIncome { get; } = new("ProductionIncome");
    public static StructureView Mine { get; } = new("Mine");
    public static StructureView PowerNode { get; } = new("PowerNode");
    public static StructureView Farm { get; } = new("Farm");
    public static StructureView WatchTower { get; } = new("WatchTower");
    public static StructureView Altar { get; } = new("Altar");
    public static StructureView Sign { get; } = new("Sign");
    public static StructureView Cave { get; } = new("Cave");
    public static StructureView Teleport { get; } = new("Teleport");
    public static StructureView ExplorationSite { get; } = new("ExplorationSite");
    public static StructureView WizardsTower { get; } = new("WizardsTower");
    public static StructureView Arena { get; } = new("Arena");
}
