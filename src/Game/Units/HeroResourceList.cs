// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

public partial class HeroResourceList : ResourceCollection, IChildList<HeroResource>
{
    // The hero resource in slot index, read as the original reads the list, without a range check.
    // AcceptsResource admits only a HeroResource, so the cast never fails.
    public new HeroResource this[int index] => (HeroResource)GetChild(index);

    protected sealed override bool AcceptsResource(EngineResource resource) => resource is HeroResource;

    int IChildSink<HeroResource>.AddChild(HeroResource child) => AddChild(child);

    // Hero.PAS:2360
    public HeroResourceList(CustomNode owner) : base(owner)
    {
        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, ControlStyleElement.ArrayMode));
    }

    // Hero.PAS:2363
    public HeroResource GetHeroResource(int index)
    {
        HeroResource result;
        if (index >= 0 && index < Count)
        {
            result = this[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Hero.PAS:2369
    public StringList CreateHeroResourceStrings(Set<HeroType> heroTypes)
    {
        HeroResource resource;
        StringList list;
        list = new StringList();
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            resource = this[i];
            if (resource != null && (GetEngine().User.Contains(EngineUserElement.Designer) || !resource.Hidden) && heroTypes.Contains(resource.HeroType))
            {
                list.AddObject(resource.Name, IntToObject(i));
            }
        }

        return list;
    }

    // Hero.PAS:2382
    public int Find(int race, HeroType heroType)
    {
        int i;
        HeroResource resource;
        i = 0;
        int result = -1;
        while (i < Count && result == -1)
        {
            resource = this[i];
            if (resource != null && race == resource.Race && heroType == resource.HeroType)
            {
                result = i;
            }

            i += 1;
        }

        return result;
    }

    // intf_P2_07.inc:353

}
