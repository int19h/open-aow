// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

public partial class HeroList : ReferenceNode, IChildList<Hero>, IHeroHolder
{
    // The hero in slot index, without a range check.
    public Hero this[int index] => Heroes[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is Hero;

    int IChildSink<Hero>.AddChild(Hero child) => AddChild(child);

    public HeroList(CustomNode owner) : base(owner)
    { }

    /// <summary>The heroes of this list, in their stored order.</summary>
    public HeroCollection Heroes => new(this);

    // Hero.PAS:2395
    public Hero FindHero(int id) => Heroes.FindHero(id);

    // Hero.PAS:2407
    public Hero GetHero(int index) => Heroes.GetHero(index);

    // Engine.pas:631

}
