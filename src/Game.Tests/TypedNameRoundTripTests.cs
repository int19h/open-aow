using Aow.Engine;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Game.Draw.Images;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Platform;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class TypedNameRoundTripTests
{
    // strings.md section 4: a player name typed through InputMapper.Text with the euro sign, the low
    // double quote, a-umlaut and sharp s keeps its bytes through save and load.
    [Fact]
    public void TypedLeaderNameKeepsItsBytesThroughSaveAndLoad()
    {
        var font = new ImageLibraryFont(null!);
        var edit = new Edit(null!) { Font = font, Valid = true, Redraw = true, FirstChar = 1 };
        edit.SetVisibleWithoutUpdate(true);
        edit.SetWinWidthWithoutInvalidate(10000);
        Leader source = null!, loaded = null!;
        try
        {
            InputMapper.Text("Jörg €„äß", new Keyboard(edit));
            string typed = edit.Text;
            Assert.Equal("Jörg €„äß", typed);
            Assert.Equal(new byte[] { 0x4A, 0xF6, 0x72, 0x67, 0x20, 0x80, 0x84, 0xE4, 0xDF }, Cp1252.Encode(typed).ToArray());

            source = new Leader(null!) { Holder = EngineFixture.Engine, HeroResourceIndex = -1 };
            source.HeroName = typed;
            byte[] saved = Save(source);
            Assert.True(saved.AsSpan().IndexOf(new byte[] { 0x80, 0x84, 0xE4, 0xDF }) > 0, "the saved bytes hold 0x80, 0x84, 0xE4 and 0xDF");
            Assert.True(saved.AsSpan().IndexOf(Cp1252.Encode(typed)) > 0, "the saved bytes hold the typed name");

            loaded = new Leader(null!) { Holder = EngineFixture.Engine };
            TreeReader.Read(OriginalBackend.Read(classValue: FormatSchema.Current.Get("Leader"), image: saved, schema: FormatSchema.Current, engine: "game"), objectValue: loaded, factory: new EnginePersistableFactory(EngineFixture.Engine));
            Assert.Equal(typed, loaded.HeroName);
            Assert.Equal("Jörg €„äß", loaded.HeroName);
            Assert.Equal(saved, Save(loaded));
        }
        finally
        {
            loaded?.Dispose();
            source?.Dispose();
            edit.Dispose();
            font.Dispose();
        }
    }

    private static byte[] Save(Leader leader)
    {
        var factory = new EnginePersistableFactory(EngineFixture.Engine);
        return OriginalBackend.Write(TreeWriter.Write(leader, factory: factory), classValue: FormatSchema.Current.Get(factory.ClassName(leader)), schema: FormatSchema.Current, engine: "game");
    }

    private sealed class Keyboard(Edit edit) : IDisplayEvents
    {
        public void KeyPress(byte value) => edit.CheckKeyPress(ref value);

        public void KeyDown(ushort key, Set<ShiftState> shift) { }

        public void KeyUp(ushort key, Set<ShiftState> shift) { }

        public void MouseMove(Set<ShiftState> shift, int x, int y) { }

        public void MouseDown(MouseButton button, Set<ShiftState> shift, int x, int y) { }

        public void MouseUp(MouseButton button, Set<ShiftState> shift, int x, int y) { }

        public void DoubleClick() { }
    }
}
