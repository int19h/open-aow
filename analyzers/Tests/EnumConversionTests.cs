using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Aow.Analyzers.Tests;

public sealed class EnumConversionTests
{
    private const string Library = """
        using System;
        using System.Runtime.CompilerServices;

        namespace Aow.Rtl
        {
            [AttributeUsage(AttributeTargets.Struct)]
            public sealed class StoredLayoutAttribute : Attribute;
        }

        namespace Aow.Formats.Tree
        {
            public interface IPersister
            {
                int Int32(string name, int value);
            }
        }

        namespace Game
        {
            public enum Terrain : byte { Grass = 0, Water = 6, Lava = 7 }
            public enum Other { First, Second }
            public enum TerrainTypes : uint { None = 0, Grass = 1 }
        }
        """;

    private static async Task<ImmutableArray<Diagnostic>> ReportedDiagnosticsAsync(string members, string space = "Game.Logic", string? namespaces = null, string? allowList = null, string? allowListFile = null)
    {
        string source = Library + "\nnamespace " + space + "\n{\n    using Game;\n    using Aow.Formats.Tree;\n" + members + "\n}\n";
        return await Harness.DiagnosticsAsync(source, analyzer: new EnumConversionAnalyzer(), namespaces: namespaces, allowList: allowList, allowListFile: allowListFile);
    }

    private static async Task<string[]> ReportedAsync(string members, string space = "Game.Logic", string? namespaces = null, string? allowList = null, string? allowListFile = null)
    {
        ImmutableArray<Diagnostic> diagnostics = await ReportedDiagnosticsAsync(members, space: space, namespaces: namespaces, allowList: allowList, allowListFile: allowListFile);
        return [.. diagnostics.Select(diagnostic => diagnostic.Properties[EnumConversionAnalyzer.KindProperty]!)];
    }

    [Fact]
    public async Task CastsInGameLogicAreReported()
        => Assert.Equal(["ConstantCast", "Cast", "EnumToEnumCast", "UnboxingCast"], await ReportedAsync("""
            public static class Rules
            {
                public static Terrain Constant() => (Terrain)6;
                public static Terrain Value(int index) => (Terrain)index;
                public static Terrain Retype(Other other) => (Terrain)other;
                public static Terrain Unbox(int index) => (Terrain)(object)index;
            }
            """));

    [Fact]
    public async Task CallsInGameLogicAreReported()
        => Assert.Equal(["EnumToObject", "EnumParse", "EnumTryParse", "Reinterpretation"], await ReportedAsync("""
            public static class Rules
            {
                public static object Box(int index) => Enum.ToObject(typeof(Terrain), index);
                public static Terrain Parse(string text) => Enum.Parse<Terrain>(text);
                public static bool TryParseTerrain(string text, out Terrain terrain) => Enum.TryParse(text, out terrain);
                public static Terrain Pun(ref byte value) => System.Runtime.CompilerServices.Unsafe.As<byte, Terrain>(ref value);
            }
            """));

    [Fact]
    public async Task OtherConversionsPass()
        => Assert.Empty(await ReportedAsync("""
            public static class Rules
            {
                public static Terrain Zero() => 0;
                public static int Ordinal(Terrain terrain) => (int)terrain;
                public static Terrain Same(Terrain terrain) => (Terrain)terrain;
                public static ref Terrain Alias(ref Terrain terrain) => ref System.Runtime.CompilerServices.Unsafe.AsRef(ref terrain);
            }
            """));

    [Fact]
    public async Task ReadAndWriteCodePasses()
        => Assert.Empty(await ReportedAsync("""
            public sealed class Field
            {
                public Terrain Terrain { get; set; }

                public void Persist(IPersister persister) => Terrain = (Terrain)persister.Int32("Terrain", (int)Terrain);

                public void PersistLater(IPersister persister)
                {
                    Func<int, Terrain> read = value => (Terrain)value;
                    Terrain = read(persister.Int32("Terrain", (int)Terrain));
                }
            }

            [Aow.Rtl.StoredLayout]
            public struct FieldImage
            {
                public byte TerrainImage;

                public readonly Terrain Terrain => (Terrain)TerrainImage;
            }

            public static class StreamReader
            {
                public static Terrain Read(System.IO.Stream stream) => (Terrain)stream.ReadByte();
            }

            public sealed class Reader : IPersister
            {
                public int Int32(string name, int value) => (int)(Terrain)value;
            }
            """));

    [Fact]
    public async Task ListedNamespacePasses()
    {
        const string Members = "public static class Codec { public static Terrain Read(byte value) => (Terrain)value; }";
        Assert.Empty(await ReportedAsync(Members, space: "Game.Codecs", namespaces: "Game.Codecs, Game.Other"));
        Assert.Equal(["Cast"], await ReportedAsync(Members, space: "Game.Codecs", namespaces: "Game.Other"));
    }

    [Fact]
    public async Task AllowListedMemberPasses()
    {
        const string Members = """
            public static class Rules
            {
                public static Terrain Value(int index) => (Terrain)index;
            }
            """;
        Assert.Equal(["Cast"], await ReportedAsync(Members));
        Assert.Empty(await ReportedAsync(Members, allowList: """
            # The members that keep a conversion for now.
            M:Game.Logic.Rules.Value(System.Int32)~Game.Terrain

            """));
    }

    [Fact]
    public async Task OtherMembersStillReported()
    {
        const string Members = """
            public static class Rules
            {
                public static Terrain Listed(int index) => (Terrain)index;
                public static Terrain Missing(int index) => (Terrain)index;
            }
            """;
        Assert.Equal(["Cast"], await ReportedAsync(Members, allowList: "M:Game.Logic.Rules.Listed(System.Int32)~Game.Terrain"));
    }

    [Fact]
    public async Task AllowListFileNameFilters()
    {
        const string Members = """
            public static class Rules
            {
                public static Terrain Value(int index) => (Terrain)index;
            }
            """;
        const string AllowList = "M:Game.Logic.Rules.Value(System.Int32)~Game.Terrain";
        Assert.Equal(["Cast"], await ReportedAsync(Members, allowList: AllowList, allowListFile: "Other.txt"));
        Assert.Empty(await ReportedAsync(Members, allowList: AllowList));
    }

    [Fact]
    public async Task MalformedAllowListLinesPass()
    {
        const string Members = """
            public static class Rules
            {
                public static Terrain Value(int index) => (Terrain)index;
            }
            """;
        Assert.Equal(["Cast"], await ReportedAsync(Members, allowList: "not an id"));
        Assert.Empty(await ReportedAsync(Members, allowList: """
            # A comment and a malformed line stay out.
            not an id
            M:Game.Logic.Rules.Value(System.Int32)~Game.Terrain
            trailing garbage
            """));
    }

    [Fact]
    public async Task PropertyAccessorConversionBelongsToProperty()
    {
        const string Members = """
            public class Holder
            {
                private int image;

                public Terrain Terrain => (Terrain)image;
            }
            """;
        Assert.Equal(["Cast"], await ReportedAsync(Members));
        Assert.Empty(await ReportedAsync(Members, allowList: "P:Game.Logic.Holder.Terrain"));
    }

    [Fact]
    public async Task ConstructorConversionBelongsToConstructor()
    {
        const string Members = """
            public class Holder
            {
                public Terrain Terrain;

                public Holder(int image)
                {
                    Terrain = (Terrain)image;
                }
            }
            """;
        Assert.Equal(["Cast"], await ReportedAsync(Members));
        Assert.Empty(await ReportedAsync(Members, allowList: "M:Game.Logic.Holder.#ctor(System.Int32)"));
    }

    [Fact]
    public async Task PrimaryConstructorBaseCallBelongsToType()
    {
        const string Members = """
            public class Base
            {
                public Base(Terrain terrain)
                {
                }
            }

            public class Child(int index) : Base((Terrain)index)
            {
            }
            """;
        Assert.Equal(["Cast"], await ReportedAsync(Members));
        Assert.Empty(await ReportedAsync(Members, allowList: "T:Game.Logic.Child"));
    }

    [Fact]
    public async Task MultiVariableFieldConversionBelongsToDeclarator()
    {
        const string Members = """
            public static class Fields
            {
                public static Terrain First = 0, Second = (Terrain)1;
            }
            """;
        Assert.Equal(["ConstantCast"], await ReportedAsync(Members));
        Assert.Empty(await ReportedAsync(Members, allowList: "F:Game.Logic.Fields.Second"));
    }

    [Fact]
    public async Task ExtensionBlockConversionReportsGeneratedId()
    {
        const string Members = """
            public static class TerrainConversions
            {
                extension(Terrain value)
                {
                    public TerrainTypes ToFlag() => (TerrainTypes)(1U << (int)value);
                }
            }
            """;
        ImmutableArray<Diagnostic> diagnostics = await ReportedDiagnosticsAsync(Members);
        string id = Assert.Single(diagnostics).Properties[EnumConversionAnalyzer.MemberIdProperty]!;
        Assert.StartsWith("M:Game.Logic.TerrainConversions.", id, comparisonType: StringComparison.Ordinal);
        Assert.EndsWith(".ToFlag~Game.TerrainTypes", id, comparisonType: StringComparison.Ordinal);
        Assert.Contains(id, diagnostics[0].GetMessage(), comparisonType: StringComparison.Ordinal);
        Assert.Empty(await ReportedDiagnosticsAsync(Members, allowList: id));
    }

    [Fact]
    public async Task ByRefParameterConversionBelongsToSignature()
    {
        const string Members = """
            public static class Rules
            {
                public static Terrain Convert(ref int image) => (Terrain)image;
            }
            """;
        Assert.Equal(["Cast"], await ReportedAsync(Members));
        Assert.Empty(await ReportedAsync(Members, allowList: "M:Game.Logic.Rules.Convert(System.Int32@)~Game.Terrain"));
    }

    [Fact]
    public async Task SameNameOverloadWithOtherParametersStillReported()
    {
        const string Members = """
            public static class Rules
            {
                public static Terrain Convert(int image) => (Terrain)image;
                public static Terrain Convert(ref int image) => (Terrain)image;
            }
            """;
        ImmutableArray<Diagnostic> diagnostics = await ReportedDiagnosticsAsync(Members, allowList: "M:Game.Logic.Rules.Convert(System.Int32)~Game.Terrain");
        Assert.Equal("M:Game.Logic.Rules.Convert(System.Int32@)~Game.Terrain", Assert.Single(diagnostics).Properties[EnumConversionAnalyzer.MemberIdProperty]);
    }

    [Fact]
    public async Task LambdaAndLocalFunctionConversionsBelongToMember()
    {
        const string Members = """
            public static class Rules
            {
                public static Terrain Convert(int image)
                {
                    Func<Terrain> convert = () => (Terrain)image;
                    Terrain Local() => (Terrain)image;
                    return convert() == Local() ? convert() : Local();
                }
            }
            """;
        Assert.Equal(["Cast", "Cast"], await ReportedAsync(Members));
        Assert.Empty(await ReportedAsync(Members, allowList: "M:Game.Logic.Rules.Convert(System.Int32)~Game.Terrain"));
    }

    [Fact]
    public async Task EnumParseInListedMemberPasses()
    {
        const string Members = """
            public static class Rules
            {
                public static Terrain Parse(string text) => Enum.Parse<Terrain>(text);
            }
            """;
        Assert.Equal(["EnumParse"], await ReportedAsync(Members));
        Assert.Empty(await ReportedAsync(Members, allowList: "M:Game.Logic.Rules.Parse(System.String)~Game.Terrain"));
    }

    [Fact]
    public async Task ConstantCastBecomesMember()
    {
        string source = Library + """

            namespace Game.Logic
            {
                public static class Rules
                {
                    public static Game.Terrain Water() => (Game.Terrain)6;
                }
            }
            """;
        string fixedSource = await Harness.FixAsync(source, analyzer: new EnumConversionAnalyzer(), fix: new EnumConversionCodeFix());
        Assert.Contains("=> Game.Terrain.Water;", fixedSource, comparisonType: StringComparison.Ordinal);
    }

    [Fact]
    public async Task ValueWithoutMemberHasNoFix()
    {
        string source = Library + """

            namespace Game.Logic
            {
                public static class Rules
                {
                    public static Game.Terrain Unknown() => (Game.Terrain)3;
                }
            }
            """;
        Assert.False(await Harness.HasFixAsync(source, analyzer: new EnumConversionAnalyzer(), fix: new EnumConversionCodeFix()));
    }
}
