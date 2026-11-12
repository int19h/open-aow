using Aow.Formats.Tree;

namespace Aow.Engine.Nodes;

/// <summary>
/// The children of a reference list (Delphi <c>TEReferenceNode</c>), in stored order. A child with
/// no owner or with the list as its owner is stored, the set the stream write stores after it makes
/// the list the owner of every child without one. The tree write leaves every owner as it is. The
/// load attaches and places a child as a node's children are placed, so the list takes its
/// reference through <see cref="EReferenceNode.AddChild"/>.
/// </summary>
public sealed class ReferenceChildren(EReferenceNode list) : IChildCollection
{
    private readonly NodeChildren _placement = new(list);

    public int Count => list.GetCount();

    public IPersistable Get(int index) => list.GetChild(index);

    public bool IsStored(int index)
    {
        var child = list.GetChild(index);
        return child != null && (child.Owner == null || child.Owner == list)
            && !child.GetControlStyle().Contains(EngineServices.Shared)
            && child.GetControlStyle().Contains(EngineServices.Stored);
    }

    public int Slot(int index) => index;

    public bool KeepIndex => _placement.KeepIndex;

    public void Attach(IPersistable child) => _placement.Attach(child);

    public void Place(int slot, IPersistable child) => _placement.Place(slot: slot, child: child);
}
