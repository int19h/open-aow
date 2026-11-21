#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

namespace Aow.Game.Units;

public sealed class FaceResourceList : ResourceCollection, IChildList<FaceResource>
{
    // The face in slot index, read as the original reads the list, without a range check.
    // AcceptsResource admits only a FaceResource, so the cast never fails.
    public new FaceResource this[int index] => GetChild<FaceResource>(index);

    protected sealed override bool AcceptsResource(EngineResource resource) => resource is FaceResource;

    int IChildSink<FaceResource>.AddChild(FaceResource child) => AddChild(child);

    // Ability.PAS:520
    public FaceResourceList(CustomNode owner) : base(owner) => SetControlStyle(ControlStyles.SaveLoad | ControlStyles.FixedChild | ControlStyles.ArrayMode);

    // Ability.PAS:526
    public int IndexOfFaceImage(LibraryImage image)
    {
        int result = GetCount();
        while (result != 0)
        {
            result -= 1;
            FaceResource face = this[result];
            if (face is not null && face.FaceImage == image)
            {
                return result;
            }
        }

        return -1;
    }

    // Ability.PAS:540
    public FaceResource? GetFaceResource(int index) => index < Count && index >= 0 ? this[index] : null;

    // Ability.PAS:547
    public int PreviousFace(int index)
    {
        int result = index;
        bool developer = GetEngine().Registry.Developer;
        // { Original assumes the list contains a selectable face. }
        do
        {
            result -= 1;
            if (result < 0)
            {
                result = Count - 1;
            }
        }
        while (!(GetChild(result) is not null && (developer || !this[result].DeveloperOnly)));
        return result;
    }

    // Ability.PAS:561
    public int NextFace(int index)
    {
        int result = index;
        bool developer = GetEngine().Registry.Developer;
        // { Original assumes the list contains a selectable face. }
        do
        {
            result += 1;
            if (result >= Count)
            {
                result = 0;
            }
        }
        while (!(GetChild(result) is not null && (developer || !this[result].DeveloperOnly)));
        return result;
    }
}
