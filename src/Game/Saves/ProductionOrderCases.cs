#nullable enable
using Aow.Game.World.Structures;

namespace Aow.Game.Saves;

/// <summary>TProductionItem: the 9.6-6 production orders, keyed by ProductionID.</summary>
public sealed class ProductionOrderCases : KindCases
{
    public override IEnumerable<string> Members => ["ProductionID", "Parameter", "GroupID"];

    public override IReadOnlyDictionary<string, long> Written(long key, IReadOnlyDictionary<string, long> stored)
    {
        var order = ProductionOrder.FromFields((int)key, (int)stored.GetValueOrDefault("Parameter"), (int)stored.GetValueOrDefault("GroupID"));
        var (id, parameter, groupId) = order.ToFields();
        return new Dictionary<string, long> { ["ProductionID"] = id, ["Parameter"] = parameter, ["GroupID"] = groupId };
    }
}
