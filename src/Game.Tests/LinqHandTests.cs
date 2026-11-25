using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Tests;

// Issue 317 packages 2 and 3. Each fact pins a converted query on duplicates, an empty slot, and an empty list.
public sealed class LinqHandTests
{
    [Fact]
    public void ValueListSearchesReturnTheFirstMatchAndIgnoreSpareCapacity()
    {
        Int32List numbers = [];
        Assert.Equal(-1, numbers.IndexOf(4));
        _ = numbers.Add(4);
        _ = numbers.Add(0);
        _ = numbers.Add(4);
        numbers.Capacity = 8;
        numbers.List[3] = 9;
        Assert.Equal(0, numbers.IndexOf(4));
        Assert.Equal(1, numbers.IndexOf(0));
        Assert.Equal(-1, numbers.IndexOf(9));

        ByteList bytes = [];
        Assert.Equal(-1, bytes.IndexOf(2));
        _ = bytes.Add(2);
        _ = bytes.Add(0);
        _ = bytes.Add(2);
        bytes.Capacity = 8;
        bytes.List[3] = 9;
        Assert.Equal(0, bytes.IndexOf(2));
        Assert.Equal(1, bytes.IndexOf(0));
        Assert.Equal(-1, bytes.IndexOf(9));

        SByteList signed = [];
        Assert.Equal(-1, signed.IndexOf(-2));
        _ = signed.Add(-2);
        _ = signed.Add(0);
        _ = signed.Add(-2);
        Assert.Equal(0, signed.IndexOf(-2));
        Assert.Equal(1, signed.IndexOf(0));

        UInt16List words = [];
        Assert.Equal(-1, words.IndexOf(2));
        _ = words.Add(2);
        _ = words.Add(0);
        _ = words.Add(2);
        Assert.Equal(0, words.IndexOf(2));
        Assert.Equal(1, words.IndexOf(0));

        DoubleItemList pairs = [];
        Assert.Equal(-1, pairs.IndexOf(new DoubleItemList.DoubleItem(First: 1, Second: 2)));
        _ = pairs.Add(new DoubleItemList.DoubleItem(First: 1, Second: 2));
        _ = pairs.Add(new DoubleItemList.DoubleItem(First: 0, Second: 0));
        _ = pairs.Add(new DoubleItemList.DoubleItem(First: 1, Second: 2));
        Assert.Equal(0, pairs.IndexOf(new DoubleItemList.DoubleItem(First: 1, Second: 2)));
        Assert.Equal(1, pairs.IndexOfItemA(0));
        Assert.Equal(0, pairs.IndexOfItemB(2));

        QuadItemList quads = [];
        QuadItemList.QuadInteger quad = new(First: 1, Second: 2, Third: 3, Fourth: 4);
        Assert.Equal(-1, quads.IndexOf(quad));
        _ = quads.Add(quad);
        _ = quads.Add(new QuadItemList.QuadInteger(First: 0, Second: 0, Third: 0, Fourth: 0));
        _ = quads.Add(quad);
        Assert.Equal(0, quads.IndexOf(quad));
        Assert.Equal(1, quads.IndexOfItemA(0));
        Assert.Equal(0, quads.IndexOfItemD(4));
    }

    [Fact]
    public void BitCountsAndSearchesTreatFalseAsTheEmptySlot()
    {
        BitList empty = [];
        Assert.Equal(0, empty.GetBitsSet());
        Assert.Equal(0, empty.GetBitsCleared());
        Assert.Equal(-1, empty.IndexOf(true));

        // Add grows through Put, which raises the count to the new index. Count assigns three slots.
        BitList bits = [];
        bits.Count = 3;
        bits[0] = true;
        bits[2] = true;
        Assert.Equal(2, bits.GetBitsSet());
        Assert.Equal(1, bits.GetBitsCleared());
        Assert.Equal(0, bits.IndexOf(true));
        Assert.Equal(1, bits.IndexOf(false));
    }

    [Fact]
    public void Int32RandomChanceWeightSumThrowsPastInt32Max()
    {
        // The element add stays unchecked. The checked sum is what throws.
        Int32List weights = [];
        _ = weights.Add(int.MaxValue);
        _ = weights.Add(1);
        _ = Assert.Throws<OverflowException>(() => weights.RandomChanceItem(0));
    }

    [Fact]
    public void Int32DefaultRunsRoundTripThroughTheStream()
    {
        Int32List list = [];
        list.Default = 0;
        _ = list.Add(4);
        _ = list.Add(0);
        _ = list.Add(0);
        _ = list.Add(4);
        using MemoryStream stream = new();
        list.SaveToStream(stream);
        stream.Position = 0;
        Int32List loaded = [];
        loaded.LoadFromStream(stream);
        Assert.True(list.Compare(loaded));

        Int32List empty = [];
        using MemoryStream emptyStream = new();
        empty.SaveToStream(emptyStream);
        emptyStream.Position = 0;
        Int32List loadedEmpty = [];
        loadedEmpty.LoadFromStream(emptyStream);
        Assert.Empty(loadedEmpty);
    }
}

[Collection("Engine")]
[EngineState]
public sealed class LinqHandRandomTests
{
    [Fact]
    public void Int32RandomChanceUsesTheCheckedWeightSum()
    {
        int previous = DelphiRandom.RandSeed;
        try
        {
            Int32List empty = [];
            Assert.Equal(-1, empty.RandomChanceItem(0));

            Int32List zeros = [];
            _ = zeros.Add(0);
            _ = zeros.Add(0);
            DelphiRandom.RandSeed = 1;
            Assert.Equal(0, zeros.RandomChanceItem(0));

            // Seed 123456789 draws 1 from a total of 2. The first weight meets that draw, so the result is 1.
            Int32List weights = [];
            _ = weights.Add(1);
            _ = weights.Add(0);
            _ = weights.Add(1);
            DelphiRandom.RandSeed = 123456789;
            Assert.Equal(1, weights.RandomChanceItem(0));
        }
        finally
        {
            DelphiRandom.RandSeed = previous;
        }
    }
}

[Collection("Engine")]
[EngineState]
public sealed class LinqHandNodeTests
{
    [Fact]
    public void FindChildIndexKeepsTheLastMatchAndANullSlot()
    {
        SearchNode node = new();
        Assert.Equal(-1, node.FindChildIndex(null));
        Slot first = new();
        Slot second = new();
        _ = node.AddChild(first);
        _ = node.AddChild(second);
        node.SetChild(3, child: first);
        Assert.Equal(3, node.FindChildIndex(first));
        Assert.Equal(1, node.FindChildIndex(second));
        Assert.Equal(2, node.FindChildIndex(null));

        OwningNode owner = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(-1, owner.FindChildIndex(null));
        _ = owner.AddChild(first);
        _ = owner.AddChild(null);
        _ = owner.AddChild(first);
        Assert.Equal(2, owner.FindChildIndex(first));
        Assert.Equal(1, owner.FindChildIndex(null));
    }

    [Fact]
    public void FindClassReturnsTheFirstRegisteredClass()
    {
        EngineClass first = EngineClass.All[0];
        EngineClass second = EngineClass.All.First(engineClass => engineClass.ClassId != first.ClassId);
        EngineObjectClassList empty = [];
        Assert.Null(empty.FindClass(first.ClassId));
        Assert.Equal(-1, empty.FindClassIndex(first.ClassId));

        EngineObjectClassList list = [];
        _ = list.Add(first);
        _ = list.Add(second);
        _ = list.Add(first);
        Assert.Equal(0, list.FindClassIndex(first.ClassId));
        Assert.Same(first, list.FindClass(first.ClassId));
        _ = list.Add(null);
        _ = Assert.Throws<NullReferenceException>(() => list.FindClass(-1));
    }

    [Fact]
    public void EmptyGroupMinimaStayAtTheSeed()
    {
        AIGroup group = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(50, group.GetMovePointsMin());
        Assert.Equal(50, group.GetMovesMin());
    }

    [Fact]
    public void CooperateMoveDaysSkipTheFirstGroup()
    {
        AIGroupCooperationControl control = new(null) { Holder = EngineFixture.Engine };
        AIRecruitmentGroup first = new(null) { Holder = EngineFixture.Engine };
        AIRecruitmentGroup second = new(null) { Holder = EngineFixture.Engine };
        first.SetGroup(new AIGroup(null) { Holder = EngineFixture.Engine });
        first.MovePoints = 10_001;
        second.SetGroup(new AIGroup(null) { Holder = EngineFixture.Engine });
        second.MovePoints = 101;
        _ = control.Groups.AddChild(first);
        Assert.Equal(-1, control.GetCooperateMoveDays());
        _ = control.Groups.AddChild(second);
        Assert.Equal(2, control.GetCooperateMoveDays());
    }

    [Fact]
    public void EmptyProductionAndRecruitDaysStayAtTheSeed()
    {
        AIGroupProductionControl production = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(-1, production.GetCurrentProductionTime());
        AIGroupRecruitControl recruit = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(-1, recruit.GetRecruitMoveDays());
    }

    [Fact]
    public void FindUnitReturnsTheLastOfTwoMatchingGroups()
    {
        AIRecruitmentGroupList list = new(null!) { Holder = EngineFixture.Engine };
        Troop troop = new(null) { Holder = EngineFixture.Engine };
        Assert.Null(list.Groups.FindUnit(troop));
        Assert.False(list.Groups.UnitExists(troop));

        AIGroup firstGroup = new(null) { Holder = EngineFixture.Engine };
        AIGroup secondGroup = new(null) { Holder = EngineFixture.Engine };
        _ = firstGroup.AddChild(troop);
        _ = secondGroup.AddChild(troop);
        AIRecruitmentGroup first = new(null) { Holder = EngineFixture.Engine };
        AIRecruitmentGroup second = new(null) { Holder = EngineFixture.Engine };
        first.SetGroup(firstGroup);
        second.SetGroup(secondGroup);
        _ = list.AddChild(first);
        _ = list.AddChild(second);
        Assert.Same(second, list.Groups.FindUnit(troop));
        Assert.True(list.Groups.UnitExists(troop));
    }

    [Fact]
    public void FindObjectReturnsTheFirstMatchAndSkipsAnEmptySlot()
    {
        EngineObjectList objects = [];
        AIGroup first = new(null) { Holder = EngineFixture.Engine };
        AIGroup second = new(null) { Holder = EngineFixture.Engine };
        int classId = first.EngineClass.ClassId;
        Assert.Null(objects.FindObject(classId));
        Assert.Equal(-1, objects.FindObjectIndex(classId));
        _ = objects.Add(null);
        _ = objects.Add(new OwningNode(null) { Holder = EngineFixture.Engine });
        _ = objects.Add(first);
        _ = objects.Add(null);
        _ = objects.Add(second);
        Assert.Same(first, objects.FindObject(classId));
        Assert.Equal(2, objects.FindObjectIndex(classId));
    }

    [Fact]
    public void FindChildReturnsTheFirstMatchAndSkipsAnEmptySlot()
    {
        SearchNode node = new();
        AIGroup first = new(null) { Holder = EngineFixture.Engine };
        AIGroup second = new(null) { Holder = EngineFixture.Engine };
        int classId = first.EngineClass.ClassId;
        Assert.Null(node.FindChild(classId));
        _ = node.AddChild(null);
        _ = node.AddChild(first);
        _ = node.AddChild(null);
        _ = node.AddChild(second);
        Assert.Same(first, node.FindChild(classId));
    }

    [Fact]
    public void FindOwnedChildSkipsAnUnownedDuplicateAndAnEmptySlot()
    {
        OwningNode owner = new(null) { Holder = EngineFixture.Engine };
        AIGroup unowned = new(null) { Holder = EngineFixture.Engine };
        AIGroup first = new(null) { Holder = EngineFixture.Engine };
        AIGroup second = new(null) { Holder = EngineFixture.Engine };
        int classId = first.EngineClass.ClassId;
        Assert.Null(owner.FindChild(classId));
        Assert.Null(owner.FindOwnedChild(classId));
        _ = owner.AddChild(unowned);
        _ = owner.AddChild(null);
        first.SetOwner(owner);
        second.SetOwner(owner);
        Assert.Same(unowned, owner.FindChild(classId));
        Assert.Same(first, owner.FindOwnedChild(classId));
    }

    [Fact]
    public void FindChildInTreeReturnsTheFirstNestedMatchAndSkipsAnEmptySlot()
    {
        OwningNode root = new(null) { Holder = EngineFixture.Engine };
        OwningNode firstBranch = new(null) { Holder = EngineFixture.Engine };
        OwningNode secondBranch = new(null) { Holder = EngineFixture.Engine };
        AIGroup nested = new(null) { Holder = EngineFixture.Engine };
        AIGroup later = new(null) { Holder = EngineFixture.Engine };
        int classId = nested.EngineClass.ClassId;
        Assert.Null(root.FindChildInTree(classId));
        _ = firstBranch.AddChild(null);
        _ = firstBranch.AddChild(nested);
        _ = secondBranch.AddChild(later);
        _ = root.AddChild(null);
        _ = root.AddChild(firstBranch);
        _ = root.AddChild(secondBranch);
        Assert.Same(nested, root.FindChildInTree(classId));

        AIGroup direct = new(null) { Holder = EngineFixture.Engine };
        _ = root.AddChild(direct);
        Assert.Same(direct, root.FindChildInTree(classId));
    }

    private sealed class Slot : EngineObject
    {
        public Slot() : base(null) => Holder = EngineFixture.Engine;
    }

    private sealed class SearchNode : ObservableNode
    {
        public SearchNode() : base(null) => Holder = EngineFixture.Engine;
    }
}
