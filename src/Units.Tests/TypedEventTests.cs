using Aow.Layout;
using Aow.Units.AoWButton;
using Aow.Units.AoWComp;
using Aow.Units.AoWEmailGameSettingsDlg;
using Aow.Units.AoWExeForms;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWListBox;
using Aow.Units.AoWWinMan;
using Aow.Units.FontMod;
using Aow.Units.ImageLib;

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
        dialog.EmailAddressEdit1.OnChange(dialog.EmailAddressEdit1);
        dialog.EmailAddressEdit6.Text = "six@a";
        dialog.EmailAddressEdit6.OnChange(dialog.EmailAddressEdit6);

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
