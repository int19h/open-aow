using System.Text;

namespace Aow.Formats;

/// <summary>A JSON path such as root.Players.Children[1].Leader, built only when a diagnostic needs it.</summary>
public sealed class DocumentNodePath
{
    private readonly DocumentNodePath? _parent;
    private readonly string? _name;
    private readonly int _index;

    private DocumentNodePath(DocumentNodePath? parent, string? name, int index)
    {
        this._parent = parent;
        this._name = name;
        this._index = index;
    }

    public static DocumentNodePath Root { get; } = new(null, "root", -1);
    public static DocumentNodePath Envelope { get; } = new(null, "$", -1);

    public static DocumentNodePath Named(string name) => new(null, name, -1);

    public DocumentNodePath Member(string member) => new(this, member, -1);

    public DocumentNodePath Item(int item) => new(this, null, item);

    public override string ToString()
    {
        var parts = new List<DocumentNodePath>();
        for (var p = this; p is not null; p = p._parent) parts.Add(p);
        parts.Reverse();
        var builder = new StringBuilder();
        foreach (var p in parts)
        {
            if (p._name is not null)
            {
                if (builder.Length > 0) builder.Append('.');
                builder.Append(p._name);
            }
            else builder.Append('[').Append(p._index).Append(']');
        }
        return builder.ToString();
    }
}
