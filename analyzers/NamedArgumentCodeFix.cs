using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Aow.Analyzers;

/// <summary>
/// Names the arguments AOW0001 reports. One fix names every reported argument of the call, and a
/// fix of a document or a solution rewrites each call once.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NamedArgumentCodeFix)), Shared]
public sealed class NamedArgumentCodeFix : CodeFixProvider
{
    private const string Title = "Name the arguments";

    public override ImmutableArray<string> FixableDiagnosticIds { get; } = [NamedArgumentAnalyzer.DiagnosticId];

    public override FixAllProvider GetFixAllProvider() => FixAllProvider.Create(FixDocumentAsync);

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root?.FindNode(context.Span, getInnermostNodeForTie: true)?.FirstAncestorOrSelf<ArgumentSyntax>()?.Parent is not ArgumentListSyntax)
        {
            return;
        }
        context.RegisterCodeFix(
            CodeAction.Create(
                title: Title,
                createChangedDocument: cancellationToken => FixDocumentAsync(context.Document, context.Diagnostics, cancellationToken),
                equivalenceKey: Title),
            context.Diagnostics);
    }

    private static async Task<Document?> FixDocumentAsync(FixAllContext context, Document document, ImmutableArray<Diagnostic> diagnostics)
        => await FixDocumentAsync(document, diagnostics: diagnostics, cancellationToken: context.CancellationToken).ConfigureAwait(false);

    private static async Task<Document> FixDocumentAsync(Document document, ImmutableArray<Diagnostic> diagnostics, CancellationToken cancellationToken)
    {
        SyntaxNode? root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        SemanticModel? model = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
        if (root is null || model is null)
        {
            return document;
        }
        HashSet<ArgumentListSyntax> lists = [];
        foreach (Diagnostic diagnostic in diagnostics)
        {
            if (root.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true)?.FirstAncestorOrSelf<ArgumentSyntax>()?.Parent is ArgumentListSyntax list)
            {
                _ = lists.Add(list);
            }
        }
        Dictionary<ArgumentListSyntax, ArgumentListSyntax> replaced = [];
        foreach (ArgumentListSyntax list in lists)
        {
            if (list.Parent is not null && model.GetOperation(list.Parent, cancellationToken) is IOperation operation)
            {
                ArgumentListSyntax named = NameArguments(list, verdicts: NamedArgumentRules.Classify(operation));
                if (named != list)
                {
                    replaced[list] = named;
                }
            }
        }
        return replaced.Count == 0 ? document : document.WithSyntaxRoot(root.ReplaceNodes(replaced.Keys, (original, _) => replaced[original]));
    }

    /// <summary>The argument list with a name on every argument whose verdict needs one.</summary>
    public static ArgumentListSyntax NameArguments(ArgumentListSyntax list, ImmutableArray<ArgumentVerdict> verdicts)
    {
        Dictionary<ArgumentSyntax, ArgumentSyntax> named = [];
        foreach (ArgumentVerdict verdict in verdicts)
        {
            if (verdict.Reason == ArgumentReason.NeedsName && verdict.Parameter is not null)
            {
                named[verdict.Syntax] = Named(verdict.Syntax, verdict.Parameter);
            }
        }
        return named.Count == 0 ? list : list.ReplaceNodes(named.Keys, (original, _) => named[original]);
    }

    private static ArgumentSyntax Named(ArgumentSyntax argument, IParameterSymbol parameter)
    {
        SyntaxTriviaList leading = argument.GetLeadingTrivia();
        SyntaxToken identifier = SyntaxFactory.Identifier(
            leading,
            contextualKind: SyntaxKind.IdentifierToken,
            text: NamedArgumentRules.ArgumentName(parameter),
            valueText: parameter.Name,
            trailing: SyntaxTriviaList.Empty);
        NameColonSyntax nameColon = SyntaxFactory.NameColon(
            SyntaxFactory.IdentifierName(identifier),
            colonToken: SyntaxFactory.Token(SyntaxKind.ColonToken).WithTrailingTrivia(SyntaxFactory.Space));
        return argument.WithoutLeadingTrivia().WithNameColon(nameColon);
    }
}
