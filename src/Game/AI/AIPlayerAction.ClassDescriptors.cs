
namespace Aow.Game.AI;

public partial class AIPlayerAction
{
    // The action descriptor of the instance's exact type, read once and cached like EngineClass.
    public AIPlayerActionClass ActionClass => field ??= AIPlayerActionClass.Of(GetType());
}
