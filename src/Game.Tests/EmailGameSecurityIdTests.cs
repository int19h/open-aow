using Aow.Game.Net;

namespace Aow.Game.Tests;

/// <summary>
/// Issue 344 package 4. The email game ids draw Random(GetElapsedMilliSeconds and $FFFFFF) + 1
/// (Diplomacy.pas:2546 and 2564). The oracle's pbem chain pins the clock to 0 on both sides, so
/// every id there is 1 and the rotation of the security id is invisible. With a running clock
/// the ids range over the clock, and each UpdateGame draws a new security id, which
/// EmailGameRestarted compares against the incoming game (Diplomacy.pas:2535).
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class EmailGameSecurityIdTests
{
    private sealed class FixedClock(uint tickCount) : IClock
    {
        public DateTime Now => new(2000, 1, 1, 0, 0, 0, kind: DateTimeKind.Utc);
        public uint TickCount => tickCount;
        public double ElapsedMilliseconds => tickCount;

        public void WaitMilliseconds(int milliseconds)
        {
        }
    }

    private static (EmailGame Game, int Created, int Updated, bool IsRestarted) CreateAndUpdate(uint tickCount)
    {
        int seed = DelphiRandom.RandSeed;
        try
        {
            RtlHost.Clock = new FixedClock(tickCount);
            DelphiRandom.RandSeed = 123_456_789;
            EmailGameControl control = new(null) { Holder = EngineFixture.Engine };
            EmailGame game = control.CreateEmailGame("oracle-pbem", emailAddress: "seat1@example.invalid");
            int created = game.SecurityId;
            EmailGame incoming = new(null!)
            {
                Holder = EngineFixture.Engine,
                UniqueLocalId = game.UniqueLocalId,
                UniqueId = game.UniqueId,
                SecurityId = created
            };
            Assert.True(control.UpdateGame(incoming));
            return (game, created, incoming.SecurityId, control.EmailGameRestarted(new(null!) { Holder = EngineFixture.Engine, UniqueLocalId = game.UniqueLocalId, UniqueId = game.UniqueId, SecurityId = created }));
        }
        finally
        {
            DelphiRandom.RandSeed = seed;
        }
    }

    [Fact]
    public void ARunningClockGivesIdsAboveOneAndUpdateRotatesTheSecurityId()
    {
        (EmailGame game, int created, int updated, bool isRestarted) = CreateAndUpdate(tickCount: 1_000_000);

        Assert.True(game.UniqueId > 1);
        Assert.True(created > 1);
        Assert.NotEqual(created, updated);
        Assert.Equal(updated, game.SecurityId);
        Assert.True(isRestarted);
    }

    [Fact]
    public void AClockAtZeroGivesOneEverywhere()
    {
        (EmailGame game, int created, int updated, bool isRestarted) = CreateAndUpdate(tickCount: 0);

        Assert.Equal(1, game.UniqueId);
        Assert.Equal(1, created);
        Assert.Equal(1, updated);
        Assert.False(isRestarted);
    }
}
