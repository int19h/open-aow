using System.Reflection;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;

namespace Aow.Units.Tests;

public sealed class EngineLeafConstructionTests
{
    private static T Field<T>(object value, string name) => (T)value.GetType()
        .GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(value)!;

    private sealed class LeafOwner : EngineFixture.Owner
    {

        internal readonly List<EObject> ObservedBeforeLeafBody = [];

        public override int AddChild(EObject child)
        {
            Assert.Same(this, child.Owner);
            if (child is PlugInControl plugIn) Assert.Null(plugIn.UsedPlugIns);
            if (child is EControlStyleSortNode style) Assert.Equal(default, style.GetControlStyle());
            if (child is ETimerNode timer)
            {
                var lists = Field<FixedArray_Aow_Rtl_TList_10>(timer, "FTimerLists");
                for (int i = 0; i < 10; i++) Assert.Null(lists[i]);
            }
            ObservedBeforeLeafBody.Add(child);
            base.AddChild(child);
            return Members.Count - 1;
        }
    }

    private static Engine CreateEngine()
    {
        if (EngineHost.Engines is null) EngineHost.Initialize();
        var engine = new Engine(null!);
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
        var owner = new LeafOwner();
        ECustomNode argument = owned ? owner : null!;
        Type type = kind switch
        {
            0 => typeof(EControlStyleSortNode),
            1 => typeof(ETimerNode),
            2 => typeof(EUser),
            _ => typeof(PlugInControl)
        };
        var meta = EClass.Of(type);
        engine.RegisterEClasses([EClass.Of(type)]);
        EObject direct = kind switch
        {
            0 => new EControlStyleSortNode(argument),
            1 => new ETimerNode(argument),
            2 => new EUser(argument),
            _ => new PlugInControl(argument)
        };
        EObject[] objects = [direct, meta.Create(argument), engine.CreateEObject(meta.ClassID, argument)];
        EObject? copy = null;
        try
        {
            Assert.Equal(kind == 0 ? 261 : kind == 1 ? 264 : 80, meta.ClassID);
            Assert.Equal(kind switch { 0 => 28, 1 => 56, 2 => 12, _ => 16 }, meta.InstanceSize);
            foreach (var value in objects)
            {
                Assert.IsType(type, value);
                Assert.Same(argument, value.Owner);
                Assert.Same(meta, value.EClass);
                AssertLeafState(value);
            }
            if (direct is EControlStyleSortNode style)
                style.SetControlStyle(Set<EControlStyleElement>.Of(
                    EngineServices.csStored, EngineServices.csShared));
            copy = direct.Copy(engine);
            Assert.IsType(type, copy);
            Assert.Null(copy.Owner);
            // Sort style is constructor state and has no ReadWrite override.
            AssertLeafState(copy);
            if (copy is PlugInControl copiedPlugIn)
                Assert.NotSame(((PlugInControl)direct).UsedPlugIns, copiedPlugIn.UsedPlugIns);
            if (copy is ETimerNode copiedTimer)
            {
                var original = Field<FixedArray_Aow_Rtl_TList_10>(direct, "FTimerLists");
                var copied = Field<FixedArray_Aow_Rtl_TList_10>(copiedTimer, "FTimerLists");
                for (int i = 0; i < 10; i++) Assert.NotSame(original[i], copied[i]);
            }
            Assert.Equal(owned ? 3 : 0, owner.Members.Count);
            Assert.Equal(owned ? 3 : 0, owner.ObservedBeforeLeafBody.Count);
        }
        finally
        {
            copy?.Dispose();
            foreach (var value in objects) value.Dispose();
            engine.Dispose();
        }
        Assert.Empty(owner.Members);
    }

    private static void AssertLeafState(EObject value)
    {
        switch (value)
        {
            case EControlStyleSortNode sort:
                Assert.Equal(0, sort.Count); Assert.Equal(0, Field<int>(sort, "FCapacity")); Assert.Empty(Field<ESortNode.SortItem[]>(sort, "FItems"));
                Assert.False(Field<bool>(sort, "FSorted"));
                Assert.Equal(Set<EControlStyleElement>.Of(EngineServices.csStored), sort.GetControlStyle());
                break;
            case ETimerNode timer:
                Assert.Equal(0, timer.Count); Assert.Equal(0, timer.Tick);
                var lists = Field<FixedArray_Aow_Rtl_TList_10>(timer, "FTimerLists");
                for (int i = 0; i < 10; i++) { Assert.NotNull(lists[i]); Assert.Equal(0, lists[i].Count); Assert.Equal(0, lists[i].Capacity); }
                Assert.Equal(10, Enumerable.Range(0, 10).Select(i => lists[i]).Distinct().Count());
                break;
            case EUser user: Assert.Null(user.Node); break;
            case PlugInControl plugIn:
                Assert.Null(plugIn.Engine);
                var strings = Assert.IsType<StringList>(plugIn.UsedPlugIns);
                Assert.Equal(0, strings.Count); Assert.True(strings.Sorted);
                Assert.Equal(plugIn, strings.OnChange!.Target);
                Assert.Equal(plugIn, strings.OnChanging!.Target);
                break;
        }
    }

    private sealed class ConvertedNode : ECustomNode
    {
        internal int OwnerCalls;

        internal ConvertedNode(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }

        public override int GetCount() => 0;
    }

    private sealed class ConvertedSort : ESortNode
    {
        internal int OwnerCalls;

        internal ConvertedSort(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner)
        {
            OwnerCalls++;
            base.SetOwner(owner);
            FItems = new SortItem[1];
        }
    }

    [Fact]
    public void SortStyleUsesTheNonvirtualInheritedStyleAndBothMetaclassFacades()
    {
        var value = (EControlStyleSortNode)EClass.Of<EControlStyleSortNode>().Create(null!);
        var other = (EControlStyleSortNode)EClass.Of<EControlStyleSortNode>().Create(null!);
        try { AssertLeafState(value); AssertLeafState(other); }
        finally { value.Dispose(); other.Dispose(); }
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
        object value = kind switch
        {
            0 => new BitList(),
            1 => new CallBackEventList(),
            2 => new DoubleItemList(),
            3 => new EBufferStream(),
            4 => new EObjectList(),
            5 => new EventList(),
            6 => new PropertyTable(),
            7 => new QuadItemList(),
            8 => new ShortIntList(),
            9 => new WordList(),
            10 => new EObjectClassList(),
            _ => new EObjectEditorList()
        };
        try
        {
            switch (value)
            {
                case BitList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.True(Field<Memory<byte>>(list, "FList").IsEmpty);
                    break;
                case CallBackEventList list:
                    Assert.Equal(0, list.Count); Assert.Empty(Field<List<CallBackEventList.CallBackEvent>>(list, "FList"));
                    break;
                case DoubleItemList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.Empty(list.List);
                    break;
                case EBufferStream stream:
                    Assert.Equal(0L, stream.Length); Assert.Equal(0L, stream.Position);
                    Assert.Equal(0, stream.Capacity); Assert.Empty(stream.Memory);
                    break;
                case EventList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity);
                    Assert.Empty(list.List); Assert.Null(list.Default);
                    break;
                case PropertyTable table:
                    Assert.Empty(Field<Array>(table, "FItems")); Assert.Empty(Field<Array>(table, "FStack"));
                    Assert.Equal(0, Field<int>(table, "FCount")); Assert.Equal(0, Field<int>(table, "FCapacity"));
                    Assert.Equal(0, Field<int>(table, "FNextIndex")); Assert.Equal(0, Field<int>(table, "FMaxID"));
                    Assert.Equal(0, Field<int>(table, "FStackCount")); Assert.Equal(0, Field<int>(table, "FStackCapacity"));
                    Assert.Equal(0, Field<int>(table, "FFirst"));
                    break;
                case QuadItemList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.Empty(list.List);
                    break;
                case ShortIntList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.Empty(list.List);
                    break;
                case WordList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.Empty(list.List);
                    break;
                case EObjectEditorList editors:
                    Assert.Equal(0, editors.Count);
                    break;
                case DelphiList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity);
                    break;
                default: Assert.Fail("Missing helper assertion"); break;
            }
        }
        finally { ((value ?? throw new NullReferenceException()) as IDisposable)?.Dispose(); }
    }
}
