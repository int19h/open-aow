
namespace Aow.Game.World.Structures;

public partial class Altar
{
    protected override StructureView OwnerPanel() => StructureView.Altar;

    // Starts targeting a storm, or cancels the targeting under way.
    public override StructureView InfoButton(sbyte viewer)
    {
        string reason = "";
        if (!GetTargeting())
        {
            if (CanTarget(ref reason))
            {
                Target();
            }
            else
            {
                Map.AoWHSMap.Current.ShowWarning(reason);
            }
        }
        else
        {
            CancelTargeting();
        }

        return StructureView.None;
    }
}
