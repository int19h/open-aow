
namespace Aow.Game.AI;

public partial class AIPlayerAction
{
    private AIPlayerActionClass _actionClass;

    // The action descriptor of the instance's exact type, read once and cached like EngineClass.
    public AIPlayerActionClass ActionClass => _actionClass ??= AIPlayerActionClass.Of(GetType());
}
