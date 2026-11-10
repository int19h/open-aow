using Aow.Game.Audio;
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;

namespace Aow.Units.Tests;

// Typed events keep one slot, bind a base handler as a method group, and pass a layout slot
// from the binding instead of the control tag.
[Collection("Engine")]
[EngineState]
public sealed class TypedEventTests : IClassFixture<TypedEventTests.Host>
{
    private readonly Host host;

    public TypedEventTests(Host host) => this.host = host;

    [Fact]
    public void LayoutSlotReachesTheSharedHandler()
    {
        var dialog = host.Dialog;
        dialog.PlayerSB.Position = 0;
        dialog.FEmailAddresses.Clear();
        for (int i = 0; i < 6; i++) dialog.FEmailAddresses.Add("");

        dialog.EmailAddressEdit1.Text = "one@a";
        dialog.EmailAddressEdit1.FOnChange(dialog.EmailAddressEdit1);
        dialog.EmailAddressEdit6.Tag = 99;
        dialog.EmailAddressEdit6.Text = "six@a";
        dialog.EmailAddressEdit6.FOnChange(dialog.EmailAddressEdit6);

        Assert.Equal("one@a", dialog.FEmailAddresses[0]);
        Assert.Equal("six@a", dialog.FEmailAddresses[5]);
    }

    [Fact]
    public void OneHandlerRunsFromAButtonAndAList()
    {
        var owner = new TComponent(null!);
        var button = new TAOWButton(owner);
        var list = new TAOWListBox(owner);
        int runs = 0;
        void Open() => runs++;
        button.OnClick = _ => Open();
        list.OnDoubleClick = _ => Open();

        button.OnClick(button);
        list.OnDoubleClick(list);

        Assert.Equal(2, runs);
        owner.Dispose();
    }

    [Fact]
    public void BaseHandlerBindsToTheButtonEvent()
    {
        var owner = new TComponent(null!);
        var button = new TAOWButton(owner);
        object? seen = null;
        void Ignore(object sender) => seen = sender;
        button.OnClick = Ignore;

        button.OnClick(button);

        Assert.Same(button, seen);
        owner.Dispose();
    }

    [Fact]
    public void DerivedMouseEventRoundTripsThroughTheBaseSlot()
    {
        var owner = new TComponent(null!);
        var button = new TAOWButton(owner);
        TAOWButton? seen = null;
        TNotifyEvent<TAOWButton> click = sender => seen = sender;
        button.OnClick = click;
        Assert.Same(click, button.OnClick);
        button.OnClick(button);
        Assert.Same(button, seen);

        int runs = 0;
        TAoWComponent? mouseSender = null;
        void Down(TAoWComponent sender, TMouseButton mouseButton, TSet<TShiftState> shift, int x, int y)
        {
            runs++;
            mouseSender = sender;
        }

        TAoWComponent component = button;
        component.OnMouseDown = Down;
        TMouseEvent<TAOWButton> saved = button.OnMouseDown;
        saved(button, TMouseButton.mbLeft, default, 1, 2);
        Assert.Equal(1, runs);
        Assert.Same(button, mouseSender);

        button.OnMouseDown = null;
        Assert.Null(button.OnMouseDown);
        Assert.Null(component.OnMouseDown);

        button.OnMouseDown = saved;
        button.FOnMouseDown(button, TMouseButton.mbLeft, default, 1, 2);
        Assert.Equal(2, runs);
        Assert.Same(button, mouseSender);
        owner.Dispose();
    }

    [Fact]
    public void DirectCallPassesNoSender()
    {
        using var owner = new TComponent(null!);
        var window = new TRelWindow(owner);
        window.SBGoldSlid = new TAOWHScrollBar(owner);
        window.SBGoldV = new TAOWLabel(owner);
        window.SBGoldSlid.FPosition = 17;

        window.SBGoldSlidChange();

        Assert.Equal("17", window.SBGoldV.Text);
    }

    [Fact]
    public void FileDialogClicksReachOpenbutClick()
    {
        var previousImagePath = Aow.Game.Draw.Images.ImageLibraries.BaseImageLibraryPath;
        Aow.Game.Draw.Images.ImageLibraries.BaseImageLibraryPath = EngineFixture.Root + Path.DirectorySeparatorChar;
        var system = Aow.Game.Audio.AudioHost.SoundSystem;
        bool muted = false;
        if (system != null && system.SFXEnabled)
        {
            system.SetSFXEnabled(false);
            muted = true;
        }

        TFileDlgWindow dialog = null!;
        var previousDirectories = Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories;
        try
        {
            dialog = Load(new LayoutSession(), new TFileDlgWindow(null!), static module => { _ = module; });
            Assert.NotNull(dialog.Openbut.OnClick);
            Assert.NotNull(dialog.FileLB.OnDoubleClick);
            Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories = null!;
            var click = Assert.Throws<NullReferenceException>(() => dialog.Openbut.OnClick(dialog.Openbut));
            var doubleClick = Assert.Throws<NullReferenceException>(() => dialog.FileLB.OnDoubleClick(dialog.FileLB));
            Assert.Contains(nameof(TFileDlgWindow.OpenbutClick), click.ToString(), StringComparison.Ordinal);
            Assert.Contains(nameof(TFileDlgWindow.OpenbutClick), doubleClick.ToString(), StringComparison.Ordinal);
        }
        finally
        {
            Aow.Game.Ui.Dialogs.TFileDlgWindow.Directories = previousDirectories;
            dialog?.Dispose();
            if (muted) system!.SetSFXEnabled(true);
            Aow.Game.Draw.Images.ImageLibraries.BaseImageLibraryPath = previousImagePath;
        }
    }

    [Fact]
    public void TwoLevelMouseDownFiresTheDerivedOwner()
    {
        using var owner = new TComponent(null!);
        var button = new TAOWButton(owner);
        var other = new TAOWButton(owner);
        TAOWButton? seen = null;
        button.OnMouseDown = (sender, mouseButton, shift, x, y) => seen = sender;
        TAOWBaseButton middle = button;
        TMouseEvent<TAOWBaseButton> read = middle.OnMouseDown;
        Assert.NotSame(button.OnMouseDown, read);

        button.FOnMouseDown(other, TMouseButton.mbLeft, default, 4, 5);

        Assert.Same(button, seen);
        read(other, TMouseButton.mbLeft, default, 4, 5);
        Assert.Same(button, seen);
    }

    [Fact]
    public void BaseReassignmentReplacesTheDerivedHandler()
    {
        using var owner = new TComponent(null!);
        var button = new TAOWButton(owner);
        TAOWButton? seen = null;
        button.OnMouseDown = (sender, mouseButton, shift, x, y) => seen = sender;
        TAoWComponent component = button;
        TAoWComponent? replaced = null;
        TMouseEvent<TAoWComponent> replacement = (sender, mouseButton, shift, x, y) => replaced = sender;
        component.OnMouseDown = replacement;

        Assert.Same(replacement, button.OnMouseDown);
        button.FOnMouseDown(button, TMouseButton.mbLeft, default, 1, 2);

        Assert.Null(seen);
        Assert.Same(button, replaced);
    }

    [Fact]
    public void SavedBaseFieldRestoresTheTypedHandler()
    {
        using var owner = new TComponent(null!);
        var button = new TAOWButton(owner);
        var other = new TAOWButton(owner);
        TAOWButton? seen = null;
        byte key = 65;
        TKeyPressEvent<TAOWButton> handler = (sender, ref pressed) =>
        {
            seen = sender;
            pressed = 66;
        };
        button.OnKeyPress = handler;
        TAoWComponent component = button;
        TKeyPressEvent<TAoWComponent> saved = component.OnKeyPress;
        button.OnKeyPress = null;
        Assert.Null(button.OnKeyPress);
        component.OnKeyPress = saved;

        Assert.Same(handler, button.OnKeyPress);
        button.FOnKeyPress(other, ref key);

        Assert.Equal(66, key);
        Assert.Same(button, seen);
    }

    [Fact]
    public void CopiedWrapperKeepsTheOriginalOwner()
    {
        using var owner = new TComponent(null!);
        var button = new TAOWButton(owner);
        var other = new TAOWButton(owner);
        TAOWButton? seen = null;
        TMouseEvent<TAOWButton> handler = (sender, mouseButton, shift, x, y) => seen = sender;
        button.OnMouseDown = handler;
        other.FOnMouseDown = button.FOnMouseDown;

        Assert.Same(button.FOnMouseDown, other.OnMouseDown);
        Assert.NotSame(handler, other.OnMouseDown);
        other.FOnMouseDown(other, TMouseButton.mbLeft, default, 1, 2);

        Assert.Same(button, seen);
    }

    private static T Load<T>(LayoutSession session, T module, Action<T?> publish) where T : TComponent, ILayoutModule
    {
        session.Load(module, publish);
        return module;
    }

    // Image libraries resolve under the engine fixture root. A missing library shows no message,
    // the same way WheelLayoutTests loads.
    public sealed class Host : IDisposable
    {
        private readonly Action<string> previousMessage = Runtime.MessageHandler;
        private readonly TIntGfxMod? previousGfx = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod;
        private readonly TFontModule? previousFont = Aow.Game.Ui.Dialogs.DialogHosts.FontModule;
        private readonly TGeneral? previousGeneral = Aow.Game.Ui.Dialogs.DialogHosts.General;
        private TIntGfxMod gfx = null!;
        private TFontModule font = null!;
        private TGeneral general = null!;
        internal TEmailGameSettingsDlg Dialog = null!;

        public Host()
        {
            var previousImagePath = Aow.Game.Draw.Images.ImageLibraries.BaseImageLibraryPath;
            Runtime.MessageHandler = _ => { };
            Aow.Game.Draw.Images.ImageLibraries.BaseImageLibraryPath = EngineFixture.Root + Path.DirectorySeparatorChar;
            try
            {
                var session = new LayoutSession();
                gfx = Load(session, new TIntGfxMod(null!), module => Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod = module);
                font = Load(session, new TFontModule(null!), module => Aow.Game.Ui.Dialogs.DialogHosts.FontModule = module);
                general = Load(session, new TGeneral(null!), module => Aow.Game.Ui.Dialogs.DialogHosts.General = module);
                Dialog = Load(session, new TEmailGameSettingsDlg(null!), static module => { _ = module; });
            }
            catch
            {
                Dispose();
                throw;
            }
            finally
            {
                Aow.Game.Draw.Images.ImageLibraries.BaseImageLibraryPath = previousImagePath;
                Runtime.MessageHandler = previousMessage;
            }
        }

        public void Dispose()
        {
            try
            {
                Dialog?.Dispose();
                general?.Dispose();
                font?.Dispose();
                gfx?.Dispose();
            }
            finally
            {
                Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod = previousGfx;
                Aow.Game.Ui.Dialogs.DialogHosts.FontModule = previousFont;
                Aow.Game.Ui.Dialogs.DialogHosts.General = previousGeneral;
            }
        }
    }
}
