// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

// the order (AoWE.ProductionOrders.cs), stored as production ID, parameter and group ID
public partial class ProductionItem : EObject
{
    public new const int ClassID = 131735;

    public ProductionItem(ECustomNode owner) : base(owner)
    { }

    public ProductionOrder Order = new UnknownOrder(0, 0, 0);

    // Structur.PAS:63
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        var (id, parameter, groupId) = Order.ToFields();
        Stream.rwInteger(ref id, 20);
        Stream.rwInteger(ref parameter, 21);
        Stream.rwInteger(ref groupId, 22);
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            Order = ProductionOrder.FromFields(id, parameter, groupId);
        }
    }

    // Structur.PAS:71
    public virtual string GetName()
    {
        string Result;
        if (Owner != null)
        {
            Result = ((ProductionQueue)Owner).FProductionControl.GetProductionName(this);
        }
        else
        {
            Result = AbilityRules.TranslateRStr(GameTables.UnknownRStr);
        }

        return Result;
    }

    // Structur.PAS:77
    public string GetNameProgressive()
    {
        string Result;
        if (Owner != null)
        {
            Result = ((ProductionQueue)Owner).FProductionControl.GetProductionNameProgressive(this);
        }
        else
        {
            Result = AbilityRules.TranslateRStr(GameTables.UnknownRStr);
        }

        return Result;
    }

    // Engine.pas:524

}
