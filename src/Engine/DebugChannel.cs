namespace Aow.Engine;

// The channels of the engine's debug messages. Engine.pas numbered them at startup through
// RegisterDebugID, in the order of these values, with -1 for text that names no channel.
public enum DebugChannel
{
    User = -1,
    HexSpriteEngine = 0,
    NetworkEngine = 1,
    Combat = 2,
    AI = 3,
    Temporary = 4,
    Magic = 5,
}

public static class DebugChannelLabels
{
    extension(DebugChannel channel)
    {
        // The name the debug window shows, the name the original registered the channel under.
        public string Label => channel switch
        {
            DebugChannel.User => "User",
            DebugChannel.HexSpriteEngine => "HS-Engine",
            DebugChannel.NetworkEngine => "Network Engine",
            DebugChannel.Combat => "Combat",
            DebugChannel.AI => "AI",
            DebugChannel.Temporary => "TMP",
            DebugChannel.Magic => "MAGIC",
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, message: null),
        };
    }
}
