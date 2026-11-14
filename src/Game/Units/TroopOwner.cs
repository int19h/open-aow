using Aow.Engine.Nodes;

namespace Aow.Game.Units;

/// <summary>
/// The owner of a troop: the army it stands in, a unit list that holds troops apart from an army
/// (generated defenders and rebels, campaign transfer units), a unit user list a caller has set
/// as the owner, the hero list of a hero that no army holds (a player's heroes, a campaign
/// transfer), or a hero library. A troop takes no other owner, so a read of its owner is one of
/// these or empty.
/// </summary>
public union TroopOwner(Army, UnitList, HeroLibrary, HeroList, UnitUserList)
{
    /// <summary>
    /// The owner a troop stores, read from the untyped owner. A troop admits only these kinds, so
    /// another kind is an error in the troop and throws.
    /// </summary>
    internal static TroopOwner Of(ECustomNode owner) => owner switch
    {
        null => default,
        Army army => army,
        UnitList units => units,
        UnitUserList users => users,
        HeroLibrary library => library,
        HeroList heroes => heroes,
        _ => throw new InvalidCastException("A troop does not take an owner of type " + owner.GetType().Name + "."),
    };

    /// <summary>The owner as the node the untyped owner stores.</summary>
    public ECustomNode Node => (ECustomNode)Value;
}
