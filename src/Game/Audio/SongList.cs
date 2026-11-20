// Translated from Sound.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Rtl;
using Aow.Rtl.Host;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.Audio;

// FSongs, FSongNames (the titles, from the files). An engine object with a list of music files
public class SongList : EngineObject
{
    // Sound.pas:801
    public new const int ClassId = 2162692;

    public SongList(CustomNode owner) : base(owner)
    {
        _songs = new DelphiStringList
        {
            OnChange = SongsChanged
        };
        SongNames = new DelphiStringList();
    }

    private readonly DelphiStringList _songs;

    // Sound.pas:813
    internal void SetSongs(DelphiStringList value) => _songs.Assign(value);

    // Sound.pas:819
    // { Reads the song titles; a file that cannot be read is dropped from the list. }
    internal void SongsChanged(object sender)
    {
        SongNames.Clear();
        for (int i = _songs.Count - 1; i >= 0; i--)
        {
            try
            {
                string songName = AudioPlayback.GetSongName(_songs[i]);
                SongNames.Insert(0, value: songName);
            }
            catch
            {
                _songs.Delete(i);
            }
        }
    }

    // Sound.pas:806
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        // The original ReadWrite leaves out the songs under text only (the translate stream).
        if (!PersistSupport.Translating(persister))
        {
            PersistSupport.Lines(persister, name: "Songs", lines: _songs);
        }
    }

    // Sound.pas:794
    public override void Dispose()
    {
        _songs?.Dispose();
        SongNames?.Dispose();
        base.Dispose();
    }

    // Sound.pas:832
    public void Search(string path, string ext)
    {
        if (path[^1] != 92)
        {
            path += "\\";
        }

        string fullPath = AudioHost.BaseSoundEnginePath + path;
        _songs.BeginUpdate();
        foreach (RtlFileEntry entry in Files.FindEntries($"{fullPath}*.{ext}", attributes: 63 & ~16))
        {
            _ = _songs.Add(UpperCase(path + entry.Name));
        }

        _songs.EndUpdate();
    }

    public DelphiStringList Songs { get => _songs; set => SetSongs(value); }
    public DelphiStringList SongNames { get; }
}
