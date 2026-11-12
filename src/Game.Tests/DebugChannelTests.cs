using Aow.Engine;

namespace Aow.Game.Tests;

// The fixed channels replace the registry each engine filled at startup. The values and labels are
// the ids and names that registry held, in its order (the engine startup snapshot before issue 193).
public sealed class DebugChannelTests
{
    [Fact]
    public void ChannelsKeepTheRegisteredIdsAndNames()
    {
        (DebugChannel Channel, int Id, string Label)[] registered =
        [
            (DebugChannel.User, -1, "User"),
            (DebugChannel.HexSpriteEngine, 0, "HS-Engine"),
            (DebugChannel.NetworkEngine, 1, "Network Engine"),
            (DebugChannel.Combat, 2, "Combat"),
            (DebugChannel.AI, 3, "AI"),
            (DebugChannel.Temporary, 4, "TMP"),
            (DebugChannel.Magic, 5, "MAGIC"),
        ];
        Assert.Equal(registered.Length, Enum.GetValues<DebugChannel>().Length);
        foreach (var (channel, id, label) in registered)
        {
            Assert.Equal(id, (int)channel);
            Assert.Equal(label, channel.Label);
        }
    }

    [Fact]
    public void MessagesCarryTheirChannel()
    {
        using var engine = new GameEngine(null!);
        var received = new List<(DebugChannel Channel, string Text)>();
        void Handler(EngineDebugNotification notification) => received.Add((notification.Info.Channel, notification.Info.Text));
        engine.DebugEvents += Handler;
        engine.OutputDebugMessage(DebugChannel.Combat, "combat");
        engine.OutputDebugString("user");
        engine.DebugEvents -= Handler;
        Assert.Equal([(DebugChannel.Combat, "combat"), (DebugChannel.User, "user")], received);
    }
}
