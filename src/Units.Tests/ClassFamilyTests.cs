using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Session;
using Aow.Game.Ui.Grids;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;

namespace Aow.Units.Tests;

// Briefs 9.4-3 and 9.4-4: the instance virtuals that left the metaclass, the isometric hexagon's
// one-constructor dynamic sprite, and the descriptor families (AI player actions, AI groups,
// editors) that a list reads without an instance.
[Collection("Engine")]
[EngineState]
public sealed class ClassFamilyTests
{
    private sealed class UnregisteredAction(ECustomNode owner) : AIPlayerAction(owner);

    private sealed class UnregisteredGroup(ECustomNode owner) : AIGroup(owner);

    private sealed class UnregisteredEditor(ECustomNode owner) : EObjectEditor(owner);

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
            var expected = typeof(EObject).Assembly.GetTypes().Where(t => t.IsClass && contract.IsAssignableFrom(t)).Select(t => t.FullName).Order(StringComparer.Ordinal);
            Assert.Equal(expected, registered.Where(t => t.Assembly == typeof(EObject).Assembly).Select(t => t.FullName).Order(StringComparer.Ordinal));
        }
        Verify(typeof(IAIPlayerActionClass), AIPlayerActionClass.All.Select(c => c.Class.Type));
        Verify(typeof(IAIGroupClass), AIGroupClass.All.Select(c => c.Class.Type));
        Verify(typeof(IEObjectEditorClass), EditorClass.All.Select(c => c.Class.Type));
        Assert.Throws<AbstractErrorException>(() => AIPlayerActionClass.Of<AIPlayerAction>().Priority);
        Assert.Throws<AbstractErrorException>(() => AIPlayerActionClass.Of<StructureAIPA>().Priority);
        Assert.Equal("EndTurnAIPA", AIPlayerActionClass.Of<EndTurnAIPA>().ClassName);
        Assert.Equal("HuntAG", AIGroupClass.Of<HuntAG>().ClassName);
        Assert.Equal("EObjectEditor", EditorClass.Of<EObjectEditor>().ClassName);
        Assert.Throws<InvalidOperationException>(() => AIPlayerActionClass.Register<EndTurnAIPA>());
        Assert.Throws<InvalidOperationException>(() => AIGroupClass.Register<HuntAG>());
        Assert.Throws<InvalidOperationException>(() => EditorClass.Register<EObjectEditor>());
    }

    [Fact]
    public void ActionDescriptorsAreOnePerClassAndReadTheStatics()
    {
        var endTurn = AIPlayerActionClass.Of<EndTurnAIPA>();
        Assert.Same(endTurn, AIPlayerActionClass.Of(typeof(EndTurnAIPA)));
        Assert.Same(EClass.Of<EndTurnAIPA>(), endTurn.Class);
        Assert.Equal(EndTurnAIPA.Priority, endTurn.Priority);
        Assert.NotEqual(GroupControlLoopAIPA.Priority, endTurn.Priority);
        // the phases are inherited from TAIPlayerAction, the loop action declares its own
        Assert.True(endTurn.SupportedPhases.Contains(AIPlayerProcessPhase.Main));
        Assert.False(endTurn.SupportedPhases.Contains(AIPlayerProcessPhase.Loop));
        var loop = AIPlayerActionClass.Of<GroupControlLoopAIPA>();
        Assert.True(loop.SupportedPhases.Contains(AIPlayerProcessPhase.Loop));
        Assert.False(loop.SupportedPhases.Contains(AIPlayerProcessPhase.Main));
        // the base statics are abstract and the default
        Assert.Throws<AbstractErrorException>(() => AIPlayerAction.Priority);
        Assert.True(AIPlayerAction.SupportedPhases.Contains(AIPlayerProcessPhase.Main));
        using var instance = new EndTurnAIPA(null!);
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
            const System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly;
            for (var t = type; t != null; t = t.BaseType)
                if (t.GetProperty(name, flags) is { } property) return property.GetValue(null);
            throw new InvalidOperationException(name);
        }
        var types = typeof(AIPlayerAction).Assembly.GetTypes();
        var actions = types.Where(t => typeof(AIPlayerAction).IsAssignableFrom(t) && t != typeof(AIPlayerAction) && t != typeof(StructureAIPA)).ToList();
        Assert.Equal(13, actions.Count);
        foreach (var type in actions)
        {
            var descriptor = AIPlayerActionClass.Of(type);
            Assert.Same(EClass.Of(type), descriptor.Class);
            Assert.Equal(Nearest(type, nameof(AIPlayerAction.Priority)), descriptor.Priority);
            Assert.Equal(Nearest(type, nameof(AIPlayerAction.SupportedPhases)), descriptor.SupportedPhases);
        }
        var groups = types.Where(t => typeof(AIGroup).IsAssignableFrom(t)).ToList();
        Assert.Equal(19, groups.Count);
        foreach (var type in groups)
        {
            var descriptor = AIGroupClass.Of(type);
            Assert.Same(EClass.Of(type), descriptor.Class);
            Assert.Equal(Nearest(type, nameof(AIGroup.Behavior)), descriptor.Behavior);
        }
    }

    [Fact]
    public void ActionManagerRegistersSortsAndUnregistersDescriptors()
    {
        using var manager = new AIPlayerActionManager(null!);
        AIPlayerActionClass[] classes = [AIPlayerActionClass.Of<EndTurnAIPA>(), AIPlayerActionClass.Of<InitializeAIPA>(), AIPlayerActionClass.Of<GroupControlLoopAIPA>()];
        manager.RegisterClasses(classes);
        Assert.Equal(3, manager.GetCount());
        var registered = Enumerable.Range(0, 3).Select(manager.GetRegisteredClasses).ToList();
        foreach (var c in classes) Assert.Contains(c, registered);
        // CompareAIPlayerActionClasses orders by descending priority
        for (int i = 1; i < registered.Count; i++) Assert.True(registered[i - 1].Priority >= registered[i].Priority);
        var error = Assert.Throws<InvalidOperationException>(() => manager.RegisterClasses([AIPlayerActionClass.Of<EndTurnAIPA>()]));
        Assert.Equal("AIPlayerAction already Registered", error.Message);
        manager.UnRegisterClasses(classes);
        Assert.Equal(0, manager.GetCount());
    }

    [Fact]
    public void GroupManagerRegistersOnceAndUnregistersDescriptors()
    {
        var hunt = AIGroupClass.Of<HuntAG>();
        Assert.Same(hunt, AIGroupClass.Of(typeof(HuntAG)));
        Assert.Same(EClass.Of<HuntAG>(), hunt.Class);
        Assert.Equal(GroupBehavior.Hunt, hunt.Behavior);
        Assert.Equal(GroupBehavior.None, AIGroup.Behavior);
        Assert.Equal(GroupBehavior.Defensive, AIGroupClass.Of<DefenderAG>().Behavior);
        using var manager = new AIGroupManager(null!);
        AIGroupClass[] classes = [hunt, AIGroupClass.Of<DefenderAG>()];
        manager.RegisterAIGroupClasses(classes);
        manager.RegisterAIGroupClasses(classes);
        Assert.Equal(2, manager.FRegisteredClasses.Count);
        Assert.Same(classes[1], manager.FRegisteredClasses[1]);
        manager.UnregisterAIGroupClasses(classes);
        Assert.Equal(0, manager.FRegisteredClasses.Count);
    }

    [Fact]
    public void EditorListFindsTheRegisteredDescriptorAndTheWalkFallsBackToTheBase()
    {
        if (EngineHost.Engines is null) EngineHost.Initialize();
        using var engine = new Engine(null!);
        var mapObjectEditor = EditorClass.Of<AoWMapObjectEditor>();
        Assert.Same(mapObjectEditor, EditorClass.Of(typeof(AoWMapObjectEditor)));
        Assert.Same(EClass.Of<AoWMapObjectEditor>(), mapObjectEditor.Class);
        Assert.Equal(MapObject.ClassID, mapObjectEditor.EditorClassID);
        Assert.Equal(0, EditorClass.Of<EObjectEditor>().EditorClassID);
        engine.Editors.RegisterClass(mapObjectEditor);
        var error = Assert.Throws<InvalidOperationException>(() => engine.Editors.RegisterClass(mapObjectEditor));
        Assert.Equal("EObjectEditor Double Defined!", error.Message);
        Assert.Equal(1, engine.Editors.Count);
        Assert.Same(mapObjectEditor, engine.Editors.FindEditor(MapObject.ClassID));
        Assert.Null(engine.Editors.FindEditor(EObject.ClassID));

        var mapObject = new MapObject(engine.ControlNode);
        var node = EClass.Of<ENode>().Create(engine.ControlNode);
        try
        {
            Assert.Same(mapObjectEditor, mapObject.Editor());
            Assert.Same(EditorClass.Of<EObjectEditor>(), node.Editor());
            EditResult? result = null;
            EObject? edited = null;
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
    public void ItemGridConstructionStopsAtTheControlBoundary()
    {
        Assert.Throws<NotImplementedException>(() => new ItemGrid(null!));
    }

    [Fact]
    public void IsometricHexagonBuildsItsDynamicSpriteInOneConstructor()
    {
        using var map = new HSMap(null!);
        using var level = new MapLevel(map);
        level.CreateMapFieldList(3, 3);
        var field = level.GetField(1, 1);
        using var resource = new LowerIsometricHexagon.Resource(null!);
        var hexagon = new LowerIsometricHexagon(field) { FResource = resource };
        try
        {
            hexagon.UpdateTransitions();
            // no neighbour of its kind: the sides in range are open and the dynamic sprite exists
            Assert.NotEqual(0, hexagon.FOpenSides);
            var dynamic = hexagon.FDynamic;
            Assert.NotNull(dynamic);
            Assert.Null(dynamic.FOwner);
            Assert.Contains(dynamic, field.HSList.Take(field.FCount));
            Assert.Equal(field.Xhx, dynamic.GetXhx());
            Assert.Equal(field.Yhx, dynamic.GetYhx());
            Assert.Equal(resource.FDynamicLevel, dynamic.GetLevel());
            Assert.Equal(26, dynamic.GetLevel());
            hexagon.Dispose();
            hexagon = null!;
            Assert.DoesNotContain(dynamic, field.HSList.Take(field.FCount));
        }
        finally
        {
            hexagon?.Dispose();
        }
    }

    [Fact]
    public void WaterHexagonReadsTheShoreOfItsOpenSides()
    {
        using var map = new HSMap(null!);
        using var level = new MapLevel(map);
        level.CreateMapFieldList(3, 3);
        for (int y = 0; y < 3; y++)
            for (int x = 0; x < 3; x++)
                level.GetField(x, y).TerrainType = 3;
        var field = level.GetField(1, 1);
        using var resource = new AoWWaterHexagon.Resource(null!);
        using var water = new AoWWaterHexagon(field) { FResource = resource };
        water.UpdateTransitions();
        Assert.NotNull(water.FDynamic);
        Assert.NotEqual(0, water.FOpenSides);
        for (int hn = 1; hn <= 6; hn++)
            if ((water.FOpenSides & (1 << (hn - 1))) != 0)
                Assert.Equal(40, water.FShoreTerrain[hn - 1]);
    }
}
