// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Units;

public partial class FaceResourceList : EResourceList
{
    // Ability.PAS:520
    public FaceResourceList(ECustomNode owner) : base(owner)
    {
        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, ControlStyleElement.ArrayMode));
    }

    // Ability.PAS:526
    public int IndexOfFaceImage(LibraryImage Image)
    {
        FaceResource Face;
        int Result = GetCount();
        while (Result != 0)
        {
            Result -= 1;
            Face = (FaceResource)FChildren.List[Result];
            if (Face != null && Face.GetFaceImage() == Image)
            {
                return Result;
            }
        }

        return -1;
    }

    // Ability.PAS:540
    public FaceResource GetFaceResource(int Index)
    {
        FaceResource Result;
        if (Index < FChildren.Count && Index >= 0)
        {
            Result = (FaceResource)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Ability.PAS:547
    public int PrevFace(int Index)
    {
        bool Developer;
        int Result = Index;
        Developer = GameTables.AoWEngine.FRegistry.GetDeveloper();
        // { Original assumes the list contains a selectable face. }
        do
        {
            Result -= 1;
            if (Result < 0)
            {
                Result = FChildren.Count - 1;
            }
        }
        while (!(FChildren.List[Result] != null && (Developer || !((FaceResource)FChildren.List[Result]).FDeveloperOnly)));
        return Result;
    }

    // Ability.PAS:561
    public int NextFace(int Index)
    {
        bool Developer;
        int Result = Index;
        Developer = GameTables.AoWEngine.FRegistry.GetDeveloper();
        // { Original assumes the list contains a selectable face. }
        do
        {
            Result += 1;
            if (Result >= FChildren.Count)
            {
                Result = 0;
            }
        }
        while (!(FChildren.List[Result] != null && (Developer || !((FaceResource)FChildren.List[Result]).FDeveloperOnly)));
        return Result;
    }

    // intf_P1_07.inc:27

}
