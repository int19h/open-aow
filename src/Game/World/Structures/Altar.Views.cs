
namespace Aow.Game.World.Structures;

public partial class Altar
{
    protected override StructureView OwnerPanel() => StructureView.Altar;

    // Starts targeting a storm, or cancels the targeting under way.
    public override StructureView InfoButton(sbyte viewer)
    {
        if (!GetTargeting())
        {
            string reason = "";
            if (CanTarget(ref reason))
            {
                _ = Target();
            }
            else
            {
                Map.StrategicMap.Current.ShowWarning(reason);
            }
        }
        else
        {
            _ = CancelTargeting();
        }

        return StructureView.None;
    }
}
