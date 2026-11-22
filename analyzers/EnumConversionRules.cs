using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace Aow.Analyzers;

/// <summary>How an integer becomes an enum value (README.md, AOW0002).</summary>
public enum EnumConversionKind
{
    /// <summary>A cast of a constant integer, such as <c>(TerrainType)6</c>.</summary>
    ConstantCast,

    /// <summary>A cast of an integer value, such as <c>(TerrainType)index</c>.</summary>
    Cast,

    /// <summary>A cast of one enum type to another, which goes through the integer.</summary>
    EnumToEnumCast,

    /// <summary>A cast of a boxed integer to an enum or an enum type parameter.</summary>
    UnboxingCast,

    /// <summary>A pointer cast to a pointer to an enum.</summary>
    PointerCast,

    /// <summary><c>Enum.ToObject</c>.</summary>
    EnumToObject,

    /// <summary><c>Enum.Parse</c> or <c>Enum.TryParse</c>, which accept a number as well as a name.</summary>
    EnumParse,

    /// <summary>A reinterpretation through <c>Unsafe</c> or <c>MemoryMarshal</c> with an enum type argument.</summary>
    Reinterpretation,
}

/// <summary>Where an enum conversion stands: in read and write code or in game logic.</summary>
public enum EnumConversionPlace
{
    /// <summary>Game logic. The analyzer reports the conversion.</summary>
    GameLogic,

    /// <summary>A member of a type that carries <c>[StoredLayout]</c> or <c>[StoredRecord]</c>.</summary>
    StoredType,

    /// <summary>A member that takes a persister, or a member of a persister.</summary>
    Persister,

    /// <summary>A member that takes a stream, a binary reader or a binary writer.</summary>
    Stream,

    /// <summary>A namespace that the <c>aow_read_write_namespaces</c> option names.</summary>
    ReadWriteNamespace,
}

/// <summary>
/// The rule of issue 333: a conversion from an integer to an enum stands only in read and write
/// code. Read and write code is a member of a stored image type, a member that takes a persister,
/// a stream, a binary reader or a binary writer, a member of a persister type, and a namespace the
/// <c>aow_read_write_namespaces</c> option of .editorconfig names, with the namespaces inside it.
/// </summary>
public static class EnumConversionRules
{
    /// <summary>The .editorconfig option that lists the read and write namespaces, separated by commas.</summary>
    public const string NamespacesOption = "aow_read_write_namespaces";

    /// <summary>The name of the allow-list file the analyzer reads from the additional files.</summary>
    public const string AllowListFileName = "EnumCastAllowList.txt";

    /// <summary>
    /// The allowed members from the additional files: every documentation id the allow-list file
    /// names, with blank lines and <c>#</c> comments left out. Every other additional file is out.
    /// </summary>
    public static ImmutableHashSet<string> ParseAllowList(ImmutableArray<AdditionalText> files)
    {
        foreach (AdditionalText file in files)
        {
            if (string.Equals(Path.GetFileName(file.Path), AllowListFileName, comparisonType: StringComparison.OrdinalIgnoreCase)
                && file.GetText() is SourceText text)
            {
                return [.. text.Lines.Select(line => line.ToString().Trim()).Where(line => line.Length > 0 && !line.StartsWith('#'))];
            }
        }
        return [];
    }

    /// <summary>
    /// The member a node belongs to, found from syntax: the innermost method, constructor,
    /// property, field or type around the node. A conversion inside a lambda or a local function
    /// belongs to the member around it, so the census and the analyzer name the same member.
    /// </summary>
    public static ISymbol? DeclaringMember(SemanticModel model, SyntaxNode node)
        => Anchors(node)
            .Select(anchor => anchor is BaseFieldDeclarationSyntax fields ? FieldSymbol(model, node, fields) : model.GetDeclaredSymbol(anchor))
            .FirstOrDefault(symbol => symbol is not null);

    private static IEnumerable<MemberDeclarationSyntax> Anchors(SyntaxNode node)
    {
        foreach (MemberDeclarationSyntax anchor in node.Ancestors().OfType<MemberDeclarationSyntax>())
        {
            if (anchor is not TypeDeclarationSyntax)
            {
                yield return anchor;
            }
        }
        foreach (TypeDeclarationSyntax type in node.Ancestors().OfType<TypeDeclarationSyntax>())
        {
            yield return type;
        }
    }

    private static ISymbol? FieldSymbol(SemanticModel model, SyntaxNode node, BaseFieldDeclarationSyntax fields)
    {
        VariableDeclaratorSyntax? declarator = node.Ancestors().OfType<VariableDeclaratorSyntax>().FirstOrDefault();
        return declarator is not null && fields.Declaration.Variables.Contains(declarator)
            ? model.GetDeclaredSymbol(declarator)
            : fields.Declaration.Variables.Count == 1 ? model.GetDeclaredSymbol(fields) : null;
    }

    private static readonly ImmutableHashSet<string> s_storedAttributes = ["StoredLayoutAttribute", "StoredRecordAttribute"];

    private const string PersisterInterface = "IPersister";

    /// <summary>The kind of an operation that converts an integer to an enum, or null when it converts nothing.</summary>
    public static EnumConversionKind? Classify(IOperation operation) => operation switch
    {
        IConversionOperation conversion => ClassifyConversion(conversion),
        IInvocationOperation invocation => ClassifyInvocation(invocation),
        _ => null,
    };

    /// <summary>The enum type a conversion produces, for the message.</summary>
    public static string TargetName(IOperation operation)
    {
        ITypeSymbol? target = operation switch
        {
            IConversionOperation { Type: IPointerTypeSymbol pointer } => pointer.PointedAtType,
            IConversionOperation conversion => conversion.Type,
            IInvocationOperation { TargetMethod: var method } => method.TypeArguments.FirstOrDefault(IsEnumLike) ?? method.ReturnType,
            _ => operation.Type,
        };
        return target is null ? "enum" : Underlying(target).ToDisplayString();
    }

    /// <summary>Where the symbol that contains a conversion stands.</summary>
    public static EnumConversionPlace Place(ISymbol? containing, ImmutableArray<string> namespaces)
    {
        for (ISymbol? symbol = containing; symbol is not null; symbol = symbol.ContainingSymbol)
        {
            switch (symbol)
            {
                case IMethodSymbol method when method.Parameters.Any(parameter => IsPersister(parameter.Type)):
                    return EnumConversionPlace.Persister;
                case IMethodSymbol method when method.Parameters.Any(parameter => IsStream(parameter.Type)):
                    return EnumConversionPlace.Stream;
                case INamedTypeSymbol type when type.GetAttributes().Any(attribute => attribute.AttributeClass is { } attributeClass && s_storedAttributes.Contains(attributeClass.Name)):
                    return EnumConversionPlace.StoredType;
                case INamedTypeSymbol type when IsPersister(type):
                    return EnumConversionPlace.Persister;
                case INamespaceSymbol space when !space.IsGlobalNamespace && namespaces.Contains(space.ToDisplayString()):
                    return EnumConversionPlace.ReadWriteNamespace;
                default:
                    break;
            }
        }
        return EnumConversionPlace.GameLogic;
    }

    /// <summary>The namespaces of the option's value.</summary>
    public static ImmutableArray<string> ParseNamespaces(string? value)
        => value is null ? [] : [.. value.Split(',').Select(name => name.Trim()).Where(name => name.Length > 0)];

    private static bool IsPersister(ITypeSymbol type)
        => type is INamedTypeSymbol named && (IsPersisterInterface(named) || named.AllInterfaces.Any(IsPersisterInterface));

    private static bool IsStream(ITypeSymbol type)
    {
        for (ITypeSymbol? current = type; current is not null; current = current.BaseType)
        {
            if (current is { Name: "Stream" or "BinaryReader" or "BinaryWriter", ContainingNamespace: { Name: "IO", ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true } } })
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsPersisterInterface(INamedTypeSymbol type)
        => type is { TypeKind: TypeKind.Interface, Name: PersisterInterface };

    private static EnumConversionKind? ClassifyConversion(IConversionOperation conversion)
    {
        if (conversion.IsImplicit || conversion.Conversion.IsImplicit || conversion.Operand.Type is not ITypeSymbol source || conversion.Type is not ITypeSymbol target)
        {
            return null;
        }
        if (target is IPointerTypeSymbol { PointedAtType: var pointedTarget } && IsEnum(pointedTarget))
        {
            return source is IPointerTypeSymbol { PointedAtType: var pointedSource } && SymbolEqualityComparer.Default.Equals(pointedSource, pointedTarget) ? null : EnumConversionKind.PointerCast;
        }
        if (!IsEnum(target) && !(target is ITypeParameterSymbol parameter && IsEnumParameter(parameter)))
        {
            return null;
        }
        ITypeSymbol enumTarget = Underlying(target);
        ITypeSymbol enumSource = Underlying(source);
        return enumSource switch
        {
            _ when IsInteger(enumSource) => conversion.Operand.ConstantValue.HasValue ? EnumConversionKind.ConstantCast : EnumConversionKind.Cast,
            _ when IsEnum(enumSource) => SymbolEqualityComparer.Default.Equals(enumSource, enumTarget) ? null : EnumConversionKind.EnumToEnumCast,
            { SpecialType: SpecialType.System_Object or SpecialType.System_ValueType or SpecialType.System_Enum }
                when conversion.Operand is IConversionOperation { Operand.Type: ITypeSymbol boxed } && IsInteger(Underlying(boxed)) => EnumConversionKind.UnboxingCast,
            _ => null,
        };
    }

    private static EnumConversionKind? ClassifyInvocation(IInvocationOperation invocation)
    {
        IMethodSymbol method = invocation.TargetMethod;
        string? type = method.ContainingType?.ToDisplayString();
        return type switch
        {
            "System.Enum" when method.Name == "ToObject" => EnumConversionKind.EnumToObject,
            "System.Enum" when method.Name is "Parse" or "TryParse" => EnumConversionKind.EnumParse,
            "System.Runtime.CompilerServices.Unsafe" or "System.Runtime.InteropServices.MemoryMarshal" when Reinterprets(method) => EnumConversionKind.Reinterpretation,
            _ => null,
        };
    }

    // The result holds an enum that no parameter holds: the enum is read from memory of another type.
    private static bool Reinterprets(IMethodSymbol method)
        => method.TypeArguments.Where(IsEnumLike).Any(argument => Mentions(method.ReturnType, argument)
            && !method.Parameters.Any(parameter => Mentions(parameter.Type, argument)));

    private static bool Mentions(ITypeSymbol type, ITypeSymbol argument) => type switch
    {
        _ when SymbolEqualityComparer.Default.Equals(type, argument) => true,
        INamedTypeSymbol named => named.TypeArguments.Any(inner => Mentions(inner, argument)),
        IPointerTypeSymbol pointer => Mentions(pointer.PointedAtType, argument),
        IArrayTypeSymbol array => Mentions(array.ElementType, argument),
        _ => false,
    };

    private static bool IsEnumLike(ITypeSymbol type) => IsEnum(type) || (type is ITypeParameterSymbol parameter && IsEnumParameter(parameter));

    private static ITypeSymbol Underlying(ITypeSymbol type)
        => type is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T, TypeArguments: [var inner] } ? inner : type;

    private static bool IsEnum(ITypeSymbol type) => Underlying(type).TypeKind == TypeKind.Enum;

    private static bool IsEnumParameter(ITypeParameterSymbol parameter)
        => parameter.ConstraintTypes.Any(constraint => constraint.SpecialType == SpecialType.System_Enum);

    private static bool IsInteger(ITypeSymbol type) => type.SpecialType is SpecialType.System_Byte or SpecialType.System_SByte
        or SpecialType.System_Int16 or SpecialType.System_UInt16 or SpecialType.System_Int32 or SpecialType.System_UInt32
        or SpecialType.System_Int64 or SpecialType.System_UInt64 or SpecialType.System_Char or SpecialType.System_IntPtr or SpecialType.System_UIntPtr;
}
