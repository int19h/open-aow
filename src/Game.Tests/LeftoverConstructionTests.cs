#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using System.Collections;
using System.Reflection;
using System.Runtime.ExceptionServices;

using System.Text.Json;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Spells;
using Aow.Game.World;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class LeftoverConstructionTests
{
    private sealed class LeftoverOwner : EngineFixture.Owner
    {

        internal List<string> Observations { get; } = [];

        public override int AddChild(EngineObject child)
        {
            _ = base.AddChild(child);
            Observations.Add(Snapshot(child));
            return Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child) => base.RemoveChild(child) < 0 ? -1 : 0;
    }

    private sealed class Fixture : IDisposable
    {
        private readonly int _previousSeed = DelphiRandom.RandSeed;
        private readonly GamePack _previousSet = GamePack.Current;
        private readonly StrategicMap _previousMap = StrategicMap.Current;
        private readonly ImageLibraryManager _manager = new(null);
        private readonly MapObjectResourceList _combatResources = new(null!) { Holder = EngineFixture.Engine };

        internal Fixture()
        {
            DelphiRandom.RandSeed = 123456789;
            GamePack set = EngineFixture.Configure(EngineFixture.CreateGameSet(_manager), value => { value.CombatResources = _combatResources; });
            GamePack.Current = set;
            StrategicMap.Current = null;
            GamePack.Current.UnitGraphicsResources = null;
            DelphiRandom.RandSeed = 123456789;
        }

        public void Dispose()
        {
            DelphiRandom.RandSeed = _previousSeed;

            GamePack.Current = _previousSet;
            StrategicMap.Current = _previousMap;
            _combatResources.Dispose();
            _manager.Dispose();
        }
    }

    private static string Snapshot(object value)
    {
        Dictionary<object, string> visited = [with(ReferenceEqualityComparer.Instance)];
        object? State(object? item, string path)
        {
            if (item is null)
            {
                return null;
            }

            if (item is GameEngine)
            {
                return "$engine";
            }

            Type type = item.GetType();
            // A game pack searches the fixture's temporary game root for its images and sounds
            // (issue 200 folded the game part into the set), so the root reads as $root.
            if (item is string text)
            {
                return text.StartsWith(EngineFixture.Root, comparisonType: StringComparison.OrdinalIgnoreCase) ? $"$root{text[EngineFixture.Root.Length..]}" : text;
            }

            if (type.IsEnum)
            {
                return Convert.ToInt64(item);
            }

            if (type.IsPrimitive || item is string || item is decimal)
            {
                return item;
            }

            if (item is Type represented)
            {
                return represented.FullName;
            }

            if (item is Delegate handler)
            {
                return HandlerNames.Of(handler);
            }

            if (!type.IsValueType)
            {
                if (visited.TryGetValue(item, out string? found))
                {
                    return $"$ref:{found}";
                }

                visited.Add(item, path);
            }
            if (item is Array array)
            {
                return array.Cast<object?>().Select((x, i) => State(x, path: $"{path}[{i}]")).ToArray();
            }

            if (InlineArrays.Elements(item) is { } elements)
            {
                return elements.Select((x, i) => State(x, path: $"{path}[{i}]")).ToArray();
            }

            if (item is IList list)
            {
                return list.Cast<object?>().Select((x, i) => State(x, path: $"{path}[{i}]")).ToArray();
            }

            SortedDictionary<string, object?> fields = new(StringComparer.Ordinal) { ["$type"] = type.FullName };
            for (Type? current = type; current is not null && current != typeof(object) && current != typeof(object); current = current.BaseType)
            {
                foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(MemberNames.Of, comparer: StringComparer.Ordinal))
                {
                    if (MemberNames.Of(field) is "_engineClass" or "EngineClass" or "Holder" or "Creator")
                    {
                        continue; // the descriptor cache and the link to the engine are not constructor state
                    }
                    // CF-2 stores priority as a delegate. Keep its value in the base recording's key.
                    if (item is AIPlayerActionClass actionClass && field.Name == "_priority")
                    {
                        fields[$"{current.Name}.<Priority>k__BackingField"] = actionClass.Priority;
                        continue;
                    }
                    string key = $"{current.Name}.{MemberNames.Of(field)}";
                    object? child = field.GetValue(item);
                    if (MemberNames.Of(field) is "_owner" or "Owner")
                    {
                        fields[key] = child is null ? null : "$owner";
                        continue;
                    }
                    fields[key] = State(child, path: $"{path}.{key}");
                }
            }

            return fields;
        }
        return JsonSerializer.Serialize(State(value, path: "$"));
    }

    private static readonly string[] s_names =
    [
        "Aow.Game.AI.Groups.AIGroup",
        "Aow.Game.AI.Groups.AIGroupControl",
        "Aow.Game.AI.Groups.AIGroupTask",
        "Aow.Game.Magic.Abilities.Ability",
        "Aow.Game.Magic.Abilities.AbilityOwner",
        "Aow.Game.Session.Campaign",
        "Aow.Game.Session.CampaignScenario",
        "Aow.Game.World.Map.StrategicMap",
        "Aow.Game.Units.Army",
        "Aow.Game.Commands.ArmyCombatMoveTokenEvent",
        "Aow.Game.Commands.ArmyCombineMoveTokenEvent",
        "Aow.Game.Commands.ArmyDefaultMoveTokenEvent",
        "Aow.Game.Commands.ArmyNegotiateMoveTokenEvent",
        "Aow.Game.Commands.ChangeGlobalMagicTokenEvent",
        "Aow.Game.Commands.ChangePlayerMagicTokenEvent",
        "Aow.Game.Commands.ChangePlayerSettingsTokenEvent",
        "Aow.Game.Commands.CheatTokenEvent",
        "Aow.Game.Combat.Battle",
        "Aow.Game.Combat.Actions.CombatAction",
        "Aow.Game.Commands.CombatFlowControlTokenEvent",
        "Aow.Game.Combat.CombatObject",
        "Aow.Game.Combat.CombatUnit",
        "Aow.Game.Commands.ConstructStructureTokenEvent",
        "Aow.Game.Commands.DiplomaticActionTokenEvent",
        "Aow.Game.Commands.DisbandUnitTokenEvent",
        "Aow.Game.Commands.DispelMagicAbilityTokenEvent",
        "Aow.Game.Combat.Actions.DispelMagicCombatAction",
        "Aow.Game.Magic.Abilities.DurationAbility",
        "Aow.Game.Commands.EndTurnTokenEvent",
        "Aow.Game.Magic.Abilities.EnhancementAbility",
        "Aow.Game.World.Terrain.Fire",
        "Aow.Game.World.Terrain.Fire+Resource",
        "Aow.Game.Draw.FrozenWaterHexSprite",
        "Aow.Game.Commands.GameOverTokenEvent",
        "Aow.Game.Magic.GlobalEnchantment",
        "Aow.Game.Commands.HealingTokenEvent",
        "Aow.Game.Commands.HeroUpdateTokenEvent",
        "Aow.Game.Commands.HeroUpgradeTokenEvent",
        "Aow.Game.Units.ItemList",
        "Aow.Game.Commands.ItemTokenEvent",
        "Aow.Game.Units.LeaderSetupControl",
        "Aow.Game.Commands.MoveArmyTokenEvent",
        "Aow.Game.World.Movement.MoveControlSettings",
        "Aow.Game.Commands.MoveTokenEvent",
        "Aow.Game.Magic.Abilities.MultiLevelAbility",
        "Aow.Game.Commands.MultiMoveTokenEvent",
        "Aow.Game.Commands.NegotiateTokenEvent",
        "Aow.Game.Session.PlayerMagicControl",
        "Aow.Game.World.Structures.ProductionControl",
        "Aow.Game.Commands.ProductionTokenEvent",
        "Aow.Game.Commands.SaveGameTokenEvent",
        "Aow.Game.Magic.Spells.Spell",
        "Aow.Game.Magic.Spells.SpellCaster",
        "Aow.Game.Audio.StartupSongList",
        "Aow.Game.Draw.StructureShadow",
        "Aow.Game.Commands.TerminatePlayerTokenEvent",
        "Aow.Game.Magic.Abilities.UnitEnchantmentAbility",
        "Aow.Game.Draw.UnitShadowHexSprite",
        "Aow.Game.Magic.Spells.UnitSpell",
        "Aow.Game.Commands.UnitSpellTokenEvent",
        "Aow.Game.Draw.WorldEffectHexSprite",
        "Aow.Game.Commands.CityEnchantmentSpellTokenEvent",
        "Aow.Game.Commands.CityTargetSpellTokenEvent",
        "Aow.Game.Commands.TownGateTokenEvent",
        "Aow.Engine.Nodes.ObservableEventList",
        "Aow.Engine.Nodes.ObservableNode",
        "Aow.Engine.Nodes.ControlStyleInverseSortedNode",
        "Aow.Engine.Nodes.ControlStyleNode",
        "Aow.Engine.Nodes.EngineNode",
        "Aow.Engine.Nodes.InverseSortedNode",
        "Aow.Engine.Nodes.OwningNode",
        "Aow.Engine.Nodes.EngineObjectEditor",
        "Aow.Engine.Nodes.ReferenceNode",
        "Aow.Engine.Nodes.EngineResource",
        "Aow.Engine.Nodes.ResourceCollection",
        "Aow.Engine.Nodes.SortedNode",
        "Aow.Game.Commands.GlobalEnchantmentSpellTokenEvent",
        "Aow.Game.Draw.BirdsViewHexSprite",
        "Aow.Game.Draw.ConcealAreaHexSprite",
        "Aow.Game.Commands.DisjunctionTokenEvent",
        "Aow.Game.Commands.WarpPartyTokenEvent",
        "Aow.Game.Commands.FireBarrierTokenEvent",
        "Aow.Game.Commands.FreezeWaterTokenEvent",
        "Aow.Game.Magic.Spells.GlobalTargetSpell",
        "Aow.Game.Commands.HealingShowersTokenEvent",
        "Aow.Game.Commands.LevelTerrainTokenEvent",
        "Aow.Game.Commands.RejuvenateTokenEvent",
        "Aow.Game.Commands.TornadoTokenEvent",
        "Aow.Game.Commands.VortexTokenEvent",
        "Aow.Game.Draw.WatcherHexSprite",
        "Aow.Game.Commands.WatcherTokenEvent",
        "Aow.Game.Draw.FixedVisibleHexSprite",
        "Aow.Game.World.Map.GamePack",
        "Aow.Game.World.Map.MapField",
        "Aow.Game.World.Map.MapLevel",
        "Aow.Game.World.MapObjectResource",
        "Aow.Game.World.MultiHexMapObjectResource",
        "Aow.Game.World.Terrain.ImageLibraryTerrainMapObject",
        "Aow.Game.Commands.TokenEvent",
        "Aow.Game.Audio.SongList",
        "Aow.Game.Commands.SummonSpellTokenEvent",
    ];

    public static IEnumerable<object[]> Classes => s_names.Select(name => new object[] { name });

    private static object? Invoke(Func<object?> action)
    {
        try
        {
            return action();
        }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }

    private static EngineObject Direct(Type type, CustomNode owner)
    {
        ConstructorInfo? constructor = EngineClassCreation.OwnerConstructor(type);
        if (constructor is not null)
        {
            return (EngineObject)Invoke(() => constructor.Invoke(EngineClassCreation.OwnerArguments(constructor, owner)))!;
        }

        MethodInfo? factory = type.GetMethod("Create", bindingAttr: BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, types: [typeof(CustomNode)]);
        return factory is not null
            ? (EngineObject)Invoke(() => factory.Invoke(null, parameters: [owner]))!
            : EngineClass.Of(type).CreateInstance(owner, holder: EngineFixture.Engine);
    }

    [Theory]
    [MemberData(nameof(Classes))]
    public void StateOwnerObservationAndTeardownMatchTheOriginal(string name)
    {
        Type type = typeof(Spell).Assembly.GetType(name)!;

        for (int owned = 0; owned < 2; owned++)
        {
            for (int route = 0; route < 4; route++)
            {
                using Fixture fixture = new();
                LeftoverOwner? owner = owned == 0 ? null : new LeftoverOwner();
                CustomNode given = EngineFixture.OwnerFor(type, recorder: owner!);
                EngineObject? value = null;
                List<string> states = [];
                try
                {
                    value = route == 1 ? EngineClass.Of(type).CreateInstance(given, holder: EngineFixture.Engine)
                        : route == 0 ? Direct(type, owner: given)
                        : route == 2 ? EngineClass.Of(type).CreateInstance(given, holder: EngineFixture.Engine)
                        : Saved(type, owner: given);
                    Assert.IsType(type, @object: value);
                    Assert.Same(given, value.Owner);
                    states.Add($"success:{Snapshot(value)}");
                }
                catch (Exception error)
                {
                    states.Add($"failure:{error.GetType().FullName}:{error.Message}");
                }
                finally
                {
                    try
                    {
                        value?.Dispose();
                    }
                    catch (Exception error)
                    {
                        states.Add($"destroy:{error.GetType().FullName}:{error.Message}");
                    }
                }
                states.Add($"owner:{owner?.Members.Count ?? 0}");
                if (owner is not null)
                {
                    states.Add(JsonSerializer.Serialize(owner.Observations));
                }

                ConstructionSnapshot.Check(actual: string.Join("\n", values: states), key: $"{name}.owner{owned}.route{route}", name: "LeftoverConstruction");
            }
        }
    }

    private static EngineObject Saved(Type type, CustomNode owner)
    {
        using GameEngine engine = new(null) { Registry = EngineFixture.Engine.Registry, Dictionary = EngineFixture.Engine.Dictionary };
        EngineClass descriptor = EngineClass.Of(type);
        engine.Options &= ~GameEngine.VerifyClasses;
        try
        {
            engine.UnregisterEngineClasses([engine.GetEngineClass(descriptor.ClassId)]);
        }
        catch (Exception error) when (PortedExceptions.Is(error)) { }
        engine.RegisterEngineClasses([descriptor]);
        return engine.CreateEngineObject(descriptor.ClassId, owner);
    }

    private sealed class RejectingOwner : EngineFixture.Owner
    {
        public RejectingOwner() : base() { }

        private StrategicMap? _captured;

        public override int AddChild(EngineObject child)
        {
            _captured = Assert.IsType<StrategicMap>(child);
            Assert.Equal(0, _captured.MapChangedLockCount);
            throw new InvalidOperationException("reject after insertion");
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void PlainWorldMapPublishesItsCompletedState(int route)
    {
        using Fixture fixture = new();
        StrategicMap value = Assert.IsType<StrategicMap>(route switch
        {
            0 => new StrategicMap(null!, holder: EngineFixture.Engine),
            1 => EngineClass.Of<StrategicMap>().CreateInstance(null!, holder: EngineFixture.Engine),
            2 => EngineClass.Of(typeof(StrategicMap)).CreateInstance(null!, holder: EngineFixture.Engine),
            _ => Saved(typeof(StrategicMap), owner: null!)
        });
        Assert.Equal(0, value.MapChangedLockCount);
        Assert.NotNull(value.ItemControl);
        Assert.NotNull(value.Effects);
        value.Dispose();
    }

    [Fact]
    public void MapMouseKeepsItsInheritedInitialization()
    {
        using AowHexSpriteMapMouse mouse = new();
        Assert.Equal((-1, -1, -1, -1), (mouse.SelectionRectangle.Left, mouse.SelectionRectangle.Top,
            mouse.SelectionRectangle.Right, mouse.SelectionRectangle.Bottom));
        Assert.Equal(0, mouse.RectangleSelectEventsStore.Count);
    }
}
