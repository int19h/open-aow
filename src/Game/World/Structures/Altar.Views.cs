
namespace Aow.Game.World.Structures;

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
                Map.AoWHSMap.Current.ShowWarning(Reason);
            }
        }
        else
        {
            CancelTargeting();
        }

        return StructureView.None;
    }
}
