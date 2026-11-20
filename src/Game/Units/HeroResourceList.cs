#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.Units;

public class HeroResourceList : ResourceCollection, IChildList<HeroResource>
{
    // The hero resource in slot index, read as the original reads the list, without a range check.
    // AcceptsResource admits only a HeroResource, so the cast never fails.
    public new HeroResource this[int index] => GetChild<HeroResource>(index);

    protected sealed override bool AcceptsResource(EngineResource resource) => resource is HeroResource;

    int IChildSink<HeroResource>.AddChild(HeroResource child) => AddChild(child);

    // Hero.PAS:2360
    public HeroResourceList(CustomNode owner) : base(owner) => SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, ControlStyleElement.ArrayMode));

    // Hero.PAS:2363
    public HeroResource GetHeroResource(int index) => index >= 0 && index < Count ? this[index] : null;

    // Hero.PAS:2369
    public DelphiStringList CreateHeroResourceStrings(Set<HeroType> heroTypes)
    {
        DelphiStringList list = new();
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            HeroResource resource = this[i];
            if (resource is not null && (GetEngine().User.Contains(EngineUserElement.Designer) || !resource.Hidden) && heroTypes.Contains(resource.HeroType))
            {
                _ = list.AddObject(resource.Name, item: IntToObject(i));
            }
        }

        return list;
    }

    // Hero.PAS:2382
    public int Find(int race, HeroType heroType)
    {
        int i = 0;
        int result = -1;
        while (i < Count && result == -1)
        {
            HeroResource resource = this[i];
            if (resource is not null && race == resource.Race && heroType == resource.HeroType)
            {
                result = i;
            }

            i += 1;
        }

        return result;
    }
}
