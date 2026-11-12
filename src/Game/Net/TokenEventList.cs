// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Commands;

namespace Aow.Game.Net;

// FList (stored with id 5), a reference node of token events
public partial class TokenEventList : EngineObject
{
    // NetworkE.pas:995
    public new const int ClassId = ClassIdTokenEventList;

    internal EReferenceNode List { get; }

    // NetworkE.pas:1000
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(List, 5);
    }

    // NetworkE.pas:982
    public TokenEventList(ECustomNode owner) : base(owner)
    {
        List = new EReferenceNode(null);
    }

    // NetworkE.pas:1046
    public override void Clear()
    {
        List.Clear();
    }

    // NetworkE.pas:1006
    public override void Assign(EngineObject source, GameEngine engine)
    {
        List.Clear();
        AddEvents((TokenEventList)source);
    }

    // NetworkE.pas:988
    public override void Dispose()
    {
        Clear();
        List?.Dispose();
        base.Dispose();
    }

    // NetworkE.pas:1012
    public int IndexOfTokenCounter(int tokenCounter)
    {
        int result = List.Count;
        while (result != 0)
        {
            result -= 1;
            if (((TokenEvent)List._children.List[result])._tokenCounter == tokenCounter)
            {
                return result;
            }
        }

        return -1;
    }

    // NetworkE.pas:1024
    public int FindTokenEvent(int classId)
    {
        int C;
        int result = 0;
        C = List.Count;
        while (result != C && ((EngineObject)List._children.List[result]).EngineClass.ClassId != classId)
        {
            result += 1;
        }

        if (result == C)
        {
            result = -1;
        }

        return result;
    }

    // NetworkE.pas:1036
    public TokenEvent GetEvent(int index) => (TokenEvent)List._children.List[index];

    // NetworkE.pas:1041
    public int GetCount() => List.Count;

    // NetworkE.pas:1056
    public void SortOnTokenCounter()
    {
        List._children.Sort(CompareTokenCounter);
    }

    // NetworkE.pas:1061
    public void AddEvents(TokenEventList events)
    {
        for (int I = 0, last = events.Count - 1; I <= last; I++)
        {
            var tokenEvent = events[I];
            List.AddChild(tokenEvent);
        }
    }

    // NetworkE.pas:1069
    public void Add(TokenEvent tokenEvent)
    {
        List.AddChild(tokenEvent);
    }

    // NetworkE.pas:1074
    public void Delete(int index)
    {
        var child = List.Children[index];
        List.RemoveChild(child);
    }

    // NetworkE.pas:1080
    // { Inserts without TEReferenceNode.AddChild, so the reference is taken here }
    public void Insert(int index, TokenEvent tokenEvent)
    {
        tokenEvent.AddRef();
        List.InsertChild(index, tokenEvent);
    }

    public int Count { get => GetCount(); }

    public TokenEvent this[int index] { get => GetEvent(index); }

    private const short ClassIdTokenEventList = 1362;

    private static int CompareTokenCounter(object item1, object item2)
    {
        return ((TokenEvent)item1)._tokenCounter - ((TokenEvent)item2)._tokenCounter;
    }

}
