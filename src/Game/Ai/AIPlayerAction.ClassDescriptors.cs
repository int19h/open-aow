
namespace Aow.Game.Ai;

public partial class AIPlayerAction
{
    private AIPlayerActionClass actionClass;

    // The action descriptor of the instance's exact type, read once and cached like EClass.
    public AIPlayerActionClass ActionClass => actionClass ??= AIPlayerActionClass.Of(GetType());
}
