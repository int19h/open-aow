#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

namespace Aow.Game.Events;

public partial class GameOverEventLog
{
    // The seated player sees the game over screen, every other player the message.
    public override EventView View() => GetEngine().StrategicMap.SeatedPlayer == Player ? EventView.GameOver : EventView.ReceivedMessage;
}
