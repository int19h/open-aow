// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

namespace Aow.Game.AI.Groups;

// children
public partial class AIGroupProductionLocationList(CustomNode owner) : ReferenceNode(owner), IChildList<AIGroupProductionLocation>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIGroupProductionLocation, so the cast never fails.
    public AIGroupProductionLocation this[int index] => Locations[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIGroupProductionLocation;

    int IChildSink<AIGroupProductionLocation>.AddChild(AIGroupProductionLocation child) => AddChild(child);

    /// <summary>The production locations of this list, in their stored order.</summary>
    public AIGroupProductionLocationCollection Locations => new(this);

    // AIGroup.pas:907
    public AIGroupProductionLocation GetProductionLocation(int index) => Locations.GetProductionLocation(index);

    // AIGroup.pas:919
    public void SortOnMovePoints() => Locations.SortOnMovePoints();

    // AIGroup.pas:924
    public void Add(AIGroupProductionLocation location) => AddChild(location);

    // AIGroup.pas:929
    public void Delete(int index) => RemoveChild(GetProductionLocation(index));

    // AIGroup.pas:934
    public int IndexOfPC(ProductionControl control) => Locations.IndexOfPC(control);

    // AIGroup.pas:945
    public AIGroupProductionLocation FindPC(ProductionControl control) => Locations.FindPC(control);

    // AIGroup.pas:956
    public bool Validate(AIGroupControl control)
    {
        bool result = true;
        for (int i = Count - 1; i >= 0; i--)
        {
            AIGroupProductionLocation location = this[i];
            if (!location.Validate(control))
            {
                _ = RemoveChild(location);
                result = false;
            }
        }

        return result;
    }

    // Engine.pas:631

    // Engine.pas:631

}
