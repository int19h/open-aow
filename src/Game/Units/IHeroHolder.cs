namespace Aow.Game.Units;

/// <summary>
/// A hero list or a hero library, toward its heroes. Callers that test whether a troop's owner is
/// a hero list test this, so a library hero stays in that set after the library stops deriving
/// from the list.
/// </summary>
internal interface IHeroHolder
{
    HeroCollection Heroes { get; }
}
