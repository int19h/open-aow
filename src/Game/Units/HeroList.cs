// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

public partial class HeroList : EReferenceNode, IChildList<Hero>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a Hero, so the cast never fails.
    public Hero this[int index] => (Hero)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is Hero;

    int IChildSink<Hero>.AddChild(Hero child) => AddChild(child);

    public HeroList(ECustomNode owner) : base(owner)
    { }

    // Hero.PAS:2395
    public Hero FindHero(int id)
    {
        int I;
        I = 0;
        Hero result = null;
        while (I != Count && result == null)
        {
            if (id == this[I].UnitId)
            {
                result = this[I];
            }

            I += 1;
        }

        return result;
    }

    // Hero.PAS:2407
    public Hero GetHero(int index)
    {
        Hero result;
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

    // Engine.pas:631

}
