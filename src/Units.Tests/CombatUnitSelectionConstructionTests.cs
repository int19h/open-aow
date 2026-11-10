using System.Reflection;
using System.Text.Json;
using Aow.Game.Combat;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;

namespace Aow.Units.Tests;

[Collection("Engine")]
[EngineState]
public sealed class CombatUnitSelectionConstructionTests
{
    private static TCombatUnitSelectionControl Create()
    {
        try
        {
            var factory = typeof(TCombatUnitSelectionControl).GetMethod("Create", BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);
            return factory is not null ? (TCombatUnitSelectionControl)factory.Invoke(null, null)! : new TCombatUnitSelectionControl();
        }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(error.InnerException).Throw();
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
            var entry = fixture.Manager.FDiskList.AddILFile("TargHex.ILB");
            entry.FImageLibrary = new TImageLibrary(null!);
            var set = Aow.Game.Magic.Abilities.AoWEUnit.AoWHSSet;
            if (fail == 1) Aow.Game.Magic.Abilities.AoWEUnit.AoWHSSet = null!;
            int before = entry.RefCount;
            TCombatUnitSelectionControl? value = null;
            string state;
            try
            {
                value = Create();
                Assert.IsType<TCombatMoveControlSettings>(value.FMoveControlSettings);
                Assert.Same(entry.ImageLibrary, value.FTargetImages);
                state = OrderingRecording.Snapshot(value);
            }
            catch (System.Exception error) when (error is not Xunit.Sdk.XunitException) { state = error.GetType().FullName + ":" + error.Message; }
            finally { Aow.Game.Magic.Abilities.AoWEUnit.AoWHSSet = set; }
            Assert.Equal(fail == 0, value is not null);
            actual.Add("fail" + fail, JsonSerializer.Serialize(new { State = state, Before = before, After = entry.RefCount }));
            value?.Dispose();
            Assert.Equal(before, entry.RefCount);
        }
        OrderingRecording.Check("CombatUnitSelectionConstruction", actual);
    }
}
