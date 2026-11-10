#nullable enable
using Aow.Formats;
using Aow.Formats.Json;

namespace Aow.Game.Saves;

/// <summary>A list member ({ "default", "items" }) whose items name identities of a domain.</summary>
public sealed class ReferenceList(string member, ReferenceDomain domain) : ContentRule
{
    public override IEnumerable<string> Members => [member];

    public override IEnumerable<FormatDiagnostic> Check(ContentScope scope, JsonObject value, DocPath path)
    {
        if (value[member] is not JsonObject list || list["items"] is not JsonArray items) yield break;
        for (int i = 0; i < items.Items.Count; i++)
            if (items.Items[i] is JsonInteger target && !scope.Identities(domain).Contains(target.Value))
                yield return Reference.Dangling(scope, target, path.Member(member).Member("items").Item(i), domain);
    }
}
