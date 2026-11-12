using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;

namespace Aow.Formats.Migrations;

/// <summary>
/// A document from before a property rename stores the property under its old name (the
/// property's oldNames in schema.json). This pass gives each such member its current name. It
/// walks the current schema the way the record step walks version 2, so a property keeps the
/// class that owns it. It runs on every loaded document, so an older document of the current
/// version loads as well as a migrated one.
/// </summary>
internal static class MemberNameMigration
{
    /// <summary>Renames the old members of the document. True when it renamed one.</summary>
    internal static bool Apply(JsonObject document)
    {
        if (document["root"] is not JsonObject root) return false;
        if (RootClass(document, root) is not SchemaClass rootClass) return false;
        var walk = new Walk();
        walk.Visit(root, rootClass);
        return walk.RenamedCount > 0;
    }

    private static SchemaClass? RootClass(JsonObject document, JsonObject root)
    {
        var schema = FormatSchema.Current;
        if (root["$class"] is JsonString name) return schema.Find(name.Value);
        if (document["$container"] is JsonObject container && container["profile"] is JsonString profile
            && FileProfile.Find(profile.Value) is { } found)
            return schema.Find(found.RootFamily);
        return null;
    }

    private sealed class Walk
    {
        internal int RenamedCount { get; private set; }

        internal void Visit(JsonObject objectValue, SchemaClass classValue)
        {
            foreach (var property in classValue.Properties)
            {
                var member = objectValue.Find(property.Name) ?? Renamed(objectValue, property);
                if (member is null) continue;
                if (property.IsDynamic)
                {
                    if (member.Value is JsonArray array)
                        foreach (var item in array.Items) VisitDynamic(item, property);
                    continue;
                }
                Descend(member, classValue, property);
            }
        }

        // Replaces the first member stored under an old name with the same value under the current name.
        private JsonMember? Renamed(JsonObject objectValue, SchemaProperty property)
        {
            foreach (string old in property.OldNames)
            {
                int index = objectValue.Members.FindIndex(member => member.Name == old);
                if (index < 0) continue;
                var found = objectValue.Members[index];
                var renamed = new JsonMember(property.Name, found.Value) { NameSpan = found.NameSpan };
                objectValue.Members[index] = renamed;
                RenamedCount++;
                return renamed;
            }
            return null;
        }

        private void VisitDynamic(JsonValue item, SchemaProperty property)
        {
            if (item is not JsonObject objectValue) return;
            var schema = FormatSchema.Current;
            if (property.Codec is FixedObjectCodec fixedObject && schema.Find(fixedObject.ClassName) is { } fixedClass)
                Visit(objectValue, fixedClass);
            else if (property.Codec is CreatedObjectCodec && objectValue["$class"] is JsonString name && schema.Find(name.Value) is { } createdClass)
                Visit(objectValue, createdClass);
        }

        private void Descend(JsonMember member, SchemaClass ownerClass, SchemaProperty property)
        {
            var schema = FormatSchema.Current;
            switch (property.Codec)
            {
                case FixedObjectCodec fixedObject when member.Value is JsonObject nested && schema.Find(fixedObject.ClassName) is { } fixedClass:
                    Visit(nested, fixedClass);
                    break;
                case CreatedObjectCodec when member.Value is JsonObject created && created["$class"] is JsonString createdName
                    && schema.Find(createdName.Value) is { } createdClass:
                    Visit(created, createdClass);
                    break;
                case ChildrenCodec when member.Value is JsonArray children:
                    foreach (var child in children.Items)
                        if (child is JsonObject childObject && childObject["$class"] is JsonString childName && schema.Find(childName.Value) is { } childClass)
                            Visit(childObject, childClass);
                    break;
                case MapFieldsCodec when member.Value is JsonArray rows && schema.Find(ownerClass.FieldClass ?? "MapField") is { } fieldClass:
                    foreach (var row in rows.Items)
                        if (row is JsonArray cells)
                            foreach (var cell in cells.Items)
                                if (cell is JsonObject fieldObject) Visit(fieldObject, fieldClass);
                    break;
                case CampaignMapCodec when member.Value is JsonObject map && map["root"] is JsonObject campaignRoot:
                    var campaignClass = campaignRoot["$class"] is JsonString nestedName ? schema.Find(nestedName.Value) : schema.Find("AoWHSMap");
                    if (campaignClass is not null) Visit(campaignRoot, campaignClass);
                    break;
            }
        }
    }
}
