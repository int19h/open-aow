using System.Reflection;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;

namespace Aow.Units.Tests;

// Brief 9.6-4: the message records over MsgArg, the dispatcher's answered test and one case per
// slot family that the 9.6 census saw carrying both an int and a reference.
[Collection("Engine")]
[EngineState]
public sealed class MessageSlotTests
{
    private delegate void Proc(ref TMapFieldMsg message);

    // A sprite that runs one message handler and counts its calls.
    private sealed class Handler(Proc proc) : THexagonSprite(null!)
    {
        internal int Calls;

        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            Calls++;
            proc(ref Msg);
        }
    }

    private static void None(ref TMapFieldMsg message) { }

    // The dispatcher calls the sprites from the last index down, and AddHS puts a sprite of equal
    // priority in front, so the handlers run in the order given.
    private static (TMapField Field, Handler[] Handlers) Field(params Proc[] procs)
    {
        var field = new TMapField(null!);
        var handlers = procs.Select(p => new Handler(p)).ToArray();
        foreach (var handler in handlers) field.AddHS(handler, 0);
        return (field, handlers);
    }

    private static int[] Calls(Handler[] handlers) => handlers.Select(h => h.Calls).ToArray();

    [Fact]
    public void TheMessageRecordsKeepTheProtocolArity()
    {
        Assert.Equal(3, TEMsgParams.Arity);
        Assert.Equal(4, TMapFieldMsgParams.Arity);
        TEMsg engineMessage = default;
        TMapFieldMsg fieldMessage = default;
        Assert.Equal(TEMsgParams.Arity, ((Span<MsgArg>)engineMessage.Param).Length);
        Assert.Equal(TMapFieldMsgParams.Arity, ((Span<MsgArg>)fieldMessage.Param).Length);
        Assert.Equal(typeof(MsgArg), typeof(TEMsg).GetField(nameof(TEMsg.Payload))!.FieldType);
        Assert.Equal(typeof(MsgArg), typeof(TMapFieldMsg).GetField(nameof(TMapFieldMsg.Result))!.FieldType);
    }

    [Fact]
    public void TheDispatcherStopsAtTheFirstAnswerOfEitherCase()
    {
        var control = new TObject();
        // No answer: every handler runs.
        var (field, handlers) = Field(None, None, None);
        TMapFieldMsg message = default;
        field.SendMapFieldMsg(ref message);
        Assert.Equal(new[] { 1, 1, 1 }, Calls(handlers));
        Assert.Same(field, message.MapField);
        // An int answer (0x1100, the field's player) and a reference answer (0x1201, the
        // production control) stop the delivery at the same handler.
        (field, handlers) = Field(None, (ref m) => m.Result = 3, None);
        message = default;
        message.MsgID = 0x1100;
        message.Result = -1;
        field.SendMapFieldMsg(ref message);
        Assert.Equal(new[] { 1, 1, 0 }, Calls(handlers));
        Assert.Equal(3, message.Result.Int);
        (field, handlers) = Field(None, (ref m) => m.Result = control, None);
        message = default;
        message.MsgID = 0x1201;
        message.Result = 0;
        field.SendMapFieldMsg(ref message);
        Assert.Equal(new[] { 1, 1, 0 }, Calls(handlers));
        Assert.Same(control, message.Result.As<TObject>());
        // A handler writing 0 or nil into a slot that holds 0 has not answered, as in the original.
        (field, handlers) = Field((ref m) => m.Result = 0, (ref m) => m.Result = (TObject?)null, None);
        message = default;
        field.SendMapFieldMsg(ref message);
        Assert.Equal(new[] { 1, 1, 1 }, Calls(handlers));
        // A reference the caller set before the delivery (0x1109, the combat result query) stays,
        // so every handler runs and fills the object.
        (field, handlers) = Field(None, None, None);
        message = default;
        message.MsgID = 0x1109;
        message.Result = control;
        field.SendMapFieldMsg(ref message);
        Assert.Equal(new[] { 1, 1, 1 }, Calls(handlers));
        // Replacing one reference with another is an answer.
        (field, handlers) = Field((ref m) => m.Result = new TObject(), None);
        message = default;
        message.Result = control;
        field.SendMapFieldMsg(ref message);
        Assert.Equal(new[] { 1, 0 }, Calls(handlers));
    }

    // TEMsg.Payload: 0x20000008 carries the engine, 0x20020002 the player of the new turn.
    [Fact]
    public void ThePayloadCarriesTheEngineOrThePlayer()
    {
        var engine = new TEngine(null!);
        try
        {
            TEMsg message = default;
            EngineUnit.InitMsg(ref message, 0x20000008, engine);
            message.Payload = engine;
            Assert.Same(engine, message.Payload.As<TEngine>());
            Assert.Throws<EInvalidCast>(() => message.Payload.Int);
            Assert.Equal(-1, message.Result);
            EngineUnit.InitMsg(ref message, 0x20020002, engine);
            Assert.False(message.Payload.HasValue);
            message.Payload = 5;
            Assert.Equal(5, (sbyte)message.Payload.Int);
            Assert.Null(message.Payload.As<TEngine>());
        }
        finally
        {
            engine.Destroy();
        }
    }

    // TMapFieldMsg.Param[0]: 0x1102 carries the placed army, 0x110a the distance to the viewer.
    [Fact]
    public void TheFirstParameterCarriesAnArmyOrADistance()
    {
        var army = new TArmyHS(null!);
        TArmyHS? placed = null;
        int distance = -1;
        var (field, _) = Field((ref m) =>
        {
            if (m.MsgID == 0x1102) placed = m.Param[0].As<TArmyHS>();
            else if (m.MsgID == 0x110a) distance = m.Param[0].Int;
        });
        TMapFieldMsg message = default;
        message.MsgID = 0x1102;
        message.Param[0] = army;
        field.SendMapFieldMsg(ref message);
        Assert.Same(army, placed);
        Assert.Throws<EInvalidCast>(() => message.Param[0].Int);
        message = default;
        message.MsgID = 0x110a;
        message.Param[0] = 4;
        field.SendMapFieldMsg(ref message);
        Assert.Equal(4, distance);
        Assert.Null(message.Param[0].As<TArmyHS>());
    }

    // TMapFieldMsg.Param[1]: 0x1127 carries the defense requirements query, 0x1101 the move action.
    [Fact]
    public void TheSecondParameterCarriesAQueryOrAMoveAction()
    {
        var query = new DefenseRequirementsQuery();
        var (field, _) = Field((ref m) =>
        {
            if (m.MsgID == 0x1127) m.Param[1].As<DefenseRequirementsQuery>()!.Requirements.Strength = 9;
            else if (m.MsgID == 0x1101) m.Param[1] = m.Param[1].Int + 2;
        });
        TMapFieldMsg message = default;
        message.MsgID = 0x1127;
        message.Param[0] = 1;
        message.Param[1] = query;
        field.SendMapFieldMsg(ref message);
        Assert.Equal(9, query.Requirements.Strength);
        Assert.Throws<EInvalidCast>(() => message.Param[1].Int);
        message = default;
        message.MsgID = 0x1101;
        message.Param[1] = 1;
        field.SendMapFieldMsg(ref message);
        Assert.Equal(3, message.Param[1].Int);
    }

    // TMapFieldMsg.Param[2]: 0x1151 carries the storm damage query, 0x1150 the storm's player.
    [Fact]
    public void TheThirdParameterCarriesAQueryOrAPlayer()
    {
        var query = new StormDamageQuery();
        sbyte player = 0;
        var (field, _) = Field((ref m) =>
        {
            if (m.MsgID == 0x1151) m.Param[2].As<StormDamageQuery>()!.Info.Player = 7;
            else if (m.MsgID == 0x1150) player = (sbyte)m.Param[2].Int;
        });
        TMapFieldMsg message = default;
        message.MsgID = 0x1151;
        message.Param[2] = query;
        field.SendMapFieldMsg(ref message);
        Assert.Equal(7, query.Info.Player);
        Assert.Throws<EInvalidCast>(() => message.Param[2].Int);
        message = default;
        message.MsgID = 0x1150;
        message.Param[2] = 3;
        field.SendMapFieldMsg(ref message);
        Assert.Equal(3, player);
        Assert.Null(message.Param[2].As<StormDamageQuery>());
    }

    // TMapFieldMsg.Result: 0x1201 answers the production control, 0x1100 the field's player.
    [Fact]
    public void TheResultCarriesAControlOrAPlayer()
    {
        var control = new TProductionControl(null!);
        var (field, handlers) = Field((ref m) =>
        {
            if (m.MsgID == 0x1201) m.Result = control;
            else if (m.MsgID == 0x1100) m.Result = 2;
        }, None);
        TMapFieldMsg message = default;
        message.MsgID = 0x1201;
        message.Result = 0;
        field.SendMapFieldMsg(ref message);
        Assert.Same(control, message.Result.As<TProductionControl>());
        Assert.Throws<EInvalidCast>(() => message.Result.Int);
        Assert.Equal(new[] { 1, 0 }, Calls(handlers));
        message = default;
        message.MsgID = 0x1100;
        message.Result = -1;
        field.SendMapFieldMsg(ref message);
        Assert.Equal(2, (sbyte)message.Result.Int);
        Assert.Null(message.Result.As<TProductionControl>());
        Assert.Equal(new[] { 2, 0 }, Calls(handlers));
    }

    // Spec section 4: the message records never reach a stream. No storage method of the product
    // takes a message record or one of its slots.
    [Fact]
    public void NoStorageMethodTakesAMessageRecord()
    {
        Type[] message = [typeof(TEMsg), typeof(TMapFieldMsg), typeof(TEMsgParams), typeof(TMapFieldMsgParams), typeof(MsgArg)];
        var found = new List<string>();
        foreach (var type in typeof(TEMsg).Assembly.GetTypes().Concat(typeof(MsgArg).Assembly.GetTypes()))
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (!method.Name.StartsWith("rw", StringComparison.Ordinal) && !method.Name.StartsWith("Read", StringComparison.Ordinal)
                    && !method.Name.StartsWith("Write", StringComparison.Ordinal)) continue;
                foreach (var parameter in method.GetParameters())
                {
                    var parameterType = parameter.ParameterType.IsByRef ? parameter.ParameterType.GetElementType()! : parameter.ParameterType;
                    if (message.Contains(parameterType)) found.Add($"{type.FullName}.{method.Name}({parameter.Name})");
                }
            }
        Assert.Empty(found);
    }
}
