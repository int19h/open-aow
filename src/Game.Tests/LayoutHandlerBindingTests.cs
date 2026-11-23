using Aow.Game.Audio;
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Dialogs;

namespace Aow.Game.Tests;

// A layout binds a dialog's handlers to its controls' events. A handler shared by several
// controls takes its slot from the binding instead of the control's tag, a click or a double
// click raised by the control reaches the handler the form named, and a handler that changes
// the key reaches the caller through the raise as the old ref parameter did.
[Collection("Engine")]
[EngineState]
public sealed class LayoutHandlerBindingTests(LayoutHandlerBindingTests.Host host) : IClassFixture<LayoutHandlerBindingTests.Host>
{
    private readonly Host _host = host;

    [Fact]
    public void LayoutSlotReachesTheSharedHandler()
    {
        EmailGameSettingsDialog dialog = _host.Dialog!;
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
    public void FileDialogClicksReachOpenbutClick()
    {
        string previousImagePath = ImageLibraries.BaseImageLibraryPath;
        ImageLibraries.BaseImageLibraryPath = EngineFixture.Root + Path.DirectorySeparatorChar;
        SoundSystem? system = AudioHost.SoundSystem;
        bool muted = false;
        if (system is not null && system.SfxEnabled)
        {
            _ = system.SetSfxEnabled(false);
            muted = true;
        }

        FileDialogWindow? dialog = null;
        DelphiStringList? previousDirectories = FileDialogWindow.Directories;
        try
        {
            dialog = Load(new LayoutSession(), module: new FileDialogWindow(_host.UI), publish: static module => { _ = module; });
            Assert.NotNull(Handler(dialog.OpenButton, declaring: typeof(Button), name: nameof(Button.Clicked)));
            Assert.NotNull(Handler(dialog.FileList, declaring: typeof(ListBox), name: nameof(ListBox.DoubleClicked)));
            FileDialogWindow.Directories = null;
            NullReferenceException click = Assert.Throws<NullReferenceException>(() => Click(dialog.OpenButton));
            NullReferenceException doubleClick = Assert.Throws<NullReferenceException>(() => DoubleClick(dialog.FileList));
            Assert.Contains(nameof(FileDialogWindow.OpenbutClick), click.ToString(), comparisonType: StringComparison.Ordinal);
            Assert.Contains(nameof(FileDialogWindow.OpenbutClick), doubleClick.ToString(), comparisonType: StringComparison.Ordinal);
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
    public void KeyHandlerZeroesTheKeyTheCallerReads()
    {
        using Component owner = new(null);
        WindowControl control = new(owner);
        control.SetVisibleWithoutUpdate(true);
        control.KeyPressed += (_, e) => e.Key = 0;
        ushort key = 13;

        control.CheckKeyDown(ref key, shift: default);

        Assert.Equal((ushort)0, key);
    }

    // The events, raised the way their declaring classes raise them: the control is the sender.
    private static EventHandler? Handler(UIComponent component, Type declaring, string name)
        => EventFields.Handler<EventHandler>(component, declaring, name);

    private static void Change(UIComponent component)
        => Handler(component, declaring: typeof(UIComponent), name: nameof(UIComponent.Changed))!(component, EventArgs.Empty);

    private static void Click(Button button)
        => Handler(button, declaring: typeof(Button), name: nameof(Button.Clicked))!(button, EventArgs.Empty);

    private static void DoubleClick(ListBox list)
        => Handler(list, declaring: typeof(ListBox), name: nameof(ListBox.DoubleClicked))!(list, EventArgs.Empty);

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
        internal EmailGameSettingsDialog? Dialog { get; }

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
