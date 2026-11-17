using System.Reflection;
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

// Brief 9.6-4: the message records over MsgArg, the dispatcher's answered test and one case per
// slot family that the 9.6 census saw carrying both an int and a reference.
[Collection("Engine")]
[EngineState]
public sealed class MessageSlotTests
{
    private delegate void Proc(ref MapFieldMessage message);

    // A sprite that runs one message handler and counts its calls.
    private sealed class Handler(Proc proc) : HexSprite(null!)
    {
        internal int Calls { get; private set; }

        public override void MapFieldMessageProc(ref MapFieldMessage message)
        {
            Calls++;
            proc(ref message);
        }
    }

    private static void None(ref MapFieldMessage message) { }

    // The dispatcher calls the sprites from the last index down, and AddHS puts a sprite of equal
    // priority in front, so the handlers run in the order given.
    private static (MapField Field, Handler[] Handlers) Field(params Proc[] procs)
    {
        MapField field = new(null) { Holder = EngineFixture.Engine };
        Handler[] handlers = procs.Select(p => new Handler(p)).ToArray();
        foreach (Handler? handler in handlers)
        {
            field.AddHexSprite(handler, 0);
        }

        return (field, handlers);
    }

    private static int[] Calls(Handler[] handlers) => handlers.Select(h => h.Calls).ToArray();

    [Fact]
    public void TheMessageRecordsKeepTheProtocolArity()
    {
        Assert.Equal(3, EngineMessageParameters.Arity);
        Assert.Equal(4, MapFieldMessageParameters.Arity);
        EngineMessage engineMessage = default;
        MapFieldMessage fieldMessage = default;
        EngineMessageParameters engineParam = engineMessage.Param;
        MapFieldMessageParameters fieldParam = fieldMessage.Param;
        Assert.Equal(EngineMessageParameters.Arity, ((Span<MsgArg>)engineParam).Length);
        Assert.Equal(MapFieldMessageParameters.Arity, ((Span<MsgArg>)fieldParam).Length);
        Assert.Equal(typeof(MsgArg), typeof(EngineMessage).GetProperty(nameof(EngineMessage.Payload))!.PropertyType);
        Assert.Equal(typeof(MsgArg), typeof(MapFieldMessage).GetProperty(nameof(MapFieldMessage.Result))!.PropertyType);
    }

    [Fact]
    public void TheDispatcherStopsAtTheFirstAnswerOfEitherCase()
    {
        object control = new();
        // No answer: every handler runs.
        (MapField? field, Handler[]? handlers) = Field(None, None, None);
        MapFieldMessage message = default;
        field.SendMapFieldMessage(ref message);
        Assert.Equal([1, 1, 1], Calls(handlers));
        Assert.Same(field, message.MapField);
        // An int answer (0x1100, the field's player) and a reference answer (0x1201, the
        // production control) stop the delivery at the same handler.
        (field, handlers) = Field(None, (ref m) => m.Result = 3, None);
        message = default;
        message.MessageId = 0x1100;
        message.Result = -1;
        field.SendMapFieldMessage(ref message);
        Assert.Equal([1, 1, 0], Calls(handlers));
        Assert.Equal(3, message.Result.Int);
        (field, handlers) = Field(None, (ref m) => m.Result = control, None);
        message = default;
        message.MessageId = 0x1201;
        message.Result = 0;
        field.SendMapFieldMessage(ref message);
        Assert.Equal([1, 1, 0], Calls(handlers));
        Assert.Same(control, message.Result.As<object>());
        // A handler writing 0 or nil into a slot that holds 0 has not answered, as in the original.
        (field, handlers) = Field((ref m) => m.Result = 0, (ref m) => m.Result = (object?)null, None);
        message = default;
        field.SendMapFieldMessage(ref message);
        Assert.Equal([1, 1, 1], Calls(handlers));
        // A reference the caller set before the delivery (0x1109, the combat result query) stays,
        // so every handler runs and fills the object.
        (field, handlers) = Field(None, None, None);
        message = default;
        message.MessageId = 0x1109;
        message.Result = control;
        field.SendMapFieldMessage(ref message);
        Assert.Equal([1, 1, 1], Calls(handlers));
        // Replacing one reference with another is an answer.
        (field, handlers) = Field((ref m) => m.Result = new object(), None);
        message = default;
        message.Result = control;
        field.SendMapFieldMessage(ref message);
        Assert.Equal([1, 0], Calls(handlers));
    }

    // TEMsg.Payload: 0x20000008 carries the engine, 0x20020002 the player of the new turn.
    [Fact]
    public void ThePayloadCarriesTheEngineOrThePlayer()
    {
        using GameEngine engine = new(null);
        EngineMessage.InitializeMessage(out EngineMessage message, 0x20000008, engine);
        message.Payload = engine;
        Assert.Same(engine, message.Payload.As<GameEngine>());
        Assert.Throws<InvalidCastException>(() => message.Payload.Int);
        Assert.Equal(-1, message.Result);
        EngineMessage.InitializeMessage(out message, 0x20020002, engine);
        Assert.False(message.Payload.HasValue);
        message.Payload = 5;
        Assert.Equal(5, (sbyte)message.Payload.Int);
        Assert.Null(message.Payload.As<GameEngine>());
    }

    // TMapFieldMsg.Param[0]: 0x1102 carries the placed army, 0x110a the distance to the viewer.
    [Fact]
    public void TheFirstParameterCarriesAnArmyOrADistance()
    {
        ArmyHexSprite army = new(null) { Holder = EngineFixture.Engine };
        ArmyHexSprite? placed = null;
        int distance = -1;
        (MapField? field, Handler[] _) = Field((ref m) =>
        {
            if (m.MessageId == 0x1102)
            {
                placed = m.Param[0].As<ArmyHexSprite>();
            }
            else if (m.MessageId == 0x110a)
            {
                distance = m.Param[0].Int;
            }
        });
        MapFieldMessage message = default;
        message.MessageId = 0x1102;
        MapFieldMessageParameters param = message.Param;
        param[0] = army;
        message.Param = param;
        field.SendMapFieldMessage(ref message);
        Assert.Same(army, placed);
        Assert.Throws<InvalidCastException>(() => message.Param[0].Int);
        message = default;
        message.MessageId = 0x110a;
        MapFieldMessageParameters param2 = message.Param;
        param2[0] = 4;
        message.Param = param2;
        field.SendMapFieldMessage(ref message);
        Assert.Equal(4, distance);
        Assert.Null(message.Param[0].As<ArmyHexSprite>());
    }

    // TMapFieldMsg.Param[1]: 0x1127 carries the defense requirements query, 0x1101 the move action.
    [Fact]
    public void TheSecondParameterCarriesAQueryOrAMoveAction()
    {
        DefenseRequirementsQuery query = new();
        (MapField? field, Handler[] _) = Field((ref m) =>
        {
            if (m.MessageId == 0x1127)
            {
                DefenseRequirements requirements = m.Param[1].As<DefenseRequirementsQuery>()!.Requirements;
                requirements.Strength = 9;
                m.Param[1].As<DefenseRequirementsQuery>()!.Requirements = requirements;
            }
            else if (m.MessageId == 0x1101)
            {
                MapFieldMessageParameters param = m.Param;
                param[1] = param[1].Int + 2;
                m.Param = param;
            }
        });
        MapFieldMessage message = default;
        message.MessageId = 0x1127;
        MapFieldMessageParameters param2 = message.Param;
        param2[0] = 1;
        message.Param = param2;
        MapFieldMessageParameters param3 = message.Param;
        param3[1] = query;
        message.Param = param3;
        field.SendMapFieldMessage(ref message);
        Assert.Equal(9, query.Requirements.Strength);
        Assert.Throws<InvalidCastException>(() => message.Param[1].Int);
        message = default;
        message.MessageId = 0x1101;
        MapFieldMessageParameters param4 = message.Param;
        param4[1] = 1;
        message.Param = param4;
        field.SendMapFieldMessage(ref message);
        Assert.Equal(3, message.Param[1].Int);
    }

    // TMapFieldMsg.Param[2]: 0x1151 carries the storm damage query, 0x1150 the storm's player.
    [Fact]
    public void TheThirdParameterCarriesAQueryOrAPlayer()
    {
        StormDamageQuery query = new();
        sbyte player = 0;
        (MapField? field, Handler[] _) = Field((ref m) =>
        {
            if (m.MessageId == 0x1151)
            {
                StormDamageInfo info = m.Param[2].As<StormDamageQuery>()!.Info;
                info.Player = 7;
                m.Param[2].As<StormDamageQuery>()!.Info = info;
            }
            else if (m.MessageId == 0x1150)
            {
                player = (sbyte)m.Param[2].Int;
            }
        });
        MapFieldMessage message = default;
        message.MessageId = 0x1151;
        MapFieldMessageParameters param = message.Param;
        param[2] = query;
        message.Param = param;
        field.SendMapFieldMessage(ref message);
        Assert.Equal(7, query.Info.Player);
        Assert.Throws<InvalidCastException>(() => message.Param[2].Int);
        message = default;
        message.MessageId = 0x1150;
        MapFieldMessageParameters param2 = message.Param;
        param2[2] = 3;
        message.Param = param2;
        field.SendMapFieldMessage(ref message);
        Assert.Equal(3, player);
        Assert.Null(message.Param[2].As<StormDamageQuery>());
    }

    // TMapFieldMsg.Result: 0x1201 answers the production control, 0x1100 the field's player.
    [Fact]
    public void TheResultCarriesAControlOrAPlayer()
    {
        ProductionControl control = new(null) { Holder = EngineFixture.Engine };
        (MapField? field, Handler[]? handlers) = Field((ref m) =>
        {
            if (m.MessageId == 0x1201)
            {
                m.Result = control;
            }
            else if (m.MessageId == 0x1100)
            {
                m.Result = 2;
            }
        }, None);
        MapFieldMessage message = default;
        message.MessageId = 0x1201;
        message.Result = 0;
        field.SendMapFieldMessage(ref message);
        Assert.Same(control, message.Result.As<ProductionControl>());
        Assert.Throws<InvalidCastException>(() => message.Result.Int);
        Assert.Equal([1, 0], Calls(handlers));
        message = default;
        message.MessageId = 0x1100;
        message.Result = -1;
        field.SendMapFieldMessage(ref message);
        Assert.Equal(2, (sbyte)message.Result.Int);
        Assert.Null(message.Result.As<ProductionControl>());
        Assert.Equal([2, 0], Calls(handlers));
    }

    // Spec section 4: the message records never reach a stream. No storage method of the product
    // takes a message record or one of its slots.
    [Fact]
    public void NoStorageMethodTakesAMessageRecord()
    {
        Type[] message = [typeof(EngineMessage), typeof(MapFieldMessage), typeof(EngineMessageParameters), typeof(MapFieldMessageParameters), typeof(MsgArg)];
        List<string> found = [];
        foreach (Type? type in typeof(EngineMessage).Assembly.GetTypes().Concat(typeof(MsgArg).Assembly.GetTypes()))
        {
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (!method.Name.StartsWith("rw", StringComparison.Ordinal) && !method.Name.StartsWith("Read", StringComparison.Ordinal)
                    && !method.Name.StartsWith("Write", StringComparison.Ordinal))
                {
                    continue;
                }

                foreach (ParameterInfo parameter in method.GetParameters())
                {
                    Type parameterType = parameter.ParameterType.IsByRef ? parameter.ParameterType.GetElementType()! : parameter.ParameterType;
                    if (message.Contains(parameterType))
                    {
                        found.Add($"{type.FullName}.{method.Name}({parameter.Name})");
                    }
                }
            }
        }

        Assert.Empty(found);
    }
}
