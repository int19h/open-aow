#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

using Aow.Game.Session;

namespace Aow.Game.World.Structures;

public partial class Altar
{
    protected override StructureView OwnerPanel() => StructureView.Altar;

    // Starts targeting a storm, or cancels the targeting under way.
    public override StructureView InfoButton(PlayerId? viewer)
    {
        if (!Targeting)
        {
            string reason = "";
            if (CanTarget(ref reason))
            {
                _ = Target();
            }
            else
            {
                GetEngine().StrategicMap.ShowWarning(reason);
            }
        }
        else
        {
            _ = CancelTargeting();
        }

        return StructureView.None;
    }
}
