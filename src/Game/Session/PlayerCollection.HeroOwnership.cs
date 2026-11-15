
namespace Aow.Game.Session;

public partial class PlayerCollection
{
    public override void Dispose()
    {
        base.Dispose();
        _localControlledPlayers?.Dispose();
        LocalHumanPlayers?.Dispose();
        ActivePlayers?.Dispose();
        LocalPlayers?.Dispose();
    }
}
