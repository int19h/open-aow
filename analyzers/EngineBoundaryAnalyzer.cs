using System.Collections.Concurrent;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Aow.Analyzers;

/// <summary>
/// AOW0003: code in the engine files refers only to engine types, to the product assemblies the
/// <c>aow_engine_boundary</c> option names and to assemblies outside the product, apart from the
/// references the allow list names (issue 311). An allow list entry that matches no reference is
/// reported as stale, so the list only shrinks.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class EngineBoundaryAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "AOW0003";

    /// <summary>The property of a diagnostic that carries the documentation id of the member that holds the reference.</summary>
    public const string MemberProperty = "Member";

    /// <summary>The property of a diagnostic that carries the documentation id of the symbol the reference names.</summary>
    public const string TargetProperty = "Target";

    private const string Description = "Engine code (the files the aow_engine_boundary option of .editorconfig marks) refers only to engine types, to the product assemblies the option names and to assemblies outside the product. EngineBoundaryAllowList.csv names the references that stay for now, and an entry that matches no reference is stale (issue 311).";

    public static DiagnosticDescriptor Rule { get; } = new(
        id: DiagnosticId,
        title: "Engine code refers only to the engine, Aow.Rtl and Aow.Formats",
        messageFormat: "Engine member '{0}' refers to '{1}' outside the engine boundary (the allow list has no line '{2}')",
        category: "Design",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    public static DiagnosticDescriptor StaleRule { get; } = new(
        id: DiagnosticId,
        title: "An engine boundary allow list entry matches no reference",
        messageFormat: "The allow list entry '{0}' matches no reference from engine code: delete its line",
        category: "Design",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description,
        customTags: WellKnownDiagnosticTags.CompilationEnd);

    public static DiagnosticDescriptor MalformedRule { get; } = new(
        id: DiagnosticId,
        title: "An engine boundary allow list line names no reference",
        messageFormat: "The allow list line '{0}' names no reference: write 'member,target'",
        category: "Design",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description,
        customTags: WellKnownDiagnosticTags.CompilationEnd);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rule, StaleRule, MalformedRule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(compilationContext =>
        {
            ImmutableArray<EngineAllowEntry> entries = EngineBoundaryRules.ParseAllowList(compilationContext.Options.AdditionalFiles, compilationContext.CancellationToken);
            ImmutableArray<EngineMalformedLine> malformed = EngineBoundaryRules.MalformedLines(compilationContext.Options.AdditionalFiles, compilationContext.CancellationToken);
            ImmutableHashSet<EngineReference> allowed = [.. entries.Select(entry => entry.Reference)];
            ConcurrentDictionary<EngineReference, bool> seen = new();
            compilationContext.RegisterSemanticModelAction(modelContext => Analyze(modelContext, allowed: allowed, seen: seen));
            compilationContext.RegisterCompilationEndAction(endContext => ReportStale(endContext, entries: entries, seen: seen));
            compilationContext.RegisterCompilationEndAction(endContext => ReportMalformed(endContext, malformed: malformed));
        });
    }

    private static void Analyze(SemanticModelAnalysisContext context, ImmutableHashSet<EngineReference> allowed, ConcurrentDictionary<EngineReference, bool> seen)
    {
        SemanticModel model = context.SemanticModel;
        if (EngineBoundaryRules.BoundaryAssemblies(context.Options, tree: model.SyntaxTree) is not ImmutableHashSet<string> assemblies)
        {
            return;
        }
        SyntaxNode root = model.SyntaxTree.GetRoot(context.CancellationToken);
        foreach (SyntaxNode node in root.DescendantNodes())
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            if (node is not (SimpleNameSyntax or ImplicitObjectCreationExpressionSyntax))
            {
                continue;
            }
            SymbolInfo info = model.GetSymbolInfo(node, context.CancellationToken);
            ISymbol? symbol = info.Symbol ?? (info.CandidateSymbols.Length == 1 ? info.CandidateSymbols[0] : null);
            if (EngineBoundaryRules.Target(symbol) is not ISymbol target
                || EngineBoundaryRules.OwningType(target) is not INamedTypeSymbol owner
                || EngineBoundaryRules.IsAllowed(owner, compilation: model.Compilation, options: context.Options, assemblies: assemblies))
            {
                continue;
            }
            ISymbol? member = EnumConversionRules.DeclaringMember(model, node: node);
            EngineReference reference = new(member is null ? "" : EngineBoundaryRules.Id(member), Target: EngineBoundaryRules.Id(target));
            if (allowed.Contains(reference))
            {
                seen[reference] = true;
                continue;
            }
            ImmutableDictionary<string, string?> properties = ImmutableDictionary<string, string?>.Empty
                .Add(MemberProperty, reference.Member)
                .Add(TargetProperty, reference.Target);
            context.ReportDiagnostic(Diagnostic.Create(
                Rule,
                location: node.GetLocation(),
                properties: properties,
                reference.Member,
                reference.Target,
                EngineBoundaryRules.FormatLine(reference)));
        }
    }

    // Only a compilation that holds engine files reports stale entries: every product project
    // receives the allow list, and the others have no engine code to match it.
    private static void ReportStale(CompilationAnalysisContext context, ImmutableArray<EngineAllowEntry> entries, ConcurrentDictionary<EngineReference, bool> seen)
    {
        if (entries.IsEmpty || !context.Compilation.SyntaxTrees.Any(tree => EngineBoundaryRules.BoundaryAssemblies(context.Options, tree: tree) is not null))
        {
            return;
        }
        foreach (EngineAllowEntry entry in entries.Where(entry => !seen.ContainsKey(entry.Reference)))
        {
            context.ReportDiagnostic(Diagnostic.Create(StaleRule, location: entry.Location, EngineBoundaryRules.FormatLine(entry.Reference)));
        }
    }

    private static void ReportMalformed(CompilationAnalysisContext context, ImmutableArray<EngineMalformedLine> malformed)
    {
        foreach (EngineMalformedLine line in malformed)
        {
            context.ReportDiagnostic(Diagnostic.Create(MalformedRule, location: line.Location, line.Line));
        }
    }
}
