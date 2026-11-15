using System.Globalization;
using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Formats.Original;

/// <summary>
/// One map coordinate stored as three original properties and one document member.
/// Width 1 is an unsigned byte, the two's complement byte of an sbyte. Width 4 is a
/// signed 32-bit integer in the range -128 to 127, the range of a MapCoordinate.
/// A component the tree or the document carries is written, and a present zero is written
/// too. An empty coordinate writes no image entry and no document member.
/// </summary>
internal sealed class CoordinateCodec : PropertyCodec
{
    private readonly int _width;
    private readonly bool _signed;
    private readonly IntegerCodec _integers;
    private readonly (string Name, int Id)[] _parts;

    private CoordinateCodec(int width, (string Name, int Id)[] parts)
    {
        _width = width;
        _signed = width != 1;
        _integers = new IntegerCodec(width, signed: _signed);
        _parts = parts;
    }

    internal override bool GroupsEntries => true;

    /// <summary>Every original property id of the coordinate, in part order.</summary>
    internal IReadOnlyList<int> ImageIds
    {
        get
        {
            var ids = new int[_parts.Length];
            for (int i = 0; i < _parts.Length; i++) ids[i] = _parts[i].Id;
            return ids;
        }
    }

    internal static CoordinateCodec Create(JsonObject source, string path)
    {
        if (source["width"] is not JsonInteger width || width.Value is not (1 or 4))
            throw new InvalidOperationException(path + " coordinate width is 1 or 4.");
        if (source["parts"] is not JsonArray parts || parts.Items.Count != 3)
            throw new InvalidOperationException(path + " coordinate has the parts X, Y and MapLevel.");
        string[] expected = ["X", "Y", "MapLevel"];
        var parsed = new (string Name, int Id)[3];
        for (int i = 0; i < 3; i++)
        {
            if (parts.Items[i] is not JsonObject part
                || part["name"] is not JsonString { Value: var name }
                || part["id"] is not JsonInteger id)
                throw new InvalidOperationException(path + " coordinate part " + expected[i] + " names itself and its id.");
            if (name != expected[i])
                throw new InvalidOperationException(path + " part " + i.ToString(CultureInfo.InvariantCulture) + " is " + expected[i] + ".");
            parsed[i] = (name, (int)id.Value);
        }
        if (source["id"] is not JsonInteger property || (int)property.Value != parsed[0].Id)
            throw new InvalidOperationException(path + " coordinate id is the id of X.");
        return new CoordinateCodec((int)width.Value, parsed);
    }

    internal override bool StoresImage(TreeValue value) => value is not RecordNode node || node.Members.Count != 0;

    internal override void WriteMemberImage(TreeValue value, EncodeContext context, ObjectWriter writer, ImageFrame frame)
    {
        if (value is not RecordNode node)
            throw context.Fail(Codes.Type, frame.Property.Name + " is a coordinate.", null, frame.Path);
        foreach (var (name, id) in _parts)
        {
            var found = node.Find(name);
            if (found is null) continue;
            var part = frame.Path.Member(name);
            long number = Require(found, context, name, part);
            var site = new ValueSite { Property = frame.Property, Id = id, Path = part, Owner = frame.Owner };
            writer.Begin(id, frame.Property);
            _integers.Encode(context, new JsonInteger(number), writer.Data, site);
            writer.End();
        }
    }

    internal override TreeValue ReadGroupImage(DecodeContext context, IReadOnlyList<SlotImage> entries, ImageFrame frame)
    {
        var node = new RecordNode();
        foreach (var entry in entries)
        {
            int index = IndexOf(entry.Id);
            if (index < 0)
                throw context.Fail(Codes.UnknownProperty, frame.Property.Name + " has no part " + entry.Id.ToString(CultureInfo.InvariantCulture) + ".", entry.Start, frame.Path);
            string name = _parts[index].Name;
            var site = new ValueSite { Property = frame.Property, Id = entry.Id, Path = frame.Path.Member(name), Owner = frame.Owner };
            node.Add(name, new IntegerNode(_width, _signed, ReadInteger(context, entry.Start, entry.End, name, site)));
        }
        return node;
    }

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site) =>
        throw new InvalidOperationException("A coordinate is stored as one entry per part.");

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site) =>
        throw new InvalidOperationException("A coordinate is stored as one entry per part.");

    internal override void EncodeMember(EncodeContext context, JsonValue value, ObjectWriter writer, ValueSite site)
    {
        if (value is not JsonObject objectValue)
            throw context.Fail(Codes.Type, site.Property.Name + " is a coordinate.", value, site.Path, value.KindName);
        RejectUnknown(objectValue, (member, code, message) => context.Fail(code, message, member.Value, site.Path.Member(member.Name), member.Name));
        foreach (var (name, id) in _parts)
        {
            var member = objectValue.Find(name);
            if (member is null) continue;
            var part = site.Path.Member(name);
            if (member.Value is JsonInteger number) CheckRange(number.Value, name, value => context.Fail(Codes.Range, name + " is outside -128 to 127.", member.Value, part, Text(number.Value), "-128 to 127"));
            var partSite = new ValueSite { Property = site.Property, Id = id, Path = part, Owner = site.Owner, Reader = site.Reader };
            writer.Begin(id, site.Property);
            _integers.Encode(context, member.Value, writer.Data, partSite);
            writer.End();
        }
    }

    internal override JsonValue DecodeGroup(DecodeContext context, IReadOnlyList<SlotImage> entries, ValueSite site)
    {
        var objectValue = new JsonObject();
        var numbers = new long?[3];
        foreach (var entry in entries)
        {
            int index = IndexOf(entry.Id);
            if (index < 0)
                throw context.Fail(Codes.UnknownProperty, site.Property.Name + " has no part " + entry.Id.ToString(CultureInfo.InvariantCulture) + ".", entry.Start, site.Path);
            string name = _parts[index].Name;
            var partSite = new ValueSite { Property = site.Property, Id = entry.Id, Path = site.Path.Member(name), Owner = site.Owner, Builder = site.Builder };
            numbers[index] = ReadInteger(context, entry.Start, entry.End, name, partSite);
        }
        for (int i = 0; i < 3; i++)
            if (numbers[i] is long number) objectValue.Add(_parts[i].Name, new JsonInteger(number));
        return objectValue;
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not RecordNode node) throw site.Mismatch((site.Property?.Name ?? "The value") + " is a coordinate.");
        if (node.Members.Count == 0) return null;
        var json = new JsonObject();
        foreach (var (name, _) in _parts)
        {
            var found = node.Find(name);
            if (found is null) continue;
            var part = site.At(site.Path.Member(name), element: false);
            long number = Require(found, name, message => part.Mismatch(message));
            CheckRange(number, name, stored => part.Fail(Codes.Range, name + " is outside -128 to 127.", null, Text(stored), "-128 to 127"));
            json.Add(name, _integers.WriteTree(new IntegerNode(_width, _signed, number), part)!);
        }
        foreach (var (name, _) in node.Members)
            if (IndexOf(name) < 0)
                throw site.At(site.Path.Member(name), element: false).Fail(Codes.UnknownMember, "The coordinate has no member \"" + name + "\".", value: name);
        return json;
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is not JsonObject objectValue) throw site.Mismatch((site.Property?.Name ?? "The value") + " is a coordinate.", value);
        RejectUnknown(objectValue, (member, code, message) => site.At(site.Path.Member(member.Name), element: false).Fail(code, message, member.Value, member.Name));
        var node = new RecordNode();
        foreach (var (name, _) in _parts)
        {
            var member = objectValue.Find(name);
            if (member is null) continue;
            var part = site.At(site.Path.Member(name), element: false);
            if (_integers.ReadTree(member.Value, part) is not IntegerNode integer)
                throw part.Mismatch(name + " is an integer.", member.Value);
            CheckRange(integer.Value, name, number => part.Fail(Codes.Range, name + " is outside -128 to 127.", member.Value, Text(number), "-128 to 127"));
            node.Add(name, integer);
        }
        return node;
    }

    private int IndexOf(int id)
    {
        for (int i = 0; i < _parts.Length; i++)
            if (_parts[i].Id == id) return i;
        return -1;
    }

    private int IndexOf(string name)
    {
        for (int i = 0; i < _parts.Length; i++)
            if (_parts[i].Name == name) return i;
        return -1;
    }

    private long ReadInteger(DecodeContext context, int start, int end, string name, ValueSite site)
    {
        if (_integers.Decode(context, start, end, site) is not JsonInteger number)
            throw context.Fail(Codes.Type, name + " is an integer.", start, site.Path);
        CheckRange(number.Value, name, value => context.Fail(Codes.Range, name + " is outside -128 to 127.", start, site.Path, Text(value), "-128 to 127"));
        return number.Value;
    }

    private long Require(TreeValue found, EncodeContext context, string name, DocumentNodePath path)
    {
        if (found is not IntegerNode integer || integer.Width != _width || integer.Signed != _signed)
            throw context.Fail(Codes.Type, name + " is an integer.", null, path);
        CheckRange(integer.Value, name, value => context.Fail(Codes.Range, name + " is outside -128 to 127.", null, path, Text(value), "-128 to 127"));
        return integer.Value;
    }

    private long Require(TreeValue found, string name, Func<string, ConversionException> mismatch)
    {
        if (found is not IntegerNode integer || integer.Width != _width || integer.Signed != _signed)
            throw mismatch(name + " is an integer.");
        return integer.Value;
    }

    private void CheckRange(long number, string name, Func<long, ConversionException> fail)
    {
        if (_width == 1 || number is >= -128 and <= 127) return;
        throw fail(number);
    }

    private static void RejectUnknown(JsonObject objectValue, Func<JsonMember, string, string, ConversionException> fail)
    {
        foreach (var member in objectValue.Members)
        {
            if (member.Name is "X" or "Y" or "MapLevel") continue;
            string code = member.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember;
            throw fail(member, code, "The coordinate has no member \"" + member.Name + "\".");
        }
    }
}
