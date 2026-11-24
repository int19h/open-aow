using Aow.Game.Session;

namespace Aow.Game.Tests;

[Collection("Engine")]
public sealed class PlayerCollectionIdTests
{
    [Fact]
    public void TypedLookupsAndEnumerationsUseTheSameSlots()
    {
        PlayerCollection players = new(null);
        try
        {
            Assert.Same(players[0], players[PlayerId.Independent]);
            Assert.Same(players.GetPlayers(1), players.GetPlayers(PlayerId.Player1));
            Assert.Null(players.GetPlayers(null));
            Assert.Equal(players.Count, players.Ids.Count());
            Assert.Equal(players.Count - 1, players.GameIds.Count());
            Assert.Equal(PlayerId.Independent, players.Ids.First());
            Assert.Equal(PlayerId.Player1, players.GameIds.First());
        }
        finally
        {
            players.Dispose();
        }
    }
}
