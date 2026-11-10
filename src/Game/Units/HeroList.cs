// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

public partial class HeroList : EReferenceNode
{
    public HeroList(ECustomNode owner) : base(owner)
    { }

    // Hero.PAS:2395
    public Hero FindHero(int ID)
    {
        int I;
        I = 0;
        Hero Result = null;
        while (I != FChildren.Count && Result == null)
        {
            if (ID == ((AbstractUnit)FChildren.List[I]).FUnitID)
            {
                Result = (Hero)FChildren.List[I];
            }

            I += 1;
        }

        return Result;
    }

    // Hero.PAS:2407
    public Hero GetHero(int Index)
    {
        Hero Result;
        if (Index >= 0 && Index < FChildren.Count)
        {
            Result = (Hero)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:631

}
