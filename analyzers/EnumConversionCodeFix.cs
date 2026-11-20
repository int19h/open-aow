using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Aow.Analyzers;

/// <summary>
/// Replaces a cast of a constant that AOW0002 reports with the enum member of that value, as in
/// <c>(TerrainType)6</c> to <c>TerrainType.Water</c>. A value that no member or more than one
/// member has stays, since only a decision on the enum can name it.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EnumConversionCodeFix)), Shared]
public sealed class EnumConversionCodeFix : CodeFixProvider
{
    private const string Title = "Use the enum member";

    public override ImmutableArray<string> FixableDiagnosticIds { get; } = [EnumConversionAnalyzer.DiagnosticId];

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        foreach (Diagnostic diagnostic in context.Diagnostics)
        {
            if (!diagnostic.Properties.TryGetValue(EnumConversionAnalyzer.KindProperty, out string? kind) || kind != nameof(EnumConversionKind.ConstantCast))
            {
                continue;
            }
            SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            SemanticModel? model = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
            if (root?.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true) is not CastExpressionSyntax cast || model is null
                || Member(cast, model, context.CancellationToken) is not IFieldSymbol member)
            {
                continue;
            }
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Title,
                    createChangedDocument: _ => Task.FromResult(context.Document.WithSyntaxRoot(root.ReplaceNode(cast, MemberAccess(cast, member)))),
                    equivalenceKey: Title),
                diagnostic);
        }
    }

    /// <summary>The one member of the cast's enum type whose value is the cast constant, or null.</summary>
    public static IFieldSymbol? Member(CastExpressionSyntax cast, SemanticModel model, CancellationToken cancellationToken)
    {
        if (model.GetTypeInfo(cast, cancellationToken).Type is not INamedTypeSymbol { TypeKind: TypeKind.Enum } type
            || model.GetConstantValue(cast.Expression, cancellationToken) is not { HasValue: true, Value: { } value })
        {
            return null;
        }
        IFieldSymbol[] members = [.. type.GetMembers().OfType<IFieldSymbol>().Where(field => field.HasConstantValue && Equals(field.ConstantValue, Convert(value, type)))];
        return members.Length == 1 ? members[0] : null;
    }

    private static object? Convert(object value, INamedTypeSymbol type) => type.EnumUnderlyingType?.SpecialType switch
    {
        SpecialType.System_Byte => unchecked((byte)ToInt64(value)),
        SpecialType.System_SByte => unchecked((sbyte)ToInt64(value)),
        SpecialType.System_Int16 => unchecked((short)ToInt64(value)),
        SpecialType.System_UInt16 => unchecked((ushort)ToInt64(value)),
        SpecialType.System_Int32 => unchecked((int)ToInt64(value)),
        SpecialType.System_UInt32 => unchecked((uint)ToInt64(value)),
        SpecialType.System_Int64 => ToInt64(value),
        SpecialType.System_UInt64 => unchecked((ulong)ToInt64(value)),
        SpecialType.None => throw new NotImplementedException(),
        SpecialType.System_Object => throw new NotImplementedException(),
        SpecialType.System_Enum => throw new NotImplementedException(),
        SpecialType.System_MulticastDelegate => throw new NotImplementedException(),
        SpecialType.System_Delegate => throw new NotImplementedException(),
        SpecialType.System_ValueType => throw new NotImplementedException(),
        SpecialType.System_Void => throw new NotImplementedException(),
        SpecialType.System_Boolean => throw new NotImplementedException(),
        SpecialType.System_Char => throw new NotImplementedException(),
        SpecialType.System_Decimal => throw new NotImplementedException(),
        SpecialType.System_Single => throw new NotImplementedException(),
        SpecialType.System_Double => throw new NotImplementedException(),
        SpecialType.System_String => throw new NotImplementedException(),
        SpecialType.System_IntPtr => throw new NotImplementedException(),
        SpecialType.System_UIntPtr => throw new NotImplementedException(),
        SpecialType.System_Array => throw new NotImplementedException(),
        SpecialType.System_Collections_IEnumerable => throw new NotImplementedException(),
        SpecialType.System_Collections_Generic_IEnumerable_T => throw new NotImplementedException(),
        SpecialType.System_Collections_Generic_IList_T => throw new NotImplementedException(),
        SpecialType.System_Collections_Generic_ICollection_T => throw new NotImplementedException(),
        SpecialType.System_Collections_IEnumerator => throw new NotImplementedException(),
        SpecialType.System_Collections_Generic_IEnumerator_T => throw new NotImplementedException(),
        SpecialType.System_Collections_Generic_IReadOnlyList_T => throw new NotImplementedException(),
        SpecialType.System_Collections_Generic_IReadOnlyCollection_T => throw new NotImplementedException(),
        SpecialType.System_Nullable_T => throw new NotImplementedException(),
        SpecialType.System_DateTime => throw new NotImplementedException(),
        SpecialType.System_Runtime_CompilerServices_IsVolatile => throw new NotImplementedException(),
        SpecialType.System_IDisposable => throw new NotImplementedException(),
        SpecialType.System_TypedReference => throw new NotImplementedException(),
        SpecialType.System_ArgIterator => throw new NotImplementedException(),
        SpecialType.System_RuntimeArgumentHandle => throw new NotImplementedException(),
        SpecialType.System_RuntimeFieldHandle => throw new NotImplementedException(),
        SpecialType.System_RuntimeMethodHandle => throw new NotImplementedException(),
        SpecialType.System_RuntimeTypeHandle => throw new NotImplementedException(),
        SpecialType.System_IAsyncResult => throw new NotImplementedException(),
        SpecialType.System_AsyncCallback => throw new NotImplementedException(),
        SpecialType.System_Runtime_CompilerServices_RuntimeFeature => throw new NotImplementedException(),
        SpecialType.System_Runtime_CompilerServices_PreserveBaseOverridesAttribute => throw new NotImplementedException(),
        SpecialType.System_Runtime_CompilerServices_InlineArrayAttribute => throw new NotImplementedException(),
        null => throw new NotImplementedException(),
        _ => null,
    };

    private static long ToInt64(object value) => value is ulong large ? unchecked((long)large) : System.Convert.ToInt64(value, provider: null);

    private static ExpressionSyntax MemberAccess(CastExpressionSyntax cast, IFieldSymbol member)
        => SyntaxFactory.MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            expression: cast.Type.WithoutTrivia(),
            name: SyntaxFactory.IdentifierName(member.Name))
        .WithTriviaFrom(cast);
}
