using System.Reflection;
using Aow.Engine;
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Tests;

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

    private static GameEngine CreateEngine()
    {
        if (EngineHost.Engines is null) EngineHost.Initialize();
        GameEngine engine = new GameEngine(null!);
        return engine;
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(1, true)]
    public void LeafCreationRoutesKeepTypeOwnerAndState(int kind, bool owned)
    {
        GameEngine engine = CreateEngine();
        var owner = new LeafOwner();
        ECustomNode argument = owned ? owner : null!;
        Type type = kind switch
        {
            0 => typeof(ETimerNode),
            _ => typeof(PlugInControl)
        };
        var meta = EClass.Of(type);
        engine.RegisterEClasses([EClass.Of(type)]);
        EObject direct = kind switch
        {
            0 => new ETimerNode(argument),
            _ => new PlugInControl(argument)
        };
        EObject[] objects = [direct, meta.Create(argument), engine.CreateEObject(meta.ClassID, argument)];
        EObject? copy = null;
        try
        {
            Assert.Equal(kind == 0 ? 264 : 80, meta.ClassID);
            Assert.Equal(kind == 0 ? 56 : 16, meta.InstanceSize);
            foreach (var value in objects)
            {
                Assert.IsType(type, value);
                Assert.Same(argument, value.Owner);
                Assert.Same(meta, value.EClass);
                AssertLeafState(value);
            }
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
            case ETimerNode timer:
                Assert.Equal(0, timer.Count); Assert.Equal(0, timer.Tick);
                var lists = Field<FixedArray_Aow_Rtl_TList_10>(timer, "FTimerLists");
                for (int i = 0; i < 10; i++) { Assert.NotNull(lists[i]); Assert.Equal(0, lists[i].Count); Assert.Equal(0, lists[i].Capacity); }
                Assert.Equal(10, Enumerable.Range(0, 10).Select(i => lists[i]).Distinct().Count());
                break;
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
    public void HelperConstructionKeepsTheFactoryState(int kind)
    {
        object value = kind switch
        {
            0 => new BitList(),
            1 => new DoubleItemList(),
            2 => new EBufferStream(),
            3 => new EObjectList(),
            4 => new PropertyTable(),
            5 => new QuadItemList(),
            6 => new ShortIntList(),
            7 => new WordList(),
            8 => new EObjectClassList(),
            _ => new EObjectEditorList()
        };
        try
        {
            switch (value)
            {
                case BitList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.True(Field<Memory<byte>>(list, "FList").IsEmpty);
                    break;
                case DoubleItemList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.Empty(list.List);
                    break;
                case EBufferStream stream:
                    Assert.Equal(0L, stream.Length); Assert.Equal(0L, stream.Position);
                    Assert.Equal(0, stream.Capacity); Assert.Empty(stream.Memory);
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
