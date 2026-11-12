using Aow.Engine.Storage;
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
        var edit = new Edit(null!) { _font = font, Valid = true, Redraw = true, FirstChar = 1 };
        edit.SetVisibleWithoutUpdate(true);
        edit.SetWinWidthWithoutInvalidate(10000);
        Leader source = null!, loaded = null!;
        try
        {
            InputMapper.Text("Jörg €„äß", new Keyboard(edit));
            string typed = edit._text;
            Assert.Equal("Jörg €„äß", typed);
            Assert.Equal(new byte[] { 0x4A, 0xF6, 0x72, 0x67, 0x20, 0x80, 0x84, 0xE4, 0xDF }, Cp1252.Encode(typed).ToArray());

            source = new Leader(null!) { HeroResourceIndex = -1 };
            source.HeroName = typed;
            byte[] saved = Save(source);
            Assert.True(saved.AsSpan().IndexOf(new byte[] { 0x80, 0x84, 0xE4, 0xDF }) > 0, "the saved bytes hold 0x80, 0x84, 0xE4 and 0xDF");
            Assert.True(saved.AsSpan().IndexOf(Cp1252.Encode(typed)) > 0, "the saved bytes hold the typed name");

            loaded = new Leader(null!);
            using (var memory = new MemoryStream())
            {
                memory.Write(saved);
                memory.Position = 0;
                using var reader = new EReadStorageStream(memory, EngineFixture.Engine, false);
                loaded.ReadWrite(reader);
            }
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
        using var memory = new MemoryStream();
        using (var writer = new EWriteStorageStream(memory, EngineFixture.Engine, false)) { leader.ReadWrite(writer); }
        return memory.ToArray();
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
