using System.Reflection;
using Aow.Engine;
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Tests;

public sealed class EngineLeafConstructionTests
{
    private static T Field<T>(object value, string name) => (T)MemberNames.Field(value.GetType(), name)!.GetValue(value)!;

    private sealed class LeafOwner : EngineFixture.Owner
    {

        internal List<EngineObject> ObservedBeforeLeafBody { get; } = [];

        public override int AddChild(EngineObject child)
        {
            Assert.Same(this, child.Owner);
            if (child is PluginControl plugin) Assert.Null(plugin.UsedPlugins);
            if (child is ETimerNode timer)
            {
                var lists = Field<ListArray10>(timer, "_timerLists");
                for (int i = 0; i < 10; i++) Assert.Null(lists[i]);
            }
            ObservedBeforeLeafBody.Add(child);
            base.AddChild(child);
            return Members.Count - 1;
        }
    }

    private static GameEngine CreateEngine()
    {
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
            _ => typeof(PluginControl)
        };
        var meta = EngineClass.Of(type);
        engine.RegisterEngineClasses([EngineClass.Of(type)]);
        EngineObject direct = kind switch
        {
            0 => new ETimerNode(argument),
            _ => new PluginControl(argument)
        };
        EngineObject[] objects = [direct, meta.CreateInstance(argument), engine.CreateEngineObject(meta.ClassId, argument)];
        EngineObject? copy = null;
        try
        {
            Assert.Equal(kind == 0 ? 264 : 80, meta.ClassId);
            foreach (var value in objects)
            {
                Assert.IsType(type, value);
                Assert.Same(argument, value.Owner);
                Assert.Same(meta, value.EngineClass);
                AssertLeafState(value);
            }
            copy = direct.Copy(engine);
            Assert.IsType(type, copy);
            Assert.Null(copy.Owner);
            // Sort style is constructor state and has no ReadWrite override.
            AssertLeafState(copy);
            if (copy is PluginControl copiedPlugin)
                Assert.NotSame(((PluginControl)direct).UsedPlugins, copiedPlugin.UsedPlugins);
            if (copy is ETimerNode copiedTimer)
            {
                var original = Field<ListArray10>(direct, "_timerLists");
                var copied = Field<ListArray10>(copiedTimer, "_timerLists");
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

    private static void AssertLeafState(EngineObject value)
    {
        switch (value)
        {
            case ETimerNode timer:
                Assert.Equal(0, timer.Count); Assert.Equal(0, timer.Tick);
                var lists = Field<ListArray10>(timer, "_timerLists");
                for (int i = 0; i < 10; i++) { Assert.NotNull(lists[i]); Assert.Equal(0, lists[i].Count); Assert.Equal(0, lists[i].Capacity); }
                Assert.Equal(10, Enumerable.Range(0, 10).Select(i => lists[i]).Distinct().Count());
                break;
            case PluginControl plugin:
                Assert.Null(plugin.Engine);
                var strings = Assert.IsType<StringList>(plugin.UsedPlugins);
                Assert.Equal(0, strings.Count); Assert.True(strings.Sorted);
                Assert.Equal(plugin, strings.OnChange!.Target);
                Assert.Null(strings.OnChanging);
                break;
        }
    }

    private sealed class ConvertedNode : ECustomNode
    {
        private int _ownerCalls;

        internal ConvertedNode(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { _ownerCalls++; base.SetOwner(owner); }

        public override int GetCount() => 0;
    }

    private sealed class ConvertedSort : ESortNode
    {
        private int _ownerCalls;

        internal ConvertedSort(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner)
        {
            _ownerCalls++;
            base.SetOwner(owner);
            Items = new SortItem[1];
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
            2 => new BufferStream(),
            3 => new EngineObjectList(),
            4 => new PropertyTable(),
            5 => new QuadItemList(),
            6 => new SByteList(),
            7 => new UInt16List(),
            8 => new EngineObjectClassList(),
            _ => new EngineObjectEditorList()
        };
        try
        {
            switch (value)
            {
                case BitList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.True(Field<Memory<byte>>(list, "List").IsEmpty);
                    break;
                case DoubleItemList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.Empty(list.List);
                    break;
                case BufferStream stream:
                    Assert.Equal(0L, stream.Length); Assert.Equal(0L, stream.Position);
                    Assert.Equal(0, stream.Capacity); Assert.Empty(stream.Memory);
                    break;
                case PropertyTable table:
                    Assert.Empty(Field<Array>(table, "_items")); Assert.Empty(Field<Array>(table, "_stack"));
                    Assert.Equal(0, Field<int>(table, "_count")); Assert.Equal(0, Field<int>(table, "_capacity"));
                    Assert.Equal(0, Field<int>(table, "_nextIndex")); Assert.Equal(0, Field<int>(table, "_maxId"));
                    Assert.Equal(0, Field<int>(table, "_stackCount")); Assert.Equal(0, Field<int>(table, "_stackCapacity"));
                    Assert.Equal(0, Field<int>(table, "_first"));
                    break;
                case QuadItemList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.Empty(list.List);
                    break;
                case SByteList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.Empty(list.List);
                    break;
                case UInt16List list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.Empty(list.List);
                    break;
                case EngineObjectEditorList editors:
                    Assert.Equal(0, editors.Count);
                    break;
                case EngineObjectList children:
                    Assert.Equal(0, children.Count); Assert.Equal(0, children.Capacity); Assert.Empty(children.List);
                    break;
                case EngineObjectClassList list:
                    Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity);
                    break;
                default: Assert.Fail("Missing helper assertion"); break;
            }
        }
        finally { ((value ?? throw new NullReferenceException()) as IDisposable)?.Dispose(); }
    }
}
