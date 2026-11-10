
namespace Aow.Game.Ai.Groups;

// The class-side member of an AI group class that the group manager reads before it creates a
// group (AIGroup.pas TAIGroup.Behavior).
public interface IAIGroupClass
{
    static abstract GroupBehavior Behavior { get; }
}
