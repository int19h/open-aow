// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Commands;

namespace Aow.Game.Net;

// FList (stored with id 5), a reference node of token events
public partial class TokenEventList : EngineObject
{
    // NetworkE.pas:995
    public new const int ClassID = CID_TokenEventList;

    internal EReferenceNode FList;

    // NetworkE.pas:1000
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FList, 5);
    }

    // NetworkE.pas:982
    public TokenEventList(ECustomNode owner) : base(owner)
    {
        FList = new EReferenceNode(null);
    }

    // NetworkE.pas:1046
    public override void Clear()
    {
        FList.Clear();
    }

    // NetworkE.pas:1006
    public override void Assign(EngineObject Source, GameEngine AEngine)
    {
        FList.Clear();
        AddEvents((TokenEventList)Source);
    }

    // NetworkE.pas:988
    public override void Dispose()
    {
        Clear();
        FList?.Dispose();
        base.Dispose();
    }

    // NetworkE.pas:1012
    public int IndexOfTokenCounter(int ATokenCounter)
    {
        int Result = FList.Count;
        while (Result != 0)
        {
            Result -= 1;
            if (((TokenEvent)FList.FChildren.List[Result]).FTokenCounter == ATokenCounter)
            {
                return Result;
            }
        }

        return -1;
    }

    // NetworkE.pas:1024
    public int FindTE(int AClassID)
    {
        int C;
        int Result = 0;
        C = FList.Count;
        while (Result != C && ((EngineObject)FList.FChildren.List[Result]).EClass.ClassID != AClassID)
        {
            Result += 1;
        }

        if (Result == C)
        {
            Result = -1;
        }

        return Result;
    }

    // NetworkE.pas:1036
    public TokenEvent GetEvent(int Index) => (TokenEvent)FList.FChildren.List[Index];

    // NetworkE.pas:1041
    public int GetCount() => FList.Count;

    // NetworkE.pas:1056
    public void SortOnTokenCounter()
    {
        FList.FChildren.Sort(CompareTokenCounter);
    }

    // NetworkE.pas:1061
    public void AddEvents(TokenEventList Events)
    {
        for (int I = 0, last = Events.Count - 1; I <= last; I++)
        {
            var tokenEvent = Events[I];
            FList.AddChild(tokenEvent);
        }
    }

    // NetworkE.pas:1069
    public void Add(TokenEvent TE)
    {
        FList.AddChild(TE);
    }

    // NetworkE.pas:1074
    public void Delete(int Index)
    {
        var child = FList.Children[Index];
        FList.RemoveChild(child);
    }

    // NetworkE.pas:1080
    // { Inserts without TEReferenceNode.AddChild, so the reference is taken here }
    public void Insert(int Index, TokenEvent TE)
    {
        TE.AddRef();
        FList.FChildren.Insert(Index, TE);
    }

    public int Count { get => GetCount(); }

    public TokenEvent this[int Index] { get => GetEvent(Index); }

    private const short CID_TokenEventList = 1362;

    private static int CompareTokenCounter(object Item1, object Item2)
    {
        return ((TokenEvent)Item1).FTokenCounter - ((TokenEvent)Item2).FTokenCounter;
    }

}
