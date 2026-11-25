using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Aow.Analyzers;

/// <summary>
/// One allowed reference from engine code to a type outside the boundary: the member that holds
/// it and the symbol it names, both as documentation ids.
/// </summary>
public readonly record struct EngineReference(string Member, string Target);

/// <summary>An allow list entry with the line of the file that holds it.</summary>
public sealed record EngineAllowEntry(EngineReference Reference, Location Location);

/// <summary>A line of the allow list that names no reference, with its text and location.</summary>
public sealed record EngineMalformedLine(string Line, Location Location);

/// <summary>
/// The rule of issue 311: code in the engine files (the files the <c>aow_engine_boundary</c>
/// option of .editorconfig marks) refers only to engine types, to the product assemblies the
/// option names and to assemblies outside the product.
/// </summary>
public static class EngineBoundaryRules
{
    /// <summary>
    /// The .editorconfig option that marks a file as engine code. Its value names the product
    /// assemblies engine code refers to besides the engine files, separated by commas.
    /// </summary>
    public const string BoundaryOption = "aow_engine_boundary";

    /// <summary>The name of the allow-list file the analyzer reads from the additional files.</summary>
    public const string AllowListFileName = "EngineBoundaryAllowList.csv";

    /// <summary>The header line of the allow list.</summary>
    public const string Header = "member,target";

    /// <summary>
    /// The entries of every additional file named <see cref="AllowListFileName"/>, with the
    /// header and blank lines left out. A later line that repeats an entry is dropped.
    /// </summary>
    public static ImmutableArray<EngineAllowEntry> ParseAllowList(ImmutableArray<AdditionalText> files, CancellationToken cancellationToken)
        => ParseFiles(files, cancellationToken).Entries;

    /// <summary>
    /// The lines of every additional file named <see cref="AllowListFileName"/> that name no
    /// reference: anything but the header, a blank line or two fields.
    /// </summary>
    public static ImmutableArray<EngineMalformedLine> MalformedLines(ImmutableArray<AdditionalText> files, CancellationToken cancellationToken)
        => ParseFiles(files, cancellationToken).Malformed;

    private static (ImmutableArray<EngineAllowEntry> Entries, ImmutableArray<EngineMalformedLine> Malformed) ParseFiles(ImmutableArray<AdditionalText> files, CancellationToken cancellationToken)
    {
        ImmutableArray<EngineAllowEntry>.Builder entries = ImmutableArray.CreateBuilder<EngineAllowEntry>();
        ImmutableArray<EngineMalformedLine>.Builder malformed = ImmutableArray.CreateBuilder<EngineMalformedLine>();
        HashSet<EngineReference> known = [];
        foreach (AdditionalText file in files)
        {
            if (!string.Equals(Path.GetFileName(file.Path), AllowListFileName, comparisonType: StringComparison.OrdinalIgnoreCase)
                || file.GetText(cancellationToken) is not SourceText text)
            {
                continue;
            }
            foreach (TextLine line in text.Lines)
            {
                string content = line.ToString().Trim();
                Location location = Location.Create(file.Path, textSpan: line.Span, lineSpan: text.Lines.GetLinePositionSpan(line.Span));
                if (content.Length == 0 || content == Header)
                {
                    continue;
                }
                if (ParseLine(content) is not [string member, string target])
                {
                    malformed.Add(new EngineMalformedLine(content, location));
                    continue;
                }
                EngineReference reference = new(member, Target: target);
                if (known.Add(reference))
                {
                    entries.Add(new EngineAllowEntry(reference, location));
                }
            }
        }
        return (entries.ToImmutable(), malformed.ToImmutable());
    }

    /// <summary>The fields of one CSV line: commas separate fields, a quoted field holds commas, and two quotes stand for one.</summary>
    public static ImmutableArray<string> ParseLine(string line)
    {
        ImmutableArray<string>.Builder fields = ImmutableArray.CreateBuilder<string>();
        StringBuilder field = new();
        bool isQuoted = false;
        for (int i = 0; i < line.Length; i++)
        {
            char current = line[i];
            if (isQuoted)
            {
                if (current != '"')
                {
                    _ = field.Append(current);
                }
                else if (i + 1 < line.Length && line[i + 1] == '"')
                {
                    _ = field.Append('"');
                    i++;
                }
                else
                {
                    isQuoted = false;
                }
            }
            else if (current == '"')
            {
                isQuoted = true;
            }
            else if (current == ',')
            {
                fields.Add(field.ToString());
                _ = field.Clear();
            }
            else
            {
                _ = field.Append(current);
            }
        }
        fields.Add(field.ToString());
        return fields.ToImmutable();
    }

    /// <summary>One CSV line for an entry, with a field quoted when it holds a comma or a quote.</summary>
    public static string FormatLine(EngineReference reference) => Quote(reference.Member) + "," + Quote(reference.Target);

    private static string Quote(string field)
        => field.Contains(',') || field.Contains('"') ? "\"" + field.Replace("\"", "\"\"", comparisonType: StringComparison.Ordinal) + "\"" : field;

    /// <summary>The product assemblies the option's value names, or null when the option is not set on the tree.</summary>
    public static ImmutableHashSet<string>? BoundaryAssemblies(AnalyzerOptions options, SyntaxTree tree)
        => options.AnalyzerConfigOptionsProvider.GetOptions(tree).TryGetValue(BoundaryOption, out string? value)
            ? [.. value.Split(',').Select(name => name.Trim()).Where(name => name.Length > 0)]
            : null;

    /// <summary>
    /// The symbol a name refers to, as the allow list names it: an alias becomes its target, an
    /// array or a pointer its element type, a reduced extension method the method it reduces
    /// from, and a constructed symbol its definition. A local, a parameter, a namespace, a label
    /// and a type parameter refer to nothing the rule checks, so they give null.
    /// </summary>
    public static ISymbol? Target(ISymbol? symbol) => symbol switch
    {
        IAliasSymbol alias => Target(alias.Target),
        IArrayTypeSymbol array => Target(array.ElementType),
        IPointerTypeSymbol pointer => Target(pointer.PointedAtType),
        INamedTypeSymbol type => type.OriginalDefinition,
        IMethodSymbol { MethodKind: MethodKind.LocalFunction or MethodKind.AnonymousFunction } => null,
        IMethodSymbol method => (method.ReducedFrom ?? method).OriginalDefinition,
        IPropertySymbol or IFieldSymbol or IEventSymbol => symbol.OriginalDefinition,
        _ => null,
    };

    /// <summary>The type that owns a target: the type itself, or the type that declares the member.</summary>
    public static INamedTypeSymbol? OwningType(ISymbol target) => target as INamedTypeSymbol ?? target.ContainingType;

    /// <summary>
    /// Whether engine code may refer to a type: an engine type (declared only in engine files),
    /// a type of an assembly the option names, and a type of an assembly outside the product
    /// (any assembly whose name is not <c>Aow</c> and does not start with <c>Aow.</c>).
    /// </summary>
    public static bool IsAllowed(INamedTypeSymbol type, Compilation compilation, AnalyzerOptions options, ImmutableHashSet<string> assemblies)
    {
        INamedTypeSymbol outermost = type;
        while (outermost.ContainingType is INamedTypeSymbol containing)
        {
            outermost = containing;
        }
        IAssemblySymbol? assembly = outermost.ContainingAssembly;
        if (assembly is null)
        {
            return true;
        }
        if (SymbolEqualityComparer.Default.Equals(assembly, compilation.Assembly))
        {
            ImmutableArray<SyntaxReference> declarations = outermost.DeclaringSyntaxReferences;
            return declarations.Length > 0 && declarations.All(declaration => BoundaryAssemblies(options, tree: declaration.SyntaxTree) is not null);
        }
        string name = assembly.Name;
        return assemblies.Contains(name) || !(name == "Aow" || name.StartsWith("Aow.", comparisonType: StringComparison.Ordinal));
    }

    /// <summary>The documentation id of a symbol, or its display string when it has none.</summary>
    public static string Id(ISymbol symbol) => DocumentationCommentId.CreateDeclarationId(symbol) ?? symbol.ToDisplayString();
}
