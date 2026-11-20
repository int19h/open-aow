using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Aow.Analyzers;

/// <summary>
/// AOW0001: from the second argument of a call on, an argument carries its parameter name unless
/// a rule of <see cref="NamedArgumentRules"/> leaves it positional.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class NamedArgumentAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "AOW0001";

    /// <summary>The property of a diagnostic that carries the parameter name.</summary>
    public const string ParameterProperty = "Parameter";

    public static DiagnosticDescriptor Rule { get; } = new(
        id: DiagnosticId,
        title: "Name the argument",
        messageFormat: "Name the argument for parameter '{0}'",
        category: "Style",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "From the second argument on, a call names its arguments, and a boolean argument is named unless it is the first (AGENTS.md, How new code is written).");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterOperationAction(Analyze, OperationKind.Invocation, OperationKind.ObjectCreation);
    }

    private static void Analyze(OperationAnalysisContext context)
    {
        if (context.Operation.IsImplicit)
        {
            return;
        }
        foreach (ArgumentVerdict verdict in NamedArgumentRules.Classify(context.Operation))
        {
            if (verdict.Reason == ArgumentReason.NeedsName && verdict.Parameter is not null)
            {
                ImmutableDictionary<string, string?> properties = ImmutableDictionary<string, string?>.Empty.Add(ParameterProperty, verdict.Parameter.Name);
                context.ReportDiagnostic(Diagnostic.Create(Rule, location: verdict.Syntax.GetLocation(), properties, verdict.Parameter.Name));
            }
        }
    }
}
