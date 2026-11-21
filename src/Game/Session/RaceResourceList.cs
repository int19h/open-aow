#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public sealed class RaceResourceList : ResourceCollection, IChildList<RaceResource>
{
    // The race resource in slot index, read as the original reads the list, without a range check.
    // AcceptsResource admits only a RaceResource, so the cast never fails.
    public new RaceResource this[int index] => GetChild<RaceResource>(index);

    protected sealed override bool AcceptsResource(EngineResource resource) => resource is RaceResource;

    int IChildSink<RaceResource>.AddChild(RaceResource child) => AddChild(child);

    // PlayerC.PAS:1890
    public RaceResourceList(CustomNode owner) : base(owner) => SetControlStyle(ControlStyles.SaveLoad | ControlStyles.FixedChild | ControlStyles.ArrayMode);

    // PlayerC.PAS:1896
    public RaceResource GetRaceResource(int index) => index < GetCount() && index >= 0 ? this[index] : null;

    // PlayerC.PAS:1903
    public int GetMaxRaceIndex()
    {
        int result = Count - 1;
        while (result > 0 && GetChild(result) is null)
        {
            result -= 1;
        }

        return result;
    }
}
