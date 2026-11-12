// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

public partial class HeroList : EReferenceNode
{
    public HeroList(ECustomNode owner) : base(owner)
    { }

    // Hero.PAS:2395
    public Hero FindHero(int id)
    {
        int I;
        I = 0;
        Hero result = null;
        while (I != _children.Count && result == null)
        {
            if (id == ((Troop)_children.List[I]).UnitId)
            {
                result = (Hero)_children.List[I];
            }

            I += 1;
        }

        return result;
    }

    // Hero.PAS:2407
    public Hero GetHero(int index)
    {
        Hero result;
        if (index >= 0 && index < _children.Count)
        {
            result = (Hero)_children.List[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Engine.pas:631

}
