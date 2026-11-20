#nullable enable
using Aow.Game.World.Structures;

namespace Aow.Game.Saves;

/// <summary>TProductionItem: the 9.6-6 production orders, keyed by Kind.</summary>
public sealed class ProductionOrderCases : KindCases
{
    public override IEnumerable<string> Members => ["Kind", "Parameter", "GroupId"];

    public override IReadOnlyDictionary<string, long> Written(long key, IReadOnlyDictionary<string, long> stored)
    {
        ProductionOrder order = ProductionOrder.FromFields((int)key, parameter: (int)stored.GetValueOrDefault("Parameter"), groupId: (int)stored.GetValueOrDefault("GroupId"));
        (int kind, int parameter, int groupId) = order.ToFields();
        return new Dictionary<string, long> { ["Kind"] = kind, ["Parameter"] = parameter, ["GroupId"] = groupId };
    }
}
