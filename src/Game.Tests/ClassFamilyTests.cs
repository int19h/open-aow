using System.Reflection;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Session;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Briefs 9.4-3 and 9.4-4: the instance virtuals that left the metaclass, the isometric hexagon's
// one-constructor dynamic sprite, and the descriptor families (AI player actions, AI groups,
// editors) that a list reads without an instance.
[Collection("Engine")]
[EngineState]
public sealed class ClassFamilyTests
{
    // A map object's owner is a field. This one answers the engine the editor lookup reads.
    private sealed class EngineField(GameEngine engine) : EngineFixture.RecordingField
    {
        public override GameEngine GetEngine() => engine;
    }

    private sealed class UnregisteredAction(ECustomNode owner) : AIPlayerAction(owner);

    private sealed class UnregisteredGroup(ECustomNode owner) : AIGroup(owner);

    private sealed class UnregisteredEditor(ECustomNode owner) : EngineObjectEditor(owner);

    [Fact]
    public void UnregisteredTypesHaveNoFamilyDescriptor()
    {
        Assert.Throws<InvalidOperationException>(() => AIPlayerActionClass.Of<UnregisteredAction>());
        Assert.Throws<InvalidOperationException>(() => AIPlayerActionClass.Of(typeof(UnregisteredAction)));
        Assert.Throws<InvalidOperationException>(() => AIGroupClass.Of<UnregisteredGroup>());
        Assert.Throws<InvalidOperationException>(() => AIGroupClass.Of(typeof(UnregisteredGroup)));
        Assert.Throws<InvalidOperationException>(() => EditorClass.Of<UnregisteredEditor>());
        Assert.Throws<InvalidOperationException>(() => EditorClass.Of(typeof(UnregisteredEditor)));
    }

    [Fact]
    public void EachProductFamilyImplementorHasOneExplicitRegistration()
    {
        static void Verify(Type contract, IEnumerable<Type> registered)
        {
            var expected = typeof(EngineObject).Assembly.GetTypes().Where(t => t.IsClass && contract.IsAssignableFrom(t)).Select(t => t.FullName).Order(StringComparer.Ordinal);
            Assert.Equal(expected, registered.Where(t => t.Assembly == typeof(EngineObject).Assembly).Select(t => t.FullName).Order(StringComparer.Ordinal));
        }
        Verify(typeof(IAIPlayerActionClass), AIPlayerActionClass.All.Select(c => c.Class.Type));
        Verify(typeof(IAIGroupClass), AIGroupClass.All.Select(c => c.Class.Type));
        Verify(typeof(IEngineObjectEditorClass), EditorClass.All.Select(c => c.Class.Type));
        Assert.Throws<AbstractErrorException>(() => AIPlayerActionClass.Of<AIPlayerAction>().Priority);
        Assert.Throws<AbstractErrorException>(() => AIPlayerActionClass.Of<StructureAIPlayerAction>().Priority);
        Assert.Equal("EndTurnAIPlayerAction", AIPlayerActionClass.Of<EndTurnAIPlayerAction>().StorageKey);
        Assert.Equal("HuntAIGroup", AIGroupClass.Of<HuntAIGroup>().StorageKey);
        Assert.Equal("EngineObjectEditor", EditorClass.Of<EngineObjectEditor>().StorageKey);
        Assert.Throws<InvalidOperationException>(() => AIPlayerActionClass.Register<EndTurnAIPlayerAction>());
        Assert.Throws<InvalidOperationException>(() => AIGroupClass.Register<HuntAIGroup>());
        Assert.Throws<InvalidOperationException>(() => EditorClass.Register<EngineObjectEditor>());
    }

    [Fact]
    public void ActionDescriptorsAreOnePerClassAndReadTheStatics()
    {
        var endTurn = AIPlayerActionClass.Of<EndTurnAIPlayerAction>();
        Assert.Same(endTurn, AIPlayerActionClass.Of(typeof(EndTurnAIPlayerAction)));
        Assert.Same(EngineClass.Of<EndTurnAIPlayerAction>(), endTurn.Class);
        Assert.Equal(EndTurnAIPlayerAction.Priority, endTurn.Priority);
        Assert.NotEqual(GroupControlLoopAIPlayerAction.Priority, endTurn.Priority);
        // the phases are inherited from TAIPlayerAction, the loop action declares its own
        Assert.True(endTurn.SupportedPhases.Contains(AIPlayerProcessPhase.Main));
        Assert.False(endTurn.SupportedPhases.Contains(AIPlayerProcessPhase.Loop));
        var loop = AIPlayerActionClass.Of<GroupControlLoopAIPlayerAction>();
        Assert.True(loop.SupportedPhases.Contains(AIPlayerProcessPhase.Loop));
        Assert.False(loop.SupportedPhases.Contains(AIPlayerProcessPhase.Main));
        // the base statics are abstract and the default
        Assert.Throws<AbstractErrorException>(() => AIPlayerAction.Priority);
        Assert.True(AIPlayerAction.SupportedPhases.Contains(AIPlayerProcessPhase.Main));
        using var instance = new EndTurnAIPlayerAction(null!) { Holder = EngineFixture.Engine };
        Assert.Same(endTurn, instance.ActionClass);
        Assert.Same(instance.ActionClass, instance.ActionClass);
    }

    [Fact]
    public void EveryFamilyClassReadsTheStaticsItsNearestDeclarationGives()
    {
        // The descriptor of each class reads the static that C# member lookup finds from that class,
        // which is the value the Delphi class method returned through the class reference.
        static object? Nearest(Type type, string name)
        {
            const BindingFlags Flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
            for (var t = type; t != null; t = t.BaseType)
                if (t.GetProperty(name, Flags) is { } property) return property.GetValue(null);
            throw new InvalidOperationException(name);
        }
        var types = typeof(AIPlayerAction).Assembly.GetTypes();
        var actions = types.Where(t => typeof(AIPlayerAction).IsAssignableFrom(t) && t != typeof(AIPlayerAction) && t != typeof(StructureAIPlayerAction)).ToList();
        Assert.Equal(13, actions.Count);
        foreach (var type in actions)
        {
            var descriptor = AIPlayerActionClass.Of(type);
            Assert.Same(EngineClass.Of(type), descriptor.Class);
            Assert.Equal(Nearest(type, nameof(AIPlayerAction.Priority)), descriptor.Priority);
            Assert.Equal(Nearest(type, nameof(AIPlayerAction.SupportedPhases)), descriptor.SupportedPhases);
        }
        var groups = types.Where(t => typeof(AIGroup).IsAssignableFrom(t)).ToList();
        Assert.Equal(19, groups.Count);
        foreach (var type in groups)
        {
            var descriptor = AIGroupClass.Of(type);
            Assert.Same(EngineClass.Of(type), descriptor.Class);
            Assert.Equal(Nearest(type, nameof(AIGroup.Behavior)), descriptor.Behavior);
        }
    }

    [Fact]
    public void ActionManagerRegistersSortsAndUnregistersDescriptors()
    {
        using var manager = new AIPlayerActionManager(null!) { Holder = EngineFixture.Engine };
        AIPlayerActionClass[] classes = [AIPlayerActionClass.Of<EndTurnAIPlayerAction>(), AIPlayerActionClass.Of<InitializeAIPlayerAction>(), AIPlayerActionClass.Of<GroupControlLoopAIPlayerAction>()];
        manager.RegisterClasses(classes);
        Assert.Equal(3, manager.GetCount());
        var registered = Enumerable.Range(0, 3).Select(manager.GetRegisteredClasses).ToList();
        foreach (var c in classes) Assert.Contains(c, registered);
        // CompareAIPlayerActionClasses orders by descending priority
        for (int i = 1; i < registered.Count; i++) Assert.True(registered[i - 1].Priority >= registered[i].Priority);
        var error = Assert.Throws<InvalidOperationException>(() => manager.RegisterClasses([AIPlayerActionClass.Of<EndTurnAIPlayerAction>()]));
        Assert.Equal("AIPlayerAction already Registered", error.Message);
        manager.UnregisterClasses(classes);
        Assert.Equal(0, manager.GetCount());
    }

    [Fact]
    public void GroupManagerRegistersOnceAndUnregistersDescriptors()
    {
        var hunt = AIGroupClass.Of<HuntAIGroup>();
        Assert.Same(hunt, AIGroupClass.Of(typeof(HuntAIGroup)));
        Assert.Same(EngineClass.Of<HuntAIGroup>(), hunt.Class);
        Assert.Equal(GroupBehavior.Hunt, hunt.Behavior);
        Assert.Equal(GroupBehavior.None, AIGroup.Behavior);
        Assert.Equal(GroupBehavior.Defensive, AIGroupClass.Of<DefenderAIGroup>().Behavior);
        using var manager = new AIGroupManager(null!) { Holder = EngineFixture.Engine };
        AIGroupClass[] classes = [hunt, AIGroupClass.Of<DefenderAIGroup>()];
        manager.RegisterAIGroupClasses(classes);
        manager.RegisterAIGroupClasses(classes);
        Assert.Equal(2, manager.RegisteredClasses.Count);
        Assert.Same(classes[1], manager.RegisteredClasses[1]);
        manager.UnregisterAIGroupClasses(classes);
        Assert.Empty(manager.RegisteredClasses);
    }

    [Fact]
    public void EditorListFindsTheRegisteredDescriptorAndTheWalkFallsBackToTheBase()
    {
        using GameEngine engine = new GameEngine(null!);
        var mapObjectEditor = EditorClass.Of<MapObjectEditor>();
        Assert.Same(mapObjectEditor, EditorClass.Of(typeof(MapObjectEditor)));
        Assert.Same(EngineClass.Of<MapObjectEditor>(), mapObjectEditor.Class);
        Assert.Equal(MapObject.ClassId, mapObjectEditor.EditorClassId);
        Assert.Equal(0, EditorClass.Of<EngineObjectEditor>().EditorClassId);
        engine.Editors.RegisterClass(mapObjectEditor);
        var error = Assert.Throws<InvalidOperationException>(() => engine.Editors.RegisterClass(mapObjectEditor));
        Assert.Equal("EObjectEditor Double Defined!", error.Message);
        Assert.Equal(1, engine.Editors.Count);
        Assert.Same(mapObjectEditor, engine.Editors.FindEditor(MapObject.ClassId));
        Assert.Null(engine.Editors.FindEditor(EngineObject.ClassId));

        var mapObject = new MapObject(new EngineField(engine));
        var node = EngineClass.Of<ENode>().CreateInstance(engine.ControlNode, holder: EngineFixture.Engine);
        try
        {
            Assert.Same(mapObjectEditor, mapObject.Editor());
            Assert.Same(EditorClass.Of<EngineObjectEditor>(), node.Editor());
            EditResult? result = null;
            EngineObject? edited = null;
            node.Editor().Edit(node, engine, (r, o) => { result = r; edited = o; });
            Assert.Equal(EditResult.Cancel, result);
            Assert.Same(node, edited);
        }
        finally
        {
            node.Dispose();
            mapObject.Dispose();
        }
    }

    [Fact]
    public void IsometricHexBuildsItsDynamicSpriteInOneConstructor()
    {
        using var map = new GameMap(null!) { Holder = EngineFixture.Engine };
        using var container = new MapContainer(map);
        using var level = new MapLevel(container);
        level.CreateMapFieldList(3, 3);
        var field = level.GetField(1, 1);
        using var resource = new LowerIsometricHex.Resource(null!) { Holder = EngineFixture.Engine };
        var hex = new LowerIsometricHex(field) { _resource = resource };
        try
        {
            hex.UpdateTransitions();
            // no neighbour of its kind: the sides in range are open and the dynamic sprite exists
            Assert.NotEqual(0, hex.OpenSides);
            var dynamic = hex.Dynamic;
            Assert.NotNull(dynamic);
            Assert.Null(dynamic.Owner);
            Assert.Contains(dynamic, field.HexSpriteList.Take(field._count));
            Assert.Equal(field.HexX, dynamic.GetHexX());
            Assert.Equal(field.HexY, dynamic.GetHexY());
            Assert.Equal(resource.DynamicLevel, dynamic.GetLevel());
            Assert.Equal(26, dynamic.GetLevel());
            hex.Dispose();
            hex = null!;
            Assert.DoesNotContain(dynamic, field.HexSpriteList.Take(field._count));
        }
        finally
        {
            hex?.Dispose();
        }
    }

    [Fact]
    public void WaterHexReadsTheShoreOfItsOpenSides()
    {
        using var map = new GameMap(null!) { Holder = EngineFixture.Engine };
        using var container = new MapContainer(map);
        using var level = new MapLevel(container);
        level.CreateMapFieldList(3, 3);
        for (int y = 0; y < 3; y++)
            for (int x = 0; x < 3; x++)
                level.GetField(x, y).TerrainType = 3;
        var field = level.GetField(1, 1);
        using var resource = new WaterHex.Resource(null!) { Holder = EngineFixture.Engine };
        using var water = new WaterHex(field) { _resource = resource };
        water.UpdateTransitions();
        Assert.NotNull(water.Dynamic);
        Assert.NotEqual(0, water.OpenSides);
        for (int hexNumber = 1; hexNumber <= 6; hexNumber++)
            if ((water.OpenSides & (1 << (hexNumber - 1))) != 0)
                Assert.Equal(40, water.ShoreTerrain[hexNumber - 1]);
    }
}
