#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
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
            MethodInfo? factory = typeof(CombatUnitSelectionControl).GetMethod("Create", bindingAttr: BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);
            return factory is not null ? (CombatUnitSelectionControl)factory.Invoke(null, parameters: null)! : new CombatUnitSelectionControl(EngineFixture.Engine);
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
        SortedDictionary<string, string> actual = [with(StringComparer.Ordinal)];
        for (int fail = 0; fail < 2; fail++)
        {
            using OrderingRecording.Fixture fixture = new();
            ImageLibraryManagerObject entry = fixture.Manager.DiskList.AddImageLibraryFile("TargHex.ILB");
            entry.ImageLibrary = new ImageLibrary(null!);
            IDisposable setScope = EngineFixture.UsePack(GamePack.Current);
            if (fail == 1)
            {
                EngineFixture.SetPack(null!);
            }

            int before = entry.RefCount;
            CombatUnitSelectionControl? value = null;
            string state;
            try
            {
                value = Create();
                _ = Assert.IsType<CombatMoveControlSettings>(value.MoveControlSettings);
                Assert.Same(entry.ImageLibrary, value.TargetImages);
                state = OrderingRecording.Snapshot(value);
            }
            catch (Exception error) when (error is not XunitException)
            {
                state = $"{error.GetType().FullName}:{error.Message}";
            }
            finally
            {
                setScope.Dispose();
            }
            Assert.Equal(fail == 0, actual: value is not null);
            actual.Add($"fail{fail}", JsonSerializer.Serialize(new { State = state, Before = before, After = entry.RefCount }));
            value?.Dispose();
            Assert.Equal(before, entry.RefCount);
        }
        OrderingRecording.Check("CombatUnitSelectionConstruction", actual);
    }
}
