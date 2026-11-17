#nullable enable
using Aow.Formats.Json;

namespace Aow.Game.Saves;

/// <summary>The players of a world map: the stored slots of root.Players.Children.</summary>
public sealed class PlayerSlots : ReferenceDomain
{
    public static PlayerSlots Instance { get; } = new();
    public override string Name => "player-slot";
    public override string Owner => "Players.Children";

    public override IReadOnlySet<long> Collect(JsonObject root)
    {
        HashSet<long> slots = [];
        if (root["Players"] is JsonObject players && players["Children"] is JsonArray children)
        {
            foreach (JsonValue child in children.Items)
            {
                if (child is JsonObject { } player && player["$slot"] is JsonInteger slot)
                {
                    _ = slots.Add(slot.Value);
                }
            }
        }

        return slots;
    }
}
