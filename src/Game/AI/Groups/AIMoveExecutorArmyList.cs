#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

// armies
public sealed class AIMoveExecutorArmyList(CustomNode? owner) : OwningNode(owner), IChildList<AIMoveExecutorArmy>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIMoveExecutorArmy.
    public AIMoveExecutorArmy this[int index] => GetChild<AIMoveExecutorArmy>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIMoveExecutorArmy;

    int IChildSink<AIMoveExecutorArmy>.AddChild(AIMoveExecutorArmy child) => AddChild(child);

    // AIGroup.pas:4642
    public AIMoveExecutorArmy GetArmies(int index) => this[index];

    // AIGroup.pas:4647
    public int IndexOf(ArmyHexSprite army, int selection)
    {
        // { byte local precedes the pointer local }
        int i = Count;
        while (i != 0)
        {
            i -= 1;
            AIMoveExecutorArmy entry = this[i];
            if (army.HexX == entry.Location.X && army.HexY == entry.Location.Y && army.HexLevel == entry.Location.MapLevel)
            {
                entry.RetrieveArmy(255, army: out ArmyHexSprite foundArmy, armySelection: out int foundSelection);
                if (army == foundArmy && foundSelection == selection)
                {
                    return i;
                }
            }
        }

        return -1;
    }

    // AIGroup.pas:4673
    public int IndexOfMapCoordinate(int X, int Y, int L)
    {
        int i = Count;
        while (i != 0)
        {
            i -= 1;
            AIMoveExecutorArmy army = this[i];
            if (X == army.Location.X && army.Location.Y == Y && army.Location.MapLevel == L)
            {
                return i;
            }
        }

        return -1;
    }

    // AIGroup.pas:4693
    public int IndexOfLocation(MapCoordinate location)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (this[result].Location == location)
            {
                return result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4704
    public int IndexOfTransportStatus(AIMoveExecutorTransportStatus status)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (this[result].TransportStatus == status)
            {
                return result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4721
    public void SortOnMovePoints() => SortChildren<AIMoveExecutorArmy>(CompareExecutorArmies);

    private static int CompareExecutorArmies(AIMoveExecutorArmy item1, AIMoveExecutorArmy item2) => item1.MovePoints(255) - item2.MovePoints(255);
}
