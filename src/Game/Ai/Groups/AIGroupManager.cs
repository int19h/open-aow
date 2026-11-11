// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.Ai.Groups;

// next ID, classes
public partial class AIGroupManager : AIGroupList
{
    // AIGroup.pas:564
    public AIGroupManager(ECustomNode owner) : base(owner)
    {
        FNextID = 1;
        FRegisteredClasses = new List<AIGroupClass>();
    }

    public int FNextID;
    public List<AIGroupClass> FRegisteredClasses;

    // AIGroup.pas:571
    public override void Dispose()
    {
        FRegisteredClasses?.Clear();
        base.Dispose();
    }

    // AIGroup.pas:577
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwIntegerEx(ref FNextID, 1, 30);
    }

    // AIGroup.pas:583
    public void RegisterAIGroupClasses(ReadOnlySpan<AIGroupClass> Classes)
    {
        for (int I = 0, last = Classes.Length - 1; I <= last; I++)
        {
            if (FRegisteredClasses.IndexOf(Classes[I]) == -1)
            {
                FRegisteredClasses.Add(Classes[I]);
            }
        }
    }

    // AIGroup.pas:592
    public void UnregisterAIGroupClasses(ReadOnlySpan<AIGroupClass> Classes)
    {
        for (int I = 0, last = Classes.Length - 1; I <= last; I++)
        {
            FRegisteredClasses.Remove(Classes[I]);
        }
    }

    // AIGroup.pas:600
    public AIGroup CreateGroup(sbyte Player, sbyte Relation, GroupBehavior Behavior)
    {
        AIGroup Result;
        int I;
        bool Found;
        Found = false;
        I = FRegisteredClasses.Count;
        while (!Found && I != 0)
        {
            I -= 1;
            Found = Behavior == (FRegisteredClasses[I]).Behavior;
        }

        if (Found)
        {
            Result = (FRegisteredClasses[I]).Create(this);
            Result.FGroupID = CreateUniqueID();
            Result.FPlayer = Player;
            Result.FIndependentRelation = Relation;
            Result.AddRef();
            Result.Activate();
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // AIGroup.pas:624
    public int CreateUniqueID()
    {
        int Result = FNextID;
        while (IndexOfID(Result) != -1)
        {
            Result += 1;
            if (Result > 268435455)
            {
                Result = 1;
            }
        }

        FNextID = Result + 1;
        return Result;
    }

    // intf_P5_07.inc:101

}
