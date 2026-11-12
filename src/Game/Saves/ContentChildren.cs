#nullable enable
using Aow.Formats;
using Aow.Formats.Json;

namespace Aow.Game.Saves;

internal static class ContentChildren
{
    public static int Count(ContentScope scope, JsonObject value, string family)
    {
        if (value["Children"] is not JsonArray children) return 0;
        int count = 0;
        foreach (var child in children.Items)
            if (child is JsonObject c && c["$class"] is JsonString name && GameFiles.Schema.Find(name.Value) is { } classValue && classValue.InheritsFrom(family)) count++;
        return count;
    }
}
