
namespace Aow.Game.AI;

public partial class AIPlayerAction
{
    private AIPlayerActionClass actionClass;

    // The action descriptor of the instance's exact type, read once and cached like EngineClass.
    public AIPlayerActionClass ActionClass => actionClass ??= AIPlayerActionClass.Of(GetType());
}
