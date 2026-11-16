using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class HashTableReplacementTests
{
    [Fact]
    public void UnitTableHitsMissesAndKeepsTheFirstDuplicate()
    {
        UnitControl control = new(null!) { Holder = EngineFixture.Engine };
        Unit unit = new(null!) { Holder = EngineFixture.Engine, UnitId = 7 };
        control.RegisterUnit(unit);
        Assert.Same(unit, control.FindUnit(7));
        Assert.Same(unit, control.FindUnit<Unit>(7));
        Assert.Null(control.FindUnit(8));
        Assert.Null(control.FindUnit<Unit>(8));
        Assert.Null(control.FindUnit<Hero>(7));

        InvalidOperationException duplicate = Assert.Throws<InvalidOperationException>(() =>
            HashTables.Add(control.UnitTable, key: 7, value: new Unit(null!) { Holder = EngineFixture.Engine }));
        Assert.Equal("ID already in HashTable", duplicate.Message);
        Assert.Same(unit, control.FindUnit(7));
    }

    [Fact]
    public void UnregisterRemovesOnlyTheStoredUnit()
    {
        UnitControl control = new(null!) { Holder = EngineFixture.Engine };
        Unit first = new(null!) { Holder = EngineFixture.Engine, UnitId = 4 };
        control.RegisterUnit(first);
        Unit other = new(null!)
        {
            Holder = EngineFixture.Engine,
            UnitId = 4,
            UnitStatus = Set<UnitStatusElement>.Of(UnitStatusElement.Registered)
        };
        control.UnregisterUnit(other);
        Assert.Same(first, control.FindUnit(4));
        Assert.Contains(first, control.Units);
        control.UnregisterUnit(first);
        Assert.Null(control.FindUnit(4));
        Assert.DoesNotContain(first, control.Units);
    }

    [Fact]
    public void ClassTableHitsMissesAndRejectsASecondRegistration()
    {
        GameEngine engine = new(null!);
        try
        {
            EngineClass style = EngineClass.Of<ControlStyleObject>();
            Assert.Same(style, engine.GetEngineClass(style.ClassId));
            KeyNotFoundException missing = Assert.Throws<KeyNotFoundException>(() => engine.GetEngineClass(-999999));
            Assert.Equal("Unregistered E-ClassID found (-999999)", missing.Message);
            InvalidOperationException duplicate = Assert.Throws<InvalidOperationException>(() => engine.RegisterEngineClasses([style]));
            Assert.Equal("EObject Class ID double defined!", duplicate.Message);
            Assert.Same(style, engine.GetEngineClass(style.ClassId));
            engine.UnregisterEngineClasses([style]);
            Assert.Throws<KeyNotFoundException>(() => engine.GetEngineClass(style.ClassId));
        }
        finally
        {
            engine.Dispose();
        }
    }

    [Fact]
    public void ImageLibraryNamesHitMissAndRejectASecondFile()
    {
        ImageLibraryManagerList list = new(new ImageLibraryManager(null!));
        ImageLibraryManagerObject added = list.AddImageLibraryFile("Town.ilb");
        Assert.True(list.HashTable.TryGetValue("TOWN.ILB", out ImageLibraryManagerObject? found));
        Assert.Same(added, found);
        Assert.False(list.HashTable.ContainsKey("missing.ilb"));
        Assert.Null(list.LinkToImageLibrary("missing.ilb"));
        Assert.False(list.ReleaseImageLibrary("missing.ilb"));
        InvalidOperationException duplicate = Assert.Throws<InvalidOperationException>(() => list.AddImageLibraryFile("town.ilb"));
        Assert.Equal("ID already in HashTable", duplicate.Message);
        Assert.Same(added, list.HashTable["TOWN.ILB"]);
    }

    [Fact]
    public void AnsiCompareKeysKeepTheOriginalCollation()
    {
        Dictionary<string, string> table = [with(AnsiCompareComparer.Instance)];
        HashTables.Add(table, key: "A", value: "upper");
        HashTables.Add(table, key: "a", value: "lower");
        Assert.Equal("upper", table["A"]);
        Assert.Equal("lower", table["a"]);
        Assert.False(table.ContainsKey("missing"));
        HashTables.Add(table, key: "\u00ad", value: "soft");
        InvalidOperationException soft = Assert.Throws<InvalidOperationException>(() => HashTables.Add(table, key: "\0", value: "nul"));
        Assert.Equal("ID already in HashTable", soft.Message);
        Assert.Equal("soft", table["\0"]);
        HashTables.Add(table, key: "\u02c6", value: "hat");
        InvalidOperationException hat = Assert.Throws<InvalidOperationException>(() => HashTables.Add(table, key: "^", value: "caret"));
        Assert.Equal("ID already in HashTable", hat.Message);
        Assert.Equal("hat", table["^"]);
        Assert.Equal(AnsiCompareComparer.Instance.GetHashCode("\u00ad"), AnsiCompareComparer.Instance.GetHashCode("\0"));
        Assert.Equal(AnsiCompareComparer.Instance.GetHashCode("\u02c6"), AnsiCompareComparer.Instance.GetHashCode("^"));
    }
}
