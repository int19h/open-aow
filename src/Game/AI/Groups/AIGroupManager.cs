#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.AI.Groups;

// The original's TAIGroupManager derives from TAIGroupList. The port's manager owns the groups
// it creates and reads them through Groups. A player's AIGroups list is a separate index.
public sealed class AIGroupManager(CustomNode? owner) : OwningNode(owner), IChildList<AIGroup>
{

    /// <summary>The groups this manager owns, in their stored order.</summary>
    public AIGroupCollection Groups => new(this);

    // The group in slot index, read as the original reads the list, without a range check.
    public AIGroup this[int index] => Groups[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIGroup;

    int IChildSink<AIGroup>.AddChild(AIGroup child) => AddChild(child);

    private int _nextId = 1;

    public List<AIGroupClass> RegisteredClasses { get; } = [];

    // AIGroup.pas:571
    public override void Dispose()
    {
        RegisteredClasses?.Clear();
        base.Dispose();
    }

    // AIGroup.pas:577
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _nextId = persister.Int32("NextId", value: _nextId, fallback: 1);
    }

    // AIGroup.pas:583
    public void RegisterAIGroupClasses(ReadOnlySpan<AIGroupClass> classes)
    {
        foreach (AIGroupClass groupClass in classes)
        {
            if (RegisteredClasses.IndexOf(groupClass) == -1)
            {
                RegisteredClasses.Add(groupClass);
            }
        }
    }

    // AIGroup.pas:592
    public void UnregisterAIGroupClasses(ReadOnlySpan<AIGroupClass> classes)
    {
        foreach (AIGroupClass groupClass in classes)
        {
            _ = RegisteredClasses.Remove(groupClass);
        }
    }

    // AIGroup.pas:600
    public AIGroup? CreateGroup(sbyte player, sbyte relation, GroupBehavior behavior)
    {
        bool found = false;
        int i = RegisteredClasses.Count;
        while (!found && i != 0)
        {
            i -= 1;
            found = behavior == RegisteredClasses[i].Behavior;
        }

        AIGroup result;
        if (found)
        {
            result = RegisteredClasses[i].CreateInstance(this, holder: CreationHolder());
            result.GroupId = CreateUniqueId();
            result.Player = player;
            result.IndependentRelation = relation;
            _ = result.AddRef();
            result.Activate();
        }
        else
        {
            return null;
        }

        return result;
    }

    // The behavior selects the registered group class, which constructs T.
    public T CreateGroup<T>(sbyte player, sbyte relation) where T : AIGroup, IAIGroupClass
        => (T)CreateGroup(player, relation: relation, behavior: T.Behavior);

    // AIGroup.pas:624
    public int CreateUniqueId()
    {
        int result = _nextId;
        while (Groups.IndexOfId(result) != -1)
        {
            result += 1;
            if (result > 268_435_455)
            {
                result = 1;
            }
        }

        _nextId = result + 1;
        return result;
    }
}
