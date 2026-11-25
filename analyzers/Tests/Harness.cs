using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Aow.Analyzers.Tests;

/// <summary>Compiles a source with an analyzer and applies a code fix to every diagnostic of it.</summary>
internal static partial class Harness
{
    private static readonly ImmutableArray<MetadataReference> s_references =
    [
        .. ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Where(path => Path.GetFileName(path).StartsWith("System.", comparisonType: StringComparison.Ordinal) || Path.GetFileName(path) is "mscorlib.dll" or "netstandard.dll")
            .Select(path => MetadataReference.CreateFromFile(path)),
    ];

    [SuppressMessage("Usage", "CA2255:The 'ModuleInitializer' attribute should not be used in libraries", Justification = "The test assembly sets the error mode before any test runs (AGENTS.md).")]
    [ModuleInitializer]
    internal static void Initialize()
    {
        if (OperatingSystem.IsWindows())
        {
            _ = SetErrorMode(0x8003);
        }
    }

    [LibraryImport("kernel32.dll")]
    private static partial uint SetErrorMode(uint mode);

    public static async Task<ImmutableArray<Diagnostic>> DiagnosticsAsync(string source, DiagnosticAnalyzer analyzer, string? namespaces = null, string? allowList = null, string? allowListFile = null)
    {
        Document document = CreateDocument(source);
        return await DiagnosticsAsync(document, analyzer: analyzer, namespaces: namespaces, allowList: allowList, allowListFile: allowListFile);
    }

    public static async Task<string> FixAsync(string source, DiagnosticAnalyzer analyzer, CodeFixProvider fix)
    {
        Document document = CreateDocument(source);
        ImmutableArray<Diagnostic> diagnostics = await DiagnosticsAsync(document, analyzer: analyzer, namespaces: null);
        Assert.NotEmpty(diagnostics);
        FixAllProvider? fixAll = fix.GetFixAllProvider();
        Assert.NotNull(fixAll);
        string key = await EquivalenceKeyAsync(document, fix: fix, diagnostic: diagnostics[0]);
        FixAllContext context = new(
            document,
            codeFixProvider: fix,
            scope: FixAllScope.Document,
            codeActionEquivalenceKey: key,
            diagnosticIds: fix.FixableDiagnosticIds,
            fixAllDiagnosticProvider: new FixedDiagnostics(diagnostics),
            cancellationToken: CancellationToken.None);
        CodeAction? action = await fixAll.GetFixAsync(context);
        Assert.NotNull(action);
        ImmutableArray<CodeActionOperation> operations = await action.GetOperationsAsync(CancellationToken.None);
        Solution changed = operations.OfType<ApplyChangesOperation>().Single().ChangedSolution;
        Document result = changed.GetDocument(document.Id)!;
        Compilation? compilation = await result.Project.GetCompilationAsync();
        Assert.Empty(compilation!.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error));
        return (await result.GetTextAsync()).ToString();
    }

    public static async Task<bool> HasFixAsync(string source, DiagnosticAnalyzer analyzer, CodeFixProvider fix)
    {
        Document document = CreateDocument(source);
        ImmutableArray<Diagnostic> diagnostics = await DiagnosticsAsync(document, analyzer: analyzer, namespaces: null);
        List<CodeAction> actions = [];
        foreach (Diagnostic diagnostic in diagnostics)
        {
            await fix.RegisterCodeFixesAsync(new CodeFixContext(
                document,
                diagnostic: diagnostic,
                registerCodeFix: (action, _) => actions.Add(action),
                cancellationToken: CancellationToken.None));
        }
        return actions.Count > 0;
    }

    private static async Task<string> EquivalenceKeyAsync(Document document, CodeFixProvider fix, Diagnostic diagnostic)
    {
        List<CodeAction> actions = [];
        await fix.RegisterCodeFixesAsync(new CodeFixContext(
            document,
            diagnostic: diagnostic,
            registerCodeFix: (action, _) => actions.Add(action),
            cancellationToken: CancellationToken.None));
        return Assert.Single(actions).EquivalenceKey!;
    }

    private static async Task<ImmutableArray<Diagnostic>> DiagnosticsAsync(Document document, DiagnosticAnalyzer analyzer, string? namespaces, string? allowList = null, string? allowListFile = null)
    {
        Compilation? compilation = await document.Project.GetCompilationAsync();
        Assert.NotNull(compilation);
        Assert.Empty(compilation.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error));
        Dictionary<string, string> options = namespaces is null ? [] : new() { [EnumConversionRules.NamespacesOption] = namespaces };
        ImmutableArray<AdditionalText> files = allowList is null
            ? []
            : [new MemoryAdditionalText(allowListFile ?? EnumConversionRules.AllowListFileName, text: SourceText.From(allowList))];
        AnalyzerOptions analyzerOptions = new(additionalFiles: files, optionsProvider: new OptionsProvider(options));
        CompilationWithAnalyzers withAnalyzers = compilation.WithAnalyzers([analyzer], options: analyzerOptions);
        ImmutableArray<Diagnostic> diagnostics = await withAnalyzers.GetAnalyzerDiagnosticsAsync();
        return [.. diagnostics.OrderBy(diagnostic => diagnostic.Location.SourceSpan.Start)];
    }

    /// <summary>
    /// The AOW0003 diagnostics of a compilation of several files. A file whose path starts with
    /// <c>Engine/</c> carries the boundary option with the value <paramref name="boundary"/>.
    /// Each source of <paramref name="assemblies"/> compiles to a referenced assembly of its name.
    /// </summary>
    public static async Task<ImmutableArray<Diagnostic>> BoundaryDiagnosticsAsync(
        ImmutableArray<(string Path, string Source)> documents,
        string boundary,
        string? allowList = null,
        ImmutableArray<(string Name, string Source)> assemblies = default)
    {
        List<MetadataReference> references = [.. s_references];
        foreach ((string name, string source) in assemblies.IsDefault ? [] : assemblies)
        {
            CSharpCompilation library = CSharpCompilation.Create(
                name,
                syntaxTrees: [CSharpSyntaxTree.ParseText(source, options: new CSharpParseOptions(LanguageVersion.Preview))],
                references: s_references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            using MemoryStream image = new();
            Assert.True(library.Emit(image).Success);
            references.Add(MetadataReference.CreateFromImage(image.ToArray()));
        }
        CSharpCompilation compilation = CSharpCompilation.Create(
            "Sample",
            syntaxTrees: [.. documents.Select(document => CSharpSyntaxTree.ParseText(document.Source, options: new CSharpParseOptions(LanguageVersion.Preview), path: document.Path))],
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));
        Assert.Empty(compilation.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error));
        ImmutableArray<AdditionalText> files = allowList is null
            ? []
            : [new MemoryAdditionalText(EngineBoundaryRules.AllowListFileName, text: SourceText.From(allowList))];
        Dictionary<string, string> engineOptions = new() { [EngineBoundaryRules.BoundaryOption] = boundary };
        AnalyzerOptions analyzerOptions = new(additionalFiles: files, optionsProvider: new PathOptionsProvider(engineOptions));
        CompilationWithAnalyzers withAnalyzers = compilation.WithAnalyzers([new EngineBoundaryAnalyzer()], options: analyzerOptions);
        ImmutableArray<Diagnostic> diagnostics = await withAnalyzers.GetAnalyzerDiagnosticsAsync();
        return [.. diagnostics.OrderBy(diagnostic => diagnostic.Location.SourceTree?.FilePath ?? diagnostic.Location.GetLineSpan().Path, comparer: StringComparer.Ordinal).ThenBy(diagnostic => diagnostic.Location.SourceSpan.Start)];
    }

    private static Document CreateDocument(string source)
    {
        AdhocWorkspace workspace = new();
        Project project = workspace.AddProject(ProjectInfo.Create(
            ProjectId.CreateNewId(),
            version: VersionStamp.Default,
            name: "Sample",
            assemblyName: "Sample",
            language: LanguageNames.CSharp,
            compilationOptions: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true, nullableContextOptions: NullableContextOptions.Enable),
            parseOptions: new CSharpParseOptions(LanguageVersion.Preview),
            metadataReferences: s_references));
        return project.AddDocument("Sample.cs", text: SourceText.From(source));
    }

    /// <summary>The text of the source with LF line ends.</summary>
    public static string Lines(string text) => text.Replace("\r\n", "\n", comparisonType: StringComparison.Ordinal);

    private sealed class FixedDiagnostics(ImmutableArray<Diagnostic> diagnostics) : FixAllContext.DiagnosticProvider
    {
        public override Task<IEnumerable<Diagnostic>> GetAllDiagnosticsAsync(Project project, CancellationToken cancellationToken)
            => Task.FromResult<IEnumerable<Diagnostic>>(diagnostics);

        public override Task<IEnumerable<Diagnostic>> GetDocumentDiagnosticsAsync(Document document, CancellationToken cancellationToken)
            => Task.FromResult<IEnumerable<Diagnostic>>(diagnostics);

        public override Task<IEnumerable<Diagnostic>> GetProjectDiagnosticsAsync(Project project, CancellationToken cancellationToken)
            => Task.FromResult<IEnumerable<Diagnostic>>([]);
    }

    private sealed class OptionsProvider(Dictionary<string, string> values) : AnalyzerConfigOptionsProvider
    {
        private readonly Options _options = new(values);

        public override AnalyzerConfigOptions GlobalOptions => _options;

        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => _options;

        public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) => _options;
    }

    // The engine options for a tree whose path starts with Engine/, and none for the others.
    private sealed class PathOptionsProvider(Dictionary<string, string> engineValues) : AnalyzerConfigOptionsProvider
    {
        private readonly Options _engineOptions = new(engineValues);
        private readonly Options _otherOptions = new([]);

        public override AnalyzerConfigOptions GlobalOptions => _otherOptions;

        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
            => tree.FilePath.StartsWith("Engine/", comparisonType: StringComparison.Ordinal) ? _engineOptions : _otherOptions;

        public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) => _otherOptions;
    }

    private sealed class MemoryAdditionalText(string path, SourceText text) : AdditionalText
    {
        public override string Path => path;

        public override SourceText? GetText(CancellationToken cancellationToken = default) => text;
    }

    private sealed class Options(Dictionary<string, string> values) : AnalyzerConfigOptions
    {
        public override bool TryGetValue(string key, [NotNullWhen(true)] out string? value) => values.TryGetValue(key, out value);
    }
}
