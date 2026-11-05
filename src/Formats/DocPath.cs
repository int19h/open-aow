using System.Text;

namespace Aow.Formats;

/// <summary>A JSON path such as root.Players.Children[1].Leader, built only when a diagnostic needs it.</summary>
public sealed class DocPath
{
    private readonly DocPath? parent;
    private readonly string? name;
    private readonly int index;

    private DocPath(DocPath? parent, string? name, int index)
    {
        this.parent = parent;
        this.name = name;
        this.index = index;
    }

    public static DocPath Root { get; } = new(null, "root", -1);
    public static DocPath Envelope { get; } = new(null, "$", -1);

    public static DocPath Named(string name) => new(null, name, -1);

    public DocPath Member(string member) => new(this, member, -1);

    public DocPath Item(int item) => new(this, null, item);

    public override string ToString()
    {
        var parts = new List<DocPath>();
        for (var p = this; p is not null; p = p.parent) parts.Add(p);
        parts.Reverse();
        var builder = new StringBuilder();
        foreach (var p in parts)
        {
            if (p.name is not null)
            {
                if (builder.Length > 0) builder.Append('.');
                builder.Append(p.name);
            }
            else builder.Append('[').Append(p.index).Append(']');
        }
        return builder.ToString();
    }
}
