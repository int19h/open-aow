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
        var owner = new Component(null!);
        var button = new AOWButton(owner);
        var list = new AOWListBox(owner);
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
        var owner = new Component(null!);
        var button = new AOWButton(owner);
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
        var owner = new Component(null!);
        var button = new AOWButton(owner);
        AOWButton? seen = null;
        NotifyEvent<AOWButton> click = sender => seen = sender;
        button.OnClick = click;
        Assert.Same(click, button.OnClick);
        button.OnClick(button);
        Assert.Same(button, seen);

        int runs = 0;
        AoWComponent? mouseSender = null;
        void Down(AoWComponent sender, MouseButton mouseButton, Set<ShiftState> shift, int x, int y)
        {
            runs++;
            mouseSender = sender;
        }

        AoWComponent component = button;
        component.OnMouseDown = Down;
        MouseEvent<AOWButton> saved = button.OnMouseDown;
        saved(button, MouseButton.Left, default, 1, 2);
        Assert.Equal(1, runs);
        Assert.Same(button, mouseSender);

        button.OnMouseDown = null;
        Assert.Null(button.OnMouseDown);
        Assert.Null(component.OnMouseDown);

        button.OnMouseDown = saved;
        button.FOnMouseDown(button, MouseButton.Left, default, 1, 2);
        Assert.Equal(2, runs);
        Assert.Same(button, mouseSender);
        owner.Dispose();
    }

    [Fact]
    public void DirectCallPassesNoSender()
    {
        using var owner = new Component(null!);
        var window = new RelWindow(owner);
        window.SBGoldSlid = new AOWHScrollBar(owner);
        window.SBGoldV = new AOWLabel(owner);
        window.SBGoldSlid.FPosition = 17;

        window.SBGoldSlidChange();

        Assert.Equal("17", window.SBGoldV.Text);
    }

    [Fact]
    public void FileDialogClicksReachOpenbutClick()
    {
        var previousImagePath = ImageLibraries.BaseImageLibraryPath;
        ImageLibraries.BaseImageLibraryPath = EngineFixture.Root + Path.DirectorySeparatorChar;
        var system = AudioHost.SoundSystem;
        bool muted = false;
        if (system != null && system.SFXEnabled)
        {
            system.SetSFXEnabled(false);
            muted = true;
        }

        FileDlgWindow dialog = null!;
        var previousDirectories = FileDlgWindow.Directories;
        try
        {
            dialog = Load(new LayoutSession(), new FileDlgWindow(null!), static module => { _ = module; });
            Assert.NotNull(dialog.Openbut.OnClick);
            Assert.NotNull(dialog.FileLB.OnDoubleClick);
            FileDlgWindow.Directories = null!;
            var click = Assert.Throws<NullReferenceException>(() => dialog.Openbut.OnClick(dialog.Openbut));
            var doubleClick = Assert.Throws<NullReferenceException>(() => dialog.FileLB.OnDoubleClick(dialog.FileLB));
            Assert.Contains(nameof(FileDlgWindow.OpenbutClick), click.ToString(), StringComparison.Ordinal);
            Assert.Contains(nameof(FileDlgWindow.OpenbutClick), doubleClick.ToString(), StringComparison.Ordinal);
        }
        finally
        {
            FileDlgWindow.Directories = previousDirectories;
            dialog?.Dispose();
            if (muted) system!.SetSFXEnabled(true);
            ImageLibraries.BaseImageLibraryPath = previousImagePath;
        }
    }

    [Fact]
    public void TwoLevelMouseDownFiresTheDerivedOwner()
    {
        using var owner = new Component(null!);
        var button = new AOWButton(owner);
        var other = new AOWButton(owner);
        AOWButton? seen = null;
        button.OnMouseDown = (sender, mouseButton, shift, x, y) => seen = sender;
        AOWBaseButton middle = button;
        MouseEvent<AOWBaseButton> read = middle.OnMouseDown;
        Assert.NotSame(button.OnMouseDown, read);

        button.FOnMouseDown(other, MouseButton.Left, default, 4, 5);

        Assert.Same(button, seen);
        read(other, MouseButton.Left, default, 4, 5);
        Assert.Same(button, seen);
    }

    [Fact]
    public void BaseReassignmentReplacesTheDerivedHandler()
    {
        using var owner = new Component(null!);
        var button = new AOWButton(owner);
        AOWButton? seen = null;
        button.OnMouseDown = (sender, mouseButton, shift, x, y) => seen = sender;
        AoWComponent component = button;
        AoWComponent? replaced = null;
        MouseEvent<AoWComponent> replacement = (sender, mouseButton, shift, x, y) => replaced = sender;
        component.OnMouseDown = replacement;

        Assert.Same(replacement, button.OnMouseDown);
        button.FOnMouseDown(button, MouseButton.Left, default, 1, 2);

        Assert.Null(seen);
        Assert.Same(button, replaced);
    }

    [Fact]
    public void SavedBaseFieldRestoresTheTypedHandler()
    {
        using var owner = new Component(null!);
        var button = new AOWButton(owner);
        var other = new AOWButton(owner);
        AOWButton? seen = null;
        byte key = 65;
        KeyPressEvent<AOWButton> handler = (sender, ref pressed) =>
        {
            seen = sender;
            pressed = 66;
        };
        button.OnKeyPress = handler;
        AoWComponent component = button;
        KeyPressEvent<AoWComponent> saved = component.OnKeyPress;
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
        using var owner = new Component(null!);
        var button = new AOWButton(owner);
        var other = new AOWButton(owner);
        AOWButton? seen = null;
        MouseEvent<AOWButton> handler = (sender, mouseButton, shift, x, y) => seen = sender;
        button.OnMouseDown = handler;
        other.FOnMouseDown = button.FOnMouseDown;

        Assert.Same(button.FOnMouseDown, other.OnMouseDown);
        Assert.NotSame(handler, other.OnMouseDown);
        other.FOnMouseDown(other, MouseButton.Left, default, 1, 2);

        Assert.Same(button, seen);
    }

    private static T Load<T>(LayoutSession session, T module, Action<T?> publish) where T : Component, ILayoutModule
    {
        session.Load(module, publish);
        return module;
    }

    // Image libraries resolve under the engine fixture root. A missing library shows no message,
    // the same way WheelLayoutTests loads.
    public sealed class Host : IDisposable
    {
        private readonly Action<string> previousMessage = Runtime.MessageHandler;
        private readonly IntGfxMod? previousGfx = DialogHosts.IntGfxMod;
        private readonly FontModule? previousFont = DialogHosts.FontModule;
        private readonly General? previousGeneral = DialogHosts.General;
        private IntGfxMod gfx = null!;
        private FontModule font = null!;
        private General general = null!;
        internal EmailGameSettingsDlg Dialog = null!;

        public Host()
        {
            var previousImagePath = ImageLibraries.BaseImageLibraryPath;
            Runtime.MessageHandler = _ => { };
            ImageLibraries.BaseImageLibraryPath = EngineFixture.Root + Path.DirectorySeparatorChar;
            try
            {
                var session = new LayoutSession();
                gfx = Load(session, new IntGfxMod(null!), module => DialogHosts.IntGfxMod = module);
                font = Load(session, new FontModule(null!), module => DialogHosts.FontModule = module);
                general = Load(session, new General(null!), module => DialogHosts.General = module);
                Dialog = Load(session, new EmailGameSettingsDlg(null!), static module => { _ = module; });
            }
            catch
            {
                Dispose();
                throw;
            }
            finally
            {
                ImageLibraries.BaseImageLibraryPath = previousImagePath;
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
                DialogHosts.IntGfxMod = previousGfx;
                DialogHosts.FontModule = previousFont;
                DialogHosts.General = previousGeneral;
            }
        }
    }
}
