
namespace Aow.Game.Events;

public partial class GameOverEventLog
{
    // The seated player sees the game over screen, every other player the message.
    public override EventView View() => World.Map.AoWHSMap.Current.FSeatedPlayer == FPlayer ? EventView.GameOver : EventView.ReceivedMessage;
}
