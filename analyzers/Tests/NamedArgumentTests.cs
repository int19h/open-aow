using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Aow.Analyzers.Tests;

public sealed class NamedArgumentTests
{
    private const string Library = """
        using System;
        using System.Collections.Generic;
        using System.Linq.Expressions;

        public class Sample
        {
            public Sample() { }
            public Sample(int count, int limit) { }
            public static void Two(int count, int limit) { }
            public static void Flag(int count, bool visible) { }
            public static void Flags(bool flag1, bool flag2) { }
            public static void Format(string format, params object[] args) { }
            public static void Run(int count, Func<int, int> step) { }
            public static void RunFirst(Func<int, int> step, int count) { }
            public static void Move(object sprite, int x, int y) { }
            public static void Copy(int sourceX, int sourceY) { }
            public static void Box(int x, int y, int width, int height) { }
            public static void Size(int width, int depth) { }
            public static void Field(int x, int y, int mapLevel) { }
            public static void Pair(string name, int value) { }
            public static void Setting(string key, string name, ref bool value) { }
            public static int Max(int val1, int val2) => val1;
            public static int Expression(int count, int limit) => count;
            public static bool TryFind(int key, out int value) { value = key; return true; }
            public static void Keyword(int count, int @event) { }
            public int this[int x, int y] => x;
            public int Limit => 0;
        }

        public static class Extensions
        {
            public static int Add(this int value, int first, int second) => value;
        }

        public sealed class SampleList : List<int>
        {
            public void Add(int first, int second) { }
        }

        [AttributeUsage(AttributeTargets.Class)]
        public sealed class PairAttribute(int count, int limit) : Attribute;

        public class Derived : Sample
        {
            public Derived() : base(1, 2) { }
        }

        public sealed class Primary(int count) : Sample(count, 2);

        [Pair(1, 2)]
        public sealed class Marked;
        """;

    private static async Task<string[]> ReportedAsync(string body)
    {
        string source = Library + "\npublic static class Calls\n{\n    private static int _limit;\n    public static void Body(Sample other, int limit)\n    {\n" + body + "\n    }\n}\n";
        ImmutableArray<Diagnostic> diagnostics = await Harness.DiagnosticsAsync(source, analyzer: new NamedArgumentAnalyzer());
        return [.. diagnostics.Where(diagnostic => diagnostic.Location.SourceSpan.Start > Library.Length).Select(diagnostic => diagnostic.Properties[NamedArgumentAnalyzer.ParameterProperty]!)];
    }

    [Fact]
    public async Task SecondArgumentNeedsName() => Assert.Equal(["limit"], await ReportedAsync("Sample.Two(1, 2);"));

    [Fact]
    public async Task OneArgumentAndNamedArgumentsPass()
        => Assert.Empty(await ReportedAsync("Sample.Max(1, val2: 2); Sample.Two(1, limit: 2); _ = new List<int>(4);"));

    [Fact]
    public async Task BooleanNeedsNameEvenInSeries() => Assert.Equal(["visible", "flag2"], await ReportedAsync("Sample.Flag(1, true); Sample.Flags(true, false);"));

    [Fact]
    public async Task ParamsElementsTakeNoName() => Assert.Empty(await ReportedAsync("Sample.Format(\"{0}{1}\", 1, 2);"));

    [Fact]
    public async Task TrailingLambdaTakesNoName() => Assert.Equal(["count"], await ReportedAsync("Sample.Run(1, x => x); Sample.RunFirst(x => x, 2);"));

    [Fact]
    public async Task SameNameTakesNoName()
        => Assert.Empty(await ReportedAsync("Sample.Two(1, limit); Sample.Two(1, _limit); Sample.Two(1, other.Limit); _ = Sample.TryFind(1, out int value); Sample.Flag(1, visible: true);"));

    [Fact]
    public async Task CallResultNeedsName() => Assert.Equal(["limit"], await ReportedAsync("Sample.Two(1, other.GetHashCode());"));

    [Fact]
    public async Task ConventionGroupsTakeNoName()
        => Assert.Empty(await ReportedAsync("Sample.Move(other, 10, 20); Sample.Copy(1, 2); Sample.Box(1, 2, 3, 4); _ = Sample.Max(1, 2); Sample.Field(1, 2, 0); Sample.Pair(\"Gold\", 5);"));

    [Fact]
    public async Task GroupWithBooleanNeedsNames() => Assert.Equal(["name", "value"], await ReportedAsync("bool on = false; Sample.Setting(\"Key\", \"Name\", ref on);"));

    [Fact]
    public async Task IncompleteGroupNeedsName() => Assert.Equal(["depth"], await ReportedAsync("Sample.Size(1, 2);"));

    [Fact]
    public async Task ExpressionTreeTakesNoName() => Assert.Empty(await ReportedAsync("Expression<Func<int>> tree = () => Sample.Expression(1, 2);"));

    [Fact]
    public async Task IndexersAttributesAndInitializersTakeNoName()
        => Assert.Empty(await ReportedAsync("_ = other[1, 2]; _ = new SampleList { { 1, 2 } }; _ = new Dictionary<int, int> { [1] = 2 };"));

    [Fact]
    public async Task ConstructorsNeedNames() => Assert.Equal(["limit", "limit"], await ReportedAsync("_ = new Sample(1, 2); Sample implicitSample = new(1, 2);"));

    [Fact]
    public async Task ConstructorInitializersNeedNames()
    {
        ImmutableArray<Diagnostic> diagnostics = await Harness.DiagnosticsAsync(Library, analyzer: new NamedArgumentAnalyzer());
        Assert.Equal(2, diagnostics.Length);
    }

    [Fact]
    public async Task ReducedExtensionCountsFromItsArgumentList() => Assert.Equal(["second"], await ReportedAsync("_ = 5.Add(1, 2);"));

    [Fact]
    public async Task ExternalMethodsFollowTheRule()
        => Assert.Equal(["comparisonType"], await ReportedAsync("_ = string.Equals(\"a\", \"b\", StringComparison.Ordinal); _ = Math.Max(1, 2);"));

    [Fact]
    public async Task FixNamesEveryArgumentOfTheCall()
    {
        string source = Library + """

            public static class Calls
            {
                public static void Body()
                {
                    Sample.Two(1, 2);
                    Sample.Keyword(1, 2);
                    Sample.Format("{0}", 1, true);
                    Sample.Flags(
                        true,
                        false);
                }
            }
            """;
        string fixedSource = await Harness.FixAsync(source, analyzer: new NamedArgumentAnalyzer(), fix: new NamedArgumentCodeFix());
        string expected = (Library + """

            public static class Calls
            {
                public static void Body()
                {
                    Sample.Two(1, limit: 2);
                    Sample.Keyword(1, @event: 2);
                    Sample.Format("{0}", 1, true);
                    Sample.Flags(
                        true,
                        flag2: false);
                }
            }
            """).Replace("public Derived() : base(1, 2)", "public Derived() : base(1, limit: 2)", comparisonType: StringComparison.Ordinal)
            .Replace(": Sample(count, 2);", ": Sample(count, limit: 2);", comparisonType: StringComparison.Ordinal);
        Assert.Equal(Harness.Lines(expected), Harness.Lines(fixedSource));
    }

    [Fact]
    public async Task FixKeepsParamsAfterNamedArgumentValid()
    {
        string source = """
            public static class Calls
            {
                public static void Log(int level, string format, params object[] args) { }
                public static void Body() => Log(1, "{0} {1}", 2, 3);
            }
            """;
        string fixedSource = await Harness.FixAsync(source, analyzer: new NamedArgumentAnalyzer(), fix: new NamedArgumentCodeFix());
        Assert.Contains("Log(1, format: \"{0} {1}\", 2, 3)", fixedSource, comparisonType: StringComparison.Ordinal);
    }
}
