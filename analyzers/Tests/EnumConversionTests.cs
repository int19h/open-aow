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
        }
        """;

    private static async Task<string[]> ReportedAsync(string members, string space = "Game.Logic", string? namespaces = null, string? allowList = null)
    {
        string source = Library + "\nnamespace " + space + "\n{\n    using Game;\n    using Aow.Formats.Tree;\n" + members + "\n}\n";
        ImmutableArray<Diagnostic> diagnostics = await Harness.DiagnosticsAsync(source, analyzer: new EnumConversionAnalyzer(), namespaces: namespaces, allowList: allowList);
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
        => Assert.Equal(["EnumToObject", "EnumParse", "Reinterpretation"], await ReportedAsync("""
            public static class Rules
            {
                public static object Box(int index) => Enum.ToObject(typeof(Terrain), index);
                public static Terrain Parse(string text) => Enum.Parse<Terrain>(text);
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
