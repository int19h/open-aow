using Aow.Game.Audio;
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Dialogs;

namespace Aow.Game.Tests;

// Typed events subscribe a view to the base event, bind a base handler as a method group, and
// pass a layout slot from the binding instead of the control tag.
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
        Change(dialog.EmailAddressEdit1);
        dialog.EmailAddressEdit6.Tag = 99;
        dialog.EmailAddressEdit6.Text = "six@a";
        Change(dialog.EmailAddressEdit6);

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
        button.OnClick += _ => Open();
        list.OnDoubleClick += _ => Open();

        Click(button);
        DoubleClick(list);

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
        button.OnClick += Ignore;

        Click(button);

        Assert.Same(button, seen);
        owner.Dispose();
    }

    [Fact]
    public void TypedAndBaseHandlersReachTheBaseEventAndLeaveIt()
    {
        var owner = new Component(null!);
        var button = new Button(owner);
        Button? seen = null;
        NotifyEvent<Button> click = sender => seen = sender;
        button.OnClick += click;
        Click(button);
        Assert.Same(button, seen);

        int runs = 0;
        UIComponent? mouseSender = null;
        void Down(UIComponent sender, MouseButton mouseButton, Set<ShiftState> shift, int x, int y)
        {
            runs++;
            mouseSender = sender;
        }

        UIComponent component = button;
        component.OnMouseDown += Down;
        MouseDown(button, sender: button);
        Assert.Equal(1, runs);
        Assert.Same(button, mouseSender);

        component.OnMouseDown -= Down;
        Assert.Null(MouseDownHandler(button));

        Button? typedSender = null;
        MouseEvent<Button> typed = (sender, mouseButton, shift, x, y) => typedSender = sender;
        button.OnMouseDown += typed;
        Assert.NotNull(MouseDownHandler(button));
        button.OnMouseDown -= typed;
        Assert.Null(MouseDownHandler(button));
        Assert.Null(typedSender);
        Assert.Equal(1, runs);
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
            Assert.NotNull(EventFields.Handler<NotifyEvent<Button>>(dialog.OpenButton, declaring: typeof(Button), name: nameof(Button.OnClick)));
            Assert.NotNull(EventFields.Handler<NotifyEvent<ListBox>>(dialog.FileList, declaring: typeof(ListBox), name: nameof(ListBox.OnDoubleClick)));
            FileDialogWindow.Directories = null!;
            var click = Assert.Throws<NullReferenceException>(() => Click(dialog.OpenButton));
            var doubleClick = Assert.Throws<NullReferenceException>(() => DoubleClick(dialog.FileList));
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
        button.OnMouseDown += (sender, mouseButton, shift, x, y) => seen = sender;
        BaseButton? middleSeen = null;
        BaseButton middle = button;
        middle.OnMouseDown += (sender, mouseButton, shift, x, y) => middleSeen = sender;

        MouseDown(button, sender: other);

        Assert.Same(button, seen);
        Assert.Same(button, middleSeen);
    }

    [Fact]
    public void BaseAndDerivedHandlersBothRunInTheOrderTheyJoined()
    {
        using var owner = new Component(null!);
        var button = new Button(owner);
        List<string> runs = [];
        button.OnMouseDown += (sender, mouseButton, shift, x, y) => runs.Add("button " + (sender == button));
        UIComponent component = button;
        component.OnMouseDown += (sender, mouseButton, shift, x, y) => runs.Add("component " + (sender == button));

        MouseDown(button, sender: button);

        Assert.Equal(["button True", "component True"], runs);
    }

    [Fact]
    public void RemoveTakesTheTypedHandlerOffTheBaseEvent()
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
        button.OnKeyPress += handler;
        button.OnKeyPress -= handler;
        Assert.Null(KeyPressHandler(button));
        button.OnKeyPress += handler;

        KeyPressHandler(button)!(other, ref key);

        Assert.Equal(66, key);
        Assert.Same(button, seen);
    }

    [Fact]
    public void RemovingAnotherHandlerKeepsTheView()
    {
        using var owner = new Component(null!);
        var button = new Button(owner);
        var other = new Button(owner);
        Button? seen = null;
        MouseEvent<Button> handler = (sender, mouseButton, shift, x, y) => seen = sender;
        MouseEvent<Button> stranger = (sender, mouseButton, shift, x, y) => { };
        button.OnMouseDown += handler;
        button.OnMouseDown -= stranger;
        other.OnMouseDown -= handler;

        MouseDown(button, sender: other);

        Assert.Same(button, seen);
    }

    // The base events, raised and read the way their declaring classes raise them.
    private static void Change(UIComponent component) =>
        EventFields.Handler<NotifyEvent<UIComponent>>(component, declaring: typeof(UIComponent), name: nameof(UIComponent.OnChange))!(component);

    private static void Click(Button button) =>
        EventFields.Handler<NotifyEvent<Button>>(button, declaring: typeof(Button), name: nameof(Button.OnClick))!(button);

    private static void DoubleClick(ListBox list) =>
        EventFields.Handler<NotifyEvent<ListBox>>(list, declaring: typeof(ListBox), name: nameof(ListBox.OnDoubleClick))!(list);

    private static MouseEvent<UIComponent>? MouseDownHandler(UIComponent component) =>
        EventFields.Handler<MouseEvent<UIComponent>>(component, declaring: typeof(UIComponent), name: nameof(UIComponent.OnMouseDown));

    private static void MouseDown(UIComponent component, UIComponent sender) =>
        MouseDownHandler(component)!(sender, MouseButton.Left, default, 1, 2);

    private static KeyPressEvent<UIComponent>? KeyPressHandler(UIComponent component) =>
        EventFields.Handler<KeyPressEvent<UIComponent>>(component, declaring: typeof(UIComponent), name: nameof(UIComponent.OnKeyPress));

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
