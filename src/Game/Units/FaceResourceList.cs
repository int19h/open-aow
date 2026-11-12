// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Units;

public partial class FaceResourceList : EResourceList, IChildList<FaceResource>
{
    // The face in slot index, read as the original reads the list, without a range check.
    // AcceptsResource admits only a FaceResource, so the cast never fails.
    public new FaceResource this[int index] => (FaceResource)_children.List[index];

    protected sealed override bool AcceptsResource(EngineResource resource) => resource is FaceResource;

    int IChildSink<FaceResource>.AddChild(FaceResource child) => AddChild(child);

    // Ability.PAS:520
    public FaceResourceList(ECustomNode owner) : base(owner)
    {
        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, ControlStyleElement.ArrayMode));
    }

    // Ability.PAS:526
    public int IndexOfFaceImage(LibraryImage image)
    {
        FaceResource face;
        int result = GetCount();
        while (result != 0)
        {
            result -= 1;
            face = this[result];
            if (face != null && face.GetFaceImage() == image)
            {
                return result;
            }
        }

        return -1;
    }

    // Ability.PAS:540
    public FaceResource GetFaceResource(int index)
    {
        FaceResource result;
        if (index < _children.Count && index >= 0)
        {
            result = this[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Ability.PAS:547
    public int PreviousFace(int index)
    {
        bool developer;
        int result = index;
        developer = GameTables.GameEngine.Registry.GetDeveloper();
        // { Original assumes the list contains a selectable face. }
        do
        {
            result -= 1;
            if (result < 0)
            {
                result = _children.Count - 1;
            }
        }
        while (!(_children.List[result] != null && (developer || !this[result].DeveloperOnly)));
        return result;
    }

    // Ability.PAS:561
    public int NextFace(int index)
    {
        bool developer;
        int result = index;
        developer = GameTables.GameEngine.Registry.GetDeveloper();
        // { Original assumes the list contains a selectable face. }
        do
        {
            result += 1;
            if (result >= _children.Count)
            {
                result = 0;
            }
        }
        while (!(_children.List[result] != null && (developer || !this[result].DeveloperOnly)));
        return result;
    }

    // intf_P1_07.inc:27

}
