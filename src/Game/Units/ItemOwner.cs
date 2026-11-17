using Aow.Engine.Nodes;

namespace Aow.Game.Units;

/// <summary>
/// The owner of an item. It is the item list the item lies in (items on the ground, a prisoners'
/// army, a campaign transfer, an item hex or an exploration site), a hero's inventory, the items
/// a hero wears, or an item library. An item takes no other owner, so a read of its owner is one
/// of these or empty.
/// </summary>
public readonly union ItemOwner(ItemList, HeroInventory, HeroItems, ItemLibrary)
{
    /// <summary>
    /// The owner an item stores, read from the untyped owner. An item admits only these kinds, so
    /// another kind is an error in the item and throws.
    /// </summary>
    internal static ItemOwner Of(CustomNode owner) => owner switch
    {
        null => default,
        HeroInventory inventory => inventory,
        HeroItems worn => worn,
        ItemLibrary library => library,
        ItemList items => items,
        _ => throw new InvalidCastException("An item does not take an owner of type " + owner.GetType().Name + "."),
    };

    /// <summary>The owner as the node the untyped owner stores.</summary>
    public readonly CustomNode Node => (CustomNode)Value;

    /// <summary>The owner as the observable node every admitted kind is. Null stays null.</summary>
    public readonly ObservableNode Observable => (ObservableNode)Value;
}
