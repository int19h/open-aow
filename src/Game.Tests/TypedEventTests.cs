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
public sealed class TypedEventTests(TypedEventTests.Host host) : IClassFixture<TypedEventTests.Host>
{
    private readonly Host _host = host;

    [Fact]
    public void LayoutSlotReachesTheSharedHandler()
    {
        EmailGameSettingsDialog dialog = _host.Dialog;
        dialog.PlayerScrollBar.Position = 0;
        dialog.EmailAddresses.Clear();
        for (int i = 0; i < 6; i++)
        {
            _ = dialog.EmailAddresses.Add("");
        }

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
        Component owner = new(null);
        Button button = new(owner);
        ListBox list = new(owner);
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
        Component owner = new(null);
        Button button = new(owner);
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
        Component owner = new(null);
        Button button = new(owner);
        Button? seen = null;
        void click(Button sender) => seen = sender;
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
        void typed(Button sender, MouseButton mouseButton, Set<ShiftState> shift, int x, int y) => typedSender = sender;
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
        using Component owner = new(null);
        using RelationWindow window = new(new UIContext(EngineFixture.Engine));
        window.BribeGoldSlider = new HorizontalScrollBar(owner);
        window.BribeGoldValueLabel = new Label(owner);
        window.BribeGoldSlider.SetPositionUnclamped(17);

        window.SBGoldSlidChange();

        Assert.Equal("17", window.BribeGoldValueLabel.Text);
    }

    [Fact]
    public void FileDialogClicksReachOpenbutClick()
    {
        string previousImagePath = ImageLibraries.BaseImageLibraryPath;
        ImageLibraries.BaseImageLibraryPath = EngineFixture.Root + Path.DirectorySeparatorChar;
        SoundSystem system = AudioHost.SoundSystem;
        bool muted = false;
        if (system is not null && system.SfxEnabled)
        {
            _ = system.SetSfxEnabled(false);
            muted = true;
        }

        FileDialogWindow dialog = null!;
        DelphiStringList previousDirectories = FileDialogWindow.Directories;
        try
        {
            dialog = Load(new LayoutSession(), module: new FileDialogWindow(_host.UI), publish: static module => { _ = module; });
            Assert.NotNull(EventFields.Handler<NotifyEvent<Button>>(dialog.OpenButton, declaring: typeof(Button), name: nameof(Button.OnClick)));
            Assert.NotNull(EventFields.Handler<NotifyEvent<ListBox>>(dialog.FileList, declaring: typeof(ListBox), name: nameof(ListBox.OnDoubleClick)));
            FileDialogWindow.Directories = null!;
            NullReferenceException click = Assert.Throws<NullReferenceException>(() => Click(dialog.OpenButton));
            NullReferenceException doubleClick = Assert.Throws<NullReferenceException>(() => DoubleClick(dialog.FileList));
            Assert.Contains(nameof(FileDialogWindow.OpenbutClick), click.ToString(), StringComparison.Ordinal);
            Assert.Contains(nameof(FileDialogWindow.OpenbutClick), doubleClick.ToString(), StringComparison.Ordinal);
        }
        finally
        {
            FileDialogWindow.Directories = previousDirectories;
            dialog?.Dispose();
            if (muted)
            {
                _ = system!.SetSfxEnabled(true);
            }

            ImageLibraries.BaseImageLibraryPath = previousImagePath;
        }
    }

    [Fact]
    public void TwoLevelMouseDownFiresTheDerivedOwner()
    {
        using Component owner = new(null);
        Button button = new(owner);
        Button other = new(owner);
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
        using Component owner = new(null);
        Button button = new(owner);
        List<string> runs = [];
        button.OnMouseDown += (sender, mouseButton, shift, x, y) => runs.Add($"button {sender == button}");
        UIComponent component = button;
        component.OnMouseDown += (sender, mouseButton, shift, x, y) => runs.Add($"component {sender == button}");

        MouseDown(button, sender: button);

        Assert.Equal(["button True", "component True"], runs);
    }

    [Fact]
    public void RemoveTakesTheTypedHandlerOffTheBaseEvent()
    {
        using Component owner = new(null!);
        Button button = new(owner);
        Button other = new(owner);
        Button? seen = null;
        byte key = 65;
        void handler(Button sender, ref byte pressed)
        {
            seen = sender;
            pressed = 66;
        }
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
        using Component owner = new(null);
        Button button = new(owner);
        Button other = new(owner);
        Button? seen = null;
        void handler(Button sender, MouseButton mouseButton, Set<ShiftState> shift, int x, int y) => seen = sender;
        void stranger(Button sender, MouseButton mouseButton, Set<ShiftState> shift, int x, int y) { }
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
        private readonly Action<string> _previousMessage = LegacyApplication.MessageHandler;

        internal UIContext UI { get; } = new(EngineFixture.Engine);
        internal EmailGameSettingsDialog Dialog { get; } = null!;

        public Host()
        {
            string previousImagePath = ImageLibraries.BaseImageLibraryPath;
            LegacyApplication.MessageHandler = _ => { };
            ImageLibraries.BaseImageLibraryPath = EngineFixture.Root + Path.DirectorySeparatorChar;
            try
            {
                LayoutSession session = new();
                _ = UI.Load(session, module: new UIGraphics());
                _ = UI.Load(session, module: new UIFonts());
                _ = UI.Load(session, module: new General(UI));
                Dialog = Load(session, module: new EmailGameSettingsDialog(UI), publish: static module => { _ = module; });
            }
            catch
            {
                Dispose();
                throw;
            }
            finally
            {
                ImageLibraries.BaseImageLibraryPath = previousImagePath;
                LegacyApplication.MessageHandler = _previousMessage;
            }
        }

        public void Dispose()
        {
            Dialog?.Dispose();
            UI.General?.Dispose();
            UI.Dispose();
        }
    }
}
