
namespace Aow.Game.AI.Groups;

public partial class AIGroup
{
    private AIGroupClass groupClass;

    // The group descriptor of the instance's exact type, read once and cached like EngineClass.
    public AIGroupClass GroupClass => groupClass ??= AIGroupClass.Of(GetType());
}
