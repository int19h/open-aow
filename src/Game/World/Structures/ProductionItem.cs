// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

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
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        var (kind, parameter, groupId) = Order.ToFields();
        stream.Int32(ref kind, 20);
        stream.Int32(ref parameter, 21);
        stream.Int32(ref groupId, 22);
        if (stream.StorageMode().Contains(StorageModeElement.Read))
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
