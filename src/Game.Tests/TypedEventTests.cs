using Aow.Game.Audio;
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Dialogs;

namespace Aow.Game.Tests;

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
        var button = new Button(owner);
        var list = new ListBox(owner);
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
        var button = new Button(owner);
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
        var button = new Button(owner);
        Button? seen = null;
        NotifyEvent<Button> click = sender => seen = sender;
        button.OnClick = click;
        Assert.Same(click, button.OnClick);
        button.OnClick(button);
        Assert.Same(button, seen);

        int runs = 0;
        UIComponent? mouseSender = null;
        void Down(UIComponent sender, MouseButton mouseButton, Set<ShiftState> shift, int x, int y)
        {
            runs++;
            mouseSender = sender;
        }

        UIComponent component = button;
        component.OnMouseDown = Down;
        MouseEvent<Button> saved = button.OnMouseDown;
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
        var window = new RelationWindow(owner);
        window.SBGoldSlid = new HorizontalScrollBar(owner);
        window.SBGoldV = new Label(owner);
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

        FileDialogWindow dialog = null!;
        var previousDirectories = FileDialogWindow.Directories;
        try
        {
            dialog = Load(new LayoutSession(), new FileDialogWindow(null!), static module => { _ = module; });
            Assert.NotNull(dialog.Openbut.OnClick);
            Assert.NotNull(dialog.FileLB.OnDoubleClick);
            FileDialogWindow.Directories = null!;
            var click = Assert.Throws<NullReferenceException>(() => dialog.Openbut.OnClick(dialog.Openbut));
            var doubleClick = Assert.Throws<NullReferenceException>(() => dialog.FileLB.OnDoubleClick(dialog.FileLB));
            Assert.Contains(nameof(FileDialogWindow.OpenbutClick), click.ToString(), StringComparison.Ordinal);
            Assert.Contains(nameof(FileDialogWindow.OpenbutClick), doubleClick.ToString(), StringComparison.Ordinal);
        }
        finally
        {
            FileDialogWindow.Directories = previousDirectories;
            dialog?.Dispose();
            if (muted) system!.SetSFXEnabled(true);
            ImageLibraries.BaseImageLibraryPath = previousImagePath;
        }
    }

    [Fact]
    public void TwoLevelMouseDownFiresTheDerivedOwner()
    {
        using var owner = new Component(null!);
        var button = new Button(owner);
        var other = new Button(owner);
        Button? seen = null;
        button.OnMouseDown = (sender, mouseButton, shift, x, y) => seen = sender;
        BaseButton middle = button;
        MouseEvent<BaseButton> read = middle.OnMouseDown;
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
        var button = new Button(owner);
        Button? seen = null;
        button.OnMouseDown = (sender, mouseButton, shift, x, y) => seen = sender;
        UIComponent component = button;
        UIComponent? replaced = null;
        MouseEvent<UIComponent> replacement = (sender, mouseButton, shift, x, y) => replaced = sender;
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
        var button = new Button(owner);
        var other = new Button(owner);
        Button? seen = null;
        byte key = 65;
        KeyPressEvent<Button> handler = (sender, ref pressed) =>
        {
            seen = sender;
            pressed = 66;
        };
        button.OnKeyPress = handler;
        UIComponent component = button;
        KeyPressEvent<UIComponent> saved = component.OnKeyPress;
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
        var button = new Button(owner);
        var other = new Button(owner);
        Button? seen = null;
        MouseEvent<Button> handler = (sender, mouseButton, shift, x, y) => seen = sender;
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
        private readonly UIGraphics? previousGfx = DialogHosts.IntGfxMod;
        private readonly UIFonts? previousFont = DialogHosts.FontModule;
        private readonly General? previousGeneral = DialogHosts.General;
        private UIGraphics gfx = null!;
        private UIFonts font = null!;
        private General general = null!;
        internal EmailGameSettingsDialog Dialog = null!;

        public Host()
        {
            var previousImagePath = ImageLibraries.BaseImageLibraryPath;
            Runtime.MessageHandler = _ => { };
            ImageLibraries.BaseImageLibraryPath = EngineFixture.Root + Path.DirectorySeparatorChar;
            try
            {
                var session = new LayoutSession();
                gfx = Load(session, new UIGraphics(null!), module => DialogHosts.IntGfxMod = module);
                font = Load(session, new UIFonts(null!), module => DialogHosts.FontModule = module);
                general = Load(session, new General(null!), module => DialogHosts.General = module);
                Dialog = Load(session, new EmailGameSettingsDialog(null!), static module => { _ = module; });
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
