using Aow.Game.Draw.Images;
using Aow.Game.Engine.Storage;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Platform;

namespace Aow.Units.Tests;

[Collection("Engine")]
[EngineState]
public sealed class TypedNameRoundTripTests
{
    // strings.md section 4: a player name typed through InputMapper.Text with the euro sign, the low
    // double quote, a-umlaut and sharp s keeps its bytes through save and load.
    [Fact]
    public void TypedLeaderNameKeepsItsBytesThroughSaveAndLoad()
    {
        var font = new TImageLibraryFont(null!);
        var edit = new TAOWEdit(null!) { FAOWFont = font, FValid = true, FRedraw = true, FVisible = true, FWinWidth = 10000, FFirstChar = 1 };
        TLeader source = null!, loaded = null!;
        try
        {
            InputMapper.Text("Jörg €„äß", new Keyboard(edit));
            string typed = edit.FText;
            Assert.Equal("Jörg €„äß", typed);
            Assert.Equal(new byte[] { 0x4A, 0xF6, 0x72, 0x67, 0x20, 0x80, 0x84, 0xE4, 0xDF }, Cp1252.Encode(typed).ToArray());

            source = new TLeader(null!) { FHeroResourceIndex = -1 };
            source.FHeroName = typed;
            byte[] saved = Save(source);
            Assert.True(saved.AsSpan().IndexOf(new byte[] { 0x80, 0x84, 0xE4, 0xDF }) > 0, "the saved bytes hold 0x80, 0x84, 0xE4 and 0xDF");
            Assert.True(saved.AsSpan().IndexOf(Cp1252.Encode(typed)) > 0, "the saved bytes hold the typed name");

            loaded = new TLeader(null!);
            using (var memory = new MemoryStream())
            {
                memory.Write(saved);
                memory.Position = 0;
                using var reader = new TEReadStorageStream(memory, EngineFixture.Engine, false);
                loaded.ReadWrite(reader);
            }
            Assert.Equal(typed, loaded.FHeroName);
            Assert.Equal("Jörg €„äß", loaded.FHeroName);
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

    private static byte[] Save(TLeader leader)
    {
        using var memory = new MemoryStream();
        using (var writer = new TEWriteStorageStream(memory, EngineFixture.Engine, false)) { leader.ReadWrite(writer); }
        return memory.ToArray();
    }

    private sealed class Keyboard(TAOWEdit edit) : IDisplayEvents
    {
        public void KeyPress(byte value) => edit.CheckKeyPress(ref value);

        public void KeyDown(ushort key, TSet<TShiftState> shift) { }

        public void KeyUp(ushort key, TSet<TShiftState> shift) { }

        public void MouseMove(TSet<TShiftState> shift, int x, int y) { }

        public void MouseDown(TMouseButton button, TSet<TShiftState> shift, int x, int y) { }

        public void MouseUp(TMouseButton button, TSet<TShiftState> shift, int x, int y) { }

        public void DoubleClick() { }
    }
}
