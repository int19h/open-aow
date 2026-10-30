namespace Aow.Rtl.Tests;

[Collection("Engine")]
[EngineState]
public sealed class SweepFailureTests
{
    public static IEnumerable<object[]> Cases => SweepFailureFixture.Names.Select(name => new object[] { name });

    [Theory]
    [MemberData(nameof(Cases))]
    public void PlainConstructorsKeepCompletedOwnerState(string name)
    {
        for (int route = 0; route < 4; route++) SweepFailureFixture.CheckPlainState(name, route);
    }
}
