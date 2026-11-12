using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using Aow.Game.Combat;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Xunit.Sdk;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class CombatUnitSelectionConstructionTests
{
    private static CombatUnitSelectionControl Create()
    {
        try
        {
            var factory = typeof(CombatUnitSelectionControl).GetMethod("Create", BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);
            return factory is not null ? (CombatUnitSelectionControl)factory.Invoke(null, null)! : new CombatUnitSelectionControl();
        }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }

    [Fact]
    public void StateVirtualSettingsLinkCountOffsetsAndFailureMatchBaseCommit()
    {
        var actual = new SortedDictionary<string, string>(StringComparer.Ordinal);
        for (int fail = 0; fail < 2; fail++)
        {
            using var fixture = new OrderingRecording.Fixture();
            var entry = fixture.Manager.DiskList.AddImageLibraryFile("TargHex.ILB");
            entry.ImageLibrary = new ImageLibrary(null!);
            var set = GamePack.Current;
            if (fail == 1) GamePack.Current = null!;
            int before = entry.RefCount;
            CombatUnitSelectionControl? value = null;
            string state;
            try
            {
                value = Create();
                Assert.IsType<CombatMoveControlSettings>(value.MoveControlSettings);
                Assert.Same(entry.ImageLibrary, value.TargetImages);
                state = OrderingRecording.Snapshot(value);
            }
            catch (Exception error) when (error is not XunitException) { state = error.GetType().FullName + ":" + error.Message; }
            finally { GamePack.Current = set; }
            Assert.Equal(fail == 0, value is not null);
            actual.Add("fail" + fail, JsonSerializer.Serialize(new { State = state, Before = before, After = entry.RefCount }));
            value?.Dispose();
            Assert.Equal(before, entry.RefCount);
        }
        OrderingRecording.Check("CombatUnitSelectionConstruction", actual);
    }
}
