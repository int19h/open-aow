#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.AI.Groups;

// The original's TAIGroupManager derives from TAIGroupList. The port's manager owns the groups
// it creates and reads them through Groups. A player's AIGroups list is a separate index.
public partial class AIGroupManager : OwningNode, IChildList<AIGroup>
{
    /// <summary>The groups this manager owns, in their stored order.</summary>
    public AIGroupCollection Groups => new(this);

    // The group in slot index, read as the original reads the list, without a range check.
    public AIGroup this[int index] => Groups[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIGroup;

    int IChildSink<AIGroup>.AddChild(AIGroup child) => AddChild(child);

    // AIGroup.pas:564
    public AIGroupManager(CustomNode owner) : base(owner)
    {
        _nextId = 1;
        RegisteredClasses = new List<AIGroupClass>();
    }

    private int _nextId;

    public List<AIGroupClass> RegisteredClasses { get; }

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
        for (int i = 0, last = classes.Length - 1; i <= last; i++)
        {
            if (RegisteredClasses.IndexOf(classes[i]) == -1)
            {
                RegisteredClasses.Add(classes[i]);
            }
        }
    }

    // AIGroup.pas:592
    public void UnregisterAIGroupClasses(ReadOnlySpan<AIGroupClass> classes)
    {
        for (int i = 0, last = classes.Length - 1; i <= last; i++)
        {
            RegisteredClasses.Remove(classes[i]);
        }
    }

    // AIGroup.pas:600
    public AIGroup CreateGroup(sbyte player, sbyte relation, GroupBehavior behavior)
    {
        AIGroup result;
        int i;
        bool found;
        found = false;
        i = RegisteredClasses.Count;
        while (!found && i != 0)
        {
            i -= 1;
            found = behavior == RegisteredClasses[i].Behavior;
        }

        if (found)
        {
            result = RegisteredClasses[i].CreateInstance(this, holder: CreationHolder());
            result.GroupId = CreateUniqueId();
            result.Player = player;
            result.IndependentRelation = relation;
            result.AddRef();
            result.Activate();
        }
        else
        {
            result = null;
        }

        return result;
    }

    // AIGroup.pas:624
    public int CreateUniqueId()
    {
        int result = _nextId;
        while (Groups.IndexOfId(result) != -1)
        {
            result += 1;
            if (result > 268435455)
            {
                result = 1;
            }
        }

        _nextId = result + 1;
        return result;
    }

    // intf_P5_07.inc:101

}
