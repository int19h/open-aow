
namespace Aow.Game.AI.Groups;

public partial class AIGroup
{
    // The group descriptor of the instance's exact type, read once and cached like EngineClass.
    public AIGroupClass GroupClass => field ??= AIGroupClass.Of(GetType());
}
