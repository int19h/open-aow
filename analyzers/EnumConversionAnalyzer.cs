using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Aow.Analyzers;

/// <summary>
/// AOW0002: an integer becomes an enum value only in read and write code (issue 333). The
/// places that count as read and write code are those of <see cref="EnumConversionRules.Place"/>.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class EnumConversionAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "AOW0002";

    /// <summary>The property of a diagnostic that carries the <see cref="EnumConversionKind"/>.</summary>
    public const string KindProperty = "Kind";

    public static DiagnosticDescriptor Rule { get; } = new(
        id: DiagnosticId,
        title: "Convert integers to enums only in read and write code",
        messageFormat: "{0} converts an integer to '{1}' outside read and write code",
        category: "Design",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "An integer becomes an enum value only where a format is read or written: a stored image type, a member that takes a persister, a stream, a binary reader or a binary writer, a persister, or a namespace that aow_read_write_namespaces names (issue 333).");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterOperationAction(Analyze, OperationKind.Conversion, OperationKind.Invocation);
    }

    private static void Analyze(OperationAnalysisContext context)
    {
        if (EnumConversionRules.Classify(context.Operation) is not EnumConversionKind kind)
        {
            return;
        }
        string? option = context.Options.AnalyzerConfigOptionsProvider.GetOptions(context.Operation.Syntax.SyntaxTree)
            .TryGetValue(EnumConversionRules.NamespacesOption, out string? value) ? value : null;
        if (EnumConversionRules.Place(context.ContainingSymbol, namespaces: EnumConversionRules.ParseNamespaces(option)) != EnumConversionPlace.GameLogic)
        {
            return;
        }
        ImmutableDictionary<string, string?> properties = ImmutableDictionary<string, string?>.Empty.Add(KindProperty, kind.ToString());
        context.ReportDiagnostic(Diagnostic.Create(
            Rule,
            location: context.Operation.Syntax.GetLocation(),
            properties: properties,
            KindText(kind),
            EnumConversionRules.TargetName(context.Operation)));
    }

    private static string KindText(EnumConversionKind kind) => kind switch
    {
        EnumConversionKind.ConstantCast => "A cast of a constant",
        EnumConversionKind.Cast => "A cast",
        EnumConversionKind.EnumToEnumCast => "A cast of another enum",
        EnumConversionKind.UnboxingCast => "An unboxing cast",
        EnumConversionKind.PointerCast => "A pointer cast",
        EnumConversionKind.EnumToObject => "Enum.ToObject",
        EnumConversionKind.EnumParse => "Enum.Parse",
        EnumConversionKind.Reinterpretation => "A reinterpretation",
        _ => kind.ToString(),
    };
}
