#nullable enable
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Schema;

namespace Aow.Game.Saves;

/// <summary>
/// Walks a document and applies the rules registered for each object's class and its
/// ancestors. Objects without $class have their class fixed by schema context and carry no
/// mapped rules in version 1, apart from a root whose profile fixes its class.
/// </summary>
internal sealed class GameContentValidator : IDocumentValidator
{
    public string Name => "unit-content";

    public IEnumerable<FormatDiagnostic> Validate(ValidationContext context)
    {
        JsonObject root = context.Root;
        ContentScope scope = new(context, root, rootPath: DocumentNodePath.Root);
        List<FormatDiagnostic> findings = [];
        SchemaClass? rootClass = root["$class"] is JsonString name ? GameFiles.Schema.Find(name.Value) : GameFiles.Schema.Find(context.Profile.RootFamily);
        Visit(scope, value: root, classValue: rootClass, path: DocumentNodePath.Root, findings);
        return findings;
    }

    private static void Visit(ContentScope scope, JsonValue value, SchemaClass? classValue, DocumentNodePath path, List<FormatDiagnostic> findings)
    {
        switch (value)
        {
            case JsonObject objectValue:
                // A nested document (a campaign's Map) is a map of its own: its references
                // resolve inside its root.
                if (objectValue["$container"] is JsonObject && objectValue["root"] is JsonObject nested && objectValue["$class"] is null)
                {
                    Visit(new ContentScope(scope.Context, root: nested, rootPath: path.Member("root")), value: nested, classValue: null, path: path.Member("root"), findings);
                    break;
                }
                if (objectValue["$class"] is JsonString name)
                {
                    classValue = GameFiles.Schema.Find(name.Value);
                }

                for (SchemaClass? c = classValue; c is not null; c = c.Parent)
                {
                    if (FormatMapping.All.TryGetValue(c.Name, out ContentRule[]? rules))
                    {
                        foreach (ContentRule rule in rules)
                        {
                            findings.AddRange(rule.Check(scope, value: objectValue, path));
                        }
                    }
                }

                foreach (JsonMember member in objectValue.Members)
                {
                    if (member.Value is JsonObject or JsonArray && member.Name != "Fields")
                    {
                        Visit(scope, member.Value, classValue: null, path: path.Member(member.Name), findings);
                    }
                }

                break;
            case JsonArray array:
                for (int i = 0; i < array.Items.Count; i++)
                {
                    if (array.Items[i] is JsonObject or JsonArray)
                    {
                        Visit(scope, value: array.Items[i], classValue: null, path: path.Item(i), findings);
                    }
                }

                break;
            default:
                break;
        }
    }
}
