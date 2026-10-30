using System.Reflection;
using Aow.Units.Engine;

namespace Aow.Rtl.Tests;

public sealed class EngineLeafConstructionTests
{
    private static T Field<T>(object value, string name) => (T)value.GetType()
        .GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(value)!;

    private sealed class Owner : TECustomNode
    {
        public Owner() : base(null!) { }

        internal readonly List<TEObject> Members = [];
        internal readonly List<TEObject> ObservedBeforeLeafBody = [];
        public override int AddChild(TEObject child)
        {
            Assert.Same(this, child.Owner);
            if (child is TPlugInControl plugIn) Assert.Null(plugIn.UsedPlugIns);
            if (child is TEControlStyleSortNode style) Assert.Equal(default, style.GetControlStyle());
            if (child is TETimerNode timer)
            {
                var lists = Field<Aow.Units.FixedArray_Aow_Rtl_TList_10>(timer, "FTimerLists");
                for (int i = 0; i < 10; i++) Assert.Null(lists[i]);
            }
            ObservedBeforeLeafBody.Add(child);
            Members.Add(child);
            return Members.Count - 1;
        }
        public override int RemoveChild(TEObject child)
        {
            int index = Members.IndexOf(child);
            Members.Remove(child);
            return index;
        }
    }

    private static TEngine CreateEngine()
    {
        if (Aow.Units.Engine.Engine.Engines is null) Aow.Units.Engine.Engine.Initialize();
        var engine = new TEngine(null!);
        return engine;
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(1, true)]
    [InlineData(2, false)]
    [InlineData(2, true)]
    [InlineData(3, false)]
    [InlineData(3, true)]
    public void LeafCreationRoutesKeepTypeOwnerAndState(int kind, bool owned)
    {
        var engine = CreateEngine();
        var owner = new Owner();
        TECustomNode argument = owned ? owner : null!;
        Type type = kind switch
        {
            0 => typeof(TEControlStyleSortNode), 1 => typeof(TETimerNode),
            2 => typeof(TEUser), _ => typeof(TPlugInControl)
        };
        var meta = (TEObject.Meta)MetaRegistry.Of(type);
        engine.RegisterEClasses([EClass.Of(type)]);
        TEObject direct = kind switch
        {
            0 => new TEControlStyleSortNode(argument), 1 => new TETimerNode(argument),
            2 => new TEUser(argument), _ => new TPlugInControl(argument)
        };
        TEObject[] objects = [direct, meta.Create(argument), engine.CreateEObject(meta.ClassID(), argument)];
        TEObject? copy = null;
        try
        {
            Assert.Equal(kind == 0 ? 261 : kind == 1 ? 264 : 80, meta.ClassID());
            Assert.Equal(kind switch { 0 => 28, 1 => 56, 2 => 12, _ => 16 }, meta.InstanceSize);
            foreach (var value in objects)
            {
                Assert.IsType(type, value);
                Assert.Same(argument, value.Owner);
                Assert.Same(meta, value.ClassType);
                AssertLeafState(value);
            }
            if (direct is TEControlStyleSortNode style)
                style.SetControlStyle(TSet<TEControlStyleElement>.Of(
                    Aow.Units.Engine.Engine.csStored, Aow.Units.Engine.Engine.csShared));
            copy = direct.Copy(engine);
            Assert.IsType(type, copy);
            Assert.Null(copy.Owner);
            // Sort style is constructor state and has no ReadWrite override.
            AssertLeafState(copy);
            if (copy is TPlugInControl copiedPlugIn)
                Assert.NotSame(((TPlugInControl)direct).UsedPlugIns, copiedPlugIn.UsedPlugIns);
            if (copy is TETimerNode copiedTimer)
            {
                var original = Field<Aow.Units.FixedArray_Aow_Rtl_TList_10>(direct, "FTimerLists");
                var copied = Field<Aow.Units.FixedArray_Aow_Rtl_TList_10>(copiedTimer, "FTimerLists");
                for (int i = 0; i < 10; i++) Assert.NotSame(original[i], copied[i]);
            }
            Assert.Equal(owned ? 3 : 0, owner.Members.Count);
            Assert.Equal(owned ? 3 : 0, owner.ObservedBeforeLeafBody.Count);
            Assert.Throws<InvalidOperationException>(() => meta.NewInstance());
        }
        finally
        {
            copy?.Destroy();
            foreach (var value in objects) value.Destroy();
            engine.Destroy();
        }
        Assert.Empty(owner.Members);
    }

    private static void AssertLeafState(TEObject value)
    {
        switch (value)
        {
            case TEControlStyleSortNode sort:
                Assert.Equal(0, sort.Count); Assert.Equal(0, Field<int>(sort, "FCapacity")); Assert.Empty(Field<TSortItem[]>(sort, "FItems"));
                Assert.False(Field<bool>(sort, "FSorted"));
                Assert.Equal(TSet<TEControlStyleElement>.Of(Aow.Units.Engine.Engine.csStored), sort.GetControlStyle());
                break;
            case TETimerNode timer:
                Assert.Equal(0, timer.Count); Assert.Equal(0, timer.Tick);
                var lists = Field<Aow.Units.FixedArray_Aow_Rtl_TList_10>(timer, "FTimerLists");
                for (int i = 0; i < 10; i++) { Assert.NotNull(lists[i]); Assert.Equal(0, lists[i].Count); Assert.Equal(0, lists[i].Capacity); }
                Assert.Equal(10, Enumerable.Range(0, 10).Select(i => lists[i]).Distinct().Count());
                break;
            case TEUser user: Assert.Null(user.Node); break;
            case TPlugInControl plugIn:
                Assert.Null(plugIn.Engine);
                var strings = Assert.IsType<TStringList>(plugIn.UsedPlugIns);
                Assert.Equal(0, strings.Count); Assert.True(strings.Sorted);
                Assert.Equal(plugIn, strings.OnChange!.Target);
                Assert.Equal(plugIn, strings.OnChanging!.Target);
                break;
        }
    }

    private sealed class ConvertedNode : TECustomNode
    {
        internal int OwnerCalls;
        internal ConvertedNode(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
        public override int GetCount() => 0;
    }



    private sealed class ConvertedSort : TESortNode
    {
        internal int OwnerCalls;
        internal ConvertedSort(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner)
        {
            OwnerCalls++;
            base.SetOwner(owner);
            FItems = new TSortItem[1];
        }
    }





    [Fact]
    public void SortStyleUsesTheNonvirtualInheritedStyleAndBothMetaclassFacades()
    {
        var value = (TEControlStyleSortNode)TEControlStyleSortNode.Meta.Of<TEControlStyleSortNode>().Create(null!);
        var other = (TEControlStyleSortNode)TESortNode.Meta.Of<TEControlStyleSortNode>().Create(null!);
        try { AssertLeafState(value); AssertLeafState(other); }
        finally { value.Destroy(); other.Destroy(); }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    public void HelperConstructionKeepsTheFactoryState(int kind)
    {
        TObject value = kind switch
        {
            0 => new TBitList(),
            1 => new TCallBackEventList(),
            2 => new TDoubleItemList(),
            3 => new TEBufferStream(),
            4 => new TEObjectList(),
            5 => new TEventList(),
            6 => new TPropertyTable(),
            7 => new TQuadItemList(),
            8 => new TShortIntList(),
            9 => new TWordList(),
            10 => new TEObjectClassList(),
            _ => new TEObjectEditorList()
        };
        try
        {
            switch (value)
            {
                case TBitList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.True(Field<Memory<byte>>(list, "FList").IsEmpty);
                    break;
                case TCallBackEventList list:
                    Assert.Equal(0, list.Count); Assert.Empty(Field<List<TCallBackEvent>>(list, "FList"));
                    break;
                case TDoubleItemList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.Empty(list.List);
                    break;
                case TEBufferStream stream:
                    Assert.Equal(0, stream.Size); Assert.Equal(0, stream.Position);
                    Assert.Equal(0, stream.Capacity); Assert.Empty(stream.Memory);
                    break;
                case TEventList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity);
                    Assert.Empty(list.List); Assert.Null(list.Default);
                    break;
                case TPropertyTable table:
                    Assert.Empty(Field<Array>(table, "FItems")); Assert.Empty(Field<Array>(table, "FStack"));
                    Assert.Equal(0, Field<int>(table, "FCount")); Assert.Equal(0, Field<int>(table, "FCapacity"));
                    Assert.Equal(0, Field<int>(table, "FNextIndex")); Assert.Equal(0, Field<int>(table, "FMaxID"));
                    Assert.Equal(0, Field<int>(table, "FStackCount")); Assert.Equal(0, Field<int>(table, "FStackCapacity"));
                    Assert.Equal(0, Field<int>(table, "FFirst"));
                    break;
                case TQuadItemList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.Empty(list.List);
                    break;
                case TShortIntList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.Empty(list.List);
                    break;
                case TWordList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.Empty(list.List);
                    break;
                case TEObjectEditorList editors:
                    Assert.Equal(0, editors.Count);
                    break;
                case TList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity);
                    break;
                default: Assert.Fail("Missing helper assertion"); break;
            }
        }
        finally { value.Destroy(); }
    }
}
