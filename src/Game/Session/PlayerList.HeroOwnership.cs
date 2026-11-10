
namespace Aow.Game.Session;

public partial class PlayerList
{
    public override void Dispose()
    {
        base.Dispose();
        FLocalControlledPlayers?.Dispose();
        FLocalHumanPlayers?.Dispose();
        FActivePlayers?.Dispose();
        FLocalPlayers?.Dispose();
    }
}
