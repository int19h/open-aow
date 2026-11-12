// Translated from Sound.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;
using Aow.Rtl.Host;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// FSongs, FSongNames (the titles, from the files). An engine object with a list of music files
public partial class SongList : EngineObject
{
    // Sound.pas:801
    public new const int ClassId = 2162692;

    public SongList(ECustomNode owner) : base(owner)
    {
        _songs = new StringList();
        _songs.OnChange = SongsChanged;
        _songNames = new StringList();
    }

    private StringList _songs;
    private StringList _songNames;

    // Sound.pas:813
    internal void SetSongs(StringList value)
    {
        _songs.Assign(value);
    }

    // Sound.pas:819
    // { Reads the song titles; a file that cannot be read is dropped from the list. }
    internal void SongsChanged(object sender)
    {
        _songNames.Clear();
        for (int I = _songs.Count - 1; I >= 0; I--)
        {
            try
            {
                var songName = AudioPlayback.GetSongName(_songs[I]);
                _songNames.Insert(0, songName);
            }
            catch
            {
                _songs.Delete(I);
            }
        }
    }

    // Sound.pas:806
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        if (!stream.StorageMode().Contains(EngineServices.Text))
        {
            stream.Lines(_songs, 10);
        }
    }

    // Sound.pas:786

    // Sound.pas:794
    public override void Dispose()
    {
        _songs?.Dispose();
        _songNames?.Dispose();
        base.Dispose();
    }

    // Sound.pas:832
    public void Search(string path, string ext)
    {
        string fullPath;
        if (path[path.Length - 1] != 92)
        {
            path = path + "\\";
        }

        fullPath = AudioHost.BaseSoundEnginePath + path;
        _songs.BeginUpdate();
        foreach (var entry in Files.FindEntries(fullPath + "*." + ext, attributes: 63 & ~16))
        {
            _songs.Add(UpperCase(path + entry.Name));
        }

        _songs.EndUpdate();
    }

    public StringList Songs { get => _songs; set => SetSongs(value); }
    public StringList SongNames { get => _songNames; }

}
