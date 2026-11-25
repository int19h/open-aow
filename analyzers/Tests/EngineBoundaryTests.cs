using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace Aow.Analyzers.Tests;

public sealed class EngineBoundaryTests
{
    private const string Boundary = "Aow.Rtl, Aow.Formats";

    private const string Game = """
        namespace Aow.Game
        {
            public sealed class GameEngine
            {
                public int Editors => 1;

                public static int Create(int classId, int level) => classId + level;
            }

            public sealed class Unit;

            public class UnitBase;

            public static class GameExtensions
            {
                public static int Twice(this GameEngine engine) => 1;
            }
        }
        """;

    private const string RtlLibrary = """
        namespace Aow.Rtl
        {
            public sealed class Canvas;
        }
        """;

    private const string GraphicsLibrary = """
        namespace Aow.Graphics
        {
            public readonly record struct Color(int Value);
        }
        """;

    private static async Task<ImmutableArray<Diagnostic>> ReportedAsync(string engine, string? allowList = null, string game = Game)
        => await Harness.BoundaryDiagnosticsAsync(
            [("Engine/Node.cs", engine), ("Game/Game.cs", game)],
            boundary: Boundary,
            allowList: allowList,
            assemblies: [("Aow.Rtl", RtlLibrary), ("Aow.Graphics", GraphicsLibrary)]);

    private static (string Member, string Target) Reference(Diagnostic diagnostic)
        => (diagnostic.Properties[EngineBoundaryAnalyzer.MemberProperty]!, diagnostic.Properties[EngineBoundaryAnalyzer.TargetProperty]!);

    [Fact]
    public async Task ReferenceToGameTypeIsRefused()
    {
        ImmutableArray<Diagnostic> diagnostics = await ReportedAsync("""
            namespace Aow.Engine
            {
                public class Node
                {
                    public Aow.Game.GameEngine? GetEngine() => null;
                }
            }
            """);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.Equal(EngineBoundaryAnalyzer.DiagnosticId, diagnostic.Id);
        Assert.Equal(("M:Aow.Engine.Node.GetEngine~Aow.Game.GameEngine", "T:Aow.Game.GameEngine"), Reference(diagnostic));
        Assert.Contains("M:Aow.Engine.Node.GetEngine~Aow.Game.GameEngine,T:Aow.Game.GameEngine", diagnostic.GetMessage(), comparisonType: StringComparison.Ordinal);
    }

    [Fact]
    public async Task MemberOfGameTypeIsRefusedByMember()
    {
        ImmutableArray<Diagnostic> diagnostics = await ReportedAsync("""
            using Aow.Game;

            namespace Aow.Engine
            {
                public class Node
                {
                    public int Count() => GameEngine.Create(1, level: 2);
                }
            }
            """);
        Assert.Equal(
            [("M:Aow.Engine.Node.Count~System.Int32", "T:Aow.Game.GameEngine"), ("M:Aow.Engine.Node.Count~System.Int32", "M:Aow.Game.GameEngine.Create(System.Int32,System.Int32)~System.Int32")],
            diagnostics.Select(Reference));
    }

    [Fact]
    public async Task VarOfGameTypeIsRefused()
    {
        ImmutableArray<Diagnostic> diagnostics = await ReportedAsync("""
            using Aow.Game;

            namespace Aow.Engine
            {
                public class Node
                {
                    public GameEngine? Engine { get; set; }

                    public int Use()
                    {
                        var engine = Engine;
                        return engine.Editors;
                    }
                }
            }
            """);
        Assert.Equal(
            [("P:Aow.Engine.Node.Engine", "T:Aow.Game.GameEngine"), ("M:Aow.Engine.Node.Use~System.Int32", "T:Aow.Game.GameEngine"), ("M:Aow.Engine.Node.Use~System.Int32", "P:Aow.Game.GameEngine.Editors")],
            diagnostics.Select(Reference));
    }

    [Fact]
    public async Task GameExtensionMethodIsRefused()
    {
        ImmutableArray<Diagnostic> diagnostics = await ReportedAsync("""
            using Aow.Game;

            namespace Aow.Engine
            {
                public class Node
                {
                    public GameEngine? Engine { get; set; }

                    public int Double() => Engine!.Twice();
                }
            }
            """);
        Assert.Equal(
            [("P:Aow.Engine.Node.Engine", "T:Aow.Game.GameEngine"), ("M:Aow.Engine.Node.Double~System.Int32", "M:Aow.Game.GameExtensions.Twice(Aow.Game.GameEngine)~System.Int32")],
            diagnostics.Select(Reference));
    }

    [Fact]
    public async Task GenericTypeArgumentOfGameTypeIsRefused()
    {
        ImmutableArray<Diagnostic> diagnostics = await ReportedAsync("""
            using System.Collections.Generic;

            namespace Aow.Engine
            {
                public class Node
                {
                    public List<Aow.Game.GameEngine> Engines { get; } = [];
                }
            }
            """);
        Assert.Equal([("P:Aow.Engine.Node.Engines", "T:Aow.Game.GameEngine")], diagnostics.Select(Reference));
    }

    [Fact]
    public async Task BaseListOfGameTypeIsRefused()
    {
        ImmutableArray<Diagnostic> diagnostics = await ReportedAsync("""
            namespace Aow.Engine
            {
                public class Special : Aow.Game.UnitBase;
            }
            """);
        Assert.Equal([("T:Aow.Engine.Special", "T:Aow.Game.UnitBase")], diagnostics.Select(Reference));
    }

    [Fact]
    public async Task AllowedReferencePasses()
    {
        const string Engine = """
            namespace Aow.Engine
            {
                public class Node
                {
                    public Aow.Game.GameEngine? Engine { get; set; }

                    public int Editors() => Engine!.Editors + Aow.Game.GameEngine.Create(1, level: 2);
                }
            }
            """;
        Assert.Equal(4, (await ReportedAsync(Engine)).Length);
        Assert.Empty(await ReportedAsync(Engine, allowList: """
            member,target
            P:Aow.Engine.Node.Engine,T:Aow.Game.GameEngine
            M:Aow.Engine.Node.Editors~System.Int32,P:Aow.Game.GameEngine.Editors
            M:Aow.Engine.Node.Editors~System.Int32,T:Aow.Game.GameEngine
            M:Aow.Engine.Node.Editors~System.Int32,"M:Aow.Game.GameEngine.Create(System.Int32,System.Int32)~System.Int32"
            """));
    }

    [Fact]
    public async Task StaleEntryIsReported()
    {
        ImmutableArray<Diagnostic> diagnostics = await ReportedAsync(
            """
            namespace Aow.Engine
            {
                public class Node;
            }
            """,
            allowList: """
            member,target
            M:Aow.Engine.Node.Gone,T:Aow.Game.Unit
            """);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.Same(EngineBoundaryAnalyzer.StaleRule, diagnostic.Descriptor);
        Assert.Equal(EngineBoundaryRules.AllowListFileName, diagnostic.Location.GetLineSpan().Path);
        Assert.Equal(1, diagnostic.Location.GetLineSpan().StartLinePosition.Line);
        Assert.Contains("M:Aow.Engine.Node.Gone,T:Aow.Game.Unit", diagnostic.GetMessage(), comparisonType: StringComparison.Ordinal);
    }

    [Fact]
    public async Task ReferenceInLocalFunctionBelongsToEnclosingMember()
    {
        ImmutableArray<Diagnostic> diagnostics = await ReportedAsync("""
            using Aow.Game;

            namespace Aow.Engine
            {
                public class Node
                {
                    public int Use(GameEngine engine)
                    {
                        int Double()
                        {
                            return engine.Editors + GameEngine.Create(1, level: 2);
                        }

                        return Double();
                    }
                }
            }
            """);
        Assert.Equal(4, diagnostics.Length);
        Assert.All(diagnostics, diagnostic => Assert.Equal("M:Aow.Engine.Node.Use(Aow.Game.GameEngine)~System.Int32", diagnostic.Properties[EngineBoundaryAnalyzer.MemberProperty]));
    }

    [Fact]
    public async Task MalformedAllowListLineIsReported()
    {
        ImmutableArray<Diagnostic> diagnostics = await ReportedAsync(
            """
            namespace Aow.Engine
            {
                public class Node;
            }
            """,
            allowList: """
            member,target
            M:Aow.Engine.Node.Gone
            """);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.Same(EngineBoundaryAnalyzer.MalformedRule, diagnostic.Descriptor);
        Assert.Equal(EngineBoundaryAnalyzer.DiagnosticId, diagnostic.Id);
        Assert.Equal(EngineBoundaryRules.AllowListFileName, diagnostic.Location.GetLineSpan().Path);
        Assert.Equal(1, diagnostic.Location.GetLineSpan().StartLinePosition.Line);
        Assert.Contains("M:Aow.Engine.Node.Gone", diagnostic.GetMessage(), comparisonType: StringComparison.Ordinal);
    }

    [Fact]
    public async Task EngineListedAndOutsideAssembliesPass()
        => Assert.Empty(await ReportedAsync("""
            using System.Collections.Generic;

            namespace Aow.Engine
            {
                public class Node
                {
                    public List<Node> Children { get; } = [];

                    public Aow.Rtl.Canvas? Canvas { get; set; }

                    public Node Self() => new();
                }
            }
            """));

    [Fact]
    public async Task UnlistedProductAssemblyIsRefused()
    {
        ImmutableArray<Diagnostic> diagnostics = await ReportedAsync("""
            namespace Aow.Engine
            {
                public class Node
                {
                    public Aow.Graphics.Color Color { get; set; }
                }
            }
            """);
        Assert.Equal([("P:Aow.Engine.Node.Color", "T:Aow.Graphics.Color")], diagnostics.Select(Reference));
    }

    [Fact]
    public async Task GameCodeIsNotChecked()
        => Assert.Empty(await ReportedAsync(
            """
            namespace Aow.Engine
            {
                public class Node;
            }
            """,
            game: Game + """

            namespace Aow.Game
            {
                public sealed class Army : Aow.Engine.Node
                {
                    public Unit? Leader { get; set; }
                }
            }
            """));

    [Fact]
    public async Task CompilationWithoutEngineFilesReportsNoStaleEntry()
        => Assert.Empty(await Harness.BoundaryDiagnosticsAsync(
            [("Game/Game.cs", Game)],
            boundary: Boundary,
            allowList: "member,target\nM:Aow.Engine.Node.Gone,T:Aow.Game.Unit\n"));

    [Fact]
    public async Task ProductEditorConfigMarksEngineFiles()
    {
        (string pattern, string boundary) = ProductEngineMarking();
        Assert.True(SectionMatchesEnginePath(pattern, path: "src/Engine/Node.cs"));
        Assert.True(SectionMatchesEnginePath(pattern, path: "src/Engine/Nodes/EngineObject.cs"));
        Assert.False(SectionMatchesEnginePath(pattern, path: "src/Game/Node.cs"));
        ImmutableArray<Diagnostic> diagnostics = await Harness.BoundaryDiagnosticsAsync(
            [("Engine/Node.cs", """
            namespace Aow.Engine
            {
                public class Node
                {
                    public Aow.Game.GameEngine? Engine { get; set; }
                }
            }
            """), ("Game/Game.cs", Game)],
            boundary: boundary,
            assemblies: [("Aow.Rtl", RtlLibrary), ("Aow.Graphics", GraphicsLibrary)]);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.Same(EngineBoundaryAnalyzer.Rule, diagnostic.Descriptor);
    }

    private static (string Pattern, string Boundary) ProductEngineMarking()
    {
        string? directory = AppContext.BaseDirectory;
        while (directory is not null && !(File.Exists(Path.Combine(directory, ".editorconfig")) && File.Exists(Path.Combine(directory, "analyzers", "EngineBoundaryAnalyzer.cs"))))
        {
            directory = Directory.GetParent(directory)?.FullName;
        }
        Assert.True(condition: directory is not null, userMessage: "The product .editorconfig was not found above the test output.");
        Assert.NotNull(directory);
        string[] lines = File.ReadAllLines(Path.Combine(directory, ".editorconfig"));
        int option = Array.FindIndex(lines, line => line.Trim().StartsWith("aow_engine_boundary =", comparisonType: StringComparison.Ordinal));
        Assert.InRange(option, low: 0, high: lines.Length - 1);
        int section = Array.FindLastIndex(lines, startIndex: option, count: option + 1, match: line => line.Trim().StartsWith("[", comparisonType: StringComparison.Ordinal));
        Assert.InRange(section, low: 0, high: option);
        string boundary = lines[option].Split('=', count: 2)[1].Trim();
        Assert.True(condition: boundary.Split(',').Select(name => name.Trim()).Where(name => name.Length > 0).ToArray() is [_, ..], userMessage: "The product .editorconfig names no boundary assembly.");
        return (lines[section].Trim(), boundary);
    }

    private static bool SectionMatchesEnginePath(string section, string path)
    {
        string inner = section.Trim('[', ']', '{', '}');
        return inner.Split(',').Any(pattern => GlobMatches(pattern.Trim(), path));
    }

    private static bool GlobMatches(string pattern, string path)
    {
        StringBuilder expression = new("^");
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i] == '*' && i + 1 < pattern.Length && pattern[i + 1] == '*')
            {
                _ = expression.Append(".*");
                i++;
                if (i + 1 < pattern.Length && pattern[i + 1] == '/')
                {
                    _ = expression.Append('/');
                    i++;
                }
            }
            else
            {
                _ = expression.Append(pattern[i] == '*' ? "[^/]*" : Regex.Escape(pattern[i].ToString()));
            }
        }
        _ = expression.Append('$');
        return Regex.IsMatch(input: path, pattern: expression.ToString(), options: RegexOptions.None, matchTimeout: TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void QuotedFieldsKeepTheirCommas()
    {
        EngineReference reference = new(Member: "M:Aow.Engine.Node.Edit(Aow.Game.GameEngine,System.Int32)", Target: "T:Aow.Game.GameEngine");
        string line = EngineBoundaryRules.FormatLine(reference);
        Assert.Equal("\"M:Aow.Engine.Node.Edit(Aow.Game.GameEngine,System.Int32)\",T:Aow.Game.GameEngine", line);
        string[] fields = [.. EngineBoundaryRules.ParseLine(line)];
        Assert.Equal([reference.Member, reference.Target], fields);
    }
}
