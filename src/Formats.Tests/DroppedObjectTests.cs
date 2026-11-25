using System.Collections.ObjectModel;
using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Formats.Tests;

/// <summary>
/// Issue 384, site by site: each place where the tree loader goes on without an object names that
/// object in the dropped list by class, document path and message, and the load leaves the same
/// state with and without the list. A part stored with a negative number fails its read.
/// </summary>
public sealed class DroppedObjectTests : FormatsTest
{
    private static string PartClass => NameTable.Stored(typeof(Part));

    [Fact]
    public void EachSiteNamesTheObjectItGoesOnWithout()
    {
        ObjectNode tree = TreeWriter.Write(Holder.WithFailures(), factory: new PartFactory());
        List<DroppedObject> dropped = [];
        Holder recorded = new();
        TreeReader.ReadObject(tree, objectValue: recorded, factory: new PartFactory(), dropped: dropped);
        Holder unrecorded = new();
        TreeReader.ReadObject(tree, objectValue: unrecorded, factory: new PartFactory());

        // The holder reads its slots, its grid, its sequence and then its list.
        Assert.Equal(["root.Slots[2]", "root.Fields[1][1]", "root.Sequence[1]", "root.Items[1]"], dropped.Select(static item => item.Path));
        Assert.All(dropped, static item => Assert.Equal(PartClass, item.ClassName));
        Assert.All(dropped, static item => Assert.Equal("bad part -1", item.Message));
        Assert.All(dropped, static item => Assert.True(item.Error is InvalidOperationException));
        AssertSame(unrecorded, recorded);
        Assert.Equal([0, 1, -1], recorded.Slots.Select(static part => part.Number));
        Assert.Equal([10, 11, 12, -1], recorded.Fields.Select(static part => part.Number));
        Assert.Equal([20], recorded.Sequence.Select(static part => part.Number));
        Assert.Equal([30, -2], recorded.Items.Select(static part => part?.Number ?? -2));
    }

    [Fact]
    public void AHandlerThatStopsAtTheSecondFailureKeepsTheFirstEntry()
    {
        ObjectNode tree = TreeWriter.Write(Holder.WithFailures(), factory: new PartFactory());
        List<DroppedObject> dropped = [];
        int callCount = 0;
        void ContinueOnce(string _, out bool continueLoad) => continueLoad = ++callCount == 1;

        InvalidOperationException error = Assert.Throws<InvalidOperationException>(() =>
            TreeReader.ReadObject(tree, objectValue: new Holder(), factory: new PartFactory(), onError: ContinueOnce, dropped: dropped));

        Assert.Equal("bad part -1", error.Message);
        Assert.Equal(2, callCount);
        DroppedObject first = Assert.Single(dropped);
        Assert.Equal("root.Slots[2]", first.Path);
    }

    [Fact]
    public void AFixedRootThatFailsIsNamedAtTheRoot()
    {
        ObjectNode tree = TreeWriter.Write(new Part { Number = -1 }, factory: new PartFactory());
        List<DroppedObject> fromReadObject = [];
        Part readObject = new();
        TreeReader.ReadObject(tree, objectValue: readObject, factory: new PartFactory(), dropped: fromReadObject);
        List<DroppedObject> fromRead = [];
        Part read = new();
        TreeReader.Read(tree, objectValue: read, factory: new PartFactory(), dropped: fromRead);

        foreach (List<DroppedObject> list in (List<DroppedObject>[])[fromReadObject, fromRead])
        {
            DroppedObject root = Assert.Single(list);
            Assert.Equal("root", root.Path);
            Assert.Equal(PartClass, root.ClassName);
            Assert.Equal("bad part -1", root.Message);
        }

        Assert.Equal(-1, readObject.Number);
        Assert.Equal(-1, read.Number);
        // Without a list Read stays strict at the root, as before issue 384.
        _ = Assert.Throws<InvalidOperationException>(() => TreeReader.Read(tree, objectValue: new Part(), factory: new PartFactory()));
    }

    [Fact]
    public void AFactoryThatCannotNameAFixedObjectFallsBackToItsTypeName()
    {
        ObjectNode tree = TreeWriter.Write(Holder.WithFailures(), factory: new PartFactory());
        List<DroppedObject> dropped = [];
        Holder recorded = new();
        TreeReader.ReadObject(tree, objectValue: recorded, factory: new UnnamingFactory(), dropped: dropped);
        Holder unrecorded = new();
        TreeReader.ReadObject(tree, objectValue: unrecorded, factory: new UnnamingFactory());

        Assert.Equal(["root.Slots[2]", "root.Fields[1][1]", "root.Sequence[1]", "root.Items[1]"], dropped.Select(static item => item.Path));
        // A fixed object takes its class from the factory. A created one takes it from the tree.
        Assert.Equal([nameof(Part), nameof(Part), PartClass, PartClass], dropped.Select(static item => item.ClassName));
        AssertSame(unrecorded, recorded);
    }

    [Fact]
    public void AListThatRefusesAnEntryLeavesTheLoadAsItWas()
    {
        ObjectNode tree = TreeWriter.Write(Holder.WithFailures(), factory: new PartFactory());
        Holder recorded = new();
        TreeReader.ReadObject(tree, objectValue: recorded, factory: new PartFactory(), dropped: new RefusingList());
        Holder unrecorded = new();
        TreeReader.ReadObject(tree, objectValue: unrecorded, factory: new PartFactory());

        AssertSame(unrecorded, recorded);
    }

    private static void AssertSame(Holder expected, Holder actual)
    {
        Assert.Equal(expected.Slots.Select(static part => part.Number), actual.Slots.Select(static part => part.Number));
        Assert.Equal(expected.Fields.Select(static part => part.Number), actual.Fields.Select(static part => part.Number));
        Assert.Equal(expected.Sequence.Select(static part => part.Number), actual.Sequence.Select(static part => part.Number));
        Assert.Equal(expected.Items.Select(static part => part?.Number), actual.Items.Select(static part => part?.Number));
        Assert.Equal(expected.Number, actual.Number);
    }

    private sealed class Part : IPersistable
    {
        public int Number { get; set; }

        public void Persist(IPersister persister)
        {
            Number = persister.Int32("N", Number);
            if (persister.Loading && Number < 0)
            {
                throw new InvalidOperationException($"bad part {Number}");
            }
        }
    }

    // Three fixed slots, a 2 by 2 grid of fixed fields, a sequence of created parts and a list of
    // created parts, each read by its own site of the tree loader.
    private sealed class Holder : IPersistable
    {
        public Part[] Slots { get; } = [new(), new(), new()];

        public Part[] Fields { get; } = [new(), new(), new(), new()];

        public List<Part> Sequence { get; } = [];

        public List<Part?> Items { get; } = [];

        public int Number { get; set; }

        public static Holder WithFailures()
        {
            Holder holder = new() { Number = 7 };
            int[] slots = [0, 1, -1];
            int[] fields = [10, 11, 12, -1];
            for (int i = 0; i < slots.Length; i++)
            {
                holder.Slots[i].Number = slots[i];
            }

            for (int i = 0; i < fields.Length; i++)
            {
                holder.Fields[i].Number = fields[i];
            }

            holder.Sequence.Add(new Part { Number = 20 });
            holder.Sequence.Add(new Part { Number = -1 });
            holder.Items.Add(new Part { Number = 30 });
            holder.Items.Add(new Part { Number = -1 });
            return holder;
        }

        public void Persist(IPersister persister)
        {
            if (persister.Loading)
            {
                Sequence.Clear();
            }

            for (int i = 0; i < Slots.Length; i++)
            {
                persister.ObjectSlot("Slots", slot: i, objectValue: Slots[i]);
            }

            persister.Grid("Fields", width: 2, height: 2, fields: Fields);
            persister.CreatedSequence("Sequence", items: new PartList(Sequence));
            persister.List("Items", items: Items, transfer: static (slot, value) => (Part?)slot.Object(value));
            Number = persister.Int32("Number", Number);
        }
    }

    private sealed class PartList(List<Part> parts) : IChildCollection
    {
        public int Count => parts.Count;

        public IPersistable Get(int index) => parts[index];

        public bool IsStored(int index) => true;

        public int Slot(int index) => index;

        public bool KeepIndex => false;

        public void Attach(IPersistable child)
        {
        }

        public void Place(int slot, IPersistable child) => parts.Add((Part)child);
    }

    private class PartFactory : IPersistableFactory
    {
        public virtual string ClassName(IPersistable objectValue) => NameTable.Stored(objectValue.GetType());

        public IPersistable Create(string className)
            => NameTable.Names(typeof(Part), name: className) ? new Part() : throw new InvalidOperationException($"The test factory has no class {className}.");
    }

    // Reads as the part factory does, and cannot name an object.
    private sealed class UnnamingFactory : PartFactory
    {
        public override string ClassName(IPersistable objectValue) => throw new InvalidOperationException("no name");
    }

    private sealed class RefusingList : Collection<DroppedObject>
    {
        protected override void InsertItem(int index, DroppedObject item) => throw new NotSupportedException("refused");
    }
}
