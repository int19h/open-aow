using Aow.Layout;
using Aow.Units.AoWButton;
using Aow.Units.AoWComp;
using Aow.Units.AoWEmailGameSettingsDlg;
using Aow.Units.AoWExeForms;
using Aow.Units.AoWFileDialog;
using Aow.Units.AoWHScroll;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWRelations;
using Aow.Units.AoWWinMan;
using Aow.Units.FontMod;
using Aow.Units.ImageLib;
using Aow.Units.Sound;

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
        owner.Destroy();
    }

    [Fact]
    public void BaseHandlerBindsToTheButtonEvent()
    {
        var owner = new TComponent(null!);
        var button = new TAOWButton(owner);
        TObject? seen = null;
        void Ignore(TObject sender) => seen = sender;
        button.OnClick = Ignore;

        button.OnClick(button);

        Assert.Same(button, seen);
        owner.Destroy();
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
        owner.Destroy();
    }

    [Fact]
    public void DirectCallPassesNoSender()
    {
        var owner = new TComponent(null!);
        var window = new TRelWindow(owner);
        window.SBGoldSlid = new TAOWHScrollBar(owner);
        window.SBGoldV = new TAOWLabel(owner);
        window.SBGoldSlid.FPosition = 17;

        window.SBGoldSlidChange();

        Assert.Equal("17", window.SBGoldV.Text);
        owner.Destroy();
    }

    [Fact]
    public void FileDialogClicksReachOpenbutClick()
    {
        var previousImagePath = ImageLibUnit.BaseImageLibraryPath;
        ImageLibUnit.BaseImageLibraryPath = EngineFixture.Root + Path.DirectorySeparatorChar;
        var system = SoundUnit.SoundSystem;
        bool muted = false;
        if (system != null && system.SFXEnabled)
        {
            system.SetSFXEnabled(false);
            muted = true;
        }

        TFileDlgWindow dialog = null!;
        var previousDirectories = AoWFileDialogUnit.Directories;
        try
        {
            dialog = Load(new LayoutSession(), new TFileDlgWindow(null!), static module => { _ = module; });
            Assert.NotNull(dialog.Openbut.OnClick);
            Assert.NotNull(dialog.FileLB.OnDoubleClick);
            AoWFileDialogUnit.Directories = null!;
            var click = Assert.Throws<NullReferenceException>(() => dialog.Openbut.OnClick(dialog.Openbut));
            var doubleClick = Assert.Throws<NullReferenceException>(() => dialog.FileLB.OnDoubleClick(dialog.FileLB));
            Assert.Contains(nameof(TFileDlgWindow.OpenbutClick), click.ToString(), StringComparison.Ordinal);
            Assert.Contains(nameof(TFileDlgWindow.OpenbutClick), doubleClick.ToString(), StringComparison.Ordinal);
        }
        finally
        {
            AoWFileDialogUnit.Directories = previousDirectories;
            dialog?.Destroy();
            if (muted) system!.SetSFXEnabled(true);
            ImageLibUnit.BaseImageLibraryPath = previousImagePath;
        }
    }

    [Fact]
    public void TwoLevelMouseDownFiresTheDerivedOwner()
    {
        var owner = new TComponent(null!);
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
        owner.Destroy();
    }

    [Fact]
    public void BaseReassignmentReplacesTheDerivedHandler()
    {
        var owner = new TComponent(null!);
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
        owner.Destroy();
    }

    [Fact]
    public void SavedBaseFieldRestoresTheTypedHandler()
    {
        var owner = new TComponent(null!);
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
        owner.Destroy();
    }

    [Fact]
    public void CopiedWrapperKeepsTheOriginalOwner()
    {
        var owner = new TComponent(null!);
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
        owner.Destroy();
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
        private readonly TIntGfxMod? previousGfx = aowIntGfxModUnit.IntGfxMod;
        private readonly TFontModule? previousFont = AoWExeFormsUnit.FontModule;
        private readonly TGeneral? previousGeneral = AoWWinManUnit.General;
        private TIntGfxMod gfx = null!;
        private TFontModule font = null!;
        private TGeneral general = null!;
        internal TEmailGameSettingsDlg Dialog = null!;

        public Host()
        {
            var previousImagePath = ImageLibUnit.BaseImageLibraryPath;
            Runtime.MessageHandler = _ => { };
            ImageLibUnit.BaseImageLibraryPath = EngineFixture.Root + Path.DirectorySeparatorChar;
            try
            {
                var session = new LayoutSession();
                gfx = Load(session, new TIntGfxMod(null!), module => aowIntGfxModUnit.IntGfxMod = module);
                font = Load(session, new TFontModule(null!), module => AoWExeFormsUnit.FontModule = module);
                general = Load(session, new TGeneral(null!), module => AoWWinManUnit.General = module);
                Dialog = Load(session, new TEmailGameSettingsDlg(null!), static module => { _ = module; });
            }
            catch
            {
                Dispose();
                throw;
            }
            finally
            {
                ImageLibUnit.BaseImageLibraryPath = previousImagePath;
                Runtime.MessageHandler = previousMessage;
            }
        }

        public void Dispose()
        {
            try
            {
                Dialog?.Destroy();
                general?.Destroy();
                font?.Destroy();
                gfx?.Destroy();
            }
            finally
            {
                aowIntGfxModUnit.IntGfxMod = previousGfx;
                AoWExeFormsUnit.FontModule = previousFont;
                AoWWinManUnit.General = previousGeneral;
            }
        }
    }
}
