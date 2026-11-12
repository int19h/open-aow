// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// next ID, classes
public partial class AIGroupManager : AIGroupList
{
    // AIGroup.pas:564
    public AIGroupManager(ECustomNode owner) : base(owner)
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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwIntegerEx(ref _nextId, 1, 30);
    }

    // AIGroup.pas:583
    public void RegisterAIGroupClasses(ReadOnlySpan<AIGroupClass> classes)
    {
        for (int I = 0, last = classes.Length - 1; I <= last; I++)
        {
            if (RegisteredClasses.IndexOf(classes[I]) == -1)
            {
                RegisteredClasses.Add(classes[I]);
            }
        }
    }

    // AIGroup.pas:592
    public void UnregisterAIGroupClasses(ReadOnlySpan<AIGroupClass> classes)
    {
        for (int I = 0, last = classes.Length - 1; I <= last; I++)
        {
            RegisteredClasses.Remove(classes[I]);
        }
    }

    // AIGroup.pas:600
    public AIGroup CreateGroup(sbyte player, sbyte relation, GroupBehavior behavior)
    {
        AIGroup result;
        int I;
        bool found;
        found = false;
        I = RegisteredClasses.Count;
        while (!found && I != 0)
        {
            I -= 1;
            found = behavior == (RegisteredClasses[I]).Behavior;
        }

        if (found)
        {
            result = (RegisteredClasses[I]).CreateInstance(this);
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
        while (IndexOfId(result) != -1)
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
