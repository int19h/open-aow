// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

// the order (AoWE.ProductionOrders.cs), stored as production ID, parameter and group ID
public partial class ProductionItem : EngineObject, IOwned<ProductionQueue>, IOwnedSink<ProductionQueue>
{
    // The queue the item waits in. AcceptsOwner admits only a ProductionQueue, so the cast never fails.
    public new ProductionQueue Owner { get => (ProductionQueue)base.Owner; set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(CustomNode owner) => owner is ProductionQueue;

    void IOwnedSink<ProductionQueue>.SetOwner(ProductionQueue owner) => SetOwner(owner);

    public new const int ClassId = 131735;

    public ProductionItem(CustomNode owner) : base(owner)
    { }

    public ProductionOrder Order { get; set; } = new UnknownOrder(0, 0, 0);

    // Structur.PAS:63
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        (int kind, int parameter, int groupId) = Order.ToFields();
        kind = persister.Int32("Kind", value: kind);
        parameter = persister.Int32("Parameter", value: parameter);
        groupId = persister.Int32("GroupId", value: groupId);
        // The translate stream's mode holds read, so the original sets the order there too.
        if (persister.Loading || PersistSupport.Translating(persister))
        {
            Order = ProductionOrder.FromFields(kind, parameter, groupId);
        }
    }

    // Structur.PAS:71
    public virtual string GetName()
    {
        string result = Owner != null
            ? Owner.ProductionControl.GetProductionName(this)
            : AbilityRules.TranslateResourceString(GetEngine(), GameTables.UnknownText);
        return result;
    }

    // Structur.PAS:77
    public string GetNameProgressive()
    {
        string result = Owner != null
            ? Owner.ProductionControl.GetProductionNameProgressive(this)
            : AbilityRules.TranslateResourceString(GetEngine(), GameTables.UnknownText);
        return result;
    }

    // Engine.pas:524

}
