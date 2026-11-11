// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

public partial class HeroResourceList : EResourceList
{
    // Hero.PAS:2360
    public HeroResourceList(ECustomNode owner) : base(owner)
    {
        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, ControlStyleElement.ArrayMode));
    }

    // Hero.PAS:2363
    public HeroResource GetHeroResource(int Index)
    {
        HeroResource Result;
        if (Index >= 0 && Index < FChildren.Count)
        {
            Result = (HeroResource)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Hero.PAS:2369
    public StringList CreateHeroResourceStrings(Set<HeroType> HeroTypes)
    {
        HeroResource Resource;
        StringList List;
        List = new StringList();
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            Resource = (HeroResource)FChildren.List[I];
            if (Resource != null && (GameTables.AoWEngine.FUser.Contains(EngineUserElement.Designer) || !Resource.FHidden) && HeroTypes.Contains(Resource.FHeroType))
            {
                List.AddObject(Resource.FName, IntToObject(I));
            }
        }

        return List;
    }

    // Hero.PAS:2382
    public int Find(int Race, HeroType HeroType)
    {
        int I;
        HeroResource Resource;
        I = 0;
        int Result = -1;
        while (I < FChildren.Count && Result == -1)
        {
            Resource = (HeroResource)FChildren.List[I];
            if (Resource != null && Race == Resource.FRace && HeroType == Resource.FHeroType)
            {
                Result = I;
            }

            I += 1;
        }

        return Result;
    }

    // intf_P2_07.inc:353

}
