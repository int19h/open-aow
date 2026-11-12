// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

// the order (AoWE.ProductionOrders.cs), stored as production ID, parameter and group ID
public partial class ProductionItem : EngineObject
{
    public new const int ClassId = 131735;

    public ProductionItem(ECustomNode owner) : base(owner)
    { }

    public ProductionOrder Order { get; set; } = new UnknownOrder(0, 0, 0);

    // Structur.PAS:63
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        var (id, parameter, groupId) = Order.ToFields();
        stream.rwInteger(ref id, 20);
        stream.rwInteger(ref parameter, 21);
        stream.rwInteger(ref groupId, 22);
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            Order = ProductionOrder.FromFields(id, parameter, groupId);
        }
    }

    // Structur.PAS:71
    public virtual string GetName()
    {
        string result;
        if (Owner != null)
        {
            result = ((ProductionQueue)Owner).ProductionControl.GetProductionName(this);
        }
        else
        {
            result = AbilityRules.TranslateResourceString(GameTables.UnknownText);
        }

        return result;
    }

    // Structur.PAS:77
    public string GetNameProgressive()
    {
        string result;
        if (Owner != null)
        {
            result = ((ProductionQueue)Owner).ProductionControl.GetProductionNameProgressive(this);
        }
        else
        {
            result = AbilityRules.TranslateResourceString(GameTables.UnknownText);
        }

        return result;
    }

    // Engine.pas:524

}
