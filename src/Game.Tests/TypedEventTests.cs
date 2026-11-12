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
    private readonly Host _host;

    public TypedEventTests(Host host) => this._host = host;

    [Fact]
    public void LayoutSlotReachesTheSharedHandler()
    {
        var dialog = _host.Dialog;
        dialog.PlayerScrollBar.Position = 0;
        dialog.EmailAddresses.Clear();
        for (int i = 0; i < 6; i++) dialog.EmailAddresses.Add("");

        dialog.EmailAddressEdit1.Text = "one@a";
        dialog.EmailAddressEdit1._onChange(dialog.EmailAddressEdit1);
        dialog.EmailAddressEdit6.Tag = 99;
        dialog.EmailAddressEdit6.Text = "six@a";
        dialog.EmailAddressEdit6._onChange(dialog.EmailAddressEdit6);

        Assert.Equal("one@a", dialog.EmailAddresses[0]);
        Assert.Equal("six@a", dialog.EmailAddresses[5]);
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
        button._onMouseDown(button, MouseButton.Left, default, 1, 2);
        Assert.Equal(2, runs);
        Assert.Same(button, mouseSender);
        owner.Dispose();
    }

    [Fact]
    public void DirectCallPassesNoSender()
    {
        using var owner = new Component(null!);
        var window = new RelationWindow(owner);
        window.BribeGoldSlider = new HorizontalScrollBar(owner);
        window.BribeGoldValueLabel = new Label(owner);
        window.BribeGoldSlider.SetPositionUnclamped(17);

        window.SBGoldSlidChange();

        Assert.Equal("17", window.BribeGoldValueLabel.Text);
    }

    [Fact]
    public void FileDialogClicksReachOpenbutClick()
    {
        var previousImagePath = ImageLibraries.BaseImageLibraryPath;
        ImageLibraries.BaseImageLibraryPath = EngineFixture.Root + Path.DirectorySeparatorChar;
        var system = AudioHost.SoundSystem;
        bool muted = false;
        if (system != null && system.SfxEnabled)
        {
            system.SetSfxEnabled(false);
            muted = true;
        }

        FileDialogWindow dialog = null!;
        var previousDirectories = FileDialogWindow.Directories;
        try
        {
            dialog = Load(new LayoutSession(), new FileDialogWindow(null!), static module => { _ = module; });
            Assert.NotNull(dialog.OpenButton.OnClick);
            Assert.NotNull(dialog.FileList.OnDoubleClick);
            FileDialogWindow.Directories = null!;
            var click = Assert.Throws<NullReferenceException>(() => dialog.OpenButton.OnClick(dialog.OpenButton));
            var doubleClick = Assert.Throws<NullReferenceException>(() => dialog.FileList.OnDoubleClick(dialog.FileList));
            Assert.Contains(nameof(FileDialogWindow.OpenbutClick), click.ToString(), StringComparison.Ordinal);
            Assert.Contains(nameof(FileDialogWindow.OpenbutClick), doubleClick.ToString(), StringComparison.Ordinal);
        }
        finally
        {
            FileDialogWindow.Directories = previousDirectories;
            dialog?.Dispose();
            if (muted) system!.SetSfxEnabled(true);
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

        button._onMouseDown(other, MouseButton.Left, default, 4, 5);

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
        button._onMouseDown(button, MouseButton.Left, default, 1, 2);

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
        button._onKeyPress(other, ref key);

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
        other._onMouseDown = button._onMouseDown;

        Assert.Same(button._onMouseDown, other.OnMouseDown);
        Assert.NotSame(handler, other.OnMouseDown);
        other._onMouseDown(other, MouseButton.Left, default, 1, 2);

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
        private readonly Action<string> _previousMessage = Runtime.MessageHandler;
        private readonly UIGraphics? _previousGraphics = DialogHosts.UIGraphics;
        private readonly UIFonts? _previousFont = DialogHosts.UIFonts;
        private readonly General? _previousGeneral = DialogHosts.General;
        private UIGraphics _graphics = null!;
        private UIFonts _font = null!;
        private General _general = null!;

        internal EmailGameSettingsDialog Dialog { get; } = null!;

        public Host()
        {
            var previousImagePath = ImageLibraries.BaseImageLibraryPath;
            Runtime.MessageHandler = _ => { };
            ImageLibraries.BaseImageLibraryPath = EngineFixture.Root + Path.DirectorySeparatorChar;
            try
            {
                var session = new LayoutSession();
                _graphics = Load(session, new UIGraphics(null!), module => DialogHosts.UIGraphics = module);
                _font = Load(session, new UIFonts(null!), module => DialogHosts.UIFonts = module);
                _general = Load(session, new General(null!), module => DialogHosts.General = module);
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
                Runtime.MessageHandler = _previousMessage;
            }
        }

        public void Dispose()
        {
            try
            {
                Dialog?.Dispose();
                _general?.Dispose();
                _font?.Dispose();
                _graphics?.Dispose();
            }
            finally
            {
                DialogHosts.UIGraphics = _previousGraphics;
                DialogHosts.UIFonts = _previousFont;
                DialogHosts.General = _previousGeneral;
            }
        }
    }
}
