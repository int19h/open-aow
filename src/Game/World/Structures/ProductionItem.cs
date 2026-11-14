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

    protected sealed override bool AcceptsOwner(ECustomNode owner) => owner is ProductionQueue;

    void IOwnedSink<ProductionQueue>.SetOwner(ProductionQueue owner) => SetOwner(owner);

    public new const int ClassId = 131735;

    public ProductionItem(ECustomNode owner) : base(owner)
    { }

    public ProductionOrder Order { get; set; } = new UnknownOrder(0, 0, 0);

    // Structur.PAS:63
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        var (kind, parameter, groupId) = Order.ToFields();
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
        string result;
        if (Owner != null)
        {
            result = Owner.ProductionControl.GetProductionName(this);
        }
        else
        {
            result = AbilityRules.TranslateResourceString(GetEngine(), GameTables.UnknownText);
        }

        return result;
    }

    // Structur.PAS:77
    public string GetNameProgressive()
    {
        string result;
        if (Owner != null)
        {
            result = Owner.ProductionControl.GetProductionNameProgressive(this);
        }
        else
        {
            result = AbilityRules.TranslateResourceString(GetEngine(), GameTables.UnknownText);
        }

        return result;
    }

    // Engine.pas:524

}
