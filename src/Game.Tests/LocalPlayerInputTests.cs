#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Game.Combat;
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using Aow.Game.Magic.Spells;
using Aow.Game.UI;
using Aow.Game.UI.Dialogs;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Issue 349 part B, review finding 5. LocalPlayerInput releases the handed caster exactly once
// on every exit: a dialog that opens, a handler that throws, the combat path and a missing UI.
[Collection("Engine")]
[EngineState]
public sealed class LocalPlayerInputTests
{
    private sealed class MapScope : IDisposable
    {
        private readonly StrategicMap _previous = StrategicMap.Current;

        public MapScope()
        {
            Map = new StrategicMap(null, holder: EngineFixture.Engine);
            StrategicMap.Current = Map;
        }

        public StrategicMap Map { get; }

        public void Dispose() => StrategicMap.Current = _previous;
    }

    // The graphics, fonts, general module, map events and disjunction dialog, loaded the way
    // LayoutHandlerBindingTests loads its dialog.
    private sealed class UiHost : IDisposable
    {
        private readonly Action<string> _previousMessage = LegacyApplication.MessageHandler;
        private readonly string _previousImagePath = ImageLibraries.BaseImageLibraryPath;

        internal UiHost()
        {
            LegacyApplication.MessageHandler = _ => { };
            ImageLibraries.BaseImageLibraryPath = EngineFixture.Root + Path.DirectorySeparatorChar;
            UI = new UIContext(EngineFixture.Engine);
            try
            {
                LayoutSession session = new();
                _ = UI.Load(session, module: new UIGraphics());
                _ = UI.Load(session, module: new UIFonts());
                _ = UI.Load(session, module: new General(UI));
                _ = UI.Load(session, module: new MapEvents(UI));
                _ = UI.Load(session, module: new DisjunctionDialog(UI));
            }
            catch
            {
                Dispose();
                throw;
            }
            finally
            {
                ImageLibraries.BaseImageLibraryPath = _previousImagePath;
                LegacyApplication.MessageHandler = _previousMessage;
            }
        }

        internal UIContext UI { get; }

        public void Dispose()
        {
            UI?.Dispose();
            ImageLibraries.BaseImageLibraryPath = _previousImagePath;
            LegacyApplication.MessageHandler = _previousMessage;
        }
    }

    private static UIContext MinimalUi()
    {
        UIContext ui = new(EngineFixture.Engine);
        _ = ui.Add(new MapEvents(ui));
        _ = ui.Add(new CombatMapEvents(ui));
        return ui;
    }

    [Fact]
    public void DialogOpenReleasesTheHandedReferenceOnce()
    {
        using MapScope scope = new();
        using UiHost host = new();
        LocalPlayerInput input = new(() => host.UI);
        DisjunctionSpellCaster caster = new(null) { Holder = EngineFixture.Engine, CasterSpellId = 51, CasterSourceId = 11 };
        input.ChooseSpellTargets(caster);
        // The dialog took its own reference and the handed one is gone, so this release is the
        // dialog reference and lands on zero.
        Assert.Equal(0, caster.Release());
        host.UI.DisjunctionDialog.ClearCaster();
    }

    [Fact]
    public void NoDialogReleasesOnce()
    {
        UIContext ui = MinimalUi();
        LocalPlayerInput input = new(() => ui);
        SpellCaster caster = new(null) { Holder = EngineFixture.Engine };
        input.ChooseSpellTargets(caster);
        Assert.Equal(-1, caster.Release());
    }

    [Fact]
    public void HandlerThrowReleasesOnce()
    {
        UIContext ui = MinimalUi();
        LocalPlayerInput input = new(() => ui);
        UnitSpellCaster caster = new(null) { Holder = EngineFixture.Engine, CasterSpellId = 1, CasterSourceId = 11 };
        input.ChooseSpellTargets(caster);
        Assert.Equal(-1, caster.Release());
    }

    [Fact]
    public void CombatHandlerThrowReleasesOnce()
    {
        UIContext ui = MinimalUi();
        LocalPlayerInput input = new(() => ui);
        CombatSpellCaster caster = new(null) { Holder = EngineFixture.Engine, CombatCasterSpellId = 1, CombatCasterSourceId = 11 };
        input.ChooseCombatSpellTarget(caster);
        Assert.Equal(-1, caster.Release());
    }

    [Fact]
    public void NoUiContextThrowsAndReleases()
    {
        LocalPlayerInput input = new(() => null);
        SpellCaster caster = new(null) { Holder = EngineFixture.Engine };
        _ = Assert.Throws<InvalidOperationException>(() => input.ChooseSpellTargets(caster));
        Assert.Equal(-1, caster.Release());
    }
}
